using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_StatusLog_FF : System.Web.UI.Page {
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
		var d = db.ASPLD.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_keterangan.InnerHtml = d.Keterangan;
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
		var rs = from p in db.ASPLDLog
				 where
					p.IDMaster == myID &&
					p.Tgl.Value.Month == GetBulan &&
					p.Tgl.Value.Year == GetTahun
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
			td.Text = Str.Tgl(r.Tgl);
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.MEPeriodeMin.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.MEPeriodeMinLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.MEPeriodeMax.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.MEPeriodeMaxLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.MERumus.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.MERumusLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI1_PeriodeMin.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI1_PeriodeMinLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI1_PeriodeMax.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI1_PeriodeMaxLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI1_Rumus.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI1_RumusLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI2_PeriodeMin.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI2_PeriodeMinLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI2_PeriodeMax.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI2_PeriodeMaxLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI2_Rumus.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TI2_RumusLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.R1_PeriodeMin.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.R1_PeriodeMinLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.R1_PeriodeMax.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.R1_PeriodeMaxLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.R1_Rumus.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.R1_RumusLama.ToString("N0");
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
}