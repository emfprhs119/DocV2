using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.DataFormat;
using unvell.ReoGrid.Events;
using unvell.ReoGrid.IO;

namespace Estimate
{
    
    public class SheetControl
    //public partial class DocForm : Form
    {
        ReoGridControl reoGrid;
        Font font;
        Worksheet sheet;
        int initMaxRow = 100;
        private float[] columnWidths;
        private bool isResetColumnWidth;
        Graphics g;
        readonly Color sumColor = System.Drawing.Color.LightYellow;
        Label totalLabel;
        HashSet<CellConfig> restoreSet;
        public bool isModified;
        DocForm docForm;
        public float[] GetColumnWidths()
        {
            return columnWidths;
        }
        public void SetColumnWidths(float[] widths)
        {
            columnWidths = widths;
            ResetSheetWidth(null);
        }
        public string TotalString()
        {

            uint sumValue = 0;
            for (int i = 0; i < sheet.Columns; i++)
            {
                if (sheet.GetCellData(sheet.Rows - 1, i) != null)
                {
                    uint tmp;
                    if (uint.TryParse(sheet.GetCellData((sheet.Rows - 1), i).ToString(), out tmp))
                    {
                        sumValue += tmp;
                    }
                    else
                    {
                        return "NaN";
                    }
                }
            }
            return string.Format("{0:c}", sumValue).Substring(1);
        }

        private void RefreshTotalLabel()
        {
            totalLabel.Text = TotalString() + "원";
        }

        public SheetControl(DocForm docForm, unvell.ReoGrid.ReoGridControl reoGrid, Label label23, Graphics g)
        {
            this.docForm = docForm;
            this.reoGrid = reoGrid;
            totalLabel = label23;
            this.g = g;
            isModified = true;


        }
        private void FormulaUpdate(RangePosition rangePosition)
        {
            
            sheet.SetRangeBorders(rangePosition, BorderPositions.All, new RangeBorderStyle { Color = Color.Black, Style = BorderLineStyle.Solid });
            
            foreach (CellConfig cellConfig in restoreSet.AsEnumerable())
            {

                if (cellConfig.special != null && cellConfig.special.Equals("LastSum"))
                {
                    string currChar = ((char)('A' + cellConfig.column)).ToString();
                    RangePosition endPosition = RangePosition.FromCellPosition(sheet.Rows, cellConfig.column, sheet.Rows, cellConfig.column);
                    sheet.SetRangeData(endPosition, "=SUM(" + currChar + "1:" + currChar + (sheet.Rows - 1) + ")");
                    continue;
                }
                for (int i= rangePosition.Row;i<= rangePosition.EndRow;i++)
                {
                    sheet.Cells[i, cellConfig.column].Formula = cellConfig.formula;
                    sheet.Cells[i, cellConfig.column].DataFormatArgs = cellConfig.dataFormatArgs;
                    sheet.Cells[i, cellConfig.column].IsReadOnly = cellConfig.isReadOnly;
                    sheet.Cells[i, cellConfig.column].DataFormat = cellConfig.dataFormat;
                }
            }
            for (int i = 0; i < sheet.Columns; i++)
            {
                if (sheet.Cells[0, i].IsReadOnly)
                {
                    RangePosition startPosition = RangePosition.FromCellPosition(0, i, 0, i);
                    RangePosition fillPosition = RangePosition.FromCellPosition(1, i, sheet.Rows - 2, i);
                    sheet.IterateCells(fillPosition, false, (row, col, cell) =>
                    {
                        if (cell != null)
                            cell.IsReadOnly = false;
                        return true;
                    });
                    sheet.AutoFillSerial(startPosition, fillPosition);
                    sheet.IterateCells(fillPosition, false, (row, col, cell) =>
                    {
                        cell.IsReadOnly = true;
                        return true;
                    });
                }
            }
        }
        public void AttachMenu(ToolStripMenuItem[] menuItems)
        {
            foreach(ToolStripMenuItem menuItem in menuItems)
            {
                switch (menuItem.Name)
                {
                    case "menu_RowAdd":
                        menuItem.Click += (object sender, EventArgs e) =>
                        {
                            sheet.InsertRows(sheet.SelectionRange.Row, sheet.SelectionRange.Rows);
                            FormulaUpdate(sheet.SelectionRange);
                        };
                        break;
                    case "menu_RowDelete":
                        menuItem.Click += (object sender, EventArgs e) =>
                        {
                            sheet.DeleteRows(sheet.SelectionRange.Row, sheet.SelectionRange.Rows);
                            FormulaUpdate(sheet.SelectionRange);
                        };
                        break;
                    case "menu_DataRemove":
                        menuItem.Click += (object sender, EventArgs e) =>
                        sheet.DeleteRangeData(sheet.SelectionRange,true);
                        break;
                    case "menu_Cut":
                        menuItem.Click += (object sender, EventArgs e) =>
                        {
                            sheet.Copy();
                            sheet.DeleteRangeData(sheet.SelectionRange, true);
                        };
                        break;
                    case "menu_Copy":
                        menuItem.Click += (object sender, EventArgs e) =>
                        sheet.Copy();
                        break;
                    case "menu_Paste":
                        menuItem.Click += (object sender, EventArgs e) =>
                        {
                            string text = Clipboard.GetText();
                            object[,] data = RGUtility.ParseTabbedString(text);
                            sheet.SetRangeData(sheet.SelectionRange, data, true);
                        };
                        break;
                }
            }
        }

        public void InitSheetStyle(JsonElement columnElements)
        {
            InitReoGrid();
            font = new System.Drawing.Font("맑은 고딕", 11);
            restoreSet = new HashSet<CellConfig>();
            var columnSize = columnElements.GetArrayLength();
            sheet.Columns = columnSize;
            columnWidths = new float[sheet.Columns];
            sheet.Rows = initMaxRow + 1;
            sheet.HideRows(initMaxRow, 1);
            
            sheet.RowHeaderWidth = 35;

            Dictionary<string, int> headerFindIndex = new Dictionary<string, int>();
            {
                int i = 0;
                foreach (JsonElement columnElement in columnElements.EnumerateArray())
                {
                    RangePosition rangePosition = RangePosition.FromCellPosition(0, i, sheet.Rows, i);
                    // for Predefined;
                    sheet.SetRangeStyles(rangePosition, new WorksheetRangeStyle()
                    {
                        Flag = PlainStyleFlag.FontSize | PlainStyleFlag.FontName | PlainStyleFlag.VerticalAlign | PlainStyleFlag.TextWrap,
                        FontName = font.Name,
                        FontSize = font.Size,
                        TextWrapMode = TextWrapMode.WordBreak,
                        VAlign = ReoGridVerAlign.Middle,
                    });
                    sheet.SetRangeBorders(rangePosition, BorderPositions.All,new RangeBorderStyle{Color = Color.Black,Style = BorderLineStyle.Solid});
                    // name
                    sheet.ColumnHeaders[i].Text = columnElement.GetProperty("Name").GetString();
                    headerFindIndex.Add(columnElement.GetProperty("Name").GetString(), i);
                    // width
                    {
                        if (columnElement.TryGetProperty("Width", out JsonElement subElement))
                            columnWidths[i] = subElement.GetUInt16();
                    }
                    // format
                    {
                        if (columnElement.TryGetProperty("Format", out JsonElement subElement))
                            switch (subElement.GetString())
                            {
                                case "Number":
                                case "Currency":
                                    sheet.SetRangeDataFormat(rangePosition, unvell.ReoGrid.DataFormat.CellDataFormatFlag.Currency, (new unvell.ReoGrid.DataFormat.CurrencyDataFormatter.CurrencyFormatArgs
                                    {
                                        DecimalPlaces =0
                                    }));
                                    break;
                            }
                        else
                            sheet.SetRangeDataFormat(rangePosition, unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text);

                    }
                    // align
                    {
                        ReoGridHorAlign horAlign = ReoGridHorAlign.Left;
                        if (columnElement.TryGetProperty("Align", out JsonElement subElement))
                            switch (subElement.GetString())
                            {
                                case "Mid": horAlign = ReoGridHorAlign.Center; break;
                                case "Left": horAlign = ReoGridHorAlign.Left; break;
                                case "Right": horAlign = ReoGridHorAlign.Right; break;
                            }
                        sheet.SetRangeStyles(rangePosition, new WorksheetRangeStyle()
                        {
                            Flag = PlainStyleFlag.HorizontalAlign,
                            HAlign = horAlign
                        });
                    }
                    // calc

                    {
                        if (columnElement.TryGetProperty("Calc", out JsonElement subElement))
                        {
                            string exp = subElement.GetString();
                            foreach (KeyValuePair<string,int> pair in headerFindIndex.AsEnumerable())
                            {
                                exp = exp.Replace(pair.Key,((char)('A'+pair.Value)).ToString()+1);
                            }
                            RangePosition startPosition = RangePosition.FromCellPosition(0, i, 0, i);
                            RangePosition fillPosition = RangePosition.FromCellPosition(1, i, sheet.Rows - 2, i);
                            
                            sheet.SetRangeData(startPosition, exp);
                            sheet.AutoFillSerial(startPosition, fillPosition);
                        }
                    }
                    // lastSum
                    {
                        if (columnElement.TryGetProperty("LastSum", out JsonElement subElement))
                        {
                            if (subElement.GetBoolean())
                            {
                                string currChar = ((char)('A' + i)).ToString();
                                RangePosition endPosition = RangePosition.FromCellPosition(sheet.Rows, i, sheet.Rows, i);
                                sheet.SetRangeData(endPosition, "=SUM(" + currChar + "1:" + currChar + (sheet.Rows - 1) + ")");
                                restoreSet.Add(new CellConfig(i,"LastSum"));
                            }
                        }
                    }
                    // backColor
                    {
                        if (columnElement.TryGetProperty("BackColor", out JsonElement subElement))
                            sheet.SetRangeStyles(rangePosition, new WorksheetRangeStyle() {
                                Flag = PlainStyleFlag.BackColor,
                                BackColor = Color.FromArgb(Int32.Parse(subElement.ToString(), System.Globalization.NumberStyles.HexNumber))
                            });
                    }
                    // readOnly
                    {
                        if (columnElement.TryGetProperty("ReadOnly", out JsonElement subElement))
                        {
                            if (subElement.GetBoolean())
                            {
                                sheet.IterateCells(rangePosition, false, (row, col, cell) =>
                                {
                                    cell.IsReadOnly = true;
                                    return true;
                                });
                            }
                        }
                    }
                    restoreSet.Add(new CellConfig(i, sheet.Cells[0, i].Formula, sheet.Cells[0, i].DataFormat, sheet.Cells[0, i].DataFormatArgs, sheet.Cells[0, i].IsReadOnly));
                    
                    i++;
                }
            }
            
            ResetSheetWidth(null);

            isModified = false;
        }

        internal void FormResize(float scale)
        {
            for (int i = 0; i < columnWidths.Length; i++)
                columnWidths[i] = (columnWidths[i] * scale);
            ResetSheetWidth(null);
        }

        private void LoadData()
        {
            string text = "";
            object[,] data = RGUtility.ParseTabbedString(text);
            //sheet.SetRangeData(e.Range, data, true);
        }

        private int ColumnIndexFindFromName(string str)
        {
            for (int i=0;i< sheet.Columns;i++)
            {
                if (sheet.ColumnHeaders[i].Text.Equals(str))
                    return i;
            }
            return -1;
        }

        private void InitReoGrid()
        {
            reoGrid.SetSettings(WorkbookSettings.View_ShowHorScroll, false);
            reoGrid.SheetTabVisible = false;
            ControlAppearanceStyle rgcs = new ControlAppearanceStyle(Color.LightGray, Color.LightGray, true);
            rgcs.SelectionBorderWidth = 3;
            rgcs[ControlAppearanceColors.GridText] = Color.Black;
            rgcs[ControlAppearanceColors.ColHeadText] = Color.Black;
            rgcs[ControlAppearanceColors.ColHeadNormalStart] = Color.WhiteSmoke;
            rgcs[ControlAppearanceColors.ColHeadNormalEnd] = Color.WhiteSmoke;
            rgcs[ControlAppearanceColors.ColHeadSelectedStart] = Color.LightGray;
            rgcs[ControlAppearanceColors.ColHeadSelectedEnd] = Color.LightGray;
            rgcs[ControlAppearanceColors.ColHeadFullSelectedStart] = Color.LightGray;
            rgcs[ControlAppearanceColors.ColHeadFullSelectedEnd] = Color.LightGray;
            rgcs[ControlAppearanceColors.RowHeadFullSelected] = Color.LightGray;
            rgcs[ControlAppearanceColors.RowHeadText] = Color.Black;
            reoGrid.ControlStyle = rgcs;

            sheet = reoGrid.Worksheets[0];
            //sheet.SelectionForwardDirection = SelectionForwardDirection.Down;
            sheet.SetSettings(WorksheetSettings.Behavior_MouseWheelToZoom, false);
            sheet.SetSettings(WorksheetSettings.Behavior_DoubleClickToResizeHeader, false);
            sheet.SetSettings(WorksheetSettings.Behavior_ShortcutKeyToZoom, false);
            sheet.SetSettings(WorksheetSettings.Edit_AllowAdjustColumnWidth, true);
            sheet.SetSettings(WorksheetSettings.Edit_AllowAdjustRowHeight, false);
            sheet.SetSettings(WorksheetSettings.Edit_AutoExpandColumnWidth, false);
            sheet.SetSettings(WorksheetSettings.Edit_AutoExpandRowHeight, false);
            sheet.SetSettings(WorksheetSettings.View_AllowCellTextOverflow, false);
            sheet.SetSettings(WorksheetSettings.Behavior_DoubleClickToFitRowHeight, false);
            sheet.SetSettings(WorksheetSettings.Behavior_DoubleClickToFitColumnWidth, false);

            sheet.CellDataChanged += Sheet_CellDataChanged;
            sheet.BeforePaste += Sheet_BeforePaste;
            sheet.AfterPaste += Sheet_RangeDataChanged;
            sheet.RangeDataChanged += Sheet_RangeDataChanged;
            sheet.ColumnsWidthChanged += Sheet_ColumnsWidthChanged;
            sheet.BeforeCut += Sheet_BeforeCut;
            sheet.BeforeRangeMove += Sheet_BeforeRangeMove;
            sheet.CellDataChanged += (object sender, CellEventArgs e) => RefreshTotalLabel();
            sheet.RangeDataChanged += (object sender, RangeEventArgs e) => RefreshTotalLabel();
        }


        private void Sheet_BeforeRangeMove(object sender, BeforeCopyOrMoveRangeEventArgs e)
        {
            e.IsCancelled = true;
            sheet.SelectionRange = e.FromRange;
            sheet.Cut();
            sheet.SelectionRange = e.ToRange;
            sheet.Paste();
        }

        private void Sheet_BeforeCut(object sender, BeforeRangeOperationEventArgs e)
        {
            e.IsCancelled = true;
            sheet.Copy();
            sheet.DeleteRangeData(e.Range, true);
        }

        private void Sheet_BeforePaste(object sender, BeforeRangeOperationEventArgs e)
        {
            
            e.IsCancelled = true;
            PasteData(Clipboard.GetText(),e.Range);
            
        }
        public void PasteData(string text,RangePosition rangePosition)
        {
            sheet.EndEdit(EndEditReason.Cancel);
            if (!text.Contains("\t"))
                return;
            object[,] data = RGUtility.ParseTabbedString(text);
            sheet.SetRangeData(rangePosition, data, true);
        }
        public void Clear()
        {
            sheet.ClearRangeContent(sheet.UsedRange, CellElementFlag.Data, true);
        }
        private void Sheet_ColumnsWidthChanged(object sender, ColumnsWidthChangedEventArgs e)
        {
            for (int i = 0; i < sheet.Rows-1; i++)
            {
                ReshapeFontSize(sheet.Cells[i, e.Index]);
            }
            if (e.Index != sheet.Columns - 1 && !isResetColumnWidth)
            {
                var width_sum = 0;
                for (int i = 0; i < sheet.Columns - 1; i++)
                {
                    width_sum += sheet.ColumnHeaders[i].Width;
                }
                var width = reoGrid.Width - SystemInformation.VerticalScrollBarWidth - 36;
                if (width_sum > width)
                {
                    ResetSheetWidth("최대 넓이를 초과하였습니다.");
                    return;
                }
                if (e.Width < 10)
                {
                    ResetSheetWidth("최소 넓이보다 작습니다.");
                    return;
                }
                sheet.ColumnHeaders[sheet.Columns - 1].Width = (ushort)(width - width_sum);
                columnWidths[columnWidths.Length-1] = (ushort)(width - width_sum);
                if (Math.Abs(e.Width - columnWidths[e.Index]) > 1)
                {
                    columnWidths[e.Index] = e.Width;
                }
            }
        }
        
        private void ResetSheetWidth(string str)
        {
            if (str != null)
                PrintSheetError(str);
            isResetColumnWidth = true;
            for (int i = 0; i < sheet.Columns - 1; i++)
                sheet.ColumnHeaders[i].Width = (ushort)columnWidths[i];
            isResetColumnWidth = false;
            sheet.ColumnHeaders[0].Width = (ushort)columnWidths[0];
        }

        private void Sheet_RangeDataChanged(object sender, RangeEventArgs e)
        {
            if (!isModified)
            {
                docForm.Text = docForm.Text + " *";
                isModified = true;
            }
            for (int i = e.Range.Col; i <= e.Range.EndCol; i++)
            {
                for (int j = e.Range.Row; j <= e.Range.EndRow; j++)
                {
                    ReshapeFontSize(sheet.Cells[j, i]);
                }
            }
        }


        private void Sheet_CellDataChanged(object sender, CellEventArgs e)
        {
            if (!isModified)
            {
                docForm.Text = "*"+docForm.Text;
                isModified = true;
            }
            if (e.Cell.Data != null && e.Cell.DisplayText == "" && !e.Cell.IsReadOnly)
                e.Cell.Data = null;
            if (e.Cell.Data != null && e.Cell.Data.ToString().Contains(",") && e.Cell.DataFormat == unvell.ReoGrid.DataFormat.CellDataFormatFlag.Currency)
                e.Cell.Data = e.Cell.Data.ToString().Replace(",", "");
            
            ReshapeFontSize(e.Cell);
        }

        public string[][] ExtractItemArray()
        {
            sheet.EndEdit(EndEditReason.NormalFinish);
            string[][] strArr = new string[sheet.RowCount][];
            int lastRow = 0;
            for (int i = 0; i < sheet.RowCount; i++)
            {
                strArr[i] = new string[sheet.ColumnCount];
                for (int j = 0; j < sheet.ColumnCount; j++)
                {
                    if (sheet.Cells[i, j].Data != null)
                    {
                        strArr[i][j] = sheet.Cells[i, j].DisplayText;
                        if (j != 5 && j != 6)
                            lastRow = i + 1;
                    }
                }
            }
            var segment = new ArraySegment<string[]>(strArr, 0, lastRow);
            return segment.ToArray();
        }
        private void PrintSheetError(string str)
        {
            MessageBox.Show(str);
        }
        private void ReshapeFontSize(Cell cell)
        {
            float fontWidth = 0;
            float fontSize = font.Size;

            while (fontSize >= font.Size / 2)
            {
                fontWidth = g.MeasureString(cell.DisplayText, new Font(font.Name, fontSize)).Width;
                if (fontWidth > sheet.ColumnHeaders[cell.Column].Width)
                    fontSize = fontSize - 0.5f;
                else
                    break;
            }
            if (fontSize < font.Size / 2)
            {
                cell.Style.FontSize = font.Size / 2;
            }
            else
                cell.Style.FontSize = fontSize;
        }

        class CellConfig
        {
            public int column;
            public string formula;
            public bool isReadOnly;
            public CellDataFormatFlag dataFormat;
            public object dataFormatArgs;
            public string special;

            public CellConfig(int i, string special)
            {
                this.column = i;
                this.special = special;
            }

            public CellConfig(int i, string formula, CellDataFormatFlag dataFormat, object dataFormatArgs, bool isReadOnly)
            {
                this.column = i;
                this.formula = formula;
                this.dataFormat = dataFormat;
                this.dataFormatArgs = dataFormatArgs;
                this.isReadOnly = isReadOnly;
            }
        }
    }
}
