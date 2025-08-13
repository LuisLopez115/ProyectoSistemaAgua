using Microsoft.AspNetCore.Mvc;
using ProyectoSistemaAgua.Models;

namespace ProyectoSistemaAgua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase // <<--- importante
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("admin")]
        public IActionResult GetDashboardAdmin()
        {
            var totalUsuarios = _context.Usuarios.Count();
            var totalVentas = _context.Compras.Sum(c => c.Cantidad); // Cambia Monto por tu campo real
            var cantidadVentas = _context.Compras.Count();
            var totalProductos = _context.Productos.Count();
            var totalProveedores = _context.Proveedores.Count();

            return Ok(new
            {
                totalUsuarios,
                totalVentas,
                cantidadVentas,
                totalProductos,
                totalProveedores
            });
        }
    }
}
