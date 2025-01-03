using System.ComponentModel.DataAnnotations; //資料模型設計 (key)
namespace WebApplication1.Models
{
    public class Address
    {
        //Plain Old Class Object (POCO->SQL)
        [Key]
        //pribate int Id {get; set; }
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        //public int ZipCode { get; set; }    //區號
    }
}
