using System;
using System.Diagnostics;
using System.Threading;

namespace Dijkstra
{
    class Program
    {
        static void DijkstraAlgorythm(int[,] matrix, int currentVertex)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int size = matrix.GetLength(0);

            int[] distanceFromCurrentToI = new int[size];

            bool[] processedList = new bool[size];

            for (int i = 0; i < size; i++)
            {
                distanceFromCurrentToI[i] = int.MaxValue;
                processedList[i] = false;
            }

            distanceFromCurrentToI[currentVertex] = 0;

            for (int count = 0; count < size - 1; count++)
            {
                int minimalDistanceNotProccessed = minimalPath(distanceFromCurrentToI, processedList);

                processedList[minimalDistanceNotProccessed] = true;

                for (int v = 0; v < size; v++)
                {
                    if (!processedList[v] && matrix[minimalDistanceNotProccessed, v] != 0 &&
                         distanceFromCurrentToI[minimalDistanceNotProccessed] != int.MaxValue &&
                         distanceFromCurrentToI[minimalDistanceNotProccessed] + matrix[minimalDistanceNotProccessed, v] < distanceFromCurrentToI[v])
                    {
                        distanceFromCurrentToI[v] = distanceFromCurrentToI[minimalDistanceNotProccessed] + matrix[minimalDistanceNotProccessed, v];
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Elapsed (single thread) = {sw.Elapsed.TotalMilliseconds}");
        }

        static void DividedDijkstraAlgorythm(int[,] matrix, int threadNumber, int threadCounter, int[] distanceFromCurrentToI)
        {
            int currentVertex = 0;
            int size = matrix.GetLength(0);
            int verticesPerThread = size / threadNumber;
            int numberOfCalculatedVertices = threadCounter * verticesPerThread;
            int numberOfVerticesCalculatedByThread = size - verticesPerThread * (threadNumber - 1 - threadCounter);

            bool[] processedList = new bool[size];

            distanceFromCurrentToI[currentVertex] = 0;

            for (int i = numberOfCalculatedVertices; i < numberOfVerticesCalculatedByThread ; i++)
            {
                int minimalDistanceNotProccessed = minimalPath(distanceFromCurrentToI, processedList);

                processedList[minimalDistanceNotProccessed] = true;

                for (int v = numberOfCalculatedVertices; v < numberOfVerticesCalculatedByThread; v++)
                {
                    if (!processedList[v] && matrix[minimalDistanceNotProccessed, v] != 0 &&
                         distanceFromCurrentToI[minimalDistanceNotProccessed] != int.MaxValue &&
                         distanceFromCurrentToI[minimalDistanceNotProccessed] + matrix[minimalDistanceNotProccessed, v] < distanceFromCurrentToI[v])
                    {
                        distanceFromCurrentToI[v] = distanceFromCurrentToI[minimalDistanceNotProccessed] + matrix[minimalDistanceNotProccessed, v];
                    }
                }
            }
        }

        static void MultithreadingDijkstraAlgorythmResult(int[,] matrix, int threadNumber, int[] distanceFromCurrentToI)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < threadNumber; i++)
            {
                Thread thread = new Thread(() => DividedDijkstraAlgorythm(matrix, threadNumber, i, distanceFromCurrentToI));
                thread.Start();
                thread.Join();
            }

            sw.Stop();
            Console.WriteLine($"Elapsed (multithreading) = {sw.Elapsed.TotalMilliseconds}");
        }

        static int minimalPath(int[] distanceFromCurrentToI, bool[] processedList)
        {
            int min = int.MaxValue;
            int min_index = 0;

            for (int i = 0; i < distanceFromCurrentToI.Length; i++)
                if (processedList[i] == false && distanceFromCurrentToI[i] <= min)
                {
                    min = distanceFromCurrentToI[i];
                    min_index = i;
                }
            return min_index;
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

            int[,] matrixB = new int[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; ++i)
            {
                for (int j = 0; j < matrixSize; ++j)
                {
                    matrixB[i, j] = matrixA[i, j];
                }
            }

            int[] distanceFromCurrentToI = new int[matrixA.GetLength(0)];
            for (int i = 0; i < distanceFromCurrentToI.Length; ++i)  
            {
                distanceFromCurrentToI[i] = int.MaxValue;
            }

            DijkstraAlgorythm(matrixA, 0);
            MultithreadingDijkstraAlgorythmResult(matrixB, threadNumber, distanceFromCurrentToI);
            Console.ReadKey();

            // Multithreading Dejkstra: 1000(2 - 25,2(ms) 5 - 38,8(ms) 8 - 39,6(ms))   
            //                          2000(2 - 38,1(ms) 5 - 51,7(ms) 8 - 56,7(ms))
            //                          5000(2 - 169,8(ms) 5 - 151,7(ms) 8 - 155,2(ms))
            //                          10000(2 - 639,2(ms) 5 - 596,7(ms) 8 - 580,2(ms))

            // Single thread Dejkstra: 1000(15,1)  / 2000(35,6)  / 5000(212,5)   / 10000(750,5)
        }

    }
}
