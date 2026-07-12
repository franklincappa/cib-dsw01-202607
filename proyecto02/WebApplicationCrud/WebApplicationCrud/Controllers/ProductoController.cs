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

        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => productos()));
        }



    }
}
