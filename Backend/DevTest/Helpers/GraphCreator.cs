using System;
using System.Collections.Generic;
using System.IO;
using static System.Int32;

namespace DevTest.Helpers
{
    public static class GraphCreator
    {
        private const string FilePath = "routes.txt";

        public static (int[,] graph, Dictionary<string, int> dictionary) CreateGraphFromFile()
        {
            string[] lines = File.ReadAllLines(FilePath);

            var dict = new Dictionary<string, int>();
            var index = 0;

            foreach (string line in lines)
            {
                var index1 = line.IndexOf(",", StringComparison.Ordinal);
                var index2 = line.LastIndexOf(",", StringComparison.Ordinal);
                var start = line.Substring(0, index1);
                var end = line.Substring(index1 + 1, index2 - index1 - 1);
                if (!dict.ContainsKey(start))
                {
                    dict.Add(start, index++);
                }
                if (!dict.ContainsKey(end))
                {
                    dict.Add(end, index++);
                }
            }

            int[,] graph = new int[dict.Count, dict.Count];

            for (int i = 0; i < dict.Count; ++i)
            {
                for (int j = 0; j < dict.Count; ++j)
                {
                    graph[i, j] = 0;
                }
            }

            foreach (string line in lines)
            {
                var index1 = line.IndexOf(",", StringComparison.Ordinal);
                var index2 = line.LastIndexOf(",", StringComparison.Ordinal);
                var start = line.Substring(0, index1);
                var end = line.Substring(index1 + 1, index2 - index1 - 1);
                var minutes = Parse(line.Substring(index2 + 1));
                graph[dict[start], dict[end]] = minutes;
                graph[dict[end], dict[start]] = minutes;
            }
            return (graph,dict);
        }
    }
}
