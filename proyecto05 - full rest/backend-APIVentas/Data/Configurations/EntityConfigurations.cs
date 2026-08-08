using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasAPI.Models.Entities;

namespace VentasAPI.Data.Configurations;

// ─────────────────────────────────────────────
// DISTRITO CONFIGURATION
// ─────────────────────────────────────────────
public class DistritoConfiguration : IEntityTypeConfiguration<Distrito>
{
    public void Configure(EntityTypeBuilder<Distrito> builder)
    {
        builder.ToTable("DISTRITOS");
        builder.HasKey(d => d.IdDistrito);

        builder.Property(d => d.IdDistrito)
               .HasColumnName("ID_DISTRITO")
               .HasColumnType("char(4)")
               .IsRequired();

        builder.Property(d => d.NombreDistrito)
               .HasColumnName("NOMBRE_DISTRITO")
               .HasMaxLength(40)
               .IsRequired();
    }
}

// ─────────────────────────────────────────────
// CARGO CONFIGURATION
// ─────────────────────────────────────────────
public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
{
    public void Configure(EntityTypeBuilder<Cargo> builder)
    {
        builder.ToTable("CARGOS");
        builder.HasKey(c => c.CodCargo);

        builder.Property(c => c.CodCargo)
               .HasColumnName("COD_CARGO")
               .HasColumnType("char(3)")
               .IsRequired();

        builder.Property(c => c.NombreCargo)
               .HasColumnName("NOMBRE_CARGO")
               .HasMaxLength(30)
               .IsRequired();
    }
}

// ─────────────────────────────────────────────
// EMPLEADO CONFIGURATION
// ─────────────────────────────────────────────
public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
{
    public void Configure(EntityTypeBuilder<Empleado> builder)
    {
        builder.ToTable("EMPLEADO");
        builder.HasKey(e => e.CodEmple);

        builder.Property(e => e.CodEmple).HasColumnName("COD_EMPLE").HasColumnType("char(5)").IsRequired();
        builder.Property(e => e.Nombres).HasColumnName("NOMBRES").HasMaxLength(25).IsRequired();
        builder.Property(e => e.Apellidos).HasColumnName("APELLIDOS").HasMaxLength(25).IsRequired();
        builder.Property(e => e.DniEmple).HasColumnName("DNI_EMPLE").HasColumnType("char(8)").IsRequired();
        builder.Property(e => e.Direccion).HasColumnName("DIRECCION").HasMaxLength(60).IsRequired();
        builder.Property(e => e.EstadoCivil).HasColumnName("ESTADO_CIVIL").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.NivelEduca).HasColumnName("NIVEL_EDUCA").HasMaxLength(30).IsRequired();
        builder.Property(e => e.Telefono).HasColumnName("TELEFONO").HasMaxLength(12).IsRequired();
        builder.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(35).IsRequired();
        builder.Property(e => e.SueldoBasico).HasColumnName("SUELDO_BASICO").HasColumnType("money").IsRequired();
        builder.Property(e => e.FechaIngreso).HasColumnName("FECHA_INGRESO").IsRequired();
        builder.Property(e => e.IdDistrito).HasColumnName("ID_DISTRITO").HasColumnType("char(4)").IsRequired();
        builder.Property(e => e.CodCargo).HasColumnName("COD_CARGO").HasColumnType("char(3)").IsRequired();

        builder.HasOne(e => e.Distrito)
               .WithMany(d => d.Empleados)
               .HasForeignKey(e => e.IdDistrito)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Cargo)
               .WithMany(c => c.Empleados)
               .HasForeignKey(e => e.CodCargo)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─────────────────────────────────────────────
// CLIENTE CONFIGURATION
// ─────────────────────────────────────────────
public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("CLIENTE");
        builder.HasKey(c => c.IdCliente);

        builder.Property(c => c.IdCliente).HasColumnName("ID_CLIENTE").HasColumnType("char(6)").IsRequired();
        builder.Property(c => c.Nombres).HasColumnName("NOMBRES").HasMaxLength(25).IsRequired();
        builder.Property(c => c.Apellidos).HasColumnName("APELLIDOS").HasMaxLength(25).IsRequired();
        builder.Property(c => c.Direccion).HasColumnName("DIRECCION").HasMaxLength(60).IsRequired(false);
        builder.Property(c => c.Fono).HasColumnName("FONO").HasColumnType("char(9)").IsRequired(false);
        builder.Property(c => c.IdDistrito).HasColumnName("ID_DISTRITO").HasColumnType("char(4)").IsRequired();
        builder.Property(c => c.Email).HasColumnName("EMAIL").HasMaxLength(35).IsRequired(false);

        builder.HasOne(c => c.Distrito)
               .WithMany(d => d.Clientes)
               .HasForeignKey(c => c.IdDistrito)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─────────────────────────────────────────────
