using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nkust.Web.Entities
{
    public class PoliceOffice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string DisplayName { get; set; }


        public ICollection<PoliceStation>? PoliceStations { get; set; }

    }
}
