using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_PG : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
            fillFilter();
            //Generate
            if (App.Refresh(this))
                fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.StockReturnPG
				 where p.Draft == false && p.Approval != null
				 select p;

        if (kw.Text != "")
        {
            rs = rs.Where(p => p.NoDokumen.Contains(kw.Text) || p.StockReturnPG_Product.Any(z => z.StockProductPG.Nomor.Contains(kw.Text)));
        }

        DateTime start;
        DateTime end;

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
        }

        rs = rs.OrderBy(p => p.LocWarehouse.Nama).ThenBy(p => p.ID);

        if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.IDWarehouse) {
				Sub_Header(r);
				prev = r.IDWarehouse;
			}

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nomor(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.SupplierNomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.SupplierNama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockReturnPG_Product.Count().ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.TglJam(r.Tgl);
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Operator(r);
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.NamaKurir;
            tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(StockReturnPG r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.LocWarehouse.Nama + " - " + r.LocWarehouse.CompanyBrand.Nama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(StockReturnPG r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"PG_Detail.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Open";
		x += "</a>";

		return x;
	}
	protected string TD_Nomor(StockReturnPG r) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + r.NoDokumen + "</p>";

		return x;
	}
	protected string TD_Operator(StockReturnPG r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }
}