using Microsoft.EntityFrameworkCore;
using VentasAPI.Data;
using VentasAPI.Mappings;
using VentasAPI.Repositories.Implementations;
using VentasAPI.Repositories.Interfaces;
using VentasAPI.Services.Implementations;
using VentasAPI.Services.Interfaces;

namespace VentasAPI.Configs;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── DbContext ──────────────────────────────────
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        // ── AutoMapper ────────────────────────────────
        services.AddAutoMapper(typeof(MappingProfile)); 
        //services.AddAutoMapper(typeof(DependencyInjectionConfig).Assembly);

        // ── Health Checks ─────────────────────────────
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("sqlserver", tags: ["db", "ready"]);

        // ── Repositories ──────────────────────────────
        services.AddScoped<IDistritoRepository, DistritoRepository>();
        services.AddScoped<ICargoRepository, CargoRepository>();
        services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<IBoletaRepository, BoletaRepository>();

        // ── Services ──────────────────────────────────
        services.AddScoped<IDistritoService, DistritoService>();
        services.AddScoped<ICargoService, CargoService>();
        services.AddScoped<IEmpleadoService, EmpleadoService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IBoletaService, BoletaService>();

        return services;
    }
}
