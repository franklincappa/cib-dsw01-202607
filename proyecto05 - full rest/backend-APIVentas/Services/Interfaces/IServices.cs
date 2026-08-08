using VentasAPI.Models.DTOs.Boleta;
using VentasAPI.Models.DTOs.Cargo;
using VentasAPI.Models.DTOs.Categoria;
using VentasAPI.Models.DTOs.Cliente;
using VentasAPI.Models.DTOs.Distrito;
using VentasAPI.Models.DTOs.Empleado;
using VentasAPI.Models.DTOs.Producto;

namespace VentasAPI.Services.Interfaces;

public interface IDistritoService
{
    Task<IEnumerable<DistritoDto>> GetAllAsync();
    Task<DistritoDto?> GetByIdAsync(string id);
    Task<DistritoDto> CreateAsync(CreateUpdateDistritoDto dto);
    Task<DistritoDto?> UpdateAsync(string id, CreateUpdateDistritoDto dto);
    Task<bool> DeleteAsync(string id);
}

public interface ICargoService
{
    Task<IEnumerable<CargoDto>> GetAllAsync();
    Task<CargoDto?> GetByIdAsync(string id);
    Task<CargoDto> CreateAsync(CreateUpdateCargoDto dto);
    Task<CargoDto?> UpdateAsync(string id, CreateUpdateCargoDto dto);
    Task<bool> DeleteAsync(string id);
}

public interface IEmpleadoService
{
    Task<IEnumerable<EmpleadoDto>> GetAllAsync();
    Task<EmpleadoDto?> GetByIdAsync(string id);
    Task<EmpleadoDto> CreateAsync(CreateEmpleadoDto dto);
    Task<EmpleadoDto?> UpdateAsync(string id, UpdateEmpleadoDto dto);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<EmpleadoDto>> GetByCargoAsync(string codCargo);
    Task<IEnumerable<EmpleadoDto>> GetByDistritoAsync(string idDistrito);
}

public interface IClienteService
{
    Task<IEnumerable<ClienteDto>> GetAllAsync();
    Task<ClienteDto?> GetByIdAsync(string id);
    Task<ClienteDto> CreateAsync(CreateClienteDto dto);
    Task<ClienteDto?> UpdateAsync(string id, UpdateClienteDto dto);
    Task<bool> DeleteAsync(string id);
}

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> GetAllAsync();
    Task<CategoriaDto?> GetByIdAsync(string id);
    Task<CategoriaDto> CreateAsync(CreateUpdateCategoriaDto dto);
    Task<CategoriaDto?> UpdateAsync(string id, CreateUpdateCategoriaDto dto);
    Task<bool> DeleteAsync(string id);
}

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> GetAllAsync();
    Task<ProductoDto?> GetByIdAsync(string id);
    Task<ProductoDto> CreateAsync(CreateProductoDto dto);
    Task<ProductoDto?> UpdateAsync(string id, UpdateProductoDto dto);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(string codCate);
    Task<IEnumerable<ProductoDto>> GetStockBajoAsync();
}

public interface IBoletaService
{
    Task<IEnumerable<BoletaDto>> GetAllAsync();
    Task<BoletaDto?> GetByIdAsync(string id);
    Task<BoletaDto> CreateAsync(CreateBoletaDto dto);
    Task<BoletaDto?> UpdateEstadoAsync(string id, UpdateBoletaDto dto);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<BoletaDto>> GetByClienteAsync(string idCliente);
    Task<IEnumerable<BoletaDto>> GetByEmpleadoAsync(string codEmple);
    Task<IEnumerable<BoletaDto>> GetByFechaAsync(DateOnly desde, DateOnly hasta);
}
