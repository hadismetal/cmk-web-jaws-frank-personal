using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;

public partial class Page_Account_Packaging : System.Web.UI.Page {
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

		if (!IsPostBack)
		{
			fillTable();
			Response.ContentType = "application/vnd.ms-excel";

			Response.AppendHeader("content-disposition", "attachment; filename=Mutasi_Export_Return.xls");
		}
	}

	//Filter
	//Display Table
	protected void fillTable() {
		//WHERE
		string kw = Request.QueryString["kw"];
		int disable = Convert.ToInt32(Request.QueryString["disable"]);

		string w = "1=1";
		if (disable == 1) w += " AND Draft = False AND Disable = False";
		if (disable == 2) w += " AND Draft = False AND Disable = True";
		if (disable == 3) w += " AND Draft = True";

		//ORDER BY
		string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];

		//SELECT
		var rs = from p in db.Packaging.Where(w).OrderBy(o)
				 where
					(
					p.Nama.Contains(kw) ||
                    p.Kode.Contains(kw) ||
					p.Keterangan.Contains(kw)
					)
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
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Info(r.Kode);
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Info(r.Satuan);
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Barcode(r.Kode);
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(Packaging r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"ProductFile.aspx?id=" + r.ID + "\">";
		x += "      <i class=\"icon-file\"></i> ";
		x += "      Open";
		x += "  </a>";

		if (Role.Operator || Role.Manager) {
			x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"ProductUpdate.aspx?id=" + r.ID + "\">";
			x += "      <i class=\"icon-pencil\"></i> ";
			x += "      Update";
			x += "  </a>";
		}
        if (r.Draft == true)
        {
            x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + r.ID + "')\">";
            x += "      <i class=\"icon-remove\"></i> ";
            x += "      Delete";
            x += "  </a>";
        }

		x += "</div>";

		return x;
	}
	protected string TD_Nama(Packaging r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		return x;
	}
	protected string TD_Info(string value) {
		string x = string.Empty;

		
		x += "<p class=\"text-draft\">" + value + "</p>";
		

		return x;
	}

	protected string TD_Barcode(string code)
	{
		List<byte> bytePackaging = Encoding.ASCII.GetBytes(code).ToList();
		List<byte> codePackaging = new List<byte>();
		int based = 104;
		int index = 1;
		int value = based;
		string start = "Ì";
		foreach (byte p in bytePackaging)
		{
			value += ((Convert.ToInt32(p) - 32) * index);
			codePackaging.Add(p);
			index++;
		}
		int final = value % (based - 1);
		codePackaging.Add(Convert.ToByte(final + 32));
		string stop = "Î";
		string resultCode = Encoding.ASCII.GetString(codePackaging.ToArray());
		string resultPackaging = start + resultCode + stop;
		string x = string.Empty;


		x += "<p class=\"text-draft\" style=\"font-family: 'Libre Barcode 128 Text', cursive;font-size: 45px;\"> " + resultPackaging + "</p>";


		return x;
	}
}