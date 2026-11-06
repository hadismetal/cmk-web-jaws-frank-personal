using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Bank_Report_EDC_Export : System.Web.UI.Page
{
    protected string apcode { get { return Request.QueryString["apcode"] ; } }
    protected string noso { get { return Request.QueryString["noso"]; } }
    protected string status { get { return Request.QueryString["status"]; } }
    protected string tglupload { get { return Request.QueryString["tgl"]; } }
    protected string bank { get { return Request.QueryString["bank"]; } }
	protected string store { get { return Request.QueryString["store"]; } }
	protected string tglsostart {get { return Request.QueryString["tglsostart"]; } }
	protected string tglsoend {get { return Request.QueryString["tglsoend"]; } }
	
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Report_EDC.xls");

            if (App.Refresh(this)) 
				fillTable(sender, e);
        }
    }
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string query = "sp_list_approvalcode_cc";
        DataTable dt = new DataTable();
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
                new SqlParameter("@SONumber", SqlDbType.NVarChar) { Value = noso },
                new SqlParameter("@AppCode", SqlDbType.NVarChar) { Value = apcode },
                new SqlParameter("@Tgl", SqlDbType.NVarChar) { Value = tglupload },
                new SqlParameter("@IDBank", SqlDbType.Int) { Value = bank },
                new SqlParameter("@Status", SqlDbType.Int) { Value = status },
                new SqlParameter("@TglSOStart", SqlDbType.NVarChar) { Value = tglsostart },
                new SqlParameter("@TglSOEnd", SqlDbType.NVarChar) { Value = tglsoend },
                new SqlParameter("@KodeLokasi", SqlDbType.NVarChar) { Value = store },
                new SqlParameter("@TOEXCEL", SqlDbType.Int) { Value = 1 }
        };
        dt = Rs(query, Db.Cnn_DB, parameters);
		
        int i = 1;
		decimal nominalcair = 0m;
		decimal nominalso = 0m;
		decimal nominalmdr = 0m;
		decimal nominalgesek = 0m;

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow x in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = i.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["Nomor"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["Lokasi"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DateTime.Parse(x["TglSO"].ToString()).ToShortDateString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["ApprovalCode"].ToString();
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Convert.ToDecimal(x["NominalSO"]).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["EDC"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(x["NominalGesek"]).ToString("N2", nfi);
                td.Visible = false;
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = (Convert.ToDecimal(x["NominalCair"]) + Convert.ToDecimal(x["NominalMDR"])).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(x["NominalCair"]).ToString("N2", nfi);
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Convert.ToDecimal(x["NominalMDR"]).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["Status"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["CCNumber"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["CCName"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["Customer"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["Operator"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                nominalcair += Convert.ToDecimal(x["NominalCair"]);
				nominalso += Convert.ToDecimal(x["NominalSO"]);
				nominalmdr += Convert.ToDecimal(x["NominalMDR"]);
				nominalgesek += Convert.ToDecimal(x["NominalCair"]) + Convert.ToDecimal(x["NominalMDR"]);
                i++;
            }
			
			TableRow tr1 = new TableRow();
			tr1.Style.Add("background-color","lightblue");
            TableCell td1 = new TableCell();
            td1.ColumnSpan = 5;
            td1.Text = "Total";
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
			
			td1 = new TableCell();
            td1.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(nominalso).ToString("N2", nfi) + "</p>";
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
			
			td1 = new TableCell();
			td1.ColumnSpan = 1;
            td1.Text = "<p style='font-family:consolas'>" + "" + "</p>";
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
			
			td1 = new TableCell();
            td1.Text = Convert.ToDecimal(nominalcair).ToString("N2", nfi);
            td1.CssClass = "text-center";
            td1.Visible = false;
            tr1.Cells.Add(td1);
			
			td1 = new TableCell();
            td1.Text = Convert.ToDecimal(nominalgesek).ToString("N2", nfi);
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
			
			td1 = new TableCell();
            td1.Text = Convert.ToDecimal(nominalcair).ToString("N2", nfi);
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
			
			td1 = new TableCell();
            td1.Text = Convert.ToDecimal(nominalmdr).ToString("N2", nfi);
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
			
			td1 = new TableCell();
            td1.ColumnSpan = 8;
            td1.Text = "";
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
            tb.Rows.Add(tr1);
        }
        else
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.ColumnSpan = 9;
            td.Text = "No Data";
            td.CssClass = "text-center";
            tr.Cells.Add(td);
            tb.Rows.Add(tr);
        }
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
            sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
            DataSet objDS = new DataSet();
            sqlAdapter.Fill(objDS, "data");
            sqlCnn.Close();
            rs = objDS.Tables["data"];
        }
        return rs;
    }
}