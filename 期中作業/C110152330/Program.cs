using System;
using System.Data;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "./經發局招商科重大投資成果 113.11.12.csv";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("檔案不存在！");
            return;
        }

        try
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                var records = new List<Dictionary<string, string>>();

                while (csv.Read())
                {
                    var record = csv.GetRecord<dynamic>() as IDictionary<string, object>;
                    records.Add(record.ToDictionary(k => k.Key, v => v.Value?.ToString() ?? string.Empty));
                }

                Console.WriteLine($"已載入 {records.Count} 筆資料。\n");

                // 資料分析範例：列出所有欄位名稱
                var headers = csv.Context.Reader.HeaderRecord;
                Console.WriteLine("欄位名稱:");
                Console.WriteLine(string.Join(", ", headers));

                // 顯示每筆資料
                Console.WriteLine("\n每筆資料內容:");
                foreach (var record in records)
                {
                    Console.WriteLine(string.Join(", ", record.Select(kv => $"{kv.Key}: {kv.Value}")));
                }

                // 資料分析範例：按某個欄位進行分組計數
                Console.WriteLine("\n按特定欄位分組計數:");
                var grouped = records.GroupBy(r => r[headers[0]]).Select(g => new { Key = g.Key, Count = g.Count() });
                foreach (var item in grouped)
                {
                    Console.WriteLine($"{item.Key}: {item.Count} 筆");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"發生錯誤: {ex.Message}");
        }
    }
}
