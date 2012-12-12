namespace Generator
{
    public static class MatrixUtil
    {
        public static double[] MultiplyMatrixVector(double[,] m, double[] v)
        {
            double[] result = new double[m.GetLength(0)];

            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < v.Length; j++)
                {
                    result[i] = result[i] + m[i, j] * v[j];
                }
            }

            return result;
        }

        public static double DotProduct(double[] a, double[] b)
        {
            double sum = 0;

            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i] * b[i];
            }

            return sum;
        }

        public static double[] Minus(double[] a, double[] b)
        {
            double[] result = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] - b[i];
            }

            return result;
        }

        public static void Add(ref double[] a, double[] b, double coeficientA = 1.0, double coeficientB = 1.0)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = coeficientA * a[i] + coeficientB * b[i];
            }
        }
    }
}
