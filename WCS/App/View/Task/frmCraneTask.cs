using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Net;
using System.Net.Sockets;

namespace App.View.Task
{
    public partial class frmCraneTask : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        DataRow dr;
        string CraneNo = "01";
        string AreaCode = "";
        public frmCraneTask()
        {
            InitializeComponent();
        }

        private void frmCraneTask_Load(object sender, EventArgs e)
        {
            AreaCode = BLL.Server.GetAreaCode();
            DataTable dt = bll.FillDataTable("CMD.SelectCrane", new DataParameter[] { new DataParameter("{0}", "CMD_Crane.State='1'") });
            this.cmbCraneNo.DataSource = dt.DefaultView;
            this.cmbCraneNo.ValueMember = "CraneNo";
            this.cmbCraneNo.DisplayMember = "CraneNo";
            this.cmbTaskType.SelectedIndex = 0;
            this.txtTaskNo1.Text = DateTime.Now.ToString("yyMMdd") + "0000";
            
        }

        private void cmbCraneNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CraneNo = this.cmbCraneNo.Text;
            DataTable dt = bll.FillDataTable("CMD.SelectStation",new DataParameter("{0}",string.Format("CraneNo='{0}'",CraneNo)));
            this.cmbStationNo.DataSource = dt.DefaultView;
            this.cmbStationNo.ValueMember = "StationNo";
            this.cmbStationNo.DisplayMember = "StationNo";
        }

        private void cmbStationNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAisleNo();
            
        }
        private void BindAisleNo()
        {
            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("CraneNo='{0}' and StationNo='{1}' and AreaCode='{2}'", this.cmbCraneNo.Text,this.cmbStationNo.Text,this.AreaCode))
            };

            DataTable dt = bll.FillDataTable("CMD.SelectAisle", param);
            this.cmbAisleNo.DataSource = dt.DefaultView;
            this.cmbAisleNo.ValueMember = "AisleNo";
            this.cmbAisleNo.DisplayMember = "AisleNo";
        }      
        private void cmbTaskType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShelf();
            if (this.cmbTaskType.SelectedIndex == 1)
                this.checkBox1.Visible = true;
            else
            {
                this.checkBox1.Visible = false;
                this.checkBox1.Checked = false;
            }
        }
        private void BindShelf()
        {
            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("CraneNo='{0}' and StationNo='{1}' and AisleNo='{2}' and AreaCode='{3}'", this.cmbCraneNo.Text,this.cmbStationNo.Text,this.cmbAisleNo.Text,this.AreaCode))
            };
            DataParameter[] param1 = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("CraneNo='{0}' and StationNo='{1}' and AisleNo='{2}' and IsStation='1' and AreaCode='{3}'", this.cmbCraneNo.Text,this.cmbStationNo.Text,this.cmbAisleNo.Text,this.AreaCode))
            };
            if (this.cmbTaskType.SelectedIndex == 0)
            {
                DataTable dt = bll.FillDataTable("CMD.SelectShelf", param1);
                this.cbFromRow.DataSource = dt;
                this.cbFromRow.DisplayMember = "shelfcode";
                this.cbFromRow.ValueMember = "shelfcode";
            }
            else 
            {
                DataTable dt = bll.FillDataTable("CMD.SelectShelf", param);
                this.cbFromRow.DataSource = dt.DefaultView;
                this.cbFromRow.ValueMember = "shelfcode";
                this.cbFromRow.DisplayMember = "shelfcode";
            }

            if (this.cmbTaskType.SelectedIndex == 1)
            {
                DataTable dt = bll.FillDataTable("CMD.SelectShelf", param1);
                this.cbToRow.DataSource = dt;
                this.cbToRow.DisplayMember = "shelfcode";
                this.cbToRow.ValueMember = "shelfcode";
            }
            else
            {
                DataTable dtt = bll.FillDataTable("CMD.SelectShelf", param);
                this.cbToRow.DataSource = dtt.DefaultView;
                this.cbToRow.ValueMember = "shelfcode";
                this.cbToRow.DisplayMember = "shelfcode";
            }
        }
        //private DataRow drText(DataRow dr)
        //{
        //    if (this.cmbStationNo.Text == "01")
        //    {
        //        dr["dtText"] = "001003";
        //        dr["dtValue"] = "001003";
        //    }
        //    else if (this.cmbStationNo.Text == "02")
        //    {
        //        dr["dtText"] = "001003";
        //        dr["dtValue"] = "001003";
        //    }
        //    else if (this.cmbStationNo.Text == "03")
        //    {
        //        dr["dtText"] = "001006";
        //        dr["dtValue"] = "001006";
        //    }
        //    else if (this.cmbStationNo.Text == "04")
        //    {
        //        dr["dtText"] = "001007";
        //        dr["dtValue"] = "001007";
        //    }
        //    else if (this.cmbStationNo.Text == "05")
        //    {
        //        dr["dtText"] = "001010";
        //        dr["dtValue"] = "001010";
        //    }
        //    else
        //    {
        //        dr["dtText"] = "001011";
        //        dr["dtValue"] = "001011";
        //    }
        //    return dr;
        //}
        private void cbFromRow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbFromRow.Text == "System.Data.DataRowView")
                return;

            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("ShelfCode='{0}'",this.cbFromRow.Text))
            };


            if (this.cmbTaskType.SelectedIndex == 0)
            {
                DataTable dt = new DataTable("dt");
                dt.Columns.Add("dtText");
                dt.Columns.Add("dtValue");
                DataRow dr = dt.NewRow();

                //if (this.cbFromRow.Text == "001005")
                //{
                //    dr["dtText"] = "12";
                //    dr["dtValue"] = "12";
                //}
                //else
                //{
                    dr["dtText"] = "1";
                    dr["dtValue"] = "1";
                //}

                dt.Rows.Add(dr);
                this.cbFromColumn.DataSource = dt;
                this.cbFromColumn.DisplayMember = "dtText";
                this.cbFromColumn.ValueMember = "dtValue";
            }
            else
            {
                DataTable dt = bll.FillDataTable("CMD.SelectColumn", param);
                this.cbFromColumn.DataSource = dt.DefaultView;
                this.cbFromColumn.ValueMember = "CellColumn";
                this.cbFromColumn.DisplayMember = "CellColumn";
            }
        }
        private void cbToRow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbToRow.Text == "System.Data.DataRowView")
                return;

            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("ShelfCode='{0}'",this.cbToRow.Text))
            };

            if (this.cmbTaskType.SelectedIndex == 1)
            {
                DataTable dt = new DataTable("dt");
                dt.Columns.Add("dtText");
                dt.Columns.Add("dtValue");
                DataRow dr = dt.NewRow();

                dr["dtText"] = "1";
                dr["dtValue"] = "1";

                dt.Rows.Add(dr);
                this.cbToColumn.DataSource = dt;
                this.cbToColumn.DisplayMember = "dtText";
                this.cbToColumn.ValueMember = "dtValue";
            }
            else
            {
                DataTable dt = bll.FillDataTable("CMD.SelectColumn", param);

                this.cbToColumn.DataSource = dt.DefaultView;
                this.cbToColumn.ValueMember = "CellColumn";
                this.cbToColumn.DisplayMember = "CellColumn";
            }
        }

        private void cbFromColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbFromRow.Text == "System.Data.DataRowView")
                return;
            if (this.cbFromColumn.Text == "System.Data.DataRowView")
                return;

            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("ShelfCode='{0}' and CellColumn={1}",this.cbFromRow.Text,this.cbFromColumn.Text))
            };

            if (this.cmbTaskType.SelectedIndex == 0)
            {
                DataTable dt = new DataTable("dt");
                dt.Columns.Add("dtText");
                dt.Columns.Add("dtValue");
                DataRow dr = dt.NewRow();
                dr["dtText"] = "1";
                dr["dtValue"] = "1";
                dt.Rows.Add(dr);
                this.cbFromHeight.DataSource = dt;
                this.cbFromHeight.DisplayMember = "dtText";
                this.cbFromHeight.ValueMember = "dtValue";
            }
            else
            {
                DataTable dt = bll.FillDataTable("CMD.SelectCell", param);
                DataView dv = dt.DefaultView;
                dv.Sort = "CellRow";
                this.cbFromHeight.DataSource = dv;
                this.cbFromHeight.ValueMember = "CellRow";
                this.cbFromHeight.DisplayMember = "CellRow";
            }
        }
        private void cbToColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbToRow.Text == "System.Data.DataRowView")
                return;
            if (this.cbToColumn.Text == "System.Data.DataRowView")
                return;

            DataParameter[] param = new DataParameter[] 
            { 
                new DataParameter("{0}", string.Format("ShelfCode='{0}' and CellColumn={1}",this.cbToRow.Text,this.cbToColumn.Text))
            };

            if (this.cmbTaskType.SelectedIndex == 1)
            {
                DataTable dt = new DataTable("dt");
                dt.Columns.Add("dtText");
                dt.Columns.Add("dtValue");
                DataRow dr = dt.NewRow();

                dr["dtText"] = "1";
                dr["dtValue"] = "1";

                dt.Rows.Add(dr);
                this.cbToHeight.DataSource = dt;
                this.cbToHeight.DisplayMember = "dtText";
                this.cbToHeight.ValueMember = "dtValue";
            }
            else
            {
                DataTable dt = bll.FillDataTable("CMD.SelectCell", param);
                DataView dv = dt.DefaultView;
                dv.Sort = "CellRow";
                this.cbToHeight.DataSource = dv;
                this.cbToHeight.ValueMember = "CellRow";
                this.cbToHeight.DisplayMember = "CellRow";
            }
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            string serviceName = "CranePLC" + this.cmbCraneNo.Text.Substring(1,1);
            int[] cellAddr = new int[9];
            cellAddr[0] = 0;
            cellAddr[1] = 0;
            cellAddr[2] = 0;
            
            cellAddr[3] = int.Parse(this.cbFromColumn.Text);
            cellAddr[4] = int.Parse(this.cbFromHeight.Text);
            cellAddr[5] = int.Parse(this.cbFromRow.Text.Substring(3, 3));            
            cellAddr[6] = int.Parse(this.cbToColumn.Text);
            cellAddr[7] = int.Parse(this.cbToHeight.Text);
            cellAddr[8] = int.Parse(this.cbToRow.Text.Substring(3, 3));

            int taskNo = int.Parse(this.txtTaskNo1.Text);
            

            Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);            
            Context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
            if (int.Parse((this.cmbTaskType.SelectedIndex).ToString()) == 3)
                Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 2);
            else if (int.Parse((this.cmbTaskType.SelectedIndex).ToString()) == 5)
                Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 3);
            else
                Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 1);

            string fromStation = this.cbFromRow.Text.Substring(3, 3) + (1000 + int.Parse(this.cbFromColumn.Text)).ToString().Substring(1, 3) + (1000 + int.Parse(this.cbFromHeight.Text)).ToString().Substring(1, 3);
            string toStation = this.cbToRow.Text.Substring(3, 3) + (1000 + int.Parse(this.cbToColumn.Text)).ToString().Substring(1, 3) + (1000 + int.Parse(this.cbToHeight.Text)).ToString().Substring(1, 3);
            MCP.Logger.Info("测试任务已下发给" + this.cmbCraneNo.Text + "堆垛机;起始地址:" + fromStation + ",目标地址:" + toStation);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string serviceName = "CranePLC" + this.cmbCraneNo.Text.Substring(1, 1);
            byte[] cellAddr = new byte[9];
            cellAddr[0] = 0;
            cellAddr[1] = 0;
            
            cellAddr[2] = byte.Parse(this.cbFromColumn.Text);
            cellAddr[3] = byte.Parse(this.cbFromHeight.Text);
            cellAddr[4] = byte.Parse(this.cbFromRow.Text.Substring(3, 3));            
            cellAddr[5] = byte.Parse(this.cbToColumn.Text);
            cellAddr[6] = byte.Parse(this.cbToHeight.Text);
            cellAddr[7] = byte.Parse(this.cbToRow.Text.Substring(3, 3));
            cellAddr[8] = 1;

            sbyte[] taskNo = new sbyte[10];
            Util.ConvertStringChar.stringToBytes(this.txtTaskNo1.Text, 10).CopyTo(taskNo, 0);

            Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
            Context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
            Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 1);

            string fromStation = this.cbFromRow.Text.Substring(3, 3) + (1000 + int.Parse(this.cbFromColumn.Text)).ToString().Substring(1, 3) + (1000 + int.Parse(this.cbFromHeight.Text)).ToString().Substring(1, 3);
            string toStation = this.cbToRow.Text.Substring(3, 3) + (1000 + int.Parse(this.cbToColumn.Text)).ToString().Substring(1, 3) + (1000 + int.Parse(this.cbToHeight.Text)).ToString().Substring(1, 3);
            MCP.Logger.Info("测试任务已下发给" + this.cmbCraneNo.Text + "堆垛机;起始地址:" + toStation + ",目标地址:" + fromStation);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
                this.button1.Visible = true;
            else
                this.button1.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string serviceName = "CranePLC" + this.cmbCraneNo.Text.Substring(1, 1);
            int[] cellAddr = new int[9];
            cellAddr[0] = 0;
            cellAddr[1] = 1;

            Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
            Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 0);

            MCP.Logger.Info("测试任务已下发给" + this.cmbCraneNo.Text + "取消任务指令");
        }

        private void cmbAisleNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShelf();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string TaskNo = this.textBox3.Text;
                //DataParameter[] param1 = new DataParameter[] 
                //        {             
                //            new DataParameter("@CellCode", this.textBox3.Text),                    
                //            new DataParameter("@TaskNo", TaskNo),
                //            new DataParameter("@StationNo", this.cmbStationNo.Text)
                //        };

                DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo) };
                //bll.ExecNonQueryTran("WCS.Sp_ExecuteInStockTask", param1);
                DataTable dtXml = bll.FillDataTable("WCS.Sp_TaskProcess", param);

                //判断任务号是什么类型，如果是盘点另外处理
                param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_Task.TaskNo='{0}'", TaskNo)) };
                DataTable dt = bll.FillDataTable("WCS.SelectTask", param);

                string Flag = "BatchInStock";
                string TaskType = "";
                string CellCode = "";
                string strState = "";
                if (dt.Rows.Count > 0)
                {
                    TaskType = dt.Rows[0]["TaskType"].ToString();
                    CellCode = dt.Rows[0]["CellCode"].ToString();
                    strState = dt.Rows[0]["State"].ToString();
                    if (TaskType == "12")
                        Flag = "BatchOutStock";
                    else if (TaskType == "14")
                        Flag = "BatchCheckStock";
                }
                if (dtXml.Rows.Count > 0)
                {
                    string BillNo = dtXml.Rows[0][0].ToString();
                    if (BillNo.Trim().Length > 0)
                    {

                        string xml = Util.ConvertObj.ConvertDataTableToXmlOperation(dtXml, Flag);
                        Context.ProcessDispatcher.WriteToService("ERP", "ACK", xml);
                        MCP.Logger.Info("单号" + dtXml.Rows[0][0].ToString() + "已完成，开始上报ERP系统");
                    }
                }
            }
            catch (Exception ex)
            {

                MCP.Logger.Error(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_Task.TaskNo='{0}'", "1606010004")) };
            //DataTable dt = bll.FillDataTable("WCS.SelectTask", param);
            //View.CheckScan frm = new CheckScan(6, dt);
            //frm.ShowDialog();

            IPAddress ip = IPAddress.Parse("192.168.3.3");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 6016)); //配置服务器IP与端口  
                Console.WriteLine("连接服务器成功");
                string senStr = "ssseee";
                byte[] sendBytes = Encoding.ASCII.GetBytes(senStr);
                clientSocket.Send(sendBytes);
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                return;
            }  
        }
    }
}
