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
            int docnumberlength = 19;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesReceiptDPPO
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

    public static string Generate_Nomor_DownPayment(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "DP";
            int docnumberlength = 19;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;
            int Nomor = 1;

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
            int Tahun2 = Convert.ToInt32(Tahun.ToString().Substring(2, 2));
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrderSouvenir
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
            int Tahun2 = Convert.ToInt32(Tahun.ToString().Substring(2, 2));
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrderDJ
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

    public static string Generate_Nomor_SalesOrder_PG(SalesOrderPG d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/P";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(Tahun.ToString().Substring(2, 2));
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrderPG
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

    public static string Generate_Nomor_SalesOrder_GJ(SalesOrderGJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/G";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(Tahun.ToString().Substring(2, 2));
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrderGJ
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

    public static string Generate_Nomor_SalesOrder_LD(SalesOrderLD d)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "INV/L";
            int docnumberlength = 22;
            int Tahun = d.SalesOrder.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(Tahun.ToString().Substring(2, 2));
            int Bulan = d.SalesOrder.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrderLD
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


    public static string Generate_Nomor_QCTP(DocQCDJCustomer j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/TP";
            int docnumberlength = 16;
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
                         orderby (p.Nomor.Substring(12, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(12, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCDJ(DocQCDJ j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/DJ";
            int docnumberlength = 16;
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
                         orderby (p.Nomor.Substring(12, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(12, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCPG(DocQCPG j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/PG";
            int docnumberlength = 16;
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
                         orderby (p.Nomor.Substring(12, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(12, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCGJ(DocQCGJ j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/GJ";
            int docnumberlength = 16;
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
                         orderby (p.Nomor.Substring(12, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(12, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_QCLD(DocQCLD j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "QC/LD";
            int docnumberlength = 16;
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
                         orderby (p.Nomor.Substring(12, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(12, 4)) + 1; }
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
            int docnumberlength = 19;
            int Tahun = j.Tgl.Value.Year;
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.TradeIn
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && p.OperatorTgl.Year == Tahun
                         && p.OperatorTgl.Month == Bulan 
                         && p.Nomor.Substring(3, 1) == "" + j.TipeLokasi.ToString() + ""
                         && p.Nomor.Substring(5, 3) == "" + j.IDLokasi.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(15, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(15, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + j.TipeLokasi.ToString() + "/" + j.IDLokasi.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_Resell(Resell j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "RO";
            int docnumberlength = 19;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.Resell
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && Convert.ToInt32(p.Nomor.Substring(9, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(12, 2)) == Bulan
                         && p.Nomor.Substring(3, 1) == "" + j.TipeLokasi.ToString() + ""
                         && p.Nomor.Substring(5, 3) == "" + j.IDLokasi.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(15, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(15, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + j.TipeLokasi.ToString() + "/" + j.IDLokasi.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_SalesOrder(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "SO";
            int docnumberlength = 19;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrder
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && Convert.ToInt32(p.Nomor.Substring(9, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(12, 2)) == Bulan
                         && p.Nomor.Substring(3, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.Nomor.Substring(5, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(15, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(15, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_Souvenir(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "SV";
            int docnumberlength = 19;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrder
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && Convert.ToInt32(p.Nomor.Substring(9, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(12, 2)) == Bulan
                         && p.Nomor.Substring(3, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.Nomor.Substring(5, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(15, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(15, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }

    public static string Generate_Nomor_Packaging(SalesOrder j)
    {
        using (Map_DB db = new Map_DB())
        {

            string prefix = "PK";
            int docnumberlength = 19;
            int Tahun = j.Tgl.Value.Year;
            int Tahun2 = Convert.ToInt32(j.Tgl.Value.Year.ToString().Substring(2, 2));
            int Bulan = j.Tgl.Value.Month;
            int Nomor = 1;

            try
            {
                var s = (from p in db.SalesOrder
                         where p.Nomor.Length == docnumberlength
                         && p.Nomor.Substring(0, (prefix.Length)) == prefix
                         && Convert.ToInt32(p.Nomor.Substring(9, 2)) == Tahun2
                         && Convert.ToInt32(p.Nomor.Substring(12, 2)) == Bulan
                         && p.Nomor.Substring(3, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                         && p.Nomor.Substring(5, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                         orderby (p.Nomor.Substring(15, 4)) descending
                         select p).FirstOrDefault();

                if (s.Nomor != null) { Nomor = Convert.ToInt32(s.Nomor.Substring(15, 4)) + 1; }
            }
            catch (Exception) { }

            return prefix + "/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
            }
    }
}
