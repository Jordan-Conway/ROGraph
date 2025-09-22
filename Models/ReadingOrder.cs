using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public class ReadingOrder
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int PageNumber { get; set; }
        public ReadingOrderNodes Nodes { get; set; }

        public ReadingOrder(String name, int pageNumber, ReadingOrderNodes? nodes = null, string description = "")
        {
            this.Name = name;
            this.PageNumber = pageNumber;
            
            if (nodes == null )
            {
                this.Nodes = new ReadingOrderNodes();
            }
            else
            {
                this.Nodes = nodes;
            }

                this.Description = description;
        }
    }
}
