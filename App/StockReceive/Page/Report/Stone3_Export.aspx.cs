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

            Response.AppendHeader("content-disposition", "attachment; filename=Stone3_Export.xls");
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
        /*var rs = from p in db.Stone3.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(Request.QueryString["kw"]) ||
                     p.Keterangan.Contains(Request.QueryString["kw"]) ||
                     p.KodeParcel.Contains(Request.QueryString["kw"])
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

                  }).OrderBy(o).Where(z => z.Nama.Contains(Request.QueryString["kw"]) ||
                     z.KodeParcel.Contains(Request.QueryString["kw"]) || z.Nomor.Contains(Request.QueryString["kw"]));

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "")
            rs = rs.Where(p => p.StockReceiveDJ.SupplierNama.Equals(Supplier));

        //rs = rs.Where(p => p.StockReceiveDJ.StatusLebur == false && p.StockReceiveDJ.ReturnProduct == false && p.StockReceiveDJ.StatusPenjualan == 0);

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
            td.Text = p_ForceConvertToDecimal(r.TotalButir).ToString("N0").Replace(".", "").Replace(",", ".");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.TotalCarat).ToString("N3").Replace(".", "").Replace(",", ".");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.HargaSatuan).ToString("N2").Replace(".", "").Replace(",", ".");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.HargaTotal).ToString("N2").Replace(".", "").Replace(",", ".");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.HargaSatuanM).ToString("N2").Replace(".", "").Replace(",", ".");
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = p_ForceConvertToDecimal(r.HargaTotalM).ToString("N2").Replace(".", "").Replace(",", ".");
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
        td2.ColumnSpan = 8;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalButir.ToString("N0").Replace(".","").Replace(",",".");
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalCarat.ToString("N3").Replace(".","").Replace(",",".");
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "-";
        td2.CssClass = "text-right";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = TotalHarga.ToString("N2").Replace(".","").Replace(",",".");
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