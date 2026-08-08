using System.ComponentModel.DataAnnotations;

namespace VentasAPI.Models.DTOs.Distrito;

public record DistritoDto(string IdDistrito, string NombreDistrito);

public record CreateUpdateDistritoDto(
    [Required][StringLength(4, MinimumLength = 1)] string IdDistrito,
    [Required][StringLength(40, MinimumLength = 2)] string NombreDistrito
);
