using System.ComponentModel.DataAnnotations;

namespace VentasAPI.Models.DTOs.Cargo;

public record CargoDto(string CodCargo, string NombreCargo);

public record CreateUpdateCargoDto(
    [Required][StringLength(3, MinimumLength = 1)] string CodCargo,
    [Required][StringLength(30, MinimumLength = 2)] string NombreCargo
);
