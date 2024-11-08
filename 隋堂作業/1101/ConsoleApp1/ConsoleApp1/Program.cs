using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
namespace DataSummaryApp
{
    public class DataRecord
    {
        [Name("序號")]
        public int SerialNumber { get; set; }          // 序號

        [Name("行政區")]
        public string District { get; set; }           // 行政區

        [Name("屬性")]
        public string Attribute { get; set; }          // 屬性

        [Name("機構名稱")]
        public string InstitutionName { get; set; }    // 機構名稱

        [Name("機構類型")]
        public string InstitutionType { get; set; }    // 機構類型

        [Name("服務對象")]
        public string ServiceTarget { get; set; }      // 服務對象

        [Name("核定服務床位數量")]
        public int ApprovedBeds { get; set; }          // 核定服務床位數量

        [Name("可申請床位數量")]
        public int AvailableBeds { get; set; }         // 可申請床位數量

        [Name("說明")]
        public string Description { get; set; }        // 說明

        [Name("評鑑")]
        public string Evaluation { get; set; }         // 評鑑

        [Name("地址")]
        public string Address { get; set; }            // 地址

        [Name("電話")]
        public string Phone { get; set; }              // 電話

        [Name("聯絡人")]
        public string ContactPerson { get; set; }      // 聯絡人

        [Name("網址")]
        public string Website { get; set; }            // 網址

        [Name("更新日期")]
        public DateTime UpdateDate { get; set; }       // 更新日期
    }
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C://Users/user/Desktop/HW_F/ConsoleApp1/兒少機構收容暨空位狀態.csv";  // 請確認路徑

            var records = ReadCsvData(filePath);


            // 總記錄數量
            Console.WriteLine($"總記錄數量: {records.Count}");

            // 不同行政區的分布
            var districtDistribution = records.GroupBy(r => r.District)
                                              .Select(g => new { District = g.Key, Count = g.Count() });

            Console.WriteLine("\n行政區分布:");
            foreach (var district in districtDistribution)
            {
                Console.WriteLine($"{district.District}: {district.Count} 個機構");
            }

            // 核定服務床位數量和可申請床位數量的最大值、最小值和平均值
            var approvedBedsMax = records.Max(r => r.ApprovedBeds);
            var approvedBedsMin = records.Min(r => r.ApprovedBeds);
            var approvedBedsAvg = records.Average(r => r.ApprovedBeds);

            var availableBedsMax = records.Max(r => r.AvailableBeds);
            var availableBedsMin = records.Min(r => r.AvailableBeds);
            var availableBedsAvg = records.Average(r => r.AvailableBeds);

            Console.WriteLine("\n核定服務床位數量:");
            Console.WriteLine($"最大值: {approvedBedsMax}, 最小值: {approvedBedsMin}, 平均值: {approvedBedsAvg:F2}");

            Console.WriteLine("\n可申請床位數量:");
            Console.WriteLine($"最大值: {availableBedsMax}, 最小值: {availableBedsMin}, 平均值: {availableBedsAvg:F2}");
        }

        static List<DataRecord> ReadCsvData(string filePath)
        {
            using (var reader = new StreamReader(filePath))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //reader.ReadLine();
                return csv.GetRecords<DataRecord>().ToList();
            }
        }
    }
}
