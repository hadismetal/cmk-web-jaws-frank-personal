using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.SqlServer;

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
			//Setup
			div_setup.Visible = Role.Manager;

			//Summary
			fill1();
			fill2();
			fill3();
            Sync();
		}
	}
	protected void fill1() {
		Table tb = tb1;
		fill(tb, "TG", "Today Gold Price");
		fill(tb, "US", "USD Rate");
	}
	protected void fill2() {
		Table tb = tb2;
		fill(tb, "FM", "Mounting Fee");
		fill(tb, "FS", "Setting Fee");
		fill(tb, "FF", "Finishing Fee");
        fill(tb, "Repair", "Repair Fee");
	}
	protected void fill3() {
		Table tb = tb3;
		fill(tb, "MU", "Mark-Up System");
		fill(tb, "SS", "Segmen System");
	}

	//Display-Driver
	protected void fill(Table tb, string TB, string TB_Nama) {
		TableRow tr = new TableRow();
		TableCell td = new TableCell();

		td = new TableCell();
		td.Text = TD_Keterangan(TB_Nama);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Status(TB, 0);
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Status(TB, 1);
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Status(TB, 2);
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_Keterangan(string Keterangan) {
		string x = string.Empty;

		x += "<p class=\"hang-20\"><strong>" + Keterangan + "</strong></p>";

		return x;
	}
	protected string TD_Status(string TB, int Status) {
		string x = string.Empty;
		string css = string.Empty;
		string strSql = string.Empty;

		switch (Status) {
			case 0: css = "text-success"; break;
			case 1: css = "text-info"; break;
			case 2: css = "text-danger"; break;
		}

		strSql += " SELECT COUNT(ID) FROM PricingTable" + TB;
		strSql += " WHERE StatusPricing = " + Status;

		int count = Db.SingleInteger(strSql, Db.Cnn_DB);

		x += "<p class=\"text-lg " + css + "\">";
		x += count > 0 ? Str.Integer(count) : "&nbsp;";
		x += "</p>";

		return x;
	}
    protected void Sync()
    {
        //Db.Execute("EXEC msdb.dbo.sp_start_job @job_name = N'SUWANDI-PC-Mondial_DB-Mondial_DB-1'", Db.Cnn_DB);
    }
}