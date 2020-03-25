using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using unvell.ReoGrid;

namespace Estimate
{
    public partial class DocForm : Form
    {

        ExportPDF exportPDF;
        SheetControl sheetControl;
        string formName;
        int formWidth;
        string docID;
        LoadDoc loadDoc;

        public DocForm()
        {
            InitializeComponent();
            sheetControl = new SheetControl(this,reoGrid, label23, this.CreateGraphics());
            sheetControl.AttachMenu(new ToolStripMenuItem[] { menu_RowAdd,menu_RowDelete,menu_DataRemove,menu_Cut,menu_Copy,menu_Paste});
            exportPDF = new ExportPDF();
            loadDoc = new LoadDoc(this);
        }

        private void DocForm_Load(object sender, EventArgs e)
        {
            InitConfig();
            formWidth = this.Width;
            this.ActiveControl = reoGrid;
        }

        private void InitConfig()
        {
            string jsonString = File.ReadAllText("doc.config");
            int orderCount=0;
            int columnCount=0;
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                foreach (JsonElement docElement in root.EnumerateArray())
                {
                    formName = docElement.GetProperty("Form").GetString();
                    this.Text = formName;
                    if (docElement.TryGetProperty("Supply", out JsonElement supplyElement))
                        InitSupply(supplyElement);
                    if (docElement.TryGetProperty("Orderer", out JsonElement OrdererElement))
                        InitOrder(OrdererElement);
                    orderCount = OrdererElement.GetArrayLength();
                    if (docElement.TryGetProperty("Column", out JsonElement columnElements))
                        sheetControl.InitSheetStyle(columnElements);
                    columnCount = columnElements.GetArrayLength();
                }
            }

            SQLiteWrapper.PrepareDB(1+ orderCount, columnCount);
        }
        private bool SaveQuestion(string message)
        {
            if (sheetControl.isModified)
            {
                DialogResult result = MessageBox.Show("수정된 내용이 있습니다. "+message, formName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:SaveDoc();break;
                    case DialogResult.No:break;
                    default:return false;
                }
        }
            return true;
        }
        internal void Load_fromDB(string key)
        {
            if (!SaveQuestion("\n저장 후 여시겠습니까?"))
                return;

            string loadData = SQLiteWrapper.LoadDoc(key);
            Console.WriteLine(loadData);
            string[] sn = loadData.Split(new string[]{ ",SplitText"}, StringSplitOptions.None);

            sheetControl.Clear();
            string[] sn0 = sn[0].Split(',');
            float[] columnWidths = sheetControl.GetColumnWidths();
            int i = 0;
            while (i < columnWidths.Length) {
                columnWidths[i] = float.Parse(sn0[i]);
                i++;
            }
            var segment = new ArraySegment<string>(sn0, i, sn0.Length-i);
            LoadOrderData(segment.ToArray());
            sheetControl.SetColumnWidths(columnWidths);
            sheetControl.PasteData(sn[1], new RangePosition("A1"));
            docID = key;
            this.Text = formName+"_"+docID;
            sheetControl.isModified = false;
            loadDoc.Hide();
        }

        private void InitSupply(JsonElement supplyElement)
        {
            s1.Text = supplyElement.GetProperty("등록번호").GetString();
            s2.Text = supplyElement.GetProperty("상호").GetString();
            s3.Text = supplyElement.GetProperty("성명").GetString();
            s4.Text = supplyElement.GetProperty("주소").GetString();
            s5.Text = supplyElement.GetProperty("업태").GetString();
            s6.Text = supplyElement.GetProperty("종목").GetString();
            s7.Text = supplyElement.GetProperty("전화").GetString();
            s8.Text = supplyElement.GetProperty("팩스").GetString();
        }
        private void LoadOrderData(string[] baseInfo)
        {
            int i = 0;
            foreach (string orderKey in baseInfo)
            {
                if (i == 0)
                    t1.Text = orderKey;
                if (i == 1)
                    t2.Text = orderKey;
                if (i == 2)
                    t3.Text = orderKey;
                if (i == 3)
                    t4.Text = orderKey;
                i++;
            }
        }
        private void InitOrder(JsonElement ordererElement)
        {
            int i = 0;
            foreach (JsonElement orderKey in ordererElement.EnumerateArray())
            {
                if (i==0)
                    o1.Text = orderKey.GetString();
                if (i==1)
                    o2.Text = orderKey.GetString();
                if (i == 2)
                    o3.Text = orderKey.GetString();
                if (i == 3)
                    o4.Text = orderKey.GetString();
                i++;
            }
            if (i == 2)
            {
                o3.Visible = false;
                t3.Visible = false;
                o4.Visible = false;
                t4.Visible = false;
            }
        }


        private void PrintError(string str)
        {
            MessageBox.Show(str);
        }

        private void DocForm_Resize(object sender, EventArgs e)
        {
            if (this.Width < 800)
                this.Width = 800;
            var scale = (float)(this.Width) / formWidth;
            sheetControl.FormResize(scale);
            
            formWidth = this.Width;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveDoc();
        }

        private void SaveDoc()
        {
            string[][] extractItem = sheetControl.ExtractItemArray();
            float[] columnWidths = sheetControl.GetColumnWidths();
            string configDataString = "";
            for (int i = 0; i < columnWidths.Length; i++)
            {
                configDataString += columnWidths[i].ToString();
                if (i < columnWidths.Length - 1)
                {
                    configDataString += ",";
                }
            }
            try
            {
                docID = SQLiteWrapper.AddDoc(docID, new string[] { configDataString, t1.Text, t2.Text, t3.Text, t4.Text }, extractItem);
                exportPDF.Save("견적서_" + docID, columnWidths, extractItem, sheetControl.TotalString());
                this.Text = formName + " - " + docID;
                AutoClosingMessageBox.Show("저장되었습니다.", formName, 1000);
                //MessageBox.Show("저장되었습니다.");
                sheetControl.isModified = false;
            }
            catch
            {
                MessageBox.Show("저장이 실패하였습니다.");
            }
        }

        private void b2_Click(object sender, EventArgs e)
        {
            if (!loadDoc.Visible)
            {
                loadDoc.Show();
            }
        }

        private void b1_Click(object sender, EventArgs e)
        {
            var process = Process.GetCurrentProcess(); // Or whatever method you are using
            string fullPath = process.MainModule.FileName;
            Process.Start(fullPath);
        }

        private void DocForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveQuestion("\n저장 후 종료하시겠습니까?"))
            {
                e.Cancel = true;
            }
        }
    }
}
