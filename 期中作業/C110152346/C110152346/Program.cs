using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Data
{
    public string Area { get; set; }
    public int PaidVisitors { get; set; }
    public int FreeVisitors { get; set; }
    public int HolidayVisitors { get; set; }
    public int WeekdayVisitors { get; set; }
    public int TicketRevenue { get; set; }
    public int LastYearVisitors { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string filePath = "C:/Users/tt266/Desktop/git/test.csv"; // 檔案路徑(.csv)
        List<Data> table = new List<Data>();

        // 讀取 CSV 文件
        using (var reader = new StreamReader(filePath))
        {
            // 跳過標題行
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                // 將每行數據進行分析，處理不是數字的情況，將其轉為 0
                int paidVisitors = values[1] == " - " ? 0 : int.Parse(values[1]);
                int freeVisitors = values[2] == " - " ? 0 : int.Parse(values[2]);
                int holidayVisitors = values[3] == " - " ? 0 : int.Parse(values[3]);
                int weekdayVisitors = values[4] == " - " ? 0 : int.Parse(values[4]);
                int ticketRevenue = values[5] == " - " ? 0 : int.Parse(values[5]);
                int lastYearVisitors = (values[6] == " - " || values[6] == "休館") ? 0 : int.Parse(values[6]);

                table.Add(new Data
                {
                    Area = values[0],
                    PaidVisitors = paidVisitors,
                    FreeVisitors = freeVisitors,
                    HolidayVisitors = holidayVisitors,
                    WeekdayVisitors = weekdayVisitors,
                    TicketRevenue = ticketRevenue,
                    LastYearVisitors = lastYearVisitors
                });
            }
        }

        // 分析數據
        int totalPaidVisitors = table.Sum(r => r.PaidVisitors);
        int totalFreeVisitors = table.Sum(r => r.FreeVisitors);
        int totalHolidayVisitors = table.Sum(r => r.HolidayVisitors);
        int totalWeekdayVisitors = table.Sum(r => r.WeekdayVisitors);
        int maxRevenue = table.Max(r => r.TicketRevenue);
        int minRevenue = table.Min(r => r.TicketRevenue);
        double averageRevenue = table.Average(r => r.TicketRevenue);

        Console.WriteLine($"總遊客人次（有門票）: {totalPaidVisitors} 人");
        Console.WriteLine($"總遊客人次（無門票）: {totalFreeVisitors} 人");
        Console.WriteLine($"總遊客人次（假日）: {totalHolidayVisitors} 人");
        Console.WriteLine($"總遊客人次（非假日）: {totalWeekdayVisitors} 人");
        Console.WriteLine($"門票收入最大值: {maxRevenue} TWD");
        Console.WriteLine($"門票收入最小值: {minRevenue} TWD");
        Console.WriteLine($"門票收入平均值: {averageRevenue:F2} TWD");

        // 類別分布（不同區域的遊客人次）
        Console.WriteLine("\n各區域遊客人次分布:");
        foreach (var ALLdata in table)
        {
            Console.WriteLine($"{ALLdata.Area}: {ALLdata.PaidVisitors + ALLdata.FreeVisitors} 人");
        }
    }
}
