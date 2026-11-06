using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;

public partial class Sales_DataCustomerStamps : System.Web.UI.Page
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
    protected string myMODULE { get { return Request.QueryString["mod"]; } }
    protected string myKEYWORD { get { return Request.QueryString["key"]; } }
    protected int mySTAMPSID { get { return App.QueryInt(this, "stid"); } }

    //protected string myMODULE
    //{
    //    get
    //    {
    //        string result = "";
    //        if (myMODE == "PACKAGING") { result = "Sales_DataCustomer"; }
    //        else if (myMODE == "DOWNPAYMENT") { result = "PaymentDP_DataCustomer"; }
    //        else if (myMODE == "REPAIR") { result = "Repair_DataCustomer"; }
    //        else if (myMODE == "RESELL") { result = "Resell_DataCustomer"; }
    //        else if (myMODE == "SALES") { result = "Sales_DataCustomer"; }
    //        else if (myMODE == "SOUVENIR") { result = "Souvenir_DataCustomer"; }
    //        else if (myMODE == "TITIPAN") { result = "Titipan_DataCustomer"; }
    //        else if (myMODE == "TRADEIN") { result = "TradeIn_DataCustomer"; }
    //        return result;
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (myMODULE == null || myID == 0) { DataError(); }

        if (!IsPostBack)
        {
            keyword.Focus();

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("/Repair/Page/Summary.aspx"); }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            keyword.Text = Request.QueryString["key"];
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        href += "&id=" + myID;
        href += "&mod=" + myMODULE;
        href += "&key=" + keyword.Text;

        Response.Redirect(href);
    }

    protected void fillTable(object sender, EventArgs e)
    {
        string errMsg = "";

        string Kode = "CMK";

        try
        {
            Kode = getLocation();
        }
        catch (Exception) { errMsg = "JAWS Error : cannot get Location Code"; }

        Literal lit = lit_Stamps;
        Table tbl = tb;

        if (mySTAMPSID > 0)
        {
            string xstampsid = mySTAMPSID.ToString();
            string xmod = "";

            if (myMODULE == "PACKAGING") { xmod = "Sales"; }
            else if (myMODULE == "DOWNPAYMENT") { xmod = "PaymentDP"; }
            else if (myMODULE == "REPAIR") { xmod = "Repair"; }
            else if (myMODULE == "RESELL") { xmod = "Resell"; }
            else if (myMODULE == "SALES") { xmod = "Sales"; }
            else if (myMODULE == "SOUVENIR") { xmod = "Sales"; }
            else if (myMODULE == "TITIPAN") { xmod = "Titipan"; }
            else if (myMODULE == "TRADEIN") { xmod = "TradeIn"; }

            int xid = myID;

            try
            {
                errMsg = STAMPSM.GetMembership(lit, xstampsid, xmod, xid, Kode);

                //Redirect
                Response.Redirect(backlinkURL());
            }
            catch (Exception ex) { errMsg = errMsg + ex.Message + ex.StackTrace + ex.InnerException; }

        }
        else
        {

            try
            {
                STAMPSM.MemberSuggestions(lit, tbl, myMODULE, myID, myKEYWORD);
            }
            catch (Exception ex) { errMsg = "STAMPS MemberSuggestions Invalid : " + ex.Message; }


        }

        if (errMsg != "") { errMsg = "<div class='alert alert-danger'>" + errMsg + " <br />Terjadi gangguan teknis. Silahkan mencoba beberapa saat lagi. Jika kendala teknis masih berlanjut harap hubungi ke bagian IT Support." + "</div>"; }
        lit_errMsg.Text = (errMsg != "") ? errMsg : "";
    }

    protected bool checkValidID(ref string Kode, ref string IDBrand)
    {
        bool status = false;
        int iTipeLokasi = 0;
        int iIDLokasi = 0;

        if (myMODULE == "PACKAGING")
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) { status = false; }
            else
            {
                status = true;
                iTipeLokasi = d.TipeLokasi.Value;
                iIDLokasi = d.IDLokasi.Value;
            }
        }
        else if (myMODULE == "DOWNPAYMENT")
        {
            var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) { status = false; }
            else
            {
                status = true;
                iTipeLokasi = d.TipeLokasi.Value;
                iIDLokasi = d.IDLokasi.Value;
            }
        }
        else if (myMODULE == "REPAIR")
        {
            var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) { status = false; }
            else
            {
                status = true;
                iTipeLokasi = d.TipeLokasi.Value;
                iIDLokasi = d.IDLokasi.Value;
            }
        }
        else if (myMODULE == "RESELL")
        {
            var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) { status = false; }
            else
            {
                status = true;
                iTipeLokasi = d.TipeLokasi;
                iIDLokasi = d.IDLokasi;
            }
        }
        else if (myMODULE == "SALES")
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) { status = false; }
            else
            {
                status = true;
                iTipeLokasi = d.TipeLokasi.Value;
                iIDLokasi = d.IDLokasi.Value;
            }

        }
        else if (myMODULE == "SOUVENIR")
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) { status = false; }
            else
            {
                status = true;
                iTipeLokasi = d.TipeLokasi.Value;
                iIDLokasi = d.IDLokasi.Value;
            }
        }
        else if (myMODULE == "TITIPAN")
        {
            var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) { status = false; }
            else
            {
                status = true;
                iTipeLokasi = d.TipeLokasi.Value;
                iIDLokasi = d.IDLokasi.Value;
            }
        }
        else if (myMODULE == "TRADEIN")
        {
            var d = db.TradeIn.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) { status = false; }
            else
            {
                status = true;
                iTipeLokasi = d.TipeLokasi;
                iIDLokasi = d.IDLokasi;
            }
        }

        if(status)
        {
            if (iTipeLokasi == 1)
            {
                var kd = db.LocOutlet.SingleOrDefault(p => p.ID == iIDLokasi);
                Kode = (kd.Kode != null) ? kd.Kode : "NONE";
                IDBrand = kd.IDBrand.ToString();
            }
            else if (iTipeLokasi == 2)
            {
                var kd = db.LocExhibition.SingleOrDefault(p => p.ID == iIDLokasi);
                Kode = (kd.Kode != null) ? kd.Kode : "NONE";
                IDBrand = kd.IDBrand.ToString();
            }
        }

        return status;
    }

    protected string getLocation()
    {
        string result = "";

        string Kode = "NON";
        string IDBrand = "0";

        if (checkValidID(ref Kode, ref IDBrand))
        {
            result = Kode;
        }

        return result;
    }

    protected string backlinkURL()
    {
        string result = "";

        result += "/Repair/Page/Create";

        if (myMODULE == "PACKAGING") { result += "/Packaging/Sales_Form"; }
        else if (myMODULE == "DOWNPAYMENT") { result += "/Payment/PaymentDP_Form"; }
        else if (myMODULE == "REPAIR") { result += "/Repair/Repair_Form"; }
        else if (myMODULE == "RESELL") { result += "/Resell/Resell_Form"; }
        else if (myMODULE == "SALES") { result += "/Sales/Sales_Form"; }
        else if (myMODULE == "SOUVENIR") { result += "/Souvenir/Sales_Form"; }
        else if (myMODULE == "TITIPAN") { result += "/Titipan/Titipan_Form"; }
        else if (myMODULE == "TRADEIN") { result += "/TradeIn/TradeIn_Form"; }

        result += ".aspx?id=" + myID + "&status=1";

        return result;
    }
	
	protected void backlinkURLwithBackButton(object sender, EventArgs e)
    {
        string result = "";

        result += "/Repair/Page/Create";

        if (myMODULE == "PACKAGING") { result += "/Packaging/Sales_Form"; }
        else if (myMODULE == "DOWNPAYMENT") { result += "/Payment/PaymentDP_Form"; }
        else if (myMODULE == "REPAIR") { result += "/Repair/Repair_Form"; }
        else if (myMODULE == "RESELL") { result += "/Resell/Resell_Form"; }
        else if (myMODULE == "SALES") { result += "/Sales/Sales_Form"; }
        else if (myMODULE == "SOUVENIR") { result += "/Souvenir/Sales_Form"; }
        else if (myMODULE == "TITIPAN") { result += "/Titipan/Titipan_Form"; }
        else if (myMODULE == "TRADEIN") { result += "/TradeIn/TradeIn_Form"; }

        result += ".aspx?id=" + myID + "&status=1";

        //return result;
        Response.Redirect(result);
    }
}