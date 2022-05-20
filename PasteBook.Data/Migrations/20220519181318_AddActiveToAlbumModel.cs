using Microsoft.EntityFrameworkCore.Migrations;

namespace PasteBook.Data.Migrations
{
    public partial class AddActiveToAlbumModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Albums",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Albums");
        }
    }
}
