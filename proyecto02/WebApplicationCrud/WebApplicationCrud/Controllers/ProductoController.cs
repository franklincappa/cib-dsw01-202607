using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplicationCrud.Models;

namespace WebApplicationCrud.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _config;

        public ProductoController(IConfiguration config)
        {
            _config = config;            
        }

        IEnumerable<Producto> productos() 
        {
            List<Producto> listaProductos = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            { 
                cn.Open();
                SqlCommand cmd = new SqlCommand("exec usp_productos",cn);
                SqlDataReader dr= cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaProductos.Add(new Producto
                    {
                        idproducto = dr.GetInt32(0),
                        descripcion = dr.GetString(1),
                        umedida = dr.GetString(2),
                        precio = dr.GetDecimal(3),
                        stock = dr.GetInt32(4)
                    });

                }
                dr.Close();

            }
            return listaProductos;
        }

        IEnumerable<Producto> filtro_productos(string descripcion)
        {
            List<Producto> listaProductos = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_productos_filtrar", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaProductos.Add(new Producto
                    {
                        idproducto = dr.GetInt32(0),
                        descripcion = dr.GetString(1),
                        umedida = dr.GetString(2),
                        precio = dr.GetDecimal(3),
                        stock = dr.GetInt32(4)
                    });

                }
                dr.Close();

            }
            return listaProductos;
        }



        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => productos()));
        }

        public async Task<IActionResult> Filtrar(string? descripcion = null)
        {
            return View(await Task.Run(() =>
                string.IsNullOrEmpty(descripcion) ? productos() : filtro_productos(descripcion)));

        }

        public async Task<IActionResult> Paginacion(int pagina =0)
        {
            IEnumerable <Producto> listaProductos = productos();

            int fila = 10;
            int cant = listaProductos.Count();
            int paginas = cant % fila == 0 ? cant / fila : cant / fila + 1;

            ViewBag.Pagina = pagina;
            ViewBag.Paginas = paginas;

            return View(await Task.Run(() =>
                listaProductos.Skip(fila * pagina).Take(fila)));

        }





    }
}
