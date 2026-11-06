using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_LD : System.Web.UI.Page {
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

    protected int id { get { return App.QueryInt(this, "id"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            fillTable();
            
		}
        
	}

    //Display Table
    protected void fillTable()
    {
        title.InnerText = "Audit ID : " + id.ToString();

        

        var d = db.StockAuditLD.SingleOrDefault(p => p.ID == id && p.StatusFinish.Value == true);

        if (d != null)
        {
            var sai = d.StockAuditItemLD; //StockAuditItemLD
            var sadtl = d.StockAuditLDDetail; //StockAuditLDDetail

            string NamaLokasi = "";
            string TipeLokasi = DL.LocationType(d.TipeLokasi);
            if (Convert.ToInt32(d.IDLokasi) == 0)
                NamaLokasi = "ALL";
            else
            {
                if (d.TipeLokasi == 0)
                {
                    var w = db.LocWarehouse.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = w.Nama;
                }
                if (d.TipeLokasi == 1)
                {
                    var o = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = o.Nama;
                }
                if (d.TipeLokasi == 2)
                {
                    var exh = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = exh.Nama;
                }
            }

            title2.InnerText = TipeLokasi + " " + NamaLokasi;
            title3.InnerText = Str.TglJam(d.OperatorTgl) + " - " + Str.TglJam(d.FinishJam);

            int index = 1;

            TableRow tr = new TableRow();
            TableCell td;

            int err1 = 0; // Fisik Ada, data tidak ada
            int err2 = 0; // Fisik Tidak Ada, data ada
            int err3 = 0; // Beda Gambar
            int err4 = 0; // Beda Bandrol


            td = new TableCell();
            td.Text = "<a href=\"StockAuditLD_DetailList.aspx?id=" + id.ToString() + "&tipe=5\"" + ">" + Convert.ToDecimal(sai.Count()).ToString("N0") + "</a>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<a href=\"StockAuditLD_DetailList.aspx?id=" + id.ToString() + "&tipe=6\"" + ">" + Convert.ToDecimal(d.TotalItemAudit).ToString("N0") + "</a>";
            tr.Cells.Add(td);

            err1 = (from c in sadtl
                   where !(from o in sai
                           select o.IDProduct)
                          .Contains(c.IDProduct)
                   select c).Count();

            td = new TableCell();
            td.Text = "<a href=\"StockAuditLD_DetailList.aspx?id=" + id.ToString() + "&tipe=1\"" + ">" + err1.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            var rs = from p in sai
                     where p.AuditID == id
                     && !(from z in sadtl
                          where z.AuditID == id
                          select z.IDProduct).Contains(p.IDProduct)
                     select p;
            err2 = rs.Count();

            td = new TableCell();
            td.Text = "<a href=\"StockAuditLD_DetailList.aspx?id=" + id.ToString() + "&tipe=2\"" + ">" + err2.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            err3 = (from p in sadtl
                          where p.ImgCheck == true
                          select p).Count();


            td = new TableCell();
            td.Text = "<a href=\"StockAuditLD_DetailList.aspx?id=" + id.ToString() + "&tipe=3\"" + ">" + err3.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            err4 = (from p in sadtl
                           where p.BandrolCheck == true
                           select p).Count();


            td = new TableCell();
            td.Text = "<a href=\"StockAuditLD_DetailList.aspx?id=" + id.ToString() + "&tipe=\"" + ">" + err4.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            var src5 = (from p in sadtl
                        select p);

            var ctr5 = src5.GroupBy(p => p.IDProduct);

            ctr5 = ctr5.Where(p => p.Count() > 1);

            td = new TableCell();
            td.Text = "<a href=\"StockAuditLD_DetailList.aspx?id=" + id.ToString() + "&tipe=8\"" + ">" + ctr5.Count().ToString("N0") + "</a>";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

        }
        else
            Response.Redirect("StockAuditLD.aspx");

    }
}