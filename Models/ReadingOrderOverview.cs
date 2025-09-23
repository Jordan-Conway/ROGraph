using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public class ReadingOrderOverview
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public ReadingOrderOverview(string? name, string? description = null)
        {
            this.Name = name == null ? string.Empty : name;
            this.Description = description;
        }
    }
}