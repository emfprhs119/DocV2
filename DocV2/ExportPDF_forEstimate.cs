using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocV2
{
    public class ExportParts_forEstimate : IExportParts
    {
        Image stampImage;
        public ExportParts_forEstimate(ExportPDF exportPDF)
        {
            this.exportPDF = exportPDF;
            FrontShowRow = 24;
            BackShowRow = 34;
            formName = "견적서";
            try
            {
                stampImage = Image.GetInstance(System.Drawing.Image.FromFile(@"stamp.png"), System.Drawing.Imaging.ImageFormat.Png);
            }
            catch { }
        }
        public override Paragraph GetTitle()
        {
            Paragraph paragraph = new Paragraph("견  적  서", titleFont_forEstimate);
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.SpacingAfter = 20f;
            return paragraph;
        }

        public override void ExAdd(Document document, int page)
        {
            if (page == 0 && stampImage != null)
            {
                stampImage.ScaleToFit(30f, 30f);
                stampImage.SetAbsolutePosition(document.PageSize.Width - 72, document.PageSize.Height / 4 * 3 + 65);
                document.Add(stampImage);
            }
        }
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
        private static string[] hangulUnits = { "", "십", "백", "천", "만", "십만", "백만", "천만", "억", "십억", "백억", "천억", "조", "십조", "백조", "천조" };
        private static string[] hangulNumbers = { "영", "일", "이", "삼", "사", "오", "육", "칠", "팔", "구" };

        public static string ConvertToHangul(long number)
        {
            if (number == 0)
            {
                return "영";
            }

            if (number < 0)
            {
                return "마이너스 " + ConvertToHangul(-number);
            }

            string result = "";

            if (number >= 10000)
            {
                long unit = 10000;
                int unitIndex = 4;

                while (number / unit >= 10000)
                {
                    unit *= 10000;
                    unitIndex++;
                }

                result += ConvertToHangul(number / unit) + hangulUnits[unitIndex];

                number %= unit;
            }

            if (number >= 1000)
            {
                result += ConvertToHangul(number / 1000) + "천";
                number %= 1000;
            }

            if (number >= 100)
            {
                result += ConvertToHangul(number / 100) + "백";
                number %= 100;
            }

            if (number >= 10)
            {
                result += ConvertToHangul(number / 10) + "십";
                number %= 10;
            }

            if (number > 0)
            {
                result += hangulNumbers[number];
            }

            return result;
        }
    public override PdfPTable GetHeader(Dictionary<string, string> kv, string sum)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100f;
            //table.SpacingAfter = 8;
            // Header
            /* 임시 데이터 */
            var ltWidth = new float[] { 2, 5 };
            var ltKVArr = new string[] {
                "       ", kv["전화번호"],
                " 견 적 일 :", kv["견적일"],
                " 상     호 :", kv["상 호"],
                " 담 당 자 :", kv["담당자"],
                "아래와 ", "같이 견적합니다."};
            var ltHAligns = new int[] { Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, };

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

            string korSum = ConvertToHangul(int.Parse(sum.Replace(",", "")));
            var btWidth = new float[] { 2,2,7, 4,1};
            var btKVArr = new string[] { "합계금액","(공급가액)", korSum+" 원정","(\\  "+ sum, ")" };
            var btHAligns = new int[] { Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_RIGHT, Element.ALIGN_LEFT, Element.ALIGN_LEFT };
            PdfPCell btCell = new PdfPCell(GetDocHeader(btWidth, btKVArr, btHAligns, null, 50f, TableType.BT_ESTIMATE));
            btCell.Border = Cell.RIGHT_BORDER | Cell.TOP_BORDER | Cell.LEFT_BORDER;
            btCell.Colspan = 2;
            btCell.FixedHeight = 25;
            table.AddCell(btCell);
            table.SetWidths(new float[] {5,6 });
            return table;
        }

        

        public override PdfPCell[] GetCellTableHeader()
        {
            PdfPCell[] titleCell = new PdfPCell[8];
            int i = 0;
            titleCell[i++] = new PdfPCell(new Paragraph("번호", itemHeaderFont));
            titleCell[i++] = new PdfPCell(new Paragraph("품목", itemHeaderFont));
            titleCell[i++] = new PdfPCell(new Paragraph("규격", itemHeaderFont));
            titleCell[i++] = new PdfPCell(new Paragraph("자재비", itemHeaderFont));
            titleCell[i++] = new PdfPCell(new Paragraph("가공비", itemHeaderFont));
            titleCell[i++] = new PdfPCell(new Paragraph("수량", itemHeaderFont));
            titleCell[i++] = new PdfPCell(new Paragraph("단가", itemHeaderFont));
            titleCell[i++] = new PdfPCell(new Paragraph("공급가액", itemHeaderFont));
            //titleCell[i++] = new PdfPCell(new Paragraph("비고", itemHeaderFont));
            // 컬럼 바탕색
            foreach (PdfPCell cell in titleCell)
            {
                cell.FixedHeight = 23f;
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
                    case 3:
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

        internal override void FixTable(string[][] table, float[] widths)
        {
            for (int i = 0; i < table.Length; i++)
            {
                for (int j = table[i].Length - 1; j > 0; j--)
                {
                    table[i][j] = table[i][j - 1];
                }
                table[i][0] = (i + 1).ToString();
            }

            for (int j = widths.Length - 1; j > 0; j--)
            {
                widths[j] = widths[j - 1];
            }
            widths[0] = 25;
        }
    }
}