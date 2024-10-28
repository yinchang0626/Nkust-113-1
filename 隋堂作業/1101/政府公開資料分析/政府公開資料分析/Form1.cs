using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;

namespace 政府公開資料分析
{
    public partial class form1 : Form
    {
        //private Data data;
        //private Data_cal all, male, female;

        private Panel_total panel_total;
        private Panel_U20 panel_U20;
        private Panel_20_24 panel_20_24;
        private Panel_25_29 panel_25_29;
        private Panel_30_44 panel_30_44;
        private Panel_45_64 panel_45_64;
        private Panel_65 panel_65;
        public form1(Data input_data, Data_cal input_all, Data_cal input_male, Data_cal input_female)
        {
            InitializeComponent();
            panel_total = new Panel_total(input_data, input_all, input_male, input_female);
            panel_U20 = new Panel_U20(input_data, input_all, input_male, input_female);
            panel_20_24 = new Panel_20_24(input_data, input_all, input_male, input_female);
            panel_25_29 = new Panel_25_29(input_data, input_all, input_male, input_female);
            panel_30_44 = new Panel_30_44(input_data, input_all, input_male, input_female);
            panel_45_64 = new Panel_45_64(input_data, input_all, input_male, input_female);
            panel_65 = new Panel_65(input_data, input_all, input_male, input_female);
            this.panel1.Controls.Add(panel_total);
        }
        private void button_all(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(panel_total);
        }
        private void button_U20(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(panel_U20);
        }
        private void button_20_24(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(panel_20_24);
        }
        private void button_25_29(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(panel_25_29);
        }
        private void button_30_44(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(panel_30_44);
        }
        private void button_45_64(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(panel_45_64);
        }
        private void button_65(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(panel_65);
        }
    }
}
