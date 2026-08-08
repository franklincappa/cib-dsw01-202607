using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VentasAPI.Models.DTOs.Categoria;
using VentasAPI.Models.DTOs.Cliente;
using VentasAPI.Models.DTOs.Common;
using VentasAPI.Models.DTOs.Producto;
using VentasAPI.Services.Interfaces;

namespace VentasAPI.Controllers.v1;

// ─────────────────────────────────────────────
// CLIENTES CONTROLLER
// ─────────────────────────────────────────────
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ClientesController(IClienteService service) : ControllerBase
{
    /// <summary>Obtiene todos los clientes con su distrito</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClienteDto>>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<IEnumerable<ClienteDto>>.SuccessResult(await service.GetAllAsync()));

    /// <summary>Obtiene un cliente por su ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), 404)]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<ClienteDto>.NotFoundResult($"Cliente '{id}' no encontrado"))
            : Ok(ApiResponse<ClienteDto>.SuccessResult(data));
    }

    /// <summary>Registra un nuevo cliente</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), 400)]
    public async Task<IActionResult> Create([FromBody] CreateClienteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ClienteDto>.FailResult("Datos inválidos",
                errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdCliente, version = "1" },
            ApiResponse<ClienteDto>.SuccessResult(created, "Cliente creado exitosamente", 201));
    }

    /// <summary>Actualiza los datos de un cliente</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto>), 404)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateClienteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ClienteDto>.FailResult("Datos inválidos"));

        var updated = await service.UpdateAsync(id, dto);
        return updated is null
            ? NotFound(ApiResponse<ClienteDto>.NotFoundResult($"Cliente '{id}' no encontrado"))
            : Ok(ApiResponse<ClienteDto>.SuccessResult(updated, "Cliente actualizado exitosamente"));
    }

    /// <summary>Elimina un cliente por su ID</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await service.DeleteAsync(id);
        return result
            ? Ok(ApiResponse<bool>.SuccessResult(true, "Cliente eliminado exitosamente"))
            : NotFound(ApiResponse<bool>.NotFoundResult($"Cliente '{id}' no encontrado"));
    }
}

// ─────────────────────────────────────────────
// CATEGORIAS CONTROLLER
// ─────────────────────────────────────────────
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class CategoriasController(ICategoriaService service) : ControllerBase
{
    /// <summary>Obtiene todas las categorías de productos</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoriaDto>>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<IEnumerable<CategoriaDto>>.SuccessResult(await service.GetAllAsync()));

    /// <summary>Obtiene una categoría por su código</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategoriaDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<CategoriaDto>), 404)]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<CategoriaDto>.NotFoundResult($"Categoría '{id}' no encontrada"))
            : Ok(ApiResponse<CategoriaDto>.SuccessResult(data));
    }

    /// <summary>Crea una nueva categoría</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CategoriaDto>), 201)]
    public async Task<IActionResult> Create([FromBody] CreateUpdateCategoriaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<CategoriaDto>.FailResult("Datos inválidos",
                errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.CodCate, version = "1" },
            ApiResponse<CategoriaDto>.SuccessResult(created, "Categoría creada exitosamente", 201));
    }

    /// <summary>Actualiza una categoría existente</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategoriaDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<CategoriaDto>), 404)]
    public async Task<IActionResult> Update(string id, [FromBody] CreateUpdateCategoriaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<CategoriaDto>.FailResult("Datos inválidos"));

        var updated = await service.UpdateAsync(id, dto);
        return updated is null
            ? NotFound(ApiResponse<CategoriaDto>.NotFoundResult($"Categoría '{id}' no encontrada"))
            : Ok(ApiResponse<CategoriaDto>.SuccessResult(updated, "Categoría actualizada exitosamente"));
    }

    /// <summary>Elimina una categoría por su código</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await service.DeleteAsync(id);
        return result
            ? Ok(ApiResponse<bool>.SuccessResult(true, "Categoría eliminada exitosamente"))
            : NotFound(ApiResponse<bool>.NotFoundResult($"Categoría '{id}' no encontrada"));
    }
}

// ─────────────────────────────────────────────
// PRODUCTOS CONTROLLER
// ─────────────────────────────────────────────
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ProductosController(IProductoService service) : ControllerBase
{
    /// <summary>Obtiene todos los productos con su categoría</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductoDto>>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<IEnumerable<ProductoDto>>.SuccessResult(await service.GetAllAsync()));

    /// <summary>Obtiene un producto por su ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductoDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<ProductoDto>), 404)]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<ProductoDto>.NotFoundResult($"Producto '{id}' no encontrado"))
            : Ok(ApiResponse<ProductoDto>.SuccessResult(data));
    }

    /// <summary>Obtiene productos filtrados por categoría</summary>
    [HttpGet("por-categoria/{codCate}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductoDto>>), 200)]
    public async Task<IActionResult> GetByCategoria(string codCate)
        => Ok(ApiResponse<IEnumerable<ProductoDto>>.SuccessResult(await service.GetByCategoriaAsync(codCate)));

    /// <summary>Obtiene productos con stock igual o menor al mínimo</summary>
    [HttpGet("stock-bajo")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductoDto>>), 200)]
    public async Task<IActionResult> GetStockBajo()
        => Ok(ApiResponse<IEnumerable<ProductoDto>>.SuccessResult(await service.GetStockBajoAsync(),
            "Productos con stock bajo"));

    /// <summary>Registra un nuevo producto</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProductoDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<ProductoDto>), 400)]
    public async Task<IActionResult> Create([FromBody] CreateProductoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ProductoDto>.FailResult("Datos inválidos",
                errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdProducto, version = "1" },
            ApiResponse<ProductoDto>.SuccessResult(created, "Producto creado exitosamente", 201));
    }

    /// <summary>Actualiza los datos de un producto</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductoDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<ProductoDto>), 404)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ProductoDto>.FailResult("Datos inválidos"));

        var updated = await service.UpdateAsync(id, dto);
        return updated is null
            ? NotFound(ApiResponse<ProductoDto>.NotFoundResult($"Producto '{id}' no encontrado"))
            : Ok(ApiResponse<ProductoDto>.SuccessResult(updated, "Producto actualizado exitosamente"));
    }

    /// <summary>Elimina un producto por su ID</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await service.DeleteAsync(id);
        return result
            ? Ok(ApiResponse<bool>.SuccessResult(true, "Producto eliminado exitosamente"))
            : NotFound(ApiResponse<bool>.NotFoundResult($"Producto '{id}' no encontrado"));
    }
}
