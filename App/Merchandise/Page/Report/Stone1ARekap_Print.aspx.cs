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
			fillTable(sender, e);
		}
	}

	//Display Table
    protected void fillTable(object sender, EventArgs e)
    {
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
                      StockActualDJ = p.StockProductDJ.StockActualDJ
                  }).OrderBy(o).Where(z => z.Nama.Contains(Request.QueryString["kw"]) ||
                     z.KodeParcel.Contains(Request.QueryString["kw"]) || z.Nomor.Contains(Request.QueryString["kw"]));

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(Request.QueryString["parcel01"]));
        if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(Request.QueryString["parcel02"]));
        if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(Request.QueryString["parcel03"]));
        if (Convert.ToInt32(Request.QueryString["parcel04"]) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(Request.QueryString["parcel04"]));

        var stone = rs.GroupBy(p => p.KodeParcel);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        else
        {
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

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
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalButir += Convert.ToDecimal(r.Sum(p => p.TotalButir));
                TotalCarat += Convert.ToDecimal(r.Sum(p => p.TotalCarat));
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 5;
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
}