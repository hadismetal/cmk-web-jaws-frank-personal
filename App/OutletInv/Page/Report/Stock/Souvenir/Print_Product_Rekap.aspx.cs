using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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
    protected int cabang { get { return App.QueryInt(this, "cabang"); } }

    protected int brand { get { return App.QueryInt(this, "brand"); } }

    private int tipe = 1;

    private string kw = string.Empty;
    protected void Page_Load(object sender, EventArgs e) {
        App.Protect();

        if (!IsPostBack)
        {

            int UserID = App.UserID;
            var d = from p in db.DataAdminSales
                    where p.IDUser == UserID
                    select p;
            if (d.Count() > 0)
            {
                string Lokasi = "";
                var x = d.First();

                if (x.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                else
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);

                outlet.Text = Lokasi;
            }
            else if (cabang > 0)
            {
                string Lokasi = string.Empty;
                if (tipe == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + cabang, Db.Cnn_DB);
                else
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + cabang, Db.Cnn_DB);

                outlet.Text = Lokasi;
            }
            else
                outlet.Text = "ALL";
            printtime.Text = DateTime.Now.ToString();
            kw = Request.QueryString["kw"];
            //Generate
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.StockActualSouvenir
                 where
                    (
                    p.Souvenir.Nama.Contains(kw) ||
                    p.Souvenir.Kode.Contains(kw) ||
                    p.Souvenir.Keterangan.Contains(kw)
                    )
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.TipeLokasi == d.First().TipeLokasi && p.IDLokasi == d.First().IDLokasi);
        }

        rs = rs.OrderByDescending(p => p.Souvenir.Tgl);

        int Cabang = Convert.ToInt32(cabang);
        if (Cabang > 0)
            rs = rs.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
        else
            rs = rs.Where(p => p.TipeLokasi == tipe);

        int Brand = Convert.ToInt32(brand);
        if (Brand != null && Brand > 0)
            rs = rs.Where(p => p.Souvenir.IDBrand == Brand);

        if (rs.Count() > 0)
        {
            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Kode;
                tr.Cells.Add(td);

                string Lokasi = "";

                if (r.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                else if(r.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Satuan;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.InHand.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.InTransit).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Damaged).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.NeverArrived).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
    }
}