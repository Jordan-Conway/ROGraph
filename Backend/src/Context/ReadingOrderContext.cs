using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ROGraph.Backend.DatabaseModels;

namespace ROGraph.Backend.Context;

internal class ReadingOrderContext : DbContext, IDBContext
{
    public DbSet<ConnectorDbModel> Connectors { get; set; }
    public DbSet<NodeDbModel> Nodes { get; set; }
    public DbSet<NodePlacementDbModel>  NodePlacements { get; set; }
    public DbSet<ReadingOrderOverviewDbModel> ReadingOrderOverviews { get; set; }
    
    private string DbFilePath { get; } = FilePathProvider.GetDatabaseFilePath();
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) 
        => options.UseSqlite($"Data Source={DbFilePath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConnectorDbModel>();
        modelBuilder.Entity<NodeDbModel>();
        modelBuilder.Entity<NodePlacementDbModel>();
        modelBuilder.Entity<ReadingOrderOverviewDbModel>();
    }
    
    public DbSet<T> GetSet<T>() where T : class
    {
        return Set<T>();
    }

    public async Task Add<T>(T entity, CancellationToken token) where T : class
    {
        await GetSet<T>().AddAsync(entity, token);
    }

    public async Task<int> Save(CancellationToken token)
    {
        return await base.SaveChangesAsync(token);
    }
}