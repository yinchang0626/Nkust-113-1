using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        string url = "https://data.moenv.gov.tw/api/v2/aqx_p_432?api_key=e8dd42e6-9b8b-43f8-991e-b3dee723a52d&limit=1000&sort=ImportDate%20desc&format=JSON";
        List<AQIRecord> records = new List<AQIRecord>();

        try
        {
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("正在下載資料...");

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    // 解析 JSON
                    var data = JsonConvert.DeserializeObject<RootObject>(jsonContent);
                    records = data.records;

                    Console.WriteLine("資料下載成功！\n");
                    ShowMenu(records);
                }
                else
                {
                    Console.WriteLine($"下載失敗，狀態碼: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"發生錯誤: {ex.Message}");
        }
    }

    // 功能選單
    static void ShowMenu(List<AQIRecord> records)
    {
        while (true)
        {
            Console.WriteLine("\n請選擇功能：");
            Console.WriteLine("1. 查看所有站點 AQI 資料");
            Console.WriteLine("2. 計算 AQI 平均值、最大值、最小值");
            Console.WriteLine("3. 查詢特定站點 AQI");
            Console.WriteLine("4. 過濾 AQI 超過指定數值的站點");
            Console.WriteLine("5. 結束程式");

            Console.Write("輸入選項 (1-5): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllAQI(records);
                    break;
                case "2":
                    AnalyzeAQI(records);
                    break;
                case "3":
                    SearchSiteAQI(records);
                    break;
                case "4":
                    FilterAQI(records);
                    break;
                case "5":
                    Console.WriteLine("程式結束。再見！");
                    return;
                default:
                    Console.WriteLine("無效選項，請重新輸入！");
                    break;
            }
        }
    }

    // 功能 1: 查看所有站點 AQI 資料
    static void DisplayAllAQI(List<AQIRecord> records)
    {
        Console.WriteLine("\n所有站點 AQI 資料：");
        foreach (var record in records)
        {
            Console.WriteLine($"站點: {record.sitename}, AQI: {record.aqi}, 時間: {record.importdate}");
        }
    }

    // 功能 2: 分析 AQI 資料
    static void AnalyzeAQI(List<AQIRecord> records)
    {
        var validAQI = records.Where(r => int.TryParse(r.aqi, out _)).Select(r => int.Parse(r.aqi)).ToList();

        if (validAQI.Count > 0)
        {
            Console.WriteLine($"\nAQI 平均值: {validAQI.Average():F2}");
            Console.WriteLine($"AQI 最大值: {validAQI.Max()}");
            Console.WriteLine($"AQI 最小值: {validAQI.Min()}");
        }
        else
        {
            Console.WriteLine("無有效的 AQI 資料可供分析。");
        }
    }

    // 功能 3: 查詢特定站點 AQI
    static void SearchSiteAQI(List<AQIRecord> records)
    {
        Console.Write("\n請輸入站點名稱: ");
        string siteName = Console.ReadLine();

        var result = records.Where(r => r.sitename.Contains(siteName)).ToList();

        if (result.Any())
        {
            foreach (var record in result)
            {
                Console.WriteLine($"站點: {record.sitename}, AQI: {record.aqi}, 時間: {record.importdate}");
            }
        }
        else
        {
            Console.WriteLine("找不到該站點的 AQI 資料。");
        }
    }

    // 功能 4: 過濾 AQI 超過指定數值的站點
    static void FilterAQI(List<AQIRecord> records)
    {
        Console.Write("\n請輸入 AQI 閾值: ");
        if (int.TryParse(Console.ReadLine(), out int threshold))
        {
            var result = records.Where(r => int.TryParse(r.aqi, out int aqiValue) && aqiValue > threshold).ToList();

            if (result.Any())
            {
                Console.WriteLine($"AQI 超過 {threshold} 的站點：");
                foreach (var record in result)
                {
                    Console.WriteLine($"站點: {record.sitename}, AQI: {record.aqi}");
                }
            }
            else
            {
                Console.WriteLine("沒有站點的 AQI 超過該數值。");
            }
        }
        else
        {
            Console.WriteLine("無效的數字輸入。");
        }
    }
}

// JSON 資料模型
public class AQIRecord
{
    public string siteid { get; set; }
    public string sitename { get; set; }
    public string aqi { get; set; }
    public string importdate { get; set; }
}

public class RootObject
{
    public List<AQIRecord> records { get; set; }
}
