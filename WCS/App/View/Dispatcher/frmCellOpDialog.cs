using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View.Dispatcher
{
    public partial class frmCellOpDialog : Form
    {
        string BillTypeCode = "";
        string CellCode = "";
        string AreaCode = "";
        DataRow dr;
        private Dictionary<string, string> BillFields = new Dictionary<string, string>();
        private Dictionary<string, string> ProductFields = new Dictionary<string, string>();
        private Dictionary<string, string> StateFields = new Dictionary<string, string>();
        private Dictionary<string, string> TaskFields = new Dictionary<string, string>();
        BLL.BLLBase bll = new BLL.BLLBase();

        public frmCellOpDialog()
        {
            InitializeComponent();
        }
        public frmCellOpDialog(DataRow dr)
        {
            InitializeComponent();
            this.dr = dr;
        }

        private void CellOpDialog_Load(object sender, EventArgs e)
        {
            DataTable dtBillType = bll.FillDataTable("Cmd.SelectBillType", new DataParameter[] { new DataParameter("{0}", "Flag=1") });
            DataRow drNew = dtBillType.NewRow();
            drNew["BillTypeCode"] = "";
            drNew["BillTypeName"] = "请选择";
            dtBillType.Rows.InsertAt(drNew, 0);

            CellCode = dr["CellCode"].ToString();
            AreaCode = dr["AreaCode"].ToString();
            this.txtCellCode.Text = CellCode;
            this.txtBarcode.Text = dr["BarCode"].ToString();
            this.txtBillNo.Text = dr["BillNo"].ToString();
            this.txtProductCode.Text = dr["ProductCode"].ToString();
            BillTypeCode = dr["BillTypeCode"].ToString();
            this.checkBox1.Checked = dr["IsLock"].ToString() == "1";
            this.checkBox2.Checked = dr["IsActive"].ToString() == "0";
            this.checkBox3.Checked = dr["ErrorFlag"].ToString() == "1";
            if (dr["InDate"].ToString() == "")
            {
                this.dtpInDate.Checked = false;
            }
            else
            {
                this.dtpInDate.Checked = true;
                this.dtpInDate.Value = DateTime.Parse(dr["InDate"].ToString());
            }
            this.groupBox2.Enabled = false;
            BillFields.Add("BillID", "单据号码");
            BillFields.Add("BillDate", "单据日期");
            BillFields.Add("BillTypeCode", "类型编号");
            BillFields.Add("BillTypeName", "单据类型");
            BillFields.Add("AreaCode", "库区编号");
            BillFields.Add("AreaName", "库区名称");
            BillFields.Add("Memo", "备注");
            BillFields.Add("Tasker", "作业人员");
            BillFields.Add("TaskDate", "作业日期");

            ProductFields.Add("ProductCode", "产品编号");
            ProductFields.Add("ProductName", "产品名称");
            ProductFields.Add("ProductNo", "ERP物料编号");
            ProductFields.Add("Spec", "规格");

            TaskFields.Add("BillID", "单据号码");
            TaskFields.Add("TaskNo", "任务编号");
            TaskFields.Add("BillTypeName", "任务类型");
            TaskFields.Add("ProductCode", "产品编号");
            TaskFields.Add("ProductName", "产品名称");
            TaskFields.Add("ProductTypeName", "产品类型");
            TaskFields.Add("CraneNo", "堆垛机");
            TaskFields.Add("StartDate", "起始时间");
            TaskFields.Add("FinishDate", "结束时间");

            this.groupBox2.Enabled = !radioButton1.Checked;
            this.groupBox3.Enabled = !radioButton1.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要对货位" + this.txtCellCode.Text + "修改吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                DataParameter[] param;
                if (this.radioButton1.Checked)
                {
                    param = new DataParameter[] { new DataParameter("{0}", "IsLock='0'"), new DataParameter("{1}", string.Format("CellCode='{0}'", this.txtCellCode.Text)) };
                    bll.ExecNonQuery("WCS.UpdateCellByFilter", param);
                }
                else if (this.radioButton2.Checked)
                {
                    param = new DataParameter[] { new DataParameter("{0}", "IsLock='0',ProductCode='',PalletBarCode='',Quantity=0,InDate=NULL,BillNo=''"), new DataParameter("{1}", string.Format("CellCode='{0}'", this.txtCellCode.Text)) };
                    bll.ExecNonQuery("WCS.UpdateCellByFilter", param);
                }
                else if (this.radioButton3.Checked)
                {
                    param = new DataParameter[] { new DataParameter("{0}", "ErrorFlag=''"), new DataParameter("{1}", string.Format("CellCode='{0}'", this.txtCellCode.Text)) };
                    bll.ExecNonQuery("WCS.UpdateCellByFilter", param);
                }
                else if (this.radioButton4.Checked)
                {
                    param = new DataParameter[] { new DataParameter("{0}", "ErrorFlag='',ProductCode='',PalletBarCode='',Quantity=0,InDate=NULL,BillNo=''"), new DataParameter("{1}", string.Format("CellCode='{0}'", this.txtCellCode.Text)) };
                    bll.ExecNonQuery("WCS.UpdateCellByFilter", param);
                }
                else if (this.radioButton6.Checked)
                {
                    bll.ExecNonQuery("WCS.UpdateCellByTaskNo", new DataParameter[] { new DataParameter("@TaskNo", this.txtTaskNo.Text) });

                }
                else if (this.radioButton5.Checked)
                {
                    string IsLock = this.checkBox1.Checked ? "1" : "0";
                    string IsActive = this.checkBox2.Checked ? "0" : "1";
                    string ErrorFlag = this.checkBox3.Checked ? "1" : "0";

                    string sql = string.Format("IsLock='{0}'", IsLock);
                    sql += string.Format(",IsActive='{0}'", IsActive);
                    sql += string.Format(",ErrorFlag='{0}'", ErrorFlag);

                    //if (this.txtProductCode.Text.Trim().Length > 0)
                        sql += string.Format(",ProductCode='{0}'", this.txtProductCode.Text.Trim());                        

                    //if (this.txtBillNo.Text.Trim().Length > 0)
                        sql += string.Format(",BillNo='{0}'", this.txtBillNo.Text.Trim());

                    if (this.dtpInDate.Checked)
                        sql += string.Format(",InDate='{0}'", this.dtpInDate.Value);


                    sql += string.Format(",Barcode='{0}'", this.txtBarcode.Text.Trim());

                    param = new DataParameter[] { new DataParameter("{0}", sql), new DataParameter("{1}", string.Format("CellCode='{0}'", this.txtCellCode.Text)) };
                    bll.ExecNonQuery("WCS.UpdateCellByFilter", param);                    
                }

                
            }
            DialogResult = DialogResult.OK;
        }

        private void btnBillNo_Click(object sender, EventArgs e)
        {
            DataTable dt = bll.FillDataTable("WMS.SelectBillMaster", new DataParameter[] { new DataParameter("{0}", string.Format("TaskType='11' and AreaCode='{0}'",AreaCode)) });


            SelectDialog selectDialog = new SelectDialog(dt, BillFields, false);
            if (selectDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtBillNo.Text = selectDialog["BillID"];
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton1.Checked;
            this.groupBox3.Enabled = !radioButton1.Checked;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton2.Checked;
            this.groupBox3.Enabled = !radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton3.Checked;
            this.groupBox3.Enabled = !radioButton3.Checked;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton4.Checked;
            this.groupBox3.Enabled = !radioButton4.Checked;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = radioButton5.Checked;
            this.groupBox3.Enabled = !radioButton5.Checked;
        }

        private void btnProductCode_Click(object sender, EventArgs e)
        {
            if (this.txtBillNo.Text.Trim().Length <= 0)
            {
                MessageBox.Show("请先选择入库单据号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtBillNo.Focus();
                return;
            }

            DataTable dt = bll.FillDataTable("CMD.SelectProduct", new DataParameter[] { new DataParameter("{0}", string.Format("AreaCode='{0}'",AreaCode))});

            SelectDialog selectDialog = new SelectDialog(dt, ProductFields, false);
            if (selectDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtProductCode.Text = selectDialog["ProductCode"];                
            }
        }

        private void btnTaskID_Click(object sender, EventArgs e)
        {
            DataTable dt = bll.FillDataTable("WCS.SelectTask", new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TaskType='11' and WCS_TASK.CellCode='{0}'",CellCode)) });

            SelectDialog selectDialog = new SelectDialog(dt, TaskFields, false);
            if (selectDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtTaskNo.Text = selectDialog["TaskNo"];
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !radioButton6.Checked;
            this.groupBox3.Enabled = radioButton6.Checked;
        }        
    }
}
