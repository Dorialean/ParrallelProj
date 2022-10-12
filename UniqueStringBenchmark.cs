using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParrallelProj
{
    [MemoryDiagnoser]
    [RankColumn]
    public class UniqueStringBenchmark
    {
        [Params(10, 100, 1000, 100_000, 1_000_000)]
        public int Amount { get; set; }

        private string _s = string.Empty;
        private StringWorker _sw;

        [GlobalSetup]
        public void Setup()
        {
            Random rnd = new();
            for (int i = 0; i < Amount; i++)
            {
                _s += (char)rnd.Next(0, 256);
            }
            _sw = new();
        }

        [Benchmark]
        public Dictionary<char, int> Usual_Way() => _sw.CountUniqueSymbols(_s);

        [Benchmark]
        public Task<ConcurrentDictionary<char, int>> Task_Way() => _sw.CountUniqueSymbolsTask(_s);

        [Benchmark]
        public ConcurrentDictionary<char, int> Parrallel_Way() => _sw.CountUniqueSymbolsParrallel(_s);



    }
}
