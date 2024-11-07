using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string filePath = @"E:\Homework\NPA_LineID.csv"; // CSV 檔案路徑
        var data = ReadCsv(filePath);

        // 只取前50筆資料
        var first50Records = data.Take(50).ToList();

        // 數據的總量
        int totalRecords = first50Records.Count;
        Console.WriteLine($"前50筆資料總量: {totalRecords} 筆");

        // 不同帳號的數據分布
        var accountDistribution = GetAccountDistribution(first50Records, 1); // 假設第二欄為帳號欄
        Console.WriteLine("\n前50筆資料中，不同帳號的數據分布:");
        foreach (var account in accountDistribution)
        {
            Console.WriteLine($"{account.Key}: {account.Value} 筆");
        }

        // 日期欄位的最早和最晚日期
        var dateRange = GetDateRange(first50Records, 2); // 假設第三欄為日期欄
        Console.WriteLine("\n前50筆資料中的日期範圍:");
        Console.WriteLine($"最早日期: {dateRange.minDate:yyyy-MM-dd}");
        Console.WriteLine($"最晚日期: {dateRange.maxDate:yyyy-MM-dd}");

        // 不同日期的帳號數量統計
        var accountCountByDate = GetAccountCountByDate(first50Records, 2); // 假設第三欄為日期欄
        Console.WriteLine("\n前50筆資料中，不同日期的帳號數量:");

        // 計算最大值、最小值、平均值
        var accountCounts = accountCountByDate.Values.ToList();
        var maxCount = accountCounts.Max();
        var minCount = accountCounts.Min();
        var avgCount = accountCounts.Average();

        Console.WriteLine("\n根據前50筆資料的帳號數量統計:");
        Console.WriteLine($"最大帳號數量: {maxCount} 筆");
        Console.WriteLine($"最小帳號數量: {minCount} 筆");
        Console.WriteLine($"平均帳號數量: {avgCount:F2} 筆");

        // 顯示每個日期的帳號數量
        foreach (var date in accountCountByDate)
        {
            Console.WriteLine($"{date.Key:yyyy-MM-dd}: {date.Value} 筆");
        }
    }

    static List<string[]> ReadCsv(string filePath)
    {
        var rows = new List<string[]>();

        try
        {
            using (var reader = new StreamReader(filePath))
            {
                // 忽略第一行標題
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(','); // 根據逗號分隔
                        rows.Add(values);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("讀取CSV文件時發生錯誤: " + ex.Message);
        }

        return rows;
    }

    static Dictionary<string, int> GetAccountDistribution(List<string[]> data, int accountIndex)
    {
        var accountCount = new Dictionary<string, int>();

        foreach (var row in data)
        {
            var account = row[accountIndex];
            if (accountCount.ContainsKey(account))
            {
                accountCount[account]++;
            }
            else
            {
                accountCount[account] = 1;
            }
        }

        return accountCount;
    }

    static (DateTime minDate, DateTime maxDate) GetDateRange(List<string[]> data, int dateIndex)
    {
        var dates = new List<DateTime>();

        foreach (var row in data)
        {
            if (DateTime.TryParse(row[dateIndex], out DateTime date))
            {
                dates.Add(date);
            }
        }

        if (dates.Count == 0)
        {
            return (DateTime.MinValue, DateTime.MinValue); // 若無有效日期數據，返回最小日期
        }

        DateTime minDate = dates.Min();
        DateTime maxDate = dates.Max();

        return (minDate, maxDate);
    }

    static Dictionary<DateTime, int> GetAccountCountByDate(List<string[]> data, int dateIndex)
    {
        var accountCountByDate = new Dictionary<DateTime, int>();

        foreach (var row in data)
        {
            if (DateTime.TryParse(row[dateIndex], out DateTime date))
            {
                if (accountCountByDate.ContainsKey(date))
                {
                    accountCountByDate[date]++;
                }
                else
                {
                    accountCountByDate[date] = 1;
                }
            }
        }

        return accountCountByDate;
    }
}
