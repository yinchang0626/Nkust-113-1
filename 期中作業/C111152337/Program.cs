using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\User\\Desktop\\C#\\ConsoleApp1\\CSV\\桃園市政府消費諮詢商品類型分析表.csv"; // 更新檔案名稱

            if (!File.Exists(filePath))
            {
                Console.WriteLine("找不到檔案： " + filePath);
                return;
            }

            // 讀取 CSV 資料
            var lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                Console.WriteLine("檔案內容不足，請檢查資料格式。");
                return;
            }

            // 嘗試解析標題列
            var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray(); // 移除多餘空格
            var data = new List<Dictionary<string, int>>();

            // 解析每行數據
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    Console.WriteLine($"警告：第 {i + 1} 行為空白，已跳過。");
                    continue;
                }

                var values = lines[i].Split(',').Select(v => v.Trim()).ToArray();
                if (values.Length != headers.Length)
                {
                    // Commenting out the column mismatch warning message
                    // Console.WriteLine($"警告：第 {i + 1} 行的欄位數量與標題列不符，已嘗試自動補齊。");
                    // 自動補齊或忽略額外欄位
                    Array.Resize(ref values, headers.Length);
                }

                var row = new Dictionary<string, int>();
                bool isValidRow = true;

                // 解析每一個欄位的數據，若有無法解析的數值，則不顯示該行
                for (int j = 0; j < headers.Length; j++)
                {
                    if (int.TryParse(values[j], out int number))
                    {
                        row[headers[j]] = number;
                    }
                    else
                    {
                        // 如果有無法解析的數值，設置標誌為無效並跳出
                        isValidRow = false;
                        break;
                    }
                }

                // 若該行有效，則將其加入數據列表
                if (isValidRow)
                {
                    data.Add(row);
                }
                // Removed the warning message about unparseable values
                // else
                // {
                //     Console.WriteLine($"警告：第 {i + 1} 行包含無法解析的數值，已跳過。");
                // }
            }

            // 顯示每個欄位資料
            Console.WriteLine("讀取的數據：");
            foreach (var row in data)
            {
                foreach (var kvp in row)
                {
                    Console.Write($"{kvp.Key}: {kvp.Value}\t");
                }
                Console.WriteLine();
            }

            // 總結每年度最高物品
            Console.WriteLine("\n各年度最高物品：");
            foreach (var row in data)
            {
                if (row.ContainsKey("年度") && row["年度"] != 0)
                {
                    int year = row["年度"];
                    var maxItem = row.Where(kvp => kvp.Key != "年度" && kvp.Key != "合計")
                                     .OrderByDescending(kvp => kvp.Value)
                                     .First();

                    Console.WriteLine($"年度 {year}: {maxItem.Key} - {maxItem.Value}");
                }
            }
        }
    }
}
