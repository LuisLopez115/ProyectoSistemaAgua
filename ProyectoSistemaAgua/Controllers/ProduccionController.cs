using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSistemaAgua.Models;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduccionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProduccionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> ProducirKit(int productoFinalId, int cantidadProduccion)
        {
            var receta = await _context.Recetas
                .Where(r => r.ProductoFinalId == productoFinalId)
                .ToListAsync();

            if (receta.Count == 0)
                return BadRequest("No hay receta para este producto.");

            var productoFinal = await _context.Productos.FindAsync(productoFinalId);
            if (productoFinal == null)
                return NotFound("Producto final no encontrado.");

            decimal costoTotal = 0;

            foreach (var item in receta)
            {
                var componente = await _context.Productos.FindAsync(item.ProductoComponenteId);
                if (componente == null)
                    return NotFound($"Componente {item.ProductoComponenteId} no encontrado.");

                int cantidadNecesaria = item.Cantidad * cantidadProduccion;

                if (componente.Stock < cantidadNecesaria)
                    return BadRequest($"Stock insuficiente del componente {componente.Nombre}.");

                // Descontar stock del componente
                componente.Stock -= cantidadNecesaria;

                // Calcular costo total
                costoTotal += componente.CostoPromedio * cantidadNecesaria;
            }

            // Actualizar stock del producto final
            int stockAntes = productoFinal.Stock;
            productoFinal.Stock += cantidadProduccion;

            // Calcular nuevo costo promedio del producto final
            if (stockAntes == 0)
            {
                productoFinal.CostoPromedio = costoTotal / cantidadProduccion;
            }
            else
            {
                productoFinal.CostoPromedio =
                    ((productoFinal.CostoPromedio * stockAntes) + costoTotal) / (stockAntes + cantidadProduccion);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Producción completada",
                productoFinal = productoFinal.Nombre,
                cantidadProducida = cantidadProduccion,
                costoPromedioActualizado = productoFinal.CostoPromedio
            });
        }
    }

}
