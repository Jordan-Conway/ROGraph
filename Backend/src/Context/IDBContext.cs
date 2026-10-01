using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Context;

public interface IDBContext
{
    public Task Add<T>(T entity, CancellationToken token) where T : class;
    
    public DbSet<T> GetSet<T>() where T : class;
    
    public Task<int> Save(CancellationToken token);
}