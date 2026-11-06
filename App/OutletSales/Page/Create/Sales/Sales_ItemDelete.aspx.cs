using System;
using System.Linq;

public partial class Page_Register_Sales_ItemDelete : System.Web.UI.Page
{
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (cs != null)
	        cs.Dispose();
	    base.Dispose();
	}
	protected int myID { get { return App.QueryInt(this); } }
	protected int myItemIndex { get { return App.QueryInt(this, "itemindex"); } }
	protected int myIID { get { return App.QueryInt(this, "iid"); } }

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }
    protected void LinkError(int IDSO, int IDTI, int IDRO)
    {
        Response.Redirect("Sales_Data.aspx?id=" + IDSO);
    }
    protected void LinkSuccess(int IDSO, int IDTI, int IDRO)
    {
        Response.Redirect("Sales_Data.aspx?id=" + IDSO);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else
        {
            int IDSO = d.ID;
            int IDTI = 0;
            int IDRO = 0;

            //Process			
            switch (myItemIndex)
            {
                case 0: saveDJ(IDSO, IDTI, IDRO); break;
                case 1: savePG(IDSO, IDTI, IDRO); break;
                case 2: saveGJ(IDSO, IDTI, IDRO); break;
                case 3: saveLD(IDSO, IDTI, IDRO); break;
                case 9: savePackaging(IDSO, IDTI, IDRO); break;
                case 10: saveSouvenir(IDSO, IDTI, IDRO); break;
                case 11: saveSouvenirGJ(IDSO, IDTI, IDRO); break;
                case 12: saveSouvenirDJ(IDSO, IDTI, IDRO); break;
                case 13: saveSouvenirLD(IDSO, IDTI, IDRO); break;
                case 14: saveSouvenirPG(IDSO, IDTI, IDRO); break;
                case 15: saveVoucher(IDSO, IDTI, IDRO); break;
            }

            //Redirect
            LinkSuccess(IDSO, IDTI, IDRO);

        }
	}
    protected void saveDJ(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {

            bool voidx = false;
            bool stampsNotActivate = false;

            if (STAMPS.xActivate)
            {
                var so = db.SalesOrder.Single(p => p.ID == IDSO);
                // STAMPS VOID VOUCHER
                //voidx = STAMPSM.VoidAllVoucherByPLU(so, 1, d.ID);
                voidx = true;
            }

            if ((STAMPS.xActivate && voidx) || stampsNotActivate)
            {

                cancelVoucher(IDSO, IDTI, IDRO, 1, d.IDProduct);

                var x = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProduct);
                if (x != null)
                    x.StatusPenjualan = 0;

                //Commit
                db.SalesOrderDJ.DeleteOnSubmit(d);
                db.SubmitChanges();
            }

        }
    }
    protected void savePG(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            cancelVoucher(IDSO, IDTI, IDRO, 2, d.IDProduct);

            var x = db.StockProductPG.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.SalesOrderPG.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveGJ(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            cancelVoucher(IDSO, IDTI, IDRO, 3, d.IDProduct);

            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.SalesOrderGJ.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveLD(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            cancelVoucher(IDSO, IDTI, IDRO, 4, d.IDProduct);

            var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.SalesOrderLD.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void savePackaging(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderPackaging.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            var x = db.StockActualPackaging.SingleOrDefault(p => p.ID == d.IDProduct && p.TipeLokasi == d.SalesOrder.TipeLokasi && p.IDLokasi == d.SalesOrder.IDLokasi);
            if (x != null)
                x.InHand = x.InHand + d.Qty.Value;

            //Commit
            db.SalesOrderPackaging.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveSouvenir(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            var x = db.StockActualSouvenir.SingleOrDefault(p => p.ID == d.IDProduct && p.TipeLokasi == d.SalesOrder.TipeLokasi && p.IDLokasi == d.SalesOrder.IDLokasi);
            if (x != null)
                x.InHand = x.InHand + d.Qty.Value;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    
    protected void saveSouvenirGJ(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID && p.TipeProduct == 2);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == d.IDProductSouvenir);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }

    protected void saveSouvenirDJ(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID && p.TipeProduct == 1);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProductSouvenir);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }

    protected void saveSouvenirLD(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID && p.TipeProduct == 4);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == d.IDProductSouvenir);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }

    protected void saveSouvenirPG(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == IDSO && p.ID == myIID && p.TipeProduct == 3);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == d.IDProductSouvenir);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }

    protected void saveVoucher(int IDSO, int IDTI, int IDRO)
    {
        var d = db.SalesVoucher.SingleOrDefault(p => p.IDSalesOrder == IDSO && p.ID == myIID);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            string xNomorVoucher = d.NomorVoucher;
            var c = cs.T_voucher.SingleOrDefault(p => p.M_nomor == xNomorVoucher);
            if (c == null) LinkError(IDSO, IDTI, IDRO);
            else
            {
                c.M_klaimdoc = "";
                c.M_tanggal2 = null;
                c.M_store2 = "";

                cs.SubmitChanges();
            }

            //Commit
            db.SalesVoucher.DeleteOnSubmit(d);
            db.SubmitChanges();

        }

    }

    protected void cancelVoucher(int IDSO, int IDTI, int IDRO, int iProductType, int iProductID)
    {
        var d = db.SalesVoucher.Where(p => p.ProductType == iProductType && p.ProductID == iProductID);
        if (d == null) LinkError(IDSO, IDTI, IDRO);
        else
        {
            if (d.Count() > 0)
            {
                foreach (var x in d)
                {

                    string xNomorVoucher = x.NomorVoucher;
                    var c = cs.T_voucher.SingleOrDefault(p => p.M_nomor == xNomorVoucher);
                    if (c == null) LinkError(IDSO, IDTI, IDRO);
                    else
                    {
                        c.M_klaimdoc = "";
                        c.M_tanggal2 = null;
                        c.M_store2 = "";

                        cs.SubmitChanges();
                    }

                }

                //Commit
                db.SalesVoucher.DeleteAllOnSubmit(d);
                db.SubmitChanges();



            }

        }
        
    }
}