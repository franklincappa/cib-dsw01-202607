using System.ComponentModel.DataAnnotations;

namespace VentasAPI.Models.DTOs.Empleado;

public record EmpleadoDto(
    string CodEmple,
    string Nombres,
    string Apellidos,
    string DniEmple,
    string Direccion,
    string EstadoCivil,
    string NivelEduca,
    string Telefono,
    string Email,
    decimal SueldoBasico,
    DateOnly FechaIngreso,
    string IdDistrito,
    string NombreDistrito,
    string CodCargo,
    string NombreCargo
);

public record CreateEmpleadoDto(
    [Required][StringLength(5, MinimumLength = 5)] string CodEmple,
    [Required][StringLength(25, MinimumLength = 2)] string Nombres,
    [Required][StringLength(25, MinimumLength = 2)] string Apellidos,
    [Required][StringLength(8, MinimumLength = 8)] string DniEmple,
    [Required][StringLength(60, MinimumLength = 5)] string Direccion,
    [Required][StringLength(1, MinimumLength = 1)] string EstadoCivil,
    [Required][StringLength(30, MinimumLength = 2)] string NivelEduca,
    [Required][StringLength(12, MinimumLength = 7)] string Telefono,
    [Required][EmailAddress][StringLength(35)] string Email,
    [Required][Range(0, 999999.99)] decimal SueldoBasico,
    [Required] DateOnly FechaIngreso,
    [Required][StringLength(4, MinimumLength = 4)] string IdDistrito,
    [Required][StringLength(3, MinimumLength = 3)] string CodCargo
);

public record UpdateEmpleadoDto(
    [Required][StringLength(25, MinimumLength = 2)] string Nombres,
    [Required][StringLength(25, MinimumLength = 2)] string Apellidos,
    [Required][StringLength(8, MinimumLength = 8)] string DniEmple,
    [Required][StringLength(60, MinimumLength = 5)] string Direccion,
    [Required][StringLength(1, MinimumLength = 1)] string EstadoCivil,
    [Required][StringLength(30, MinimumLength = 2)] string NivelEduca,
    [Required][StringLength(12, MinimumLength = 7)] string Telefono,
    [Required][EmailAddress][StringLength(35)] string Email,
    [Required][Range(0, 999999.99)] decimal SueldoBasico,
    [Required] DateOnly FechaIngreso,
    [Required][StringLength(4, MinimumLength = 4)] string IdDistrito,
    [Required][StringLength(3, MinimumLength = 3)] string CodCargo
);
