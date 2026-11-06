using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page
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

            Response.AppendHeader("content-disposition", "attachment; filename=ST_PreciousStone_Export_Excel.xls");
            //Generate
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //ORDER BY
        string Order = Request.QueryString["order"];
        string Orderby = Request.QueryString["orderby"];
        string o = Order + " " + Orderby;
  
        //SELECT
        var rs = from p in db.Stone3
                 where
                     (p.Nama.Contains(Request.QueryString["kw"]) || p.KodeParcel.Contains(Request.QueryString["kw"])) &&
                     !p.Disable
                 select p;

       
        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        int Parcel01 = Convert.ToInt32(Request.QueryString["parcel01"]);
        if (Parcel01 > 0) rs = rs.Where(p => p.Parcel01 == Parcel01);
        int Parcel02 = Convert.ToInt32(Request.QueryString["parcel02"]);
        if (Parcel02 > 0) rs = rs.Where(p => p.Parcel02 == Parcel02);
        int Parcel03 = Convert.ToInt32(Request.QueryString["parcel03"]);
        if (Parcel03 > 0) rs = rs.Where(p => p.Parcel03 == Parcel03);
        int Parcel04 = Convert.ToInt32(Request.QueryString["parcel04"]);
        if (Parcel04 > 0) rs = rs.Where(p => p.Parcel04 == Parcel04);
        int Parcel05 = Convert.ToInt32(Request.QueryString["parcel05"]);
        if (Parcel05 > 0) rs = rs.Where(p => p.Parcel05 == Parcel05);
        int Parcel06 = Convert.ToInt32(Request.QueryString["parcel06"]);
        if (Parcel06 > 0) rs = rs.Where(p => p.Parcel06 == Parcel06);
        int Parcel07 = Convert.ToInt32(Request.QueryString["parcel07"]);
        if (Parcel07 > 0) rs = rs.Where(p => p.Parcel07 == Parcel07);
        int Parcel08 = Convert.ToInt32(Request.QueryString["parcel08"]);
        if (Parcel08 > 0) rs = rs.Where(p => p.Parcel08 == Parcel08);
        int Status = Convert.ToInt32(Request.QueryString["status"]);
        if (Status != 3) rs = rs.Where(p => p.StatusCosting == Status);
        //Response.Redirect("njks");
        rs = rs.OrderBy(o);

       
        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;
            
            if (r.StoneCosting3 == null)
            {
                continue;
            }

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Approval(r.StoneCosting3);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Harga(r.StoneCosting3);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }

    protected string TD_Nama(Stone3 r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";
        if (r.KodeParcel != r.Nama)
            x += "<p class=\"text-muted\">" + r.KodeParcel + "</p>";

        return x;
    }
    protected string TD_Approval(StoneCosting3 r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Approval + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.ApprovalTgl) + "</p>";

        return x;
    }
    protected string TD_Harga(StoneCosting3 r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += Str.Number2(r.Harga);
        x += "</p>";

        return x;
    }
}