namespace ProyectoSistemaAgua.DTO
{
    public class ProductoClienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public List<string> DocumentacionUrls { get; set; } = new(); // PDF, Manuales, etc.
    }

}
