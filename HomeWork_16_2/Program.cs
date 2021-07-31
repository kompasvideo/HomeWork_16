using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeWork_16_2
{
    class Program
    {
        static int[] mas;
        static int[] numbersMas;
        static int potokClose;
        static Stopwatch stopWatch;

        static void Main(string[] args)
        {
            stopWatch = new Stopwatch();
            //stopWatch.Start();
            int i = Calculate();
            //stopWatch.Stop();
            Console.WriteLine($"Количество чисел {i}");
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadKey();
        }
        #region bak
        static int Calculate1()
        {
            int numbers = 0;
            for (int i = 1_000_000_000; i <= 2_000_000_000; i++)
            {
                string str = i.ToString();
                int sum = 0;
                int end = 0;
                for(int j = 0; j < 10; j++)
                {
                    end = Convert.ToInt32(str[j].ToString()); 
                    sum += end;
                }
                if (sum == end)
                {
                    numbers++;
                }
            }
            return numbers;
        }
        static int Calculate2()
        {
            int numbers = 0;
            for (int i = 1_000_000_000; i <= 2_000_000_000; i++)
            {
                int endDigit = i - i / 10 * 10;
                if(endDigit != 0)
                {
                    if ((i % endDigit) == 0)
                    {
                        numbers++;
                    }
                }
            }
            return numbers;
        }
        static int Calculate3()
        {
            int potok = 16;
            int n = 1_000_000_000;
            int sub = n / potok;
            int[] mas = new int[potok +1];
            for (int i=0; i < (potok + 1); i++)
            {
                mas[i] = n + i * sub;
            }
            if ((n % potok) != 0)
            {
                mas[potok] = 2_000_000_000;
            }
            int numbers = 0;
            for (int k = 0; k < potok; k++)
            {
                for (int i = mas[k]; i < mas[k+1]; i++)
                {
                    int endDigit = i - i / 10 * 10;
                    if (endDigit != 0)
                    {
                        if ((i % endDigit) == 0)
                        {
                            numbers++;
                        }
                    }
                }
            }
            return numbers;
        }
        static int Calculate4()
        {
            int potok = Environment.ProcessorCount;
            Console.WriteLine($"Количество потоков = {potok}");
            potok--;
            int n = 1_000_000_000;
            int sub = n / potok;
            mas = new int[potok + 1];
            for (int i = 0; i < (potok + 1); i++)
            {
                mas[i] = n + i * sub;
            }
            if ((n % potok) != 0)
            {
                mas[potok] = 2_000_000_000;
            }
            int numbers = 0;
            numbersMas = new int[potok];
            potokClose = 0;
            for (int k = 0; k < potok; k++)
            {
                _ = Func(k);
            }
            while(true)
            {
                if (potokClose == potok)
                {
                    break;
                }
            }
            foreach(int i in numbersMas)
            {
                numbers += i;
            }
            return numbers;            
        }
        static async Task Func(int k)
        {
            await Task.Run(() =>
            {
                for (int i = mas[k]; i < mas[k + 1]; i++)
                {
                    int endDigit = i - i / 10 * 10;
                    if (endDigit != 0)
                    {
                        if ((i % endDigit) == 0)
                        {
                            numbersMas[k]++;
                        }
                    }
                }
                potokClose++;
            });
        }
        static int Calculate5()
        {
            int potok = Environment.ProcessorCount;
            Console.WriteLine($"Количество потоков = {potok}");
            potok--;
            potok=1;
            int n = 1_000_000_000;
            int sub = n / potok;
            mas = new int[potok + 1];
            for (int i = 0; i < (potok + 1); i++)
            {
                mas[i] = n + i * sub;
            }
            if ((n % potok) != 0)
            {
                mas[potok] = 2_000_000_000;
            }
            int numbers = 0;
            int[] numbersMas = new int[potok];
            var tasks = new Task<int>[potok];
            stopWatch.Start();
            for (int k = 0; k < potok; k++)
            {
                tasks[k] = Task<int>.Run(() => Func2(mas[k], mas[k+1]));
            }
            Task.WaitAll(tasks);

            foreach (var task in tasks)
            {
                numbers += task.Result;
            }
            stopWatch.Stop();
            return numbers;
        }
        static int Func2(int begin, int end)
        {
            int result = 0;
            try
            {
                for (int i = begin; i < end; i++)
                {
                    int endDigit = i - i / 10 * 10;
                    if (endDigit != 0)
                    {
                        if ((i % endDigit) == 0)
                        {
                            result++;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException){}
            catch (Exception){}
            return result;
        }
        public static int Func3(int start, int end)
        {
            int result = 0;
            try
            {
                int sum, currNumb, lastDigit;

                for (int i = start; i <= end; i++)
                {
                    sum = 0;
                    currNumb = i;

                    while (currNumb != 0)
                    {
                        sum += currNumb % 10;
                        currNumb /= 10;
                    }

                    lastDigit = i % 10;

                    if (lastDigit == 0 || lastDigit == 1 || sum % lastDigit == 0)
                    {
                        result++;
                    }
                }
            }
            catch(IndexOutOfRangeException){}
            catch(Exception){}
            return result;
        }
        #endregion

        static int Calculate()
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
            int n = 1_000_000_000;
            int sub = n / potok;
            mas = new int[potok + 1];
            for (int i = 0; i < (potok + 1); i++)
            {
                mas[i] = n + i * sub;
            }
            if ((n % potok) != 0)
            {
                mas[potok] = 2_000_000_000;
            }
            int numbers = 0;
            int[] numbersMas = new int[potok];
            var tasks = new List<Task<int>>();
            stopWatch.Start();
            for (int k = 0; k < potok; k++)
            {
                tasks.Add(Task<int>.Factory.StartNew(action, k));
            }
            Task.WaitAll(tasks.ToArray());            
            foreach (var task in tasks)
            {
                numbers += task.Result;
            }
            stopWatch.Stop();
            return numbers;                      
        }

        static Func<object, int> action = (object obj) =>
        {
            int k2 = (int)obj;
            int result = 0;
            for (int i = mas[k2]; i < mas[k2 + 1]; i++)
            {
                int endDigit = i - i / 10 * 10;
                if (endDigit != 0)
                {
                    if ((i % endDigit) == 0)
                    {
                        result++;
                    }
                }
            }
            return result;
        };
    }
}
