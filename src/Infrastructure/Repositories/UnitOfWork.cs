using System.Collections;
using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.Persistence;

namespace Ecommerce.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable? _repositories;
    private readonly EcommerceDbContext _context;

    public UnitOfWork(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<int> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories is null)
        {
            _repositories = new Hashtable();
        }
        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<>);
            var repositotyInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(TEntity)));
            _repositories.Add(type, repositotyInstance);
        }

        return (IAsyncRepository<TEntity>)_repositories[type]!;
    }
}