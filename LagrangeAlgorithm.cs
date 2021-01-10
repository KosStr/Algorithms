using System;

namespace Lagrange
{
    class LagrangeAlgorithm
    {
        static double CountRow(double[] arrayY, double[] arrayX, int i)
        {
            double result = arrayY[i];
            for (int j = 0; j < arrayX.Length - 1; ++j)
            {
                if (i != j)
                {
                    result *= (arrayX[arrayX.Length - 1] - arrayX[j]) / (arrayX[i] - arrayX[j]);
                }
            }
            return result;
        }

        static void Lagrange(double[] arrayX, double[] arrayY)
        {
            int size = arrayX.Length;
            for (int i = 0; i < size - 1; ++i) 
            {
                arrayY[size - 1] += CountRow(arrayY, arrayX, i);
            }
            Console.WriteLine($"F(x): {arrayY[size - 1]}");
        }

        static void Main(string[] args)
        {
            double[] arrayX = { 0, 0.25, 1.25, 2.12, 3.25, 1.2 };
            double[] arrayY = { 2, 1.6, 2.32, 2.02, 2.83, 0 };
            Lagrange(arrayX, arrayY);
        }
    }
}
