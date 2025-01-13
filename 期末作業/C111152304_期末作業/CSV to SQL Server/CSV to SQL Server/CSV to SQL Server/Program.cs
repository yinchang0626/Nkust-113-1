using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace CSVToDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=StockDatabase;Trusted_Connection=True;";
            string csvFilePath = @"C:\Users\user\Desktop\mvc\CSV to SQL Server\stockdata.csv";

            try
            {
                using (StreamReader reader = new StreamReader(csvFilePath))
                {
                    string line;
                    int lineNumber = 0;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        while ((line = reader.ReadLine()) != null)
                        {
                            lineNumber++;

                            if (lineNumber == 1) continue;

                            var fields = line.Split(',');
                            for (int i = 0; i < fields.Length; i++)
                            {
                                fields[i] = fields[i].Trim('"').Trim();
                            }

                            if (fields.Length != 10)
                            {
                                Console.WriteLine($"第 {lineNumber} 行數據不完整，已跳過。");
                                continue;
                            }

                            try
                            {
                                string securityCode = fields[0];
                                string securityName = fields[1];
                                long tradeVolume = ParseLong(fields[2]);
                                long tradeValue = ParseLong(fields[3]);
                                decimal openingPrice = ParseDecimal(fields[4]);
                                decimal highestPrice = ParseDecimal(fields[5]);
                                decimal lowestPrice = ParseDecimal(fields[6]);
                                decimal closingPrice = ParseDecimal(fields[7]);
                                decimal priceDifference = ParseDecimal(fields[8]);
                                int tradeCount = ParseInt(fields[9]);

                                string query = @"
                                    INSERT INTO StockData (
                                        SecurityCode, SecurityName, TradeVolume, TradeValue, 
                                        OpeningPrice, HighestPrice, LowestPrice, ClosingPrice, 
                                        PriceDifference, TradeCount
                                    )
                                    VALUES (
                                        @SecurityCode, @SecurityName, @TradeVolume, @TradeValue, 
                                        @OpeningPrice, @HighestPrice, @LowestPrice, @ClosingPrice, 
                                        @PriceDifference, @TradeCount
                                    )";

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@SecurityCode", securityCode);
                                    command.Parameters.AddWithValue("@SecurityName", securityName);
                                    command.Parameters.AddWithValue("@TradeVolume", tradeVolume);
                                    command.Parameters.AddWithValue("@TradeValue", tradeValue);
                                    command.Parameters.AddWithValue("@OpeningPrice", openingPrice);
                                    command.Parameters.AddWithValue("@HighestPrice", highestPrice);
                                    command.Parameters.AddWithValue("@LowestPrice", lowestPrice);
                                    command.Parameters.AddWithValue("@ClosingPrice", closingPrice);
                                    command.Parameters.AddWithValue("@PriceDifference", priceDifference);
                                    command.Parameters.AddWithValue("@TradeCount", tradeCount);

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"第 {lineNumber} 行格式錯誤：{ex.Message}，已跳過此行。");
                                continue;
                            }
                        }

                        Console.WriteLine("CSV 資料匯入完成！");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤：{ex.Message}");
            }
        }

        static long ParseLong(string input)
        {
            if (long.TryParse(input, out long result))
                return result;

            throw new FormatException($"無法將 '{input}' 轉換為整數。");
        }

        static int ParseInt(string input)
        {
            if (int.TryParse(input, out int result))
                return result;

            throw new FormatException($"無法將 '{input}' 轉換為整數。");
        }

        static decimal ParseDecimal(string input)
        {
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                return result;

            throw new FormatException($"無法將 '{input}' 轉換為小數。");
        }
    }
}
