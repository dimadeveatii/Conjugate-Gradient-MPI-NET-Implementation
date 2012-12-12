using System;

namespace Generator
{
    public class ResultOption
    {
        public static readonly ResultOption None = new ResultOption();

        public int ConvergenceIteration { get; set; }

        public double SolveTime { get; set; }

        public static void PrintResultWithCheck(double[,] a, double[] x, double[] b, ResultOption result)
        {
            // gets the result of A * X
            var ax = MatrixUtil.MultiplyMatrixVector(a, x);

            // calculates: Ax = A * X - B (residual)
            MatrixUtil.Add(ref ax, b, 1, -1);

            Console.WriteLine(
                "Solution error {0}\r\nConverged in: {1}\r\nSolve time: {2}s",
                MatrixUtil.DotProduct(ax, ax),
                result.ConvergenceIteration,
                result.SolveTime / 1000);
        }
    }
}
