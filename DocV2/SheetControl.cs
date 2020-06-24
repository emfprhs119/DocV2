using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.Data;
using unvell.ReoGrid.DataFormat;
using unvell.ReoGrid.Events;

namespace DocV2
{

    public class SheetControl
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
        AutoColumnFilter autoColumnFilter;
        int zoom;
        public float[] GetColumnWidths()
        {
            if (columnWidths == null)
                return null;
            return (float[])columnWidths.Clone();
        }
        public string[] GetColumnHeaderNames() {
            string[] strs = new string[sheet.Columns];
            for(int i = 0; i < strs.Length; i++)
            {
                strs[i] = sheet.ColumnHeaders[i].Text;
            }
            return strs;
        }
        public void SetColumnWidths(float[] widths)
        {
            if (columnWidths != null)
            {
                float sum = 0;
                float sumRp = 0;
                foreach (float w in columnWidths)
                    sum += w;
                foreach (float w in widths)
                    sumRp += w;
                for(int i=0;i< columnWidths.Length; i++)
                {
                    columnWidths[i] = widths[i] * sum/sumRp;
                }
            }else
                columnWidths = widths;
            ResetSheetWidth(null);
        }
        public string GetSelectionRowLine()
        {
            string str = sheet.GetCell(sheet.SelectionRange.Row, 0).DisplayText;
            for (int i = 1; i < sheet.ColumnCount; i++)
                str += "_"+sheet.GetCell(sheet.SelectionRange.Row, i).DisplayText;
            return str;
        }
        public void SetSelectionMode(WorksheetSelectionMode mode)
        {
            sheet.SelectionMode = mode;
        }
        public void EnableFilter()
        {
            if (autoColumnFilter != null)
                autoColumnFilter.Detach();
            autoColumnFilter = sheet.CreateColumnFilter(0,sheet.ColumnCount-1);
        }

        public string TotalString()
        {
            float sumValue = 0;
            for (int i = 0; i < sheet.Columns; i++)
            {
                if (sheet.GetCellData(sheet.Rows - 1, i) != null)
                {
                    float tmp;
                    if (float.TryParse(sheet.GetCellData((sheet.Rows - 1), i).ToString(), out tmp))
                    {
                        sumValue += tmp;
                    }
                    else
                    {
                        return "NaN";
                    }
                }
            }
            return string.Format("{0:c}", sumValue).Replace("₩","");
        }

        private void RefreshTotalLabel()
        {
            totalLabel.Text = TotalString() + "원";
        }

