using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJDO_DJDO_StoneDelete : System.Web.UI.Page
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
    protected int myStoneIndex { get { return App.QueryInt(this, "si"); } }
    protected int myTableID { get { return App.QueryInt(this, "tid"); } }
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected string myNOBRG { get { return Request.QueryString["nobrg"]; } }
    protected string myNOVAL { get { return Request.QueryString["noval"]; } }
    protected string myNOBAG { get { return Request.QueryString["nobag"]; } }

    protected void DataError() { Response.Redirect("DJDO.aspx"); }
    protected void LinkError() { Response.Redirect("DJDO_Detail.aspx?id=" + myID + "&nodo=" + myNODO); }
    protected void ProductError() { Response.Redirect("DJDO_Detail.aspx?id=" + myID + "&nodo=" + myNODO); }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Process
            var product = d.StockReceiveDJ_CharProduct;
            if (product == null) ProductError();
            else
            {
                switch (myStoneIndex)
                {
                    case 0: save1A(d); break;
                    case 1: save1B(d); break;
                    case 2: save2(d); break;
                    case 3: save3(d); break;
                    case 4: save4(d); break;
                    case 5: save5(d); break;
                }

                //Recalculate
                Lib.RecalWeight(db, d, product);
            }

            //Redirect
            Response.Redirect("DJDO_Detail.aspx?id=" + myID + "&nodo=" + myNODO);
        }
    }
    protected void save1A(StockReceiveDJ ms)
    {
        var d = db.StockReceiveDJ_Stone1A.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.StockReceiveDJ_Stone1A.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save1B(StockReceiveDJ ms)
    {
        var d = db.StockReceiveDJ_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.StockReceiveDJ_Stone1B.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save2(StockReceiveDJ ms)
    {
        var d = db.StockReceiveDJ_Stone2.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.StockReceiveDJ_Stone2.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save3(StockReceiveDJ ms)
    {
        var d = db.StockReceiveDJ_Stone3.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.StockReceiveDJ_Stone3.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save4(StockReceiveDJ ms)
    {
        var d = db.StockReceiveDJ_Stone4.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.StockReceiveDJ_Stone4.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save5(StockReceiveDJ ms)
    {
        var d = db.StockReceiveDJ_Stone5.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.StockReceiveDJ_Stone5.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}