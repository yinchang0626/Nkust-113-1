using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Shelter
{
    public string 類別 { get; set; }
    public string 建築物名稱 { get; set; }
    public string 電腦編號 { get; set; }
    public string 村里別 { get; set; }
    public string 地址 { get; set; }
    public string 緯度 { get; set; }
    public string 經度 { get; set; }
    public int 地下樓層數 { get; set; }
    public int 可容納人數 { get; set; }
    public string 轄管分局 { get; set; }
}

class Program
{
    static void Main()
    {
        // 讀取 JSON 檔案
        string jsonPath = "防空.json";
        var shelters = JsonConvert.DeserializeObject<List<Shelter>>(File.ReadAllText(jsonPath));

        // 顯示每筆資料
        foreach (var shelter in shelters)
        {
            Console.WriteLine($"類別: {shelter.類別}");
            Console.WriteLine($"建築物名稱: {shelter.建築物名稱}");
            Console.WriteLine($"電腦編號: {shelter.電腦編號}");
            Console.WriteLine($"村里別: {shelter.村里別}");
            Console.WriteLine($"地址: {shelter.地址}");
            Console.WriteLine($"緯度: {shelter.緯度}");
            Console.WriteLine($"經度: {shelter.經度}");
            Console.WriteLine($"地下樓層數: {shelter.地下樓層數}");
            Console.WriteLine($"可容納人數: {shelter.可容納人數}");
            Console.WriteLine($"轄管分局: {shelter.轄管分局}");
            Console.WriteLine(new string('-', 50));
        }

        Console.WriteLine("按任意鍵結束...");
        Console.ReadKey();
    }
}