// CATEGORIA CONFIGURATION
// ─────────────────────────────────────────────
public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("CATEGORIAS");
        builder.HasKey(c => c.CodCate);

        builder.Property(c => c.CodCate).HasColumnName("COD_CATE").HasColumnType("char(3)").IsRequired();
        builder.Property(c => c.Nombre).HasColumnName("NOMBRE").HasMaxLength(25).IsRequired();
    }
}

// ─────────────────────────────────────────────
// PRODUCTO CONFIGURATION
// ─────────────────────────────────────────────
public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("PRODUCTO");
        builder.HasKey(p => p.IdProducto);

        builder.Property(p => p.IdProducto).HasColumnName("ID_PRODUCTO").HasColumnType("char(6)").IsRequired();
        builder.Property(p => p.Descripcion).HasColumnName("DESCRIPCION").HasMaxLength(45).IsRequired();
        builder.Property(p => p.PrecioVenta).HasColumnName("PRECIO_VENTA").HasColumnType("money").IsRequired();
        builder.Property(p => p.StockMinimo).HasColumnName("STOCK_MINIMO").IsRequired(false);
        builder.Property(p => p.StockActual).HasColumnName("STOCK_ACTUAL").IsRequired(false);
        builder.Property(p => p.FechaVenc).HasColumnName("FECHA_VENC").IsRequired(false);
        builder.Property(p => p.CodCate).HasColumnName("COD_CATE").HasColumnType("char(3)").IsRequired();

        builder.HasOne(p => p.Categoria)
               .WithMany(c => c.Productos)
               .HasForeignKey(p => p.CodCate)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─────────────────────────────────────────────
// BOLETA CONFIGURATION
// ─────────────────────────────────────────────
public class BoletaConfiguration : IEntityTypeConfiguration<Boleta>
{
    public void Configure(EntityTypeBuilder<Boleta> builder)
    {
        builder.ToTable("BOLETA");
        builder.HasKey(b => b.NumBoleta);

        builder.Property(b => b.NumBoleta).HasColumnName("NUM_BOLETA").HasColumnType("char(8)").IsRequired();
        builder.Property(b => b.FechaEmi).HasColumnName("FECHA_EMI").IsRequired();
        builder.Property(b => b.IdCliente).HasColumnName("ID_CLIENTE").HasColumnType("char(6)").IsRequired();
        builder.Property(b => b.CodEmple).HasColumnName("COD_EMPLE").HasColumnType("char(5)").IsRequired();
        builder.Property(b => b.EstadoBoleta).HasColumnName("ESTADO_BOLETA").HasMaxLength(25).IsRequired();

        builder.HasOne(b => b.Cliente)
               .WithMany(c => c.Boletas)
               .HasForeignKey(b => b.IdCliente)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Empleado)
               .WithMany(e => e.Boletas)
               .HasForeignKey(b => b.CodEmple)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─────────────────────────────────────────────
// DETALLE BOLETA CONFIGURATION
// ─────────────────────────────────────────────
public class DetalleBoletaConfiguration : IEntityTypeConfiguration<DetalleBoleta>
{
    public void Configure(EntityTypeBuilder<DetalleBoleta> builder)
    {
        builder.ToTable("DETALLE_BOLETA");

        // Clave compuesta
        builder.HasKey(d => new { d.NumBoleta, d.IdProducto });

        builder.Property(d => d.NumBoleta).HasColumnName("NUM_BOLETA").HasColumnType("char(8)").IsRequired();
        builder.Property(d => d.IdProducto).HasColumnName("ID_PRODUCTO").HasColumnType("char(6)").IsRequired();
        builder.Property(d => d.Cantidad).HasColumnName("CANTIDAD").IsRequired();
        builder.Property(d => d.Importe).HasColumnName("IMPORTE").HasColumnType("money").IsRequired();

        builder.HasOne(d => d.Boleta)
               .WithMany(b => b.DetallesBoleta)
               .HasForeignKey(d => d.NumBoleta)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Producto)
               .WithMany(p => p.DetallesBoleta)
               .HasForeignKey(d => d.IdProducto)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
