using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hw1
{
    public partial class Form1 : Form
    {
        public Form1(List<BirthData> birthDataList, AnalyzeData analyzeData)
        {
            InitializeComponent();
            PopulateChart(birthDataList);
            PopulateTextBoxes(analyzeData); 
        }

        private void PopulateChart(List<BirthData> birthDataList)
        {
            // 清除之前的數據
            chart1.Series.Clear();

            // 創建男性和女性的系列
            var maleSeries = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Male",
                Color = System.Drawing.Color.RoyalBlue,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };

            var femaleSeries = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Female",
                Color = System.Drawing.Color.HotPink,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };

            // 向系列中添加數據
            foreach (var data in birthDataList)
            {
                maleSeries.Points.AddXY(data.Year, data.Male);
                femaleSeries.Points.AddXY(data.Year, data.Female);
            }

            // 將系列添加到圖表中
            chart1.Series.Add(maleSeries);
            chart1.Series.Add(femaleSeries);

            // 添加 X 軸和 Y 軸的標題（可選）
            chart1.ChartAreas[0].AxisX.Title = "Year";
            chart1.ChartAreas[0].AxisY.Title = "Number of Births";
        }
        private void PopulateTextBoxes(AnalyzeData analyzeData)
        {
            // 將分析數據顯示在 TextBox 中
            textBox1.Text = analyzeData.totalMale.ToString();
            textBox2.Text = analyzeData.totalFemale.ToString();
            textBox3.Text = analyzeData.maxMale.ToString();
            textBox4.Text = analyzeData.maxFemale.ToString();
            textBox5.Text = analyzeData.minMale.ToString();
            textBox6.Text = analyzeData.minFemale.ToString();
            textBox7.Text = analyzeData.avgMale.ToString("F2");   // 格式化為兩位小數
            textBox8.Text = analyzeData.avgFemale.ToString("F2"); // 格式化為兩位小數
        }
    }
}