        public SheetControl(DocForm docForm, unvell.ReoGrid.ReoGridControl reoGrid, Label label23, Graphics g,int zoom,bool useFunc)
        {
            this.docForm = docForm;
            this.reoGrid = reoGrid;
            totalLabel = label23;
            this.g = g;
            isModified = true;
            this.zoom = zoom;
            InitReoGrid(useFunc);
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

        internal void PasteData(DataTable dataTable)
        {
            sheet.Rows = dataTable.Rows.Count;
            sheet.Columns = dataTable.Columns.Count;
            for (int i = 0; i < dataTable.Columns.Count; i++)
                sheet.ColumnHeaders[i].Text = dataTable.Columns[i].Caption;
            sheet["A1"] = dataTable;
            RangePosition rangePosition = RangePosition.FromCellPosition(0, 0, sheet.Rows, sheet.Columns);
            if (dataTable.Rows.Count != 0)
            {
                reoGrid.Enabled = true;
                sheet.IterateCells(rangePosition, false, (row, col, cell) =>
                {
                    cell.IsReadOnly = true;
                    return true;
                });
                sheet.SetRangeBorders(rangePosition, BorderPositions.All, new RangeBorderStyle { Color = Color.Black, Style = BorderLineStyle.Solid });
                sheet.SetRangeDataFormat(rangePosition, unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text);
            }
            else
                reoGrid.Enabled = false;
        }
        public void RegistFunc(System.EventHandler<unvell.ReoGrid.Events.CellMouseEventArgs> sheet_CellMouseDown)
        {
            sheet.CellMouseDown += sheet_CellMouseDown;
            sheet.SelectionRangeChanged += (object sender, RangeEventArgs e)=> {
                if (sheet.SelectionMode == WorksheetSelectionMode.SingleRow)
                    sheet.SelectionRange = new RangePosition(e.Range.Row, 0, 1, sheet.ColumnCount);
            };
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
        public void SheetFontChange()
        {
            SheetFontChange(new System.Drawing.Font(Properties.Settings.Default["SheetFontName"].ToString(), float.Parse(Properties.Settings.Default["SheetFontSize"].ToString())));
        }
        public void SheetFontChange(Font newFont)
        {

            Properties.Settings.Default["SheetFontName"] = newFont.Name;
            Properties.Settings.Default["SheetFontSize"] = newFont.Size.ToString();
            Properties.Settings.Default.Save();
            this.font = new Font(newFont.Name,newFont.Size);

            /* unkown bug */
            var rowCount = sheet.Rows-1;
            if (sheet.Columns == 8)
                rowCount = sheet.Rows - 2;
            /* mayby relative display cell */

            RangePosition rangePosition = RangePosition.FromCellPosition(0, 0, rowCount, sheet.Columns-1);
            
            sheet.SetRangeStyles(rangePosition.ToAddress(), new WorksheetRangeStyle()
            {
                Flag =  PlainStyleFlag.FontName| PlainStyleFlag.FontSize,
                FontName = font.Name,
                FontSize = font.Size
            });
        }
        public void InitSheetStyle(JsonElement columnElements)
        {
            sheet.SetRowsHeight(0, initMaxRow, 20);
            restoreSet = new HashSet<CellConfig>();
            var columnSize = columnElements.GetArrayLength();
            sheet.Columns = columnSize;
            columnWidths = new float[sheet.Columns];
            sheet.Rows = initMaxRow + 1;
            sheet.HideRows(initMaxRow, 1);
            sheet.RowHeaderWidth = 25;
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
                                exp = exp.Replace(pair.Key, "IF(ISNUMBER("+((char)('A'+pair.Value)).ToString()+1+ "),"+((char)('A'+pair.Value)).ToString()+1+",0)");
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

            RangePosition lastRows = RangePosition.FromCellPosition(sheet.Rows, 0, sheet.Rows, sheet.Columns);
            sheet.IterateCells(lastRows, false, (row, col, cell) =>
            {
                cell.IsReadOnly = true;
                return true;
            });
            Space();
            RefreshTotalLabel();
            SheetFontChange();
            isModified = false;
        }

        internal void FormResize(float scale)
        {
            if (columnWidths == null)
                return;
            for (int i = 0; i < columnWidths.Length; i++)
                columnWidths[i] = (columnWidths[i] * scale);
            ResetSheetWidth(null);
        }
        internal void FormResizeOrigin(int width)
        {
            float max = 0;
            for (int i = 0; i < columnWidths.Length; i++)
                max += columnWidths[i];
            FormResize((width/2.2f) / max);
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

        private void InitReoGrid(bool useFunc)
        {
            reoGrid.SetSettings(WorkbookSettings.View_ShowHorScroll, false);
            reoGrid.SheetTabVisible = false;
            ControlAppearanceStyle rgcs = new ControlAppearanceStyle(Color.LightGray, Color.LightGray, true);
            rgcs.SelectionBorderWidth = 5;
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
            sheet.SelectionForwardDirection = SelectionForwardDirection.Down;
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
            
            
            //sheet.SetSettings(WorksheetSettings.View_ShowColumnHeader, false);
            if (useFunc)
            {
                sheet.CellDataChanged += Sheet_CellDataChanged;
                sheet.BeforePaste += Sheet_BeforePaste;
                sheet.AfterPaste += Sheet_RangeDataChanged;
                sheet.RangeDataChanged += Sheet_RangeDataChanged;
                sheet.BeforeRangeMove += Sheet_BeforeRangeMove;
                sheet.CellDataChanged += (object sender, CellEventArgs e) => {
                    if (e.Cell.Row == sheet.RowCount-1)
                        RefreshTotalLabel();
                    };
                sheet.BeforeCellEdit += Sheet_BeforeCellEdit;
                sheet.AfterCellEdit += Sheet_AfterCellEdit;
                sheet.SelectionRangeChanged += Sheet_SelectionRangeChanged;
                sheet.AfterCellKeyDown += Sheet_AfterCellKeyDown;
            }else
                sheet.SetSettings(WorksheetSettings.View_ShowRowHeader, false);
            
            sheet.ColumnsWidthChanged += Sheet_ColumnsWidthChanged;
            sheet.BeforeCut += Sheet_BeforeCut;
            font = new System.Drawing.Font(Properties.Settings.Default["SheetFontName"].ToString(), float.Parse(Properties.Settings.Default["SheetFontSize"].ToString()));

            for (int i = 0;i<zoom;i++)
                sheet.ZoomIn();
        }

        public void Select_Single()
        {
            //sheet.SelectionRange = new RangePosition(sheet.SelectionRange.Row,0, 1, sheet.Columns);
            sheet.SelectRows(sheet.SelectionRange.Row, 1);
        }

        private void Sheet_AfterCellKeyDown(object sender, AfterCellKeyDownEventArgs e)
        {
            if (e.Cell.Data == null && !e.Cell.IsReadOnly)
            {
                e.Cell.Data = " ";
            }
        }

        private void Sheet_SelectionRangeChanged(object sender, RangeEventArgs e)
        {
            if (docForm.formName != "거래명세서")
                return;
            if (e.Range.Cols == 1 && e.Range.Rows == 1 && e.Range.Col == 0)
            {
                if (sheet.Cells[e.Range.Row,0].Data.Equals(" "))
                {
                    if (e.Range.Row > 0)
                        sheet.Cells[e.Range.Row, 0].Data = sheet.Cells[e.Range.Row - 1, 0].Data;
                    else
                        sheet.Cells[e.Range.Row, 0].Data = docForm.ReadToday();

                }
            }
        }

        private void Sheet_AfterCellEdit(object sender, CellAfterEditEventArgs e)
        {
            if (e.NewData.ToString().Equals(""))
                e.NewData = " ";
        }

        private void Sheet_BeforeCellEdit(object sender, CellBeforeEditEventArgs e)
        {
            if (e.EditText.Equals(" "))
                e.EditText = "";
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
            RangePosition allCols = RangePosition.FromCellPosition(0, 0, sheet.Rows, sheet.Columns);
            sheet.SuspendDataChangedEvents();
            sheet.SuspendFormulaReferenceUpdates();
            sheet.SuspendUIUpdates();
            
            if (text == "")
                text = ",";
            
            text = text.Replace(",", "&comma;");
            object[,] data = RGUtility.ParseTabbedString(text);
            sheet.SetRangeData(rangePosition, data, true);
            sheet.ResumeDataChangedEvents();
            sheet.ResumeFormulaReferenceUpdates();
            sheet.ResumeUIUpdates();
            Space();
            sheet.Recalculate();
        }
        public void Clear()
        {
            sheet.ClearRangeContent(sheet.UsedRange, CellElementFlag.Data, false);
        }
        public void Space()
        {
            RangePosition allCols = RangePosition.FromCellPosition(0, 0, sheet.Rows, sheet.Columns);
            sheet.SuspendDataChangedEvents();
            sheet.SuspendFormulaReferenceUpdates();
            sheet.SuspendUIUpdates();
            sheet.IterateCells(allCols, false, (row, col, cell) =>
            {
                if ((cell.Data == null || cell.Data.Equals("")) && !cell.IsReadOnly)
                {
                    cell.Data = " ";
                }
                else
                {
                    if (cell.DisplayText.Contains("&comma;")){
                        if (cell.DataFormat == CellDataFormatFlag.Number || cell.DataFormat == CellDataFormatFlag.Currency)
                            cell.Data = cell.DisplayText.Replace("&comma;", "");
                        else
                            cell.Data = cell.DisplayText.Replace("&comma;", ",");
                    }

                    ReshapeFontSize(cell);
                }
                return true;
            });
            sheet.ResumeDataChangedEvents();
            sheet.ResumeFormulaReferenceUpdates();
            sheet.ResumeUIUpdates();
        }
        private void Sheet_ColumnsWidthChanged(object sender, ColumnsWidthChangedEventArgs e)
        {
            if (reoGrid.Width < 10)
                return;
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
                
                var width = reoGrid.Width*(1-(0.0585*zoom)) - SystemInformation.VerticalScrollBarWidth - 17.5;

                /*
                if (width_sum > width)
                {
                    ResetSheetWidth("최대 넓이를 초과하였습니다.");
                    return;
                }
                if (e.Width < 10)
                {
                    ResetSheetWidth("최소 넓이보다 작습니다.");
                    return;
                }*/
                
                sheet.ColumnHeaders[sheet.Columns - 1].Width = (ushort)(width - width_sum-8);
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
                    if (sheet.Cells[j, i].Data == null)
                    {
                        sheet.Cells[j, i].Data = " ";
                    }
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
                    if (sheet.Cells[i, j].Data != null && !sheet.Cells[i, j].Data.Equals(" "))
                    {
                        strArr[i][j] = sheet.Cells[i, j].DisplayText;
                        if (!sheet.Cells[i, j].IsReadOnly)
                            lastRow = i + 1;
                        //if (j != 5 && j != 6)
                    }
                }
            }
            var segment = new ArraySegment<string[]>(strArr, 0, lastRow);
            return segment.ToArray();
        }
        public string[] SumValues()
        {
            string[] strArr = new string[sheet.ColumnCount];
            for (int j = 0; j < sheet.ColumnCount; j++)
            {
                strArr[j] = sheet.Cells[sheet.RowCount-1, j].DisplayText;
            }
            return strArr;
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
