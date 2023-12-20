using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KudissangaSite.Migrations
{
    /// <inheritdoc />
    public partial class Tabelas02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservaItems_Suites_SuiteId",
                table: "ReservaItems");

            migrationBuilder.DropIndex(
                name: "IX_ReservaItems_SuiteId",
                table: "ReservaItems");

            migrationBuilder.DropColumn(
                name: "Comprovativo",
                table: "ReservaItems");

            migrationBuilder.DropColumn(
                name: "DiasEstadia",
                table: "ReservaItems");

            migrationBuilder.DropColumn(
                name: "SuiteId",
                table: "ReservaItems");

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "ReservaItems");

            migrationBuilder.AddColumn<string>(
                name: "Comprovativo",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DiasEstadia",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SuiteId",
                table: "Reservas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Valor",
                table: "Reservas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_SuiteId",
                table: "Reservas",
                column: "SuiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Suites_SuiteId",
                table: "Reservas",
                column: "SuiteId",
                principalTable: "Suites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Suites_SuiteId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_SuiteId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Comprovativo",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "DiasEstadia",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "SuiteId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Reservas");

            migrationBuilder.AddColumn<string>(
                name: "Comprovativo",
                table: "ReservaItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DiasEstadia",
                table: "ReservaItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SuiteId",
                table: "ReservaItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Valor",
                table: "ReservaItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaItems_SuiteId",
                table: "ReservaItems",
                column: "SuiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaItems_Suites_SuiteId",
                table: "ReservaItems",
                column: "SuiteId",
                principalTable: "Suites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
