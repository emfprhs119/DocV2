using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DocV2
{
    public partial class LoadDoc : Form
    {
        DocForm docForm;
        public LoadDoc(DocForm docForm)
        {
            InitializeComponent();
            this.docForm = docForm;
            dataGridView1.Font = new Font("맑은 고딕", 8);
            dataGridView1.DoubleClick += DataGridView1_DoubleClick;
        }


        private void LoadDoc_Load(object sender, EventArgs e)
        {
            RefreshData(null);
        }
        private void RefreshData(string query)
        {
            if (query == null || query == "")
                query = "select info_1 as '   날짜',info_2 as '   상호',num as '번호' from doc where docID LIKE '"+docForm.formName+"%' order by info_1 desc,info_2,num";
            dataGridView1.DataSource = SQLiteWrapper.GetDataTable(query);
            dataGridView1.Columns[0].Width = 80;

            int modif = 0;
            if (dataGridView1.Controls.OfType<VScrollBar>().First().Visible)
                modif = SystemInformation.VerticalScrollBarWidth;
            dataGridView1.Columns[1].Width = dataGridView1.Width - 80 - 40 - 3 - modif;
            dataGridView1.Columns[2].Width = 40;
            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

        }


        public new void Show()
        {
            RefreshData(null);
            base.Show();
        }

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            docForm.Load_fromDB(GetId());
            //this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            docForm.Load_fromDB(GetId());
            //this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SQLiteWrapper.DeleteDoc(GetId());
            searchBox.Text = "";
            RefreshData(null);
        }
        private string GetId()
        {
            if (dataGridView1.Rows.Count > 0)
                return docForm.formName+"-"+dataGridView1.SelectedRows[0].Cells[0].Value + "_" + dataGridView1.SelectedRows[0].Cells[1].Value + "_" + dataGridView1.SelectedRows[0].Cells[2].Value;
            else
                return "";
        }

        private void LoadDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            searchBox.Text = "";
            e.Cancel = true;
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "select doc.info_1 as '   날짜',doc.info_2 as '   상호',num as '번호' from doc,item where doc.docId=item.docId and "+
                "(item.column_0 like '%"+searchBox.Text+ "%' or  item.column_1 like '%" + searchBox.Text + "%' or item.column_2 like '%" + searchBox.Text + "%') "
                + "and doc.docID LIKE '" + docForm.formName + "%' order by doc.info_1 desc,doc.info_2,doc.num";

            RefreshData(query);
        }
    }
}
