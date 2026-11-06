using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_GJ : System.Web.UI.Page {
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
        /*string href = "GJ_MoveOpnameItem.aspx?id=1";

        //Customize-Qstr
        href += "&cabang=" + cabang.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&tipe=" + tipe;

        Response.Redirect(href);*/

        //var rs = from p in db.StockProductGJ
        //         where p.ReturnProduct == false && p.StatusLebur == false
        //                   && !(from s in db.SalesOrderGJ
        //                        where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
        //                        select s.IDProduct).Contains(p.ID)
        //         select p;

        var ctr = from p in db.StockAuditGJ
                  where p.OperatorTgl.Year == DateTime.Today.Year 
                  && p.OperatorTgl.Month == DateTime.Today.Month
                  && p.OperatorTgl.Day == DateTime.Today.Day
                  && p.TipeLokasi == tipe
                  orderby p.ID descending
                  select p;

        if (ctr.Count() > 0)
            Response.Redirect("GJ_Opname.aspx?id=" + ctr.First().ID);
        else
        {

            var rs = from p in db.StockActualGJ
                     where p.StockProductGJ.ReturnProduct == false && p.StockProductGJ.StatusLebur == false
                               && !(from s in db.SalesOrderGJ
                                    where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                                    select s.IDProduct).Contains(p.IDProduct)
                               && !(from s in db.SalesOrderSouvenir
                                    where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 2
                                    select s.IDProductSouvenir).Contains(p.IDProduct)
                     select p;

            rs = rs.OrderByDescending(p => p.StockProductGJ.Tgl);

            int Cabang = Convert.ToInt32(cabang.SelectedValue);
            if (Cabang > 0) rs = rs.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
            int ProductItem = Convert.ToInt32(productitem.SelectedValue);
            if (ProductItem > 0) rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == ProductItem);

            //rs = rs.Where(p => p.StockActualGJ.InHand == 1);

            int AuditID = 0;

            if (rs != null)
            {
                int index = 1;

                StockAuditGJ sa = new StockAuditGJ();

                sa.IDLokasi = Cabang;
                sa.TipeLokasi = tipe;
                sa.Nomor = "";
                sa.Operator = App.UserNama;
                sa.OperatorTgl = DateTime.Now;
                sa.Keterangan = "";
                sa.TotalItemAudit = rs.Count();
                sa.StatusFinish = false;

                db.StockAuditGJ.InsertOnSubmit(sa);
                db.SubmitChanges();

                AuditID = Db.SingleInteger("SELECT TOP 1 ID FROM StockAuditGJ ORDER BY ID DESC", Db.Cnn_DB);

                foreach (var r in rs)
                {
                    StockAuditItemGJ i = new StockAuditItemGJ();

                    i.AuditID = AuditID;
                    i.IDProduct = r.IDProduct;
                    i.Nomor = r.Nomor;

                    db.StockAuditItemGJ.InsertOnSubmit(i);
                    db.SubmitChanges();
                }
            }

            Response.Redirect("GJ_Opname.aspx?id=" + AuditID);
        }
    }
}