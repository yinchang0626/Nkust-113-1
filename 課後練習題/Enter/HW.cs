using System.Globalization;
using System.Numerics;
using CsvHelper;
using Analysis;

namespace Enter
{
    class HW
    {
        static void Main()
        {
            using var reader = new StreamReader("新北市嫌疑犯人數按按教育程度別.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            var records = csv.GetRecords<Entry>();
            foreach (var record in records)
            {
                if(record.Year == "105" && record.Education == "國中")
                {
                    Console.WriteLine($"total: {record.Total}  item: {record.Item}  male: {record.Male}  female: {record.Female}");
                }
            }
        }
    }
}