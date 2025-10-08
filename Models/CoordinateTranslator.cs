using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Models
{
    public class CoordinateTranslator
    {
        private Dictionary<Guid, int> ColumnIds;
        private Dictionary<Guid, int> RowIds;
        private Dictionary<int, Guid> ReversedColumnIds;
        private Dictionary<int, Guid> ReversedRowIds;
        public CoordinateTranslator(int maxX, int maxY)
        {
            this.ColumnIds = new Dictionary<Guid, int>();
            this.RowIds = new Dictionary<Guid, int>();

            for (int i = 0; i < maxX; i++)
            {
                this.ColumnIds.Add(Guid.NewGuid(), i);
            }

            for (int i = 0; i < maxY; i++)
            {
                this.RowIds.Add(Guid.NewGuid(), i);
            }

            this.InvalidateReversedIds();
        }

        public (int, int) Translate(DrawableNode node)
        {
            return (this.ColumnIds[node.X], this.RowIds[node.Y]);
        }

        public Result<int> GetXFromId(Guid id)
        {
            if (this.ColumnIds.ContainsKey(id))
            {
                return new Result<int>(true, this.ColumnIds[id]);
            }

            return new Result<int>(false, 0);
        }

        public Result<int> GetYFromId(Guid id)
        {
            if (this.RowIds.ContainsKey(id))
            {
                return new Result<int>(true, this.RowIds[id]);
            }

            return new Result<int>(false, 0);
        }

        public Guid GetXFromInt(int coordinate)
        {
            if (this.ReversedColumnIds.TryGetValue(coordinate, out Guid value))
            {
                return value;
            }

            return this.AddNewColumn(coordinate);
        }

        public Guid GetYFromInt(int coordinate)
        {
            if (this.ReversedRowIds.TryGetValue(coordinate, out Guid value))
            {
                return value;
            }

            return this.AddNewRow(coordinate);
        }

        private Guid AddNewRow(int rowNumber)
        {
            Guid newGuid = Guid.NewGuid();
            this.RowIds.Add(newGuid, rowNumber);
            this.InvalidateReversedYIds();

            return newGuid;
        }

        private Guid AddNewColumn(int colNumber)
        {
            Guid newGuid = Guid.NewGuid();
            this.ColumnIds.Add(newGuid, colNumber);
            this.InvalidateReversedXIds();

            return newGuid;
        }

        private void InvalidateReversedIds()
        {
            this.InvalidateReversedXIds();
            this.InvalidateReversedYIds();
        }

        private void InvalidateReversedXIds()
        {
            this.ReversedColumnIds = this.ColumnIds.ToDictionary(x => x.Value, x => x.Key);
        }

        private void InvalidateReversedYIds()
        {
            this.ReversedRowIds = this.RowIds.ToDictionary(x => x.Value, x => x.Key);
        }
    }
    public struct Result<T>(bool success, T output)
    {
        public bool Success { get;} = success;
        public T? Output { get;} = output;
    }
}
