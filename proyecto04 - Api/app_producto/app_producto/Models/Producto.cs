using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_producto.Models
{
    [Table("producto")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProducto { get; set; }
        [StringLength(70)]
        public string  Descripcion { get; set; }
        [StringLength(50)]
        public string? UMedida { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
    }
}
