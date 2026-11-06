using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_AccessLogDaily : System.Web.UI.Page {
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
	protected DateTime myTgl { get { return App.QueryDate(this); } }
	protected int myAktivitas { get { return App.QueryInt(this, "aktivitas"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			//Header
			header_tgl.InnerHtml = Str.FullDate(myTgl);
			header_aktivitas.InnerHtml = DL.AccessLog(myAktivitas);

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&tgl=" + myTgl;
		href += "&aktivitas=" + myAktivitas;

		//Customize-Qstr
		href += "&order=" + order.SelectedIndex;
		href += "&orderby=" + orderby.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
			orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//ORDER BY
		string o = order.SelectedValue + " " + orderby.SelectedValue;
		if (order.SelectedIndex == 1)
			o = orderby.SelectedIndex == 0 ? "UserNama, UserID" : "UserNama DESC, UserID DESC";

		//SELECT
		var rs = from p in db.AccessLog.OrderBy(o)
                 join q in db.UserAccount on p.UserID equals q.ID
				 where
					p.Tgl.Date == myTgl &&
					p.Aktivitas == myAktivitas
				 select new { p, q };

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
			td.Text = Str.Jam(r.p.Tgl);
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.UserNumber != null ? r.q.UserNumber : "";
            tr.Cells.Add(td);

            td = new TableCell();
			td.Text = r.p.UserNama;
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Keterangan;
            tr.Cells.Add(td);

            td = new TableCell();
			td.Text = Str.ID_Account(r.p.UserID);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.p.IPAddr;
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
}