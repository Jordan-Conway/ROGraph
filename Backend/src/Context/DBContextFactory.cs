namespace ROGraph.Backend.Context;

public class DBContextFactory : IDBContextFactory
{
    public IDBContext CreateDbContext()
    {
        return new ReadingOrderContext();
    }
}