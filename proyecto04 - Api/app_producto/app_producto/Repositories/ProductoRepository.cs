using app_producto.Data;
using app_producto.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace app_producto.Repositories
{
    public class ProductoRepository
    {
        //instanciar Contexto
        private readonly AppDbContext _context;

        //Constructor
        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        //listar todos los registros
        public async Task<IEnumerable<Producto>> GetAllAsync()
            => await _context.productos.ToListAsync();

        //obtener 1 registro
        public async Task<Producto?> GetByIdAsync(int id)
            => await _context.productos.FindAsync(id);

        //crear un registro
        public async Task<Producto> CreateAsync(Producto producto)
        {
            _context.productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        //actualizar un registro
        public async Task<Producto> UpdateAsync(int id, Producto producto)
        {
            var existe = await _context.productos.FindAsync(id);
            if (existe == null) return null;
            
            existe.Descripcion = producto.Descripcion;
            existe.UMedida= producto.UMedida;
            existe.Precio=producto.Precio;  
            existe.Stock= producto.Stock;

            await _context.SaveChangesAsync();
            return existe;
        }

        //eliminar un registro
        public async Task<bool> DeleteAsync(int id)
        {
            var existe = await _context.productos.FindAsync(id);
            if (existe == null) return false;

            _context.productos.Remove(existe);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
