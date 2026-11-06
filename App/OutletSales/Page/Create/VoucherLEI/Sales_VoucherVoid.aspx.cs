using System;
using System.Linq;

public partial class Sales_VoucherVoid : System.Web.UI.Page
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
    protected int myItemID { get { return App.QueryInt(this, "itemid"); } }
    protected int myItemType { get { return App.QueryInt(this, "itemtype"); } }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

        }

        Save(myID);
        Response.Redirect("Sales_Data.aspx?id=" + myID);

    }

    //Exception
    protected void DataError(int errnumber = 0) { Response.Redirect("Sales.aspx?err=" + errnumber); }
    
    protected void Save(int IDSO)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == IDSO);
        if (d == null) DataError(1);
        else
        {
            if (myItemType == 1)
            {
                var x = db.SalesOrderDJ.SingleOrDefault(p => p.ID == myItemID);
                if (x == null) DataError(2);
                else
                {
                    var y = db.SalesVoucher.Where(p => p.IDSalesOrder == IDSO && p.ProductType == myItemType && p.ProductID == x.IDProduct);
                    if (y == null) DataError(3);
                    else
                    {
                        x.DiscountGift = 0;
                        db.SubmitChanges();

                        foreach (var z in y)
                        {
                            string NomorVoucher = z.NomorVoucher;
                            var c = cs.T_voucher.SingleOrDefault(p => p.M_nomor == NomorVoucher);
                            if (c == null) DataError(4);
                            else
                            {
                                c.M_klaimdoc = "";
                                c.M_tanggal2 = null;
                                c.M_store2 = "";

                                cs.SubmitChanges();
                            }

                        }

                        //Commit
                        db.SalesVoucher.DeleteAllOnSubmit(y);
                        db.SubmitChanges();

                    }
                }
            }
            else if (myItemType == 2)
            {
                var x = db.SalesOrderPG.SingleOrDefault(p => p.ID == myItemID);
                if (x == null) DataError(2);
                else
                {
                    var y = db.SalesVoucher.Where(p => p.IDSalesOrder == IDSO && p.ProductType == myItemType && p.ProductID == x.IDProduct);
                    if (y == null) DataError(3);
                    else
                    {
                        x.DiscountGift = 0;
                        db.SubmitChanges();

                        foreach (var z in y)
                        {
                            string NomorVoucher = z.NomorVoucher;
                            var c = cs.T_voucher.SingleOrDefault(p => p.M_nomor == NomorVoucher);
                            if (c == null) DataError(4);
                            else
                            {
                                c.M_klaimdoc = "";
                                c.M_tanggal2 = null;
                                c.M_store2 = "";

                                cs.SubmitChanges();
                            }

                        }

                        //Commit
                        db.SalesVoucher.DeleteAllOnSubmit(y);
                        db.SubmitChanges();

                    }
                }
            }
            else if (myItemType == 3)
            {
                var x = db.SalesOrderGJ.SingleOrDefault(p => p.ID == myItemID);
                if (x == null) DataError(2);
                else
                {
                    var y = db.SalesVoucher.Where(p => p.IDSalesOrder == IDSO && p.ProductType == myItemType && p.ProductID == x.IDProduct);
                    if (y == null) DataError(3);
                    else
                    {
                        x.DiscountGift = 0;
                        db.SubmitChanges();

                        foreach (var z in y)
                        {
                            string NomorVoucher = z.NomorVoucher;
                            var c = cs.T_voucher.SingleOrDefault(p => p.M_nomor == NomorVoucher);
                            if (c == null) DataError(4);
                            else
                            {
                                c.M_klaimdoc = "";
                                c.M_tanggal2 = null;
                                c.M_store2 = "";

                                cs.SubmitChanges();
                            }

                        }

                        //Commit
                        db.SalesVoucher.DeleteAllOnSubmit(y);
                        db.SubmitChanges();

                    }
                }
            }
            else if (myItemType == 4)
            {
                var x = db.SalesOrderLD.SingleOrDefault(p => p.ID == myItemID);
                if (x == null) DataError(2);
                else
                {
                    var y = db.SalesVoucher.Where(p => p.IDSalesOrder == IDSO && p.ProductType == myItemType && p.ProductID == x.IDProduct);
                    if (y == null) DataError(3);
                    else
                    {
                        x.DiscountGift = 0;
                        db.SubmitChanges();

                        foreach (var z in y)
                        {
                            string NomorVoucher = z.NomorVoucher;
                            var c = cs.T_voucher.SingleOrDefault(p => p.M_nomor == NomorVoucher);
                            if (c == null) DataError(4);
                            else
                            {
                                c.M_klaimdoc = "";
                                c.M_tanggal2 = null;
                                c.M_store2 = "";

                                cs.SubmitChanges();
                            }

                        }

                        //Commit
                        db.SalesVoucher.DeleteAllOnSubmit(y);
                        db.SubmitChanges();

                    }
                }
            }

        }
    }

}