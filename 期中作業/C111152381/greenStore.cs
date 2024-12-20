using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace greenStore
{
    // 定義 JSON 檔案中每筆紀錄的結構
    public class Record
    {
        public string Classtype { get; set; }
        public string Flagno { get; set; }
        public string Storeno { get; set; }
        public string Storename { get; set; }
        public string Undertaker { get; set; }
        public string Storeaddr { get; set; }
        public string Contacttel { get; set; }
        public string Taxno { get; set; }
    }

    public class Root
    {
        public List<Record> Records { get; set; }
    }

    class Program
    {
        static void Main()
        {
            // JSON 檔案路徑
            string jsonPath = "/Users/linjie/Desktop/C#/C111152381/bin/Debug/net8.0/gp_p_01.json";

            try
            {
                // 讀取並解析 JSON 檔案到 Root 物件
                var root = JsonConvert.DeserializeObject<Root>(File.ReadAllText(jsonPath));

                // 顯示每筆紀錄的詳細資料
                foreach (var record in root.Records)
                {
                    Console.WriteLine($"類別: {record.Classtype}");
                    Console.WriteLine($"旗標號: {record.Flagno}");
                    Console.WriteLine($"商店編號: {record.Storeno}");
                    Console.WriteLine($"商店名稱: {record.Storename}");
                    Console.WriteLine($"負責人: {record.Undertaker}");
                    Console.WriteLine($"地址: {record.Storeaddr}");
                    Console.WriteLine($"聯絡電話: {record.Contacttel}");
                    Console.WriteLine($"統一編號: {record.Taxno}");
                    Console.WriteLine(new string('-', 50));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("錯誤: " + ex.Message);
            }

            Console.WriteLine("按任意鍵結束...");
            Console.ReadKey();
        }
    }
}
