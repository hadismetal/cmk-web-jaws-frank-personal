using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_StatusLog_RZ : System.Web.UI.Page {
	protected int myID { get { return App.QueryInt(this); } }

	//Parameter Periode
	protected int GetBulan { get { return Convert.ToInt32(bulan.SelectedValue); } }
	protected int GetTahun { get { return Convert.ToInt32(tahun.SelectedValue); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			//DLC
			DateDrv.ListBulan(bulan);
			DateDrv.ListTahun(tahun);

			//Generate
			fillFilter();
			fillTable(sender, e);
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
		//SELECT
		DataTable dt = Db.Rs("SELECT * FROM PricingLogExtraRingSize WHERE IDMaster = " + myID + " AND MONTH(Tgl) = " + GetBulan + " AND YEAR(Tgl) = " + GetTahun + " ", Db.Cnn_DB);
		
		int rowcount = dt.Rows.Count;
		//NO-RESULT
		if (rowcount == 0) App.TableNoResult(tb);
		else 
		{
			//RESULT
			int index = 1;
			foreach (DataRow r in dt.Rows) {
				if (!Response.IsClientConnected) break;

				TableRow tr = new TableRow();
				TableCell td;

				td = new TableCell();
				td.Text = index++.ToString() + ".";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Str.Tgl(Convert.ToDateTime(r["Tgl"]));
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Str.Jam(Convert.ToDateTime(r["Tgl"]));
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = r["Approval"].ToString();
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Str.Number2(Convert.ToDecimal(r["HargaLama"]));
				td.CssClass = "text-right";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Str.Number2(Convert.ToDecimal(r["HargaBaru"]));
				td.CssClass = "text-right";
				tr.Cells.Add(td);

				tb.Rows.Add(tr);
			}
		}
	}
}