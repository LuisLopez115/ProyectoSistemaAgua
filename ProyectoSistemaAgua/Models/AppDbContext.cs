using Microsoft.EntityFrameworkCore;
namespace ProyectoSistemaAgua.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Venta> Ventas { get; set; }

        public DbSet<Receta> Recetas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Receta>()
      .HasOne(r => r.ProductoFinal)
      .WithMany()
      .HasForeignKey(r => r.ProductoFinalId)
      .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receta>()
                .HasOne(r => r.ProductoComponente)
                .WithMany()
                .HasForeignKey(r => r.ProductoComponenteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}


