using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.SAP_ZSDFM_SD_INTRFC;

public partial class SAPM
{
    public static ZsdfmSdIntrfcResponse PostDataSalesToWs(SalesOrder d, bool isTradeIn)
    {
        ZSDFM_SD_INTRFC _proxy = new ZSDFM_SD_INTRFC();
        ZsdfmSdIntrfc _input = new ZsdfmSdIntrfc();
        ZsdfmSdIntrfcResponse _output = new ZsdfmSdIntrfcResponse();

        System.Net.NetworkCredential cred = new System.Net.NetworkCredential();

        using (Map_DB db = new Map_DB())
        {

            string SoNo = d.Nomor;
            string SoType = (isTradeIn) ? "Z203" : "Z202";
            string SoOrg = "CMK";
            string SoDc = ""; // key !!! RT
            string SoDivision = ""; // key !!! FC, MD, MM, TP
            string SoOffice = ""; // key !!! FC02
            string SoGroup = ""; // key !!! F01
            string SoCustomer = d.DataCustomer.NoCustomer;
            string SoCurrency = "IDR";
            string SoTerm = "C001";
            string SoExecutive = (d.DataSales.Keterangan != null) ? d.DataSales.Keterangan : "";
            string SoQty = "1";
            string SoUom = "PC";
            string SoToko = ""; // kode lokasi JAWS

            int IDBrand = 0;

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                IDBrand = (loc).IDBrand;
                SoToko = (loc).Kode;
            }
            if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                IDBrand = (loc).IDBrand;
                SoToko = (loc).Kode;
            }

            // Validasi Mapping Location Here

            string spname = "[SP_SAP_LOCATION_VALIDATION]";
            string query = " exec " + spname
                         + Str.isString(SoToko, false)
                         + Str.isNumber(IDBrand)
                         + Str.isNumber(d.TipeLokasi.Value)
                         + Str.isNumber(d.IDLokasi.Value)
                         ;
        
            //Dokumen DO tidak valid berdasarkan Brand Company.Silahkan check kembali Nomor DO anda.
            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            bool isValidLocationSAP = false;

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    isValidLocationSAP = Convert.ToBoolean((dtRow["isValid"].ToString() == "1" ? "True" : "False"));

