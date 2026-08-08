using System.ComponentModel.DataAnnotations;

namespace VentasAPI.Models.DTOs.Producto;

public record ProductoDto(
    string IdProducto,
    string Descripcion,
    decimal PrecioVenta,
    int? StockMinimo,
    int? StockActual,
    DateOnly? FechaVenc,
    string CodCate,
    string NombreCategoria
);

public record CreateProductoDto(
    [Required][StringLength(6, MinimumLength = 6)] string IdProducto,
    [Required][StringLength(45, MinimumLength = 2)] string Descripcion,
    [Required][Range(0.01, 999999.99)] decimal PrecioVenta,
    [Range(0, 999999)] int? StockMinimo,
    [Range(0, 999999)] int? StockActual,
    DateOnly? FechaVenc,
    [Required][StringLength(3, MinimumLength = 3)] string CodCate
);

public record UpdateProductoDto(
    [Required][StringLength(45, MinimumLength = 2)] string Descripcion,
    [Required][Range(0.01, 999999.99)] decimal PrecioVenta,
    [Range(0, 999999)] int? StockMinimo,
    [Range(0, 999999)] int? StockActual,
    DateOnly? FechaVenc,
    [Required][StringLength(3, MinimumLength = 3)] string CodCate
);
