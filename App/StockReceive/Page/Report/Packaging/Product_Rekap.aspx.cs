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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
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

        var rs = from p in db.StockReceivePackaging_Product
                 where
                    (
                    p.Packaging.Nama.Contains(kw.Text) ||
                    p.Packaging.Kode.Contains(kw.Text) ||
                    p.Packaging.Keterangan.Contains(kw.Text)
                    )
                 select p;

        rs = rs.OrderByDescending(p => p.Packaging.Tgl);

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
            rs = rs.Where(p => p.StockReceivePackaging.IDWarehouse == Cabang);


        
        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand != null && Brand > 0)
            rs = rs.Where(p => p.Packaging.IDBrand == Brand);

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
                td.Text = r.Packaging.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Kode;
                tr.Cells.Add(td);

                string Lokasi = "";

                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockReceivePackaging.IDWarehouse, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Satuan;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Received.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Damaged.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NeverArrived.Value.ToString("N0");
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
}