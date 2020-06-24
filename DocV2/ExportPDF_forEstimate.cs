using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocV2
{
    public class ExportParts_forEstimate : IExportParts
    {
        public ExportParts_forEstimate(ExportPDF exportPDF)
        {
            this.exportPDF = exportPDF;
            FrontShowRow = 22;
            BackShowRow = 34;
            formName = "견적서";
        }
        public override Paragraph GetTitle()
        {
            Paragraph paragraph = new Paragraph("견적서", titleFont_forEstimate);
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingAfter = 20f;
            return paragraph;
        }
        public override void ExAdd(Document document, int page) { }
        public override String GetMidPath(Dictionary<string, string> headerDatas)
        {
            return Path.Combine("견적서", headerDatas["상 호"]);
        }
        /*
        public override PdfPTable GetTableItem(float[] widths, PdfPCell[] itemTableHeader, PdfPCell[] itemTableFooter, string[][] strArr, int index)
        {
            return GetTableItem(widths, itemTableHeader, itemTableFooter, strArr, index, TableType.TABLE_ESTIMATE);
        }
        */
        public override TableType GetTableType()
        {
            return TableType.TABLE_ESTIMATE;
        }
        public override PdfPTable GetHeader(Dictionary<string, string> kv, string sum)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100f;
            table.SpacingAfter = 8;
            // Header
            /* 임시 데이터 */
            var ltWidth = new float[] { 2, 5 };
            var ltKVArr = new string[] {
                " 견 적 일 :", kv["견적일"],
                " 상     호 :", kv["상 호"],
                "전화번호 :", kv["전화번호"],
                " 담 당 자 :", kv["담당자"],
                "합계금액 :", sum+"원"};
            var ltHAligns = new int[] { Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT, };

            var rtWidth = new float[] { 8, 12, 30, 12, 30 };
            var rtKVArr = new string[] { "",
                "등록"+new Chunk("\n")+"번호",kv["등록번호"],
                "공","상호", kv["상호"],
                "성명", kv["성명"],
                "급","주소", kv["주소"],
                "자","업태", kv["업태"],
                "종목", kv["종목"],
                "","전화", kv["전화"],
                "팩스", kv["팩스"]};
            var rtHAligns = new int[] { Element.ALIGN_CENTER, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, };

            var colspansSup = new int[] { 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            /* *********** */
            PdfPCell ltCell = new PdfPCell(GetDocHeader(ltWidth, ltKVArr, ltHAligns, null, 24f, TableType.LT_ESTIMATE));
            ltCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ltCell.Border = 0;
            PdfPCell rtCell = new PdfPCell(GetDocHeader(rtWidth, rtKVArr, rtHAligns, colspansSup,24f, TableType.RT_ESTIMATE));
            rtCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rtCell.Border = 0;
            ltCell.PaddingRight = 10;
            table.AddCell(ltCell);
            table.AddCell(rtCell);
            table.SetWidths(new float[] {5,6 });
            return table;
        }

        

        public override PdfPCell[] GetCellTableHeader()
        {
            PdfPCell[] titleCell = new PdfPCell[8];
            titleCell[0] = new PdfPCell(new Paragraph("품목", itemHeaderFont));
            titleCell[1] = new PdfPCell(new Paragraph("규격", itemHeaderFont));
            titleCell[2] = new PdfPCell(new Paragraph("자재비", itemHeaderFont));
            titleCell[3] = new PdfPCell(new Paragraph("가공비", itemHeaderFont));
            titleCell[4] = new PdfPCell(new Paragraph("수량", itemHeaderFont));
            titleCell[5] = new PdfPCell(new Paragraph("단가", itemHeaderFont));
            titleCell[6] = new PdfPCell(new Paragraph("공급가액", itemHeaderFont));
            titleCell[7] = new PdfPCell(new Paragraph("비고", itemHeaderFont));
            // 컬럼 바탕색
            foreach (PdfPCell cell in titleCell)
            {
                cell.FixedHeight = 24f;
                cell.BackgroundColor = Color.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            }
            return titleCell;
        }

        public override PdfPCell[] GetCellTableFooter(string[] sumValues)
        {
            PdfPCell[] footerCell = new PdfPCell[4];
            for (var i = 0; i < footerCell.Length; i++)
            {

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
                        text = sumValues[6];
                        backColor = sumColor;
                        break;
                }
                footerCell[i] = new PdfPCell(new Paragraph(text, itemHeaderFont));
                footerCell[i].HorizontalAlignment = alignment;
                footerCell[i].VerticalAlignment = Element.ALIGN_MIDDLE;
                footerCell[i].Colspan = colspan;
                footerCell[i].BackgroundColor = backColor;
                footerCell[i].FixedHeight = 24f;
            }
            return footerCell;
        }

        

    }
}