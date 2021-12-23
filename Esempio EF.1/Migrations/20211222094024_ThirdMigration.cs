using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Esempio_EF._1.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Azienda",
                columns: table => new
                {
                    AziendaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Azienda", x => x.AziendaId);
                });

            migrationBuilder.CreateTable(
                name: "Impiegato",
                columns: table => new
                {
                    ImpiegatoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AziendaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impiegato", x => x.ImpiegatoId);
                    table.ForeignKey(
                        name: "FK_Impiegato_Azienda_AziendaId",
                        column: x => x.AziendaId,
                        principalTable: "Azienda",
                        principalColumn: "AziendaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Impiegato_AziendaId",
                table: "Impiegato",
                column: "AziendaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Impiegato");

            migrationBuilder.DropTable(
                name: "Azienda");
        }
    }
}
