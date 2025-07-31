using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using ProyectoSistemaAgua.Models;
using BCrypt.Net;


namespace ProyectoSistemaAgua.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }



        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });
            return Ok(usuario);
        }


        [HttpPost("crear")]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuarios usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Correo) || string.IsNullOrWhiteSpace(usuario.Password))
                return BadRequest(new { mensaje = "Correo y contraseña son obligatorios" });

            var correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == usuario.Correo);
            if (correoExiste)
                return BadRequest(new { mensaje = "Ya existe un usuario con ese correo" });

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario creado correctamente", usuario });
        }




        // PUT: api/usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuarios usuarioActualizado)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }
            if (!string.IsNullOrWhiteSpace(usuarioActualizado.Password))
            {
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuarioActualizado.Password);
            }

            // Actualizar campos
            usuario.Nombre = usuarioActualizado.Nombre;
            usuario.Correo = usuarioActualizado.Correo;
            usuario.Password = usuarioActualizado.Password;
            usuario.Rol = usuarioActualizado.Rol;
            usuario.Telefono = usuarioActualizado.Telefono;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario actualizado correctamente", usuario });
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario eliminado correctamente" });
        }


    }
}