using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;

public partial class Page_Summary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
			Ddl.Store(lokasi);

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "0";
			ereceipttype.SelectedValue = "0";
            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {	
		string query = "sp_report_sales_order_e_receipt_new";
        string[] start = dari.Text.Split('-');
        string[] end = sampai.Text.Split('-');

        string startdate = start[2] + "-" + start[1] + "-" + start[0];
        string enddate = end[2] + "-" + end[1] + "-" + end[0];
		
		DataTable dt = Db.Rs("EXEC " + query + " '"
            + kw.Text + "','"
            + startdate + "','"
            + enddate + "','"
            + lokasi.SelectedValue + "','" 
            + ereceipttype.SelectedValue + "','" 
			+ status.SelectedValue + "', 1, 20, 0" , Db.Cnn_DB);

        int rowcount = dt.Rows.Count;
        if (rowcount > 0)
        {
            int index = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = TD_HyperLink((int)dr["ID"]);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(Convert.ToDateTime(dr["TGL"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["NOSO"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["LOKASI"].ToString();
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = dr["STATUS"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["CUSTOMER"].ToString() + " (" + dr["KODE_CUSTOMER"].ToString() + ")" + "<br/>" + "<label class=\"text-sm\">HP : " + dr["NOHP"].ToString() + "</label>";
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = dr["ERECEIPT_TYPE"].ToString();
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = dr["ERECEIPT_KETERANGAN"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b></b>";
            td2.ColumnSpan = 8;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            status.Text = Request.QueryString["status"];
            lokasi.Text = Request.QueryString["lokasi"];
			ereceipttype.SelectedValue = Request.QueryString["ereceipttype"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;
        href += "&lokasi=" + lokasi.SelectedValue;
		href += "&ereceipttype=" + ereceipttype.SelectedValue;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(int idso)
    {
        string x = string.Empty;
		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../../ReportSales/SalesOrderReport_Detail.aspx?id=" + idso + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  ";
		x += "</a>";
        
        return x;
    }
	
	protected void fillExcel(object sender, EventArgs e)
    {
		string kw_text = kw.Text;//Request.QueryString["kw"];
        string dari_text = dari.Text;//Request.QueryString["dari"];
        string sampai_text = sampai.Text;//Request.QueryString["sampai"];
        string status_text = status.SelectedValue;//Request.QueryString["status"];
        string lokasi_text = lokasi.SelectedValue;//Request.QueryString["lokasi"];
		string ereceipttype_text = ereceipttype.SelectedValue;//Request.QueryString["ereceipttype"];
			
		string query = "sp_report_sales_order_e_receipt_new";
        string[] start = dari_text.Split('-');
        string[] end = sampai_text.Split('-');

        string startdate = start[2] + "-" + start[1] + "-" + start[0];
        string enddate = end[2] + "-" + end[1] + "-" + end[0];
		
		System.Data.DataTable dt = Db.Rs("EXEC " + query + " '"
            + kw_text + "','"
            + startdate + "','"
            + enddate + "','"
            + lokasi_text + "','" 
            + ereceipttype_text + "','" 
		+ status_text + "', 1, 20, 1" , Db.Cnn_DB);
		
		CreateExcel(dt);
	}
	
	protected MemoryStream GetStream(ExcelPackage excelPackage)
	{
		MemoryStream fs = new MemoryStream();
		excelPackage.SaveAs(fs);
		fs.Position = 0;
		return fs;
	}
	
	protected void CreateExcel(DataTable dt)
	{
		using (var package = new ExcelPackage())
		{
			ExcelWorkbook wb = package.Workbook;
			ExcelWorksheet sheet = package.Workbook.Worksheets.Add("ReportEReceipt");
			//sheet.Cells["A1"].LoadFromDataTable(dt, true);

			/*int i = 1;
			foreach (DataColumn column in dt.Columns) 
			{
				sheet.Cells[1,i].Value = column.ColumnName;
				i++;
			}*/
			
			sheet.Cells[1,1].Value = "No";
			sheet.Cells[1,2].Value = "Tanggal";
			sheet.Cells[1,3].Value = "No. SO";
			sheet.Cells[1,4].Value = "Store";
			sheet.Cells[1,5].Value = "Status";
			sheet.Cells[1,6].Value = "Customer";
			sheet.Cells[1,7].Value = "Tipe";
			sheet.Cells[1,8].Value = "Keterangan";

			foreach (DataRow dr in dt.Rows)
			{
				/*foreach (DataColumn column in dt.Columns)
				{
					if(column.ColumnName == "ID" || column.ColumnName == "GIVENTO")
					{
						sheet.Cells[dt.Rows.IndexOf(row) + 2, dt.Columns.IndexOf(column) + 1].Value = row[column].ToString();
					}
				}*/
				
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 1].Value = (dt.Rows.IndexOf(dr) + 1).ToString();
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 2].Value = Str.Tgl(Convert.ToDateTime(dr["TGL"]));
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 3].Value = dr["NOSO"].ToString();
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 4].Value = dr["LOKASI"].ToString();
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 5].Value = dr["STATUS"].ToString();
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 6].Value = dr["CUSTOMER"].ToString() + " (" + dr["KODE_CUSTOMER"].ToString() + ") HP : " + dr["NOHP"].ToString() + "";
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 7].Value = dr["ERECEIPT_TYPE"].ToString();
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 8].Value = dr["ERECEIPT_KETERANGAN"].ToString();
			}

			// Setting & getting values
			/*ExcelRange firstCell = sheet.Cells[1, 1];
			firstCell.Value = "will it work?";
			sheet.Cells["A2"].Formula = "CONCATENATE(A1,\" ... Of course it will!\")";*/
			//Assert.That(firstCell.Text, Is.EqualTo("will it work?"));

			// Numbers
			/*var moneyCell = sheet.Cells["A3"];
			moneyCell.Style.Numberformat.Format = "$#,##0.00";
			moneyCell.Value = 15.25M;*/

			// Easily write any Enumerable to a sheet
			// In this case: All Excel functions implemented by EPPlus
			//var funcs = package.Workbook.FormulaParserManager.GetImplementedFunctions()
				//.Select(x => new { FunctionName = x.Key, TypeName = x.Value.GetType().FullName });
			//sheet.Cells["A4"].LoadFromCollection(funcs, true);
			
			//create a range for the table
			ExcelRange range = sheet.Cells[1, 1, dt.Rows.Count + 1, 8];

			//add a table to the range
			ExcelTable tab = sheet.Tables.Add(range, "Table1");

			//format the table
			tab.TableStyle = TableStyles.Medium13;
			
			//sheet.View.FreezePanes(1, 0);

			// Styling cells
			/*var someCells = sheet.Cells[1,1];
			someCells.Style.Font.Bold = true;
			someCells.Style.Font.Color.SetColor(Color.Ivory);
			someCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
			someCells.Style.Fill.BackgroundColor.SetColor(Color.Navy);*/

			sheet.Cells.AutoFitColumns();
			//package.SaveAs(new FileInfo(@"basicUsage.xslx"));

			MemoryStream stream = GetStream(package);// The method is defined below
			DateTime now = DateTime.Now;
			string tgl = now.Day.ToString().PadLeft(2,'0');
			string bulan = now.Month.ToString().PadLeft(2, '0');
			string tahun = now.Year.ToString().PadLeft(4, '0');
			string jam = now.Hour.ToString().PadLeft(2, '0');
			string menit = now.Minute.ToString().PadLeft(2, '0');
			string detik = now.Second.ToString().PadLeft(2, '0');
			
			Response.Clear();
			Response.Buffer = true;
			Response.AddHeader("content-disposition",
			"attachment; filename=Repost_Ereceipt_" + tgl + bulan + tahun + "_" + jam + menit + detik + ".xls");
			Response.ContentType = "application/vnd.ms-excel";
			Response.BinaryWrite(stream.ToArray());
			Response.End();
		}
    }
}