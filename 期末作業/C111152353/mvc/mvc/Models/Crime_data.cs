using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace mvc.Models
{
    public class Crime_data
    {
        [Key]
        public int Id { get; set; }
        public string? OwnerID { get; set; }
        [Display(Name = "案類")]
        public String Type { get; set; }
        [Display(Name = "發生年度")]
        public int Year { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "發生日期")]
        public DateTime Date { get; set; }
        [Display(Name = "發生縣市")]
        public String Country { get; set; }
        [Display(Name = "發生鄉鎮市區")]
        public String Region { get; set; }
    }
}
