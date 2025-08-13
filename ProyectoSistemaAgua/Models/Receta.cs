using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoSistemaAgua.Models
{
    public class Receta
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductoFinalId { get; set; }

        [Required]
        public int ProductoComponenteId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [JsonIgnore]
        public virtual Producto ProductoFinal { get; set; }

        [JsonIgnore]
        public virtual Producto ProductoComponente { get; set; }
    }

}
