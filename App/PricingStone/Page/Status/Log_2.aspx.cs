using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Status_Log_2 : System.Web.UI.Page {
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

	//Parameter Periode
	//protected int GetBulan { get { return Convert.ToInt32(bulan.SelectedValue); } }
	protected int GetTahun { get { return Convert.ToInt32(tahun.SelectedValue); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			fillForm();

			//DLC
			//DateDrv.ListBulan(bulan);
			DateDrv.ListTahun(tahun);

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}
	protected void fillForm() {
		var d = db.StonePricing2.SingleOrDefault(p => p.ID == myID && !p.Stone2.Disable);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nama.InnerHtml = d.Stone2.Nama;
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		//Customize-Qstr
		//href += "&bulan=" + bulan.SelectedIndex;
		href += "&tahun=" + tahun.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			//bulan.SelectedIndex = Convert.ToInt32(Request.QueryString["bulan"]);
			tahun.SelectedIndex = Convert.ToInt32(Request.QueryString["tahun"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.StonePricingLog2
                 where
                    p.IDMaster == myID
                     //&& p.Tgl.Month == GetBulan
                    && p.Tgl.Year == GetTahun
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index++.ToString() + ".";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Tgl(r.Tgl);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Jam(r.Tgl);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Approval;
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaLama);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaBaru);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
}