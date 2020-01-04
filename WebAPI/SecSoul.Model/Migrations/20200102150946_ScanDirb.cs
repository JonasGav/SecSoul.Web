using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecSoul.Model.Migrations
{
    public partial class ScanDirb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScanDirb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScanRequestId = table.Column<int>(nullable: false),
                    ScanResult = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ScanDirb_pk", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "ScanDirb_ScanRequest_Id_fk",
                        column: x => x.ScanRequestId,
                        principalTable: "ScanRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ScanDirb_Id_uindex",
                table: "ScanDirb",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScanDirb_ScanRequestId",
                table: "ScanDirb",
                column: "ScanRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScanDirb");
        }
    }
}
