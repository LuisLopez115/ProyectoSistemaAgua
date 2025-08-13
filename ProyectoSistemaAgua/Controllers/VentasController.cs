using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSistemaAgua.DTO;
using ProyectoSistemaAgua.Models;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Venta venta)
        {
            var producto = await _context.Productos.FindAsync(venta.ProductoId);
            if (producto == null) return BadRequest("Producto no encontrado.");

            if (producto.Stock < venta.Cantidad)
                return BadRequest("Stock insuficiente.");

            // Restar stock del producto final (el kit)
            producto.Stock -= venta.Cantidad;

            // Registrar la venta
            venta.Fecha = DateTime.Now;
            _context.Ventas.Add(venta);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Venta registrada",
                producto = producto.Nombre,
                cantidad = venta.Cantidad,
                precioUnitario = venta.PrecioUnitario,
                total = venta.Cantidad * venta.PrecioUnitario
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<Venta>>> Get()
        {
            return await _context.Ventas.ToListAsync();
        }


    



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return NotFound("Venta no encontrada");

            var producto = await _context.Productos.FindAsync(venta.ProductoId);
            if (producto == null)
                return BadRequest("Producto no encontrado");

            // Devolver stock al producto al eliminar la venta
            producto.Stock += venta.Cantidad;

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}