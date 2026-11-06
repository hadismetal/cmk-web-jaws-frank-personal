using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page {
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
    App price = new App();

    decimal Setting = 0m;

    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            
		}
        if (App.Refresh(this)) fillTable(sender, e);
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.StockAuditDJ where p.StatusFinish == true
                 
                 select p;

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.OperatorTgl >= start && p.OperatorTgl < end.AddDays(1));
        }

        if (rs != null)
        {
            int index = 1;

            foreach (var r in rs)
            {
                

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ID.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.TipeLokasi);
                tr.Cells.Add(td);

                string NamaLokasi = "";
                if (Convert.ToInt32(r.IDLokasi) == 0)
                    NamaLokasi = "ALL";
                else
                {
                    if (r.TipeLokasi == 0)
                    {
                        var w = db.LocWarehouse.SingleOrDefault(p => p.ID == r.IDLokasi);
                        NamaLokasi = w.Nama;
                    }
                    if (r.TipeLokasi == 1)
                    {
                        var o = db.LocOutlet.SingleOrDefault(p => p.ID == r.IDLokasi);
                        NamaLokasi = o.Nama;
                    }
                    if (r.TipeLokasi == 2)
                    {
                        var exh = db.LocExhibition.SingleOrDefault(p => p.ID == r.IDLokasi);
                        NamaLokasi = exh.Nama;
                    }
                }

                td = new TableCell();
                td.Text = NamaLokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.FinishJam);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.TotalItemAudit);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.TotalItemActual);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;

            }

            
        }


    }
    protected string TD_HyperLink(StockAuditDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"StockAuditDJ_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}