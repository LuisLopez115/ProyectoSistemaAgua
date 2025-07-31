using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSistemaAgua.DTO;
using ProyectoSistemaAgua.Models;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaPrimaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MateriaPrimaController(AppDbContext context)
        {
            _context = context;
        }
        // Este es el método completo (para admin, pruebas, etc.)
        [HttpGet("completo")]
        public async Task<IActionResult> GetMateriasPrimas()
        {
            var materias = await _context.MateriaPrima
                .Include(m => m.Proveedor)
                .ToListAsync();

            return Ok(materias);
        }

        // Este es el que usarás en Angular (DTO limpio)
        [HttpGet]
        public async Task<IActionResult> GetMateriasDTO()
        {
            var materias = await _context.MateriaPrima
                .Include(m => m.Proveedor)
                .Select(m => new MateriaPrimaDto
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Descripcion = m.Descripcion,
                    CostoPromedio = m.CostoPromedio,
                    CantidadStock = m.CantidadStock,
                    ProveedorId = m.ProveedorId,
                    ProveedorNombre = m.Proveedor.Nombre
                })
                .ToListAsync();

            return Ok(materias);
        }




        [HttpPost]
        public async Task<IActionResult> CrearMateriaPrima([FromBody] MateriaPrimaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar si existe el proveedor usando proveedorId
            var proveedorExiste = await _context.Proveedores.FindAsync(dto.ProveedorId);
            if (proveedorExiste == null)
                return NotFound(new { mensaje = "Proveedor no encontrado" });

            // Mapear dto a entidad MateriaPrima
            var materia = new MateriaPrima
            {
                Descripcion = dto.Descripcion,
                Nombre = dto.Nombre,
                CostoPromedio = dto.CostoPromedio,
                CantidadStock = dto.CantidadStock,
                ProveedorId = dto.ProveedorId
            };

            _context.MateriaPrima.Add(materia);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Materia prima registrada correctamente", materia });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarMateriaPrima(int id, [FromBody] MateriaPrimaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest(new { mensaje = "El ID no coincide" });

            var materiaExistente = await _context.MateriaPrima.FindAsync(id);
            if (materiaExistente == null)
                return NotFound(new { mensaje = "Materia prima no encontrada" });

            var proveedorExiste = await _context.Proveedores.FindAsync(dto.ProveedorId);
            if (proveedorExiste == null)
                return NotFound(new { mensaje = "Proveedor no encontrado" });

            // Actualizar campos
            materiaExistente.Descripcion = dto.Descripcion;
            materiaExistente.Nombre = dto.Nombre;
            materiaExistente.CostoPromedio = dto.CostoPromedio;
            materiaExistente.CantidadStock = dto.CantidadStock;
            materiaExistente.ProveedorId = dto.ProveedorId;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Materia prima actualizada correctamente", materiaExistente });
        }

        // Opcional: método para eliminar
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMateriaPrima(int id)
        {
            var materiaExistente = await _context.MateriaPrima.FindAsync(id);
            if (materiaExistente == null)
                return NotFound(new { mensaje = "Materia prima no encontrada" });

            _context.MateriaPrima.Remove(materiaExistente);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Materia prima eliminada correctamente" });
        }
    }

}
