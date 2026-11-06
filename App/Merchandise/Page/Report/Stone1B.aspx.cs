using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

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
    string mode = "merchandise"; /// accounting, merchandise
                                /// 
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//DLC
			DLC.Parcel101(parcel01);
			DLC.Parcel102(parcel02,true);
			DLC.Parcel103(parcel03);
			DLC.Parcel104(parcel04);
            DLC.Parcel105(parcel05);
            DLC.Parcel106(parcel06);
            DLC.Parcel107(parcel07);

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

        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        //ORDER BY
        string o = order.SelectedValue + " " + orderby.SelectedValue;

        //SELECT
        /*var rs = from p in db.Stone1B.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(kw.Text) ||
                     p.Keterangan.Contains(kw.Text) ||
                     p.KodeParcel.Contains(kw.Text)
                     )
                 select p;*/

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
                      HargaSatuan = p.HargaSatuan.Value == null ? 0 : p.HargaSatuan.Value,
                      HargaTotal = p.HargaTotal.Value == null ? 0 : p.HargaTotal.Value
                  }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
                     z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

        if (Tipe != -1)
            rs = rs.Where(p => p.TipeLokasi == Tipe);
        if (IDLokasi != -1)
            rs = rs.Where(p => p.IDLokasi == IDLokasi && p.TipeLokasi == Tipe);

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "" && Supplier != "Supplier" && Supplier!="0")
            rs = rs.Where(p => p.StockProductDJ.SupplierNama.Equals(Supplier));

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (Convert.ToInt32(parcel01.SelectedValue) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(parcel01.SelectedValue));
        if (Convert.ToInt32(parcel02.SelectedValue) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(parcel02.SelectedValue));
        if (Convert.ToInt32(parcel03.SelectedValue) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(parcel03.SelectedValue));
        if (Convert.ToInt32(parcel04.SelectedValue) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(parcel04.SelectedValue));
        if (Convert.ToInt32(parcel05.SelectedValue) > 0) rs = rs.Where(p => p.Parcel05 == Convert.ToInt32(parcel05.SelectedValue));
        if (Convert.ToInt32(parcel06.SelectedValue) > 0) rs = rs.Where(p => p.Parcel06 == Convert.ToInt32(parcel06.SelectedValue));
        if (Convert.ToInt32(parcel07.SelectedValue) > 0) rs = rs.Where(p => p.Parcel07 == Convert.ToInt32(parcel07.SelectedValue));

        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 1;

        decimal TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

        TotalPage = Math.Ceiling(TotalPage);

        rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

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

        tb.Rows.Add(htr);

        //RESULT
        int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            if (r.StockActualDJ.IDLokasi < 0) continue;
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
            td.Text = Str.Tgl(r.StockProductDJ.OperatorTgl);
            tr.Cells.Add(td);

            double TotalDays = 0;
            TotalDays = (DateTime.Today - r.StockProductDJ.OperatorTgl.Value).TotalDays;

            td = new TableCell();
            td.Text = TotalDays.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TotalButir.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TotalCarat.ToString("N3");
            tr.Cells.Add(td);

            if (mode == "accounting")
            {
                td = new TableCell();
                td.Text = r.HargaSatuan.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaTotal.ToString("N2");
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
        td2.Text = "<b>Total</b>";
        td2.ColumnSpan = (mode == "accounting") ? 16 : 14;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalButir.ToString("N0");
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalCarat.ToString("N3");
        tr2.Cells.Add(td2);

        if (mode == "accounting")
        {
            td2 = new TableCell();
            td2.Text = "-";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalHarga.ToString("N2");
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