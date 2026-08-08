using AutoMapper;
using VentasAPI.Models.DTOs.Boleta;
using VentasAPI.Models.DTOs.Cargo;
using VentasAPI.Models.DTOs.Categoria;
using VentasAPI.Models.DTOs.Cliente;
using VentasAPI.Models.DTOs.Distrito;
using VentasAPI.Models.DTOs.Empleado;
using VentasAPI.Models.DTOs.Producto;
using VentasAPI.Models.Entities;
using VentasAPI.Repositories.Interfaces;
using VentasAPI.Services.Interfaces;

namespace VentasAPI.Services.Implementations;

// ─────────────────────────────────────────────
// DISTRITO SERVICE
// ─────────────────────────────────────────────
public class DistritoService(IDistritoRepository repo, IMapper mapper) : IDistritoService
{
    public async Task<IEnumerable<DistritoDto>> GetAllAsync()
        => mapper.Map<IEnumerable<DistritoDto>>(await repo.GetAllAsync());

    public async Task<DistritoDto?> GetByIdAsync(string id)
    {
        var entity = await repo.GetByIdAsync(id);
        return entity is null ? null : mapper.Map<DistritoDto>(entity);
    }

    public async Task<DistritoDto> CreateAsync(CreateUpdateDistritoDto dto)
    {
        var entity = mapper.Map<Distrito>(dto);
        return mapper.Map<DistritoDto>(await repo.CreateAsync(entity));
    }

    public async Task<DistritoDto?> UpdateAsync(string id, CreateUpdateDistritoDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return null;
        mapper.Map(dto, entity);
        return mapper.Map<DistritoDto>(await repo.UpdateAsync(entity));
    }

    public async Task<bool> DeleteAsync(string id) => await repo.DeleteAsync(id);
}

// ─────────────────────────────────────────────
// CARGO SERVICE
// ─────────────────────────────────────────────
public class CargoService(ICargoRepository repo, IMapper mapper) : ICargoService
{
    public async Task<IEnumerable<CargoDto>> GetAllAsync()
        => mapper.Map<IEnumerable<CargoDto>>(await repo.GetAllAsync());

    public async Task<CargoDto?> GetByIdAsync(string id)
    {
        var entity = await repo.GetByIdAsync(id);
        return entity is null ? null : mapper.Map<CargoDto>(entity);
    }

    public async Task<CargoDto> CreateAsync(CreateUpdateCargoDto dto)
    {
        var entity = mapper.Map<Cargo>(dto);
        return mapper.Map<CargoDto>(await repo.CreateAsync(entity));
    }

    public async Task<CargoDto?> UpdateAsync(string id, CreateUpdateCargoDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return null;
        mapper.Map(dto, entity);
        return mapper.Map<CargoDto>(await repo.UpdateAsync(entity));
    }

    public async Task<bool> DeleteAsync(string id) => await repo.DeleteAsync(id);
}

// ─────────────────────────────────────────────
// EMPLEADO SERVICE
// ─────────────────────────────────────────────
public class EmpleadoService(IEmpleadoRepository repo, IMapper mapper) : IEmpleadoService
{
    public async Task<IEnumerable<EmpleadoDto>> GetAllAsync()
        => mapper.Map<IEnumerable<EmpleadoDto>>(await repo.GetAllWithDetailsAsync());

    public async Task<EmpleadoDto?> GetByIdAsync(string id)
    {
        var entity = await repo.GetByIdWithDetailsAsync(id);
        return entity is null ? null : mapper.Map<EmpleadoDto>(entity);
    }

    public async Task<EmpleadoDto> CreateAsync(CreateEmpleadoDto dto)
    {
        var entity = mapper.Map<Empleado>(dto);
        await repo.CreateAsync(entity);
        var created = await repo.GetByIdWithDetailsAsync(entity.CodEmple);
        return mapper.Map<EmpleadoDto>(created!);
    }

    public async Task<EmpleadoDto?> UpdateAsync(string id, UpdateEmpleadoDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return null;
        mapper.Map(dto, entity);
        await repo.UpdateAsync(entity);
        var updated = await repo.GetByIdWithDetailsAsync(id);
        return mapper.Map<EmpleadoDto>(updated!);
    }

    public async Task<bool> DeleteAsync(string id) => await repo.DeleteAsync(id);

    public async Task<IEnumerable<EmpleadoDto>> GetByCargoAsync(string codCargo)
        => mapper.Map<IEnumerable<EmpleadoDto>>(await repo.GetByCargo(codCargo));

    public async Task<IEnumerable<EmpleadoDto>> GetByDistritoAsync(string idDistrito)
        => mapper.Map<IEnumerable<EmpleadoDto>>(await repo.GetByDistrito(idDistrito));
}

// ─────────────────────────────────────────────
// CLIENTE SERVICE
// ─────────────────────────────────────────────
public class ClienteService(IClienteRepository repo, IMapper mapper) : IClienteService
{
    public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        => mapper.Map<IEnumerable<ClienteDto>>(await repo.GetAllWithDetailsAsync());

    public async Task<ClienteDto?> GetByIdAsync(string id)
    {
        var entity = await repo.GetByIdWithDetailsAsync(id);
        return entity is null ? null : mapper.Map<ClienteDto>(entity);
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteDto dto)
    {
        var entity = mapper.Map<Cliente>(dto);
        await repo.CreateAsync(entity);
        var created = await repo.GetByIdWithDetailsAsync(entity.IdCliente);
        return mapper.Map<ClienteDto>(created!);
    }

