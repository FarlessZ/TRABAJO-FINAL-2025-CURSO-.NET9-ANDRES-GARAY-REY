using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Entities.Infos;
using PortalGalaxy.Repositories.Interfaces;

namespace PortalGalaxy.Repositories.Implementaciones;

public class InstructorRepository : RepositoryBase<Instructor>, IInstructorRepository
{
    public InstructorRepository(PortalGalaxyDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<InstructorInfo>> ListAsync(string? nombre, string? nroDocumento, int? categoriaId)
    {
        Expression<Func<Instructor, bool>> predicate = x =>
            (string.IsNullOrEmpty(nombre) || x.Nombres.Contains(nombre)) &&
            (string.IsNullOrEmpty(nroDocumento) || x.NroDocumento.Equals(nroDocumento)) &&
            (!categoriaId.HasValue || x.CategoriaId == categoriaId.Value);

        return await Context.Set<Instructor>()
            .Where(predicate)
            // .IgnoreQueryFilters() // Ignorar filtros globales (soft delete)
            .Select(x => new InstructorInfo
            {
                Id = x.Id,
                Nombres = x.Nombres,
                NroDocumento = x.NroDocumento,
                Categoria = x.Categoria.Nombre, // Lazy loading - EF Core
                CategoriaId = x.CategoriaId
            })
            .ToListAsync();
    }
}
