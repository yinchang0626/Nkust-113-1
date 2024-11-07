using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace hw1
{
    internal static class Program
    {
        static void Main()
        {
            string filePath = "臺南市112年第4季獨居老人人數.csv";
            var lines = File.ReadAllLines(filePath);

            Dictionary<string, (int maleCount, int femaleCount)> regionData = new Dictionary<string, (int maleCount, int femaleCount)>();
            int totalMales = 0;
            int totalFemales = 0;

            // 使用正則表達式來處理雙引號中的逗號
            Regex csvRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            for (int i = 1; i < lines.Length; i++) // 從第2行開始，跳過第一行標題
            {
                var columns = csvRegex.Split(lines[i]);

                // 去掉雙引號並去除多餘的空格
                for (int j = 0; j < columns.Length; j++)
                {
                    columns[j] = columns[j].Trim('\"').Trim();
                }

                // 提取區域、男性、女性數據
                string region = columns[1]; // 區域在第2欄

                // 排除 "台南總計"
                if (region == "台南總計")
                {
                    continue; // 跳過這一筆資料
                }

                int maleCount;
                int femaleCount;

                // 嘗試解析男女人數，將字串轉換為整數
                if (!int.TryParse(columns[3].Replace(",", ""), out maleCount) || !int.TryParse(columns[4].Replace(",", ""), out femaleCount))
                {
                    Console.WriteLine($"警告: 無法解析數值 男: {columns[3]} 或 女: {columns[4]}");
                    continue; // 跳過無法解析的行
                }

                // 若地區尚未在字典中，則初始化
                if (!regionData.ContainsKey(region))
                {
                    regionData[region] = (0, 0);
                }

                // 累加每個地區的男女人數
                regionData[region] = (regionData[region].maleCount + maleCount, regionData[region].femaleCount + femaleCount);
                totalMales += maleCount; // 累加台南市內男性總數
                totalFemales += femaleCount; // 累加台南市內女性總數
            }

            // 計算台南市總獨居老人數
            int totalElderly = totalMales + totalFemales;

            // 初始化最大和最小的地區
            string maxRegion = "";
            string minRegion = "";
            int maxElderly = int.MinValue;
            int minElderly = int.MaxValue;

            // 遍歷所有地區，找出最多和最少獨居老人的地區
            foreach (var region in regionData)
            {
                int totalRegionElderly = region.Value.maleCount + region.Value.femaleCount;

                if (totalRegionElderly > maxElderly)
                {
                    maxElderly = totalRegionElderly;
                    maxRegion = region.Key;
                }

                if (totalRegionElderly < minElderly)
                {
                    minElderly = totalRegionElderly;
                    minRegion = region.Key;
                }
            }

            // 計算最多和最少地區的占比
            double maxRegionRatio = (double)maxElderly / totalElderly * 100;
            double minRegionRatio = (double)minElderly / totalElderly * 100;

            // 顯示結果
            Console.WriteLine($"全台南市獨居老人總數: {totalElderly}");
            Console.WriteLine($"最多獨居老人的地區: {maxRegion}, 數量: {maxElderly}, 占比: {maxRegionRatio:F2}%");
            Console.WriteLine($"最少獨居老人的地區: {minRegion}, 數量: {minElderly}, 占比: {minRegionRatio:F2}%");

            // 顯示每個地區的男女比例與總計
            foreach (var region in regionData)
            {
                string regionName = region.Key;  // 取得地區名稱
                int maleCount = region.Value.maleCount;  // 取得該地區的男性人數
                int femaleCount = region.Value.femaleCount;  // 取得該地區的女性人數
                int total = maleCount + femaleCount;
                double ratio = femaleCount == 0 ? 0 : (double)maleCount / femaleCount;

                Console.WriteLine($"{regionName} - 男: {maleCount}, 女: {femaleCount}, 總計: {total}, 男女比例: {ratio:F2}");
            }

            // 計算並顯示全市平均
            int totalRegions = regionData.Count;
            double avgMale = totalMales / (double)totalRegions;
            double avgFemale = totalFemales / (double)totalRegions;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm()); // 啟動並顯示主表單
            Console.WriteLine($"全台南市獨居老人平均 - 男: {avgMale:F2}, 女: {avgFemale:F2}");
            Console.ReadLine();
        }
    }
}
