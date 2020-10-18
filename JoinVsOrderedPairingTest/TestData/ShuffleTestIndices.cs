using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinVsOrderedPairingTest.TestData
{
    public static class ShuffleTestIndices
    {
        // Probability of success with these parameters is
        //      ~ 0.999999999999999999999999999899
        // which means probability of failure is
        //      ~ 1e-28
        public const int Length = 8;
        public const int Tries = 32;

        public static IEnumerable<(int, string)> All
            => Enumerable
                .Range(0, Length)
                .Zip(Enumerable.Repeat(FailureMessage(Length), Length));

        private static string FailureMessage(int m)
            => $"Probability to have all of these {m} index tests to " +
            // Using the lower line causes tests not to be run in visual studio
            $"succeed is (1 - 1 / {Length} ^ {Tries} ^ {m} ~= {FailureProbability(m)}.";
          //$"succeed is (1 - 1 / {Length} ^ {Tries}) ^ {m} ~= {FailureProbability(m)}.";


        //private static string FailureProbabilityFormula(int m)
        //    => $"(1 - 1 / {Length} ^ {Tries}) ^ {m}";

        // p(l, n, m) = (1 - 1 / l ^ n) ^ m
        // with
        //      l = Length
        //      n = Tries
        private static double FailureProbability(int m)
            => Math.Pow(1 - 1 / Math.Pow(Length, Tries), m);
    }
}
