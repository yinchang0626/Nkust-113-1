using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;


namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadJsonButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "data/data.json";

            
            if (!File.Exists(filePath))
            {
                MessageBox.Show("JSON 檔案不存在！", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var data = ReadJson(filePath);
            dataGrid.ItemsSource = data;
        }

        
           private List<TouristData> ReadJson(string filePath)
          {
              string jsonContent = File.ReadAllText(filePath);

              var rootData = JsonSerializer.Deserialize<RootData>(jsonContent, new JsonSerializerOptions
              {
                  PropertyNameCaseInsensitive = true 
              });
        
              return rootData?.data;
          }
        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
           
            e.Handled = true;
            var column = e.Column;
            var direction = ListSortDirection.Descending; 


            if (column.SortDirection == null || column.SortDirection == ListSortDirection.Ascending)
            {
                direction = ListSortDirection.Descending; 
            }

            if (column.Header.ToString() == "門票收入_元" || column.Header.ToString() == "遊客人次有門票_需購票" || column.Header.ToString() == "遊客人次無門票_免費" || column.Header.ToString() == "遊客人次假日" || column.Header.ToString() == "遊客人次非假日" || column.Header.ToString() == "上年同月遊客人數")
            {
                var sortedData = dataGrid.ItemsSource.Cast<TouristData>()
                                   .OrderByDescending(d => ParseValue(d, column.Header.ToString()))
                                   .ToList();
                dataGrid.ItemsSource = sortedData;
            }
            else
            {

                var sortedData = dataGrid.ItemsSource.Cast<TouristData>()
                                   .OrderBy(d => d.觀光遊憩區別) 
                                   .ToList();
                dataGrid.ItemsSource = sortedData;
            }

            column.SortDirection = direction;
        }
        private object ParseValue(TouristData data, string columnHeader)
        {
            string value = columnHeader switch
            {
                "門票收入_元" => data.門票收入_元,
                "遊客人次有門票_需購票" => data.遊客人次有門票_需購票,
                "遊客人次無門票_免費" => data.遊客人次無門票_免費,
                "遊客人次假日" => data.遊客人次假日,
                "遊客人次非假日" => data.遊客人次非假日,
                "上年同月遊客人數" => data.上年同月遊客人數,
                _ => null
            };


            if (value == "-")
            {
                return int.MinValue; 
            }
            return int.TryParse(value, out var result) ? result : 0;
        }
    }

   
    public class TouristData
    {
        public string 觀光遊憩區別 { get; set; }
        public string 遊客人次有門票_需購票 { get; set; }
        public string 遊客人次無門票_免費 { get; set; }
        public string 遊客人次假日 { get; set; }
        public string 遊客人次非假日 { get; set; }
        public string 門票收入_元 { get; set; }
        public string 上年同月遊客人數 { get; set; }
        public string 備註 { get; set; }
    }

    public class RootData
    {
        public List<TouristData> data { get; set; } 
    }
}