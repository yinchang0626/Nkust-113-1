using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 政府公開資料分析
{
    public partial class Panel_45_64 : UserControl
    {
        private Data data;
        private Data_cal all, male, female;
        public Panel_45_64(Data input_data, Data_cal input_all, Data_cal input_male, Data_cal input_female)
        {
            InitializeComponent();
            data = input_data;
            all = input_all;
            male = input_male;
            female = input_female;
        }
        private void Panel_total_Load(object sender, EventArgs e)
        {
            for (int i = 57; i <= 110; i++)
            {
                chart2.Series[0].Points.AddXY(i + "年", data.all_45_64[i - 57]);
                chart2.Series[1].Points.AddXY(i + "年", data.male_45_64[i - 57]);
                chart2.Series[2].Points.AddXY(i + "年", data.female_45_64[i - 57]);
            }
            label1.Font = new System.Drawing.Font("標楷體", 12, System.Drawing.FontStyle.Regular);
            label1.Text = $"合計平均:{all.average_45_64}人, 最大值:{all.max_year_45_64}年 {all.max_45_64}人, 最小值:{all.min_year_45_64}年 {all.min_45_64}人\n" +
                       $"男平均:{male.average_45_64}人, 最大值:{male.max_year_45_64}年 {male.max_45_64}人, 最小值:{male.min_year_45_64}年 {male.min_45_64}人\n" +
                       $"女平均:{female.average_45_64}人, 最大值:{female.max_year_45_64}年 {female.max_45_64}人, 最小值:{female.min_year_45_64}年 {female.min_45_64}人\n";
        }
    }
}
