using ROGraph.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public class Node
    {
        public required string Name { get; set; }
        public required NodeType Type { get; set; }
    }
}
