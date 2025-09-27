using ROGraph.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public abstract class Node
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public Checklist? Checklist { get; set; }
        public Guid Origin { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }

        public Node(
            Guid id,
            string name, 
            Guid origin, 
            DateTime created, 
            DateTime lastModified, 
            bool isCompleted = false,
            Checklist? checklist = null,
            string? description = null
            )
        {
            this.Id = id;
            this.Name = name;
            this.Origin = origin;
            this.created = created;
            this.lastModified = lastModified;
            this.IsCompleted = isCompleted;
            this.Checklist = checklist;
            this.Description = description;
        }

        public abstract Guid GetX();
        public abstract Guid GetY();
    }
}
