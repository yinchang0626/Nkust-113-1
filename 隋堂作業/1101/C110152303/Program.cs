using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using CsvHelper.Configuration;

public class SubsidyData
{
    public string 年別 { get; set; }
    public int 女性受領補助人數 { get; set; }
    public int 男性受領補助人數 { get; set; }
    public int 女性死亡受領補助人數 { get; set; }
    public int 男性死亡受領補助人數 { get; set; }
}

class Program
{
    [STAThread]
    static void Main()
    {
        // 檔案路徑
        var path = @"C:\Users\Admin\Downloads\a157-.csv"; // CSV 檔案路徑

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
                var relist = csv.GetRecords<SubsidyData>().ToList();

                // 逐年計算男女比例
                foreach (var data in relist)
                {
                    // 每年總的男性和女性數量
                    var totalWomen = data.女性受領補助人數 + data.女性死亡受領補助人數;
                    var totalMen = data.男性受領補助人數 + data.男性死亡受領補助人數;

                    // 總人數
                    var totalPeople = totalWomen + totalMen;

                    // 計算男女比例
                    double womenPercentage = totalPeople > 0 ? (double)totalWomen / totalPeople * 100 : 0;
                    double menPercentage = totalPeople > 0 ? (double)totalMen / totalPeople * 100 : 0;

                    // 顯示每年的結果
                    Console.WriteLine($"年別: {data.年別}");
                    Console.WriteLine($"女性受領補助人數: {data.女性受領補助人數}, 女性死亡受領補助人數: {data.女性死亡受領補助人數}");
                    Console.WriteLine($"男性受領補助人數: {data.男性受領補助人數}, 男性死亡受領補助人數: {data.男性死亡受領補助人數}");
                    Console.WriteLine($"總女性人數: {totalWomen}");
                    Console.WriteLine($"總男性人數: {totalMen}");
                    Console.WriteLine($"總人數: {totalPeople}");
                    Console.WriteLine($"女性比例: {womenPercentage:F2}%");
                    Console.WriteLine($"男性比例: {menPercentage:F2}%");
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
//var filePath = @"C:\Users\Admin\Downloads\a157-.csv"; // CSV 檔案路徑