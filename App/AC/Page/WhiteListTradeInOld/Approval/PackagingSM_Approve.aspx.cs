using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Approval_PackagingSM_Approve : System.Web.UI.Page
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

    public override void Dispose()
    {
        if (ac != null)
            ac.Dispose();
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
            var user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            string NoPackaging = Request.QueryString["nopck"];
            string query = "UPDATE FormApprovalPackaging SET ApprovalPackagingSM = '" + user.Nama + "', ApprovalPackagingSMNIK = '" + user.UserNumber + "' WHERE NomorPackaging = '" + NoPackaging + "'";
			test.InnerText = query;
            Db.Execute(query, Db.Cnn_DB);
        }
        catch(Exception ex)
        {
            msg = ex.Message;
        }
		Response.Redirect("PackagingSM.aspx?msg=" + msg);
    }
}