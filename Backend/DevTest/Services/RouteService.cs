using DevTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DevTest.Services
{
    public class RouteService : IRouteService
    {
        private const string filePath = "routes.txt";

        public Route GetRoute(string start, string destination)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var (path,totaltime) = FindShortestRoute(start, destination);
            sw.Stop();

            return new Route
            {
                Duration = sw.Elapsed.TotalSeconds,
                Path = path,
                TotalTime = totaltime,
                Stamp = DateTime.Now
            };
        }

        private (List<string> route, long totalTime) FindShortestRoute(string start, string destination)
        {
            var (graph, stationMap) = GetGraphFromTextFile();
            int startIndex = stationMap[start];
            int endIndex = stationMap[destination];

            var (distances, parents) = Dijkstras(graph, startIndex, stationMap.Count);

            var path = new List<int>();
            GetPath(endIndex, parents, path);

            var route = path.Select(i => stationMap.Single(d => d.Value == i).Key).ToList();

            return (route,distances[stationMap[destination]]);
        }

        private static (int[] distances,int[] parent) Dijkstras(int[,] graph, int source, int size)
        {
            int[] distances = new int[size];
            bool[] visited = new bool[size];

            for (int i = 0; i < size; ++i)
            {
                distances[i] = int.MaxValue;
                visited[i] = false;
            }

            int[] parents = new int[size];
            parents[source] = -1;

            distances[source] = 0;

            for (int i = 0; i < size - 1; ++i)
            {
                int toVisit = ClosestUnvisitedVertex(distances, visited, size);
                visited[toVisit] = true;

                for (int vertex = 0; vertex < size; ++vertex)
                {
                    if (!visited[vertex]
                        &&  0 < graph[toVisit, vertex]
                        && distances[toVisit] != int.MaxValue
                        && distances[toVisit] + graph[toVisit, vertex] < distances[vertex])
                    {
                         parents[vertex] = toVisit;
                         distances[vertex] = distances[toVisit] + graph[toVisit, vertex];
                    }
                }
            }
            return (distances, parents);
        }

        private static void GetPath(
            int currentVertex,
            int[] parents,
            List<int> list)
        {
            if (currentVertex == -1)
            {
                return;
            }
            GetPath(parents[currentVertex], parents, list);
            list.Add(currentVertex);
        }

        private static int ClosestUnvisitedVertex(int[] distance, bool[] visited, int size)
        {
            int min = int.MaxValue;
            int closestVertex = 0;

            for (int v = 0; v < size; ++v)
            {
                if (!visited[v] && distance[v] <= min)
                {
                    min = distance[v];
                    closestVertex = v;
                }
            }
            return closestVertex;
        }

        private static (int[,] graph, Dictionary<string, int> dictionary) GetGraphFromTextFile()
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);

            var dict = new Dictionary<string, int>();
            var index = 0;

            foreach (string line in lines)
            {
                var index1 = line.IndexOf(",");
                var index2 = line.LastIndexOf(",");
                var start = line.Substring(0, index1);
                var end = line.Substring(index1 + 1, index2 - index1 - 1);
                var minues = line.Substring(index2 + 1);
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
                var index1 = line.IndexOf(",");
                var index2 = line.LastIndexOf(",");
                var start = line.Substring(0, index1);
                var end = line.Substring(index1 + 1, index2 - index1 - 1);
                var minues = int.Parse(line.Substring(index2 + 1));
                graph[dict[start], dict[end]] = minues;
                graph[dict[end], dict[start]] = minues;
            }
            return (graph,dict);
        }
    }
}