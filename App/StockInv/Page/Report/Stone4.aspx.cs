using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Account_Stone : System.Web.UI.Page {
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
    string mode = "accounting"; // accounting, merchandise
    string action = "display"; // display, excel

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//DLC
			Ddl.Parcel(parcel01,"Parcel401");
			Ddl.Parcel(parcel02,"Parcel402");
			Ddl.Parcel(parcel03,"Parcel403");

            tipelokasi.Items.Clear();
            tipelokasi.Items.Add(new ListItem("All", "-1"));
            tipelokasi.Items.Add(new ListItem("Warehouse", "0"));
            tipelokasi.Items.Add(new ListItem("Outlet", "1"));
            tipelokasi.Items.Add(new ListItem("Exhibition", "2"));

            lokasi.Items.Clear();
            lokasi.Items.Add(new ListItem("All", "-1"));

            int Tipe = -1;
            if (Request.QueryString["tipelokasi"] != null)
            {
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            }
            DLC.LocationList(lokasi, Tipe);

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
		href += "&parcel01=" + parcel01.SelectedIndex;
		href += "&parcel02=" + parcel02.SelectedIndex;
		href += "&parcel03=" + parcel03.SelectedIndex;
		href += "&order=" + order.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
		href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&startROW=1";

		Response.Redirect(href);
	}
	
	protected void Export(object sender, EventArgs e)
    {
        string href = "Stone4_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }
	
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			parcel01.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel01"]);
			parcel02.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel02"]);
			parcel03.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel03"]);
			order.SelectedValue = Request.QueryString["order"];
            supplier.SelectedValue = Request.QueryString["supplier"];
			orderby.SelectedValue = Request.QueryString["orderby"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
		}
	}
	
	

	//Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        // INIT
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        string Supplier = Request.QueryString["supplier"];
        string keyword = Request.QueryString["kw"].ToString();
        int parcel01 = Convert.ToInt32(Request.QueryString["parcel01"]);
        int parcel02 = Convert.ToInt32(Request.QueryString["parcel02"]);
        int parcel03 = Convert.ToInt32(Request.QueryString["parcel03"]);
        char[] charsToTrim = { '*', ' ', '\''};

        //ORDER BY
        
        string o = (Request.QueryString["order"] == "Nama" ? "aaa.Nama" : Request.QueryString["order"] == "ID" ? "a.ID" :  Request.QueryString["order"] == "Nomor" ? "a.Nomor" : Request.QueryString["order"] == "KodeParcel" ? "ba.KodeParcel": Request.QueryString["order"] ) + " " + Request.QueryString["orderby"];
        string query=@"        
                    select a.Nomor as PLU, aaa.Nama as Item, 
                    IIF(C.TipeLokasi=0, 'Warehouse',IIF(C.TipeLokasi = 1,'Outlet','Exhibition')) TipeLokasi,
                    c.NamaLokasi, ba.Nama as Parcel, baa.Nama as Colour, bab.Nama as Shape, bac.Nama as Size,b.TotalButir,(b.TotalButir * b.CaratButir) carat,
                    b.HargaSatuan,b.HargaTotal
                    from StockProductDJ a 
                    JOIN StockProductDJ_CharProduct aa on aa.ID=a.ID
                    JOIN CharProductItem aaa on aaa.ID=aa.ProductItem
                    JOIN StockProductDJ_Stone4 b on b.IDForm=a.ID
                    JOIN StockActualDJ c on c.IDProduct=a.ID
                    JOIN Stone4 ba on ba.ID=b.IDStone
                    JOIN Parcel401 baa on baa.ID=ba.Parcel01
                    JOIN Parcel402 bab on bab.ID=ba.Parcel02
                    JOIN Parcel403 bac on bac.ID=ba.Parcel03
                    where a.StatusPenjualan=0 and a.StatusLebur=0 and a.ReturnProduct=0 and a.Draft=0
                    and (@Parcel01=0 or ba.Parcel01=@Parcel01) and (@Parcel02=0 or ba.Parcel02=@Parcel02)
                    and (@Parcel03=0 or ba.Parcel03=@Parcel03) and (@Supplier = 'Supplier' or a.SupplierNama=@Supplier)
                    and (@TipeLokasi = -1 or c.TipeLokasi = @TipeLokasi) and ((@IDLokasi <= 0 or c.IDLokasi=@IDLokasi) and c.IDLokasi <> -1)
                    and (@keyword = '' or ba.Nama like '%'+ @keyword +'%' or ba.KodeParcel like '%'+ @keyword +'%' or a.Nomor like '%'+ @keyword +'%')
                    ORDER BY "+ o +@"
                    OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY

                    select count(*) as TotalRow 
                    from StockProductDJ a 
                    JOIN StockProductDJ_CharProduct aa on aa.ID=a.ID
                    JOIN CharProductItem aaa on aaa.ID=aa.ProductItem
                    JOIN StockProductDJ_Stone4 b on b.IDForm=a.ID
                    JOIN StockActualDJ c on c.IDProduct=a.ID
                    JOIN Stone4 ba on ba.ID=b.IDStone
                    JOIN Parcel401 baa on baa.ID=ba.Parcel01
                    JOIN Parcel402 bab on bab.ID=ba.Parcel02
                    JOIN Parcel403 bac on bac.ID=ba.Parcel03
                    where a.StatusPenjualan=0 and a.StatusLebur=0 and a.ReturnProduct=0 and a.Draft=0
                    and (@Parcel01=0 or ba.Parcel01=@Parcel01) and (@Parcel02=0 or ba.Parcel02=@Parcel02)
                    and (@Parcel03=0 or ba.Parcel03=@Parcel03) and (@Supplier = 'Supplier' or a.SupplierNama=@Supplier)
                    and (@TipeLokasi = -1 or c.TipeLokasi = @TipeLokasi) and ((@IDLokasi <= 0 or c.IDLokasi=@IDLokasi) and c.IDLokasi <> -1)
                    and (@Keyword = '' or ba.Nama like '%'+ @Keyword +'%' or ba.KodeParcel like '%'+ @Keyword +'%' or a.Nomor like '%'+ @Keyword +'%')
            
                ";                

        // PAGING (FOR DISPLAY ONLY)
        int pageNumber = Convert.ToInt32(Request.QueryString["startROW"]);
        int numberOfObjectsPerPage = 30;
        int totalData = 0;
        decimal TotalPage = 0;
        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHargaSatuan = 0m;
        decimal TotalHargaTotal = 0m;
        
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {            
            using (SqlCommand command = new SqlCommand(query, connection)){
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.VarChar) { Value = kw.Text.Trim(charsToTrim) });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@Parcel01", SqlDbType.Int) { Value = parcel01 });
                command.Parameters.Add(new SqlParameter("@Parcel02", SqlDbType.Int) { Value = parcel02 });
                command.Parameters.Add(new SqlParameter("@Parcel03", SqlDbType.Int) { Value = parcel03 });
                command.Parameters.Add(new SqlParameter("@Supplier", SqlDbType.VarChar) { Value = supplier.SelectedValue });
                command.Parameters.Add(new SqlParameter("@Offset", SqlDbType.Int) { Value = (pageNumber - 1) * numberOfObjectsPerPage });
                command.Parameters.Add(new SqlParameter("@Limit", SqlDbType.Int) { Value = numberOfObjectsPerPage });
                connection.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                

                DataTable data = ds1.Tables[0];
                DataTable totalRow = ds1.Tables[1];  

                if (totalRow.Rows.Count > 0 ) 
                {
                    totalData = Convert.ToInt32(totalRow.Rows[0]["TotalRow"]);
                }else{
                    TableNoResult(tb);
                }  

                if (action == "display")
                {
                    if (Request.QueryString["startROW"] != "")
                    {
                        pageNumber = Convert.ToInt32(Request.QueryString["startROW"]);                     
                    }
                    if (pageNumber <= 1)
                    {
                        pageNumber = 1;                            
                    }
                    TotalPage = Convert.ToDecimal(totalData) / Convert.ToDecimal(numberOfObjectsPerPage);
                    TotalPage = Math.Ceiling(TotalPage);
                }       

                 //Build Header     
                TableHeaderRow htr = new TableHeaderRow();        
                TableHeaderCell htd = new TableHeaderCell();            
                tableBuilder(htr, htd, mode);        

                tb.Rows.Add(htr);


                int index = (pageNumber > 0) ? ((pageNumber-1) * 30) + 1 : 1;                        
                foreach (DataRow r in data.Rows)
                {
                    if (!Response.IsClientConnected) break;

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["PLU"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Item"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["TipeLokasi"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["NamaLokasi"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Parcel"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Shape"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Colour"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Size"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (action == "excel") ? Convert.ToDecimal(r["TotalButir"]).ToString("0").Replace(",", ".") : Convert.ToDecimal(r["TotalButir"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (action == "excel") ? Convert.ToDecimal(r["carat"]).ToString("0.000").Replace(",", ".") : Convert.ToDecimal(r["carat"]).ToString("N3");
                    tr.Cells.Add(td);

                    if (mode == "accounting")
                    {
                        td = new TableCell();
                        td.Text = (action == "excel") ? Convert.ToDecimal(r["HargaSatuan"]).ToString("0.00").Replace(",", ".") : Convert.ToDecimal(r["HargaSatuan"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (action == "excel") ? Convert.ToDecimal(r["HargaTotal"]).ToString("0.00").Replace(",", ".") : Convert.ToDecimal(r["HargaTotal"]).ToString("N2");
                        tr.Cells.Add(td);
                    }

                    tb.Rows.Add(tr);

                    TotalButir += Convert.ToDecimal(r["TotalButir"]);
                    TotalCarat += Convert.ToDecimal(r["carat"]);
                    TotalHargaSatuan += Convert.ToDecimal(r["HargaSatuan"]);
                    TotalHargaTotal += Convert.ToDecimal(r["HargaTotal"]);
                }

            }
        }
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01;
        href += "&parcel02=" + parcel02;
        href += "&parcel03=" + parcel03;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&startRow=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&parcel01=" + parcel01;
        href2 += "&parcel02=" + parcel02;
        href2 += "&parcel03=" + parcel03;
        href2 += "&order=" + order.SelectedValue;
        href2 += "&orderby=" + orderby.SelectedValue;
        href2 += "&supplier=" + supplier.SelectedValue;
        href2 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href2 += "&lokasi=" + lokasi.SelectedValue;
        href2 += "&startRow=" + (pageNumber - 1).ToString();

        string href3 = App.RefreshURL(this);

        //Customize-Qstr
        href3 += "&kw=" + kw.Text;
        href3 += "&parcel01=" + parcel01;
        href3 += "&parcel02=" + parcel02;
        href3 += "&parcel03=" + parcel03;
        href3 += "&order=" + order.SelectedValue;
        href3 += "&orderby=" + orderby.SelectedValue;
        href3 += "&supplier=" + supplier.SelectedValue;
        href3 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href3 += "&lokasi=" + lokasi.SelectedValue;
        href3 += "&startRow=1";

        string href4 = App.RefreshURL(this);

        //Customize-Qstr
        href4 += "&kw=" + kw.Text;
        href4 += "&parcel01=" + parcel01;
        href4 += "&parcel02=" + parcel02;
        href4 += "&parcel03=" + parcel03;
        href4 += "&order=" + order.SelectedValue;
        href4 += "&orderby=" + orderby.SelectedValue;
        href4 += "&supplier=" + supplier.SelectedValue;
        href4 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href4 += "&lokasi=" + lokasi.SelectedValue;
        href4 += "&startRow=" + TotalPage;

        string paging = "";
                                                
        string disablefirst = (pageNumber == 1) ? "style='color: gray; cursor: not-allowed;' onclick='return false;'" : "href='" + href3 + "' ";
        string disableprev = (pageNumber == 1) ? "style='color: gray; cursor: not-allowed;' onclick='return false;'" : "href='" + href2 + "' ";
        string disablenext = (pageNumber == TotalPage) ? "style='color: gray; cursor: not-allowed;' onclick='return false;'" : "href='" + href + "' ";
        string disablelast = (pageNumber == TotalPage) ? "style='color: gray; cursor: not-allowed;' onclick='return false;'" : "href='" + href4 + "' ";
        
        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a " + disablefirst + ">First</a>&nbsp;&nbsp;";
        paging += "<a " + disableprev + ">Prev</a>&nbsp;&nbsp;";
        paging += "<a " + disablenext + " >Next</a>&nbsp;&nbsp;";
        paging += "<a " + disablelast +">Last</a>";
        paging += "<br /> Page " + (Convert.ToInt32(Request.QueryString["startROW"]) == 0 ? 1 : Convert.ToInt32(Request.QueryString["startROW"])) + " of " + TotalPage + " </div>";

        paging_text.Text = paging;                                        
    }
    
    // protected void fillTable(object sender, EventArgs e)
    // {
    //     /// Master Brand
    //     var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

    //     /// Master Lokasi LocWarehouse
    //     var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

    //     /// Master Lokasi LocOutlet
    //     var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

    //     /// Master Lokasi LocExhibition
    //     var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

    //     // INIT
    //     int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
    //     int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
    //     string keyword = Request.QueryString["kw"].ToString();

    //     //ORDER BY
    //     string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];

    //     var rs = (from p in db.StockProductDJ_Stone4
    //               where !p.StockProductDJ.Draft.Value
    //               && p.StockProductDJ.StockActualDJ.IDLokasi != -1
                  
    //               select new
    //               {
    //                   TipeLokasi = p.StockProductDJ.StockActualDJ.TipeLokasi,
    //                   IDLokasi = p.StockProductDJ.StockActualDJ.IDLokasi,
    //                   ID = p.Stone4.ID,
    //                   Nama = p.Stone4.Nama,
    //                   KodeParcel = p.Stone4.KodeParcel,
    //                   TotalButir = p.TotalButir,
    //                   TotalCarat = p.TotalCarat,
    //                   Nomor = p.StockProductDJ.Nomor,
    //                   Parcel01 = p.Stone4.Parcel01,
    //                   Parcel02 = p.Stone4.Parcel02,
    //                   Parcel03 = p.Stone4.Parcel03,
    //                   Nama01 = p.Stone4.Parcel401.Nama,
    //                   Nama02 = p.Stone4.Parcel402.Nama,
    //                   Nama03 = p.Stone4.Parcel403.Nama,
    //                   StockProductDJ = p.StockProductDJ,
    //                   StockActualDJ = p.StockProductDJ.StockActualDJ,
    //                   HargaSatuan = p.HargaSatuan.Value,
    //                   HargaTotal = p.HargaTotal.Value
    //               });
    //     //.OrderBy(o);
    //     if (keyword != "")
    //     {
    //         rs = rs.Where(z => z.Nama.Contains(keyword)
    //                    || z.KodeParcel.Contains(keyword)
    //                    || z.Nomor.Contains(keyword)
    //                    );
    //     }

    //     if (Tipe != -1)
    //         rs = rs.Where(p => p.TipeLokasi == Tipe);
    //     if (IDLokasi != -1)
    //         rs = rs.Where(p => p.IDLokasi == IDLokasi && p.TipeLokasi == Tipe);

    //     string Supplier = Request.QueryString["supplier"];
    //     if (Supplier != "" && Supplier != "Supplier")
    //         rs = rs.Where(p => p.StockProductDJ.SupplierNama.Equals(Supplier));

    //     rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

    //     if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(Request.QueryString["parcel01"]));
    //     if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(Request.QueryString["parcel02"]));
    //     if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(Request.QueryString["parcel03"]));

    //     // PAGING (FOR DISPLAY ONLY)
    //     int pageNumber = 1;
    //     int numberOfObjectsPerPage = 30;
    //     decimal TotalPage = 0;

    //     if (action == "display")
    //     {
    //         if (Request.QueryString["page"] != "")
    //             pageNumber = Convert.ToInt32(Request.QueryString["page"]);
    //         if (pageNumber < 0)
    //             pageNumber = 1;

    //         TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

    //         TotalPage = Math.Ceiling(TotalPage);

    //         rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);
    //     }

    //     //NO-RESULT
    //     if (rs.Count() == 0) TableNoResult(tb);

    //     decimal TotalButir = 0m;
    //     decimal TotalCarat = 0m;
    //     decimal TotalHargaSatuan = 0m;
    //     decimal TotalHargaTotal = 0m;

    //     TableHeaderRow htr = new TableHeaderRow();
    //     TableHeaderCell htd;

    //     htd = new TableHeaderCell();
    //     htd.Text = "No.";
    //     htd.Width = 50;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "PLU";
    //     htd.Width = 100;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "Product Item";
    //     htd.Width = 100;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "Tipe Lokasi";
    //     htd.Width = 100;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "Lokasi";
    //     htd.Width = 150;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "Nama";
    //     htd.Width = 100;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "Shape";
    //     htd.Width = 150;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "Colour";
    //     htd.Width = 150;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "Butir";
    //     htd.Width = 100;
    //     htr.Cells.Add(htd);

    //     htd = new TableHeaderCell();
    //     htd.Text = "Carat";
    //     htd.Width = 100;
    //     htr.Cells.Add(htd);

    //     if (mode == "accounting")
    //     {
    //         htd = new TableHeaderCell();
    //         htd.Text = "Harga Per Karat USD";
    //         htd.Width = 100;
    //         htr.Cells.Add(htd);

    //         htd = new TableHeaderCell();
    //         htd.Text = "Total Harga USD";
    //         htd.Width = 100;
    //         htr.Cells.Add(htd);
    //     }

    //     tb.Rows.Add(htr);

    //     //RESULT
    //     int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
    //     foreach (var r in rs)
    //     {
    //         if (!Response.IsClientConnected) break;

    //         TableRow tr = new TableRow();
    //         TableCell td;

    //         td = new TableCell();
    //         td.Text = "<p>" + index++.ToString() + ".</p>";
    //         tr.Cells.Add(td);

    //         td = new TableCell();
    //         td.Text = r.Nomor;
    //         tr.Cells.Add(td);

    //         td = new TableCell();
    //         td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
    //         tr.Cells.Add(td);

    //         td = new TableCell();
    //         td.Text = DL.LocationType(r.StockActualDJ.TipeLokasi);
    //         tr.Cells.Add(td);

    //         string Lokasi = "";

    //         if (r.StockActualDJ.TipeLokasi == 0)
    //             Lokasi = rsWarehouse[r.StockActualDJ.IDLokasi];
    //         else if (r.StockActualDJ.TipeLokasi == 1)
    //             Lokasi = rsOutlet[r.StockActualDJ.IDLokasi];
    //         else if (r.StockActualDJ.TipeLokasi == 2)
    //             Lokasi = rsExhibition[r.StockActualDJ.IDLokasi];

    //         td = new TableCell();
    //         td.Text = Lokasi;
    //         tr.Cells.Add(td);

    //         td = new TableCell();
    //         td.Text = r.Nama;
    //         tr.Cells.Add(td);

    //         td = new TableCell();
    //         td.Text = r.Nama01;
    //         tr.Cells.Add(td);

    //         td = new TableCell();
    //         td.Text = r.Nama03;
    //         tr.Cells.Add(td);

    //         td = new TableCell();
    //         td.Text = (action == "excel") ? r.TotalButir.ToString("0").Replace(",", ".") : r.TotalButir.ToString("N0");
    //         tr.Cells.Add(td);

    //         td = new TableCell();
    //         td.Text = (action == "excel") ? r.TotalCarat.ToString("0.000").Replace(",", ".") : r.TotalCarat.ToString("N3");
    //         tr.Cells.Add(td);

    //         if (mode == "accounting")
    //         {
    //             td = new TableCell();
    //             td.Text = (action == "excel") ? r.HargaSatuan.ToString("0.00").Replace(",", ".") : r.HargaSatuan.ToString("N2");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = (action == "excel") ? r.HargaTotal.ToString("0.00").Replace(",", ".") : r.HargaTotal.ToString("N2");
    //             tr.Cells.Add(td);
    //         }

    //         tb.Rows.Add(tr);

    //         TotalButir += r.TotalButir;
    //         TotalCarat += r.TotalCarat;
    //         TotalHargaSatuan += r.HargaSatuan;
    //         TotalHargaTotal += r.HargaTotal;
    //     }

    //     TableRow tr2 = new TableRow();
    //     TableCell td2;

    //     td2 = new TableCell();
    //     td2.Text = "<b>Total</b>";
    //     int cols = 8;
    //     td2.ColumnSpan = (mode == "accounting") ? cols : cols - 2;
    //     tr2.Cells.Add(td2);

    //     td2 = new TableCell();
    //     td2.Text = (action == "excel") ? TotalButir.ToString("0").Replace(",", ".") : TotalButir.ToString("N0");
    //     tr2.Cells.Add(td2);

    //     td2 = new TableCell();
    //     td2.Text = (action == "excel") ? TotalCarat.ToString("0.000").Replace(",", ".") : TotalCarat.ToString("N3");
    //     tr2.Cells.Add(td2);

    //     if (mode == "accounting")
    //     {

    //         td2 = new TableCell();
    //         td2.Text = (action == "excel") ? TotalHargaSatuan.ToString("0.00").Replace(",", ".") : TotalHargaSatuan.ToString("N2");
    //         tr2.Cells.Add(td2);
			
    //         td2 = new TableCell();
    //         td2.Text = (action == "excel") ? TotalHargaTotal.ToString("0.00").Replace(",", ".") : TotalHargaTotal.ToString("N2");
    //         tr2.Cells.Add(td2);
    //     }

    //     tb.Rows.Add(tr2);

    //     string href = App.RefreshURL(this);

    //     //Customize-Qstr
    //     href += "&kw=" + kw.Text;
    //     href += "&parcel01=" + parcel01.SelectedIndex;
    //     href += "&parcel02=" + parcel02.SelectedIndex;
    //     href += "&parcel03=" + parcel03.SelectedIndex;
    //     href += "&order=" + order.SelectedIndex;
    //     href += "&orderby=" + orderby.SelectedIndex;
    //     href += "&supplier=" + supplier.SelectedValue;
    //     href += "&tipelokasi=" + tipelokasi.SelectedValue;
    //     href += "&lokasi=" + lokasi.SelectedValue;
    //     href += "&page=" + (pageNumber + 1).ToString();

    //     string href2 = App.RefreshURL(this);

    //     //Customize-Qstr
    //     href2 += "&kw=" + kw.Text;
    //     href2 += "&parcel01=" + parcel01.SelectedIndex;
    //     href2 += "&parcel02=" + parcel02.SelectedIndex;
    //     href2 += "&parcel03=" + parcel03.SelectedIndex;
    //     href2 += "&order=" + order.SelectedIndex;
    //     href2 += "&orderby=" + orderby.SelectedIndex;
    //     href2 += "&supplier=" + supplier.SelectedValue;
    //     href2 += "&tipelokasi=" + tipelokasi.SelectedValue;
    //     href2 += "&lokasi=" + lokasi.SelectedValue;
    //     href2 += "&page=" + (pageNumber - 1).ToString();

    //     string href3 = App.RefreshURL(this);

    //     //Customize-Qstr
    //     href3 += "&kw=" + kw.Text;
    //     href3 += "&parcel01=" + parcel01.SelectedIndex;
    //     href3 += "&parcel02=" + parcel02.SelectedIndex;
    //     href3 += "&parcel03=" + parcel03.SelectedIndex;
    //     href3 += "&order=" + order.SelectedIndex;
    //     href3 += "&orderby=" + orderby.SelectedIndex;
    //     href3 += "&supplier=" + supplier.SelectedValue;
    //     href3 += "&tipelokasi=" + tipelokasi.SelectedValue;
    //     href3 += "&lokasi=" + lokasi.SelectedValue;
    //     href3 += "&page=1";

    //     string href4 = App.RefreshURL(this);

    //     //Customize-Qstr
    //     href4 += "&kw=" + kw.Text;
    //     href4 += "&parcel01=" + parcel01.SelectedIndex;
    //     href4 += "&parcel02=" + parcel02.SelectedIndex;
    //     href4 += "&parcel03=" + parcel03.SelectedIndex;
    //     href4 += "&order=" + order.SelectedIndex;
    //     href4 += "&orderby=" + orderby.SelectedIndex;
    //     href4 += "&supplier=" + supplier.SelectedValue;
    //     href4 += "&tipelokasi=" + tipelokasi.SelectedValue;
    //     href4 += "&lokasi=" + lokasi.SelectedValue;
    //     href4 += "&page=" + TotalPage;

    //     string paging = "";

    //     paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
    //     paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
    //     paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
    //     paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
    //     paging += "<a href=\"" + href4 + "\">Last</a>";
    //     paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

    //     paging_text.Text = paging;
    // }
    
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        lokasi.Items.Clear();
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        else { lokasi.Items.Add(new ListItem("All", "-1")); }
        lokasi.Focus();
    }
    protected static void TableNoResult(Table tb)
    {
        tb.Width = new Unit("100%");

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td.Text = "No Data";
        //td.ColumnSpan = TableColspan(tb);
        //td.Width = new Unit("100%");
        tr.Cells.Add(td);

        tr.CssClass = "no-result";
        tb.Rows.Add(tr);
    }

    protected void tableBuilder(TableHeaderRow htr, TableHeaderCell htd, string mode){
        htd = new TableHeaderCell();
        htd.Text = "No.";
        htd.Width = 50;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "PLU";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Product Item";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Tipe Lokasi";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Lokasi";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Nama";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Colour";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Size";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Butir";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Carat";
        htd.Width = 100;
        htr.Cells.Add(htd);

        if (mode == "accounting")
        {
            htd = new TableHeaderCell();
            htd.Text = "Harga Per Karat USD";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Total Harga USD";
            htd.Width = 100;
            htr.Cells.Add(htd);
        }
    }
}