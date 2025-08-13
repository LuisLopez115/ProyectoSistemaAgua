namespace ProyectoSistemaAgua.Models
{
    public class Producto
    {

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal CostoPromedio { get; set; }  // para el método de costeo
        public int Stock { get; set; }
        public string? Categoria { get; set; }


    }
}
