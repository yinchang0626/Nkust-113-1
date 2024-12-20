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

        private void button1_Click(object sender, EventArgs e){
            string jsonFilePath = "F:\\User\\軟體工程\\C111152335\\隋堂作業\\1101\\C111152335\\Data1.json"; 
            if (!File.Exists(jsonFilePath)) {
                MessageBox.Show("JSON 檔案未找到！");
                return;
            }

            string jsonData = File.ReadAllText(jsonFilePath);

            // 解析 JSON 到ROOT對象
            RootObject root = JsonConvert.DeserializeObject<RootObject>(jsonData);

            if (root == null || root.Data == null || root.Data.Count == 0) {
                MessageBox.Show("JSON 資料為空或格式不正確！");
                return;
            }


            // ***過濾掉派出所為空的資料***=============================================================================
            var stationCameraCount = root.Data
                .Where(c => !string.IsNullOrEmpty(c.派出所))  // 過濾掉派出所為 null 或空的資料
                .GroupBy(c => c.派出所)  // 以 派出所 作為分組依據
                .ToDictionary(g => g.Key, g => g.Count());  // 將分組的結果轉換成字典
            //==========================================================================================================
            int totalStations = stationCameraCount.Count;
            double averageCamerasPerStation = root.Data.Count / (double)totalStations;

            label1.Text = "高雄共有幾間派出所 : " + totalStations.ToString(); 
            label2.Text = "高雄共有幾支測速照相 : " + root.Data.Count.ToString();
            label3.Text = "平均派出所管理幾支測速 : " + averageCamerasPerStation.ToString("0.00"); 
        }

        // JSON.ROOT
        public class RootObject {
            public object ContentType { get; set; }
            public bool IsImage { get; set; }
            public List<SpeedCamera> Data { get; set; }  // ****包含測速照相機資料***
            public string Id { get; set; }
            public object Message { get; set; }
            public bool Success { get; set; }
        }

        public class SpeedCamera {
            public int Seq { get; set; }
            public string No { get; set; }
            public string 分局 { get; set; }  
            public string 派出所 { get; set; }  
            public string 警編 { get; set; } 
            public string 位置 { get; set; } 
        }
    }
}
