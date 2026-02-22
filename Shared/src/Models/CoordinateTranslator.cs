using System.Diagnostics.CodeAnalysis;

namespace ROGraph.Shared.Models
{
    public class CoordinateTranslator
    {
        private readonly Dictionary<Guid, int> _columnIds;
        private readonly Dictionary<Guid, int> _rowIds;
        private Dictionary<int, Guid> _reversedColumnIds;
        private Dictionary<int, Guid> _reversedRowIds;
        public CoordinateTranslator(int maxX, int maxY)
        {
            this._columnIds = new Dictionary<Guid, int>();
            this._rowIds = new Dictionary<Guid, int>();

            for (int i = 0; i < maxX; i++)
            {
                this._columnIds.Add(Guid.NewGuid(), i);
            }

            for (int i = 0; i < maxY; i++)
            {
                this._rowIds.Add(Guid.NewGuid(), i);
            }

            this.InvalidateReversedIds();
        }

        public (int, int) Translate(Node node)
        {
            return this.Translate(node.GetPosition());
        }

        public (int, int) Translate((Guid, Guid) position)
        {
            return (this._columnIds[position.Item1], this._rowIds[position.Item2]);
        }

        public Result<int> GetXFromId(Guid id)
        {
            return this._columnIds.TryGetValue(id, out var columnId) ? new Result<int>(true, columnId) : new Result<int>(false, 0);
        }

        public Result<int> GetYFromId(Guid id)
        {
            return this._rowIds.TryGetValue(id, out var rowId) ? new Result<int>(true, rowId) : new Result<int>(false, 0);
        }

        public Guid GetXFromInt(int coordinate)
        {
            if (this._reversedColumnIds.TryGetValue(coordinate, out Guid value))
            {
                return value;
            }

            return this.AddNewColumn(coordinate);
        }

        public Guid GetYFromInt(int coordinate)
        {
            if (this._reversedRowIds.TryGetValue(coordinate, out Guid value))
            {
                return value;
            }

            return this.AddNewRow(coordinate);
        }

        public Guid AddNewRow(int rowNumber)
        {
            foreach(Guid rowId in this._rowIds.Keys)
            {
                if(this._rowIds[rowId] >= rowNumber)
                {
                    this._rowIds[rowId] = this._rowIds[rowId] + 1;
                }
            }

            Guid newGuid = Guid.NewGuid();
            this._rowIds.Add(newGuid, rowNumber);
            this.InvalidateReversedRowIds();

            return newGuid;
        }

        public void DeleteRow(int rowNumber)
        {
            Guid toDelete = this.GetYFromInt(rowNumber);
            this._rowIds.Remove(toDelete);
            foreach(Guid rowId in this._rowIds.Keys)
            {
                if (this._rowIds[rowId] > rowNumber)
                {
                    this._rowIds[rowId]--;
                }
            }
            this.InvalidateReversedRowIds();
        }

        public Guid AddNewColumn(int colNumber)
        {
            foreach (Guid colId in this._columnIds.Keys)
            {
                if (this._columnIds[colId] >= colNumber)
                {
                    this._columnIds[colId] = this._columnIds[colId] + 1;
                }
            }

            Guid newGuid = Guid.NewGuid();
            this._columnIds.Add(newGuid, colNumber);
            this.InvalidateReversedColumnIds();

            return newGuid;
        }

        public void DeleteColumn(int colNumber)
        {
            Guid toDelete = this.GetXFromInt(colNumber);
            this._columnIds.Remove(toDelete);
            foreach(Guid colId in this._columnIds.Keys)
            {
                if (this._columnIds[colId] > colNumber)
                {
                    this._columnIds[colId]--;
                }
            }
            this.InvalidateReversedColumnIds();
        }

        [MemberNotNull(nameof(_reversedColumnIds))]
        [MemberNotNull(nameof(_reversedRowIds))]
        private void InvalidateReversedIds()
        {
            this.InvalidateReversedColumnIds();
            this.InvalidateReversedRowIds();
        }
        
        [MemberNotNull(nameof(_reversedColumnIds))]
        private void InvalidateReversedColumnIds()
        {
            this._reversedColumnIds = this._columnIds.ToDictionary(x => x.Value, x => x.Key);
        }

        [MemberNotNull(nameof(_reversedRowIds))]
        private void InvalidateReversedRowIds()
        {
            this._reversedRowIds = this._rowIds.ToDictionary(x => x.Value, x => x.Key);
        }
        
        public int GetNumberOfRows()
        {
            return _reversedRowIds.Keys.Max();
        }

        public int GetNumberOfColumns()
        {
            return _reversedColumnIds.Keys.Max();
        }
    }
    public readonly struct Result<T>(bool success, T output)
    {
        public bool Success { get;} = success;
        public T? Output { get;} = output;
    }
}
