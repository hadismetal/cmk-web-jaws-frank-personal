using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
{
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
	protected void Page_Load(object sender, EventArgs e)
	{
		App.Protect();

		if (!IsPostBack)
		{
			//Summary
			tb1.Visible = false;
			tb2.Visible = false;
			tbs.Visible = false;
			fillBusiness();
			fillInformation();
			fillSettings();
		}
	}

	//Master
	protected void fillBusiness()
	{
		Table tb = tb1;
		fill(tb, "CharProductItem", "Product Item");
		fill(tb, "CharProductCategory", "Product Category");
		fill(tb, "CharProductLevel", "Product Level");
		fill(tb, "CharProductSegmen", "Product Segmen");
		fill(tb, "CharProcessCons", "Construction Process");
		fill(tb, "CharProcessFinishing", "Finishing Process");
		fill(tb, "CharStoneDist", "Stone Distribution");
		fill(tb, "CharGoldModel", "Model (PG)");
		fill(tb, "CharProcessRepair", "Repair Process");
	}
	protected void fillInformation()
	{
		Table tb = tb2;
		fill(tb, "CharDesignCategory", "Design Category");
		fill(tb, "CharDesignConcept", "Design Concept");
		fill(tb, "CharDesignProcess", "Design Process");
		fill(tb, "CharStoneFluor", "Stone Fluorescence");
		fill(tb, "CharStoneSetting", "Stone Setting");
		fill(tb, "CharFrameMaterial", "Frame Material");
		fill(tb, "CharFrameFinishing", "Frame Finishing");
		fill(tb, "CharFrameColor", "Frame Color");
		fill(tb, "CharTargetAge", "Target Age");
		fill(tb, "CharTargetGender", "Target Gender");
		fill(tb, "CharGoldLevel", "Product Level (PG)");
	}

	//Settings
	protected void fillSettings()
	{
		Table tb = tbs;
		fill(tb, "Product Category", OK_ProductCategory);
		fill(tb, "Product Level", OK_ProductLevel);
		fill(tb, "Segmen System", OK_Segmen);
		fill(tb, "Stone Distribution", OK_StoneDist);
	}

	//Display-Driver
	protected void fill(Table tb, string TB, string TB_Nama)
	{
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
	protected void fill(Table tb, string Keterangan, bool ok)
	{
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
	protected string TD_Keterangan(string Keterangan)
	{
		string x = string.Empty;

		x += "<p class=\"hang-20\"><strong>" + Keterangan + "</strong></p>";

		return x;
	}
	protected string TD_Content(string TB, int count)
	{
		string x = string.Empty;

		if (count > 0)
		{
			x += "<p class=\"text-muted\">";

			//OPEN DATABASE
			DataTable rs = Db.Rs("SELECT TOP 3 NamaKode FROM " + TB + " WHERE Disable = 0 ORDER BY SN", Db.Cnn_DB);
			for (int i = 0; i < rs.Rows.Count; i++)
			{
				x += rs.Rows[i][0];
				if (i < rs.Rows.Count - 1) x += ", ";
			}
			if (CountTable(TB) > 3) x += ", ...";

			x += "</p>";
		}

		return x;
	}
	protected string TD_Status(bool ok)
	{
		string x = string.Empty;

		string css = ok ? "icon-ok text-success" : "icon-warning-sign text-red";
		x += "<p><i class=\"icon-large " + css + "\"></i></p>";

		return x;
	}
	protected int CountTable(string TB)
	{
		return Db.SingleInteger("SELECT COUNT(ID) FROM " + TB + " WHERE Disable = 0", Db.Cnn_DB);
	}

	//Settings-Driver
	protected bool OK_ProductCategory
	{
		get
		{
			bool x = true;

			var rs = from p in db.CharProductCategory where !p.Disable select p;
			foreach (var r in rs)
				if (r.KodeProto == null) { x = false; break; }

			return x;
		}
	}
	protected bool OK_ProductLevel
	{
		get
		{
			bool x = true;

			var rs = from p in db.CharProductLevel where !p.Disable select p;
			foreach (var r in rs)
				if (!r.Grup.HasValue) { x = false; break; }

			return x;
		}
	}
	protected bool OK_Segmen
	{
		get
		{
			bool x = true;

			var rs1 = from p in db.CharProductCategory where !p.Disable select p;
			var rs2 = from p in db.CharProductLevel where !p.Disable select p;

			foreach (var r1 in rs1)
			{
				foreach (var r2 in rs2)
				{
					int count = db.SettingsSegmen.Count(p => p.IDCategory == r1.ID && p.IDLevel == r2.ID);
					if (count == 0) { x = false; break; }
				}
			}

			return x;
		}
	}
	protected bool OK_StoneDist
	{
		get
		{
			bool x = true;
			return x;
		}
	}
}