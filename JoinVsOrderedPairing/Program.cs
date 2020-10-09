using JoinVsOrderedPairing.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JoinVsOrderedPairing
{
    class Program
    {
        //static IEnumerable<int> GenerateLeft() => Enumerable.Range(0, 1 << 25);
        //static IEnumerable<int> GenerateRight() => GenerateLeft();

        static IEnumerable<int> GenerateLeft() => Enumerable.Range(0, 4);
        static IEnumerable<int> GenerateRight() => new[] { 0, 2 };

        static void Measure(Action action, string caption)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine(caption);
            stopwatch.Start();
            action();
            stopwatch.Stop();
            Console.WriteLine($"Executing the action took {stopwatch.Elapsed.TotalSeconds} seconds.");
            Console.WriteLine("");
        }

        static void Benchmark(Func<IEnumerable<int>> generateLeft,
            Func<IEnumerable<int>> generateRight)
        {
            var left = generateLeft().ToList();
            var right = generateRight().ToList();
            Measure(() => left.Pair(right).ToList(), "Pair");

            left = generateLeft().ToList();
            right = generateRight().ToList();
            Measure(() => left.JoinPair(right).ToList(), "Join");
        }

        static void Main(string[] args)
        {
            //Benchmark(GenerateLeft, GenerateRight);

            //var lazyPairing = GenerateLeft().PairSelectOnOrderedInputsLazy(
            //    GenerateRight(),
            //    x => x,
            //    x => x,
            //    (l, r) => (l, r));

            //Console.WriteLine($"Doing something after pairing?");

            //lazyPairing
            //    .Select(p => 2 * p.l)
            //    .ToList();

            //// Lazy eval
            //var numbers = new[] { 0, 1, 2 };
            //var modified = numbers
            //    .Select(n => { Console.WriteLine($"First Eval: {n}"); return n; })
            //    .Select(n => { Console.WriteLine($"Second Eval: {n}"); return n; });

            //foreach (var n in modified)
            //    Console.WriteLine($"Number is {n}");

            //// ToList in between
            //var numbers = new[] { 0, 1, 2 };
            //var modified = numbers
            //    .Select(n => { Console.WriteLine($"First Eval: {n}"); return n; })
            //    .ToList()
            //    .Select(n => { Console.WriteLine($"Second Eval: {n}"); return n; });

            //foreach (var n in modified)
            //    Console.WriteLine($"Number is {n}");

            // Order by in between
            //var numbers = new[] { 0, 1, 2 };
            //var modified = numbers
            //    .Select(n => { Console.WriteLine($"First Eval: {n}"); return n; })
            //    .OrderBy(n => n)
            //    .Select(n => { Console.WriteLine($"Second Eval: {n}"); return n; });

            //foreach (var n in modified)
            //    Console.WriteLine($"Number is {n}");
        }
    }
}
