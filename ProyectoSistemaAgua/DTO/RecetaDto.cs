using System.ComponentModel.DataAnnotations;

namespace ProyectoSistemaAgua.DTO
{
    public class RecetaDto
    {
        [Required]
        public int ProductoFinalId { get; set; }

        [Required]
        public int ProductoComponenteId { get; set; }

        [Required]
        public int Cantidad { get; set; }
    }

}
