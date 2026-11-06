using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJDO_DJDO_InsertSPK : System.Web.UI.Page
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

    protected int myID { get { return App.QueryInt(this); } }
    protected int myIDKodeKaret { get { return Convert.ToInt32(Request.QueryString["idkodekaret"]); } }
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected string myNOBRG { get { return Request.QueryString["kdbrg"]; } }
    protected string myNOVAL { get { return Request.QueryString["kdvlds"]; } }

    //Exception
    protected void DataError() { Response.Redirect("DJDO.aspx?nodo=" + myNODO); }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        // ambil data SPK
        var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == myIDKodeKaret);
        if (proto == null) { DataError(); }
        else
        {
            RecalWeight.RecalWeightProductOrder(db, proto.ProductOrder, proto.ProductOrder.ProductOrder_CharProduct);

            // proses update data t_brj !!!
            var brj = cs.T_brj.Single(p => p.NoKodeBarang == myNOBRG && p.NoValidasi == myNOVAL && p.NoDOBRJ == myNODO);
            if (brj != null)
            {
                brj.NoSPK = proto.NoSPK;
                brj.IDSPK = proto.IDProductOrder;
                cs.SubmitChanges();

                Response.Redirect("DJDO.aspx?nodo=" + myNODO + "&d=1");

            }
            else
            {
                Response.Redirect("DJDO_SPKList.aspx?nodo=" + myNODO + "&kdbrg=" + myNOBRG + "&kdvlds=" + myNOVAL + "");
            }
        }

    }

}