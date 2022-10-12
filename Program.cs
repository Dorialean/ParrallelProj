using BenchmarkDotNet.Running;

namespace ParrallelProj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<UniqueStringBenchmark>();
        }
    }
}