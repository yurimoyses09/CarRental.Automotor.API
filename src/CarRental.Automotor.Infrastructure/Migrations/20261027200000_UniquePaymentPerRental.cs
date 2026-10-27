using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Automotor.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20261027200000_UniquePaymentPerRental")]
    public partial class UniquePaymentPerRental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_payment_rental_id",
                table: "tb_payment");

            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX ""IX_tb_payment_rental_id"" ON tb_payment (rental_id)
                WHERE payment_status = 2 AND delete_at = '0001-01-01 00:00:00+00';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_payment_rental_id",
                table: "tb_payment");

            migrationBuilder.CreateIndex(
                name: "IX_tb_payment_rental_id",
                table: "tb_payment",
                column: "rental_id");
        }
    }
}
