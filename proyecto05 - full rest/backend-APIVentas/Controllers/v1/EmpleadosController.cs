using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VentasAPI.Models.DTOs.Common;
using VentasAPI.Models.DTOs.Empleado;
using VentasAPI.Services.Interfaces;

namespace VentasAPI.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class EmpleadosController(IEmpleadoService service) : ControllerBase
{
    /// <summary>Obtiene todos los empleados con su cargo y distrito</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoDto>>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<IEnumerable<EmpleadoDto>>.SuccessResult(await service.GetAllAsync()));

    /// <summary>Obtiene un empleado por su código</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<EmpleadoDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<EmpleadoDto>), 404)]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<EmpleadoDto>.NotFoundResult($"Empleado '{id}' no encontrado"))
            : Ok(ApiResponse<EmpleadoDto>.SuccessResult(data));
    }

    /// <summary>Obtiene empleados filtrados por cargo</summary>
    [HttpGet("por-cargo/{codCargo}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoDto>>), 200)]
    public async Task<IActionResult> GetByCargo(string codCargo)
        => Ok(ApiResponse<IEnumerable<EmpleadoDto>>.SuccessResult(await service.GetByCargoAsync(codCargo)));

    /// <summary>Obtiene empleados filtrados por distrito</summary>
    [HttpGet("por-distrito/{idDistrito}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoDto>>), 200)]
    public async Task<IActionResult> GetByDistrito(string idDistrito)
        => Ok(ApiResponse<IEnumerable<EmpleadoDto>>.SuccessResult(await service.GetByDistritoAsync(idDistrito)));

    /// <summary>Registra un nuevo empleado</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EmpleadoDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<EmpleadoDto>), 400)]
    public async Task<IActionResult> Create([FromBody] CreateEmpleadoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<EmpleadoDto>.FailResult("Datos inválidos",
                errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.CodEmple, version = "1" },
            ApiResponse<EmpleadoDto>.SuccessResult(created, "Empleado creado exitosamente", 201));
    }

    /// <summary>Actualiza los datos de un empleado</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<EmpleadoDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<EmpleadoDto>), 404)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateEmpleadoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<EmpleadoDto>.FailResult("Datos inválidos"));

        var updated = await service.UpdateAsync(id, dto);
        return updated is null
            ? NotFound(ApiResponse<EmpleadoDto>.NotFoundResult($"Empleado '{id}' no encontrado"))
            : Ok(ApiResponse<EmpleadoDto>.SuccessResult(updated, "Empleado actualizado exitosamente"));
    }

    /// <summary>Elimina un empleado por su código</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await service.DeleteAsync(id);
        return result
            ? Ok(ApiResponse<bool>.SuccessResult(true, "Empleado eliminado exitosamente"))
            : NotFound(ApiResponse<bool>.NotFoundResult($"Empleado '{id}' no encontrado"));
    }
}
