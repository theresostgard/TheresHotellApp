using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotellApp.Migrations
{
    /// <inheritdoc />
    public partial class DeletedcollectionInvoicefromGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Guest_GuestId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_GuestId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Invoice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuestId",
                table: "Invoice",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_GuestId",
                table: "Invoice",
                column: "GuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Guest_GuestId",
                table: "Invoice",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "GuestId");
        }
    }
}
