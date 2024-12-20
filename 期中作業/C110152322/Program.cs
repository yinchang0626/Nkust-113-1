using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1
{
    class Program
    {
        public class Elderlylivingalone
        {
            public string 統計期 { get; set; }
            public string 捷運站別 { get; set; }
            public int 進站人數 { get; set; }
            public int 出站人數 { get; set; }
        }
        static void Main(string[] args)
        {
            // CSV 檔案的路徑
            string filePath = "臺北捷運各站進出人次時間數列統計資料.csv";

            // 使用 Dictionary 來儲存統計結果
            var periodInCount = new Dictionary<string, int>();
            var periodOutCount = new Dictionary<string, int>();

            // 用來儲存進站人數最多的捷運站及其統計期
            string maxStation1 = "";
            string maxPeriod1 = "";
            int maxInCount = 0;
            string maxStation2 = "";
            string maxPeriod2 = "";
            int maxOutCount = 0;
            string minStation3 = "";
            string minPeriod3 = "";
            int minInCount = 99999999;
            string minStation4 = "";
            string minPeriod4 = "";
            int minOutCount = 99999999;

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    // 讀取並跳過表頭
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        // 讀取並分割每行
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        // 解析資料
                        string period = values[0];
                        string station = values[1];
                        int inCount = int.Parse(values[2]);
                        int outCount = int.Parse(values[3]);

                        // 計算統計期總進站人數
                        if (!periodInCount.ContainsKey(period))
                            periodInCount[period] = 0;
                        periodInCount[period] += inCount;

                        // 計算統計期總出站人數
                        if (!periodOutCount.ContainsKey(period))
                            periodOutCount[period] = 0;
                        periodOutCount[period] += outCount;

 

                        // 判斷是否為進站人數最多的捷運站
                        if (inCount > maxInCount)
                        {
                            maxInCount = inCount;
                            maxStation1 = station;
                            maxPeriod1 = period;
                        }
                        // 判斷是否為出站人數最多的捷運站
                        if (outCount > maxOutCount)
                        {
                            maxOutCount = outCount;
                            maxStation2 = station;
                            maxPeriod2 = period;
                        }
                        // 判斷是否為進站人數最少的捷運站
                        if (inCount < minInCount)
                        {
                            minInCount = inCount;
                            minStation3 = station;
                            minPeriod3 = period;
                        }
                        // 判斷是否為出站人數最少的捷運站
                        if (outCount < minOutCount)
                        {
                            minOutCount = outCount;
                            minStation4 = station;
                            minPeriod4 = period;
                        }
                    }
                }

                // 輸出結果
                Console.WriteLine("統計期總進站人數:");
                foreach (var entry in periodInCount)
                    Console.WriteLine($"統計期 {entry.Key}: {entry.Value} 人");

                Console.WriteLine("\n統計期總出站人數:");
                foreach (var entry in periodOutCount)
                    Console.WriteLine($"統計期 {entry.Key}: {entry.Value} 人");


                Console.WriteLine($"\n進站人數最多的捷運站: {maxStation1}，統計期: {maxPeriod1}，人數: {maxInCount} 人");
            }
            catch (Exception ex)
            {
                Console.WriteLine("統計期總進站人數:");
                foreach (var entry in periodInCount)
                    Console.WriteLine($"統計期 {entry.Key}: {entry.Value} 人");

                Console.WriteLine("\n統計期總出站人數:");
                foreach (var entry in periodOutCount)
                    Console.WriteLine($"統計期 {entry.Key}: {entry.Value} 人");


                Console.WriteLine($"\n進站人數最多的捷運站: {maxStation1}，統計期: {maxPeriod1}，人數: {maxInCount} 人");
                Console.WriteLine($"\n出站人數最多的捷運站: {maxStation2}，統計期: {maxPeriod2}，人數: {maxOutCount} 人");
                Console.WriteLine($"\n進站人數最少的捷運站: {minStation3}，統計期: {minPeriod3}，人數: {minInCount} 人");
                Console.WriteLine($"\n出站人數最少的捷運站: {minStation4}，統計期: {minPeriod4}，人數: {minOutCount} 人");

            }
            Console.ReadKey();
        }
    }
}
