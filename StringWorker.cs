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

        public async Task<ConcurrentDictionary<char, int>> CountUniqueSymbolsTask(string s)
        {
            var dict = new ConcurrentDictionary<char, int>();
            foreach (char c in s.Distinct())
            {
                dict[c] = await Task.Run(() => CountSymbolInSequence(c, s));
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

        private int CountSymbolInSequence(char c, string s)
        {
            int count = 0;
            foreach (char ch in s)
            {
                if (c == ch)
                    count++;
            }
            return count;
        }
    }
}
