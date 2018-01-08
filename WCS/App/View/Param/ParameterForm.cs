using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MCP.Config;
using Util;


namespace App.View.Param
{
    public partial class ParameterForm : View.BaseForm
    {
        private App.View.Param.Parameter parameter = new App.View.Param.Parameter();
        //private DBConfig config = new DBConfig();


        MCP.Service.TCP.Config.Configuration TcpConfg = new MCP.Service.TCP.Config.Configuration("Crane.xml");

        MCP.Service.Siemens.Config.Configuration PLC1 = new MCP.Service.Siemens.Config.Configuration("CranePLC1.xml");
       
        private Dictionary<string, string> attributes = null;

        public ParameterForm()
        {
            InitializeComponent();
            ReadParameter();
        }

        private void ReadParameter()
        {
            //本机数据库连接参数
            //parameter.ServerName = config.Parameters["server"].ToString();
            //parameter.DBUser = config.Parameters["uid"].ToString();
            //parameter.Password = config.Parameters["pwd"].ToString();


            //扫描枪--由于使用USB接口，而屏蔽
            //ConfigUtil configUtil = new ConfigUtil();
            //attributes = configUtil.GetAttribute();          

            //PLC1
            parameter.PLC1ServerName = PLC1.ProgID;
            parameter.PLC1ServerIP = PLC1.ServerName;
            parameter.PLC1GroupString = PLC1.GroupString;
            parameter.PLC1UpdateRate = PLC1.UpdateRate;
            
           
            propertyGrid.SelectedObject = parameter;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //保存本机数据库连接参数
                //config.Parameters["server"] = parameter.ServerName;
                //config.Parameters["uid"] = parameter.DBUser;
                //config.Parameters["pwd"] = parameter.Password;
                //config.Save();


                //由于扫码枪使用USB接口，而屏蔽。
                ////保存Context参数
                //attributes["ScanPortName"] = parameter.ScanPortName;
                //attributes["ScanBaudRate"] = parameter.ScanBaudRate;

                //attributes["IsOutOrder"] = parameter.IsOutOrder;
                //attributes["MesWebServiceUrl"] = parameter.MesWebServiceUrl;
                //ConfigUtil configUtil = new ConfigUtil();
                //configUtil.Save(attributes);

                //PLC1
                PLC1.GroupString = parameter.PLC1GroupString;
                PLC1.ProgID = parameter.PLC1ServerName;
                PLC1.UpdateRate = parameter.PLC1UpdateRate;
                PLC1.ServerName = parameter.PLC1ServerIP;
                PLC1.Save();
                 

                MessageBox.Show("系统参数保存成功，请重新启动本系统。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存系统参数过程中出现异常，原因：" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

