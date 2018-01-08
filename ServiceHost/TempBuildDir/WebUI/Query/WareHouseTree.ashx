<%@ WebHandler Language="C#" Class="WareHouseTree" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;
using Util;
using System.Web.Security;


public class WareHouseTree : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        if (context.Session["G_user"] == null)
        {
            context.Response.Write("-1");
            return;
        }
        context.Response.ContentType = "text/plain";
        BLL.BLLBase bll = new BLL.BLLBase();
        DataTable dtWareHouse = bll.FillDataTable("Cmd.SelectWarehouse");
        string json = "[{id:0,text:'仓库资料',leaf:false";

        //仓库
        for (int j = 0; j < dtWareHouse.Rows.Count; j++)
        {
            DataRow dr = dtWareHouse.Rows[j];
            string areatree = GetWareHoseTree(dr["WareHouseCode"].ToString());
            if (j == 0)
            {
                json += ",children:[{";
                json += "id:'" + dr["WareHouseCode"].ToString() + "'";
                json += ",text:'" + dr["WareHouseName"].ToString() + "'";
                if (dtWareHouse.Rows.Count == 1)
                {
                    if (areatree.Length > 0)
                        json += ",leaf:false" + areatree + "}]}";
                    else
                        json += ",leaf:true }]}";
                }
                else
                {
                    if (areatree.Length > 0)
                        json += ",leaf:false" + areatree + "}";
                    else
                        json += ",leaftrue}";
                }
            }

            else if (j > 0 && j < dtWareHouse.Rows.Count - 1)
            {
                json += ",{id:'" + dr["WareHouseCode"].ToString() + "'";
                json += ",text:'" + dr["WareHouseName"].ToString() + "'";

                if (areatree.Length > 0)
                    json += ",leaf:false" + areatree + "}";
                else
                    json += ",leaf:true}";
            }
            else
            {
                json += ",{id:'" + dr["WareHouseCode"].ToString() + "'";
                json += ",text:'" + dr["WareHouseName"].ToString() + "'";

                if (areatree.Length > 0)
                    json += ",leaf:false" + areatree + "}]}";
                else
                    json += ",leaf:true}]}";
            }

        }

        json += "]";
        context.Response.Write(json);
    }
    private string GetWareHoseTree(string WareHouseCode)
    {
        string json = "";
        BLL.BLLBase bll = new BLL.BLLBase();
        DataTable dtArea = bll.FillDataTable("Cmd.SelectArea", new DataParameter[] { new DataParameter("{0}", string.Format("WarehouseCode='{0}'", WareHouseCode)) });
        for (int j = 0; j < dtArea.Rows.Count; j++)
        {
            DataRow dr = dtArea.Rows[j];
            string shelfTree = GetAreaTree(dr["AreaCode"].ToString());

            if (j == 0)
            {
                json += ",children:[{";
                json += "id:'" + dr["AreaCode"].ToString() + "'";
                json += ",text:'" + dr["AreaName"].ToString() + "'";

                if (dtArea.Rows.Count == 1)
                {
                    if (shelfTree.Length > 0)
                        json += ",leaf:false" + shelfTree + "}]";
                    else
                        json += ",leaf:true }]";
                }
                else
                {
                    if (shelfTree.Length > 0)
                        json += ",leaf:false" + shelfTree + "}";
                    else
                        json += ",leaftrue}";
                }
            }
            else if (j > 0 && j < dtArea.Rows.Count - 1)
            {
                json += ",{id:'" + dr["AreaCode"].ToString() + "'";
                json += ",text:'" + dr["AreaName"].ToString() + "'";

                if (shelfTree.Length > 0)
                    json += ",leaf:false" + shelfTree + "}";
                else
                    json += ",leaf:true}";
            }
            else
            {
                json += ",{id:'" + dr["AreaCode"].ToString() + "'";
                json += ",text:'" + dr["AreaName"].ToString() + "'";

                if (shelfTree.Length > 0)
                    json += ",leaf:false" + shelfTree + "}]";
                else
                    json += ",leaf:true}]";
            }
        }
        return json;
    }

    private string GetAreaTree(string AreaCode)
    {
        string json = "";
        BLL.BLLBase bll = new BLL.BLLBase();
        DataTable dtShelf = bll.FillDataTable("Cmd.SelectCellShelf", new DataParameter[] { new DataParameter("{0}", string.Format("AreaCode='{0}'", AreaCode)) });
        for (int j = 0; j < dtShelf.Rows.Count; j++)
        {
            DataRow dr = dtShelf.Rows[j];
            if (j == 0)
            {
                json += ",children:[{";
                json += "id:'" + dr["ShelfCode"].ToString() + "'";
                json += ",text:'" + dr["ShelfName"].ToString() + "'";

                if (dtShelf.Rows.Count == 1)
                {
                    json += ",leaf:true }]";
                }
                else
                {
                    json += ",leaf:true}";
                }
            }
            else if (j > 0 && j < dtShelf.Rows.Count - 1)
            {
                json += ",{id:'" + dr["ShelfCode"].ToString() + "'";
                json += ",text:'" + dr["ShelfName"].ToString() + "'";
                json += ",leaf:true}";
            }
            else
            {
                json += ",{id:'" + dr["ShelfCode"].ToString() + "'";
                json += ",text:'" + dr["ShelfName"].ToString() + "'";
                json += ",leaf:true}]";
            }
        }
        return json;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


}
