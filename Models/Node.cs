using ROGraph.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public class Node(string name, NodeType type, int x = 0, int y = 0)
    {
        public required string Name { get; set; } = name;

        public required int X { get; set; } = x;

        public required int Y { get; set; } = y;
        public required NodeType Type { get; set; } = type;
    }
}
