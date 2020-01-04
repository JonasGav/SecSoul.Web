using Microsoft.EntityFrameworkCore.Migrations;

namespace SecSoul.Model.Migrations
{
    public partial class UpdatedFtpCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FtpPassword",
                table: "ScanRequest",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FtpUsername",
                table: "ScanRequest",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FtpPassword",
                table: "ScanRequest");

            migrationBuilder.DropColumn(
                name: "FtpUsername",
                table: "ScanRequest");
        }
    }
}
