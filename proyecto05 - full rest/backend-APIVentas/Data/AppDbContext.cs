using Microsoft.EntityFrameworkCore;
using VentasAPI.Models.Entities;

namespace VentasAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Distrito> Distritos => Set<Distrito>();
    public DbSet<Cargo> Cargos => Set<Cargo>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Boleta> Boletas => Set<Boleta>();
    public DbSet<DetalleBoleta> DetallesBoleta => Set<DetalleBoleta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Aplica todas las configuraciones del assembly automáticamente
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
