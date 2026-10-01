namespace ROGraph.Backend.Context;

public interface IDBContextFactory
{
    public IDBContext CreateDbContext();
}