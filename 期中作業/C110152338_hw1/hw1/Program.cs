// 2024/10/18 kerong
// hw1
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace hw1
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>

        // 將 BirthData 類改為 private

        [STAThread]
        static void Main()
        {
            
            // 顯示terminal作為測試
            //Console.WriteLine("應用程式啟動~");

            // 相對路徑
            string filePath = "桃園市出生登記人數.csv";

            //test();

            // 讀取數據
            var birthDataList = ReadCsv(filePath);

            
            // 分析數據
            var analyzeData = calculate(birthDataList);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 執行 form1
            // 執行 form1，並將 birthDataList 傳遞進去
            Application.Run(new Form1(birthDataList, analyzeData));
            //Console.WriteLine("按任意鍵關閉...");
            //Console.ReadKey();
        }
        
        // 將 ReadCsv 函數改為 private
        private static List<BirthData> ReadCsv(string filePath)
        {
            var birthDataList = new List<BirthData>();
            string line;
            Encoding encoding = Encoding.UTF8; // 設定編碼格式，可以根據需要修改

            //Console.WriteLine(filePath);
            try
            {
                using (StreamReader reader = new StreamReader(filePath, encoding))
                {
                    bool isFirstLine = true;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        // 建立 BirthData 物件並加入列表
                        var birthData = new BirthData();
                        //Console.WriteLine(line);

                        // 在 ReadCsv 函數中
                        var regex = new Regex(@"(?<=^|,)(?:""([^""]*)""|([^,""]+))(?:,|$)");
                        var matches = regex.Matches(line);
                        
                        if (matches.Count == 3) // 確保有三個匹配項
                        {
                            var yearStr = matches[0].Groups[1].Success ? matches[0].Groups[1].Value.Trim() : matches[0].Groups[2].Value.Trim();
                            var maleStr = matches[1].Groups[1].Success ? matches[1].Groups[1].Value.Trim() : matches[1].Groups[2].Value.Trim();
                            var femaleStr = matches[2].Groups[1].Success ? matches[2].Groups[1].Value.Trim() : matches[2].Groups[2].Value.Trim();

                            // 調用 Print 方法
                            //Print(yearStr, maleStr, femaleStr);

                            // 嘗試解析 Year
                            if (int.TryParse(yearStr, out int year))
                            {
                                birthData.Year = year;
                            }
                            else
                            {
                                Console.WriteLine($"Year 解析失敗: {yearStr}");
                            }

                            // 嘗試解析 Male
                            if (int.TryParse(maleStr.Replace(",", ""), out int male)) // 確保去掉逗號
                            {
                                birthData.Male = male;
                            }
                            else
                            {
                                Console.WriteLine($"Male 解析失敗: {maleStr}");
                            }

                            // 嘗試解析 Female
                            if (int.TryParse(femaleStr.Replace(",", ""), out int female)) // 確保去掉逗號
                            {
                                birthData.Female = female;
                            }
                            else
                            {
                                Console.WriteLine($"Female 解析失敗: {femaleStr}");
                            }

                            // 將 birthData 添加到列表中
                            birthDataList.Add(birthData);
                        }
                        else
                        {
                            Console.WriteLine($"行格式不正確: {line}");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // 異常狀態輸出
                Console.WriteLine($"讀取文件時發生錯誤: {ex.Message}");
            }
            return birthDataList;
        }

        // 將 AnalyzeData 函數改為 private
        private static AnalyzeData calculate(List<BirthData> birthDataList)
        {
            // 創建 AnalyzeData 的實例
            AnalyzeData analyzeData = new AnalyzeData();
            analyzeData.totalRecords = birthDataList.Count;
            //Print(totalRecords);
            analyzeData.totalMale = 0;
            analyzeData.totalFemale = 0;
            analyzeData.maxMale = int.MinValue;
            analyzeData.minMale = int.MaxValue;
            analyzeData.maxFemale = int.MinValue;
            analyzeData.minFemale = int.MaxValue;

            foreach (var data in birthDataList)
            {
                analyzeData.totalMale += data.Male;
                //Print(data.Male);
                analyzeData.totalFemale += data.Female;

                analyzeData.maxMale = Math.Max(analyzeData.maxMale, data.Male);
                analyzeData.minMale = Math.Min(analyzeData.minMale, data.Male);

                analyzeData.maxFemale = Math.Max(analyzeData.maxFemale, data.Female);
                analyzeData.minFemale = Math.Min(analyzeData.minFemale, data.Female);
            }

            //Print(totalMale, totalFemale);
            analyzeData.avgMale = (double)analyzeData.totalMale / analyzeData.totalRecords;
            analyzeData.avgFemale = (double)analyzeData.totalFemale / analyzeData.totalRecords;

            // 輸出結果
            //Console.WriteLine($"總記錄數：{totalRecords}");
            //Console.WriteLine($"男性總數：{totalMale}");
            //Console.WriteLine($"女性總數：{totalFemale}");
            //Console.WriteLine($"男性最大值：{maxMale}, 最小值：{minMale}, 平均值：{avgMale:F2}");
            //Console.WriteLine($"女性最大值：{maxFemale}, 最小值：{minFemale}, 平均值：{avgFemale:F2}");
            return analyzeData;
        }

        static void test()
        {
            string str1 = "12,356";
            int num1 = 0;

            // 去除逗號
            str1 = str1.Replace(",", "");

            num1 = int.Parse(str1);
            Console.WriteLine(num1); // 輸出: 12356
        }

        private static int str_to_int(string str)
        {
            // 去除逗號
            str = str.Replace(",", "");
            // 直接返回轉換結果
            return int.Parse(str);
        }
        private static void Print(params int[] values)
        {
            Console.WriteLine(string.Join(", ", values));
        }
    }
}
