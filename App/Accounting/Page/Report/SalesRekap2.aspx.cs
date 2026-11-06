using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;


public partial class Page_Summary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductSegmen(productsegmen);
            Ddl.PaymentType(payment);
            Ddl.Store(locationcode);
            Ddl.TipeLokasi(tipelokasi);

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "1";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTableNew(sender, e);
        }

    }

    //Display Table
    protected void fillTableNew(object sender, EventArgs e)
    {
        string kw_text = kw.Text;
        DateTime start_text = Convert.ToDateTime(dari.Text);
        DateTime end_text = Convert.ToDateTime(sampai.Text);
        string status_text = status.SelectedValue;
        string basic_text = filter.SelectedValue;
        string payment_text = payment.SelectedValue;
        string productitem_text = productitem.SelectedValue;
        string productcategory_text = productcategory.SelectedValue;
        string productsegmen_text = productsegmen.SelectedValue;
        string kodelokasi_text = locationcode.SelectedValue;
        string tipeproduct_text = tipeproduct.SelectedValue;
        string tipelokasi_text = tipelokasi.SelectedValue;
        string filter_text = filter.SelectedValue;        

        string query = "sp_report_sales_detail_accounting_new";
        SqlParameter param_kw = new SqlParameter("@KEYWORD", SqlDbType.VarChar) { Value = kw_text };
        SqlParameter param_start = new SqlParameter("@TANGGALSTART", SqlDbType.DateTime) { Value = start_text };
        SqlParameter param_end = new SqlParameter("@TANGGALEND", SqlDbType.DateTime) { Value = end_text };
        SqlParameter param_loccode = new SqlParameter("@LOCCODE", SqlDbType.VarChar) { Value = kodelokasi_text };
        SqlParameter param_type = new SqlParameter("@PRODUCTTYPE", SqlDbType.VarChar) { Value = tipeproduct_text };
        SqlParameter param_item = new SqlParameter("@ITEM", SqlDbType.Int) { Value = productitem_text };
        SqlParameter param_category = new SqlParameter("@CATEGORY", SqlDbType.Int) { Value = productcategory_text };
        SqlParameter param_segmen = new SqlParameter("@SEGMEN", SqlDbType.Int) { Value = productsegmen_text };
        SqlParameter param_basic = new SqlParameter("@BASIC", SqlDbType.Int) { Value = basic_text };
        SqlParameter param_payment = new SqlParameter("@PAYMENT", SqlDbType.Int) { Value = payment_text };
        SqlParameter param_filter = new SqlParameter("@FILTER", SqlDbType.Int) { Value = filter_text };
        SqlParameter param_status = new SqlParameter("@STATUS", SqlDbType.Int) { Value = status_text };
        SqlParameter param_page = new SqlParameter("@PAGE", SqlDbType.Int) { Value = 1 };
        SqlParameter param_rows = new SqlParameter("@ROWS", SqlDbType.Int) { Value = 20 };
        SqlParameter param_excel = new SqlParameter("@EXCEL", SqlDbType.Bit) { Value = 100 };
        SqlParameter param_tipelokasi = new SqlParameter("@TIPELOKASI", SqlDbType.Int) { Value = tipelokasi_text };

        DataSet ds = DbAccess.Ds(query, Db.Cnn_DB, new List<SqlParameter>()
        {
            param_kw, param_start, param_end, param_loccode, param_type, param_item,
            param_category, param_segmen, param_basic, param_payment, param_filter,
            param_status, param_page, param_rows, param_excel, param_tipelokasi
        });

        DataTable data = ds.Tables[0];
        int datacount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
        int index = 1;
        string SO = "";
        if (datacount > 0)
        {
            decimal TotalHargaNet = 0m;
            decimal TotalHargaRupiah = 0m;
            decimal TotalHargaM = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountPercent = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountProgramPercent = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalDiscountPromo = 0m;
            decimal TotalDiscountBank = 0m;
            decimal TotalDiscountOther = 0m;
            decimal TotalExtraRingSize = 0m;
            decimal TotalFeeExtraRingSize = 0m;
            decimal TotalTotalFeeExtraRingSize = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalBayarCash = 0m;
            decimal TotalBayarCreditCard = 0m;
            decimal TotalBayarTransfer = 0m;
            decimal TotalBayarDebit = 0m;
            decimal TotalBayarVoucher = 0m;
            decimal TotalBayarExchange = 0m;
            decimal TotalBayarPoin = 0m;
            decimal TotalBayarDownPayment = 0m;
            decimal TotalBayarBrideStoryPay = 0m;
            decimal TotalBayarGiftCard = 0m;
            decimal TotalBayarLakuEmas = 0m;
            decimal TotalBayarQRIS = 0m;
            decimal TotalBayarXendit = 0m;
            decimal TotalBayarQRISLEI = 0m;
            decimal TotalBayarEcommerce = 0m;
            decimal TotalBayarDOKU = 0m;
            decimal TotalBayarCashA = 0m;
            decimal TotalMDR = 0m;
            decimal TTerima = 0m;
            decimal TTerimaAfterMDR = 0m;
            decimal TotalPrice_ex_Vat = 0m;
            decimal TotalPPN = 0m;

            foreach (DataRow r in data.Rows)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header
                TableRow tr = new TableRow();
                TableCell td;

                var noso = r["NO_SO"].ToString();
                decimal MDR = 0m;
                decimal Terima = 0m;
                decimal dTerimaSetelahMDR = 0m;
                if (SO != noso || SO == noso)
                {
                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDate(Convert.ToDateTime(r["TGL"]));
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["NO_SO"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["INVOICE"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["STORE"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["TIPE"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["LEVEL"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["PLU"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["ITEM"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["PROTO_NOMOR"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["KATEGORI"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["SEGMEN"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["CUSTOMER_NAMA"].ToString();
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["CUSTOMER_CODE"].ToString();
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["GIVENTO"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["MEMBERSHIP"].ToString();
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["SALES_NAMA"].ToString();
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["KETERANGAN"].ToString();
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["TGL_VOID"] == DBNull.Value ? "" : Convert.ToDateTime(r["TGL_VOID"]).ToString("dd-MM-yyyy");
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["KETERANGAN_VOID"].ToString();
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = 1.ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["HARGA_M"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["HARGA_RUPIAH"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_NORMAL_PERCENT"]).ToString("N0") + " %";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_NORMAL_NOMINAL"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_PROMO"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_PERCENT"]).ToString("N0") + " %";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_NOMINAL"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_GIFT"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (string)r["TypeVoucher"];
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (string)r["NamaVoucher"];
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_BANK"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_OTHER"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_ROUND"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["EXTRA_RING_SIZE"]).ToString("N1");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["FEE_EXTRA_RING_SIZE"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TOTAL_FEE_EXTRA_RING_SIZE"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TOTAL_BAYAR"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["PRICE_EXC_VAT"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["PPN"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["PPN_PEMBAGI"]).ToString("N4");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["POINT_RECEIVED"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["KADAR_TUKARAN"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TOTAL_BUTIR"]).ToString("N0");
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TOTAL_CARAT"]).ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["NETTO_WEIGHT"]).ToString("N2");
                    tr.Cells.Add(td);

                    int sod = 1;
                    decimal BayarCash = Convert.ToDecimal(r["CASH"]);
                    decimal BayarCreditCard = Convert.ToDecimal(r["CREDIT_CARD"]);
                    decimal BayarTransfer = Convert.ToDecimal(r["TRANSFER"]);
                    decimal BayarDebit = Convert.ToDecimal(r["DEBIT_CARD"]);
                    decimal BayarVoucher = Convert.ToDecimal(r["VOUCHER"]);
                    decimal BayarExchange = Convert.ToDecimal(r["EXCHANGE"]);
                    decimal BayarPoin = Convert.ToDecimal(r["POIN"]);
                    decimal BayarDownPayment = Convert.ToDecimal(r["DOWN_PAYMENT"]);
                    decimal BayarBrideStoryPay = Convert.ToDecimal(r["BRIDESTORY_PAY"]);
                    decimal BayarGiftCard = Convert.ToDecimal(r["GIFT_CARD"]);
                    decimal BayarLakuEmas = Convert.ToDecimal(r["LAKU_EMAS"]);
                    decimal BayarQRIS = Convert.ToDecimal(r["QRIS"]);
                    decimal BayarXendit = Convert.ToDecimal(r["XENDIT"]);
                    decimal BayarQRISLEI = Convert.ToDecimal(r["QRIS_LEI"]);
                    decimal BayarEcommerce = Convert.ToDecimal(r["E-COMMERCE"]);
                    decimal BayarDOKU = Convert.ToDecimal(r["DOKU"]);
                    decimal BayarCashA = Convert.ToDecimal(r["CASH-A"]);
                    string Bank = r["BANK"].ToString();
                    // string EDC = r["EDC"].ToString();

                    td = new TableCell();
                    td.Text = BayarCash.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarCreditCard.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarTransfer.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarDebit.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarVoucher.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarExchange.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarPoin.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarDownPayment.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarBrideStoryPay.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarGiftCard.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarLakuEmas.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarQRIS.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarXendit.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarQRISLEI.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarEcommerce.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarDOKU.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BayarCashA.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    Terima = BayarCash + BayarCreditCard + BayarTransfer + BayarDebit + BayarVoucher + BayarExchange + BayarPoin + BayarDownPayment + BayarBrideStoryPay + BayarGiftCard + BayarLakuEmas + BayarQRIS + BayarXendit + BayarQRISLEI + BayarEcommerce + BayarDOKU + BayarCashA;
                    MDR = Convert.ToDecimal(r["MDR"]);
                    dTerimaSetelahMDR = Terima - MDR;

                    td = new TableCell();
                    td.Text = Terima.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = MDR.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dTerimaSetelahMDR.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bank;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("white-space", "normal");
                    td.Style.Add("word-wrap", "break-word");
                    td.Style.Add("overflow-wrap", "break-word");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);


                    // td = new TableCell();
                    // td.Text = EDC;
                    // td.Style.Add("text-align", "center");
                    // td.RowSpan = sod;
                    // tr.Cells.Add(td);

                    TotalBayarCash += BayarCash;
                    TotalBayarCreditCard += BayarCreditCard;
                    TotalBayarTransfer += BayarTransfer;
                    TotalBayarDebit += BayarDebit;
                    TotalBayarVoucher += BayarVoucher;
                    TotalBayarExchange += BayarExchange;
                    TotalBayarPoin += BayarPoin;
                    TotalBayarDownPayment += BayarDownPayment;
                    TotalBayarBrideStoryPay += BayarBrideStoryPay;
                    TotalBayarGiftCard += BayarGiftCard;
                    TotalBayarQRIS += BayarQRIS;
                    TotalBayarLakuEmas += BayarLakuEmas;
                    TotalBayarQRISLEI += BayarQRISLEI;
                    TotalBayarXendit += BayarXendit;
                    TotalBayarEcommerce += BayarEcommerce;
                    TotalBayarDOKU += BayarDOKU;
                    TotalBayarCashA += BayarCashA;
                }
                else
                {
                    td = new TableCell();
                    td.Text = "";
                    td.ColumnSpan = 3;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["INVOICE"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["TIPE"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["PLU"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["ITEM"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["PROTO_NOMOR"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["KATEGORI"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["SEGMEN"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    td.ColumnSpan = 7;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = 1.ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["HARGA_M"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["HARGA_RUPIAH"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_NORMAL_PERCENT"]).ToString("N0") + " %";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_NORMAL_NOMINAL"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_PROMO"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_PERCENT"]).ToString("N0") + " %";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_NOMINAL"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_GIFT"]).ToString("N0");
                    tr.Cells.Add(td);


                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_BANK"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_OTHER"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["DISCOUNT_ROUND"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["EXTRA_RING_SIZE"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["FEE_EXTRA_RING_SIZE"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TOTAL_FEE_EXTRA_RING_SIZE"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TOTAL_BAYAR"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["PRICE_EXC_VAT"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["PPN"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["POINT_RECEIVED"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["KADAR_TUKARAN"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TOTAL_BUTIR"]).ToString("N0");
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TOTAL_CARAT"]).ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["NETTO_WEIGHT"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    td.ColumnSpan = 19;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);
                }

                SO = noso;

                tb.Rows.Add(tr);

                TotalHargaNet += Convert.ToDecimal(r["TOTAL_BAYAR"]);
                TotalPrice_ex_Vat += Convert.ToDecimal(r["PRICE_EXC_VAT"]);
                TotalPPN += Convert.ToDecimal(r["PPN"]);
                TotalHargaRupiah += Convert.ToDecimal(r["HARGA_RUPIAH"]);
                TotalHargaM += Convert.ToDecimal(r["HARGA_M"]);
                TotalDiscount += Convert.ToDecimal(r["DISCOUNT_NORMAL_NOMINAL"]);
                TotalDiscountPercent += Convert.ToDecimal(r["DISCOUNT_NORMAL_PERCENT"]);
                TotalDiscountProgram += Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_NOMINAL"]);
                TotalDiscountProgramPercent += Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_PERCENT"]);
                TotalDiscountGift += Convert.ToDecimal(r["DISCOUNT_GIFT"]);
                TotalDiscountRound += Convert.ToDecimal(r["DISCOUNT_ROUND"]);
                TotalDiscountPromo += Convert.ToDecimal(r["DISCOUNT_PROMO"]);
                TotalDiscountBank += Convert.ToDecimal(r["DISCOUNT_BANK"]);
                TotalDiscountOther += Convert.ToDecimal(r["DISCOUNT_OTHER"]);
                TotalExtraRingSize += Convert.ToDecimal(r["EXTRA_RING_SIZE"]);
                TotalFeeExtraRingSize += Convert.ToDecimal(r["FEE_EXTRA_RING_SIZE"]);
                TotalTotalFeeExtraRingSize += Convert.ToDecimal(r["TOTAL_FEE_EXTRA_RING_SIZE"]);
                TotalButir += Convert.ToDecimal(r["TOTAL_BUTIR"]);
                TotalCarat += Convert.ToDecimal(r["TOTAL_CARAT"]);
                TotalNetWeight += Convert.ToDecimal(r["NETTO_WEIGHT"]);
                TTerima += Terima;
                TotalMDR += MDR;
                TTerimaAfterMDR += dTerimaSetelahMDR;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 20;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + data.Rows.Count.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaM).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaRupiah).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountPercent / data.Rows.Count).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountPromo).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgramPercent / data.Rows.Count).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountBank).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountOther).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalExtraRingSize).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalFeeExtraRingSize).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalTotalFeeExtraRingSize).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaNet).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPrice_ex_Vat).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPPN).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 3;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarCash).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarCreditCard).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarTransfer).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarDebit).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarVoucher).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarExchange).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarPoin).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarDownPayment).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarBrideStoryPay).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarGiftCard).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarLakuEmas).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarQRIS).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarXendit).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarQRISLEI).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarEcommerce).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarDOKU).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayarCashA).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TTerima).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalMDR).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TTerimaAfterMDR).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.ColumnSpan = 2;
            td2.Text = "";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
        else
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.Text = "No Data";
            td.ColumnSpan = 60;
            tr.Cells.Add(td);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productsegmen.SelectedValue = Request.QueryString["productsegmen"];
            filter.SelectedValue = Request.QueryString["filter"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.SelectedValue = Request.QueryString["locationtype"];
            locationcode.SelectedValue = Request.QueryString["locationcode"];
            payment.SelectedValue = Request.QueryString["payment"];
            tipeproduct.SelectedValue = Request.QueryString["tipeproduct"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productsegmen=" + productsegmen.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&locationType=" + tipelokasi.Text;
        href += "&locationcode=" + locationcode.SelectedValue;
        href += "&payment=" + payment.SelectedValue;
        href += "&tipeproduct=" + tipeproduct.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string kw_text = kw.Text;
        DateTime start_text = Convert.ToDateTime(dari.Text);
        DateTime end_text = Convert.ToDateTime(sampai.Text);
        string status_text = status.SelectedValue;
        string basic_text = filter.SelectedValue;
        string payment_text = payment.SelectedValue;
        string productitem_text = productitem.SelectedValue;
        string productcategory_text = productcategory.SelectedValue;
        string productsegmen_text = productsegmen.SelectedValue;
        string kodelokasi_text = locationcode.SelectedValue;
        string tipeproduct_text = tipeproduct.SelectedValue;
        string filter_text = filter.SelectedValue;
        string tipelokasi_text = tipelokasi.SelectedValue;

        string query = "sp_report_sales_detail_accounting_new";
        SqlParameter param_kw = new SqlParameter("@KEYWORD", SqlDbType.VarChar) { Value = kw_text };
        SqlParameter param_start = new SqlParameter("@TANGGALSTART", SqlDbType.DateTime) { Value = start_text };
        SqlParameter param_end = new SqlParameter("@TANGGALEND", SqlDbType.DateTime) { Value = end_text };
        SqlParameter param_loccode = new SqlParameter("@LOCCODE", SqlDbType.VarChar) { Value = kodelokasi_text };
        SqlParameter param_type = new SqlParameter("@PRODUCTTYPE", SqlDbType.VarChar) { Value = tipeproduct_text };
        SqlParameter param_item = new SqlParameter("@ITEM", SqlDbType.Int) { Value = productitem_text };
        SqlParameter param_category = new SqlParameter("@CATEGORY", SqlDbType.Int) { Value = productcategory_text };
        SqlParameter param_segmen = new SqlParameter("@SEGMEN", SqlDbType.Int) { Value = productsegmen_text };
        SqlParameter param_basic = new SqlParameter("@BASIC", SqlDbType.Int) { Value = basic_text };
        SqlParameter param_payment = new SqlParameter("@PAYMENT", SqlDbType.Int) { Value = payment_text };
        SqlParameter param_filter = new SqlParameter("@FILTER", SqlDbType.Int) { Value = filter_text };
        SqlParameter param_status = new SqlParameter("@STATUS", SqlDbType.Int) { Value = status_text };
        SqlParameter param_page = new SqlParameter("@PAGE", SqlDbType.Int) { Value = 1 };
        SqlParameter param_rows = new SqlParameter("@ROWS", SqlDbType.Int) { Value = 20 };
        SqlParameter param_excel = new SqlParameter("@EXCEL", SqlDbType.Bit) { Value = 100 };
        SqlParameter param_tipelokasi = new SqlParameter("@TIPELOKASI", SqlDbType.Int) { Value = tipelokasi_text };

        DataSet ds = DbAccess.Ds(query, Db.Cnn_DB, new List<SqlParameter>()
        {
            param_kw, param_start, param_end, param_loccode, param_type, param_item,
            param_category, param_segmen, param_basic, param_payment, param_filter,
            param_status, param_page, param_rows, param_excel, param_tipelokasi
        });

        DataTable data = ds.Tables[0];
        CreateExcel(data);
    }

    protected MemoryStream GetStream(ExcelPackage excelPackage)
    {
        MemoryStream fs = new MemoryStream();
        excelPackage.SaveAs(fs);
        fs.Position = 0;
        return fs;
    }

    protected void CreateExcel(DataTable dt)
    {
        using (var package = new ExcelPackage())
        {
            ExcelWorkbook wb = package.Workbook;
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("SalesRekap");


            sheet.Cells[1, 1].Value = "No";
            sheet.Cells[1, 2].Value = "Tanggal";
            sheet.Cells[1, 3].Value = "No. Sales Order";
            sheet.Cells[1, 4].Value = "No. Referensi";
            sheet.Cells[1, 5].Value = "Lokasi";
            sheet.Cells[1, 6].Value = "Tipe";
            sheet.Cells[1, 7].Value = "Level";
            sheet.Cells[1, 8].Value = "PLU";
            sheet.Cells[1, 9].Value = "Item";
            sheet.Cells[1, 10].Value = "Kode Karet";
            sheet.Cells[1, 11].Value = "Product Category";
            sheet.Cells[1, 12].Value = "Segmen";
            sheet.Cells[1, 13].Value = "Nama Customer";
            sheet.Cells[1, 14].Value = "Kode Customer";
            sheet.Cells[1, 15].Value = "Given To";
            sheet.Cells[1, 16].Value = "Membership";
            sheet.Cells[1, 17].Value = "Sales";
            sheet.Cells[1, 18].Value = "Keterangan";
            sheet.Cells[1, 19].Value = "Tgl Void";
            sheet.Cells[1, 20].Value = "Keterangan Void";
            sheet.Cells[1, 21].Value = "Qty";
            sheet.Cells[1, 22].Value = "M";
            sheet.Cells[1, 23].Value = "Harga";
            sheet.Cells[1, 24].Value = "Discount Normal Percentage";
            sheet.Cells[1, 25].Value = "Discount Normal Nominal";
            sheet.Cells[1, 26].Value = "Discount Promo Nominal";
            sheet.Cells[1, 27].Value = "Discount Membership Percentage";
            sheet.Cells[1, 28].Value = "Discount Membership Nominal";
            sheet.Cells[1, 29].Value = "Discount Voucher Nominal";
            sheet.Cells[1, 30].Value = "Tipe Discount Voucher";
            sheet.Cells[1, 31].Value = "Nomor Discount Voucher";
            sheet.Cells[1, 32].Value = "Discount Bank Nominal";
            sheet.Cells[1, 33].Value = "Discount Other Nominal";
            sheet.Cells[1, 34].Value = "Discount Round Nominal";
            sheet.Cells[1, 35].Value = "Extra Ring Size";
            sheet.Cells[1, 36].Value = "Fee Extra Ring Size Per Size";
            sheet.Cells[1, 37].Value = "Total Fee Extra Ring Size";
            sheet.Cells[1, 38].Value = "Harga Net";
            sheet.Cells[1, 39].Value = "Price Exclude VAT";
            sheet.Cells[1, 40].Value = "PPN";
            sheet.Cells[1, 41].Value = "PPN Pembagi";
            sheet.Cells[1, 42].Value = "Point Received";
            sheet.Cells[1, 43].Value = "Kadar Tukaran Beli";
            sheet.Cells[1, 44].Value = "Total Butir";
            sheet.Cells[1, 45].Value = "Total Carat";
            sheet.Cells[1, 46].Value = "Net Weight";
            sheet.Cells[1, 47].Value = "Cash / Tunai";
            sheet.Cells[1, 48].Value = "Kartu Kredit";
            sheet.Cells[1, 49].Value = "Transfer";
            sheet.Cells[1, 50].Value = "Debit";
            sheet.Cells[1, 51].Value = "Voucher";
            sheet.Cells[1, 52].Value = "Exchange";
            sheet.Cells[1, 53].Value = "Poin";
            sheet.Cells[1, 54].Value = "DP PO";
            sheet.Cells[1, 55].Value = "Bridestory Pay";
            sheet.Cells[1, 56].Value = "Gift Card";
            sheet.Cells[1, 57].Value = "Laku Emas";
            sheet.Cells[1, 58].Value = "QRIS";
            sheet.Cells[1, 59].Value = "Xendit";
            sheet.Cells[1, 60].Value = "QRISLEI";
            sheet.Cells[1, 61].Value = "Ecommerce";
            sheet.Cells[1, 62].Value = "DOKU";
            sheet.Cells[1, 63].Value = "Cash-A";
            sheet.Cells[1, 64].Value = "Total Terima";
            sheet.Cells[1, 65].Value = "MDR";
            sheet.Cells[1, 66].Value = "Total Terima setelah MDR";
            sheet.Cells[1, 67].Value = "Bank";
            // sheet.Cells[1, 63].Value = "EDC";

            int index = 1;
            string SO = "";
            decimal TotalHargaNet = 0m;
            decimal TotalHargaRupiah = 0m;
            decimal TotalHargaM = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountPercent = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountProgramPercent = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalDiscountPromo = 0m;
            decimal TotalDiscountBank = 0m;
            decimal TotalDiscountOther = 0m;
            decimal TotalExtraRingSize = 0m;
            decimal TotalFeeExtraRingSize = 0m;
            decimal TotalTotalFeeExtraRingSize = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalBayarCash = 0m;
            decimal TotalBayarCreditCard = 0m;
            decimal TotalBayarTransfer = 0m;
            decimal TotalBayarDebit = 0m;
            decimal TotalBayarVoucher = 0m;
            decimal TotalBayarExchange = 0m;
            decimal TotalBayarPoin = 0m;
            decimal TotalBayarDownPayment = 0m;
            decimal TotalBayarBrideStoryPay = 0m;
            decimal TotalBayarGiftCard = 0m;
            decimal TotalBayarLakuEmas = 0m;
            decimal TotalBayarQRIS = 0m;
            decimal TotalBayarXendit = 0m;
            decimal TotalBayarQRISLEI = 0m;
            decimal TotalBayarEcommerce = 0m;
            decimal TotalBayarDOKU = 0m;
            decimal TotalBayarCashA = 0m;
            decimal TotalMDR = 0m;
            decimal TTerima = 0m;
            decimal TTerimaAfterMDR = 0m;
            decimal TotalPrice_ex_Vat = 0m;
            decimal TotalPPN = 0m;
            // Column 67 (which is column "BO")

            sheet.Column(30).Style.WrapText = true;
            sheet.Column(30).Width = 200; // set width in Excel units
            sheet.Column(31).Style.WrapText = true;
            sheet.Column(31).Width = 200; // set width in Excel units
            sheet.Column(67).Style.WrapText = true;
            sheet.Column(67).Width = 200; // set width in Excel units



            int TotalPCS = 0;
            foreach (DataRow r in dt.Rows)
            {
                var noso = r["NO_SO"].ToString();
                decimal MDR = 0m;
                decimal Terima = 0m;
                decimal dTerimaSetelahMDR = 0m;
                if (SO != noso || SO == noso)
                {
                    decimal BayarCash = Convert.ToDecimal(r["CASH"]);
                    decimal BayarCreditCard = Convert.ToDecimal(r["CREDIT_CARD"]);
                    decimal BayarTransfer = Convert.ToDecimal(r["TRANSFER"]);
                    decimal BayarDebit = Convert.ToDecimal(r["DEBIT_CARD"]);
                    decimal BayarVoucher = Convert.ToDecimal(r["VOUCHER"]);
                    decimal BayarExchange = Convert.ToDecimal(r["EXCHANGE"]);
                    decimal BayarPoin = Convert.ToDecimal(r["POIN"]);
                    decimal BayarDownPayment = Convert.ToDecimal(r["DOWN_PAYMENT"]);
                    decimal BayarBrideStoryPay = Convert.ToDecimal(r["BRIDESTORY_PAY"]);
                    decimal BayarGiftCard = Convert.ToDecimal(r["GIFT_CARD"]);
                    decimal BayarLakuEmas = Convert.ToDecimal(r["LAKU_EMAS"]);
                    decimal BayarQRIS = Convert.ToDecimal(r["QRIS"]);
                    decimal BayarXendit = Convert.ToDecimal(r["XENDIT"]);
                    decimal BayarQRISLEI = Convert.ToDecimal(r["QRIS_LEI"]);
                    decimal BayarEcommerce = Convert.ToDecimal(r["E-COMMERCE"]);
                    decimal BayarDOKU = Convert.ToDecimal(r["DOKU"]);
                    decimal BayarCashA = Convert.ToDecimal(r["CASH-A"]);
                    string Bank = r["BANK"].ToString();
                    Terima = BayarCash + BayarCreditCard + BayarTransfer + BayarDebit + BayarVoucher + BayarExchange + BayarPoin + BayarDownPayment + BayarBrideStoryPay + BayarGiftCard + BayarLakuEmas + BayarQRIS + BayarXendit + BayarQRISLEI + BayarEcommerce + BayarDOKU + BayarCashA;
                    MDR = Convert.ToDecimal(r["MDR"]);
                    dTerimaSetelahMDR = Terima - MDR;
                    // string EDC = r["EDC"].ToString();

                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 1].Value = index++.ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 2].Value = Str.FullDate(Convert.ToDateTime(r["TGL"]));
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 3].Value = r["NO_SO"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 4].Value = r["INVOICE"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 5].Value = r["STORE"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 6].Value = r["TIPE"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 7].Value = r["LEVEL"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 8].Value = r["PLU"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 9].Value = r["ITEM"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 10].Value = r["PROTO_NOMOR"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 11].Value = r["KATEGORI"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 12].Value = r["SEGMEN"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 13].Value = r["CUSTOMER_NAMA"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 14].Value = r["CUSTOMER_CODE"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 15].Value = r["GIVENTO"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 16].Value = r["MEMBERSHIP"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 17].Value = r["SALES_NAMA"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 18].Value = r["KETERANGAN"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 19].Value = r["TGL_VOID"] == DBNull.Value ? "" : Str.FullDate(Convert.ToDateTime(r["TGL_VOID"]));
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 20].Value = r["KETERANGAN_VOID"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 21].Value = 1.ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 22].Value = Convert.ToDecimal(r["HARGA_M"]).ToString("N2");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 23].Value = Convert.ToDecimal(r["HARGA_RUPIAH"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 24].Value = Convert.ToDecimal(r["DISCOUNT_NORMAL_PERCENT"]).ToString() + "%";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 25].Value = Convert.ToDecimal(r["DISCOUNT_NORMAL_NOMINAL"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 26].Value = Convert.ToDecimal(r["DISCOUNT_PROMO"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 27].Value = Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_PERCENT"]).ToString() + "%";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 28].Value = Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_NOMINAL"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 29].Value = Convert.ToDecimal(r["DISCOUNT_GIFT"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 30].Value = Convert.ToString(r["TypeVoucher"]);
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 31].Value = Convert.ToString(r["NamaVoucher"]);
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 32].Value = Convert.ToDecimal(r["DISCOUNT_BANK"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 33].Value = Convert.ToDecimal(r["DISCOUNT_OTHER"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 34].Value = Convert.ToDecimal(r["DISCOUNT_ROUND"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 35].Value = Convert.ToDecimal(r["EXTRA_RING_SIZE"]).ToString("N3");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 36].Value = Convert.ToDecimal(r["FEE_EXTRA_RING_SIZE"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 37].Value = Convert.ToDecimal(r["TOTAL_FEE_EXTRA_RING_SIZE"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 38].Value = Convert.ToDecimal(r["TOTAL_BAYAR"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 39].Value = Convert.ToDecimal(r["PRICE_EXC_VAT"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 40].Value = Convert.ToDecimal(r["PPN"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 41].Value = Convert.ToDecimal(r["PPN_PEMBAGI"]).ToString("N4");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 42].Value = Convert.ToDecimal(r["POINT_RECEIVED"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 43].Value = Convert.ToDecimal(r["KADAR_TUKARAN"]).ToString("N2");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 44].Value = Convert.ToDecimal(r["TOTAL_BUTIR"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 45].Value = Convert.ToDecimal(r["TOTAL_CARAT"]).ToString("N3");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 46].Value = Convert.ToDecimal(r["NETTO_WEIGHT"]).ToString("N2");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 47].Value = BayarCash.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 48].Value = BayarCreditCard.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 49].Value = BayarTransfer.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 50].Value = BayarDebit.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 51].Value = BayarVoucher.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 52].Value = BayarExchange.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 53].Value = BayarPoin.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 54].Value = BayarDownPayment.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 55].Value = BayarBrideStoryPay.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 56].Value = BayarGiftCard.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 57].Value = BayarLakuEmas.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 58].Value = BayarQRIS.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 59].Value = BayarXendit.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 60].Value = BayarQRISLEI.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 61].Value = BayarEcommerce.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 62].Value = BayarDOKU.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 63].Value = BayarCashA.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 64].Value = TTerima.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 65].Value = MDR.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 66].Value = dTerimaSetelahMDR.ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 67].Value = Bank;
                    // sheet.Cells[dt.Rows.IndexOf(r) + 2, 63].Value = EDC;

                    TotalBayarCash += BayarCash;
                    TotalBayarCreditCard += BayarCreditCard;
                    TotalBayarTransfer += BayarTransfer;
                    TotalBayarDebit += BayarDebit;
                    TotalBayarVoucher += BayarVoucher;
                    TotalBayarExchange += BayarExchange;
                    TotalBayarPoin += BayarPoin;
                    TotalBayarDownPayment += BayarDownPayment;
                    TotalBayarBrideStoryPay += BayarBrideStoryPay;
                    TotalBayarGiftCard += BayarGiftCard;
                    TotalBayarQRIS += BayarQRIS;
                    TotalBayarLakuEmas += BayarLakuEmas;
                    TotalBayarQRISLEI += BayarQRISLEI;
                    TotalBayarXendit += BayarXendit;
                    TotalBayarEcommerce += BayarEcommerce;
                    TotalBayarDOKU += BayarDOKU;
                    TotalBayarCashA += BayarCashA;
                }
                else
                {
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 1].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 2].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 3].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 4].Value = r["INVOICE"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 5].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 6].Value = r["TIPE"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 7].Value = r["LEVEL"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 8].Value = r["PLU"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 9].Value = r["ITEM"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 10].Value = r["PROTO_NOMOR"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 11].Value = r["KATEGORI"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 12].Value = r["SEGMEN"].ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 13].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 14].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 15].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 16].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 17].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 18].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 19].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 20].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 21].Value = 1.ToString();
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 22].Value = Convert.ToDecimal(r["HARGA_M"]).ToString("N2");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 23].Value = Convert.ToDecimal(r["HARGA_RUPIAH"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 24].Value = Convert.ToDecimal(r["DISCOUNT_NORMAL_PERCENT"]).ToString() + "%";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 25].Value = Convert.ToDecimal(r["DISCOUNT_NORMAL_NOMINAL"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 26].Value = Convert.ToDecimal(r["DISCOUNT_PROMO"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 27].Value = Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_PERCENT"]).ToString() + "%";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 28].Value = Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_NOMINAL"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 29].Value = Convert.ToDecimal(r["DISCOUNT_GIFT"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 30].Value = Convert.ToString(r["TypeVoucher"]);
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 31].Value = Convert.ToString(r["NamaVoucher"]);
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 32].Value = Convert.ToDecimal(r["DISCOUNT_BANK"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 33].Value = Convert.ToDecimal(r["DISCOUNT_OTHER"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 34].Value = Convert.ToDecimal(r["DISCOUNT_ROUND"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 35].Value = Convert.ToDecimal(r["EXTRA_RING_SIZE"]).ToString("N3");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 36].Value = Convert.ToDecimal(r["FEE_EXTRA_RING_SIZE"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 37].Value = Convert.ToDecimal(r["TOTAL_FEE_EXTRA_RING_SIZE"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 38].Value = Convert.ToDecimal(r["TOTAL_BAYAR"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 39].Value = Convert.ToDecimal(r["PRICE_EXC_VAT"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 40].Value = Convert.ToDecimal(r["PPN"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 41].Value = Convert.ToDecimal(r["PPN_PEMBAGI"]).ToString("N4");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 42].Value = Convert.ToDecimal(r["POINT_RECEIVED"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 43].Value = Convert.ToDecimal(r["KADAR_TUKARAN"]).ToString("N2");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 44].Value = Convert.ToDecimal(r["TOTAL_BUTIR"]).ToString("F0");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 45].Value = Convert.ToDecimal(r["TOTAL_CARAT"]).ToString("N3");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 46].Value = Convert.ToDecimal(r["NETTO_WEIGHT"]).ToString("N2");
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 47].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 48].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 49].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 50].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 51].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 52].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 53].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 54].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 55].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 56].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 57].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 58].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 59].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 60].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 61].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 62].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 63].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 64].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 65].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 66].Value = "";
                    sheet.Cells[dt.Rows.IndexOf(r) + 2, 67].Value = "";
                }

                SO = noso;

                TotalHargaNet += Convert.ToDecimal(r["TOTAL_BAYAR"]);
                TotalPrice_ex_Vat += Convert.ToDecimal(r["PRICE_EXC_VAT"]);
                TotalPPN += Convert.ToDecimal(r["PPN"]);
                TotalHargaRupiah += Convert.ToDecimal(r["HARGA_RUPIAH"]);
                TotalHargaM += Convert.ToDecimal(r["HARGA_M"]);
                TotalDiscount += Convert.ToDecimal(r["DISCOUNT_NORMAL_NOMINAL"]);
                TotalDiscountPercent += Convert.ToDecimal(r["DISCOUNT_NORMAL_PERCENT"]);
                TotalDiscountProgram += Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_NOMINAL"]);
                TotalDiscountProgramPercent += Convert.ToDecimal(r["DISCOUNT_MEMBERSHIP_PERCENT"]);
                TotalDiscountGift += Convert.ToDecimal(r["DISCOUNT_GIFT"]);
                TotalDiscountRound += Convert.ToDecimal(r["DISCOUNT_ROUND"]);
                TotalDiscountPromo += Convert.ToDecimal(r["DISCOUNT_PROMO"]);
                TotalDiscountBank += Convert.ToDecimal(r["DISCOUNT_BANK"]);
                TotalDiscountOther += Convert.ToDecimal(r["DISCOUNT_OTHER"]);
                TotalExtraRingSize += Convert.ToDecimal(r["EXTRA_RING_SIZE"]);
                TotalFeeExtraRingSize += Convert.ToDecimal(r["FEE_EXTRA_RING_SIZE"]);
                TotalTotalFeeExtraRingSize += Convert.ToDecimal(r["TOTAL_FEE_EXTRA_RING_SIZE"]);
                TotalButir += Convert.ToDecimal(r["TOTAL_BUTIR"]);
                TotalCarat += Convert.ToDecimal(r["TOTAL_CARAT"]);
                TotalNetWeight += Convert.ToDecimal(r["NETTO_WEIGHT"]);
                TTerima += Terima;
                TotalMDR += MDR;
                TTerimaAfterMDR += dTerimaSetelahMDR;
            }

            //create a range for the table
            ExcelRange range = sheet.Cells[1, 1, dt.Rows.Count + 2, 63];

            sheet.Cells[dt.Rows.Count + 2, 1].Value = "No";
            sheet.Cells[dt.Rows.Count + 2, 2].Value = "Tanggal";
            sheet.Cells[dt.Rows.Count + 2, 3].Value = "No. Sales Order";
            sheet.Cells[dt.Rows.Count + 2, 4].Value = "No. Referensi";
            sheet.Cells[dt.Rows.Count + 2, 5].Value = "Lokasi";
            sheet.Cells[dt.Rows.Count + 2, 6].Value = "Tipe";
            sheet.Cells[dt.Rows.Count + 2, 7].Value = "Level";
            sheet.Cells[dt.Rows.Count + 2, 8].Value = "PLU";
            sheet.Cells[dt.Rows.Count + 2, 9].Value = "Item";
            sheet.Cells[dt.Rows.Count + 2, 10].Value = "Kode Karet";
            sheet.Cells[dt.Rows.Count + 2, 11].Value = "Product Category";
            sheet.Cells[dt.Rows.Count + 2, 12].Value = "Segmen";
            sheet.Cells[dt.Rows.Count + 2, 13].Value = "Nama Customer";
            sheet.Cells[dt.Rows.Count + 2, 14].Value = "Kode Customer";
            sheet.Cells[dt.Rows.Count + 2, 15].Value = "Given To";
            sheet.Cells[dt.Rows.Count + 2, 16].Value = "Membership";
            sheet.Cells[dt.Rows.Count + 2, 17].Value = "Sales";
            sheet.Cells[dt.Rows.Count + 2, 18].Value = "Keterangan";
            sheet.Cells[dt.Rows.Count + 2, 19].Value = "Tgl Void";
            sheet.Cells[dt.Rows.Count + 2, 20].Value = "Keterangan Void";

            sheet.Cells[dt.Rows.Count + 2, 1, dt.Rows.Count + 2, 20].Merge = true;
            sheet.Cells[dt.Rows.Count + 2, 1, dt.Rows.Count + 2, 20].Value = "Total";

            sheet.Cells[dt.Rows.Count + 2, 21].Value = dt.Rows.Count;
            sheet.Cells[dt.Rows.Count + 2, 22].Value = TotalHargaM.ToString("N2");
            sheet.Cells[dt.Rows.Count + 2, 23].Value = TotalHargaRupiah.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 24].Value = Convert.ToDecimal(TotalDiscountPercent / dt.Rows.Count).ToString("N0") + "%";
            sheet.Cells[dt.Rows.Count + 2, 25].Value = TotalDiscount.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 26].Value = TotalDiscountPromo.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 27].Value = Convert.ToDecimal(TotalDiscountProgramPercent / dt.Rows.Count).ToString("N0") + "%";
            sheet.Cells[dt.Rows.Count + 2, 28].Value = TotalDiscountProgram.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 29].Value = TotalDiscountGift.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 30].Value = "";
            sheet.Cells[dt.Rows.Count + 2, 31].Value = "";
            sheet.Cells[dt.Rows.Count + 2, 32].Value = TotalDiscountBank.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 33].Value = TotalDiscountOther.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 34].Value = TotalDiscountRound.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 35].Value = TotalExtraRingSize.ToString("N3");
            sheet.Cells[dt.Rows.Count + 2, 36].Value = TotalFeeExtraRingSize.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 37].Value = TotalTotalFeeExtraRingSize.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 38].Value = TotalHargaNet.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 39].Value = TotalPrice_ex_Vat.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 40].Value = TotalPPN.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 41].Value = "";
            sheet.Cells[dt.Rows.Count + 2, 42].Value = "";
            sheet.Cells[dt.Rows.Count + 2, 43].Value = "";
            sheet.Cells[dt.Rows.Count + 2, 44].Value = TotalButir.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 45].Value = TotalCarat.ToString("N3");
            sheet.Cells[dt.Rows.Count + 2, 46].Value = TotalNetWeight.ToString("N2");
            sheet.Cells[dt.Rows.Count + 2, 47].Value = TotalBayarCash.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 48].Value = TotalBayarCreditCard.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 49].Value = TotalBayarTransfer.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 50].Value = TotalBayarDebit.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 51].Value = TotalBayarVoucher.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 52].Value = TotalBayarExchange.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 53].Value = TotalBayarPoin.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 54].Value = TotalBayarDownPayment.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 55].Value = TotalBayarBrideStoryPay.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 56].Value = TotalBayarGiftCard.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 57].Value = TotalBayarLakuEmas.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 58].Value = TotalBayarQRIS.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 59].Value = TotalBayarXendit.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 60].Value = TotalBayarQRISLEI.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 61].Value = TotalBayarEcommerce.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 62].Value = TotalBayarDOKU.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 63].Value = TotalBayarCashA.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 64].Value = TTerima.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 65].Value = TotalMDR.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 66].Value = TTerimaAfterMDR.ToString("F0");
            sheet.Cells[dt.Rows.Count + 2, 67].Value = "";

            //add a table to the range
            ExcelTable tab = sheet.Tables.Add(range, "Table1");

            //format the table
            tab.TableStyle = TableStyles.Medium13;

            sheet.Cells.AutoFitColumns();

            MemoryStream stream = GetStream(package);// The method is defined below
            DateTime now = DateTime.Now;
            string tgl = now.Day.ToString().PadLeft(2, '0');
            string bulan = now.Month.ToString().PadLeft(2, '0');
            string tahun = now.Year.ToString().PadLeft(4, '0');
            string jam = now.Hour.ToString().PadLeft(2, '0');
            string menit = now.Minute.ToString().PadLeft(2, '0');
            string detik = now.Second.ToString().PadLeft(2, '0');

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
            "attachment; filename=SalesRekap_" + tgl + bulan + tahun + "_" + jam + menit + detik + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
    

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {            
            Ddl.Store(locationcode,Tipe);
        }
        else
        {
            Ddl.Store(locationcode);
        }        
    }

}