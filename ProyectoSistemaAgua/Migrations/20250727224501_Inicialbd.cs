using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoSistemaAgua.Migrations
{
    /// <inheritdoc />
    public partial class Inicialbd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contraseña",
                table: "Usuarios",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Usuarios",
                newName: "Contraseña");
        }
    }
}
