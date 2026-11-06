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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Stone1A_Export.xls");
			fillTable(sender, e);
		}
	}

	//Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        //ORDER BY
        string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];

        //SELECT
        /*var rs = from p in db.Stone1A.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(Request.QueryString["kw"]) ||
                     p.Keterangan.Contains(Request.QueryString["kw"]) ||
                     p.KodeParcel.Contains(Request.QueryString["kw"])
                     )
                 select p;*/

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
                      Nama01 = p.Stone1A.Parcel101.Nama,
                      Nama02 = p.Stone1A.Parcel102.Nama,
                      Nama03 = p.Stone1A.Parcel103.Nama,
                      Nama04 = p.Stone1A.Parcel104.Nama,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      HargaSatuan = p.HargaSatuan.Value,
                      HargaTotal = p.HargaTotal.Value
                  }).OrderBy(o).Where(z => z.Nama.Contains(Request.QueryString["kw"]) ||
                     z.KodeParcel.Contains(Request.QueryString["kw"]) || z.Nomor.Contains(Request.QueryString["kw"]));

        if (Tipe != -1)
            rs = rs.Where(p => p.TipeLokasi == Tipe);
        if (IDLokasi != 0)
            rs = rs.Where(p => p.IDLokasi == IDLokasi && p.TipeLokasi == Tipe);

        string Supplier = Request.QueryString["supplier"];
        if(Supplier !="" && Supplier !="Supplier" && Supplier!="0")
            rs = rs.Where(p=>p.StockProductDJ.SupplierNama.Equals(Supplier));

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(Request.QueryString["parcel01"]));
        if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(Request.QueryString["parcel02"]));
        if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(Request.QueryString["parcel03"]));
        if (Convert.ToInt32(Request.QueryString["parcel04"]) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(Request.QueryString["parcel04"]));

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHarga = 0m;

        //RESULT
        int index = 0;
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
            td.Text = DL.LocationType(r.StockActualDJ.TipeLokasi);
            tr.Cells.Add(td);

            string Lokasi = "";

            if (r.StockActualDJ.TipeLokasi == 0)
                Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID = " + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
            else if (r.StockActualDJ.TipeLokasi == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
            else if (r.StockActualDJ.TipeLokasi == 2)
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + r.StockActualDJ.IDLokasi, Db.Cnn_DB);

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
            td.Text = r.TotalButir.ToString("N0").Replace(".","").Replace(",",".");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TotalCarat.ToString("N3").Replace(".","").Replace(",",".");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.HargaSatuan.ToString("N2").Replace(".","").Replace(",",".");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.HargaTotal.ToString("N2").Replace(".","").Replace(",",".");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            TotalButir += r.TotalButir;
            TotalCarat += r.TotalCarat;
            TotalHarga += r.HargaTotal;
        }

        TableRow tr2 = new TableRow();
        TableCell td2;

        td2 = new TableCell();
        td2.Text = "<b>Total</b>";
        td2.ColumnSpan = 8;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalButir.ToString("N0").Replace(".","").Replace(",",".");
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalCarat.ToString("N3").Replace(".","").Replace(",",".");
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "-";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalHarga.ToString("N2").Replace(".","").Replace(",",".");
        tr2.Cells.Add(td2);

        tb.Rows.Add(tr2);

    }
}