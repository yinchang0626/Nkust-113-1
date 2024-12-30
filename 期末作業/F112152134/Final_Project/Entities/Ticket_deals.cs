using Microsoft.VisualBasic;

namespace Final_Project.Entities
{
    public class Ticket_deals
    {
        public string Year { get; set; } = null!;
        public int Cumulative_amount { get; set; }
        public int Deal_payment { get; set; }
        public int Quantity_total { get; set; }
        public int Quantity_metro { get; set; }
        public int Quantity_bus { get; set; }
        public int Quantity_boat { get; set; }
    }
}
