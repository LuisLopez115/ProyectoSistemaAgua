using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSistemaAgua.DTO;
using ProyectoSistemaAgua.Models;
using System.Security.Claims;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoDetalleAdminDTO dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                ImagenUrl = dto.ImagenUrl,
                Receta = new List<ProductoMateriaPrima>()
            };

            foreach (var ingrediente in dto.Receta)
            {
                var materia = await _context.MateriaPrima.FindAsync(ingrediente.MateriaPrimaId);
                if (materia == null)
                    return NotFound(new { mensaje = $"Materia prima ID {ingrediente.MateriaPrimaId} no encontrada" });

                producto.Receta.Add(new ProductoMateriaPrima
                {
                    MateriaPrimaId = ingrediente.MateriaPrimaId,
                    Cantidad = (int)ingrediente.Cantidad
                });
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Producto creado correctamente", producto });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Receta)
                .ThenInclude(r => r.MateriaPrima)
                .ToListAsync();

            // Devuelve el listado sin filtro por rol
            return Ok(productos);
        }
    }
}
