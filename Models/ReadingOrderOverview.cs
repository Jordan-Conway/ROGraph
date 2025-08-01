using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public class ReadingOrderOverview
    {
        public required string Name { get; set; }
        public required string Page { get; set; }

        public string? Description { get; set; }
    }
}