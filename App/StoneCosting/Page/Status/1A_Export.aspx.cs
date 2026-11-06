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

            Response.AppendHeader("content-disposition", "attachment; filename=StoneCoasting_DR_Export_Excel.xls");
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

       // string o = order.SelectedValue + " " + orderby.SelectedValue;
        
        //SELECT
        var rs = from p in db.Stone1A
                 where
                     (p.Nama.Contains(Request.QueryString["kw"]) || p.KodeParcel.Contains(Request.QueryString["kw"])) &&
                     !p.Disable
                 select p;

        int Parcel01 = Convert.ToInt32(Request.QueryString["parcel01"]);
        if (Parcel01 > 0) rs = rs.Where(p => p.Parcel01 == Parcel01);
        int Parcel02 = Convert.ToInt32(Request.QueryString["parcel02"]);
        if (Parcel02 > 0) rs = rs.Where(p => p.Parcel02 == Parcel02);
        int Parcel03 = Convert.ToInt32(Request.QueryString["parcel03"]);
        if (Parcel03 > 0) rs = rs.Where(p => p.Parcel03 == Parcel03);
        int Parcel04 = Convert.ToInt32(Request.QueryString["parcel04"]);
        if (Parcel04 > 0) rs = rs.Where(p => p.Parcel04 == Parcel04);
        int Status = Convert.ToInt32(Request.QueryString["status"]);
        if (Status != 3) rs = rs.Where(p => p.StatusCosting == Status);

        rs = rs.OrderBy(o);
        //string order = Request.QueryString["order"];
        //if (order == "1")
        //    rs.OrderBy(p => p.Nama);
        //else if (order == "2")
        //    rs.OrderBy(p => p.KodeParcel);
        //else if (order == "3")
        //    rs.OrderBy(p => p.ID);
        //string orderby = Request.QueryString["orderby"];
        
        

       // rs = rs.Where(w);

        //Response.Redirect("dfsjkfn");
        
        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;
            
            if (r.StoneCosting1A == null)
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
            td.Text = TD_Approval(r.StoneCosting1A);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Harga(r.StoneCosting1A);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }

    protected string TD_Nama(Stone1A r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";
        if (r.KodeParcel != r.Nama)
            x += "<p class=\"text-muted\">" + r.KodeParcel + "</p>";

        return x;
    }
    protected string TD_Approval(StoneCosting1A r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Approval + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.ApprovalTgl) + "</p>";

        return x;
    }
    protected string TD_Harga(StoneCosting1A r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += Str.Number2(r.Harga);
        x += "</p>";

        return x;
    }
}