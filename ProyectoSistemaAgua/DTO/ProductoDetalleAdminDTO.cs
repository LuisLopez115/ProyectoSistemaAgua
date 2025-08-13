namespace ProyectoSistemaAgua.DTO
{
    public class ProductoDetalleAdminDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public string Documentacion { get; set; } = string.Empty; // <-- ¡AGREGA ESTO!

        public List<IngredienteDetalleDTO> Receta { get; set; } = new();
        public decimal CostoTotal { get; set; }
    }

    public class IngredienteDetalleDTO
    {
        public int MateriaPrimaId { get; set; }
        public string NombreMateria { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

}
