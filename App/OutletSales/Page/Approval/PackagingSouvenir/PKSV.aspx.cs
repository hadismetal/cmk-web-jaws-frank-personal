using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;

public partial class Page_PKSV : System.Web.UI.Page
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

        HfOperatorNama.Value = App.UserNama;

        if (!IsPostBack)
        {
            DateTime now = DateTime.Now;
            start.Text = now.ToString("dd-MM-yyyy");
            end.Text = now.ToString("dd-MM-yyyy");
            Location(loccode);

            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected string getLocationUser()
    {
        string result = "";
        int xUserID = App.UserID;
        int xisAM = 0;
        string listStore = "";
        //check area manager
        var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
        int amcount = am.Count();
        if (amcount > 0)
        {
            int i = 0;
            foreach (var x in am)
            {
                int j = 0;
                var lokasi = x.DataAreaManager_Lokasi
                    .GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
                    .Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
                foreach (var y in lokasi)
                {
                    //listStore = "";
                    if (y.TipeLokasi == 1)
                        listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
                    else
                        listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
                    j++;
                    listStore += ",";
                }
                i++;
            }
            xisAM = 1;
        }

        if (xisAM == 0)
        {
            //check admin sales
            var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
            if (ad.Count() > 0)
            {
                int k = 0;
                foreach (var x in ad)
                {
                    listStore = "";
                    if (x.TipeLokasi == 1)
                        listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
                    else
                        listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
                    k++;
                    listStore += ",";
                }
            }
        }

        result = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;

        return result;
    }

    protected void Location(DropDownList ddl)
    {
        string keyloc = getLocationUser();

        string query = "SELECT A.* FROM (" +
            "SELECT Kode, UPPER(Nama) [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
            "UNION ALL " +
            "SELECT Kode, UPPER(Nama) [Nama] FROM LocExhibition WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
            ") A";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("All", ""));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["Kode"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            tipe.SelectedValue = Request.QueryString["tipe"];
            loccode.SelectedValue = Request.QueryString["location"];
            status.SelectedValue = Request.QueryString["status"];
            start.Text = Request.QueryString["start"];
            end.Text = Request.QueryString["end"];
        }
    }
    protected void fillTable(object sender, EventArgs e)
    {
        string keyword = ChangeStringWildCardCharacterSQL(kw.Text);
        string[] starttgl = start.Text.Split('-');
        string[] endtgl = end.Text.Split('-');
        string tipet = tipe.SelectedValue;

        string started = ChangeStringWildCardCharacterSQL(starttgl[2] + "-" + starttgl[1] + "-" + starttgl[0]);
        string ended = ChangeStringWildCardCharacterSQL(endtgl[2] + "-" + endtgl[1] + "-" + endtgl[0]);
        string statusddl = ChangeStringWildCardCharacterSQL(status.SelectedValue);

        string xKeyloc = "";

        if (loccode.SelectedValue == "")
            xKeyloc = getLocationUser();
        else
            xKeyloc = loccode.SelectedValue;

        string query = "EXEC sp_report_approval_pksv_new " +
            "'" + keyword + "', " +
            "'" + xKeyloc + "', " +
            "'" + tipet + "', " +
            "'" + started + "', " +
            "'" + ended + "', " +
            "'" + statusddl + "', " +
            "'1','50','0'";

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_DB);

        int index = 1;
        int TotalQty = 0;
		if (dt.Rows.Count > 0)
		{
			foreach (DataRow dr in dt.Rows)
			{
				TableRow tr = new TableRow();
				TableCell td;

				int statusint = Convert.ToInt32(dr["STATUSINT"]);

				td = new TableCell();
				td.Text = TD_Hyperlink(Convert.ToInt32(dr["ID"]), statusint, Convert.ToInt32(dr["IDSALESORDER"]), Convert.ToInt32(dr["IDRECEIPT"]), dr["NOSO"].ToString(), dr["TIPE"].ToString());
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = index++.ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["TIPE"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["NOSO"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["LOCCODE"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = dr["SALESNAMA"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = dr["CUSTOMERNAMA"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["KETERANGAN"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Convert.ToInt32(dr["QTY"]).ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["ITEM"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["STATUS"].ToString().ToUpper();
				td.Style.Add("text-align", "center");
				//pending
				if (statusint == 1)
				{
					td.Style.Add("background-color", "silver");
				}
				//approved
				else if (statusint == 2)
				{
					td.Style.Add("background-color", "limegreen");
				}
				else if (statusint == 3)
				{
					td.Style.Add("background-color", "orange");
				}

				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["OPERATOR"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["APPROVAL"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["APPROVALTGL"] == DBNull.Value ? "" : Convert.ToDateTime(dr["APPROVALTGL"]).ToString("dd-MM-yyyy");
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["REJECTED"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["REJECTEDTGL"] == DBNull.Value ? "" : Convert.ToDateTime(dr["REJECTEDTGL"]).ToString("dd-MM-yyyy");
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = dr["KETERANGANREJECT"].ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				tb.Rows.Add(tr);

				TotalQty += Convert.ToInt32(dr["QTY"]);
			}
			TableRow tr2 = new TableRow();
			TableCell td2;

			td2 = new TableCell();
			td2.Text = "<b>Total</b>";
			td2.ColumnSpan = 8;
			tr2.Cells.Add(td2);

			td2 = new TableCell();
			td2.Text = "<b>" + Convert.ToDecimal(TotalQty).ToString("N0") + "</b>";
			td2.Style.Add("text-align", "center");
			tr2.Cells.Add(td2);

			td2 = new TableCell();
			td2.Text = "";
			td2.ColumnSpan = 8;
			tr2.Cells.Add(td2);

			tb.Rows.Add(tr2);
		}
		else
		{
			TableRow tr2 = new TableRow();
			TableCell td2;

			td2 = new TableCell();
			td2.Text = "<h3 class=\"text-muted text-center\">Data Not Found.<h3>";
			td2.ColumnSpan = 16;
			tr2.Cells.Add(td2);

			tb.Rows.Add(tr2);
		}
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);
        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipe=" + tipe.SelectedValue;
        href += "&location=" + loccode.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&start=" + start.Text;
        href += "&end=" + end.Text;
        Response.Redirect(href);
    }


    protected string TD_Hyperlink(int id, int status, int idso, int idreceipt, string nomor, string tipe)
    {
        string x = string.Empty;
        if (status == 1)
        {
			string check = CheckAvailibilityStockBeforeApprove(id, tipe);
			if(check == "")
			{
				x += "<button class=\"btn btn-xs btn-success\" data-id=\"" + id + "\" data-no=\"" + nomor + "\" data-tipe=\"" + tipe + "\" onclick=\"approve(this);\">";
				x += "Approve";
				x += "</button>";
				
				x += "<button class=\"btn btn-xs btn-danger\" style=\"margin-left:2px\" data-id=\"" + id + "\" data-no=\"" + nomor + "\" data-tipe=\"" + tipe + "\" onclick=\"reject(this);\">";
				x += "Reject";
				x += "</button>";
			}
			else
			{
				x += "<label class=\"text-danger\">" + check + "</label>";
				
				x += "<button class=\"btn btn-xs btn-danger\" style=\"margin-left:2px\" data-id=\"" + id + "\" data-no=\"" + nomor + "\" data-tipe=\"" + tipe + "\" onclick=\"reject(this);\">";
				x += "Reject";
				x += "</button>";
			}
        }
        else if (status == 2)
        {
            x += "<a target=\"_blank\" class=\"btn btn-xs btn-info\" style=\"margin-left:2px\" href=\"../../Create/Payment/Sales_Packaging.aspx?id=" + idso + "&sid=" + idreceipt + "\">";
            x += "Print";
            x += "</a>";
        }

        return x;
    }
	
	protected string CheckAvailibilityStockBeforeApprove(int id, string tipe)
	{
		string query = "EXEC sp_check_availibility_pksv " + id + ", '" + tipe + "'";
		return Db.SingleString(query, Db.Cnn_DB);
	}

    protected string ChangeStringWildCardCharacterSQL(string input = "")
    {
        string x = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].ToString() == "'")
                x += input[i] + "'";
            else
                x += input[i];
        }
        return x;
    }

    protected void excel_Click(object sender, EventArgs e)
    {
        string keyword = ChangeStringWildCardCharacterSQL(kw.Text);
        string[] starttgl = start.Text.Split('-');
        string[] endtgl = end.Text.Split('-');
        string tipet = tipe.SelectedValue;

        string started = ChangeStringWildCardCharacterSQL(starttgl[2] + "-" + starttgl[1] + "-" + starttgl[0]);
        string ended = ChangeStringWildCardCharacterSQL(endtgl[2] + "-" + endtgl[1] + "-" + endtgl[0]);
        string statusddl = ChangeStringWildCardCharacterSQL(status.SelectedValue);

        string xKeyloc = "";
        if (loccode.SelectedValue == "")
            xKeyloc = getLocationUser();
        else
            xKeyloc = loccode.SelectedValue;

        string query = "EXEC sp_report_approval_pksv_new " +
            "'" + keyword + "', " +
            "'" + xKeyloc + "', " +
            "'" + tipet + "', " +
            "'" + started + "', " +
            "'" + ended + "', " +
            "'" + statusddl + "', " +
            "'1','50','0'";

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_DB);

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
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("ReportPKSV");

            sheet.Cells[1, 1].Value = "No";
            sheet.Cells[1, 2].Value = "Tipe";
            sheet.Cells[1, 3].Value = "Nomor";
            sheet.Cells[1, 4].Value = "Lokasi";
			sheet.Cells[1, 5].Value = "Sales";
			sheet.Cells[1, 6].Value = "Customer";
            sheet.Cells[1, 7].Value = "Keterangan";
            sheet.Cells[1, 8].Value = "Qty";
			sheet.Cells[1, 9].Value = "Item";
            sheet.Cells[1, 10].Value = "Status";
            sheet.Cells[1, 11].Value = "Operator";
            sheet.Cells[1, 12].Value = "Approval By";
            sheet.Cells[1, 13].Value = "Approval Tgl";
            sheet.Cells[1, 14].Value = "Rejected By";
            sheet.Cells[1, 15].Value = "Rejected Tgl";
            sheet.Cells[1, 16].Value = "Keterangan Reject";

            int TotalPCS = 0;
            foreach (DataRow dr in dt.Rows)
            {
                int statusint = Convert.ToInt32(dr["STATUSINT"]);

                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 1].Value = (dt.Rows.IndexOf(dr) + 1).ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 2].Value = dr["TIPE"].ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 3].Value = dr["NOSO"].ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 4].Value = dr["LOCCODE"].ToString();
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 5].Value = dr["SALESNAMA"].ToString();
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 6].Value = dr["CUSTOMERNAMA"].ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 7].Value = dr["KETERANGAN"].ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 8].Value = Convert.ToInt32(dr["QTY"]);
				sheet.Cells[dt.Rows.IndexOf(dr) + 2, 9].Value = dr["ITEM"].ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 10].Value = dr["STATUS"].ToString().ToUpper();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 11].Value = dr["OPERATOR"].ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 12].Value = dr["APPROVAL"].ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 13].Value = dr["APPROVALTGL"] == DBNull.Value ? "" : Convert.ToDateTime(dr["APPROVALTGL"]).ToString("dd-MM-yyyy");
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 14].Value = dr["REJECTED"].ToString();
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 15].Value = dr["REJECTEDTGL"] == DBNull.Value ? "" : Convert.ToDateTime(dr["REJECTEDTGL"]).ToString("dd-MM-yyyy");
                sheet.Cells[dt.Rows.IndexOf(dr) + 2, 16].Value = dr["KETERANGANREJECT"].ToString();

                TotalPCS += Convert.ToInt32(dr["QTY"]);
            }

            //create a range for the table
            ExcelRange range = sheet.Cells[1, 1, dt.Rows.Count + 1, 16];

            //add a table to the range
            ExcelTable tab = sheet.Tables.Add(range, "Table1");

            //format the table
            tab.TableStyle = TableStyles.Medium13;


            sheet.Cells.AutoFitColumns();

            MemoryStream stream = GetStream(package);// The method is defined below
            DateTime now = DateTime.Now;
            string tgl = now.Day.ToString().PadLeft(2, '0');
            string bulan = now.Month.ToString().PadLeft(2, '0');
            string tahun = now.Year.ToString().PadLeft(4, '0');
            string jam = now.Hour.ToString().PadLeft(2, '0');
            string menit = now.Minute.ToString().PadLeft(2, '0');
            string detik = now.Second.ToString().PadLeft(2, '0');

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
            "attachment; filename=Report_PKSV_" + tgl + bulan + tahun + "_" + jam + menit + detik + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
}