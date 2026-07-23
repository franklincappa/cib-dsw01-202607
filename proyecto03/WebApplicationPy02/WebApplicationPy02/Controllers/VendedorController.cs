using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Threading.Tasks;
using WebApplicationPy02.Models;

namespace WebApplicationPy02.Controllers
{
    public class VendedorController : Controller
    {
        private readonly IConfiguration _config;

        public VendedorController(IConfiguration config)
        {
            _config = config;
        }

        IEnumerable<Vendedor> vendedores() 
        {
            List<Vendedor> listaVendedores = new List<Vendedor>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_vendedores", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaVendedores.Add(new Vendedor
                    {
                        idVendedor = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        direccion = dr.GetString(2),
                        idPais = dr.GetString(3),
                        email = dr.GetString(4)
                    });

                }
                dr.Close();

            }
            return listaVendedores;
        }

        IEnumerable<Pais> paises()
        {
            List<Pais> listaPaises = new List<Pais>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_paises", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaPaises.Add(new Pais
                    {
                        idPais = dr.GetString(0),
                        nombre = dr.GetString(1)                        
                    });

                }
                dr.Close();

            }
            return listaPaises;
        }

        Vendedor buscar(int id)
        {
            return vendedores().Where(ven => ven.idVendedor == id).FirstOrDefault();
        }

        String registrarVendedor(Vendedor vendedor)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_merge_vendedor", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idvendedor", vendedor.idVendedor);
                    cmd.Parameters.AddWithValue("@nombre", vendedor.nombre);
                    cmd.Parameters.AddWithValue("@direccion", vendedor.direccion);
                    cmd.Parameters.AddWithValue("@idpais", vendedor.idPais);
                    cmd.Parameters.AddWithValue("@email", vendedor.email);
                    cn.Open();
                    int filas = cmd.ExecuteNonQuery();
                    mensaje = $"Se insertó {filas} vendedor";

                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cn.Close();  }
            }
            return mensaje;
        }


        public async Task<IActionResult> Index()
        {
            return View( await Task.Run(()=> vendedores()));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.paises = new SelectList(paises(), "idPais", "nombre");
            return View(await Task.Run(() => new Vendedor()));
        }


        [HttpPost]
        public async Task<IActionResult> Create(Vendedor vendedor)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.paises = new SelectList(paises(), "idPais", "nombre", vendedor.idPais);
                return View(await Task.Run(() => vendedor));
            }

            ViewBag.mensaje = registrarVendedor(vendedor);
            ViewBag.paises = new SelectList(paises(), "idPais", "nombre", vendedor.idPais);
            return View(await Task.Run(()=> vendedor));
        }

    }
}
