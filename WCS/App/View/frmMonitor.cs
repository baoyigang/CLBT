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
using OPC;

namespace App.View
{
    public partial class frmMonitor : BaseForm
    {
        private Point InitialP1;
        private Point InitialP2;

        float colDis1 = 20f;
        float rowDis1 = 55f;

        float colDis2 = 21.55f;
        float rowDis2 = 60f;
        
        private System.Timers.Timer tmWorkTimer = new System.Timers.Timer();
        private System.Timers.Timer tmCrane1 = new System.Timers.Timer();
        private System.Timers.Timer tmCrane2 = new System.Timers.Timer();
        BLL.BLLBase bll = new BLL.BLLBase();
        Dictionary<int, string> dicCraneFork = new Dictionary<int, string>();
        Dictionary<int, string> dicCraneState = new Dictionary<int, string>();
        Dictionary<int, string> dicCraneAction = new Dictionary<int, string>();
        Dictionary<int, string> dicCraneOver = new Dictionary<int, string>();
        DataTable dtCraneErr;
        public frmMonitor()
        {
            InitializeComponent();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Point P2 = picCrane1.Location;
            P2.X = P2.X - 90;

            this.picCrane1.Location = P2;
        }

        private void frmMonitor_Load(object sender, EventArgs e)
        {
            //MainData.OnTask += new TaskEventHandler(Data_OnTask);
            Cranes.OnCrane += new CraneEventHandler(Monitor_OnCrane);
            AddDicKeyValue();

            InitialP1 = picCrane1.Location;
            picCrane1.Parent = pictureBox1;
            picCrane1.BackColor = Color.Transparent;

            InitialP2 = picCrane2.Location;
            picCrane2.Parent = pictureBox1;
            picCrane2.BackColor = Color.Transparent;


            //this.BindData();
            //for (int i = 0; i < this.dgvMain.Columns.Count - 1; i++)
            //    ((DataGridViewAutoFilterTextBoxColumn)this.dgvMain.Columns[i]).FilteringEnabled = true;

            //tmWorkTimer.Interval = 3000;
            //tmWorkTimer.Elapsed += new System.Timers.ElapsedEventHandler(tmWorker);
            //tmWorkTimer.Start();

            tmCrane1.Interval = 3007;
            tmCrane1.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker1);
            tmCrane1.Start();

