using CsvHelper;
using System.Globalization;

namespace E_ticket
{
    class Program
    {
        static void Main()
        {
            using var reader = new StreamReader("高雄都會區大眾捷運系統電子票證發行量及交易量.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            Dictionary<string, int> total_deal = new Dictionary<string, int>();
            Dictionary<string, int> amount_per_year = new Dictionary<string, int>();
            
            var records = csv.GetRecords<dynamic>();
            int max_deal_value = 0;
            string max_deal_year = "年份";
            int last_year_amount = 0;
            int max_amount = 0;
            string max_amount_year = "年分";
            double value_per_deal = 0.0;
            foreach (var record in records)
            {
                var dict = (IDictionary<string, object>)record;

                // 計算搭乘不同工具之交一筆數總和，是否符合"交易筆數-合計"
                // 其中一筆不符合(2014年)，以三者相加(sum)為主
                int sum = Convert.ToInt32(dict["交易筆數-撘乘渡輪"]) + Convert.ToInt32(dict["交易筆數-搭乘公車"]) + Convert.ToInt32(dict["交易筆數-搭乘捷運"]);
                int data = Convert.ToInt32(dict["交易筆數-合計"]);
                // if(sum == data)
                // {
                //     Console.WriteLine("True");
                // }
                // else
                // {
                //     Console.WriteLine($"False, sum = {sum}, data = {data}");
                // }
                string year = dict["年分"]?.ToString() ?? "其他年份";
                total_deal.Add(year, sum);

                //計算每筆之交易金額
                value_per_deal = Convert.ToInt32(dict["交易金額"]) / Convert.ToDouble(dict["交易筆數-合計"]);

                // 找出交易金額最高的年份與其數值
                if (Convert.ToInt32(dict["交易金額"]) > max_deal_value)
                {
                    max_deal_value = Convert.ToInt32(dict["交易金額"]);
                    max_deal_year = year;
                }
                Console.WriteLine($"-----{year}年-----");

                // 計算每一年的發卡量與最高發卡量的年份
                if (year != "2011")
                {
                    amount_per_year.Add(year, Convert.ToInt32(dict["累計發卡量"]) - last_year_amount);
                    Console.WriteLine($"發卡量為{amount_per_year[year]}");
                    if (amount_per_year[year] > max_amount)
                    {
                        max_amount = amount_per_year[year];
                        max_amount_year = year;
                    }
                }
                Console.WriteLine($"每筆交易金額:{value_per_deal}");
                last_year_amount = Convert.ToInt32(dict["累計發卡量"]);
            }
            Console.WriteLine($"-----------------------------");
            Console.WriteLine($"最高發卡量為{max_amount_year}年的{max_amount}");
            Console.WriteLine($"最高交易金額為{max_deal_year}年的{max_deal_value}");

            
        }
    }
}