using System.Diagnostics;
using System.Text.Json;

namespace MidProject
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            listView1 = new ListView();
            comboBox1 = new ComboBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "查詢";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listView1
            // 
            listView1.Location = new Point(220, 12);
            listView1.Name = "listView1";
            listView1.Size = new Size(581, 426);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(93, 13);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 2;
            // 
            // button2
            // 
            button2.Location = new Point(12, 41);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "重置";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(810, 447);
            Controls.Add(button2);
            Controls.Add(comboBox1);
            Controls.Add(listView1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "ParkingSlotSearching";
            ResumeLayout(false);

            LoadData();
        }

        #endregion

        private Button button1;
        private ListView listView1;

        List<ParkingData> parkingDatas;

        private void LoadData()
        {
            string projectRootPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string jsonFilePath = Path.Combine(projectRootPath, "ParkSpace.json");
            string jsonString = File.ReadAllText(jsonFilePath);

            // 使用 JsonDocument 解析 JSON 資料
            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
                {
                    // 提取你需要的部分
                    parkingDatas = new List<ParkingData>();
                    HashSet<string> areaSet = new HashSet<string>();
                    foreach (JsonElement item in dataElement.EnumerateArray())
                    {
                        // 只提取需要的部分
                        var parkingData = new ParkingData
                        {
                            Seq = item.GetProperty("seq").GetInt32(),
                            Area = item.GetProperty("行政區").GetString(),
                            ParkingSpot = item.GetProperty("臨時停車處所").GetString(),
                            SmallSlots = item.GetProperty("可提供小型車停車位").GetString(),
                            Address = item.GetProperty("地址").GetString()
                        };
                        parkingDatas.Add(parkingData);
                        areaSet.Add(parkingData.Area);
                    }

                    // 處理提取的資料
                    if (parkingDatas.Count > 0)
                    {
                        // 根據資料的欄位數量動態新增欄位
                        listView1.Columns.Add("Seq", 35, HorizontalAlignment.Left);
                        listView1.Columns.Add("行政區", 60, HorizontalAlignment.Left);
                        listView1.Columns.Add("臨時停車處所", 145, HorizontalAlignment.Left);
                        listView1.Columns.Add("可提供小型車停車位", 120, HorizontalAlignment.Left);
                        listView1.Columns.Add("地址", 200, HorizontalAlignment.Left);

                        foreach (var row in parkingDatas)
                        {
                            Debug.WriteLine($"{row.Seq}, {row.Area}, {row.ParkingSpot}, {row.SmallSlots}, {row.Address}");
                        }

                        // 新增資料到 ListView
                        foreach (var row in parkingDatas)
                        {
                            var listViewItem = new ListViewItem(new string[] {
                                row.Seq.ToString(),
                                row.Area,
                                row.ParkingSpot,
                                row.SmallSlots.ToString(),
                                row.Address
                            });
                            listView1.Items.Add(listViewItem);
                        }

                        // 填充 ComboBox 的選項
                        comboBox1.Items.AddRange(areaSet.ToArray());
                    }
                }
            }
        }

        private ComboBox comboBox1;
        private Button button2;
    }

    public class ParkingData
    {
        public int Seq { get; set; }
        public string Area { get; set; }
        public string ParkingSpot { get; set; }
        public string SmallSlots { get; set; }
        public string Address { get; set; }
    }
}
