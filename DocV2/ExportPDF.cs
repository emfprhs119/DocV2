using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocV2
{
    public abstract class IExportParts
    {
        protected ExportPDF exportPDF;
        public int maxPage;
        public enum TableType { LT_ESTIMATE, RT_ESTIMATE, LT_SPECIFICATION, RT_SPECIFICATION, TABLE_ESTIMATE, TABLE_SPECIFICATION }
        public abstract PdfPTable GetHeader(Dictionary<string, string> kv, string sum);
        public abstract PdfPCell[] GetCellTableHeader();
        public abstract PdfPCell[] GetCellTableFooter(String[] sumValues);
        public abstract Paragraph GetTitle();
        public abstract String GetMidPath(Dictionary<string, string> headerDatas);

        public Font titleFont_forEstimate, itemFont_forEstimate, titleFont_forSpecification, itemFont_forSpecification, BOLD16Font, NORMAL10Font;
        string pdfFontTTF = @"Font\malgun.ttf";
        public Color specificationBorderColor = new Color(System.Drawing.Color.FromArgb(124, 153, 131));
        public Color specificationBackgroundColor = new Color(System.Drawing.Color.FromArgb(215, 224, 217));
        public Color sumColor = new Color(System.Drawing.Color.LightYellow);
        public int FrontShowRow;
        public int BackShowRow;

        public IExportParts()
        {
            BaseFont objFont = BaseFont.CreateFont(pdfFontTTF, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            itemFont_forEstimate = new Font(objFont, 10);
            itemFont_forSpecification = new Font(objFont, 8);
            NORMAL10Font = new Font(objFont, 10, Font.NORMAL);
            titleFont_forSpecification = new Font(objFont, 6);

            BOLD16Font = new Font(objFont, 16, Font.BOLD);
            titleFont_forEstimate = new Font(objFont, 50, Font.BOLD);
        }
        public abstract void ExAdd(Document document, int page);

        public int CalcPage(string[][] table)
        {
            int index = 0;
            int page = 0;
            do
            {
                index += index == 0 ? FrontShowRow : BackShowRow;
                page++;
            } while (index < table.Length);
            maxPage = page;
            return page;
        }

        public PdfPTable GetTableHeader(float[] widths, string[] kvArr, int[] hAligns, int[] colVSpans, float fixedHeightStd, TableType type)
        {
            PdfPTable table = new PdfPTable(widths.Length);
            table.SetWidths(widths);
            for (int i = 0; i < kvArr.Length; i++)
            {
                Font font = NORMAL10Font;

                switch (type)
                {
                    case TableType.LT_SPECIFICATION:
                        if (i == 6)
                            font = BOLD16Font;
                        break;
                    case TableType.RT_SPECIFICATION:
                        if (i == 1)
                            font = BOLD16Font;
                        break;
                    case TableType.LT_ESTIMATE:
                        if (i == 8 || i == 9)
                            font = BOLD16Font;
                        break;
                    case TableType.RT_ESTIMATE:
                        if (i == 0 || i == 2)
                            font = BOLD16Font;
                        break;
                }
                PdfPCell cell = new PdfPCell(new Paragraph(kvArr[i], font))
                {
                    Border = 0,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                cell.FixedHeight = fixedHeightStd;
                if (type == TableType.LT_SPECIFICATION)
                {
                    if (i > 5)
                    {
                        cell.PaddingTop = 3;
                        cell.PaddingBottom = 3;
                    }
                    else
                        cell.PaddingBottom = 7;
                }
                if (hAligns != null)
                    cell.HorizontalAlignment = hAligns[i];
                if (colVSpans != null)
                    cell.Colspan = colVSpans[i];
                // for estimate right_top
                if (type == TableType.RT_ESTIMATE)
                {
                    if (i == 0)
                    {
                        cell.PaddingTop = 35;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Rowspan = 5;
                    }
                    cell.Border = 15;
                }

                // for estimate left_top
                if (type == TableType.LT_ESTIMATE)
                {
                    if (i >= kvArr.Length - 2)
                    {
                        cell.Border = 15;
                        if (i == kvArr.Length - 2)
                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        if (i == kvArr.Length - 1)
                            cell.BackgroundColor = sumColor;
                    }
                }

                table.AddCell(cell);
            }
            return table;
        }
        public abstract PdfPTable GetTableItem(float[] widths, PdfPCell[] itemTableHeader, PdfPCell[] itemTableFooter, string[][] strArr, int index);

        public PdfPTable GetTableItem(float[] widths, PdfPCell[] itemTableHeader, PdfPCell[] itemTableFooter, string[][] strArr, int index, TableType tableType)
        {
            Font font = null;
            float fixedHeight = 0;
            Color borderColor = null;
            int[] hAligns = null;
            if (tableType == TableType.TABLE_ESTIMATE)
            {
                fixedHeight = 20f;
                font = itemFont_forEstimate;
                borderColor = Color.BLACK;
                hAligns = new int[] { Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, Element.ALIGN_CENTER, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT };

            }
            if (tableType == TableType.TABLE_SPECIFICATION)
            {
                fixedHeight = 13.61f;
                font = itemFont_forSpecification;
                borderColor = specificationBorderColor;
                widths = new float[] { 34.7f, 92.8f, 172, 74, 46.5f, 71, 97, 79 };
                hAligns = new int[] { Element.ALIGN_CENTER, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT };
            }
            PdfPTable table = new PdfPTable(8);
            table.WidthPercentage = 100f;
            table.SetWidths(widths);
            PdfPCell cell = null;

            int rowStart, rowEnd;

            if (index == 0)
            {
                rowStart = 0;
                rowEnd = FrontShowRow;
                if (itemTableHeader != null)
                    foreach (PdfPCell hCell in itemTableHeader)
                        table.AddCell(hCell);
            }
            else
            {
                rowStart = FrontShowRow + (index - 1) * BackShowRow;
                rowEnd = rowStart + BackShowRow;
            }
            for (int i = rowStart; i < rowEnd; i++)
            {
                for (int j = 0; j < widths.Length; j++)
                {
                    if (i < strArr.Length)
                        cell = new PdfPCell(new Paragraph(strArr[i][j], font));
                    else
                        cell = new PdfPCell(new Paragraph(null, font));
                    cell.HorizontalAlignment = hAligns[j];
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.FixedHeight = fixedHeight;
                    cell.BorderColor = borderColor;
                    if (j == 6 && tableType == TableType.TABLE_ESTIMATE)
                        cell.BackgroundColor = sumColor;
                    if (i % 2 != 1 && tableType == TableType.TABLE_SPECIFICATION)
                    {
                        cell.BackgroundColor = specificationBackgroundColor;

                    }
                    table.AddCell(cell);
                }
            }
            if (tableType == TableType.TABLE_ESTIMATE)
            {
                cell = new PdfPCell
                {
                    Colspan = widths.Length,
                    FixedHeight = 1,
                    Border = 0
                };
                table.AddCell(cell);
            }
            if (index+1 == maxPage)
            {
                foreach (PdfPCell fCell in itemTableFooter)
                {
                    table.AddCell(fCell);
                }
            }
            else
            {
                cell = new PdfPCell
                {
                    Colspan = widths.Length,
                    FixedHeight = 24,
                    Border = 0
                };
                table.AddCell(cell);
            }
            table.SpacingAfter = 0;
            return table;
        }
    }
    public class ExportPDF
    {
        IExportParts partsEstimate, partsSpecification;
        //String currPath = Path.Combine(@"C:\Users\emfpr\Desktop", "pdf");
        //String pdfPath = Path.Combine(Environment.CurrentDirectory, "pdf");
        public ExportPDF()
        {
            partsEstimate = new ExportParts_forEstimate(this);
            partsSpecification = new ExportParts_forSpecification(this);
        }

        public void Save(String formName,String docID, Dictionary<string, string> headerDatas, float[] widths, String[][] table, String[] sumValues, String sum)
        {
            IExportParts exportParts = null;
            if (formName == "견적서")
            {
                exportParts = partsEstimate;
            }
            if (formName == "거래명세서")
            {
                exportParts = partsSpecification;
            }
            exportParts.CalcPage(table);

            Document document = new Document(PageSize.A4);
            document.SetMargins(48, 48, 45, 45);
            string pdfPath = Path.Combine(Path.Combine(Properties.Settings.Default.path, "PDF"), exportParts.GetMidPath(headerDatas));
           
            Directory.CreateDirectory(pdfPath);
            PdfWriter wr = PdfWriter.GetInstance(document, new FileStream(Path.Combine(pdfPath, docID + ".pdf"), FileMode.Create));
            PageEventHelper pageEventHelper = new PageEventHelper(formName);
            wr.PageEvent = pageEventHelper;
            // Document 열기
            document.Open();
            // MainTitle
            document.Add(exportParts.GetTitle());
            // 기본정보
            document.Add(exportParts.GetHeader(headerDatas, sum));
            // 테이블
            for (int i = 0;i< exportParts.maxPage; i++)
            {
                exportParts.ExAdd(document,i);
                var tables = exportParts.GetTableItem(widths, exportParts.GetCellTableHeader(), exportParts.GetCellTableFooter(sumValues), table, i);
                document.Add(tables);
            }
            document.Close();

        }
    }


    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;
        Font RunDateFont;
        String fName;
        float x = 0;
        float y = 0;
        public PageEventHelper(String fName)
        {
            string pdfFontTTF = @"Font\malgun.ttf";
            BaseFont objFont = BaseFont.CreateFont(pdfFontTTF, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            RunDateFont = new Font(objFont, 10, Font.BOLD);
            this.fName = fName;
        }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            base.OnEndPage(writer, document);

            int pageN = writer.PageNumber;
            String text = /*"Page " + */pageN.ToString() + " / ";
            float len = RunDateFont.BaseFont.GetWidthPoint(text, RunDateFont.Size);
            float loX = (document.Left + document.Right) / 2;

            iTextSharp.text.Rectangle pageSize = document.PageSize;
            if (fName == "견적서") {
                x = loX - len / 2;
                y = pageSize.GetBottom(document.BottomMargin + 2);
            }
            else if (fName == "거래명세서")
            {
                x = loX - (len / 2) - (document.Right/4)-28f;
                y = document.Top-document.TopMargin+17f;
            }

            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(RunDateFont.BaseFont, RunDateFont.Size);
            cb.SetTextMatrix(x,y);
            cb.ShowText(text);
            cb.EndText();

            cb.AddTemplate(template, x, y);
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.SetFontAndSize(RunDateFont.BaseFont, RunDateFont.Size);
            template.SetTextMatrix(17, 0);
            template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }
    }
}
