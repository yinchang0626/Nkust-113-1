using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Font = new Font("Arial", 16, FontStyle.Bold);
            label2.Font = new Font("Arial", 16, FontStyle.Bold);
            label3.Font = new Font("Arial", 16, FontStyle.Bold);

            label1.Text = "高雄共有幾間派出所 : ";
            label2.Text = "高雄共有幾支測速照相 : ";
            label3.Text = "平均派出所管理幾支測速 : ";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 讀取 JSON 檔案路徑
            string jsonFilePath = "F:\\User\\軟體工程\\C111152335\\隋堂作業\\1101\\C111152335\\Data1.json"; // 替換為你的 JSON 檔案路徑
            if (!File.Exists(jsonFilePath))
            {
                MessageBox.Show("JSON 檔案未找到！");
                return;
            }

            // 讀取 JSON 資料
            string jsonData = File.ReadAllText(jsonFilePath);

            // 解析 JSON 到根對象
            RootObject root = JsonConvert.DeserializeObject<RootObject>(jsonData);

            if (root == null || root.Data == null || root.Data.Count == 0)
            {
                MessageBox.Show("JSON 資料為空或格式不正確！");
                return;
            }

            // 計算每間派出所的測速照相機數量
            var stationCameraCount = root.Data
                .GroupBy(c => c.派出所)
                .ToDictionary(g => g.Key, g => g.Count());

            // 計算派出所數量和平均測速照相機數量
            int totalStations = stationCameraCount.Count;
            double averageCamerasPerStation = root.Data.Count / (double)totalStations;

            // 顯示結果
            label1.Text = "高雄共有幾間派出所 : " + totalStations.ToString(); // 總派出所數量
            label2.Text = "高雄共有幾支測速照相 : " + root.Data.Count.ToString();
            label3.Text = "平均派出所管理幾支測速 : " + averageCamerasPerStation.ToString("0.00"); // 平均測速照相機數量
        }

        // JSON 根對象類型
        public class RootObject
        {
            public string ContentType { get; set; }
            public bool IsImage { get; set; }
            public List<SpeedCamera> Data { get; set; }
            public string Id { get; set; }
            public string Message { get; set; }
            public bool Success { get; set; }
        }

        // 測速照相機類型
        public class SpeedCamera
        {
            public int Seq { get; set; }
            public string No { get; set; }
            public string 分局 { get; set; }
            public string 派出所 { get; set; }
            public string 警編 { get; set; }
            public string 位置 { get; set; }
        }
    }
}
