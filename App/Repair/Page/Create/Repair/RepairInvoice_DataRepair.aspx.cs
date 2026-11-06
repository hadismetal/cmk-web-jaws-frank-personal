using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DocRepair : System.Web.UI.Page
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//Form
			fillForm();

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("RepairInvoice.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID);
		if (d == null) DataError();
		else {
			//Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/RepairInvoice_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

			//Hidden-ID
			//idbrand.Text = d.DocRepair.IDBrand.ToString();

            
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		//Customize-Qstr
		href += "&kw=" + kw.Text;

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
		var rs = from p in db.DocRepair
				 orderby p.Tgl
				 where !p.Draft.Value
                    && p.TipeLokasi == (db.DocRepairResult.SingleOrDefault(z => z.ID == myID).TipeLokasi)
                    && p.IDLokasi == (db.DocRepairResult.SingleOrDefault(z => z.ID == myID).IDLokasi)
                    && (p.Nomor.Contains(kw.Text) || p.CustomerNama.Contains(kw.Text) || p.DataCustomer.Nama.Contains(kw.Text)) && p.StatusRepair == 0
                 orderby p.OperatorTgl descending
				 select p;

		//NO-RESULT
        if (rs.Count() == 0)
            App.TableNoResult(tb);


		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            int Tipe = r.TipeLokasi.Value;
            string TipeLokasi = "";
            if (Tipe == 0)
                TipeLokasi = "Warehouse";
            else if (Tipe == 1)
                TipeLokasi = "Outlet";
            else if (Tipe == 2)
                TipeLokasi = "Exhibition";

            string Lokasi = "";
            if (Tipe == 0)
                Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
            else if (Tipe == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
            else if (Tipe == 2)
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

			td = new TableCell();
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.TglJam(r.OperatorTgl);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (db.DocRepair.Any(u => u.ID == r.ID)) ? (r.IDCustomer != null) ? r.DataCustomer.Nama : "" : "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (db.DocRepair.Any(u => u.ID == r.ID)) ? (r.IDSales != null) ? r.DataSales.Nama : "" : "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TipeLokasi;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Lokasi;
            tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(DocRepair r) {
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
	protected string TD_HyperLink(DocRepair r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" href=\"RepairInvoice_RepairSave.aspx?id=" + myID + "&link=" + r.ID + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
    protected string TD_Nama(DocRepair r)
    {
        string x = string.Empty;

        string Nama = "";

        Nama = r.Nomor;

        x += "<p class=\"text-sm\">" + Nama + "</p>";

        return x;
    }
}