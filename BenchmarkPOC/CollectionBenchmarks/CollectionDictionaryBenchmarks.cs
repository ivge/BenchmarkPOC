using BenchmarkDotNet.Attributes;
using BenchmarkPOC.Model;
using System;
using System.Collections.Generic;

namespace BenchmarkPOC
{
    [RankColumn]
    public partial class CollectionBenchmarks
    {
        private Dictionary<int,ClassA> dictionary = new Dictionary<int,ClassA>(arraySize);

        [BenchmarkCategory("Create"), Benchmark]
        public void CreateDictionaty()
        {
            var dictionary = new Dictionary<int, ClassA>(arraySize);
        }

        [BenchmarkCategory("AddElements"), Benchmark]
        public void AddElemetsToDictionary()
        {
            var _dictionary = new Dictionary<int,ClassA>();
            for (int i = 0; i < arraySize; i++)
            {
                _dictionary.Add(i, array[i]);
            }
        }


        public void SumElementsInDiciotnary()
        {
            int sum = 0;

            for (int i = 0; i < arraySize; i++)
            {
                sum += dictionary[i].Item1;
            }
        }


        [BenchmarkCategory("FindFirst"), Benchmark]
        public void FindFirstInDictionaryByKey()
        {
            if (!dictionary.ContainsKey(firstToFind.Item1))
                throw new Exception();
        }

        [BenchmarkCategory("FindFirst"), Benchmark]
        public void FindFirstInDictionaryByValue()
        {
            if (!dictionary.ContainsValue(firstToFind))
                throw new Exception();
        }

        [BenchmarkCategory("FindSecond"), Benchmark]
        public void FindSecondInDictionaryByKey()
        {
            if (!dictionary.ContainsKey(secondToFind.Item1))
                throw new Exception();
        }

        [BenchmarkCategory("FindSecond"), Benchmark]
        public void FindSecondInDictionaryByValue()
        {
            if (!dictionary.ContainsValue(secondToFind))
                throw new Exception();
        }


        [BenchmarkCategory("FindThird"), Benchmark]
        public void FindThirdInDictionaryByKey()
        {
            if (!dictionary.ContainsKey(thirdToFind.Item1))
                throw new Exception();
        }

        [BenchmarkCategory("FindThird"), Benchmark]
        public void FindThirdInDictionaryByValue()
        {
            if (!dictionary.ContainsValue(thirdToFind))
                throw new Exception();
        }

    }
}

