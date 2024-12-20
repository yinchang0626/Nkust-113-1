using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nkust.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class _0001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cameras",
                newName: "PoliceOffice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PoliceOffice",
                table: "Cameras",
                newName: "Name");
        }
    }
}
