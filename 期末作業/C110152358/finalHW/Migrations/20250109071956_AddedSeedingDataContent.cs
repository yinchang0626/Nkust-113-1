using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace finalHW.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedingDataContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Datas",
                columns: new[] { "Id", "CompanyName", "Email", "FirstName", "Gender", "LastName" },
                values: new object[,]
                {
                    { 1, "PENTECH", "yunyaoteoh@gmail.com", "YAO", "Male", "YUN" },
                    { 2, "PENTECH", "yunsheng@gmail.com", "SHENG", "Male", "YUN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Datas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Datas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
