using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Linq;

public class ElectionData
{
    public string 年別 { get; set; }
    public string 性別 { get; set; }
    public int 國民大會代表 { get; set; }
    public int 立法委員 { get; set; }
    public int 監察委員 { get; set; }
    public int 市議員 { get; set; }
}

// 自訂的轉換器類別
public class NullableIntConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text) || text == "-")
        {
            return 0; // 將 "-" 或空值視為 0
        }
        return int.Parse(text);
    }
}

// 定義 ElectionData 的映射類別
public sealed class ElectionDataMap : ClassMap<ElectionData>
{
    public ElectionDataMap()
    {
        Map(m => m.年別);
        Map(m => m.性別);
        Map(m => m.國民大會代表).TypeConverter<NullableIntConverter>();
        Map(m => m.立法委員).TypeConverter<NullableIntConverter>();
        Map(m => m.監察委員).TypeConverter<NullableIntConverter>();
        Map(m => m.市議員).TypeConverter<NullableIntConverter>();
    }
}

class Program
{
    [STAThread]
    static void Main()
    {
        // 檔案路徑
        var path = @"C:\Users\Joe\Desktop\ConsoleApp1\gs00380yac.csv";

        // 檢查檔案是否存在
        if (!File.Exists(path))
        {
            Console.WriteLine("指定的檔案不存在。");
            return;
        }

        try
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // 設定映射
                csv.Context.RegisterClassMap<ElectionDataMap>();

                // 讀取數據
                var relist = csv.GetRecords<ElectionData>().ToList();

                // 依照年別分組並計算各年男女比例
                var groupedByYear = relist
                    .GroupBy(r => r.年別)
                    .ToList();

                foreach (var group in groupedByYear)
                {
                    var year = group.Key;

                    // 總計數據
                    var totalMen = group.Where(r => r.性別 == "男");
                    var totalWomen = group.Where(r => r.性別 == "女");
                    var totalCandidates = group.Where(r => r.性別 == "總計");

                    // 計算市議員的男女比例
                    var totalMenCityCouncils = totalMen.Sum(r => r.市議員);
                    var totalWomenCityCouncils = totalWomen.Sum(r => r.市議員);
                    var totalCityCouncils = totalCandidates.Sum(r => r.市議員);
                    double menCityCouncilPercentage = totalCityCouncils > 0 ? (double)totalMenCityCouncils / totalCityCouncils * 100 : 0;
                    double womenCityCouncilPercentage = totalCityCouncils > 0 ? (double)totalWomenCityCouncils / totalCityCouncils * 100 : 0;

                    // 計算其他職位的男女比例
                    var menDelegates = totalMen.Sum(r => r.國民大會代表);
                    var womenDelegates = totalWomen.Sum(r => r.國民大會代表);
                    var totalDelegates = totalCandidates.Sum(r => r.國民大會代表);
                    double menDelegatePercentage = totalDelegates > 0 ? (double)menDelegates / totalDelegates * 100 : 0;
                    double womenDelegatePercentage = totalDelegates > 0 ? (double)womenDelegates / totalDelegates * 100 : 0;

                    var menLegislators = totalMen.Sum(r => r.立法委員);
                    var womenLegislators = totalWomen.Sum(r => r.立法委員);
                    var totalLegislators = totalCandidates.Sum(r => r.立法委員);
                    double menLegislatorPercentage = totalLegislators > 0 ? (double)menLegislators / totalLegislators * 100 : 0;
                    double womenLegislatorPercentage = totalLegislators > 0 ? (double)womenLegislators / totalLegislators * 100 : 0;

                    var menSupervisors = totalMen.Sum(r => r.監察委員);
                    var womenSupervisors = totalWomen.Sum(r => r.監察委員);
                    var totalSupervisors = totalCandidates.Sum(r => r.監察委員);
                    double menSupervisorPercentage = totalSupervisors > 0 ? (double)menSupervisors / totalSupervisors * 100 : 0;
                    double womenSupervisorPercentage = totalSupervisors > 0 ? (double)womenSupervisors / totalSupervisors * 100 : 0;

                    // 顯示每一年的結果
                    Console.WriteLine($"年別: {year}");
                    Console.WriteLine($"總市議員人數: {totalCityCouncils}");
                    Console.WriteLine($"男性市議員人數: {totalMenCityCouncils}");
                    Console.WriteLine($"女性市議員人數: {totalWomenCityCouncils}");
                    Console.WriteLine($"男性市議員比例: {menCityCouncilPercentage:F2}%");
                    Console.WriteLine($"女性市議員比例: {womenCityCouncilPercentage:F2}%");

                    Console.WriteLine($"總國民大會代表人數: {totalDelegates}");
                    Console.WriteLine($"男性國民大會代表人數: {menDelegates}");
                    Console.WriteLine($"女性國民大會代表人數: {womenDelegates}");
                    Console.WriteLine($"男性國民大會代表比例: {menDelegatePercentage:F2}%");
                    Console.WriteLine($"女性國民大會代表比例: {womenDelegatePercentage:F2}%");

                    Console.WriteLine($"總立法委員人數: {totalLegislators}");
                    Console.WriteLine($"男性立法委員人數: {menLegislators}");
                    Console.WriteLine($"女性立法委員人數: {womenLegislators}");
                    Console.WriteLine($"男性立法委員比例: {menLegislatorPercentage:F2}%");
                    Console.WriteLine($"女性立法委員比例: {womenLegislatorPercentage:F2}%");

                    Console.WriteLine($"總監察委員人數: {totalSupervisors}");
                    Console.WriteLine($"男性監察委員人數: {menSupervisors}");
                    Console.WriteLine($"女性監察委員人數: {womenSupervisors}");
                    Console.WriteLine($"男性監察委員比例: {menSupervisorPercentage:F2}%");
                    Console.WriteLine($"女性監察委員比例: {womenSupervisorPercentage:F2}%");
                    Console.WriteLine(); // 空行分隔
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("找不到檔案");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"發生錯誤: {ex.Message}");
        }
        finally
        {
            Console.ReadLine();
        }
    }
}