                    if(isValidLocationSAP)
                    {
                        SoDc = Convert.ToString(dtRow["SAP_SD_DISTRIBUTIONCHANNEL"].ToString()); // key !!!
                        SoDivision = Convert.ToString(dtRow["SAP_SD_DIVISION"].ToString()); // key !!!
                        SoOffice = Convert.ToString(dtRow["SAP_SD_SALESOFFICE"].ToString()); // key !!!
                        SoGroup = Convert.ToString(dtRow["SAP_SD_SALESGROUP"].ToString()); // key !!!
                    }
                }

            }

            if (isValidLocationSAP)
            {
                var dj = d.SalesOrderDJ;
                var pg = d.SalesOrderPG;
                var gj = d.SalesOrderGJ;
                var ld = d.SalesOrderLD;

                List<ZsdstSoHeader> _listHead = new List<ZsdstSoHeader>();

                if (dj.Count > 0)
                {
                    foreach (var item in dj)
                    {
                        string SoPlu = item.StockProductDJ.Nomor;

                        string SoPrice = Str.Number0(item.HargaRupiah).Replace(".", "");
                        string SoDiscount01 = Str.Number0(item.DiscountNominal).Replace(".", "");
                        string SoDiscount02 = Str.Number0(item.DiscountPromo).Replace(".", "");
                        string SoDiscount03 = Str.Number0(item.DiscountProgramNominal).Replace(".", "");
                        string SoDiscount04 = Str.Number0(item.DiscountGift).Replace(".", "");
                        string SoDiscount05 = Str.Number0(item.DiscountBank).Replace(".", "");
                        string SoDiscount06 = Str.Number0(item.DiscountOther).Replace(".", "");
                        string SoDiscount07 = Str.Number0(item.DiscountRound).Replace(".", "");
                        string SoDiscount08 = "";
                        string SoDiscount09 = "";
                        string SoDiscount10 = "";
                        string SoTax = Str.Number0(Math.Round((item.TotalBayar.Value / 1.02m * 2) / 100)).Replace(".", "");

                        ZsdstSoHeader _header = new ZsdstSoHeader();
                        {
                            _header.BillNoSap = "";
                            _header.DoNoSap = "";
                            _header.ItemProcess = "";
                            _header.Message = "";
                            _header.PgiNoSap = "";
                            _header.ReprocessProcess = "";
                            _header.RunnoProcess = "";
                            _header.SoCategory = "";
                            _header.SoCurrency = SoCurrency;
                            _header.SoCustomer = SoCustomer;
                            _header.SoDc = SoDc;
                            _header.SoDiscount01 = SoDiscount01;
                            _header.SoDiscount02 = SoDiscount02;
                            _header.SoDiscount03 = SoDiscount03;
                            _header.SoDiscount04 = SoDiscount04;
                            _header.SoDiscount05 = SoDiscount05;
                            _header.SoDiscount06 = SoDiscount06;
                            _header.SoDiscount07 = SoDiscount07;
                            _header.SoDiscount08 = SoDiscount08;
                            _header.SoDiscount09 = SoDiscount09;
                            _header.SoDiscount10 = SoDiscount10;
                            _header.SoDivision = SoDivision;
                            _header.SoExecutive = SoExecutive;
                            _header.SoGroup = SoGroup;
                            _header.SoJasa1 = "";
                            _header.SoJasa2 = "";
                            _header.SoJasa3 = "";
                            _header.SoMatcode = "";
                            _header.SoNo = SoNo;
                            _header.SoNoSap = "";
                            _header.SoOffice = SoOffice;
                            _header.SoOrg = SoOrg;
                            _header.SoPlant = "";
                            _header.SoPlu = SoPlu;
                            _header.SoPrice = SoPrice;
                            _header.SoPriceSap = 0;
                            _header.SoQty = SoQty;
                            _header.SoQtySap = 0;
                            _header.SoStLoc = "";
                            _header.SoTax = SoTax;
                            _header.SoTaxSap = 0;
                            _header.SoTerm = SoTerm;
                            _header.SoTitipan = "";
                            _header.SoToko = SoToko;
                            _header.SoType = SoType;
                            _header.SoUom = SoUom;
                            _header.SoVoucher = "";
                            _header.SoVoucherItem = "";
                            _header.Status = "";
                        };

                        _listHead.Add(_header);

                    }
                }

                if (pg.Count > 0)
                {
                    foreach (var item in pg)
                    {
                        string SoPlu = item.StockProductPG.Nomor;

                        string SoPrice = Str.Number0(item.HargaRupiah).Replace(".", "");
                        string SoDiscount01 = Str.Number0(item.DiscountNominal).Replace(".", "");
                        string SoDiscount02 = Str.Number0(item.DiscountPromo).Replace(".", "");
                        string SoDiscount03 = Str.Number0(item.DiscountProgramNominal).Replace(".", "");
                        string SoDiscount04 = Str.Number0(item.DiscountGift).Replace(".", "");
                        string SoDiscount05 = Str.Number0(item.DiscountBank).Replace(".", "");
                        string SoDiscount06 = Str.Number0(item.DiscountOther).Replace(".", "");
                        string SoDiscount07 = Str.Number0(item.DiscountRound).Replace(".", "");
                        string SoDiscount08 = "";
                        string SoDiscount09 = "";
                        string SoDiscount10 = "";
                        string SoTax = Str.Number0(Math.Round((item.TotalBayar.Value / 1.02m * 2) / 100)).Replace(".", "");

                        ZsdstSoHeader _header = new ZsdstSoHeader();
                        {
                            _header.BillNoSap = "";
                            _header.DoNoSap = "";
                            _header.ItemProcess = "";
                            _header.Message = "";
                            _header.PgiNoSap = "";
                            _header.ReprocessProcess = "";
                            _header.RunnoProcess = "";
                            _header.SoCategory = "";
                            _header.SoCurrency = SoCurrency;
                            _header.SoCustomer = SoCustomer;
                            _header.SoDc = SoDc;
                            _header.SoDiscount01 = SoDiscount01;
                            _header.SoDiscount02 = SoDiscount02;
                            _header.SoDiscount03 = SoDiscount03;
                            _header.SoDiscount04 = SoDiscount04;
                            _header.SoDiscount05 = SoDiscount05;
                            _header.SoDiscount06 = SoDiscount06;
                            _header.SoDiscount07 = SoDiscount07;
                            _header.SoDiscount08 = SoDiscount08;
                            _header.SoDiscount09 = SoDiscount09;
                            _header.SoDiscount10 = SoDiscount10;
                            _header.SoDivision = SoDivision;
                            _header.SoExecutive = SoExecutive;
                            _header.SoGroup = SoGroup;
                            _header.SoJasa1 = "";
                            _header.SoJasa2 = "";
                            _header.SoJasa3 = "";
                            _header.SoMatcode = "";
                            _header.SoNo = SoNo;
                            _header.SoNoSap = "";
                            _header.SoOffice = SoOffice;
                            _header.SoOrg = SoOrg;
                            _header.SoPlant = "";
                            _header.SoPlu = SoPlu;
                            _header.SoPrice = SoPrice;
                            _header.SoPriceSap = 0;
                            _header.SoQty = SoQty;
                            _header.SoQtySap = 0;
                            _header.SoStLoc = "";
                            _header.SoTax = SoTax;
                            _header.SoTaxSap = 0;
                            _header.SoTerm = SoTerm;
                            _header.SoTitipan = "";
                            _header.SoToko = SoToko;
                            _header.SoType = SoType;
                            _header.SoUom = SoUom;
                            _header.SoVoucher = "";
                            _header.SoVoucherItem = "";
                            _header.Status = "";
                        };

                        _listHead.Add(_header);

                    }
                }

                if (gj.Count > 0)
                {
                    foreach (var item in gj)
                    {
                        string SoPlu = item.StockProductGJ.Nomor;

                        string SoPrice = Str.Number0(item.HargaRupiah).Replace(".", "");
                        string SoDiscount01 = Str.Number0(item.DiscountNominal).Replace(".", "");
                        string SoDiscount02 = Str.Number0(item.DiscountPromo).Replace(".", "");
                        string SoDiscount03 = Str.Number0(item.DiscountProgramNominal).Replace(".", "");
                        string SoDiscount04 = Str.Number0(item.DiscountGift).Replace(".", "");
                        string SoDiscount05 = Str.Number0(item.DiscountBank).Replace(".", "");
                        string SoDiscount06 = Str.Number0(item.DiscountOther).Replace(".", "");
                        string SoDiscount07 = Str.Number0(item.DiscountRound).Replace(".", "");
                        string SoDiscount08 = "";
                        string SoDiscount09 = "";
                        string SoDiscount10 = "";
                        string SoTax = Str.Number0(Math.Round((item.TotalBayar.Value / 1.02m * 2) / 100)).Replace(".", "");

                        ZsdstSoHeader _header = new ZsdstSoHeader();
                        {
                            _header.BillNoSap = "";
                            _header.DoNoSap = "";
                            _header.ItemProcess = "";
                            _header.Message = "";
                            _header.PgiNoSap = "";
                            _header.ReprocessProcess = "";
                            _header.RunnoProcess = "";
                            _header.SoCategory = "";
                            _header.SoCurrency = SoCurrency;
                            _header.SoCustomer = SoCustomer;
                            _header.SoDc = SoDc;
                            _header.SoDiscount01 = SoDiscount01;
                            _header.SoDiscount02 = SoDiscount02;
                            _header.SoDiscount03 = SoDiscount03;
                            _header.SoDiscount04 = SoDiscount04;
                            _header.SoDiscount05 = SoDiscount05;
                            _header.SoDiscount06 = SoDiscount06;
                            _header.SoDiscount07 = SoDiscount07;
                            _header.SoDiscount08 = SoDiscount08;
                            _header.SoDiscount09 = SoDiscount09;
                            _header.SoDiscount10 = SoDiscount10;
                            _header.SoDivision = SoDivision;
                            _header.SoExecutive = SoExecutive;
                            _header.SoGroup = SoGroup;
                            _header.SoJasa1 = "";
                            _header.SoJasa2 = "";
                            _header.SoJasa3 = "";
                            _header.SoMatcode = "";
                            _header.SoNo = SoNo;
                            _header.SoNoSap = "";
                            _header.SoOffice = SoOffice;
                            _header.SoOrg = SoOrg;
                            _header.SoPlant = "";
                            _header.SoPlu = SoPlu;
                            _header.SoPrice = SoPrice;
                            _header.SoPriceSap = 0;
                            _header.SoQty = SoQty;
                            _header.SoQtySap = 0;
                            _header.SoStLoc = "";
                            _header.SoTax = SoTax;
                            _header.SoTaxSap = 0;
                            _header.SoTerm = SoTerm;
                            _header.SoTitipan = "";
                            _header.SoToko = SoToko;
                            _header.SoType = SoType;
                            _header.SoUom = SoUom;
                            _header.SoVoucher = "";
                            _header.SoVoucherItem = "";
                            _header.Status = "";
                        };

                        _listHead.Add(_header);

                    }
                }

                if (ld.Count > 0)
                {
                    foreach (var item in ld)
                    {
                        string SoPlu = item.StockProductLD_Stone1B.Nomor;

                        string SoPrice = Str.Number0(item.HargaRupiah).Replace(".", "");
                        string SoDiscount01 = Str.Number0(item.DiscountNominal).Replace(".", "");
                        string SoDiscount02 = Str.Number0(item.DiscountPromo).Replace(".", "");
                        string SoDiscount03 = Str.Number0(item.DiscountProgramNominal).Replace(".", "");
                        string SoDiscount04 = Str.Number0(item.DiscountGift).Replace(".", "");
                        string SoDiscount05 = Str.Number0(item.DiscountBank).Replace(".", "");
                        string SoDiscount06 = Str.Number0(item.DiscountOther).Replace(".", "");
                        string SoDiscount07 = Str.Number0(item.DiscountRound).Replace(".", "");
                        string SoDiscount08 = "";
                        string SoDiscount09 = "";
                        string SoDiscount10 = "";
                        string SoTax = Str.Number0(Math.Round((item.TotalBayar.Value / 1.02m * 2) / 100)).Replace(".", "");

                        ZsdstSoHeader _header = new ZsdstSoHeader();
                        {
                            _header.BillNoSap = "";
                            _header.DoNoSap = "";
                            _header.ItemProcess = "";
                            _header.Message = "";
                            _header.PgiNoSap = "";
                            _header.ReprocessProcess = "";
                            _header.RunnoProcess = "";
                            _header.SoCategory = "";
                            _header.SoCurrency = SoCurrency;
                            _header.SoCustomer = SoCustomer;
                            _header.SoDc = SoDc;
                            _header.SoDiscount01 = SoDiscount01;
                            _header.SoDiscount02 = SoDiscount02;
                            _header.SoDiscount03 = SoDiscount03;
                            _header.SoDiscount04 = SoDiscount04;
                            _header.SoDiscount05 = SoDiscount05;
                            _header.SoDiscount06 = SoDiscount06;
                            _header.SoDiscount07 = SoDiscount07;
                            _header.SoDiscount08 = SoDiscount08;
                            _header.SoDiscount09 = SoDiscount09;
                            _header.SoDiscount10 = SoDiscount10;
                            _header.SoDivision = SoDivision;
                            _header.SoExecutive = SoExecutive;
                            _header.SoGroup = SoGroup;
                            _header.SoJasa1 = "";
                            _header.SoJasa2 = "";
                            _header.SoJasa3 = "";
                            _header.SoMatcode = "";
                            _header.SoNo = SoNo;
                            _header.SoNoSap = "";
                            _header.SoOffice = SoOffice;
                            _header.SoOrg = SoOrg;
                            _header.SoPlant = "";
                            _header.SoPlu = SoPlu;
                            _header.SoPrice = SoPrice;
                            _header.SoPriceSap = 0;
                            _header.SoQty = SoQty;
                            _header.SoQtySap = 0;
                            _header.SoStLoc = "";
                            _header.SoTax = SoTax;
                            _header.SoTaxSap = 0;
                            _header.SoTerm = SoTerm;
                            _header.SoTitipan = "";
                            _header.SoToko = SoToko;
                            _header.SoType = SoType;
                            _header.SoUom = SoUom;
                            _header.SoVoucher = "";
                            _header.SoVoucherItem = "";
                            _header.Status = "";
                        };

                        _listHead.Add(_header);

                    }
                }

                _input.SalesOrder = _listHead.ToArray();

                cred.UserName = SAP.xUser;
                cred.Password = SAP.xPwd;

                _proxy.PreAuthenticate = true;
                _proxy.Credentials = cred;
            
                _output = _proxy.ZsdfmSdIntrfc(_input);
            }

            return _output;
            }
}
    
}

