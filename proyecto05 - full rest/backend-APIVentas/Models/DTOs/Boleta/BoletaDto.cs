using System.ComponentModel.DataAnnotations;

namespace VentasAPI.Models.DTOs.Boleta;

public record DetalleBoletaDto(
    string NumBoleta,
    string IdProducto,
    string DescripcionProducto,
    int Cantidad,
    decimal Importe
);

public record CreateDetalleBoletaDto(
    [Required][StringLength(6, MinimumLength = 6)] string IdProducto,
    [Required][Range(1, 9999)] int Cantidad,
    [Required][Range(0.01, 9999999.99)] decimal Importe
);

public record BoletaDto(
    string NumBoleta,
    DateOnly FechaEmi,
    string IdCliente,
    string NombreCliente,
    string CodEmple,
    string NombreEmpleado,
    string EstadoBoleta,
    IEnumerable<DetalleBoletaDto> Detalles,
    decimal Total
);

public record CreateBoletaDto(
    [Required][StringLength(8, MinimumLength = 8)] string NumBoleta,
    [Required] DateOnly FechaEmi,
    [Required][StringLength(6, MinimumLength = 6)] string IdCliente,
    [Required][StringLength(5, MinimumLength = 5)] string CodEmple,
    [Required][StringLength(25, MinimumLength = 2)] string EstadoBoleta,
    [Required][MinLength(1)] List<CreateDetalleBoletaDto> Detalles
);

public record UpdateBoletaDto(
    [Required][StringLength(25, MinimumLength = 2)] string EstadoBoleta
);
