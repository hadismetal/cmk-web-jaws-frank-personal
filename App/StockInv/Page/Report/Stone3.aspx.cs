using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
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
			// DLC.Parcel301(parcel01);
			// DLC.Parcel302(parcel02);
			// DLC.Parcel303(parcel03);
			// DLC.Parcel304(parcel04);
            
			Ddl.Parcel(parcel01,"parcel301");
			Ddl.Parcel(parcel02,"parcel302");
			Ddl.Parcel(parcel03,"parcel303");
			Ddl.Parcel(parcel04,"parcel304");



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
		href += "&order=" + order.SelectedValue;        
		href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&startROW=1";

		Response.Redirect(href);
	}
	
	protected void Export(object sender, EventArgs e)
    {
        string href = "Stone3_Export.aspx?id=1";

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
	
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			parcel01.SelectedValue = Request.QueryString["parcel01"];
			parcel02.SelectedValue = Request.QueryString["parcel02"];
			parcel03.SelectedValue = Request.QueryString["parcel03"];
			parcel04.SelectedValue = Request.QueryString["parcel04"];
			order.SelectedValue = Request.QueryString["order"];            
			orderby.SelectedValue = Request.QueryString["orderby"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
            startROW.InnerHtml = Request.QueryString["startROW"];            
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

        var rs = (from p in db.StockProductDJ_Stone3
                  where !p.StockProductDJ.Draft.Value
                  && p.StockProductDJ.StockActualDJ.IDLokasi != -1
                  select new
                  {
                      TipeLokasi = p.StockProductDJ.StockActualDJ.TipeLokasi,
                      IDLokasi = p.StockProductDJ.StockActualDJ.IDLokasi,
                      ID = p.Stone3.ID,
                      Nama = p.Stone3.Nama,
                      KodeParcel = p.Stone3.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone3.Parcel01,
                      Parcel02 = p.Stone3.Parcel02,
                      Parcel03 = p.Stone3.Parcel03,
                      Parcel04 = p.Stone3.Parcel04,
                      Nama01 = p.Stone3.Parcel301.Nama,
                      Nama02 = p.Stone3.Parcel302.Nama,
                      Nama03 = p.Stone3.Parcel303.Nama,
                      Nama04 = p.Stone3.Parcel304.Nama,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      HargaSatuan = p.HargaSatuan.Value,
                      HargaTotal = p.HargaTotal.Value,
                      GIA = p.GIA
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
        if (Supplier != "" && Supplier != "Supplier")
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
        htd.Width = 150;
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
        htd.Width = 200;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Item";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Size";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Colour";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "GIA";
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
            td.Text = r.Nama02;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama03;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama04;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.GIA;
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
            TotalHargaSatuan += r.HargaSatuan;
            TotalHargaTotal += r.HargaTotal;
        }

        TableRow tr2 = new TableRow();
        TableCell td2;

        td2 = new TableCell();
        td2.Text = "<b>Total</b>";
        int cols = 11;
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
            td2.Text = (action == "excel") ? TotalHargaSatuan.ToString("0.00").Replace(",", ".") : TotalHargaSatuan.ToString("N2");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = (action == "excel") ? TotalHargaTotal.ToString("0.00").Replace(",", ".") : TotalHargaTotal.ToString("N2");
            tr2.Cells.Add(td2);
        }

        tb.Rows.Add(tr2);

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&order=" + order.SelectedIndex;
        href += "&orderby=" + orderby.SelectedIndex;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&page=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&parcel01=" + parcel01.SelectedIndex;
        href2 += "&parcel02=" + parcel02.SelectedIndex;
        href2 += "&parcel03=" + parcel03.SelectedIndex;
        href2 += "&parcel04=" + parcel04.SelectedIndex;
        href2 += "&order=" + order.SelectedIndex;
        href2 += "&orderby=" + orderby.SelectedIndex;
        href2 += "&supplier=" + supplier.SelectedValue;
        href2 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href2 += "&lokasi=" + lokasi.SelectedValue;
        href2 += "&page=" + (pageNumber - 1).ToString();

        string href3 = App.RefreshURL(this);

        //Customize-Qstr
        href3 += "&kw=" + kw.Text;
        href3 += "&parcel01=" + parcel01.SelectedIndex;
        href3 += "&parcel02=" + parcel02.SelectedIndex;
        href3 += "&parcel03=" + parcel03.SelectedIndex;
        href3 += "&parcel04=" + parcel04.SelectedIndex;
        href3 += "&order=" + order.SelectedIndex;
        href3 += "&orderby=" + orderby.SelectedIndex;
        href3 += "&supplier=" + supplier.SelectedValue;
        href3 += "&tipelokasi=" + tipelokasi.SelectedValue;
        href3 += "&lokasi=" + lokasi.SelectedValue;
        href3 += "&page=1";

        string href4 = App.RefreshURL(this);

        //Customize-Qstr
        href4 += "&kw=" + kw.Text;
        href4 += "&parcel01=" + parcel01.SelectedIndex;
        href4 += "&parcel02=" + parcel02.SelectedIndex;
        href4 += "&parcel03=" + parcel03.SelectedIndex;
        href4 += "&parcel04=" + parcel04.SelectedIndex;
        href4 += "&order=" + order.SelectedIndex;
        href4 += "&orderby=" + orderby.SelectedIndex;
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
    
    protected void fillTable1(object sender, EventArgs e)
    {        
        // INIT
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        string keyword = kw.Text;
        string Parcels01 = parcel01.SelectedValue;
        string Parcels02 = parcel02.SelectedValue;;
        string Parcels03 = parcel03.SelectedValue;
        string Parcels04 = parcel04.SelectedValue;   

        Response.Write(parcel04.SelectedValue+" - "+ parcel04.SelectedIndex);      

        //ORDER BY
        string orderan = order.SelectedValue=="PLU" ? "a." : "ba.";
        string o = orderan+order.SelectedValue + " " + orderby.SelectedValue;
                     
        string startRowValue = startROW.InnerHtml;        
        int pageStart = int.Parse(startRowValue);                

        int pageSize=10;   
        int OFFSET = (pageStart <= 0?1:pageStart -1) * pageSize;        

        string query = @"select a.Nomor as PLU, aaa.Nama as ProductItem,IIF(C.TipeLokasi=0,'Warehouse',IIF(c.TipeLokasi = 1,'Outlet','Exhibition')) TipeLokasi
                        ,c.NamaLokasi,ba.ID as IDStone, ba.Nama as Parcel,
                         ISNULL(b.TotalButir,0) TotalButir,ISNULL(b.TotalCarat,0) TotalCarat,
                         baa.Nama as Item,bab.Nama as shape,bac.Nama as size,bad.Nama as colour
                         ,isnull(b.HargaSatuan,0) HargaSatuan,isnull(b.HargaTotal,0) HargaTotal,b.GIA
                         from StockProductDJ a
                         JOIN StockProductDJ_stone3 b on b.IDForm=a.id
                         JOIN StockActualDJ c on c.IDProduct=a.id                        
                         JOIN StockProductDJ_CharProduct aa on aa.ID=a.ID
                         JOIN CharProductItem aaa on aaa.ID=aa.ProductItem
                         JOIN Stone3 ba on ba.ID=b.IDStone
                         JOIN Parcel301 baa on ba.Parcel01 = baa.ID
                         JOIN Parcel302 bab on ba.Parcel02 = bab.ID
                         JOIN Parcel303 bac on ba.Parcel03 = bac.ID
                         JOIN Parcel304 bad on ba.Parcel04 = bad.ID                         
                         where a.Draft=0 and a.statuslebur=0 and a.ReturnProduct=0 and a.StatusPenjualan=0
                         and (@keyword = '' or (ba.Nama like '%'+@keyword+'%' OR ba.KodeParcel like '%'+@keyword+'%' OR a.Nomor like '%'+@keyword+'%'))
                         and (@tipelokasi < 0 or c.TipeLokasi=@tipelokasi) and ((@lokasi <= 0 or c.IDLokasi=@lokasi) and c.IDLokasi <> -1)
                         and /* Parcel01/Item */ (@parcel1=0 or baa.ID=@parcel1) and /* Parcel02/Shape */ (@parcel2=0 or bab.ID=@parcel2) 
                         and /* Parcel03/Size */ (@parcel3=0 or bac.ID=@parcel3) and /* Parcel04/Colour */ (@parcel4=0 or bad.ID=@parcel4)
                         order by "+o+@"
                         OFFSET @offset ROWS
                         FETCH NEXT @pagesize ROWS ONLY 
                         
                         select count(*) totalPages 
                         FROM (
                        select a.Nomor as PLU, aaa.Nama as ProductItem,IIF(C.TipeLokasi=0,'Warehouse',IIF(c.TipeLokasi = 1,'Outlet','Exhibition')) TipeLokasi
                        ,c.NamaLokasi,ba.ID as IDStone, ba.Nama as Parcel,
                         ISNULL(b.TotalButir,0) TotalButir,ISNULL(b.TotalCarat,0) TotalCarat,
                         baa.Nama as Item,bab.Nama as shape,bac.Nama as size,bad.Nama as colour
                         ,isnull(b.HargaSatuan,0) HargaSatuan,isnull(b.HargaTotal,0) HargaTotal,b.GIA
                         from StockProductDJ a
                         JOIN StockProductDJ_stone3 b on b.IDForm=a.id
                         JOIN StockActualDJ c on c.IDProduct=a.id                        
                         JOIN StockProductDJ_CharProduct aa on aa.ID=a.ID
                         JOIN CharProductItem aaa on aaa.ID=aa.ProductItem
                         JOIN Stone3 ba on ba.ID=b.IDStone
                         JOIN Parcel301 baa on ba.Parcel01 = baa.ID
                         JOIN Parcel302 bab on ba.Parcel02 = bab.ID
                         JOIN Parcel303 bac on ba.Parcel03 = bac.ID
                         JOIN Parcel304 bad on ba.Parcel04 = bad.ID                         
                         where a.Draft=0 and a.statuslebur=0 and a.ReturnProduct=0 and a.StatusPenjualan=0
                         and (@keyword = '' or (ba.Nama like '%'+@keyword+'%' OR ba.KodeParcel like '%'+@keyword+'%' OR a.Nomor like '%'+@keyword+'%'))
                         and (@tipelokasi < 0 or c.TipeLokasi=@tipelokasi) and ((@lokasi <= 0 or c.IDLokasi=@lokasi) and c.IDLokasi <> -1)
                         and /* Parcel01/Item */ (@parcel1=0 or baa.ID=@parcel1) and /* Parcel02/Shape */ (@parcel2=0 or bab.ID=@parcel2) 
                         and /* Parcel03/Size */ (@parcel3=0 or bac.ID=@parcel3) and /* Parcel04/Colour */ (@parcel4=0 or bad.ID=@parcel4)
                         )ax                         
                         ";     
                        //  Response.Write(Tipe+" "+IDLokasi+"/"+keyword+"/"+Parcels01+" "+Parcels02+" "+Parcels03+" "+Parcels04+" "+o);                                                 
                         
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))        
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 10;
                command.Parameters.AddWithValue("@keyword", keyword);
                command.Parameters.AddWithValue("@tipelokasi", Tipe);
                command.Parameters.AddWithValue("@lokasi", IDLokasi);
                command.Parameters.AddWithValue("@parcel1", Parcels01);
                command.Parameters.AddWithValue("@parcel2", Parcels02);
                command.Parameters.AddWithValue("@parcel3", Parcels03);
                command.Parameters.AddWithValue("@parcel4", Parcels04);                
                command.Parameters.AddWithValue("@offset", OFFSET);                
                command.Parameters.AddWithValue("@pagesize", pageSize);             

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int totalData = Convert.ToInt32(ds.Tables[1].Rows[0]["totalPages"].ToString());                                

                BindData(pageStart,pageSize,totalData);  //Buat pagination
                    
                //Buat table header
                TableHeaderRow htr = new TableHeaderRow();        
                TableHeaderCell htd = new TableHeaderCell();            
                tableBuilder(htr, htd, mode);                            
                tb.Rows.Add(htr);                
                
                if (totalData > 0){
                    DataTable datas = ds.Tables[0];

                    int no = 0;                                             
                    pageStart = pageStart <= 0 ? 1 : pageStart; 
                    var counter = ((pageStart-1) * pageSize) + 1;        
                    decimal TotalButir = 0m;                    
                    decimal TotalCarat = 0m;
                    decimal TotalHargaSatuan = 0m;
                    decimal TotalHargaTotal = 0m;

                    
                    foreach (DataRow r in datas.Rows)
                    {   
                        if (!Response.IsClientConnected) break;

                        TableRow tr = new TableRow();
                        TableCell td;                        

                        td = new TableCell();
                        td.Text = "<p>" + counter.ToString() + ".</p>";
                        tr.Cells.Add(td);
                        

                        td = new TableCell();
                        td.Text = r["PLU"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["ProductItem"].ToString();
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
                        td.Text = r["Item"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["shape"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["size"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["colour"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["GIA"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (action == "excel") ? Convert.ToDecimal(r["TotalButir"]).ToString("0").Replace(",", ".") : Convert.ToDecimal(r["TotalButir"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (action == "excel") ? Convert.ToDecimal(r["TotalCarat"]).ToString("0.000").Replace(",", ".") : Convert.ToDecimal(r["TotalCarat"]).ToString("N3");
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
                        TotalCarat += Convert.ToDecimal(r["TotalCarat"]);
                        TotalHargaSatuan += Convert.ToDecimal(r["HargaSatuan"]);
                        TotalHargaTotal += Convert.ToDecimal(r["HargaTotal"]);
                        counter++;
                    }
                }else{
                    TableNoResult(tb);                   
                }
            
            }
            
        }    
        

        // PAGING (FOR DISPLAY ONLY)
        
             

        // if (action == "display")
        // {
        //     if (Request.QueryString["page"] != "")
        //         pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        //     if (pageNumber < 0)
        //         pageNumber = 1;

        //     TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

        //     TotalPage = Math.Ceiling(TotalPage);

        //     rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);
        // }

        //NO-RESULT
        // if (rs.Count() == 0) TableNoResult(tb);

        // decimal TotalButir = 0m;
        // decimal TotalCarat = 0m;
        // decimal TotalHargaSatuan = 0m;
        // decimal TotalHargaTotal = 0m;

        // TableHeaderRow htr = new TableHeaderRow();
        // TableHeaderCell htd;

        // htd = new TableHeaderCell();
        // htd.Text = "No.";
        // htd.Width = 50;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "PLU";
        // htd.Width = 100;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Product Item";
        // htd.Width = 150;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Tipe Lokasi";
        // htd.Width = 100;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Lokasi";
        // htd.Width = 150;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Nama";
        // htd.Width = 200;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Item";
        // htd.Width = 100;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Shape";
        // htd.Width = 100;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Size";
        // htd.Width = 150;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Colour";
        // htd.Width = 100;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "GIA";
        // htd.Width = 100;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Butir";
        // htd.Width = 100;
        // htr.Cells.Add(htd);

        // htd = new TableHeaderCell();
        // htd.Text = "Carat";
        // htd.Width = 100;
        // htr.Cells.Add(htd);

        // if (mode == "accounting")
        // {
        //     htd = new TableHeaderCell();
        //     htd.Text = "Harga Per Karat USD";
        //     htd.Width = 100;
        //     htr.Cells.Add(htd);

        //     htd = new TableHeaderCell();
        //     htd.Text = "Total Harga USD";
        //     htd.Width = 100;
        //     htr.Cells.Add(htd);
        // }

        // tb.Rows.Add(htr);

        // //RESULT
        // int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
        // foreach (var r in rs)
        // {
        //     if (!Response.IsClientConnected) break;

        //     TableRow tr = new TableRow();
        //     TableCell td;

        //     td = new TableCell();
        //     td.Text = "<p>" + index++.ToString() + ".</p>";
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = r.Nomor;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = DL.LocationType(r.StockActualDJ.TipeLokasi);
        //     tr.Cells.Add(td);

        //     string Lokasi = "";

        //     if (r.StockActualDJ.TipeLokasi == 0)
        //         Lokasi = rsWarehouse[r.StockActualDJ.IDLokasi];
        //     else if (r.StockActualDJ.TipeLokasi == 1)
        //         Lokasi = rsOutlet[r.StockActualDJ.IDLokasi];
        //     else if (r.StockActualDJ.TipeLokasi == 2)
        //         Lokasi = rsExhibition[r.StockActualDJ.IDLokasi];

        //     td = new TableCell();
        //     td.Text = Lokasi;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = r.Nama;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = r.Nama01;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = r.Nama02;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = r.Nama03;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = r.Nama04;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = r.GIA;
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = (action == "excel") ? r.TotalButir.ToString("0").Replace(",", ".") : r.TotalButir.ToString("N0");
        //     tr.Cells.Add(td);

        //     td = new TableCell();
        //     td.Text = (action == "excel") ? r.TotalCarat.ToString("0.000").Replace(",", ".") : r.TotalCarat.ToString("N3");
        //     tr.Cells.Add(td);

        //     if (mode == "accounting")
        //     {
        //         td = new TableCell();
        //         td.Text = (action == "excel") ? r.HargaSatuan.ToString("0.00").Replace(",", ".") : r.HargaSatuan.ToString("N2");
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = (action == "excel") ? r.HargaTotal.ToString("0.00").Replace(",", ".") : r.HargaTotal.ToString("N2");
        //         tr.Cells.Add(td);
        //     }

        //     tb.Rows.Add(tr);

        //     TotalButir += r.TotalButir;
        //     TotalCarat += r.TotalCarat;
        //     TotalHargaSatuan += r.HargaSatuan;
        //     TotalHargaTotal += r.HargaTotal;
        // }

        // TableRow tr2 = new TableRow();
        // TableCell td2;

        // td2 = new TableCell();
        // td2.Text = "<b>Total</b>";
        // int cols = 11;
        // td2.ColumnSpan = (mode == "accounting") ? cols : cols - 2;
        // tr2.Cells.Add(td2);

        // td2 = new TableCell();
        // td2.Text = (action == "excel") ? TotalButir.ToString("0").Replace(",", ".") : TotalButir.ToString("N0");
        // tr2.Cells.Add(td2);

        // td2 = new TableCell();
        // td2.Text = (action == "excel") ? TotalCarat.ToString("0.000").Replace(",", ".") : TotalCarat.ToString("N3");
        // tr2.Cells.Add(td2);

        // if (mode == "accounting")
        // {


        //     td2 = new TableCell();
        //     td2.Text = (action == "excel") ? TotalHargaSatuan.ToString("0.00").Replace(",", ".") : TotalHargaSatuan.ToString("N2");
        //     tr2.Cells.Add(td2);

        //     td2 = new TableCell();
        //     td2.Text = (action == "excel") ? TotalHargaTotal.ToString("0.00").Replace(",", ".") : TotalHargaTotal.ToString("N2");
        //     tr2.Cells.Add(td2);
        // }

        // tb.Rows.Add(tr2);

        // string href = App.RefreshURL(this);

        // //Customize-Qstr
        // href += "&kw=" + kw.Text;
        // href += "&parcel01=" + parcel01.SelectedIndex;
        // href += "&parcel02=" + parcel02.SelectedIndex;
        // href += "&parcel03=" + parcel03.SelectedIndex;
        // href += "&parcel04=" + parcel04.SelectedIndex;
        // href += "&order=" + order.SelectedIndex;
        // href += "&orderby=" + orderby.SelectedIndex;
        // href += "&supplier=" + supplier.SelectedValue;
        // href += "&tipelokasi=" + tipelokasi.SelectedValue;
        // href += "&lokasi=" + lokasi.SelectedValue;
        // href += "&page=" + (pageNumber + 1).ToString();

        // string href2 = App.RefreshURL(this);

        // //Customize-Qstr
        // href2 += "&kw=" + kw.Text;
        // href2 += "&parcel01=" + parcel01.SelectedIndex;
        // href2 += "&parcel02=" + parcel02.SelectedIndex;
        // href2 += "&parcel03=" + parcel03.SelectedIndex;
        // href2 += "&parcel04=" + parcel04.SelectedIndex;
        // href2 += "&order=" + order.SelectedIndex;
        // href2 += "&orderby=" + orderby.SelectedIndex;
        // href2 += "&supplier=" + supplier.SelectedValue;
        // href2 += "&tipelokasi=" + tipelokasi.SelectedValue;
        // href2 += "&lokasi=" + lokasi.SelectedValue;
        // href2 += "&page=" + (pageNumber - 1).ToString();

        // string href3 = App.RefreshURL(this);

        // //Customize-Qstr
        // href3 += "&kw=" + kw.Text;
        // href3 += "&parcel01=" + parcel01.SelectedIndex;
        // href3 += "&parcel02=" + parcel02.SelectedIndex;
        // href3 += "&parcel03=" + parcel03.SelectedIndex;
        // href3 += "&parcel04=" + parcel04.SelectedIndex;
        // href3 += "&order=" + order.SelectedIndex;
        // href3 += "&orderby=" + orderby.SelectedIndex;
        // href3 += "&supplier=" + supplier.SelectedValue;
        // href3 += "&tipelokasi=" + tipelokasi.SelectedValue;
        // href3 += "&lokasi=" + lokasi.SelectedValue;
        // href3 += "&page=1";

        // string href4 = App.RefreshURL(this);

        // //Customize-Qstr
        // href4 += "&kw=" + kw.Text;
        // href4 += "&parcel01=" + parcel01.SelectedIndex;
        // href4 += "&parcel02=" + parcel02.SelectedIndex;
        // href4 += "&parcel03=" + parcel03.SelectedIndex;
        // href4 += "&parcel04=" + parcel04.SelectedIndex;
        // href4 += "&order=" + order.SelectedIndex;
        // href4 += "&orderby=" + orderby.SelectedIndex;
        // href4 += "&supplier=" + supplier.SelectedValue;
        // href4 += "&tipelokasi=" + tipelokasi.SelectedValue;
        // href4 += "&lokasi=" + lokasi.SelectedValue;
        // href4 += "&page=" + TotalPage;

        // string paging = "";

        // paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        // paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
        // paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        // paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
        // paging += "<a href=\"" + href4 + "\">Last</a>";
        // paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

        // paging_text.Text = paging;
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


    private void BindData(int page,int pageSize,int totalRecord)
    {     
        int totalRecords = totalRecord; // Fetch total records from DB
        int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);        

        GeneratePagination(page, totalPages);
    }  

    private void GeneratePagination(int currentPage, int totalPages)
    {        
        string paginationHtml = "";

        // Previous Button            
        // paginationHtml += $"<li class='{(currentPage == 1 ? "disabled" : "")}'> <a href='{(currentPage > 1 ? url + (currentPage - 1) : "#")}'>&laquo;</a> </li>";
                        
        int maxDisplay = 10;
        int startPage = Math.Max(1, currentPage - maxDisplay / 2);
        int endPage = Math.Min(totalPages, startPage + maxDisplay - 1);

        // Previous Button
        paginationHtml += "<a href='Stone3.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=" + (int.Parse(startROW.InnerHtml) - 1) + "' class='btn btn-default' " + (currentPage == 1 ? "disabled" : "") + " runat='server'>Prev</a>";

        // First Page and Ellipsis
        if (startPage > 1)
        {
            paginationHtml += "<a href='Stone3.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=1' class='btn btn-default' runat='server'>1</a>";
            if (startPage > 2)
            {
            paginationHtml += "<span readonly class='btn btn-default'>...</span>";
            }
        }

        // Page Numbers
        for (int i = startPage; i <= endPage; i++)
        {
            paginationHtml += "<a href='Stone3.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=" + i + "' class='btn btn-default' " + (currentPage == i ? "disabled" : "") + " runat='server'>" + i + "</a>";
        }

        // Last Page and Ellipsis
        if (endPage < totalPages)
        {
            if (endPage < totalPages - 1)
            {
            paginationHtml += "<span readonly class='btn btn-default'>...</span>";
            }
            paginationHtml += "<a href='Stone3.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=" + totalPages + "' class='btn btn-default' runat='server'>" + totalPages + "</a>";
        }

        // Next Button
        paginationHtml += "<a href='Stone3.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=" + (int.Parse(startROW.InnerHtml) + 1) + "' class='btn btn-default' " + ((currentPage == totalPages || currentPage > totalPages) ? "disabled" : "") + " runat='server'>Next</a>";

        paginationGRP.InnerHtml = paginationHtml;
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
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Nama";
        htd.Width = 200;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Item";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Size";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Colour";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "GIA";
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
}