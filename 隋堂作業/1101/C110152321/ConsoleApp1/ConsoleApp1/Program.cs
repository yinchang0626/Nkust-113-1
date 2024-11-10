using System;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using System.Collections.Generic;

// 定義對應欄位的類別
class YachtInvestmentData
{
    public string Project { get; set; } // 項目
    public string FundingSource { get; set; } // 經費來源
    public decimal InvestmentAmount { get; set; } // 投資金額
    public decimal InvestmentRatio { get; set; } // 投資比例
    public decimal TotalInvestment { get; set; } // 投資總金額
}

// 自定義轉換器，處理可能的非數字字符
public class DecimalConverterWithFallback : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (decimal.TryParse(text?.Replace(",", "").Replace("%", ""), out var result))
        {
            return result;
        }
        return 0; // 若轉換失敗，回傳0
    }
}

// 設定 CSV 檔案的欄位映射
class YachtInvestmentDataMap : ClassMap<YachtInvestmentData>
{
    public YachtInvestmentDataMap()
    {
        Map(m => m.Project).Name("項目");
        Map(m => m.FundingSource).Name("經費來源");
        Map(m => m.InvestmentAmount).Name("投資金額").TypeConverter<DecimalConverterWithFallback>();
        Map(m => m.InvestmentRatio).Name("投資比例").TypeConverter<DecimalConverterWithFallback>();
        Map(m => m.TotalInvestment).Name("投資總金額").TypeConverter<DecimalConverterWithFallback>();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // 設定資料檔案位置
        string filePath = "D:\\fri\\hw1\\ConsoleApp1\\ConsoleApp1\\108-512.csv";

        // 使用 CsvHelper 讀取 CSV 檔案，並設置映射
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(), // 自動對應欄位名稱
            MissingFieldFound = null, // 忽略缺失欄位
        };

        List<YachtInvestmentData> records;
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            // 設定映射類別
            csv.Context.RegisterClassMap<YachtInvestmentDataMap>();
            records = csv.GetRecords<YachtInvestmentData>().ToList();
        }

        // 1. 計算數據總量
        Console.WriteLine($"數據總量: {records.Count}");

        // 2. 不同經費來源的數據分布
        var fundingGroup = records.GroupBy(r => r.FundingSource)
                                   .Select(g => new { Source = g.Key, Count = g.Count() });
        Console.WriteLine("\n不同經費來源的投資數量分布:");
        foreach (var group in fundingGroup)
        {
            Console.WriteLine($"{group.Source}: {group.Count}");
        }

        // 3. 投資金額的最大值、最小值和平均值
        var investmentAmounts = records.Select(r => r.InvestmentAmount).ToList();
        Console.WriteLine($"\n投資金額 - 最大值: {investmentAmounts.Max()}");
        Console.WriteLine($"投資金額 - 最小值: {investmentAmounts.Min()}");
        Console.WriteLine($"投資金額 - 平均值: {investmentAmounts.Average():F2}");

        // 4. 依不同經費來源統計投資比例平均值
        var fundingAverageRatio = records.GroupBy(r => r.FundingSource)
                                         .Select(g => new { Source = g.Key, AverageRatio = g.Average(r => r.InvestmentRatio) });
        Console.WriteLine("\n不同經費來源的平均投資比例:");
        foreach (var funding in fundingAverageRatio)
        {
            Console.WriteLine($"{funding.Source}: {funding.AverageRatio:P2}");
        }
    }
}
