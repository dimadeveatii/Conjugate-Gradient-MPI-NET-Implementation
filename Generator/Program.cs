using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class Program
    {
        public static void Main(string[] args)
        {
            int n = ReadN(args);

            if (n == 0)
            {
                Console.WriteLine("Please specify the size of the matrix to generate.");
                return;
            }

            string fileName = ReadFileName(args, n);

            double[,] a;
            double[] b;

            AlgebraGenerator.GenerateInputForConjugateGradinet(n, out a, out b);

            try
            {
                WriteFiles(fileName, a, b);
                Console.WriteLine(string.Format("The matrix of size {0}x{0} has been saved in '{1}'.", b.Length, fileName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void WriteFiles(string fileName, double[,] a, double[] b)
        {
            using (var writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < b.Length; i++)
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        writer.Write(string.Format("{0} ", a[i,j].ToString("0.00")));
                    }

                    writer.WriteLine("{0}", b[i].ToString("0.00"));
                }
            }
        }

        private static int ReadN(string[] args)
        {
            int n;
            if (args.Length > 0 && int.TryParse(args[0], out n))
            {
                return n;
            }

            return 0;
        }

        private static string ReadFileName(string[] args, int n)
        {
            if (args.Length < 2)
            {
                return "matrix_" + n.ToString() + ".txt";
            }

            return args[1];
        }
    }
}
