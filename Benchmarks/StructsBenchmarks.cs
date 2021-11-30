using BenchmarkDotNet.Attributes;
using BenchmarkPOC.Model;
using System;


namespace BenchmarkPOC
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class StructsBenchmarks
    {
        private const int arraySize = 1000000;

        [Benchmark]
        public void TupleBenchmark()
        {
            var tuples = new Tuple<int, string>[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                tuples[i] = new Tuple<int, string>(i, i.ToString());
            }

            int sum = 0;

            for (int i = 0; i < arraySize; i++)
            {
                sum += tuples[i].Item1;
            }
        }

        [Benchmark]
        public void DynamicBenchmark()
        {
            var dynamics = new dynamic[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                dynamics[i] = new { Item1 = i, Item2 = i.ToString() };
            }

            int sum = 0;

            for (int i = 0; i < arraySize; i++)
            {
                sum += dynamics[i].Item1;
            }
        }

        [Benchmark]
        public void ClassBenchmark()
        {
            var objects = new Model.ClassA[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                objects[i] = new Model.ClassA { Item1 = i, Item2 = i.ToString() };
            }

            int sum = 0;

            for (int i = 0; i < arraySize; i++)
            {
                sum += objects[i].Item1;
            }
        }

        [Benchmark]
        public void StructBenchmark()
        {
            var structs = new StructA[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                structs[i] = new StructA { Item1 = i, Item2 = i.ToString() };
            }

            int sum = 0;

            for (int i = 0; i < arraySize; i++)
            {
                sum += structs[i].Item1;
            }
        }


        [Benchmark]
        public void RecordBenchmark()
        {
            var structs = new RecordA [arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                structs[i] = new RecordA { Item1 = i, Item2 = i.ToString() };
            }

            int sum = 0;

            for (int i = 0; i < arraySize; i++)
            {
                sum += structs[i].Item1;
            }
        }
    }
}

