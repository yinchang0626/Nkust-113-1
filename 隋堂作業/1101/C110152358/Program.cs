
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text;

namespace TainanStatistics
{
    public class AreaData
    {
        public int num { get; set; }       // Replace if needed: Ensure num matches data type in CSV
        public string area { get; set; }  // Replace: Ensure area matches string column in CSV
        public int total { get; set; }    // Replace: Ensure total is numeric in the CSV
        public int man { get; set; }      // Replace: Ensure man matches numeric column
        public int women { get; set; }    // Replace: Ensure women matches numeric column
    }

    // Custom CSV Mapping
    public sealed class AreaDataMap : ClassMap<AreaData>
    {
        public AreaDataMap()
        {
            // Match your CSV headers exactly
            Map(m => m.num).Name("序號").TypeConverterOption.NullValues("").Default(0);
            Map(m => m.area).Name("區域別").TypeConverterOption.NullValues("").Default("未知");
            Map(m => m.total).Name("總計").TypeConverterOption.NullValues("").Default(0).TypeConverter(new IntWithCommasConverter());
            Map(m => m.man).Name("男").TypeConverterOption.NullValues("").Default(0).TypeConverter(new IntWithCommasConverter());
            Map(m => m.women).Name("女").TypeConverterOption.NullValues("").Default(0).TypeConverter(new IntWithCommasConverter());
        }
    }

    public class IntWithCommasConverter : CsvHelper.TypeConversion.DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            // Remove commas and try to convert to int
            if (!string.IsNullOrWhiteSpace(text))
            {
                var cleanedText = text.Replace(",", "");
                if (int.TryParse(cleanedText, out int result))
                {
                    return result;
                }
            }
            return 0;  // Return a default value if conversion fails
        }
    }


    class Program
    {
        static List<AreaData> ReadCsv(string filePath)
        {

            using (var reader = new StreamReader(filePath, Encoding.UTF8))  // Ensure proper encoding
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,  // The CSV must have headers
                Delimiter = ","          // Ensure this matches your CSV file's delimiter
            }))
            {
                // Register custom ClassMap
                csv.Context.RegisterClassMap<AreaDataMap>();

                // Gracefully skip problematic rows
                var records = new List<AreaData>();
                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<AreaData>();
                        if (!string.IsNullOrWhiteSpace(record.area)) // Avoid null or empty area
                        {
                            records.Add(record);
                        }
                    }
                    catch (CsvHelper.TypeConversion.TypeConverterException ex)
                    {
                        Console.WriteLine($"Skipping problematic row: {csv.Parser.RawRow}");
                        Console.WriteLine($"Error: {ex.Message}");

                    }
                }
                return records;
            }

        }

        static void Main(string[] args)
        {
            Console.Clear();
            var filePath = "C:/C++/C110152358/test.csv";  // Replace with your CSV path

            // Read the CSV file
            var records = ReadCsv(filePath);

            if (records.Count == 0)
            {
                Console.WriteLine("No valid records found in the CSV file.");
                return;
            }

            // Output each record
            Console.WriteLine("\n\t\t\t臺南市112年獨居老人人數\n");
            foreach (var record in records)
            {
                if (record.area.CompareTo("台南總計") == 0)
                    Console.WriteLine(String.Format("序號: {0, -5}| 區域別: {1, -6}| 總計: {2, -5}| 男: {3, -5}| 女: {4, -5}|", record.num, record.area, record.total, record.man, record.women));
                else if (record.area.Length ==4)
                    Console.WriteLine(String.Format("序號: {0, -5}| 區域別: {1, -8}| 總計: {2, -5}| 男: {3, -5}| 女: {4, -5}|", record.num, record.area, record.total, record.man, record.women));
                else
                    Console.WriteLine(String.Format("序號: {0, -5}| 區域別: {1, -7}| 總計: {2, -5}| 男: {3, -5}| 女: {4, -5}|", record.num, record.area, record.total, record.man, record.women));
            }

            // Exclude the summary row "台南總計"
            var areaRecords = records.Where(r => r.area != "台南總計").ToList();

            // Total area count
            //int totalAreas = areaRecords.Count;
            Console.WriteLine($"\n總共區域數量: {areaRecords.Count}");

            // Max and Min calculations for "man"
            var maxMen = areaRecords.OrderByDescending(r => r.man).First();
            Console.WriteLine($"男生最大值: {maxMen.man}\t區域別: {maxMen.area}");

            var minMen = areaRecords.OrderBy(r => r.man).First();
            Console.WriteLine($"男生最小值: {minMen.man}\t區域別: {minMen.area}");

            var avgMen = areaRecords.Average(r => r.man);
            Console.WriteLine($"男生平均值: {avgMen:F2}");

            // Max and Min calculations for "women"
            var maxWomen = areaRecords.OrderByDescending(r => r.women).First();
            Console.WriteLine($"女生最大值: {maxWomen.women}\t區域別: {maxWomen.area}");

            var minWomen = areaRecords.OrderBy(r => r.women).First();
            Console.WriteLine($"女生最小值: {minWomen.women}\t區域別: {minWomen.area}");

            var avgWomen = areaRecords.Average(r => r.women);
            Console.WriteLine($"女生平均值: {avgWomen:F2}");
        }
    }
}