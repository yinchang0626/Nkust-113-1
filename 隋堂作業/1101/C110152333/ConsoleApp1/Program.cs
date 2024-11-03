using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

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
        var path = @"C:\Users\user\Desktop\C110152333\ConsoleApp1\1-3 桃園市(縣)長當選人性別.csv";

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
                
                var relist = csv.GetRecords<ElectionData>().ToList();// 讀取數據

                var Men = relist.Sum(r => r.男性);
                var Women = relist.Sum(r => r.女性);
                var ALL = Men + Women;

                Console.WriteLine($"總男性候選人數: {Men}");
                Console.WriteLine($"總女性候選人數: {Women}");
                Console.WriteLine($"總候選人數: {ALL}");

                // 計算比例
                double menpp = 0; double womenpp = 0;
                if (ALL > 0)
                {
                    menpp = (double)Men / ALL * 100;//男性百分比
                    womenpp = (double)Women / ALL * 100;//女性百分比

                    Console.WriteLine($"男性比例: {menpp:F2}%");
                    Console.WriteLine($"女性比例: {womenpp:F2}%");
                }

                
                Draw(menpp, womenpp);//圖表
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("找不到檔案");
        }
        finally
        {
            Console.ReadLine(); // 等待用戶按下回車
        }
    }

    private static void Draw(double malePercentage, double femalePercentage)//圖表程式
    {
        var pieChart = new LiveCharts.Wpf.PieChart
        {
            Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "男性",
                    Values = new ChartValues<double> { malePercentage },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y:F2}%"//小數點第二位
                },
                new PieSeries
                {
                    Title = "女性",
                    Values = new ChartValues<double> {  femalePercentage },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y:F2}%"//小數點第二位
                }
            }
        };

        using (var form = new Form())
        using (var elementHost = new ElementHost
        {
            Dock = DockStyle.Fill,
            Child = pieChart
        })
        {
            form.Controls.Add(elementHost);
            Application.Run(form);
        }
    }
}
