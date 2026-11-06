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
			kw.Focus(); //Keyword

			//DLC
			DLC.Parcel101(parcel01);
			DLC.Parcel102(parcel02,true);
			DLC.Parcel103(parcel03);
			DLC.Parcel104(parcel04);

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
		href += "&parcel04=" + parcel04.SelectedIndex;
		href += "&order=" + order.SelectedIndex;
		href += "&orderby=" + orderby.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			parcel01.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel01"]);
			parcel02.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel02"]);
			parcel03.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel03"]);
			parcel04.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel04"]);
			order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
			orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
		}
	}

	//Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //ORDER BY
        string o = order.SelectedValue + " " + orderby.SelectedValue;

        //SELECT
        /*var rs = from p in db.Stone1A.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(kw.Text) ||
                     p.Keterangan.Contains(kw.Text) ||
                     p.KodeParcel.Contains(kw.Text)
                     )
                 select p;*/

        var rs = (from p in db.StockProductDJ_Stone1A
                  select new
                  {
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
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ
                  }).Union
                (from p in db.StockProductDJ_Stone1B
                 select new
                 {
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
                     StockProductDJ = p.StockProductDJ,
                     StockActualDJ = p.StockProductDJ.StockActualDJ
                 }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
                     z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == parcel04.SelectedIndex);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;

        //RESULT
        int index = 1;
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
            td.Text = r.KodeParcel;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TotalButir.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TotalCarat.ToString("N3");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            TotalButir += r.TotalButir;
            TotalCarat += r.TotalCarat;
        }

        TableRow tr2 = new TableRow();
        TableCell td2;

        td2 = new TableCell();
        td2.Text = "<b>Total</b>";
        td2.ColumnSpan = 6;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalButir.ToString("N0");
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalCarat.ToString("N3");
        tr2.Cells.Add(td2);

        tb.Rows.Add(tr2);
    }
}