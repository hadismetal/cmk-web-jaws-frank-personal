using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_Product : System.Web.UI.Page
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
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);

        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.DataVoucher
                 
                 //where p.Draft == false && p.ApprovalTgl == null && p.StatusActive == 1
                 select p;

        string Keyword = kw.Text;
        if (Keyword != "")
            rs = rs.Where(p => p.Nomor.Contains(Keyword) || p.CustomerNama.Contains(Keyword));
        int Jenis = Convert.ToInt32(Request.QueryString["jenis"]);
        if (Jenis > 0)
            rs = rs.Where(p => p.JenisVoucher == Jenis);
        int Status = Convert.ToInt32(Request.QueryString["status"]);
        if (Status > -1)
            rs = rs.Where(p => p.Status == Status);

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        //rs = from p in rs
        //     where !(from z in db.Bundle_ItemMaster
        //             select z.IDForm).Contains(p.ID)
        //     select p;

        //NO-RESULT
        if(rs.Count() == 0)
            App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.CompanyBrand.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DL.JenisVoucher(r.JenisVoucher);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Tgl);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.TglExpired);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.CustomerNama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.Nominal);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            status.Text = Request.QueryString["status"];
            kw.Text = Request.QueryString["kw"];
            ddlJenis.SelectedValue = Request.QueryString["jenis"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;
        href += "&kw=" + kw.Text;
        href += "&jenis=" + ddlJenis.SelectedValue;

        Response.Redirect(href);
    }
}