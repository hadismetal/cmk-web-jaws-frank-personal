using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page {
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
        /*string href = "DJ_MoveOpnameItem.aspx?id=1";

        //Customize-Qstr
        href += "&cabang=" + cabang.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&tipe=" + tipe;

        Response.Redirect(href);*/

        //var rs = from p in db.StockProductDJ
        //         where p.ReturnProduct == false && p.StatusLebur == false
        //                   && !(from s in db.SalesOrderDJ
        //                        where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
        //                        select s.IDProduct).Contains(p.ID)
        //         select p;

        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        int Cabang = Convert.ToInt32(cabang.SelectedValue);

        var ctr = from p in db.StockAuditDJ
                  where p.OperatorTgl.Year == DateTime.Today.Year 
                  && p.OperatorTgl.Month == DateTime.Today.Month
                  && p.OperatorTgl.Day == DateTime.Today.Day
                  && p.TipeLokasi == tipe && p.IDLokasi == Cabang
                  orderby p.ID descending
                  select p;

        if (ctr.Count() > 0)
            Response.Redirect("DJ_Opname.aspx?id=" + ctr.First().ID);
        else
        {

            var rs = from p in db.StockActualDJ
                     where p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusLebur == false && !p.StockProductDJ.Draft.Value
                               && !(from s in db.SalesOrderDJ
                                    where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                    select s.IDProduct).Contains(p.IDProduct)
                               && !(from s in db.SalesOrderSouvenir
                                    where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                    select s.IDProductSouvenir).Contains(p.IDProduct)
                     select p;

            rs = rs.OrderByDescending(p => p.StockProductDJ.Tgl);


            if (Cabang > 0) rs = rs.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);

            if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);

            //rs = rs.Where(p => p.StockActualDJ.InHand == 1);

            int AuditID = 0;

            if (rs != null)
            {
                int index = 1;

                StockAuditDJ sa = new StockAuditDJ();

                sa.IDLokasi = Cabang;
                sa.TipeLokasi = tipe;
                sa.Nomor = "";
                sa.Operator = App.UserNama;
                sa.OperatorTgl = DateTime.Now;
                sa.Keterangan = "";
                sa.TotalItemAudit = rs.Count();
                sa.StatusFinish = false;

                db.StockAuditDJ.InsertOnSubmit(sa);
                db.SubmitChanges();

                AuditID = Db.SingleInteger("SELECT TOP 1 ID FROM StockAuditDJ ORDER BY ID DESC", Db.Cnn_DB);

                foreach (var r in rs)
                {
                    StockAuditItemDJ i = new StockAuditItemDJ();

                    i.AuditID = AuditID;
                    i.IDProduct = r.IDProduct;
                    i.Nomor = r.Nomor;

                    db.StockAuditItemDJ.InsertOnSubmit(i);
                    db.SubmitChanges();
                }
            }

            Response.Redirect("DJ_Opname.aspx?id=" + AuditID);
        }
    }
}