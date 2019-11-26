using System;
using System.Collections.Generic;

namespace DevTest.Models
{
    public class Route
    {
        public List<string> Path { get; set; }

        public long TotalTime { get; set; }

        public double Duration { get; set; }

        public DateTime? Stamp { get; set; }
    }
}