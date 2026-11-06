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
    protected int tipe { get { return 1; } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {

            kw.Focus(); //Keyword

            //DLC
            DLC.CompanyBrand(brand);

            DLC.StockLocationList(cabang, tipe);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
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
                    p.Souvenir.Nama.Contains(kw.Text) ||
                    p.Souvenir.Kode.Contains(kw.Text) ||
                    p.Souvenir.Keterangan.Contains(kw.Text)
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

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
            rs = rs.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
        else
            rs = rs.Where(p => p.TipeLokasi == tipe);

        int Brand = Convert.ToInt32(brand.SelectedValue);
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
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            brand.SelectedValue = Request.QueryString["brand"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&cabang=" + cabang.Text;
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.SelectedValue;
        href += "&tipe=" + tipe;

        Response.Redirect(href);
    }

    protected void print_ServerClick(object sender, EventArgs e)
    {
        string href = "Print_Product_Rekap.aspx?";

        href += "&cabang=" + cabang.Text;
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.SelectedValue;

        Response.Redirect(href);
    }
}