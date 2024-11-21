using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string filePath = @"C:\Users\q2541\source\repos\ConsoleApp1\各類再生能源發電量.csv";

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                // 略過標題行
                sr.ReadLine();

                // 打印標題
                Console.WriteLine($"{"年份",-10} {"太陽光電 (百萬度)",-20} {"風力發電 (百萬度)",-20} {"水力發電 (百萬度)",-20} {"生質能 (百萬度)",-20} {"地熱 (百萬度)",-20} {"廢棄物 (百萬度)",-20} {"合計 (百萬度)",-20}");
                Console.WriteLine(new string('-', 160));

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(',');

                    Record record = new Record
                    {
                        Year = values[0],
                        SolarPower = ParseDouble(values[1]),
                        WindPower = ParseDouble(values[2]),
                        Hydropower = ParseDouble(values[3]),
                        Biomass = ParseDouble(values[4]),
                        Geothermal = ParseDouble(values[5]),
                        Waste = ParseDouble(values[6]),
                        Total = ParseDouble(values[7])
                    };

                    // 計算比率
                    double solarRatio = CalculatePercentage(record.SolarPower, record.Total);
                    double windRatio = CalculatePercentage(record.WindPower, record.Total);
                    double hydroRatio = CalculatePercentage(record.Hydropower, record.Total);
                    double biomassRatio = CalculatePercentage(record.Biomass, record.Total);
                    double geothermalRatio = CalculatePercentage(record.Geothermal, record.Total);
                    double wasteRatio = CalculatePercentage(record.Waste, record.Total);

                    // 打印發電量
                    Console.WriteLine($"{record.Year,-10} {record.SolarPower,-20:F2} {record.WindPower,-20:F2} {record.Hydropower,-20:F2} {record.Biomass,-20:F2} {record.Geothermal,-20:F2} {record.Waste,-20:F2} {record.Total,-20:F2}");

                    // 打印比率
                    Console.WriteLine($"{new string(' ', 10)} {solarRatio,-20:F2} {windRatio,-20:F2} {hydroRatio,-20:F2} {biomassRatio,-20:F2} {geothermalRatio,-20:F2} {wasteRatio,-20:F2}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("讀取檔案時出錯：" + e.Message);
        }
    }

    // 解析字符串為雙精度浮點數
    static double ParseDouble(string value)
    {
        return double.TryParse(value, out double result) ? result : 0;
    }

    // 計算比率
    static double CalculatePercentage(double part, double total)
    {
        return total > 0 ? (part / total) * 100 : 0;
    }
}

class Record
{
    public string Year { get; set; }
    public double SolarPower { get; set; }
    public double WindPower { get; set; }
    public double Hydropower { get; set; }
    public double Biomass { get; set; }
    public double Geothermal { get; set; }
    public double Waste { get; set; }
    public double Total { get; set; }
}
