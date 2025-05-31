using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaSeuManoel.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caixas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Altura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Largura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comprimento = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidosCaixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    CaixaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosCaixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosCaixa_Caixas_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidosCaixa_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Altura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Largura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comprimento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    PedidoCaixaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_PedidosCaixa_PedidoCaixaId",
                        column: x => x.PedidoCaixaId,
                        principalTable: "PedidosCaixa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Produtos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCaixa_CaixaId",
                table: "PedidosCaixa",
                column: "CaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCaixa_PedidoId",
                table: "PedidosCaixa",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_PedidoCaixaId",
                table: "Produtos",
                column: "PedidoCaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_PedidoId",
                table: "Produtos",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "PedidosCaixa");

            migrationBuilder.DropTable(
                name: "Caixas");

            migrationBuilder.DropTable(
                name: "Pedidos");
        }
    }
}
