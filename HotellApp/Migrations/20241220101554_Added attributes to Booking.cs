using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotellApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedattributestoBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountOfExtraBeds",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsExtraBedAllowed",
                table: "Booking",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfExtraBeds",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "IsExtraBedAllowed",
                table: "Booking");
        }
    }
}
