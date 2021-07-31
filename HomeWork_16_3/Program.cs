using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_16_3
{
    class Program
    {
        static Stopwatch stopWatch;
        static Stopwatch stopWatch2;
        const int dimension_1 = 1000;
        const int dimension_2 = 2000;
        static int[,] matrix_1;
        static int[,] matrix_2;
        static int[,] matrix_Result;
        static int[] mas;
        static int cycle = 0;

        static void Main(string[] args)
        {
            CreateMatrix();
            stopWatch = new Stopwatch();
            Console.WriteLine($"Переумножение матриц {dimension_1},{dimension_2} на {dimension_2},{dimension_1} в однопотоке");
            stopWatch.Start();
            for (int cycle = 0; cycle < dimension_1; cycle++)
            {
                Calculate1(cycle);
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            stopWatch2 = new Stopwatch();
            Console.WriteLine($"\n\nПереумножение матриц {dimension_1},{dimension_2} на {dimension_2},{dimension_1} в многопотоке");
            Calculate();
            TimeSpan ts2 = stopWatch2.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts2.Hours, ts2.Minutes, ts2.Seconds,
                ts2.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);


            Console.ReadKey();
        }

        static void CreateMatrix()
        {
            Random rnd = new Random();
            matrix_1 = new int[dimension_1, dimension_2];
            matrix_2 = new int[dimension_2, dimension_1];
            matrix_Result = new int[dimension_1, dimension_1];
            for (int i= 0; i < dimension_1; i++)
            {
                for (int k = 0; k < dimension_2; k++)
                {
                    matrix_1[i, k] = rnd.Next(0, 101);
                    matrix_2[k, i] = rnd.Next(0, 101);
                }
            }
        }

        static void Calculate()
        {
            int coreCount = 0;
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            Console.WriteLine($"Физических ядер = {coreCount}");
            int potok = Environment.ProcessorCount;
            Console.WriteLine($"Количество логических потоков = {potok}");
            potok--;
            Console.WriteLine($"Количество рабочих потоков = {potok}");
            int sub = dimension_1 / potok;
            mas = new int[potok + 1];
            for (int i = 0; i < (potok + 1); i++)
            {
                mas[i] = i * sub;
            }
            if ((dimension_1 % potok) != 0)
            {
                mas[potok] = dimension_1;
            }
            var tasks = new List<Task<int>>();
            stopWatch2.Start();
            for (int k = 0; k < potok; k++)
            {
                tasks.Add(Task<int>.Factory.StartNew(action, k));
            }
            Task.WaitAll(tasks.ToArray());
            stopWatch2.Stop();
        }

        static void Calculate2()
        {
            cycle = 0;
            stopWatch2.Start();
            Parallel.For(0, dimension_1, Calculate1);
            stopWatch2.Stop();
        }

        static Func<object, int> action = (object obj) =>
        {
            int n = (int)obj;
            for (int cycle = mas[n]; cycle < mas[n+1]; cycle++)
            {
                for (int i = 0; i < dimension_1; i++)
                {
                    for (int k = 0; k < dimension_2; k++)
                    {
                        matrix_Result[cycle, i] += matrix_1[i, k] * matrix_2[k, cycle];
                    }
                }
            }
            return 0;
        };

        static void Calculate1(int c)
        {
            for (int i = 0; i < dimension_1; i++)
            {
                for (int k = 0; k < dimension_2; k++)
                {
                    matrix_Result[cycle, i] += matrix_1[i, k] * matrix_2[k, cycle];
                }
            }
            cycle++;
        }
    }
}
