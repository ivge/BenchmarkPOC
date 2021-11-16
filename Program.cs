using BenchmarkDotNet.Running;
using System;

namespace BenchmarkPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = BenchmarkRunner.Run<StructsBenchmarks>();



            /*BenchmarkRunner.Run<IteratorStructBenchmarks>();
            BenchmarkRunner.Run<IteratorClassBenchmarks>();*/
        }
    }
}
