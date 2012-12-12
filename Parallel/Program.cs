using Generator;
using Library;
using System;

namespace Parallel
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isMaster = false;
            try
            {
                using (new MPI.Environment(ref args))
                {
                    isMaster = MPI.Communicator.world.Rank == 0;

                    // reads the specified size of matrix
                    string fileName = ReadFileName(args);

                    if (string.IsNullOrEmpty(fileName))
                    {
                        Console.WriteLine("Please specify the input file.");
                        return;
                    }

                    // generates a positive-definite symetric matrix A, and a vector B 
                    double[,] a = new double[0, 0];
                    double[] b = new double[0];                    

                    var algorithm = new ConjugateGradient();

                    if (isMaster)
                    {
                        try
                        {
                            IOUtils.ReadInput(fileName, out a, out b);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return;
                        }
                        
                        algorithm.SetEquation(a, b);
                    }

                    var options = algorithm.Solve(MPI.Communicator.world);
                    if (algorithm.IsMaster)
                    {
                        IOUtils.WriteVector(fileName + "_pout.txt", algorithm.X);
                        ResultOption.PrintResultWithCheck(a, algorithm.X, b, options);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
