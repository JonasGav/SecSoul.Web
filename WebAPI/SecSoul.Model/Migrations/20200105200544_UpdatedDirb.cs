using Microsoft.EntityFrameworkCore.Migrations;

namespace SecSoul.Model.Migrations
{
    public partial class UpdatedDirb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HttpStatus",
                table: "ScanDirb",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsListable",
                table: "ScanDirb",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HttpStatus",
                table: "ScanDirb");

            migrationBuilder.DropColumn(
                name: "IsListable",
                table: "ScanDirb");
        }
    }
}
