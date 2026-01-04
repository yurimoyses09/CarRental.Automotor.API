using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Locadora.AutoMotors.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_automovel",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    modelo = table.Column<string>(type: "text", nullable: false),
                    ano_fabricacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    placa = table.Column<string>(type: "text", nullable: false),
                    combustivel = table.Column<string>(type: "text", nullable: false),
                    cor = table.Column<string>(type: "text", nullable: false),
                    dt_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_alteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_automovel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "text", nullable: false),
                    idade = table.Column<int>(type: "integer", nullable: false),
                    documento = table.Column<string>(type: "text", nullable: false),
                    dt_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_alteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_cliente", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_locadora",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_cliente = table.Column<int>(type: "integer", nullable: false),
                    id_automovel = table.Column<int>(type: "integer", nullable: false),
                    dt_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_fim_prevista = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_fim_real = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    valor_diaria = table.Column<decimal>(type: "numeric", nullable: false),
                    valor_total = table.Column<decimal>(type: "numeric", nullable: false),
                    status_aluguel = table.Column<int>(type: "integer", nullable: false),
                    dt_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_alteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_locadora", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_locadora_tb_automovel_id_automovel",
                        column: x => x.id_automovel,
                        principalTable: "tb_automovel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_locadora_tb_cliente_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "tb_cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_locadora_id_automovel",
                table: "tb_locadora",
                column: "id_automovel");

            migrationBuilder.CreateIndex(
                name: "IX_tb_locadora_id_cliente",
                table: "tb_locadora",
                column: "id_cliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_locadora");

            migrationBuilder.DropTable(
                name: "tb_automovel");

            migrationBuilder.DropTable(
                name: "tb_cliente");
        }
    }
}
