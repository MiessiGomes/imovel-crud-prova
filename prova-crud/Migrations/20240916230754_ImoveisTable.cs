using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prova_crud.Migrations
{
    /// <inheritdoc />
    public partial class ImoveisTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Imoveis",
                columns: table => new
                {
                    Descricao = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    dataCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endereco = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imoveis", x => x.Descricao);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imoveis");
        }
    }
}
