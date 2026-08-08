using AutoMapper;
using VentasAPI.Models.DTOs.Boleta;
using VentasAPI.Models.DTOs.Cargo;
using VentasAPI.Models.DTOs.Categoria;
using VentasAPI.Models.DTOs.Cliente;
using VentasAPI.Models.DTOs.Distrito;
using VentasAPI.Models.DTOs.Empleado;
using VentasAPI.Models.DTOs.Producto;
using VentasAPI.Models.Entities;

namespace VentasAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Distrito
        CreateMap<Distrito, DistritoDto>();
        CreateMap<CreateUpdateDistritoDto, Distrito>();

        // Cargo
        CreateMap<Cargo, CargoDto>();
        CreateMap<CreateUpdateCargoDto, Cargo>();

        // Empleado
        CreateMap<Empleado, EmpleadoDto>()
            .ForCtorParam("NombreDistrito", opt => opt.MapFrom(src => src.Distrito != null ? src.Distrito.NombreDistrito : ""))
            .ForCtorParam("NombreCargo", opt => opt.MapFrom(src => src.Cargo != null ? src.Cargo.NombreCargo : ""));
        CreateMap<CreateEmpleadoDto, Empleado>();
        CreateMap<UpdateEmpleadoDto, Empleado>();

        // Cliente
        CreateMap<Cliente, ClienteDto>()
            .ForCtorParam("NombreDistrito", opt => opt.MapFrom(src => src.Distrito != null ? src.Distrito.NombreDistrito : ""));
        CreateMap<CreateClienteDto, Cliente>();
        CreateMap<UpdateClienteDto, Cliente>();

        // Categoria
        CreateMap<Categoria, CategoriaDto>();
        CreateMap<CreateUpdateCategoriaDto, Categoria>();

        // Producto
        CreateMap<Producto, ProductoDto>()
            .ForCtorParam("NombreCategoria", opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : ""));
        CreateMap<CreateProductoDto, Producto>();
        CreateMap<UpdateProductoDto, Producto>();

        // DetalleBoleta
        CreateMap<DetalleBoleta, DetalleBoletaDto>()
            .ForCtorParam("DescripcionProducto", opt => opt.MapFrom(src => src.Producto != null ? src.Producto.Descripcion : ""));
        CreateMap<CreateDetalleBoletaDto, DetalleBoleta>();

        // Boleta
        CreateMap<Boleta, BoletaDto>()
            .ForCtorParam("NombreCliente", opt => opt.MapFrom(src =>
                src.Cliente != null ? $"{src.Cliente.Nombres} {src.Cliente.Apellidos}" : ""))
            .ForCtorParam("NombreEmpleado", opt => opt.MapFrom(src =>
                src.Empleado != null ? $"{src.Empleado.Nombres} {src.Empleado.Apellidos}" : ""))
            .ForCtorParam("Detalles", opt => opt.MapFrom(src => src.DetallesBoleta))
            .ForCtorParam("Total", opt => opt.MapFrom(src =>
                src.DetallesBoleta != null ? src.DetallesBoleta.Sum(d => d.Importe) : 0));
        CreateMap<CreateBoletaDto, Boleta>()
            .ForMember(dest => dest.DetallesBoleta, opt => opt.MapFrom(src => src.Detalles));
    }
}
