using System.ComponentModel.DataAnnotations;

namespace WebApplicationPy02.Models
{
    public class Vendedor
    {
        [Display(Name = "Id Vendedor")] public int idVendedor { get; set; }
        [Display(Name = "Nombres")] public string? nombre { get; set; }
        [Display(Name = "Direción")] public string? direccion { get; set; }
        [Display(Name = "Id Pais")] public string? idPais { get; set; }
        [Required, DataType(DataType.EmailAddress),Display(Name = "Email")] public string? email { get; set; }

    }
}
