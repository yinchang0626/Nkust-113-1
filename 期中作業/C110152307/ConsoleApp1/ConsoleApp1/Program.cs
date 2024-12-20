using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class MonthlyData
{
    public int 年 { get; set; }
    public int 月 { get; set; }
    public int 總運量 { get; set; }
    public int 日均運量 { get; set; }
    public int 假日均運量 { get; set; }
    public double 月台上刷卡日均筆數 { get; set; }
    public double 車上刷卡日均筆數 { get; set; }
    public double 售票機日均筆數 { get; set; }
    public double 補票日均筆數 { get; set; }
    public double 團體票日均筆數 { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // 假設 JSON 資料存儲在檔案中
        string filePath = "data.json";

        if (File.Exists(filePath))
        {   
            // read file
            string jsonContent = File.ReadAllText(filePath);

            // 反序列化 JSON 資料為 MonthlyData 類型的集合
            List<MonthlyData> monthlyDataList = JsonConvert.DeserializeObject<List<MonthlyData>>(jsonContent);

            // 1. caculate sum
            int total運量 = monthlyDataList.Sum(x => x.總運量);
            int total日均運量 = monthlyDataList.Sum(x => x.日均運量);
            int total假日均運量 = monthlyDataList.Sum(x => x.假日均運量);
            double total月台上刷卡日均筆數 = monthlyDataList.Sum(x => x.月台上刷卡日均筆數);
            double total車上刷卡日均筆數 = monthlyDataList.Sum(x => x.車上刷卡日均筆數);
            double total售票機日均筆數 = monthlyDataList.Sum(x => x.售票機日均筆數);
            double total補票日均筆數 = monthlyDataList.Sum(x => x.補票日均筆數);
            double total團體票日均筆數 = monthlyDataList.Sum(x => x.團體票日均筆數);

            // 2. caculate max min average
            var max運量 = monthlyDataList.Max(x => x.總運量);
            var min運量 = monthlyDataList.Min(x => x.總運量);
            var avg運量 = monthlyDataList.Average(x => x.總運量);

            var max日均運量 = monthlyDataList.Max(x => x.日均運量);
            var min日均運量 = monthlyDataList.Min(x => x.日均運量);
            var avg日均運量 = monthlyDataList.Average(x => x.日均運量);

            var max月台上刷卡日均筆數 = monthlyDataList.Max(x => x.月台上刷卡日均筆數);
            var min月台上刷卡日均筆數 = monthlyDataList.Min(x => x.月台上刷卡日均筆數);
            var avg月台上刷卡日均筆數 = monthlyDataList.Average(x => x.月台上刷卡日均筆數);

            // 3. output
            Console.WriteLine("---- 總量 ----");
            Console.WriteLine($"總運量: {total運量}");
            Console.WriteLine($"總日均運量: {total日均運量}");
            Console.WriteLine($"總假日均運量: {total假日均運量}");
            Console.WriteLine($"總月台上刷卡日均筆數: {total月台上刷卡日均筆數:F1}");
            Console.WriteLine($"總車上刷卡日均筆數: {total車上刷卡日均筆數:F1}");
            Console.WriteLine($"總售票機日均筆數: {total售票機日均筆數:F1}");
            Console.WriteLine($"總補票日均筆數: {total補票日均筆數:F1}");
            Console.WriteLine($"總團體票日均筆數: {total團體票日均筆數:F1}");

            Console.WriteLine("\n---- 統計數據 ----");
            Console.WriteLine($"總運量 - 最大值: {max運量}, 最小值: {min運量}, 平均值: {avg運量:F1}");
            Console.WriteLine($"日均運量 - 最大值: {max日均運量}, 最小值: {min日均運量}, 平均值: {avg日均運量:F1}");
            Console.WriteLine($"月台上刷卡日均筆數 - 最大值: {max月台上刷卡日均筆數}, 最小值: {min月台上刷卡日均筆數}, 平均值: {avg月台上刷卡日均筆數:F1}");

            Console.WriteLine("\n---- 各年總量  ----");
            // 分年輸出每年的總運量
            var yearlySummary = monthlyDataList.GroupBy(x => x.年)
                                               .Select(g => new
                                               {
                                                   年 = g.Key,
                                                   總運量 = g.Sum(x => x.總運量),
                                                   平均日均運量 = g.Average(x => x.日均運量)
                                               });

            foreach (var yearData in yearlySummary)
            {
                Console.WriteLine($"年: {yearData.年}, 總運量: {yearData.總運量}, 平均日均運量: {yearData.平均日均運量:F1}");
            }
            Console.WriteLine("\n---- 年增長率 ----");
            var yearlyDataList = monthlyDataList
                                  .GroupBy(x => x.年)
                                  .OrderBy(g => g.Key)
                                  .Select(g => new
                                  {
                                      年 = g.Key,
                                      總運量 = g.Sum(x => x.總運量)
                                  }).ToList();

            for (int i = 1; i < yearlyDataList.Count; i++)
            {
                var currentYear = yearlyDataList[i];
                var previousYear = yearlyDataList[i - 1];
                double growthRate = ((currentYear.總運量 - previousYear.總運量) / (double)previousYear.總運量) * 100;
                Console.WriteLine($"從 {previousYear.年} 到 {currentYear.年} 的增長率: {growthRate:F2}%");
            }

        }
        else
        {
            Console.WriteLine("檔案未找到!");
        }
    }
}
