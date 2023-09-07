using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocV2
{
    public partial class ExportParts_forSpecification : IExportParts
    {
        Image basicImage, frontImage, backImage, stampImage;

        public ExportParts_forSpecification(ExportPDF exportPDF)
        {
            this.exportPDF = exportPDF;
            FrontShowRow = 14;
            BackShowRow = 20;
            basicImage = Image.GetInstance(System.Drawing.Image.FromFile(@"resources\print_demand.png"), System.Drawing.Imaging.ImageFormat.Png);
            frontImage = Image.GetInstance(System.Drawing.Image.FromFile(@"resources\printFront_demand.png"), System.Drawing.Imaging.ImageFormat.Png);
            backImage = Image.GetInstance(System.Drawing.Image.FromFile(@"resources\printBack_demand.png"), System.Drawing.Imaging.ImageFormat.Png);
            try
            {
                stampImage = Image.GetInstance(System.Drawing.Image.FromFile(@"stamp.png"), System.Drawing.Imaging.ImageFormat.Png);
            }
            catch { }

            formName = "거래명세서";
        }

        public override void ExAdd(Document document,int page)
        {
            Image img;
            float space;
            if (maxPage == 1)
            {
                space = 14f;
                img = basicImage;
            }
            else
            {
                if (page == 0)
                {
                    space = 14f;
                    img = frontImage;
                }
                else
                {
                    document.NewPage();
                    space = 46.5f;
                    img = backImage;
                }
            }
            img.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
            img.SetAbsolutePosition(-4, document.PageSize.Height / 2);
            document.Add(img);
            if (page == 0 && stampImage != null)
            {
                stampImage.ScaleToFit(30f, 30f);
                stampImage.SetAbsolutePosition(document.PageSize.Width - 72, document.PageSize.Height / 4 * 3 + 110);
                document.Add(stampImage);
            }
            Paragraph paragraph = new Paragraph();
            paragraph.SpacingAfter = space;
            document.Add(paragraph);
        }

        public override Paragraph GetTitle()
        {
            Paragraph paragraph = new Paragraph(" ", titleFont_forSpecification);
            return paragraph;
        }
        public override String GetMidPath(Dictionary<string, string> headerDatas)
        {
              return Path.Combine("거래명세서",headerDatas["거래처명"]);
        }
        /*
        public override PdfPTable GetTableItem(float[] widths, PdfPCell[] itemTableHeader, PdfPCell[] itemTableFooter, string[][] strArr, int index)
        {
            return GetTableItem(widths, itemTableHeader, itemTableFooter, strArr, index, TableType.TABLE_SPECIFICATION);
        }
        */
        public override TableType GetTableType()
        {
            return TableType.TABLE_SPECIFICATION;
        }
        public override PdfPTable GetHeader(Dictionary<string, string> kv, string sum)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100f;
            // Header
            /* 임시 데이터 */

            var ltWidth = new float[] { 1.1f,3, 5 };
            var ltKVArr = new string[] {
                "","",kv["발행일자"],"",kv["거래처명"],"",
                "", sum};
            var ltHAligns = new int[] { Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_LEFT, Element.ALIGN_LEFT };
            var ltcolspansSup = new int[] { 3, 1,2,1,1,1,2, 1 };

            var rtWidth = new float[] { 9, 39,8,7, 11, 30 };
            var rtKVArr = new string[] {
                "", kv["등록번호"],
                "", kv["상호"],
                "", kv["성명"],
                "", kv["주소"],
                "", kv["업태"],
                "", kv["종목"],
                "", kv["전화"],
                "", kv["팩스"]};
            var rtHAligns = new int[] {Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, Element.ALIGN_CENTER, Element.ALIGN_LEFT, };

            var colspansSup = new int[] {
                1, 5,
                1, 3, 1, 1,
                1, 5,
                1, 1, 1, 3,
                1, 1, 1, 3 };
            /* *********** */
            PdfPCell ltCell = new PdfPCell(GetDocHeader(ltWidth, ltKVArr, ltHAligns, ltcolspansSup, 26f, TableType.LT_SPECIFICATION));
            ltCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ltCell.Border = 0;
            ltCell.FixedHeight = 20.8f * 5;
            PdfPCell rtCell = new PdfPCell(GetDocHeader(rtWidth, rtKVArr, rtHAligns, colspansSup, 20.8f, TableType.RT_SPECIFICATION));
            //PdfPCell rtCell = new PdfPCell(GetDocHeader(rtWidth, rtKVArr, rtHAligns, colspansSup, 24.8f, TableType.RT_SPECIFICATION));
            rtCell.FixedHeight = 20.8f * 5;
            rtCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rtCell.Border = 0;
            table.SetWidths(new float[] { 20f, 15f });
            table.AddCell(ltCell);
            table.AddCell(rtCell);

            return table;
        }
        public override PdfPCell[] GetCellTableHeader()
        {
            PdfPCell[] titleCell = new PdfPCell[0];
            return titleCell;
        }

        public override PdfPCell[] GetCellTableFooter(String[] sumValues)
        {
            PdfPCell[] footerCell = new PdfPCell[3];
            for (var i = 0; i < footerCell.Length; i++)
            {

                var text = "";
                var alignment = Element.ALIGN_RIGHT;
                var colspan = 1;
                switch (i)
                {
                    case 0:
                        colspan = 6;
                        break;
                    case 1:
                        text = sumValues[6];
                        break;
                    case 2:
                        text = sumValues[7];
                        break;
                }
                footerCell[i] = new PdfPCell(new Paragraph(text, itemFont_forSpecification))
                {
                    HorizontalAlignment = alignment,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Colspan = colspan,
                    FixedHeight = 16f,
                    Border = 0
                };

            }

            return footerCell;
        }

        internal override void FixTable(string[][] table, float[] widths)
        {
            //throw new NotImplementedException();
        }
    }
}
