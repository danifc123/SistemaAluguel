using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaAluguel.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    SenhaHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_ImovelId",
                table: "Contratos",
                column: "ImovelId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_InquilinoId",
                table: "Contratos",
                column: "InquilinoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_Imoveis_ImovelId",
                table: "Contratos",
                column: "ImovelId",
                principalTable: "Imoveis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_Inquilinos_InquilinoId",
                table: "Contratos",
                column: "InquilinoId",
                principalTable: "Inquilinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_Imoveis_ImovelId",
                table: "Contratos");

            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_Inquilinos_InquilinoId",
                table: "Contratos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Contratos_ImovelId",
                table: "Contratos");

            migrationBuilder.DropIndex(
                name: "IX_Contratos_InquilinoId",
                table: "Contratos");
        }
    }
}
