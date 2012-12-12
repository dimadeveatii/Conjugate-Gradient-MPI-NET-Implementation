using Generator;
using System;
using System.Diagnostics;

namespace Parallel
{
    /// <summary>
    /// A parallel implementation of conjugate gradient algorithm for solving systems of linear equations.
    /// A * X = B, where:
    ///     - A is a square matrix of size NxN
    ///     - A is symetric and positive-definite
    ///     - X is a n-dimensional vector
    ///     - B is a n-dimensional vector
    /// </summary>
    public class ConjugateGradient
    {
        private const int Root = 0;

        public int N { get { return n; } }

        public double[,] A { get { return a; } }
        public double[] B { get { return b; } }
        public double[] X { get; private set; }

        public bool IsMaster { get; private set; }

        private int n;

        private double[,] a;

        private double[] b;

        public void SetEquation(double[,] a, double[] b)
        {
            n = b.Length;
            this.a = a;
            this.b = b;
        } 

        public ResultOption Solve(MPI.Intracommunicator mpi)
        {
            IsMaster = mpi.Rank == 0;
            
            mpi.Broadcast(ref n, Root);
            
            Validate(mpi);

            int n_block = N / mpi.Size;

            double[] aFlattened = null; 
            if (IsMaster)
            {
                aFlattened = new double[N * N];
                ToArray(a, ref aFlattened);
            }

            if (!IsMaster) b = new double[N];
            // send B across all processes
            mpi.Broadcast(ref b, Root);
            
            // send A
            double[] a_block = new double[n_block];
            mpi.ScatterFromFlattened(aFlattened, N * n_block, Root, ref a_block);

            // x0 = (0, 0, ..., 0)
            double[] x = new double[N];

            // r = b - A*x0;
            double[] residual_block = new double[n_block];
            CalculateResidue(ref residual_block, b, a_block, x, mpi.Rank);

            double[] p_block = new double[n_block];
            Array.Copy(residual_block, p_block, n_block);

            double residualOld_block = MatrixUtil.DotProduct(residual_block, residual_block);
            double residualOld = mpi.Allreduce(residualOld_block, MPI.Operation<double>.Add);

            double rsnew;
            double[] x_block = new double[n_block];
            double[] p = new double[N];

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            double[] Ap;
            double pAp_block;
            double pAp;
            double alpha;
            double rsnew_block;

            int iteration;
            for (iteration = 0; iteration < N; iteration++)
            {
                ToArray(mpi.Allgather(p_block), ref p);

                Ap = MultiDotProduct(a_block, p);

                pAp_block = MatrixUtil.DotProduct(p_block, Ap);

                pAp = mpi.Allreduce(pAp_block, MPI.Operation<double>.Add);

                alpha = residualOld / pAp;

                MatrixUtil.Add(ref x_block, p_block, 1, alpha);

                MatrixUtil.Add(ref residual_block, Ap, 1, -alpha);

                rsnew_block = MatrixUtil.DotProduct(residual_block, residual_block);
                rsnew = mpi.Allreduce(rsnew_block, MPI.Operation<double>.Add);

                if (rsnew <= 1e-15)
                {
                    break;
                }

                MatrixUtil.Add(ref p_block, residual_block, rsnew / residualOld, 1);

                residualOld = rsnew;
            }

            ToArray(mpi.Allgather(x_block), ref x);
            X = x;

            stopwatch.Stop();

            return new ResultOption { ConvergenceIteration = iteration, SolveTime = stopwatch.ElapsedMilliseconds };
        }

        private static void CalculateResidue(ref double[] r_block, double[] b, double[] a_block, double[] x, int rank)
        {
            int index;

            double[] ax = MultiDotProduct(a_block, x);

            // r = b - A*x0;
            for (int i = 0; i < r_block.Length; i++)
            {
                index = r_block.Length * rank;
                r_block[i] = b[index + i] - ax[i];
            }
        }

        private static double[] MultiDotProduct(double[] a, double[] b)
        {
            int rows = a.Length / b.Length;
            double[] result = new double[rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    result[i] += a[i * b.Length + j] * b[j];
                }
            }

            return result;
        }

        private static void ToArray(double[][] a, ref double[] dest)
        {
            int index = 0;
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a[i].Length; j++)
                {
                    dest[index++] = a[i][j];
                }
            }
        }

        private static void ToArray(double[,] a, ref double[] dest)
        {
            int index = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    dest[index++] = a[i, j];
                }
            }
        }

        private void Validate(MPI.Intracommunicator mpi)
        {
            if (N % mpi.Size != 0)
            {
                throw new InvalidOperationException("The matrix size should be divisible by the number of processes used by MPI.");
            }
        }
    }
}