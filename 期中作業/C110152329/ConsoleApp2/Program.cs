using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DataGeneration
{
    public class Statistic
    {
        public string 地區 { get; set; }
        public string 項目 { get; set; }
        public string 欄位名稱 { get; set; }
        public int 數值 { get; set; }
        public DateTime 資料時間日期 { get; set; }
        public string 資料週期 { get; set; }
        public string 郵遞區號 { get; set; }
        public string 機關代碼 { get; set; }
        public string 電子郵件 { get; set; }
        public string 行動電話 { get; set; }
        public string 市話 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 指定檔案路徑
            string filePath = @"C:/Users/User/source/repos/ConsoleApp2/2024_06_10720-06-03-2_臺中市替代役役男家屬安家費及三節生活扶助金統計.json";

            // 讀取JSON檔案並解析為Statistic物件列表
            List<Statistic> statistics = JsonConvert.DeserializeObject<List<Statistic>>(File.ReadAllText(filePath));

            // 根據地區統計人數
            var areaCounts = statistics
                .GroupBy(stat => stat.地區)
                .Select(group => new
                {
                    Area = group.Key,
                    Count = group.Count()
                });

            // 輸出每個地區的人數
            foreach (var areaGroup in areaCounts)
            {
                Console.WriteLine($"地區: {areaGroup.Area}, 人數: {areaGroup.Count}");
            }
        }
    }
}
