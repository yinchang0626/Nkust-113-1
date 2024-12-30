using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class ListClubByStateViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public bool NoClubWarning { get; set; } = false;
        public string State { get; set; }
    }
}
