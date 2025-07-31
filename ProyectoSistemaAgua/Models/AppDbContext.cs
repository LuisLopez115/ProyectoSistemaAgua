using Microsoft.EntityFrameworkCore;

namespace ProyectoSistemaAgua.Models
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }


        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<MateriaPrima> MateriaPrima { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }


        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoMateriaPrima> ProductoMateriasPrimas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductoMateriaPrima>()
                .HasOne(pm => pm.Producto)
                .WithMany(p => p.Receta)
                .HasForeignKey(pm => pm.ProductoId);

            modelBuilder.Entity<ProductoMateriaPrima>()
                .HasOne(pm => pm.MateriaPrima)
                .WithMany()
                .HasForeignKey(pm => pm.MateriaPrimaId);

            modelBuilder.Entity<DetalleVenta>()
    .HasOne(dv => dv.Venta)
    .WithMany(v => v.Detalles)
    .HasForeignKey(dv => dv.VentaId);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Producto)
                .WithMany()
                .HasForeignKey(dv => dv.ProductoId);

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Usuario)
                .WithMany()
                .HasForeignKey(v => v.UsuarioId);

        }
    }

}

