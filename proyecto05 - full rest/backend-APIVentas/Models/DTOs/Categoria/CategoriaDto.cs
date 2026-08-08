using System.ComponentModel.DataAnnotations;

namespace VentasAPI.Models.DTOs.Categoria;

public record CategoriaDto(string CodCate, string Nombre);

public record CreateUpdateCategoriaDto(
    [Required][StringLength(3, MinimumLength = 1)] string CodCate,
    [Required][StringLength(25, MinimumLength = 2)] string Nombre
);
