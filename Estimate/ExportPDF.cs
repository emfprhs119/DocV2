using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimate
{
    interface IDocPdf
    {

    }
    class ExportPDF
    {
        enum TableType {LT_ESTIMATE,RT_ESTIMATE }
        Font titleFont, bold20Font, bold12Font, bold10Font, normal10Font;
        String pdfPath = Path.Combine(Environment.CurrentDirectory, "pdf");
        Color sumColor = new Color(System.Drawing.Color.LightYellow);
        int FrontShowRow = 22;
        int BackShowRow = 34;
        public ExportPDF()
        {
            InitFont();
        }
        void InitFont()
        {

            string pdfFontTTF = @"Font\malgun.ttf";
            BaseFont objFont = BaseFont.CreateFont(pdfFontTTF, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            titleFont = new Font(objFont, 50, Font.BOLD);
            bold20Font = new Font(objFont, 20, Font.BOLD);
            bold12Font = new Font(objFont, 12, Font.BOLD);
            bold10Font = new Font(objFont, 10, Font.BOLD);
            normal10Font = new Font(objFont, 10);
        }

        public void Save(String fName, float[] widths, String[][] table,String sum)
        {

            Document document = new Document(PageSize.A4);
            document.SetMargins(45, 45, 45, 45);
            //String path = "pdf\\" + est.getDemand().getName();

            /*
            File a = new File(path);
            if (a.exists() == false)
            {
                a.mkdirs();
            }
            */
            Directory.CreateDirectory(pdfPath);
            PdfWriter wr = PdfWriter.GetInstance(document, new FileStream(Path.Combine(pdfPath, fName+".pdf"), FileMode.Create));
            PageEventHelper pageEventHelper = new PageEventHelper();
            wr.PageEvent = pageEventHelper;
            // Document 열기
            //doc.Open();
            //PdfWriter.getInstance(document, new FileOutputStream(path + "\\" + fName + ".pdf"));
            document.Open();

            // 타이틀
            Paragraph paragraph = new Paragraph("견적서", titleFont);
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingAfter = 20f;
            document.Add(paragraph);
            // 기본정보
            document.Add(GetHeader(/*sum 포함 정보*/));
            // 아이템
            int page = 1;
            int index = 0;
            do
            {
                if (page > 1)
                {
                    paragraph = new Paragraph();
                    paragraph.SpacingAfter = 5f;
                    document.Add(paragraph);
                }

                var tables = CreateItemTable(widths, GetItemTableHeader(), GetItemTableFooter(sum), table, index);
                /*
                var cell = new PdfPCell(new Paragraph((page++).ToString() + "/" + (endRow < FrontShowRow ? 1 : (endRow - FrontShowRow) / BackShowRow + 2).ToString(), bold10Font));
                cell.Colspan = widths.Length;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_BOTTOM;
                cell.Border = 0;
                cell.FixedHeight = 30f;
                tables.AddCell(cell);
                */
                document.Add(tables);
                index += index == 0 ? FrontShowRow : BackShowRow;
            } while (index < table.Length);

            document.Close();
            //JOptionPane.showMessageDialog(null, "pdf 파일이 저장되었습니다.");
        }

        private PdfPTable GetHeader()
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100f;
            table.SpacingAfter = 8;
            // Header
            /* 임시 데이터 */
            var ltWidth = new float[] { 2, 5 };
            var ltKVArr = new string[] { " 견 적 일 :", "2020-02-23", " 상     호 :", "하민수", " 전화번호:", "010-2661-2267", " 담 당 자 :", "하현수", "합계금액", "20,000원 " };
            var ltFontKV = new Font[] { bold12Font, bold10Font, bold12Font, bold10Font, bold12Font, bold10Font, bold12Font, bold10Font, bold20Font, bold20Font };
            var ltHAligns = new int[] { Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, };

            var rtWidth = new float[] { 8, 12, 30, 12, 30 };
            var rtKVArr = new string[] { "공급자", "등록번호", "하민수", "상호", "010-2661-2267", "성명", "하현수", "성명", "하현수", "성명", "하현수", "성명", "하현수", "성명", "하현수", "성명", "하현수" };
            var rtFontKV = new Font[] { bold20Font, bold10Font, bold20Font, bold12Font, bold10Font, bold12Font, bold10Font, bold12Font, bold10Font, bold12Font, bold10Font, bold12Font, bold10Font, bold12Font, bold10Font, bold12Font, bold10Font, };
            var tmFont = new ArrayList();
            for (var i = 0; i < rtKVArr.Length; i++)
            {
                tmFont.Add(bold12Font);
            }
            var colspansSup = new int[] { 5, 1, 3, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1 };
            /* *********** */
            PdfPCell ltCell = new PdfPCell(GetTableKV(ltWidth, ltKVArr, ltFontKV, ltHAligns, null, TableType.LT_ESTIMATE));
            ltCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ltCell.Border = 0;
            PdfPCell rtCell = new PdfPCell(GetTableKV(rtWidth, rtKVArr, rtFontKV, null, colspansSup,TableType.RT_ESTIMATE));
            rtCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rtCell.Border = 0;
            ltCell.PaddingRight = 10;
            table.AddCell(ltCell);
            table.AddCell(rtCell);

            return table;
        }

        private PdfPTable GetTableKV(float[] widths, string[] kvArr, Font[] fonts, int[] hAligns, int[] colVSpans, TableType type)
        {
            PdfPTable table = new PdfPTable(widths.Length);
            table.SetWidths(widths);
            for (int i = 0; i < kvArr.Length; i++)
            {
                Font font;
                if (fonts == null)
                    font = bold10Font;
                else
                    font = fonts[i];
                PdfPCell cell = new PdfPCell(new Paragraph(kvArr[i], font))
                {
                    Border = 0,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                cell.FixedHeight = 24f;
                if (hAligns != null)
                    cell.HorizontalAlignment = hAligns[i];
                
                // for estimate right_top
                if (type == TableType.RT_ESTIMATE)
                {
                    if (i == 0)
                        cell.Rowspan = 5;
                    else if (colVSpans != null)
                        cell.Colspan = colVSpans[i];
                    cell.Border = 15;
                }
                
                // for estimate left_top
                if (type == TableType.LT_ESTIMATE)
                {
                    if (i>= kvArr.Length - 2)
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

        public PdfPCell[] GetItemTableHeader()
        {
            PdfPCell[] titleCell = new PdfPCell[8];
            titleCell[0] = new PdfPCell(new Paragraph("품목", bold12Font));
            titleCell[1] = new PdfPCell(new Paragraph("규격", bold12Font));
            titleCell[2] = new PdfPCell(new Paragraph("자재비", bold12Font));
            titleCell[3] = new PdfPCell(new Paragraph("가공비", bold12Font));
            titleCell[4] = new PdfPCell(new Paragraph("수량", bold12Font));
            titleCell[5] = new PdfPCell(new Paragraph("단가", bold12Font));
            titleCell[6] = new PdfPCell(new Paragraph("공급가액", bold12Font));
            titleCell[7] = new PdfPCell(new Paragraph("비고", bold12Font));
            // 컬럼 바탕색
            foreach (PdfPCell cell in titleCell)
            {
                cell.FixedHeight = 24f;
                cell.BackgroundColor=Color.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            }
            return titleCell;
        }

        public PdfPCell[] GetItemTableFooter(string sum)
        {
            PdfPCell[] footerCell = new PdfPCell[4];
            for (var i = 0; i < footerCell.Length; i++) {
                
                var text = "";
                var alignment = Element.ALIGN_RIGHT;
                var colspan = 1;
                var backColor = Color.WHITE;
                switch (i)
                {
                    case 0:
                        alignment = Element.ALIGN_CENTER;
                        colspan = 2;
                        text = "합계";
                        break;
                    case 1:
                        colspan = 4;
                        break;
                    case 2:
                        text = sum;
                        backColor = sumColor;
                        break;
                }
                footerCell[i] = new PdfPCell(new Paragraph(text, bold12Font));
                footerCell[i].HorizontalAlignment = alignment;
                footerCell[i].VerticalAlignment = Element.ALIGN_MIDDLE;
                footerCell[i].Colspan = colspan;
                footerCell[i].BackgroundColor = backColor;
                footerCell[i].FixedHeight = 24f;
            }
            return footerCell;
        }

        public PdfPTable CreateItemTable(float[] widths, PdfPCell[] itemTableHeader, PdfPCell[] itemTableFooter, string[][] strArr, int index)
        {
            int count;
            PdfPTable table = new PdfPTable(8);
            table.WidthPercentage = 100f;
            table.SetWidths(widths);
            PdfPCell cell = null;
            if (index == 0)
            {
                count = FrontShowRow;
                foreach (PdfPCell hCell in itemTableHeader)
                    table.AddCell(hCell);
            }
            else
                count = index + BackShowRow;
            for (int i = index; i < count; i++)
            {
                for(int j = 0; j < widths.Length; j++)
                {
                    if (i< strArr.Length)
                        cell = new PdfPCell(new Paragraph(strArr[i][j], normal10Font));
                    else
                        cell = new PdfPCell(new Paragraph(null, normal10Font));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.FixedHeight = 20f;
                    if (j == 6)
                        cell.BackgroundColor = sumColor;
                    table.AddCell(cell);
                }
            }

            cell = new PdfPCell
            {
                Colspan = widths.Length,
                FixedHeight = 1,
                Border = 0
            };
            table.AddCell(cell);
            if (index == 0 ? FrontShowRow >= strArr.Length : index + BackShowRow >= strArr.Length)
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
    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;
        Font RunDateFont;
        public PageEventHelper()
        {
            string pdfFontTTF = @"na.ttf";
            BaseFont objFont = BaseFont.CreateFont(pdfFontTTF, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            RunDateFont = new Font(objFont, 10, Font.BOLD);

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
            String text = /*"Page " + */pageN.ToString() + " of ";
            float len = RunDateFont.BaseFont.GetWidthPoint(text, RunDateFont.Size);
            float loX = (document.Left + document.Right) / 2;
            iTextSharp.text.Rectangle pageSize = document.PageSize;

            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(RunDateFont.BaseFont, RunDateFont.Size);
            cb.SetTextMatrix(loX - len/2, pageSize.GetBottom(document.BottomMargin+2));
            cb.ShowText(text);

            cb.EndText();

            cb.AddTemplate(template, loX + len/2, pageSize.GetBottom(document.BottomMargin+2));
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.SetFontAndSize(RunDateFont.BaseFont, RunDateFont.Size);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }
    }
}
