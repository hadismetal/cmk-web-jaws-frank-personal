using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_DJ_RekapDetail_Export : System.Web.UI.Page
{
    private Map_DB _db;

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
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DJ_RekapDetailLebur_ExportExcel.xls");
            //Generate
            fillTable(sender, e);
        }
    }

    protected void fillTable(object sender, EventArgs e)
    {
        string start = Request.QueryString["dari"];
        string end = Request.QueryString["sampai"];
        int mode  = Convert.ToInt32(Request.QueryString["status"]);

        string query = "exec sp_rekaplebur '" + start + "','" + end + "','" + mode + "'";

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        int GT_butir = 0;
        decimal GT_M = 0;
        decimal GT_Harga = 0;
        decimal GT_S = 0;
        if (dt == null) { App.TableNoResult(tb); }
        else if (dt.Rows.Count == 0) { App.TableNoResult(tb); }
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

                int a = 1;
                // On all tables' columns
                foreach (DataColumn dc in dt.Columns)
                {
                    if (a == 3)
                    {
                        td = new TableCell();
                        //td.Text = Str.Number3(Convert.ToDecimal(dtRow[dc]));
                        td.Text = Convert.ToDecimal(dtRow[dc]).ToString("N3").Replace(".", "").Replace(",", ".");
                        GT_M += Convert.ToDecimal(dtRow[dc]);
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);
                    }
                    else if (a == 4)
                    {
                        td = new TableCell();
                        //td.Text = Str.Number0(Convert.ToDecimal(dtRow[dc]));
                        td.Text = Convert.ToDecimal(dtRow[dc]).ToString("N0");
                        //GT_Harga += Convert.ToDecimal(dtRow[dc]);
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);
                    }
                    else if (a == 1)
                    {
                        td = new TableCell();
                        td.Text = dtRow[dc].ToString();
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);
                    }
                    else if (a == 2)
                    {
                        td = new TableCell();
                        //td.Text = Str.Number0(Convert.ToDecimal(dtRow[dc]));
                        td.Text = Convert.ToDecimal(dtRow[dc]).ToString("N0");
                        GT_butir += Convert.ToInt32(dtRow[dc]);
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);
                    }
                    else if (a == 5)
                    {
                        td = new TableCell();
                        //td.Text = Str.Number3(Convert.ToDecimal(dtRow[dc]));
                        td.Text = Convert.ToDecimal(dtRow[dc]).ToString("N3").Replace(".", "").Replace(",", ".");
                        GT_S += Convert.ToDecimal(dtRow[dc]);
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);
                    }
                    a++;
                }

                tb.Rows.Add(tr);
                index++;
            }
            Sub_Total(GT_butir, GT_M, GT_Harga, GT_S);
        }

    }

    protected void Sub_Total(int GT_butir, decimal GT_M, decimal GT_Harga, decimal GT_S)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "text-grandtotal";

        td = new TableCell();
        td.ColumnSpan = 2;
        td.Text = "Total";
        td.Style.Add("text-align", "center");
        td.Style.Add("vertical-align", "top");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(GT_butir);
        td.Style.Add("text-align", "center");
        td.Style.Add("vertical-align", "top");
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = Str.Number3(GT_M);
        td.Text = GT_M.ToString("N3").Replace(".", "").Replace(",", ".");
        td.Style.Add("text-align", "center");
        td.Style.Add("vertical-align", "top");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "-";
        td.Style.Add("text-align", "center");
        td.Style.Add("vertical-align", "top");
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = Str.Number3(GT_S);
        td.Text = GT_S.ToString("N3").Replace(".", "").Replace(",", ".");
        td.Style.Add("text-align", "center");
        td.Style.Add("vertical-align", "top");
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
}