using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentasAPI.Models.Entities;

// ─────────────────────────────────────────────
// DISTRITOS
// ─────────────────────────────────────────────
public class Distrito
{
    [Key]
    [Column(TypeName = "char(4)")]
    [Required]
    [StringLength(4)]
    public string IdDistrito { get; set; } = null!;

    [Required]
    [StringLength(40)]
    public string NombreDistrito { get; set; } = null!;

    // Nav
    public ICollection<Empleado> Empleados { get; set; } = [];
    public ICollection<Cliente> Clientes { get; set; } = [];
}

// ─────────────────────────────────────────────
// CARGOS
// ─────────────────────────────────────────────
public class Cargo
{
    [Key]
    [Column(TypeName = "char(3)")]
    [Required]
    [StringLength(3)]
    public string CodCargo { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string NombreCargo { get; set; } = null!;

    // Nav
    public ICollection<Empleado> Empleados { get; set; } = [];
}

// ─────────────────────────────────────────────
// EMPLEADO
// ─────────────────────────────────────────────
public class Empleado
{
    [Key]
    [Column(TypeName = "char(5)")]
    [Required]
    [StringLength(5)]
    public string CodEmple { get; set; } = null!;

    [Required]
    [StringLength(25)]
    public string Nombres { get; set; } = null!;

    [Required]
    [StringLength(25)]
    public string Apellidos { get; set; } = null!;

    [Required]
    [Column(TypeName = "char(8)")]
    [StringLength(8)]
    public string DniEmple { get; set; } = null!;

    [Required]
    [StringLength(60)]
    public string Direccion { get; set; } = null!;

    [Required]
    [Column(TypeName = "char(1)")]
    [StringLength(1)]
    public string EstadoCivil { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string NivelEduca { get; set; } = null!;

    [Required]
    [StringLength(12)]
    public string Telefono { get; set; } = null!;

    [Required]
    [StringLength(35)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Column(TypeName = "money")]
    public decimal SueldoBasico { get; set; }

    [Required]
    public DateOnly FechaIngreso { get; set; }

    [Required]
    [Column(TypeName = "char(4)")]
    [StringLength(4)]
    public string IdDistrito { get; set; } = null!;

    [Required]
    [Column(TypeName = "char(3)")]
    [StringLength(3)]
    public string CodCargo { get; set; } = null!;

    // Nav
    public Distrito Distrito { get; set; } = null!;
    public Cargo Cargo { get; set; } = null!;
    public ICollection<Boleta> Boletas { get; set; } = [];
}

// ─────────────────────────────────────────────
// CLIENTE
// ─────────────────────────────────────────────
public class Cliente
{
    [Key]
    [Column(TypeName = "char(6)")]
    [Required]
    [StringLength(6)]
    public string IdCliente { get; set; } = null!;

    [Required]
    [StringLength(25)]
    public string Nombres { get; set; } = null!;

    [Required]
    [StringLength(25)]
    public string Apellidos { get; set; } = null!;

    [StringLength(60)]
    public string? Direccion { get; set; }

    [Column(TypeName = "char(9)")]
    [StringLength(9)]
    public string? Fono { get; set; }

    [Required]
    [Column(TypeName = "char(4)")]
    [StringLength(4)]
    public string IdDistrito { get; set; } = null!;

    [StringLength(35)]
    [EmailAddress]
    public string? Email { get; set; }

    // Nav
    public Distrito Distrito { get; set; } = null!;
    public ICollection<Boleta> Boletas { get; set; } = [];
}

// ─────────────────────────────────────────────
// CATEGORIAS
// ─────────────────────────────────────────────
public class Categoria
{
    [Key]
    [Column(TypeName = "char(3)")]
    [Required]
    [StringLength(3)]
    public string CodCate { get; set; } = null!;

    [Required]
    [StringLength(25)]
    public string Nombre { get; set; } = null!;

    // Nav
    public ICollection<Producto> Productos { get; set; } = [];
}

// ─────────────────────────────────────────────
// PRODUCTO
// ─────────────────────────────────────────────
public class Producto
{
    [Key]
    [Column(TypeName = "char(6)")]
    [Required]
    [StringLength(6)]
    public string IdProducto { get; set; } = null!;

    [Required]
    [StringLength(45)]
    public string Descripcion { get; set; } = null!;

    [Required]
    [Column(TypeName = "money")]
    public decimal PrecioVenta { get; set; }

    public int? StockMinimo { get; set; }
    public int? StockActual { get; set; }
    public DateOnly? FechaVenc { get; set; }

    [Required]
    [Column(TypeName = "char(3)")]
    [StringLength(3)]
    public string CodCate { get; set; } = null!;

    // Nav
    public Categoria Categoria { get; set; } = null!;
    public ICollection<DetalleBoleta> DetallesBoleta { get; set; } = [];
}

// ─────────────────────────────────────────────
// BOLETA
// ─────────────────────────────────────────────
public class Boleta
{
    [Key]
    [Column(TypeName = "char(8)")]
    [Required]
    [StringLength(8)]
    public string NumBoleta { get; set; } = null!;

    [Required]
    public DateOnly FechaEmi { get; set; }

    [Required]
    [Column(TypeName = "char(6)")]
    [StringLength(6)]
    public string IdCliente { get; set; } = null!;

    [Required]
    [Column(TypeName = "char(5)")]
    [StringLength(5)]
    public string CodEmple { get; set; } = null!;

    [Required]
    [StringLength(25)]
    public string EstadoBoleta { get; set; } = null!;

    // Nav
    public Cliente Cliente { get; set; } = null!;
    public Empleado Empleado { get; set; } = null!;
    public ICollection<DetalleBoleta> DetallesBoleta { get; set; } = [];
}

// ─────────────────────────────────────────────
// DETALLE BOLETA
// ─────────────────────────────────────────────
public class DetalleBoleta
{
    [Required]
    [Column(TypeName = "char(8)")]
    [StringLength(8)]
    public string NumBoleta { get; set; } = null!;

    [Required]
    [Column(TypeName = "char(6)")]
    [StringLength(6)]
    public string IdProducto { get; set; } = null!;

    [Required]
    public int Cantidad { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal Importe { get; set; }

    // Nav
    public Boleta Boleta { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
