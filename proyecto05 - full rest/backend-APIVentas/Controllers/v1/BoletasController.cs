using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VentasAPI.Models.DTOs.Boleta;
using VentasAPI.Models.DTOs.Common;
using VentasAPI.Services.Interfaces;

namespace VentasAPI.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class BoletasController(IBoletaService service) : ControllerBase
{
    /// <summary>Obtiene todas las boletas con sus detalles, cliente y empleado</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BoletaDto>>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<IEnumerable<BoletaDto>>.SuccessResult(await service.GetAllAsync()));

    /// <summary>Obtiene una boleta por su número incluyendo todos sus detalles</summary>
    /// 
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BoletaDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<BoletaDto>), 404)]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<BoletaDto>.NotFoundResult($"Boleta '{id}' no encontrada"))
            : Ok(ApiResponse<BoletaDto>.SuccessResult(data));
    }

    /// <summary>Obtiene todas las boletas de un cliente específico</summary>
    [HttpGet("por-cliente/{idCliente}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BoletaDto>>), 200)]
    public async Task<IActionResult> GetByCliente(string idCliente)
        => Ok(ApiResponse<IEnumerable<BoletaDto>>.SuccessResult(
            await service.GetByClienteAsync(idCliente),
            $"Boletas del cliente '{idCliente}'"));

    /// <summary>Obtiene todas las boletas atendidas por un empleado</summary>
    [HttpGet("por-empleado/{codEmple}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BoletaDto>>), 200)]
    public async Task<IActionResult> GetByEmpleado(string codEmple)
        => Ok(ApiResponse<IEnumerable<BoletaDto>>.SuccessResult(
            await service.GetByEmpleadoAsync(codEmple),
            $"Boletas del empleado '{codEmple}'"));

    /// <summary>Obtiene boletas en un rango de fechas (formato: yyyy-MM-dd)</summary>
    [HttpGet("por-fecha")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BoletaDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<BoletaDto>>), 400)]
    public async Task<IActionResult> GetByFecha(
        [FromQuery] DateOnly desde,
        [FromQuery] DateOnly hasta)
    {
        if (desde > hasta)
            return BadRequest(ApiResponse<IEnumerable<BoletaDto>>.FailResult(
                "La fecha 'desde' no puede ser mayor que 'hasta'"));

        var data = await service.GetByFechaAsync(desde, hasta);
        return Ok(ApiResponse<IEnumerable<BoletaDto>>.SuccessResult(data,
            $"Boletas del {desde} al {hasta}"));
    }

    /// <summary>Registra una nueva boleta con su detalle de productos</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BoletaDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<BoletaDto>), 400)]
    public async Task<IActionResult> Create([FromBody] CreateBoletaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<BoletaDto>.FailResult("Datos inválidos",
                errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.NumBoleta, version = "1" },
            ApiResponse<BoletaDto>.SuccessResult(created, "Boleta creada exitosamente", 201));
    }

    /// <summary>Actualiza el estado de una boleta (EMITIDO, ANULADO, PAGADO)</summary>
    [HttpPatch("{id}/estado")]
    [ProducesResponseType(typeof(ApiResponse<BoletaDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<BoletaDto>), 404)]
    public async Task<IActionResult> UpdateEstado(string id, [FromBody] UpdateBoletaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<BoletaDto>.FailResult("Datos inválidos"));

        var updated = await service.UpdateEstadoAsync(id, dto);
        return updated is null
            ? NotFound(ApiResponse<BoletaDto>.NotFoundResult($"Boleta '{id}' no encontrada"))
            : Ok(ApiResponse<BoletaDto>.SuccessResult(updated, "Estado de boleta actualizado"));
    }

    /// <summary>Elimina una boleta y sus detalles en cascada</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await service.DeleteAsync(id);
        return result
            ? Ok(ApiResponse<bool>.SuccessResult(true, "Boleta eliminada exitosamente"))
            : NotFound(ApiResponse<bool>.NotFoundResult($"Boleta '{id}' no encontrada"));
    }
}
