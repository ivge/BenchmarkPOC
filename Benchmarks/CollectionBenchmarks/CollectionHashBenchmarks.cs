using BenchmarkDotNet.Attributes;
using BenchmarkPOC.Model;
using System;
using System.Collections.Generic;

namespace BenchmarkPOC
{
    public partial class CollectionBenchmarks
    {

        private HashSet<ClassA> hashSet = new HashSet<ClassA>(arraySize);

        [BenchmarkCategory("Create"), Benchmark]
        public void CreateHashSet()
        {
            var hashSet = new HashSet<ClassA>(arraySize);
        }

        [BenchmarkCategory("AddElements"), Benchmark]
        public void AddElemetsToHashSet()
        {
            var _hashset = new HashSet<ClassA>();
            for (int i = 0; i < arraySize; i++)
            {
                _hashset.Add(array[i]);
            }
        }

        [BenchmarkCategory("FindFirst"), Benchmark]
        public void FindFirstInHashset()
        {
            if (!hashSet.Contains(firstToFind))
                throw new Exception();
        }

        [BenchmarkCategory("FindSecond"), Benchmark]
        public void FindSecondInHashset()
        {
            if (!hashSet.Contains(secondToFind))
                throw new Exception();
        }

        [BenchmarkCategory("FindThird"), Benchmark]
        public void FindThirdInHashset()
        {
            if (!hashSet.Contains(thirdToFind))
                throw new Exception();
        }
    }
}

