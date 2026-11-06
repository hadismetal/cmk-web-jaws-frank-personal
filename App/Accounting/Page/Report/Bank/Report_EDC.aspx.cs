using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Page_Report_Bank_Report_EDC : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);
        ddlEDC.Items.Add(new ListItem("-", "0"));
        ddlEDCBank.Items.Add(new ListItem("-", "0"));
        DLC.EDCList(ddlEDC);
        DLC.EDCList(ddlEDCBank);



        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            dt = Db.Rs("SELECT 'ALL' [KODE] UNION SELECT KODE FROM LOCOUTLET WHERE DRAFT = 0 AND DISABLE = 0 AND ISNULL(KODE,'') <> '' UNION ALL SELECT KODE FROM LOCEXHIBITION WHERE DRAFT = 0 AND DISABLE = 0 AND ISNULL(KODE,'') <> ''", Db.Cnn_DB);
            ddlStore.DataTextField = "KODE";
            ddlStore.DataValueField = "KODE";

            ddlStore.DataSource = dt;
            ddlStore.DataBind();
			
			var now = DateTime.Now;
            var start = new DateTime(now.Year, now.Month, 1); 
            tglSOstart.Text = start.ToString("yyyy-MM-dd");
            tglSOend.Text = DateTime.Now.ToString("yyyy-MM-dd");

			
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            txtAppCode.Text = Request.QueryString["apcode"];
            txtSONumber.Text = Request.QueryString["noso"];
            ddlStatus.SelectedValue = Request.QueryString["status"];
            ddlEDCBank.SelectedValue = Request.QueryString["bank"];
            tglUpload.Text = Request.QueryString["tgl"];
            ddlStore.SelectedValue = Request.QueryString["store"];
            tglSOstart.Text = Request.QueryString["tglsostart"];
            tglSOend.Text = Request.QueryString["tglsoend"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&apcode=" + txtAppCode.Text;
        href += "&noso=" + txtSONumber.Text;
        href += "&status=" + ddlStatus.SelectedValue;
        href += "&tgl=" + tglUpload.Text;
        href += "&bank=" + ddlEDCBank.SelectedValue;
        href += "&store=" + ddlStore.SelectedValue;
        href += "&tglsostart=" + tglSOstart.Text;
        href += "&tglsoend=" + tglSOend.Text;

        Response.Redirect(href);
    }

    protected void fillTable(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = Db.Rs("exec sp_list_approvalcode_cc '" + txtSONumber.Text + "','" + txtAppCode.Text + "','" + tglUpload.Text + "', '" + ddlEDCBank.SelectedValue + "', '" + ddlStatus.SelectedValue + "', '" + tglSOstart.Text + "', '" + tglSOend.Text + "', '" + ddlStore.SelectedValue + "' ,'0' ", Db.Cnn_DB);

        int i = 1;
        decimal nominalcair = 0m;
		decimal nominalso = 0m;
		decimal nominalmdr = 0m;

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow x in dt.Rows)
            {
                TableRow tr = new TableRow();
				
				TableCell td = new TableCell();
                td.Text = TD_HyperLink(Convert.ToInt32(x["IDSO"]), x["TIPE"].ToString());
                tr.Cells.Add(td);
				
                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + i.ToString() + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["Nomor"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["Lokasi"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + ((x["TglSO"] == null) ? "" : DateTime.Parse(x["TglSO"].ToString()).ToShortDateString()) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["ApprovalCode"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(x["NominalSO"]).ToString("N2") + "</p>";
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["EDC"].ToString() + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(x["NominalGesek"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                td.Visible = false;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(x["NominalCair"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(x["NominalMDR"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                if(x["Status"].ToString().ToUpper() == "CAIR")
                    td.Text = "<p style='font-family:consolas;color:blue;'>" + x["Status"].ToString() + "</p>";
                else
                    td.Text = "<p style='font-family:consolas;color:red;'>" + x["Status"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["StatusPostGL"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["CCNumber"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["CCName"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["Customer"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + x["Operator"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
				td.Visible = false;
                if (x["Status"].ToString().ToUpper() == "CAIR" && x["StatusPostGL"].ToString() == "")
                {
                    td.Text = "<button type='button' class='btn btn-danger' id=\"btn-show-modal-update-" + i + "\" onclick=\"onClickModalUpdateAppCode(this);\">Delete</button>";
                    td.Text += "<script type='text/javascript'>function onClickModalUpdateAppCode(e){$('#txtappcodemodal').val($(e).closest('tr').find('td')[4].innerText); $('#txtbank').val($(e).closest('tr').find('td')[5].innerText); $('#txtnominalmodal').val($(e).closest('tr').find('td')[6].innerText); $('#txtidtable').val($(e).closest('tr').find('td')[14].innerText); document.getElementById(\"btnDeleteAppCode\").style.display = \"\"; document.getElementById(\"btnPostAppCode\").style.display = \"none\"; $('#updateModal').modal('show'); }</script>";
                }
                else
                {
                    td.Text = "";
                }
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = x["ID"].ToString();
                td.Style.Add("display","none");
                tr.Cells.Add(td);

                td = new TableCell();
				td.Visible = false;
                if(x["StatusPostGL"].ToString() == "" && x["Status"].ToString().ToUpper() == "CAIR" && x["Nomor"].ToString() != "")
                {
                    td.Text = "<button type='button' class='btn btn-success' id=\"btn-show-modal-posting-" + i + "\" onclick=\"onClickModalPostAppCode(this);\">Posting</button>";
                    td.Text += "<script type='text/javascript'>function onClickModalPostAppCode(e){$('#txtappcodemodal').val($(e).closest('tr').find('td')[4].innerText); $('#txtbank').val($(e).closest('tr').find('td')[5].innerText); $('#txtnominalmodal').val($(e).closest('tr').find('td')[6].innerText); $('#txtidtable').val($(e).closest('tr').find('td')[14].innerText); document.getElementById(\"btnDeleteAppCode\").style.display = \"none\"; document.getElementById(\"btnPostAppCode\").style.display = \"\"; $('#updateModal').modal('show'); }</script>";
                }
                else
                {
                    td.Text = "";
                }
                tr.Cells.Add(td);

                tbl.Rows.Add(tr);

                nominalcair += Convert.ToDecimal(x["NominalCair"]);
				nominalso += Convert.ToDecimal(x["NominalSO"]);
				nominalmdr += Convert.ToDecimal(x["NominalMDR"]);
                i++;
            }

            TableRow tr1 = new TableRow();
            tr1.Style.Add("background-color", "lightblue");
            TableCell td1 = new TableCell();
            td1.ColumnSpan = 6;
            td1.Text = "<p style='font-family:consolas'>" + "Total" + "</p>";
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
            td1.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(nominalcair).ToString("N2", nfi) + "</p>";
            td1.CssClass = "text-center";
            td1.Visible = false;
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(nominalcair).ToString("N2", nfi) + "</p>";
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
			
			td1 = new TableCell();
            td1.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(nominalmdr).ToString("N2", nfi) + "</p>";
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.ColumnSpan = 8;
            td1.Text = "";
            td1.CssClass = "text-center";
            tr1.Cells.Add(td1);
            tbl.Rows.Add(tr1);

        }
        else
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.ColumnSpan = 21;
            td.Text = "<p style='font-family:consolas'>" + "No Data" + "</p>";
            td.CssClass = "text-center";
            tr.Cells.Add(td);
            tbl.Rows.Add(tr);

        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string message = "";
        if ((fileexcel.PostedFile != null) && (fileexcel.PostedFile.ContentLength > 0))
        {
            string fn = System.IO.Path.GetFileName(fileexcel.PostedFile.FileName);
            string sheetname = fn.Remove(fn.IndexOf('.'));
            string FolderName = Server.MapPath("~/App_Data/Data");
            if (!Directory.Exists(FolderName))
            {
                Directory.CreateDirectory(FolderName);
            }
            string SaveLocation = FolderName + "\\" + fn;

            if (File.Exists(SaveLocation))
            {
                File.Delete(SaveLocation);
            }
            fileexcel.PostedFile.SaveAs(SaveLocation);
            Response.Write("The file has been uploaded.");

            string ConStr = "";
            string ext = Path.GetExtension(fileexcel.FileName).ToLower();
            if (ext.Trim() == ".xls")
            {
                //connection string for that file which extantion is .xls  
                ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + SaveLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (ext.Trim() == ".xlsx")
            {
                //connection string for that file which extantion is .xlsx  
                ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + SaveLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            //making query  
            string query = "SELECT * FROM [Sheet1$]";
            //Providing connection  
            OleDbConnection conn = new OleDbConnection(ConStr);
            //checking that connection state is closed or not if closed the   
            //open the connection  
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
			else
            {
                conn.Close();
                conn.Open();
            }
            //try
            //{
            //create command object  
            OleDbCommand cmd = new OleDbCommand(query, conn);
            // create a data adapter and get the data into dataadapter  
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            //fill the Excel data to data set  
            da.Fill(dt);
            conn.Close();

            if (dt.Rows.Count > 0)
            {
                string Msg = "";
                if (Msg == "")
                {
					try
					{
						string query2 = "";
						foreach (DataRow x in dt.Rows)
						{
							int IDEDC = Convert.ToInt32(ddlEDC.SelectedValue);
							string NomorEDC = x[0].ToString();
							string NominalCair = x[1].ToString();
							string TanggalCair = x[4].ToString() + "-" + x[3].ToString() + "-" + x[2].ToString();
                            string MID = x[5].ToString();
                            if (!String.IsNullOrEmpty(NomorEDC))
                            {
								query2 += "EXEC sp_insert_update_edc 'I', '" + NomorEDC + "','" + NominalCair + "'," + IDEDC + ", '" + TanggalCair + "', '" + MID + "' ,'" + App.UserNama + "' ";
                                //Db.Execute("EXEC sp_insert_update_edc 'I', '" + NomorEDC + "','" + NominalCair + "'," + IDEDC + ", '" + TanggalCair + "', '" + MID + "' ,'" + App.UserNama + "' ", Db.Cnn_DB);
                            }
						}
						if(query2 != "")
							ExecuteSql(query2, Db.Cnn_DB);
					}
					catch(Exception ex)
					{
						Msg = ex.Message.ToString();
					}
					if(Msg != "")
						message = Msg;
                }   
            }
            else
            {
                message = "Data gagal dimasukkan (Data sudah ada atau excel yang diimport kosong).";
            }
        }
        else
        {
            message = "Please select a file to upload.";
        }
		if (message == "")
            lblMessage1.Text = "Proses Upload Berhasil!!!";
        else
            lblMessage1.Text = message;

        ScriptManager.RegisterStartupScript(this, GetType(), "alertsave", "$(document).ready(function(){ $('#postingModal').modal('show'); });", true/* addScriptTags */);
    }

    protected void btnTemplateExcel_Click(object sender, EventArgs e)
    {

        string fileName = "Form-Upload-ApproveCode.xlsx";
        string outputdirectory = Server.MapPath("~/Form/Data");
        string Tanggal = DateTime.Today.ToString();
        //FileInfo fi = ConvertToXLSX(fileName, outputdirectory);

        HttpResponse Response = HttpContext.Current.Response;
        //clear the buffer stream
        Response.ClearHeaders();
        Response.ClearContent();
        Response.Clear();
        Response.Buffer = true;

        //add the content type
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //add a filename
        Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
        Response.TransmitFile(outputdirectory + "\\" + fileName);

        //cleanup
        Response.Flush();
        Response.End();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string href = "Report_EDC_Export.aspx?refreshpage=1";
        href += "&apcode=" + txtAppCode.Text;
        href += "&noso=" + txtSONumber.Text;
        href += "&status=" + ddlStatus.SelectedValue;
        href += "&tgl=" + tglUpload.Text;
        href += "&bank=" + ddlEDCBank.SelectedValue;
        href += "&store=" + ddlStore.SelectedValue;
        href += "&tglsostart=" + tglSOstart.Text;
        href += "&tglsoend=" + tglSOend.Text;

        Response.Redirect(href);
    }
    protected string validateExcel(DataTable dt)
    {
        string error = "";
        int index = 1;
        List<int> errorindex = new List<int>();
        foreach (DataRow dr in dt.Rows)
        {
            string NomorEDC = dr[0].ToString();
            string NominalGesek = dr[1].ToString();
            string NominalCair = dr[2].ToString();
            if (NomorEDC == "" || NominalGesek == "" || NominalCair == "")
                errorindex.Add(index);
            index++;
        }

        string rowerror = "";
        foreach (int er in errorindex)
        {
            rowerror += er.ToString() + ",";
        }
        rowerror = rowerror.Remove(rowerror.Length - 1);

        error += "Error : Excel gagal upload, baris ke " + rowerror + " tidak boleh kosong field Nomor Approval Code, Nominal Gesek atau Cair.";
        return error;
    }
	//Execute Command
    protected void ExecuteSql(string strSql, string CnnString)
    {
        SqlConnection sqlCnn = new SqlConnection(CnnString);
        SqlCommand sqlCmd = new SqlCommand(strSql, sqlCnn);
        sqlCmd.CommandTimeout = 900; // 300 sec or 5 mins for long time query
        sqlCnn.Open();
        sqlCmd.ExecuteNonQuery();
        sqlCnn.Close();
    }
	
	protected string TD_HyperLink(int id, string tipe)
    {
		string x = string.Empty;
		if(tipe == "DP")
		{
			x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
			x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../../ReportSales/DPPO_Report_Detail.aspx?id=" + id + "\">";
			x += "  <i class=\"icon-search\"></i> ";
			x += "  ";
			x += "</a>";
			x += "</div>";
		}
		else
		{
			x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
			x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../../ReportSales/SalesOrderReport_Detail.aspx?id=" + id + "\">";
			x += "  <i class=\"icon-search\"></i> ";
			x += "  ";
			x += "</a>";
			x += "</div>";
		}
        return x;
    }
}