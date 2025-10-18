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
        public Guid Id { get; set; }
        public CoordinateTranslator? CoordinateTranslator { get; set; }
        public ReadingOrderContents Contents { get; set; }

        public ReadingOrder(String name, Guid? guid, ReadingOrderContents? nodes = null, string description = "")
        {
            this.Name = name;

            if (guid == null)
            {
                this.Id = Guid.NewGuid();
            }
            else
            {
                this.Id = (Guid)guid;
            }

            if (nodes == null)
            {
                this.Contents = new ReadingOrderContents();
            }
            else
            {
                this.Contents = nodes;
            }

            this.Description = description;
        }

        public void AddColumn(int colNumber)
        {
            if (this.CoordinateTranslator == null)
            {
                return;
            }

            this.CoordinateTranslator.AddNewColumn(colNumber);
        }

        public void AddRow(int rowNumber)
        {
            if (this.CoordinateTranslator == null)
            {
                return;
            }

            this.CoordinateTranslator.AddNewRow(rowNumber);
        }
    }
}
