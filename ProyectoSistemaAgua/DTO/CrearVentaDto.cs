namespace ProyectoSistemaAgua.DTO
{
    public class CrearVentaDto
    {
        public int UsuarioId { get; set; } // Cliente
        public string Comentarios { get; set; } = string.Empty;
        public List<DetalleVentaDto> Detalles { get; set; } = new();
    }





}
