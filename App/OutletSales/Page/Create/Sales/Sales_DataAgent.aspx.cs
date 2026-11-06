using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DataAgent : System.Web.UI.Page
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

	// carries DataSalesGroup/CompanyBrand alongside the row so TD_* helpers
	// don't lazy-load those nav properties per row (was a query per row per column)
	// (protected, not private: used as a parameter type by protected methods below)
	protected class DataSalesRow
	{
	    public DataSales Entity { get; set; }
	    public string GroupNamaKode { get; set; }
	    public string GroupNama { get; set; }
	    public string BrandNama { get; set; }
	}

	protected int myID { get { return App.QueryInt(this); } }
    protected int myTipe { get { return App.QueryInt(this,"tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

            //DLC.CompanyBrand(brand);

			//Form
			fillForm();

			//Generate
			fillFilter();
			//if (App.Refresh(this)) 
		}
        fillTable(sender, e);
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {

            cancel.HRef += "?id=" + d.ID.ToString();
            cancel.HRef += "&status=1";


			//Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Sales_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

			//Hidden-ID
			//idbrand.Text = d.DataSales.IDBrand.ToString();
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		//Customize-Qstr
		href += "&kw=" + kw.Text;
        href += "&tipe=" + myTipe;
        //href += "&brand=" + brand.SelectedValue;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
            //brand.SelectedValue = Request.QueryString["brand"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        int IDBrand = 1;
        if(d.TipeLokasi == 1)
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

        if(d.TipeLokasi == 2)
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
		
		string NamaLokasiSO = "";
        if (d.TipeLokasi == 1)
            NamaLokasiSO = Db.SingleString("SELECT Kode FROM LocOutlet WHERE ID =" + d.IDLokasi, Db.Cnn_DB);
        else if (d.TipeLokasi == 2)
            NamaLokasiSO = Db.SingleString("SELECT Kode FROM LocExhibition WHERE ID =" + d.IDLokasi, Db.Cnn_DB);

        // get all data sales agent
        // project DataSalesGroup/CompanyBrand into the query itself so LINQ-to-SQL
        // resolves them via JOIN in the same round-trip, instead of lazy-loading
        // them per row later in TD_Addr/TD_Group (was N extra queries per row)
		var rsQuery = from p in db.DataSales
				 orderby p.Nama
				 where
					!p.Disable && p.IDBrand == IDBrand &&
					(p.Nama.Contains(kw.Text)) &&
					p.DataSalesGroup.NamaKode.Equals(NamaLokasiSO)
				 select new DataSalesRow
				 {
				     Entity = p,
				     GroupNamaKode = p.DataSalesGroup.NamaKode,
				     GroupNama = p.DataSalesGroup.Nama,
				     BrandNama = p.CompanyBrand.Nama
				 };

        // get user id login AE
        int UserID = App.UserID;
        var asl = from p in db.DataAdminSales
                  where p.IDUser == UserID
                  select p;
        if (asl.Count() > 0)
        {
            var x = asl.First();

            string NamaLokasi = "";
            if(x.TipeLokasi == 1)
                NamaLokasi = Db.SingleString("SELECT Kode FROM LocOutlet WHERE ID =" + x.IDLokasi,Db.Cnn_DB);
            else if(x.TipeLokasi == 2)
                NamaLokasi = Db.SingleString("SELECT Kode FROM LocExhibition WHERE ID =" + x.IDLokasi,Db.Cnn_DB);

            // add filter to data sales with same location name
            if(NamaLokasi!="")
                rsQuery = rsQuery.Where(p => p.GroupNamaKode.Equals(NamaLokasi));
        }

        //int Brand = Convert.ToInt32(brand.SelectedValue);
        //if (Brand > 0)
        //    rs = rs.Where(p => p.IDBrand == Brand);

        // materialize once - previously the same query was executed twice
        // (once for Count() below, once for the foreach)
        var rs = rsQuery.ToList();

		//NO-RESULT
		if (rs.Count == 0) App.TableNoResult(tb);

		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.Entity.IDBrand) {
				Sub_Header(r);
				prev = r.Entity.IDBrand;
			}

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r);
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Addr(r);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Group(r);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(DataSalesRow r) {
		/*TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

        string Username = Db.SingleString("SELECT TOP 1 Nama FROM UserAccount WHERE ID = " + r.IDUser, Db.Cnn_AC);

		x += "<br />";
		x += r.Nama + " (" + Username + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);*/
	}
	protected string TD_HyperLink(DataSalesRow r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" href=\"Sales_DataAgentSave.aspx?id=" + myID + "&link=" + r.Entity.ID + "&tipe=" + myTipe +"\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  ";
		x += "</a>";

		return x;
	}
    protected string TD_Nama(DataSalesRow r)
    {
        string x = string.Empty;

        string Nama = "";
        string styleFontColor = "";
        string nikinfo = "";

        Nama = Str.CapUpper(r.Entity.Nama);

        if (r.Entity.Keterangan.Trim() != "")
        {
            nikinfo = "<p class=\"text-muted\">" + r.Entity.Keterangan.Trim() + "</p>";
        }
        else
        {
            styleFontColor = "style='color:red;'";
        }

        x += "<p><strong " + styleFontColor + ">" + Nama + "</strong></p>";
        x += nikinfo;

        return x;
    }
    protected string TD_Addr(DataSalesRow r)
    {
        string x = string.Empty;

        string Location = "";
        string Location2 = "";

        Location = r.GroupNamaKode;
        Location2 = r.GroupNama;

        x += "<p><strong>" + Location + "</strong></p>";
        x += "<p class=\"text-muted\">" + Location2 + "</p>";

        return x;
    }
	protected string TD_Group(DataSalesRow r) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + r.BrandNama + "</p>";

		return x;
	}
}