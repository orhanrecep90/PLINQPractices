using ConsoleApp1.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = Enumerable.Range(1, 100).ToList();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("LINQ query started.");
            var resultArray2 = (from num in array
                                select new
                                {
                                    num,
                                    hashed = BCrypt.Net.BCrypt.HashPassword(num.ToString())
                                }).ToList();

            resultArray2.ForEach(x =>
            {
                Console.WriteLine($"{x.num} Hashed :{x.hashed}");
            });
            stopwatch.Stop();
            var LinqTotalTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("LINQ query finished");

            stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("PLINQ query started.");
            var resultArray = (from num in array.AsParallel()
                               select new
                               {
                                   num,
                                   hashed = BCrypt.Net.BCrypt.HashPassword(num.ToString())
                               });

            //ForAll method is used as ParallelQuery<T> object extensin method.It run as ForEach but run ascync.
            resultArray.ForAll(x =>
            {
                Console.WriteLine($"{x.num} Hashed : {x.hashed} threadId : {Thread.CurrentThread.ManagedThreadId}");
            });
            stopwatch.Stop();
            Console.WriteLine("PLINQ query finished");
            Console.WriteLine("*******  Results *******");
            Console.WriteLine($"LINQ Total time {LinqTotalTime}");
            Console.WriteLine($"PLINQ Total time {stopwatch.ElapsedMilliseconds}");
        }
    }
}
