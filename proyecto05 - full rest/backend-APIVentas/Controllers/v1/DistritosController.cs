using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VentasAPI.Models.DTOs.Common;
using VentasAPI.Models.DTOs.Distrito;
using VentasAPI.Services.Interfaces;

namespace VentasAPI.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class DistritosController(IDistritoService service) : ControllerBase
{
    /// <summary>Obtiene la lista completa de distritos</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<DistritoDto>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var data = await service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<DistritoDto>>.SuccessResult(data));
    }

    /// <summary>Obtiene un distrito por su ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<DistritoDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<DistritoDto>), 404)]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<DistritoDto>.NotFoundResult($"Distrito '{id}' no encontrado"))
            : Ok(ApiResponse<DistritoDto>.SuccessResult(data));
    }

    /// <summary>Crea un nuevo distrito</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<DistritoDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<DistritoDto>), 400)]
    public async Task<IActionResult> Create([FromBody] CreateUpdateDistritoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<DistritoDto>.FailResult("Datos inválidos",
                errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdDistrito, version = "1" },
            ApiResponse<DistritoDto>.SuccessResult(created, "Distrito creado exitosamente", 201));
    }

    /// <summary>Actualiza un distrito existente</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<DistritoDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<DistritoDto>), 404)]
    public async Task<IActionResult> Update(string id, [FromBody] CreateUpdateDistritoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<DistritoDto>.FailResult("Datos inválidos"));

        var updated = await service.UpdateAsync(id, dto);
        return updated is null
            ? NotFound(ApiResponse<DistritoDto>.NotFoundResult($"Distrito '{id}' no encontrado"))
            : Ok(ApiResponse<DistritoDto>.SuccessResult(updated, "Distrito actualizado exitosamente"));
    }

    /// <summary>Elimina un distrito por su ID</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await service.DeleteAsync(id);
        return result
            ? Ok(ApiResponse<bool>.SuccessResult(true, "Distrito eliminado exitosamente"))
            : NotFound(ApiResponse<bool>.NotFoundResult($"Distrito '{id}' no encontrado"));
    }
}
