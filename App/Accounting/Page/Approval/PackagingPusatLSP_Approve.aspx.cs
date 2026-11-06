using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Approval_PackagingPusatLSP_Approve : System.Web.UI.Page
{
    private Map_AC _ac;

    private Map_AC ac
    {
        get
        {
            if (_ac == null)
                _ac = new Map_AC();
            return _ac;
        }
    }

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
        if (ac != null)
            ac.Dispose();
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        save();
    }
    protected void save()
    {
        string msg = "";
        try
        {
			string NoPackaging = Request.QueryString["nopck"];
            /*var pk = db.SalesOrder.SingleOrDefault(p => p.Nomor == NoPackaging);
            if(pk != null)
            {
                var sop = pk.SalesOrderPackaging;
                foreach (var item in sop) 
                {
                    var s = db.StockActualPackaging.SingleOrDefault(p => p.IDLokasi == item.SalesOrder.IDLokasi && p.TipeLokasi == item.SalesOrder.TipeLokasi && p.IDProduct == item.IDProduct && p.InHand > 0);
                    s.InHand = s.InHand - (item.Qty ?? 0m);
                    db.SubmitChanges();
                }
            }*/
			
            var user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            string query = "UPDATE FormApprovalPackaging SET ApprovalPackagingPusat = '" + user.Nama + "', ApprovalPackagingPusatNIK = '" + user.UserNumber + "' WHERE NomorPackaging = '" + NoPackaging + "'";
            Db.Execute(query, Db.Cnn_DB);
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        Response.Redirect("PackagingPusatLSP.aspx");
    }
}