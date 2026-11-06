using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for RetailNext
/// </summary>
public partial class RetailNext
{
    public static string api;
    public static string PostingSalesOrderRetailNext(SalesOrder d, string filepath, string filepathdetail, bool statusvoid = false)
    {
        string message = "";
        string separator = ",";
        string fulltextcsv = "";
        string fulltextcsvitem = "";

        string store = (d.TipeLokasi == 1) ? Db.SingleString("SELECT TOP 1 Kode FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB) : Db.SingleString("SELECT TOP 1 Kode FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        var sr = d.SalesReceipt;
        if (sr.Count != 0)
        {
            List<Transaction> trs = new List<Transaction>();
            foreach (var i in sr)
            {
                foreach (var x in i.SalesReceiptDetail.Where(v => !v.VoidReference.HasValue))
                {
                    Transaction tr = new Transaction();
                    tr.CurrencyCode = "IDR";
                    tr.OperatorIDs = d.Operator;
                    tr.LineItemsTotal = Math.Ceiling((x.Nominal) / 1.02m ?? 0m);
                    tr.PaymentType = x.PaymentType.Nama.ToUpper();
                    tr.ReceiptDateTime = Convert.ToDateTime(d.OperatorTgl).ToString("yyyy-MM-dd HH:mm:ss");
                    tr.ReceiptNumber = d.Nomor;
                    tr.SalesAssociateID = d.SalesNama;
                    tr.StoreID = store;
                    tr.TaxAmount = x.Nominal - Math.Ceiling((x.Nominal) / 1.02m ?? 0m) ?? 0m;
                    tr.TransactionStatus = "Successful";
                    tr.TransactionTotal = x.Nominal;
                    tr.TransactionType = "Sale";
                    tr.WorkStationID = store;
                    tr.Brand = "F";
                    trs.Add(tr);
                }
            }

            List<LineItem> lis = new List<LineItem>();
            foreach (var x in d.SalesOrderDJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductDJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                li.Void = "N";
                lis.Add(li);
            }
            foreach (var x in d.SalesOrderPG)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductPG.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductPG.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                li.Void = "N";
                lis.Add(li);
            }
            foreach (var x in d.SalesOrderGJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductGJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                li.Void = "N";
                lis.Add(li);
            }
            foreach (var x in d.SalesOrderLD)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductLD_Stone1B.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductLD_Stone1B.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                li.Void = "N";
                lis.Add(li);
            }

            string nikae = "-";
            string niksales = "-";
            using (Map_DB db = new Map_DB())
            using (Map_AC ac = new Map_AC())
            {
                var a = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
                var b = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales);

                if (a != null)
                    nikae = a.UserNumber;
                if (b != null)
                    niksales = b.Keterangan;
            }

