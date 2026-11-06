using System;
using System.Linq;

public partial class Page_Approval_TradeIn_Void : System.Web.UI.Page
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

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    App price = new App();

    decimal Setting = 0m;

    protected int myID { get { return App.QueryInt(this); } }

    //Exception
    protected void DeleteError(string sebab)
    {
        Response.Redirect("TradeIn.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        var d = db.TradeIn.SingleOrDefault(p => p.ID == myID && (p.StatusVoid == null || p.StatusVoid == 0));
        if (d == null) App.DataNull(this);
        else
        {
            //Delete
            voidProcess(d);

            d.StatusVoid = 1;
            db.SubmitChanges();

            //Redirect
            Response.Redirect("TradeIn.aspx?d=1");
        }
    }

    //Delete
    protected void voidProcess(TradeIn d)
    {

        var dj = d.Resell.ResellDJ;
        var pg = d.Resell.ResellPG;
        var gj = d.Resell.ResellGJ;
        var ld = d.Resell.ResellLD;
         
        foreach (var r in dj)
        {
            var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductDJ.StockActualDJ.Substorage = 1;
                x.StockProductDJ.StockActualDJ.InHand = 0;
                db.SubmitChanges();
            }     
        }




        foreach (var r in pg)
        {
            var x = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductPG.StockActualPG.Substorage = 1;
                x.StockProductPG.StockActualPG.InHand = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in gj)
        {
            var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductGJ.StockActualGJ.Substorage = 1;
                x.StockProductGJ.StockActualGJ.InHand = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in ld)
        {
            var x = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 1;
                x.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand = 0;
                db.SubmitChanges();
            }
        }

        var sdj = d.SalesOrder.SalesOrderDJ;
        var spg = d.SalesOrder.SalesOrderPG;
        var sgj = d.SalesOrder.SalesOrderGJ;
        var sld = d.SalesOrder.SalesOrderLD;
        var spc = d.SalesOrder.SalesOrderPackaging;
        var svr = d.SalesOrder.SalesOrderSouvenir;

        foreach (var r in sdj)
        {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                x.StockActualDJ.InHand = 1;
                x.StockActualDJ.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in spg)
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                x.StockActualPG.InHand = 1;
                x.StockActualPG.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in sgj)
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                x.StockActualGJ.InHand = 1;
                x.StockActualGJ.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in sld)
        {
            var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                x.StockActualLD_Stone1B.InHand = 1;
                x.StockActualLD_Stone1B.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in spc)
        {
            var x = db.StockActualPackaging.SingleOrDefault(p => p.ID == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
            if (x != null)
            {
                x.InHand = x.InHand + r.Qty.Value;
                
                db.SubmitChanges();
            }
        }

        foreach (var r in svr)
        {
            if (r.TipeProduct == 0)
            {
                var x = db.StockActualSouvenir.SingleOrDefault(p => p.ID == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
                if (x != null)
                {
                    x.InHand = x.InHand + r.Qty.Value;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 1)
            {
                var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
                    x.StatusPenjualan = 0;
                    x.StockActualDJ.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 2)
            {
                var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
                    x.StatusPenjualan = 0;
                    x.StockActualGJ.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 3)
            {
                var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
                    x.StatusPenjualan = 0;
                    x.StockActualPG.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 4)
            {
                var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
                    x.StatusPenjualan = 0;
                    x.StockActualLD_Stone1B.InHand = 1;
                    db.SubmitChanges();
                }
            }
        }

        foreach (var r in d.SalesOrder.SalesReceiptDPPO)
        {
            r.StatusDP = 0;
            r.IDSalesOrder = null;
            db.SubmitChanges();
        }

        d.StatusVoid = 1;
        d.Resell.StatusVoid = 1;
        d.SalesOrder.StatusVoid = true;
        db.SubmitChanges();
    }
}