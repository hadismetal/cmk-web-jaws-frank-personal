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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            DLC.LocationList(cabang, tipe);
            cabang.Items.Remove(cabang.Items.FindByValue("-1"));
            
		}
	}

    protected void Next(object sender, EventArgs e)
    {
        /*string href = "LD_MoveOpnameItem.aspx?id=1";

        //Customize-Qstr
        href += "&cabang=" + cabang.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&tipe=" + tipe;

        Response.Redirect(href);*/

        //var rs = from p in db.StockProductLD_Stone1B
        //         where p.ReturnProduct == false && p.StatusPenjualan == 0
        //         select p;

        var ctr = from p in db.StockAuditLD
                  where p.OperatorTgl.Year == DateTime.Today.Year 
                  && p.OperatorTgl.Month == DateTime.Today.Month
                  && p.OperatorTgl.Day == DateTime.Today.Day
                  && p.TipeLokasi == tipe
                  orderby p.ID descending
                  select p;

        if (ctr.Count() > 0)
            Response.Redirect("LD_Opname.aspx?id=" + ctr.First().ID);
        else
        {

            var rs = from p in db.StockProductLD_Stone1B
                     where p.ReturnProduct == false
                               && !(from s in db.SalesOrderLD
                                    where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                    select s.IDProduct).Contains(p.ID)
                               && !(from s in db.SalesOrderSouvenir
                                    where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
                                    select s.IDProductSouvenir).Contains(p.ID)
                     select p;

            //rs = rs.OrderByDescending(p => p.Tgl);

            int Cabang = Convert.ToInt32(cabang.SelectedValue);
            if (Cabang > 0) rs = rs.Where(p => p.StockActualLD_Stone1B.IDLokasi == Cabang && p.StockActualLD_Stone1B.TipeLokasi == tipe);

            //rs = rs.Where(p => p.StockActualLD_Stone1B.InHand == 1);

            int AuditID = 0;

            if (rs.Count() > 0)
            {
                int index = 1;

                StockAuditLD sa = new StockAuditLD();

                sa.IDLokasi = Cabang;
                sa.TipeLokasi = tipe;
                sa.Nomor = "";
                sa.Operator = App.UserNama;
                sa.OperatorTgl = DateTime.Now;
                sa.Keterangan = "";
                sa.TotalItemAudit = rs.Count();
                sa.StatusFinish = false;

                db.StockAuditLD.InsertOnSubmit(sa);
                db.SubmitChanges();

                AuditID = Db.SingleInteger("SELECT TOP 1 ID FROM StockAuditLD ORDER BY ID DESC", Db.Cnn_DB);

                foreach (var r in rs)
                {

                    StockAuditItemLD i = new StockAuditItemLD();

                    i.AuditID = AuditID;
                    i.IDProduct = r.ID;
                    i.Nomor = r.Nomor;

                    db.StockAuditItemLD.InsertOnSubmit(i);
                    db.SubmitChanges();
                }
            }

            Response.Redirect("LD_Opname.aspx?id=" + AuditID);
        }
    }
}