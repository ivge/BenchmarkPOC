using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkPOC.Model;
using System;
using System.Collections.Generic;

namespace BenchmarkPOC
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public partial class CollectionBenchmarks
    {
        private const int arraySize = 1000000;
        private ClassA[] array = new ClassA[arraySize];

        private ClassA firstToFind;
        private ClassA secondToFind;
        private ClassA thirdToFind;

        public CollectionBenchmarks()
        {
            for (int i = 0; i < arraySize; i++)
            {
                array[i] = new Model.ClassA
                {
                    Item1 = i,
                    Item2 = i.ToString()
                };

                list.Add(array[i]);
                hashSet.Add(array[i]);
                dictionary.Add(i, array[i]);
            }

            firstToFind = array[arraySize / 3];
            secondToFind = array[(arraySize / 3) * 2];
            thirdToFind = array[arraySize - 1];
        }

        ~CollectionBenchmarks()
        {
            Array.Clear(array, 0, arraySize);
            hashSet.Clear();
            list.Clear();
            dictionary.Clear();
            GC.Collect();
        }


    }
}

