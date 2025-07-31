namespace ProyectoSistemaAgua.Models
{
    public class MateriaPrima
    {

        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public decimal CostoPromedio { get; set; }
        public int CantidadStock { get; set; }


        public string Nombre { get; set; } 


        public int ProveedorId { get; set; }

        public Proveedor Proveedor { get; set; }


    }
}
