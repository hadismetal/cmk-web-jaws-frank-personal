using System;
using System.Linq;

public partial class Page_Register_Sales_ItemAddSave : System.Web.UI.Page
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
    protected int myItemIndex { get { return App.QueryInt(this, "item"); } }
    protected int myIDItem { get { return App.QueryInt(this, "iditem"); } }
    protected int myPLU { get { return App.QueryInt(this, "plu"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

        }

        Save();
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }


    protected void Save()
    {
        
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            if (myItemIndex == 0)
            {                
                var s = db.StockActualSouvenir.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem && p.InHand > 0);
                if (s == null) DataError();
                else
                {
                    var stock = from p in db.StockReceiveSouvenir_Product
                                where p.IDProduct == myIDItem
                                orderby p.StockReceiveSouvenir.OperatorTgl
                                select p;
                    decimal Modal = 0m;
                    try
                    {
                        Modal = Db.SingleDecimal("SELECT ISNULL(Harga, 0) FROM Souvenir WHERE ID = " + myIDItem, Db.Cnn_DB);
                    }
                    catch (Exception) { }

                    SalesOrderSouvenir n = new SalesOrderSouvenir();

                    n.IDForm = myID;
                    n.IDProduct = myIDItem;
                    n.TipeProduct = myItemIndex;
                    n.ModalRupiah = Modal;
                    n.TotalRupiah = 0m;
                    n.Qty = 1m;
                    n.TotalModalRupiah = n.Qty * Modal;
                    if (myPLU > 0)
                    {
                        if (myItemIndex == 1)
                            n.IDProductDJ = myPLU;
                        else if (myItemIndex == 2)
                            n.IDProductPG = myPLU;
                        else if (myItemIndex == 3)
                            n.IDProductGJ = myPLU;
                        else if (myItemIndex == 4)
                            n.IDProductLD = myPLU;
                    }

                    db.SalesOrderSouvenir.InsertOnSubmit(n);
                    db.SubmitChanges();

                    n.Invoice = Document.Generate_Nomor_SalesOrderSouvenir(n);
                    db.SubmitChanges();

                }

                Response.Redirect("Sales_Data.aspx?id=" + myID);
            }

            else if (myItemIndex == 1)
            {
                var s = db.StockActualDJ.SingleOrDefault(p => p.IDLokasi == d.IDLokasi 
                    && 
                    p.TipeLokasi == d.TipeLokasi 
                    && 
                    p.IDProduct == myIDItem 
                    && p.InHand > 0);
                if (s == null) DataError();
                else
                {
                    decimal Modal = 0m;
                    try
                    {
                        Modal = s.StockProductDJ.StockProductDJ_PricingMU.ModalRupiah;
                    }
                    catch (Exception) { }

                    SalesOrderSouvenir n = new SalesOrderSouvenir();

                    n.IDForm = myID;
                    n.IDProductSouvenir = myIDItem;
                    n.TipeProduct = myItemIndex;
                    n.ModalRupiah = Modal;
                    n.TotalRupiah = 0m;
                    n.Qty = 1m;
                    //s.InHand = s.InHand - 1;
                    s.StockProductDJ.StatusPenjualan = 1;
                    if (myPLU > 0)
                    {
                        if (myItemIndex == 1)
                            n.IDProductDJ = myPLU;
                        else if (myItemIndex == 2)
                            n.IDProductPG = myPLU;
                        else if (myItemIndex == 3)
                            n.IDProductGJ = myPLU;
                        else if (myItemIndex == 4)
                            n.IDProductLD = myPLU;
                    }

                    db.SalesOrderSouvenir.InsertOnSubmit(n);
                    db.SubmitChanges();

                    n.Invoice = Document.Generate_Nomor_SalesOrderSouvenir(n);
                    db.SubmitChanges();

                }
                Response.Redirect("Sales_Data.aspx?id=" + myID);
            }

            else if (myItemIndex == 2)
            {
                var s = db.StockActualGJ.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem && p.InHand > 0);
                if (s == null) DataError();
                else
                {
                    decimal Modal = 0m;
                    try
                    {
                        Modal = s.StockProductGJ.StockProductGJ_PricingMU.ModalRupiah;
                    }
                    catch (Exception) { }

                    SalesOrderSouvenir n = new SalesOrderSouvenir();

                    n.IDForm = myID;
                    n.IDProductSouvenir = myIDItem;
                    n.TipeProduct = myItemIndex;
                    n.ModalRupiah = Modal;
                    n.TotalRupiah = 0m;
                    n.Qty = 1m;
                    //s.InHand = s.InHand - 1;
                    s.StockProductGJ.StatusPenjualan = 1;
                    if (myPLU > 0)
                    {
                        if (myItemIndex == 1)
                            n.IDProductDJ = myPLU;
                        else if (myItemIndex == 2)
                            n.IDProductPG = myPLU;
                        else if (myItemIndex == 3)
                            n.IDProductGJ = myPLU;
                        else if (myItemIndex == 4)
                            n.IDProductLD = myPLU;
                    }

                    db.SalesOrderSouvenir.InsertOnSubmit(n);
                    db.SubmitChanges();

                    n.Invoice = Document.Generate_Nomor_SalesOrderSouvenir(n);
                    db.SubmitChanges();

                }
                Response.Redirect("Sales_Data.aspx?id=" + myID);
            }

            else if (myItemIndex == 3)
            {
                var s = db.StockActualPG.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem && p.InHand > 0);
                if (s == null) DataError();
                else
                {
                    decimal Modal = 0m;
                    try
                    {
                        Modal = s.StockProductPG.TotalHarga24.Value;
                    }
                    catch (Exception) { }

                    SalesOrderSouvenir n = new SalesOrderSouvenir();

                    n.IDForm = myID;
                    n.IDProductSouvenir = myIDItem;
                    n.TipeProduct = myItemIndex;
                    n.ModalRupiah = Modal;
                    n.TotalRupiah = 0m;
                    n.Qty = 1m;
                    //s.InHand = s.InHand - 1;
                    s.StockProductPG.StatusPenjualan = 1;
                    if (myPLU > 0)
                    {
                        if (myItemIndex == 1)
                            n.IDProductDJ = myPLU;
                        else if (myItemIndex == 2)
                            n.IDProductPG = myPLU;
                        else if (myItemIndex == 3)
                            n.IDProductGJ = myPLU;
                        else if (myItemIndex == 4)
                            n.IDProductLD = myPLU;
                    }

                    db.SalesOrderSouvenir.InsertOnSubmit(n);
                    db.SubmitChanges();

                    n.Invoice = Document.Generate_Nomor_SalesOrderSouvenir(n);
                    db.SubmitChanges();

                }
                Response.Redirect("Sales_Data.aspx?id=" + myID);
            }

            else if (myItemIndex == 4)
            {
                var s = db.StockActualLD_Stone1B.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem && p.InHand > 0);
                if (s == null) DataError();
                else
                {
                    decimal Modal = 0m;
                    try
                    {
                        Modal = s.StockProductLD_Stone1B.HargaTotalM.Value;
                    }
                    catch (Exception) { }

                    SalesOrderSouvenir n = new SalesOrderSouvenir();

                    n.IDForm = myID;
                    n.IDProductSouvenir = myIDItem;
                    n.TipeProduct = myItemIndex;
                    n.ModalRupiah = Modal;
                    n.TotalRupiah = 0m;
                    n.Qty = 1m;
                    s.StockProductLD_Stone1B.StatusPenjualan = 1;
                    if (myPLU > 0)
                    {
                        if (myItemIndex == 1)
                            n.IDProductDJ = myPLU;
                        else if (myItemIndex == 2)
                            n.IDProductPG = myPLU;
                        else if (myItemIndex == 3)
                            n.IDProductGJ = myPLU;
                        else if (myItemIndex == 4)
                            n.IDProductLD = myPLU;
                    }

                    db.SalesOrderSouvenir.InsertOnSubmit(n);
                    db.SubmitChanges();

                    n.Invoice = Document.Generate_Nomor_SalesOrderSouvenir(n);
                    db.SubmitChanges();
                }
                Response.Redirect("Sales_Data.aspx?id=" + myID);
            }
        }
    }

}