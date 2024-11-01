using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CsvHelper.Configuration.Attributes;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private int stockUp;
        private int stockDown;
        private int stockNoChange;
        private int transactionVolumeOverMillion;
        private int transactionVolumeBelowTenThousand;
        public Form1()
        {
            InitializeComponent();
            ProcessStockData();
            Setupchart();
        }

        private void Setupchart() 
        {
            Chart chart = new Chart();
            ChartArea chartArea = new ChartArea();

            chart.Dock = DockStyle.Fill;
            chartArea.AxisX.Title = "Info";
            chartArea.AxisY.Title = "Num";
            chartArea.BackColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.TitleFont = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            chartArea.AxisY.TitleFont = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            chartArea.AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas.Add(chartArea);

            Title chartTitle = new Title("2024/10/18�x�W�W���Ѳ���T", Docking.Top, new System.Drawing.Font("Verdana", 16, System.Drawing.FontStyle.Bold), System.Drawing.Color.Black);
            chart.Titles.Add(chartTitle);

            Series series = new Series("Stock Data")
            {
                ChartType = SeriesChartType.Column,
                BorderWidth = 12,
                Font = new System.Drawing.Font("Arial", 12)
            };

            var dataPoints = new[]
            {
                new { Label = "�ѻ��W��", Value = stockUp, Color = System.Drawing.Color.Red },
                new { Label = "�ѻ��U�^", Value = stockDown, Color = System.Drawing.Color.Green },
                new { Label = "�ѻ����L", Value = stockNoChange, Color = System.Drawing.Color.RoyalBlue },
                new { Label = "����Ѽ� > 1M", Value = transactionVolumeOverMillion, Color = System.Drawing.Color.Orange },
                new { Label = "����Ѽ� < 10,000", Value = transactionVolumeBelowTenThousand, Color = System.Drawing.Color.Orange }
            };

            foreach (var dataPoint in dataPoints)
            {
                int pointIndex = series.Points.AddXY(dataPoint.Label, dataPoint.Value);
                series.Points[pointIndex].Color = dataPoint.Color;
            }

            series.IsValueShownAsLabel = true;
            chart.Series.Add(series);
            this.Controls.Add(chart);
        }
        private void ProcessStockData()
        {
            var projectDirectory = Directory.GetCurrentDirectory();
            var csvFilePath = Path.Combine(projectDirectory, "data", "STOCK_DAY_ALL_20241018.csv");
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null
            };

            List<StockData> records = new List<StockData>();

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                records = csv.GetRecords<StockData>().ToList();
            }

            stockUp = records.Count(r => r.ClosePrice > r.OpenPrice);
            stockDown = records.Count(r => r.ClosePrice < r.OpenPrice);
            stockNoChange = records.Count(r => r.ClosePrice == r.OpenPrice);
            transactionVolumeOverMillion = records.Count(r => r.TransactionVolume > 1000000);
            transactionVolumeBelowTenThousand = records.Count(r => r.TransactionVolume < 10000);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
    public class StockData
    {
        [CsvHelper.Configuration.Attributes.Name("�Ҩ�N��")]
        public string StockCode { get; set; }

        [CsvHelper.Configuration.Attributes.Name("�Ҩ�W��")]
        public string StockName { get; set; }

        [CsvHelper.Configuration.Attributes.Name("����Ѽ�")]
        public int? TransactionVolume { get; set; }

        [CsvHelper.Configuration.Attributes.Name("������B")]
        public decimal? TransactionAmount { get; set; }

        [CsvHelper.Configuration.Attributes.Name("�}�L��")]
        public decimal? OpenPrice { get; set; }

        [CsvHelper.Configuration.Attributes.Name("�̰���")]
        public decimal? HighPrice { get; set; }

        [CsvHelper.Configuration.Attributes.Name("�̧C��")]
        public decimal? LowPrice { get; set; }

        [CsvHelper.Configuration.Attributes.Name("���L��")]
        public decimal? ClosePrice { get; set; }

        [CsvHelper.Configuration.Attributes.Name("���浧��")]
        public int? TransactionCount { get; set; }
    }
}
