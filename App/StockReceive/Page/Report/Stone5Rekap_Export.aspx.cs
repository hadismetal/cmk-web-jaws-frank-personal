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

            Response.AppendHeader("content-disposition", "attachment; filename=Stone5Rekap_Export.xls");
            fillTable(sender, e);
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
        string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];

        //SELECT
        /*var rs = from p in db.Stone5.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(Request.QueryString["kw"]) ||
                     p.Keterangan.Contains(Request.QueryString["kw"]) ||
                     p.KodeParcel.Contains(Request.QueryString["kw"])
                     )
                 select p;*/

        var rs = (from p in db.StockReceiveDJ_Stone5
                  where !p.StockReceiveDJ.Draft
                  select new
                  {
                      ID = p.Stone5.ID,
                      Nama = p.Stone5.Nama,
                      KodeParcel = p.Stone5.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockReceiveDJ.Nomor,
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
                      Nama08 = p.Stone5.Parcel508.Nama,
                      Nama09 = p.Stone5.Parcel509.Nama,
                      StockReceiveDJ = p.StockReceiveDJ,
                      IDWarehouse = p.StockReceiveDJ.IDWarehouse,
                      HargaSatuan = p_ForceConvertToDecimal(p.HargaSatuan),
                      HargaTotal = p_ForceConvertToDecimal(p.HargaTotal),
                      HargaSatuanM = p_ForceConvertToDecimal(p.HargaSatuanM),
                      HargaTotalM = p_ForceConvertToDecimal(p.HargaTotalM)

                  }).OrderBy(o).Where(z => z.Nama.Contains(Request.QueryString["kw"]) ||
                     z.KodeParcel.Contains(Request.QueryString["kw"]) || z.Nomor.Contains(Request.QueryString["kw"]));

        //rs = rs.Where(p => p.StockReceiveDJ.StatusLebur == false && p.StockReceiveDJ.ReturnProduct == false && p.StockReceiveDJ.StatusPenjualan == 0);

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "")
            rs = rs.Where(p => p.StockReceiveDJ.SupplierNama.Equals(Supplier));

        DateTime start;
        DateTime end;
        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            start = Convert.ToDateTime(Request.QueryString["dari"]);
            end = Convert.ToDateTime(Request.QueryString["sampai"]);
            rs = rs.Where(p => p.StockReceiveDJ.ApprovalTgl >= start && p.StockReceiveDJ.ApprovalTgl < end.AddDays(1));
        }

        if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(Request.QueryString["parcel01"]));
        if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(Request.QueryString["parcel02"]));
        if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(Request.QueryString["parcel03"]));
        if (Convert.ToInt32(Request.QueryString["parcel04"]) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(Request.QueryString["parcel04"]));
        if (Convert.ToInt32(Request.QueryString["parcel05"]) > 0) rs = rs.Where(p => p.Parcel05 == Convert.ToInt32(Request.QueryString["parcel05"]));
        if (Convert.ToInt32(Request.QueryString["parcel06"]) > 0) rs = rs.Where(p => p.Parcel06 == Convert.ToInt32(Request.QueryString["parcel06"]));
        if (Convert.ToInt32(Request.QueryString["parcel07"]) > 0) rs = rs.Where(p => p.Parcel07 == Convert.ToInt32(Request.QueryString["parcel07"]));

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
                td.Text = r.First().Nama02;
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
                td.Text = r.First().Nama07;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama08;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama09;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.TotalButir))).ToString("N0").Replace(".", "").Replace(",", "."); // Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.TotalCarat))).ToString("N3").Replace(".", "").Replace(",", "."); // Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(p_ForceConvertToDecimal(r.First().HargaSatuan)).ToString("N2").Replace(".", "").Replace(",", "."); // Convert.ToDecimal(r.First().HargaSatuan.Value).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.HargaTotal))).ToString("N2").Replace(".", "").Replace(",", "."); // Convert.ToDecimal(r.Sum(p => p.HargaTotal.Value)).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(p_ForceConvertToDecimal(r.First().HargaSatuanM)).ToString("N2").Replace(".", "").Replace(",", "."); // Convert.ToDecimal(r.First().HargaSatuanM.Value).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.Sum(p => p_ForceConvertToDecimal(p.HargaTotalM))).ToString("N2").Replace(".", "").Replace(",", "."); // Convert.ToDecimal(r.Sum(p => p.HargaTotalM.Value)).ToString("N2");
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
            td2.ColumnSpan = 11;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalButir.ToString("N0").Replace(".", "").Replace(",", ".");
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalCarat.ToString("N3").Replace(".", "").Replace(",", ".");
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "-";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalHarga.ToString("N2").Replace(".", "").Replace(",", ".");
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "-";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalHargaTotalM.ToString("N2").Replace(".", "").Replace(",", ".");
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}