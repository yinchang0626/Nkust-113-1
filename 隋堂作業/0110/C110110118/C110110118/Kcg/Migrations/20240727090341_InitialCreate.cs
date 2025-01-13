using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kcg.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    EndDateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    InsertDateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    InsertEmployeeId = table.Column<int>(type: "int", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdateEmployeeId = table.Column<int>(type: "int", nullable: false),
                    Click = table.Column<int>(type: "int", nullable: false),
                    Enable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                });

            migrationBuilder.CreateTable(
                name: "NewsFiles",
                columns: table => new
                {
                    NewsFilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    NewsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsFiles", x => x.NewsFilesId);
                });

            migrationBuilder.CreateTable(
                name: "TOPMenu",
                columns: table => new
                {
                    TOPMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Orders = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOPMenu", x => x.TOPMenuId);
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentId", "Name" },
                values: new object[,]
                {
                    { 1, "交通局" },
                    { 2, "工務局" },
                    { 3, "農業局" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "DepartmentId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "王大明" },
                    { 2, 2, "林曉明" },
                    { 3, 3, "陳芸芸" }
                });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "NewsId", "Click", "Contents", "DepartmentId", "Enable", "EndDateTime", "InsertDateTime", "InsertEmployeeId", "StartDateTime", "Title", "UpdateDateTime", "UpdateEmployeeId" },
                values: new object[,]
                {
                    { new Guid("00de1a6a-77c9-4a20-b353-da9167c8f311"), 1161, "高雄出產的新鮮紅心芭樂持續進入馬來西亞吉隆坡市場，今年度首次上架新開張的高端超市「Imby Greens」，該超市位於馬來西亞第二高樓、去年甫開幕的敦拉薩國際貿易中心（Tun Razak Exchange，簡稱「TRX」） 內，以販售世界各地頂級商品為主，高雄首選紅心芭樂除上架銷售外，還舉辦試吃推廣活動，受到當地消費者的熱烈迴響，直呼「又脆又好吃」，顯示高雄紅心芭樂不僅在品質上備受肯定，更展現了台灣農產品在國際市場上的競爭力和影響力。", 3, true, new DateTime(2025, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "台灣好芭 進軍大馬 紅心芭樂魅力席捲吉隆坡", new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { new Guid("4d9b4c16-98de-40bc-9267-1f2e754bc2b8"), 261, "翠亨路是前鎮、小港區的幹道之一，也是兩地居民往返高雄市區的重要道路，翠亨路早期原有台糖運送貨物鐵道，經市府工務局改造成自行車道後，逐步升級周邊街景，從日常的設施維護改善、綠美化到專案辦理鐵道意象改造設置觀機平台、花架綠廊與翠平公園旁閒置空地環境再造等，近期再接力完成翠亨南路（平和東路至崇安街）路面改善及高雄國際花卉市場南側空地景觀營造，以提升區域生活品質、優化高市迎賓門戶整體風貌。", 2, true, new DateTime(2025, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "高雄國際花卉市場周邊道路景觀整建 營造舒適綠空間", new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { new Guid("a780e4d9-7742-4736-a395-340929e2dfa9"), 161, "【高雄訊】夏日日頭赤炎炎，機車族在停等沒有綠蔭的路口紅燈時，不免會有為何不能縮短秒數，減少停等曝曬時間的想法。高雄市交通局表示，7月起逐步於三民區以及苓雅區部分路段試辦縮短號誌週期秒數，可減少紅燈停等時的陽光曝曬時間，交通局也會持續觀察車流紓解狀況，適時檢討調整。", 1, true, new DateTime(2025, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "夏日氣溫炎熱！交通局研擬調整號誌秒數，縮短停等時間。", new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { new Guid("ac18009b-b2e7-4bec-bc0a-8cc301a81fe9"), 61, "楠梓火車站是楠梓與周邊地區的運輸門戶，鄰近工業區、加工出口區、高雄科技大學、高雄都會公園等，形成機能豐富的生活圈。為提升楠梓區域旅運與民生往來品質，市府工務局道路養護工程處針對車站周邊人行道進行改善，已陸續整新楠梓新路東側（建楠路至楠梓區公所）、建楠路（全線）人行道，楠梓新路西側（建楠路至惠民捷道）人行道改善也接續於今（113）年6月底進場施做。", 2, true, new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "楠梓火車站周邊人行道改善 提升通行舒適度", new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { new Guid("b3bff517-d479-4c2b-84e8-a43e541a02b3"), 1561, "【高雄訊】近日高雄青年路(復興-林森)如火如荼進行道路重鋪工程，有眼尖民眾發現標線繪製線條與之前不一樣，高雄市交通局特別說明，因施工範圍長，道路兩側寬度不同處，在施工過程中依實地狀況做適時調整，並重新分配車道空間，為了交通安全，這都是必要的工作。", 1, true, new DateTime(2025, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "青年路家具街增設左轉車道及路面邊線，有效界定車流動線、安全再升級！", new DateTime(2024, 7, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "TOPMenu",
                columns: new[] { "TOPMenuId", "Icon", "Name", "Orders", "Url" },
                values: new object[,]
                {
                    { new Guid("1d979111-390d-4d28-9fd7-76a860c19f3a"), "Icon5", "市長信箱", 5, "a5" },
                    { new Guid("3572b1bd-5c1d-436d-85cf-9124abe67a0c"), "Icon4", "雙語詞彙", 4, "a4" },
                    { new Guid("44f4916c-f792-4ba2-bdfa-cfd6c24b2929"), "Icon6", "洽公導覽", 6, "a6" },
                    { new Guid("6a068953-3822-44d9-ba7d-03676b305a02"), "Icon3", "English", 3, "a3" },
                    { new Guid("909d9a18-7330-4f74-a0ab-59c8d7c9a6d9"), "Icon2", "回首頁", 2, "a2" },
                    { new Guid("9268d41f-6c5c-4bcb-a306-260e802ab1b0"), "Icon1", "網站導覽", 1, "a1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "NewsFiles");

            migrationBuilder.DropTable(
                name: "TOPMenu");
        }
    }
}
