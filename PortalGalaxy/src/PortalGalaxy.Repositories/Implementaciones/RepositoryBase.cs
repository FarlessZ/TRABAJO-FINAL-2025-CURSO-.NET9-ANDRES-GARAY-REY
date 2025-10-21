using Microsoft.EntityFrameworkCore;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;
using System.Linq.Expressions;

namespace PortalGalaxy.Repositories.Implementaciones;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    protected readonly DbContext Context;

    protected RepositoryBase(DbContext context)
    {
        Context = context;
    }

    public async Task<ICollection<TEntity>> ListAsync()
    {
        return await Context.Set<TEntity>()
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ToListAsync();
    }

    public async Task<TEntity?> FindAsync(int id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync()
    {
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var registro = await FindAsync(id);
        if (registro is null)
            throw new InvalidOperationException("No se encontró el registro");

        registro.Estado = false;
        await UpdateAsync();
    }

    public async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>()
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<TInfo>> ListAsync<TInfo>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TInfo>> selector, 
        string? relations = null)
    {
        var collection = Context.Set<TEntity>()
            .Where(predicate)
            .AsQueryable();

        // SELECT DE TALLERS: "Instructor,Categoria"
        if (!string.IsNullOrWhiteSpace(relations))
        {
            foreach (var tabla in relations.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                collection = collection.Include(tabla);
            }
        }

        return await collection
            .AsNoTracking()
            .Select(selector)
            .ToListAsync();
    }

    public async Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TInfo>> selector, Expression<Func<TEntity, TKey>> orderBy, int pageNumber = 1, int pageSize = 5, string? relations = null)
    {
        var collection = Context.Set<TEntity>()
            .Where(predicate)
            .AsQueryable();

        // SELECT DE TALLERS: "Instructor,Categoria"
        if (!string.IsNullOrWhiteSpace(relations))
        {
            foreach (var tabla in relations.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                collection = collection.Include(tabla);
            }
        }

        var total = await Context.Set<TEntity>()
                    .Where(predicate)
                    .CountAsync();

        var items = await collection
            .AsNoTracking()
            .IgnoreQueryFilters()
            .OrderBy(orderBy)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync();

        return (items, total);
    }
}