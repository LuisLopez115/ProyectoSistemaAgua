using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSistemaAgua.DTO;
using ProyectoSistemaAgua.Models;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RecetaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecetaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("crearRecetaCompleta")]
        public async Task<IActionResult> CrearRecetaCompleta([FromBody] List<RecetaDto> recetasDto)
        {
            if (recetasDto == null || !recetasDto.Any())
                return BadRequest("La receta no puede estar vacía.");

            // Convertís los DTOs a entidades Receta si necesitás guardar relaciones
            var recetas = recetasDto.Select(dto => new Receta
            {
                ProductoFinalId = dto.ProductoFinalId,
                ProductoComponenteId = dto.ProductoComponenteId,
                Cantidad = dto.Cantidad
            }).ToList();

            var productoFinalId = recetas[0].ProductoFinalId;

            var recetaExistente = _context.Recetas.Where(r => r.ProductoFinalId == productoFinalId);
            _context.Recetas.RemoveRange(recetaExistente);

            await _context.Recetas.AddRangeAsync(recetas);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Receta completa creada o actualizada" });
        }

        // Obtener receta por producto final
        [HttpGet("{productoFinalId}")]
        public async Task<ActionResult<List<Receta>>> Get(int productoFinalId)
        {
            var receta = await _context.Recetas
                .Include(r => r.ProductoComponente) // para detalles del componente
                .Where(r => r.ProductoFinalId == productoFinalId)
                .ToListAsync();

            if (receta.Count == 0)
                return NotFound("No hay receta registrada para este producto.");

            return receta;
        }

        // Actualizar un componente de receta
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarReceta(int id, Receta receta)
        {
            if (id != receta.Id)
                return BadRequest("Id no coincide.");

            var recetaExistente = await _context.Recetas.FindAsync(id);
            if (recetaExistente == null)
                return NotFound();

            recetaExistente.ProductoFinalId = receta.ProductoFinalId;
            recetaExistente.ProductoComponenteId = receta.ProductoComponenteId;
            recetaExistente.Cantidad = receta.Cantidad;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("ProductosConReceta")]
        public async Task<ActionResult<List<Producto>>> GetProductosConReceta()
        {
            var productosConReceta = await _context.Recetas
                .Include(r => r.ProductoFinal)
                .Select(r => r.ProductoFinal)
                .Distinct()
                .ToListAsync();

            if (!productosConReceta.Any())
                return NotFound("No hay productos con receta registrada.");

            return productosConReceta;
        }

        // Eliminar un componente de receta
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarReceta(int id)
        {
            var receta = await _context.Recetas.FindAsync(id);
            if (receta == null)
                return NotFound();

            _context.Recetas.Remove(receta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        }

    }
