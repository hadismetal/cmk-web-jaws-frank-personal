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
                    p.AdjustmentSouvenir.Nomor.Contains(kw.Text)
                    )
                 select p;
        if (status.SelectedValue == "1")
            rs = rs.Where(p => p.AdjustmentSouvenir.Status == 1);
        else if(status.SelectedValue == "2")
            rs = rs.Where(p=>p.AdjustmentSouvenir.Status == 2);
        else if(status.SelectedValue == "3")
            rs = rs.Where(p=>p.AdjustmentSouvenir.Draft.Value == true);

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
                td.Text = r.Souvenir.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Kode;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.AdjustmentSouvenir.TipeLokasi.Value);
                tr.Cells.Add(td);

                string Lokasi = "";
                if(r.AdjustmentSouvenir.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.AdjustmentSouvenir.IDLokasi, Db.Cnn_DB);
                if(r.AdjustmentSouvenir.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.AdjustmentSouvenir.IDLokasi, Db.Cnn_DB);
                if(r.AdjustmentSouvenir.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.AdjustmentSouvenir.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
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
            status.SelectedValue = Request.QueryString["status"];
            kw.Text = Request.QueryString["kw"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&status=" + status.Text;
        href += "&kw=" + kw.Text;
        href += "&tipe=" + tipe;

        Response.Redirect(href);
    }
}