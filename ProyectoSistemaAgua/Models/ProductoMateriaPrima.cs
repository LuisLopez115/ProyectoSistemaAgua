﻿namespace ProyectoSistemaAgua.Models
{
    public class ProductoMateriaPrima
    {

        public int Id { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; }

        public int Cantidad { get; set; } 







    }
}
