using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DocumentsPages
{
    public class StateSuggestionProvider
    {
        private const int LevensteinDist = 2;

        private static string Cut(string lastName, int n)
        {
            return lastName.Length < n ? lastName : lastName.Substring(0, n);
        }

        public List<string> SearchByName(string name, List<string> names)
        {
            var resultList = new List<(string, int)>();
            var d1 = DateTime.Now;
            Parallel.ForEach(names, (filter) =>
            {
                int dist = LevenshteinDistance(Cut(filter, name.Length), name);
                if (dist >= LevensteinDist) return;
                lock (resultList)
                    resultList.Add((filter, dist));
            });
            resultList.Sort((x, y) => -y.Item2.CompareTo(x.Item2));
            var d2 = DateTime.Now;
            Debug.WriteLine(d2 - d1);
            return resultList.ConvertAll(input => input.Item1);
        }
        private static int LevenshteinDistance(string string1, string string2)
        {
            if (string1 == null) throw new ArgumentNullException();
            if (string2 == null) throw new ArgumentNullException();
            var m = new int[string1.Length + 1, string2.Length + 1];

            for (var i = 0; i <= string1.Length; i++) m[i, 0] = i;
            for (var j = 0; j <= string2.Length; j++) m[0, j] = j;

            for (var i = 1; i <= string1.Length; i++)
                for (var j = 1; j <= string2.Length; j++)
                {
                    int diff = char.ToLower(string1[i - 1]) == char.ToLower(string2[j - 1]) ? 0 : 1;
                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                            m[i, j - 1] + 1),
                        m[i - 1, j - 1] + diff);
                }
            return m[string1.Length, string2.Length];
        }
    }
}