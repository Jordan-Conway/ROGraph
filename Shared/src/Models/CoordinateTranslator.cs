using ROGraph.Shared.Models;

namespace ROGraph.Shared.Models
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

        public (int, int) Translate(Node node)
        {
            return this.Translate(node.GetPosition());
        }

        public (int, int) Translate((Guid, Guid) posiion)
        {
            return (this.ColumnIds[posiion.Item1], this.RowIds[posiion.Item2]);
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

        public Guid AddNewRow(int rowNumber)
        {
            foreach(Guid rowId in this.RowIds.Keys)
            {
                if(this.RowIds[rowId] >= rowNumber)
                {
                    this.RowIds[rowId] = this.RowIds[rowId] + 1;
                }
            }

            Guid newGuid = Guid.NewGuid();
            this.RowIds.Add(newGuid, rowNumber);
            this.InvalidateReversedYIds();

            return newGuid;
        }

        public void DeleteRow(int rowNumber)
        {
            Guid toDelete = this.GetYFromInt(rowNumber);
            this.RowIds.Remove(toDelete);
            foreach(Guid rowId in this.RowIds.Keys)
            {
                if (this.RowIds[rowId] > rowNumber)
                {
                    this.RowIds[rowId]--;
                }
            }
            this.InvalidateReversedYIds();
        }

        public Guid AddNewColumn(int colNumber)
        {
            foreach (Guid colId in this.ColumnIds.Keys)
            {
                if (this.ColumnIds[colId] >= colNumber)
                {
                    this.ColumnIds[colId] = this.ColumnIds[colId] + 1;
                }
            }

            Guid newGuid = Guid.NewGuid();
            this.ColumnIds.Add(newGuid, colNumber);
            this.InvalidateReversedXIds();

            return newGuid;
        }

        public void DeleteColumn(int colNumber)
        {
            Guid toDelete = this.GetXFromInt(colNumber);
            this.ColumnIds.Remove(toDelete);
            foreach(Guid colId in this.ColumnIds.Keys)
            {
                if (this.ColumnIds[colId] > colNumber)
                {
                    this.ColumnIds[colId]--;
                }
            }
            this.InvalidateReversedXIds();
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
