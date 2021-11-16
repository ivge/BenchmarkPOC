using BenchmarkDotNet.Attributes;
using BenchmarkPOC.Model;
using System;
using System.Collections.Generic;

namespace BenchmarkPOC
{
    [RankColumn]
    public partial class CollectionBenchmarks
    {
        private List<ClassA> list = new List<ClassA>(arraySize);

        [BenchmarkCategory("Create"), Benchmark]
        public void CreateList()
        {
            var list = new List<ClassA>(arraySize);
        }

        [BenchmarkCategory("AddElements"), Benchmark]
        public void AddElemetsToList()
        {
            var list = new List<ClassA>();
            for (int i = 0; i < arraySize; i++)
            {
                list.Add(array[i]);
            }
        }


        public void SumElementsInList()
        {
            int sum = 0;

            for (int i = 0; i < arraySize; i++)
            {
                sum += list[i].Item1;
            }
        }


        [BenchmarkCategory("FindFirst"), Benchmark]

        public void FindFirstInList()
        {
            if (!list.Contains(firstToFind))
                throw new Exception();
        }

        [BenchmarkCategory("FindSecond"), Benchmark]
        public void FindSecondInList()
        {
            if (!list.Contains(secondToFind))
                throw new Exception();
        }

        [BenchmarkCategory("FindThird"), Benchmark]
        public void FindThirdInList()
        {
            if (!list.Contains(thirdToFind))
                throw new Exception();
        }

    }
}

