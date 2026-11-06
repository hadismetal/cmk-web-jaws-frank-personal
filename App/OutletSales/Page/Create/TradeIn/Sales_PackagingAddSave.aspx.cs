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
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }
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
    protected void DataError() { Response.Redirect("TradeIn.aspx"); }


    protected void Save()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO);
        if (d == null) DataError();
        else
        {
            var s = db.StockActualPackaging.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem && p.InHand > 0);
            if (s == null) DataError();
            else
            {
                var stock = from p in db.StockReceivePackaging_Product
                            where p.IDProduct == myIDItem
                            orderby p.StockReceivePackaging.OperatorTgl
                            select p;
                decimal Modal = 0m;
                try{
                    Modal = stock.First().Harga.Value;
                }
                catch(Exception ex){}

                SalesOrderPackaging n = new SalesOrderPackaging();

                n.IDForm = myIDSO;
                n.IDProduct = myIDItem;
                n.ModalRupiah = Modal;
                n.TotalRupiah = 0m;
				n.Qty = 1m;
				n.TotalModalRupiah = n.Qty * Modal;
                db.SalesOrderPackaging.InsertOnSubmit(n);
                db.SubmitChanges();

                n.Invoice = Document.Generate_Nomor_SalesOrderPackaging(n);
                db.SubmitChanges();
            }

            Response.Redirect("TradeIn_Data.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO);
        }
    }

}