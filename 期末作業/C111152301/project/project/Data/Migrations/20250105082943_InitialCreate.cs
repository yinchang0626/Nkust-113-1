using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sheep",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Num = table.Column<int>(type: "int", nullable: false),
                    AvgWeight = table.Column<int>(type: "int", nullable: false),
                    HighestPrice = table.Column<int>(type: "int", nullable: false),
                    AvgPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sheep", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sheep");
        }
    }
}
