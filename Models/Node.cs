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
        public string Name { get; set; } = name;

        public int X { get; set; } = x;

        public int Y { get; set; } = y;
        public NodeType Type { get; set; } = type;
    }
}
