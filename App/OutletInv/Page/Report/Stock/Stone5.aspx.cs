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
			DLC.Parcel501(parcel01);
			DLC.Parcel502(parcel02);
			DLC.Parcel503(parcel03);
			DLC.Parcel504(parcel04);
            DLC.Parcel505(parcel05);
            DLC.Parcel506(parcel06);
            DLC.Parcel507(parcel07);

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
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
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
            parcel05.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel05"]);
            parcel06.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel06"]);
            parcel07.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel07"]);
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
        /*var rs = from p in db.Stone5.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(kw.Text) ||
                     p.Keterangan.Contains(kw.Text) ||
                     p.KodeParcel.Contains(kw.Text)
                     )
                 select p;*/

        var rs = (from p in db.StockProductDJ_Stone5
                  where !p.StockProductDJ.Draft.Value
                  select new
                  {
                      ID = p.Stone5.ID,
                      Nama = p.Stone5.Nama,
                      KodeParcel = p.Stone5.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone5.Parcel01,
                      Parcel02 = p.Stone5.Parcel02,
                      Parcel03 = p.Stone5.Parcel03,
                      Parcel04 = p.Stone5.Parcel04,
                      Parcel05 = p.Stone5.Parcel05,
                      Parcel06 = p.Stone5.Parcel06,
                      Parcel07 = p.Stone5.Parcel07,
                      Nama01 = p.Stone5.Parcel501.Nama,
                      Nama02 = p.Stone5.Parcel502.Nama,
                      Nama03 = p.Stone5.Parcel503.Nama,
                      Nama04 = p.Stone5.Parcel504.Nama,
                      Nama05 = p.Stone5.Parcel505.Nama,
                      Nama06 = p.Stone5.Parcel506.Nama,
                      Nama07 = p.Stone5.Parcel507.Nama,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ
                  }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
                     z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.StockActualDJ.TipeLokasi == d.First().TipeLokasi && p.StockActualDJ.IDLokasi == d.First().IDLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualDJ.IDLokasi) && p.StockActualDJ.TipeLokasi == 1);
        }

        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == parcel04.SelectedIndex);
        if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Parcel05 == parcel05.SelectedIndex);
        if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Parcel06 == parcel06.SelectedIndex);
        if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Parcel07 == parcel07.SelectedIndex);

        int pageNumber = 0;
        int numberOfObjectsPerPage = 30;

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 0;

        rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;

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
            td.Text = r.Nama02;
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

            td = new TableCell();
            td.Text = r.Nama06;
            tr.Cells.Add(td);
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
        td2.ColumnSpan = 10;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalButir.ToString("N0");
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalCarat.ToString("N3");
        tr2.Cells.Add(td2);

        tb.Rows.Add(tr2);

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&order=" + order.SelectedIndex;
        href += "&orderby=" + orderby.SelectedIndex;
        href += "&page=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&parcel01=" + parcel01.SelectedIndex;
        href2 += "&parcel02=" + parcel02.SelectedIndex;
        href2 += "&parcel03=" + parcel03.SelectedIndex;
        href2 += "&parcel04=" + parcel04.SelectedIndex;
        href2 += "&parcel05=" + parcel05.SelectedIndex;
        href2 += "&parcel06=" + parcel06.SelectedIndex;
        href2 += "&parcel07=" + parcel07.SelectedIndex;
        href2 += "&order=" + order.SelectedIndex;
        href2 += "&orderby=" + orderby.SelectedIndex;
        href2 += "&page=" + (pageNumber - 1).ToString();

        string paging = "";

        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href + "\">Next</a>";
        paging += "</div>";

        paging_text.Text = paging;
    }
}