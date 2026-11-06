using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Ddl
/// </summary>
public class Ddl
{
    public static void Substorage(DropDownList ddl)
    {
        string query = "SELECT ID, Nama FROM SubStorage";
        System.Data.DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Add(new ListItem("All", "0"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
}