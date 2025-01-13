namespace WebApplication1.Models
{
    public class StockData
    {
        public int Id { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityName { get; set; }
        public long TradeVolume { get; set; }
        public long TradeValue { get; set; } 
        public decimal OpeningPrice { get; set; }
        public decimal HighestPrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal ClosingPrice { get; set; }
        public decimal PriceDifference { get; set; }
        public int TradeCount { get; set; }
    }

}
