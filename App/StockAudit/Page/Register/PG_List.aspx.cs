using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_PG : System.Web.UI.Page {
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
            DLC.CharProductItem(productitem);
            DLC.LocationList(cabang, tipe);
            cabang.Items.Remove(cabang.Items.FindByValue("-1"));
		}
	}

    protected void Next(object sender, EventArgs e)
    {

        //var rs = from p in db.StockProductPG
        //         where p.Draft == false && p.Approval != null && p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
        //         select p;

        var ctr = from p in db.StockAuditPG
                  where p.OperatorTgl.Year == DateTime.Today.Year 
                  && p.OperatorTgl.Month == DateTime.Today.Month
                  && p.OperatorTgl.Day == DateTime.Today.Day
                  && p.TipeLokasi == tipe
                  orderby p.ID descending
                  select p;

        if (ctr.Count() > 0)
            Response.Redirect("PG_Opname.aspx?id=" + ctr.First().ID);
        else
        {

            var rs = from p in db.StockActualPG
                     where p.StockProductPG.ReturnProduct == false && p.StockProductPG.StatusLebur == false
                               && !(from s in db.SalesOrderPG
                                    where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                    select s.IDProduct).Contains(p.IDProduct)
                               && !(from s in db.SalesOrderSouvenir
                                    where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                    select s.IDProductSouvenir).Contains(p.IDProduct)
                     select p;

            rs = rs.OrderByDescending(p => p.Tgl);

            int Cabang = Convert.ToInt32(cabang.SelectedValue);
            if (Cabang > 0) rs = rs.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
            int ProductItem = Convert.ToInt32(productitem.SelectedValue);
            if (ProductItem > 0) rs = rs.Where(p => p.StockProductPG.ProductItem == ProductItem);

            //rs = rs.Where(p => p.StockActualPG.InHand == 1);

            int AuditID = 0;

            if (rs != null)
            {
                int index = 1;

                StockAuditPG sa = new StockAuditPG();

                sa.IDLokasi = Cabang;
                sa.TipeLokasi = tipe;
                sa.Nomor = "";
                sa.Operator = App.UserNama;
                sa.OperatorTgl = DateTime.Now;
                sa.Keterangan = "";
                sa.TotalItemAudit = rs.Count();
                sa.StatusFinish = false;

                db.StockAuditPG.InsertOnSubmit(sa);
                db.SubmitChanges();

                AuditID = Db.SingleInteger("SELECT TOP 1 ID FROM StockAuditPG ORDER BY ID DESC", Db.Cnn_DB);

                foreach (var r in rs)
                {
                    StockAuditItemPG i = new StockAuditItemPG();

                    i.AuditID = AuditID;
                    i.IDProduct = r.IDProduct;
                    i.Nomor = r.Nomor;

                    db.StockAuditItemPG.InsertOnSubmit(i);
                    db.SubmitChanges();
                }
            }

            Response.Redirect("PG_Opname.aspx?id=" + AuditID);
        }
    }
}