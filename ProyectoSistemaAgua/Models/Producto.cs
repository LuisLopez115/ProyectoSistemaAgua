namespace ProyectoSistemaAgua.Models
{
    public class Producto
    {
        public int Id { get; set; } 

       public string Nombre { get; set; }  

        public string Descripcion { get; set; } 


        public string ImagenUrl { get; set; }   

        public string Documentacion { get; set; }

        public ICollection<ProductoMateriaPrima> Receta { get; set; }

    }
}
