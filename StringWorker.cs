using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParrallelProj
{
    public sealed class StringWorker
    {
        public Dictionary<char, int> CountUniqueSymbols(string s) 
        {
            var dict = new Dictionary<char, int>();
            foreach (char ch in s)
            {
                if (!dict.ContainsKey(ch))
                {
                    dict[ch] = 1;
                }
                else
                    dict[ch]++;
            }
            return dict;
        }

        public ConcurrentDictionary<char, int> CountUniqueSymbolsParrallel(string s)
        {
            var dict = new ConcurrentDictionary<char, int>();
            Parallel.ForEach(s, ch =>
            {
                if (!dict.ContainsKey(ch))
                {
                    dict[ch] = 1;
                }
                else
                    dict[ch]++;
            });
            return dict;
        }

        public async Task<ConcurrentDictionary<char, int>> CountUniqueSymbolsAllTask(string s)
        {
            var dict = new ConcurrentDictionary<char, int>();
            foreach (char c in s.Distinct())
            {
                Task.Run(() => CountSymbolInSequence(dict, c, s));
            }
            Task.WaitAll();
            return dict;
        }

        public async Task<ConcurrentDictionary<char,int>> CountUniqueSymbolsChunkTask(string s, int chunckSize)
        {
            var dict = new ConcurrentDictionary<char, int>();
            if (chunckSize <= s.Length)
            {
                List<Dictionary<char, int>> allTempDicts = new();

                foreach (char[] chunck in s.Chunk(chunckSize))
                {
                     allTempDicts.Add(await Task.Run(() => CountUniqueSymbols(chunck.ToString())));
                }

                for (int i = 0; i < allTempDicts.Count; i++)
                {
                    foreach (KeyValuePair<char, int> pair in allTempDicts[i])
                    {
                        if(dict.ContainsKey(pair.Key))
                            dict[pair.Key] += pair.Value;
                        else
                            dict.TryAdd(pair.Key, pair.Value);
                    }
                }
                return dict;
            }
            else
            {
                return dict;
            }
        }

        public ConcurrentDictionary<char, int> CountUniqueSymbolsChunkThread(string s, int chunckSize)
        {
            var dict = new ConcurrentDictionary<char, int>();
            if (chunckSize <= s.Length)
            {
                List<Thread> threads = new();
                foreach (char[] chunk in s.Chunk(chunckSize))
                {
                    threads.Add(new Thread(() => CountUniqueSymbols(dict, chunk.ToString())));
                }
                foreach (Thread thread in threads)
                {
                    thread.Start();
                }
                while(!threads.All(x => x.IsAlive == false)){ }
                return dict;
            }
            else
            {
                return dict;
            }
        }

        private void CountUniqueSymbols(ConcurrentDictionary<char, int> dict, string s)
        {
            foreach (char ch in s)
            {
                if (!dict.ContainsKey(ch))
                {
                    dict.TryAdd(ch, 1);
                }
                else
                {
                    dict[ch]++;
                }
            }
        }

        private void CountSymbolInSequence(ConcurrentDictionary<char,int> dict, char c, string s)
        {
            int count = 0;
            foreach (char ch in s)
            {
                if (c == ch)
                    count++;
            }
            dict[c] = count;
        }
    }
}
