using System;
using System.Collections.Generic;

namespace DevTest.Helpers
{
    public static class Dijkstras
    {
        public static (int[] distances,int[] parent) Calculate(int[,] graph, int source, int size)
        {
            var distances = new int[size];
            var visited = new bool[size];

            for (int i = 0; i < size; ++i)
            {
                distances[i] = Int32.MaxValue;
                visited[i] = false;
            }

            var parents = new int[size];
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
                        && distances[toVisit] != Int32.MaxValue
                        && distances[toVisit] + graph[toVisit, vertex] < distances[vertex])
                    {
                        parents[vertex] = toVisit;
                        distances[vertex] = distances[toVisit] + graph[toVisit, vertex];
                    }
                }
            }
            return (distances, parents);
        }

        public static ICollection<int> GetPath(int currentVertex, int[] parents)
        {
            var pathList = new List<int>();
            Path(currentVertex,parents,pathList);
            return pathList;
        }

        private static int ClosestUnvisitedVertex(int[] distance, bool[] visited, int size)
        {
            var min = Int32.MaxValue;
            var closestVertex = 0;

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



        private static void Path(
            int currentVertex,
            int[] parents,
            ICollection<int> list)
        {
            if (currentVertex == -1)
            {
                return;
            }
            Path(parents[currentVertex], parents, list);
            list.Add(currentVertex);
        }

    }
}
