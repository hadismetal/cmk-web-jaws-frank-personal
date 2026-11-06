using System;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

/// <summary>
/// Customer System
/// </summary>
public partial class Ledger
{

    public static void createLedgerVoidSalesOrderDJ(SalesOrder d, SalesOrderDJ r)
    {
        using (Map_DB db = new Map_DB())
        {

            // Ledger 
            StockLedgerDJ l = new StockLedgerDJ();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = 1; // 1 : in , -1 : out
            int Remark = 10; // See Library DB - Ledger
            string Keterangan = r.Invoice; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") { Keterangan = DL.LedgerRemark(Remark); } // dont change!

            string Lokasi = "";
            if (d.TipeLokasi == 1)
            {
                Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Nama;
            }
            else if (d.TipeLokasi == 2)
            {
                Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Nama;
            }

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
            }
    }

    public static void createLedgerVoidSalesOrderPG(SalesOrder d, SalesOrderPG r)
    {
        using (Map_DB db = new Map_DB())
        {

            // Ledger 
            StockLedgerPG l = new StockLedgerPG();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = 1; // 1 : in , -1 : out
            int Remark = 10; // See Library DB - Ledger
            string Keterangan = r.Invoice; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") { Keterangan = DL.LedgerRemark(Remark); } // dont change!

            string Lokasi = "";
            if (d.TipeLokasi == 1)
            {
                Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Nama;
            }
            else if (d.TipeLokasi == 2)
            {
                Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Nama;
            }

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
            }
    }

    public static void createLedgerVoidSalesOrderGJ(SalesOrder d, SalesOrderGJ r)
    {
        using (Map_DB db = new Map_DB())
        {

            // Ledger 
            StockLedgerGJ l = new StockLedgerGJ();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = 1; // 1 : in , -1 : out
            int Remark = 10; // See Library DB - Ledger
            string Keterangan = r.Invoice; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") { Keterangan = DL.LedgerRemark(Remark); } // dont change!

            string Lokasi = "";
            if (d.TipeLokasi == 1)
            {
                Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Nama;
            }
            else if (d.TipeLokasi == 2)
            {
                Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Nama;
            }

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
            }
    }

    public static void createLedgerVoidSalesOrderLD(SalesOrder d, SalesOrderLD r)
    {
        using (Map_DB db = new Map_DB())
        {

            // Ledger 
            StockLedgerLD l = new StockLedgerLD();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = 1; // 1 : in , -1 : out
            int Remark = 10; // See Library DB - Ledger
            string Keterangan = r.Invoice; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") { Keterangan = DL.LedgerRemark(Remark); } // dont change!

            string Lokasi = "";
            if (d.TipeLokasi == 1)
            {
                Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Nama;
            }
            else if (d.TipeLokasi == 2)
            {
                Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Nama;
            }

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
            }
    }

    public static void createLedgerVoidSalesOrderSouvenir(SalesOrder d, SalesOrderSouvenir r)
    {
        using (Map_DB db = new Map_DB())
        {

            // Ledger 
            if (r.TipeProduct == 1)
            {
                StockLedgerDJ l = new StockLedgerDJ();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 10; // See Library DB - Ledger
                string Keterangan = r.Invoice; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") { Keterangan = DL.LedgerRemark(Remark); } // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                {
                    Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Nama;
                }
                else if (d.TipeLokasi == 2)
                {
                    Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Nama;
                }

                //Input
                l.IDProduct = r.IDProductSouvenir != null ? (int)r.IDProductSouvenir : 0;
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
            }
            else if (r.TipeProduct == 2)
            {
                StockLedgerGJ l = new StockLedgerGJ();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 10; // See Library DB - Ledger
                string Keterangan = r.Invoice; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") { Keterangan = DL.LedgerRemark(Remark); } // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                {
                    Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Nama;
                }
                else if (d.TipeLokasi == 2)
                {
                    Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Nama;
                }

                //Input
                l.IDProduct = r.IDProductSouvenir != null ? (int)r.IDProductSouvenir : 0;
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
            }
            else if (r.TipeProduct == 3)
            {
                StockLedgerPG l = new StockLedgerPG();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 10; // See Library DB - Ledger
                string Keterangan = r.Invoice; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") { Keterangan = DL.LedgerRemark(Remark); } // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                {
                    Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Nama;
                }
                else if (d.TipeLokasi == 2)
                {
                    Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Nama;
                }

                //Input
                l.IDProduct = r.IDProductSouvenir != null ? (int)r.IDProductSouvenir : 0;
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
            }
            else if (r.TipeProduct == 4)
            {
                StockLedgerLD l = new StockLedgerLD();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 10; // See Library DB - Ledger
                string Keterangan = r.Invoice; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") { Keterangan = DL.LedgerRemark(Remark); } // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                {
                    Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Nama;
                }
                else if (d.TipeLokasi == 2)
                {
                    Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Nama;
                }

                //Input
                l.IDProduct = r.IDProductSouvenir != null ? (int)r.IDProductSouvenir : 0;
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
            }
            }
    }

}
