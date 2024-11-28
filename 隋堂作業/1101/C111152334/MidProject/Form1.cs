namespace MidProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateListView();
        }

        private void UpdateListView()
        {
            if (parkingDatas == null)
            {
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                return;
            }

            string selectedArea = comboBox1.SelectedItem?.ToString();
            listView1.Items.Clear();

            foreach (var row in parkingDatas)
            {
                if (row.Area == selectedArea)
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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            listView1.Items.Clear();
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
        }
    }
}
