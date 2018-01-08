using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using MCP;
using OPC;

namespace App.View.Task
{
    public partial class frmInStockTask :BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        string CraneNo = "02";
        string AreaCode = "";
        private DataTable dtSource = null;
        public frmInStockTask()
        {
            InitializeComponent();
        }

        private void frmInStockTask_Load(object sender, EventArgs e)
        {

            AreaCode = BLL.Server.GetAreaCode();
            DataTable dt = bll.FillDataTable("WCS.SelectInStationByArea", new DataParameter("{0}", "AreaCode=" + this.AreaCode));
            this.cmbStationNo.DataSource = dt.DefaultView;
            this.cmbStationNo.ValueMember = "InStation";
            this.cmbStationNo.DisplayMember = "InStation";
            
           
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked || radioButton3.Checked)
            {
                this.cbRow.Enabled = false;
                this.cbColumn.Enabled = false;
                this.cbHeight.Enabled = false;
            }
            else
            {
                this.cbRow.Enabled = true;
                this.cbColumn.Enabled = true;
                this.cbHeight.Enabled = true;
            }
        }

        private void cmbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAisleNo();
        }
        private void BindAisleNo()
        {
            if (this.cmbStationNo.SelectedIndex == 0)
            {
                CraneNo = "01";
            }
            else
            {
                CraneNo = "02";
            }

            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("CraneNo='{0}' and InStation='{1}' and AreaCode={2}",CraneNo, this.cmbStationNo.Text,this.AreaCode))
            };
            if (this.cmbStationNo.Text != "1" && this.cmbStationNo.Text != "2")
            {
                param = new DataParameter[] 
                { 
                new DataParameter("{0}", string.Format("CraneNo='{0}' and InStation='1' and AreaCode={1}",CraneNo,this.AreaCode))
                 };
            }

            DataTable dt = bll.FillDataTable("CMD.SelectAisle", param);
            this.cmbAisleNo.DataSource = dt.DefaultView;
            this.cmbAisleNo.ValueMember = "AisleNo";
            this.cmbAisleNo.DisplayMember = "AisleNo";

            try
            {

                object[] objRow = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService("CranePLC" + CraneNo.Substring(1, 1), "CraneAlarmCode"));
                int CraneAisle = int.Parse(objRow[4].ToString());
                DataTable dtAisleCell = bll.FillDataTable("CMD.SelectCraneAisleCell", new DataParameter("{0}", CraneAisle.ToString()));
                if (int.Parse(dtAisleCell.Rows[0][0].ToString()) > 0 && CraneNo == "01")
                {
                    this.cmbAisleNo.SelectedIndex = CraneAisle - 1;
                }
                else if (int.Parse(dtAisleCell.Rows[0][0].ToString()) > 0 && CraneNo == "02")
                {
                    this.cmbAisleNo.SelectedIndex = CraneAisle - 6;
                }
                else
                {
                    dtAisleCell = bll.FillDataTable("CMD.SelectAllAisleCell");
                    this.cmbAisleNo.SelectedIndex = int.Parse(dtAisleCell.Rows[0][0].ToString().Substring(1)) - 2;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                //}

            }
        }
        private void cbRow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbRow.Text == "System.Data.DataRowView")
                return;

            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("ShelfCode='{0}'",this.cbRow.Text))
            };
            DataTable dt = bll.FillDataTable("CMD.SelectColumn", param);

            this.cbColumn.DataSource = dt.DefaultView;
            this.cbColumn.ValueMember = "CellColumn";
            this.cbColumn.DisplayMember = "CellColumn";
        }

        private void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbRow.Text == "System.Data.DataRowView")
                return;
            if (this.cbColumn.Text == "System.Data.DataRowView")
                return;

            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("ShelfCode='{0}' and CellColumn={1}",this.cbRow.Text,this.cbColumn.Text))
            };
            DataTable dt = bll.FillDataTable("CMD.SelectCell", param);
            DataView dv = dt.DefaultView;
            dv.Sort = "CellRow";
            this.cbHeight.DataSource = dv;
            this.cbHeight.ValueMember = "CellRow";
            this.cbHeight.DisplayMember = "CellRow";
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {


                if (dtSource == null || dtSource.Rows.Count == 0)
                {
                    MessageBox.Show("熔次卷号不能为空,请扫码或输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtBarcode.Focus();
                    return;
                }

                DataTable dt;
                DataParameter[] param;


                param = new DataParameter[] 
                    { 
                      new DataParameter("@AreaCode", AreaCode) ,
                      new DataParameter("@AisleNo",this.cmbAisleNo.Text)
                    };
                string strTaskNo = "";
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    strTaskNo += "'" + dtSource.Rows[i]["TaskNo"] + "'";
                    if (i < dtSource.Rows.Count - 1)
                        strTaskNo += ",";

                }
                bll.ExecNonQuery("WCS.updateCrane", new DataParameter[] { new DataParameter("{0}", this.CraneNo), new DataParameter("{1}", "(" + strTaskNo + ")") });

                if (this.radioButton1.Checked)
                {
                    dt = bll.FillDataTable("WCS.sp_GetCellByAisle", param);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtCellCode.Text = dt.Rows[0][0].ToString();
                    }
                    else
                        this.txtCellCode.Text = "";
                }
                else if (this.radioButton2.Checked)
                {
                    this.txtCellCode.Text = this.cbRow.Text.Substring(3, 3) + (1000 + int.Parse(this.cbColumn.Text)).ToString().Substring(1, 3) + (1000 + int.Parse(this.cbHeight.Text)).ToString().Substring(1, 3);
                }
                else
                {
                    if (dtSource.Rows.Count > 1)
                    {
                        MCP.Logger.Info("缓存区只能处理单个熔次卷号！");
                        return;
                    }

                    strTaskNo = dtSource.Rows[0]["TaskNo"].ToString();
                    bll.ExecNonQuery("WCS.UpdateTaskAreaCodeByTaskNo", new DataParameter[] { new DataParameter("@AreaCode", AreaCode), new DataParameter("@TaskNo", strTaskNo) });
                    param = new DataParameter[] { new DataParameter("@TaskNo", strTaskNo) };

                    DataTable dtXml = bll.FillDataTable("WCS.Sp_TaskProcessNoShelf", param);
                    if (dtXml.Rows.Count > 0)
                    {
                        string BillNo = dtXml.Rows[0][0].ToString();
                        if (BillNo.Trim().Length > 0)
                        {
                            string xml = Util.ConvertObj.ConvertDataTableToXmlOperation(dtXml, "BatchInStock");
                            Context.ProcessDispatcher.WriteToService("ERP", "ACK", xml);
                            MCP.Logger.Info("单号" + dtXml.Rows[0][0].ToString() + "已完成，开始上报ERP系统");
                        }
                    }
                    SetControlEmpty();
                    this.txtBarcode.Focus();
                    return;

                }


                //判断货位是否为空
                param = new DataParameter[] 
                    { 
                      new DataParameter("{0}", string.Format("CellCode='{0}' and ProductCode='' and IsActive='1' and IsLock='0' and AreaCode='{1}'", this.txtCellCode.Text,AreaCode))
                    };
                dt = bll.FillDataTable("CMD.SelectCell", param);
                if (dt.Rows.Count <= 0)
                {
                    MessageBox.Show("自动获取的货位或指定的货位非空货位,请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //锁定货位



                param = new DataParameter[] 
                    {             
                        new DataParameter("@CellCode", this.txtCellCode.Text),                    
                        new DataParameter("@TaskNo", strTaskNo),
                        new DataParameter("@StationNo", this.cmbAisleNo.Text)
                    };

                bll.ExecNonQueryTran("WCS.Sp_ExecuteInStockTask", param);


                this.dtSource = null;
                this.bsMain.DataSource = null;
                SetControlEmpty();
                this.txtBarcode.Focus();

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private void SetControlEmpty()
        {
            this.txtBarcode.Text = "";
            this.txtCellCode.Text = "";

        }
        private void SetDataSource(DataTable dt)
        {
            if (dtSource == null)
            {
                dtSource = dt.Clone();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow[] drs = dtSource.Select("TaskNo='" + dt.Rows[i]["TaskNo"].ToString() + "'");
                if (drs.Length > 0)
                    continue;

                drs = dtSource.Select("ProductCode<>'" + dt.Rows[i]["ProductCode"].ToString() + "'");
                if (drs.Length > 0)
                {
                    MCP.Logger.Info("不同物料，不能同时存放同一个货位！");
                    continue;
                }


                DataRow dr = dt.Rows[i];
                dtSource.Rows.Add(dr.ItemArray);
            }
            this.txtBarcode.Text = "";
            this.bsMain.DataSource = dtSource;
        }

        

        private void frmInStockTask_Activated(object sender, EventArgs e)
        {
            this.txtBarcode.Focus();
        }

        private void cmbAisleNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbAisleNo.Text == "System.Data.DataRowView")
                return;

            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("CraneNo='{0}' and  StationNo='{1}' and AisleNo='{2}' and AreaCode='{3}'",this.CraneNo, this.cmbAisleNo.Text,this.cmbAisleNo.Text,this.AreaCode))
            };
            DataTable dt = bll.FillDataTable("CMD.SelectCellShelf", param);
            this.cbRow.DataSource = dt.DefaultView;
            this.cbRow.ValueMember = "shelfcode";
            this.cbRow.DisplayMember = "shelfcode";
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentRow == null)
                return;
            if (this.dgvMain.CurrentRow.Index >= 0)
            {
                DataRow[] drs = dtSource.Select("TaskNo='" + this.dgvMain.CurrentRow.Cells["colTaskNo"].Value.ToString() + "'");
                if (drs.Length > 0)
                    dtSource.Rows.Remove(drs[0]);
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.txtBarcode.Text.Trim().Length == 0)
                    return;
                //根据熔次卷号获取任务

                int count = bll.GetRowCount("CMD_Cell", string.Format("BarCode like '% {0} %' or BarCode like '{0} %' or BarCode like '% {0}' or Barcode='{0}'", this.txtBarcode.Text.Trim()));
                if (count > 0)
                {
                    MCP.Logger.Error("此熔次卷号 " + this.txtBarcode.Text.Trim() + "在库存中已经存在，不能入库！");
                    this.txtBarcode.Text = "";
                }
                else
                {
                    DataTable dt = bll.FillDataTable("WCS.GetTaskByBarcode", new DataParameter[] { new DataParameter("@Barcode", this.txtBarcode.Text.Trim()) });
                    if (dt.Rows.Count > 0)
                    {
                        SetDataSource(dt);
                    }
                    else
                    {

                        count = bll.GetRowCount("WCS_Task", string.Format("BarCode='{0}' and State>0 and State<7", this.txtBarcode.Text.Trim()));
                        if (count > 0)
                        {
                            MCP.Logger.Error("此熔次卷号 " + this.txtBarcode.Text.Trim() + " 已经扫描，正在执行中!");
                        }
                        else
                        {
                            MCP.Logger.Error("此熔次卷号 " + this.txtBarcode.Text.Trim() + " 还未产生入库任务，请查询后再入库！");
                        }
                    }




                    this.txtBarcode.Text = "";
                }

            }

        }       
    }
}
