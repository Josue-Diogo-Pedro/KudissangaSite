using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KudissangaSite.Migrations
{
    /// <inheritdoc />
    public partial class Tabelas01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comprovativo",
                table: "ReservaItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comprovativo",
                table: "ReservaItems");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Funcionarios");
        }
    }
}
