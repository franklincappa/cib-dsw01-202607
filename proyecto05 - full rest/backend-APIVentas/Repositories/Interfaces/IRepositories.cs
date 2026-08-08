using System.Linq.Expressions;
using VentasAPI.Models.Entities;

namespace VentasAPI.Repositories.Interfaces;

// ─────────────────────────────────────────────
// GENERIC REPOSITORY
// ─────────────────────────────────────────────
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(object id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(object id);
    Task<bool> ExistsAsync(object id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}

// ─────────────────────────────────────────────
// SPECIFIC REPOSITORIES
// ─────────────────────────────────────────────
public interface IDistritoRepository : IGenericRepository<Distrito> { }

public interface ICargoRepository : IGenericRepository<Cargo> { }

public interface IEmpleadoRepository : IGenericRepository<Empleado>
{
    Task<IEnumerable<Empleado>> GetAllWithDetailsAsync();
    Task<Empleado?> GetByIdWithDetailsAsync(string codEmple);
    Task<IEnumerable<Empleado>> GetByCargo(string codCargo);
    Task<IEnumerable<Empleado>> GetByDistrito(string idDistrito);
}

public interface IClienteRepository : IGenericRepository<Cliente>
{
    Task<IEnumerable<Cliente>> GetAllWithDetailsAsync();
    Task<Cliente?> GetByIdWithDetailsAsync(string idCliente);
}

public interface ICategoriaRepository : IGenericRepository<Categoria> { }

public interface IProductoRepository : IGenericRepository<Producto>
{
    Task<IEnumerable<Producto>> GetAllWithDetailsAsync();
    Task<Producto?> GetByIdWithDetailsAsync(string idProducto);
    Task<IEnumerable<Producto>> GetByCategoria(string codCate);
    Task<IEnumerable<Producto>> GetStockBajoAsync();
}

public interface IBoletaRepository : IGenericRepository<Boleta>
{
    Task<IEnumerable<Boleta>> GetAllWithDetailsAsync();
    Task<Boleta?> GetByIdWithDetailsAsync(string numBoleta);
    Task<IEnumerable<Boleta>> GetByClienteAsync(string idCliente);
    Task<IEnumerable<Boleta>> GetByEmpleadoAsync(string codEmple);
    Task<IEnumerable<Boleta>> GetByFechaAsync(DateOnly desde, DateOnly hasta);
}
