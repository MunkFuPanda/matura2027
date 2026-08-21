using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra___Algorithmen
{
    internal class Edge
    {
        public Place From { get; set; }
        public Place To { get; set; }
        public double Distance { get; set; }

        public Edge(Place from, Place to, double distance)
        {
            From = from;
            To = to;
            Distance = distance;
        }
    }
}
