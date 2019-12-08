using Microsoft.EntityFrameworkCore.Migrations;

namespace SecSoul.Model.Migrations.SecSoul
{
    public partial class UpdateScanRequest_IsProcessed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "ScanRequest",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "ScanRequest");
        }
    }
}
