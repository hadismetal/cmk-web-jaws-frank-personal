using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WarningSystem : System.Web.UI.Page
{
    private Map_AC _ac;
    private Map_DB _db;

    private Map_AC ac
    {
        get
        {
            if (_ac == null)
                _ac = new Map_AC();
            return _ac;
        }
    }

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    public override void Dispose()
    {
        if (ac != null)
            ac.Dispose();
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    protected int id { get { return App.QueryInt(this); } } /// id warning

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        if (!IsPostBack)
        {
            FillForm();
        }
    }
    protected void FillForm()
    {
        bool access_all = false;
        bool access_1 = false;
        bool access_2 = false;
        bool access_3 = false;

        if (App.UserNama == "Administrator" && (App.UserID == 1)) { access_all = true; }
        if (App.UserNama == "Andika" && (App.UserID == 253 || App.UserID == 163 || App.UserID == 162)) { access_all = true; }
        //if (App.UserNama == "Setiawan Susilo" && (App.UserID == 160 || App.UserID == 267 || App.UserID == 163)) { access_all = true; }
        if (App.UserNama == "Budhi Hartono" && (App.UserID == 561 || App.UserID == 265 || App.UserID == 160)) { access_all = true; }
        if (App.UserNama == "Mirah Jaya" && (App.UserID == 30)) { access_1 = true; access_3 = true; }
        if (App.UserNama == "Muna" && (App.UserID == 35)) { access_1 = true; access_3 = true; }
        if (App.UserNama == "Evi" && (App.UserID == 29)) { access_1 = true; access_3 = true; }

        div_1.Visible = false;
        div_2.Visible = false;
        div_3.Visible = false;

        if (id == 1 && (access_all || access_1)) { div_1.Visible = true; FillReport_ProductInvalid(); }
        if (id == 2 && (access_all || access_2)) { div_2.Visible = true; FillReport_CustomerMultiple(); }
        if (id == 3 && (access_all || access_3)) { div_3.Visible = true; FillReport_ProductStoneCaratSizeOverlapping(); }

    }

    protected void FillReport_ProductInvalid()
    {
        Table formtable = tb1;

        string username = App.UserNama;
        string prefixbefore = "";
        string prefixafter = "";
        string resultcomplete = "";
        string xresult = "";
        string xlabel = "";

        var d = db.IT_SP_Audit.SingleOrDefault(p => p.ID == id);
        lit_subtitle.Text = d.Nama;
        string query = " exec " + d.StoreProcedure + " '" + username + "' ";

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt == null) { App.TableNoResult(formtable); }
        else if (dt.Rows.Count == 0) { App.TableNoResult(formtable); }
        else
        {
            string result = "";
            int indexColumn = 0;
            int index = 1;
            int indexInfo = 1;
            int lastindexInfo = 1;
            lastindexInfo = tb1.Rows[0].Cells.Count;

            TableRow tr;
            TableCell td;

            // On all tables' rows
            foreach (DataRow dtRow in dt.Rows)
            {

                tr = new TableRow();

                td = new TableCell();
                td.Text = index.ToString();
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                indexColumn = 0;
                indexInfo = 1;
                resultcomplete = "";

                // On all tables' columns
                foreach (DataColumn dc in dt.Columns)
                {
                    prefixafter = dc.ColumnName.Substring(0, 3);
                    if ((prefixafter != prefixbefore) && indexColumn == 0) { prefixbefore = prefixafter; }

                    if ((prefixafter != prefixbefore))
                    {
                        td = new TableCell();
                        //td.Text = indexInfo + " - " + lastindexInfo + " - " + indexColumn + " - " + tb1.Rows[0].Cells.Count + " - " + dt.Columns.Count + " - ";
                        //td.Text += resultcomplete + " - " + prefixafter + " - " + prefixbefore;
                        td.Text = resultcomplete;
                        td.Style.Add("text-align", "left");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        resultcomplete = "";

                        prefixbefore = prefixafter;
                        indexInfo++;
                    }

                    xlabel = dc.ColumnName.Substring(3);
                    result = dtRow[dc] != null ? dtRow[dc].ToString() : "";

                    try
                    {
                        if (Convert.ToInt32(result) == 1) { result = "<span class='glyphicon glyphicon-ok' style='color:green; font-size:12px;' aria-hidden='true'></span>"; }
                        else if (Convert.ToInt32(result) == 0) { result = "<span class='glyphicon glyphicon-remove' style='color:red; font-size:12px;' aria-hidden='true'></span>"; }
                    }
                    catch (Exception ex) { }

                    indexColumn++;

                    xresult = "";
                    xresult = "<div><div style='float:left; width:60px;'>" + xlabel + "</div><div style='float:left;'>&nbsp;:&nbsp;</div><div style='float:left;'>" + result + "</div><div style='clear:both;'></div></div>";
                    
                    //resultcomplete += xlabel + " : " + result + "<br />";
                    resultcomplete += xresult;

                    if (indexInfo == (lastindexInfo + (-1)) && indexColumn == (dt.Columns.Count))
                    {
                        td = new TableCell();
                        //td.Text = indexInfo + " - " + lastindexInfo + " - " + indexColumn + " - " + tb1.Rows[0].Cells.Count + " - " + dt.Columns.Count + " - ";
                        //td.Text += resultcomplete + " - " + prefixafter + " - " + prefixbefore;
                        td.Text = resultcomplete;
                        td.Style.Add("text-align", "left");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        resultcomplete = "";

                        prefixbefore = prefixafter;
                        indexInfo++;
                    }


                    //if ((prefixafter != prefixbefore) || (indexInfo == 5 || indexInfo == 6))
                    //{
                    //    td = new TableCell();
                    //    td.Text = resultcomplete + " - " + prefixbefore + " - " + prefixafter;
                    //    td.Style.Add("text-align", "left");
                    //    td.Style.Add("vertical-align", "top");
                    //    tr.Cells.Add(td);

                    //    resultcomplete = "";

                    //    prefixbefore = prefixafter;
                    //    indexInfo++;
                    //}


                }

                formtable.Rows.Add(tr);
                index++;
            }
        }
    }

    protected void FillReport_CustomerMultiple()
    {
        Table formtable = tb2;

        string username = App.UserNama;

        var d = db.IT_SP_Audit.SingleOrDefault(p => p.ID == id);
        lit_subtitle.Text = d.Nama;
        string query = " exec " + d.StoreProcedure + " '" + username + "' ";

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt == null) { App.TableNoResult(formtable); }
        else if (dt.Rows.Count == 0) { App.TableNoResult(formtable); }
        else
        {
            int index = 1;
            TableRow tr;
            TableCell td;

            // On all tables' rows
            foreach (DataRow dtRow in dt.Rows)
            {
                tr = new TableRow();

                td = new TableCell();
                td.Text = index.ToString();
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                int a = 0;
                string b = "";
                List<int> listMiddle = new List<int>(new int[] { 0,1,2,3,4,5,6,7,8,9,10,13 });
                List<int> listRight = new List<int>(new int[] { });

                foreach (DataColumn dc in dt.Columns)
                {
                    td = new TableCell();
                    td.Text = dtRow[dc].ToString();

                    //// class
                    if (a == 11)
                    {
                        b = dtRow[dc].ToString();
                    }
                    else if (a == 12)
                    {
                        if (dtRow[dc].ToString().Trim() != b.Trim()) { td.Style["color"] = "red"; }
                    }

                    //// alignment
                    if (listMiddle.Contains(a))
                    {
                        td.Style.Add("text-align", "center");
                    }
                    else if (listRight.Contains(a))
                    {
                        td.Style.Add("text-align", "right");
                    }
                    else
                    {
                        td.Style.Add("text-align", "left");
                    }
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    a++;
                }

                formtable.Rows.Add(tr);
                index++;
            }
        }
    }

    protected void FillReport_ProductStoneCaratSizeOverlapping()
    {
        Table formtable = tb3;

        string username = App.UserNama;

        var d = db.IT_SP_Audit.SingleOrDefault(p => p.ID == id);
        lit_subtitle.Text = d.Nama;
        string query = " exec " + d.StoreProcedure + " '" + username + "' ";

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt == null) { App.TableNoResult(formtable); }
        else if (dt.Rows.Count == 0) { App.TableNoResult(formtable); }
        else
        {
            int index = 1;
            TableRow tr;
            TableCell td;

            // On all tables' rows
            foreach (DataRow dtRow in dt.Rows)
            {
                tr = new TableRow();

                td = new TableCell();
                td.Text = index.ToString();
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                int a = 0;
                List<int> listMiddle = new List<int>(new int[] { 3, 2, 10, 6 });
                List<int> listRight = new List<int>(new int[] { 5, 7, 8 });

                foreach (DataColumn dc in dt.Columns)
                {

                    td = new TableCell();
                    td.Text = dtRow[dc].ToString();

                    if (listMiddle.Contains(a))
                    {
                        td.Style.Add("text-align", "center");
                    }
                    else if (listRight.Contains(a))
                    {
                        td.Style.Add("text-align", "right");
                    }
                    else
                    {
                        td.Style.Add("text-align", "left");
                    }
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    a++;
                }

                formtable.Rows.Add(tr);
                index++;
            }
        }
    }

    protected void btn_2_deletecustomer_multiple_nontransaction_Click(object sender, EventArgs e)
    {
        try
        {
            string SPname = "sp_audit_customer_delete_multiple";

            Db.Execute("EXEC " + SPname + " "
                , Db.Cnn_DB);
        }
        catch (Exception ex) { }

        FillReport_CustomerMultiple();
    }
}