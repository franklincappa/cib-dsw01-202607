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

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");
                    
            Producto producto = new Producto();
            using (var cli = new HttpClient())
            {
                cli.BaseAddress = new Uri("https://localhost:7060/");
                HttpResponseMessage response = await cli.GetAsync("api/productos/"+id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                producto = JsonConvert.DeserializeObject<Producto>(apiResponse);
            }
            return View(await Task.Run(()=>producto));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Producto producto)
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
                StringContent content = new StringContent(JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await cli.PutAsync("api/productos", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = "Actualizado: "+apiResponse;
            }

            TempData["mensaje"] = mensaje;
            return View(await Task.Run(() => producto));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string mensaje = "";
            using (var cli = new HttpClient())
            {
                cli.BaseAddress = new Uri("https://localhost:7060/");
                HttpResponseMessage response = await cli.DeleteAsync("api/productos/"+id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = "Registro Eliminado: " + apiResponse;
            }

            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

    }
}
