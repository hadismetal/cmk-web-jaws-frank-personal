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
			DLC.Parcel301(parcel01);
			DLC.Parcel302(parcel02);
			DLC.Parcel303(parcel03);
			DLC.Parcel304(parcel04);

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
		href += "&order=" + order.SelectedIndex;
		href += "&orderby=" + orderby.SelectedIndex;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

		Response.Redirect(href);
	}
    protected void Export(object sender, EventArgs e)
    {
        string href = "Stone3Rekap_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue ;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "Stone3Rekap_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

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
        /*var rs = from p in db.Stone3.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(kw.Text) ||
                     p.Keterangan.Contains(kw.Text) ||
                     p.KodeParcel.Contains(kw.Text)
                     )
                 select p;*/

        var rs = (from p in db.StockReceiveDJ_Stone3
                  where !p.StockReceiveDJ.Draft
                  select new
                  {
                      ID = p.Stone3.ID,
                      Nama = p.Stone3.Nama,
                      KodeParcel = p.Stone3.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockReceiveDJ.Nomor,
                      Parcel01 = p.Stone3.Parcel01,
                      Parcel02 = p.Stone3.Parcel02,
                      Parcel03 = p.Stone3.Parcel03,
                      Parcel04 = p.Stone3.Parcel04,
                      Nama01 = p.Stone3.Parcel301.Nama,
                      Nama02 = p.Stone3.Parcel302.Nama,
                      Nama03 = p.Stone3.Parcel303.Nama,
                      Nama04 = p.Stone3.Parcel304.Nama,
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

        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == parcel04.SelectedIndex);

        var stone = rs.GroupBy(p => p.KodeParcel);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        else
        {
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalHarga = 0m;
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
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.TotalButir))).ToString("N0"); // Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.TotalCarat))).ToString("N3"); // Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(p_ForceConvertToDecimal(r.First().HargaSatuan)).ToString("N2"); // Convert.ToDecimal(r.First().HargaSatuan.Value).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.HargaTotal))).ToString("N2"); // Convert.ToDecimal(r.Sum(p => p.HargaTotal.Value)).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(p_ForceConvertToDecimal(r.First().HargaSatuanM)).ToString("N2"); // Convert.ToDecimal(r.First().HargaSatuanM.Value).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.HargaTotalM))).ToString("N2"); // Convert.ToDecimal(r.Sum(p => p.HargaTotalM.Value)).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalButir += p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.TotalButir))); // Convert.ToDecimal(r.Sum(p => p.TotalButir));
                TotalCarat += p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.TotalCarat))); // Convert.ToDecimal(r.Sum(p => p.TotalCarat));
                TotalHarga += p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.HargaTotal))); // Convert.ToDecimal(r.Sum(p => p.HargaTotal));
                //TotalHargaSatuanM += p_ForceConvertToDecimal(r.Sum(p => p.HargaSatuanM)); // Convert.ToDecimal(r.Sum(p => p.HargaSatuanM));
                TotalHargaTotalM += p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.HargaTotalM))); // Convert.ToDecimal(r.Sum(p => p.HargaTotalM));
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 5;
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