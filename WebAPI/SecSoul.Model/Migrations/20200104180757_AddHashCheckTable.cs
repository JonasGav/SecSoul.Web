using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecSoul.Model.Migrations
{
    public partial class AddHashCheckTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScanHashCheck",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScanRequestId = table.Column<int>(nullable: false),
                    Hash = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Location = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    MalwarePercentage = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ScanHashCheck_pk", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "ScanHashCheck_ScanRequest_Id_fk",
                        column: x => x.ScanRequestId,
                        principalTable: "ScanRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ScanHashCheck_Id_uindex",
                table: "ScanHashCheck",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScanHashCheck_ScanRequestId",
                table: "ScanHashCheck",
                column: "ScanRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScanHashCheck");
        }
    }
}
