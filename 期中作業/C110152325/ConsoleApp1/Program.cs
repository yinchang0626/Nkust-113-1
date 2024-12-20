using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    public class Elderlylivingalone
    {
        public string 序號 { get; set; }
        public string 區域別 { get; set; }
        public int 總計 { get; set; }
        public int 男 { get; set; }
        public int 女 { get; set; }
    }
    public class Root
    {
        public string contentType { get; set; }
        public bool isImage { get; set; }
        public int size { get; set; }
        public List<Elderlylivingalone> data { get; set; }
        public int errorCode { get; set; }
        public string id { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
    static async Task Main(string[] args)
    {
        string url = "https://soa.tainan.gov.tw/Api/Service/Get/e88b3e4a-6126-4a27-8f7b-d92ecae0f8e0";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var root = JsonConvert.DeserializeObject<Root>(jsonResponse);
                var centers = root.data;

                // 數據總計
                int totalCenters = centers.Count;
                int totalPeople = centers.Sum(c => c.總計);
                int totalMale = centers.Sum(c => c.男);
                int totalFeMale = centers.Sum(c => c.女);

                //算出各區域的最大值、最小值、平均值
                int maxTotal = centers.Max(c => c.總計);
                int minTotal = centers.Min(c => c.總計);
                double avgTotal = centers.Average(c => c.總計);

                var areaWithMaxTotal = centers.FirstOrDefault(c => c.總計 == maxTotal)?.區域別;
                var areaWithMinTotal = centers.FirstOrDefault(c => c.總計 == minTotal)?.區域別;

                // 輸出結果
                Console.WriteLine($"總資料數量：{totalCenters}");
                Console.WriteLine($"總人數：{totalPeople} (男：{totalMale}, 女：{totalFeMale})");
                Console.WriteLine($"各區域總計最大值：{maxTotal} ({areaWithMaxTotal})");
                Console.WriteLine($"各區域總計最小值：{minTotal} ({areaWithMinTotal})");
                Console.WriteLine($"各區域總計平均值：{avgTotal:F0}");
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
