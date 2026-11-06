using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_Detail_Certificate_Export : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        //App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Detail_Certificate_ExportExcel.xls");
            //Generate
            fillTable(sender, e);
        }

    }

    protected void fillTable(object sender, EventArgs e)
    {
        var res = from p in db.PrintCertificate_Detail where p.IDForm == myID select p;

        int idx = 1;

        if (res.Count() == 0) App.TableNoResult(tb);

        foreach (var r in res)
        {
            if (!Response.IsClientConnected) break;


            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = r.PLU.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Item.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.URL.ToString();
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

    }
}