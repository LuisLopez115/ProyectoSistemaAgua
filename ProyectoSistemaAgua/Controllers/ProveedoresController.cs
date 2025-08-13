using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSistemaAgua.Models;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerProveedores()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            return Ok(proveedores);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProveedor([FromBody] Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Proveedor creado correctamente", proveedor });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] Proveedor proveedorActualizado)
        {
            var proveedorExistente = await _context.Proveedores.FindAsync(id);
            if (proveedorExistente == null)
            {
                return NotFound(new { mensaje = "Proveedor no encontrado" });
            }

            // Actualiza solo los campos necesarios
            proveedorExistente.Nombre = proveedorActualizado.Nombre;
            proveedorExistente.Tel = proveedorActualizado.Tel;
            proveedorExistente.Direccion = proveedorActualizado.Direccion;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Proveedor actualizado correctamente", proveedor = proveedorExistente });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound(new { mensaje = "Proveedor no encontrado" });
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Proveedor eliminado correctamente" });
        }

    }
}
