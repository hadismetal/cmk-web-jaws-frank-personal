using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_AccessLog : System.Web.UI.Page {
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

	//Index Aktivitas
	protected int aktivitas_min { get { return 0; } }
	protected int aktivitas_max { get { return 3; } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		setCalendar();
		if (!IsPostBack) {
			//Generate
			fillTable(sender, e);
		}
	}

	//Generate
	protected void setCalendar() {
		tgl.SelectedDates.Clear();
		if (!IsPostBack) tgl.SelectedDate = DateTime.Today;
	}
	protected void fillTable(object sender, EventArgs e) {
		for (int i = aktivitas_min; i <= aktivitas_max; i++) {
			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
            td.Text = TD_HyperLink(i);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Nama(i);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Count(i);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(int Aktivitas) {
		string x = string.Empty;
		string href = string.Empty;

		href += "AccessLogDaily.aspx";
		href += "?tgl=" + tgl.SelectedDate;
		href += "&aktivitas=" + Aktivitas;

		x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Detail";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(int Aktivitas) {
		string x = string.Empty;

		x += "<p><strong>" + DL.AccessLog(Aktivitas) + "</strong></p>";

		return x;
	}
	protected string TD_Count(int Aktivitas) {
		string x = string.Empty;

		int count = db.AccessLog.Count(p =>
			p.Tgl.Date == tgl.SelectedDate &&
			p.Aktivitas == Aktivitas
			);

		x += "<p class=\"text-lg\">";
		x += count > 0 ? Str.Integer(count) : "&nbsp;";
		x += "</p>";

		return x;
	}
}