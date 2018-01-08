using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View
{
    public partial class frmReassign : Form
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        DataRow dr;
        string CraneNo = "01";

        public frmReassign()
        {
            InitializeComponent();
        }
        public frmReassign(DataRow dr)
        {
            InitializeComponent();
            this.dr = dr;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentRow == null)
                return;

            if (this.dgvMain.CurrentRow.Index == -1)
            {
                MessageBox.Show("请选择货位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string CellCode = this.dgvMain.Rows[this.dgvMain.CurrentRow.Index].Cells[0].Value.ToString();
            string TaskNo = this.txtTaskNo.Text;

            DataParameter[] param;
            param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("CellCode='{0}' and ProductCode='{1}' and IsActive='1' and IsLock='0' and ErrorFlag!='1'",CellCode,this.txtProductCode.Text))
            };
            DataTable dt = bll.FillDataTable("CMD.SelectCell", param);
            if (dt.Rows.Count <= 0)
            {
                MessageBox.Show("此货位非指定出库产品货位,请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            param = new DataParameter[] 
            { 
                new DataParameter("@TaskNo", TaskNo), 
                new DataParameter("@NewCellCode", CellCode),
                new DataParameter("@IsTarget", "1")
            };

            bll.ExecNonQuery("WCS.Sp_UpdateTaskCellCode", param);
            
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void frmReassign_Load(object sender, EventArgs e)
        {
            this.txtTaskNo.Text = dr["TaskNo"].ToString();
            this.txtCellCode.Text = dr["CellCode"].ToString();
            this.txtAisleNo.Text = dr["AisleNo"].ToString();
            this.txtCraneNo.Text = dr["CraneNo"].ToString();
            this.txtProductCode.Text = dr["ProductCode"].ToString();
            this.txtProductName.Text = dr["ProductName"].ToString();
            this.txtSpec.Text = dr["Spec"].ToString();
            
            CraneNo = dr["CraneNo"].ToString();

            string filter = string.Format("CMD_Cell.ProductCode='{0}' and CMD_Cell.IsLock='0' and CMD_Cell.IsActive='1' and CMD_Cell.ErrorFlag!='1' and CMD_Shelf.CraneNo='{1}' and CMD_Shelf.AisleNo='{2}' ", this.txtProductCode.Text, CraneNo, this.txtAisleNo.Text);

            DataTable dt = bll.FillDataTable("WCS.SelectCellByFilter", new DataParameter[] { new DataParameter("{0}", filter) });

            this.bsMain.DataSource = dt;
        }

        private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {                
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        
    }
    
}
