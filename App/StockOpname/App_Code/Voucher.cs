using System;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

/// <summary>
/// Voucher Checking System
/// </summary>
public partial class Voucher
{
    
    public static void isDoubleRewardText(Literal lit)
    {
        string xPromoMembership = "";

        // hardcode promo nasional 2019 Februari : double reward membership redemption
        if (System.DateTime.Now >= Convert.ToDateTime("2019-02-01 00:00:00.000")
            && System.DateTime.Now < Convert.ToDateTime("2019-04-01 00:00:00.000")
            )
        {
            xPromoMembership = "Double Reward Promo Nasional (1 Feb 2019 - 31 Mar 2019).";
        }

        lit.Text = (xPromoMembership != "") ? "<span data-toggle='tooltip' title='' data-placement='bottom' class='glyphicon glyphicon-info-sign' style='font-size:13px; color:green; font-weight:normal;' data-original-title='" + xPromoMembership + "'></span>" : "";

    }

    public static string isValidVoucher(string voucher_type_allowed, string NomorVoucher, int IDSO, int IDBRAND, string CUSTCODE, int ItemType)
    {
        using (Map_CS cs = new Map_CS())
        {

            string result = "";
        
            /// RULE START
        
            string x_vcr_nomor = "";
            string x_vcr_jenisvoucher = "";
            string x_vcr_namatype = "";
            string x_vcr_startdate = "";
            string x_vcr_expiredate = "";
            string x_vcr_klaimdoc = "";
            string x_vcr_customercodevoucher = "";
            string x_vcr_kodejenisvoucher = "";

            var rs = (from p in cs.T_voucher
                      join q in cs.Tb_type_voucher on p.M_type equals q.Ckodetype
                      where p.M_nomor.Equals(NomorVoucher)
                      select new { p, q }).FirstOrDefault();

            if(rs != null)
            {
                x_vcr_nomor = rs.p.M_nomor.ToString();
                x_vcr_jenisvoucher = rs.q.Cjenisvoucher.ToString();
                x_vcr_namatype = rs.q.Cnamatype.ToString();
                x_vcr_klaimdoc = (rs.p.M_klaimdoc == null) ? "" : rs.p.M_klaimdoc.ToString();
                x_vcr_startdate = rs.p.M_tanggal.ToString();
                x_vcr_expiredate = rs.p.M_expiredate.ToString();
                x_vcr_customercodevoucher = rs.p.M_kodecust.ToString();
                x_vcr_kodejenisvoucher = rs.p.M_type.ToString();

                // cek validation (general)
                if (result == "")
                {
                    result = isValidVoucherRule_General(x_vcr_nomor, voucher_type_allowed, x_vcr_jenisvoucher, x_vcr_namatype, x_vcr_klaimdoc, x_vcr_startdate, x_vcr_expiredate);
                }

                // cek validation (discount)
                if (result == "" && voucher_type_allowed == "DISCOUNT")
                {
                    result = isValidVoucherRule_Discount(x_vcr_nomor, IDSO, CUSTCODE, x_vcr_customercodevoucher, x_vcr_kodejenisvoucher, x_vcr_namatype, ItemType);
                }

                // cek validation (payment)
                if (result == "" && voucher_type_allowed == "PAYMENT")
                {
                    result = isValidVoucherRule_Payment(x_vcr_nomor, IDSO, CUSTCODE);
                }
            }
            else
            {
                result = "Kode voucher <b>" + NomorVoucher + "</b> tidak ditemukan. Silahkan periksa kode voucher kembali. ";
            }
        
            /// RULE END

            return result;
            }
    }

