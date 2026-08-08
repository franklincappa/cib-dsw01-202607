using Microsoft.EntityFrameworkCore;
using VentasAPI.Data;
using VentasAPI.Models.Entities;
using VentasAPI.Repositories.Interfaces;

namespace VentasAPI.Repositories.Implementations;

// ─────────────────────────────────────────────
// DISTRITO
// ─────────────────────────────────────────────
public class DistritoRepository(AppDbContext context)
    : GenericRepository<Distrito>(context), IDistritoRepository { }

// ─────────────────────────────────────────────
// CARGO
// ─────────────────────────────────────────────
public class CargoRepository(AppDbContext context)
    : GenericRepository<Cargo>(context), ICargoRepository { }

// ─────────────────────────────────────────────
// EMPLEADO
// ─────────────────────────────────────────────
public class EmpleadoRepository(AppDbContext context)
    : GenericRepository<Empleado>(context), IEmpleadoRepository
{
    public async Task<IEnumerable<Empleado>> GetAllWithDetailsAsync()
        => await _context.Empleados
            .Include(e => e.Distrito)
            .Include(e => e.Cargo)
            .ToListAsync();

    public async Task<Empleado?> GetByIdWithDetailsAsync(string codEmple)
        => await _context.Empleados
            .Include(e => e.Distrito)
            .Include(e => e.Cargo)
            .FirstOrDefaultAsync(e => e.CodEmple == codEmple);

    public async Task<IEnumerable<Empleado>> GetByCargo(string codCargo)
        => await _context.Empleados
            .Include(e => e.Cargo)
            .Where(e => e.CodCargo == codCargo)
            .ToListAsync();

    public async Task<IEnumerable<Empleado>> GetByDistrito(string idDistrito)
        => await _context.Empleados
            .Include(e => e.Distrito)
            .Where(e => e.IdDistrito == idDistrito)
            .ToListAsync();
}

// ─────────────────────────────────────────────
// CLIENTE
// ─────────────────────────────────────────────
public class ClienteRepository(AppDbContext context)
    : GenericRepository<Cliente>(context), IClienteRepository
{
    public async Task<IEnumerable<Cliente>> GetAllWithDetailsAsync()
        => await _context.Clientes
            .Include(c => c.Distrito)
            .ToListAsync();

    public async Task<Cliente?> GetByIdWithDetailsAsync(string idCliente)
        => await _context.Clientes
            .Include(c => c.Distrito)
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente);
}

// ─────────────────────────────────────────────
// CATEGORIA
// ─────────────────────────────────────────────
public class CategoriaRepository(AppDbContext context)
    : GenericRepository<Categoria>(context), ICategoriaRepository { }

// ─────────────────────────────────────────────
// PRODUCTO
// ─────────────────────────────────────────────
public class ProductoRepository(AppDbContext context)
    : GenericRepository<Producto>(context), IProductoRepository
{
    public async Task<IEnumerable<Producto>> GetAllWithDetailsAsync()
        => await _context.Productos
            .Include(p => p.Categoria)
            .ToListAsync();

    public async Task<Producto?> GetByIdWithDetailsAsync(string idProducto)
        => await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

    public async Task<IEnumerable<Producto>> GetByCategoria(string codCate)
        => await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.CodCate == codCate)
            .ToListAsync();

    public async Task<IEnumerable<Producto>> GetStockBajoAsync()
        => await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.StockActual.HasValue && p.StockMinimo.HasValue
                        && p.StockActual <= p.StockMinimo)
            .ToListAsync();
}

// ─────────────────────────────────────────────
// BOLETA
// ─────────────────────────────────────────────
public class BoletaRepository(AppDbContext context)
    : GenericRepository<Boleta>(context), IBoletaRepository
{
    public async Task<IEnumerable<Boleta>> GetAllWithDetailsAsync()
        => await _context.Boletas
            .Include(b => b.Cliente)
            .Include(b => b.Empleado)
            .Include(b => b.DetallesBoleta).ThenInclude(d => d.Producto)
            .ToListAsync();

    public async Task<Boleta?> GetByIdWithDetailsAsync(string numBoleta)
        => await _context.Boletas
            .Include(b => b.Cliente)
            .Include(b => b.Empleado)
            .Include(b => b.DetallesBoleta).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(b => b.NumBoleta == numBoleta);

    public async Task<IEnumerable<Boleta>> GetByClienteAsync(string idCliente)
        => await _context.Boletas
            .Include(b => b.Cliente)
            .Include(b => b.DetallesBoleta).ThenInclude(d => d.Producto)
            .Where(b => b.IdCliente == idCliente)
            .ToListAsync();

    public async Task<IEnumerable<Boleta>> GetByEmpleadoAsync(string codEmple)
        => await _context.Boletas
            .Include(b => b.Empleado)
            .Include(b => b.DetallesBoleta).ThenInclude(d => d.Producto)
            .Where(b => b.CodEmple == codEmple)
            .ToListAsync();

    public async Task<IEnumerable<Boleta>> GetByFechaAsync(DateOnly desde, DateOnly hasta)
        => await _context.Boletas
            .Include(b => b.Cliente)
            .Include(b => b.Empleado)
            .Include(b => b.DetallesBoleta).ThenInclude(d => d.Producto)
            .Where(b => b.FechaEmi >= desde && b.FechaEmi <= hasta)
            .ToListAsync();
}
