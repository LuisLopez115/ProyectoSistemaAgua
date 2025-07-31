using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSistemaAgua.DTO;
using ProyectoSistemaAgua.Models;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/ventas
        [HttpPost]
        public async Task<IActionResult> CrearVenta([FromBody] CrearVentaDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null)
                return BadRequest(new { mensaje = "Usuario no encontrado" });

            var venta = new Venta
            {
                UsuarioId = dto.UsuarioId,
                Fecha = DateTime.Now,
                Comentarios = dto.Comentarios,
                Detalles = new List<DetalleVenta>()
            };

            decimal total = 0;

            foreach (var detalle in dto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto == null)
                    return BadRequest(new { mensaje = $"Producto con ID {detalle.ProductoId} no encontrado" });

                venta.Detalles.Add(new DetalleVenta
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                });

                total += detalle.Cantidad * detalle.PrecioUnitario;
            }

            venta.Total = total;

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Venta registrada correctamente", venta.Id });
        }

        // GET: api/ventas
        [HttpGet]
        public async Task<IActionResult> ObtenerVentas()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();

            var resultado = ventas.Select(v => new
            {
                v.Id,
                Cliente = v.Usuario.Nombre,
                v.Fecha,
                v.Total,
                v.Comentarios,
                Detalles = v.Detalles.Select(d => new
                {
                    Producto = d.Producto.Nombre,
                    d.Cantidad,
                    d.PrecioUnitario,
                    Subtotal = d.Cantidad * d.PrecioUnitario
                })
            });

            return Ok(resultado);
        }

        // GET: api/ventas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVentaPorId(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada" });

            var resultado = new
            {
                venta.Id,
                Cliente = venta.Usuario.Nombre,
                venta.Fecha,
                venta.Total,
                venta.Comentarios,
                Detalles = venta.Detalles.Select(d => new
                {
                    Producto = d.Producto.Nombre,
                    d.Cantidad,
                    d.PrecioUnitario,
                    Subtotal = d.Cantidad * d.PrecioUnitario
                })
            };

            return Ok(resultado);
        }

        // PUT: api/ventas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarVenta(int id, [FromBody] CrearVentaDto dto)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada" });

            venta.Comentarios = dto.Comentarios;
            venta.Fecha = DateTime.Now;
            venta.Detalles.Clear();

            decimal total = 0;

            foreach (var detalle in dto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto == null)
                    return BadRequest(new { mensaje = $"Producto con ID {detalle.ProductoId} no encontrado" });

                venta.Detalles.Add(new DetalleVenta
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                });

                total += detalle.Cantidad * detalle.PrecioUnitario;
            }

            venta.Total = total;

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Venta actualizada correctamente" });
        }

        // DELETE: api/ventas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada" });

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Venta eliminada correctamente" });
        }
    }
}
