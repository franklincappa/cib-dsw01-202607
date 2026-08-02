using app_producto.Models;
using Microsoft.EntityFrameworkCore;

namespace app_producto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Producto>( entity => 
            {
                entity.ToTable("producto");
                entity.HasKey(e=> e.IdProducto);
                entity.Property(e => e.IdProducto).HasColumnName("idProducto");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.UMedida).HasColumnName("uMedida");
                entity.Property(e => e.Precio).HasColumnName("precio");
                entity.Property(e => e.Stock).HasColumnName("stock");
            });
        }
    }
}
