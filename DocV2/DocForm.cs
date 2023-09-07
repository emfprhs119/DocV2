
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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
        ListView loadDocView;
        ListView searchView;
        PdfView pdfView;
        Dictionary<string,string> headerInfo;
        String configFileName;
        public DocForm(String arg)
        {

            FontRegistryWrapper.ReadRegistry();
            InitializeComponent();
            //MinimumSize = new System.Drawing.Size(884, 600);
            formName = arg;
            if (arg == "견적서")
                configFileName = @"resources\estimate.json";
            else if (arg == "거래명세서")
                configFileName = @"resources\specification.json";
            sheetControl = new SheetControl(this,reoGrid, label23, this.CreateGraphics(),7,true);
            sheetControl.AttachMenu(new ToolStripMenuItem[] { menu_RowAdd,menu_RowDelete,menu_DataRemove,menu_Cut,menu_Copy,menu_Paste});
            headerInfo = new Dictionary<string, string>();
            exportPDF = new ExportPDF(this.CreateGraphics(),arg);
            loadDocView = new ListView(this,ListView.ViewType.DOC);
            searchView = new ListView(this, ListView.ViewType.ITEM);
            pdfView = new PdfView(this,exportPDF);
            searchView.SetRefSheet(sheetControl);
            KeyPreview = true;
        }

        private void DocForm_Load(object sender, EventArgs e)
        {
            InitPath(false);
            InitConfig();
            InitSupply();
            InitForm();
            InitFunc();
            formWidth = int.Parse(Properties.Settings.Default["Width"].ToString());
            sheetControl.FormResizeOrigin(formWidth);
            this.Width = formWidth;
            this.Height = int.Parse(Properties.Settings.Default["Height"].ToString());
            SystemFontChange();
            this.ActiveControl = reoGrid;
        }

        private void InitConfig()
        {
            string jsonString = File.ReadAllText(configFileName);
            int columnCount=0;
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement docElement = document.RootElement;
                //foreach (JsonElement docElement in root.EnumerateArray())
                //{
                    formName = docElement.GetProperty("Form").GetString();
                    this.Text = formName;
                    //headLabel.Text = formName;
                    if (docElement.TryGetProperty("Orderer", out JsonElement OrdererElement))
                        InitOrder(OrdererElement);
                    if (docElement.TryGetProperty("Column", out JsonElement columnElements))
                        sheetControl.InitSheetStyle(columnElements);
                    columnCount = columnElements.GetArrayLength();
                //}
            }

            SQLiteWrapper.PrepareDB(columnCount);
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
        }
        private void InitForm()
        {
            if (formName == "거래명세서")
            {
                int topMargin = 65;
                //panel4.Top += topMargin;
                //panel4.Height -= topMargin;
                //label23.Top -= topMargin;
                //label24.Top -= topMargin;
            }
            Title.Text = formName;
            Title.Left = Width / 2 - Title.Width / 2;
            툴바ToolStripMenuItem.Checked = Boolean.Parse(Properties.Settings.Default["ToolbarEnabled"].ToString());
            toolStrip1.Visible = 툴바ToolStripMenuItem.Checked;
            label24.Top = (label23.Top + label23.Height / 2) - (label24.Height / 2);
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                systemFontDropdown.Items.Add(font.Name);
            }
            CompanyDropDownRefresh();
        }
        private void CompanyDropDownRefresh()
        {
            DataTable table = SQLiteWrapper.GetDataTable("select DISTINCT info_2 from doc order by info_2");
            t2.Items.Clear();
            foreach (DataRow dataRow in table.Rows)
                t2.Items.Add(dataRow[0]);
        }
        private void InitFunc()
        {
            void newDocFunc(object sender, EventArgs e)
            {
                if (!SaveQuestion())
                    return;
                sheetControl.Clear();
                t1.Value = DateTime.Today;
                t2.Text = "";
                t3.Text = "";
                t4.Text = "";
                docID = null;
                this.Text = formName;
                sheetControl.isModified = false;
                sheetControl.Space();
            }
            void loadDocFunc(object sender, EventArgs e)
            {
                if (!loadDocView.Visible)
                    loadDocView.Show();
            };
            void searchDocBtnFunc(object sender, EventArgs e)
            {
                if (!searchView.Visible)
                    searchView.Show();
            };
            void saveDocFunc(object sender, EventArgs e)
            {
                SaveDoc();
            };
            void previewFunc(object sender, EventArgs e)
            {
                pdfView.Show(true);
            };
            void printDocFunc(object sender, EventArgs e)
            {

                //SaveDoc();
                //fullPath
                string fullPath = SaveDoc(true);
                if (fullPath == null)
                    return;
                PdfDocument pdfDoc = PdfDocument.Load(fullPath);
                PrintDocument pd = pdfDoc.CreatePrintDocument();
                PrintDialog dialog = new PrintDialog();
                dialog.PrinterSettings = pd.PrinterSettings;
                DialogResult result = dialog.ShowDialog();
                if (result.Equals(DialogResult.OK))
                {
                    //pd.DocumentName = "fileName";
                    pd.PrinterSettings = dialog.PrinterSettings;
                    //pd.PrinterSettings.PrintFileName = fullPath;
                    pd.Print();
                }
                pdfDoc.Dispose();
                dialog.Dispose();
                pd.Dispose();
            }
            테이블폰트ToolStripMenuItem.Click += (object sender, EventArgs e) =>
            {
                FontDialog dialog = new FontDialog();
                dialog.Font = new Font(Properties.Settings.Default["SheetFontName"].ToString(), float.Parse(Properties.Settings.Default["SheetFontSize"].ToString()));
                DialogResult result = dialog.ShowDialog();
                if (result.Equals(DialogResult.OK))
                {
                    sheetControl.SheetFontChange(dialog.Font);
                }
            };
            출력폰트ToolStripMenuItem.Click += (object sender, EventArgs e) =>
            {
                pdfView.Show(false);
            };

            newDocBtn.Click += newDocFunc;
            새문서ToolStripMenuItem.Click += newDocFunc;
            loadDocBtn.Click += loadDocFunc;
            불러오기ToolStripMenuItem.Click += loadDocFunc;
            searchDocBtn.Click += searchDocBtnFunc;
            검색하기ToolStripMenuItem.Click += searchDocBtnFunc;
            saveDocBtn.Click += saveDocFunc;
            저장하기ToolStripMenuItem.Click += saveDocFunc;
            previewBtn.Click += previewFunc;
            미리보기ToolStripMenuItem.Click += previewFunc;
            printDocBtn.Click += printDocFunc;
            인쇄하기ToolStripMenuItem.Click += printDocFunc;
            경로초기화ToolStripMenuItem.Click += (object sender, EventArgs e) =>
            {
                InitPath(true);
            };
        }

        private bool SaveQuestion()
        {
            if (sheetControl.isModified)
            {
                DialogResult result = MessageBox.Show("변경된 내용이 있습니다.\n" + "변경 내용을 저장하시겠습니까?", "저장", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:return SaveDoc()!=null;
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
                loadDocView.Hide();
                return;
            }
            if (!SaveQuestion())
                return;
            string loadData = SQLiteWrapper.LoadDoc(key);
            
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
                t1.Location = new Point(t1.Location.X, t1.Location.Y + 5);
                t2.Location = new Point(t2.Location.X, t2.Location.Y + 15);
                o3.Visible = false;
                t3.Visible = false;
                o4.Visible = false;
                t4.Visible = false;
            }
            o1.Location = new Point(o1.Location.X, (t1.Location.Y + t1.Height / 2) - (o1.Height / 2));
            o2.Location = new Point(o2.Location.X, (t2.Location.Y + t2.Height / 2) - (o2.Height / 2));
            o3.Location = new Point(o3.Location.X, (t3.Location.Y + t3.Height / 2) - (o3.Height / 2));
            o4.Location = new Point(o4.Location.X, (t4.Location.Y + t4.Height / 2) - (o4.Height / 2));
        }


        private void PrintError(string str)
        {
            MessageBox.Show(str);
        }

        private void DocForm_Resize(object sender, EventArgs e)
        {
            var scale = (float)(this.Width) / formWidth;
            reoGridPanel.Height = this.Height- (label23.Bottom+38);
            if (formWidth == 0)
                scale = 1;
            if (sheetControl != null) {
                sheetControl.FormResize(scale);
            }
            formWidth = this.Width;
            Properties.Settings.Default["Width"] = this.Width.ToString();
            Properties.Settings.Default["Height"] = this.Height.ToString();
            Properties.Settings.Default.Save();
        }


        public string SaveDoc(bool isPreview = false)
        {
            if (t2.Text == "" && !isPreview)
            {
                MessageBox.Show("거래처가 입력되지 않았습니다.");
                return null;
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
                string saveKey = docID;
                if (!isPreview)
                    saveKey = SQLiteWrapper.AddDoc(formName, saveKey, new string[] { configDataString, t1.Text, t2.Text, t3.Text, t4.Text }, extractItem);
                else
                    saveKey = "tmp";
                var fullPath = exportPDF.Save(saveKey, headerInfo, columnWidths, extractItem, sheetControl.SumValues(), sheetControl.TotalString());
                if (!isPreview)
                {
                    docID = saveKey;
                    this.Text = docID;
                    AutoClosingMessageBox.Show("저장되었습니다.", formName, 1000);
                    sheetControl.isModified = false;
                }
                CompanyDropDownRefresh();
                return fullPath;
            }
            catch
            {
                MessageBox.Show("저장이 실패하였습니다.");
                return null;
            }
        }


        private void DocForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveQuestion())
            {
                e.Cancel = true;
            }
        }

        private void UserField_DoubleClick(object sender, EventArgs e)
        {
            Label label = ((Label)sender);
            string title = "";
            string text = label.Text;
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
            System.Drawing.Size size = new System.Drawing.Size(400, 70);
            Form inputBox = new Form();
            inputBox.MinimizeBox = false;
            inputBox.MaximizeBox = false;
            inputBox.StartPosition = FormStartPosition.CenterScreen;
            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new Point(5,5);
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

        internal string ReadToday()
        {
            string[] stn = t1.Text.Split('-');
            return stn[1] + "." + stn[2];
        }


        private void systemFontDropdown_TextChanged(object sender, EventArgs e)
        {
            ToolStripComboBox dropdown = (ToolStripComboBox)sender;
            if (dropdown.Items.Contains(dropdown.Text)){
                Properties.Settings.Default["SystemFontName"] = dropdown.Text;
                Properties.Settings.Default.Save();
                SystemFontChange();
            }
        }
        void SystemFontChange()
        {
            //systemFontDropdown.Text = Properties.Settings.Default["SystemFontName"].ToString();
            SystemFontModifySub(Properties.Settings.Default["SystemFontName"].ToString(),this);
        }
        void SystemFontModifySub(string fontName,Control control)
        {
            if (control.Controls.Count == 0)
            {
                if (control is ToolStrip)
                {
                    for (int i = 0; i < ((ToolStrip)control).Items.Count; i++) { 
                        ((ToolStrip)control).Items[i].Font = new Font(fontName, ((ToolStrip)control).Items[i].Font.Size);
                    }
                }
                else
                {
                    control.Font = new Font(fontName, control.Font.Size);
                }
            }
            else
            {
                for (int i = 0; i < control.Controls.Count; i++)
                {
                    SystemFontModifySub(fontName,control.Controls[i]);
                }
            }
        }

        private void DocForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                SaveDoc();
        }

        private void StampMenu_Click(object sender, EventArgs e)
        {
            /*
            StampView sv = new StampView();
            sv.Controls.
            new StampView().ShowDialog();

            
            Form stampForm = new Form();
            stampForm.Size = new Size(250, 290);
            Button imgBtn = new Button();
            imgBtn.Image = Image.FromFile(@"D:\D_DEVELOP\Develop\PP.JPG");
            imgBtn.Dock = DockStyle.Fill;
            Panel panel = new Panel();
            panel.Height = 40;
            panel.Dock = DockStyle.Bottom;
            Button modifBtn = new Button();
            modifBtn.Text = "변경";
            modifBtn.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            modifBtn.Dock = DockStyle.Left;
            Button removeBtn = new Button();
            removeBtn.Text = "제거";
            removeBtn.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            removeBtn.Dock = DockStyle.Right;
            panel.Controls.Add(modifBtn);
            panel.Controls.Add(removeBtn);

            stampForm.Controls.Add(imgBtn);
            stampForm.Controls.Add(panel);
            stampForm.ShowDialog();
            */

        }

        private void 설정초기화ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 툴바ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            툴바ToolStripMenuItem.Checked = !툴바ToolStripMenuItem.Checked;
            toolStrip1.Visible = 툴바ToolStripMenuItem.Checked;
        }


        private static void InitPath(bool reset)
        {
            if (!reset && Properties.Settings.Default.path != "")
            {
                return;
            }
            DialogResult dialogResult;
            dialogResult = MessageBox.Show("Data 저장 경로를 선택해주세요.", "데이터베이스", MessageBoxButtons.OK);
            if (dialogResult == DialogResult.OK)
            {
                FolderBrowserDialog folderBrowser = new FolderBrowserDialog
                {
                    Description = "견적서 및 거래명세서 데이터를 저장할 경로를 선택하세요.",
                    RootFolder = Environment.SpecialFolder.Desktop
                };
                dialogResult = folderBrowser.ShowDialog();
                if (dialogResult != DialogResult.Cancel)
                {
                    Properties.Settings.Default.path = folderBrowser.SelectedPath;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
