using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Factory
{
    public string 類別 { get; set; }
    public string 館舍名稱 { get; set; }
    public string 公司名稱 { get; set; }
    public string 連繫電話 { get; set; }
    public string 郵遞區號 { get; set; }
    public string 住址 { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        // 設定CSV檔案路徑
        string csvFilePath = "C:\\Users\\Win10\\Desktop\\test\\Calculator\\觀光工廠及產業文化館名冊(2022).csv";

        // 讀取並解析CSV檔案
        var factories = ReadCsvFile(csvFilePath);

        // 顯示數據總量
        Console.WriteLine($"總計紀錄數: {factories.Count}");

        // 顯示每個館舍的名稱、公司名稱及電話
        Console.WriteLine("\n工廠與產業文化館列表:");
        foreach (var factory in factories)
        {
            Console.WriteLine($"館舍名稱: {factory.館舍名稱}, 公司名稱: {factory.公司名稱}, 連繫電話: {factory.連繫電話}");
        }

        // 顯示不同類別的數量
        var categoryCounts = factories.GroupBy(f => f.類別)
                                      .Select(g => new { 類別 = g.Key, Count = g.Count() })
                                      .OrderBy(g => g.Count)
                                      .ToList();

        Console.WriteLine("\n按類別統計的工廠/文化館數量:");
        foreach (var category in categoryCounts)
        {
            Console.WriteLine($"{category.類別}: {category.Count} 件");
        }

        // 顯示不同地區（郵遞區號）的數量
        var areaCounts = factories.GroupBy(f => f.郵遞區號)
                                  .Select(g => new { 郵遞區號 = g.Key, Count = g.Count() })
                                  .OrderBy(g => g.Count)
                                  .ToList();

        Console.WriteLine("\n按郵遞區號統計的工廠/文化館數量:");
        foreach (var area in areaCounts)
        {
            Console.WriteLine($"{area.郵遞區號}: {area.Count} 件");
        }

        // 計算郵遞區號統計的最大值、最小值和平均值
        int maxAreaCount = areaCounts.Max(a => a.Count);
        int minAreaCount = areaCounts.Min(a => a.Count);
        double avgAreaCount = areaCounts.Average(a => a.Count);

        // 找出最大和最小區域
        var maxArea = areaCounts.First(a => a.Count == maxAreaCount);
        var minArea = areaCounts.First(a => a.Count == minAreaCount);

        // 顯示統計數據
        Console.WriteLine($"\n最大區域工廠數量: {maxAreaCount} 件, 郵遞區號: {maxArea.郵遞區號}");
        Console.WriteLine($"最小區域工廠數量: {minAreaCount} 件, 郵遞區號: {minArea.郵遞區號}");
        Console.WriteLine($"區域工廠數量平均值: {avgAreaCount:F2} 件");
    }

    static List<Factory> ReadCsvFile(string filePath)
    {
        var records = new List<Factory>();

        try
        {
            using (var reader = new StreamReader(filePath))
            {
                string line;
                // 跳過標題行
                reader.ReadLine();

                // 讀取每一行數據並解析
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');

                    // 檢查是否有正確的欄位數
                    if (values.Length == 6)
                    {
                        var factory = new Factory
                        {
                            類別 = values[0],              // 解析 類別
                            館舍名稱 = values[1],          // 解析 館舍名稱
                            公司名稱 = values[2],          // 解析 公司名稱
                            連繫電話 = values[3],          // 解析 連繫電話
                            郵遞區號 = values[4],          // 解析 郵遞區號
                            住址 = values[5]               // 解析 住址
                        };

                        records.Add(factory);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"讀取CSV檔案時出錯: {ex.Message}");
        }

        return records;
    }
}