using System;
using System.Linq;

public partial class Page_Register_Titipan_ItemAddSave : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

    protected int myID { get { return App.QueryInt(this); } }
    protected int myItemIndex { get { return App.QueryInt(this, "item"); } }
    protected int myIDItem { get { return App.QueryInt(this, "iditem"); } }

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
    protected void DataError() { Response.Redirect("Titipan.aspx"); }


    protected void Save()
    {
        int itemid = 0;
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            if (myItemIndex == 1)
            {
                var s = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == myIDItem && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                if (s == null) DataError();
                else
                {
                    
                        DocTitipan_Product n = new DocTitipan_Product();

                        n.IDForm = myID;
                        n.IDProduct = s.IDProduct;
                        

                        
                        db.DocTitipan_Product.InsertOnSubmit(n);
                        db.SubmitChanges();

                        itemid = n.ID;
                }
            }
            Response.Redirect("Titipan_Picture.aspx?id=" + myID + "&itemid=" + itemid);
        }
    }

}