    public async Task<ClienteDto?> UpdateAsync(string id, UpdateClienteDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return null;
        mapper.Map(dto, entity);
        await repo.UpdateAsync(entity);
        var updated = await repo.GetByIdWithDetailsAsync(id);
        return mapper.Map<ClienteDto>(updated!);
    }

    public async Task<bool> DeleteAsync(string id) => await repo.DeleteAsync(id);
}

// ─────────────────────────────────────────────
// CATEGORIA SERVICE
// ─────────────────────────────────────────────
public class CategoriaService(ICategoriaRepository repo, IMapper mapper) : ICategoriaService
{
    public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        => mapper.Map<IEnumerable<CategoriaDto>>(await repo.GetAllAsync());

    public async Task<CategoriaDto?> GetByIdAsync(string id)
    {
        var entity = await repo.GetByIdAsync(id);
        return entity is null ? null : mapper.Map<CategoriaDto>(entity);
    }

    public async Task<CategoriaDto> CreateAsync(CreateUpdateCategoriaDto dto)
    {
        var entity = mapper.Map<Categoria>(dto);
        return mapper.Map<CategoriaDto>(await repo.CreateAsync(entity));
    }

    public async Task<CategoriaDto?> UpdateAsync(string id, CreateUpdateCategoriaDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return null;
        mapper.Map(dto, entity);
        return mapper.Map<CategoriaDto>(await repo.UpdateAsync(entity));
    }

    public async Task<bool> DeleteAsync(string id) => await repo.DeleteAsync(id);
}

// ─────────────────────────────────────────────
// PRODUCTO SERVICE
// ─────────────────────────────────────────────
public class ProductoService(IProductoRepository repo, IMapper mapper) : IProductoService
{
    public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        => mapper.Map<IEnumerable<ProductoDto>>(await repo.GetAllWithDetailsAsync());

    public async Task<ProductoDto?> GetByIdAsync(string id)
    {
        var entity = await repo.GetByIdWithDetailsAsync(id);
        return entity is null ? null : mapper.Map<ProductoDto>(entity);
    }

    public async Task<ProductoDto> CreateAsync(CreateProductoDto dto)
    {
        var entity = mapper.Map<Producto>(dto);
        await repo.CreateAsync(entity);
        var created = await repo.GetByIdWithDetailsAsync(entity.IdProducto);
        return mapper.Map<ProductoDto>(created!);
    }

    public async Task<ProductoDto?> UpdateAsync(string id, UpdateProductoDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return null;
        mapper.Map(dto, entity);
        await repo.UpdateAsync(entity);
        var updated = await repo.GetByIdWithDetailsAsync(id);
        return mapper.Map<ProductoDto>(updated!);
    }

    public async Task<bool> DeleteAsync(string id) => await repo.DeleteAsync(id);

    public async Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(string codCate)
        => mapper.Map<IEnumerable<ProductoDto>>(await repo.GetByCategoria(codCate));

    public async Task<IEnumerable<ProductoDto>> GetStockBajoAsync()
        => mapper.Map<IEnumerable<ProductoDto>>(await repo.GetStockBajoAsync());
}

// ─────────────────────────────────────────────
// BOLETA SERVICE
// ─────────────────────────────────────────────
public class BoletaService(IBoletaRepository repo, IMapper mapper) : IBoletaService
{
    public async Task<IEnumerable<BoletaDto>> GetAllAsync()
        => mapper.Map<IEnumerable<BoletaDto>>(await repo.GetAllWithDetailsAsync());

    public async Task<BoletaDto?> GetByIdAsync(string id)
    {
        var entity = await repo.GetByIdWithDetailsAsync(id);
        return entity is null ? null : mapper.Map<BoletaDto>(entity);
    }

    public async Task<BoletaDto> CreateAsync(CreateBoletaDto dto)
    {
        var entity = mapper.Map<Boleta>(dto);
        await repo.CreateAsync(entity);
        var created = await repo.GetByIdWithDetailsAsync(entity.NumBoleta);
        return mapper.Map<BoletaDto>(created!);
    }

    public async Task<BoletaDto?> UpdateEstadoAsync(string id, UpdateBoletaDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return null;
        entity.EstadoBoleta = dto.EstadoBoleta;
        await repo.UpdateAsync(entity);
        var updated = await repo.GetByIdWithDetailsAsync(id);
        return mapper.Map<BoletaDto>(updated!);
    }

    public async Task<bool> DeleteAsync(string id) => await repo.DeleteAsync(id);

    public async Task<IEnumerable<BoletaDto>> GetByClienteAsync(string idCliente)
        => mapper.Map<IEnumerable<BoletaDto>>(await repo.GetByClienteAsync(idCliente));

    public async Task<IEnumerable<BoletaDto>> GetByEmpleadoAsync(string codEmple)
        => mapper.Map<IEnumerable<BoletaDto>>(await repo.GetByEmpleadoAsync(codEmple));

    public async Task<IEnumerable<BoletaDto>> GetByFechaAsync(DateOnly desde, DateOnly hasta)
        => mapper.Map<IEnumerable<BoletaDto>>(await repo.GetByFechaAsync(desde, hasta));
}
