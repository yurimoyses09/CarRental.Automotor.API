using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locadora.AutoMotors.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteAnoFabricacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ano_fabricacao",
                table: "tb_automovel",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ano_fabricacao",
                table: "tb_automovel",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