            int counttr = 0;
            foreach (Transaction trx in trs)
            {
                fulltextcsv += string.Join(separator, trx.ReceiptNumber, trx.StoreID, trx.StoreID, nikae, trx.ReceiptDateTime, trx.TransactionType, trx.TransactionStatus, trx.PaymentType, Convert.ToDecimal(trx.LineItemsTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(trx.TransactionTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(trx.TaxAmount).ToString("N2", nfi).Replace(",", ""), niksales, trx.CurrencyCode);
                counttr++;
                if (counttr < trs.Count)
                    fulltextcsv += "\n";
            }

            int countlis = 0;
            foreach (LineItem li in lis)
            {
                fulltextcsvitem += string.Join(separator, li.SKU, li.Description, Convert.ToDecimal(li.Price).ToString("N2", nfi).Replace(",", ""), li.Quantity, li.ReceiptNumber);
                countlis++;
                if (countlis < lis.Count)
                    fulltextcsvitem += "\n";
            }

            DateTime saiki = DateTime.Now;

            string filepath1 = filepath + (!statusvoid ? "\\SalesOrder\\TransactionInfo_" : "\\SalesOrderVoid\\TransactionInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filepathItem1 = filepathdetail + (!statusvoid ? "\\SalesOrder\\LineItemInfo_" : "\\SalesOrderVoid\\LineItemInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            File.WriteAllText(filepath1, fulltextcsv.ToString());
            File.WriteAllText(filepathItem1, fulltextcsvitem.ToString());

            string filenametrans = "TransactionInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filenameitem = "LineItemInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            message += WebAPIPOST(filenametrans, fulltextcsv, d.Nomor);
            if (!string.IsNullOrEmpty(message)) //fail
            {
                SetMessageStatusSent(trs,"",message,false,false);
            }
            else
            {
                SetMessageStatusSent(trs,message,"",true, false);
            }
            message += WebAPIPOST(filenameitem, fulltextcsvitem, d.Nomor);
            if (!string.IsNullOrEmpty(message)) //fail
            {
                SetMessageStatusItemSent(lis);
            }
            else
            {
                SetMessageStatusItemSent(lis);
            }
        }
        else
        {
            message = "Data Not Found.";
        }
        return message;
    }
    public static string VoidSalesOrderRetailNext(SalesOrder d, string filepath, string filepathdetail, bool statusvoid = true)
    {
        string message = "";
        string separator = ",";
        string fulltextcsv = "";
        string fulltextcsvitem = "";

        string store = (d.TipeLokasi == 1) ? Db.SingleString("SELECT TOP 1 Kode FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB) : Db.SingleString("SELECT TOP 1 Kode FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        System.Data.DataTable dt = Db.Rs("SELECT * FROM t_transrn WHERE TransactionID = '" + d.Nomor + "' ", Db.Cnn_CS);

        if (dt.Rows.Count != 0)
        {
            List<Transaction> trs = new List<Transaction>();
            foreach (System.Data.DataRow x in dt.Rows)
            {
                Transaction tr = new Transaction();
                tr.CurrencyCode = x["CurrencyCode"].ToString();
                tr.OperatorIDs = x["OperatorID"].ToString();
                tr.LineItemsTotal = (-1) * Convert.ToDecimal(x["LineItemsTotal"]);
                tr.PaymentType = x["PaymentType"].ToString();
                tr.ReceiptDateTime = Convert.ToDateTime(x["TimeStamp"]).ToString("yyyy-MM-dd HH:mm:ss");
                tr.ReceiptNumber = x["TransactionID"].ToString();
                tr.SalesAssociateID = x["SalesAssociateID"].ToString();
                tr.StoreID = x["StoreID"].ToString();
                tr.TaxAmount = (-1) * Convert.ToDecimal(x["TaxAmount"]);
                tr.TransactionStatus = "POST-VOID";
                tr.TransactionTotal = (-1) * Convert.ToDecimal(x["TransactionTotal"]);
                tr.TransactionType = x["TransactionType"].ToString();
                tr.WorkStationID = x["WorkStationID"].ToString();
                tr.Brand = "F";
                trs.Add(tr);
            }
            List<LineItem> lis = new List<LineItem>();
            foreach (var x in d.SalesOrderDJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductDJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;

                li.Price = x.TotalBayar;
                lis.Add(li);
            }
            foreach (var x in d.SalesOrderPG)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductPG.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductPG.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis.Add(li);
            }
            foreach (var x in d.SalesOrderGJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductGJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis.Add(li);
            }
            foreach (var x in d.SalesOrderLD)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductLD_Stone1B.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductLD_Stone1B.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis.Add(li);
            }

            string nikae = "-";
            string niksales = "-";
            using (Map_DB db = new Map_DB())
            using (Map_AC ac = new Map_AC())
            {
                var a = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
                var b = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales);

                if (a != null)
                    nikae = a.UserNumber;
                if (b != null)
                    niksales = b.Keterangan;
            }

            int counttr = 0;
            foreach (Transaction trx in trs)
            {
                fulltextcsv += string.Join(separator, trx.ReceiptNumber, trx.StoreID, trx.StoreID, nikae, trx.ReceiptDateTime, trx.TransactionType, trx.TransactionStatus, trx.PaymentType, Convert.ToDecimal(trx.LineItemsTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(trx.TransactionTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(trx.TaxAmount).ToString("N2", nfi).Replace(",", ""), niksales, trx.CurrencyCode);
                counttr++;
                if (counttr < trs.Count)
                    fulltextcsv += "\n";
            }

            int countlis = 0;
            foreach (LineItem li in lis)
            {
                fulltextcsvitem += string.Join(separator, li.SKU, li.Description, Convert.ToDecimal(li.Price).ToString("N2", nfi).Replace(",", ""), li.Quantity, li.ReceiptNumber);
                countlis++;
                if (countlis < lis.Count)
                    fulltextcsvitem += "\n";
            }

            DateTime saiki = DateTime.Now;

            string filepath1 = filepath + (!statusvoid ? "\\SalesOrder\\TransactionInfo_" : "\\SalesOrderVoid\\TransactionInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filepathItem1 = filepathdetail + (!statusvoid ? "\\SalesOrder\\LineItemInfo_" : "\\SalesOrderVoid\\LineItemInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            File.WriteAllText(filepath1, fulltextcsv.ToString());
            File.WriteAllText(filepathItem1, fulltextcsvitem.ToString());

            string filenametrans = "TransactionInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filenameitem = "LineItemInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            message += WebAPIPOST(filenametrans, fulltextcsv, d.Nomor);
            message += WebAPIPOST(filenameitem, fulltextcsvitem, d.Nomor);

        }
        else
        {
            message = "Data Not Found";
        }

        return message;
    }
    public static string PostingResellOrderRetailNext(Resell r, string filepath, string filepathdetail, bool statusvoid = false)
    {
        string message = "";
        string separator = ",";
        string fulltextcsv = "";
        string fulltextcsvitem = "";

        string store = (r.TipeLokasi == 1) ? Db.SingleString("SELECT TOP 1 Kode FROM LocOutlet WHERE ID = " + r.IDLokasi, Db.Cnn_DB) : Db.SingleString("SELECT TOP 1 Kode FROM LocExhibition WHERE ID = " + r.IDLokasi, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        Transaction tr = new Transaction();
        if (!statusvoid)
        {
            tr.CurrencyCode = "IDR";
            tr.OperatorIDs = r.OperatorNama;
            tr.LineItemsTotal = r.TotalHarga;
            tr.PaymentType = "Cash";
            tr.ReceiptDateTime = Convert.ToDateTime(r.OperatorTgl).ToString("yyyy-MM-dd HH:mm:ss");
            tr.ReceiptNumber = r.Nomor;
            tr.SalesAssociateID = r.SalesNama;
            tr.StoreID = store;
            tr.TaxAmount = 0m;
            tr.TransactionStatus = "Successful";
            tr.TransactionTotal = r.TotalHarga;
            tr.TransactionType = "Return";
            tr.WorkStationID = store;
        }
        else
        {
            tr.CurrencyCode = "IDR";
            tr.OperatorIDs = r.OperatorNama;
            tr.LineItemsTotal = (-1) * r.TotalHarga;
            tr.PaymentType = "Cash";
            tr.ReceiptDateTime = Convert.ToDateTime(r.OperatorTgl).ToString("yyyy-MM-dd HH:mm:ss");
            tr.ReceiptNumber = r.Nomor;
            tr.SalesAssociateID = r.SalesNama;
            tr.StoreID = store;
            tr.TaxAmount = (-1) * 0m;
            tr.TransactionStatus = "Successful";
            tr.TransactionTotal = (-1) * r.TotalHarga;
            tr.TransactionType = "POST-VOID";
            tr.WorkStationID = store;
        }

        List<LineItem> lis = new List<LineItem>();
        foreach (var x in r.ResellDJ)
        {
            LineItem li = new LineItem();
            li.SKU = x.StockProductDJ.Nomor;
            li.Quantity = 1;
            li.Description = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToUpper();
            li.ReceiptNumber = r.Nomor;
            li.Price = x.HargaRupiah;
            if (!statusvoid)
                li.Void = "N";
            else
                li.Void = "Y";
            lis.Add(li);
        }
        foreach (var x in r.ResellPG)
        {
            LineItem li = new LineItem();
            li.SKU = x.StockProductPG.Nomor;
            li.Quantity = 1;
            li.Description = x.StockProductPG.CharProductItem.Nama.ToUpper();
            li.ReceiptNumber = r.Nomor;
            li.Price = x.HargaRupiah;
            if (!statusvoid)
                li.Void = "N";
            else
                li.Void = "Y";
            lis.Add(li);
        }
        foreach (var x in r.ResellGJ)
        {
            LineItem li = new LineItem();
            li.SKU = x.StockProductGJ.Nomor;
            li.Quantity = 1;
            li.Description = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama.ToUpper();
            li.ReceiptNumber = r.Nomor;
            li.Price = x.HargaRupiah;
            if (!statusvoid)
                li.Void = "N";
            else
                li.Void = "Y";
            lis.Add(li);
        }
        foreach (var x in r.ResellLD)
        {
            LineItem li = new LineItem();
            li.SKU = x.StockProductLD_Stone1B.Nomor;
            li.Quantity = 1;
            li.Description = x.StockProductLD_Stone1B.CharProductItem.Nama.ToUpper();
            li.ReceiptNumber = r.Nomor;
            li.Price = x.HargaRupiah;
            if (!statusvoid)
                li.Void = "N";
            else
                li.Void = "Y";
            lis.Add(li);
        }

        string nikae = "-";
        string niksales = "-";
        using (Map_DB db = new Map_DB())
        using (Map_AC ac = new Map_AC())
        {
            var a = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            var b = db.DataSales.SingleOrDefault(p => p.ID == r.IDSales);

            if (a != null)
                nikae = a.UserNumber;
            if (b != null)
                niksales = b.Keterangan;
        }

        fulltextcsv = string.Join(separator, tr.ReceiptNumber, tr.StoreID, tr.StoreID, nikae, tr.ReceiptDateTime, tr.TransactionType, tr.TransactionStatus, tr.PaymentType, Convert.ToDecimal(tr.LineItemsTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(tr.TransactionTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(tr.TaxAmount).ToString("N2", nfi).Replace(",", ""), niksales, tr.CurrencyCode);

        int countlis = 0;
        foreach (LineItem li in lis)
        {
            fulltextcsvitem += string.Join(separator, li.SKU, li.Description, Convert.ToDecimal(li.Price).ToString("N2", nfi).Replace(",", ""), li.Quantity, li.ReceiptNumber);
            countlis++;
            if (countlis < lis.Count)
                fulltextcsvitem += "\n";
        }

        DateTime saiki = DateTime.Now;

        string filepath1 = filepath + (!statusvoid ? "\\Resell\\TransactionInfo_" : "\\ResellVoid\\TransactionInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
        string filepathItem1 = filepathdetail + (!statusvoid ? "\\Resell\\LineItemInfo_" : "\\ResellVoid\\LineItemInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
        File.WriteAllText(filepath1, fulltextcsv.ToString());
        File.WriteAllText(filepathItem1, fulltextcsvitem.ToString());

        string filenametrans = "TransactionInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
        string filenameitem = "LineItemInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
        message += WebAPIPOST(filenametrans, fulltextcsv, r.Nomor);
        message += WebAPIPOST(filenameitem, fulltextcsvitem, r.Nomor);

        return message;
    }
    public static string PostingTradeInRetailNext(TradeIn t, string filepath, string filepathdetail, bool statusvoid = false)
    {
        string message = "";
        string separator = ",";
        string fulltextcsvso = "";
        string fulltextcsvitemso = "";
        string fulltextcsvro = "";
        string fulltextcsvitemro = "";

        string store = (t.TipeLokasi == 1) ? Db.SingleString("SELECT TOP 1 Kode FROM LocOutlet WHERE ID = " + t.IDLokasi, Db.Cnn_DB) : Db.SingleString("SELECT TOP 1 Kode FROM LocExhibition WHERE ID = " + t.IDLokasi, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        SalesOrder d = t.SalesOrder;
        Resell r = t.Resell;

        if (d != null && r != null)
        {
            Transaction tr1 = new Transaction();
            tr1.CurrencyCode = "IDR";
            tr1.OperatorIDs = r.OperatorNama;
            tr1.LineItemsTotal = r.TotalHarga;
            tr1.PaymentType = "Cash";
            tr1.ReceiptDateTime = Convert.ToDateTime(r.OperatorTgl).ToString("yyyy-MM-dd HH:mm:ss");
            tr1.ReceiptNumber = r.Nomor;
            tr1.SalesAssociateID = r.SalesNama;
            tr1.StoreID = store;
            tr1.TaxAmount = 0m;
            tr1.TransactionStatus = "Succesfull";
            tr1.TransactionTotal = r.TotalHarga;
            tr1.TransactionType = "Return";
            tr1.WorkStationID = store;

            var sr = d.SalesReceipt;
            List<Transaction> trs2 = new List<Transaction>();
            if (statusvoid)
                foreach (var i in sr)
                {
                    foreach (var x in i.SalesReceiptDetail.Where(v => !v.VoidReference.HasValue))
                    {
                        Transaction tr2 = new Transaction();
                        if (!statusvoid)
                        {
                            tr2.CurrencyCode = "IDR";
                            tr2.OperatorIDs = d.Operator;
                            tr2.LineItemsTotal = Math.Ceiling((x.Nominal) / 1.02m ?? 0m);
                            tr2.PaymentType = x.PaymentType.Nama.ToUpper();
                            tr2.ReceiptDateTime = Convert.ToDateTime(d.OperatorTgl).ToString("yyyy-MM-dd HH:mm:ss");
                            tr2.ReceiptNumber = d.Nomor;
                            tr2.SalesAssociateID = d.SalesNama;
                            tr2.StoreID = store;
                            tr2.TaxAmount = x.Nominal - Math.Ceiling((x.Nominal) / 1.02m ?? 0m) ?? 0m;
                            tr2.TransactionStatus = "Successful";
                            tr2.TransactionTotal = x.Nominal;
                            tr2.TransactionType = "Sale";
                            tr2.WorkStationID = store;
                        }
                        else
                        {
                            tr2.CurrencyCode = "IDR";
                            tr2.OperatorIDs = d.Operator;
                            tr2.LineItemsTotal = (-1) * Math.Ceiling((x.Nominal) / 1.02m ?? 0m);
                            tr2.PaymentType = x.PaymentType.Nama.ToUpper();
                            tr2.ReceiptDateTime = Convert.ToDateTime(d.OperatorTgl).ToString("yyyy-MM-dd HH:mm:ss");
                            tr2.ReceiptNumber = d.Nomor;
                            tr2.SalesAssociateID = d.SalesNama;
                            tr2.StoreID = store;
                            tr2.TaxAmount = (-1) * ((x.Nominal - Math.Ceiling((x.Nominal) / 1.02m ?? 0m)) ?? 0m);
                            tr2.TransactionStatus = "POST-VOID";
                            tr2.TransactionTotal = (-1) * x.Nominal;
                            tr2.TransactionType = "Sale";
                            tr2.WorkStationID = store;
                        }
                        trs2.Add(tr2);
                    }
                }

            List<LineItem> lis1 = new List<LineItem>();
            foreach (var x in r.ResellDJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductDJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = r.Nomor;
                li.Price = x.HargaRupiah;
                lis1.Add(li);
            }
            foreach (var x in r.ResellPG)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductPG.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductPG.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = r.Nomor;
                li.Price = x.HargaRupiah;
                lis1.Add(li);
            }
            foreach (var x in r.ResellGJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductGJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = r.Nomor;
                li.Price = x.HargaRupiah;
                lis1.Add(li);
            }
            foreach (var x in r.ResellLD)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductLD_Stone1B.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductLD_Stone1B.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = r.Nomor;
                li.Price = x.HargaRupiah;
                lis1.Add(li);
            }

            List<LineItem> lis2 = new List<LineItem>();
            foreach (var x in d.SalesOrderDJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductDJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis2.Add(li);
            }
            foreach (var x in d.SalesOrderPG)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductPG.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductPG.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis2.Add(li);
            }
            foreach (var x in d.SalesOrderGJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductGJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis2.Add(li);
            }
            foreach (var x in d.SalesOrderLD)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductLD_Stone1B.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductLD_Stone1B.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis2.Add(li);
            }

            string nikae = "-";
            string niksales = "-";
            using (Map_DB db = new Map_DB())
            using (Map_AC ac = new Map_AC())
            {
                var a = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
                var b = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales);

                if (a != null)
                    nikae = a.UserNumber;
                if (b != null)
                    niksales = b.Keterangan;
            }

            fulltextcsvro = string.Join(separator, tr1.ReceiptNumber, tr1.StoreID, tr1.StoreID, nikae, tr1.ReceiptDateTime, tr1.TransactionType, tr1.TransactionStatus, tr1.PaymentType, Convert.ToDecimal(tr1.LineItemsTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(tr1.TransactionTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(tr1.TaxAmount).ToString("N2", nfi).Replace(",", ""), niksales, tr1.CurrencyCode);

            int counttr = 0;
            foreach (Transaction trx in trs2)
            {
                fulltextcsvso += string.Join(separator, trx.ReceiptNumber, trx.StoreID, trx.StoreID, nikae, trx.ReceiptDateTime, trx.TransactionType, trx.TransactionStatus, trx.PaymentType, Convert.ToDecimal(trx.LineItemsTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(trx.TransactionTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(trx.TaxAmount).ToString("N2", nfi).Replace(",", ""), niksales, trx.CurrencyCode);
                counttr++;
                if (counttr < trs2.Count)
                    fulltextcsvso += "\n";
            }

            int countlis1 = 0;
            foreach (LineItem li in lis1)
            {
                fulltextcsvitemro += string.Join(separator, li.SKU, li.Description, Convert.ToDecimal(li.Price).ToString("N2", nfi).Replace(",", ""), li.Quantity, li.ReceiptNumber);
                countlis1++;
                if (countlis1 < lis1.Count)
                    fulltextcsvitemro += "\n";
            }

            int countlis2 = 0;
            foreach (LineItem li in lis2)
            {
                fulltextcsvitemso += string.Join(separator, li.SKU, li.Description, Convert.ToDecimal(li.Price).ToString("N2", nfi).Replace(",", ""), li.Quantity, li.ReceiptNumber);
                countlis2++;
                if (countlis2 < lis2.Count)
                    fulltextcsvitemso += "\n";
            }

            DateTime saiki = DateTime.Now;

            string filepathro1 = filepath + (!statusvoid ? "\\Resell\\TransactionInfo_" : "\\ResellVoid\\TransactionInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filepathItemro1 = filepathdetail + (!statusvoid ? "\\Resell\\LineItemInfo_" : "\\ResellVoid\\LineItemInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filepathso1 = filepath + (!statusvoid ? "\\SalesOrder\\TransactionInfo_" : "\\SalesOrderVoid\\TransactionInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filepathItemso1 = filepathdetail + (!statusvoid ? "\\SalesOrder\\LineItemInfo_" : "\\SalesOrderVoid\\LineItemInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            File.WriteAllText(filepathro1, fulltextcsvro.ToString());
            File.WriteAllText(filepathItemro1, fulltextcsvitemro.ToString());
            File.WriteAllText(filepathso1, fulltextcsvso.ToString());
            File.WriteAllText(filepathItemso1, fulltextcsvitemso.ToString());

            string filenametrans = "TransactionInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filenameitem = "LineItemInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            message += WebAPIPOST(filenametrans, fulltextcsvro, d.Nomor);
            message += WebAPIPOST(filenameitem, fulltextcsvitemro, d.Nomor);
            message += WebAPIPOST(filenametrans, fulltextcsvso, d.Nomor);
            message += WebAPIPOST(filenameitem, fulltextcsvitemso, d.Nomor);
        }
        else
        {
            if (d == null && r != null)
                message = "Data Sales Order not Found.";
            else if (d != null && r == null)
                message = "Data Resell not Found";
            else
                message = "Data Sales Order and Resell not Found";
        }
        return message;
    }

    public static string VoidTradeInRetailNext(TradeIn t, string filepath, string filepathdetail, bool statusvoid = true)
    {
        string message = "";
        string separator = ",";
        string fulltextcsvso = "";
        string fulltextcsvitemso = "";
        string fulltextcsvro = "";
        string fulltextcsvitemro = "";

        string store = (t.TipeLokasi == 1) ? Db.SingleString("SELECT TOP 1 Kode FROM LocOutlet WHERE ID = " + t.IDLokasi, Db.Cnn_DB) : Db.SingleString("SELECT TOP 1 Kode FROM LocExhibition WHERE ID = " + t.IDLokasi, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        SalesOrder d = t.SalesOrder;
        Resell r = t.Resell;

        Transaction tr1 = new Transaction();
        tr1.CurrencyCode = "IDR";
        tr1.OperatorIDs = r.OperatorNama;
        tr1.LineItemsTotal = r.TotalHarga;
        tr1.PaymentType = "Cash";
        tr1.ReceiptDateTime = Convert.ToDateTime(r.OperatorTgl).ToString("yyyy-MM-dd HH:mm:ss");
        tr1.ReceiptNumber = r.Nomor;
        tr1.SalesAssociateID = r.SalesNama;
        tr1.StoreID = store;
        tr1.TaxAmount = 0m;
        tr1.TransactionStatus = "Succesfull";
        tr1.TransactionTotal = r.TotalHarga;
        tr1.TransactionType = "Return";
        tr1.WorkStationID = store;

        System.Data.DataTable dt = Db.Rs("SELECT * FROM t_transrn WHERE TransactionID = '" + d.Nomor + "' ", Db.Cnn_CS);

        if (dt.Rows.Count > 0 && r != null)
        {
            List<Transaction> trs2 = new List<Transaction>();
            foreach (System.Data.DataRow x in dt.Rows)
            {
                Transaction tr = new Transaction();
                tr.CurrencyCode = x["CurrencyCode"].ToString();
                tr.OperatorIDs = x["OperatorID"].ToString();
                tr.LineItemsTotal = Convert.ToDecimal(x["LineItemsTotal"]);
                tr.PaymentType = x["PaymentType"].ToString();
                tr.ReceiptDateTime = Convert.ToDateTime(x["TimeStamp"]).ToString("yyyy-MM-dd HH:mm:ss");
                tr.ReceiptNumber = x["TransactionID"].ToString();
                tr.SalesAssociateID = x["SalesAssociateID"].ToString();
                tr.StoreID = x["StoreID"].ToString();
                tr.TaxAmount = Convert.ToDecimal(x["TaxAmount"]);
                tr.TransactionStatus = "POST-VOID";
                tr.TransactionTotal = (-1) * Convert.ToDecimal(x["TransactionTotal"]);
                tr.TransactionType = x["TransactionType"].ToString();
                tr.WorkStationID = x["WorkStationID"].ToString();

                trs2.Add(tr);
            }

            List<LineItem> lis1 = new List<LineItem>();
            foreach (var x in r.ResellDJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductDJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = r.Nomor;
                li.Price = x.HargaRupiah;
                lis1.Add(li);
            }
            foreach (var x in r.ResellPG)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductPG.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductPG.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = r.Nomor;
                li.Price = x.HargaRupiah;
                lis1.Add(li);
            }
            foreach (var x in r.ResellGJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductGJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = r.Nomor;
                li.Price = x.HargaRupiah;
                lis1.Add(li);
            }
            foreach (var x in r.ResellLD)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductLD_Stone1B.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductLD_Stone1B.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = r.Nomor;
                li.Price = x.HargaRupiah;
                lis1.Add(li);
            }

            List<LineItem> lis2 = new List<LineItem>();
            foreach (var x in d.SalesOrderDJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductDJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis2.Add(li);
            }
            foreach (var x in d.SalesOrderPG)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductPG.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductPG.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis2.Add(li);
            }
            foreach (var x in d.SalesOrderGJ)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductGJ.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis2.Add(li);
            }
            foreach (var x in d.SalesOrderLD)
            {
                LineItem li = new LineItem();
                li.SKU = x.StockProductLD_Stone1B.Nomor;
                li.Quantity = 1;
                li.Description = x.StockProductLD_Stone1B.CharProductItem.Nama.ToUpper();
                li.ReceiptNumber = d.Nomor;
                li.Price = x.TotalBayar;
                lis2.Add(li);
            }

            string nikae = "-";
            string niksales = "-";
            using (Map_DB db = new Map_DB())
            using (Map_AC ac = new Map_AC())
            {
                var a = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
                var b = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales);

                if (a != null)
                    nikae = a.UserNumber;
                if (b != null)
                    niksales = b.Keterangan;
            }

            fulltextcsvro = string.Join(separator, tr1.ReceiptNumber, tr1.StoreID, tr1.StoreID, nikae, tr1.ReceiptDateTime, tr1.TransactionType, tr1.TransactionStatus, tr1.PaymentType, Convert.ToDecimal(tr1.LineItemsTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(tr1.TransactionTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(tr1.TaxAmount).ToString("N2", nfi).Replace(",", ""), niksales, tr1.CurrencyCode);

            int counttr = 0;
            foreach (Transaction trx in trs2)
            {
                fulltextcsvso += string.Join(separator, trx.ReceiptNumber, trx.StoreID, trx.StoreID, nikae, trx.ReceiptDateTime, trx.TransactionType, trx.TransactionStatus, trx.PaymentType, Convert.ToDecimal(trx.LineItemsTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(trx.TransactionTotal).ToString("N2", nfi).Replace(",", ""), Convert.ToDecimal(trx.TaxAmount).ToString("N2", nfi).Replace(",", ""), niksales, trx.CurrencyCode);
                counttr++;
                if (counttr < trs2.Count)
                    fulltextcsvso += "\n";
            }

            int countlis1 = 0;
            foreach (LineItem li in lis1)
            {
                fulltextcsvitemro += string.Join(separator, li.SKU, li.Description, Convert.ToDecimal(li.Price).ToString("N2", nfi).Replace(",", ""), li.Quantity, li.ReceiptNumber);
                countlis1++;
                if (countlis1 < lis1.Count)
                    fulltextcsvitemro += "\n";
            }

            int countlis2 = 0;
            foreach (LineItem li in lis2)
            {
                fulltextcsvitemso += string.Join(separator, li.SKU, li.Description, Convert.ToDecimal(li.Price).ToString("N2", nfi).Replace(",", ""), li.Quantity, li.ReceiptNumber);
                countlis2++;
                if (countlis2 < lis2.Count)
                    fulltextcsvitemso += "\n";
            }

            DateTime saiki = DateTime.Now;

            string filepathro1 = filepath + (!statusvoid ? "\\Resell\\TransactionInfo_" : "\\ResellVoid\\TransactionInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filepathItemro1 = filepathdetail + (!statusvoid ? "\\Resell\\LineItemInfo_" : "\\ResellVoid\\LineItemInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filepathso1 = filepath + (!statusvoid ? "\\SalesOrder\\TransactionInfo_" : "\\SalesOrderVoid\\TransactionInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filepathItemso1 = filepathdetail + (!statusvoid ? "\\SalesOrder\\LineItemInfo_" : "\\SalesOrderVoid\\LineItemInfo_") + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            File.WriteAllText(filepathro1, fulltextcsvro.ToString());
            File.WriteAllText(filepathItemro1, fulltextcsvitemro.ToString());
            File.WriteAllText(filepathso1, fulltextcsvso.ToString());
            File.WriteAllText(filepathItemso1, fulltextcsvitemso.ToString());

            string filenametrans = "TransactionInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            string filenameitem = "LineItemInfo_" + store + "_" + saiki.ToString("yyyyMMdd") + "_" + saiki.ToString("HHmmss") + ".csv";
            message += WebAPIPOST(filenametrans, fulltextcsvro, d.Nomor);
            message += WebAPIPOST(filenameitem, fulltextcsvitemro, d.Nomor);
            message += WebAPIPOST(filenametrans, fulltextcsvso, d.Nomor);
            message += WebAPIPOST(filenameitem, fulltextcsvitemso, d.Nomor);

        }
        else
        {
            if (dt.Rows.Count == 0 && r != null)
                message = "Data Sales Order Not Found.";
            else if (dt.Rows.Count > 0 && r == null)
                message = "Data Resell Not Found.";
            else
                message = "Data Sales Order and Resell Not Found.";
        }
        return message;
    }

    public static string WebAPIPOST(string filename, string file, string transaction_id)
    {
        //JSON result
        string result = "";
        try{
            DateTime saiki = DateTime.Now;

            string url = "https://centralmegakencana.upload.ops.retailnext.net/pos/";

            WebRequest req = WebRequest.Create(url);
            req.Method = WebRequestMethods.Http.Post;
            req.ContentType = "text/csv";
            req.Headers.Add("Authorization", "Basic cmV0YWlsbmV4dF9uRTNSQVFNcnU6YnlGOEtTdVhKazNzMw==");
            req.Headers.Add("content-disposition", "attachment; filename=\"" + filename + "\"");
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3; 

            using (StreamWriter sw = new StreamWriter(req.GetRequestStream()))
            {
                if (!string.IsNullOrEmpty(file))
                {
                    sw.Write(file);
                    sw.Flush();
                }
            }

            using (Stream res = req.GetResponse().GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(res))
                {
                    result = sr.ReadToEnd();
                }
            }
		}
		catch(Exception ex)
		{
			result = ex.Message.ToString();
		}
        return result;
    }

    public static string SetMessageStatusSent(List<Transaction> trs,string msgsent, string msgfail, bool sent = false, bool postvoid = false)
    {
        string message = "";
        try
        {
            foreach (Transaction tr in trs)
            {
                int count = Db.SingleInteger("SELECT COUNT('') FROM t_transrn WHERE TransactionID = '" + tr.ReceiptNumber + "' ", Db.Cnn_CS);
                if (count == 0)
                {
                    if (sent)
                    {
                        Db.Execute("INSERT INTO t_transrn(TransactionID, Brand, StoreID, WorkStationID, TimeStamp, LineItemsTotal, TransactionTotal, TaxAmount, CurrencyCode,TransactionType, TransactionStatus, PaymentType, OperatorID, SalesAssociateID, Status, ResponseSent, ResponseFail, PostVoid) " +
                            "VALUES('" + tr.ReceiptNumber + "','" + tr.Brand + "', '" + tr.StoreID + "', '" + tr.WorkStationID + "', '" + tr.ReceiptDateTime + "', '" + tr.LineItemsTotal + "', '" + tr.TransactionTotal + "', '" + tr.TaxAmount + "','" + tr.CurrencyCode + "','" + tr.TransactionType + "', '" + tr.TransactionStatus + "','" + tr.PaymentType + "','" + tr.OperatorIDs + "','" + tr.SalesAssociateID + "','1','" + msgsent + "', null, '0') ", Db.Cnn_CS);
                    }
                    else
                    {
                        Db.Execute("INSERT INTO t_transrn(TransactionID, Brand, StoreID, WorkStationID, TimeStamp, LineItemsTotal, TransactionTotal, TaxAmount, CurrencyCode,TransactionType, TransactionStatus, PaymentType, OperatorID, SalesAssociateID, Status, ResponseSent, ResponseFail, PostVoid) " +
                            "VALUES('" + tr.ReceiptNumber + "','" + tr.Brand + "', '" + tr.StoreID + "', '" + tr.WorkStationID + "', '" + tr.ReceiptDateTime + "', '" + tr.LineItemsTotal + "', '" + tr.TransactionTotal + "', '" + tr.TaxAmount + "','" + tr.CurrencyCode + "','" + tr.TransactionType + "', '" + tr.TransactionStatus + "','" + tr.PaymentType + "','" + tr.OperatorIDs + "','" + tr.SalesAssociateID + "','0', null, '" + msgfail + "', '0') ", Db.Cnn_CS);
                    }
                }
                else
                {
                    if (sent)
                    {
                        Db.Execute("UPDATE t_transrn SET Status = '1', ResponseSent = '" + msgsent + "', ResponseFail = null " + (postvoid ? ", PostVoid = '1' " : "") + " WHERE transaction_id = '" + tr.ReceiptNumber + "' ", Db.Cnn_CS);
                    }
                    else
                    {
                        Db.Execute("UPDATE t_transrn SET Status = '0', ResponseFail = '" + msgfail + "', ResponseSent = null " + (postvoid ? ", PostVoid = '1' " : "") + " WHERE transaction_id = '" + tr.ReceiptNumber + "' ", Db.Cnn_CS);
                    }
                    break;
                }
            }
        }
        catch(Exception ex)
        {
            message = ex.Message.ToString();
        }
        return message;
    }
    public static string SetMessageStatusItemSent(List<LineItem> lis)
    {
        string message = "";
        try
        {
            foreach (LineItem li in lis)
            {
                int count = Db.SingleInteger("SELECT COUNT('') FROM t_lineitemrn WHERE SKU = '" + li.SKU + "' ", Db.Cnn_CS);
                if (count == 0)
                {
                    Db.Execute("INSERT INTO t_lineitemrn(TransactionID, SKU, Price, Description, Quantity, Void) VALUES('" + li.ReceiptNumber + "', '" + li.SKU + "', '" + li.Price + "', '" + li.Description + "', '" + li.Quantity + "', '" + li.Void + "') ", Db.Cnn_CS);
                }
                else
                {
                    Db.Execute("UPDATE t_lineitemrn SET Void = '" + li.Void + "', TransactionID = '" + li.ReceiptNumber + "', Price ='" + li.Price + "', Description = '" + li.Description + "' WHERE SKU = '" + li.SKU + "' ", Db.Cnn_CS);
                }
            }
        }
        catch(Exception ex)
        {
            message = ex.Message.ToString();
        }
        return message;
    }

    public class Transaction
    {
        public string StoreID { get; set; }
        public string ReceiptNumber { get; set; } // Transaction ID
        public string WorkStationID { get; set; }
        public string ReceiptDateTime { get; set; } // TimeStamp
        public decimal? LineItemsTotal { get; set; }
        public decimal? TransactionTotal { get; set; }
        public string OperatorIDs { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public string PaymentType { get; set; }
        public decimal TaxAmount { get; set; }
        public string SalesAssociateID { get; set; }
        public string CurrencyCode { get; set; }
        public string Brand { get; set; }
    }
    public class LineItem
    {
        public string SKU { get; set; }
        public string ReceiptNumber { get; set; } // Transaction ID
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Void { get; set; }
    }
    public static string MappingPayment(int idpayment)
    {
        string RetailNextPayment = "";
        switch (idpayment)
        {
            case 1: //Cash
                RetailNextPayment = "Cash";
                break;
            case 2: //CreditCard
                RetailNextPayment = "CreditCard";
                break;
            case 3: //Transfer
                RetailNextPayment = "Transfer";
                break;
            case 4: //DebitCard
                RetailNextPayment = "DebitCard";
                break;
            case 5: //Voucher
                RetailNextPayment = "Voucher";
                break;
            case 6: //Exchange
                RetailNextPayment = "Exchange";
                break;
            case 7: //Poin
                RetailNextPayment = "Poin";
                break;
            case 8: //DP
                RetailNextPayment = "DownPayment";
                break;
            case 9: //GC
                RetailNextPayment = "GiftCard";
                break;
            case 10: //BSP
                RetailNextPayment = "BridestoryPay";
                break;
            case 11: //Laku Emas
                RetailNextPayment = "LE";
                break;
            default: //Cash
                RetailNextPayment = "Cash";
                break;
        }
        return RetailNextPayment;
    }
}