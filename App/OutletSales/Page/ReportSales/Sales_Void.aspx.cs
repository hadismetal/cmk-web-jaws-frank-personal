using System;
using System.Linq;

public partial class Page_Approval_Sales_Void : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

    protected int myID { get { return App.QueryInt(this); } }

    //Exception
    protected void DeleteError(string sebab)
    {
        Response.Redirect("Sales.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Delete
            voidProcess(d);
            recalcSO(d);

            //Redirect
            Response.Redirect("SalesOrderReport.aspx?d=1");
        }
    }

    protected void recalcSO(SalesOrder d)
    {
        d.TglVoid = DateTime.Today;
        d.StatusVoid = true;
        d.Unpaid = d.TotalBayar;
        d.Paid = 0;
        db.SubmitChanges();
    }

    //Delete
    protected void voidProcess(SalesOrder d)
    {
        var dj = d.SalesOrderDJ;
        var pg = d.SalesOrderPG;
        var gj = d.SalesOrderGJ;
        var ld = d.SalesOrderLD;
        var pc = d.SalesOrderPackaging;
        var svr = d.SalesOrderSouvenir;

        foreach(var r in d.SalesReceipt)
        {
            foreach(var rslt in r.SalesReceiptDetail.Where(p => p.VoidReference == null))
            {
                decimal Pembayaran = 0m;
                Pembayaran = rslt.Nominal.Value * -1;

                SalesReceiptDetail n = new SalesReceiptDetail();

                n.IDBankIssuer = rslt.IDBankIssuer;
                n.IDCard = rslt.IDCard;
                n.CCName = rslt.CCName;
                n.CCNumber = rslt.CCNumber;
                n.EDCList = rslt.EDCList;
                n.IDEDC = rslt.IDEDC;
                n.IDPaymentType = rslt.IDPaymentType;
                n.IDProgram = rslt.IDProgram;
                n.IDSalesReceipt = rslt.IDSalesReceipt;
                n.MDR = rslt.MDR;
                n.Nominal = Pembayaran;
                n.OperatorNama = rslt.OperatorNama;
                n.OperatorTgl = rslt.OperatorTgl;
                n.VoidReference = rslt.ID; // Add Void Reference ID dari pembayaran yang mau di void

                db.SalesReceiptDetail.InsertOnSubmit(n);
                db.SubmitChanges();

                // int IDvoid = n.ID;
                // var xvoid = db.SalesReceiptDetail.Single(p => p.ID == rslt.ID);
                // xvoid.VoidReference = IDvoid; // Update Void Reference dari ID pembayaran lawannya
                // db.SubmitChanges();

                Db.Execute("UPDATE SalesReceiptDetail SET VoidReference = " + IDvoid.ToString() + " WHERE ID = " + rslt.ID.ToString(), Db.Cnn_DB);

                if (rslt.IDPaymentType == 5)
                {
                    var x = cs.T_voucher.SingleOrDefault(p => p.M_nomor == rslt.CCNumber);

                    if (x != null)
                    {
                        x.M_klaimdoc = "";
                        x.M_tanggal2 = null;
                        x.M_store2 = "";

                        cs.SubmitChanges();
                    }
                }


            }
        }

        foreach (var r in dj)
        {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                // Ledger 
                StockLedgerDJ l = new StockLedgerDJ();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 10; // See Library DB - Ledger
                string Keterangan = r.Invoice; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                else if (d.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                //Input
                l.IDProduct = r.IDProduct;
                l.TipeLokasi = d.TipeLokasi.Value;
                l.IDLokasi = d.IDLokasi.Value;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerDJ.InsertOnSubmit(l);
                db.SubmitChanges();

                x.StatusPenjualan = 0;
                x.StockActualDJ.InHand = 1;
                x.StockActualDJ.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in pg)
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                // Ledger 
                StockLedgerPG l = new StockLedgerPG();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 10; // See Library DB - Ledger
                string Keterangan = r.Invoice; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                else if (d.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                //Input
                l.IDProduct = r.IDProduct;
                l.TipeLokasi = d.TipeLokasi.Value;
                l.IDLokasi = d.IDLokasi.Value;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerPG.InsertOnSubmit(l);
                db.SubmitChanges();

                x.StatusPenjualan = 0;
                x.StockActualPG.InHand = 1;
                x.StockActualPG.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in gj)
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                // Ledger 
                StockLedgerGJ l = new StockLedgerGJ();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 10; // See Library DB - Ledger
                string Keterangan = r.Invoice; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                else if (d.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                //Input
                l.IDProduct = r.IDProduct;
                l.TipeLokasi = d.TipeLokasi.Value;
                l.IDLokasi = d.IDLokasi.Value;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerGJ.InsertOnSubmit(l);
                db.SubmitChanges();

                x.StatusPenjualan = 0;
                x.StockActualGJ.InHand = 1;
                x.StockActualGJ.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in ld)
        {
            var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                // Ledger 
                StockLedgerLD l = new StockLedgerLD();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 10; // See Library DB - Ledger
                string Keterangan = r.Invoice; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                else if (d.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                //Input
                l.IDProduct = r.IDProduct;
                l.TipeLokasi = d.TipeLokasi.Value;
                l.IDLokasi = d.IDLokasi.Value;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerLD.InsertOnSubmit(l);
                db.SubmitChanges();

                x.StatusPenjualan = 0;
                x.StockActualLD_Stone1B.InHand = 1;
                x.StockActualLD_Stone1B.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in pc)
        {
            var x = db.StockActualPackaging.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
            if (x != null)
            {
                if (r.Qty != null)
                {
                    x.InHand = x.InHand + r.Qty.Value;
                    db.SubmitChanges();
                }
                
            }
        }

        foreach (var r in svr)
        {
            if (r.TipeProduct == 0)
            {
                var x = db.StockActualSouvenir.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
                if (x != null)
                {
                    if (r.Qty != null)
                    {
                        x.InHand = x.InHand + r.Qty.Value;
                        db.SubmitChanges();
                    }
                }
            }
            else if (r.TipeProduct == 1)
            {
                var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
		    Ledger.createLedgerVoidSalesOrderSouvenir(d, r);
                    x.StatusPenjualan = 0;
                    x.StockActualDJ.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 2)
            {
                var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
		    Ledger.createLedgerVoidSalesOrderSouvenir(d, r);
                    x.StatusPenjualan = 0;
                    x.StockActualGJ.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 3)
            {
                var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
		    Ledger.createLedgerVoidSalesOrderSouvenir(d, r);
                    x.StatusPenjualan = 0;
                    x.StockActualPG.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 4)
            {
                var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
		    Ledger.createLedgerVoidSalesOrderSouvenir(d, r);
                    x.StatusPenjualan = 0;
                    x.StockActualLD_Stone1B.InHand = 1;
                    db.SubmitChanges();
                }
            }
        }

        foreach (var r in d.SalesReceiptDPPO)
        {
            r.StatusDP = 0;
            r.IDSalesOrder = null;
            db.SubmitChanges();
        }

        foreach (var r in d.SalesVoucher)
        {
            var x = cs.T_voucher.SingleOrDefault(p => p.M_nomor == r.NomorVoucher);

            if (x != null)
            {
                x.M_klaimdoc = "";
                x.M_tanggal2 = null;
                x.M_store2 = "";

                cs.SubmitChanges();
            }
        }

        int tradein_resell_id = 0;
        int salesorder_id = 0;

        try
        {
            var tradein = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder == d.ID);

            var resell = db.Resell.SingleOrDefault(p => p.ID == tradein.IDResell);

            foreach (var r in resell.ResellDJ)
            {
                var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellPG)
            {
                var x = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellGJ)
            {
                var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellLD)
            {
                var x = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            tradein_resell_id = tradein.IDResell.Value;

        }
        catch (Exception) { }


        try
        {
            salesorder_id = d.ID;

            if (tradein_resell_id > 0)
            {
                Db.Execute("DELETE FROM ResellDJ WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellPG WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellGJ WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellLD WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
            }

            if (salesorder_id > 0)
            {
                Db.Execute("DELETE FROM TradeIn WHERE IDSalesOrder = " + salesorder_id.ToString(), Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderPackaging WHERE IDForm = " + salesorder_id.ToString(), Db.Cnn_DB);

                string NamaKodeSBU = "";
                string TransactionType = "so";
                string TransactionNumber = d.Nomor;

                if (d.TipeLokasi == 0)
                {
                    NamaKodeSBU = db.LocWarehouse.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                }
                else if (d.TipeLokasi == 1)
                {
                    NamaKodeSBU = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                }
                else if (d.TipeLokasi == 2)
                {
                    NamaKodeSBU = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                }

                Db.Execute("EXEC sp_repost_all_void_transaction 0 "
                    + ", '" + NamaKodeSBU + "'"
                    + ", '" + TransactionType + "'"
                    + ", '" + TransactionNumber + "'"
                    , Db.Cnn_CS);
            }
        }
        catch (Exception) { }
    }
}