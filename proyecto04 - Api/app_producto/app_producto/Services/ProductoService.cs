using app_producto.Dtos;
using app_producto.Models;
using app_producto.Repositories;

namespace app_producto.Services
{
    public class ProductoService
    {
        private readonly ProductoRepository _repository;

        public ProductoService(ProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Producto?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<Producto> CreateAsync(ProductoDto dto)
        {
            var producto = new Producto
            {
                Descripcion = dto.Descripcion,
                UMedida = dto.UMedida,
                Precio = dto.Precio,
                Stock = dto.Stock,
            };

            return await _repository.CreateAsync(producto);
        }

        public async Task<Producto> UpdatAsync(int id, ProductoDto dto)
        {
            var producto = new Producto
            {
                Descripcion = dto.Descripcion,
                UMedida = dto.UMedida,
                Precio = dto.Precio,
                Stock = dto.Stock,
            };

            return await _repository.UpdateAsync(id, producto);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _repository.DeleteAsync(id);

    }
}
