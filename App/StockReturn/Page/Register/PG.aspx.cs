using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Register_PG : System.Web.UI.Page {
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
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//DLC
			Ddl.Filter(brand,"CompanyBrand");

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&kw=" + kw.Text;
		href += "&brand=" + brand.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			brand.SelectedIndex = Convert.ToInt32(Request.QueryString["brand"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		Response.Write(brand.SelectedValue);
		// int IDBrand = -1;
		string IDBrand = Convert.ToString(brand.SelectedValue) == "Semua" ? "0" : Convert.ToString(brand.SelectedValue);

		//SELECT
		string query=@"SELECT a.ID, a.Nama, a.AddrAlamat, a.IDBrand, a.IDGroup, b.Nama AS NamaBrand, b.NamaKode AS kodeBrand,
		c.Namakode as KodeGroupBrand
		FROM LocWarehouse a
		JOIN CompanyBrand b ON a.IDBrand = b.ID
		JOIN LocWarehouseGroup c ON a.IDGroup = c.ID
		WHERE a.Disable=0 and (@IDBrand = 0 or a.IDBrand = @IDBrand)
		and (@kw = '' or (a.Nama like '%' + @kw + '%' or a.AddrAlamat like '%' + @kw + '%')) 
		ORDER BY b.SN, a.Nama";
		using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
			using (SqlCommand command = new SqlCommand(query, connection))
            {
				command.CommandTimeout = 1;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text.Trim() });
                command.Parameters.Add(new SqlParameter("@IDBrand", SqlDbType.VarChar) { Value = IDBrand });

				SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

				DataTable dt = ds.Tables[0];

				//NO-RESULT
				if (dt.Rows.Count == 0) App.TableNoResult(tb);

				int prev = 0;
				int index = 1;
				foreach (DataRow r in dt.Rows) {
					if (!Response.IsClientConnected) break;

					//Sub-Header
					if (prev != Convert.ToInt32(r["IDBrand"])) {
						Sub_Header(r["NamaBrand"].ToString(), r["kodeBrand"].ToString());
						prev = Convert.ToInt32(r["IDBrand"]);
					}

					TableRow tr = new TableRow();
					TableCell td;

					td = new TableCell();
					td.Text = TD_HyperLink(Convert.ToInt32(r["ID"]));
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = "<p>" + index++.ToString() + ".</p>";
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = TD_Nama(r["Nama"].ToString());
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = TD_Addr(r["AddrAlamat"].ToString());
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = TD_Group(r["KodeGroupBrand"].ToString());
					td.CssClass = "text-center";
					tr.Cells.Add(td);

					tb.Rows.Add(tr);
				}
			}
		}


		// var rs = from p in db.LocWarehouse.Where(w)
		// 		 orderby p.CompanyBrand.SN, p.Nama
		// 		 where
		// 			!p.Disable &&
		// 			(p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
		// 		 select p;
		
	}
	
	// protected void fillTable(object sender, EventArgs e) {
	// 	//WHERE
	// 	string w = "1=1";
	// 	if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;

	// 	//SELECT
	// 	var rs = from p in db.LocWarehouse.Where(w)
	// 			 orderby p.CompanyBrand.SN, p.Nama
	// 			 where
	// 				!p.Disable &&
	// 				(p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
	// 			 select p;

	// 	//NO-RESULT
	// 	if (rs.Count() == 0) App.TableNoResult(tb);

	// 	//RESULT
	// 	int prev = 0;
	// 	int index = 1;
	// 	foreach (var r in rs) {
	// 		if (!Response.IsClientConnected) break;

	// 		//Sub-Header
	// 		if (prev != r.IDBrand) {
	// 			Sub_Header(r);
	// 			prev = r.IDBrand;
	// 		}

	// 		TableRow tr = new TableRow();
	// 		TableCell td;

	// 		td = new TableCell();
	// 		td.Text = TD_HyperLink(r);
	// 		tr.Cells.Add(td);

	// 		td = new TableCell();
	// 		td.Text = "<p>" + index++.ToString() + ".</p>";
	// 		tr.Cells.Add(td);

	// 		td = new TableCell();
	// 		td.Text = TD_Nama(r);
	// 		tr.Cells.Add(td);

	// 		td = new TableCell();
	// 		td.Text = TD_Addr(r);
	// 		tr.Cells.Add(td);

	// 		td = new TableCell();
	// 		td.Text = TD_Group(r);
	// 		td.CssClass = "text-center";
	// 		tr.Cells.Add(td);

	// 		tb.Rows.Add(tr);
	// 	}
	// }
	
	protected void Sub_Header(string companyNama,string CompanyCode) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += companyNama + " (" + CompanyCode + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(int ID) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" href=\"PG_Form.aspx?idwarehouse=" + ID + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(string NamaLokasi) {
		string x = string.Empty;

		x += "<p><strong>" + NamaLokasi + "</strong></p>";

		return x;
	}
	protected string TD_Addr(string AddrAlamat) {
		string x = string.Empty;

		if (AddrAlamat != "" && AddrAlamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(AddrAlamat) + "</p>";

		return x;
	}
	protected string TD_Group(string kodeGroupBrand) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + kodeGroupBrand + "</p>";

		return x;
	}
}