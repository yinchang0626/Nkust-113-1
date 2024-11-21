using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class RetirementData
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
    public string 縣市別代碼 { get; set; }
    public string 行政區域代碼 { get; set; }
}

class Program
{
    static void Main()
    {
        // 讀取 JSON 文件
        string jsonFilePath = "C:/Users/KAFKA/Downloads/1.json";
        string jsonContent = File.ReadAllText(jsonFilePath);

        // 反序列化 JSON 資料
        List<RetirementData> data = JsonConvert.DeserializeObject<List<RetirementData>>(jsonContent);

        // 顯示資料
        foreach (var item in data)
        {
            Console.WriteLine($"地區: {item.地區}, 項目: {item.項目}, 欄位名稱: {item.欄位名稱}, 數值: {item.數值}");
        }
    }
}
