using Generator;
using System;
using System.Diagnostics;

namespace Sequential
{
    /// <summary>
    /// A conjugate gradient algorithm implementation for solving systems of linear equations.
    /// A * X = B, where:
    ///     - A is a square matrix of size NxN
    ///     - A is symetric and positive-definite
    ///     - X is a n-dimensional vector
    ///     - B is a n-dimensional vector
    /// </summary>
    public class ConjugateGradient
    {
        /// <summary>
        /// Gets a value indicating the number of unknown variables.
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Gets a value indicating the matrix A. 
        /// </summary>
        public double[,] A { get; private set; }

        /// <summary>
        /// Gets a value indicating the vector B.
        /// </summary>
        public double[] B { get; private set; }

        /// <summary>
        /// Gets a value indicating the solution, after the Solve method was invoked.
        /// </summary>
        public double[] X { get; private set; }

        public void SetEquation(double[,] a, double[] b)
        {
            N = b.Length;
            A = a;
            B = b;
        }

        public ResultOption Solve()
        {  
            // x0 = (0, 0, ..., 0)
            double[] x = new double[N];

            // r = b - A * x0;
            double[] residual = MatrixUtil.Minus(B, MatrixUtil.MultiplyMatrixVector(A, x));

            // p = r, p - directions vector
            double[] p = new double[N];
            Array.Copy(residual, p, N);

            double residualErrorOld = MatrixUtil.DotProduct(residual, residual);
            double residualErrorNew;

            double[] Ap;
            double alpha = 0;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int iteration;
            for (iteration = 0; iteration < N; iteration++)
            {
                // Ap = A * p
                Ap = MatrixUtil.MultiplyMatrixVector(A, p);

                // alpha = residualError / (p'*Ap)
                alpha = residualErrorOld / (MatrixUtil.DotProduct(p, Ap));

                // x = x + alpha * p
                MatrixUtil.Add(ref x, p, 1, alpha);

                // r = r - alpha * Ap
                MatrixUtil.Add(ref residual, Ap, 1, -alpha);

                // residualError = r' * r
                residualErrorNew = MatrixUtil.DotProduct(residual, residual);

                if (residualErrorNew <= 1e-15)
                {
                    break;
                }

                // p = r + rsnew / rsold * p
                MatrixUtil.Add(ref p, residual, residualErrorNew / residualErrorOld, 1);
                
                residualErrorOld = residualErrorNew;
            }

            // set solution
            X = x;

            stopwatch.Stop();
            
            return new ResultOption { ConvergenceIteration = iteration, SolveTime = stopwatch.ElapsedMilliseconds };
        }
    }
}
