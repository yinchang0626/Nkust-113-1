using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

public class ElectionData
{
    public string 屆別 { get; set; }
    public string 性別 { get; set; }

    // 使用 Name 特性來映射 CSV 的列名
    [Name("里長候選人數[人]")]
    public int 里長候選人數 { get; set; }
}

class Program
{
    [STAThread]
    static void Main()
    {
        // 檔案路徑
        var path = @"C:\Users\simplelife\source\repos\ConsoleApp2\ConsoleApp2\gsa002yac.csv";

        // 檢查檔案是否存在
        if (!File.Exists(path))
        {
            Console.WriteLine("指定的檔案不存在。");
            return;
        }

        try
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // 讀取數據
                var relist = csv.GetRecords<ElectionData>().ToList();

                // 依照屆別分組並計算每屆的男女比例
                var groupedByYear = relist
                    .GroupBy(r => r.屆別)
                    .ToList();

                foreach (var group in groupedByYear)
                {
                    var year = group.Key;
                    var totalMen = group.Where(r => r.性別 == "男").Sum(r => r.里長候選人數);
                    var totalWomen = group.Where(r => r.性別 == "女").Sum(r => r.里長候選人數);
                    var totalCandidates = group.Where(r => r.性別 == "總計").Sum(r => r.里長候選人數);

                    // 計算男女比例
                    double menPercentage = totalCandidates > 0 ? (double)totalMen / totalCandidates * 100 : 0;
                    double womenPercentage = totalCandidates > 0 ? (double)totalWomen / totalCandidates * 100 : 0;

                    // 顯示每一屆的結果
                    Console.WriteLine($"屆別: {year}");
                    Console.WriteLine($"總候選人數: {totalCandidates}");
                    Console.WriteLine($"男性候選人數: {totalMen}");
                    Console.WriteLine($"女性候選人數: {totalWomen}");
                    Console.WriteLine($"男性比例: {menPercentage:F2}%");
                    Console.WriteLine($"女性比例: {womenPercentage:F2}%");
                    Console.WriteLine(); // 空行分隔
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("找不到檔案");
        }
        finally
        {
            Console.ReadLine();
        }
    }
}