            tmCrane2.Interval = 3007;
            tmCrane2.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker2);
            tmCrane2.Start();
        }
        private void AddDicKeyValue()
        {
            dicCraneFork.Add(0, "非原点");
            dicCraneFork.Add(1, "原点");

            dicCraneState.Add(0, "空闲");
            dicCraneState.Add(1, "等待");
            dicCraneState.Add(2, "定位");
            dicCraneState.Add(3, "取货");
            dicCraneState.Add(4, "放货");
            dicCraneState.Add(98, "维修");

            dicCraneAction.Add(0, "非自动");
            dicCraneAction.Add(1, "自动");

            dtCraneErr = bll.FillDataTable("WCS.SelectCraneError"); ;
        }
        //void Data_OnTask(TaskEventArgs args)
        //{
        //    if (InvokeRequired)
        //    {
        //        BeginInvoke(new TaskEventHandler(Data_OnTask), args);
        //    }
        //    else
        //    {
        //        lock (this.dgvMain)
        //        {
        //            DataTable dt = args.datatTable;
        //            this.bsMain.DataSource = dt;
        //            for (int i = 0; i < this.dgvMain.Rows.Count; i++)
        //            {
        //                if (dgvMain.Rows[i].Cells["colErrCode"].Value.ToString() != "0")
        //                    this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.Red;
        //                else
        //                {
        //                    if(i%2==0)
        //                        this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.White;
        //                    else
        //                        this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);

        //                }
        //            }
        //        }
        //    }
        //}
        void Monitor_OnCrane(CraneEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CraneEventHandler(Monitor_OnCrane), args);
            }
            else
            {
                Crane crane = args.crane;
                TextBox txt = GetTextBox("txtTaskNo", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.TaskNo;

                txt = GetTextBox("txtState", crane.CraneNo);
                if (txt != null && dicCraneState.ContainsKey(crane.TaskType))
                    txt.Text = dicCraneState[crane.TaskType];

                txt = GetTextBox("txtCraneAction", crane.CraneNo);
                if (txt != null && dicCraneAction.ContainsKey(crane.Action))
                    txt.Text = dicCraneAction[crane.Action];

                txt = GetTextBox("txtRow", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Row.ToString();

                txt = GetTextBox("txtColumn", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Column.ToString();

                //堆垛机位置
                if (crane.CraneNo == 1)
                {
                    this.picCrane1.Visible = true;
                    Point P1 = InitialP1;
                    if(crane.Column<67)
                        P1.X = P1.X + (int)((crane.Column-1) * colDis1);
                    else
                        P1.X=1090;// = picCar.Location.X+15;

                    P1.Y = P1.Y + (int)(rowDis1 * (crane.Row-1));
                    this.picCrane1.Location = P1;

                    //Point P2 = InitialP2;
                    //P2.Y = P2.Y + (int)(rowDis * (crane.Row - 1));
                    //this.picCar.Location = P2;
                }
                if (crane.CraneNo==2)
                {

                    this.picCrane2.Visible = true;
                    Point P2 = InitialP2;
                    if (crane.Column < 61)
                    {
                        P2.X = P2.X + (int)((crane.Column) * colDis2);
                    }
                    else 
                    {
                        P2.X = 1090; 
                    }

                    P2.Y = P2.Y + (int)(rowDis2 * (crane.Row - 6));
                    this.picCrane2.Location = P2;
                }

                txt = GetTextBox("txtHeight", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Height.ToString();

                txt = GetTextBox("txtForkStatus", crane.CraneNo);
                if (txt != null && dicCraneFork.ContainsKey(crane.ForkStatus))
                    txt.Text = dicCraneFork[crane.ForkStatus];

                string strError = "";
                txt = GetTextBox("txtErrorDesc", crane.CraneNo);
                if (txt != null)
                {
                    if (crane.ErrCode != 0)
                    {
                        DataRow[] drs = dtCraneErr.Select(string.Format("CraneErrCode={0}", crane.ErrCode));
                        if (drs.Length > 0)
                            strError = drs[0]["CraneErrDesc"].ToString();
                        else
                            strError = "堆垛机未知错误！";
                    }
                    else
                    {
                        strError = "";
                    }
                    txt.Text = strError;
                }
                //更新错误代码、错误描述
                //更新任务状态为执行中
                //bll.ExecNonQuery("WCS.UpdateTaskError", new DataParameter[] { new DataParameter("@CraneErrCode", crane.ErrCode.ToString()), new DataParameter("@CraneErrDesc", dicCraneError[crane.ErrCode]), new DataParameter("@TaskNo", crane.TaskNo) });
                txt = GetTextBox("txtErrorNo", crane.CraneNo);
                if (txt != null)
                {
                    if (crane.ErrCode > 0)
                    {
                        DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", crane.TaskNo), new DataParameter("@CraneErrCode",                                crane.ErrCode.ToString()), new DataParameter("@CraneErrDesc", strError) };
                        bll.ExecNonQueryTran("WCS.Sp_UpdateTaskError", param);
                  
                            if (txt.Text.Trim() == "")
                                txt.Text = "0";
                            if (crane.ErrCode != int.Parse(txt.Text))
                                Logger.Error(crane.CraneNo.ToString() + "堆垛机执行时出现错误,代码:" + crane.ErrCode.ToString() + ",描述:" + strError);
                            txt.Text = crane.ErrCode.ToString();
                   

                    }
                    else
                    {
                        txt.Text = "0";
                    }
                }
            }
        }
        TextBox GetTextBox(string name, int CraneNo)
        {
            Control[] ctl = this.Controls.Find(name + CraneNo.ToString(), true);
            if (ctl.Length > 0)
                return (TextBox)ctl[0];
            else
                return null;
        }
        
        //private void tmWorker(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        tmWorkTimer.Stop();
        //        DataTable dt = GetMonitorData();
        //        MainData.TaskInfo(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message);
        //    }
        //    finally
        //    {
        //        tmWorkTimer.Start();
        //    }
        //}

        private void tmCraneWorker1(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCrane1.Stop();
                string binary = Convert.ToString(255, 2).PadLeft(8, '0');
                
                string serviceName = "CranePLC1";
                string plcTaskNo = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneTaskNo")).ToString();

                string craneMode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneMode")).ToString();
                string craneFork = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneFork")).ToString();
                object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "CraneAlarmCode"));

                Crane crane1 = new Crane();
                crane1.CraneNo = 1;
                crane1.Row = int.Parse(obj[4].ToString());
                crane1.Column = int.Parse(obj[2].ToString());
                crane1.Height = int.Parse(obj[3].ToString());
                crane1.ForkStatus = int.Parse(craneFork);
                crane1.Action = int.Parse(craneMode);
                crane1.TaskType = int.Parse(obj[1].ToString());
                crane1.ErrCode = int.Parse(obj[0].ToString());
                crane1.PalletCode = "";
                crane1.TaskNo =  plcTaskNo ;

                Cranes.CraneInfo(crane1);
            }
            catch (Exception ex)
            {
                Logger.Error("1号堆垛机监控故障:"+ex.Message);
            }
            finally
            {
                tmCrane1.Start();
            }
        }

        private void tmCraneWorker2(object sender, System.Timers.ElapsedEventArgs e) 
        {
            try
            {
                tmCrane2.Stop();

                string serviceName = "CranePLC2";

                string plcTaskNo = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneTaskNo")).ToString();

                string craneMode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneMode")).ToString();
                string craneFork = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneFork")).ToString();
                object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "CraneAlarmCode"));

                Crane crane2 = new Crane();
                crane2.CraneNo = 2 ;
                crane2.Row = int.Parse(obj[4].ToString());
                crane2.Column = int.Parse(obj[2].ToString());
                crane2.Height = int.Parse(obj[3].ToString());
                crane2.ForkStatus = int.Parse(craneFork);
                crane2.Action = int.Parse(craneMode);
                crane2.TaskType = int.Parse(obj[1].ToString());
                crane2.ErrCode = int.Parse(obj[0].ToString());
                crane2.PalletCode = "";
                crane2.TaskNo = plcTaskNo;

                Cranes.CraneInfo(crane2);
            }
            catch (Exception ex)
            {

                Logger.Error("2号堆垛机监控故障:" + ex.Message);
            }
            finally 
            {
                tmCrane2.Start();
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            if (this.btnBack1.Text == "启动")
            {
                Context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 1);
                this.btnBack1.Text = "停止";
            }
            else
            {
                Context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 0);
                this.btnBack1.Text = "启动";
            }
        }

        private void btnBack1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要召回1号堆垛机到初始位置?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Context.ProcessDispatcher.WriteToService("CranePLC1", "Reset", 1);
                Logger.Info("1号堆垛机下发召回命令");
            }
        }
        private void PutCommand(string craneNo, byte TaskType)
        {
            string serviceName = "CranePLC" + craneNo;
            int[] cellAddr = new int[9];
            cellAddr[TaskType] = 1;            

            //cellAddr[3] = int.Parse(this.cbFromColumn.Text);
            //cellAddr[4] = int.Parse(this.cbFromHeight.Text);
            //cellAddr[5] = int.Parse(this.cbFromRow.Text.Substring(3, 3));
            //cellAddr[6] = int.Parse(this.cbToColumn.Text);
            //cellAddr[7] = int.Parse(this.cbToHeight.Text);
            //cellAddr[8] = int.Parse(this.cbToRow.Text.Substring(3, 3));

            Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
            Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 0);
        }

        private void btnStop1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要急停1号堆垛机?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                PutCommand("1", 2);
                Logger.Info("1号堆垛机下发急停命令");
            }
        }


        Button GetButton(string CraneNo)
        {
            Control[] ctl = this.Controls.Find("btn" + CraneNo, true);
            if (ctl.Length > 0)
                return (Button)ctl[0];
            else
                return null;
        }



        private void btnClearAlarm_Click(object sender, EventArgs e)
        {
            Context.ProcessDispatcher.WriteToService("CranePLC1", "Reset", 1);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            PutCommand("1", 0);
        }

        private void btnClearAlarm2_Click(object sender, EventArgs e)
        {
            Context.ProcessDispatcher.WriteToService("CranePLC2", "Reset", 1);
        }

        private void btnReset2_Click(object sender, EventArgs e)
        {
            PutCommand("2", 0);
        }

        private void btnBack2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要召回2号堆垛机到初始位置?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                PutCommand("2", 0);
                Logger.Info("2号堆垛机下发召回命令");
            }
        }

        private void btnStop2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要急停2号堆垛机?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                PutCommand("2", 2);
                Logger.Info("2号堆垛机下发急停命令");
            }
        }
    
    }
}
