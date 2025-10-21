using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortalGalaxy.Common.Configuration;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Repositories.Interfaces;
using PortalGalaxy.Services.Implementaciones;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Services.Profiles;
using QuestPDF.Infrastructure;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

string corsConfiguration = "PortalGalaxyCORS";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(policy =>
{
    policy.AddPolicy(corsConfiguration, p =>
    {
        p.AllowAnyOrigin();
        p.AllowAnyHeader();
        p.AllowAnyMethod();
    });
});

// Registramos las dependencias de forma automatica con Scrutor
builder.Services.Scan(s => s
    .FromAssemblies(typeof(ICategoriaRepository).Assembly,
    typeof(IUserService).Assembly)
    .AddClasses(publicOnly: false)
    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
    .AsMatchingInterface()
    .WithScopedLifetime()
);

QuestPDF.Settings.License = LicenseType.Community;

// Configuramos los AutoMapper
builder.Services.AddAutoMapper(c =>
{
    c.AddMaps(typeof(TallerProfile).Assembly);
});

builder.Services.AddDbContext<PortalGalaxyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PortalGalaxy"));

    options.EnableSensitiveDataLogging();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("GalaxySecurity"));
});

// Configuramos ASP.NET Identity Core
builder.Services.AddIdentity<GalaxyIdentityUser, IdentityRole>(policies =>
    {
        policies.Password.RequireDigit = false;
        policies.Password.RequireLowercase = true;
        policies.Password.RequireUppercase = true;
        policies.Password.RequireNonAlphanumeric = false;
        policies.Password.RequiredLength = 8;

        policies.User.RequireUniqueEmail = true;

        // Politica de bloqueo de cuentas
        policies.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        policies.Lockout.MaxFailedAccessAttempts = 3;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configuramos el contexto de seguridad del API
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ??
                                           throw new InvalidOperationException("No se configuro el SecretKey"));

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

// Mapea el contenido de la configuracion en una clase fuertemente tipada
builder.Services.Configure<AppSettings>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "PortalGalaxy.ApiRest v1"));
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(corsConfiguration);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var group = app.MapGroup("api/talleres/reportes");
group.MapGet("tallerespormes/{anio:int}", async (int anio, ITallerService service) =>
{
    var response = await service.ReporteTalleresPorMes(anio);
    if (response.Success)
        return Results.Ok(response);

    return Results.BadRequest(response);
});
group.MapGet("talleresporinstructor/{anio:int}", async (int anio, ITallerService service) =>
{
    var response = await service.ReporteTalleresPorInstructor(anio);
    if (response.Success)
        return Results.Ok(response);

    return Results.BadRequest(response);
});


app.MapPost("api/seed/{tipo:int}", async (int tipo, ILogger<Program> logger) =>
{
    try
    {
        var scope = app.Services.CreateAsyncScope();
        if (tipo == 0)
            await DataSeeder.SeedInstructores(scope.ServiceProvider);
        else
            await DataSeeder.SeedTalleres(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al hacer el data Seeding {Message}", ex.Message);
    }

    return Results.Ok();
});

app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var dbContextGalaxy = scope.ServiceProvider.GetRequiredService<PortalGalaxyDbContext>();
    var dbContextSecurity = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Con estas dos lineas, me aseguro que se crea las migraciones de cada DbContext
    dbContextGalaxy.Database.Migrate();
    dbContextSecurity.Database.Migrate();

    await UserDataSeeder.Seed(scope.ServiceProvider);
}

app.Run();
