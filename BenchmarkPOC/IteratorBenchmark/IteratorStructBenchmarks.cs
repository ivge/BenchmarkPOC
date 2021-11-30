using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkPOC
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class IteratorStructBenchmarks 
    {
        private StructA[] elements;

        public IteratorStructBenchmarks()
        {
            int arraySize = 1000000;
            elements = new StructA[arraySize];
            for (int i = 0; i < arraySize; i++)
                elements[i] = new StructA { Item1 = i, Item2 = i.ToString() };
        }

        [Benchmark]
        public void ForBenchmark()
        {
            int sum = 0;
            for (int i = 0; i < elements.Length; i++)
            {
                sum += elements[i].Item1;
            }
        }
        [Benchmark]
        public void ForEachBenchmark()
        {
            int sum = 0;
            foreach (var e in elements)
            {
                sum += e.Item1;
            }
        }

        [Benchmark]
        public void LinqBenchmark()
        {
            int sum = 0;
            elements.All(e =>
            {
                sum += e.Item1;
                return true;
            });

        }
    }
}
