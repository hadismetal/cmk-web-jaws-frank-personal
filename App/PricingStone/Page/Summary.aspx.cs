using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;

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
			fill("1A", "Diamond Reguler");
			fill("1B", "Diamond Rapaport");
			fill("2", "Colour Diamond");
			fill("3", "Precious Stone");
			fill("4", "Semi Precious");
			fill("5", "Pearl");
            fill("Repair", "Diamond Repair");
		}
	}

	//Display-Driver
	protected void fill(string TB, string TB_Nama) {
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

		td = new TableCell();
		td.Text = TD_Invalid(TB);
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

		strSql += " SELECT COUNT(ID) FROM Stone" + TB;
		strSql += " WHERE Disable = 0 AND StatusPricing = " + Status;

		int count = Db.SingleInteger(strSql, Db.Cnn_DB);

		x += "<p class=\"text-lg " + css + "\">";
		x += count > 0 ? Str.Integer(count) : "&nbsp;";
		x += "</p>";

		return x;
	}
	protected string TD_Invalid(string TB) {
		string x = string.Empty;
		string strSql = string.Empty;

		strSql += " SELECT COUNT(Stone.ID)";
		strSql += "		FROM Stone" + TB + " Stone";
		strSql += "		INNER JOIN StonePricing" + TB + " Pricing ON Stone.ID = Pricing.ID";
		strSql += " WHERE";
		strSql += "		Stone.Disable = 0";
		strSql += "		AND Selisih IS NOT NULL";
        if(TB != "1B")
	    	strSql += "		AND Selisih < 0";
        else
            strSql += "		AND Selisih < 0";
		int count = Db.SingleInteger(strSql, Db.Cnn_DB);

		x += "<p class=\"text-lg text-red\">";
		x += count > 0 ? Str.Integer(count) : "&nbsp;";
		x += "</p>";

		return x;
	}
    protected void Sync(object sender, EventArgs e)
    {
        Db.Execute("EXEC msdb.dbo.sp_start_job @job_name = N'MLI-SERVER-Mondial_DB-Mondial_DB-6'", Db.Cnn_DB);
        Thread.Sleep(10000);

        Db.Execute("EXEC msdb.dbo.sp_start_job @job_name = N'MLI-SERVER-Mondial_DB-Mondial_DB-MLI-SERVER-17'", Db.Cnn_DB);
        Db.Execute("EXEC msdb.dbo.sp_start_job @job_name = N'MLI-SERVER-Mondial_DB-Mondial_DB-MLI-SERVER-50'", Db.Cnn_DB);
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&d=1";

        Response.Redirect(href);
    }
    protected void SyncFrank(object sender, EventArgs e)
    {
        Db.Execute("EXEC msdb.dbo.sp_start_job @job_name = N'MLI-SERVER-Mondial_DB-Mondial_DB-6'", Db.Cnn_DB);
        Thread.Sleep(10000);

        Db.Execute("EXEC msdb.dbo.sp_start_job @job_name = N'MLI-SERVER-Mondial_DB-Mondial_DB-MLI-SERVER-50'", Db.Cnn_DB);
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&d=1";

        Response.Redirect(href);
    }
    protected void SyncPalace(object sender, EventArgs e)
    {
        Db.Execute("EXEC msdb.dbo.sp_start_job @job_name = N'MLI-SERVER-Mondial_DB-Mondial_DB-6'", Db.Cnn_DB);
        Thread.Sleep(10000);

        Db.Execute("EXEC msdb.dbo.sp_start_job @job_name = N'MLI-SERVER-Mondial_DB-Mondial_DB-MLI-SERVER-17'", Db.Cnn_DB);
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&d=1";

        Response.Redirect(href);
    }
}