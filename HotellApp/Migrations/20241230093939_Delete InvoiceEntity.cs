using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotellApp.Migrations
{
    /// <inheritdoc />
    public partial class DeleteInvoiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "GuestId", "EmailAdress", "FirstName", "GuestStatus", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "jon.snow@GoT.com", "Jon", 0, "Snow", "0732652651" },
                    { 2, "tyrion.lannister@GoT.com", "Tyrion", 0, "Lannister", "0735454748" },
                    { 3, "daenerys.targaryen@GoT.com", "Daenerys", 0, "Targaryen", "0707455478" },
                    { 4, "shireen.baratheon@GoT.com", "Shireen", 0, "Baratheon", "0768597584" },
                    { 5, "sam.tarly@GoT.com", "Samwell", 0, "Tarly", "0705963587" },
                    { 6, "jorah.mormont@GoT.com", "Jorah", 0, "Mormont", "0768574236" },
                    { 7, "talisa.stark@GoT.com", "Talisa", 0, "Stark", "0761235426" },
                    { 8, "grey.worm@GoT.com", "Grey", 0, "Worm", "0709876985" }
                });

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "RoomId", "AmountOfExtraBeds", "IsExtraBedAllowed", "PricePerNight", "RoomSize", "RoomType", "Status" },
                values: new object[,]
                {
                    { 101, "One", true, 2000m, 25, "Double", "Active" },
                    { 102, "None", false, 1200m, 14, "Single", "Active" },
                    { 103, "Two", true, 2000m, 32, "Double", "Active" },
                    { 104, "One", true, 1800m, 17, "Double", "Active" },
                    { 105, "One", true, 2100m, 25, "Double", "Active" },
                    { 106, "None", false, 1400m, 14, "Double", "UnderMaintenance" },
                    { 107, "Two", true, 3500m, 42, "Double", "InActive" },
                    { 108, "None", false, 1300m, 15, "Single", "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Guest",
                keyColumn: "GuestId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomId",
                keyValue: 108);

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoice_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_BookingId",
                table: "Invoice",
                column: "BookingId");
        }
    }
}
