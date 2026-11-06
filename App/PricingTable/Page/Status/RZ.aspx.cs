using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Status_RZ : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&status=" + status.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "";
		if (status.SelectedIndex == 1) 
		{
			w += " AND Approval IS NULL AND ApprovalTgl IS NULL";
		}
		else if(status.SelectedIndex == 2)
		{
			w += " AND Approval IS NOT NULL AND ApprovalTgl IS NOT NULL";
		}

		//SELECT
		DataTable dt = Db.Rs("SELECT * FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0" + w, Db.Cnn_DB);
		
		int rowcount = dt.Rows.Count;
		//RESULT
		if (rowcount == 0) App.TableNoResult(tb);
		else
		{
			//RESULT
			foreach (DataRow r in dt.Rows) {
				if (!Response.IsClientConnected) break;

				TableRow tr = new TableRow();
				TableCell td;

				td = new TableCell();
				td.Text = TD_HyperLink(Convert.ToInt32(r["ID"]));
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_Nama(r["Nama"].ToString(), r["Keterangan"].ToString());
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = TD_Operator((r["OperatorTgl"] == DBNull.Value) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(r["OperatorTgl"]), r["Operator"].ToString());
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_Approval((r["ApprovalTgl"] == DBNull.Value) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(r["ApprovalTgl"]), r["Approval"].ToString());
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_Harga(Convert.ToDecimal(r["Harga"]));
				td.CssClass = "text-right";
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = TD_Status(r["Approval"].ToString());
				tr.Cells.Add(td);

				tb.Rows.Add(tr);
			}
		}
	}
	protected string TD_HyperLink(int id) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../StatusLog/RZ.aspx?id=" + id + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Detail";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(string nama, string keterangan) {
		string x = string.Empty;

		x += "<p><strong>" + nama + "</strong></p>";
		if (keterangan != "")
			x += "<p class=\"text-muted\">" + Str.HtmlBreak(keterangan) + "</p>";

		return x;
	}
	protected string TD_Operator(DateTime? operator_tgl, string operator_nama) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + operator_nama + "</strong></p>";
		if(operator_tgl != Convert.ToDateTime("1900-01-01"))
			x += "<p class=\"text-sm\">" + Str.FullDate(operator_tgl) + "</p>";

		return x;
	}
	protected string TD_Approval(DateTime? approval_tgl, string approval) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + approval + "</strong></p>";
		if(approval_tgl != Convert.ToDateTime("1900-01-01"))
			x += "<p class=\"text-sm\">" + Str.FullDate(approval_tgl) + "</p>";

		return x;
	}
	protected string TD_Harga(decimal harga) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(harga);
		x += "</p>";

		return x;
	}
	protected string TD_Status(string approval) {
		string x = string.Empty;
		
		if(approval == null || approval == "")
		{
			x += "<p class=\"text-danger text-lg\">";
			x += "Draft";
			x += "</p>";
		}
		else
		{
			x += "<p class=\"text-success text-lg\">";
			x += "Approved";
			x += "</p>";
		}

		return x;
	}
}