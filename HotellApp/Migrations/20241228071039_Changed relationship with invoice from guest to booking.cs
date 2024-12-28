using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotellApp.Migrations
{
    /// <inheritdoc />
    public partial class Changedrelationshipwithinvoicefromguesttobooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Guest_GuestId",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "Invoice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_BookingId",
                table: "Invoice",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Booking_BookingId",
                table: "Invoice",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Guest_GuestId",
                table: "Invoice",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "GuestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Booking_BookingId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Guest_GuestId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_BookingId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Guest_GuestId",
                table: "Invoice",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "GuestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
