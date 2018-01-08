using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View.Dispatcher
{
    public partial class frmCellQuery : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        
        private Dictionary<int, DataRow[]> shelf = new Dictionary<int, DataRow[]>();
        private Dictionary<int, string> ShelfCode = new Dictionary<int, string>();

        private DataTable cellTable = null;        
        private bool needDraw = false;
        private bool filtered = false;

        private int columns = 67;
        private int rows = 6;
        private int cellWidth = 0;
        private int cellHeight = 0;
        private int currentPage = 1;
        private int[] top = new int[2];
        private int left = 5;
        string CellCode = "";
        private bool IsWheel = true;

        public frmCellQuery()
        {
            InitializeComponent();
            //设置双缓冲
            SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint, true);

            Filter.EnableFilter(dgvMain);
            pnlData.Visible = true;
            pnlData.Dock = DockStyle.Fill;

            pnlChart.Visible = false;
            pnlChart.Dock = DockStyle.Fill;

            pnlChart.MouseWheel += new MouseEventHandler(pnlChart_MouseWheel);

            this.PColor.Visible = false;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsMain.Filter.Trim().Length != 0)
                {
                    DialogResult result = MessageBox.Show("重新读入数据请选择'是(Y)',清除过滤条件请选择'否(N)'", "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.No:
                            DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMain);
                            return;
                        case DialogResult.Cancel:
                            return;
                    }
                }
                ShelfCode.Clear();
                DataTable dtShelf = bll.FillDataTable("CMD.SelectShelf", new DataParameter("{0}", "Areacode='001'"));
                for (int i = 0; i < dtShelf.Rows.Count; i++)
                {
                    ShelfCode.Add(i + 1, dtShelf.Rows[i]["ShelfCode"].ToString());
                }

                btnRefresh.Enabled = false;
                btnChart.Enabled = false;

                pnlProgress.Top = (pnlMain.Height - pnlProgress.Height) / 3;
                pnlProgress.Left = (pnlMain.Width - pnlProgress.Width) / 2;
                pnlProgress.Visible = true;
                Application.DoEvents();

                cellTable = bll.FillDataTable("WCS.SelectCell1", new DataParameter("{0}", "CMD_Area.AreaCode='001'"));
                bsMain.DataSource = cellTable;

                pnlProgress.Visible = false;
                btnRefresh.Enabled = true;
                btnChart.Enabled = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("读入数据失败，原因：" + exp.Message);
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (cellTable != null && cellTable.Rows.Count != 0)
            {
                if (pnlData.Visible)
                {
                    this.PColor.Visible = true;
                    filtered = bsMain.Filter != null;
                    needDraw = true;
                    btnRefresh.Enabled = false;
                    pnlData.Visible = false;
                    pnlChart.Visible = true;
                    btnChart.Text = "列表";
                }
                else
                {
                    this.PColor.Visible = false;
                    needDraw = false;
                    btnRefresh.Enabled = true;
                    pnlData.Visible = true;
                    pnlChart.Visible = false;
                    btnChart.Text = "图形";
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (needDraw)
                {
                    Font font = new Font("微软雅黑", 12);
                    SizeF size = e.Graphics.MeasureString("第1排第5层", font);
                    float adjustHeight = Math.Abs(size.Height - cellHeight) / 2;
                    size = e.Graphics.MeasureString("13", font);
                    float adjustWidth = (cellWidth - size.Width) / 2;

                    for (int i = 0; i <= 1; i++)
                    {
                        int key = currentPage * 2 + i - 1;
                        if (!shelf.ContainsKey(key))
                        {
                            DataRow[] rows = cellTable.Select(string.Format("ShelfCode='{0}'", ShelfCode[key]), "CellCode desc");
                            shelf.Add(key, rows);
                        }

                        DrawShelf(shelf[key], e.Graphics, top[i], font, adjustWidth);

                        int tmpLeft = left + columns * cellWidth + 5;

                        for (int j = 0; j < rows; j++)
                        {
                            string s = string.Format("第{0}排第{1}层", key, Convert.ToString(rows - j).PadLeft(2, '0'));
                            e.Graphics.DrawString(s, font, Brushes.DarkCyan, tmpLeft, top[i] + (j + 1) * cellHeight + adjustHeight);
                        }
                    }

                    if (filtered)
                    {
                        int i = currentPage * 2;
                        foreach (DataGridViewRow gridRow in dgvMain.Rows)
                        {
                            DataRowView cellRow = (DataRowView)gridRow.DataBoundItem;
                            int shelf = 0;
                            for (int j = 1; j <= ShelfCode.Count; j++)
                            {
                                if (ShelfCode[j].CompareTo(cellRow["ShelfCode"].ToString()) >= 0)
                                {
                                    shelf = j;
                                    break;
                                }
                            }
                            if (shelf == i || shelf == i - 1)
                            {
                                int top = 0;
                                if (shelf % 2 == 0)
                                    top = pnlContent.Height / 2;

                                int column = Convert.ToInt32(cellRow["CellColumn"]);
                                int row = rows - Convert.ToInt32(cellRow["CellRow"]) + 1;
                                int quantity = ReturnColorFlag(cellRow["ProductCode"].ToString(), cellRow["IsActive"].ToString(), cellRow["IsLock"].ToString(), cellRow["ErrorFlag"].ToString());
                                //FillCell(e.Graphics, top, row, column, quantity);
                                FillCell(e.Graphics, top, row, column, quantity, cellRow["ShelfCode"].ToString());
                            }
                        }
                    }
                }
                PColor.Refresh();
                IsWheel = false;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        private void DrawShelf(DataRow[] cellRows, Graphics g, int top, Font font, float adjustWidth)
        {
            string shelfCode = "001";
            foreach (DataRow cellRow in cellRows)
            {
                shelfCode = cellRow["ShelfCode"].ToString();
                int column = Convert.ToInt32(cellRow["CellColumn"]) ;
                

                int row = rows - Convert.ToInt32(cellRow["CellRow"]) + 1;
                int quantity = ReturnColorFlag(cellRow["ProductCode"].ToString(), cellRow["IsActive"].ToString(), cellRow["IsLock"].ToString(), cellRow["ErrorFlag"].ToString());

                int x = left + (column-1) * cellWidth;
                int y = top + row * cellHeight;

                Pen pen = new Pen(Color.DarkCyan, 2);
                g.DrawRectangle(pen, new Rectangle(x, y, cellWidth, cellHeight));

                if (!filtered)
                {
                    FillCell(g, top, row, column, quantity, shelfCode);
                }
            }
            for (int j = 1; j <= columns; j++)
            {
                if (j == 1 && cellRows.Length < columns * rows)
                    continue;
                g.DrawString(Convert.ToString(j), new Font("微软雅黑", 10), Brushes.DarkCyan, left + (j - 1) * cellWidth + adjustWidth, top + cellHeight * (rows + 1) + 3);
            }
        }

        private void FillCell(Graphics g, int top, int row, int column, int quantity)
        {
            int x = left + (44  - column) * cellWidth;
            int y = top + row * cellHeight;
            if (quantity == 1)  //空货位锁定
                g.FillRectangle(Brushes.Yellow, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 2) //有货未锁定
                g.FillRectangle(Brushes.Blue, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 3) //有货且锁定
                g.FillRectangle(Brushes.Green, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 4) //禁用
                g.FillRectangle(Brushes.Gray, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 5) //有问题
                g.FillRectangle(Brushes.Red, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 6) //托盘
                g.FillRectangle(Brushes.Orange, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 7) //托盘锁定
                g.FillRectangle(Brushes.Gold, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
        }
        private void FillCell(Graphics g, int top, int row, int column, int quantity,string shelfCode)
        {           
            int x = left + (column-1) * cellWidth;

            int y = top + row * cellHeight;
            if (quantity == 1)  //空货位锁定
                g.FillRectangle(Brushes.Yellow, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 2) //有货未锁定
                g.FillRectangle(Brushes.Blue, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 3) //有货且锁定
                g.FillRectangle(Brushes.Green, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 4) //禁用
                g.FillRectangle(Brushes.Gray, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 5) //有问题
                g.FillRectangle(Brushes.Red, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 6) //托盘
                g.FillRectangle(Brushes.Orange, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 7) //托盘锁定
                g.FillRectangle(Brushes.Gold, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
        }
        private void pnlChart_Resize(object sender, EventArgs e)
        {
            cellWidth = (pnlContent.Width - 90 - sbShelf.Width - 20) / columns;
            cellHeight = (pnlContent.Height / 2) / (rows + 2);

            top[0] = 0;
            top[1] = pnlContent.Height / 2;            
        }

        private void pnlChart_MouseClick(object sender, MouseEventArgs e)
        {
            int i = e.Y < top[1] ? 0 : 1;
            int shelf = currentPage * 2 + i - 1;

            int column = (e.X - left) / cellWidth +1;
            
            int row = rows - (e.Y - top[i]) / cellHeight + 1;

            if (column <= columns && row <= rows)
            {
                DataRow[] cellRows = cellTable.Select(string.Format("ShelfCode='{0}' AND CellColumn='{1}' AND CellRow='{2}'", ShelfCode[shelf], column, row));
                if (cellRows.Length != 0)
                    CellCode = cellRows[0]["CellCode"].ToString();
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (cellRows.Length != 0)
                    {
                        Dictionary<string, Dictionary<string, object>> properties = new Dictionary<string, Dictionary<string, object>>();
                        Dictionary<string, object> property = new Dictionary<string, object>();
                        property.Add("产品编号", cellRows[0]["ProductCode"]);
                        property.Add("产品名称", cellRows[0]["ProductName"]);
                        property.Add("入库类型", cellRows[0]["BillTypeName"]);
                        property.Add("产品规格", cellRows[0]["Spec"]);
                        property.Add("条码", cellRows[0]["Barcode"]);
                        property.Add("重量", cellRows[0]["Weight"]);
                        //property.Add("托盘", cellRows[0]["PalletCode"]);

                        property.Add("单据号", cellRows[0]["BillNo"]);
                        property.Add("入库时间", cellRows[0]["InDate"]);
                        properties.Add("产品信息", property);

                        property = new Dictionary<string, object>();
                        property.Add("库区名称", cellRows[0]["AreaName"]);
                        property.Add("货架名称", cellRows[0]["ShelfName"]);
                        property.Add("列", column);
                        property.Add("层", row);
                        string strState = "正常";
                        if (cellRows[0]["IsLock"].ToString() == "0")
                            strState = "正常";
                        else
                            strState = "锁定";
                        if (cellRows[0]["ErrorFlag"].ToString() == "1")
                            strState = "异常";

                        if (cellRows[0]["IsActive"].ToString() == "0")
                            strState = "禁用";

                        property.Add("状态", strState);
                        properties.Add("货位信息", property);
                        if (cellRows[0]["ProductCode"].ToString().Length > 0)
                        {
                            frmCellDialog cellDialog = new frmCellDialog(properties);
                            cellDialog.ShowDialog();
                        }
                    }
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }

        }
        private void pnlChart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int i = e.Y < top[1] ? 0 : 1;
            int shelf = currentPage * 2 + i - 1;

            int column = 45 - (e.X - left) / cellWidth;
            int row = rows - (e.Y - top[i]) / cellHeight + 1;

            if (column <= columns && row <= rows)
            {
                DataRow[] cellRows = cellTable.Select(string.Format("ShelfCode='{0}' AND CellColumn='{1}' AND CellRow='{2}'", ShelfCode[shelf], column, row));
                if (cellRows.Length != 0)
                {
                    Dictionary<string, Dictionary<string, object>> properties = new Dictionary<string, Dictionary<string, object>>();
                    Dictionary<string, object> property = new Dictionary<string, object>();
                    property.Add("产品名称", cellRows[0]["ProductName"]);
                    property.Add("产品规格", cellRows[0]["Spec"]);
                    property.Add("条码", cellRows[0]["Barcode"]);
                    //property.Add("托盘条码", cellRows[0]["PalletCode"]);

                    property.Add("单据号", cellRows[0]["BillNo"]);
                    property.Add("入库时间", cellRows[0]["InDate"]);
                    properties.Add("产品信息", property);

                    property = new Dictionary<string, object>();
                    property.Add("库区名称", cellRows[0]["AreaName"]);
                    property.Add("货架名称", cellRows[0]["ShelfName"]);
                    property.Add("列", column);
                    property.Add("层", row);
                    string strState = "正常";
                    if (cellRows[0]["IsLock"].ToString() == "0")
                        strState = "正常";
                    else
                        strState = "锁定";
                    if (cellRows[0]["ErrorFlag"].ToString() == "1")
                        strState = "异常";

                    property.Add("状态", strState);
                    properties.Add("货位信息", property);

                    //CellDialog cellDialog = new CellDialog(properties);
                    //cellDialog.ShowDialog();
                }
            }
        }
        private void pnlChart_MouseEnter(object sender, EventArgs e)
        {
            pnlChart.Focus();
        }

        private void pnlChart_MouseWheel(object sender, MouseEventArgs e)
        {
            IsWheel = true;
            if (e.Delta < 0 && currentPage + 1 <= 7)
                sbShelf.Value = (currentPage) * 30;
            else if (e.Delta > 0 && currentPage - 1 >= 1)
                sbShelf.Value = (currentPage - 2) * 30;
        }

        private void sbShelf_ValueChanged(object sender, EventArgs e)
        {
            int pos = sbShelf.Value / 30 + 1;
            if (pos > 9)
                return;
            if (sbShelf.Value < 150)
            {
                rows = 6;
                columns = 67;
            }
            else
            {
                rows = 6;
                columns = 61;
            }
            if (pos != currentPage)
            {
                currentPage = pos;
                pnlChart.Invalidate();
                cellWidth = (pnlContent.Width - 90 - sbShelf.Width - 20) / columns;
            }
        }

        private void dgvMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvMain.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgvMain.RowHeadersDefaultCellStyle.Font, rectangle, dgvMain.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private int ReturnColorFlag(string ProductCode, string IsActive, string IsLock, string ErrFlag)
        {
            int Flag = 0;
            if (ProductCode == "")
            {
                if (IsLock == "1")
                {
                    Flag = 1;
                }
            }
            else
            {
                if (IsLock == "0")
                {
                    if (ProductCode == "0001")
                        Flag = 6;
                    else
                        Flag = 2;
                }
                else
                {
                    if (ProductCode == "0001")
                        Flag = 7;
                    else
                        Flag = 3;
                }
            }
            if (IsActive == "0")
                Flag = 4;
            if (ErrFlag == "1")
                Flag = 5;
            return Flag;
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataRow[] drs = cellTable.Select(string.Format("CellCode='{0}'", CellCode));
            if (drs.Length > 0)
            {
                DataRow dr = drs[0];
                frmCellOpDialog cellDialog = new frmCellOpDialog(dr);
                cellDialog.ShowDialog();
            }
        }

        private int X, Y;
        private void pnlChart_MouseMove(object sender, MouseEventArgs e)
        {

            if (IsWheel) return;
            if (X != e.X || Y != e.Y)
            {
                int i = e.Y < top[1] ? 0 : 1;
                int shelf = currentPage * 2 + i - 1;

                int column =  (e.X - left) / cellWidth + 1;
                int row = rows - (e.Y - top[i]) / cellHeight + 1;
                if (column <= columns && row <= rows && row > 0 && column > 0)
                {
                    string tip = "货架:" + shelf.ToString() + ";列:" + column.ToString() + ";层:" + row.ToString();
                    toolTip1.SetToolTip(pnlChart, tip);
                }
                else
                    toolTip1.SetToolTip(pnlChart, null);

                X = e.X;
                Y = e.Y;
            }
        }       
    }
}
