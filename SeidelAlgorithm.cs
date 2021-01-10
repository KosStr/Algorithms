using System;
using System.Collections;

namespace Seidel
{
    class SeidelAlgorithm
    {
        static void Seidel(double[,] matrix, double[] resultingArray, double eps)
        {
            int iteration = 1;

            double substractionVectors = 0;

            int size = matrix.GetLength(0);

            ArrayList result = new ArrayList();

            for (int i = 0; i < size; ++i) 
            {
                result.Add(0.0);
            }

            double[,] args0 = new double[size, size];

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    if (i != j)
                    {
                        args0[i, j] = -matrix[i, j] / matrix[i, i];
                    }
                    else
                    {
                        args0[i, j] = resultingArray[i] / matrix[i, i];
                    }
                }
            }

            double[] xArray = new double[size];

            do
            {
                ++iteration;
                substractionVectors = 0.1 * eps;
                for (int i = 0; i < size; ++i)
                {
                    xArray[i] = args0[i, i];
                    for (int j = 0; j < size; ++j)
                    {
                        if (i != j)
                        {
                            xArray[i] += args0[i, j] * xArray[j];
                        }
                    }
                    result.Add(xArray[i]);
                    double substractionBuffer = Math.Abs((double)result[size * (iteration - 1) + i] - (double)result[size * (iteration - 2) + i]);
                    if (substractionBuffer > substractionVectors)
                    {
                        substractionVectors = substractionBuffer;
                    }
                }
            }
            while (substractionVectors >= eps);

            foreach (var value in result)
            {
                Console.WriteLine(value);
            }

            Console.WriteLine($"Iteration: {iteration-1}");
        }

        static void Main(string[] args)
        {
            double[,] matrix = { { -0.86, 0.23, 0.18, 0.17},
                                 { 0.12, -1.14, 0.08, 0.09},
                                 { 0.16, 0.24, -1.0, -0.35},
                                 { 0.23, -0.08, 0.05, -0.75} };

            double[] resultingArray = { 1.42, 0.83, -1.21, -0.65 };

            Seidel(matrix, resultingArray, 0.001);

            Console.ReadKey();
        }
    }
}
