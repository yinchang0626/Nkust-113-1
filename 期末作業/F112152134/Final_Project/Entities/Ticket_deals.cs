using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using Microsoft.VisualBasic;

namespace Final_Project.Entities
{
    public class Ticket_deals
    {
        [Key]
        [Name("年分")] public string Year { get; set; } = null!;
        [Name("累計發卡量")] public int Cumulative_amount { get; set; }
        [Name("交易金額")] public int Deal_payment { get; set; }
        [Name("交易筆數-合計")] public int Quantity_total { get; set; }
        [Name("交易筆數-搭乘捷運")] public int Quantity_metro { get; set; }
        [Name("交易筆數-搭乘公車")] public int Quantity_bus { get; set; }
        [Name("交易筆數-撘乘渡輪")] public int Quantity_boat { get; set; }
    }
}
