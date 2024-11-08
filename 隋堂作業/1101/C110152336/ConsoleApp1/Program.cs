using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    public class ChildCareCenter
    {
        public string 編號 { get; set; }
        public string 資源彙整機關 { get; set; }
        public string 辦理單位 { get; set; }
        public string 成立時間 { get; set; }
        public string 電話 { get; set; }
        public string 電子郵件 { get; set; }
        public string 地址 { get; set; }
        public string 網址 { get; set; }
        public string 核定收托人數 { get; set; }
        public string 最後更新時間 { get; set; }
    }

    static async Task Main(string[] args)
    {
        string url = "https://ws.hsinchu.gov.tw/001/Upload/1/opendata/8774/987/dfe9a1e9-4579-41a9-a7ea-5e3e30abfe45.json";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                // 發送 GET 請求
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // 確保請求成功

                // 讀取響應內容
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // 解析 JSON 數據
                List<ChildCareCenter> centers = JsonConvert.DeserializeObject<List<ChildCareCenter>>(jsonResponse);

                // 數據的總量
                int totalCenters = centers.Count;

                // 不同類別的數據分布
                var uniqueAgencies = centers.Select(c => c.資源彙整機關).Distinct().ToList();
                var uniqueUnits = centers.Select(c => c.辦理單位).Distinct().ToList();

                // 成立時間範圍
                var establishedDates = centers.Select(c => DateTime.Parse(c.成立時間)).ToList();
                DateTime minEstablishmentDate = establishedDates.Min();
                DateTime maxEstablishmentDate = establishedDates.Max();

                // 核定收托人數（數字）
                var approvedCounts = centers.Select(c => int.Parse(c.核定收托人數)).ToList();
                int maxCount = approvedCounts.Max();
                int minCount = approvedCounts.Min();
                double averageCount = approvedCounts.Average();

                // 電話和電子郵件的缺失計算
                int missingPhoneCount = centers.Count(c => string.IsNullOrEmpty(c.電話));
                int missingEmailCount = centers.Count(c => string.IsNullOrEmpty(c.電子郵件));

                // 輸出結果
                Console.WriteLine("總數量: " + totalCenters);
                Console.WriteLine("資源彙整機關: " + string.Join(", ", uniqueAgencies));
                Console.WriteLine("辦理單位: " + string.Join(", ", uniqueUnits));
                Console.WriteLine("成立時間範圍: " + minEstablishmentDate.ToString("yyyy/MM/dd") + " 到 " + maxEstablishmentDate.ToString("yyyy/MM/dd"));
                Console.WriteLine("核定收托人數 - 最大值: " + maxCount);
                Console.WriteLine("核定收托人數 - 最小值: " + minCount);
                Console.WriteLine("核定收托人數 - 平均值: " + averageCount);
                Console.WriteLine("電話信息缺失: " + missingPhoneCount);
                Console.WriteLine("電子郵件缺失: " + missingEmailCount);
                Console.WriteLine("最後更新時間: " + centers.First().最後更新時間);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"請求錯誤: {e.Message}");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"數據格式錯誤: {e.Message}");
            }
        }
    }
}
