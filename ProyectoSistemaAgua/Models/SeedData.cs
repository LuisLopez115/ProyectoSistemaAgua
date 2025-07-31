namespace ProyectoSistemaAgua.Models
{
    public class SeedData
    {


        public static void Inicializar(AppDbContext context)
        {
            // Si ya hay usuarios, no inserta nada
            if (context.Usuarios.Any())
            {
                return;
            }


            context.Usuarios.AddRange(
                new Usuarios
                {
                    Nombre = "Luis",
                    Correo = "luis@example.com",
                    Password = "123456",
                    Rol = "Admin",
                    Telefono = "4771234567"
                },
                new Usuarios
                {
                    Nombre = "Carlos",
                    Correo = "carlos@example.com",
                    Password = "abcdef",
                    Rol = "Cliente",
                    Telefono = "4779876543"
                }
            );

            context.SaveChanges();
        }
    }


}

