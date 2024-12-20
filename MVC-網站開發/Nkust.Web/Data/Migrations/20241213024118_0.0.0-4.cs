using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nkust.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class _0004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PoliceOfficeId",
                table: "Cameras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PoliceStationId",
                table: "Cameras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PoliceOffices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceOffices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliceStation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PoliceOfficeId = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliceStation_PoliceOffices_PoliceOfficeId",
                        column: x => x.PoliceOfficeId,
                        principalTable: "PoliceOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PoliceStation_PoliceOfficeId",
                table: "PoliceStation",
                column: "PoliceOfficeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoliceStation");

            migrationBuilder.DropTable(
                name: "PoliceOffices");

            migrationBuilder.DropColumn(
                name: "PoliceOfficeId",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "PoliceStationId",
                table: "Cameras");
        }
    }
}
