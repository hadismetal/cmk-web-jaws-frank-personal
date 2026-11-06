using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;

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

    //Globalisasi penarikan filter hadis

    public static void Filter(DropDownList ddl, string tipe){
        string query = "select ID,Nama + ' ('+ NamaKode+')' as Nama from " + tipe + " where Disable = 0 ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
        
    public static void Supplier(DropDownList ddl){
        string query = "select ID,Nama from DataSupplier where Draft=0 and Disable = 0 ORDER BY Nama asc";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    public static void TipeLokasi(DropDownList ddl){
        string query = @"select * from(
                         SELECT 'All' AS Nama, '-1' AS ID
                        UNION 
                        SELECT 'Warehouse' AS Nama,'0' AS ID
                        UNION 
                        SELECT 'Outlet' AS Nama,'1' AS ID
                        UNION 
                        SELECT 'Exhibition' AS Nama,'2' AS ID
                        ) AX
                        ORDER BY AX.ID ASC";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    //Globalisasi penarikan filter hadis

    public static void LocationList(DropDownList ddl, int tipelokasi){
        string query="";
        if (tipelokasi == 0){
            query = "select ID,Nama from LocWarehouse where Disable = 0  ";
        }else if (tipelokasi == 1){
            query = "select ID,Nama from LocOutlet where Disable = 0  ";
        }else if (tipelokasi == 2){
            query = "select ID,Nama from LocExhibition where Disable = 0  ";
        }

        if (tipelokasi != -1)
        {
            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            foreach (System.Data.DataRow item in dt.Rows)
            {
                string t = item["Nama"].ToString();
                string v = item["ID"].ToString();
                ddl.Items.Add(new ListItem(t, v));
            }   
        }else{
                ddl.Items.Add(new ListItem("ALL", "-1"));
        }
    }
}