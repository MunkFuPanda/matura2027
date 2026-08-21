using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra___Algorithmen
{
    internal class Node
    {
        public Place Place { get; set; }
        public List<Edge> Edges { get; set; } = new();
        public double Distance { get; set; } = double.MaxValue;

        public Node? Previous { get; set; }
    }
}
