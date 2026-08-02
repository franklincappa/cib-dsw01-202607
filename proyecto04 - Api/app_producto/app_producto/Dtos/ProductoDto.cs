using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_producto.Dtos
{
    public class ProductoDto
    {
        public string Descripcion { get; set; }
        public string? UMedida { get; set; }
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
    }
}
