using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Library.SAP_ZFIFM_POST_FI_NEW;

public partial class SAPM
{
    public static ZfifmPostFiNewResponse PostDataPaymentSalesOrderToWs(SalesOrder d, Literal lit)
    {
        using (Map_DB db = new Map_DB())
        {

            string spname = "";
            string query = "";
            DataTable dt;

            string _x1_FiAllocation = "";
            string _x1_FiAmount = "";
            string _x1_FiCategory = "";
            string _x1_FiCc = "";
            string _x1_FiCurrency = "";
            string _x1_FiDate = "";
            string _x1_FiGlAccount = "";
            string _x1_FiHeaderText = "";
            string _x1_FiItem = "";
            string _x1_FiRefKey = "";
            string _x1_FiKeySap = "";
            string _x1_FiName = "";
            string _x1_FiNo = "";
            string _x1_FiPc = "";
            string _x1_FiRate = "";
            string _x1_FiSpecialGl = "";
            string _x1_FiText = "";
            string _x1_FiTypeSap = "";
            string _x1_FiVC = "";
            string _x1_Message = "";
            string _x1_Status = "";

            string _x2_FiAllocation = "";
            string _x2_FiAmount = "";
            string _x2_FiCategory = "";
            string _x2_FiCc = "";
            string _x2_FiCurrency = "";
            string _x2_FiDate = "";
            string _x2_FiGlAccount = "";
            string _x2_FiHeaderText = "";
            string _x2_FiItem = "";
            string _x2_FiRefKey = "";
            string _x2_FiKeySap = "";
            string _x2_FiName = "";
            string _x2_FiNo = "";
            string _x2_FiPc = "";
            string _x2_FiRate = "";
            string _x2_FiSpecialGl = "";
            string _x2_FiText = "";
            string _x2_FiTypeSap = "";
            string _x2_FiVC = "";
            string _x2_Message = "";
            string _x2_Status = "";

            // BILL NUMBER
            string _BillNoSap = "";

            spname = "[SP_SALESORDER_GET_DATA]";
            query = " exec " + spname
                    + Str.isString(d.Nomor, false)
                    ;

            dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    _BillNoSap = dtRow["BillNoSap"].ToString();
                }
            }

            // PROFIT CENTER
            string _KodePlant = "";

