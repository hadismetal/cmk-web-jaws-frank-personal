using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_LD : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int idlokasi { get { return App.QueryInt(this, "idlokasi"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        App.ProtectRole(Role.Manager);
		Ddl.Substorage(substorage);

		if (!IsPostBack) {

            DLC.Parcel101(parcel01);
            DLC.Parcel102(parcel02, true);
            DLC.Parcel103(parcel03);
            DLC.Parcel104(parcel04);
            DLC.Parcel105(parcel05);
            DLC.Parcel106(parcel06);
            DLC.Parcel107(parcel07);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductLD_Stone1B
                 where p.ReturnProduct == false && !p.StockProductLD.Draft.Value
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.StockActualLD_Stone1B.TipeLokasi == d.First().TipeLokasi && p.StockActualLD_Stone1B.IDLokasi == d.First().IDLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualLD_Stone1B.IDLokasi) && p.StockActualLD_Stone1B.TipeLokasi == 1);
        }

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualLD_Stone1B.Substorage == Substorage);
        }

        rs = rs.OrderByDescending(p => p.StockProductLD.ApprovalTgl);

        /// Master Brand
        var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocWarehouse
        var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocOutlet
        var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocExhibition
        var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);


        if (kw.Text != "") { rs = rs.Where(p => p.Nomor == "" + kw.Text + "" || p.GIA == "" + kw.Text + ""); }
        
        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel04 == parcel04.SelectedIndex);
        if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel05 == parcel05.SelectedIndex);
        if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel06 == parcel06.SelectedIndex);
        if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel07 == parcel07.SelectedIndex);

        int pageNumber = 0;
        int numberOfObjectsPerPage = 30;

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 0;

        rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

        if (rs != null)
        {
            int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalCarat = 0m;
            //decimal TotalHarga = 0m;
            foreach (var r in rs)
            {
                

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                /// PLU
                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                /// GIA
                td = new TableCell();
                td.Text = r.GIA;
                tr.Cells.Add(td);

                //decimal Jual = 0m;
                //decimal Exchange = 0m;
                //decimal Resell = 0m;

                //if (r.StockActualLD_Stone1B.Substorage == 1) { Jual = r.StockActualLD_Stone1B.InHand; }
                //else if (r.StockActualLD_Stone1B.Substorage == 3) { Exchange = r.StockActualLD_Stone1B.InHand; }
                //else if (r.StockActualLD_Stone1B.Substorage == 2) { Resell = r.StockActualLD_Stone1B.InHand; }

                //td = new TableCell();
                //td.Text = Jual.ToString("N0");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Exchange.ToString("N0");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Resell.ToString("N0");
                //tr.Cells.Add(td);

                /// Status
                td = new TableCell();
                td.Text = DL.SubStorage(r.StockActualLD_Stone1B.Substorage);
                tr.Cells.Add(td);

                /// Lokasi
                string Lokasi = "";
                if (r.StockActualLD_Stone1B.TipeLokasi == 0) { Lokasi = Lokasi = rsWarehouse[r.StockActualLD_Stone1B.IDLokasi]; }
                else if (r.StockActualLD_Stone1B.TipeLokasi == 1) { Lokasi = Lokasi = rsOutlet[r.StockActualLD_Stone1B.IDLokasi]; }
                else if (r.StockActualLD_Stone1B.TipeLokasi == 2) { Lokasi = Lokasi = rsExhibition[r.StockActualLD_Stone1B.IDLokasi]; }
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                /// Shape
                td = new TableCell();
                td.Text = r.Stone1B.Parcel101.Nama;
                tr.Cells.Add(td);

                /// Size
                td = new TableCell();
                td.Text = r.Stone1B.Parcel102.Nama;
                tr.Cells.Add(td);

                /// Brand
                td = new TableCell();
                td.Text = r.Stone1B.Parcel106.Nama;
                tr.Cells.Add(td);

                /// Colour
                td = new TableCell();
                td.Text = Convert.ToString(r.Stone1B.Parcel103.Nama);
                tr.Cells.Add(td);

                /// Clarity
                td = new TableCell();
                td.Text = Convert.ToString(r.Stone1B.Parcel104.Nama);
                tr.Cells.Add(td);

                /// Qty
                td = new TableCell();
                td.Text = r.StockActualLD_Stone1B.InHand.ToString("N0");
                tr.Cells.Add(td);

                /// Carat
                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalCarat).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                //TotalStock += Jual;
                //TotalResell += Resell;
                //TotalExchange += Exchange;
                //TotalModal += Convert.ToDecimal(r.HargaTotalM.Value);
                //TotalHarga += Convert.ToDecimal(r.HargaTotal.Value);
                TotalCarat += Convert.ToDecimal(r.TotalCarat);

                index++;
            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                td.ColumnSpan = 10;
                tr.Cells.Add(td);

                ///// Brand
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                ///// PLU
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                ///// GIA
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                ///// Status
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                ///// Lokasi
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                ///// Shape
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                ///// Size
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                ///// Colour
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                ///// Clarity
                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                /// Qty
                td = new TableCell();
                td.Text = rs.Sum(p => p.StockActualLD_Stone1B.InHand).ToString("N0");
                tr.Cells.Add(td);

                // Carat
                td = new TableCell();
                td.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
                tr.Cells.Add(td);


                tb.Rows.Add(tr);
            }
        }

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipe=" + tipe;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&page=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&tipe=" + tipe;
        href2 += "&parcel01=" + parcel01.SelectedIndex;
        href2 += "&parcel02=" + parcel02.SelectedIndex;
        href2 += "&parcel03=" + parcel03.SelectedIndex;
        href2 += "&parcel04=" + parcel04.SelectedIndex;
        href2 += "&parcel05=" + parcel05.SelectedIndex;
        href2 += "&parcel06=" + parcel06.SelectedIndex;
        href2 += "&parcel07=" + parcel07.SelectedIndex;
        href2 += "&substorage=" + substorage.SelectedValue;
        href2 += "&page=" + (pageNumber - 1).ToString();

        string paging = "";

        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href + "\">Next</a>";
        paging += "</div>";

        paging_text.Text = paging;
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            parcel01.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel01"]);
            parcel02.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel02"]);
            parcel03.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel03"]);
            parcel04.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel04"]);
            parcel05.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel05"]);
            parcel06.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel06"]);
            parcel07.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel07"]);
            substorage.SelectedValue = Request.QueryString["substorage"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipe=" + tipe;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "LD_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipe=" + tipe;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "LD_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipe=" + tipe;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }
}