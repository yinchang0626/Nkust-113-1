using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace hw1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadDataAndDisplayChart();
        }

        // 載入數據並顯示在 Chart 上
        private void LoadDataAndDisplayChart()
        {
            string filePath = "臺南市112年第4季獨居老人人數.csv";
            var lines = File.ReadAllLines(filePath);

            Dictionary<string, (int maleCount, int femaleCount)> regionData = new Dictionary<string, (int maleCount, int femaleCount)>();
            Regex csvRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            int totalPeople = 0; // 台南市獨居老人總數
            string maxRegion = ""; // 獨居老人最多的區域
            string minRegion = ""; // 獨居老人最少的區域
            int maxCount = 0; // 獨居老人最多的數量
            int minCount = int.MaxValue; // 獨居老人最少的數量

            for (int i = 1; i < lines.Length; i++) // 跳過標題
            {
                var columns = csvRegex.Split(lines[i]);

                // 去掉雙引號並去除多餘的空格
                for (int j = 0; j < columns.Length; j++)
                {
                    columns[j] = columns[j].Trim('\"').Trim();
                }

                string region = columns[1]; // 區域名稱

                // 排除 "台南總計"
                if (region == "台南總計") continue;

                int maleCount, femaleCount;

                // 嘗試解析數值
                if (!int.TryParse(columns[3].Replace(",", ""), out maleCount) || !int.TryParse(columns[4].Replace(",", ""), out femaleCount))
                {
                    continue;
                }

                int regionTotal = maleCount + femaleCount;

                // 儲存區域的男女人數
                if (!regionData.ContainsKey(region))
                {
                    regionData[region] = (maleCount, femaleCount);
                }

                totalPeople += regionTotal; // 累計全台南市獨居老人總數

                // 檢查最大最小值
                if (regionTotal > maxCount)
                {
                    maxCount = regionTotal;
                    maxRegion = region;
                }

                if (regionTotal < minCount)
                {
                    minCount = regionTotal;
                    minRegion = region;
                }
            }

            // 設定 Chart 樣式
            chart1.Series.Clear();
            chart1.Titles.Add("台南市各區獨居老人人數男女比例");

            var maleSeries = new Series("男性");
            var femaleSeries = new Series("女性");

            maleSeries.ChartType = SeriesChartType.Column;
            femaleSeries.ChartType = SeriesChartType.Column;

            // 將數據加入圖表
            foreach (var region in regionData)
            {
                maleSeries.Points.AddXY(region.Key, region.Value.maleCount);
                femaleSeries.Points.AddXY(region.Key, region.Value.femaleCount);
            }

            chart1.Series.Add(maleSeries);
            chart1.Series.Add(femaleSeries);

            // 設定圖表外觀
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8);
            chart1.ChartAreas[0].AxisX.Title = "區域";
            chart1.ChartAreas[0].AxisY.Title = "獨居老人數";

            // 計算最多最少地區的占比
            double maxRegionPercent = (maxCount / (double)totalPeople) * 100;
            double minRegionPercent = (minCount / (double)totalPeople) * 100;

            // 顯示結果到 Label
            label1.Text = $"全台南市獨居老人總數: {totalPeople}";
            label2.Text = $"最多獨居老人的地區: {maxRegion}, 數量: {maxCount}, 占比: {maxRegionPercent:F2}%";
            label3.Text = $"最少獨居老人的地區: {minRegion}, 數量: {minCount}, 占比: {minRegionPercent:F2}%";
        }
    }
}
