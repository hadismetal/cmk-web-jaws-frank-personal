using System;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;

/// <summary>
/// Document Numbering System
/// </summary>
public partial class Document
{

    public static string Generate_Nomor_DownPayment_PO(SalesReceiptDPPO j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "PO";
            int docnumberlength = 21;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;
    		string Brand = "";

            if(j.TipeLokasi == 1)
            {
                Brand = db.LocOutlet.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }
            else if (j.TipeLokasi == 2)
            {
                Brand = db.LocExhibition.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }

            try
            {
                var s = (from p in db.SalesReceiptDPPO
                         where p.NomorDP.Length == docnumberlength 
                         && p.OperatorTgl.Value.Year == Tahun 
                         && p.OperatorTgl.Value.Month == Bulan 
                         && p.NomorDP != null
                         && p.NomorDP.Substring(5, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.NomorDP.Substring(7, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.NomorDP.Substring(17, 4)) descending
                         select p).FirstOrDefault();

                if (s.NomorDP != null) { Nomor = Convert.ToInt32(s.NomorDP.Substring(17, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Brand + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_DownPayment(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "DP";
            int docnumberlength = 19;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;
    		string Brand = "";

            if(j.TipeLokasi == 1)
            {
                Brand = db.LocOutlet.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }
            else if (j.TipeLokasi == 2)
            {
                Brand = db.LocExhibition.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }

            try
            {
                var s = (from p in db.SalesReceipt
                         where p.NomorDP.Length == docnumberlength 
                         && p.OperatorTgl.Value.Year == Tahun 
                         && p.OperatorTgl.Value.Month == Bulan 
                         && p.NomorDP != null
                         && p.NomorDP.Substring(3, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.NomorDP.Substring(5, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.NomorDP.Substring(15, 4)) descending
                         select p).FirstOrDefault();

                if (s.NomorDP != null) { Nomor = Convert.ToInt32(s.NomorDP.Substring(15, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_SalesOrderSouvenir(SalesOrderSouvenir d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/S";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;
            int TipeLokasi = d.SalesOrder.TipeLokasi.Value;
            int IDLokasi = d.SalesOrder.IDLokasi.Value;

            try
            {
                // Dulu filter tahun/bulan/lokasi lewat p.Invoice.Substring(...) — non-sargable,
                // full-table-scan SalesOrderSouvenir tiap kali nomor invoice baru dibuat. Semua
                // nilai yang di-parse dari string itu sebenarnya sudah tersedia langsung dari
                // SalesOrder (sumber datanya sendiri) — filter lewat situ, jauh lebih murah.
                // Substring cuma tetap dipakai untuk ambil 4 digit sequence number terakhir dari
                // kandidat yang sudah kefilter (bukan dari seluruh tabel).
                var s = (from p in db.SalesOrderSouvenir
                         where p.Invoice.Length == docnumberlength
                         && p.SalesOrder.Tgl.Value.Year == Tahun
                         && p.SalesOrder.Tgl.Value.Month == Bulan
                         && p.SalesOrder.TipeLokasi == TipeLokasi
                         && p.SalesOrder.IDLokasi == IDLokasi
                         orderby (p.Invoice.Substring(18, 4)) descending
                         select p).FirstOrDefault();

                if (s.Invoice != null) { Nomor = Convert.ToInt32(s.Invoice.Substring(18, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + TipeLokasi.ToString() + "/" + IDLokasi.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_SalesOrderPackaging(SalesOrderPackaging d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/C";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(Tahun.ToString().Substring(2, 2));
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrderPackaging
                         where p.Invoice.Length == docnumberlength
                         && Convert.ToInt32(p.Invoice.Substring(12, 2)) == Tahun2
                         && Convert.ToInt32(p.Invoice.Substring(15, 2)) == Bulan
                         && p.Invoice.Substring(6, 1) == "" + d.SalesOrder.TipeLokasi.Value.ToString() + ""
                         && p.Invoice.Substring(8, 3) == "" + d.SalesOrder.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Invoice.Substring(18, 4)) descending
                         select p).FirstOrDefault();

                if (s.Invoice != null) { Nomor = Convert.ToInt32(s.Invoice.Substring(18, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + d.SalesOrder.TipeLokasi.Value.ToString() + "/" + d.SalesOrder.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_SalesOrder_DJ(SalesOrderDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/D";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;
            int TipeLokasi = d.SalesOrder.TipeLokasi.Value;
            int IDLokasi = d.SalesOrder.IDLokasi.Value;

            try
            {
                // Dulu filter tahun/bulan/lokasi lewat p.Invoice.Substring(...) — non-sargable,
                // full-table-scan SalesOrderDJ tiap kali nomor invoice baru dibuat, tepat di tabel
                // yang terus di-INSERT bersamaan oleh banyak user (kandidat utama lock contention
                // di load test). Semua nilai itu sebenarnya sudah tersedia langsung dari SalesOrder
                // (sumber datanya sendiri) — filter lewat situ, sama seperti fix
                // Generate_Nomor_SalesOrderSouvenir. Substring cuma tetap dipakai untuk ambil 4
                // digit sequence number terakhir dari kandidat yang sudah kefilter.
                var s = (from p in db.SalesOrderDJ
                         where p.Invoice.Length == docnumberlength
                         && p.SalesOrder.Tgl.Value.Year == Tahun
                         && p.SalesOrder.Tgl.Value.Month == Bulan
                         && p.SalesOrder.TipeLokasi == TipeLokasi
                         && p.SalesOrder.IDLokasi == IDLokasi
                         orderby (p.Invoice.Substring(18, 4)) descending
                         select p).FirstOrDefault();

                if (s.Invoice != null) { Nomor = Convert.ToInt32(s.Invoice.Substring(18, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + d.SalesOrder.TipeLokasi.Value.ToString() + "/" + d.SalesOrder.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_SalesOrder_PG(SalesOrderPG d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/P";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;
            int TipeLokasi = d.SalesOrder.TipeLokasi.Value;
            int IDLokasi = d.SalesOrder.IDLokasi.Value;

            try
            {
                // Sargable rewrite — lihat komentar di Generate_Nomor_SalesOrder_DJ untuk alasan
                // lengkap. Filter tahun/bulan/lokasi lewat SalesOrder langsung, bukan parse ulang
                // Invoice.Substring(...) tiap kandidat (full-table-scan SalesOrderPG).
                var s = (from p in db.SalesOrderPG
                         where p.Invoice.Length == docnumberlength
                         && p.SalesOrder.Tgl.Value.Year == Tahun
                         && p.SalesOrder.Tgl.Value.Month == Bulan
                         && p.SalesOrder.TipeLokasi == TipeLokasi
                         && p.SalesOrder.IDLokasi == IDLokasi
                         orderby (p.Invoice.Substring(18, 4)) descending
                         select p).FirstOrDefault();

                if (s.Invoice != null) { Nomor = Convert.ToInt32(s.Invoice.Substring(18, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + d.SalesOrder.TipeLokasi.Value.ToString() + "/" + d.SalesOrder.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_SalesOrder_GJ(SalesOrderGJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/G";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;
            int TipeLokasi = d.SalesOrder.TipeLokasi.Value;
            int IDLokasi = d.SalesOrder.IDLokasi.Value;

            try
            {
                // Sargable rewrite — lihat komentar di Generate_Nomor_SalesOrder_DJ untuk alasan
                // lengkap. Filter tahun/bulan/lokasi lewat SalesOrder langsung, bukan parse ulang
                // Invoice.Substring(...) tiap kandidat (full-table-scan SalesOrderGJ).
                var s = (from p in db.SalesOrderGJ
                         where p.Invoice.Length == docnumberlength
                         && p.SalesOrder.Tgl.Value.Year == Tahun
                         && p.SalesOrder.Tgl.Value.Month == Bulan
                         && p.SalesOrder.TipeLokasi == TipeLokasi
                         && p.SalesOrder.IDLokasi == IDLokasi
                         orderby (p.Invoice.Substring(18, 4)) descending
                         select p).FirstOrDefault();

                if (s.Invoice != null) { Nomor = Convert.ToInt32(s.Invoice.Substring(18, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + d.SalesOrder.TipeLokasi.Value.ToString() + "/" + d.SalesOrder.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_SalesOrder_LD(SalesOrderLD d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/L";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;
            int TipeLokasi = d.SalesOrder.TipeLokasi.Value;
            int IDLokasi = d.SalesOrder.IDLokasi.Value;

            try
            {
                // Sargable rewrite — lihat komentar di Generate_Nomor_SalesOrder_DJ untuk alasan
                // lengkap. Filter tahun/bulan/lokasi lewat SalesOrder langsung, bukan parse ulang
                // Invoice.Substring(...) tiap kandidat (full-table-scan SalesOrderLD).
                var s = (from p in db.SalesOrderLD
                         where p.Invoice.Length == docnumberlength
                         && p.SalesOrder.Tgl.Value.Year == Tahun
                         && p.SalesOrder.Tgl.Value.Month == Bulan
                         && p.SalesOrder.TipeLokasi == TipeLokasi
                         && p.SalesOrder.IDLokasi == IDLokasi
                         orderby (p.Invoice.Substring(18, 4)) descending
                         select p).FirstOrDefault();

                if (s.Invoice != null) { Nomor = Convert.ToInt32(s.Invoice.Substring(18, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + d.SalesOrder.TipeLokasi.Value.ToString() + "/" + d.SalesOrder.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

	public static string Generate_Nomor_SalesOrder_PGVoucherLEI(SalesOrderPG d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "ORV/P";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(Tahun.ToString().Substring(2, 2));
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrderPG
                         where p.Invoice.Length == docnumberlength
    					 && p.Invoice.Substring(0, (prefix.Length)) == prefix
                         && Convert.ToInt32(p.Invoice.Substring(12, 2)) == Tahun2
                         && Convert.ToInt32(p.Invoice.Substring(15, 2)) == Bulan
                         && p.Invoice.Substring(6, 1) == "" + d.SalesOrder.TipeLokasi.Value.ToString() + ""
                         && p.Invoice.Substring(8, 3) == "" + d.SalesOrder.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Invoice.Substring(18, 4)) descending
                         select p).FirstOrDefault();

                if (s.Invoice != null) { Nomor = Convert.ToInt32(s.Invoice.Substring(18, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + d.SalesOrder.TipeLokasi.Value.ToString() + "/" + d.SalesOrder.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCTP(DocQCDJCustomer j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/F/TP";
            int docnumberlength = 18;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.DocQCDJCustomer
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.OperatorTgl.Value.Year == Tahun
                         && p.OperatorTgl.Value.Month == Bulan
                         orderby (p.Nomor.Substring(14, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(14, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCDJ(DocQCDJ j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/F/DJ";
            int docnumberlength = 18;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.DocQCDJ
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.OperatorTgl.Value.Year == Tahun 
                         && p.OperatorTgl.Value.Month == Bulan
                         orderby (p.Nomor.Substring(14, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(14, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCPG(DocQCPG j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/F/PG";
            int docnumberlength = 18;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.DocQCPG
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.OperatorTgl.Value.Year == Tahun
                         && p.OperatorTgl.Value.Month == Bulan
                         orderby (p.Nomor.Substring(14, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(14, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCGJ(DocQCGJ j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/F/GJ";
            int docnumberlength = 18;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.DocQCGJ
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.OperatorTgl.Value.Year == Tahun
                         && p.OperatorTgl.Value.Month == Bulan
                         orderby (p.Nomor.Substring(14, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(14, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCLD(DocQCLD j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/F/LD";
            int docnumberlength = 18;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.DocQCLD
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.OperatorTgl.Value.Year == Tahun
                         && p.OperatorTgl.Value.Month == Bulan
                         orderby (p.Nomor.Substring(14, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(14, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_Invoice(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV";
            int docnumberlength = 20;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesReceipt
                         where p.NomorInvoice.Length == docnumberlength
                         && p.NomorInvoice.Substring(0, (prefix.Length)) == prefix
                         && p.OperatorTgl.Value.Year == Tahun 
                         && p.OperatorTgl.Value.Month == Bulan 
                         && p.NomorInvoice != null
                         && p.NomorInvoice.Substring(4, 1) == "" + j.TipeLokasi.ToString() + ""
                         && p.NomorInvoice.Substring(6, 3) == "" + j.IDLokasi.ToString().PadLeft(3, '0') + ""
                         orderby (p.NomorInvoice.Substring(16, 4)) descending
                         select p).FirstOrDefault();

                if (s.NomorInvoice != null) { Nomor = Convert.ToInt32(s.NomorInvoice.Substring(16, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_TradeIn(TradeIn j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "TI";
            int docnumberlength = 21;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;
            string Brand = "";

            if (j.TipeLokasi == 1)
            {
                Brand = db.LocOutlet.Single(p => p.ID == j.IDLokasi).CompanyBrand.NamaKode;
            }
            else if (j.TipeLokasi == 2)
            {
                Brand = db.LocExhibition.Single(p => p.ID == j.IDLokasi).CompanyBrand.NamaKode;
            }

            try
            {
                var s = (from p in db.TradeIn
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.Nomor.Substring(3, 1) == Brand
                         && Convert.ToInt32(p.Nomor.Substring(11, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(14, 2)) == Bulan
                         && p.Nomor.Substring(5, 1) == "" + j.TipeLokasi.ToString() + ""
                         && p.Nomor.Substring(7, 3) == "" + j.IDLokasi.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(17, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(17, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Brand + "/" + j.TipeLokasi.ToString() + "/" + j.IDLokasi.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_Resell(Resell j)
    {
        using (Map_DB db = new Map_DB())
        {

    		string groupresell = Db.SingleString("SELECT TOP 1 [GroupResell] FROM Resell WHERE ID = " + j.ID, Db.Cnn_DB);
            string prefix = "";
            if (groupresell.ToUpper() == "GT")
                prefix = "RG";
            else if (groupresell.ToUpper() == "LE")
                prefix = "RB";
            else
                prefix = "RO";
            int docnumberlength = 21;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;
            string Brand = "";

            if (j.TipeLokasi == 1)
            {
                Brand = db.LocOutlet.Single(p => p.ID == j.IDLokasi).CompanyBrand.NamaKode;
            }
            else if (j.TipeLokasi == 2)
            {
                Brand = db.LocExhibition.Single(p => p.ID == j.IDLokasi).CompanyBrand.NamaKode;
            }

            try
            {
                var s = (from p in db.Resell
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.Nomor.Substring(3, 1) == Brand
                         && Convert.ToInt32(p.Nomor.Substring(11, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(14, 2)) == Bulan
                         && p.Nomor.Substring(5, 1) == "" + j.TipeLokasi.ToString() + ""
                         && p.Nomor.Substring(7, 3) == "" + j.IDLokasi.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(17, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(17, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Brand + "/" + j.TipeLokasi.ToString() + "/" + j.IDLokasi.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_SalesOrder(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            // SO/1/014/18/12/0001
            // SO/F/1/014/18/12/0001 --> 2019

            string prefix = "SO";
            int docnumberlength = 21;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;
            string Brand = "";

            if(j.TipeLokasi == 1)
            {
                Brand = db.LocOutlet.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }
            else if (j.TipeLokasi == 2)
            {
                Brand = db.LocExhibition.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }

            try
            {
                var s = (from p in db.SalesOrder
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.Nomor.Substring(3,1) == Brand
    					 && p.Nomor != null && p.Nomor != ""
                         && Convert.ToInt32(p.Nomor.Substring(11, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(14, 2)) == Bulan
                         && p.Nomor.Substring(5, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.Nomor.Substring(7, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(17, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(17, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Brand + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }
	
	public static string Generate_Nomor_SalesOrderVoucherLEI(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            // SO/1/014/18/12/0001
            // SO/F/1/014/18/12/0001 --> 2019

            string prefix = "OR";
            int docnumberlength = 21;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;
            string Brand = "";

            if(j.TipeLokasi == 1)
            {
                Brand = db.LocOutlet.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }
            else if (j.TipeLokasi == 2)
            {
                Brand = db.LocExhibition.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }

            try
            {
                var s = (from p in db.SalesOrder
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.Nomor.Substring(3,1) == Brand
                         && Convert.ToInt32(p.Nomor.Substring(11, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(14, 2)) == Bulan
    					 && p.Nomor != null
    					 && p.Nomor != ""
                         && p.Nomor.Substring(5, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.Nomor.Substring(7, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(17, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(17, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Brand + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }
	
    public static string Generate_Nomor_Souvenir(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "SV";
            int docnumberlength = 21;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;
            string Brand = "";

            if (j.TipeLokasi == 1)
            {
                Brand = db.LocOutlet.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }
            else if (j.TipeLokasi == 2)
            {
                Brand = db.LocExhibition.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }

            try
            {
                var s = (from p in db.SalesOrder
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.Nomor.Substring(3, 1) == Brand
                         && Convert.ToInt32(p.Nomor.Substring(11, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(14, 2)) == Bulan
                         && p.Nomor.Substring(5, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.Nomor.Substring(7, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(17, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(17, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Brand + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_Packaging(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "PK";
            int docnumberlength = 21;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;
            string Brand = "";

            if (j.TipeLokasi == 1)
            {
                Brand = db.LocOutlet.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }
            else if (j.TipeLokasi == 2)
            {
                Brand = db.LocExhibition.Single(p => p.ID == j.IDLokasi.Value).CompanyBrand.NamaKode;
            }

            try
            {
                var s = (from p in db.SalesOrder
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.Nomor.Substring(3, 1) == Brand
                         && Convert.ToInt32(p.Nomor.Substring(11, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(14, 2)) == Bulan
                         && p.Nomor.Substring(5, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.Nomor.Substring(7, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(17, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(17, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Brand + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }
}
