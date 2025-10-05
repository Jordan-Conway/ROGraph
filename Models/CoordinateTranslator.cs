using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public class CoordinateTranslator
    {
        private Dictionary<Guid, int> XIds;
        private Dictionary<Guid, int> YIds;
        private Dictionary<int, Guid> ReversedXIds;
        private Dictionary<int, Guid> ReversedYIds;
        public CoordinateTranslator(int maxX, int maxY) 
        {
            this.XIds = new Dictionary<Guid, int>();
            this.YIds = new Dictionary<Guid, int>();

            for (int i = 0; i < maxX; i++)
            {
                this.XIds.Add(Guid.NewGuid(), i);
            }

            for (int i = 0; i < maxY; i++)
            {
                this.YIds.Add(Guid.NewGuid(), i);
            }

            this.ReversedXIds = this.XIds.ToDictionary(x => x.Value, x => x.Key);
            this.ReversedYIds = this.YIds.ToDictionary(x => x.Value, x => x.Key);
        }

        public (int, int) Translate(DrawableNode node)
        {
            return (this.XIds[node.X], this.YIds[node.Y]);
        }

        public Result<int> GetXFromId(Guid id)
        {
            if(this.XIds.ContainsKey(id))
            {
                return new Result<int>(true, this.XIds[id]);
            }

            return new Result<int>(false, 0);
        }

        public Result<int> GetYFromId(Guid id)
        {
            if (this.YIds.ContainsKey(id))
            {
                return new Result<int>(true, this.YIds[id]);
            }

            return new Result<int>(false, 0);
        }

        public Result<Guid> GetXFromInt(int coordinate)
        {
            if(this.ReversedXIds.TryGetValue(coordinate, out Guid value))
            { 
                return new Result<Guid>(true, value);
            }

            return new Result<Guid>(false, new Guid(""));
        }

        public Result<Guid> GetYFromInt(int coordinate)
        {
            if (this.ReversedYIds.TryGetValue(coordinate, out Guid value))
            {
                return new Result<Guid>(true, value);
            }

            return new Result<Guid>(false, new Guid(""));
        }
    }
    public struct Result<T>(bool success, T output)
    {
        public bool Success { get; set; } = success;
        public T Output { get; set; } = output;
    }
}
