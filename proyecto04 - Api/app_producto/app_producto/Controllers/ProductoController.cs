using app_producto.Dtos;
using app_producto.Models;
using app_producto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app_producto.Controllers
{
    [Route("api/productos")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _service;

        public ProductoController(ProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _service.GetByIdAsync(id);
            if (producto == null) { return NotFound(new { mensaje = $"Producto no encontrado" }); }
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoDto dto)
        {
            var crearProducto = await _service.CreateAsync(dto);
            return Ok(crearProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoDto dto)
        {
            var actualizarProducto = await _service.UpdatAsync(id, dto);
            if (actualizarProducto == null) { return NotFound(new { mensaje = $"Producto no encontrado" }); }
            return Ok(actualizarProducto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var eliminarProducto= await _service.DeleteAsync(id);
            return eliminarProducto ? Ok(eliminarProducto) : NotFound(new { mensaje = $"Producto no encontrado" });
        }


    }
}
