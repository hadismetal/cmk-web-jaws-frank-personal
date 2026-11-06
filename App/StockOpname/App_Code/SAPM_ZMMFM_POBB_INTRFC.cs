using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.SAP_ZMMFM_POBB_INTRFC_NEW;

public partial class SAPM
{
    public static ZmmfmPobbIntrfcNewResponse PostDataResellToWs(Resell d)
    {
        ZMMFM_POBB_INTRFC_NEW _proxy = new ZMMFM_POBB_INTRFC_NEW();
        ZmmfmPobbIntrfcNew _input = new ZmmfmPobbIntrfcNew();
        ZmmfmPobbIntrfcNewResponse _output = new ZmmfmPobbIntrfcNewResponse();

        System.Net.NetworkCredential cred = new System.Net.NetworkCredential();

        using (Map_DB db = new Map_DB())
        {

            /*
            <PoNo>RS/S/1/013/19/01/0008</PoNo>
            <PoItem>10</PoItem>
            <PoDate>14.01.2019</PoDate>
            <PoPerson>Joko</PoPerson>
            <PoPlu>H19000015</PoPlu>
            <PoVendor>E01-13000499</PoVendor>
            <PoStore>M001</PoStore>
            <PoQty>1</PoQty>
            <PoQty2></PoQty2>
            <PoUom>PC</PoUom>
            <PoAmount>230345000</PoAmount>
            <PoCurrency>IDR</PoCurrency>
            <PoNoSap></PoNoSap>
            <Message></Message>
            <Status></Status>
            <GrNoSap></GrNoSap>
            <PoInvoiceSap></PoInvoiceSap>
            <GrYearSap></GrYearSap>
            <PoInvoiceYearSap></PoInvoiceYearSap>
            */

            string PoNo = d.Nomor;
            string PoDate = d.Tgl.Value.ToString("dd.MM.yyyy");
            string PoPerson = d.CustomerNama;
            string PoVendor = d.DataCustomer.NoCustomer; // 
            string PoStore = ""; // KODE PLANT
            string PoQty = "1";
            string PoQty2 = "";
            string PoUom = "PC";
            string PoCurrency = "IDR";

            int IDBrand = 0;
            string xToko = "";

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                IDBrand = (loc).IDBrand;
                xToko = (loc).Kode;
            }
            if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                IDBrand = (loc).IDBrand;
                xToko = (loc).Kode;
            }

            // Validasi Mapping Location Here

