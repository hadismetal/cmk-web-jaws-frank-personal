using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Configuration;

public partial class PreLEI : System.Web.UI.Page
{
	protected string myBrand = ConfigurationManager.AppSettings["KodeBrand"];

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            if (App.Refresh(this))
			{
				fillTable();
			}
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
		fillTable();
    }
	
	public void TableGagalKirim(Table tb, string message) {
		TableRow tr = new TableRow();
		TableCell td = new TableCell();

		td.Text = message;
		td.ColumnSpan = tb.Rows[0].Cells.Count;
		tr.Cells.Add(td);

		tr.CssClass = "no-result";
		tb.Rows.Add(tr);
	}

    //Display Table
    protected void fillTable()
    {	
		string kW = txt_rts.Text.Trim();
		
		string query = "EXEC sp_get_list_stock_prelei_new '" + kW + "','1'";
		DataTable rs1 = Db.Rs(query, Db.Cnn_DB);
		
		int index = 1;
        
		if (rs1.Rows.Count == 0) { App.TableNoResult(tb); }
		foreach(DataRow rs in rs1.Rows)
		{	
			TableRow tr = new TableRow();
			TableCell td;
			
			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = TD_HyperLink(Convert.ToInt32(rs["ID"]));
			tr.Cells.Add(td);

			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = rs["NO_RTS"].ToString();
			tr.Cells.Add(td);

			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = rs["NAMA_ASAL"].ToString();
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = rs["NAMA_TUJUAN"].ToString();
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = rs["OPERATOR"].ToString();
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = (rs["OPERATOR_TGL"] == DBNull.Value) ? "" : Convert.ToDateTime(rs["OPERATOR_TGL"]).ToString("dd-MM-yyyy");
			tr.Cells.Add(td);
			
			tb.Rows.Add(tr);
		}
    }
	
	protected string TD_HyperLink(int id)
    {
        string x = string.Empty;
		x += "<a class=\"btn btn-info\" target=\"_self\" href=\"PreLEI_Submit.aspx?id=" + id + "\">";
		x += "Detail";
		x += "</a>";
		return x;
	}
}