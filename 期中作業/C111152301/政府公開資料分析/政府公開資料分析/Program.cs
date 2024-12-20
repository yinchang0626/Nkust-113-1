using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Windows.Forms.VisualStyles;

public struct Data
{
    public List<int> all_total, all_u20, all_20_24, all_25_29, all_30_44, all_45_64, all_65,
                    male_total, male_u20, male_20_24, male_25_29, male_30_44, male_45_64, male_65,
                    female_total, female_u20, female_20_24, female_25_29, female_30_44, female_45_64, female_65;
}
public struct Data_cal
{
    public double average_total, average_u20, average_20_24, average_25_29, average_30_44, average_45_64, average_65;
    public int max_total, max_year_total, min_total, min_year_total,
               max_u20, max_year_u20, min_u20, min_year_u20,
               max_20_24, max_year_20_24, min_20_24, min_year_20_24,
               max_25_29, max_year_25_29, min_25_29, min_year_25_29,
               max_30_44, max_year_30_44, min_30_44, min_year_30_44,
               max_45_64, max_year_45_64, min_45_64, min_year_45_64,
               max_65, max_year_65, min_65, min_year_65;
}

namespace 政府公開資料分析
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 呼叫 Load 方法來讀取和分類 CSV 數據
            Data data = Load("../../臺北市政府推介就業服務按求職人年齡別.csv");

            Data_cal all, male, female;
            (all.average_total, all.max_total, all.max_year_total, all.min_total, all.min_year_total) = PrintStatistics(data.all_total, "合計_總計 數據");
            (all.average_u20, all.max_u20, all.max_year_u20, all.min_u20, all.min_year_u20) = PrintStatistics(data.all_u20, "合計_20歲以下 數據");
            (all.average_20_24, all.max_20_24, all.max_year_20_24, all.min_20_24, all.min_year_20_24) = PrintStatistics(data.all_20_24, "合計_20-24歲 數據");
            (all.average_25_29, all.max_25_29, all.max_year_25_29, all.min_25_29, all.min_year_25_29) = PrintStatistics(data.all_25_29, "合計_25-29歲 數據");
            (all.average_30_44, all.max_30_44, all.max_year_30_44, all.min_30_44, all.min_year_30_44) = PrintStatistics(data.all_30_44, "合計_30-44歲 數據");
            (all.average_45_64, all.max_45_64, all.max_year_45_64, all.min_45_64, all.min_year_45_64) = PrintStatistics(data.all_45_64, "合計_45-64歲 數據");
            (all.average_65, all.max_65, all.max_year_65, all.min_65, all.min_year_65) = PrintStatistics(data.all_65, "合計_65歲以上 數據");

            (male.average_total, male.max_total, male.max_year_total, male.min_total, male.min_year_total) = PrintStatistics(data.male_total, "男_總計 數據");
            (male.average_u20, male.max_u20, male.max_year_u20, male.min_u20, male.min_year_u20) = PrintStatistics(data.male_u20, "男_20歲以下 數據");
            (male.average_20_24, male.max_20_24, male.max_year_20_24, male.min_20_24, male.min_year_20_24) = PrintStatistics(data.male_20_24, "男_20-24歲 數據");
            (male.average_25_29, male.max_25_29, male.max_year_25_29, male.min_25_29, male.min_year_25_29) = PrintStatistics(data.male_25_29, "男_25-29歲 數據");
            (male.average_30_44, male.max_30_44, male.max_year_30_44, male.min_30_44, male.min_year_30_44) = PrintStatistics(data.male_30_44, "男_30-44歲 數據");
            (male.average_45_64, male.max_45_64, male.max_year_45_64, male.min_45_64, male.min_year_45_64) = PrintStatistics(data.male_45_64, "男_45-64歲 數據");
            (male.average_65, male.max_65, male.max_year_65, male.min_65, male.min_year_65) = PrintStatistics(data.male_65, "男_65歲以上 數據");

            (female.average_total, female.max_total, female.max_year_total, female.min_total, female.min_year_total) = PrintStatistics(data.female_total, "女_總計 數據");
            (female.average_u20, female.max_u20, female.max_year_u20, female.min_u20, female.min_year_u20) = PrintStatistics(data.female_u20, "女_20歲以下 數據");
            (female.average_20_24, female.max_20_24, female.max_year_20_24, female.min_20_24, female.min_year_20_24) = PrintStatistics(data.female_20_24, "女_20-24歲 數據");
            (female.average_25_29, female.max_25_29, female.max_year_25_29, female.min_25_29, female.min_year_25_29) = PrintStatistics(data.female_25_29, "女_25-29歲 數據");
            (female.average_30_44, female.max_30_44, female.max_year_30_44, female.min_30_44, female.min_year_30_44) = PrintStatistics(data.female_30_44, "女_30-44歲 數據");
            (female.average_45_64, female.max_45_64, female.max_year_45_64, female.min_45_64, female.min_year_45_64) = PrintStatistics(data.female_45_64, "女_45-64歲 數據");
            (female.average_65, female.max_65, female.max_year_65, female.min_65, female.min_year_65) = PrintStatistics(data.female_65, "女_65歲以上 數據");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new form1(data, all, male, female)); // 啟動 Form1
        }
        static Data Load(string filePath)
        {
            // 註冊編碼提供程式以支援 Big5 編碼
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // 使用 Big5 編碼讀取 CSV 檔案內容
            string csvContent = File.ReadAllText(filePath, Encoding.GetEncoding("Big5"));
            List<int> all_total = new List<int>();
            List<int> all_u20 = new List<int>();
            List<int> all_20_24 = new List<int>();
            List<int> all_25_29 = new List<int>();
            List<int> all_30_44 = new List<int>();
            List<int> all_45_64 = new List<int>();
            List<int> all_65 = new List<int>();
            List<int> male_total = new List<int>();
            List<int> male_u20 = new List<int>();
            List<int> male_20_24 = new List<int>();
            List<int> male_25_29 = new List<int>();
            List<int> male_30_44 = new List<int>();
            List<int> male_45_64 = new List<int>();
            List<int> male_65 = new List<int>();
            List<int> female_total = new List<int>();
            List<int> female_u20 = new List<int>();
            List<int> female_20_24 = new List<int>();
            List<int> female_25_29 = new List<int>();
            List<int> female_30_44 = new List<int>();
            List<int> female_45_64 = new List<int>();
            List<int> female_65 = new List<int>();
            Data data = new Data();

            // 使用 TextFieldParser 從字串中解析 CSV 內容
            using (TextReader reader = new StringReader(csvContent))
            using (TextFieldParser parser = new TextFieldParser(reader))
            {
                parser.TextFieldType = FieldType.Delimited; // 設定為分隔檔案
                parser.SetDelimiters(","); // 設定分隔符號為逗號

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields(); // 讀取每一行的字段

                    if (fields[1].Contains("合計"))
                    {
                        all_total.Add(int.Parse(fields[2])); // 儲存符合條件的資料
                        all_u20.Add(int.Parse(fields[3]));
                        all_20_24.Add(int.Parse(fields[4]));
                        all_25_29.Add(int.Parse(fields[5]));
                        all_30_44.Add(int.Parse(fields[6]));
                        all_45_64.Add(int.Parse(fields[7]));
                        all_65.Add(ParseField(fields[8]));
                    }

                    if (fields[1].Contains("男"))
                    {
                        male_total.Add(int.Parse(fields[2]));
                        male_u20.Add(int.Parse(fields[3]));
                        male_20_24.Add(int.Parse(fields[4]));
                        male_25_29.Add(int.Parse(fields[5]));
                        male_30_44.Add(int.Parse(fields[6]));
                        male_45_64.Add(int.Parse(fields[7]));
                        male_65.Add(ParseField(fields[8]));
                    }

                    if (fields[1].Contains("女"))
                    {
                        female_total.Add(int.Parse(fields[2]));
                        female_u20.Add(int.Parse(fields[3]));
                        female_20_24.Add(int.Parse(fields[4]));
                        female_25_29.Add(int.Parse(fields[5]));
                        female_30_44.Add(int.Parse(fields[6]));
                        female_45_64.Add(int.Parse(fields[7]));
                        female_65.Add(ParseField(fields[8]));
                    }
                }
            }
            data.all_total = all_total;
            data.all_u20 = all_u20;
            data.all_20_24 = all_20_24;
            data.all_25_29 = all_25_29;
            data.all_30_44 = all_30_44;
            data.all_45_64 = all_45_64;
            data.all_65 = all_65;
            data.male_total = male_total;
            data.male_u20 = male_u20;
            data.male_20_24 = male_20_24;
            data.male_25_29 = male_25_29;
            data.male_30_44 = male_30_44;
            data.male_45_64 = male_45_64;
            data.male_65 = male_65;
            data.female_total = female_total;
            data.female_u20 = female_u20;
            data.female_20_24 = female_20_24;
            data.female_25_29 = female_25_29;
            data.female_30_44 = female_30_44;
            data.female_45_64 = female_45_64;
            data.female_65 = female_65;
            return data;
        }

        static int ParseField(string field)
        {
            return int.TryParse(field, out int value) ? value : 0;
        }

        static (double, int, int, int, int) PrintStatistics(List<int> data, string label)
        {
            double average = CalculateAverage(data);
            var (max, max_year) = CalculateMax(data);
            var (min, min_year) = CalculateMin(data);

            Console.WriteLine($"{label}");
            Console.WriteLine($"平均值: {average}");
            Console.WriteLine($"最大值: {max}, 年份: {max_year}");
            Console.WriteLine($"最小值: {min}, 年份: {min_year}");

            return (average, max, max_year, min, min_year);
        }

        // 計算平均值
        static double CalculateAverage(List<int> data)
        {
            double total = 0;
            foreach (var value in data)
            {
                total += value;
            }
            return Math.Round(total / data.Count);
        }

        // 計算最大值
        static (int, int) CalculateMax(List<int> data)
        {
            int max = int.MinValue;
            int year = 0;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] > max)
                {
                    max = data[i];
                    year = i + 57;
                }
            }
            return (max, year); 
        }


        // 計算最小值
        static (int, int) CalculateMin(List<int> data)
        {
            int min = int.MaxValue;
            int year = 0;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] < min)
                {
                    min = data[i];
                    year = i + 57; 
                }
            }
            return (min, year);
        }
    }
}
