using System;
using System.Diagnostics;
using System.Threading;

namespace FloydAlgorythm
{
    class Program
    {
        static void PointFloydAlgorythm(int[,] matrix, int threadNumber, int threadCounter) 
        {
            int size = matrix.GetLength(0);
            int pointsPerThread = size / threadNumber;
            int numberOfCalculatedPoints = threadCounter * pointsPerThread;
            int numberOfRowsCalculatedByThread = size - pointsPerThread * (threadNumber - 1 - threadCounter);

            for (int intermediatePoint = numberOfCalculatedPoints; intermediatePoint < numberOfRowsCalculatedByThread; ++intermediatePoint)
            {
                for (int startPoint = 0; startPoint < size; ++startPoint)
                {
                    for (int endPoint = 0; endPoint < size; ++endPoint)
                    {
                        if (matrix[startPoint, intermediatePoint] + matrix[intermediatePoint, endPoint] < matrix[startPoint, endPoint])
                        {
                            matrix[startPoint, endPoint] = matrix[startPoint, intermediatePoint] + matrix[intermediatePoint, endPoint];
                        }
                    }
                }
            }
        }

        static void MultithreadingFloydAlgorythmResult(int[,] matrix, int threadNumber)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < threadNumber; i++)
            {
                Thread thread = new Thread(() => PointFloydAlgorythm(matrix, threadNumber, i));
                thread.Start();
                thread.Join();
            }

            sw.Stop();

            Console.WriteLine($"Elapsed (multithreading) = {sw.Elapsed.TotalMilliseconds}");
        }


        static void FloydAlgorythm(int[,] matrix)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int size = matrix.GetLength(0);

            for (int intermediatePoint = 0; intermediatePoint < size; ++intermediatePoint)
            {
                for (int startPoint = 0; startPoint < size; ++startPoint)
                {
                    for (int endPoint = 0; endPoint < size; ++endPoint)
                    {
                        if (matrix[startPoint, intermediatePoint] + matrix[intermediatePoint, endPoint] < matrix[startPoint, endPoint]) 
                        {
                            matrix[startPoint, endPoint] = matrix[startPoint, intermediatePoint] + matrix[intermediatePoint, endPoint];
                        }
                    }
                }
            }

            sw.Stop();

            Console.WriteLine($"Elapsed (single thread) = {sw.Elapsed.TotalMilliseconds}");
        }

        static int[,] GenerateMatrix(int numberOfRows, int numberOfColumns)
        {
            Random rnd = new Random();
            int[,] randomizedMatrix = new int[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    randomizedMatrix[i, j] = rnd.Next(1, 15);
                }
            }
            return randomizedMatrix;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the size of the matrix: ");
            int matrixSize = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the number of threads: ");
            int threadNumber = Convert.ToInt32(Console.ReadLine());

            int[,] matrixA = GenerateMatrix(matrixSize, matrixSize);
            int[,] matrixB = matrixA;

            FloydAlgorythm(matrixA);
            MultithreadingFloydAlgorythmResult(matrixB, threadNumber);
            Console.ReadKey();

            // Multithreading Floyd Algorythm:      Size:                   500  / 1000  / 2000   / 3000
            //                                  Time(ms):   (2 threads)     1891 / 14801 / 121963 / 412666
            //                                              (5 threads)     1940 / 13030 / 121608 / 412248
            //                                              (8 threads)     2159 / 15049 / 123277 / 416095


            // Single thread Floyd Algorythm:       Size:   500  / 1000  / 2000   / 3000
            //                                  Time(ms):   1871 / 14418 / 124066 / 418924
        }
    }
}
