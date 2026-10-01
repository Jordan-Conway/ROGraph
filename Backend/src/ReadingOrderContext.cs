using Microsoft.EntityFrameworkCore;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Shared.Models;

namespace ROGraph.Backend;

internal class ReadingOrderContext : DbContext
{
    public DbSet<ConnectorDbModel> Connectors { get; set; }
    public DbSet<NodeDbModel> Nodes { get; set; }
    public DbSet<ReadingOrderOverviewDbModel> ReadingOrderOverviews { get; set; }
    
    private string DbFilePath { get; } = FilePathProvider.GetDatabaseFilePath();
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) 
        => options.UseSqlite($"Data Source={DbFilePath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConnectorDbModel>();
        modelBuilder.Entity<NodeDbModel>();
        modelBuilder.Entity<ReadingOrderOverviewDbModel>();
    }
}