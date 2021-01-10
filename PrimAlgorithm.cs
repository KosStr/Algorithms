using System;
using System.Diagnostics;
using System.Threading;

namespace Prim
{
    class Program
    {
        private static int MinKey(int[] key, bool[] set)
        {
            int min = int.MaxValue, minIndex = 0;

            for (int v = 0; v < key.Length; ++v)
            {
                if (set[v] == false && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }
            return minIndex;
        }

        static void MultithreadingPrimsAlgorythmResult(int[,] matrix, int threadNumber)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < threadNumber; i++)
            {
                Thread thread = new Thread(() => DividedPrimsAlgorithm(matrix, threadNumber, i));
                thread.Start();
                thread.Join();
            }

            sw.Stop();
            Console.WriteLine($"Elapsed (multithreading) = {sw.Elapsed.TotalMilliseconds}");
        }

        public static void DividedPrimsAlgorithm(int[,] matrix, int threadNumber, int threadCounter)
        {
            int size = matrix.GetLength(0);

            int verticesPerThread = size / threadNumber;
            int numberOfCalculatedVertices = threadCounter * verticesPerThread;
            int numberOfVerticesCalculatedByThread = size - verticesPerThread * (threadNumber - 1 - threadCounter);

            int[] parent = new int[size];
            int[] key = new int[size];
            bool[] marked = new bool[size];

            for (int i = 0; i < size; ++i)
            {
                key[i] = int.MaxValue;
                marked[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = numberOfCalculatedVertices; count < numberOfVerticesCalculatedByThread; ++count)
            {
                int u = MinKey(key, marked);
                marked[u] = true;

                for (int v = numberOfCalculatedVertices; v < numberOfVerticesCalculatedByThread; ++v)
                {
                    if (Convert.ToBoolean(matrix[u, v]) && marked[v] == false && matrix[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = matrix[u, v];
                    }
                }
            }
        }

        public static void PrimsAlgorithm(int[,] matrix)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int size = matrix.GetLength(0);

            int[] parent = new int[size];
            int[] key = new int[size];
            bool[] marked = new bool[size];

            for (int i = 0; i < size; ++i)
            {
                key[i] = int.MaxValue;
                marked[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < size - 1; ++count)
            {
                int u = MinKey(key, marked);
                marked[u] = true;

                for (int v = 0; v < size; ++v)
                {
                    if (Convert.ToBoolean(matrix[u, v]) && marked[v] == false && matrix[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = matrix[u, v];
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Time elapsed (single thread): {sw.Elapsed.TotalMilliseconds}");
        }

        static int[,] GenerateMatrix(int numberOfRows, int numberOfColumns)
        {
            Random rnd = new Random();
            int[,] matrix = new int[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = i + 1; j < numberOfColumns; j++)
                {
                    matrix[i, j] = matrix[j, i] = rnd.Next(10);
                }
            }
            return matrix;

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

            PrimsAlgorithm(matrixA);
            MultithreadingPrimsAlgorythmResult(matrixB, threadNumber);

            // Multithreading Prim's Algorythm:      Size:                1000   / 2000   / 5000    / 10000
            //                                     Time(ms):(2 threads)   19,1   / 41,1   / 176,5   / 674,8
            //                                              (5 threads)   35,2   / 49,1   / 156,8   / 568,б
            //                                              (8 threads)   45,5   / 56,4   / 159,3   / 535,1


            // Single thread Prim's Algorythm:       Size:   1000  / 2000  / 5000   / 10000
            //                                     Time(ms):   9,5   / 33,5  / 205,3  / 824,1
        }
    }
}
