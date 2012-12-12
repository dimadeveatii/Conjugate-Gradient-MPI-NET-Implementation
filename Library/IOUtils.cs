using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class IOUtils
    {
        public static void ReadInput(string fileName, out double[,] a, out double[] b)
        {
            a = new double[0, 0];
            b = new double[0];

            using (var reader = new StreamReader(fileName))
            {
                string[] lineValues;
                int n = 0;
                int lineIndex = 0;
                while (!reader.EndOfStream)
                {
                    lineValues = reader.ReadLine().Split(new[] { ' ' });
                    if (lineValues.Length == 0) continue;

                    if (n == 0)
                    {
                        n = lineValues.Length - 1;
                        a = new double[n, n];
                        b = new double[n];
                    }

                    for (int i = 0; i < n; i++)
                    {
                        a[lineIndex, i] = double.Parse(lineValues[i]);
                    }
                    b[lineIndex] = double.Parse(lineValues[n]);

                    lineIndex++;
                }
            }
        }

        public static void WriteVector(string fileName, double[] a)
        {
            using (var writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < a.Length; i++) writer.WriteLine(a[i]);
            }
        }
    }
}