    public static string isValidVoucherRule_Discount(string NomorVoucher, int IDSO, string CUSTCODE, string KodeVoucherCustomer, string KodeJenisVoucher, string NamaJenisVoucher, int ItemType)
    {
        using (Map_DB db = new Map_DB())
        {

            string result = "";

            /// RULE START
        
            // cek used by same sales order (jaws)
            if (result == "")
            {
                var vc_used = (from p in db.SalesVoucher where p.NomorVoucher == NomorVoucher && p.IDSalesOrder == IDSO select p);
            
                if (result == "" && vc_used.Count() > 0)
                {
                    result = "Double input Kode voucher <b>" + vc_used.FirstOrDefault().NomorVoucher + "</b>. Silahkan periksa kode voucher kembali. ";
                }

            }

            // cek used by other sales order (draft) (jaws)
            if (result == "")
            {
                var vc_used = (from p in db.SalesVoucher where p.NomorVoucher == NomorVoucher && p.SalesOrder.Draft == true && p.SalesOrder.StatusVoid == false select p);
            
                if (result == "" && vc_used.Count() > 0)
                {
                    result = "Kode voucher sudah pernah terinput pada Sales Order (Draft) oleh <b>"
                        + vc_used.FirstOrDefault().Operator + " (" + Str.FullDateJam(vc_used.FirstOrDefault().OperatorTgl)
                        + ")</b>. Silahkan periksa kode voucher kembali. "
                        ;
                }

            }

            // cek penggunaan voucher selain membership redemption pada pg dan gj
            if (result == "")
            {
                if (!(KodeJenisVoucher == "07") && (ItemType == 2 || ItemType == 3))
                {
                    result = "<b>" + NamaJenisVoucher + "</b> dengan kode voucher <b>" + NomorVoucher
                            + "</b> tidak dapat digunakan. Barang jenis <b>Plain Gold</b> atau <b>Gold Jewellery</b> hanya dapat menggunakan voucher jenis <b>MEMBERSHIP REDEMPTION</b>"
                            ;
                }

            }

            // cek birthday voucher (05) & membership redemption (07)
            if (result == "")
            {
                if ((KodeJenisVoucher == "05" || KodeJenisVoucher == "07") && KodeVoucherCustomer != CUSTCODE)
                {
                    result = "<b>" + NamaJenisVoucher + "</b> dengan kode voucher <b>" + NomorVoucher
                            + "</b> tidak dapat digunakan. Kode customer pada voucher <b>'" + KodeVoucherCustomer + "'</b> yang digunakan tidak sesuai <b>'"
                            + CUSTCODE + "'</b>"
                            ;
                }

            }

            /// RULE END

            return result;
            }
    }

    public static string isValidVoucherRule_Payment(string NomorVoucher, int IDSO, string CUSTCODE)
    {
        string result = "";
        
        /// RULE START
        


        /// RULE END

        return result;
    }

    protected static string isValidVoucherRule_General(string VoucherCode, string AllowedVoucher, string VoucherType, string VoucherNameType, string DocumentNumber
        , string DateStart, string DateExpired
        )
    {
        string result = "";
        
        // cek allowed (by jenis voucher)
        if (result == "")
        {
            if (AllowedVoucher != VoucherType)
            {
                result = "Kode voucher <b>" + VoucherCode + "</b> terdaftar dalam sistem sebagai Voucher <b>"
                        + VoucherNameType + " (" + VoucherType + ")"
                        + "</b>. Voucher tidak dapat digunakan untuk proses <b>" + AllowedVoucher + "</b>. "
                        ;
            }

        }

        // cek expired
        if (result == "")
        {
            if (!(Convert.ToDateTime(DateExpired) >= DateTime.Today))
            {
                result = "Kode voucher <b>" + VoucherCode + "</b> tidak dapat digunakan. Voucher expired per <b>"
                        + Str.FullDateJam(Convert.ToDateTime(DateExpired)) + "</b>"
                        ;
            }
        }

        // cek used (dbcmk)
        if (result == "")
        {
            // check utk table voucher dbcmk
            if (result == "" && DocumentNumber != "")
            {
                result = "Kode voucher <b>" + VoucherCode + "</b> sudah pernah digunakan Sales Order # <b>" + DocumentNumber + "</b>. Silahkan periksa kode voucher kembali. ";
            }

        }


        return result;
    }
}
