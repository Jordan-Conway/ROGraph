namespace ROGraph.Shared.Models
{
    public class ReadingOrderOverview
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public string? Description { get; set; }

        public int MaxX { get; set; }
        public int MaxY { get; set; }


        public ReadingOrderOverview(string? name, Guid id, string? description = null, int maxX = 0, int maxY = 0)
        {
            this.Name = name ?? string.Empty;
            this.Id = id;
            this.Description = description;
            MaxX = maxX;
            MaxY = maxY;
        }
    }
}