using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.Interfaces;

public interface IReadingOrderProvider
{
    public abstract bool CreateReadingOrder(string name, string? description);

    public abstract ReadingOrder? GetReadingOrder(ReadingOrderOverview overview);
}
