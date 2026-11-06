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
			Ddl.Parcel(parcel01,"parcel101");
			Ddl.Parcel(parcel02,"parcel102");
			Ddl.Parcel(parcel03,"parcel103");
			Ddl.Parcel(parcel04,"parcel104");
            Ddl.Parcel(parcel05,"parcel105");
            Ddl.Parcel(parcel06,"parcel106");
            Ddl.Parcel(parcel07,"parcel107");

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
			if (App.Refresh(this)) fillTable1(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&kw=" + kw.Text;
		href += "&parcel01=" + parcel01.SelectedValue;
		href += "&parcel02=" + parcel02.SelectedValue;
		href += "&parcel03=" + parcel03.SelectedValue;
		href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
		href += "&order=" + order.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
		href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&startROW=" + "1";
        // href += "&startROW=" + startROW.InnerHtml;

		Response.Redirect(href);
	}
    protected void Export(object sender, EventArgs e)
    {
        string href = "Stone1B_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Stone1B_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
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
			parcel01.SelectedValue = Request.QueryString["parcel01"];
			parcel02.SelectedValue = Request.QueryString["parcel02"];
			parcel03.SelectedValue = Request.QueryString["parcel03"];
			parcel04.SelectedValue = Request.QueryString["parcel04"];
            parcel05.SelectedValue = Request.QueryString["parcel05"];
            parcel06.SelectedValue = Request.QueryString["parcel06"];
            parcel07.SelectedValue = Request.QueryString["parcel07"];
			order.SelectedValue = Request.QueryString["order"];
            supplier.SelectedValue = Request.QueryString["supplier"];
			orderby.SelectedValue = Request.QueryString["orderby"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
            startROW.InnerHtml = Request.QueryString["startROW"];            
		}
	}


    protected void fillTable1(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        int parcel1 = Convert.ToInt32(parcel01.SelectedValue);
        int parcel2 = Convert.ToInt32(parcel02.SelectedValue);
        int parcel3 = Convert.ToInt32(parcel03.SelectedValue);
        int parcel4 = Convert.ToInt32(parcel04.SelectedValue);
        int parcel5 = Convert.ToInt32(parcel05.SelectedValue);
        int parcel6 = Convert.ToInt32(parcel06.SelectedValue);
        int parcel7 = Convert.ToInt32(parcel07.SelectedValue);
        int Supplier = Convert.ToInt32(supplier.SelectedValue);        

        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;
        decimal TotalPage = 0;

       decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHargaSatuan = 0m;
        decimal TotalHargaTotal = 0m;

        TableHeaderRow htr = new TableHeaderRow();        
        TableHeaderCell htd = new TableHeaderCell();
        tableBuilder(htr, htd, mode);

        tb.Rows.Add(htr);
        
        Response.Write(tipelokasi.SelectedValue + " " + IDLokasi + " " + parcel1 + " " + parcel2 + " " + parcel3 + " " + parcel4 + " " + parcel5 + " " + parcel6 + " " + parcel7);

        //RESULT
        

        // string o = (Request.QueryString["order"] == "Nama" ? "c.Nama" : Request.QueryString["order"] == "ID" ? "c.ID" : Request.QueryString["order"]) + " " + Request.QueryString["orderby"];
        string o = (Request.QueryString["order"] == "Nama" ? "c.Nama" : Request.QueryString["order"] == "ID" ? "c.ID" : Request.QueryString["order"] == "Nomor" ? "b.Nomor" : Request.QueryString["order"] == "KodeParcel" ? "c.KodeParcel":Request.QueryString["order"]) + " " + Request.QueryString["orderby"];


        string query = @"	select b.Nomor as PLU, baa.Nama as ProductItem, IIF(d.TipeLokasi=0,'Warehouse',IIF(d.TipeLokasi = 1,'Outlet','Exhibition')) tipelokasi,
							D.NamaLokasi, c.Nama, ca.nama nmShape, cc.nama nmColor, cd.nama nmClarity, ce.Nama as nmCutting, cf.Nama as nmBrandSt,a.GIA,ISNULL(e.m_report,'') noSarine, 
							CONVERT(varchar,b.OperatorTgl,105) TanggalPenerimaan, DATEDIFF(day, b.OperatorTgl, GETDATE()) AS UsiaStock,
                            ISNULL(a.TotalButir,0) TotalButir,ISNULL(a.TotalCarat,0) TotalCarat,ISNULL(a.HargaSatuan,0) HargaSatuan,ISNULL(a.HargaTotal,0) HargaTotal
                            from StockProductDJ_Stone1B a
                            JOIN StockProductDJ b on b.id=a.IDForm
                            JOIN StockProductDJ_CharProduct ba on ba.ID=b.ID
							JOIN CharProductItem baa on baa.id=ba.ProductItem
                            JOIN Stone1B c on c.ID=a.IDStone
                            JOIN StockActualDJ d on d.IDProduct=b.ID
                            JOIN Parcel101 ca on ca.id=c.Parcel01                            
                            JOIN Parcel103 cc on cc.id=c.Parcel03
                            JOIN Parcel104 cd on cd.id=c.Parcel04
							JOIN Parcel105 ce on ce.id=c.Parcel05
							JOIN Parcel106 cf on cf.id=c.Parcel06							
							LEFT JOIN [DB_PROD_MYAPPS].dbcmk.dbo.table_sarin e on e.m_gia=a.GIA
                            where b.Draft=0 and a.StatusTerpasang=1 and d.IDLokasi <> -1
                            and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0
                            and (@IDLokasi < 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                            and (@Parcel1 = 0 or c.Parcel01=@Parcel1) and (@Parcel2 = 0 or c.Parcel02=@Parcel2) 
                            and (@Parcel3 = 0 or c.parcel03=@Parcel3) and (@Parcel4 = 0 or c.Parcel04=@Parcel4) 
                            and (@Parcel5 = 0 or c.Parcel05=@Parcel5) and (@Parcel6 = 0 or c.parcel06=@Parcel6)
							and (@Parcel7 = 0 or c.Parcel07=@Parcel7) and (@Supplier=0 or b.Supplier=@Supplier)
							and (@kw = '' or c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%' or b.Nomor like '%'+@kw+'%' )
                            order by " + o + @"
                            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
                            
        string queryCount = @"  select count(a.ID) total
                                from StockProductDJ_Stone1B a
                                JOIN StockProductDJ b on b.id=a.IDForm
                                JOIN StockProductDJ_CharProduct ba on ba.ID=b.ID
							    JOIN CharProductItem baa on baa.id=ba.ProductItem
                                JOIN Stone1B c on c.ID=a.IDStone
                                JOIN StockActualDJ d on d.IDProduct=b.ID
                                JOIN Parcel101 ca on ca.id=c.Parcel01                            
                                JOIN Parcel103 cc on cc.id=c.Parcel03
                                JOIN Parcel104 cd on cd.id=c.Parcel04
							    JOIN Parcel105 ce on ce.id=c.Parcel05
							    JOIN Parcel106 cf on cf.id=c.Parcel06							
							    LEFT JOIN [DB_PROD_MYAPPS].dbcmk.dbo.table_sarin e on e.m_gia=a.GIA
                                where b.Draft=0 and a.StatusTerpasang=1 and d.IDLokasi <> -1
                                and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0
                                and (@IDLokasi < 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                                and (@Parcel1 = 0 or c.Parcel01=@Parcel1) and (@Parcel2 = 0 or c.Parcel02=@Parcel2) 
                                and (@Parcel3 = 0 or c.parcel03=@Parcel3) and (@Parcel4 = 0 or c.Parcel04=@Parcel4) 
                                and (@Parcel5 = 0 or c.Parcel05=@Parcel5) and (@Parcel6 = 0 or c.parcel06=@Parcel6)
							    and (@Parcel7 = 0 or c.Parcel07=@Parcel7) and (@Supplier=0 or b.Supplier=@Supplier)
							    and (@kw = '' or c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%' or b.Nomor like '%'+@kw+'%' )";

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {            
            int totalData = 0;
            using (SqlCommand command1 = new SqlCommand(queryCount, connection))
            {
                command1.CommandTimeout = 180;
                command1.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text });
                command1.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command1.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command1.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = parcel1 });
                command1.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = parcel2 });
                command1.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = parcel3 });
                command1.Parameters.Add(new SqlParameter("@parcel4", SqlDbType.Int) { Value = parcel4 });                
                command1.Parameters.Add(new SqlParameter("@parcel5", SqlDbType.Int) { Value = parcel5 });                
                command1.Parameters.Add(new SqlParameter("@parcel6", SqlDbType.Int) { Value = parcel6 });                
                command1.Parameters.Add(new SqlParameter("@parcel7", SqlDbType.Int) { Value = parcel7 });                
                command1.Parameters.Add(new SqlParameter("@Supplier", SqlDbType.Int) { Value = Supplier });
                connection.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    totalData = Convert.ToInt32(dt1.Rows[0]["total"]);
                }
                connection.Close();
            }

            if (action == "display")
            {
                if (Request.QueryString["startRow"] != "")
                {
                    pageNumber = Convert.ToInt32(Request.QueryString["startRow"]);                            
                }
                if (pageNumber <= 1)
                {
                    pageNumber = 1;                            
                }
                TotalPage = Convert.ToDecimal(totalData) / Convert.ToDecimal(numberOfObjectsPerPage);
                TotalPage = Math.Ceiling(TotalPage);
            }
            

            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 180;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = parcel1 });
                command.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = parcel2 });
                command.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = parcel3 });
                command.Parameters.Add(new SqlParameter("@parcel4", SqlDbType.Int) { Value = parcel4 });                
                command.Parameters.Add(new SqlParameter("@parcel5", SqlDbType.Int) { Value = parcel5 });                
                command.Parameters.Add(new SqlParameter("@parcel6", SqlDbType.Int) { Value = parcel6 });                
                command.Parameters.Add(new SqlParameter("@parcel7", SqlDbType.Int) { Value = parcel7 });                
                command.Parameters.Add(new SqlParameter("@Supplier", SqlDbType.Int) { Value = Supplier });
                command.Parameters.Add(new SqlParameter("@Offset", SqlDbType.Int) { Value = (pageNumber - 1) * numberOfObjectsPerPage });
                command.Parameters.Add(new SqlParameter("@Limit", SqlDbType.Int) { Value = numberOfObjectsPerPage });                

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                
                int no  = (pageNumber <= 1 ) ? 1 : ((pageNumber * 30) - 30) + 1 ;                      
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        App.TableNoResult(tb);                                              
                    }                                        

                   
                    foreach (DataRow row in dt.Rows)
                    {                        

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = no++.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["PLU"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["ProductItem"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["tipelokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["NamaLokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Nama"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmShape"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmColor"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmClarity"].ToString();
                        tr.Cells.Add(td);
                        
                        td = new TableCell();
                        td.Text = row["nmCutting"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmBrandSt"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + row["GIA"] + "\" target=\"_blank\">" + row["GIA"] + "</a>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["noSarine"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["TanggalPenerimaan"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["UsiaStock"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["TotalButir"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["TotalCarat"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = ((decimal)row["HargaSatuan"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = ((decimal)row["HargaTotal"]).ToString("N2");
                        tr.Cells.Add(td);



                        tb.Rows.Add(tr);
                    }
                }                
                string href = App.RefreshURL(this);

                //Customize-Qstr
                href += "&kw=" + kw.Text;
                href += "&parcel01=" + parcel01.SelectedValue;
                href += "&parcel02=" + parcel02.SelectedValue;
                href += "&parcel03=" + parcel03.SelectedValue;
                href += "&parcel04=" + parcel04.SelectedValue;
                href += "&parcel05=" + parcel05.SelectedValue;
                href += "&parcel06=" + parcel06.SelectedValue;
                href += "&parcel07=" + parcel07.SelectedValue;
                href += "&order=" +  order.SelectedValue;
                href += "&orderby=" + orderby.SelectedValue;
                href += "&supplier=" + supplier.SelectedValue;
                href += "&tipelokasi=" + tipelokasi.SelectedValue;
                href += "&lokasi=" + lokasi.SelectedValue;
                href += "&startROW=" + (pageNumber + 1).ToString();

                string href2 = App.RefreshURL(this);

                //Customize-Qstr
                href2 += "&kw=" + kw.Text;
                href2 += "&parcel01=" + parcel01.SelectedValue;
                href2 += "&parcel02=" + parcel02.SelectedValue;
                href2 += "&parcel03=" + parcel03.SelectedValue;
                href2 += "&parcel04=" + parcel04.SelectedValue;
                href2 += "&parcel05=" + parcel05.SelectedValue;
                href2 += "&parcel06=" + parcel06.SelectedValue;
                href2 += "&parcel07=" + parcel07.SelectedValue;
                href2 += "&order=" +  order.SelectedValue;
                href2 += "&orderby=" + orderby.SelectedValue;
                href2 += "&supplier=" + supplier.SelectedValue;
                href2 += "&tipelokasi=" + tipelokasi.SelectedValue;
                href2 += "&lokasi=" + lokasi.SelectedValue;
                href2 += "&startROW=" + (pageNumber - 1).ToString();

                string href3 = App.RefreshURL(this);

                //Customize-Qstr
                href3 += "&kw=" + kw.Text;
                href3 += "&parcel01=" + parcel01.SelectedValue;
                href3 += "&parcel02=" + parcel02.SelectedValue;
                href3 += "&parcel03=" + parcel03.SelectedValue;
                href3 += "&parcel04=" + parcel04.SelectedValue;
                href3 += "&parcel05=" + parcel05.SelectedValue;
                href3 += "&parcel06=" + parcel06.SelectedValue;
                href3 += "&parcel07=" + parcel07.SelectedValue;
                href3 += "&order=" +  order.SelectedValue;
                href3 += "&orderby=" + orderby.SelectedValue;
                href3 += "&supplier=" + supplier.SelectedValue;
                href3 += "&tipelokasi=" + tipelokasi.SelectedValue;
                href3 += "&lokasi=" + lokasi.SelectedValue;
                href3 += "&startROW=1";

                string href4 = App.RefreshURL(this);

                //Customize-Qstr
                href4 += "&kw=" + kw.Text;
                href4 += "&parcel01=" + parcel01.SelectedValue;
                href4 += "&parcel02=" + parcel02.SelectedValue;
                href4 += "&parcel03=" + parcel03.SelectedValue;
                href4 += "&parcel04=" + parcel04.SelectedValue;
                href4 += "&parcel05=" + parcel05.SelectedValue;
                href4 += "&parcel06=" + parcel06.SelectedValue;
                href4 += "&parcel07=" + parcel07.SelectedValue;
                href4 += "&order=" +  order.SelectedValue;
                href4 += "&orderby=" + orderby.SelectedValue;
                href4 += "&supplier=" + supplier.SelectedValue;
                href4 += "&tipelokasi=" + tipelokasi.SelectedValue;
                href4 += "&lokasi=" + lokasi.SelectedValue;
                href4 += "&startROW=" + TotalPage;

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

                // paging += "<br /> Page " + (Convert.ToInt32(startROW.InnerHtml) == 0 ? 1 : Convert.ToInt32(startROW.InnerHtml)) + " of " + TotalPage + " </div>";

                paging_text.Text = paging;
            }
        
        connection.Close();
        }
    }

	//Display Table
    protected void fillTable(object sender, EventArgs e)    
    {
        /// Master Brand
        var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocWarehouse
        var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocOutlet
        var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocExhibition
        var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

        // INIT
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        string keyword = Request.QueryString["kw"].ToString();

        //ORDER BY
        string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];

        var rs = (from p in db.StockProductDJ_Stone1B
                  where p.StatusTerpasang == 1
                  && !p.StockProductDJ.Draft.Value
                  select new
                  {
                      TipeLokasi = p.StockProductDJ.StockActualDJ.TipeLokasi,
                      IDLokasi = p.StockProductDJ.StockActualDJ.IDLokasi,
                      ID = p.Stone1B.ID,
                      Nama = p.Stone1B.Nama,
                      KodeParcel = p.Stone1B.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone1B.Parcel01,
                      Parcel02 = p.Stone1B.Parcel02,
                      Parcel03 = p.Stone1B.Parcel03,
                      Parcel04 = p.Stone1B.Parcel04,
                      Parcel05 = p.Stone1B.Parcel05,
                      Parcel06 = p.Stone1B.Parcel06,
                      Parcel07 = p.Stone1B.Parcel07,
                      Nama01 = p.Stone1B.Parcel101.Nama,
                      Nama02 = p.Stone1B.Parcel102.Nama,
                      Nama03 = p.Stone1B.Parcel103.Nama,
                      Nama04 = p.Stone1B.Parcel104.Nama,
                      Nama05 = p.Stone1B.Parcel105.Nama,
                      Nama06 = p.Stone1B.Parcel106.Nama,
                      Nama07 = p.Stone1B.Parcel107.Nama,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      GIA = p.GIA,
                      HargaSatuan = p.HargaSatuan.Value,
                      HargaTotal = p.HargaTotal.Value,
                      NoSarin = Db.SingleString("select top 1 m_report from table_sarin where m_gia = '"+p.GIA+"' ", Db.Cnn_CS)
                  });
        //.OrderBy(o);
        if (keyword != "")
        {
            rs = rs.Where(z => z.Nama.Contains(keyword)
                       || z.KodeParcel.Contains(keyword)
                       || z.Nomor.Contains(keyword)
                       );
        }

        if (Tipe != -1)
            rs = rs.Where(p => p.TipeLokasi == Tipe);
        if (IDLokasi != -1)
            rs = rs.Where(p => p.IDLokasi == IDLokasi && p.TipeLokasi == Tipe);

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "" && Supplier != "Supplier" && Supplier!="0")
            rs = rs.Where(p => p.StockProductDJ.SupplierNama.Equals(Supplier));

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(Request.QueryString["parcel01"]));
        if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(Request.QueryString["parcel02"]));
        if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(Request.QueryString["parcel03"]));
        if (Convert.ToInt32(Request.QueryString["parcel04"]) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(Request.QueryString["parcel04"]));
        if (Convert.ToInt32(Request.QueryString["parcel05"]) > 0) rs = rs.Where(p => p.Parcel05 == Convert.ToInt32(Request.QueryString["parcel05"]));
        if (Convert.ToInt32(Request.QueryString["parcel06"]) > 0) rs = rs.Where(p => p.Parcel06 == Convert.ToInt32(Request.QueryString["parcel06"]));
        if (Convert.ToInt32(Request.QueryString["parcel07"]) > 0) rs = rs.Where(p => p.Parcel07 == Convert.ToInt32(Request.QueryString["parcel07"]));

        // PAGING (FOR DISPLAY ONLY)
        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;
        decimal TotalPage = 0;

        if (action == "display")
        {
            if (Request.QueryString["page"] != "")
                pageNumber = Convert.ToInt32(Request.QueryString["page"]);
            if (pageNumber < 0)
                pageNumber = 1;

            TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

            TotalPage = Math.Ceiling(TotalPage);

            rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);
        }

        //NO-RESULT
        if (rs.Count() == 0) TableNoResult(tb);

        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHargaSatuan = 0m;
        decimal TotalHargaTotal = 0m;

        TableHeaderRow htr = new TableHeaderRow();        
        TableHeaderCell htd = new TableHeaderCell();
        
        tableBuilder(htr, htd, mode);

        tb.Rows.Add(htr);

        //RESULT
        int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DL.LocationType(r.StockActualDJ.TipeLokasi);
            tr.Cells.Add(td);

            string Lokasi = "";

            if (r.StockActualDJ.TipeLokasi == 0)
                Lokasi = rsWarehouse[r.StockActualDJ.IDLokasi];
            else if (r.StockActualDJ.TipeLokasi == 1)
                Lokasi = rsOutlet[r.StockActualDJ.IDLokasi];
            else if (r.StockActualDJ.TipeLokasi == 2)
                Lokasi = rsExhibition[r.StockActualDJ.IDLokasi];

            td = new TableCell();
            td.Text = Lokasi;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama01;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama03;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama04;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama05;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama06;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + r.GIA + "\" target=\"_blank\">" + r.GIA + "</a>";
            tr.Cells.Add(td);
            
            td = new TableCell();
            td.Text = r.NoSarin;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ.OperatorTgl);
            tr.Cells.Add(td);

            double TotalDays = 0;
            TotalDays = (DateTime.Today - r.StockProductDJ.OperatorTgl.Value).TotalDays;

            td = new TableCell();
            td.Text = TotalDays.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (action == "excel") ? r.TotalButir.ToString("0").Replace(",", ".") : r.TotalButir.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (action == "excel") ? r.TotalCarat.ToString("0.000").Replace(",", ".") : r.TotalCarat.ToString("N3");
            tr.Cells.Add(td);

            if (mode == "accounting")
            {
                td = new TableCell();
                td.Text = (action == "excel") ? r.HargaSatuan.ToString("0.00").Replace(",", ".") : r.HargaSatuan.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (action == "excel") ? r.HargaTotal.ToString("0.00").Replace(",", ".") : r.HargaTotal.ToString("N2");
                tr.Cells.Add(td);
            }

            tb.Rows.Add(tr);

            TotalButir += r.TotalButir;
            TotalCarat += r.TotalCarat;
           
            TotalHargaTotal += r.HargaTotal;
        }

        TableRow tr2 = new TableRow();
        TableCell td2;

        td2 = new TableCell();
        td2.Text = "<b>Total</b>";
        int cols = 15;
        td2.ColumnSpan = (mode == "accounting") ? cols : cols - 2;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = (action == "excel") ? TotalButir.ToString("0").Replace(",", ".") : TotalButir.ToString("N0");
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = (action == "excel") ? TotalCarat.ToString("0.000").Replace(",", ".") : TotalCarat.ToString("N3");
        tr2.Cells.Add(td2);

        if (mode == "accounting")
        {

            td2 = new TableCell();
            td2.Text = (action == "excel") ? "---" : "---";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = (action == "excel") ? TotalHargaTotal.ToString("0.00").Replace(",", ".") : TotalHargaTotal.ToString("N2");
            tr2.Cells.Add(td2);
        }

        tb.Rows.Add(tr2);

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        href += "&page=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&parcel01=" + parcel01.SelectedValue;
        href2 += "&parcel02=" + parcel02.SelectedValue;
        href2 += "&parcel03=" + parcel03.SelectedValue;
        href2 += "&parcel04=" + parcel04.SelectedValue;
        href2 += "&parcel05=" + parcel05.SelectedValue;
        href2 += "&parcel06=" + parcel06.SelectedValue;
        href2 += "&parcel07=" + parcel07.SelectedValue;
        href2 += "&order=" + order.SelectedValue;
        href2 += "&orderby=" + orderby.SelectedValue;
        href2 += "&supplier=" + supplier.SelectedValue;
        href2 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href2 += "&lokasi=" + lokasi.SelectedValue;

        href2 += "&page=" + (pageNumber - 1).ToString();

        string href3 = App.RefreshURL(this);

        //Customize-Qstr
        href3 += "&kw=" + kw.Text;
        href3 += "&parcel01=" + parcel01.SelectedValue;
        href3 += "&parcel02=" + parcel02.SelectedValue;
        href3 += "&parcel03=" + parcel03.SelectedValue;
        href3 += "&parcel04=" + parcel04.SelectedValue;
        href3 += "&parcel05=" + parcel05.SelectedValue;
        href3 += "&parcel06=" + parcel06.SelectedValue;
        href3 += "&parcel07=" + parcel07.SelectedValue;
        href3 += "&order=" + order.SelectedValue;
        href3 += "&orderby=" + orderby.SelectedValue;
        href3 += "&supplier=" + supplier.SelectedValue;
        href3 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href3 += "&lokasi=" + lokasi.SelectedValue;

        href3 += "&page=1";

        string href4 = App.RefreshURL(this);

        //Customize-Qstr
        href4 += "&kw=" + kw.Text;
        href4 += "&parcel01=" + parcel01.SelectedValue;
        href4 += "&parcel02=" + parcel02.SelectedValue;
        href4 += "&parcel03=" + parcel03.SelectedValue;
        href4 += "&parcel04=" + parcel04.SelectedValue;
        href4 += "&parcel05=" + parcel05.SelectedValue;
        href4 += "&parcel06=" + parcel06.SelectedValue;
        href4 += "&parcel07=" + parcel07.SelectedValue;
        href4 += "&order=" + order.SelectedValue;
        href4 += "&orderby=" + orderby.SelectedValue;
        href4 += "&supplier=" + supplier.SelectedValue;
        href4 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href4 += "&lokasi=" + lokasi.SelectedValue;

        href4 += "&page=" + TotalPage;

        string paging = "";

        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href4 + "\">Last</a>";
        paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

        paging_text.Text = paging;
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        lokasi.Items.Clear();
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        else { lokasi.Items.Add(new ListItem("All", "-1")); }
        lokasi.Focus();
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
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Tipe Lokasi";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Lokasi";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Nama";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Colour";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Clarity";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Cutting";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Brand";
        htd.Width = 200;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "GIA";
        htd.Width = 100;
        htr.Cells.Add(htd);
        
        htd = new TableHeaderCell();
        htd.Text = "No.Sarine";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Tanggal Penerimaan";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Usia Stock";
        htd.Width = 100;
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
}