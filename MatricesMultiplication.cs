using System;
using System.Diagnostics;
using System.Threading;

namespace Lab2MatrixMultiply
{
    class Program
    {
        static void MultithreadingSumMatrix1(int[,] matrix1, int[,] matrix2, int numberOfThreads, int rowsPerThread)
        {
            int[,] resultingMatrix = new int[matrix1.GetLength(0), matrix2.GetLength(1)];

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < numberOfThreads; i++)
            {
                Thread thread = new Thread(() => MultithreadingMatrixMultiply(matrix1, matrix2, resultingMatrix, rowsPerThread, numberOfThreads, i));
                thread.Start();
                thread.Join();
            }

            sw.Stop();
            Console.WriteLine($"Elapsed (multithreading) = {sw.Elapsed.TotalMilliseconds}");
            //PrintMatrix(resultingMatrix);
        }

        static void MultithreadingMatrixMultiply(int[,] matrix1, int[,] matrix2, int[,] resultingMatrix, int rowsPerThread, int numberOfThreads, int threadCounter)
        {
            try
            {
                int numberOfCalculatedRows = threadCounter * rowsPerThread;
                int numberOfRowsCalculatedByThread = matrix1.GetLength(0) - rowsPerThread * (numberOfThreads - 1 - threadCounter);
                for (int i = numberOfCalculatedRows; i < numberOfRowsCalculatedByThread; ++i)
                {
                    for (int m2ColumnCounter = 0; m2ColumnCounter < matrix2.GetLength(1); ++m2ColumnCounter)
                    {
                        for (int sameRowColumnCounter = 0; sameRowColumnCounter < matrix2.GetLength(0); ++sameRowColumnCounter)
                        {
                            resultingMatrix[i, m2ColumnCounter] += matrix1[i, sameRowColumnCounter] * matrix2[sameRowColumnCounter, m2ColumnCounter];
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException) 
            {
                Console.WriteLine("Incorrect matrices size! ");
            }
        }

        static void MultiplyMatrix(int[,] matrix1, int[,] matrix2)
        {
            int[,] resultingMatrix = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            try
            {
                for (int i = 0; i < matrix1.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix2.GetLength(1); j++)
                    {
                        for (int k = 0; k < matrix1.GetLength(1); k++)
                        {
                            resultingMatrix[i, j] += matrix1[i, k] * matrix2[k, j];
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException) 
            {
                Console.WriteLine("Incorect matrices size! ");
            }
        }

        static int[,] GenerateMatrix(int numberOfRows, int numberOfColumns)
        {
            Random rnd = new Random();
            int[,] randomizedMatrix = new int[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    randomizedMatrix[i, j] = rnd.Next(-50, 51);
                }
            }
            return randomizedMatrix;
        }

        static void PrintMatrix(int[,] matrix)
        {
            Console.WriteLine($"Matrix: ");
            Console.WriteLine();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(string.Format("{0} ", matrix[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Enter the rows amount of first matrix : ");
            int Matrix1NumberOfRows = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the columns amount of first matrix: ");
            int Matrix1NumberOfColumns = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the rows amount of second matrix : ");
            int Matrix2NumberOfRows = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the columns amount of second matrix: ");
            int Matrix2NumberOfColumns = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter number of threads: ");
            int NumberOfThreads = Convert.ToInt32(Console.ReadLine());

            int RowsPerThread = Matrix1NumberOfRows / NumberOfThreads;

            if (Matrix1NumberOfColumns != Matrix2NumberOfRows) 
            {
                return;
            }

            int[,] MatrixA = GenerateMatrix(Matrix1NumberOfRows, Matrix1NumberOfColumns);

            int[,] MatrixB = GenerateMatrix(Matrix2NumberOfRows, Matrix2NumberOfColumns);

            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            //MultiplyMatrix(MatrixA, MatrixB);

            //sw.Stop();
            //Console.WriteLine($"Elapsed (one thread working) = {sw.Elapsed.TotalMilliseconds}");

            MultithreadingSumMatrix1(MatrixA, MatrixB, NumberOfThreads, RowsPerThread);
            // Matrixes 1000x1000
            // Single thread time: 12365,8438
            // Multithreading time: 11616,4354
            Console.ReadKey();
        }
    }
}