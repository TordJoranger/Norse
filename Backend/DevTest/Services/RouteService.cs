using DevTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DevTest.Helpers;

namespace DevTest.Services
{
    public class RouteService : IRouteService
    {
        public Route GetRoute(string start, string destination)
        {
            var sw = new Stopwatch();
            sw.Start();
            var (path,totalTime) = FindShortestRoute(start, destination);
            sw.Stop();

            return new Route
            {
                Duration = sw.Elapsed.TotalSeconds,
                Path = path,
                TotalTime = totalTime,
                Stamp = DateTime.Now
            };
        }

        private static (List<string> route, long totalTime) FindShortestRoute(string start, string destination)
        {
            var (graph, stationMap) = GraphCreator.CreateGraphFromFile();
            var startIndex = stationMap[start];
            var endIndex = stationMap[destination];

            var (distances, parents) = Dijkstras.Calculate(graph, startIndex, stationMap.Count);

            var path = Dijkstras.GetPath(endIndex, parents);

            var route = path?.Select(i => stationMap.Single(d => d.Value == i).Key).ToList();

            return (route,distances[stationMap[destination]]);
        }
    }
}