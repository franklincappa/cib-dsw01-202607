using System.ComponentModel.DataAnnotations;

namespace WebApplicationCrud.Models
{
    public class Producto
    {
        [Display(Name = "Id Producto")] public int idproducto { get; set; }
        [Display(Name = "Descripciòn")] public string descripcion { get; set; }
        [Display(Name = "Medida")] public string umedida { get; set; }
        [Display(Name = "Precio")] public decimal precio { get; set; }
        [Display(Name = "Stock")] public int stock { get; set; }

    }
}
