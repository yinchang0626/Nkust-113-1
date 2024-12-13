using System.ComponentModel.DataAnnotations.Schema;

namespace Nkust.Web.Entities
{
    public class PoliceOffice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string DisplayName { get; set; } = null!;

        public ICollection<PoliceStation> PoliceStations { get; set; } = null!;

    }
}
