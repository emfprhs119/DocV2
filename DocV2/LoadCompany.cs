using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DocV2
{
    public partial class LoadCompany : Form
    {
        TextBox t2;
        public LoadCompany(TextBox t2)
        {
            InitializeComponent();
            this.t2 = t2;
            dataGridView1.Font = new Font("맑은 고딕", 8);
            dataGridView1.DoubleClick += DataGridView1_DoubleClick;
        }


        private void LoadCompany_Load(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
            string query = "select DISTINCT info_2 as '   상호' from doc order by info_2";
            dataGridView1.DataSource = SQLiteWrapper.GetDataTable(query);
            
            dataGridView1.Columns[0].Width = 100;

            int modif = 0;
            if (dataGridView1.Controls.OfType<VScrollBar>().First().Visible)
                modif = SystemInformation.VerticalScrollBarWidth;
            dataGridView1.Columns[0].Width = dataGridView1.Width - 3 - modif;
            dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
        }


        public new void Show()
        {
            RefreshData();
            base.Show();
        }

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            t2.Text = GetText();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t2.Text = GetText();
            this.Hide();
        }

        private string GetText()
        {
            if (dataGridView1.Rows.Count > 0)
                return dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            else
                return "";
        }

        private void LoadCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
