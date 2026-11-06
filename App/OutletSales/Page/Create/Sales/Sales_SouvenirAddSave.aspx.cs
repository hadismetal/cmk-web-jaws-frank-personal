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
    protected int myItemIndexSouvenir { get { return App.QueryInt(this, "itemsouvenir"); } }
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
            //
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
                    catch (Exception ex) { }

                    SalesOrderSouvenir n = new SalesOrderSouvenir();

                    n.IDForm = myID;
                    n.ModalRupiah = Modal;
                    n.TotalRupiah = 0m;
                    n.Qty = 1m;
                    n.TotalModalRupiah = n.Qty * Modal;
                    if (myItemIndex == 0)
                    {
                        n.TipeProduct = 0;
                        n.IDProduct = myIDItem;    
                    }
                    if (myItemIndex == 1)
                    {
                        n.TipeProduct = 1;
                        n.IDProductDJ = myIDItem;
                    }
                    else if (myItemIndex == 2)
                    {
                        n.TipeProduct = 2;
                        n.IDProductPG = myIDItem;
                    }
                    else if (myItemIndex == 3)
                    {
                        n.TipeProduct = 3;
                        n.IDProductGJ = myIDItem;
                    }
                    else if (myItemIndex == 4)
                    {
                        n.TipeProduct = 4;
                        n.IDProductLD = myIDItem;
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