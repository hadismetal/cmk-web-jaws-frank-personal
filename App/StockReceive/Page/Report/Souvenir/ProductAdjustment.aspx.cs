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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {

            kw.Focus(); //Keyword

            //DLC
            DLC.CompanyBrand(brand);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        var rs = from p in db.AdjustmentSouvenir_Product
                 where
                    (
                    p.Souvenir.Nama.Contains(kw.Text) ||
                    p.Souvenir.Kode.Contains(kw.Text) ||
                    p.Souvenir.Keterangan.Contains(kw.Text) ||
                    p.AdjustmentSouvenir.Nomor.Contains(kw.Text)
                    )
                 select p;

        
        
        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand != null && Brand > 0)
            rs = rs.Where(p => p.Souvenir.IDBrand == Brand);

        rs = rs.OrderByDescending(p => p.AdjustmentSouvenir.OperatorTgl);

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
                td.Text = r.AdjustmentSouvenir.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.AdjustmentSouvenir.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.AdjustmentSouvenir.TipeLokasi.Value);
                tr.Cells.Add(td);

                string Lokasi = "";
                if(r.AdjustmentSouvenir.TipeLokasi.Value == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.AdjustmentSouvenir.IDLokasi, Db.Cnn_DB);
                if (r.AdjustmentSouvenir.TipeLokasi.Value == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.AdjustmentSouvenir.IDLokasi, Db.Cnn_DB);
                if (r.AdjustmentSouvenir.TipeLokasi.Value == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.AdjustmentSouvenir.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Kode;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.AdjustmentSouvenir.StatusAdjustment == 0 ? "In Hand" : "Broken";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Qty.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.AdjustmentSouvenir.Keterangan;
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
            kw.Text = Request.QueryString["kw"];
            brand.SelectedValue = Request.QueryString["brand"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "ProductAdjustmentPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.SelectedValue;

        Response.Redirect(href);
    }
}