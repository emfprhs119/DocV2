using CefSharp.WinForms;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Text.Json.JsonElement;

namespace DocV2
{
    public partial class PdfView : Form
    {
        string target;
        ChromiumWebBrowser browser;
        DocForm docForm;
        ExportPDF exportPDF;
        Dictionary<string, string> headerInfo;
        string[] sumValues;
        string totalValues;
        string[][] extractItem;
        float[] columnWidths;
        public PdfView(DocForm docForm, ExportPDF exportPDF)
        {
            InitializeComponent();
            Init(docForm.formName);
            this.docForm = docForm;
            this.exportPDF = exportPDF;
            browser = new ChromiumWebBrowser();
            browser.Dock = DockStyle.Fill;
            panel2.Controls.Add(browser);
            accept.Click += (object sender, EventArgs e) => {
                try
                {
                    SetFont();
                    Properties.Settings.Default[docForm.formName+"ExportHeaderFontName"] = headerFontCombo.Text;
                    Properties.Settings.Default[docForm.formName + "ExportHeaderFontSize"] = headerSizeCombo.Text;
                    Properties.Settings.Default[docForm.formName + "ExportTableFontName"] = tableFontCombo.Text;
                    Properties.Settings.Default[docForm.formName + "ExportTableFontSize"] = tableSizeCombo.Text;
                    Properties.Settings.Default.Save();
                    UpdatePdf();
                }catch
                {
                    AutoClosingMessageBox.Show("정상적인 폰트 및 크기를 입력해주세요.", "오류", 3000);
                }
            };
            
            foreach (string fontName in FontRegistryWrapper.fontKV.Keys)
            {
                headerFontCombo.Items.Add(fontName);
                tableFontCombo.Items.Add(fontName);
            }
            
            for (float f = 3; f < 12.5; f += 0.5f)
            {
                headerSizeCombo.Items.Add(f);
                tableSizeCombo.Items.Add(f);
            }

            headerFontCombo.Text = Properties.Settings.Default[docForm.formName + "ExportHeaderFontName"].ToString();
            headerSizeCombo.Text = Properties.Settings.Default[docForm.formName + "ExportHeaderFontSize"].ToString();
            tableFontCombo.Text = Properties.Settings.Default[docForm.formName + "ExportTableFontName"].ToString();
            tableSizeCombo.Text = Properties.Settings.Default[docForm.formName + "ExportTableFontSize"].ToString();
            SetFont();
            this.FormClosing += PdfView_FormClosing;
        }

        void SetFont()
        {
            iTextSharp.text.Font headerFont = FontRegistryWrapper.GetITextSharpFont(headerFontCombo.Text, headerSizeCombo.Text);
            iTextSharp.text.Font tableFont = FontRegistryWrapper.GetITextSharpFont(tableFontCombo.Text, tableSizeCombo.Text);
            exportPDF.SetFont(headerFont, tableFont);
        }

        private void Init(string formName)
        {
            headerInfo = new Dictionary<string, string>();
            string jsonString = File.ReadAllText(@"resources\sample.json");
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement docElement = document.RootElement;
                if (docElement.TryGetProperty("HeaderInfo", out JsonElement headerInfoElement))
                {
                    ObjectEnumerator enumerator = headerInfoElement.EnumerateObject();
                    foreach(JsonProperty jp in enumerator){
                        headerInfo.Add(jp.Name, jp.Value.GetString());
                    }
                }
                headerInfo.Add("발행일자", DateTime.Today.ToShortDateString());
                headerInfo.Add("견적일", DateTime.Today.ToShortDateString());
                if (docElement.TryGetProperty(formName + "Width", out JsonElement widthElement))
                {
                    columnWidths = new float[8];
                    ObjectEnumerator enumerator = widthElement.EnumerateObject();
                    int i = 0;
                    foreach (JsonProperty jp in enumerator)
                    {
                        columnWidths[i] = (float)jp.Value.GetDouble();
                        i++;
                    }
                }

                if (docElement.TryGetProperty(formName+"Items", out JsonElement itemsElement))
                {
                    extractItem = new string[3][];
                    ArrayEnumerator arrEnumerator = itemsElement.EnumerateArray();
                    var row = 0;
                    foreach (JsonElement je in arrEnumerator)
                    {
                        var col = 0;
                        extractItem[row] = new string[8];
                        ObjectEnumerator enumerator = je.EnumerateObject();
                        foreach (JsonProperty jp in enumerator)
                        {
                            extractItem[row][col] = jp.Value.GetString();
                            col++;
                        }
                        row++;
                    }
                }

                if(docElement.TryGetProperty("SumValues", out JsonElement sumElement))
                {
                    sumValues = new string[8];
                    ObjectEnumerator enumerator = sumElement.EnumerateObject();
                    int i = 0;
                    foreach (JsonProperty jp in enumerator)
                    {
                        sumValues[i] = jp.Value.GetString();
                        i++;
                    }
                }
                totalValues = docElement.GetProperty(formName+"Total").GetString();
            }


        }
        private void PdfView_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            docForm.BringToFront();
            docForm.Activate();
            Hide();

        }

        public void Show(bool isPreview)
        {
            if (isPreview)
            {
                if (docForm.SaveDoc(true)==null)
                    return;
            }
            else
                exportPDF.Save("sample", headerInfo, columnWidths, extractItem, sumValues, totalValues);

            if (isPreview)
            {
                leftLayout.Visible = false;
                leftLayout.Dock = DockStyle.None;
                panel2.Dock = DockStyle.Fill;
                target = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DocV2"), "tmp.pdf");
            }
            else
            {
                leftLayout.Visible = true;
                leftLayout.Dock = DockStyle.Left;
                panel2.Dock = DockStyle.Right;
                target = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DocV2"), "sample.pdf");
            }
            browser.Load(target);
            Show();
        }

        private void UpdatePdf()
        {
            exportPDF.Save("sample", headerInfo, columnWidths, extractItem, sumValues,totalValues);
            target = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DocV2"), "sample.pdf");
            browser.Load(target);

        }
    }
}
