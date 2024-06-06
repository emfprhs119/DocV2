using System;
using System.Data;
using System.Windows.Forms;
using unvell.ReoGrid.Events;

namespace DocV2
{
    public partial class ListView : Form
    {
        public enum ViewType { DOC,ITEM };
        DocForm docForm;
        ViewType viewType;
        SheetControl sheetControl;
        SheetControl refSheetControl;
        int formWidth;
        public ListView(DocForm docForm,ViewType viewType)
        {
            InitializeComponent();
            formWidth = Width;
            sheetControl = new SheetControl(null,reoGrid, null, null, null, this.CreateGraphics(),1,false);
            KeyPreview = true;
            KeyDown += (object sender, KeyEventArgs e)=> { if (e.KeyCode == Keys.F5) RefreshData(); };
            this.docForm = docForm;
            this.viewType = viewType;

            switch (viewType)
            {
                case ViewType.DOC:
                    Width = 390;
                    Height = 600;
                    sheetControl.SetSelectionMode(unvell.ReoGrid.WorksheetSelectionMode.Cell);
                    reoGrid.DoubleClick += button1_Click;
                    break;
                case ViewType.ITEM:
                    Width = 1000;
                    Height = 600;
                    bottomPanel.Dispose();
                    break;
            }
            sheetControl.RegistFunc(CellEvent);
        }


        void CellEvent(object sender, CellMouseEventArgs e)
        {
            if (e.Clicks == 2)
                docForm.Load_fromDB(GetId());
        }

        private void LoadDoc_Load(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
            string query = null;
            switch (viewType)
            {
                case ViewType.DOC:
                    query = "select info_1 as '날짜',info_2 as '상호',num as '번호' from doc where docID LIKE '" + docForm.formName + "%'";
                    query += " and (info_1 LIKE '%" + searchBox.Text + "%'";
                    query += " OR info_2 LIKE '%" + searchBox.Text + "%')";
                    query+=" order by info_1 desc,info_2,num";
                    break;
                case ViewType.ITEM:
                    query = "select doc.info_1 as '발행일자',doc.info_2 as '상호'";
                    string []names = refSheetControl.GetColumnHeaderNames();
                    for (int i=0; i<names.Length; i++)
                        query += ",column_" + i + " as '" + names[i] + "'";
                    query += " from doc,item where doc.docId=item.docId and doc.docID LIKE '" + docForm.formName + "%' and (";
                    query += " info_1 LIKE '%" + searchBox.Text + "%' or";
                    query += " info_2 LIKE '%" + searchBox.Text + "%'";
                    for (int i = 0; i < names.Length; i++)
                        query += " or column_" + i+" LIKE '%" + searchBox.Text + "%'";
                    query += ")";
                    break;
            }

            DataTable dataTable = SQLiteWrapper.GetDataTable(query);
            sheetControl.Clear();
            sheetControl.PasteData(dataTable);

            if (sheetControl.GetColumnWidths() == null)
            {
                float[] p;
                switch (viewType)
                {
                    case ViewType.DOC:
                        p = new float[3];
                        p[0] = 80; p[1] = 120; p[2] = 30;
                        sheetControl.SetColumnWidths(p);
                        break;
                    case ViewType.ITEM:
                        float[] columnWidths = refSheetControl.GetColumnWidths();
                        p = new float[dataTable.Columns.Count];
                        p[0] = 80; p[1] = 120;
                        for (int i = 2; i < p.Length; i++)
                            p[i] = columnWidths[i - 2];
                        sheetControl.SetColumnWidths(p);
                        break;
                }
            }
            sheetControl.SheetFontChange();
            sheetControl.EnableFilter();
            docForm.CompanyDropDownRefresh();
        }
        internal void SetRefSheet(SheetControl sheetControl)
        {
            this.refSheetControl = sheetControl;
        }

        public new void Show()
        {
            RefreshData();
            base.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            docForm.Load_fromDB(GetId());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SQLiteWrapper.DeleteDoc(GetId());
            sheetControl.DeleteSelectionRowLine();

            //searchBox.Text = "";
            //RefreshData();
        }
        private string GetId()
        {
            string subId = sheetControl.GetSelectionRowLine();
            if (subId == "__")
                return "";
            else
                return docForm.formName + "-" + subId;
        }

        private void LoadDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            searchBox.Text = "";
            e.Cancel = true;
            docForm.BringToFront();
            docForm.Activate();
            Hide();
        }
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void DocView_Resize(object sender, EventArgs e)
        {
            var scale = (float)(this.Width) / formWidth;
            sheetControl.FormResize(scale);
            formWidth = this.Width;
        }
    }
}