            string spname = "[SP_SAP_LOCATION_VALIDATION]";
            string query = " exec " + spname
                         + Str.isString(xToko, false)
                         + Str.isNumber(IDBrand)
                         + Str.isNumber(d.TipeLokasi)
                         + Str.isNumber(d.IDLokasi)
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
                        PoStore = Convert.ToString(dtRow["SAP_MM_KODEPLANT"].ToString()); // key !!!
                    }
                }

            }

            int index = 1;

            if (isValidLocationSAP)
            {
                var dj = d.ResellDJ;
                var pg = d.ResellPG;
                var gj = d.ResellGJ;
                var ld = d.ResellLD;

                List<ZmmstPoBbIntrfc> _listHead = new List<ZmmstPoBbIntrfc>();

                if (dj.Count > 0)
                {
                    foreach (var item in dj)
                    {
                        string PoPlu = item.StockProductDJ.Nomor; //  PLU NUMBER LOOP PER ITEM
                        string PoItem = index.ToString() + "0"; //  INCREMENT : 10, 20, 30, ... etc
                        string PoAmount = Str.Number0(item.HargaRupiah).Replace(".",""); // PRICE LOOP PER ITEM

                        index++;

                        ZmmstPoBbIntrfc _header = new ZmmstPoBbIntrfc();
                        {
                            //PoNo = "RS/S/1/013/19/01/0008";
                            //PoItem = "10";
                            //PoDate = "14.01.2019";
                            //PoPerson = "Joko";
                            //PoPlu = "H19000015";
                            //PoVendor = "E01-13000499";
                            //PoStore = "M001";
                            //PoQty = "1";
                            //PoQty2 = "";
                            //PoUom = "PC";
                            //PoAmount = "230345000";
                            //PoCurrency = "IDR";

                            _header.PoNo = PoNo;
                            _header.PoItem = PoItem;
                            _header.PoDate = PoDate;
                            _header.PoPerson = PoPerson;
                            _header.PoPlu = PoPlu;
                            _header.PoVendor = PoVendor;
                            _header.PoStore = PoStore;
                            _header.PoQty = PoQty;
                            _header.PoQty2 = PoQty2;
                            _header.PoUom = PoUom;
                            _header.PoAmount = PoAmount;
                            _header.PoCurrency = PoCurrency;
                            _header.PoNoSap = "";
                            _header.Message = "";
                            _header.Status = "";
                            _header.GrNoSap = "";
                            _header.PoInvoiceSap = "";
                            _header.GrYearSap = "";
                            _header.PoInvoiceYearSap = "";

                            //_header.RunnoProcess = "";
                            //_header.ItemProcess = "";
                            //_header.ReprocessProcess = "";

    
                            //_header.PoNo = "";
                            //_header.PoItem = "";
                            //_header.PoDate = "";
                            //_header.PoPerson = "";
                            //_header.PoPlu = "";
                            //_header.PoVendor = "";
                            //_header.PoStore = "";
                            //_header.PoQty = "";
                            //_header.PoQty2 = "";
                            //_header.PoUom = "";
                            //_header.PoAmount = "";
                            //_header.PoCurrency = "";
                            //_header.PoNoSap = "";
                            //_header.Message = "";
                            //_header.Status = "";
                            //_header.GrNoSap = "";
                            //_header.PoInvoiceSap = "";
                            //_header.GrYearSap = "";
                            //_header.PoInvoiceYearSap = "";
                            //_header.RunnoProcess = "";
                            //_header.ItemProcess = "";
                            //_header.ReprocessProcess = "";

                        };

                        _listHead.Add(_header);

                    }
                }

                if (pg.Count > 0)
                {
                    foreach (var item in pg)
                    {
                        string PoPlu = item.StockProductPG.Nomor; //  PLU NUMBER LOOP PER ITEM
                        string PoItem = index.ToString() + "0"; //  INCREMENT : 10, 20, 30, ... etc
                        string PoAmount = Str.Number0(item.HargaRupiah).Replace(".", ""); // PRICE LOOP PER ITEM

                        index++;

                        ZmmstPoBbIntrfc _header = new ZmmstPoBbIntrfc();
                        {
                            _header.PoNo = PoNo;
                            _header.PoItem = PoItem;
                            _header.PoDate = PoDate;
                            _header.PoPerson = PoPerson;
                            _header.PoPlu = PoPlu;
                            _header.PoVendor = PoVendor;
                            _header.PoStore = PoStore;
                            _header.PoQty = PoQty;
                            _header.PoQty2 = PoQty2;
                            _header.PoUom = PoUom;
                            _header.PoAmount = PoAmount;
                            _header.PoCurrency = PoCurrency;
                            _header.PoNoSap = "";
                            _header.Message = "";
                            _header.Status = "";
                            _header.GrNoSap = "";
                            _header.PoInvoiceSap = "";
                            _header.GrYearSap = "";
                            _header.PoInvoiceYearSap = "";
                            //_header.RunnoProcess = "";
                            //_header.ItemProcess = "";
                            //_header.ReprocessProcess = "";
                        };

                        _listHead.Add(_header);

                    }
                }

                if (gj.Count > 0)
                {
                    foreach (var item in gj)
                    {
                        string PoPlu = item.StockProductGJ.Nomor; //  PLU NUMBER LOOP PER ITEM
                        string PoItem = index.ToString() + "0"; //  INCREMENT : 10, 20, 30, ... etc
                        string PoAmount = Str.Number0(item.HargaRupiah).Replace(".", ""); // PRICE LOOP PER ITEM

                        index++;

                        ZmmstPoBbIntrfc _header = new ZmmstPoBbIntrfc();
                        {
                            _header.PoNo = PoNo;
                            _header.PoItem = PoItem;
                            _header.PoDate = PoDate;
                            _header.PoPerson = PoPerson;
                            _header.PoPlu = PoPlu;
                            _header.PoVendor = PoVendor;
                            _header.PoStore = PoStore;
                            _header.PoQty = PoQty;
                            _header.PoQty2 = PoQty2;
                            _header.PoUom = PoUom;
                            _header.PoAmount = PoAmount;
                            _header.PoCurrency = PoCurrency;
                            _header.PoNoSap = "";
                            _header.Message = "";
                            _header.Status = "";
                            _header.GrNoSap = "";
                            _header.PoInvoiceSap = "";
                            _header.GrYearSap = "";
                            _header.PoInvoiceYearSap = "";
                            //_header.RunnoProcess = "";
                            //_header.ItemProcess = "";
                            //_header.ReprocessProcess = "";
                        };

                        _listHead.Add(_header);

                    }
                }

                if (ld.Count > 0)
                {
                    foreach (var item in ld)
                    {
                        string PoPlu = item.StockProductLD_Stone1B.Nomor; //  PLU NUMBER LOOP PER ITEM
                        string PoItem = index.ToString() + "0"; //  INCREMENT : 10, 20, 30, ... etc
                        string PoAmount = Str.Number0(item.HargaRupiah).Replace(".", ""); // PRICE LOOP PER ITEM

                        index++;

                        ZmmstPoBbIntrfc _header = new ZmmstPoBbIntrfc();
                        {
                            _header.PoNo = PoNo;
                            _header.PoItem = PoItem;
                            _header.PoDate = PoDate;
                            _header.PoPerson = PoPerson;
                            _header.PoPlu = PoPlu;
                            _header.PoVendor = PoVendor;
                            _header.PoStore = PoStore;
                            _header.PoQty = PoQty;
                            _header.PoQty2 = PoQty2;
                            _header.PoUom = PoUom;
                            _header.PoAmount = PoAmount;
                            _header.PoCurrency = PoCurrency;
                            _header.PoNoSap = "";
                            _header.Message = "";
                            _header.Status = "";
                            _header.GrNoSap = "";
                            _header.PoInvoiceSap = "";
                            _header.GrYearSap = "";
                            _header.PoInvoiceYearSap = "";
                            //_header.RunnoProcess = "";
                            //_header.ItemProcess = "";
                            //_header.ReprocessProcess = "";
                        };

                        _listHead.Add(_header);

                    }
                }

                _input.PurchaseOrder = _listHead.ToArray();

                cred.UserName = SAP.xUser;
                cred.Password = SAP.xPwd;

                _proxy.PreAuthenticate = true;
                _proxy.Credentials = cred;
            
                _output = _proxy.ZmmfmPobbIntrfcNew(_input);
            }

            return _output;
            }
}
    
}

