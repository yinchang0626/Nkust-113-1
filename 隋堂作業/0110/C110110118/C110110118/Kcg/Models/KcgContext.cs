using Kcg.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Kcg.Models
{
    public class KcgContext : DbContext
    {
        public KcgContext(DbContextOptions<KcgContext> options) : base(options)
        {
        }

        public DbSet<TOPMenu> TOPMenu { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<NewsFiles> NewsFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TOPMenu>().HasData(
                new TOPMenu { TOPMenuId = Guid.NewGuid(), Name = "網站導覽", Url = "a1", Icon = "Icon1", Orders = 1 },
                new TOPMenu { TOPMenuId = Guid.NewGuid(), Name = "回首頁", Url = "a2", Icon = "Icon2", Orders = 2 },
                new TOPMenu { TOPMenuId = Guid.NewGuid(), Name = "English", Url = "a3", Icon = "Icon3", Orders = 3 },
                new TOPMenu { TOPMenuId = Guid.NewGuid(), Name = "雙語詞彙", Url = "a4", Icon = "Icon4", Orders = 4 },
                new TOPMenu { TOPMenuId = Guid.NewGuid(), Name = "市長信箱", Url = "a5", Icon = "Icon5", Orders = 5 },
                new TOPMenu { TOPMenuId = Guid.NewGuid(), Name = "洽公導覽", Url = "a6", Icon = "Icon6", Orders = 6 }
                );

            modelBuilder.Entity<TOPMenu>(entity =>
            {
                entity.Property(e => e.TOPMenuId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Icon).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Orders).IsRequired();
            });

            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, DepartmentId = 1, Name = "王大明" },
                new Employee { EmployeeId = 2, DepartmentId = 2, Name = "林曉明" },
                new Employee { EmployeeId = 3, DepartmentId = 3, Name = "陳芸芸" }
                );

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, Name = "交通局" },
                new Department { DepartmentId = 2, Name = "工務局" },
                new Department { DepartmentId = 3, Name = "農業局" }
                );

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<News>().HasData(
                new News { NewsId = Guid.NewGuid(), DepartmentId = 2, InsertEmployeeId = 2, UpdateEmployeeId = 2, Enable = true, Click = 61, StartDateTime = DateTime.Parse("2024-07-22 00:00:00"), EndDateTime = DateTime.Parse("2025-07-22 00:00:00"), UpdateDateTime = DateTime.Parse("2024-07-22 15:00:00"), InsertDateTime = DateTime.Parse("2024-07-22 15:00:00"), Title = "楠梓火車站周邊人行道改善 提升通行舒適度", Contents = "楠梓火車站是楠梓與周邊地區的運輸門戶，鄰近工業區、加工出口區、高雄科技大學、高雄都會公園等，形成機能豐富的生活圈。為提升楠梓區域旅運與民生往來品質，市府工務局道路養護工程處針對車站周邊人行道進行改善，已陸續整新楠梓新路東側（建楠路至楠梓區公所）、建楠路（全線）人行道，楠梓新路西側（建楠路至惠民捷道）人行道改善也接續於今（113）年6月底進場施做。" },
                new News { NewsId = Guid.NewGuid(), DepartmentId = 2, InsertEmployeeId = 2, UpdateEmployeeId = 2, Enable = true, Click = 261, StartDateTime = DateTime.Parse("2024-06-25 00:00:00"), EndDateTime = DateTime.Parse("2025-06-25 00:00:00"), UpdateDateTime = DateTime.Parse("2024-07-22 15:00:00"), InsertDateTime = DateTime.Parse("2024-07-22 15:00:00"), Title = "高雄國際花卉市場周邊道路景觀整建 營造舒適綠空間", Contents = "翠亨路是前鎮、小港區的幹道之一，也是兩地居民往返高雄市區的重要道路，翠亨路早期原有台糖運送貨物鐵道，經市府工務局改造成自行車道後，逐步升級周邊街景，從日常的設施維護改善、綠美化到專案辦理鐵道意象改造設置觀機平台、花架綠廊與翠平公園旁閒置空地環境再造等，近期再接力完成翠亨南路（平和東路至崇安街）路面改善及高雄國際花卉市場南側空地景觀營造，以提升區域生活品質、優化高市迎賓門戶整體風貌。" },
                new News { NewsId = Guid.NewGuid(), DepartmentId = 1, InsertEmployeeId = 1, UpdateEmployeeId = 1, Enable = true, Click = 161, StartDateTime = DateTime.Parse("2024-05-27 00:00:00"), EndDateTime = DateTime.Parse("2025-05-27 00:00:00"), UpdateDateTime = DateTime.Parse("2024-07-22 15:00:00"), InsertDateTime = DateTime.Parse("2024-07-22 15:00:00"), Title = "夏日氣溫炎熱！交通局研擬調整號誌秒數，縮短停等時間。", Contents = "【高雄訊】夏日日頭赤炎炎，機車族在停等沒有綠蔭的路口紅燈時，不免會有為何不能縮短秒數，減少停等曝曬時間的想法。高雄市交通局表示，7月起逐步於三民區以及苓雅區部分路段試辦縮短號誌週期秒數，可減少紅燈停等時的陽光曝曬時間，交通局也會持續觀察車流紓解狀況，適時檢討調整。" },
                new News { NewsId = Guid.NewGuid(), DepartmentId = 1, InsertEmployeeId = 1, UpdateEmployeeId = 1, Enable = true, Click = 1561, StartDateTime = DateTime.Parse("2024-06-27 00:00:00"), EndDateTime = DateTime.Parse("2025-06-27 00:00:00"), UpdateDateTime = DateTime.Parse("2024-07-22 15:00:00"), InsertDateTime = DateTime.Parse("2024-07-22 15:00:00"), Title = "青年路家具街增設左轉車道及路面邊線，有效界定車流動線、安全再升級！", Contents = "【高雄訊】近日高雄青年路(復興-林森)如火如荼進行道路重鋪工程，有眼尖民眾發現標線繪製線條與之前不一樣，高雄市交通局特別說明，因施工範圍長，道路兩側寬度不同處，在施工過程中依實地狀況做適時調整，並重新分配車道空間，為了交通安全，這都是必要的工作。" },
                new News { NewsId = Guid.NewGuid(), DepartmentId = 3, InsertEmployeeId = 3, UpdateEmployeeId = 3, Enable = true, Click = 1161, StartDateTime = DateTime.Parse("2024-07-21 00:00:00"), EndDateTime = DateTime.Parse("2025-07-21 00:00:00"), UpdateDateTime = DateTime.Parse("2024-07-22 15:00:00"), InsertDateTime = DateTime.Parse("2024-07-22 15:00:00"), Title = "台灣好芭 進軍大馬 紅心芭樂魅力席捲吉隆坡", Contents = "高雄出產的新鮮紅心芭樂持續進入馬來西亞吉隆坡市場，今年度首次上架新開張的高端超市「Imby Greens」，該超市位於馬來西亞第二高樓、去年甫開幕的敦拉薩國際貿易中心（Tun Razak Exchange，簡稱「TRX」） 內，以販售世界各地頂級商品為主，高雄首選紅心芭樂除上架銷售外，還舉辦試吃推廣活動，受到當地消費者的熱烈迴響，直呼「又脆又好吃」，顯示高雄紅心芭樂不僅在品質上備受肯定，更展現了台灣農產品在國際市場上的競爭力和影響力。" }
                );

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.NewsId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Contents).IsRequired();
                entity.Property(e => e.Enable).HasDefaultValue(true);
                entity.Property(e => e.EndDateTime)
                    .HasColumnType("datetime");
                entity.Property(e => e.InsertDateTime)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.StartDateTime)
                    .HasColumnType("datetime");
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250);
                entity.Property(e => e.UpdateDateTime)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<NewsFiles>(entity =>
            {
                entity.Property(e => e.NewsFilesId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
                entity.Property(e => e.Path).IsRequired();
            });
        }
    }
}
