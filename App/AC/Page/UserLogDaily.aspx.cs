using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_UserLogDaily : System.Web.UI.Page {
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
	protected DateTime myTgl { get { return App.QueryDate(this); } }
	protected int myAktivitas { get { return App.QueryInt(this, "aktivitas"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			fillForm();

			//Header
			header_tgl.InnerHtml = Str.FullDate(myTgl);
			header_aktivitas.InnerHtml = DL.AccessLog(myAktivitas);

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

		//SELECT
		var rs = from p in db.AccessLog.OrderBy(o)
				 where
					p.UserID == myID &&
					p.Tgl.Date == myTgl &&
					p.Aktivitas == myAktivitas
				 select p;

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
			td.Text = Str.Jam(r.Tgl);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.IPAddr;
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
}