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
			DLC.Parcel102(parcel02, false);
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
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

		Response.Redirect(href);
	}
    protected void Export(object sender, EventArgs e)
    {
        string href = "Stone1BRekap_Export.aspx?id=1";

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
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "Stone1BRekap_Print.aspx?id=1";

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
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

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
			orderby.SelectedValue = Request.QueryString["orderby"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            supplier.SelectedValue = Request.QueryString["supplier"];
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
                      HargaSatuan = p_ForceConvertToDecimal(p.HargaSatuan),
                      HargaTotal = p_ForceConvertToDecimal(p.HargaTotal),
                      HargaSatuanM = p_ForceConvertToDecimal(p.HargaSatuanM),
                      HargaTotalM = p_ForceConvertToDecimal(p.HargaTotalM)

                  }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
                     z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

        //rs = rs.Where(p => p.StockReceiveDJ.StatusLebur == false && p.StockReceiveDJ.ReturnProduct == false && p.StockReceiveDJ.StatusPenjualan == 0);

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "")
            rs = rs.Where(p => p.StockReceiveDJ.SupplierNama.Equals(Supplier));

        DateTime start;
        DateTime end;
        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.StockReceiveDJ.ApprovalTgl >= start && p.StockReceiveDJ.ApprovalTgl < end.AddDays(1));
        }

        if (Convert.ToInt32(parcel01.SelectedValue) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(parcel01.SelectedValue));
        if (Convert.ToInt32(parcel02.SelectedValue) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(parcel02.SelectedValue));
        if (Convert.ToInt32(parcel03.SelectedValue) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(parcel03.SelectedValue));
        if (Convert.ToInt32(parcel04.SelectedValue) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(parcel04.SelectedValue));
        if (Convert.ToInt32(parcel05.SelectedValue) > 0) rs = rs.Where(p => p.Parcel05 == Convert.ToInt32(parcel05.SelectedValue));
        if (Convert.ToInt32(parcel06.SelectedValue) > 0) rs = rs.Where(p => p.Parcel06 == Convert.ToInt32(parcel06.SelectedValue));
        if (Convert.ToInt32(parcel07.SelectedValue) > 0) rs = rs.Where(p => p.Parcel07 == Convert.ToInt32(parcel07.SelectedValue));

        var stone = rs.GroupBy(p => p.KodeParcel);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        else
        {
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalHarga = 0m;
            //decimal TotalHargaSatuanM = 0m;
            decimal TotalHargaTotalM = 0m;

            //RESULT
            int index = 1;
            foreach (var r in stone)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama01;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama03;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama04;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama05;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama06;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToInt(p.TotalButir))).ToString("N0"); // Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToInt(p.TotalCarat))).ToString("N3"); // Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(p_ForceConvertToInt(r.First().HargaSatuan)).ToString("N2"); // Convert.ToDecimal(r.First().HargaSatuan.Value).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToInt(p.HargaTotal))).ToString("N2"); // Convert.ToDecimal(r.Sum(p => p.HargaTotal.Value)).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(p_ForceConvertToInt(r.First().HargaSatuanM)).ToString("N2"); // Convert.ToDecimal(r.First().HargaSatuanM.Value).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToInt(p.HargaTotalM))).ToString("N2"); // Convert.ToDecimal(r.Sum(p => p.HargaTotalM.Value)).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalButir += p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToInt(p.TotalButir))); // Convert.ToDecimal(r.Sum(p => p.TotalButir));
                TotalCarat += p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToInt(p.TotalCarat))); // Convert.ToDecimal(r.Sum(p => p.TotalCarat));
                TotalHarga += p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToInt(p.HargaTotal))); // Convert.ToDecimal(r.Sum(p => p.HargaTotal));
                //TotalHargaSatuanM += p_ForceConvertToDecimal(r.Sum(p => p.HargaSatuanM)); // Convert.ToDecimal(r.Sum(p => p.HargaSatuanM));
                TotalHargaTotalM += p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToInt(p.HargaTotalM))); // Convert.ToDecimal(r.Sum(p => p.HargaTotalM));
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 7;
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

            tb.Rows.Add(tr2);
        }
    }
}