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

		if (!IsPostBack) 
		{
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Souvenir_Export.xls");

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
                    p.Souvenir.Nama.Contains(Request.QueryString["kw"]) ||
                    p.Souvenir.Kode.Contains(Request.QueryString["kw"]) ||
                    p.Souvenir.Keterangan.Contains(Request.QueryString["kw"])
                    )
                 select p;

        rs = rs.OrderByDescending(p => p.Souvenir.Tgl);

        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        if (Cabang > 0)
            rs = rs.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
        else
            rs = rs.Where(p => p.TipeLokasi == tipe);

        int Brand = Convert.ToInt32(Request.QueryString["brand"]);
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