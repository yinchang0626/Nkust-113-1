using CsvHelper.Configuration.Attributes;

namespace WindowsFormsApp1
{
    public class PopulationData
    {
        [Name("年別")] // 對應 CSV 檔案的標題
        public int Year { get; set; }

        [Name("全體－男性")]
        public double MaleTotal { get; set; }

        [Name("全體－女性")]
        public double FemaleTotal { get; set; }

        [Name("國中及以下－男性")]
        public double MaleJuniorHighAndBelow { get; set; }

        [Name("國中及以下－女性")]
        public double FemaleJuniorHighAndBelow { get; set; }

        [Name("高中高職－男性")]
        public double MaleSeniorHigh { get; set; }

        [Name("高中高職－女性")]
        public double FemaleSeniorHigh { get; set; }

        [Name("大專及以上－男性")]
        public double MaleCollegeAndAbove { get; set; }

        [Name("大專及以上－女性")]
        public double FemaleCollegeAndAbove { get; set; }
    }
}