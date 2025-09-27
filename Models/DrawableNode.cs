using ROGraph.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public class DrawableNode: Node
    {
        public Guid X;
        public Guid Y;
        public NodeType Type;

        public DrawableNode(
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
            ): base(id, name, origin, created, lastModified, isCompleted, checklist, description) 
        {
            this.X = x;
            this.Y = y;
            this.Type = nodeType;
        }

        public override Guid GetX()
        {
            return this.X;
        }

        public override Guid GetY()
        {
            return this.Y;
        }
    }   
}
