using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

namespace BenchmarkPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = BenchmarkRunner.Run<CollectionBenchmarks>();

            //BenchmarkSwitcher.FromAssembly(typeof(CollectionBenchmarks).Assembly).Run(args, new DebugInProcessConfig());

            /*BenchmarkRunner.Run<IteratorStructBenchmarks>();
            BenchmarkRunner.Run<IteratorClassBenchmarks>();*/
        }
    }
}
