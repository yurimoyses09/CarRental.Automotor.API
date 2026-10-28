using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Automotor.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20261027210000_UniqueClientDocumentAndAutomobilePlate")]
    public partial class UniqueClientDocumentAndAutomobilePlate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX ""IX_tb_client_document"" ON tb_client (document)
                WHERE delete_at = '0001-01-01 00:00:00+00';

                CREATE UNIQUE INDEX ""IX_tb_automobile_plate"" ON tb_automobile (plate)
                WHERE delete_at = '0001-01-01 00:00:00+00';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP INDEX IF EXISTS ""IX_tb_client_document"";
                DROP INDEX IF EXISTS ""IX_tb_automobile_plate"";");
        }
    }
}
