using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Page_Account_Stone : System.Web.UI.Page
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
    string mode = "accounting"; // accounting, merchandise
    string action = "display"; // display, excel

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            //DLC
            Ddl.Parcel(parcel01,"parcel101");
            Ddl.Parcel(parcel02,"parcel102");
            Ddl.Parcel(parcel03,"parcel103");
            Ddl.Parcel(parcel04,"parcel104");
            Ddl.Supplier(supplier);
            tipelokasi.Items.Clear();
            Ddl.TipeLokasi(tipelokasi);

            int Tipe = -1;
            if (Request.QueryString["tipelokasi"] != null )
            {
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            }            
                Ddl.LocationList(lokasi, Tipe);

            //Generate
            fillFilter();

            // if (App.Refresh(this)) fillTable(sender, e);
            if (App.Refresh(this)) fillTable1(sender, e);
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
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
        string href = "Stone1A_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Stone1A_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&startROW=" + lpage;
        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            parcel01.SelectedValue = Request.QueryString["parcel01"];
            parcel02.SelectedValue = Request.QueryString["parcel02"];
            parcel03.SelectedValue = Request.QueryString["parcel03"];
            parcel04.SelectedValue = Request.QueryString["parcel04"];
            order.SelectedValue = Request.QueryString["order"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            supplier.SelectedValue = Request.QueryString["supplier"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
            startROW.InnerHtml = Request.QueryString["startROW"];            
        }
    }

    //Display Table
    protected void fillTable1(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        int parcel1 = Convert.ToInt32(parcel01.SelectedValue);
        int parcel2 = Convert.ToInt32(parcel02.SelectedValue);
        int parcel3 = Convert.ToInt32(parcel03.SelectedValue);
        int parcel4 = Convert.ToInt32(parcel04.SelectedValue);

        int pageNumber = Convert.ToInt32(Request.QueryString["startROW"]);
        int numberOfObjectsPerPage = 30;
        decimal TotalPage = 0;

        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHarga = 0m;

        TableHeaderRow htr = new TableHeaderRow();
        TableHeaderCell htd;

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
        htd.Text = "Total Butir";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Total Carat ";
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

        tb.Rows.Add(htr);

        //RESULT
        

        string o = (Request.QueryString["order"] == "Nama" ? "c.Nama" : Request.QueryString["order"] == "ID" ? "c.ID" : Request.QueryString["order"] == "Nomor" ? "b.Nomor" : Request.QueryString["order"] == "KodeParcel" ? "c.KodeParcel":Request.QueryString["order"]) + " " + Request.QueryString["orderby"];

        string query = @"select b.Nomor PLU,baa.nama nmItem,IIF(d.TipeLokasi=0,'Warehouse',IIF(d.TipeLokasi = 1,'Outlet','Exhibition')) tipelokasi
							,d.NamaLokasi,c.Nama,ca.nama nmShape,cc.nama nmColor,
                            cd.nama nmClarity,SUM(ISNULL(a.TotalButir,0)) TotalButir,SUM(ISNULL(a.TotalCarat,0))TotalCarat,ISNULL(a.HargaSatuan,0) HargaSatuan,ISNULL(HargaTotal,0) HargaTotal
                            from StockProductDJ_Stone1A a
                            JOIN StockProductDJ b on b.id=a.IDForm
                            JOIN StockProductDJ_CharProduct ba on ba.ID=b.ID
							JOIN CharProductItem baa on baa.id=ba.ProductItem
                            JOIN Stone1A c on c.ID=a.IDStone
                            JOIN StockActualDJ d on d.IDProduct=b.ID
                            JOIN Parcel101 ca on ca.id=c.Parcel01
                            JOIN Parcel102 cb on cb.id=c.Parcel02
                            JOIN Parcel103 cc on cc.id=c.Parcel03
                            JOIN Parcel104 cd on cd.id=c.Parcel04
                            where b.Draft=0
                            and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0
                            and ((@IDLokasi <= 0 and d.IDLokasi <> -1) or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                            and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                            and (@parcel4 = 0 or cd.id=@parcel4)                            
                            and (@kw = '' or c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%' or b.Nomor like '%'+@kw+'%' )
                            group by b.Nomor,c.id,c.nama,c.kodeparcel,ca.nama,cb.nama,cc.nama,cd.nama,d.NamaLokasi,a.HargaSatuan,a.HargaTotal,baa.nama,d.TipeLokasi
                            order by " + o + @"
                            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
                            
            string queryCount = @"select count(*) total
            FROM (
                    select b.Nomor PLU,baa.nama nmItem,IIF(d.TipeLokasi=0,'Warehouse',IIF(d.TipeLokasi = 1,'Outlet','Exhibition')) tipelokasi
					,d.NamaLokasi,c.Nama,ca.nama nmShape,cc.nama nmColor,
                  cd.nama nmClarity,SUM(ISNULL(a.TotalButir,0)) TotalButir,SUM(ISNULL(a.TotalCarat,0))TotalCarat,ISNULL(a.HargaSatuan,0) HargaSatuan,ISNULL(HargaTotal,0) HargaTotal
                    from StockProductDJ_Stone1A a
                    JOIN StockProductDJ b on b.id=a.IDForm
                    JOIN StockProductDJ_CharProduct ba on ba.ID=b.ID
					JOIN CharProductItem baa on baa.id=ba.ProductItem
                    JOIN Stone1A c on c.ID=a.IDStone
                    JOIN StockActualDJ d on d.IDProduct=b.ID
                    JOIN Parcel101 ca on ca.id=c.Parcel01
                    JOIN Parcel102 cb on cb.id=c.Parcel02
                    JOIN Parcel103 cc on cc.id=c.Parcel03
                    JOIN Parcel104 cd on cd.id=c.Parcel04
                    where b.Draft=0
                    and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0
                    and ((@IDLokasi <= 0 or d.IDLokasi = @IDLokasi) and d.IDLokasi <> -1) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                    and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                    and (@parcel4 = 0 or cd.id=@parcel4)                            
                    and (@kw = '' or c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%' or b.Nomor like '%'+@kw+'%' )
                    group by b.Nomor,c.id,c.nama,c.kodeparcel,ca.nama,cb.nama,cc.nama,cd.nama,d.NamaLokasi,a.HargaSatuan,a.HargaTotal,baa.nama,d.TipeLokasi
            )AX";

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {            
            int totalData = 0;
            using (SqlCommand command1 = new SqlCommand(queryCount, connection))
            {
                command1.CommandTimeout = 10;
                command1.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text });
                command1.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command1.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command1.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = parcel1 });
                command1.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = parcel2 });
                command1.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = parcel3 });
                command1.Parameters.Add(new SqlParameter("@parcel4", SqlDbType.Int) { Value = parcel4 });                
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
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = parcel1 });
                command.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = parcel2 });
                command.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = parcel3 });
                command.Parameters.Add(new SqlParameter("@parcel4", SqlDbType.Int) { Value = parcel4 });
                command.Parameters.Add(new SqlParameter("@Offset", SqlDbType.Int) { Value = (pageNumber - 1) * numberOfObjectsPerPage });
                command.Parameters.Add(new SqlParameter("@Limit", SqlDbType.Int) { Value = numberOfObjectsPerPage });
             

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                
                int no = (pageNumber > 0) ? ((pageNumber-1) * 30) + 1 : 1;
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
                        td.Text = row["nmItem"].ToString();
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

                // paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
                // paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
                // paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
                // paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
                // paging += "<a href=\"" + href4 + "\">Last</a>";
                // paging += "<br /> Page " + (Convert.ToInt32(startROW.InnerHtml) == 0 ? 1 : Convert.ToInt32(startROW.InnerHtml)) + " of " + TotalPage + " </div>";

                paging_text.Text = paging;
            }
        
        connection.Close();
        }
    }

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
        var rs = (from p in db.StockProductDJ_Stone1A
                  where !p.StockProductDJ.Draft.Value
                  select new
                  {
                      TipeLokasi = p.StockProductDJ.StockActualDJ.TipeLokasi,
                      IDLokasi = p.StockProductDJ.StockActualDJ.IDLokasi,
                      ID = p.Stone1A.ID,
                      Nama = p.Stone1A.Nama,
                      KodeParcel = p.Stone1A.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone1A.Parcel01,
                      Parcel02 = p.Stone1A.Parcel02,
                      Parcel03 = p.Stone1A.Parcel03,
                      Parcel04 = p.Stone1A.Parcel04,
                      Nama01 = p.Stone1A.Parcel101.Nama ?? string.Empty,
                      Nama02 = p.Stone1A.Parcel102.Nama ?? string.Empty,
                      Nama03 = p.Stone1A.Parcel103.Nama ?? string.Empty,
                      Nama04 = p.Stone1A.Parcel104.Nama ?? string.Empty,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      HargaSatuan = p.HargaSatuan ?? 0,
                      HargaTotal = p.HargaTotal ?? 0
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
        if (Supplier != "" && Supplier != "Supplier" && Supplier != "0")
            rs = rs.Where(p => p.StockProductDJ.SupplierNama.Equals(Supplier));

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(Request.QueryString["parcel01"]));
        if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(Request.QueryString["parcel02"]));
        if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(Request.QueryString["parcel03"]));
        if (Convert.ToInt32(Request.QueryString["parcel04"]) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(Request.QueryString["parcel04"]));

        // PAGING (FOR DISPLAY ONLY)
        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;
        decimal TotalPage = 0;

        if (action == "display")
        {
            if (Request.QueryString["page"] != "")
            {
                pageNumber = Convert.ToInt32(Request.QueryString["page"]);
                lpage.Text = pageNumber.ToString();
            }
            if (pageNumber < 0)
            {
                pageNumber = 1;
                lpage.Text = pageNumber.ToString();
            }
            TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

            TotalPage = Math.Ceiling(TotalPage);

            rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);
        }

        //NO-RESULT
        if (rs.Count() == 0) TableNoResult(tb);

        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHarga = 0m;

        TableHeaderRow htr = new TableHeaderRow();
        TableHeaderCell htd;

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
        htd.Text = "Total Butir";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Total Carat ";
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
            TotalHarga += r.HargaTotal;
        }

        TableRow tr2 = new TableRow();
        TableCell td2;

        td2 = new TableCell();
        td2.Text = "<b> Grand Total</b>";
        int cols = 9;
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
            td2.Text = "-";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = (action == "excel") ? TotalHarga.ToString("0.00").Replace(",", ".") : TotalHarga.ToString("N2");
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
        href += "&order=" + order.SelectedIndex;
        href += "&orderby=" + orderby.SelectedIndex;
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
        href2 += "&order=" + order.SelectedIndex;
        href2 += "&orderby=" + orderby.SelectedIndex;
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
        href3 += "&order=" + order.SelectedIndex;
        href3 += "&orderby=" + orderby.SelectedIndex;
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
        href4 += "&order=" + order.SelectedIndex;
        href4 += "&orderby=" + orderby.SelectedIndex;
        href4 += "&supplier=" + supplier.SelectedValue;
        href4 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href4 += "&lokasi=" + lokasi.SelectedValue;
        href4 += "&startROW=" + TotalPage;

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