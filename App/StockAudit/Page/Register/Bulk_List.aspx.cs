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

        int IDDJ = 1;
        int IDPG = 1;
        int IDGJ = 1;
        int IDLD = 1;

        int Cabang = Convert.ToInt32(cabang.SelectedValue);

        var ctr = from p in db.StockAudit
                  where p.OperatorTgl.Year == DateTime.Today.Year 
                  && p.OperatorTgl.Month == DateTime.Today.Month
                  && p.OperatorTgl.Day == DateTime.Today.Day
                  && p.TipeLokasi == tipe && p.IDLokasi == Cabang
                  orderby p.ID descending
                  select p;

        if (ctr.Count() > 0)
            Response.Redirect("Bulk_Opname.aspx?id=" + ctr.First().ID);
        else
        {
            //DJ
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
                IDDJ = AuditID;

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

            //PG
            var rs2 = from p in db.StockActualPG
                      where p.StockProductPG.ReturnProduct == false && p.StockProductPG.StatusLebur == false && !p.StockProductPG.Draft.Value
                                && !(from s in db.SalesOrderPG
                                     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                     select s.IDProduct).Contains(p.IDProduct)
                                && !(from s in db.SalesOrderSouvenir
                                     where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                     select s.IDProductSouvenir).Contains(p.IDProduct)
                      select p;

            rs2 = rs2.OrderByDescending(p => p.StockProductPG.Tgl);


            if (Cabang > 0) rs2 = rs2.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);

            //rs2 = rs2.Where(p => p.StockActualPG.InHand == 1);

            if (rs2 != null)
            {
                int index = 1;

                StockAuditPG sa = new StockAuditPG();

                sa.IDLokasi = Cabang;
                sa.TipeLokasi = tipe;
                sa.Nomor = "";
                sa.Operator = App.UserNama;
                sa.OperatorTgl = DateTime.Now;
                sa.Keterangan = "";
                sa.TotalItemAudit = rs2.Count();
                sa.StatusFinish = false;

                db.StockAuditPG.InsertOnSubmit(sa);
                db.SubmitChanges();

                AuditID = Db.SingleInteger("SELECT TOP 1 ID FROM StockAuditPG ORDER BY ID DESC", Db.Cnn_DB);
                IDPG = AuditID;

                foreach (var r in rs2)
                {
                    StockAuditItemPG i = new StockAuditItemPG();

                    i.AuditID = AuditID;
                    i.IDProduct = r.IDProduct;
                    i.Nomor = r.Nomor;

                    db.StockAuditItemPG.InsertOnSubmit(i);
                    db.SubmitChanges();
                }
            }

            //GJ
            var rs3 = from p in db.StockActualGJ
                      where p.StockProductGJ.ReturnProduct == false && p.StockProductGJ.StatusLebur == false && !p.StockProductGJ.Draft.Value
                                && !(from s in db.SalesOrderGJ
                                     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                     select s.IDProduct).Contains(p.IDProduct)
                                && !(from s in db.SalesOrderSouvenir
                                     where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 2
                                     select s.IDProductSouvenir).Contains(p.IDProduct)
                      select p;

            rs3 = rs3.OrderByDescending(p => p.StockProductGJ.Tgl);


            if (Cabang > 0) rs3 = rs3.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);

            //rs3 = rs3.Where(p => p.StockActualGJ.InHand == 1);

            if (rs3 != null)
            {
                int index = 1;

                StockAuditGJ sa = new StockAuditGJ();

                sa.IDLokasi = Cabang;
                sa.TipeLokasi = tipe;
                sa.Nomor = "";
                sa.Operator = App.UserNama;
                sa.OperatorTgl = DateTime.Now;
                sa.Keterangan = "";
                sa.TotalItemAudit = rs3.Count();
                sa.StatusFinish = false;

                db.StockAuditGJ.InsertOnSubmit(sa);
                db.SubmitChanges();

                AuditID = Db.SingleInteger("SELECT TOP 1 ID FROM StockAuditGJ ORDER BY ID DESC", Db.Cnn_DB);
                IDGJ = AuditID;

                foreach (var r in rs3)
                {
                    StockAuditItemGJ i = new StockAuditItemGJ();

                    i.AuditID = AuditID;
                    i.IDProduct = r.IDProduct;
                    i.Nomor = r.Nomor;

                    db.StockAuditItemGJ.InsertOnSubmit(i);
                    db.SubmitChanges();
                }
            }

            //LD
            var rs4 = from p in db.StockActualLD_Stone1B
                      where p.StockProductLD_Stone1B.ReturnProduct == false
                                && !(from s in db.SalesOrderLD
                                     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                     select s.IDProduct).Contains(p.IDProduct)
                                && !(from s in db.SalesOrderSouvenir
                                     where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
                                     select s.IDProductSouvenir).Contains(p.IDProduct)
                      select p;

            //rs4 = rs4.OrderByDescending(p => p.StockProductLD_Stone1B.Tgl);


            if (Cabang > 0) rs4 = rs4.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);

            //rs4 = rs4.Where(p => p.StockActualLD_Stone1B.InHand == 1);

            if (rs4 != null)
            {
                int index = 1;

                StockAuditLD sa = new StockAuditLD();

                sa.IDLokasi = Cabang;
                sa.TipeLokasi = tipe;
                sa.Nomor = "";
                sa.Operator = App.UserNama;
                sa.OperatorTgl = DateTime.Now;
                sa.Keterangan = "";
                sa.TotalItemAudit = rs4.Count();
                sa.StatusFinish = false;

                db.StockAuditLD.InsertOnSubmit(sa);
                db.SubmitChanges();

                AuditID = Db.SingleInteger("SELECT TOP 1 ID FROM StockAuditLD ORDER BY ID DESC", Db.Cnn_DB);
                IDLD = AuditID;

                foreach (var r in rs4)
                {
                    StockAuditItemLD i = new StockAuditItemLD();

                    i.AuditID = AuditID;
                    i.IDProduct = r.IDProduct;
                    i.Nomor = r.Nomor;

                    db.StockAuditItemLD.InsertOnSubmit(i);
                    db.SubmitChanges();
                }
            }

            StockAudit t = new StockAudit();
            t.IDLokasi = Cabang;
            t.TipeLokasi = tipe;
            t.Nomor = "";
            t.Operator = App.UserNama;
            t.OperatorTgl = DateTime.Now;
            t.Keterangan = "";
            t.TotalItemAudit = rs.Count() + rs2.Count() + rs3.Count() + rs4.Count();
            t.StatusFinish = false;

            db.StockAudit.InsertOnSubmit(t);
            db.SubmitChanges();

            StockAudit_Detail det = new StockAudit_Detail();
            det.IDForm = t.ID;
            det.IDDJ = IDDJ;
            det.IDGJ = IDGJ;
            det.IDPG = IDPG;
            det.IDLD = IDLD;

            db.StockAudit_Detail.InsertOnSubmit(det);
            db.SubmitChanges();

            Response.Redirect("Bulk_Opname.aspx?id=" + t.ID);
        }
    }
}