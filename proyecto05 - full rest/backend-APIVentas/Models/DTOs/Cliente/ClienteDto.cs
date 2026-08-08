using System.ComponentModel.DataAnnotations;

namespace VentasAPI.Models.DTOs.Cliente;

public record ClienteDto(
    string IdCliente,
    string Nombres,
    string Apellidos,
    string? Direccion,
    string? Fono,
    string IdDistrito,
    string NombreDistrito,
    string? Email
);

public record CreateClienteDto(
    [Required][StringLength(6, MinimumLength = 6)] string IdCliente,
    [Required][StringLength(25, MinimumLength = 2)] string Nombres,
    [Required][StringLength(25, MinimumLength = 2)] string Apellidos,
    [StringLength(60)] string? Direccion,
    [StringLength(9)] string? Fono,
    [Required][StringLength(4, MinimumLength = 4)] string IdDistrito,
    [EmailAddress][StringLength(35)] string? Email
);

public record UpdateClienteDto(
    [Required][StringLength(25, MinimumLength = 2)] string Nombres,
    [Required][StringLength(25, MinimumLength = 2)] string Apellidos,
    [StringLength(60)] string? Direccion,
    [StringLength(9)] string? Fono,
    [Required][StringLength(4, MinimumLength = 4)] string IdDistrito,
    [EmailAddress][StringLength(35)] string? Email
);
