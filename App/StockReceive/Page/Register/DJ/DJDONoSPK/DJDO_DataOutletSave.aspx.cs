using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJDO_DJDO_DataOutletSave : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }
    protected int myLink { get { return App.QueryInt(this, "link"); } }
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected string myNOBRG { get { return Request.QueryString["nobrg"]; } }
    protected string myNOVAL { get { return Request.QueryString["noval"]; } }
    protected string myNOBAG { get { return Request.QueryString["nobag"]; } }


    protected void DataError() { Response.Redirect("DJDO.aspx"); }
    protected void LinkError() { Response.Redirect("DJDO_DataOutlet.aspx?id=" + myID); }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            saveLink(d);

            //Redirect
            Response.Redirect("DJDO_Detail.aspx?id=" + myID + "&nodo=" + myNODO);
        }
    }
    protected void saveLink(StockReceiveDJ data)
    {
        var d = db.LocOutlet.SingleOrDefault(
            p =>
            p.ID == myLink && !p.Disable &&
            p.IDBrand == data.LocWarehouse.IDBrand
            );

        if (d == null) LinkError();
        else
        {
            data.RequestOutlet = d.ID;
            data.RequestOutletNama = d.Nama;

            //Commit
            db.SubmitChanges();
        }
    }
}