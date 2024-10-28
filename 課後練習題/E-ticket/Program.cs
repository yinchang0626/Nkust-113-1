using CsvHelper;
using System.Globalization;
using Analysis;

namespace E_ticket
{
    class Program
    {
        static void Main()
        {
            using var reader = new StreamReader("高雄都會區大眾捷運系統電子票證發行量及交易量.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            var records = csv.GetRecords<dynamic>();
            foreach (var record in records)
            {
                var dict = (IDictionary<string, object>)record;
                int sum = Convert.ToInt32(dict["交易筆數-撘乘渡輪"]) + Convert.ToInt32(dict["交易筆數-搭乘公車"]) + Convert.ToInt32(dict["交易筆數-搭乘捷運"]);
                int data = Convert.ToInt32(dict["交易筆數-合計"]);
                if(sum == data)
                {
                    Console.WriteLine("True");
                }
                else
                {
                    Console.WriteLine($"False, sum = {sum}, data = {data}");
                }
            }
        }
    }
}