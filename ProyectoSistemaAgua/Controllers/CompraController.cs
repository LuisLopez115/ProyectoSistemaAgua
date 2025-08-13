using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSistemaAgua.Models;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CompraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Compra>>> Get()
        {
            return await _context.Compras.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Compra>> Post(Compra compra)
        {
            var producto = await _context.Productos.FindAsync(compra.ProductoId);
            if (producto == null) return BadRequest("Producto no encontrado.");

            // Recalcular costo promedio
            if (producto.Stock == 0)
            {
                producto.CostoPromedio = compra.PrecioUnitario;
            }
            else
            {
                producto.CostoPromedio =
                    ((producto.Stock * producto.CostoPromedio) + (compra.Cantidad * compra.PrecioUnitario))
                    / (producto.Stock + compra.Cantidad);
            }

            // Aumentar stock
            producto.Stock += compra.Cantidad;

            // Guardar la compra
            compra.Fecha = DateTime.Now;
            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = compra.Id }, compra);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Compra compraActualizada)
        {
            if (id != compraActualizada.Id)
                return BadRequest("ID no coincide");

            var compraExistente = await _context.Compras.FindAsync(id);
            if (compraExistente == null)
                return NotFound("Compra no encontrada");

            var producto = await _context.Productos.FindAsync(compraActualizada.ProductoId);
            if (producto == null)
                return BadRequest("Producto no encontrado");

            // Aquí deberías revertir el stock y costo promedio de la compra vieja antes de actualizar

            // Revertir stock y costo promedio antiguos
            producto.Stock -= compraExistente.Cantidad;

            if (producto.Stock == 0)
            {
                producto.CostoPromedio = 0;
            }
            else
            {
                producto.CostoPromedio =
                    ((producto.Stock * producto.CostoPromedio) - (compraExistente.Cantidad * compraExistente.PrecioUnitario))
                    / producto.Stock;
            }

            // Actualizar compra con nuevos datos
            compraExistente.ProductoId = compraActualizada.ProductoId;
            compraExistente.Cantidad = compraActualizada.Cantidad;
            compraExistente.PrecioUnitario = compraActualizada.PrecioUnitario;
            compraExistente.Fecha = DateTime.Now;

            // Recalcular costo promedio con nueva compra
            producto.CostoPromedio =
                ((producto.Stock * producto.CostoPromedio) + (compraActualizada.Cantidad * compraActualizada.PrecioUnitario))
                / (producto.Stock + compraActualizada.Cantidad);

            // Ajustar stock con compra actualizada
            producto.Stock += compraActualizada.Cantidad;

            // Guardar cambios
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Compras.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
                return NotFound("Compra no encontrada");

            var producto = await _context.Productos.FindAsync(compra.ProductoId);
            if (producto == null)
                return BadRequest("Producto no encontrado");

            // Ajustar stock y costo promedio al eliminar la compra
            producto.Stock -= compra.Cantidad;

            if (producto.Stock == 0)
            {
                producto.CostoPromedio = 0;
            }
            else
            {
                producto.CostoPromedio =
                    ((producto.Stock * producto.CostoPromedio) - (compra.Cantidad * compra.PrecioUnitario))
                    / producto.Stock;
            }

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
    }
