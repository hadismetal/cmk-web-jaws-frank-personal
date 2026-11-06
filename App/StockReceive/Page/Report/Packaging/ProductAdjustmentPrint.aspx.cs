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

            //Generate
            fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string Kw = "";
        Kw = Request.QueryString["kw"];
        var rs = from p in db.AdjustmentPackaging_Product
                 where
                    (
                    p.Packaging.Nama.Contains(Kw) ||
                    p.Packaging.Kode.Contains(Kw) ||
                    p.Packaging.Keterangan.Contains(Kw) ||
                    p.AdjustmentPackaging.Nomor.Contains(Kw)
                    )
                 select p;

        
        
        int Brand = Convert.ToInt32(Request.QueryString["brand"]);
        if (Brand != null && Brand > 0)
        {
            rs = rs.Where(p => p.Packaging.IDBrand == Brand);
            string BrandName = "";
            BrandName = Db.SingleString("SELECT Nama FROM CompanyBrand WHERE ID=" + Brand,Db.Cnn_DB);
            brand.InnerText = BrandName;
        }
        else
            brand.InnerText = "All";

        rs = rs.OrderByDescending(p => p.AdjustmentPackaging.OperatorTgl);

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
                td.Text = r.AdjustmentPackaging.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.AdjustmentPackaging.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.AdjustmentPackaging.TipeLokasi.Value);
                tr.Cells.Add(td);

                string Lokasi = "";
                if(r.AdjustmentPackaging.TipeLokasi.Value == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.AdjustmentPackaging.IDLokasi, Db.Cnn_DB);
                if (r.AdjustmentPackaging.TipeLokasi.Value == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.AdjustmentPackaging.IDLokasi, Db.Cnn_DB);
                if (r.AdjustmentPackaging.TipeLokasi.Value == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.AdjustmentPackaging.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Kode;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.AdjustmentPackaging.StatusAdjustment == 0 ? "In Hand" : "Broken";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Qty.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.AdjustmentPackaging.Keterangan;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
    }
}