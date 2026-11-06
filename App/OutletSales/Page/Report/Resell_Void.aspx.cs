   using System;
using System.Data;
using System.Linq;

public partial class Page_Approval_Resell_Void : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

    protected int myID { get { return App.QueryInt(this); } }

    //Exception
    protected void DeleteError(string sebab)
    {
        Response.Redirect("Resell.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        string status_SAP = "";

        var d = db.Resell.SingleOrDefault(p => p.ID == myID && (p.StatusVoid == null || p.StatusVoid == 0));
        if (d == null) App.DataNull(this);
        else
        {
            string err = CheckPLU(d);
            if (err == "")
            {
                try
                {
                    if(SAP.xActivate)
                    {
                        status_SAP += ((status_SAP == "") ? SAPM.postVoidBuybackToSAP(d) : "");
                    }
                }
                catch (Exception ex)
                {
                    status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (Error message : " + ex.Message + ")";
                }

                status_SAP = "";
                if (status_SAP == "")
                {
                    //Delete
                    voidProcess(d);

                    d.StatusVoid = 1;
                    db.SubmitChanges();

                    Db.Execute("UPDATE Resell SET TglVoid = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE ID = " + d.ID, Db.Cnn_DB);

                    //Redirect
                    Response.Redirect("Resell.aspx?d=1");

                }
                else
                {
                    Response.Write(status_SAP);
                }
            }
            else Response.Write(err);
        }
    }
    
    //Delete
    protected void voidProcess(Resell d)
    {

        var dj = d.ResellDJ;
        var pg = d.ResellPG;
        var gj = d.ResellGJ;
        var ld = d.ResellLD;

        foreach (var r in dj)
        {
            var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductDJ.StockActualDJ.Substorage = 1;
                x.StockProductDJ.StockActualDJ.InHand = 0;
                db.SubmitChanges();

                StockLedgerDJ ledger = new StockLedgerDJ();
                ledger.IDProduct = r.IDProduct;
                ledger.IDLokasi = d.IDLokasi;
                ledger.TipeLokasi = d.TipeLokasi;
                ledger.NamaLokasi = r.StockProductDJ.StockActualDJ.NamaLokasi;
                ledger.Value = -1;
                ledger.Remark = 10;
                ledger.Keterangan = "Void " + d.Nomor;
                ledger.Operator = App.UserNama;
                ledger.OperatorTgl = DateTime.Now;

                db.StockLedgerDJ.InsertOnSubmit(ledger);
                db.SubmitChanges();
            }
        }

        foreach (var r in pg)
        {
            var x = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductPG.StockActualPG.Substorage = 1;
                x.StockProductPG.StockActualPG.InHand = 0;
                db.SubmitChanges();

                StockLedgerPG ledger = new StockLedgerPG();
                ledger.IDProduct = r.IDProduct;
                ledger.IDLokasi = d.IDLokasi;
                ledger.TipeLokasi = d.TipeLokasi;
                ledger.NamaLokasi = r.StockProductPG.StockActualPG.NamaLokasi;
                ledger.Value = -1;
                ledger.Remark = 10;
                ledger.Keterangan = "Void " + d.Nomor;
                ledger.Operator = App.UserNama;
                ledger.OperatorTgl = DateTime.Now;

                db.StockLedgerPG.InsertOnSubmit(ledger);
                db.SubmitChanges();
            }
        }

        foreach (var r in gj)
        {
            var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductGJ.StockActualGJ.Substorage = 1;
                x.StockProductGJ.StockActualGJ.InHand = 0;
                db.SubmitChanges();

                StockLedgerGJ ledger = new StockLedgerGJ();
                ledger.IDProduct = r.IDProduct;
                ledger.IDLokasi = d.IDLokasi;
                ledger.TipeLokasi = d.TipeLokasi;
                ledger.NamaLokasi = r.StockProductGJ.StockActualGJ.NamaLokasi;
                ledger.Value = -1;
                ledger.Remark = 10;
                ledger.Keterangan = "Void " + d.Nomor;
                ledger.Operator = App.UserNama;
                ledger.OperatorTgl = DateTime.Now;

                db.StockLedgerGJ.InsertOnSubmit(ledger);
                db.SubmitChanges();
            }
        }

        foreach (var r in ld)
        {
            var x = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 1;
                x.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand = 0;
                db.SubmitChanges();

                StockLedgerLD ledger = new StockLedgerLD();
                ledger.IDProduct = r.IDProduct;
                ledger.IDLokasi = d.IDLokasi;
                ledger.TipeLokasi = d.TipeLokasi;
                ledger.NamaLokasi = r.StockProductLD_Stone1B.StockActualLD_Stone1B.NamaLokasi;
                ledger.Value = -1;
                ledger.Remark = 10;
                ledger.Keterangan = "Void " + d.Nomor;
                ledger.Operator = App.UserNama;
                ledger.OperatorTgl = DateTime.Now;

                db.StockLedgerLD.InsertOnSubmit(ledger);
                db.SubmitChanges();
            }
        }
    }

    protected string CheckPLU(Resell d)
    {

        var dj = d.ResellDJ;
        var pg = d.ResellPG;
        var gj = d.ResellGJ;
        var ld = d.ResellLD;

        string message = string.Empty;

        foreach (var r in dj)
        {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                if (x.ReturnProduct == true || x.StatusLebur == true) message = "Barang sudah diretur/dilebur";
                else if (x.StockActualDJ.TipeLokasi != r.Resell.TipeLokasi && x.StockActualDJ.IDLokasi != r.Resell.IDLokasi) message = "Barang sudah tidak berada di outlet (Lokasi : " + x.StockActualDJ.NamaLokasi + "). Silahkan lakukan transfer ke outlet dimana resell dibuat untuk melakukan void";
            }
        }

        foreach (var r in pg)
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                if (x.ReturnProduct == true || x.StatusLebur == true) message = "Barang sudah diretur/dilebur";
                else if (x.StockActualPG.TipeLokasi != r.Resell.TipeLokasi && x.StockActualPG.IDLokasi != r.Resell.IDLokasi) message = "Barang sudah tidak berada di outlet (Lokasi : " + x.StockActualPG.NamaLokasi + "). Silahkan lakukan transfer ke outlet dimana resell dibuat untuk melakukan void";
            }
        }

        foreach (var r in gj)
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                if (x.ReturnProduct == true || x.StatusLebur == true) message = "Barang sudah diretur/dilebur";
                else if (x.StockActualGJ.TipeLokasi != r.Resell.TipeLokasi && x.StockActualGJ.IDLokasi != r.Resell.IDLokasi) message = "Barang sudah tidak berada di outlet (Lokasi : " + x.StockActualGJ.NamaLokasi + "). Silahkan lakukan transfer ke outlet dimana resell dibuat untuk melakukan void";
            }
        }

        foreach (var r in ld)
        {
            var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                if (x.ReturnProduct == true) message = "Barang sudah diretur/dilebur";
                else if (x.StockActualLD_Stone1B.TipeLokasi != r.Resell.TipeLokasi && x.StockActualLD_Stone1B.IDLokasi != r.Resell.IDLokasi) message = "Barang sudah tidak berada di outlet (Lokasi : " + x.StockActualLD_Stone1B.NamaLokasi + "). Silahkan lakukan transfer ke outlet dimana resell dibuat untuk melakukan void";
            }
        }
        return message;
    }
}