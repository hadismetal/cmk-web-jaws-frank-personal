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
            DLC.Parcel105(parcel05);
            DLC.Parcel106(parcel06);
            DLC.Parcel107(parcel07);
            DLC.DDLSupplier(supplier, 0);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

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
        href += "&supplier=" + supplier.SelectedValue;
		href += "&orderby=" + orderby.SelectedIndex;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

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
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

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
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

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
            supplier.SelectedValue = Request.QueryString["supplier"];
			orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
		}
	}

    private decimal p_ForceConvertToDecimal(object objValue)
    {
        decimal decReturn = decimal.Zero;

        try
        {
            decReturn = Convert.ToDecimal(objValue);
        }
        catch
        {
            //  do nothing.
            //  return 0;
        }

        return decReturn;
    }

    private int p_ForceConvertToInt(object objValue)
    {
        int intReturn = 0;

        try
        {
            intReturn = Convert.ToInt32(objValue);
        }
        catch
        {
            //  do nothing.
            //  return 0;
        }

        return intReturn;
    }

	//Display Table
    protected void fillTable(object sender, EventArgs e)
    {
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

        var rs = (from p in db.StockReceiveDJ_Stone1B
                  where p.StatusTerpasang == 1
                  && !p.StockReceiveDJ.Draft
                  select new
                  {
                      ID = p.Stone1B.ID,
                      Nama = p.Stone1B.Nama,
                      KodeParcel = p.Stone1B.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockReceiveDJ.Nomor,
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
                      StockReceiveDJ = p.StockReceiveDJ,
                      IDWarehouse = p.StockReceiveDJ.IDWarehouse,
                      GIA = p.GIA,
                      HargaSatuan = p_ForceConvertToDecimal(p.HargaSatuan),
                      HargaTotal = p_ForceConvertToDecimal(p.HargaTotal),
                      HargaSatuanM = p_ForceConvertToDecimal(p.HargaSatuanM),
                      HargaTotalM = p_ForceConvertToDecimal(p.HargaTotalM)

                  }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
                     z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "")
            rs = rs.Where(p => p.StockReceiveDJ.SupplierNama.Equals(Supplier));

        //rs = rs.Where(p => p.StockReceiveDJ.StatusLebur == false && p.StockReceiveDJ.ReturnProduct == false && p.StockReceiveDJ.StatusPenjualan == 0);

        DateTime start;
        DateTime end;
        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.StockReceiveDJ.ApprovalTgl >= start && p.StockReceiveDJ.ApprovalTgl < end.AddDays(1));
        }

        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == parcel04.SelectedIndex);
        if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Parcel05 == parcel05.SelectedIndex);
        if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Parcel06 == parcel06.SelectedIndex);
        if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Parcel07 == parcel07.SelectedIndex);

        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 1;

        rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHarga = 0m;
        decimal TotalHargaTotalM = 0m;

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
            td.Text = DL.LocationType(0);
            tr.Cells.Add(td);

            string Lokasi = "";

            
                Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID = " + r.IDWarehouse, Db.Cnn_DB);
            

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
            td.Text = p_ForceConvertToDecimal(r.TotalButir).ToString("N0");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.TotalCarat).ToString("N3");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.HargaSatuan).ToString("N2");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.HargaTotal).ToString("N2");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.HargaSatuanM).ToString("N2");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.HargaTotalM).ToString("N2");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = string.Empty;
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + r.GIA + "\" target=\"_blank\">" + r.GIA + "</a>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockReceiveDJ.OperatorTgl);
            tr.Cells.Add(td);

            double TotalDays = 0;
            TotalDays = (DateTime.Today - r.StockReceiveDJ.OperatorTgl).TotalDays;

            td = new TableCell();
            td.Text = TotalDays.ToString("N0");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            TotalButir += p_ForceConvertToDecimal(r.TotalButir);
            TotalCarat += p_ForceConvertToDecimal(r.TotalCarat);
            TotalHarga += p_ForceConvertToDecimal(r.HargaTotal);
            TotalHargaTotalM += p_ForceConvertToDecimal(r.HargaTotalM);
        }

        TableRow tr2 = new TableRow();
        TableCell td2;

        td2 = new TableCell();
        td2.Text = "<b>Total</b>";
        td2.ColumnSpan = 10;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalButir.ToString("N0");
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalCarat.ToString("N3");
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "-";
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalHarga.ToString("N2");
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "-";
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalHargaTotalM.ToString("N2");
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<b></b>";
        td2.ColumnSpan = 4;
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
        href += "&supplier=" + supplier.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

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
        href2 += "&supplier=" + supplier.SelectedValue;
        href2 += "&dari=" + datepicker.Text;
        href2 += "&sampai=" + datepicker2.Text;

        href2 += "&page=" + (pageNumber - 1).ToString();

        string paging = "";

        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href + "\">Next</a>";
        paging += "</div>";

        paging_text.Text = paging;
    }
}