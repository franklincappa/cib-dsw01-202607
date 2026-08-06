using Microsoft.AspNetCore.Mvc;
using mvc_producto.Models;
using Newtonsoft.Json;
using System.Text;


namespace mvc_producto.Controllers
{
    public class ProductoController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Producto> listadoProductos = new List<Producto>();
            using (var cli = new HttpClient())
            {
                cli.BaseAddress = new Uri("https://localhost:7060/");
                HttpResponseMessage response = await cli.GetAsync("api/productos");
                string apiResponse = await response.Content.ReadAsStringAsync();
                listadoProductos = JsonConvert.DeserializeObject<List<Producto>>(apiResponse).ToList();
            }
            return View(await Task.Run(() => listadoProductos));
        }

        public async Task<IActionResult> Create()
        {
            return View(await Task.Run(() => new Producto()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            string mensaje = "";

            //Impactar sobre API
            using (var cli = new HttpClient())
            {
                cli.BaseAddress = new Uri("https://localhost:7060/");
                StringContent content = new StringContent( JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await cli.PostAsync("api/productos", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse;
            }

            TempData["mensaje"] = mensaje;
            return View(await Task.Run(()=> producto));

        }




    }
}
