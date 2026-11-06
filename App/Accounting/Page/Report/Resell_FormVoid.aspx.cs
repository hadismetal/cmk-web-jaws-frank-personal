using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Resell_FormVoid : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("SalesOrderReport.aspx"); }

    protected void fillForm()
    {
        string msg = "";

        var d = db.Resell.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            rs.InnerHtml = d.Nomor;
            rs.Style.Add("color", "red");
			
			string err = CheckPLU(d);
			if(err == "")
			{
				if (d.StatusVoid == 1)
				{
					// kondisi sudah tervoid
					div_keterangan_void.Visible = false;
					div_alert.Visible = true;
					
					msg += "Resell sudah divoid. <br /> Keterangan Void : " + d.VoidKeterangan.Trim();
					lit_error.Text = "<b>" + msg + "</b>";
					
					save.Visible = false;
				}
				else
				{
					div_keterangan_void.Visible = true;
					div_alert.Visible = false;
					
					save.Visible = true;
				}
			}
			else
			{
				div_keterangan_void.Visible = false;
				div_alert.Visible = true;
				
				msg += err;
				lit_error.Text = "<b>" + msg + "</b>";
				
				save.Visible = false;
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

    protected void back_Click(object sender, EventArgs e)
    {
        Response.Redirect("Resell.aspx");
    }

    //Delete
    protected void save_Click(object sender, EventArgs e)
    {
        string status_SAP = "";
        var d = db.Resell.SingleOrDefault(p => p.ID == myID && (p.StatusVoid == null || p.StatusVoid == 0));
        if (d == null) App.DataNull(this);
        else
        {
            try
            {
                if (SAP.xActivate)
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
				d.VoidKeterangan = keterangan.Text + " (Void by " + App.UserNama + ", " + Str.FullDateJam(System.DateTime.Now) + ")";
                db.SubmitChanges();

                //Redirect
                Response.Redirect("Resell.aspx?d=1");

            }
            else
            {
                Response.Write(status_SAP);
            }
        }
    }

    protected void voidProcess(Resell d)
    {
        var dj = d.ResellDJ;
        var pg = d.ResellPG;
        var gj = d.ResellGJ;
        var ld = d.ResellLD;

        string Lokasi = "";
        int IDBrand = 1;
        if (d.TipeLokasi == 0)
        {
            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocWarehouse WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
        }
        else if (d.TipeLokasi == 1)
        {
            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
        }
        else if (d.TipeLokasi == 2)
        {
            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
        }

        foreach (var r in dj)
        {
            var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductDJ.StockActualDJ.Substorage = 1;
                x.StockProductDJ.StockActualDJ.InHand = 0;
				x.StockProductDJ.StatusPenjualan = 0;
                db.SubmitChanges();
            }

            StockLedgerDJ l = new StockLedgerDJ();

            l.IDProduct = r.IDProduct;
            l.TipeLokasi = r.Resell.TipeLokasi;
            l.IDLokasi = r.Resell.IDLokasi;
            l.NamaLokasi = Lokasi;
            l.Value = -1;
            l.Remark = 10;
            l.Keterangan = "Void " + r.Resell.Nomor;

            //User-Info
            l.Operator = App.UserNama;
            l.OperatorTgl = DateTime.Now;

            db.StockLedgerDJ.InsertOnSubmit(l);
            db.SubmitChanges();
        }

        foreach (var r in pg)
        {
            var x = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductPG.StockActualPG.Substorage = 1;
                x.StockProductPG.StockActualPG.InHand = 0;
				x.StockProductPG.StatusPenjualan = 0;
                db.SubmitChanges();
            }

            StockLedgerPG l = new StockLedgerPG();

            l.IDProduct = r.IDProduct;
            l.TipeLokasi = r.Resell.TipeLokasi;
            l.IDLokasi = r.Resell.IDLokasi;
            l.NamaLokasi = Lokasi;
            l.Value = -1;
            l.Remark = 10;
            l.Keterangan = "Void " + r.Resell.Nomor;

            //User-Info
            l.Operator = App.UserNama;
            l.OperatorTgl = DateTime.Now;

            db.StockLedgerPG.InsertOnSubmit(l);
            db.SubmitChanges();
        }

        foreach (var r in gj)
        {
            var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductGJ.StockActualGJ.Substorage = 1;
                x.StockProductGJ.StockActualGJ.InHand = 0;
				x.StockProductGJ.StatusPenjualan = 0;
                db.SubmitChanges();
            }

            StockLedgerGJ l = new StockLedgerGJ();

            l.IDProduct = r.IDProduct;
            l.TipeLokasi = r.Resell.TipeLokasi;
            l.IDLokasi = r.Resell.IDLokasi;
            l.NamaLokasi = Lokasi;
            l.Value = -1;
            l.Remark = 10;
            l.Keterangan = "Void " + r.Resell.Nomor;

            //User-Info
            l.Operator = App.UserNama;
            l.OperatorTgl = DateTime.Now;

            db.StockLedgerGJ.InsertOnSubmit(l);
            db.SubmitChanges();
        }

        foreach (var r in ld)
        {
            var x = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == r.SalesOrder.ID && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                x.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 1;
                x.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand = 0;
				x.StockProductLD_Stone1B.StatusPenjualan = 0;
                db.SubmitChanges();
            }

            StockLedgerLD l = new StockLedgerLD();

            l.IDProduct = r.IDProduct;
            l.TipeLokasi = r.Resell.TipeLokasi;
            l.IDLokasi = r.Resell.IDLokasi;
            l.NamaLokasi = Lokasi;
            l.Value = -1;
            l.Remark = 10;
            l.Keterangan = "Void " + r.Resell.Nomor;

            //User-Info
            l.Operator = App.UserNama;
            l.OperatorTgl = DateTime.Now;

            db.StockLedgerLD.InsertOnSubmit(l);
            db.SubmitChanges();
        }
    }
}