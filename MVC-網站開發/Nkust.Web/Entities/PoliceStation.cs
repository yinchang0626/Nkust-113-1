using System.ComponentModel.DataAnnotations.Schema;

namespace Nkust.Web.Entities
{
    public class PoliceStation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PoliceOfficeId { get; set; }

        public PoliceOffice? PoliceOffice { get; set; }

        public string DisplayName { get; set; } = null!;

    }
}
