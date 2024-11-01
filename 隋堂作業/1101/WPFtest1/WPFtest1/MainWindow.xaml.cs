using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace CsvReaderWpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
        }

        private void LoadCsvButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "aqf_p_01.csv";
            var data = ReadCsv(filePath);

            // 將資料按照日期（ForecastDate）從高到低排序
            data = data.OrderByDescending(d => DateTime.Parse(d.ForecastDate)).ToList();
            // 將排序後的資料綁定到 DataGrid
            dataGrid.ItemsSource = data;
        }

        private List<CsvData> ReadCsv(string filePath)
        {
            var data = new List<CsvData>();
            using (var reader = new StreamReader(filePath))
            {
                var header = reader.ReadLine()?.Split(',');

                if (header == null)
                {
                    throw new Exception("CSV檔案沒有內容或格式錯誤");
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line?.Split(',');

                    if (values == null || values.Length != header.Length)
                    {
                        // Console.WriteLine("警告：資料行與標題行的欄位數不符，已跳過該行");
                        continue; // 跳過這行
                    }

                    // 建立 CsvData 物件並僅提取指定欄位
                    var row = new CsvData
                    {
                        Area = GetColumnValue(values, header, "area"),
                        MajorPollutant = GetColumnValue(values, header, "majorpollutant"),
                        ForecastDate = GetColumnValue(values, header, "forecastdate"),
                        AQI = GetColumnValue(values, header, "aqi")
                    };

                    data.Add(row);
                }
            }
            return data;
        }

        private string GetColumnValue(string[] values, string[] header, string columnName)
        {
            int index = Array.IndexOf(header, columnName);
            return index >= 0 ? values[index] : string.Empty;
        }
        
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ForecastDate")
            {
                e.Column.SortDirection = System.ComponentModel.ListSortDirection.Descending;
                dataGrid.Items.SortDescriptions.Clear();
                dataGrid.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("ForecastDate", System.ComponentModel.ListSortDirection.Descending));
            }
        }
    }
}
