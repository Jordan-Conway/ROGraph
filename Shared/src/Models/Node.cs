using ROGraph.Shared.Enums;

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
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

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
            this.Created = created;
            this.LastModified = lastModified;
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
            const string baseUrl = "pack://application:,,,/Images/";
            return this.Type switch
            {
                NodeType.TRIANGLE => this.IsCompleted
                    ? baseUrl + "triangle_node_completed.png"
                    : baseUrl + "triangle_node_not_completed.png",
                _ => IsCompleted
                    ? baseUrl + "star_node_completed.png"
                    : baseUrl + "star_node_not_completed.png"
            };
        }
    }
}
