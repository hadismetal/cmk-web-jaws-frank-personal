using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Globalization;

public partial class PreLEI : System.Web.UI.Page
{
	protected string myBrand = "F";

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
			dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
			ddl_status.SelectedValue = "2";
            if (App.Refresh(this))
			{
				fillFilter();
				fillTable();
			}
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
		string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + txt_rts.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + ddl_status.SelectedValue;

        Response.Redirect(href);
    }
	
    protected void fillFilter()
    {
		txt_rts.Text = Request.QueryString["kw"];
		dari.Text = Request.QueryString["dari"];
		sampai.Text = Request.QueryString["sampai"];
		ddl_status.SelectedValue = Request.QueryString["status"];
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
        //GET DDL BRAND FROM-TO TYPE VALUE
		string kW = txt_rts.Text.Trim();
		string[] start = dari.Text.Split('-');
        string[] end = sampai.Text.Split('-');

        string startdate = start[2] + "-" + start[1] + "-" + start[0];
        string enddate = end[2] + "-" + end[1] + "-" + end[0];
		string status = ddl_status.SelectedValue;
        
		string query = "EXEC sp_report_prelei_stock_receive_new '" + kW + "', '" + startdate + "','" + enddate + "', " + status + ", 1, 20, 1";
		DataTable rs1 = Db.Rs(query, Db.Cnn_DB);
		
		int index = 1;
		if (rs1.Rows.Count == 0) { App.TableNoResult(tb); }
		else
		{
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
				td.Text = rs["NO_RTS"].ToString();
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = (rs["TGL_RTS"] == DBNull.Value) ? "" : Convert.ToDateTime(rs["TGL_RTS"]).ToString("dd-MM-yyyy");
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
				td.Text = (rs["TGL"] == DBNull.Value) ? "" : Convert.ToDateTime(rs["TGL"]).ToString("dd-MM-yyyy");
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = rs["TIPE_PRODUCT"].ToString();
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = rs["PLU"].ToString();
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = rs["ITEM"].ToString();
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = rs["LEVEL"].ToString();
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = rs["FRAME_COLOR"].ToString();
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = Convert.ToDouble(rs["KADAR_LOGAM"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = Convert.ToDouble(rs["WEIGHT_ORIGINAL"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = Convert.ToDouble(rs["WEIGHT_AFTER_QC"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = rs["OPERATOR_NAMA"].ToString();
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = (rs["OPERATOR_TGL"] == DBNull.Value) ? "" : Convert.ToDateTime(rs["OPERATOR_TGL"]).ToString("dd-MM-yyyy hh:mm:ss");
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = rs["APPROVED_NAMA"].ToString();
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = (rs["APPROVED_TGL"] == DBNull.Value) ? "" : Convert.ToDateTime(rs["APPROVED_TGL"]).ToString("dd-MM-yyyy hh:mm:ss");
				tr.Cells.Add(td);
				
				int statushit = Convert.ToInt32(rs["STATUS_HIT"]);
				td = new TableCell();
				td.Style.Add("text-align","center");
				td.Text = (statushit == 0) ? "" : rs["RESPONSE"].ToString();
				tr.Cells.Add(td);
				
				tb.Rows.Add(tr);
			}
		}
    }
	
	//Filter
    protected void save_Click(object sender, EventArgs e)
    {
		//Customize-Qstr
		string href = "PreLEI.aspx";

		Response.Redirect(href);
    }
}