            string xKodeLokasi = "";
            string xIDBrand = "";
            string xTipeLokasi = "";
            string xIDLokasi = "";

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                xKodeLokasi = loc.Kode.ToString();
                xIDBrand = loc.IDBrand.ToString();
                xTipeLokasi = d.TipeLokasi.ToString();
                xIDLokasi = d.IDLokasi.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                xKodeLokasi = loc.Kode.ToString();
                xIDBrand = loc.IDBrand.ToString();
                xTipeLokasi = d.TipeLokasi.ToString();
                xIDLokasi = d.IDLokasi.ToString();
            }

            spname = "[SP_SAP_LOCATION_VALIDATION]";
            query = " exec " + spname
                    + Str.isString(xKodeLokasi, false)
                    + Str.isString(xIDBrand)
                    + Str.isString(xTipeLokasi)
                    + Str.isString(xIDLokasi)
                    ;

            dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    if (Convert.ToInt32(dtRow["isValid"].ToString()) == 1)
                    {
                        _KodePlant = dtRow["SAP_MM_KODEPLANT"].ToString();
                    }
                }
            }
        
            System.Net.NetworkCredential cred = new System.Net.NetworkCredential();

            ZFIFM_POST_FI_NEW _proxy = new ZFIFM_POST_FI_NEW();
            ZfifmPostFiNew _input = new ZfifmPostFiNew();
            ZfifmPostFiNewResponse _output = new ZfifmPostFiNewResponse();

            List<ZfistPostFi> _listPayment = new List<ZfistPostFi>();

            var x = db.SalesReceiptDetail.Where(p => p.SalesReceipt.IDSalesOrder == d.ID);

            string _Nomor = d.Nomor;
            string _NoCustomer = d.DataCustomer.NoCustomer;
            string _Date = d.Tgl.Value.ToString("dd.MM.yyyy");

            string _Item1 = "";
            string _Item2 = "";

            string _Currency = "IDR";

            if (x.Count() > 0)
            {
                int index = 1;

                foreach(var data in x)
                {
                    _Item1 = index.ToString();
                    index++;
                    _Item2 = index.ToString();
                    index++;

                    decimal _dAmmount = 0;
                    string _Ammount = "";
                    string _Ammount2 = "";

                    string _Category = "";
                    string _GLAccount = "";
                    string _EDC = "";
                    string _ApprovalCode = (data.SalesReceiptDetailExtension != null) ? data.SalesReceiptDetailExtension.TransactionNumber : "";
                    string _PaymentNumber = (data.CCNumber != null) ? data.CCNumber : "";
                    string _PaymentName = (data.CCName != null) ? data.CCName : "";

                    int iIDPaymentType = 0;
                    int iEDC = 0;

                    iIDPaymentType = data.IDPaymentType;
                    iEDC = data.IDEDC.HasValue ? data.IDEDC.Value : 0;

                    if (new[] { 1, 2, 4 }.Contains(iIDPaymentType))
                    {
                        // 02-01-2019 : post SAP hanya metoda pembayaran : cash, debit, credit
                        // 03-01-2019 : post SAP tambah metoda pembayaran : down payment, bridestorypay, voucher

                        /* SAP FiCategory
                        A - Beban
                        B - Pembayarn ke Vendor
                        C - Pembayarn Customer (Kartu Kredit / Debit) 
                        D - Pembayaran Customer (Tunai)
                        E - Utang Titipan Customer (Kartu Kredit / Debit)
                        F - Utang Titipan Customer (Tunai)
                        */

                        /* JAWS PAYMENT TYPE
                        // 1 : CASH 
                        // 2 : CREDIT CARD
                        // 4 : DEBIT CARD 
                        // 5 : VOUCHER
                        // 8 : DOWN PAYMENT
                        // 10 : BRIDESTORY PAY
                        */

                        /* JAWS EDC
                        // 1 : BCA 
                        // 2 : MANDIRI
                        */

                        if (new[] { 1 }.Contains(iIDPaymentType))
                        {
                            // CASH
                            _Category = "D";
                            _GLAccount = "1101101001";

                            _dAmmount = data.Nominal.HasValue ? data.Nominal.Value : 0;
                            _Ammount = Str.Number0(_dAmmount).Replace(".", "");
                            _Ammount2 = Str.Number0(_dAmmount * -1).Replace(".", "");

                            _x1_FiCategory = _Category;
                            _x1_FiNo = _BillNoSap;
                            _x1_FiItem = _Item1;
                            _x1_FiRefKey = "";
                            _x1_FiDate = _Date;
                            _x1_FiHeaderText = _Nomor;
                            _x1_FiGlAccount = _GLAccount;
                            _x1_FiVC = "";
                            _x1_FiSpecialGl = "";
                            _x1_FiName = "";
                            _x1_FiAmount = _Ammount;
                            _x1_FiCurrency = _Currency;
                            _x1_FiRate = "";
                            _x1_FiAllocation = _BillNoSap;
                            _x1_FiCc = "";
                            _x1_FiPc = _KodePlant;
                            _x1_FiText = _Nomor;
                            _x1_Message = "";
                            _x1_Status = "";
                            _x1_FiTypeSap = "";
                            _x1_FiKeySap = "";
                        
                            _x2_FiCategory = _Category;
                            _x2_FiNo = _BillNoSap;
                            _x2_FiItem = _Item2;
                            _x2_FiRefKey = "";
                            _x2_FiDate = _Date;
                            _x2_FiHeaderText = _Nomor;
                            _x2_FiGlAccount = "";
                            _x2_FiVC = _NoCustomer;
                            _x2_FiSpecialGl = "";
                            _x2_FiName = "";
                            _x2_FiAmount = _Ammount2;
                            _x2_FiCurrency = _Currency;
                            _x2_FiRate = "";
                            _x2_FiAllocation = _BillNoSap;
                            _x2_FiCc = "";
                            _x2_FiPc = _KodePlant;
                            _x2_FiText = _Nomor;
                            _x2_Message = "";
                            _x2_Status = "";
                            _x2_FiTypeSap = "";
                            _x2_FiKeySap = "";
                        
                        }
                        else if (new[] { 2, 4 }.Contains(iIDPaymentType))
                        {
                            // CREDIT CARD / DEBIT CARD
                            _Category = "C";
                        
                            if (iEDC == 1)
                            {
                                // BCA
                                _GLAccount = "1104101002";
                            }
                            else if (iEDC == 2)
                            {
                                // MANDIRI
                                _GLAccount = "1104101003";
                            }
                            else
                            {
                                // LAIN LAIN
                                _GLAccount = "1104101004";
                            }

                            _EDC = (data.EDCList != null) ? data.EDCList.Nama : "";
                            _dAmmount = data.Nominal.HasValue ? data.Nominal.Value : 0;
                            _Ammount = Str.Number0(_dAmmount).Replace(".", "");
                            _Ammount2 = Str.Number0(_dAmmount * -1).Replace(".", "");

                            _x1_FiCategory = _Category;
                            _x1_FiNo = _BillNoSap;
                            _x1_FiItem = _Item1;
                            _x1_FiRefKey = _EDC;
                            _x1_FiDate = _Date;
                            _x1_FiHeaderText = _Nomor;
                            _x1_FiGlAccount = _GLAccount;
                            _x1_FiVC = "";
                            _x1_FiSpecialGl = "";
                            _x1_FiName = "";
                            _x1_FiAmount = _Ammount;
                            _x1_FiCurrency = _Currency;
                            _x1_FiRate = "";
                            _x1_FiAllocation = _ApprovalCode;
                            _x1_FiCc = "";
                            _x1_FiPc = _KodePlant;
                            _x1_FiText = _PaymentName;
                            _x1_Message = "";
                            _x1_Status = "";
                            _x1_FiTypeSap = "";
                            _x1_FiKeySap = "";

                            _x2_FiCategory = _Category;
                            _x2_FiNo = _BillNoSap;
                            _x2_FiItem = _Item2;
                            _x2_FiRefKey = "";
                            _x2_FiDate = _Date;
                            _x2_FiHeaderText = _Nomor;
                            _x2_FiGlAccount = "";
                            _x2_FiVC = _NoCustomer;
                            _x2_FiSpecialGl = "";
                            _x2_FiName = "";
                            _x2_FiAmount = _Ammount2;
                            _x2_FiCurrency = _Currency;
                            _x2_FiRate = "";
                            _x2_FiAllocation = _BillNoSap;
                            _x2_FiCc = "";
                            _x2_FiPc = _KodePlant;
                            _x2_FiText = _Nomor;
                            _x2_Message = "";
                            _x2_Status = "";
                            _x2_FiTypeSap = "";
                            _x2_FiKeySap = "";

                        }
                        else if (new[] { 5 }.Contains(iIDPaymentType))
                        {
                            // VOUCHER
                            _Category = "I";
                            _GLAccount = "2104101001";

                            _dAmmount = data.Nominal.HasValue ? data.Nominal.Value : 0;
                            _Ammount = Str.Number0(_dAmmount).Replace(".", "");
                            _Ammount2 = Str.Number0(_dAmmount * -1).Replace(".", "");

                            _x1_FiCategory = _Category;
                            _x1_FiNo = _BillNoSap;
                            _x1_FiItem = _Item1;
                            _x1_FiRefKey = "";
                            _x1_FiDate = _Date;
                            _x1_FiHeaderText = _PaymentNumber;
                            _x1_FiGlAccount = _GLAccount;
                            _x1_FiVC = "";
                            _x1_FiSpecialGl = "";
                            _x1_FiName = "";
                            _x1_FiAmount = _Ammount;
                            _x1_FiCurrency = _Currency;
                            _x1_FiRate = "";
                            _x1_FiAllocation = _PaymentNumber;
                            _x1_FiCc = "";
                            _x1_FiPc = _KodePlant;
                            _x1_FiText = _Nomor;
                            _x1_Message = "";
                            _x1_Status = "";
                            _x1_FiTypeSap = "";
                            _x1_FiKeySap = "";
                        
                            _x2_FiCategory = _Category;
                            _x2_FiNo = _BillNoSap;
                            _x2_FiItem = _Item2;
                            _x2_FiRefKey = "";
                            _x2_FiDate = _Date;
                            _x2_FiHeaderText = _PaymentNumber;
                            _x2_FiGlAccount = "";
                            _x2_FiVC = _NoCustomer;
                            _x2_FiSpecialGl = "";
                            _x2_FiName = "";
                            _x2_FiAmount = _Ammount2;
                            _x2_FiCurrency = _Currency;
                            _x2_FiRate = "";
                            _x2_FiAllocation = _PaymentNumber;
                            _x2_FiCc = "";
                            _x2_FiPc = _KodePlant;
                            _x2_FiText = _Nomor;
                            _x2_Message = "";
                            _x2_Status = "";
                            _x2_FiTypeSap = "";
                            _x2_FiKeySap = "";

                        }
                        else if (new[] { 10 }.Contains(iIDPaymentType))
                        {
                            // BRIDE STORY PAY
                            _Category = "H";
                            _GLAccount = "1104101008";

                            _dAmmount = data.Nominal.HasValue ? data.Nominal.Value : 0;
                            _Ammount = Str.Number0(_dAmmount).Replace(".", "");
                            _Ammount2 = Str.Number0(_dAmmount * -1).Replace(".", "");

                            _x1_FiCategory = _Category;
                            _x1_FiNo = _BillNoSap;
                            _x1_FiItem = _Item1;
                            _x1_FiRefKey = "";
                            _x1_FiDate = _Date;
                            _x1_FiHeaderText = _PaymentNumber;
                            _x1_FiGlAccount = _GLAccount;
                            _x1_FiVC = "";
                            _x1_FiSpecialGl = "";
                            _x1_FiName = "";
                            _x1_FiAmount = _Ammount;
                            _x1_FiCurrency = _Currency;
                            _x1_FiRate = "";
                            _x1_FiAllocation = _PaymentNumber;
                            _x1_FiCc = "";
                            _x1_FiPc = _KodePlant;
                            _x1_FiText = _Nomor;
                            _x1_Message = "";
                            _x1_Status = "";
                            _x1_FiTypeSap = "";
                            _x1_FiKeySap = "";
                        
                            _x2_FiCategory = _Category;
                            _x2_FiNo = _BillNoSap;
                            _x2_FiItem = _Item2;
                            _x2_FiRefKey = "";
                            _x2_FiDate = _Date;
                            _x2_FiHeaderText = _PaymentNumber;
                            _x2_FiGlAccount = "";
                            _x2_FiVC = _NoCustomer;
                            _x2_FiSpecialGl = "";
                            _x2_FiName = "";
                            _x2_FiAmount = _Ammount2;
                            _x2_FiCurrency = _Currency;
                            _x2_FiRate = "";
                            _x2_FiAllocation = _PaymentNumber;
                            _x2_FiCc = "";
                            _x2_FiPc = _KodePlant;
                            _x2_FiText = _Nomor;
                            _x2_Message = "";
                            _x2_Status = "";
                            _x2_FiTypeSap = "";
                            _x2_FiKeySap = "";
                        
                        }
                        else if (new[] { 2, 4 }.Contains(iIDPaymentType))
                        {
                            // PELUNASAN DOWN PAYMENT
                            _Category = "G";
                        
                            _dAmmount = data.Nominal.HasValue ? data.Nominal.Value : 0;
                            _Ammount = Str.Number0(_dAmmount).Replace(".", "");
                            _Ammount2 = Str.Number0(_dAmmount * -1).Replace(".", "");

                            _x1_FiCategory = _Category;
                            _x1_FiNo = _BillNoSap;
                            _x1_FiItem = _Item1;
                            _x1_FiRefKey = "";
                            _x1_FiDate = _Date;
                            _x1_FiHeaderText = _BillNoSap;
                            _x1_FiGlAccount = "";
                            _x1_FiVC = _NoCustomer;
                            _x1_FiSpecialGl = "A";
                            _x1_FiName = "";
                            _x1_FiAmount = _Ammount;
                            _x1_FiCurrency = _Currency;
                            _x1_FiRate = "";
                            _x1_FiAllocation = "";
                            _x1_FiCc = "";
                            _x1_FiPc = _KodePlant;
                            _x1_FiText = _Nomor;
                            _x1_Message = "";
                            _x1_Status = "";
                            _x1_FiTypeSap = "";
                            _x1_FiKeySap = "";

                            _x2_FiCategory = _Category;
                            _x2_FiNo = _BillNoSap;
                            _x2_FiItem = _Item2;
                            _x2_FiRefKey = "";
                            _x2_FiDate = _Date;
                            _x2_FiHeaderText = _BillNoSap;
                            _x2_FiGlAccount = "";
                            _x2_FiVC = _NoCustomer;
                            _x2_FiSpecialGl = "";
                            _x2_FiName = "";
                            _x2_FiAmount = _Ammount2;
                            _x2_FiCurrency = _Currency;
                            _x2_FiRate = "";
                            _x2_FiAllocation = _BillNoSap;
                            _x2_FiCc = "";
                            _x2_FiPc = _KodePlant;
                            _x2_FiText = _Nomor;
                            _x2_Message = "";
                            _x2_Status = "";
                            _x2_FiTypeSap = "";
                            _x2_FiKeySap = "";

                        }

                        // prepare to post
                        if (_dAmmount != 0)
                        {
                            ZfistPostFi _payment1 = new ZfistPostFi();
                            {
                                _payment1.FiAllocation = _x1_FiAllocation;
                                _payment1.FiAmount = _x1_FiAmount;
                                _payment1.FiCategory = _x1_FiCategory;
                                _payment1.FiCc = _x1_FiCc;
                                _payment1.FiCurrency = _x1_FiCurrency;
                                _payment1.FiDate = _x1_FiDate;
                                _payment1.FiGlAccount = _x1_FiGlAccount;
                                _payment1.FiHeaderText = _x1_FiHeaderText;
                                _payment1.FiItem = _x1_FiItem;
                                _payment1.FiRefKey = _x1_FiRefKey;
                                _payment1.FiKeySap = _x1_FiKeySap;
                                _payment1.FiName = _x1_FiName;
                                _payment1.FiNo = _x1_FiNo;
                                _payment1.FiPc = _x1_FiPc;
                                _payment1.FiRate = _x1_FiRate;
                                _payment1.FiSpecialGl = _x1_FiSpecialGl;
                                _payment1.FiText = _x1_FiText;
                                _payment1.FiTypeSap = _x1_FiTypeSap;
                                _payment1.FiVC = _x1_FiVC;
                                _payment1.Message = _x1_Message;
                                _payment1.Status = _x1_Status;
                            };

                            _listPayment.Add(_payment1);

                            ZfistPostFi _payment2 = new ZfistPostFi();
                            {
                                _payment2.FiAllocation = _x2_FiAllocation;
                                _payment2.FiAmount = _x2_FiAmount;
                                _payment2.FiCategory = _x2_FiCategory;
                                _payment2.FiCc = _x2_FiCc;
                                _payment2.FiCurrency = _x2_FiCurrency;
                                _payment2.FiDate = _x2_FiDate;
                                _payment2.FiGlAccount = _x2_FiGlAccount;
                                _payment2.FiHeaderText = _x2_FiHeaderText;
                                _payment2.FiItem = _x2_FiItem;
                                _payment2.FiRefKey = _x2_FiRefKey;
                                _payment2.FiKeySap = _x2_FiKeySap;
                                _payment2.FiName = _x2_FiName;
                                _payment2.FiNo = _x2_FiNo;
                                _payment2.FiPc = _x2_FiPc;
                                _payment2.FiRate = _x2_FiRate;
                                _payment2.FiSpecialGl = _x2_FiSpecialGl;
                                _payment2.FiText = _x2_FiText;
                                _payment2.FiTypeSap = _x2_FiTypeSap;
                                _payment2.FiVC = _x2_FiVC;
                                _payment2.Message = _x2_Message;
                                _payment2.Status = _x2_Status;
                            };

                            _listPayment.Add(_payment2);

                        }
                    }
                }
            }

            _input.Data = _listPayment.ToArray();
        
            cred.UserName = SAP.xUser;
            cred.Password = SAP.xPwd;

            _proxy.PreAuthenticate = true;
            _proxy.Credentials = cred;
        
            _output = _proxy.ZfifmPostFiNew(_input);
        
            return _output;
            }
}
    
}

