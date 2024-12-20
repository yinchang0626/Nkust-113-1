using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class DataItem
{
    public string 地區 { get; set; }
    public string 項目 { get; set; }
    public string 欄位名稱 { get; set; }
    public int 數值 { get; set; }
    public DateTime 資料時間日期 { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // 讀取 JSON 文件
        string filePath = "D:\\c++\\2024\\Data\\Data\\123.json";
        var dataItems = JsonConvert.DeserializeObject<List<DataItem>>(File.ReadAllText(filePath));

        // 統計欄位名稱出現次數
        var fieldNameCounts = dataItems
            .GroupBy(d => d.欄位名稱)
            .Select(g => new { FieldName = g.Key, Count = g.Count() })
            .ToList();

        Console.WriteLine("欄位名稱出現次數:");
        foreach (var field in fieldNameCounts)
        {
            Console.WriteLine($"{field.FieldName}: {field.Count} 次");
        }

        // 找出最多和最少出現的欄位名稱
        var maxOccurrence = fieldNameCounts.OrderByDescending(f => f.Count).First();
        var minOccurrence = fieldNameCounts.OrderBy(f => f.Count).First();

        Console.WriteLine($"最多出現的欄位名稱: {maxOccurrence.FieldName}, 出現次數: {maxOccurrence.Count}");
        Console.WriteLine($"最少出現的欄位名稱: {minOccurrence.FieldName}, 出現次數: {minOccurrence.Count}");
    }
}
