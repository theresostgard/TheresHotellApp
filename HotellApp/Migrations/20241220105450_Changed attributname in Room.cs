using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotellApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangedattributnameinRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Room",
                newName: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Room",
                newName: "RoomID");
        }
    }
}
