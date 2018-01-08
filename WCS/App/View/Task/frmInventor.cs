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

namespace App.View.Task
{
    public partial class frmInventor : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();

        public frmInventor()
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

        private void toolStripButton_Cancel_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentRow.Index >= 0)
            {
                if (this.dgvMain.SelectedRows[0].Cells["colState"].Value.ToString() == "等待")
                {
                    if (DialogResult.Yes == MessageBox.Show("您确定要取消此任务吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        string TaskNo = this.dgvMain.SelectedRows[0].Cells["colTaskNo"].Value.ToString();
                        DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo) };
                        bll.ExecNonQueryTran("WCS.Sp_TaskCancelProcess", param);
                        this.BindData();
                    }
                }
                else
                {
                    MessageBox.Show("选中的状态非[等待],请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void BindData()
        {
            DataTable dt = bll.FillDataTable("WCS.SelectTask", new DataParameter[] { new DataParameter("{0}", "WCS_TASK.State in('0','1','2','3','4','5','6','10','11','12','13') and WCS_TASK.TaskType='14' And WCS_TASK.AreaCode='" + BLL.Server.GetAreaCode() + "'") });
            bsMain.DataSource = dt;
        }

        private void frmMoveStock_Load(object sender, EventArgs e)
        {
            //this.BindData();
            for (int i = 0; i < this.dgvMain.Columns.Count - 1; i++)
                ((DataGridViewAutoFilterTextBoxColumn)this.dgvMain.Columns[i]).FilteringEnabled = true;
            this.txtBarCode.Focus();
        }
        private void dgvMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
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
                    //弹出操作菜单
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("0");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("3");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("5");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            DataRow dr = ((DataRowView)dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].DataBoundItem).Row;
            string State = dr["State"].ToString();
            if (State != "0" && State != "8")
            {
                string TaskNo = dr["TaskNo"].ToString();
                MCP.Logger.Info("任务号：" + TaskNo + "正在执行中请在监控界面变更状态为取消!");
                return;
            }
            UpdatedgvMainState("9");
        }
        private void UpdatedgvMainState(string State)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@State", State) };
                bll.ExecNonQueryTran("WCS.Sp_UpdateTaskState", param);

                BindData();
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("4");
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("6");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("7");
        }

        private void frmInventor_Activated(object sender, EventArgs e)
        {
            this.BindData();
            this.txtBarCode.Focus();
        }





        private void txtBarCode_TextChanged(object sender, EventArgs e)
        {
            if (this.txtBarCode.Text != "")
            {

                string BillID = bll.GetFieldValue("WCS_TASK", "BillID", string.Format("State=0 and CheckBarcode='' and CellCode='' and  TaskType='14' and AreaCode='{0}'  and Barcode='{1}'", BLL.Server.GetAreaCode(), this.txtBarCode.Text));
                if (BillID.Trim().Length > 0)
                {
                    bll.ExecNonQuery("WCS.UpdateCacheInventorBarCode", new DataParameter[] { new DataParameter("@AreaCode", BLL.Server.GetAreaCode()), new DataParameter("@BarCode", this.txtBarCode.Text) });
                    DataTable dt = bll.FillDataTable("WCS.SelectUpErpCheck", new DataParameter[] { new DataParameter("@BillID", BillID) });
                    if (dt.Rows.Count > 0)
                    {
                        string xml = Util.ConvertObj.ConvertDataTableToXmlOperation(dt, "BatchCheckStock");
                        Context.ProcessDispatcher.WriteToService("ERP", "ACK", xml);
                    }
                }
                else
                {
                    MessageBox.Show("扫描的熔次卷号不在缓存中，请确认", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.txtBarCode.Text = "";
                this.txtBarCode.Focus();
                this.BindData();
            }
        }

        private void toolStripButton_OK_Click(object sender, EventArgs e)
        {
            DataTable dtCache = bll.FillDataTable("WCS.SelectCacheInventor", new DataParameter[] { new DataParameter("@AreaCode", BLL.Server.GetAreaCode()) });
            if (dtCache.Rows.Count > 0)
            {

                if (DialogResult.Yes == MessageBox.Show("缓存中还有未扫描的熔次卷号，是否确认盘点完成？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {

                    DataParameter[] param = new DataParameter[] { new DataParameter("@AreaCode", BLL.Server.GetAreaCode()) };
                    bll.ExecNonQuery("WCS.UpdateCacheInventorFinished", param);

                   string Cmd = "WCS.SelectUpErpCheck";

                   DataTable dtBill = dtCache.DefaultView.ToTable(true, "BillID");

                   for (int i = 0; i < dtBill.Rows.Count; i++)
                   {
                       DataTable dt = bll.FillDataTable(Cmd, new DataParameter[] { new DataParameter("@BillID", dtBill.Rows[i][0].ToString()) });
                       if (dt.Rows.Count > 0)
                       {
                           string xml = Util.ConvertObj.ConvertDataTableToXmlOperation(dt, "BatchCheckStock");
                           Context.ProcessDispatcher.WriteToService("ERP", "ACK", xml);
                       }
                   }

                    this.BindData();
                }
            }

        }        
    }
}
