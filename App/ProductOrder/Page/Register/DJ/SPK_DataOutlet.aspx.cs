using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Design_DataOutlet : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this,"tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//Form
			fillForm();

			//Generate
			fillFilter();
            if (App.Refresh(this))
            {
                fillTable(sender, e);
                fillTable2(sender, e);
            }
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID;

			//Hidden-ID
			//idbrand.Text = d.DataHuman.IDBrand.ToString();
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		//Customize-Qstr
		href += "&kw=" + kw.Text;
        href += "&tipe=" + tipe;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
        var idbrand = from p in db.CompanyBrand where !p.Disable select p.ID;
		var rs = from p in db.LocOutlet
				 orderby p.CompanyBrand.SN, p.Nama
				 where !p.Disable 
                    && idbrand.Contains(p.IDBrand)
                    && (p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.IDBrand) {
				Sub_Header(r.CompanyBrand.Nama, r.CompanyBrand.NamaKode,1);
				prev = r.IDBrand;
			}

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r.ID,1);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r.Nama);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Addr(r.AddrAlamat);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Group(r.LocOutletGroup.NamaKode);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}

    protected void fillTable2(object sender, EventArgs e)
    {
        //SELECT
        var idbrand = from p in db.CompanyBrand where !p.Disable select p.ID;
        var rs = from p in db.LocExhibition
                 orderby p.CompanyBrand.SN, p.Nama
                 where !p.Disable
                    && idbrand.Contains(p.IDBrand)
                    && (p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
                 select p;

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            //Sub-Header
            if (prev != r.IDBrand)
            {
                Sub_Header(r.CompanyBrand.Nama, r.CompanyBrand.NamaKode,2);
                prev = r.IDBrand;
            }

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r.ID,2);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Addr(r.AddrAlamat);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Group(r.LocExhibitionGroup.NamaKode);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }
   

    protected void Sub_Header(string Nama, string Kode, int mode)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;
        if (mode == 1)
        {
            x += "Outlet";
            x += "<br />";
        }
        else if (mode == 2)
        {
            x += "Exhibition";
            x += "<br />";
        }
        x += Nama + " (" + Kode + ")";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr); 
    }
   

    protected string TD_HyperLink(int id, int mode)
    {
        string x = string.Empty;
        
        x += "<a class=\"btn btn-info\" href=\"SPK_DataOutletSave.aspx?id=" + myID + "&link=" + id+ "&tipe=" + tipe +"&loctip="+mode + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(string nama) {
		string x = string.Empty;

		x += "<p><strong>" + nama + "</strong></p>";

		return x;
	}

    protected string TD_Addr(string alamat) {
		string x = string.Empty;

		if (alamat != "" && alamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(alamat) + "</p>";

		return x;
	}

    protected string TD_Group(string kode) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + kode + "</p>";

		return x;
	}
}