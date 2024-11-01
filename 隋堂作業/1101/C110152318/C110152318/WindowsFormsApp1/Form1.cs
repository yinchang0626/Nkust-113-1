using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<PopulationData> records;

        public Form1()
        {
            InitializeComponent();
        }

        public List<PopulationData> ReadCSV(string filename)
        {
            try
            {
                using (var reader = new StreamReader(filename))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<PopulationData>().ToList();
                    return records;
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"檔案未找到: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"讀取 CSV 時發生錯誤: {ex.Message}");
                return null;
            }
        }

        // 更新標籤的函式，針對男性和女性分開顯示
        private void UpdateLabels(List<UnemploymentData> maleData, List<UnemploymentData> femaleData)
        {
            StringBuilder labelTextBuilder = new StringBuilder();
            string[] categories = { "全體", "國中及以下", "高中高職", "大專及以上" };

            for (int i = 0; i < categories.Length; i++)
            {
                labelTextBuilder.AppendLine($"{categories[i]}失業率:");
                labelTextBuilder.AppendLine($"  男性: 最大 {maleData[i].MaxRate}% (年份: 民國{maleData[i].MaxYear}年), 最小 {maleData[i].MinRate}% (年份: 民國{maleData[i].MinYear}年)");
                labelTextBuilder.AppendLine($"  女性: 最大 {femaleData[i].MaxRate}% (年份: 民國{femaleData[i].MaxYear}年), 最小 {femaleData[i].MinRate}% (年份: 民國{femaleData[i].MinYear}年)\n");
            }

            label1.Text = labelTextBuilder.ToString();
        }


        // 更新最大和最小值
        private void UpdateMaxMinValues(UnemploymentData data, double rate, int year)
        {
            // 更新最大值
            if (rate > data.MaxRate)
            {
                data.MaxRate = rate;
                data.MaxYear = year;
            }

            // 更新最小值
            if (rate < data.MinRate || data.MinRate == 0) // 假設初始最小值為 0
            {
                data.MinRate = rate;
                data.MinYear = year;
            }
        }

        // 繪製圖表 - 設定失業率數據
        private void DrawUnemploymentChart(Chart chart, List<PopulationData> records)
        {
            var seriesMaleTotal = new Series("男性全體失業率");
            var seriesFemaleTotal = new Series("女性全體失業率");

            // 設定折線圖型
            seriesMaleTotal.ChartType = SeriesChartType.Line;
            seriesFemaleTotal.ChartType = SeriesChartType.Line;

            // 加入數據點並找出最大、最小值
            foreach (var record in records)
            {
                seriesMaleTotal.Points.AddXY(record.Year, record.MaleTotal);
                seriesFemaleTotal.Points.AddXY(record.Year, record.FemaleTotal);
            }

            // 新增至圖表控制項
            chart.Series.Clear();
            chart.Series.Add(seriesMaleTotal);
            chart.Series.Add(seriesFemaleTotal);

            // 設定 X 軸和 Y 軸的標題
            chart.ChartAreas[0].AxisX.Title = "年別";
            chart.ChartAreas[0].AxisY.Title = "全體失業率 (%)";            

            // 重新繪製圖表
            chart.Invalidate();
        }

        // 繪製不同學歷失業率的圖表
        private void DrawEducationUnemploymentChart(Chart chart, List<PopulationData> records)
        {
            var seriesMaleLowEdu = new Series("男性國中及以下");
            var seriesFemaleLowEdu = new Series("女性國中及以下");
            var seriesMaleHighSchool = new Series("男性高中高職");
            var seriesFemaleHighSchool = new Series("女性高中高職");
            var seriesMaleCollege = new Series("男性大專及以上");
            var seriesFemaleCollege = new Series("女性大專及以上");

            // 設定折線圖型
            seriesMaleLowEdu.ChartType = SeriesChartType.Line;
            seriesFemaleLowEdu.ChartType = SeriesChartType.Line;
            seriesMaleHighSchool.ChartType = SeriesChartType.Line;
            seriesFemaleHighSchool.ChartType = SeriesChartType.Line;
            seriesMaleCollege.ChartType = SeriesChartType.Line;
            seriesFemaleCollege.ChartType = SeriesChartType.Line;

            // 加入數據點並找出最大、最小值
            foreach (var record in records)
            {
                seriesMaleLowEdu.Points.AddXY(record.Year, record.MaleJuniorHighAndBelow);
                seriesFemaleLowEdu.Points.AddXY(record.Year, record.FemaleJuniorHighAndBelow);
                seriesMaleHighSchool.Points.AddXY(record.Year, record.MaleSeniorHigh);
                seriesFemaleHighSchool.Points.AddXY(record.Year, record.FemaleSeniorHigh);
                seriesMaleCollege.Points.AddXY(record.Year, record.MaleCollegeAndAbove);
                seriesFemaleCollege.Points.AddXY(record.Year, record.FemaleCollegeAndAbove);
            }

            // 新增至圖表控制項
            chart.Series.Clear();
            chart.Series.Add(seriesMaleLowEdu);
            chart.Series.Add(seriesFemaleLowEdu);
            chart.Series.Add(seriesMaleHighSchool);
            chart.Series.Add(seriesFemaleHighSchool);
            chart.Series.Add(seriesMaleCollege);
            chart.Series.Add(seriesFemaleCollege);

            // 設定 X 軸和 Y 軸的標題
            chart.ChartAreas[0].AxisX.Title = "年別";
            chart.ChartAreas[0].AxisY.Title = "失業率 (%)";

            // 重新繪製圖表
            chart.Invalidate();
        }

        private void SetMaxMinLabelValues(List<PopulationData> records)
        {
            // 初始化男性和女性的失業率資料列表
            var maleData = new List<UnemploymentData>
            {
                new UnemploymentData(), // 全體男性
                new UnemploymentData(), // 國中及以下－男性
                new UnemploymentData(), // 高中高職－男性
                new UnemploymentData()  // 大專及以上－男性
            };

            var femaleData = new List<UnemploymentData>
            {
                new UnemploymentData(), // 全體女性
                new UnemploymentData(), // 國中及以下－女性
                new UnemploymentData(), // 高中高職－女性
                new UnemploymentData()  // 大專及以上－女性
            };

            // 遍歷每一筆資料以找出最大、最小值
            foreach (var record in records)
            {
                // 更新全體男性和女性的最大、最小值
                UpdateMaxMinValues(maleData[0], record.MaleTotal, record.Year);
                UpdateMaxMinValues(femaleData[0], record.FemaleTotal, record.Year);

                // 更新國中及以下的男性和女性失業率
                UpdateMaxMinValues(maleData[1], record.MaleJuniorHighAndBelow, record.Year);
                UpdateMaxMinValues(femaleData[1], record.FemaleJuniorHighAndBelow, record.Year);

                // 更新高中高職的男性和女性失業率
                UpdateMaxMinValues(maleData[2], record.MaleSeniorHigh, record.Year);
                UpdateMaxMinValues(femaleData[2], record.FemaleSeniorHigh, record.Year);

                // 更新大專及以上的男性和女性失業率
                UpdateMaxMinValues(maleData[3], record.MaleCollegeAndAbove, record.Year);
                UpdateMaxMinValues(femaleData[3], record.FemaleCollegeAndAbove, record.Year);
            }

            UpdateLabels(maleData, femaleData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // read csv
            var fileName = "data/2-8 桃園市失業率按教育程度別分.csv";
            records = ReadCSV(fileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // chart
            if (records == null || records.Count <= 0) { return; }

            foreach (var record in records) {
                Console.WriteLine($"年別: {record.Year}, 全體－男性: {record.MaleTotal}%, 全體－女性: {record.FemaleTotal}%, 國中及以下－男性: {record.MaleJuniorHighAndBelow}%, 國中及以下－女性: {record.FemaleJuniorHighAndBelow}%, 高中高職－男性: {record.MaleSeniorHigh}%, 高中高職－女性: {record.FemaleSeniorHigh}%, 大專及以上－男性: {record.MaleCollegeAndAbove}%, 大專及以上－女性: {record.FemaleCollegeAndAbove}%");
            }

            // 使用方法
            DrawUnemploymentChart(chart1, records);
            DrawEducationUnemploymentChart(chart2, records);
            SetMaxMinLabelValues(records);
        }
    }
}
