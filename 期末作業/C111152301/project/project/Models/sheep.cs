using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Sheep
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "日期")]
        public DateTime Date { get; set; }
        [Display(Name = "地區")]
        public required string Area { get; set; }
        [Display(Name = "市場名稱")]
        public required string MarketName { get; set; }
        [Display(Name = "羊隻種類")]
        public required string ProductName { get; set; }
        [Display(Name = "售出數量")]
        public int Num { get; set; }
        [Display(Name = "平均重量")]
        public int AvgWeight { get; set; }
        [Display(Name = "最高價格")]
        public int HighestPrice { get; set; }
        [Display(Name = "平均價格")]
        public int AvgPrice { get; set; }
    }
}
