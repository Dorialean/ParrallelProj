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
        [Params(1, 2, 5, 10)]
        public int ChunkSize { get; set; }

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
        public ConcurrentDictionary<char, int> Parrallel_Way() => _sw.CountUniqueSymbolsParrallel(_s);

        [Benchmark]
        public ConcurrentDictionary<char, int> Task_Full_Run() => _sw.CountUniqueSymbolsAllTask(_s).Result;

        [Benchmark]
        public ConcurrentDictionary<char, int> Task_Chunk_Run() => _sw.CountUniqueSymbolsChunkTask(_s,ChunkSize).Result;

        [Benchmark]
        public ConcurrentDictionary<char, int> Thread_Chunk_Run() => _sw.CountUniqueSymbolsChunkThread(_s,ChunkSize);



    }
}
