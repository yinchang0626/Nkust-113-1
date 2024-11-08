using System.Globalization;
using System.Numerics;
using CsvHelper;
using Analysis;
using System.Runtime.CompilerServices;

namespace Enter
{
    class HW
    {
        static void Main()
        {
            using var reader = new StreamReader("新北市嫌疑犯人數按按教育程度別.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            var records = csv.GetRecords<Entry>();
            Dictionary<string, string> Max_year = new Dictionary<string, string>();
            Dictionary<string, int> Max_person = new Dictionary<string, int>();
            Dictionary<string, int> Sum_person = new Dictionary<string, int>();
            int drugs_other = 1;
            string item;

            foreach (var record in records)
            {
                // 顯示女性嫌疑人 > 男性嫌疑人的情形
                if(Convert.ToInt32(record.Female) > Convert.ToInt32(record.Male))
                {
                    Console.WriteLine($"{record.Year},{record.Education},{record.Item}");
                }
                if(record.Education == "總計")
                {
                    // 區分"其他"是屬於"其他毒品"，或是屬於"其他犯罪類別"
                    (item, drugs_other) = (record.Item == "其他") ? record.Other(drugs_other) : (record.Item, drugs_other);
                    
                    // 統計item人數最高的數值與其年份
                    if(record.Year == "105")
                    {
                        Max_year.Add(item, record.Year);
                        Max_person.Add(item, Convert.ToInt32(record.Total));
                        Sum_person.Add(item, Convert.ToInt32(record.Total));
                    }
                    else if(Convert.ToInt32(record.Total) > Max_person[record.Item]){
                        Max_year[item] = record.Year;
                        Max_person[item] = Convert.ToInt32(record.Total);
                    }
                    Sum_person[item] += Convert.ToInt32(record.Total);
                }
            }
            Console.WriteLine("各項目最多人數的年份:");
            foreach (var key in Max_year.Keys)
            {
                string year = Max_year[key];
                int person = Max_person[key];

                // 計算各刑事案件平均每年的嫌疑人數
                double avg = Sum_person[key] / 8;
                Console.WriteLine($"最多為{person}人({year}年)；\t每年平均:{avg}人；  ({key})");
            }
        }
    }
}