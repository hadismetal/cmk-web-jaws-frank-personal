using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_SalesFee : System.Web.UI.Page {
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
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
            
            //Generate
            fillFilter();

            tahun.Text = (Request.QueryString["tahun"] == null) ? DateTime.Now.Year.ToString() : Request.QueryString["tahun"];
            bulan.Text = (Request.QueryString["bulan"] == null) ? DateTime.Now.Month.ToString() : Request.QueryString["bulan"];

            
		}
        fillTable(sender, e);
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesFee
                 select p;

        int Tahun = (tahun.Text != "") ? Convert.ToInt32(tahun.Text) : 0;
        int Bulan = (bulan.Text != "" ) ? Convert.ToInt32(bulan.Text) : 0;
        int index = 1;

        if (Tahun > 0)
            rs = rs.Where(p => p.Tahun == Tahun);
        if (Bulan > 0)
            rs = rs.Where(p => p.Bulan == Bulan);
        
        rs = rs.OrderByDescending(p=> p.Tahun).ThenByDescending(p=>p.Bulan);

        foreach(var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Tahun.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Bulan.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.TotalPoint.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.FullMoney.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.AE.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.DJ.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.PG.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.LM.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.PC1.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.PC2.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.PC3.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.PC4.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Inc1.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Inc2.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Inc3.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Inc4.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Kom1.Value.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Kom2.Value.ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            index++;
        }

    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            tahun.Text = Request.QueryString["tahun"];
            bulan.Text = Request.QueryString["bulan"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&tahun=" + tahun.Text;
        href += "&bulan=" + bulan.Text;

        Response.Redirect(href);
    }

}
