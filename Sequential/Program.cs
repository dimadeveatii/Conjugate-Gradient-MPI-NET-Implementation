using Generator;
using Library;
using System;

namespace Sequential
{
    class Program
    {
        static void Main(string[] args)
        {
            // reads the specified size of matrix
            string fileName = ReadFileName(args);

            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("Please specify the input file.");
                return;
            }

            // generates a positive-definite symetric matrix A, and a vector B 
            double[,] a;
            double[] b;
            try
            {
                IOUtils.ReadInput(fileName, out a, out b);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var algorithm = new ConjugateGradient();
            algorithm.SetEquation(a, b);

            var options = algorithm.Solve();

            IOUtils.WriteVector(fileName + "_sout.txt", algorithm.X);

            ResultOption.PrintResultWithCheck(algorithm.A, algorithm.X, algorithm.B, options);
        }

        private static string ReadFileName(string[] args)
        {
            if (args.Length == 0 || args[0].Length == 0)
            {
                return string.Empty;
            }

            return args[0];
        }
    }
}
