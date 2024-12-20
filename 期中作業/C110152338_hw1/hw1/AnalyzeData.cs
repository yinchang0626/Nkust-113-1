using System;

namespace hw1
{
    public class AnalyzeData
    {
        public int totalRecords { get; set; }
        public int totalMale { get; set; }
        public int totalFemale { get; set; }
        public int maxMale { get; set; }
        public int minMale { get; set; }
        public int maxFemale { get; set; }
        public int minFemale { get; set; }
        public double avgMale { get; set; }
        public double avgFemale { get; set; }

        // 建構函數
        public AnalyzeData()
        {
            // 預設值
            totalRecords = 0;
            totalMale = 0;
            totalFemale = 0;
            maxMale = int.MinValue;
            minMale = int.MaxValue;
            maxFemale = int.MinValue;
            minFemale = int.MaxValue;
            avgMale = 0.0;
            avgFemale = 0.0;
        }
        ~AnalyzeData()
        {

        }
    }
}
