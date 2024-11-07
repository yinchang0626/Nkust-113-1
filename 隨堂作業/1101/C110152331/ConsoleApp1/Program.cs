using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using CsvHelper.Configuration;

public class ElectionData
{
    public string 屆別 { get; set; }
    public int 男性 { get; set; }
    public int 女性 { get; set; }
}

class Program
{
    [STAThread]
    static void Main()
    {
        // 檔案路徑
        var path = @"C:\Users\fire4\source\repos\ConsoleApp1\1-7 桃園市(縣)議員當選人數.csv";

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
                    var men = group.Sum(r => r.男性);
                    var women = group.Sum(r => r.女性);
                    var all = men + women;

                    // 計算每一屆的男女比例
                    double menPercentage = all > 0 ? (double)men / all * 100 : 0;
                    double womenPercentage = all > 0 ? (double)women / all * 100 : 0;

                    // 顯示每一屆的結果
                    Console.WriteLine($"屆別: {year}");
                    Console.WriteLine($"總男性候選人數: {men}");
                    Console.WriteLine($"總女性候選人數: {women}");
                    Console.WriteLine($"總候選人數: {all}");
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
