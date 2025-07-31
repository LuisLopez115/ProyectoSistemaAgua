namespace ProyectoSistemaAgua.Models
{
    public class Usuarios
    {

        public int Id { get; set; }

        public String? Nombre { get; set; }


        public string? Correo { get; set; }
        public string? Password { get; set; }
        public string? Rol { get; set; }
        public string Telefono { get; set; }
    }
}
