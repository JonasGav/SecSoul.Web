using Microsoft.EntityFrameworkCore.Migrations;

namespace SecSoul.Model.Migrations
{
    public partial class updatedVirusTotalAndDirbTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScanResult",
                table: "ScanDirb");

            migrationBuilder.AlterColumn<bool>(
                name: "ScanResult",
                table: "ScanVirusTotal",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScanProvider",
                table: "ScanVirusTotal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoundUrl",
                table: "ScanDirb",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDirectory",
                table: "ScanDirb",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScanProvider",
                table: "ScanVirusTotal");

            migrationBuilder.DropColumn(
                name: "FoundUrl",
                table: "ScanDirb");

            migrationBuilder.DropColumn(
                name: "IsDirectory",
                table: "ScanDirb");

            migrationBuilder.AlterColumn<string>(
                name: "ScanResult",
                table: "ScanVirusTotal",
                unicode: false,
                nullable: true,
                oldClrType: typeof(bool),
                oldUnicode: false);

            migrationBuilder.AddColumn<string>(
                name: "ScanResult",
                table: "ScanDirb",
                unicode: false,
                nullable: true);
        }
    }
}
