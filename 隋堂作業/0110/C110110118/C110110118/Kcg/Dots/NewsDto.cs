namespace Kcg.Dots
{
    public class NewsDto
    {
        public Guid NewsId { get; set; }

        public string Title { get; set; }

        public string DepartmentName { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public List<IFormFile> Files { get; set; }

        public string UpdateEmployeeName { get; set; }

        public int Click { get; set; }

        public bool Enable { get; set; }
    }
}
