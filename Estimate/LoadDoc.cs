using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimate
{
    public partial class LoadDoc : Form
    {
        DocForm docForm;
        public LoadDoc(DocForm docForm)
        {
            InitializeComponent();
            this.docForm = docForm;
            dataGridView1.Font = new Font("맑은 고딕", 8);
        }

        private void LoadDoc_Load(object sender, EventArgs e)
        {

            string query = "select info_1 as '   날짜',info_2 as '   상호',num as '번호' from doc ";
            dataGridView1.DataSource = SQLiteWrapper.GetDataTable(query);
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = dataGridView1.Width - 80 - 40-10;
            dataGridView1.Columns[2].Width = 40;
            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.DoubleClick += DataGridView1_DoubleClick;
            
        }

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            docForm.Load_fromDB(dataGridView1.SelectedRows[0].Cells[0].Value + "_" + dataGridView1.SelectedRows[0].Cells[1].Value + "_" + dataGridView1.SelectedRows[0].Cells[2].Value);
            this.Close();
        }
    }
}
