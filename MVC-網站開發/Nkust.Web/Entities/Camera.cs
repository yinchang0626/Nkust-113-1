using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nkust.Web.Entities
{
    public class Camera
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("no")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("警編")]

        public string? Number { get; set; } = null!;

        [JsonPropertyName("分局")]

        public string PoliceOffice { get; set; } = null!;
        [JsonPropertyName("派出所")]

        public string PoliceStation { get; set; } = null!;

        public int PoliceStationId { get; set; }

        public int PoliceOfficeId { get; set; }

        [JsonPropertyName("位置")]

        public string? Location { get; set; } = null!;

        //        "分局": "新興分局",
        //"派出所": "中山所",
        //"警編": "KC98A0015",
        //"位置": "中山一路與中正四路美麗島站(東側)向西(104維)"

    }
}
