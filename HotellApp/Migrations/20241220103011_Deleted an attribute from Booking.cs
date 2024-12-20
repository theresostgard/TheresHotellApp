using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotellApp.Migrations
{
    /// <inheritdoc />
    public partial class DeletedanattributefromBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExtraBedAllowed",
                table: "Booking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExtraBedAllowed",
                table: "Booking",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
