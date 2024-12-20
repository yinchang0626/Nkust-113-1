using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

class Program
{
    class FacilityData
    {
        public string Period { get; set; }
        public int LongTermCareCount { get; set; }
        public int LongTermCareResidents { get; set; }
        public int NursingHomeCount { get; set; }
        public int NursingHomeResidents { get; set; }
        public int ElderlyHomeCount { get; set; }
        public int ElderlyHomeResidents { get; set; }
        public int DementiaCareCount { get; set; }
        public int DementiaCareResidents { get; set; }
        public int ElderlyApartmentCount { get; set; }
        public int ElderlyApartmentResidents { get; set; }
        public int ElderlyResidenceCount { get; set; }
        public int ElderlyResidenceResidents { get; set; }

        public int TotalFacilities =>
            LongTermCareCount + NursingHomeCount + ElderlyHomeCount +
            DementiaCareCount + ElderlyApartmentCount + ElderlyResidenceCount;

        public int TotalResidents =>
            LongTermCareResidents + NursingHomeResidents + ElderlyHomeResidents +
            DementiaCareResidents + ElderlyApartmentResidents + ElderlyResidenceResidents;
    }

    static void Main()
    {
        string csvPath = "a04004701-2102272956.csv"; // 請根據實際檔案路徑修改
        var data = ReadCsvData(csvPath);

        AnalyzeData(data);
        Console.ReadLine();
    }

    static List<FacilityData> ReadCsvData(string path)
    {
        var result = new List<FacilityData>();
        bool isFirstLine = true;

        foreach (string line in File.ReadAllLines(path))
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            var values = line.Split(',')
                           .Select(v => v.Trim('"'))
                           .ToArray();

            result.Add(new FacilityData
            {
                Period = values[0],
                LongTermCareCount = int.Parse(values[1]),
                LongTermCareResidents = int.Parse(values[2]),
                NursingHomeCount = int.Parse(values[3]),
                NursingHomeResidents = int.Parse(values[4]),
                ElderlyHomeCount = int.Parse(values[5]),
                ElderlyHomeResidents = int.Parse(values[6]),
                DementiaCareCount = int.Parse(values[7]),
                DementiaCareResidents = int.Parse(values[8]),
                ElderlyApartmentCount = int.Parse(values[9]),
                ElderlyApartmentResidents = int.Parse(values[10]),
                ElderlyResidenceCount = int.Parse(values[11]),
                ElderlyResidenceResidents = int.Parse(values[12])
            });
        }

        return result;
    }

    static void AnalyzeData(List<FacilityData> data)
    {
        Console.WriteLine("台北市長者照顧機構統計分析\n");

        // 分析最新月份數據
        var latest = data.Last();
        Console.WriteLine($"最新統計期間: {latest.Period}");
        Console.WriteLine("\n各類機構現況:");
        Console.WriteLine($"長期照護機構: {latest.LongTermCareCount}所，{latest.LongTermCareResidents}人");
        Console.WriteLine($"養護機構: {latest.NursingHomeCount}所，{latest.NursingHomeResidents}人");
        Console.WriteLine($"安養機構: {latest.ElderlyHomeCount}所，{latest.ElderlyHomeResidents}人");
        Console.WriteLine($"失智照顧型機構: {latest.DementiaCareCount}所，{latest.DementiaCareResidents}人");
        Console.WriteLine($"老人公寓: {latest.ElderlyApartmentCount}所，{latest.ElderlyApartmentResidents}人");
        Console.WriteLine($"老人住宅: {latest.ElderlyResidenceCount}所，{latest.ElderlyResidenceResidents}人");
        Console.WriteLine($"總計: {latest.TotalFacilities}所，{latest.TotalResidents}人\n");

        // 分析年度變化
        var years = data.Select(d => int.Parse(d.Period.Substring(0, 3)))
                       .Distinct()
                       .OrderBy(y => y)
                       .ToList();

        Console.WriteLine("年度變化分析:");
        foreach (var year in years)
        {
            var yearData = data.Where(d => d.Period.StartsWith($"{year}年"))
                              .ToList();
            if (yearData.Any())
            {
                var yearEnd = yearData.Last();
                Console.WriteLine($"\n{year + 1911}年底:");
                Console.WriteLine($"總機構數: {yearEnd.TotalFacilities}所");
                Console.WriteLine($"總進住人數: {yearEnd.TotalResidents}人");
            }
        }

        // 計算統計指標
        var totalResidentsTrend = data.Select(d => d.TotalResidents).ToList();
        Console.WriteLine("\n統計指標:");
        Console.WriteLine($"歷史最高進住人數: {totalResidentsTrend.Max()}人");
        Console.WriteLine($"歷史最低進住人數: {totalResidentsTrend.Min()}人");
        Console.WriteLine($"平均進住人數: {totalResidentsTrend.Average():F0}人");

        // 分析機構數變化趨勢
        var firstPeriod = data.First();
        Console.WriteLine("\n機構數變化趨勢(與初始期間比較):");
        Console.WriteLine($"長期照護機構: {firstPeriod.LongTermCareCount}所 → {latest.LongTermCareCount}所");
        Console.WriteLine($"養護機構: {firstPeriod.NursingHomeCount}所 → {latest.NursingHomeCount}所");
        Console.WriteLine($"安養機構: {firstPeriod.ElderlyHomeCount}所 → {latest.ElderlyHomeCount}所");
        Console.WriteLine($"失智照顧型機構: {firstPeriod.DementiaCareCount}所 → {latest.DementiaCareCount}所");
    }
}