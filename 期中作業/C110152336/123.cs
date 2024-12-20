
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public class CaseRecord
{
    public string 地區 { get; set; }
    public string 項目 { get; set; }
    public string 欄位名稱 { get; set; }
    public int 數值 { get; set; }
    public DateTime 資料時間日期 { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        // 1. 讀取 JSON 資料
        string filePath = "path_to_your_file.json";
        string jsonData = File.ReadAllText(filePath);
        List<CaseRecord> records = JsonConvert.DeserializeObject<List<CaseRecord>>(jsonData);

        // 2. 計算資料總量
        int totalRecords = records.Count;
        Console.WriteLine($"案件總數: {totalRecords}");

        // 3. 類別數據分布
        var categoryDistribution = records.GroupBy(r => r.欄位名稱)
                                          .Select(g => new { Category = g.Key, Count = g.Count() })
                                          .OrderByDescending(c => c.Count);
        
        Console.WriteLine("\n案件類別分布:");
        foreach (var category in categoryDistribution)
        {
            Console.WriteLine($"{category.Category}: {category.Count} 件");
        }

        // 4. 統計數據（數值的最大值、最小值、平均值）
        var maxValue = records.Max(r => r.數值);
        var minValue = records.Min(r => r.數值);
        var avgValue = records.Average(r => r.數值);

        Console.WriteLine($"\n案件數值統計:");
        Console.WriteLine($"最大值: {maxValue}");
        Console.WriteLine($"最小值: {minValue}");
        Console.WriteLine($"平均值: {avgValue:F2}");

        // 5. 時間趨勢分析（根據月份的數值變化趨勢）
        var monthlyTrend = records.GroupBy(r => r.資料時間日期.ToString("yyyy-MM"))
                                  .Select(g => new { Month = g.Key, TotalValue = g.Sum(r => r.數值) })
                                  .OrderBy(m => m.Month);

        Console.WriteLine("\n月份趨勢:");
        foreach (var month in monthlyTrend)
        {
            Console.WriteLine($"{month.Month}: {month.TotalValue} 件");
        }

        // 若需要進行圖形化展示，可以考慮使用簡單的 ASCII 輸出或第三方圖形庫
    }
}
