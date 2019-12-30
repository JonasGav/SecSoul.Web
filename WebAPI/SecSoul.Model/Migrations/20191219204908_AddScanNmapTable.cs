using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecSoul.Model.Migrations
{
    public partial class AddScanNmapTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsProcessed",
                table: "ScanRequest",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.CreateTable(
                name: "ScanNmap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScanRequestId = table.Column<int>(nullable: false),
                    ScanResult = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ScanNmap_pk", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "ScanNmap_ScanRequest_Id_fk",
                        column: x => x.ScanRequestId,
                        principalTable: "ScanRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ScanNmap_Id_uindex",
                table: "ScanNmap",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScanNmap_ScanRequestId",
                table: "ScanNmap",
                column: "ScanRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScanNmap");

            migrationBuilder.AlterColumn<bool>(
                name: "IsProcessed",
                table: "ScanRequest",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));
        }
    }
}
