using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // 讀取 CSV 檔案路徑
        string filePath = "data.csv";

        // 讀取 CSV 檔案的每一行
        List<(string 屆別, int 男性, int 女性)> data = new List<(string, int, int)>();
        using (StreamReader sr = new StreamReader(filePath))
        {
            // 讀取標題行，假設第一行是標題
            string header = sr.ReadLine();
            Console.WriteLine("標題: " + header);

            // 讀取資料行
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] fields = line.Split(',');
                    if (fields.Length >= 3 && int.TryParse(fields[1], out int male) && int.TryParse(fields[2], out int female))
                    {
                        data.Add((fields[0], male, female));
                    }
                }
            }
        }

        // 總數據量
        int totalRecords = data.Count;
        Console.WriteLine("總數據量: " + totalRecords);

        // 各屆別的分布
        Console.WriteLine("各屆別分布:");
        foreach (var record in data)
        {
            Console.WriteLine($"{record.屆別}: 男性 {record.男性}, 女性 {record.女性}");
        }

        // 男性和女性的總和
        int totalMale = data.Sum(record => record.男性);
        int totalFemale = data.Sum(record => record.女性);
        Console.WriteLine($"男性總數: {totalMale}");
        Console.WriteLine($"女性總數: {totalFemale}");

        // 男性和女性的最大值、最小值和平均值
        int maxMale = data.Max(record => record.男性);
        int minMale = data.Min(record => record.男性);
        double avgMale = data.Average(record => record.男性);

        int maxFemale = data.Max(record => record.女性);
        int minFemale = data.Min(record => record.女性);
        double avgFemale = data.Average(record => record.女性);

        Console.WriteLine($"男性最大值: {maxMale}, 最小值: {minMale}, 平均值: {avgMale}");
        Console.WriteLine($"女性最大值: {maxFemale}, 最小值: {minFemale}, 平均值: {avgFemale}");
    }
}
