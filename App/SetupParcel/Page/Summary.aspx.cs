using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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

		if (!IsPostBack) {
			//Summary
			fillParcel1();
			fillParcel2();
			fillParcel3();
			fillParcel4();
			fillParcel5();
			fillSize();
			fillShape();
		}
	}

	//Master
	protected void fillParcel1() {
		Table tb = tb1;
		fill(tb1, "Parcel101", "Shape");
		fill(tb1, "Parcel102", "Size");
		fill(tb1, "Parcel103", "Colour");
		fill(tb1, "Parcel104", "Clarity");
		fill(tb1, "Parcel105", "Cutting");
		fill(tb1, "Parcel106", "Brand");
		fill(tb1, "Parcel107", "Category");
	}
	protected void fillParcel2() {
		Table tb = tb2;
		fill(tb2, "Parcel201", "Shape");
		fill(tb2, "Parcel202", "Colour");
		fill(tb2, "Parcel203", "Size");
		fill(tb2, "Parcel204", "Colour Grading");
		fill(tb2, "Parcel205", "Clarity");
		fill(tb2, "Parcel206", "Material");
		fill(tb2, "Parcel207", "Category");
	}
	protected void fillParcel3() {
		Table tb = tb3;
		fill(tb3, "Parcel301", "Item");
        fill(tb3, "Parcel304", "Colour");
        fill(tb3, "Parcel305", "Colour Grading");
        fill(tb3, "Parcel309", "Colour Comment");
        fill(tb3, "Parcel310", "Brilliance");
        fill(tb3, "Parcel306", "Clarity");
        fill(tb3, "Parcel307", "Treatment");
        fill(tb3, "Parcel302", "Shape");
        fill(tb3, "Parcel311", "Proportions");
        fill(tb3, "Parcel303", "Size");		
		fill(tb3, "Parcel308", "Country of Origin");
        fill(tb3, "Parcel312", "Supplier");
    }
	protected void fillParcel4() {
		Table tb = tb4;
		fill(tb4, "Parcel401", "Item");
		fill(tb4, "Parcel402", "Shape");
		fill(tb4, "Parcel403", "Size");
	}
	protected void fillParcel5() {
		Table tb = tb5;
		fill(tb5, "Parcel501", "Code");
		fill(tb5, "Parcel502", "Source");
		fill(tb5, "Parcel503", "Colour");
		fill(tb5, "Parcel504", "Shape");
		fill(tb5, "Parcel505", "Overtone");
		fill(tb5, "Parcel506", "Diameter");
		fill(tb5, "Parcel507", "Surface");
		fill(tb5, "Parcel508", "Luster");
		fill(tb5, "Parcel509", "Treatment");
	}

	//Settings
	protected void fillSize() {
		int p1 = db.Parcel102.Count(p => !p.Disable); int c1 = db.Parcel102.Count(p => !p.SizeMin.HasValue && !p.Disable);
		int p2 = db.Parcel203.Count(p => !p.Disable); int c2 = db.Parcel203.Count(p => !p.SizeMin.HasValue && !p.Disable);
		int p3 = db.Parcel303.Count(p => !p.Disable); int c3 = db.Parcel303.Count(p => !p.SizeMin.HasValue && !p.Disable);
		int p4 = db.Parcel403.Count(p => !p.Disable); int c4 = db.Parcel403.Count(p => !p.SizeMin.HasValue && !p.Disable);

		Table tb = ts1;
		fill(tb, n1, p1 > 0 && c1 == 0);
		fill(tb, n2, p2 > 0 && c2 == 0);
		fill(tb, n3, p3 > 0 && c3 == 0);
		fill(tb, n4, p4 > 0 && c4 == 0);
	}
	protected void fillShape() {
		int p1 = db.Parcel101.Count(p => !p.Disable); int c1 = db.Parcel101.Count(p => !p.RoundShape.HasValue && !p.Disable);
		int p2 = db.Parcel201.Count(p => !p.Disable); int c2 = db.Parcel201.Count(p => !p.RoundShape.HasValue && !p.Disable);
		int p3 = db.Parcel302.Count(p => !p.Disable); int c3 = db.Parcel302.Count(p => !p.RoundShape.HasValue && !p.Disable);
		int p4 = db.Parcel402.Count(p => !p.Disable); int c4 = db.Parcel402.Count(p => !p.RoundShape.HasValue && !p.Disable);
		int p5 = db.Parcel504.Count(p => !p.Disable); int c5 = db.Parcel504.Count(p => !p.RoundShape.HasValue && !p.Disable);

		Table tb = ts2;
		fill(tb, n1, p1 > 0 && c1 == 0);
		fill(tb, n2, p2 > 0 && c2 == 0);
		fill(tb, n3, p3 > 0 && c3 == 0);
		fill(tb, n4, p4 > 0 && c4 == 0);
		fill(tb, n5, p5 > 0 && c5 == 0);
	}

	//Display-Driver
	protected void fill(Table tb, string TB, string TB_Nama) {
		TableRow tr = new TableRow();
		TableCell td = new TableCell();

		int count = CountTable(TB);

		td = new TableCell();
		td.Text = TD_Keterangan(TB_Nama);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Content(TB, count);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Status(count > 0);
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected void fill(Table tb, string Keterangan, bool ok) {
		TableRow tr = new TableRow();
		TableCell td;

		td = new TableCell();
		td.Text = TD_Keterangan(Keterangan);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Status(ok);
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_Keterangan(string Keterangan) {
		string x = string.Empty;

		x += "<p class=\"hang-20\"><strong>" + Keterangan + "</strong></p>";

		return x;
	}
	protected string TD_Content(string TB, int count) {
		string x = string.Empty;

		if (count > 0) {
			x += "<p class=\"text-muted\">";

			//OPEN DATABASE
			DataTable rs = Db.Rs("SELECT TOP 3 NamaKode FROM " + TB + " WHERE Disable = 0 ORDER BY SN", Db.Cnn_DB);
			for (int i = 0; i < rs.Rows.Count; i++) {
				x += rs.Rows[i][0];
				if (i < rs.Rows.Count - 1) x += ", ";
			}
			if (CountTable(TB) > 3) x += ", ...";

			x += "</p>";
		}

		return x;
	}
	protected string TD_Status(bool ok) {
		string x = string.Empty;

		string css = ok ? "icon-ok text-success" : "icon-warning-sign text-red";
		x += "<p><i class=\"icon-large " + css + "\"></i></p>";

		return x;
	}
	protected int CountTable(string TB) {
		return Db.SingleInteger("SELECT COUNT(ID) FROM " + TB + " WHERE Disable = 0", Db.Cnn_DB);
	}

	//Stone-Index
	protected string n1 { get { return tb1.Rows[0].Cells[0].Text; } }
	protected string n2 { get { return tb2.Rows[0].Cells[0].Text; } }
	protected string n3 { get { return tb3.Rows[0].Cells[0].Text; } }
	protected string n4 { get { return tb4.Rows[0].Cells[0].Text; } }
	protected string n5 { get { return tb5.Rows[0].Cells[0].Text; } }
}