using System;

namespace Generator
{
    public static class AlgebraGenerator
    {
        private static Random randomize = new Random();

        public static void GenerateInputForConjugateGradinet(int n, out double[,] a, out double[] b)
        {
            a = AlgebraGenerator.GenerateSymmetricPositiveDefinite(n);
            b = AlgebraGenerator.GenerateVector(n);
        }

        public static double[,] GenerateSymmetricPositiveDefinite(int n)
        {
            double[,] matrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    matrix[i, j] = Random(0, 5, 2);
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    matrix[i, j] = matrix[j, i];
                }
            }

            for (int i = 0; i < n; i++)
            {
                matrix[i, i] = LineSum(matrix, i) + Random(0.1, 10, 2); 
            }

            return matrix;
        }

        public static double[] GenerateVector(int n)
        {
            double[] v = new double[n];

            for (int i = 0; i < n; i++)
            {
                v[i] = Random(1, 2 * n, 2);
            }

            return v;
        }

        private static double Random(double min, double max, int decimals)
        {
            return Math.Round(min + randomize.NextDouble() * (max - min), decimals);
        }

        private static double LineSum(double[,] matrix, int line)
        {
            double sum = 0;

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                sum += matrix[line, i];
            }

            return sum;
        }
    }
}
