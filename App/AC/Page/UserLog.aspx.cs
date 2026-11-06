using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_UserLog : System.Web.UI.Page {
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
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

	//Index Aktivitas
	protected int aktivitas_min { get { return 0; } }
	protected int aktivitas_max { get { return 3; } }

	//Parameter Periode
	protected int GetBulan { get { return Convert.ToInt32(bulan.SelectedValue); } }
	protected int GetTahun { get { return Convert.ToInt32(tahun.SelectedValue); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			fillForm();

			//DLC
			DateDrv.ListBulan(bulan);
			DateDrv.ListTahun(tahun);

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}
	protected void fillForm() {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		//Customize-Qstr
		href += "&bulan=" + bulan.SelectedIndex;
		href += "&tahun=" + tahun.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			bulan.SelectedIndex = Convert.ToInt32(Request.QueryString["bulan"]);
			tahun.SelectedIndex = Convert.ToInt32(Request.QueryString["tahun"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		fillTH(); //Generate Header

		int count = db.AccessLog.Count(p =>
			p.UserID == myID &&
			p.Tgl.Month == GetBulan &&
			p.Tgl.Year == GetTahun
			);

		for (int i = 1; i <= DateTime.DaysInMonth(GetTahun, GetBulan); i++) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			DateTime d = new DateTime(GetTahun, GetBulan, i);

			td = new TableCell();
			td.Text = i.ToString();
			td.CssClass = DateDrv.isWeekend(d) ? "text-red" : "";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.NamaHari(d);
			td.CssClass = DateDrv.isWeekend(d) ? "text-red" : "";
			tr.Cells.Add(td);

			//Kolom Aktivitas
			fillAktivitas(tr, d, count > 0);

			tb.Rows.Add(tr);
		}
	}
	protected void fillTH() {
		for (int i = aktivitas_min; i <= aktivitas_max; i++) {
			TableRow tr = tb.Rows[0];
			TableHeaderCell th = new TableHeaderCell();

			th.Text = DL.AccessLog(i).Replace(" ", "<br />");
			th.CssClass = "text-center";
			th.Width = 150;
			tr.Cells.Add(th);
		}
	}
	protected void fillAktivitas(TableRow tr, DateTime d, bool HasData) {
		for (int i = aktivitas_min; i <= aktivitas_max; i++) {
			TableCell td = new TableCell();
			td.Text = HasData ? TD_Count(d, i) : "";
			td.CssClass = "text-center";
			tr.Cells.Add(td);
		}
	}
	protected string TD_Count(DateTime Tgl, int Aktivitas) {
		string x = string.Empty;

		int count = db.AccessLog.Count(p =>
			p.UserID == myID &&
			p.Tgl.Date == Tgl &&
			p.Aktivitas == Aktivitas
			);

		if (count > 0) {
			string href = string.Empty;

			href += "UserLogDaily.aspx";
			href += "?id=" + myID;
			href += "&tgl=" + Tgl;
			href += "&aktivitas=" + Aktivitas;

			x += "<a style=\"display:block; width:150px;\" href=\"" + href + "\">";
			x += Str.Integer(count);
			x += "</a>";
		}

		return x;
	}
}