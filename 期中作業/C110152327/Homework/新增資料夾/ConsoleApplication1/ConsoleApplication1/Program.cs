using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirQualityDataReader
{
    class Program
    {
        // 定義每筆空氣品質資料的類別
        public class AirQualityData
        {
            public string SiteName { get; set; }
            public string County { get; set; }
            public int Aqi { get; set; }
            public string Pollutant { get; set; }
            public string Status { get; set; }
            public double So2 { get; set; }
            public double Co { get; set; }
            public double O3 { get; set; }
            public double O3_8hr { get; set; }
            public double Pm10 { get; set; }
            public double Pm2_5 { get; set; }
            public double No2 { get; set; }
            public double Nox { get; set; }
            public double No { get; set; }
            public double WindSpeed { get; set; }
            public string WindDirec { get; set; }
            public DateTime PublishTime { get; set; }
            public double Co_8hr { get; set; }
            public double Pm2_5_Avg { get; set; }
            public double Pm10_Avg { get; set; }
            public double So2_Avg { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public string SiteId { get; set; }
        }

        static void Main(string[] args)
        {
            string filePath = "aqx_p_432.csv"; // CSV 檔案的路徑

            // 讀取 CSV 檔案
            List<AirQualityData> airQualityData = ReadCSV(filePath);

            if (airQualityData.Count == 0)
            {
                Console.WriteLine("沒有資料可供處理.");
                return;
            }

            // 顯示讀取到的資料（這裡僅顯示部分資料）
            /*Console.WriteLine("讀取到的資料:");
            foreach (var data in airQualityData.Take(5)) // 只顯示前 5 筆資料
            {
                Console.WriteLine($"{data.SiteName}, {data.County}, AQI: {data.Aqi}, Pollutant: {data.Pollutant}");
            }*/

            // 計算簡單統計
            double averageAqi = airQualityData.Average(d => d.Aqi);
            double averagePm10 = airQualityData.Average(d => d.Pm10);
            double averagePm2_5 = airQualityData.Average(d => d.Pm2_5);
            double averageSo2 = airQualityData.Average(d => d.So2);
            double averageCo = airQualityData.Average(d => d.Co);

            Console.WriteLine("\n統計資料:");
            Console.WriteLine($"平均 AQI: {averageAqi:F2}");
            Console.WriteLine($"平均 PM10: {averagePm10:F2}");
            Console.WriteLine($"平均 PM2.5: {averagePm2_5:F2}");
            Console.WriteLine($"平均 SO2: {averageSo2:F2}");
            Console.WriteLine($"平均 CO: {averageCo:F2}");

            // 顯示 AQI 最高的資料
            var highestAqiData = airQualityData.OrderByDescending(d => d.Aqi).First();
            Console.WriteLine($"\nAQI 最高的資料: {highestAqiData.SiteName}, {highestAqiData.County}, AQI: {highestAqiData.Aqi}");

            // 等待使用者按下任意鍵後才結束程式
            Console.WriteLine("\n按任意鍵以結束...");
            Console.ReadKey(); // 這會讓程式暫停，直到使用者按下任意鍵
        }

        static List<AirQualityData> ReadCSV(string filePath)
        {
            var airQualityData = new List<AirQualityData>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    bool isHeader = true;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (isHeader)
                        {
                            isHeader = false;
                            continue; // 跳過標題行
                        }

                        var values = line.Split(',');

                        if (values.Length == 24) // 確保資料列有正確的欄位數量
                        {
                            var data = new AirQualityData
                            {
                                SiteName = values[0],
                                County = values[1],
                                Aqi = TryParseInt(values[2]),
                                Pollutant = string.IsNullOrWhiteSpace(values[3]) ? null : values[3], // 處理空字串或 null
                                Status = values[4],
                                So2 = TryParseDouble(values[5]),
                                Co = TryParseDouble(values[6]),
                                O3 = TryParseDouble(values[7]),
                                O3_8hr = TryParseDouble(values[8]),
                                Pm10 = TryParseDouble(values[9]),
                                Pm2_5 = TryParseDouble(values[10]),
                                No2 = TryParseDouble(values[11]),
                                Nox = TryParseDouble(values[12]),
                                No = TryParseDouble(values[13]),
                                WindSpeed = TryParseDouble(values[14]),
                                WindDirec = values[15],
                                PublishTime = TryParseDateTime(values[16]),
                                Co_8hr = TryParseDouble(values[17]),
                                Pm2_5_Avg = TryParseDouble(values[18]),
                                Pm10_Avg = TryParseDouble(values[19]),
                                So2_Avg = TryParseDouble(values[20]),
                                Longitude = TryParseDouble(values[21]),
                                Latitude = TryParseDouble(values[22]),
                                SiteId = values[23]
                            };
                            airQualityData.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"錯誤: {ex.Message}");
            }

            return airQualityData;
        }

        // 嘗試解析 int 值，若無效則返回 0
        static int TryParseInt(string value)
        {
            int result;
            return int.TryParse(value, out result) ? result : 0;
        }

        static double TryParseDouble(string value)
        {
            double result;
            return double.TryParse(value, out result) ? result : 0.0;
        }

        static DateTime TryParseDateTime(string value)
        {
            DateTime result;
            return DateTime.TryParse(value, out result) ? result : DateTime.MinValue;
        }
    }
}
