using Bogus;
using Bogus.Extensions.Poland;
using Bogus.Extensions.UnitedStates;
using Microsoft.Extensions.DependencyInjection;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;

namespace PortalGalaxy.Services.Implementaciones;

public static class DataSeeder
{
    public static async Task SeedInstructores(IServiceProvider serviceProvider)
    {
        var fakerData = new Faker<Instructor>()
            .UseSeed(1234)
            .RuleFor(i => i.Nombres, f => f.Name.FullName())
            .RuleFor(i => i.NroDocumento, f => f.Person.Ssn())
            .RuleFor(i => i.CategoriaId, f => f.Random.Int(1, 4));

        var list = fakerData.Generate(50);
        await using var context = serviceProvider.GetRequiredService<PortalGalaxyDbContext>();

        context.Set<Instructor>().AddRange(list);
        await context.SaveChangesAsync();
    }
    
    
    public static async Task SeedTalleres(IServiceProvider serviceProvider)
    {
        var fakerData = new Faker<Taller>()
            .UseSeed(1234)
            .RuleFor(i => i.Nombre, f => f.Company.CompanyName())
            .RuleFor(i => i.Descripcion, f => f.Company.Regon())
            .RuleFor(i => i.CategoriaId, f => f.Random.Int(1, 4))
            .RuleFor(i => i.InstructorId, f => f.Random.Int(1, 50))
            .RuleFor(i => i.FechaInicio, f => DateOnly.FromDateTime(f.Date.Future()))
            .RuleFor(i => i.HoraInicio, f => TimeOnly.FromDateTime(f.Date.Future()))
            .RuleFor(i => i.Situacion, f => f.PickRandom<SituacionTaller>());

        var list = fakerData.Generate(150);
        await using var context = serviceProvider.GetRequiredService<PortalGalaxyDbContext>();

        context.Set<Taller>().AddRange(list);
        await context.SaveChangesAsync();
    }
}