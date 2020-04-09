using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using unvell.ReoGrid;

namespace DocV2
{
    public partial class DocForm : Form
    {
        ExportPDF exportPDF;
        SheetControl sheetControl;
        public string formName;
        int formWidth;
        string docID;
        LoadDoc loadDoc;
        LoadCompany loadCompany;
        Dictionary<string,string> headerInfo;
        String configFileName;
        public DocForm(String arg)
        {
            InitializeComponent();
            
            if (arg == "견적서")
                configFileName = @"resources\estimate.json";
            else if (arg == "거래명세서")
                configFileName = @"resources\specification.json";
            sheetControl = new SheetControl(this,reoGrid, label23, this.CreateGraphics());
            sheetControl.AttachMenu(new ToolStripMenuItem[] { menu_RowAdd,menu_RowDelete,menu_DataRemove,menu_Cut,menu_Copy,menu_Paste});
            headerInfo = new Dictionary<string, string>();
            exportPDF = new ExportPDF();
            loadDoc = new LoadDoc(this);
            loadCompany = new LoadCompany(t2);
        }
        private void DocForm_Load(object sender, EventArgs e)
        {
            InitConfig();
            InitSupply();
            formWidth = this.Width;
            this.ActiveControl = reoGrid;
        }

        private void InitConfig()
        {
            string jsonString = File.ReadAllText(configFileName);
            int columnCount=0;
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                foreach (JsonElement docElement in root.EnumerateArray())
                {
                    formName = docElement.GetProperty("Form").GetString();
                    this.Text = formName;
                    headLabel.Text = formName;
                    b1.Text = "새\n"+formName;
                    if (docElement.TryGetProperty("Orderer", out JsonElement OrdererElement))
                        InitOrder(OrdererElement);
                    if (docElement.TryGetProperty("Column", out JsonElement columnElements))
                        sheetControl.InitSheetStyle(columnElements);
                    columnCount = columnElements.GetArrayLength();
                }
            }

            SQLiteWrapper.PrepareDB(columnCount);
        }
        private bool SaveQuestion(string message)
        {
            if (sheetControl.isModified)
            {
                DialogResult result = MessageBox.Show("수정된 내용이 있습니다.\n" + message, formName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:return SaveDoc();
                    case DialogResult.No:break;
                    default:return false;
                }
        }
            return true;
        }
        internal void Load_fromDB(string key)
        {
            if (key == "")
            {
                loadDoc.Hide();
                return;
            }
            if (!SaveQuestion("저장 후 여시겠습니까?"))
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
            this.Text = docID;
            sheetControl.isModified = false;
            loadDoc.Hide();
        }

        private void InitSupply()
        {
            s1.Text = Properties.Settings.Default["s1"].ToString();
            s2.Text = Properties.Settings.Default["s2"].ToString();
            s3.Text = Properties.Settings.Default["s3"].ToString();
            s4.Text = Properties.Settings.Default["s4"].ToString();
            s5.Text = Properties.Settings.Default["s5"].ToString();
            s6.Text = Properties.Settings.Default["s6"].ToString();
            s7.Text = Properties.Settings.Default["s7"].ToString();
            s8.Text = Properties.Settings.Default["s8"].ToString();

            /*
            s1.Text = kv["등록번호"];
            s2.Text = kv["상호"];
            s3.Text = kv["성명"];
            s4.Text = kv["주소"];
            s5.Text = kv["업태"];
            s6.Text = kv["종목"];
            s7.Text = kv["전화"];
            s8.Text = kv["팩스"];
            */
        }
        private void ExtractOrder()
        {
            headerInfo[o1.Text] = t1.Text;
            headerInfo[o2.Text] = t2.Text;
            headerInfo[o3.Text] = t3.Text;
            headerInfo[o4.Text] = t4.Text;
            headerInfo["등록번호"] = s1.Text;
            headerInfo["상호"] = s2.Text;
            headerInfo["성명"] = s3.Text;
            headerInfo["주소"] = s4.Text;
            headerInfo["업태"] = s5.Text;
            headerInfo["종목"] = s6.Text;
            headerInfo["전화"] = s7.Text;
            headerInfo["팩스"] = s8.Text;
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

        private bool SaveDoc()
        {
            if (t2.Text == "")
            {
                MessageBox.Show("거래처가 입력되지 않았습니다.");
                return false;
            }
            ExtractOrder();
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
                docID = SQLiteWrapper.AddDoc(formName,docID, new string[] { configDataString, t1.Text, t2.Text, t3.Text, t4.Text }, extractItem);
                if (formName == "견적서")
                    exportPDF.Save(formName,docID, headerInfo, columnWidths, extractItem, sheetControl.SumValues(), sheetControl.TotalString());
                else if (formName == "거래명세서")
                    exportPDF.Save(formName,docID, headerInfo, columnWidths, extractItem, sheetControl.SumValues(), sheetControl.TotalString());
                this.Text = docID;
                AutoClosingMessageBox.Show("저장되었습니다.", formName, 1000);
                sheetControl.isModified = false;
                return true;
            }
            catch
            {
                MessageBox.Show("저장이 실패하였습니다.");
                return false;
            }
        }

        private void b2_Click(object sender, EventArgs e)
        {
            if (!loadDoc.Visible)
                loadDoc.Show();
        }

        private void b1_Click(object sender, EventArgs e)
        {
            var process = Process.GetCurrentProcess(); // Or whatever method you are using
            string fullPath = process.MainModule.FileName;
            Process.Start(fullPath, formName);
        }

        private void DocForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveQuestion("저장 후 종료하시겠습니까?"))
            {
                e.Cancel = true;
            }
        }

        private void loadCompanyBtn_Click(object sender, EventArgs e)
        {
            if (!loadCompany.Visible)
                loadCompany.Show();
        }

        private void UserField_DoubleClick(object sender, EventArgs e)
        {
            Label label = ((Label)sender);
            string title = "";
            string text = "";
            switch (label.Name)
            {
                case "s1":title = "등록번호";break;
                case "s2": title = "상호"; break;
                case "s3": title = "성명"; break;
                case "s4": title = "주소"; break;
                case "s5": title = "업태"; break;
                case "s6": title = "종목"; break;
                case "s7": title = "전화"; break;
                case "s8": title = "팩스"; break;
            }
            DialogResult result = ShowInputDialog(title, ref text);
            if (result == DialogResult.OK)
            {
                label.Text = text;
                Properties.Settings.Default[label.Name] = text;
                Properties.Settings.Default.Save();
            }
        }

        private static DialogResult ShowInputDialog(string title,ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();
            inputBox.MinimizeBox = false;
            inputBox.MaximizeBox = false;
            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

    }
}
