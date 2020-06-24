using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

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

        public Font titleFont_forEstimate, itemFont_forEstimate, titleFont_forSpecification, itemFont_forSpecification, headerFont, itemHeaderFont;
        public Color specificationBorderColor = new Color(System.Drawing.Color.FromArgb(124, 153, 131));
        public Color specificationBackgroundColor = new Color(System.Drawing.Color.FromArgb(235, 239, 236));
        public Color sumColor = new Color(System.Drawing.Color.FromArgb(255, 255, 150));
        public int FrontShowRow;
        public int BackShowRow;
        public BaseFont objFont;
        public string formName;
        
        public IExportParts()
        {
            /*
            string pdfFontTTF = @"Font\malgun.ttf";
            objFont = BaseFont.CreateFont(pdfFontTTF, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            itemFont_forEstimate = new Font(objFont, 11);
            itemFont_forSpecification = new Font(objFont, 11f);
            itemHeaderFont = new Font(objFont, 10, Font.NORMAL);
            titleFont_forSpecification = new Font(objFont, 6);
            headerFont = new Font(objFont, 16, Font.BOLD);
            titleFont_forEstimate = new Font(objFont, 50, Font.BOLD);
            */
        }
        
        internal void SetFont(Font ExportHeaderFont, Font ExportTableFont)
        {
            headerFont = new Font(ExportHeaderFont.BaseFont, ExportHeaderFont.Size / 9 * 14, ExportHeaderFont.Style);
            
            string pdfFontTTF = @"Font\malgun.ttf";
            objFont = BaseFont.CreateFont(pdfFontTTF, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            titleFont_forSpecification = new Font(objFont, 6, Font.NORMAL);
            titleFont_forEstimate = new Font(objFont, 50, Font.NORMAL);
            //titleFont_forEstimate = new Font(ExportHeaderFont.BaseFont, ExportHeaderFont.Size / 9 * 50, ExportHeaderFont.Style);
            itemFont_forEstimate = new Font(ExportTableFont.BaseFont, ExportTableFont.Size / 9 * 11, ExportTableFont.Style);
            itemFont_forSpecification = new Font(ExportTableFont.BaseFont, ExportTableFont.Size / 9 * 11f, ExportTableFont.Style);
            itemHeaderFont = new Font(ExportTableFont.BaseFont, ExportTableFont.Size / 9*10, ExportTableFont.Style);
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

        public PdfPTable GetDocHeader(float[] widths, string[] kvArr, int[] hAligns, int[] colVSpans, float fixedHeightStd, TableType type)
        {
            PdfPTable table = new PdfPTable(widths.Length);
            table.SetWidths(widths);
            for (int i = 0; i < kvArr.Length; i++)
            {
                var fixedHeight = fixedHeightStd;
                var fontSize = headerFont.Size;
                switch (type)
                {
                    case TableType.LT_SPECIFICATION:
                        if (i == 1 || i == 2)
                            fixedHeight = fixedHeightStd - 7.5f;
                        if (i == 3 || i == 4)
                        {
                            fixedHeight = fixedHeightStd + 7.5f;
                            if (kvArr[i].Length * 0.8f < fontSize / 2)
                                fontSize -= kvArr[i].Length * 0.8f;
                            else
                                fontSize /= 2;
                        }
                        break;
                    case TableType.RT_SPECIFICATION:
                        //exportPDF.g.MeasureString(kvArr[i], new System.Drawing.Font(headerFont.Familyname, fontSize)).Width;
                        float widthSize = headerFont.BaseFont.GetWidthPoint(kvArr[i], fontSize);
                        if ((i==7 && widthSize > 325) ||
                            (i == 9 && widthSize > 125) ||
                            (i == 11 && widthSize > 140))
                            fontSize /= 2;
                        break;
                    case TableType.RT_ESTIMATE:
                        if (i == 2 || i == 3 || i == 8 || i == 11)
                            fontSize = fontSize * 1.5f;
                        break;
                }
                float fontHeight = 0;
                float fontWidth = 0;
                while (fontSize > headerFont.Size/2)
                {
                    fontHeight = exportPDF.g.MeasureString(kvArr[i], new System.Drawing.Font(headerFont.Familyname, fontSize)).Height;
                    fontWidth = exportPDF.g.MeasureString(kvArr[i], new System.Drawing.Font(headerFont.Familyname, fontSize)).Width;
                    if (type == TableType.RT_ESTIMATE && (i == 1))
                        fontHeight /= 2;
                    if (fontHeight > fixedHeight)
                        fontSize = fontSize - 0.1f;
                    else
                        break;
                }
                if (type == TableType.RT_ESTIMATE && (i == 1))
                {
                    fontSize = fontSize * 0.8f;
                }
                    Paragraph paragraph = new Paragraph(kvArr[i], new Font(headerFont.BaseFont,fontSize));
                PdfPCell cell = new PdfPCell(paragraph)
                {
                    Border = 0,
                    Padding = 2,
                    PaddingTop = -1,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    FixedHeight = fixedHeightStd
                };
                if (hAligns != null)
                    cell.HorizontalAlignment = hAligns[i];
                if (colVSpans != null)
                    cell.Colspan = colVSpans[i];

                switch (type)
                {
                    case TableType.LT_SPECIFICATION:
                        if (i == 1 || i == 2)
                            cell.FixedHeight = fixedHeightStd - 7.5f;
                        if (i == 3 || i == 4)
                        {
                            cell.FixedHeight = fixedHeightStd + 7.5f;
                        }
                        //cell.Border = 15;
                        break;
                    case TableType.RT_SPECIFICATION:
                        cell.PaddingTop = 1f;
                        if (i == 1)
                        {
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.PaddingLeft = -kvArr[i].Length / 2;
                            //cell.PaddingRight = kvArr[i].Length / 2;
                        }
                        if (i == kvArr.Length - 1)
                        {
                            cell.PaddingLeft = 10;
                            cell.PaddingRight = -10;
                        }
                        if (i == kvArr.Length - 3)
                        {
                            cell.PaddingRight = -10;
                        }
                        break;
                    case TableType.LT_ESTIMATE:
                        if (i >= kvArr.Length - 1)
                        {
                            cell.Border = 15;
                            if (i == kvArr.Length - 2)
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            if (i == kvArr.Length - 1)
                                cell.BackgroundColor = sumColor;
                        }
                        break;
                    case TableType.RT_ESTIMATE:
                        switch (i)
                        {
                            case 0:
                                cell.Border = Cell.LEFT_BORDER | Cell.TOP_BORDER; break;
                            case 1:
                                cell.PaddingTop = 0f; cell.PaddingBottom = 0f; cell.Border = 15; break;
                            case 2:
                                cell.Border = Cell.RIGHT_BORDER | Cell.TOP_BORDER;
                                cell.PaddingLeft = -20;break;
                            case 3:
                            case 8:
                            case 11:
                                cell.Border = Cell.LEFT_BORDER; break;
                            case 16:
                                cell.Border = Cell.LEFT_BORDER | Cell.BOTTOM_BORDER; break;
                            default:
                                cell.Border = 15; break;
                        }
                        break;
                }

                table.AddCell(cell);
            }
            return table;
        }
        //public abstract PdfPTable GetTableItem(float[] widths, PdfPCell[] itemTableHeader, PdfPCell[] itemTableFooter, string[][] strArr, int index);
        public abstract TableType GetTableType();
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
                hAligns = new int[] { Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT };
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
                    float wPadding = 0.2f;
                    float tPadding = fixedHeight/2;
                    if (i < strArr.Length && strArr[i][j] != null)
                    {
                        float fontWidth = 0;
                        float fontHeight = 0;
                        float fontSize = font.Size;
                        
                        while (fontSize >= font.Size / 2)
                        {
                            fontHeight = exportPDF.g.MeasureString(strArr[i][j], new System.Drawing.Font(font.Familyname, font.Size / 9 * fontSize)).Height;
                            fontWidth = exportPDF.g.MeasureString(strArr[i][j], new System.Drawing.Font(font.Familyname, font.Size / 9 * fontSize)).Width;

                            if (fontWidth > widths[j])
                                wPadding = (fontWidth - widths[j]);
                            tPadding = (fixedHeight - (fontHeight + 5)) / 2;
                            if (fontHeight > fixedHeight + 5.4)
                            {
                                fontSize = fontSize - 0.1f;
                            }
                            else {
                                break;
                            }
                        }
                        
                        Paragraph pa = new Paragraph(strArr[i][j], new Font(font.BaseFont,font.Size / 9 * fontSize));
                        cell = new PdfPCell(pa);
                    }
                    else
                        cell = new PdfPCell(new Paragraph(null, font));
                    
                    cell.HorizontalAlignment = hAligns[j];
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.FixedHeight = fixedHeight;
                    cell.BorderColor = borderColor;
                    cell.BorderWidth = 1;
                    cell.Padding = 0.3f;
                    cell.PaddingTop = -3f;
                    switch (cell.HorizontalAlignment)
                    {
                        case Element.ALIGN_CENTER:
                            cell.PaddingRight = -wPadding / 2;
                            cell.PaddingLeft = -wPadding / 2;break;
                        case Element.ALIGN_LEFT:
                            cell.PaddingRight = -wPadding / 2;break;
                        case Element.ALIGN_RIGHT:
                            cell.PaddingLeft = -wPadding / 2; break;
                    }
                    //cell.PaddingTop = tPadding;
                    cell.NoWrap = false;
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
        IExportParts exportParts;
        PageEventHelper pageEventHelper;
        public System.Drawing.Graphics g;
        public ExportPDF(System.Drawing.Graphics graphics,string formName)
        {
            if (formName == "견적서")
                exportParts = new ExportParts_forEstimate(this);
            else if (formName == "거래명세서")
                exportParts = new ExportParts_forSpecification(this);
            pageEventHelper = new PageEventHelper(formName);
            this.g = graphics;
        }

        public string Save(String docID, Dictionary<string, string> headerDatas, float[] widths, String[][] table, String[] sumValues, String sum)
        {
            
            exportParts.CalcPage(table);
            Document document = new Document(PageSize.A4);
            document.SetMargins(48, 48, 45, 45);
            string pdfPath;
            if (docID == "sample" || docID == "tmp")
            {
                pdfPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"DocV2"));
            }else
                pdfPath = Path.Combine(Path.Combine(Properties.Settings.Default.path, "PDF"), exportParts.GetMidPath(headerDatas));

            Directory.CreateDirectory(pdfPath);
            var fullPath = Path.Combine(pdfPath, docID + ".pdf");
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            PdfWriter wr = PdfWriter.GetInstance(document, new FileStream(fullPath, FileMode.CreateNew));
            wr.PageEvent = pageEventHelper;
            // Document 열기
            document.Open();
            // MainTitle
            document.Add(exportParts.GetTitle());
            // 기본정보
            var k = exportParts.GetHeader(headerDatas, sum);
            document.Add(k);
            // 테이블
            for (int i = 0;i< exportParts.maxPage; i++)
            {
                exportParts.ExAdd(document,i);
                var tables = exportParts.GetTableItem(widths, exportParts.GetCellTableHeader(), exportParts.GetCellTableFooter(sumValues), table, i, exportParts.GetTableType());
                document.Add(tables);
            }
            document.Close();
            return fullPath;
        }

        internal void SetFont(Font headerFont, Font tableFont)
        {
            exportParts.SetFont(headerFont, tableFont);
            pageEventHelper.SetFont(headerFont);
        }
    }


    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;
        Font pageFont;
        String fName;
        float x = 0;
        float y = 0;
        public PageEventHelper(String fName)
        {
            /*
            string pdfFontTTF = @"Font\malgun.ttf";
            BaseFont objFont = BaseFont.CreateFont(pdfFontTTF, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            pageFont = new Font(objFont, 10, Font.BOLD);
            */
            this.fName = fName;
        }
        internal void SetFont(Font font)
        {
            pageFont = new Font(font.BaseFont, font.Size / 9 * 12.5f, font.Style);
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
            String text = /*"Page " + */pageN.ToString()+" / ";
            float len = pageFont.BaseFont.GetWidthPoint(text, pageFont.Size);
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

            //cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(pageFont.BaseFont, pageFont.Size);
            cb.SetTextMatrix(x,y);
            cb.ShowText(text);
            cb.EndText();

            cb.AddTemplate(template, x, y);
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.SetFontAndSize(pageFont.BaseFont, pageFont.Size);
            string pageNum = ""+(writer.PageNumber - 1);
            
            template.SetTextMatrix(pageFont.BaseFont.GetWidthPoint(pageNum+" / ", pageFont.Size), 0);
            template.ShowText(pageNum);
            template.EndText();
        }

    }
}
