namespace ProyectoSistemaAgua.Models
{
    public class Venta
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuarios Usuario { get; set; }  

        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        public string? Comentarios { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; }
    }


}
