using System.ComponentModel.DataAnnotations;

namespace ProyectoSistemaAgua.DTO
{
    public class MateriaPrimaDto
    {

        public int? Id { get; set; }  // Opcional para crear, obligatorio para editar

        public string Descripcion { get; set; }

        public string Nombre { get; set; }  // Opcional, si tu modelo lo usa

        public decimal CostoPromedio { get; set; }

        public int CantidadStock { get; set; }

        public int ProveedorId { get; set; }
        public string ProveedorNombre { get; set; } = "";

    }
}
