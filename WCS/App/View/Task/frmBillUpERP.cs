using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using DataGridViewAutoFilter;
using MCP;

namespace App.View
{
    public partial class frmBillUpERP : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();

        public frmBillUpERP()
        {
            InitializeComponent();
        }

        private void toolStripButton_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton_Refresh_Click(object sender, EventArgs e)
        {
            BindData();
            
        }

        
        private void BindData()
        {
            DataTable dt = bll.FillDataTable("WCS.SelectWmsBillID", new DataParameter[] { new DataParameter("{0}", "State=4 and IsUpERP=0 and TaskType in('11','14')") });
            bsMain.DataSource = dt;
        }

        private void frmInStock_Load(object sender, EventArgs e)
        {
            //this.BindData();
            for (int i = 0; i < this.dgvMain.Columns.Count - 1; i++)
                ((DataGridViewAutoFilterTextBoxColumn)this.dgvMain.Columns[i]).FilteringEnabled = true;
        }

        private void dgvMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvMain.Rows[e.RowIndex].Selected == false)
                    {
                        dgvMain.ClearSelection();
                        dgvMain.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvMain.SelectedRows.Count == 1)
                    {
                        dgvMain.CurrentCell = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }                    
                   
                    
                }
            }
        }

        private void frmInStock_Activated(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void toolStripButton_Enabled_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                try
                {

                    DataRow dr = ((DataRowView)dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].DataBoundItem).Row;

                    string BillID = dr["BillID"].ToString();
                    string TaskType = dr["TaskType"].ToString();
                    int drCount = bll.GetRowCount("WMS_BillMaster", string.Format("BillID='{0}' and IsUpERP='0'", BillID));
                    if (drCount > 0)
                    {
                        string Option = "";
                        bool blnCheck = false;

                        switch (TaskType)
                        {
                            case "11":
                                Option = "BatchInStock";
                                break;
                            case "12":
                                Option = "BatchOutStock";
                                break;
                            case "14":
                                Option = "BatchCheckStock";
                                blnCheck = true;
                                break;
                        }
                        UpErp(BillID, Option, blnCheck);
                    }
                    BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("frmCraneHandle中启用堆垛机出现异常" + ex.Message);

                }
            }
        }
        private void UpErp(string BillID,string Option,bool blnCheck)
        {
            string Cmd = "WCS.SelectUpErpTable";
            if (blnCheck)
                Cmd = "WCS.SelectUpErpCheck";

            DataTable dt = bll.FillDataTable(Cmd, new DataParameter[] { new DataParameter("@BillID", BillID) });
            if (dt.Rows.Count > 0)
            {
                string xml = Util.ConvertObj.ConvertDataTableToXmlOperation(dt, Option);
                Context.ProcessDispatcher.WriteToService("ERP", "ACK", xml);
            }

        }

      
    }
}
