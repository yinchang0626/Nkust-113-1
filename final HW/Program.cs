using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

public class ElectionData
{
    public int 民國年 { get; set; }
    public int 人 { get; set; }
    public int 男生 { get; set; }
    public double 男生比例 { get; set; }
    public int 女生 { get; set; }
    public double 女生比例 { get; set; }
}

class Program
{
    private static string FilePath = @"C:\Users\User\Desktop\image processing\OOP\bendun\ConsoleApp1\File_78654.csv";

    [STAThread]
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n選擇操作：");
            Console.WriteLine("1. 新增資料");
            Console.WriteLine("2. 修改資料");
            Console.WriteLine("3. 刪除資料");
            Console.WriteLine("4. 顯示所有資料");
            Console.WriteLine("5. 繪製長條圖");
            Console.WriteLine("6. 離開程式");
            Console.Write("請輸入指令編號：");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddDataCommand();
                    break;
                case "2":
                    UpdateDataCommand();
                    break;
                case "3":
                    DeleteDataCommand();
                    break;
                case "4":
                    DisplayAllData();
                    break;
                case "5":
                    DrawChart();
                    break;
                case "6":
                    Console.WriteLine("程式結束。");
                    return;
                default:
                    Console.WriteLine("無效的指令，請重新輸入。");
                    break;
            }
        }
    }

    // 新增資料指令
    private static void AddDataCommand()
    {
        Console.WriteLine("請輸入新增的資料（格式：民國年,人數,男生,男生比例,女生,女生比例）：");
        var input = Console.ReadLine();
        var parts = input.Split(',');

        if (parts.Length == 6 &&
            int.TryParse(parts[0], out int year) &&
            int.TryParse(parts[1], out int total) &&
            int.TryParse(parts[2], out int men) &&
            double.TryParse(parts[3], out double menRatio) &&
            int.TryParse(parts[4], out int women) &&
            double.TryParse(parts[5], out double womenRatio))
        {
            var newData = new ElectionData
            {
                民國年 = year,
                人 = total,
                男生 = men,
                男生比例 = menRatio,
                女生 = women,
                女生比例 = womenRatio
            };
            AddElectionData(newData);
        }
        else
        {
            Console.WriteLine("輸入格式錯誤，請重新操作。");
        }
    }

    // 修改資料指令
    private static void UpdateDataCommand()
    {
        Console.Write("請輸入要修改的民國年：");
        if (int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("請輸入修改後的資料（格式：人數,男生,男生比例,女生,女生比例）：");
            var input = Console.ReadLine();
            var parts = input.Split(',');

            if (parts.Length == 5 &&
                int.TryParse(parts[0], out int total) &&
                int.TryParse(parts[1], out int men) &&
                double.TryParse(parts[2], out double menRatio) &&
                int.TryParse(parts[3], out int women) &&
                double.TryParse(parts[4], out double womenRatio))
            {
                var updatedData = new ElectionData
                {
                    民國年 = year,
                    人 = total,
                    男生 = men,
                    男生比例 = menRatio,
                    女生 = women,
                    女生比例 = womenRatio
                };
                UpdateElectionData(year, updatedData);
            }
            else
            {
                Console.WriteLine("輸入格式錯誤，請重新操作。");
            }
        }
        else
        {
            Console.WriteLine("民國年格式錯誤，請重新操作。");
        }
    }

    // 刪除資料指令
    private static void DeleteDataCommand()
    {
        Console.Write("請輸入要刪除的民國年：");
        if (int.TryParse(Console.ReadLine(), out int year))
        {
            DeleteElectionData(year);
        }
        else
        {
            Console.WriteLine("民國年格式錯誤，請重新操作。");
        }
    }

    // 顯示所有資料
    private static void DisplayAllData()
    {
        var dataList = ReadAllDataFromCsv();
        foreach (var data in dataList)
        {
            Console.WriteLine($"年: {data.民國年}, 人數: {data.人}, 男生: {data.男生}, 男生比例: {data.男生比例}%, 女生: {data.女生}, 女生比例: {data.女生比例}%");
        }
    }

    // 繪製長條圖
    private static void DrawChart()
    {
        var dataList = ReadAllDataFromCsv();
        var totalMen = dataList.Sum(d => d.男生);
        var totalWomen = dataList.Sum(d => d.女生);
        var totalPeople = totalMen + totalWomen;

        var menPercentage = totalPeople > 0 ? (double)totalMen / totalPeople * 100 : 0;
        var womenPercentage = totalPeople > 0 ? (double)totalWomen / totalPeople * 100 : 0;

        Draw(menPercentage, womenPercentage);
    }

    // 讀取所有資料
    private static List<ElectionData> ReadAllDataFromCsv()
    {
        if (!File.Exists(FilePath))
        {
            Console.WriteLine("檔案不存在，將建立空白資料檔案。");
            return new List<ElectionData>();
        }

        using (var reader = new StreamReader(FilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<ElectionData>().ToList();
        }
    }

    // 寫入所有資料
    private static void WriteAllDataToCsv(List<ElectionData> dataList)
    {
        using (var writer = new StreamWriter(FilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(dataList);
        }
    }

    // 新增資料
    private static void AddElectionData(ElectionData newData)
    {
        var dataList = ReadAllDataFromCsv();
        dataList.Add(newData);
        WriteAllDataToCsv(dataList);
        Console.WriteLine("資料新增成功！");
    }

    // 修改資料
    private static void UpdateElectionData(int year, ElectionData updatedData)
    {
        var dataList = ReadAllDataFromCsv();
        var existingData = dataList.FirstOrDefault(data => data.民國年 == year);

        if (existingData != null)
        {
            dataList.Remove(existingData);
            dataList.Add(updatedData);
            WriteAllDataToCsv(dataList);
            Console.WriteLine("資料修改成功！");
        }
        else
        {
            Console.WriteLine($"未找到民國年 {year} 的資料，無法修改。");
        }
    }

    // 刪除資料
    private static void DeleteElectionData(int year)
    {
        var dataList = ReadAllDataFromCsv();
        var existingData = dataList.FirstOrDefault(data => data.民國年 == year);

        if (existingData != null)
        {
            dataList.Remove(existingData);
            WriteAllDataToCsv(dataList);
            Console.WriteLine($"民國年 {year} 的資料已刪除。");
        }
        else
        {
            Console.WriteLine($"未找到民國年 {year} 的資料，無法刪除。");
        }
    }

    // 繪製長條圖
    private static void Draw(double malePercentage, double femalePercentage)
    {
        var columnChart = new LiveCharts.Wpf.CartesianChart
        {
            Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "男性",
                    Values = new ChartValues<double> { malePercentage },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y:F2}%"
                },
                new ColumnSeries
                {
                    Title = "女性",
                    Values = new ChartValues<double> { femalePercentage },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y:F2}%"
                }
            },
            AxisX = new LiveCharts.Wpf.AxesCollection
            {
                new LiveCharts.Wpf.Axis
                {
                    Title = "性別",
                    Labels = new[] { "男性", "女性" }
                }
            },
            AxisY = new LiveCharts.Wpf.AxesCollection
            {
                new LiveCharts.Wpf.Axis
                {
                    Title = "比例 (%)",
                    LabelFormatter = value => $"{value:F2}%"
                }
            }
        };

        using (var form = new Form())
        using (var elementHost = new ElementHost
        {
            Dock = DockStyle.Fill,
            Child = columnChart
        })
        {
            form.Controls.Add(elementHost);
            Application.Run(form);
        }
    }
}
