using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (!ModelState.IsValid) { 
                return View(producto);
            }

            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                await cn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_insertar_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", producto.descripcion);
                cmd.Parameters.AddWithValue("@umedida", producto.umedida);
                cmd.Parameters.AddWithValue("@precio", producto.precio);
                cmd.Parameters.AddWithValue("@stock", producto.stock);

                int filas = await cmd.ExecuteNonQueryAsync();

                mensaje = $"Se inserto {filas} registro";

            }

            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");

        }

        public  IActionResult Create()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Producto producto = new Producto();

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                cn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_buscar_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idproducto", id);


                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        producto.idproducto = Convert.ToInt32(dr["idproducto"]);
                        producto.descripcion = dr["descripcion"].ToString();
                        producto.umedida = dr["umedida"].ToString();
                        producto.precio = Convert.ToDecimal(dr["precio"]);
                        producto.stock = Convert.ToInt32(dr["stock"]);
                    }
                    else 
                    {
                        TempData["mensaje"] = "Producto no encontrado";
                        return RedirectToAction("Index");
                    }
                }

            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Producto producto)
        {
            //Guardar registro a actualizar

            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                await cn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_actualizar_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idproducto", producto.idproducto);
                cmd.Parameters.AddWithValue("@descripcion", producto.descripcion);
                cmd.Parameters.AddWithValue("@umedida", producto.umedida);
                cmd.Parameters.AddWithValue("@precio", producto.precio);
                cmd.Parameters.AddWithValue("@stock", producto.stock);

                int filas = await cmd.ExecuteNonQueryAsync();

                mensaje = $"Se actualizó {filas} registro";

            }

            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");

        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                cn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_eliminar_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idproducto", id);

                int filas = await cmd.ExecuteNonQueryAsync();

                mensaje = $"Se elimino {filas} registro";

            }

            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");
        }


    }
}
