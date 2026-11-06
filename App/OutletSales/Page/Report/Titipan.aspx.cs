using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Globalization;

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
    protected string[] ImagesFormat { get { return new string[] { ".jpg", ".jpeg", ".png", ".pdf" }; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            Ddl.Location(lokasi, App.UserID, tipelokasi);
            FillStatusTitipan();
            GetTipeTitipan();
            //Generate
            fillFilter();                           
                
                // int selectedStatus;    

                // var ad = from p in db.DataAdminSales
                //          where p.IDUser == App.UserID
                //          select p;
                // int TipeLokasi = -1;
                // int IDLokasi = 0;


                // if (ad.Count() > 0)
                // {
                //     var c = ad.First();
                //     rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
                // }
                
            // imgpicture_format.InnerHtml = Str.ComaExt(Upload.ImageFormat);

            // if (App.Refresh(this)) fillTable(sender, e);
        }
    }


    //Display Table
    // protected void fillTable(object sender, EventArgs e)
    // {
	// 	FillLokasi(sender,e);
		
	// 	lokasi.SelectedValue = Request.QueryString["idlokasi"];
		
    //     var rs = from p in db.DocTitipan
    //              where p.StatusTitipan.HasValue
    //              && p.Draft.Value == false
    //              select p;

    //     //check admin sales
    //     var ad = from p in db.DataAdminSales
    //              where p.IDUser == App.UserID
    //              select p;

    //     if (ad.Count() > 0)
    //     {
    //         var c = ad.First();
    //         rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
    //     }

    //     var am = from p in db.DataAreaManager
    //              where p.IDUser == App.UserID
    //              select p;
    //     if (am.Count() > 0)
    //     {
    //         int[] OutletID = DLC.AMOutlet(App.UserID);
    //         rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
    //     }
    //    int selectedStatus;
    //     if (int.TryParse(status.SelectedValue, out selectedStatus) && selectedStatus != -1)
    //     {
    //         rs = rs.Where(p => p.StatusTitipan == selectedStatus);
    //     }

    //     rs = rs.OrderByDescending(p => p.Tgl);
		
	// 	int TipeLokasi = -1;
    //     int IDLokasi = -1;

    //     try
    //     {
    //         TipeLokasi = Convert.ToInt32(tipelokasi.SelectedValue);
    //     }
    //     catch (Exception) { }

    //     try
    //     {
    //         IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
    //     }
    //     catch (Exception) { }
        
    //     if (TipeLokasi == 1 || TipeLokasi == 2)
    //     {
    //         rs = rs.Where(p => p.TipeLokasi == TipeLokasi);

    //         if (IDLokasi > 0)
    //         {
    //             rs = rs.Where(p => p.IDLokasi == IDLokasi);
    //         }
    //     } 

    //     string keyword = kw.Text;
    //     if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "" || p.CustomerNama.Contains(kw.Text));

    //     if (dari.Text != "" && sampai.Text != "")
    //     {
    //         rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
    //     }

    //     if (rs.Count() > 0)
    //     {
    //         int prev = 0;
    //         int index = 1;
    //         foreach (var r in rs)
    //         {
    //             int Qty = 0;

    //             Qty = r.DocTitipan_Product.Count();

    //             if (!Response.IsClientConnected) break;

    //             //Sub-Header

    //             TableRow tr = new TableRow();
    //             TableCell td;

    //             td = new TableCell();
    //             td.Text = TD_HyperLink(r);
    //             td.CssClass = "col-md-6";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "<p>" + index++.ToString() + ".</p>";
    //             tr.Cells.Add(td);
				
	// 			int? tl = r.TipeLokasi;
	// 			int? l = r.IDLokasi;
	// 			string lokasih = "";
	// 			if(tl == 1)
	// 				lokasih = Db.SingleString("SELECT TOP 1 UPPER(Nama) [Lokasi] FROM LocOutlet WHERE ID = " + l.ToString(), Db.Cnn_DB);
	// 			else
	// 				lokasih = Db.SingleString("SELECT TOP 1 UPPER(Nama) [Lokasi] FROM LocExhibition WHERE ID = " + l.ToString(), Db.Cnn_DB);
	// 			td = new TableCell();
    //             td.Text = lokasih;
    //             tr.Cells.Add(td);							 

    //             td = new TableCell();
    //             td.Text = Str.FullDate(r.Tgl);
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.Nomor;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.IDCustomer.HasValue ? r.CustomerNama : "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.IDSales.HasValue ? r.DataSales.Nama : "";
    //             tr.Cells.Add(td);

	// 			string plu = "";
	// 			if(r.DocTitipan_Product.Count > 0)
	// 			{
	// 				var product = r.DocTitipan_Product;
	// 				foreach (var c in product)
	// 				{
	// 					if(c.IDProduct != null) 
    //                         plu = plu + c.StockProductDJ.Nomor + ",";
    //                     if (c.IDProductTitipan != null)
    //                         plu = plu + c.StockProductDJCustomer.Nomor + ",";
	// 				}
	// 				plu = plu.Remove(plu.Length - 1);
	// 			}
	// 			td = new TableCell();
    //             td.Text = plu;
    //             tr.Cells.Add(td);							 

    //             td = new TableCell();
    //             td.Text = Qty.ToString();
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = DL.StatusTitipan(r.StatusTitipan.Value);
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Db.SingleString("SELECT ISNULL(b.Nama, '-') as KategoriTitipan FROM DocTitipan a JOIN MasterTipeTitipan b ON a.IDMasterTipeTitipan = b.ID WHERE a.ID=" + r.ID, Db.Cnn_DB);
    //             tr.Cells.Add(td);

    //             string tglPengembalian = Db.SingleString("SELECT ISNULL(FORMAT(tglPengembalian, 'dddd, dd MMMM yyyy', 'id-ID'),'-') as TanggalPengembalian FROM DocTitipan WHERE ID=" + r.ID, Db.Cnn_DB);
    //             td = new TableCell();
    //             td.Text =  tglPengembalian;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Db.SingleString("SELECT ISNULL(OperatorPengembalian, '-') as OperatorPengembalian FROM DocTitipan where ID="+r.ID, Db.Cnn_DB);
    //             tr.Cells.Add(td);

    //             string imagePath = Db.SingleString("SELECT ISNULL(BuktiPengembalian, '-') FROM DocTitipan where ID=" + r.ID, Db.Cnn_DB);
    //             string gambarnya="";
    //             if (tglPengembalian == "-")
    //             {
    //                 gambarnya = "<span class=\"text-muted\">Titipan ini belum di kembalikan</span>";
    //             }
    //             else
    //             {                    
                    
    //                     gambarnya = "<button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#uploadModal\" data-id='"+r.ID+"' data-nama='" + r.Nomor + "' >Upload Bukti</button>";
                    
    //             }


    //             td = new TableCell();
    //             td.Text = imagePath == "-" ? gambarnya : "<img src=\"/Images.aspx?path=" + imagePath + "\" border=\"0\" width=\"150\" />";
    //             td.CssClass = "text-center";
    //             tr.Cells.Add(td);

    //             tb.Rows.Add(tr);
    //         }
    //     }
    // }

    protected void upload(object sender, EventArgs e) {
		// var d = db.DocTitipan.SingleOrDefault(p => p.ID == Convert.ToInt32(hiddenNomor.Value));
        int id;
        int.TryParse(hiddenNomor.Value, out id);
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == id);        
	
        //Process
        bool finish = finishUpload(d);

        //Redirect
        if (finish) Response.Redirect("Titipan.aspx");	
	}
	protected bool finishUpload(DocTitipan d) {
		bool x = true;
        
		x = x && saveImage(d, imgpicture, imgpicture_ERR, "Picture");

		return x;
	}

	//Process
	protected bool saveImage(DocTitipan d, FileUpload file, HtmlGenericControl lb, string name) {
		if (!Upload.HasFile(file)) {
			//Commit
			//d.ImgPicture = null;
			//db.SubmitChanges();

			return true;
		}
		else {
			if (!Upload.FormatValid(file, lb, ImagesFormat)) return false;
			else {
				string path = string.Empty;

				path += App.ImagesDir;
				path += "BuktiPengembalianTitipan" + "\\";

				if (Upload.CreateDir(path)) {
					path += name + "_";
					path += d.ID + Upload.GetExt(file.PostedFile.FileName);

					//Upload
					file.PostedFile.SaveAs(path);

					//Commit
					// d.BuktiPengembalian = path;
                    using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
                    {
                        conn.Open();

                        string sql = "UPDATE DocTitipan SET BuktiPengembalian = @path WHERE Id = @id";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@path", path);
                            cmd.Parameters.AddWithValue("@id", d.ID);

                            int rowsAffected = cmd.ExecuteNonQuery();                            
                        }
                    }					

					return true;
				}else
                {
                    return false;
                }
			}
        }
	}
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
			tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
			lokasi.SelectedValue = Request.QueryString["idlokasi"];
            status.SelectedValue = Request.QueryString["status"];
            tipeTitipan.SelectedValue = Request.QueryString["tipeTitipan"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
		href += "&tipelokasi=" + tipelokasi.SelectedValue;
		href += "&idlokasi=" + lokasi.SelectedValue;
		href += "&status=" + status.SelectedValue;
        href += "&tipeTitipan=" + tipeTitipan.SelectedValue;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(DocTitipan r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Titipan_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        x += "&nbsp;&nbsp;<a class=\"btn btn-info\" target=\"_blank\" href=\"Titipan_Update.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Update Status";
        x += "</a>";

        return x;
    }
	protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
			lokasi.Items.Add(new ListItem("Lokasi", "0"));
        }
        //lokasi.Focus();
    }

    protected void GetTipeTitipan()
    {
        tipeTitipan.Items.Clear();
        tipeTitipan.Items.Add(new ListItem("ALL", "-1"));        
        DataSet ds = new DataSet();
        using(SqlConnection  connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            string query = @"SELECT ID, Nama FROM MasterTipeTitipan WHERE Disable = 0";
            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
                DataTable dataTipeTitip = ds.Tables[0];                

                foreach (DataRow row in dataTipeTitip.Rows)
                {
                    
                string id = row["ID"].ToString();
                string nama = row["Nama"].ToString();
                tipeTitipan.Items.Add(new ListItem(nama, id));                    
                }                
            }
            connection.Close();
        }                
    }
    protected void FillStatusTitipan()
    {
        status.Items.Clear();
        status.Items.Add(new ListItem("ALL", "-1"));

        string query = "SELECT ID, Nama FROM frank_db.dbo.StatusTitipan ORDER BY Nama";

        using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(Db.Cnn_DB))
        {
            using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
            {
                conn.Open();
                using (System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader["ID"].ToString();
                        string nama = reader["Nama"].ToString();
                        status.Items.Add(new ListItem(nama, id));
                    }
                }
            }
        }
    }

	// protected void exportData_ServerClick(object sender, EventArgs e)
	// {
	// 	var rs = from p in db.DocTitipan
	// 			 where p.StatusTitipan.HasValue && p.Draft.Value == false
	// 			 select p;

	// 	var ad = from p in db.DataAdminSales
	// 			 where p.IDUser == App.UserID
	// 			 select p;
	// 	if (ad.Any())
	// 	{
	// 		var c = ad.First();
	// 		rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
	// 	}

	// 	var am = from p in db.DataAreaManager
	// 			 where p.IDUser == App.UserID
	// 			 select p;
	// 	if (am.Any())
	// 	{
	// 		int[] OutletID = DLC.AMOutlet(App.UserID);
	// 		rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
	// 	}

	// 	int selectedStatus;
	// 	if (int.TryParse(status.SelectedValue, out selectedStatus) && selectedStatus != -1)
	// 	{
	// 		rs = rs.Where(p => p.StatusTitipan == selectedStatus);
	// 	}

	// 	int tipe;
	// 	if (int.TryParse(tipelokasi.SelectedValue, out tipe) && (tipe == 1 || tipe == 2))
	// 	{
	// 		rs = rs.Where(p => p.TipeLokasi == tipe);

	// 		int idlokasi;
	// 		if (int.TryParse(lokasi.SelectedValue, out idlokasi) && idlokasi > 0)
	// 		{
	// 			rs = rs.Where(p => p.IDLokasi == idlokasi);
	// 		}
	// 	}

	// 	string keyword = kw.Text;
	// 	if (!string.IsNullOrEmpty(keyword))
	// 	{
	// 		rs = rs.Where(p => p.Nomor == keyword || p.CustomerNama.Contains(keyword));
	// 	}

	// 	DateTime dariDate, sampaiDate;
	// 	if (DateTime.TryParse(dari.Text, out dariDate) && DateTime.TryParse(sampai.Text, out sampaiDate))
	// 	{
	// 		rs = rs.Where(p => p.Tgl >= dariDate && p.Tgl < sampaiDate.AddDays(1));
	// 	}

	// 	rs = rs.OrderByDescending(p => p.Tgl);

	// 	// EXPORT SECTION
	// 	Response.Clear();
	// 	Response.Buffer = true;
	// 	Response.AddHeader("content-disposition", "attachment;filename=Export_Titipan.xls");
	// 	Response.Charset = "";
	// 	Response.ContentType = "application/vnd.ms-excel";

	// 	using (System.IO.StringWriter sw = new System.IO.StringWriter())
	// 	using (HtmlTextWriter hw = new HtmlTextWriter(sw))
	// 	{
	// 		Table table = new Table { GridLines = GridLines.Both };

	// 		// Header
	// 		TableHeaderRow header = new TableHeaderRow();
	// 		string[] headers = { "No.", "Cabang", "Tanggal", "No. Referensi", "Nama Customer", "Nama Sales", "No. PLU", "Qty", "Status", "Tanggal Pengembalian" };
	// 		foreach (string head in headers)
	// 		{
	// 			header.Cells.Add(new TableHeaderCell { Text = head });
	// 		}
	// 		table.Rows.Add(header);

	// 		int index = 1;
	// 		foreach (var r in rs)
	// 		{
	// 			TableRow tr = new TableRow();
	// 			tr.Cells.Add(new TableCell { Text = index++.ToString() });

	// 			// Cabang
	// 			string cabang = "";
	// 			if (r.TipeLokasi == 1)
	// 				cabang = Db.SingleString("SELECT TOP 1 UPPER(Nama) FROM LocOutlet WHERE ID = " + r.IDLokasi, Db.Cnn_DB);
	// 			else
	// 				cabang = Db.SingleString("SELECT TOP 1 UPPER(Nama) FROM LocExhibition WHERE ID = " + r.IDLokasi, Db.Cnn_DB);
	// 			tr.Cells.Add(new TableCell { Text = cabang });

	// 			tr.Cells.Add(new TableCell { Text = Str.FullDate(r.Tgl) });
	// 			tr.Cells.Add(new TableCell { Text = r.Nomor });
	// 			tr.Cells.Add(new TableCell { Text = r.IDCustomer.HasValue ? r.CustomerNama : "" });
	// 			tr.Cells.Add(new TableCell { Text = r.IDSales.HasValue ? r.DataSales.Nama : "" });

	// 			// PLU
	// 			string plu = "";
	// 			foreach (var c in r.DocTitipan_Product)
	// 			{
	// 				if (c.IDProduct != null)
	// 					plu += c.StockProductDJ.Nomor + ",";
	// 				if (c.IDProductTitipan != null)
	// 					plu += c.StockProductDJCustomer.Nomor + ",";
	// 			}
	// 			if (plu.Length > 0)
	// 				plu = plu.TrimEnd(',');
	// 			tr.Cells.Add(new TableCell { Text = plu });

	// 			// Qty
	// 			tr.Cells.Add(new TableCell { Text = r.DocTitipan_Product.Count.ToString() });

    //             // Status
    //             tr.Cells.Add(new TableCell { Text = DL.StatusTitipan(r.StatusTitipan.Value) });
                

    //             tr.Cells.Add(new TableCell { Text = Db.SingleString("SELECT ISNULL(FORMAT(tglPengembalian, 'dddd, dd MMMM yyyy', 'id-ID'),'-') as TanggalPengembalian FROM DocTitipan where ID="+r.ID, Db.Cnn_DB)});                


	// 			table.Rows.Add(tr);
	// 		}

	// 		table.RenderControl(hw);
	// 		Response.Output.Write(sw.ToString());
	// 		Response.Flush();
	// 		Response.End();
	// 	}
	// }

    // [WebMethod]
    // [ScriptMethod(UseHttpGet = true)]
    // public static List<DataTipeTitipan> GetTipeTitipan()
    // {
    //     var list = new List<DataTipeTitipan>();
    //     DataSet ds = new DataSet();
    //     using(SqlConnection  connection = new SqlConnection(Db.Cnn_DB))
    //     {
    //         connection.Open();
    //         string query = @"SELECT ID, Nama FROM MasterTipeTitipan WHERE Disable = 0";
    //         using(SqlCommand command = new SqlCommand(query, connection))
    //         {
    //             command.CommandType = CommandType.Text;
    //             SqlDataAdapter adapter = new SqlDataAdapter(command);
    //             adapter.Fill(ds);
    //             DataTable dataTipeTitip = ds.Tables[0];                

    //             foreach (DataRow row in dataTipeTitip.Rows)
    //             {
    //                 list.Add(new DataTipeTitipan
    //                 {
    //                     ID = Convert.ToInt32(row["ID"]),
    //                     Nama = Convert.ToString(row["Nama"])
    //                 });
    //             }                
    //         }
    //         connection.Close();
    //     }
    //     return list;
        
    // }

    [WebMethod]
    
    public static List <IDocTitipan> getData(string kw, string dari, string sampai, int tipelokasi, int idlokasi, int status, int tipeTitipan)
    {        
        var list = new List<IDocTitipan>();
        DataSet ds = new DataSet();
        string query = @"SELECT * INTO #tempLocOutlet 
                        FROM 
                        (
                            select 1 as TipeLokasi, ID AS IDLokasi, Nama as NamaLokasi from LocOutlet
                            UNION
                            select 2 as TipeLokasi, ID AS IDLokasi, Nama as NamaLokasi from LocOutlet
                        )a

                        select TipeLokasi,IDLokasi into #tempAM 
                        FROM DataAreamanager_lokasi a 
                        JOIN DataAreaManager b on b.ID = a.IDForm
                        where b.IDUser = @IDUser
                        
                        delete a
                        from #TempLocOutlet a
                        WHERE a.TipeLokasi  in (select top 1 TipeLokasi from #TempAM)
                        AND A.IDLokasi NOT in (select IDLokasi from #TempAM)


                        select a.ID, a.Nomor, FORMAT(a.tgl, 'dddd, dd MMMM yyyy', 'id-ID') as Tgl,IIF(A.TipeLokasi =1, 'Outlet', 'Exhibition') as TipeLokasi, b.NamaLokasi as Cabang
                        , a.CustomerNama, a.SalesNama, ISNULL(ab.Nomor,ad.Nomor) as PLUTitipan, xa.Qty, ISNULL(AC.Nama, '-') as StatusTitipan, ISNULL(c.Nama, '-') as TipeTitipan
                        , ISNULL(FORMAT(tglPengembalian, 'dddd, dd MMMM yyyy', 'id-ID'),'-') as TanggalPengembalian
                        , ISNULL(OperatorPengembalian,'-') AS OperatorPengembalian, ISNULL(a.BuktiPengembalian, '-') AS BuktiPengembalian, A.StatusTitipan as StatusTitipanID
                        from DocTitipan a
                        JOIN #tempLocOutlet b on b.TipeLokasi = a.TipeLokasi and b.IDLokasi = a.IDLokasi
                        LEFT JOIN MasterTipeTitipan c on c.ID = a.IDMasterTipeTitipan
                        JOIN DocTitipan_Product aa on aa.IDForm=a.ID
                        LEFT JOIN StockProductDJCustomer ab on ab.ID = aa.IDProductTitipan
                        JOIN StatusTitipan AC ON AC.ID = a.StatusTitipan
                        LEFT JOIN StockProductDJ ad on ad.ID = aa.IDProduct
                        CROSS APPLY (
                            SELECT COUNT(*) AS Qty
                            FROM DocTitipan_Product ab2
                            WHERE ab2.IDForm = a.ID
                        ) xa
                        where (@Nomor = '' OR (a.Nomor LIKE '%'+ @Nomor +'%' OR a.CustomerNama LIKE '%'+ @Nomor +'%')) and (@TipeLokasi = -1 OR a.TipeLokasi = @TipeLokasi)
                        and (@IDLokasi <= 0 OR a.IDLokasi = @IDLokasi) and CAST(a.tgl AS DATE) BETWEEN CAST(@Start AS DATE) and CAST(@End AS DATE)
                        and (@StatusTitipan = -1 OR a.StatusTitipan = @StatusTitipan) and (@TipeTitipan = -1 or a.IDMasterTipeTitipan = @TipeTitipan)
                        and a.Draft = 0


                        drop table #tempLocOutlet
        ";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = kw });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = tipelokasi });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = idlokasi });
                command.Parameters.Add(new SqlParameter("@Start", SqlDbType.DateTime) { Value = Convert.ToDateTime(dari) });
                command.Parameters.Add(new SqlParameter("@End", SqlDbType.DateTime) { Value = Convert.ToDateTime(sampai) });
                command.Parameters.Add(new SqlParameter("@StatusTitipan", SqlDbType.Int) { Value = status });
                command.Parameters.Add(new SqlParameter("@TipeTitipan", SqlDbType.Int) { Value = tipeTitipan });
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
                DataTable dataDocTitipan = ds.Tables[0];    
                

                
                foreach (DataRow row in dataDocTitipan.Rows)
                {                   
                    list.Add(new IDocTitipan
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Nomor = Convert.ToString(row["Nomor"]),
                        Tgl = Convert.ToString(row["Tgl"]),                         
                        TipeLokasi = Convert.ToString(row["TipeLokasi"]),
                        Cabang = Convert.ToString(row["Cabang"]),
                        CustomerNama = Convert.ToString(row["CustomerNama"]),
                        SalesNama = Convert.ToString(row["SalesNama"]),
                        PLUTitipan = Convert.ToString(row["PLUTitipan"]),
                        Qty = Convert.ToInt32(row["Qty"]),
                        StatusTitipan = Convert.ToString(row["StatusTitipan"]),
                        TipeTitipan = Convert.ToString(row["TipeTitipan"]),
                        TglPengembalian = Convert.ToString(row["TanggalPengembalian"]),
                        OperatorPengembalian = Convert.ToString(row["OperatorPengembalian"]),
                        BuktiPengembalian = Convert.ToString(row["BuktiPengembalian"]),
                        StatusTitipanID = Convert.ToInt32(row["StatusTitipanID"])
                    });
                }                    
            }
        }            
            return list;
    }
    
    protected void exportTableNew(object sender, EventArgs e)
    {        
        Table table = new Table();

        // =========================
        // HEADER ROW 1
        // =========================       
        TableHeaderRow headerRow1 = new TableHeaderRow(); 
        headerRow1.CssClass = "header";

        headerRow1.Cells.Add(CreateHeaderCell("No.", 1, 1, "50px"));
        headerRow1.Cells.Add(CreateHeaderCell("Cabang", 1, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("Tanggal", 1, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("No. Referensi", 1, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Nama Customer", 1, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("Nama Sales", 1, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("No. PLU", 1, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("Qty", 1, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("Status", 1, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("Kategori Titipan", 1, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("Tanggal Pengembalian", 1, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Operator Pengembalian", 1, 1, "200px"));

        table.Rows.Add(headerRow1);

        DataSet ds = new DataSet();

        string query = @"SELECT * INTO #tempLocOutlet 
                        FROM 
                        (
                            select 1 as TipeLokasi, ID AS IDLokasi, Nama as NamaLokasi from LocOutlet
                            UNION
                            select 2 as TipeLokasi, ID AS IDLokasi, Nama as NamaLokasi from LocOutlet
                        )a

                        select TipeLokasi,IDLokasi into #tempAM 
                        FROM DataAreamanager_lokasi a 
                        JOIN DataAreaManager b on b.ID = a.IDForm
                        where b.IDUser = @IDUser
                        
                        delete a
                        from #TempLocOutlet a
                        WHERE a.TipeLokasi  in (select top 1 TipeLokasi from #TempAM)
                        AND A.IDLokasi NOT in (select IDLokasi from #TempAM)


                        select a.ID, a.Nomor, FORMAT(a.tgl, 'dddd, dd MMMM yyyy', 'id-ID') as Tgl,IIF(A.TipeLokasi =1, 'Outlet', 'Exhibition') as TipeLokasi, b.NamaLokasi as Cabang
                        , a.CustomerNama, a.SalesNama, ISNULL(ab.Nomor,ad.Nomor) as PLUTitipan, xa.Qty, ISNULL(AC.Nama, '-') as StatusTitipan, ISNULL(c.Nama, '-') as TipeTitipan
                        , ISNULL(FORMAT(tglPengembalian, 'dddd, dd MMMM yyyy', 'id-ID'),'-') as TanggalPengembalian
                        , ISNULL(OperatorPengembalian,'-') AS OperatorPengembalian, ISNULL(a.BuktiPengembalian, '-') AS BuktiPengembalian, A.StatusTitipan as StatusTitipanID
                        from DocTitipan a
                        JOIN #tempLocOutlet b on b.TipeLokasi = a.TipeLokasi and b.IDLokasi = a.IDLokasi
                        LEFT JOIN MasterTipeTitipan c on c.ID = a.IDMasterTipeTitipan
                        JOIN DocTitipan_Product aa on aa.IDForm=a.ID
                        LEFT JOIN StockProductDJCustomer ab on ab.ID = aa.IDProductTitipan
                        JOIN StatusTitipan AC ON AC.ID = a.StatusTitipan
                        LEFT JOIN StockProductDJ ad on ad.ID = aa.IDProduct
                        CROSS APPLY (
                            SELECT COUNT(*) AS Qty
                            FROM DocTitipan_Product ab2
                            WHERE ab2.IDForm = a.ID
                        ) xa
                        where (@Nomor = '' OR (a.Nomor LIKE '%'+ @Nomor +'%' OR a.CustomerNama LIKE '%'+ @Nomor +'%')) and (@TipeLokasi = -1 OR a.TipeLokasi = @TipeLokasi)
                        and (@IDLokasi <= 0 OR a.IDLokasi = @IDLokasi) and CAST(a.tgl AS DATE) BETWEEN CAST(@Start AS DATE) and CAST(@End AS DATE)
                        and (@StatusTitipan = -1 OR a.StatusTitipan = @StatusTitipan) and (@TipeTitipan = -1 or a.IDMasterTipeTitipan = @TipeTitipan)
                        and a.Draft = 0


                        drop table #tempLocOutlet
        ";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = kw.Text });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = tipelokasi.SelectedValue });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = lokasi.SelectedValue });
                command.Parameters.Add(new SqlParameter("@Start", SqlDbType.DateTime) { Value = Convert.ToDateTime(dari.Text) });
                command.Parameters.Add(new SqlParameter("@End", SqlDbType.DateTime) { Value = Convert.ToDateTime(sampai.Text) });
                command.Parameters.Add(new SqlParameter("@StatusTitipan", SqlDbType.Int) { Value = status.SelectedValue });
                command.Parameters.Add(new SqlParameter("@TipeTitipan", SqlDbType.Int) { Value = tipeTitipan.SelectedValue });
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
                DataTable dataDocTitipan = ds.Tables[0];                

                
                int no = 0;           

                foreach (DataRow row in dataDocTitipan.Rows)                
                {                   
                    no++;                                                                    
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = no.ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);
                    

                    td = new TableCell();
                    td.Text = row["Cabang"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Tgl"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Nomor"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["CustomerNama"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["SalesNama"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["PLUTitipan"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Qty"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["StatusTitipan"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["TipeTitipan"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["TanggalPengembalian"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["OperatorPengembalian"].ToString();
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);


                    table.Rows.Add(tr);
                }
                        
            }
        }
        

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=ReportTitipan.xls");
        Response.ContentType = "application/vnd.ms-excel";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        table.RenderControl(hw);
        Response.Write(sw.ToString());                        
        Response.End();
        
    }
    private  TableHeaderCell CreateHeaderCell(string text, int rowSpan, int colSpan, string width = null)
    {
        TableHeaderCell cell = new TableHeaderCell();
        cell.Text = text;
        cell.RowSpan = rowSpan;
        cell.ColumnSpan = colSpan;

        cell.Style["border"] = "1px solid black"; // Tambahkan border untuk setiap header cell
        if (!string.IsNullOrEmpty(width))
            cell.Width = Unit.Parse(width);   // contoh: "120px" atau "10%"

        return cell;
    }

    private TableRow CreateDataRow(params string[] values)
    {
        TableRow row = new TableRow();

        for (int i = 0; i < values.Length; i++)
        {
            string val = values[i];
            TableCell cell = new TableCell();
            cell.Text = val;

            if (i == 0)
            {
                cell.Style["text-align"] = "center";
            }
            else
            {
                decimal dummy;
                if (decimal.TryParse(
                    val.Replace(".", "")
                    .Replace(",", ".")
                    .Replace("$", "")
                    .Trim(),
                    out dummy))
                {
                    cell.Style["text-align"] = "right";
                }
            }

            row.Cells.Add(cell);
        }

        return row;
    }

    private TableCell CreateCell(string text, string width = null)
    {
        TableCell cell = new TableCell();
        cell.Text = text;

        if (!string.IsNullOrEmpty(width))
            cell.Width = Unit.Parse(width);

        decimal dummy;
        if (decimal.TryParse(
            text.Replace(".", "")
                .Replace(",", ".")
                .Replace("$", "")
                .Trim(),
            out dummy))
        {
            cell.CssClass = "text-right";
        }

        return cell;
    }
    
    public class IDocTitipan
    {
        public int ID { get; set; }
        public string Nomor { get; set; }
        public string Tgl { get; set; }
        public string TipeLokasi { get; set; }
        public string Cabang { get; set; }
        public string CustomerNama { get; set; }
        public string SalesNama { get; set; }
        public string PLUTitipan { get; set; }
        public int Qty { get; set; }
        public string StatusTitipan { get; set; }
        public string TipeTitipan { get; set; }
        public string TglPengembalian { get; set; }
        public string OperatorPengembalian { get; set; }
        public string BuktiPengembalian { get; set; }
        public int StatusTitipanID { get; set; }
    }
     public class DataTipeTitipan
    {
        public int ID { get; set; }
        public string Nama { get; set; }
    }
    public class Message
    {
        public int StatusCode { get; set; }
        public string MessageText { get; set; }
    }
    
    
}