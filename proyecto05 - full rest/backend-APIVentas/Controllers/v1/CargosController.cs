using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VentasAPI.Models.DTOs.Cargo;
using VentasAPI.Models.DTOs.Common;
using VentasAPI.Services.Interfaces;

namespace VentasAPI.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class CargosController(ICargoService service) : ControllerBase
{
    /// <summary>Obtiene todos los cargos registrados</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CargoDto>>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<IEnumerable<CargoDto>>.SuccessResult(await service.GetAllAsync()));

    /// <summary>Obtiene un cargo por su código</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CargoDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<CargoDto>), 404)]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<CargoDto>.NotFoundResult($"Cargo '{id}' no encontrado"))
            : Ok(ApiResponse<CargoDto>.SuccessResult(data));
    }

    /// <summary>Crea un nuevo cargo</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CargoDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<CargoDto>), 400)]
    public async Task<IActionResult> Create([FromBody] CreateUpdateCargoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<CargoDto>.FailResult("Datos inválidos",
                errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.CodCargo, version = "1" },
            ApiResponse<CargoDto>.SuccessResult(created, "Cargo creado exitosamente", 201));
    }

    /// <summary>Actualiza un cargo existente</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CargoDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<CargoDto>), 404)]
    public async Task<IActionResult> Update(string id, [FromBody] CreateUpdateCargoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<CargoDto>.FailResult("Datos inválidos"));

        var updated = await service.UpdateAsync(id, dto);
        return updated is null
            ? NotFound(ApiResponse<CargoDto>.NotFoundResult($"Cargo '{id}' no encontrado"))
            : Ok(ApiResponse<CargoDto>.SuccessResult(updated, "Cargo actualizado exitosamente"));
    }

    /// <summary>Elimina un cargo por su código</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await service.DeleteAsync(id);
        return result
            ? Ok(ApiResponse<bool>.SuccessResult(true, "Cargo eliminado exitosamente"))
            : NotFound(ApiResponse<bool>.NotFoundResult($"Cargo '{id}' no encontrado"));
    }
}
