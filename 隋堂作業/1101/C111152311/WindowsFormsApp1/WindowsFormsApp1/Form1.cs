using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private TextBox txtResults;
        private Button btnLoadCsv;
        public Form1()
        {
            InitializeComponent();
            // 初始化 TextBox
            txtResults = new TextBox();
            txtResults.Multiline = true;
            txtResults.Width = 400;
            txtResults.Height = 200;
            txtResults.Location = new System.Drawing.Point(10, 10); // 設定位置
            this.Controls.Add(txtResults); // 將 TextBox 添加到表單
            btnLoadCsv = new Button();
            btnLoadCsv.Text = "載入 CSV";
            btnLoadCsv.Width = 100;
            btnLoadCsv.Height = 40;
            btnLoadCsv.Location = new System.Drawing.Point(10, 220); // 設定按鈕位置
            btnLoadCsv.Click += new EventHandler(btnLoadCsv_Click); // 綁定事件處理程序
            this.Controls.Add(btnLoadCsv); // 將按鈕添加到表單
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 這裡可以放置程式初始化的邏輯
        }


        // 按下按鈕以讀取 CSV 文件
        private void btnLoadCsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                List<Record> records = ReadCsv(filePath);
                DisplayAnalysis(records);
            }
        }

        // 顯示分析結果
        private void DisplayAnalysis(List<Record> records)
        {
            // 總數據量
            txtResults.AppendText("總數據量: " + records.Count + Environment.NewLine);

            // 計算最大值、最小值、平均值
            int max = int.MinValue;
            int min = int.MaxValue;
            int sum = 0;

            foreach (var record in records)
            {
                int value = record.役男徵集人數;
                if (value > max) max = value;
                if (value < min) min = value;
                sum += value;
            }

            double average = (double)sum / records.Count;

            // 輸出到 TextBox
            txtResults.AppendText("最大值: " + max + Environment.NewLine);
            txtResults.AppendText("最小值: " + min + Environment.NewLine);
            txtResults.AppendText("平均值: " + average.ToString("F2") + Environment.NewLine);

            // 顯示每年的數據
            foreach (var record in records)
            {
                txtResults.AppendText($"年份: {record.年別}, 徵集人數: {record.役男徵集人數}" + Environment.NewLine);
            }
        }

        // 讀取 CSV 文件
        private List<Record> ReadCsv(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                //Encoding = System.Text.Encoding.GetEncoding("big5"),
                Encoding = System.Text.Encoding.UTF8,
                Delimiter = ","
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = new List<Record>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Record
                    {
                        年別 = csv.GetField(0), // 使用索引 0 取得年份
                        役男徵集人數 = csv.GetField<int>(1) // 使用索引 1 取得徵集人數
                    };
                    records.Add(record);
                }
                return records;
            }
        }

        public class Record
        {
            public string 年別 { get; set; }
            public int 役男徵集人數 { get; set; }
        }
    }
}


