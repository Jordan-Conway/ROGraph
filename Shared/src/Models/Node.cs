using ROGraph.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Shared.Models
{
    public class Node
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public Checklist? Checklist { get; set; }
        public Guid Origin { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }

        public Guid X { get; set; }
        public Guid Y { get; set; }
        public NodeType Type { get; set; }

        public Node(
            Guid id,
            string name, 
            Guid origin, 
            DateTime created, 
            DateTime lastModified,
            Guid x,
            Guid y,
            NodeType nodeType,
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
            this.X = x;
            this.Y = y;
            this.Type = nodeType;
            this.IsCompleted = isCompleted;
            this.Checklist = checklist;
            this.Description = description;
        }

        public (Guid, Guid) GetPosition()
        {
            return (this.X, this.Y);
        }

        public string GetImagePath()
        {
            string baseUrl = "pack://application:,,,/Images/";
            switch (this.Type)
            {
                case NodeType.Triangle:
                    {
                        return this.IsCompleted == true ? baseUrl + "triangle_node_completed.png" : baseUrl + "triangle_node_not_completed.png";
                    }
                default:
                    {
                        return this.IsCompleted == true ? baseUrl + "star_node_completed.png" : baseUrl + "star_node_not_completed.png";
                    }
            }
        }
    }
}
