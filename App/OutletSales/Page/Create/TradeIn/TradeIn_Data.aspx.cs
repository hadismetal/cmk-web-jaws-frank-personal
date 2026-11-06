using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Page_Register_Sales_Item : System.Web.UI.Page
{
    private Map_DB _db;
    private Map_CS _cs;

    // lazy properties — .NET 4 compatible
    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    protected int myTIdayslimit { get { return 30; } }

    protected int myID { get { return App.QueryInt(this); } }
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }
    protected string myConfirm { get { return Request.QueryString["confirm"]; } }

    protected string hrefmain { get { return "TradeIn.aspx"; } }
    protected string hrefmain2 { get { return "TradeIn_Data.aspx"; } }
    protected string hrefmainfull { get { return "TradeIn/TradeIn.aspx"; } }
    protected string hrefpattern { get { return "?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO; } }
    protected string hrefpattern2 { get { return "&id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO; } }

    protected string sDiscNormal_N { get { return "dn1"; } }
    protected string sDiscNormal_P { get { return "dn2"; } }

    protected string sDiscPromo_N { get { return "dp1"; } }
    protected string sDiscPromo_P { get { return "dp2"; } }

    protected string sDiscBank_N { get { return "db1"; } }
    protected string sDiscBank_P { get { return "db2"; } }

    protected string sDiscMember_N { get { return "dm1"; } }
    protected string sDiscMember_P { get { return "dm2"; } }

    protected string sDiscVoucher_N { get { return "dv1"; } }
    protected string sDiscVoucher_P { get { return "dv2"; } }

    protected string sDiscOther_N { get { return "do1"; } }
    protected string sDiscOther_P { get { return "do2"; } }

    protected string sDiscRound_N { get { return "dr1"; } }
    protected string sDiscRound_P { get { return "dr2"; } }

    protected string sExtraRingSize_N { get { return "rs"; } }
    protected string sFeeExtraRingSize_N { get { return "frs"; } }
    protected string sTotalFeeExtraRingSize_N { get { return "tfrs"; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        string xPromoMembership = "";

        // hardcode promo nasional 2019 Februari : double reward membership redemption
        if (System.DateTime.Now >= Convert.ToDateTime("2019-02-01 00:00:00.000")
            && System.DateTime.Now < Convert.ToDateTime("2019-04-01 00:00:00.000")
            )
        {
            xPromoMembership = "Double Reward Promo Nasional (1 Feb 2019 - 28 Feb 2019)";
        }

        lit_promo_membership.Text = (xPromoMembership != "") ? "<span data-toggle='tooltip' title='' data-placement='bottom' class='glyphicon glyphicon-info-sign' style='font-size:13px; color:green; font-weight:normal;' data-original-title='" + xPromoMembership + "'></span>" : "";

        div_unfinished.Visible = false;
        div_finished.Visible = false;
        redirect_payment.Visible = false;
        redirect_register.Visible = false;
        div_btn_keterangan.Visible = true;

        var x = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO);
        if (x != null)
        {
            if (x.Draft)
            {
                div_unfinished.Visible = true;
                div_finished.Visible = false;

                var t = db.TradeIn.SingleOrDefault(p => p.ID == myID && p.Draft);
                var s = db.Resell.SingleOrDefault(p => p.ID == myIDRO && p.Draft);

                if (!IsPostBack)
                {
                    //HyperLink
                    addpck.HRef += hrefpattern;
                    addsv.HRef += hrefpattern;
                    addrs.HRef += hrefpattern; // tradein

                    if (myConfirm == "1")
                    {
                        div_review.Visible = false;
                        div_confirm.Visible = true;
                        update_keterangan.Visible = false;
                        div_btn_keterangan.Visible = false;

                        addpck.Disabled = true;
                        addsv.Disabled = true;
                        keterangan.Enabled = false;
                        txt_plu.Enabled = false;
                        btn_add_plu.Enabled = false;

                    }

                    //Form
                    //fillForm();

                    //Generate
                    keterangan.Text = x.Keterangan;


                    //Trans. Form
                    string Membership = "Non-Member";

                    if (STAMPS.xActivate)
                    {
                        var c = x.DataCustomer;
                        Membership = c.NoMember; // tiering membership update from STAMPS
						try
						{
							Db.Execute("EXEC sp_get_membership_customer 'UPDATE', '" + x.ID.ToString() + "' ",Db.Cnn_DB);
						}
						catch{}
                    }
                    else
                    {
                        var c = cs.Mscustomer.SingleOrDefault(p => p.M_kode == x.DataCustomer.NoCustomer);
                        if (c != null)
                        {
                            var m = cs.Msmembership.SingleOrDefault(p => p.M_kode == c.M_typemember);

                            if (m != null) { Membership = m.M_nama + " (" + m.M_kode + ")"; }
                        }
                    }

                    string Lokasi = "";
                    string BrandCompany = "";
                    if (x.TipeLokasi == 1)
                    {
                        var loc = db.LocOutlet.SingleOrDefault(p => p.ID == x.IDLokasi);
                        Lokasi = loc.Nama;
                        BrandCompany = loc.CompanyBrand.Nama;
                    }
                    else if (x.TipeLokasi == 2)
                    {
                        var loc = db.LocExhibition.SingleOrDefault(p => p.ID == x.IDLokasi);
                        Lokasi = loc.Nama;
                        BrandCompany = loc.CompanyBrand.Nama;
                    }

                    brand.InnerHtml = BrandCompany;
                    membership.InnerHtml = Membership;
                    location.InnerHtml = Lokasi;
                    description.InnerHtml = Str.HtmlBreak(x.Keterangan);
                    jpjr.InnerHtml = x.DataSales.Nama;
                    customer.InnerHtml = x.DataCustomer.Nama + " (" + x.DataCustomer.NoCustomer + ")";
                    operator_input.InnerHtml = x.Operator + " (" + Str.TglJam(x.DraftDate) + ")";
                    tgl.InnerHtml = Str.Tgl(x.Tgl);
                    lit_day_ti.Text = myTIdayslimit.ToString();
                    txtReceiver.InnerHtml = (String.IsNullOrEmpty(x.CustomerReferenceNama) ? "" : x.CustomerReferenceNama);
                }

                fillTable(sender, e, x, t, s);
            }
            else
            {
                div_unfinished.Visible = false;
                div_finished.Visible = true;
                redirect_payment.Visible = true;
                redirect_register.Visible = false;

                string xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                                + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                                + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                                + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Anda tidak dapat mengakses halaman ini. Dokumen sudah terbentuk dengan nomor Sales Order # "
                                + "<b>" + x.Nomor + "</b>"
                                + "</div>"
                                + "</div></div></div>"
                                ;

                lit_info_invalid.Text = xText;
            }

        }
        else
        {
            div_unfinished.Visible = false;
            div_finished.Visible = true;
            redirect_payment.Visible = false;
            redirect_register.Visible = true;

            string xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                            + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                            + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                            + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Dokumen tidak ditemukan !!!</b> Silahkan kembali ke menu Register Sales."
                            + "</div>"
                            + "</div></div></div>"
                            ;

            lit_info_invalid.Text = xText;
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

    //Exception
    protected void DataError() { Response.Redirect(hrefmain); }

    //Display Table
    protected void fillTable(object sender, EventArgs e, SalesOrder x, TradeIn t, Resell s)
    {
        int QTY = 0;
        int QTY_RO = 0;
        decimal GT = 0m;
        decimal GT2 = 0m;

        decimal GTRO = 0m; // tradein

        var c = cs.Mscustomer.SingleOrDefault(p => p.M_kode == x.DataCustomer.NoCustomer);
        var m = cs.Msmembership.SingleOrDefault(p => p.M_kode == c.M_typemember);

        decimal disc1 = 0m;
        decimal disc2 = 0m;

        if (STAMPS.xActivate)
        {
            List<decimal> Discounts = fillMembershipStamps(x, ref disc1, ref disc2);
            disc1 = Discounts[0]; disc2 = Discounts[1];
        }
        else
        {
            fillMembership(c, m);
        }

        //Item
        fillItemDJ(myIDSO, ref QTY, ref GT, ref GT2, m, disc1, disc2);
        fillItemPG(myIDSO, ref QTY, ref GT, ref GT2, m, disc1, disc2);
        fillItemGJ(myIDSO, ref QTY, ref GT, ref GT2, m, disc1, disc2);
        fillItemLD(myIDSO, ref QTY, ref GT, ref GT2, m, disc1, disc2);
        fillPackaging(x);
        fillSouvenir(x);
        fillSouvenirGJ();
        fillSouvenirDJ();
        fillSouvenirLD();
        fillSouvenirPG();

        bool isVisible = (myConfirm == "1") ? false : true;

        fillItemDJRO(myIDRO, ref QTY_RO, ref GTRO, isVisible);
        fillItemPGRO(myIDRO, ref QTY_RO, ref GTRO, isVisible);
        fillItemGJRO(myIDRO, ref QTY_RO, ref GTRO, isVisible);
        fillItemLDRO(myIDRO, ref QTY_RO, ref GTRO, isVisible);

        //Grand-Total Sales Order

        //Grand-Total
        if (QTY == 0)
        {
            App.TableNoResult(tb);
        }
        else
        {
            Sub_Total(GT, GT2, true);
        }


        //Grand-Total Resell Order
        if (QTY_RO == 0) { App.TableNoResult(tbti); }
        else
        {
            Sub_TotalRO(GTRO, true); // tradein
        }

        tb.Focus();
        calc();
        checkValid(x, s);

    }

    protected void checkValid(SalesOrder x, Resell s)
    {
        bool disableReviewButton = false;
        next_err_notradein.Visible = false;
        next_err_tradein.Visible = false;
        next_err_qc.Visible = false;
        next_err_nosales.Visible = false;
        addrs.Disabled = true;

        bool xvalid_nt = false;
        bool xvalid_ro = false;
        bool xvalid_gt = false;
        bool xwhite_gt = false;
        bool xvalid_so = false;
        bool xvalid_nominal_so = false;
        bool xvalid_nominal_tradein = false;
        bool xvalid_diff_sales_tradein = false;

        int idso = x.ID;
        int idro = s.ID;

        xwhite_gt = checkWhitelistPLU(idro);

        xvalid_nt = checkNoTradeIn(idro);
        xvalid_ro = checkRO(s); //tradein
        //xvalid_gt = checkGrandTotal(x, s);
        xvalid_gt = (xwhite_gt ? xwhite_gt : checkGrandTotal30(x, s));
        xvalid_so = checkItemSales(idso);
        xvalid_nominal_so = checkNilaiSales(idso);
        xvalid_nominal_tradein = checkNilaiTradeIn(idro);
        xvalid_diff_sales_tradein = checkDiffSalesTradeIn(idso, idro);

        if (!(xvalid_ro && xvalid_gt && xvalid_nt && xvalid_so && xvalid_nominal_so && xvalid_nominal_tradein && xvalid_diff_sales_tradein)) { disableReviewButton = true; div_err.Visible = true; }

        if (!xvalid_nt) { next_err_notradein.Visible = true; addrs.Disabled = false; }
        if (!xvalid_ro) { next_err_qc.Visible = true; }
        if (!xvalid_gt) { next_err_tradein.Visible = true; }
        if (!xvalid_so) { next_err_nosales.Visible = true; }
        if (!xvalid_nominal_so) { next_err_nilaisales.Visible = true; }
        if (!xvalid_nominal_tradein) { next_err_nilaitradein.Visible = true; }
        if (!xvalid_diff_sales_tradein) { next_err_diff_tradein_sales.Visible = true; }

        //untuk trade in lebih dari satu
        //addrs.Disabled = false;

        if (disableReviewButton) 
        { 
            review.Attributes["class"] = "btn btn-warning disabled";
            review.Visible = false;
        }
        else
        { 
            review.Attributes["class"] = "btn btn-warning";
            review.Visible = true;
        }

    }

    protected bool checkWhitelistPLU(int idro)
    {
        bool status = false;
        string query = "DECLARE @ISANY BIT = 0 " +
            "IF EXISTS(" +
            "SELECT 1 FROM StockProductWhitelist WITH (NOLOCK) " +
            "WHERE IDProduct IN (SELECT IDProduct FROM ResellDJ A WITH (NOLOCK) " +
            "WHERE IDForm = @ID) AND TipeProduct = 'DJ' AND Approval IS NOT NULL AND ApprovalTgl IS NOT NULL) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM StockProductWhitelist WITH (NOLOCK) " +
            "WHERE IDProduct IN (SELECT IDProduct FROM ResellPG A WITH (NOLOCK) " +
            "WHERE IDForm = @ID) AND TipeProduct = 'PG' AND Approval IS NOT NULL AND ApprovalTgl IS NOT NULL) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM StockProductWhitelist WITH (NOLOCK) " +
            "WHERE IDProduct IN (SELECT IDProduct FROM ResellGJ A WITH (NOLOCK) " +
            "WHERE IDForm = @ID) AND TipeProduct = 'GJ' AND Approval IS NOT NULL AND ApprovalTgl IS NOT NULL) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM StockProductWhitelist WITH (NOLOCK) " +
            "WHERE IDProduct IN (SELECT IDProduct FROM ResellLD A WITH (NOLOCK) " +
            "WHERE IDForm = @ID) AND TipeProduct = 'LD'AND Approval IS NOT NULL AND ApprovalTgl IS NOT NULL) " +
            "BEGIN SET @ISANY = 1 END " +
            "SELECT @ISANY [ANY]";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = idro }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        status = Convert.ToBoolean(dt.Rows[0]["ANY"]);
        return status;
    }
    protected bool checkNoTradeIn(int idro)
    {
        bool status = false;

        string query = "DECLARE @ISANY BIT = 0 " +
            "IF EXISTS(SELECT 1 FROM ResellDJ WHERE IDForm = @ID) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM ResellPG WHERE IDForm = @ID) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM ResellGJ WHERE IDForm = @ID) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM ResellLD WHERE IDForm = @ID) " +
            "BEGIN SET @ISANY = 1 END " +
            "SELECT @ISANY [ANY]";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = idro }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        status = Convert.ToBoolean(dt.Rows[0]["ANY"]);
        return status;
    }
    protected bool checkGrandTotal(SalesOrder d, Resell s)
    {
        bool status = false;

        decimal TotalSales = 0m;
        decimal TotalResell = 0m;

        TotalResell = s.ResellDJ.Sum(p => p.HargaRupiah.Value) + s.ResellPG.Sum(p => p.HargaRupiah.Value) + s.ResellGJ.Sum(p => p.HargaRupiah.Value) + s.ResellLD.Sum(p => p.HargaRupiah.Value);
        TotalSales = d.SalesOrderDJ.Sum(p => p.TotalBayar.Value) + d.SalesOrderPG.Sum(p => p.TotalBayar.Value) + d.SalesOrderGJ.Sum(p => p.TotalBayar.Value) + d.SalesOrderLD.Sum(p => p.TotalBayar.Value);

        if (TotalResell < TotalSales)
        {
            status = true;
        }

        return status;
    }

    protected bool checkItemSales(int idso)
    {
        bool status = false;
        string query = "DECLARE @ISANY BIT = 0 " +
            "IF EXISTS(SELECT 1 FROM SalesOrderDJ WHERE IDForm = @ID) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM SalesOrderPG WHERE IDForm = @ID) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM SalesOrderGJ WHERE IDForm = @ID) " +
            "BEGIN SET @ISANY = 1 END " +
            "IF @ISANY = 0 AND EXISTS(SELECT 1 FROM SalesOrderLD WHERE IDForm = @ID) " +
            "BEGIN SET @ISANY = 1 END " +
            "SELECT @ISANY [ANY]";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = idso}
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        status = Convert.ToBoolean(dt.Rows[0]["ANY"]);
        return status;
    }

    protected bool checkNilaiSales(int idso)
    {
        bool status = false;
        decimal totalsales = 0m;
        string query = "DECLARE @SUMTOTAL MONEY = 0 " +
            "IF EXISTS(SELECT 1 FROM SalesOrderDJ WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(TotalBayar,0)) FROM SalesOrderDJ WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM SalesOrderPG WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(TotalBayar,0)) FROM SalesOrderPG WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM SalesOrderGJ WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(TotalBayar,0)) FROM SalesOrderGJ WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM SalesOrderLD WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(TotalBayar,0)) FROM SalesOrderLD WHERE IDForm = @ID END " +
            "SELECT @SUMTOTAL[SUM]";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = idso}
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        totalsales = Convert.ToDecimal(dt.Rows[0]["SUM"]);
        if (totalsales >= 0m)
            status = true;
        return status;
    }

    protected bool checkNilaiTradeIn(int idro)
    {
        bool status = false;
        decimal totalsales = 0m;
        string query = "DECLARE @SUMTOTAL MONEY = 0 " +
            "IF EXISTS(SELECT 1 FROM ResellDJ WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(HargaRupiah,0)) FROM ResellDJ WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM ResellPG WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(HargaRupiah,0)) FROM ResellPG WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM ResellGJ WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(HargaRupiah,0)) FROM ResellGJ WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM ResellLD WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(HargaRupiah,0)) FROM ResellLD WHERE IDForm = @ID END " +
            "SELECT @SUMTOTAL[SUM]";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = idro}
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        totalsales = Convert.ToDecimal(dt.Rows[0]["SUM"]);
        if (totalsales >= 0m)
            status = true;
        return status;
    }

    protected bool checkDiffSalesTradeIn(int idso, int idro)
    {
        bool status = false;
        decimal totalsales = 0m;
        decimal totaltradein = 0m;

        string query = "DECLARE @SUMTOTALSO MONEY = 0 DECLARE @SUMTOTALRO MONEY = 0 " +

            "IF EXISTS(SELECT 1 FROM SalesOrderDJ WHERE IDForm = @IDSO) " +
            "BEGIN SELECT @SUMTOTALSO = @SUMTOTALSO + SUM(ISNULL(TotalBayar,0)) FROM SalesOrderDJ WHERE IDForm = @IDSO END " +
            "IF EXISTS(SELECT 1 FROM SalesOrderPG WHERE IDForm = @IDSO) " +
            "BEGIN SELECT @SUMTOTALSO = @SUMTOTALSO + SUM(ISNULL(TotalBayar,0)) FROM SalesOrderPG WHERE IDForm = @IDSO END " +
            "IF EXISTS(SELECT 1 FROM SalesOrderGJ WHERE IDForm = @IDSO) " +
            "BEGIN SELECT @SUMTOTALSO = @SUMTOTALSO + SUM(ISNULL(TotalBayar,0)) FROM SalesOrderGJ WHERE IDForm = @IDSO END " +
            "IF EXISTS(SELECT 1 FROM SalesOrderLD WHERE IDForm = @IDSO) " +
            "BEGIN SELECT @SUMTOTALSO = @SUMTOTALSO + SUM(ISNULL(TotalBayar,0)) FROM SalesOrderLD WHERE IDForm = @IDSO END " +

            "IF EXISTS(SELECT 1 FROM ResellDJ WHERE IDForm = @IDRO) " +
            "BEGIN SELECT @SUMTOTALRO = @SUMTOTALRO + SUM(ISNULL(HargaRupiah,0)) FROM ResellDJ WHERE IDForm = @IDRO END " +
            "IF EXISTS(SELECT 1 FROM ResellPG WHERE IDForm = @IDRO) " +
            "BEGIN SELECT @SUMTOTALRO = @SUMTOTALRO + SUM(ISNULL(HargaRupiah,0)) FROM ResellPG WHERE IDForm = @IDRO END " +
            "IF EXISTS(SELECT 1 FROM ResellGJ WHERE IDForm = @IDRO) " +
            "BEGIN SELECT @SUMTOTALRO = @SUMTOTALRO + SUM(ISNULL(HargaRupiah,0)) FROM ResellGJ WHERE IDForm = @IDRO END " +
            "IF EXISTS(SELECT 1 FROM ResellLD WHERE IDForm = @IDRO) " +
            "BEGIN SELECT @SUMTOTALRO = @SUMTOTALRO + SUM(ISNULL(HargaRupiah,0)) FROM ResellLD WHERE IDForm = @IDRO END " +
            "SELECT @SUMTOTALSO [SUMSO], @SUMTOTALRO [SUMRO]";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@IDSO", SqlDbType.Int){ Value = idso },
            new SqlParameter("@IDRO", SqlDbType.Int){ Value = idro }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        totalsales = Convert.ToDecimal(dt.Rows[0]["SUMSO"]);
        totaltradein = Convert.ToDecimal(dt.Rows[0]["SUMRO"]);

        if (totalsales >= totaltradein)
            status = true;

        return status;
    }

    protected bool checkGrandTotal30(SalesOrder d, Resell s)
    {
        bool status = false;

        decimal TotalSales = 0m;
        decimal TotalResell = 0m;

        TotalResell = s.ResellDJ.Sum(p => p.HargaRupiah.Value) + s.ResellPG.Sum(p => p.HargaRupiah.Value) + s.ResellGJ.Sum(p => p.HargaRupiah.Value) + s.ResellLD.Sum(p => p.HargaRupiah.Value);
        TotalSales = d.SalesOrderDJ.Sum(p => p.TotalBayar.Value) + d.SalesOrderPG.Sum(p => p.TotalBayar.Value) + d.SalesOrderGJ.Sum(p => p.TotalBayar.Value) + d.SalesOrderLD.Sum(p => p.TotalBayar.Value);

        // jika sudah lewat dari tanggal merchandise exchange maka diberlakukan barang baru harus lebih mahal 30% dari brg tradein
        var sdj = s.ResellDJ.FirstOrDefault();
        var spg = s.ResellPG.FirstOrDefault();
        var sgj = s.ResellGJ.FirstOrDefault();
        var sld = s.ResellLD.FirstOrDefault();

        DateTime xtime;
        bool xflag = false;
        int days_limit = myTIdayslimit;

        if (sdj != null)
        {
            if (sdj.SalesOrder.TglLunas.HasValue)
            {
                xtime = sdj.SalesOrder.TglLunas.Value;
                if (xtime.AddDays(days_limit).AddHours(24) < System.DateTime.Now) { xflag = true; }

                //lit_test_text.Text += xtime.AddDays(days_limit).AddHours(24).ToString() + "<br />";
                //lit_test_text.Text += System.DateTime.Now.ToString();
            }
        }
        else if (spg != null)
        {
            if (spg.SalesOrder.TglLunas.HasValue)
            {
                xtime = spg.SalesOrder.TglLunas.Value;
                if (xtime.AddDays(days_limit).AddHours(24) < System.DateTime.Now) { xflag = true; }
            }
        }
        else if (sgj != null)
        {
            if (sgj.SalesOrder.TglLunas.HasValue)
            {
                xtime = sgj.SalesOrder.TglLunas.Value;
                if (xtime.AddDays(days_limit).AddHours(24) < System.DateTime.Now) { xflag = true; }
            }
        }
        else if (sld != null)
        {
            if (sld.SalesOrder.TglLunas.HasValue)
            {
                xtime = sld.SalesOrder.TglLunas.Value;
                if (xtime.AddDays(days_limit).AddHours(24) < System.DateTime.Now) { xflag = true; }
            }
        }

        if (xflag)
        {
            if (TotalSales < (TotalResell + (30 * TotalResell / 100)))
            {
                status = false;
            }
            else
            {
                status = true;
            }
        }
        else
        {
            status = true;
        }

        return status;
    }

    protected bool checkRO(Resell s)
    {
        bool status = false;

        if (s == null) DataError();
        else
        {
            bool check = false;
            var dj = from p in s.ResellDJ where p.IDDocQC == null select p;
            if (dj.Count() > 0) { check = true; }
            var pg = from p in s.ResellPG where p.IDDocQC == null select p;
            if (pg.Count() > 0) { check = true; }
            var gj = from p in s.ResellGJ where p.IDDocQC == null select p;
            if (gj.Count() > 0) { check = true; }
            var ld = from p in s.ResellLD where p.IDDocQC == null select p;
            if (ld.Count() > 0) { check = true; }

            status = !check;
        }

        return status;
    }


    protected List<decimal> fillMembershipStamps(SalesOrder d, ref decimal disc1, ref decimal disc2)
    {
        string query = "SP_STAMPS_GET_MASTER_MEMBERSHIP_TIERING";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@tiering", SqlDbType.VarChar){ Value = d.DataCustomer.NoMember }
        };
        DataTable dt = Rs(query, Db.Cnn_IN, parameters, CommandType.StoredProcedure);
        
        string wlabel = "140";
        string sTitle = "";
        string sContent = "";

        sTitle = (d.DataCustomer.NoMember != null) ? d.DataCustomer.NoMember : "";
        sContent = "<div>";

        if (dt.Rows.Count > 0 && sTitle != "")
        {
            foreach (DataRow dtRow in dt.Rows)
            {
                decimal dDisc1 = Convert.ToDecimal(dtRow["ST_MEMBERSHIP_DISC1"].ToString());
                decimal dDisc2 = Convert.ToDecimal(dtRow["ST_MEMBERSHIP_DISC2"].ToString());

                string xTier = dtRow["ST_MEMBERSHIP_STATUS"].ToString();
                string xDisc1 = Str.Number1(dDisc1);
                string xDisc2 = Str.Number1(dDisc2);

                if (dtRow["ST_MEMBERSHIP_STATUS"].ToString() == sTitle)
                {
                    disc1 = dDisc1;
                    disc2 = dDisc2;

                }

                sContent += "<div style='float:left; width:" + wlabel + "px; " + (xTier == sTitle ? "color:blue;" : "") + "'>" + xTier + "</div>"
                      + "<div style='float:left; " + (xTier == sTitle ? "color:blue;" : "") + "'>&nbsp;=&nbsp;</div>"
                      + "<div style='float:left; " + (xTier == sTitle ? "color:blue;" : "") + "'>" + xDisc1 + " | " + xDisc2 + "</div>";
            }

        }
        sContent += "<div style='clear:both;'></div></div>";

        if (sTitle != "")
        {
            int font_size = 10;
            font_size = (sTitle.Length > 10) ? 8 : 10;
            lit_membership.Text = "<div style='width:100%;'><button OnClick='return false;' type='button' class='alert alert-info' style='width:100%; height:25px; font-size:" + font_size + "px; padding:0; margin:0; text-align:center;'"
                                + "role='button' data-html='true' data-toggle='popover' data-placement='bottom' data-trigger='focus' title=\"<b>" + sTitle + "</b>\" data-content=\"" + sContent + "\""
                                + "> "
                                + sTitle
                                + "</a></div>"
                                ;
        }
        else
        {
            lit_membership.Text = "<div type='button' class='alert alert-warning' style='font-size:10px; padding:0; margin:0; text-align:center;'> "
                                + "NON-MEMBER"
                                + "</div>"
                                ;
        }
        List<decimal> Discounts = new List<decimal>();
        Discounts.Add(disc1); Discounts.Add(disc2);
        return Discounts;
    }


    protected void fillMembership(Mscustomer customer, Msmembership member)
    {
        var m = from p in cs.Msmembership select p;

        string wlabel = "140";
        string sTitle = "";
        string sContent = "";

        sTitle = (member != null) ? member.M_nama : "";
        sContent = "<div>";
        if (m.Count() > 0 && member != null)
        {
            foreach (var data in m)
            {
                sContent += "<div style='float:left; width:" + wlabel + "px; " + (member.M_nama == data.M_nama ? "color:blue;" : "") + "'>" + data.M_nama + "</div>"
                          + "<div style='float:left; " + (member.M_nama == data.M_nama ? "color:blue;" : "") + "'>&nbsp;=&nbsp;</div>"
                          + "<div style='float:left; " + (member.M_nama == data.M_nama ? "color:blue;" : "") + "'>" + data.M_extradisc + " / " + data.M_extradisc2 + "</div>";

            }

        }
        sContent += "<div style='clear:both;'></div></div>";

        if (sTitle != "")
        {
            int font_size = 10;
            string sTglMemberSince = (Str.Tgl(customer.M_tgltier).Trim() != "") ? (" (" + Str.Tgl(customer.M_tgltier) + ")") : "";
            font_size = (sTitle.Length > 10) ? 8 : 10;
            lit_membership.Text = "<div style='width:100%;'><button OnClick='return false;' type='button' class='alert alert-info' style='width:100%; height:25px; font-size:" + font_size + "px; padding:0; margin:0; text-align:center;'"
                                + "role='button' data-html='true' data-toggle='popover' data-placement='bottom' data-trigger='focus' title=\"<b>" + customer.M_member + "</b>" + sTglMemberSince + "\" data-content=\"" + sContent + "\""
                                + "> "
                                + sTitle
                                + "</a></div>"
                                ;
        }
        else
        {
            lit_membership.Text = "<div type='button' class='alert alert-warning' style='font-size:10px; padding:0; margin:0; text-align:center;'> "
                                + "NON-MEMBER"
                                + "</div>"
                                ;
        }


    }

    //Item
    protected void fillItemDJ(int SO_ID, ref int QTY, ref decimal GT, ref decimal GT2, Msmembership m, decimal disc1, decimal disc2)
    {
        var rs = from p in db.SalesOrderDJ
                 where p.IDForm == SO_ID
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;
        decimal ST2 = 0m;

        if (iQty > 0)
        {
            var vcr = from p in db.SalesVoucher
                      where p.IDSalesOrder == SO_ID && p.ProductType == 1 && p.ProductID != null
                      select p;

            Sub_Header(0);
            foreach (var r in rs)
            {
                int qtyVoucher = 0;
                string descVoucher = "";
                if (vcr.Count() > 0)
                {
                    var vcrdtl = from p in vcr
                                 where p.ProductID == r.IDProduct
                                 select p
                                 ;
                    qtyVoucher = vcrdtl.Count();
                    if (qtyVoucher > 0)
                    {
                        descVoucher += "<table>";
                        foreach (var d in vcrdtl)
                        {
                            descVoucher += "<tr>";
                            descVoucher += "<td>" + d.NomorVoucher + "</td>";
                            descVoucher += "<td>" + Str.Number0(d.Nominal) + "</td>";
                            descVoucher += "<td>" + d.TypeVoucher + "</td>";
                            descVoucher += "</tr>";
                        }
                        descVoucher += "</table>";
                    }

                }

                string tipeproduk = "dj";
                bool isPromo = false;
                string statusPromo = "";
                string tipePromo = "";
                string descPromo = "";
                string statusSouvenir = "";
                string tipeSouvenir = "";
                string descSouvenir = "";
                decimal DiscountPersen = 0m;
                decimal DiscountPromoNominal = 0m;
                decimal DiscountMembership = 0m;
                decimal HargaPLU = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound);
                decimal DiscountProgramBank = 0m;
                decimal DiscountLainLain = 0m;
                decimal FeeExtraRingSize = 0m;

                bool isBranded = Pricing.isDiamondBranded(r.StockProductDJ.Nomor);
                bool isGeneric = Pricing.isDiamondGeneric(r.StockProductDJ.Nomor);
				if(isBranded == false)
					isBranded = isFancyCertifiedDJ(r.StockProductDJ.ID);
                //bool isRegular = Pricing.isDiamondRegular(r.StockProductDJ.Nomor);
                bool isNett = Pricing.isNett(r.StockProductDJ.Nomor);

                //lit_test_text.Text += (r.StockProductDJ.Nomor + " - " + ((isNett) ? "True" : "False") + "<br />"); // test

                bool statusNoPromoNormal = false;
                bool statusNoPromoMembership = false;
                string extra_ring_size = "0";
                int TipePromo = 0;
                decimal HargaPromo = 0m;

                // promo dj
                var t = from p in db.PromoDJ_Detail
                        where p.PromoDJ.ApprovalNama != null && p.PromoDJ.TglStart.Value <= DateTime.Today
                        && p.PromoDJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == r.IDProduct
                        && p.PromoDJ.Draft == false
                        select p;
                if (t.Count() > 0)
                {
                    var disc = t.First();
                    var discP = disc.PromoDJ;
                    isPromo = true;
                    statusNoPromoNormal = (discP.StatusDiscNormal != null) ? discP.StatusDiscNormal.Value : false;
                    statusNoPromoMembership = (discP.StatusDiscMembership != null) ? discP.StatusDiscMembership.Value : false;

                    extra_ring_size = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(ExtraRingSize,0) AS INT) [ExtraRingSize] FROM PromoDJ WHERE ID = " + disc.IDForm, Db.Cnn_DB).ToString();

                    DateTime myDate1 = Convert.ToDateTime(discP.TglExpire);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate1 - myDate2;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
                    else if (days == 0) { result = "Today is the last day!"; }
                    else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

                    string wlabel = "100";
                    tipePromo = DL.PromoStatus(discP.Tipe.Value);
                    descPromo = "<div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Nama Promo</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + discP.Nama + "</div>"
                              + ((discP.PromoHarga.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Promo 1 Harga</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.PromoHarga.Value) + "</div>" : "")
                              + ((discP.PoinReward.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Promo Poin</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.PoinReward.Value) + "</div>" : "")
                              + ((discP.DiscountNominal.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Discount Nominal</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.DiscountNominal.Value) + "</div>" : "")
                              + ((discP.DiscountPersen.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Discount Persen</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.DiscountPersen.Value) + "</div>" : "")
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Start</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(discP.TglStart) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Expire</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(discP.TglExpire) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>&nbsp;</div>" + "<div style='float:left;'>&nbsp;</div>" + "<div style='float:left;'>( " + result + " )</div>"
                              + "<div style='clear:both;'></div>"
                              + ((discP.StatusASP.Value == 1) ? "<div style='color:red;'>" + "No Exchange, No Resell" + "</div>" : "")
                              + ((discP.StatusDiscNormal != null) ? ((discP.StatusDiscNormal.Value) ? "<div style='color:red;'>" + "No Discount Normal" + "</div>" : "") : "")
                              + ((discP.StatusDiscMembership != null) ? ((discP.StatusDiscMembership.Value) ? "<div style='color:red;'>" + "No Discount Membership" + "</div>" : "") : "")
                              + "</div>"
                              ;

                    statusPromo = "<button OnClick='return false;' class='alert alert-success' style='padding:1px 3px 0 5px; margin:0 0 1px 0;'"
                                + "role='button' data-html='true' data-toggle='popover' data-trigger='focus' title=\"" + tipePromo + "\" data-content=\"" + descPromo + "\""
                                + "> "
                                + "PROMO"
                                + "</button>"
                                ;

                    if (discP.Tipe == 1)
                    {
                        // promo persen
                        DiscountPersen = discP.DiscountPersen.Value;
                        DiscountPromoNominal = discP.DiscountNominal.Value;
                        TipePromo = 1;
                    }
                    else if (discP.Tipe == 3)
                    {
                        // promo 1 harga
                        DiscountPromoNominal = HargaPLU - discP.PromoHarga.Value;
                        HargaPromo = discP.PromoHarga.Value;
                        TipePromo = 3;
                    }
                }

                // souvenir 
                var sv = from p in db.Bundle_ItemPasangan
                         where p.BundleItem.TglExpire > DateTime.Today && p.IDProduct.Value == r.IDProduct
                         && p.BundleItem.StatusActive == 1 && p.TipeProduct == 1
                         select p;

                if (sv.Count() > 0)
                {
                    var souv2 = sv.First();
                    var souv = souv2.BundleItem;

                    DateTime myDate1 = Convert.ToDateTime(souv.TglExpire);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate1 - myDate2;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
                    else if (days == 0) { result = "Today is the last day!"; }
                    else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

                    string wlabel = "100";
                    tipeSouvenir = "SOUVENIR";
                    descSouvenir = "<div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Nama Souvenir</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + souv.Nama + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Start</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(souv.TglStart) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Expire</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(souv.TglExpire) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>&nbsp;</div>" + "<div style='float:left;'>&nbsp;</div>" + "<div style='float:left;'>( " + result + " )</div>"
                              + "<div style='clear:both;'></div>"
                              + "</div>"
                              ;
                    statusSouvenir = "<button OnClick='return false;' class='alert alert-warning' style='padding:1px 3px 0 5px; margin:0 0 1px 0;'"
                                + "role='button' data-html='true' data-toggle='popover' data-trigger='focus' title=\"" + tipeSouvenir + "\" data-content=\"" + descSouvenir + "\""
                                + "> "
                                + "SOUVENIR"
                                + "</button>"
                                ;

                }

                TableRow tr = new TableRow();
                TableCell td;

                HiddenField hf = new HiddenField();
                hf.ID = tipeproduk + "hf1_" + index;
                hf.Value = r.ID.ToString();

                Literal lt = new Literal();
                lt.Text = TD_HyperLink(0, r.ID, r.IDProduct);

                td = new TableCell();
                td.Controls.Add(hf);
                td.Controls.Add(lt);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                string xPLU = r.StockProductDJ.Nomor;
                string xItem = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
				td.Text = TD_PLU(0, xPLU, xItem, statusPromo, statusSouvenir);
				td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + Convert.ToDecimal(HargaPLU).ToString("N0") + "</p>";
                tr.Cells.Add(td);

                // disc normal
                decimal disc_nd_p = 0m;
                decimal disc_nd_n = 0m;

                if (tipeproduk == "gj" || (tipeproduk == "dj" && r.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SETTING"))
                {
                    disc_nd_p = 0m;
                    disc_nd_n = 0m;
                }
                else if (tipeproduk != "pg" && tipeproduk != "ld")
                {
                    disc_nd_p = statusNoPromoNormal ? 0m : ((r.Discount.Value != null) ? r.Discount.Value : 0m);
                    disc_nd_n = statusNoPromoNormal ? 0m : ((r.DiscountNominal.Value != null) ? r.DiscountNominal.Value : 0m);

                    if (!statusNoPromoNormal && !isNett)
                    {
                        disc_nd_p = 10m;
                        if (TipePromo == 3)
                        {
                            disc_nd_n = Math.Floor(((disc_nd_p / 100) * Convert.ToDecimal(HargaPromo)) / 1000) * 1000;
                        }
                        else
                        {
                            disc_nd_n = Math.Floor(((disc_nd_p / 100) * Convert.ToDecimal(HargaPLU)) / 1000) * 1000;
                        }
                    }
                }

                r.Discount = disc_nd_p;
                r.DiscountNominal = disc_nd_n;
                db.SubmitChanges();

                string val_disc_normal_n = disc_nd_n.ToString("N0");
                string val_disc_normal_p = disc_nd_p.ToString("N0");
                td = Td_Disc_Normal(tipeproduk, index, r.ID, isPromo, val_disc_normal_n, val_disc_normal_p, statusNoPromoNormal, isNett);
                tr.Cells.Add(td);

                // disc promo
                DiscountPromoNominal = r.DiscountPromo.Value;

                string val_disc_promo_n = DiscountPromoNominal.ToString("N0");
                string val_disc_promo_p = DiscountPersen.ToString("N1");
                td = Td_Disc_Promo(tipeproduk, index, r.ID, isPromo, val_disc_promo_n, val_disc_promo_p);
                tr.Cells.Add(td);

                // disc membership
                decimal disc_ms_p = 0m;
                decimal disc_ms_n = 0m;

                // berdasarkan meeting AE, pg dan gj tidak ada diskon membership
                if (tipeproduk == "pg" || tipeproduk == "gj" || (tipeproduk == "dj" && r.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SETTING") /*|| (tipeproduk == "dj" && r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.NamaKode == "M" && r.StockProductDJ.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode == "PT")*/ || (tipeproduk == "dj" && r.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.NamaKode == "MTG" && !r.StockProductDJ.StockProductDJ_Stone1B.Any() && r.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode != "SNI"))
                { statusNoPromoMembership = true; }

                disc_ms_p = (r.DiscountProgram.Value != null) ? r.DiscountProgram.Value : 0m;
                disc_ms_n = (r.DiscountProgramNominal.Value != null) ? r.DiscountProgramNominal.Value : 0m;

                if (statusNoPromoMembership)
                {
                    disc_ms_p = 0m;
                    disc_ms_n = 0m;
                }
                else
                {

                    try
                    {
                        if (STAMPS.xActivate)
                        {
                            if (r.DiscountNominal == null) { r.DiscountNominal = 0; }
                            if (r.DiscountPromo == null) { r.DiscountPromo = 0; }
                            DiscountMembership = /*(isBranded) ? disc2 : disc1;*/(isBranded) ? disc2 : ((isGeneric) ? 0m : disc1);
                            disc_ms_p = DiscountMembership;
                            disc_ms_n = Math.Floor(((Convert.ToDecimal(HargaPLU)
                                        - disc_nd_n
                                        - DiscountPromoNominal
                                        )
                                        * DiscountMembership / 100)
                                        );
                        }
                        else if (m != null)
                        {
                            if (r.DiscountNominal == null) { r.DiscountNominal = 0; }
                            if (r.DiscountPromo == null) { r.DiscountPromo = 0; }
                            db.SubmitChanges();

                            DiscountMembership = (isBranded) ? m.M_extradisc2.Value : m.M_extradisc.Value; // diskon membership normal / branded diamond
                            DiscountMembership = (isGeneric) ? 0 : DiscountMembership;
                            disc_ms_p = DiscountMembership;
                            disc_ms_n = Math.Floor(((Convert.ToDecimal(HargaPLU)
                                        - disc_nd_n
                                        - DiscountPromoNominal
                                        )
                                        * DiscountMembership / 100)
                                        );
                        }
                    }
                    catch (Exception)
                    {

                    }

                }

                r.DiscountProgram = disc_ms_p;
                r.DiscountProgramNominal = disc_ms_n;
                db.SubmitChanges();

                string val_disc_member_n = disc_ms_n.ToString("N0");
                string val_disc_member_p = disc_ms_p.ToString("N1");
                td = Td_Disc_Member(tipeproduk, index, r.ID, isPromo, val_disc_member_n, val_disc_member_p, m, statusNoPromoMembership);
                tr.Cells.Add(td);

                // disc voucher
                decimal disc_vc_p = 0m;
                decimal disc_vc_n = 0m;
                disc_vc_p = (r.DiscountGift.Value != null) ? r.DiscountGift.Value : 0m;
                disc_vc_n = (r.DiscountGift.Value != null) ? r.DiscountGift.Value : 0m;

                string val_disc_voucher_n = disc_vc_n.ToString("N0");
                string val_disc_voucher_p = disc_vc_n.ToString("N0");
                td = Td_Disc_Voucher(tipeproduk, index, r.ID, isPromo, val_disc_voucher_n, val_disc_voucher_p, qtyVoucher, descVoucher);
                tr.Cells.Add(td);

                // disc program bank
                DiscountProgramBank = r.DiscountBank;

                string val_disc_bank_n = DiscountProgramBank.ToString("N0");
                string val_disc_bank_p = DiscountProgramBank.ToString("N1");
                td = Td_Disc_Bank(tipeproduk, index, r.ID, isPromo, val_disc_bank_n, val_disc_bank_p, HargaPLU - disc_nd_n - DiscountPromoNominal - disc_ms_n - disc_vc_n);
                tr.Cells.Add(td);

                // disc lain-lain
                DiscountLainLain = r.DiscountOther;

                string val_disc_other_n = DiscountLainLain.ToString("N0");
                string val_disc_other_p = DiscountLainLain.ToString("N1");
                td = Td_Disc_Other(tipeproduk, index, r.ID, isPromo, val_disc_other_n, val_disc_other_p);
                tr.Cells.Add(td);

                // disc pembulatan (round)
                decimal disc_rn_p = 0m;
                decimal disc_rn_n = 0m;
                decimal totalHargaPLU_wo_floor = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain);

                if (r.DiscountRound == null) { r.DiscountRound = 0m; db.SubmitChanges(); }

                if (r.DiscountRound == 0m) { }
                else if (r.DiscountRound > 0m)
                {
                    if (r.DiscountRound < 1000 && (totalHargaPLU_wo_floor % 1000 != 0))
                    {
                        disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                        disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p);

                        r.DiscountRound = disc_rn_n;
                        db.SubmitChanges();
                    }
                    else if (r.DiscountRound > 1000m)
                    {
                        if ((totalHargaPLU_wo_floor - r.DiscountRound) % 1000 != 0)
                        {
                            decimal sthousand = Math.Floor((r.DiscountRound.Value) / 1000) * 1000;
                            disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                            disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p) + sthousand;

                            r.DiscountRound = disc_rn_n;
                            db.SubmitChanges();
                        }
                        else
                        {
                            disc_rn_n = r.DiscountRound.Value;
                        }
                    }
                }



                string val_disc_round_n = disc_rn_n.ToString("N0");
                string val_disc_round_p = disc_rn_n.ToString("N0");
                td = Td_Disc_Round(tipeproduk, index, r.ID, isPromo, val_disc_round_n, val_disc_round_p);
                tr.Cells.Add(td);

                //fee extra ring size
                string fee_extra_ring_size = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(Harga,0) AS INT) [Harga] FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0", Db.Cnn_DB).ToString();
                td = Td_Fee_Extra_Ring_Size(tipeproduk, index, r.ID, isPromo, extra_ring_size, fee_extra_ring_size);
                tr.Cells.Add(td);

                FeeExtraRingSize = Convert.ToInt32(extra_ring_size) * Convert.ToInt32(fee_extra_ring_size);

                // total bayar
                decimal totalHargaPLU = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain - disc_rn_n + FeeExtraRingSize);
                r.TotalBayar = totalHargaPLU;
                db.SubmitChanges();

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + totalHargaPLU.ToString("N0") + "</p>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                ST += Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound);
                ST2 += Convert.ToDecimal(r.TotalBayar);

                index++;

            }
            QTY += iQty;
            GT += ST;
            GT2 += ST2;
            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemPG(int SO_ID, ref int QTY, ref decimal GT, ref decimal GT2, Msmembership m, decimal disc1, decimal disc2)
    {
        var rs = from p in db.SalesOrderPG
                 where p.IDForm == SO_ID
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;
        decimal ST2 = 0m;

        if (iQty > 0)
        {
            var vcr = from p in db.SalesVoucher
                      where p.IDSalesOrder == SO_ID && p.ProductType == 2 && p.ProductID != null
                      select p;

            Sub_Header(1);
            foreach (var r in rs)
            {
                int qtyVoucher = 0;
                string descVoucher = "";
                if (vcr.Count() > 0)
                {
                    var vcrdtl = from p in vcr
                                 where p.ProductID == r.IDProduct
                                 select p
                                 ;
                    qtyVoucher = vcrdtl.Count();
                    if (qtyVoucher > 0)
                    {
                        descVoucher += "<table>";
                        foreach (var d in vcrdtl)
                        {
                            descVoucher += "<tr>";
                            descVoucher += "<td>" + d.NomorVoucher + "</td>";
                            descVoucher += "<td>" + Str.Number0(d.Nominal) + "</td>";
                            descVoucher += "<td>" + d.TypeVoucher + "</td>";
                            descVoucher += "</tr>";
                        }
                        descVoucher += "</table>";
                    }

                }

                decimal GoldPrice = r.TotalRupiah.Value;

                string tipeproduk = "pg";
                bool isPromo = false;
                string statusPromo = "";
                string descPromo = "";
                string tipePromo = "";
                string statusSouvenir = "";
                string tipeSouvenir = "";
                string descSouvenir = "";
                decimal DiscountPersen = 0m;
                decimal DiscountPromoNominal = 0m;
                decimal DiscountMembership = 0m;
                decimal HargaPLU = GoldPrice;
                decimal DiscountProgramBank = 0m;
                decimal DiscountLainLain = 0m;
                decimal FeeExtraRingSize = 0m;

                bool isBranded = false; // Pricing.isDiamondBranded(r.StockProductDJ.Nomor);
                bool isNett = Pricing.isNett(r.StockProductPG.Nomor);

                bool statusNoPromoNormal = false;
                bool statusNoPromoMembership = false;
                string extra_ring_size = "0";
                int TipePromo = 0;
                decimal HargaPromo = 0m;

                // promo pg
                var t = from p in db.PromoPG_Detail
                        where p.PromoPG.ApprovalNama != null && p.PromoPG.TglStart.Value <= DateTime.Today
                        && p.PromoPG.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == r.IDProduct
                        select p;
                if (t.Count() > 0)
                {
                    var disc = t.First();
                    var discP = disc.PromoPG;
                    isPromo = true;
                    statusNoPromoNormal = (discP.StatusDiscNormal != null) ? discP.StatusDiscNormal.Value : false;
                    statusNoPromoMembership = (discP.StatusDiscMembership != null) ? discP.StatusDiscMembership.Value : false;
                    extra_ring_size = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(ExtraRingSize,0) AS INT) [ExtraRingSize] FROM PromoPG WHERE ID = " + disc.IDForm, Db.Cnn_DB).ToString();

                    DateTime myDate1 = Convert.ToDateTime(discP.TglExpire);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate1 - myDate2;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
                    else if (days == 0) { result = "Today is the last day!"; }
                    else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

                    string wlabel = "100";
                    tipePromo = DL.PromoStatus(discP.Tipe.Value);
                    descPromo = "<div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Nama Promo</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + discP.Nama + "</div>"
                              + ((discP.PromoHarga.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Promo 1 Harga</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.PromoHarga.Value) + "</div>" : "")
                              + ((discP.PoinReward.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Promo Poin</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.PoinReward.Value) + "</div>" : "")
                              + ((discP.DiscountNominal.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Discount Nominal</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.DiscountNominal.Value) + "</div>" : "")
                              + ((discP.DiscountPersen.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Discount Persen</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.DiscountPersen.Value) + "</div>" : "")
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Start</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(discP.TglStart) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Expire</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(discP.TglExpire) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>&nbsp;</div>" + "<div style='float:left;'>&nbsp;</div>" + "<div style='float:left;'>( " + result + " )</div>"
                              + "<div style='clear:both;'></div>"
                              + ((discP.StatusASP.Value == 1) ? "<div style='color:red;'>" + "No Exchange, No Resell" + "</div>" : "")
                              + ((discP.StatusDiscNormal != null) ? ((discP.StatusDiscNormal.Value) ? "<div style='color:red;'>" + "No Discount Normal" + "</div>" : "") : "")
                              + ((discP.StatusDiscMembership != null) ? ((discP.StatusDiscMembership.Value) ? "<div style='color:red;'>" + "No Discount Membership" + "</div>" : "") : "")
                              + "</div>"
                              ;

                    statusPromo = "<button OnClick='return false;' class='alert alert-success' style='padding:1px 3px 0 5px; margin:0 0 1px 0;'"
                                + "role='button' data-html='true' data-toggle='popover' data-trigger='focus' title=\"" + tipePromo + "\" data-content=\"" + descPromo + "\""
                                + "> "
                                + "PROMO"
                                + "</button>"
                                ;

                    if (discP.Tipe == 1)
                    {
                        // promo persen
                        DiscountPersen = discP.DiscountPersen.Value;
                        DiscountPromoNominal = discP.DiscountNominal.Value;
                        TipePromo = 1;
                    }
                    else if (discP.Tipe == 3)
                    {
                        // promo 1 harga
                        DiscountPromoNominal = HargaPLU - discP.PromoHarga.Value;
                        HargaPromo = discP.PromoHarga.Value;
                        TipePromo = 3;
                    }
                }

                // souvenir 
                var sv = from p in db.Bundle_ItemPasangan
                         where p.BundleItem.TglExpire > DateTime.Today && p.IDProduct.Value == r.IDProduct
                         && p.BundleItem.StatusActive == 1 && p.TipeProduct == 3
                         select p;

                if (sv.Count() > 0)
                {
                    var souv2 = sv.First();
                    var souv = souv2.BundleItem;

                    DateTime myDate1 = Convert.ToDateTime(souv.TglExpire);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate1 - myDate2;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
                    else if (days == 0) { result = "Today is the last day!"; }
                    else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

                    string wlabel = "100";
                    tipeSouvenir = "SOUVENIR";
                    descSouvenir = "<div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Nama Souvenir</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + souv.Nama + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Start</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(souv.TglStart) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Expire</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(souv.TglExpire) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>&nbsp;</div>" + "<div style='float:left;'>&nbsp;</div>" + "<div style='float:left;'>( " + result + " )</div>"
                              + "<div style='clear:both;'></div>"
                              + "</div>"
                              ;
                    statusSouvenir = "<button OnClick='return false;' class='alert alert-warning' style='padding:1px 3px 0 5px; margin:0 0 1px 0;'"
                                + "role='button' data-html='true' data-toggle='popover' data-trigger='focus' title=\"" + tipeSouvenir + "\" data-content=\"" + descSouvenir + "\""
                                + "> "
                                + "SOUVENIR"
                                + "</button>"
                                ;

                }


                TableRow tr = new TableRow();
                TableCell td;

                HiddenField hf = new HiddenField();
                hf.ID = tipeproduk + "hf1_" + index;
                hf.Value = r.ID.ToString();

                Literal lt = new Literal();
                lt.Text = TD_HyperLink(1, r.ID, r.IDProduct);

                td = new TableCell();
                td.Controls.Add(hf);
                td.Controls.Add(lt);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                string xPLU = r.StockProductPG.Nomor;
                string xItem = r.StockProductPG.CharProductItem.Nama;
                td.Text = TD_PLU(0, xPLU, xItem, statusPromo, statusSouvenir);
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + Convert.ToDecimal(GoldPrice).ToString("N0") + "</p>";
                tr.Cells.Add(td);

                // disc normal
                decimal disc_nd_p = 0m;
                decimal disc_nd_n = 0m;

                if (tipeproduk == "gj")
                {
                    disc_nd_p = 0m;
                    disc_nd_n = 0m;
                }
                else if (tipeproduk != "pg" && tipeproduk != "ld")
                {
                    disc_nd_p = statusNoPromoNormal ? 0m : (r.Discount.Value != null) ? r.Discount.Value : 0m;
                    disc_nd_n = statusNoPromoNormal ? 0m : (r.DiscountNominal.Value != null) ? r.DiscountNominal.Value : 0m;

                    if (!statusNoPromoNormal && !isNett)
                    {
                        disc_nd_p = 10m;
                        disc_nd_n = Math.Floor(((disc_nd_p / 100) * Convert.ToDecimal(GoldPrice)) / 1000) * 1000;
                    }
                }

                r.Discount = disc_nd_p;
                r.DiscountNominal = disc_nd_n;
                db.SubmitChanges();

                string val_disc_normal_n = disc_nd_n.ToString("N0");
                string val_disc_normal_p = disc_nd_p.ToString("N0");
                td = Td_Disc_Normal(tipeproduk, index, r.ID, isPromo, val_disc_normal_n, val_disc_normal_p, statusNoPromoNormal, isNett);
                tr.Cells.Add(td);

                // disc promo
                DiscountPromoNominal = r.DiscountPromo.Value;

                string val_disc_promo_n = DiscountPromoNominal.ToString("N0");
                string val_disc_promo_p = DiscountPersen.ToString("N1");
                td = Td_Disc_Promo(tipeproduk, index, r.ID, isPromo, val_disc_promo_n, val_disc_promo_p);
                tr.Cells.Add(td);

                // disc membership
                decimal disc_ms_p = 0m;
                decimal disc_ms_n = 0m;

                // berdasarkan meeting AE, pg dan gj tidak ada diskon membership
                if (tipeproduk == "pg" || tipeproduk == "gj") { statusNoPromoMembership = true; }

                disc_ms_p = (r.DiscountProgram.Value != null) ? r.DiscountProgram.Value : 0m;
                disc_ms_n = (r.DiscountProgramNominal.Value != null) ? r.DiscountProgramNominal.Value : 0m;

                if (statusNoPromoMembership)
                {
                    disc_ms_p = 0m;
                    disc_ms_n = 0m;
                }
                else
                {

                    try
                    {
                        if (STAMPS.xActivate)
                        {
                            if (r.DiscountNominal == null) { r.DiscountNominal = 0; }
                            if (r.DiscountPromo == null) { r.DiscountPromo = 0; }
                            DiscountMembership = (isBranded) ? disc2 : disc1;
                            disc_ms_p = DiscountMembership;
                            disc_ms_n = Math.Floor(((Convert.ToDecimal(HargaPLU)
                                        - disc_nd_n
                                        - DiscountPromoNominal
                                        )
                                        * DiscountMembership / 100)
                                        );
                        }
                        else if (m != null)
                        {
                            if (r.DiscountNominal == null) { r.DiscountNominal = 0; }
                            if (r.DiscountPromo == null) { r.DiscountPromo = 0; }
                            db.SubmitChanges();

                            DiscountMembership = (isBranded) ? m.M_extradisc2.Value : m.M_extradisc.Value; // diskon membership normal / branded diamond
                            disc_ms_p = DiscountMembership;
                            disc_ms_n = Math.Floor(((Convert.ToDecimal(HargaPLU)
                                        - disc_nd_n
                                        - DiscountPromoNominal
                                        )
                                        * DiscountMembership / 100)
                                        );
                        }
                    }
                    catch (Exception)
                    {

                    }


                }
                r.DiscountProgram = disc_ms_p;
                r.DiscountProgramNominal = disc_ms_n;
                db.SubmitChanges();

                string val_disc_member_n = disc_ms_n.ToString("N0");
                string val_disc_member_p = disc_ms_p.ToString("N1");
                td = Td_Disc_Member(tipeproduk, index, r.ID, isPromo, val_disc_member_n, val_disc_member_p, m, statusNoPromoMembership);
                tr.Cells.Add(td);

                // disc voucher
                decimal disc_vc_p = 0m;
                decimal disc_vc_n = 0m;
                disc_vc_p = (r.DiscountGift.Value != null) ? r.DiscountGift.Value : 0m;
                disc_vc_n = (r.DiscountGift.Value != null) ? r.DiscountGift.Value : 0m;

                string val_disc_voucher_n = disc_vc_n.ToString("N0");
                string val_disc_voucher_p = disc_vc_n.ToString("N0");
                td = Td_Disc_Voucher(tipeproduk, index, r.ID, isPromo, val_disc_voucher_n, val_disc_voucher_p, qtyVoucher, descVoucher);
                tr.Cells.Add(td);

                // disc program bank
                DiscountProgramBank = r.DiscountBank;

                string val_disc_bank_n = DiscountProgramBank.ToString("N0");
                string val_disc_bank_p = DiscountProgramBank.ToString("N1");
                td = Td_Disc_Bank(tipeproduk, index, r.ID, isPromo, val_disc_bank_n, val_disc_bank_p, HargaPLU - disc_nd_n - DiscountPromoNominal - disc_ms_n - disc_vc_n);
                tr.Cells.Add(td);

                // disc lain-lain
                DiscountLainLain = r.DiscountOther;

                string val_disc_other_n = DiscountLainLain.ToString("N0");
                string val_disc_other_p = DiscountLainLain.ToString("N1");
                td = Td_Disc_Other(tipeproduk, index, r.ID, isPromo, val_disc_other_n, val_disc_other_p);
                tr.Cells.Add(td);

                // disc pembulatan (round)
                decimal disc_rn_p = 0m;
                decimal disc_rn_n = 0m;
                decimal totalHargaPLU_wo_floor = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain);

                if (r.DiscountRound == null) { r.DiscountRound = 0m; db.SubmitChanges(); }

                if (r.DiscountRound == 0m) { }
                else if (r.DiscountRound > 0m)
                {
                    if (r.DiscountRound < 1000 && (totalHargaPLU_wo_floor % 1000 != 0))
                    {
                        disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                        disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p);

                        r.DiscountRound = disc_rn_n;
                        db.SubmitChanges();
                    }
                    else if (r.DiscountRound > 1000m)
                    {
                        if ((totalHargaPLU_wo_floor - r.DiscountRound) % 1000 != 0)
                        {
                            decimal sthousand = Math.Floor((r.DiscountRound.Value) / 1000) * 1000;
                            disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                            disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p) + sthousand;

                            r.DiscountRound = disc_rn_n;
                            db.SubmitChanges();
                        }
                        else
                        {
                            disc_rn_n = r.DiscountRound.Value;
                        }
                    }
                }

                string val_disc_round_n = disc_rn_n.ToString("N0");
                string val_disc_round_p = disc_rn_n.ToString("N0");
                td = Td_Disc_Round(tipeproduk, index, r.ID, isPromo, val_disc_round_n, val_disc_round_p);
                tr.Cells.Add(td);

                //fee extra ring size
                string fee_extra_ring_size = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(Harga,0) AS INT) [Harga] FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0", Db.Cnn_DB).ToString();
                td = Td_Fee_Extra_Ring_Size(tipeproduk, index, r.ID, isPromo, extra_ring_size, fee_extra_ring_size);
                tr.Cells.Add(td);

                FeeExtraRingSize = Convert.ToInt32(extra_ring_size) * Convert.ToInt32(fee_extra_ring_size);

                // total bayar
                decimal totalHargaPLU = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain - disc_rn_n + FeeExtraRingSize);
                r.TotalBayar = totalHargaPLU;
                db.SubmitChanges();

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + totalHargaPLU.ToString("N0") + "</p>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                ST += Convert.ToDecimal(GoldPrice);
                ST2 += Convert.ToDecimal(r.TotalBayar);

                index++;
            }
            QTY += iQty;
            GT += ST;
            GT2 += ST2;
            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemGJ(int SO_ID, ref int QTY, ref decimal GT, ref decimal GT2, Msmembership m, decimal disc1, decimal disc2)
    {
        var rs = from p in db.SalesOrderGJ
                 where p.IDForm == SO_ID
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;
        decimal ST2 = 0m;

        if (iQty > 0)
        {
            var vcr = from p in db.SalesVoucher
                      where p.IDSalesOrder == SO_ID && p.ProductType == 3 && p.ProductID != null
                      select p;

            Sub_Header(2);
            foreach (var r in rs)
            {
                int qtyVoucher = 0;
                string descVoucher = "";
                if (vcr.Count() > 0)
                {
                    var vcrdtl = from p in vcr
                                 where p.ProductID == r.IDProduct
                                 select p
                                 ;
                    qtyVoucher = vcrdtl.Count();
                    if (qtyVoucher > 0)
                    {
                        descVoucher += "<table>";
                        foreach (var d in vcrdtl)
                        {
                            descVoucher += "<tr>";
                            descVoucher += "<td>" + d.NomorVoucher + "</td>";
                            descVoucher += "<td>" + Str.Number0(d.Nominal) + "</td>";
                            descVoucher += "<td>" + d.TypeVoucher + "</td>";
                            descVoucher += "</tr>";
                        }
                        descVoucher += "</table>";
                    }

                }

                string tipeproduk = "gj";
                bool isPromo = false;
                string statusPromo = "";
                string tipePromo = "";
                string descPromo = "";
                string statusSouvenir = "";
                string tipeSouvenir = "";
                string descSouvenir = "";
                decimal DiscountPersen = 0m;
                decimal DiscountPromoNominal = 0m;
                decimal DiscountMembership = 0m;
                decimal HargaPLU = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound);
                decimal DiscountProgramBank = 0m;
                decimal DiscountLainLain = 0m;
                decimal FeeExtraRingSize = 0m;

                bool isBranded = false; // Pricing.isDiamondBranded(r.StockProductDJ.Nomor);
                bool isNett = Pricing.isNett(r.StockProductGJ.Nomor);

                bool statusNoPromoNormal = false;
                bool statusNoPromoMembership = false;
                string extra_ring_size = "0";

                // promo gj
                var t = from p in db.PromoGJ_Detail
                        where p.PromoGJ.ApprovalNama != null && p.PromoGJ.TglStart.Value <= DateTime.Today
                        && p.PromoGJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == r.IDProduct
                        select p;
                if (t.Count() > 0)
                {
                    var disc = t.First();
                    var discP = disc.PromoGJ;
                    isPromo = true;
                    statusNoPromoNormal = (discP.StatusDiscNormal != null) ? discP.StatusDiscNormal.Value : false;
                    statusNoPromoMembership = (discP.StatusDiscMembership != null) ? discP.StatusDiscMembership.Value : false;

                    DateTime myDate1 = Convert.ToDateTime(discP.TglExpire);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate1 - myDate2;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
                    else if (days == 0) { result = "Today is the last day!"; }
                    else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

                    string wlabel = "100";
                    tipePromo = DL.PromoStatus(discP.Tipe.Value);
                    descPromo = "<div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Nama Promo</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + discP.Nama + "</div>"
                              + ((discP.PromoHarga.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Promo 1 Harga</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.PromoHarga.Value) + "</div>" : "")
                              + ((discP.PoinReward.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Promo Poin</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.PoinReward.Value) + "</div>" : "")
                              + ((discP.DiscountNominal.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Discount Nominal</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.DiscountNominal.Value) + "</div>" : "")
                              + ((discP.DiscountPersen.Value != 0) ? "<div style='float:left; width:" + wlabel + "px;'>Discount Persen</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.Number0(discP.DiscountPersen.Value) + "</div>" : "")
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Start</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(discP.TglStart) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Expire</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(discP.TglExpire) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>&nbsp;</div>" + "<div style='float:left;'>&nbsp;</div>" + "<div style='float:left;'>( " + result + " )</div>"
                              + "<div style='clear:both;'></div>"
                              + ((discP.StatusASP.Value == 1) ? "<div style='color:red;'>" + "No Exchange, No Resell" + "</div>" : "")
                              + ((discP.StatusDiscNormal != null) ? ((discP.StatusDiscNormal.Value) ? "<div style='color:red;'>" + "No Discount Normal" + "</div>" : "") : "")
                              + ((discP.StatusDiscMembership != null) ? ((discP.StatusDiscMembership.Value) ? "<div style='color:red;'>" + "No Discount Membership" + "</div>" : "") : "")
                              + "</div>"
                              ;

                    statusPromo = "<button OnClick='return false;' class='alert alert-success' style='padding:1px 3px 0 5px; margin:0 0 1px 0;'"
                                + "role='button' data-html='true' data-toggle='popover' data-trigger='focus' title=\"" + tipePromo + "\" data-content=\"" + descPromo + "\""
                                + "> "
                                + "PROMO"
                                + "</button>"
                                ;

                    if (discP.Tipe == 1)
                    {
                        // promo persen
                        DiscountPersen = discP.DiscountPersen.Value;
                        DiscountPromoNominal = discP.DiscountNominal.Value;
                    }
                    else if (discP.Tipe == 3)
                    {
                        // promo 1 harga
                        DiscountPromoNominal = HargaPLU - discP.PromoHarga.Value;
                    }
                }

                // souvenir 
                var sv = from p in db.Bundle_ItemPasangan
                         where p.BundleItem.TglExpire > DateTime.Today && p.IDProduct.Value == r.IDProduct
                         && p.BundleItem.StatusActive == 1 && p.TipeProduct == 2
                         select p;

                if (sv.Count() > 0)
                {
                    var souv2 = sv.First();
                    var souv = souv2.BundleItem;

                    DateTime myDate1 = Convert.ToDateTime(souv.TglExpire);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate1 - myDate2;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
                    else if (days == 0) { result = "Today is the last day!"; }
                    else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

                    string wlabel = "100";
                    tipeSouvenir = "SOUVENIR";
                    descSouvenir = "<div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Nama Souvenir</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + souv.Nama + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Start</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(souv.TglStart) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Expire</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(souv.TglExpire) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>&nbsp;</div>" + "<div style='float:left;'>&nbsp;</div>" + "<div style='float:left;'>( " + result + " )</div>"
                              + "<div style='clear:both;'></div>"
                              + "</div>"
                              ;
                    statusSouvenir = "<button OnClick='return false;' class='alert alert-warning' style='padding:1px 3px 0 5px; margin:0 0 1px 0;'"
                                + "role='button' data-html='true' data-toggle='popover' data-trigger='focus' title=\"" + tipeSouvenir + "\" data-content=\"" + descSouvenir + "\""
                                + "> "
                                + "SOUVENIR"
                                + "</button>"
                                ;

                }

                TableRow tr = new TableRow();
                TableCell td;

                HiddenField hf = new HiddenField();
                hf.ID = tipeproduk + "hf1_" + index;
                hf.Value = r.ID.ToString();

                Literal lt = new Literal();
                lt.Text = TD_HyperLink(2, r.ID, r.IDProduct);

                td = new TableCell();
                td.Controls.Add(hf);
                td.Controls.Add(lt);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                string xPLU = r.StockProductGJ.Nomor;
                string xItem = r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
                td.Text = TD_PLU(0, xPLU, xItem, statusPromo, statusSouvenir);
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + HargaPLU.ToString("N0") + "</p>";
                tr.Cells.Add(td);

                // disc normal
                decimal disc_nd_p = 0m;
                decimal disc_nd_n = 0m;

                if (tipeproduk == "gj")
                {
                    disc_nd_p = 0m;
                    disc_nd_n = 0m;
                }
                else if (tipeproduk != "pg" && tipeproduk != "ld")
                {
                    disc_nd_p = statusNoPromoNormal ? 0m : (r.Discount.Value != null) ? r.Discount.Value : 0m;
                    disc_nd_n = statusNoPromoNormal ? 0m : (r.DiscountNominal.Value != null) ? r.DiscountNominal.Value : 0m;

                    if (!statusNoPromoNormal && !isNett)
                    {
                        disc_nd_p = 10m;
                        disc_nd_n = Math.Floor(((disc_nd_p / 100) * Convert.ToDecimal(HargaPLU)) / 1000) * 1000;
                    }
                }

                r.Discount = disc_nd_p;
                r.DiscountNominal = disc_nd_n;
                db.SubmitChanges();

                string val_disc_normal_n = disc_nd_n.ToString("N0");
                string val_disc_normal_p = disc_nd_p.ToString("N0");
                td = Td_Disc_Normal(tipeproduk, index, r.ID, isPromo, val_disc_normal_n, val_disc_normal_p, statusNoPromoNormal, isNett);
                tr.Cells.Add(td);

                // disc promo
                DiscountPromoNominal = r.DiscountPromo.Value;

                string val_disc_promo_n = DiscountPromoNominal.ToString("N0");
                string val_disc_promo_p = DiscountPersen.ToString("N1");
                td = Td_Disc_Promo(tipeproduk, index, r.ID, isPromo, val_disc_promo_n, val_disc_promo_p);
                tr.Cells.Add(td);

                // disc membership
                decimal disc_ms_p = 0m;
                decimal disc_ms_n = 0m;

                // berdasarkan meeting AE, pg dan gj tidak ada diskon membership
                if (tipeproduk == "pg" || tipeproduk == "gj") { statusNoPromoMembership = true; }

                disc_ms_p = (r.DiscountProgram.Value != null) ? r.DiscountProgram.Value : 0m;
                disc_ms_n = (r.DiscountProgramNominal.Value != null) ? r.DiscountProgramNominal.Value : 0m;

                if (statusNoPromoMembership)
                {
                    disc_ms_p = 0m;
                    disc_ms_n = 0m;
                }
                else
                {

                    try
                    {
                        if (STAMPS.xActivate)
                        {
                            if (r.DiscountNominal == null) { r.DiscountNominal = 0; }
                            if (r.DiscountPromo == null) { r.DiscountPromo = 0; }
                            DiscountMembership = (isBranded) ? disc2 : disc1;
                            disc_ms_p = DiscountMembership;
                            disc_ms_n = Math.Floor(((Convert.ToDecimal(HargaPLU)
                                        - disc_nd_n
                                        - DiscountPromoNominal
                                        )
                                        * DiscountMembership / 100)
                                        );
                        }
                        else if (m != null)
                        {
                            if (r.DiscountNominal == null) { r.DiscountNominal = 0; }
                            if (r.DiscountPromo == null) { r.DiscountPromo = 0; }
                            db.SubmitChanges();

                            DiscountMembership = (isBranded) ? m.M_extradisc2.Value : m.M_extradisc.Value; // diskon membership normal / branded diamond
                            disc_ms_p = DiscountMembership;
                            disc_ms_n = Math.Floor(((Convert.ToDecimal(HargaPLU)
                                        - disc_nd_n
                                        - DiscountPromoNominal
                                        )
                                        * DiscountMembership / 100)
                                        );
                        }
                    }
                    catch (Exception)
                    {

                    }


                }
                r.DiscountProgram = disc_ms_p;
                r.DiscountProgramNominal = disc_ms_n;
                db.SubmitChanges();

                string val_disc_member_n = disc_ms_n.ToString("N0");
                string val_disc_member_p = disc_ms_p.ToString("N1");
                td = Td_Disc_Member(tipeproduk, index, r.ID, isPromo, val_disc_member_n, val_disc_member_p, m, statusNoPromoMembership);
                tr.Cells.Add(td);

                // disc voucher
                decimal disc_vc_p = 0m;
                decimal disc_vc_n = 0m;
                disc_vc_p = (r.DiscountGift.Value != null) ? r.DiscountGift.Value : 0m;
                disc_vc_n = (r.DiscountGift.Value != null) ? r.DiscountGift.Value : 0m;

                string val_disc_voucher_n = disc_vc_n.ToString("N0");
                string val_disc_voucher_p = disc_vc_n.ToString("N0");
                td = Td_Disc_Voucher(tipeproduk, index, r.ID, isPromo, val_disc_voucher_n, val_disc_voucher_p, qtyVoucher, descVoucher);
                tr.Cells.Add(td);

                // disc program bank
                DiscountProgramBank = r.DiscountBank;

                string val_disc_bank_n = DiscountProgramBank.ToString("N0");
                string val_disc_bank_p = DiscountProgramBank.ToString("N1");
                td = Td_Disc_Bank(tipeproduk, index, r.ID, isPromo, val_disc_bank_n, val_disc_bank_p, HargaPLU - disc_nd_n - DiscountPromoNominal - disc_ms_n - disc_vc_n);
                tr.Cells.Add(td);

                // disc lain-lain
                DiscountLainLain = r.DiscountOther;

                string val_disc_other_n = DiscountLainLain.ToString("N0");
                string val_disc_other_p = DiscountLainLain.ToString("N1");
                td = Td_Disc_Other(tipeproduk, index, r.ID, isPromo, val_disc_other_n, val_disc_other_p);
                tr.Cells.Add(td);

                // disc pembulatan (round)
                decimal disc_rn_p = 0m;
                decimal disc_rn_n = 0m;
                decimal totalHargaPLU_wo_floor = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain);

                if (r.DiscountRound == null) { r.DiscountRound = 0m; db.SubmitChanges(); }

                if (r.DiscountRound == 0m) { }
                else if (r.DiscountRound > 0m)
                {
                    if (r.DiscountRound < 1000 && (totalHargaPLU_wo_floor % 1000 != 0))
                    {
                        disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                        disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p);

                        r.DiscountRound = disc_rn_n;
                        db.SubmitChanges();
                    }
                    else if (r.DiscountRound > 1000m)
                    {
                        if ((totalHargaPLU_wo_floor - r.DiscountRound) % 1000 != 0)
                        {
                            decimal sthousand = Math.Floor((r.DiscountRound.Value) / 1000) * 1000;
                            disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                            disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p) + sthousand;

                            r.DiscountRound = disc_rn_n;
                            db.SubmitChanges();
                        }
                        else
                        {
                            disc_rn_n = r.DiscountRound.Value;
                        }
                    }
                }

                string val_disc_round_n = disc_rn_n.ToString("N0");
                string val_disc_round_p = disc_rn_n.ToString("N0");
                td = Td_Disc_Round(tipeproduk, index, r.ID, isPromo, val_disc_round_n, val_disc_round_p);
                tr.Cells.Add(td);

                td = Td_Fee_Extra_Ring_Size(tipeproduk, index, r.ID, isPromo, "0", "0");
                tr.Cells.Add(td);

                // total bayar
                decimal totalHargaPLU = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain - disc_rn_n);
                r.TotalBayar = totalHargaPLU;
                db.SubmitChanges();

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + totalHargaPLU.ToString("N0") + "</p>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                ST += Convert.ToDecimal(r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound);
                ST2 += Convert.ToDecimal(r.TotalBayar);

                index++;

            }
            QTY += iQty;
            GT += ST;
            GT2 += ST2;
            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemLD(int SO_ID, ref int QTY, ref decimal GT, ref decimal GT2, Msmembership m, decimal disc1, decimal disc2)
    {
        var rs = from p in db.SalesOrderLD
                 where p.IDForm == SO_ID
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;
        decimal ST2 = 0;

        if (iQty > 0)
        {
            var vcr = from p in db.SalesVoucher
                      where p.IDSalesOrder == SO_ID && p.ProductType == 4 && p.ProductID != null
                      select p;

            Sub_Header(3);
            foreach (var r in rs)
            {
                int qtyVoucher = 0;
                string descVoucher = "";
                if (vcr.Count() > 0)
                {
                    var vcrdtl = from p in vcr
                                 where p.ProductID == r.IDProduct
                                 select p
                                 ;
                    qtyVoucher = vcrdtl.Count();
                    if (qtyVoucher > 0)
                    {
                        descVoucher += "<table>";
                        foreach (var d in vcrdtl)
                        {
                            descVoucher += "<tr>";
                            descVoucher += "<td>" + d.NomorVoucher + "</td>";
                            descVoucher += "<td>" + Str.Number0(d.Nominal) + "</td>";
                            descVoucher += "<td>" + d.TypeVoucher + "</td>";
                            descVoucher += "</tr>";
                        }
                        descVoucher += "</table>";
                    }

                }

                string tipeproduk = "ld";
                bool isPromo = false;
                string statusPromo = "";
                string tipePromo = "";
                string descPromo = "";
                string statusSouvenir = "";
                string tipeSouvenir = "";
                string descSouvenir = "";
                decimal DiscountPersen = 0m;
                decimal DiscountPromoNominal = 0m;
                decimal DiscountMembership = 0m;
                decimal HargaPLU = Convert.ToDecimal(r.HargaRupiah.Value);
                decimal DiscountProgramBank = 0m;
                decimal DiscountLainLain = 0m;
                bool isBranded = Pricing.isDiamondBranded(r.StockProductLD_Stone1B.Nomor);
                bool isGeneric = Pricing.isDiamondGeneric(r.StockProductLD_Stone1B.Nomor);
                bool isNett = Pricing.isNett(r.StockProductLD_Stone1B.Nomor);

                bool statusNoPromoNormal = false;
                bool statusNoPromoMembership = false;

                //// promo ld
                // belum ada fitur promo ld

                // souvenir 
                var sv = from p in db.Bundle_ItemPasangan
                         where p.BundleItem.TglExpire > DateTime.Today && p.IDProduct.Value == r.IDProduct
                         && p.BundleItem.StatusActive == 1 && p.TipeProduct == 4
                         select p;

                if (sv.Count() > 0)
                {
                    var souv2 = sv.First();
                    var souv = souv2.BundleItem;

                    DateTime myDate1 = Convert.ToDateTime(souv.TglExpire);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate1 - myDate2;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
                    else if (days == 0) { result = "Today is the last day!"; }
                    else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

                    string wlabel = "100";
                    tipeSouvenir = "SOUVENIR";
                    descSouvenir = "<div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Nama Souvenir</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + souv.Nama + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Start</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(souv.TglStart) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>Date Expire</div>" + "<div style='float:left;'>&nbsp;:&nbsp;</div>" + "<div style='float:left;'>" + Str.FullDate(souv.TglExpire) + "</div>"
                              + "<div style='float:left; width:" + wlabel + "px;'>&nbsp;</div>" + "<div style='float:left;'>&nbsp;</div>" + "<div style='float:left;'>( " + result + " )</div>"
                              + "<div style='clear:both;'></div>"
                              + "</div>"
                              ;
                    statusSouvenir = "<button OnClick='return false;' class='alert alert-warning' style='padding:1px 3px 0 5px; margin:0 0 1px 0;'"
                                + "role='button' data-html='true' data-toggle='popover' data-trigger='focus' title=\"" + tipeSouvenir + "\" data-content=\"" + descSouvenir + "\""
                                + "> "
                                + "SOUVENIR"
                                + "</button>"
                                ;

                }

                TableRow tr = new TableRow();
                TableCell td;

                HiddenField hf = new HiddenField();
                hf.ID = tipeproduk + "hf1_" + index;
                hf.Value = r.ID.ToString();

                Literal lt = new Literal();
                lt.Text = TD_HyperLink(3, r.ID, r.IDProduct);

                td = new TableCell();
                td.Controls.Add(hf);
                td.Controls.Add(lt);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                string xPLU = r.StockProductLD_Stone1B.Nomor;
                string xItem = "Loose Diamond";
                td.Text = TD_PLU(0, xPLU, xItem, statusPromo, statusSouvenir);
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + HargaPLU.ToString("N0") + "</p>";
                tr.Cells.Add(td);

                // disc normal
                decimal disc_nd_p = 0m;
                decimal disc_nd_n = 0m;

                if (tipeproduk == "gj")
                {
                    disc_nd_p = 0m;
                    disc_nd_n = 0m;
                }
                else if (tipeproduk != "pg" && tipeproduk != "ld")
                {
                    disc_nd_p = statusNoPromoNormal ? 0m : (r.Discount.Value != null) ? r.Discount.Value : 0m;
                    disc_nd_n = statusNoPromoNormal ? 0m : (r.DiscountNominal.Value != null) ? r.DiscountNominal.Value : 0m;

                    if (!statusNoPromoNormal && !isNett)
                    {
                        disc_nd_p = 10m;
                        disc_nd_n = Math.Floor(((disc_nd_p / 100) * Convert.ToDecimal(HargaPLU)) / 1000) * 1000;
                    }
                }

                r.Discount = disc_nd_p;
                r.DiscountNominal = disc_nd_n;
                db.SubmitChanges();

                string val_disc_normal_n = disc_nd_n.ToString("N0");
                string val_disc_normal_p = disc_nd_p.ToString("N0");
                td = Td_Disc_Normal(tipeproduk, index, r.ID, isPromo, val_disc_normal_n, val_disc_normal_p, statusNoPromoNormal, isNett);
                tr.Cells.Add(td);

                // disc promo
                DiscountPromoNominal = (r.DiscountPromo != null) ? r.DiscountPromo.Value : 0m;

                string val_disc_promo_n = DiscountPromoNominal.ToString("N0");
                string val_disc_promo_p = DiscountPersen.ToString("N1");
                td = Td_Disc_Promo(tipeproduk, index, r.ID, isPromo, val_disc_promo_n, val_disc_promo_p);
                tr.Cells.Add(td);

                // disc membership
                decimal disc_ms_p = 0m;
                decimal disc_ms_n = 0m;

                // berdasarkan meeting AE, pg dan gj tidak ada diskon membership
                if (tipeproduk == "pg" || tipeproduk == "gj") { statusNoPromoMembership = true; }

                disc_ms_p = (r.DiscountProgram.Value != null) ? r.DiscountProgram.Value : 0m;
                disc_ms_n = (r.DiscountProgramNominal.Value != null) ? r.DiscountProgramNominal.Value : 0m;

                if (statusNoPromoMembership)
                {
                    disc_ms_p = 0m;
                    disc_ms_n = 0m;
                }
                else
                {

                    try
                    {
                        if (STAMPS.xActivate)
                        {
                            if (r.DiscountNominal == null) { r.DiscountNominal = 0; }
                            if (r.DiscountPromo == null) { r.DiscountPromo = 0; }
                            DiscountMembership = (isBranded) ? disc2 : ((isGeneric) ? 0m : disc1);
                            disc_ms_p = DiscountMembership;
                            disc_ms_n = Math.Floor(((Convert.ToDecimal(HargaPLU)
                                        - disc_nd_n
                                        - DiscountPromoNominal
                                        )
                                        * DiscountMembership / 100)
                                        );
                        }
                        else if (m != null)
                        {
                            if (r.DiscountNominal == null) { r.DiscountNominal = 0; }
                            if (r.DiscountPromo == null) { r.DiscountPromo = 0; }
                            db.SubmitChanges();

                            DiscountMembership = (isBranded) ? m.M_extradisc2.Value : m.M_extradisc.Value; // diskon membership normal / branded diamond
                            disc_ms_p = DiscountMembership;
                            disc_ms_n = Math.Floor(((Convert.ToDecimal(HargaPLU)
                                        - disc_nd_n
                                        - DiscountPromoNominal
                                        )
                                        * DiscountMembership / 100)
                                        );
                        }
                    }
                    catch (Exception)
                    {

                    }


                }
                r.DiscountProgram = disc_ms_p;
                r.DiscountProgramNominal = disc_ms_n;
                db.SubmitChanges();

                string val_disc_member_n = disc_ms_n.ToString("N0");
                string val_disc_member_p = disc_ms_p.ToString("N1");
                td = Td_Disc_Member(tipeproduk, index, r.ID, isPromo, val_disc_member_n, val_disc_member_p, m, statusNoPromoMembership);
                tr.Cells.Add(td);

                // disc voucher
                decimal disc_vc_p = 0m;
                decimal disc_vc_n = 0m;
                disc_vc_p = (r.DiscountGift.Value != null) ? r.DiscountGift.Value : 0m;
                disc_vc_n = (r.DiscountGift.Value != null) ? r.DiscountGift.Value : 0m;

                string val_disc_voucher_n = disc_vc_n.ToString("N0");
                string val_disc_voucher_p = disc_vc_n.ToString("N0");
                td = Td_Disc_Voucher(tipeproduk, index, r.ID, isPromo, val_disc_voucher_n, val_disc_voucher_p, qtyVoucher, descVoucher);
                tr.Cells.Add(td);

                // disc program bank
                DiscountProgramBank = r.DiscountBank;

                string val_disc_bank_n = DiscountProgramBank.ToString("N0");
                string val_disc_bank_p = DiscountProgramBank.ToString("N1");
                td = Td_Disc_Bank(tipeproduk, index, r.ID, isPromo, val_disc_bank_n, val_disc_bank_p, HargaPLU - disc_nd_n - DiscountPromoNominal - disc_ms_n - disc_vc_n);
                tr.Cells.Add(td);

                // disc lain-lain
                DiscountLainLain = r.DiscountOther;

                string val_disc_other_n = DiscountLainLain.ToString("N0");
                string val_disc_other_p = DiscountLainLain.ToString("N1");
                td = Td_Disc_Other(tipeproduk, index, r.ID, isPromo, val_disc_other_n, val_disc_other_p);
                tr.Cells.Add(td);

                // disc pembulatan (round)
                decimal disc_rn_p = 0m;
                decimal disc_rn_n = 0m;
                decimal totalHargaPLU_wo_floor = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain);

                if (r.DiscountRound == null) { r.DiscountRound = 0m; db.SubmitChanges(); }

                if (r.DiscountRound == 0m) { }
                else if (r.DiscountRound > 0m)
                {
                    if (r.DiscountRound < 1000 && (totalHargaPLU_wo_floor % 1000 != 0))
                    {
                        disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                        disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p);

                        r.DiscountRound = disc_rn_n;
                        db.SubmitChanges();
                    }
                    else if (r.DiscountRound > 1000m)
                    {
                        if ((totalHargaPLU_wo_floor - r.DiscountRound) % 1000 != 0)
                        {
                            decimal sthousand = Math.Floor((r.DiscountRound.Value) / 1000) * 1000;
                            disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                            disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p) + sthousand;

                            r.DiscountRound = disc_rn_n;
                            db.SubmitChanges();
                        }
                        else
                        {
                            disc_rn_n = r.DiscountRound.Value;
                        }
                    }
                }

                string val_disc_round_n = disc_rn_n.ToString("N0");
                string val_disc_round_p = disc_rn_n.ToString("N0");
                td = Td_Disc_Round(tipeproduk, index, r.ID, isPromo, val_disc_round_n, val_disc_round_p);
                tr.Cells.Add(td);

                td = Td_Fee_Extra_Ring_Size(tipeproduk, index, r.ID, isPromo, "0", "0");
                tr.Cells.Add(td);

                // total bayar
                decimal totalHargaPLU = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain - disc_rn_n);
                r.TotalBayar = totalHargaPLU;
                db.SubmitChanges();

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + totalHargaPLU.ToString("N0") + "</p>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                ST += Convert.ToDecimal(r.HargaRupiah.Value);
                ST2 += Convert.ToDecimal(r.TotalBayar);

                index++;
            }
            QTY += iQty;
            GT += ST;
            GT2 += ST2;
            Sub_Total(ST, ST2, false);
        }
    }

    //Item Packaging
    protected void fillPackaging(SalesOrder x)
    {
        var rs = from p in db.SalesOrderPackaging
                 where p.IDForm == myIDSO
                 select p;

        int index = 1;

        //Sub-Header
        if (rs.Count() > 0)
        {
            Sub_HeaderPackaging();

            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(9, r.ID, r.IDProduct);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Kode;
                tr.Cells.Add(td);

                string maxQtyInput = Convert.ToInt32(r.Packaging.StockActualPackaging.SingleOrDefault(p => p.TipeLokasi == x.TipeLokasi && p.IDLokasi == x.IDLokasi).InHand).ToString();

                td = new TableCell();
                td.Text = maxQtyInput;
                tr.Cells.Add(td);

                TextBox txt = new TextBox();
                txt.ID = "pc_" + index.ToString();
                txt.Attributes["style"] = "width:50px; text-align:center;";
                txt.Attributes["type"] = "number";
                txt.Attributes["min"] = "1";
                txt.Attributes["max"] = maxQtyInput;
                txt.Text = r.Qty.HasValue ? r.Qty.Value.ToString("N0") : "1";
                //txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                //txt.AutoPostBack = true;
                //txt.TextChanged += new EventHandler(Discount_TextChanged);
                txt.Enabled = (myConfirm == "1") ? false : true;

                HiddenField hf = new HiddenField();
                hf.ID = "hfpc" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                //td.Controls.Add(txt);
                td.Text = r.Qty.Value.ToString("N0");
                //td.Controls.Add(hf);
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Attributes["style"] = "text-align:center;";
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                index++;

            }
        }
        else
        {
            App.TableNoResult(tb2);
        }
    }

    //Item Souvenir
    protected void fillSouvenir(SalesOrder x)
    {
        //Response.Redirect("uidk");
        var rs = from p in db.SalesOrderSouvenir
                 where p.IDForm == myIDSO && (p.TipeProduct == 0 || p.TipeProduct == null)
                 select p;

        int index = 1;

        //Sub-Header
        if (rs.Count() > 0)
        {
            Sub_HeaderSouvenir();

            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(10, r.ID, r.IDProduct.Value);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Kode;
                tr.Cells.Add(td);

                string maxQtyInput = Convert.ToInt32(r.Souvenir.StockActualSouvenir.SingleOrDefault(p => p.TipeLokasi == x.TipeLokasi && p.IDLokasi == x.IDLokasi).InHand).ToString();

                td = new TableCell();
                td.Text = maxQtyInput;
                tr.Cells.Add(td);

                TextBox txt = new TextBox();
                txt.ID = "sv_" + index.ToString();
                txt.Attributes["style"] = "width:50px; text-align:center;";
                txt.Attributes["type"] = "number";
                txt.Attributes["min"] = "1";
                txt.Attributes["max"] = maxQtyInput;
                txt.Text = r.Qty.HasValue ? r.Qty.Value.ToString("N0") : "1";
                //txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                //txt.AutoPostBack = true;
                //txt.TextChanged += new EventHandler(Discount_TextChanged);
                txt.Enabled = (myConfirm == "1") ? false : true;

                HiddenField hf = new HiddenField();
                hf.ID = "hfsv" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                //td.Controls.Add(txt);
                td.Text = r.Qty.Value.ToString("N0");
                //td.Controls.Add(hf);
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Attributes["style"] = "text-align:center;";
                tr.Cells.Add(td);

                tb3.Rows.Add(tr);

                index++;

            }
        }
        else
        {
            App.TableNoResult(tb3);
        }

    }

    protected void fillSouvenirGJ()
    {
        var rs = from p in db.SalesOrderSouvenir
                 join q in db.StockProductGJ on p.IDProductSouvenir equals q.ID
                 where p.IDForm == myIDSO && p.TipeProduct == 2
                 select new { p, q };

        int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenirGJ();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(11, r.p.ID, r.p.IDProductSouvenir.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.StockProductGJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
            txt.Text = r.p.Qty.HasValue ? r.p.Qty.Value.ToString("N0") : "1";
            //txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt.AutoPostBack = true;
            //txt.TextChanged += new EventHandler(Discount_TextChanged);
            txt.Enabled = (myConfirm == "1") ? false : true;

            HiddenField hf = new HiddenField();
            hf.ID = "hfsvgj" + index.ToString();
            hf.Value = r.p.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
        //Response.Redirect("bjdkndk");
    }
    protected void fillSouvenirDJ()
    {
        var rs = from p in db.SalesOrderSouvenir
                 join q in db.StockProductDJ on p.IDProductSouvenir equals q.ID
                 where p.IDForm == myIDSO && p.TipeProduct == 1
                 select new { p, q };

        int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenirDJ();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(12, r.p.ID, r.p.IDProductSouvenir.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
            txt.Text = r.p.Qty.HasValue ? r.p.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.Enabled = (myConfirm == "1") ? false : true;

            HiddenField hf = new HiddenField();
            hf.ID = "hfsvdj" + index.ToString();
            hf.Value = r.p.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }
    protected void fillSouvenirLD()
    {
        var rs = from p in db.SalesOrderSouvenir
                 join q in db.StockProductLD_Stone1B on p.IDProductSouvenir equals q.ID
                 where p.IDForm == myIDSO && p.TipeProduct == 4
                 select new { p, q };

        int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenirLD();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(13, r.p.ID, r.p.IDProductSouvenir.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "-";
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
            txt.Text = r.p.Qty.HasValue ? r.p.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.Enabled = (myConfirm == "1") ? false : true;

            HiddenField hf = new HiddenField();
            hf.ID = "hfsvld" + index.ToString();
            hf.Value = r.p.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }
    protected void fillSouvenirPG()
    {
        var rs = from p in db.SalesOrderSouvenir
                 join q in db.StockProductPG on p.IDProductSouvenir equals q.ID
                 where p.IDForm == myIDSO && p.TipeProduct == 3
                 select new { p, q };

        int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenirPG();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(14, r.p.ID, r.p.IDProductSouvenir.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.CharProductItem.Nama;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
            txt.Text = r.p.Qty.HasValue ? r.p.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.Enabled = (myConfirm == "1") ? false : true;

            HiddenField hf = new HiddenField();
            hf.ID = "hfsvpg" + index.ToString();
            hf.Value = r.p.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }

    protected void Sub_Header(int ItemIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.ItemIndex(ItemIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void Sub_HeaderPackaging()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "Packaging";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
    }
    protected void Sub_HeaderSouvenir()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "Souvenir";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }

    protected void Sub_HeaderSouvenirGJ()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "Souvenir Gold Jewellery";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }
    protected void Sub_HeaderSouvenirDJ()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "Souvenir Diamond Jewellery";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }
    protected void Sub_HeaderSouvenirPG()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "Souvenir Plain Gold";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }
    protected void Sub_HeaderSouvenirLD()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "Souvenir Loose Diamond";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }

    protected void Sub_Total(decimal Total, decimal TotalBayar, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 3;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-ms\" style='font-weight:bold;'>" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        td.ColumnSpan = 8;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-ms\" style='font-weight:bold;'>" + Str.Number0(TotalBayar) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    // Normal
    protected TableCell Td_Disc_Normal(string tipeproduk, int index, int ID, bool isPromo, string value1, string value2, bool StatusNoPromoNormal, bool isNett)
    {
        // BUSINESS RULE HERE !!! :: START
        string sID1 = sDiscNormal_N;
        string sID2 = sDiscNormal_P;

        bool isEnable1 = false;
        bool isEnable2 = false;

        bool isVisible1 = true;
        bool isVisible2 = true;

        if (tipeproduk == "pg" || tipeproduk == "ld")
        {
            isEnable1 = false;
            isEnable2 = false;

            isVisible1 = false;
            isVisible2 = false;
        }

        if (StatusNoPromoNormal || isNett)
        {
            value1 = "0";
            value2 = "0";

            isEnable1 = false;
            isEnable2 = false;
        }

        isEnable1 = (myConfirm == "1") ? false : isEnable1;
        isEnable2 = (myConfirm == "1") ? false : isEnable2;

        // BUSINESS RULE HERE !!! :: END

        TableCell td = new TableCell();

        Label lblP = new Label();
        lblP.Text = " %";
        lblP.Width = 25;

        Label lblI = new Label();
        lblI.Text = " IDR";
        lblI.Width = 25;

        Literal div1 = new Literal();
        div1.Text = "<div style='padding-bottom:5px;'>";
        Literal div2 = new Literal();
        div2.Text = "</div>";

        TextBox txt1 = new TextBox();
        txt1.ID = tipeproduk + sID1 + "_" + index.ToString();
        txt1.Attributes["style"] = "width: 74px; text-align:right;";
        txt1.Text = value1 == "" ? "0" : value1;
        txt1.Enabled = isEnable1;
        if (isEnable1 && isVisible1)
        {
            txt1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt1.AutoPostBack = isEnable1;
            txt1.TextChanged += new EventHandler(Discount_TextChanged);
        }

        TextBox txt2 = new TextBox();
        txt2.ID = tipeproduk + sID2 + "_" + index.ToString();
        txt2.Attributes["style"] = "width: 40px; text-align:right;";
        txt2.Text = value2 == "" ? "0" : value2;
        txt2.Enabled = isEnable2;
        if (isEnable2 && isVisible2)
        {
            txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt2.AutoPostBack = isEnable2;
            txt2.TextChanged += new EventHandler(Discount_TextChanged);
        }

        td.Controls.Add(div1);
        if (isVisible1)
        {
            td.Controls.Add(txt1);
            td.Controls.Add(lblI);
        }
        td.Controls.Add(div2);
        if (isVisible2)
        {
            td.Controls.Add(txt2);
            td.Controls.Add(lblP);
        }
        td.Attributes["style"] = "text-align:right;";

        return td;
    }

    // Promo After Sales Support
    protected TableCell Td_Disc_Promo(string tipeproduk, int index, int ID, bool isPromo, string value1, string value2)
    {
        // BUSINESS RULE HERE !!! :: START
        string sID1 = sDiscPromo_N;
        string sID2 = sDiscPromo_P;

        bool isEnable1 = false;
        bool isEnable2 = false;

        bool isVisible1 = true;
        bool isVisible2 = true;

        isEnable1 = (myConfirm == "1") ? false : isEnable1;
        isEnable2 = (myConfirm == "1") ? false : isEnable2;

        // BUSINESS RULE HERE !!! :: END

        TableCell td = new TableCell();

        Label lblP = new Label();
        lblP.Text = " %";
        lblP.Width = 25;

        Label lblI = new Label();
        lblI.Text = " IDR";
        lblI.Width = 25;

        Literal div1 = new Literal();
        div1.Text = "<div style='padding-bottom:5px;'>";
        Literal div2 = new Literal();
        div2.Text = "</div>";

        TextBox txt1 = new TextBox();
        txt1.ID = tipeproduk + sID1 + "_" + index.ToString();
        txt1.Attributes["style"] = "width: 74px; text-align:right;";
        txt1.Text = value1 == "" ? "0" : value1;
        txt1.Enabled = isEnable1;
        if (isEnable1 && isVisible1)
        {
            txt1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt1.AutoPostBack = isEnable1;
            txt1.TextChanged += new EventHandler(Discount_TextChanged);
        }

        TextBox txt2 = new TextBox();
        txt2.ID = tipeproduk + sID2 + "_" + index.ToString();
        txt2.Attributes["style"] = "width: 40px; text-align:right;";
        txt2.Text = value2 == "" ? "0" : value2;
        txt2.Enabled = isEnable2;
        if (isEnable2 && isVisible2)
        {
            txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt2.AutoPostBack = isEnable2;
            txt2.TextChanged += new EventHandler(Discount_TextChanged);
        }

        td.Controls.Add(div1);
        if (isVisible1)
        {
            td.Controls.Add(txt1);
            td.Controls.Add(lblI);
        }
        td.Controls.Add(div2);
        if (isVisible2)
        {
            td.Controls.Add(txt2);
            td.Controls.Add(lblP);
        }
        td.Attributes["style"] = "text-align:right;";

        return td;
    }

    // Discount Program Bank
    protected TableCell Td_Disc_Bank(string tipeproduk, int index, int ID, bool isPromo, string value1, string value2, decimal hargarupiah = 0m)
    {
        // BUSINESS RULE HERE !!! :: START
        string sID1 = sDiscBank_N;
        string sID2 = sDiscBank_P;

        bool isEnable1 = true;
        bool isEnable2 = false;

        bool isVisible1 = true;
        bool isVisible2 = false;

        isEnable1 = (myConfirm == "1") ? false : isEnable1;
        isEnable2 = (myConfirm == "1") ? false : isEnable2;

        // BUSINESS RULE HERE !!! :: END

        TableCell td = new TableCell();

        Label lblP = new Label();
        lblP.Text = " %";
        lblP.Width = 25;

        Label lblI = new Label();
        lblI.Text = " IDR";
        lblI.Width = 25;

        Literal div1 = new Literal();
        div1.Text = "<div style='padding-bottom:5px;'>";
        Literal div2 = new Literal();
        div2.Text = "</div>";

        DropDownList ddlpromo1 = new DropDownList();
        ddlpromo1.ID = tipeproduk + sID1 + "ddl_" + index.ToString();
        ddlpromo1.Attributes["style"] = "margin-top:5px;width: 100px;";
        ddlpromo1.Attributes["data-index-" + tipeproduk] = index.ToString();
        ddlpromo1.Attributes["data-id-" + tipeproduk] = ID.ToString();
        ddlpromo1.Enabled = isEnable1;
        PromoBankList(ddlpromo1, hargarupiah);

        TextBox txt1 = new TextBox();
        txt1.ID = tipeproduk + sID1 + "_" + index.ToString();
        txt1.Attributes["style"] = "width: 74px; text-align:right;";
        txt1.Text = value1 == "" ? "0" : value1;
        txt1.Enabled = isEnable1;
        if (isEnable1 && isVisible1)
        {
            txt1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt1.AutoPostBack = isEnable1;
            txt1.TextChanged += new EventHandler(Discount_TextChanged);

            ddlpromo1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddlpromo1.AutoPostBack = isEnable1;
            ddlpromo1.SelectedIndexChanged += new EventHandler(DropDownListPromoBank_Changed);
        }

        DropDownList ddlpromo2 = new DropDownList();
        ddlpromo2.ID = tipeproduk + sID2 + "ddl_" + index.ToString();
        ddlpromo2.Attributes["style"] = "margin-top:5px;width: 100px;";
        ddlpromo2.Attributes["data-index-" + tipeproduk] = index.ToString();
        ddlpromo2.Attributes["data-id-" + tipeproduk] = ID.ToString();
        ddlpromo2.Enabled = isEnable2;
        PromoBankList(ddlpromo2, hargarupiah);

        TextBox txt2 = new TextBox();
        txt2.ID = tipeproduk + sID2 + "_" + index.ToString();
        txt2.Attributes["style"] = "width: 40px; text-align:right;";
        txt2.Text = value2 == "" ? "0" : value2;
        txt2.Enabled = isEnable2;
        if (isEnable2 && isVisible2)
        {
            txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt2.AutoPostBack = isEnable2;
            txt2.TextChanged += new EventHandler(Discount_TextChanged);

            ddlpromo2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddlpromo2.AutoPostBack = isEnable1;
            ddlpromo2.SelectedIndexChanged += new EventHandler(DropDownListPromoBank_Changed);
        }

        if (tipeproduk == "dj")
        {
            int promo = Db.SingleInteger("SELECT TOP 1 ISNULL(IDPromoDetail,0) [IDPromoDetail] FROM SalesOrderDJ WHERE ID = " + ID.ToString(), Db.Cnn_DB);
            ddlpromo1.SelectedValue = promo.ToString();
            ddlpromo2.SelectedValue = promo.ToString();
        }
        else if (tipeproduk == "pg")
        {
            int promo = Db.SingleInteger("SELECT TOP 1 ISNULL(IDPromoDetail,0) [IDPromoDetail] FROM SalesOrderPG WHERE ID = " + ID.ToString(), Db.Cnn_DB);
            ddlpromo1.SelectedValue = promo.ToString();
            ddlpromo2.SelectedValue = promo.ToString();
        }
        else if (tipeproduk == "gj")
        {
            int promo = Db.SingleInteger("SELECT TOP 1 ISNULL(IDPromoDetail,0) [IDPromoDetail] FROM SalesOrderGJ WHERE ID = " + ID.ToString(), Db.Cnn_DB);
            ddlpromo1.SelectedValue = promo.ToString();
            ddlpromo2.SelectedValue = promo.ToString();
        }
        else if (tipeproduk == "ld")
        {
            int promo = Db.SingleInteger("SELECT TOP 1 ISNULL(IDPromoDetail,0) [IDPromoDetail] FROM SalesOrderLD WHERE ID = " + ID.ToString(), Db.Cnn_DB);
            ddlpromo1.SelectedValue = promo.ToString();
            ddlpromo2.SelectedValue = promo.ToString();
        }

        td.Controls.Add(div1);
        if (isVisible1)
        {
            td.Controls.Add(txt1);
            td.Controls.Add(lblI);
            td.Controls.Add(ddlpromo1);
        }
        td.Controls.Add(div2);
        if (isVisible2)
        {
            td.Controls.Add(txt2);
            td.Controls.Add(lblP);
            td.Controls.Add(ddlpromo2);
        }
        td.Attributes["style"] = "text-align:right;";

        return td;
    }

    private void PromoBankList(DropDownList c, decimal hargarupiah = 0m)
    {
        string query = "SELECT A.ID, A.Nama, MinValue, MaxValue, Discount, B.Nama [PromoName] FROM PromoDetail A " +
            "JOIN Promo B ON A.IDForm = B.ID " +
            "WHERE B.Draft = 0 AND B.Disable = 0 AND GETDATE() BETWEEN StartDate AND EndDate AND " + Convert.ToInt64(hargarupiah).ToString() + " BETWEEN MinValue AND MaxValue ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        c.Items.Add(new ListItem("No Promo", "0"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString() + " (" + item["PromoName"] + " : " + Convert.ToDecimal(item["Discount"]).ToString("N0") + ")";
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
        c.Items.Add(new ListItem("Lain Lain", "-1"));
    }
    // Diskon Membership
    protected TableCell Td_Disc_Member(string tipeproduk, int index, int ID, bool isPromo, string value1, string value2, Msmembership member, bool StatusNoPromoMembership)
    {
        // BUSINESS RULE HERE !!! :: START
        string sID1 = sDiscMember_N;
        string sID2 = sDiscMember_P;

        bool isEnable1 = false;
        bool isEnable2 = false;

        bool isVisible1 = true;
        bool isVisible2 = true;

        if (StatusNoPromoMembership)
        {
            value1 = "0";
            value2 = "0";

            isEnable1 = false;
            isEnable2 = false;
        }

        isEnable1 = (myConfirm == "1") ? false : isEnable1;
        isEnable2 = (myConfirm == "1") ? false : isEnable2;

        // BUSINESS RULE HERE !!! :: END

        TableCell td = new TableCell();

        Label lblP = new Label();
        lblP.Text = " %";
        lblP.Width = 25;

        Label lblI = new Label();
        lblI.Text = " IDR";
        lblI.Width = 25;

        Literal div1 = new Literal();
        div1.Text = "<div style='padding-bottom:5px;'>";
        Literal div2 = new Literal();
        div2.Text = "</div>";

        TextBox txt1 = new TextBox();
        txt1.ID = tipeproduk + sID1 + "_" + index.ToString();
        txt1.Attributes["style"] = "width: 74px; text-align:right;";
        txt1.Text = value1 == "" ? "0" : value1;
        txt1.Enabled = isEnable1;
        if (isEnable1 && isVisible1)
        {
            txt1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt1.AutoPostBack = isEnable1;
            txt1.TextChanged += new EventHandler(Discount_TextChanged);
        }

        TextBox txt2 = new TextBox();
        txt2.ID = tipeproduk + sID2 + "_" + index.ToString();
        txt2.Attributes["style"] = "width: 40px; text-align:right;";
        txt2.Text = value2 == "" ? "0" : value2;
        txt2.Enabled = isEnable2;
        if (isEnable2 && isVisible2)
        {
            txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt2.AutoPostBack = isEnable2;
            txt2.TextChanged += new EventHandler(Discount_TextChanged);
        }

        td.Controls.Add(div1);
        if (isVisible1)
        {
            td.Controls.Add(txt1);
            td.Controls.Add(lblI);
        }
        td.Controls.Add(div2);
        if (isVisible2)
        {
            td.Controls.Add(txt2);
            td.Controls.Add(lblP);
        }
        td.Attributes["style"] = "text-align:right;";

        return td;
    }

    // Diskon Voucher
    protected TableCell Td_Disc_Voucher(string tipeproduk, int index, int ID, bool isPromo, string value1, string value2, int qtyVoucher, string desc)
    {
        // BUSINESS RULE HERE !!! :: START
        string sID1 = sDiscVoucher_N;
        string sID2 = sDiscVoucher_P;

        bool isEnable1 = false;
        bool isEnable2 = false;

        bool isVisible1 = (tipeproduk == "dj" || tipeproduk == "pg" || tipeproduk == "gj" || tipeproduk == "ld") ? true : false;
        bool isVisible2 = false;

        // Voucher berlaku untuk 1 barang dan tidak dapat multi voucher terhadap 1 barang
        //isEnable1 = (Total_NominalVoucher > 0m) ? true : false;

        //isEnable1 = (myConfirm == "1") ? false : isEnable1;
        //isEnable2 = (myConfirm == "1") ? false : isEnable2;

        // BUSINESS RULE HERE !!! :: END

        TableCell td = new TableCell();

        Label lblP = new Label();
        lblP.Text = " %";
        lblP.Width = 25;

        Label lblI = new Label();
        lblI.Text = " IDR";
        lblI.Width = 25;

        Literal div1 = new Literal();
        div1.Text = "<div style='padding-bottom:5px;'>";
        Literal div2 = new Literal();
        div2.Text = "</div>";

        TextBox txt1 = new TextBox();
        txt1.ID = tipeproduk + sID1 + "_" + index.ToString();
        txt1.Attributes["style"] = "width: 74px; text-align:right;";
        txt1.Text = value1 == "" ? "0" : value1;
        txt1.Enabled = isEnable1;
        if (isEnable1 && isVisible1)
        {
            txt1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt1.AutoPostBack = isEnable1;
            txt1.TextChanged += new EventHandler(Discount_TextChanged);
        }

        TextBox txt2 = new TextBox();
        txt2.ID = tipeproduk + sID2 + "_" + index.ToString();
        txt2.Attributes["style"] = "width: 40px; text-align:right;";
        txt2.Text = value2 == "" ? "0" : value2;
        txt2.Enabled = isEnable2;
        if (isEnable2 && isVisible2)
        {
            txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt2.AutoPostBack = isEnable2;
            txt2.TextChanged += new EventHandler(Discount_TextChanged);
        }

        td.Controls.Add(div1);
        if (isVisible1)
        {
            td.Controls.Add(txt1);
            td.Controls.Add(lblI);
        }
        td.Controls.Add(div2);
        if (isVisible2)
        {
            td.Controls.Add(txt2);
            td.Controls.Add(lblP);
        }
        td.Attributes["style"] = "text-align:right;";

        /// Add Voucher Discount hanya untuk DJ & LD (2018-10-31)
        /// Revisi dari CRM Add Membership Redemption bisa berlaku untuk seluruh jenis barang (2018-12-27)
        if (tipeproduk == "dj" || tipeproduk == "pg" || tipeproduk == "gj" || tipeproduk == "ld")
        {
            int itemtype = 0;
            if (tipeproduk == "dj") { itemtype = 1; }
            if (tipeproduk == "pg") { itemtype = 2; }
            if (tipeproduk == "gj") { itemtype = 3; }
            if (tipeproduk == "ld") { itemtype = 4; }

            Literal litBtn1 = new Literal();
            Literal litBtn2 = new Literal();
            Literal btnL = new Literal();
            Literal btnM = new Literal();
            Literal btnR = new Literal();

            litBtn1.Text = "<div class='btn-group btn-group-xs'>";
            litBtn2.Text = "</div>";

            string url1 = "#";
            string url2 = "#";

            if (STAMPS.xActivate)
            {
                url1 = "Sales_VoucherStampsAdd.aspx?id=" + myID + "&itemtype=" + itemtype + "&itemid=" + ID + "&so=" + myIDSO + "&ro=" + myIDRO;
                url2 = "Sales_VoucherStampsVoid.aspx?id=" + myID + "&itemtype=" + itemtype + "&itemid=" + ID + "&so=" + myIDSO + "&ro=" + myIDRO;

            }
            else
            {
                url1 = "Sales_VoucherAdd.aspx?id=" + myID + "&itemtype=" + itemtype + "&itemid=" + ID + "&so=" + myIDSO + "&ro=" + myIDRO;
                url2 = "Sales_VoucherVoid.aspx?id=" + myID + "&itemtype=" + itemtype + "&itemid=" + ID + "&so=" + myIDSO + "&ro=" + myIDRO;

            }

            btnL.Text += "<a href='" + url1 + "' class='btn btn-success btn-xs'><span class='icon-plus'></span></a>";

            if (qtyVoucher > 0 && desc != "")
            {
                desc = "<div style='font-size:10px;'>" + desc + "</div>";
                btnM.Text += "<button type='button' onclick='return false;' data-toggle='popover' data-placement='bottom' data-html='true' data-content=\"" + desc + "\" class='btn btn-default btn-xs'>" + (qtyVoucher > 0 ? (qtyVoucher.ToString() + " ") : "") + "<span class='icon-ticket'></span></button>";

                btnR.Text += "<a href='" + url2 + "' class='btn btn-danger btn-xs'><span class='icon-remove'></span></a>";

            }
            else
            {
                btnM.Text += "<button type='button' onclick='return false;' class='btn btn-default btn-xs disabled'>0 <span class='icon-ticket'></span></button>";

                btnR.Text += "<a href='" + url2 + "' class='btn btn-danger btn-xs disabled'><span class='icon-remove'></span></a>";

            }

            td.Controls.Add(litBtn1);
            if (!(myConfirm == "1")) { td.Controls.Add(btnL); }
            td.Controls.Add(btnM);
            if (!(myConfirm == "1")) { td.Controls.Add(btnR); }
            td.Controls.Add(litBtn2);
        }

        return td;
    }

    // Discount Lain Lain
    protected TableCell Td_Disc_Other(string tipeproduk, int index, int ID, bool isPromo, string value1, string value2)
    {
        // BUSINESS RULE HERE !!! :: START
        string sID1 = sDiscOther_N;
        string sID2 = sDiscOther_P;

        bool isEnable1 = true;
        bool isEnable2 = false;

        bool isVisible1 = true;
        bool isVisible2 = false;

        isEnable1 = (myConfirm == "1") ? false : isEnable1;
        isEnable2 = (myConfirm == "1") ? false : isEnable2;

        // BUSINESS RULE HERE !!! :: END

        TableCell td = new TableCell();

        Label lblP = new Label();
        lblP.Text = " %";
        lblP.Width = 25;

        Label lblI = new Label();
        lblI.Text = " IDR";
        lblI.Width = 25;

        Literal div1 = new Literal();
        div1.Text = "<div style='padding-bottom:5px;'>";
        Literal div2 = new Literal();
        div2.Text = "</div>";

        TextBox txt1 = new TextBox();
        txt1.ID = tipeproduk + sID1 + "_" + index.ToString();
        txt1.Attributes["style"] = "width: 74px; text-align:right;";
        txt1.Text = value1 == "" ? "0" : value1;
        txt1.Enabled = isEnable1;
        if (isEnable1 && isVisible1)
        {
            txt1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt1.AutoPostBack = isEnable1;
            txt1.TextChanged += new EventHandler(Discount_TextChanged);
        }

        TextBox txt2 = new TextBox();
        txt2.ID = tipeproduk + sID2 + "_" + index.ToString();
        txt2.Attributes["style"] = "width: 40px; text-align:right;";
        txt2.Text = value2 == "" ? "0" : value2;
        txt2.Enabled = isEnable2;
        if (isEnable2 && isVisible2)
        {
            txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt2.AutoPostBack = isEnable2;
            txt2.TextChanged += new EventHandler(Discount_TextChanged);
        }

        td.Controls.Add(div1);
        if (isVisible1)
        {
            td.Controls.Add(txt1);
            td.Controls.Add(lblI);
        }
        td.Controls.Add(div2);
        if (isVisible2)
        {
            td.Controls.Add(txt2);
            td.Controls.Add(lblP);
        }
        td.Attributes["style"] = "text-align:right;";

        return td;
    }

    // Fee Extra Ring Size
    protected TableCell Td_Fee_Extra_Ring_Size(string tipeproduk, int index, int ID, bool isPromo, string value1, string value2)
    {
        // BUSINESS RULE HERE !!! :: START
        string sID1 = sExtraRingSize_N;
        string sID2 = sFeeExtraRingSize_N;
        string sID3 = sTotalFeeExtraRingSize_N;

        bool isEnable1 = false;

        bool isVisible1 = true;
        isEnable1 = (myConfirm == "1") ? false : isEnable1;
        // BUSINESS RULE HERE !!! :: END

        TableCell td = new TableCell();

        Label lblI = new Label();
        lblI.Text = " IDR";
        lblI.Width = 25;

        Literal div1 = new Literal();
        div1.Text = "<div style='padding-bottom:5px;'>";
        Literal div2 = new Literal();
        div2.Text = "</div>";

        TextBox txt1 = new TextBox();
        txt1.ID = tipeproduk + sID3 + "_" + index.ToString();
        txt1.Attributes["style"] = "width: 74px; text-align:right;";
        txt1.Text = (Convert.ToInt32(value1) * Convert.ToInt32(value2)).ToString("N0");
        //txt1.Text = value1.ToString() + value2.ToString();
        txt1.Enabled = isEnable1;
        if (isEnable1 && isVisible1)
        {
            txt1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt1.AutoPostBack = isEnable1;
            txt1.TextChanged += new EventHandler(Discount_TextChanged);
        }

        td.Controls.Add(div1);
        if (isVisible1)
        {
            td.Controls.Add(txt1);
            td.Controls.Add(lblI);
        }
        td.Attributes["style"] = "text-align:right;";

        return td;
    }

    // Diskon Pembulatan
    protected TableCell Td_Disc_Round(string tipeproduk, int index, int ID, bool isPromo, string value1, string value2)
    {
        // BUSINESS RULE HERE !!! :: START
        string sID1 = sDiscRound_N;
        string sID2 = sDiscRound_P;

        bool isEnable1 = true;
        bool isEnable2 = false;

        bool isVisible1 = true;
        bool isVisible2 = false;

        isEnable1 = (myConfirm == "1") ? false : isEnable1;
        isEnable2 = (myConfirm == "1") ? false : isEnable2;

        // BUSINESS RULE HERE !!! :: END

        TableCell td = new TableCell();

        Label lblP = new Label();
        lblP.Text = " %";
        lblP.Width = 25;

        Label lblI = new Label();
        lblI.Text = " IDR";
        lblI.Width = 25;

        Literal div1 = new Literal();
        div1.Text = "<div style='padding-bottom:5px;'>";
        Literal div2 = new Literal();
        div2.Text = "</div>";

        TextBox txt1 = new TextBox();
        txt1.ID = tipeproduk + sID1 + "_" + index.ToString();
        txt1.Attributes["style"] = "width: 74px; text-align:right;";
        txt1.Text = value1 == "" ? "0" : value1;
        txt1.Enabled = isEnable1;
        if (isEnable1 && isVisible1)
        {
            txt1.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt1.AutoPostBack = isEnable1;
            txt1.TextChanged += new EventHandler(Discount_TextChanged);
        }

        TextBox txt2 = new TextBox();
        txt2.ID = tipeproduk + sID2 + "_" + index.ToString();
        txt2.Attributes["style"] = "width: 40px; text-align:right;";
        txt2.Text = value2 == "" ? "0" : value2;
        txt2.Enabled = isEnable2;
        if (isEnable2 && isVisible2)
        {
            txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt2.AutoPostBack = isEnable2;
            txt2.TextChanged += new EventHandler(Discount_TextChanged);
        }

        td.Controls.Add(div1);
        if (isVisible1)
        {
            td.Controls.Add(txt1);
            td.Controls.Add(lblI);
        }
        td.Controls.Add(div2);
        if (isVisible2)
        {
            td.Controls.Add(txt2);
            td.Controls.Add(lblP);
        }
        td.Attributes["style"] = "text-align:right;";

        return td;
    }

    protected string TD_PLU(int ItemIndex, string PLU, string Item, string Promo, string Souvenir)
    {
        string x = string.Empty;

        x += "<div>";
        x += "<span class='text-bold pull-left' style='font-size:15px;'>" + PLU + "</span>";
        x += "</div>";
        x += "<div class='pull-right'>" + Promo + Souvenir + "</div>";
        x += "<div style='clear: both; border-bottom:solid 1px silver;'></div>";
        x += "<div>";
        x += "<span class='text-muted'>" + Str.CapUpper(Item) + "</span>";
        x += "</div>";

        return x;
    }

    //Display Column
    protected string TD_HyperLink(int ItemIndex, int ID, int IDProduct, string type = "", decimal dPercentageUsed = 100)
    {
        string x = string.Empty;
        string href = string.Empty;

        x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
        if (ItemIndex == 0)
        {
            x += "  <a target=\"_blank\" class=\"btn btn-xs btn-info\" href=\"DJ_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <span class=\"glyphicon glyphicon-search\" aria-hidden=\"true\"></span> ";
            x += "      ";
            x += "  </a>";
        }
        else if (ItemIndex == 1)
        {
            x += "  <a target=\"_blank\" class=\"btn btn-xs btn-info\" href=\"PG_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <span class=\"glyphicon glyphicon-search\" aria-hidden=\"true\"></span> ";
            x += "      ";
            x += "  </a>";
        }
        /// GJ Belum ada halaman detail
        //else if (ItemIndex == 2)
        //{
        //    x += "  <a target=\"_blank\" class=\"btn btn-xs btn-info\" href=\"GJ_Detail.aspx?id=" + IDProduct + "\">";
        //    x += "      <span class=\"glyphicon glyphicon-search\" aria-hidden=\"true\"></span> ";
        //    x += "      ";
        //    x += "  </a>";
        //}
        x += "</div>";

        if (((type == "voucher" && dPercentageUsed == 0) || (type == "" && dPercentageUsed == 100)) && (myConfirm != "1"))
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-xs btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + ItemIndex + "', '" + ID + "', '" + myIDSO + "', '" + myIDRO + "')\">";
            x += "      <span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span> ";
            x += "      ";
            x += "  </a>";
            x += "</div>";
        }

        return x;
    }

    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }

    protected string TD_Disc(decimal DiscountPersen, decimal DiscountPromoNominal)
    {
        string x = string.Empty;

        x += "<p class=\"text-muted text-sm\"> " + DiscountPersen.ToString("N2") + " %</p>";
        x += "<p class=\"text-muted text-sm\"> " + DiscountPromoNominal.ToString("N0") + " IDR</p>";

        return x;
    }

    //Review
    protected void review_Click(object sender, EventArgs e)
    {
        //div_review.Visible = false;
        //div_confirm.Visible = true;


        var s = db.Resell.SingleOrDefault(p => p.ID == myIDRO && p.Draft);
        bool xwhite_gt = false;
        bool xvalid_gt = false;
        xwhite_gt = checkWhitelistPLU(s.ID);


        var x = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO);
        xvalid_gt = (xwhite_gt ? xwhite_gt : checkGrandTotal30(x, s));
        if (x != null)
        {
            if (xvalid_gt)
            {
                calc();
                Response.Redirect("TradeIn_Data.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&confirm=1#content_div_confirm");
            }
        }
    }
	
	protected string validasi_BlackList(string nomor)
	{
		string query = "DECLARE @ERROR VARCHAR(100) = ''; " +
			"IF EXISTS (SELECT 1 FROM StockProductBlackList WHERE PLU = @PLU AND STATUS = 1) " +
			"BEGIN " +
			"   SET @ERROR = 'This product is blacklisted.'; " +
			"END " +
			"SELECT @ERROR AS ErrorMessage;";
		List<SqlParameter> parameters = new List<SqlParameter>
		{
			new SqlParameter("@PLU", SqlDbType.VarChar) { Value = nomor }
		};
		DataTable dt = Rs(query, Db.Cnn_IN, parameters, CommandType.Text);
		return dt.Rows[0][0].ToString();
	}
    //add_plu_Click
    protected void add_plu_Click(object sender, EventArgs e)
    {
        string plu = txt_plu.Text.Trim();
        int item = 0;
        int iditem = 0;
        bool isFound = false;

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO);
        if (d != null)
        {
			string error_blacklist = validasi_BlackList(plu);
			if (string.IsNullOrEmpty(error_blacklist))
			{
				// Search DJ
				if (!isFound)
				{
					Db.Execute("EXEC sp_solve_bug_plu_for_sales_order '" + plu + "','DJ'," + myIDSO, Db.Cnn_DB);
					var rs = from p in db.StockProductDJ
							 where p.Draft == false && p.StatusPenjualan == 0 && p.ReturnProduct == false
							 && p.StockActualDJ.IDLokasi == d.IDLokasi && p.StockActualDJ.TipeLokasi == d.TipeLokasi && p.StockActualDJ.Substorage == 1
							 && !(from z in db.SalesOrderDJ
								  where z.SalesOrder.StatusVoid == false
								  select z.IDProduct).Contains(p.ID)
							 && p.Nomor == plu
							 select p;

					if (rs.Count() > 0)
					{
						item = 1;
						iditem = rs.First().ID;
						isFound = true;
					}
				}

				// Search PG
				if (!isFound)
				{
					Db.Execute("EXEC sp_solve_bug_plu_for_sales_order '" + plu + "','PG'," + myIDSO, Db.Cnn_DB);
					var rs = from p in db.StockProductPG
							 where p.Draft == false && p.StatusPenjualan == 0 && p.StockActualPG.TipeLokasi == d.TipeLokasi
							 && p.StockActualPG.IDLokasi == d.IDLokasi && p.StockActualPG.TipeLokasi == d.TipeLokasi && p.StockActualPG.Substorage == 1
							 && p.CharGoldLevel.NamaKode != "VLEI"
							 && !(from z in db.SalesOrderPG
								  where z.SalesOrder.StatusVoid == false
								  select z.IDProduct).Contains(p.ID)
							 && p.Nomor == plu
							 select p;

					if (rs.Count() > 0)
					{
						item = 2;
						iditem = rs.First().ID;
						isFound = true;
					}
				}

				// Search GJ
				if (!isFound)
				{
					Db.Execute("EXEC sp_solve_bug_plu_for_sales_order '" + plu + "','GJ'," + myIDSO, Db.Cnn_DB);
					var rs = from p in db.StockProductGJ
							 where p.Draft == false && p.StatusPenjualan == 0 && p.ReturnProduct == false
							 && p.StockActualGJ.IDLokasi == d.IDLokasi && p.StockActualGJ.TipeLokasi == d.TipeLokasi && p.StockActualGJ.Substorage == 1
							 && !(from z in db.SalesOrderGJ
								  where z.SalesOrder.StatusVoid == false
								  select z.IDProduct).Contains(p.ID)
							 && p.Nomor == plu
							 select p;

					if (rs.Count() > 0)
					{
						item = 3;
						iditem = rs.First().ID;
						isFound = true;
					}
				}

				// Search LD
				if (!isFound)
				{
					Db.Execute("EXEC sp_solve_bug_plu_for_sales_order '" + plu + "','LD'," + myIDSO, Db.Cnn_DB);
					var rs = from p in db.StockProductLD_Stone1B
							 where p.StatusPenjualan == 0 && p.ReturnProduct == false
							 && p.StockActualLD_Stone1B.IDLokasi == d.IDLokasi && p.StockActualLD_Stone1B.TipeLokasi == d.TipeLokasi && p.StockActualLD_Stone1B.Substorage == 1
							 && !(from z in db.SalesOrderLD
								  where z.SalesOrder.StatusVoid == false
								  select z.IDProduct).Contains(p.ID)
							 && p.Nomor == plu
							 select p;

					if (rs.Count() > 0)
					{
						item = 4;
						iditem = rs.First().ID;
						isFound = true;
					}
					bool isConsignment = false;
					using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
					{
						string consignmentQuery = @"
							SELECT TOP 1 ISNULL(isConsignment, 0) AS isConsignment
							FROM StockProductLD_Stone1B 
							WHERE nomor = @NOMOR";
						using (SqlCommand cmd = new SqlCommand(consignmentQuery, conn))
						{
							cmd.Parameters.Add(new SqlParameter("@NOMOR", SqlDbType.VarChar) { Value = plu });

							conn.Open();

							object consResult = cmd.ExecuteScalar();
							if (consResult != null && consResult != DBNull.Value)
							{
								isConsignment = Convert.ToBoolean(consResult);
							}
						}
					}

					if (isConsignment)
					{
						lblwarningplu.Text = "PLU LD Consignment, Tidak Dapat Dijual. Harap hubungi Pusat.";
						return;
					}
				}
				lblwarningplu.Text = "";
				
				if (isFound)
				{
					Response.Redirect("Sales_ItemAddSave.aspx?id=" + myID + "&item=" + item + "&iditem=" + iditem + "&so=" + myIDSO + "&ro=" + myIDRO);
				}
				else
				{
					lblwarningplu.Text = "PLU tidak ditemukan di Store Ini.";
				}
			}
			else
			{
				lblwarningplu.Text = "PLU Tidak dapat Dijual/Trade In. Silakan lakukan pemesanan melalui Pre Order (PO)";
			}
		}
	}
    //update_keterangan_Click
    protected void update_keterangan_Click(object sender, EventArgs e)
    {
        //Response.Redirect("TradeIn_Data.aspx?id=" + myIDSO + "&oweifjowfj=234234");
        //Response.Write("asdasdad");

        var x = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO && p.Draft);
        if (x == null) DataError();
        {
            x.Keterangan = keterangan.Text;
            db.SubmitChanges();
            Response.Redirect("TradeIn_Data.aspx" + hrefpattern);
        }
    }

    //Cancel
    protected void cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("TradeIn_Data.aspx" + hrefpattern);
    }

    //Redirect Document Sales Order
    protected void redirect_payment_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO && !p.Draft); // no-draft !!!
        if (d == null) { DataError(); }
        else
        {
            Response.Redirect("/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?id=" + d.ID);
        }
    }

    //Redirect Document Register
    protected void redirect_register_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO && !p.Draft); // no-draft !!!
        if (d == null) { DataError(); }
        else
        {
            Response.Redirect("/OutletSales/Page/Create/" + hrefmainfull);
        }
    }
    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        string status_SAP = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO && p.Draft);
        var t = db.TradeIn.SingleOrDefault(p => p.ID == myID && p.Draft);
        var s = db.Resell.SingleOrDefault(p => p.ID == myIDRO && p.Draft);

        if (d == null || t == null || s == null) { DataError(); }
        else
        {
            ////////////////////////////////////////////// Generate Document

            // Generate Document Number --> Sales Order
            if (d.Nomor == null || d.Nomor == "")
            {
                d.Nomor = Document.Generate_Nomor_SalesOrder(d);
                db.SubmitChanges();
                List<SqlParameter> paramsa = new List<SqlParameter>
                {
                    new SqlParameter("@IDSO", SqlDbType.Int){ Value = myIDSO }
                };

                Execute("sp_solve_duplicate_so_number", Db.Cnn_DB, paramsa, CommandType.StoredProcedure);
            }

            // Generate Document Number --> Resell
            if (s.Nomor == null || s.Nomor == "")
            {
                s.Nomor = Document.Generate_Nomor_Resell(s);
                db.SubmitChanges();
            }

            // Generate Document Number --> Tradein
            if (t.Nomor == null || t.Nomor == "")
            {
                t.Nomor = Document.Generate_Nomor_TradeIn(t);
                db.SubmitChanges();
            }

            // Validation and SAP Posting
            try
            {
                if (SAP.xActivate)
                {
                    status_SAP += ((status_SAP == "") ? SAPM.prePostSalesOrderIntegration(d) : "");
                    //status_SAP += ((status_SAP == "") ? SAPM.prePostResellIntegration(s) : "");

                    status_SAP += ((status_SAP == "") ? SAPM.isValidLocationSAP(d) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.postCustomerToSAP(d) : "");

                    status_SAP += ((status_SAP == "") ? SAPM.postSalesGoodsToSAP(d, true) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.postResellGoodsToSAP(s) : "");
                }
            }
            catch (Exception ex)
            {
                status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (Error message : "
                    + ex.Message
                    //+ ex.StackTrace 
                    //+ ex.InnerException 
                    + ")";
            }

            status_SAP = ""; // for dev only, disable enable SAP validation
            if (status_SAP == "")
            {
                string query = "sp_SaveTradeIn";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ID", SqlDbType.Int){ Value = myIDSO },
                    new SqlParameter("@KETERANGAN", SqlDbType.VarChar,1000){ Value = keterangan.Text }
                };
                DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
                bool success = Convert.ToBoolean(dt.Rows[0]["SUCCESS"]);
                string error = dt.Rows[0]["ERROR"].ToString();

                if (success)
                {
                    // Go to Payment
                    Response.Redirect("/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?d=1&id=" + d.ID);
                }
                else
                {
                    lit_error_sap.Text += "<div class='alert alert-danger' role='alert'>" + error + "</div>";
                }
            }
            else
            {
                lit_error_sap.Text += "<div class='alert alert-danger' role='alert'>" + status_SAP + "</div>";
            }
        }
    }

    //Save Old
    protected void save_ClickOld(object sender, EventArgs e)
    {
        string status_SAP = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO && p.Draft);
        var t = db.TradeIn.SingleOrDefault(p => p.ID == myID && p.Draft);
        var s = db.Resell.SingleOrDefault(p => p.ID == myIDRO && p.Draft);

        if (d == null || t == null || s == null) { DataError(); }
        else
        {
            ////////////////////////////////////////////// Generate Document

            // Generate Document Number --> Sales Order
            if (d.Nomor == null || d.Nomor == "")
            {
                d.Nomor = Document.Generate_Nomor_SalesOrder(d);
                db.SubmitChanges();

                Db.Execute("EXEC sp_solve_duplicate_so_number " + myIDSO, Db.Cnn_DB);
            }

            // Generate Document Number --> Resell
            if (s.Nomor == null || s.Nomor == "")
            {
                s.Nomor = Document.Generate_Nomor_Resell(s);
                db.SubmitChanges();
            }

            // Generate Document Number --> Tradein
            if (t.Nomor == null || t.Nomor == "")
            {
                t.Nomor = Document.Generate_Nomor_TradeIn(t);
                db.SubmitChanges();
            }


            // Validation and SAP Posting
            try
            {
                if (SAP.xActivate)
                {
                    status_SAP += ((status_SAP == "") ? SAPM.prePostSalesOrderIntegration(d) : "");
                    //status_SAP += ((status_SAP == "") ? SAPM.prePostResellIntegration(s) : "");

                    status_SAP += ((status_SAP == "") ? SAPM.isValidLocationSAP(d) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.postCustomerToSAP(d) : "");

                    status_SAP += ((status_SAP == "") ? SAPM.postSalesGoodsToSAP(d, true) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.postResellGoodsToSAP(s) : "");
                }
            }
            catch (Exception ex)
            {
                status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (Error message : "
                    + ex.Message
                    //+ ex.StackTrace 
                    //+ ex.InnerException 
                    + ")";
            }

            status_SAP = ""; // for dev only, disable enable SAP validation
            if (status_SAP == "")
            {

                ////////////////////////////////////////////// Calculate Process

                decimal TotalSales = 0m;
                decimal TotalResell = 0m;

                TotalResell = s.ResellDJ.Sum(p => p.HargaRupiah.Value) + s.ResellPG.Sum(p => p.HargaRupiah.Value) + s.ResellGJ.Sum(p => p.HargaRupiah.Value) + s.ResellLD.Sum(p => p.HargaRupiah.Value);
                TotalSales = d.SalesOrderDJ.Sum(p => p.TotalBayar.Value) + d.SalesOrderPG.Sum(p => p.TotalBayar.Value) + d.SalesOrderGJ.Sum(p => p.TotalBayar.Value) + d.SalesOrderLD.Sum(p => p.TotalBayar.Value);

                decimal xModalRupiah = d.SalesOrderDJ.Sum(p => p.ModalRupiah.Value) + d.SalesOrderPG.Sum(p => p.ModalRupiah.Value) + d.SalesOrderGJ.Sum(p => p.ModalRupiah.Value) + d.SalesOrderLD.Sum(p => p.ModalRupiah.Value);
                decimal xHargaRupiah = d.SalesOrderDJ.Sum(p => p.HargaRupiah.Value) + d.SalesOrderPG.Sum(p => p.HargaRupiah.Value) + d.SalesOrderGJ.Sum(p => p.HargaRupiah.Value) + d.SalesOrderLD.Sum(p => p.HargaRupiah.Value);
                decimal xTotalBayarBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalBayar.Value) + d.SalesOrderPG.Sum(p => p.TotalBayar.Value) + d.SalesOrderGJ.Sum(p => p.TotalBayar.Value) + d.SalesOrderLD.Sum(p => p.TotalBayar.Value)) - (s.ResellDJ.Sum(p => p.HargaRupiah.Value) + s.ResellPG.Sum(p => p.HargaRupiah.Value) + s.ResellGJ.Sum(p => p.HargaRupiah.Value) + s.ResellLD.Sum(p => p.HargaRupiah.Value));
                decimal xTotalRupiahBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalRupiah.Value) + d.SalesOrderPG.Sum(p => p.TotalRupiah.Value) + d.SalesOrderGJ.Sum(p => p.TotalRupiah.Value) + d.SalesOrderLD.Sum(p => p.TotalRupiah.Value));

                ////////////////////////////////////////////// Calculate All Discount

                var inv_dj = d.SalesOrderDJ;
                var inv_pg = d.SalesOrderPG;
                var inv_gj = d.SalesOrderGJ;
                var inv_ld = d.SalesOrderLD;

                decimal totalDiscNormal = 0m;
                decimal totalDiscPromo = 0m;
                decimal totalDiscBank = 0m;
                decimal totalDiscMember = 0m;
                decimal totalDiscVoucher = 0m;
                decimal totalDiscOther = 0m;
                decimal totalDiscRound = 0m;
                decimal grandTotal = 0m;

                if (inv_dj.Count() > 0)
                {
                    totalDiscNormal += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountNominal));
                    totalDiscPromo += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountPromo));
                    totalDiscBank += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountBank));
                    totalDiscMember += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountProgramNominal));
                    totalDiscVoucher += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountGift));
                    totalDiscOther += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountOther));
                    totalDiscRound += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountRound));
                }
                if (inv_pg.Count() > 0)
                {
                    totalDiscNormal += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountNominal));
                    totalDiscPromo += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountPromo));
                    totalDiscBank += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountBank));
                    totalDiscMember += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountProgramNominal));
                    totalDiscVoucher += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountGift));
                    totalDiscOther += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountOther));
                    totalDiscRound += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountRound));
                }
                if (inv_gj.Count() > 0)
                {
                    totalDiscNormal += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountNominal));
                    totalDiscPromo += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountPromo));
                    totalDiscBank += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountBank));
                    totalDiscMember += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountProgramNominal));
                    totalDiscVoucher += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountGift));
                    totalDiscOther += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountOther));
                    totalDiscRound += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountRound));
                }
                if (inv_ld.Count() > 0)
                {
                    totalDiscNormal += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountNominal));
                    totalDiscPromo += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountPromo));
                    totalDiscBank += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountBank));
                    totalDiscMember += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountProgramNominal));
                    totalDiscVoucher += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountGift));
                    totalDiscOther += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountOther));
                    totalDiscRound += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountRound));
                }

                grandTotal = totalDiscNormal + totalDiscPromo + totalDiscBank + totalDiscMember + totalDiscVoucher + totalDiscOther + totalDiscRound;

                ////////////////////////////////////////////// Update Sales Order

                d.ModalRupiah = xModalRupiah;
                d.HargaRupiah = xHargaRupiah;
                d.TotalBayarBeforeDisc = xTotalBayarBeforeDisc;
                d.TotalRupiahBeforeDisc = xTotalRupiahBeforeDisc;
                d.TotalResell = TotalResell;
                d.Discount = 0m;
                d.DiscountNominal = totalDiscNormal;
                d.DiscountGift = totalDiscVoucher;
                d.DiscountProgram = 0m;
                d.DiscountProgramNominal = totalDiscMember;
                d.DiscountRound = totalDiscRound;
                d.TotalBayar = TotalSales;//(xHargaRupiah - grandTotal);
                d.TotalRupiah = xHargaRupiah;
                d.Unpaid = (TotalSales - TotalResell);
                d.Paid = TotalResell;
                d.Draft = false;
                d.DraftDate = DateTime.Now;
                db.SubmitChanges();

                ////////////////////////////////////////////// Update Resell

                s.Draft = false;
                s.DraftDate = null;
                decimal TotalHarga = 0m;
                TotalHarga = s.ResellDJ.Sum(p => p.HargaRupiah.Value) + s.ResellPG.Sum(p => p.HargaRupiah.Value) + s.ResellGJ.Sum(p => p.HargaRupiah.Value) + s.ResellLD.Sum(p => p.HargaRupiah.Value);

                s.TotalHarga = TotalHarga;
                db.SubmitChanges();

                ////////////////////////////////////////////// Payment Exchange Process

                SalesReceipt sr = new SalesReceipt();

                sr.IDSalesOrder = d.ID;
                sr.NoSalesOrder = d.Nomor;
                sr.OperatorNama = App.UserNama;
                sr.OperatorTgl = DateTime.Now;
                sr.TotalBayar = TotalResell;

                db.SalesReceipt.InsertOnSubmit(sr);
                db.SubmitChanges();

                int IDSalesReceipt = sr.ID;

                SalesReceiptDetail sd = new SalesReceiptDetail();

                sd.IDSalesReceipt = IDSalesReceipt;
                sd.IDPaymentType = 6;

                sd.Nominal = TotalResell;
                sd.MDR = 0m;
                sd.OperatorNama = App.UserNama;
                sd.OperatorTgl = DateTime.Now;

                db.SalesReceiptDetail.InsertOnSubmit(sd);
                db.SubmitChanges();

                ////////////////////////////////////////////// Update Trade In

                t.Draft = false;
                t.DraftDate = null;
                db.SubmitChanges();

                ////////////////////////////////////////////// Buyback Process Ledger

                foreach (var r in s.ResellDJ)
                {
                    var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                    if (x != null)
                    {
                        x.StockProductDJ.StockActualDJ.Substorage = 3;
                        x.StockProductDJ.StockActualDJ.InHand = 1;
                        x.StatusResell = 1;
                        //x.StockProductDJ.StockActualDJ.TipeLokasi = r.Resell.TipeLokasi;
                        //x.StockProductDJ.StockActualDJ.IDLokasi = r.Resell.IDLokasi;
                        x.StockProductDJ.StockActualDJ.TipeLokasi = r.Resell.TipeLokasi;
                        x.StockProductDJ.StockActualDJ.IDLokasi = r.Resell.IDLokasi;
                        db.SubmitChanges();

                        // Ledger 
                        StockLedgerDJ l = new StockLedgerDJ();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = 1; // 1 : in , -1 : out
                        int Remark = 13; // See Library DB - Ledger
                        string Keterangan = x.StockProductDJ.Nomor; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (s.TipeLokasi == 1)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + s.IDLokasi, Db.Cnn_DB);
                        else if (s.TipeLokasi == 2)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + s.IDLokasi, Db.Cnn_DB);

                        //Input
                        l.IDProduct = r.IDProduct;
                        l.TipeLokasi = s.TipeLokasi;
                        l.IDLokasi = s.IDLokasi;
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
                foreach (var r in s.ResellPG)
                {
                    var x = db.SalesOrderPG.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                    if (x != null)
                    {
                        x.StockProductPG.StockActualPG.Substorage = 3;
                        x.StockProductPG.StockActualPG.InHand = 1;
                        x.StatusResell = 1;
                        x.StockProductPG.StockActualPG.TipeLokasi = r.Resell.TipeLokasi;
                        x.StockProductPG.StockActualPG.IDLokasi = r.Resell.IDLokasi;
                        db.SubmitChanges();

                        // Ledger 
                        StockLedgerPG l = new StockLedgerPG();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = 1; // 1 : in , -1 : out
                        int Remark = 13; // See Library DB - Ledger
                        string Keterangan = x.StockProductPG.Nomor; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (s.TipeLokasi == 1)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                        else if (s.TipeLokasi == 2)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                        //Input
                        l.IDProduct = r.IDProduct;
                        l.TipeLokasi = s.TipeLokasi;
                        l.IDLokasi = s.IDLokasi;
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

                        int Consignment = 0;
                        Consignment = Db.SingleInteger("SELECT ISNULL(CAST(Consignment AS INT),0) Consignemtn from StockProductPG WHERE ID=" + r.IDProduct, Db.Cnn_DB);

                        if (Consignment == 1)
                        {
                            string query = "UPDATE StockProductPG SET Consignment = @consignment WHERE ID = @idproduct";
                            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                            {
                                // Adding parameters with values                                                            
                                SqlCommand cmd = conn.CreateCommand();
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.CommandText = query;
                                cmd.Parameters.AddWithValue("@consignment", 0);
                                cmd.Parameters.AddWithValue("@idproduct", r.IDProduct);
								
                                StockLedgerPG m = new StockLedgerPG();

                                m.IDProduct = r.IDProduct;
                                m.TipeLokasi = s.TipeLokasi;
                                m.IDLokasi = s.IDLokasi;
                                m.NamaLokasi = Lokasi;
                                m.Value = 0;
                                m.Remark = 22;
                                m.Keterangan = x.StockProductPG.Nomor;
                                //User-Info
                                m.Operator = App.UserNama;
                                m.OperatorTgl = DateTime.Now;
                                //Commit
                                db.StockLedgerPG.InsertOnSubmit(m);
                                db.SubmitChanges();
                                try
                                {
                                    conn.Open();
                                    cmd.ExecuteNonQuery();
									conn.Close();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error: " + ex.Message);
                                }
                            }
                        }
                    }
                }
                foreach (var r in s.ResellGJ)
                {
                    var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                    if (x != null)
                    {
                        x.StockProductGJ.StockActualGJ.Substorage = 3;
                        x.StockProductGJ.StockActualGJ.InHand = 1;
                        x.StatusResell = 1;
                        x.StockProductGJ.StockActualGJ.TipeLokasi = r.Resell.TipeLokasi;
                        x.StockProductGJ.StockActualGJ.IDLokasi = r.Resell.IDLokasi;
                        db.SubmitChanges();

                        // Ledger 
                        StockLedgerGJ l = new StockLedgerGJ();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = 1; // 1 : in , -1 : out
                        int Remark = 13; // See Library DB - Ledger
                        string Keterangan = x.StockProductGJ.Nomor; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (s.TipeLokasi == 1)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + s.IDLokasi, Db.Cnn_DB);
                        else if (s.TipeLokasi == 2)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + s.IDLokasi, Db.Cnn_DB);

                        //Input
                        l.IDProduct = r.IDProduct;
                        l.TipeLokasi = s.TipeLokasi;
                        l.IDLokasi = s.IDLokasi;
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
                foreach (var r in s.ResellLD)
                {
                    var x = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                    if (x != null)
                    {
                        x.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 3;
                        x.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand = 1;
                        x.StatusResell = 1;
                        x.StockProductLD_Stone1B.StockActualLD_Stone1B.TipeLokasi = r.Resell.TipeLokasi;
                        x.StockProductLD_Stone1B.StockActualLD_Stone1B.IDLokasi = r.Resell.IDLokasi;
                        db.SubmitChanges();

                        // Ledger 
                        StockLedgerLD l = new StockLedgerLD();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = 1; // 1 : in , -1 : out
                        int Remark = 13; // See Library DB - Ledger
                        string Keterangan = x.StockProductLD_Stone1B.Nomor; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (s.TipeLokasi == 1)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + s.IDLokasi, Db.Cnn_DB);
                        else if (s.TipeLokasi == 2)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + s.IDLokasi, Db.Cnn_DB);

                        //Input
                        l.IDProduct = r.IDProduct;
                        l.TipeLokasi = s.TipeLokasi;
                        l.IDLokasi = s.IDLokasi;
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

                ////////////////////////////////////////////// Sales Order Process

                // rubah status pembayaran = 1 jika barang semua harga 0
                if (d.Unpaid == 0) { d.StatusPembayaran = true; db.SubmitChanges(); }

                decimal dModalRupiah = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.ModalRupiah) + d.SalesOrderPG.Sum(p => p.ModalRupiah) + d.SalesOrderGJ.Sum(p => p.ModalRupiah) + d.SalesOrderLD.Sum(p => p.ModalRupiah));
                decimal dHargaRupiah = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.HargaRupiah) + d.SalesOrderPG.Sum(p => p.HargaRupiah) + d.SalesOrderGJ.Sum(p => p.HargaRupiah) + d.SalesOrderLD.Sum(p => p.HargaRupiah));
                decimal dTotalBayar = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.TotalBayar) + d.SalesOrderPG.Sum(p => p.TotalBayar) + d.SalesOrderGJ.Sum(p => p.TotalBayar) + d.SalesOrderLD.Sum(p => p.TotalBayar));
                decimal dTotalRupiah = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.TotalRupiah) + d.SalesOrderPG.Sum(p => p.TotalRupiah) + d.SalesOrderGJ.Sum(p => p.TotalRupiah) + d.SalesOrderLD.Sum(p => p.TotalRupiah));

                decimal dDiscNormal = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.DiscountNominal) + d.SalesOrderPG.Sum(p => p.DiscountNominal) + d.SalesOrderGJ.Sum(p => p.DiscountNominal) + d.SalesOrderLD.Sum(p => p.DiscountNominal));
                decimal dDiscPromo = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.DiscountPromo) + d.SalesOrderPG.Sum(p => p.DiscountPromo) + d.SalesOrderGJ.Sum(p => p.DiscountPromo) + d.SalesOrderLD.Sum(p => p.DiscountPromo));
                decimal dDiscBank = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.DiscountBank) + d.SalesOrderPG.Sum(p => p.DiscountBank) + d.SalesOrderGJ.Sum(p => p.DiscountBank) + d.SalesOrderLD.Sum(p => p.DiscountBank));
                decimal dDiscProgram = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.DiscountProgramNominal) + d.SalesOrderPG.Sum(p => p.DiscountProgramNominal) + d.SalesOrderGJ.Sum(p => p.DiscountProgramNominal) + d.SalesOrderLD.Sum(p => p.DiscountProgramNominal));
                decimal dDiscGift = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.DiscountGift) + d.SalesOrderPG.Sum(p => p.DiscountGift) + d.SalesOrderGJ.Sum(p => p.DiscountGift) + d.SalesOrderLD.Sum(p => p.DiscountGift));
                decimal dDiscRound = Convert.ToDecimal(d.SalesOrderDJ.Sum(p => p.DiscountRound) + d.SalesOrderPG.Sum(p => p.DiscountRound) + d.SalesOrderGJ.Sum(p => p.DiscountRound) + d.SalesOrderLD.Sum(p => p.DiscountRound));

                var loc_o = db.LocOutlet;
                var loc_e = db.LocExhibition;

                if (d.SalesOrderDJ.Count() > 0)
                {
                    foreach (var r in d.SalesOrderDJ)
                    {
                        // Ledger 
                        StockLedgerDJ l = new StockLedgerDJ();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = -1; // 1 : in , -1 : out
                        int Remark = 8; // See Library DB - Ledger
                        string Keterangan = r.Invoice; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (d.TipeLokasi == 1) { Lokasi = loc_o.Single(p => p.ID == d.IDLokasi).Nama; }
                        else if (d.TipeLokasi == 2) { Lokasi = loc_e.Single(p => p.ID == d.IDLokasi).Nama; }

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

                        // Update StockActual from 1 to 0
                        var stkact = r.StockProductDJ.StockActualDJ;
                        stkact.InHand = 0;
                        db.SubmitChanges();
                        Db.Execute("EXEC sp_update_ppn_new " + r.ID + " ,'DJ'", Db.Cnn_DB);
                    }
                }
                if (d.SalesOrderPG.Count() > 0)
                {
                    foreach (var r in d.SalesOrderPG)
                    {
                        // Ledger 
                        StockLedgerPG l = new StockLedgerPG();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = -1; // 1 : in , -1 : out
                        int Remark = 8; // See Library DB - Ledger
                        string Keterangan = r.Invoice; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (d.TipeLokasi == 1) { Lokasi = loc_o.Single(p => p.ID == d.IDLokasi).Nama; }
                        else if (d.TipeLokasi == 2) { Lokasi = loc_e.Single(p => p.ID == d.IDLokasi).Nama; }

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

                        // Update StockActual from 1 to 0
                        var stkact = r.StockProductPG.StockActualPG;
                        stkact.InHand = 0;
                        db.SubmitChanges();
                        Db.Execute("EXEC sp_update_ppn_new " + r.ID + " ,'PG'", Db.Cnn_DB);
                    }
                }
                if (d.SalesOrderGJ.Count() > 0)
                {
                    foreach (var r in d.SalesOrderGJ)
                    {
                        // Ledger 
                        StockLedgerGJ l = new StockLedgerGJ();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = -1; // 1 : in , -1 : out
                        int Remark = 8; // See Library DB - Ledger
                        string Keterangan = r.Invoice; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (d.TipeLokasi == 1) { Lokasi = loc_o.Single(p => p.ID == d.IDLokasi).Nama; }
                        else if (d.TipeLokasi == 2) { Lokasi = loc_e.Single(p => p.ID == d.IDLokasi).Nama; }

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

                        // Update StockActual from 1 to 0
                        var stkact = r.StockProductGJ.StockActualGJ;
                        stkact.InHand = 0;
                        db.SubmitChanges();
                        Db.Execute("EXEC sp_update_ppn_new " + r.ID + " ,'GJ'", Db.Cnn_DB);
                    }
                }
                if (d.SalesOrderLD.Count() > 0)
                {
                    foreach (var r in d.SalesOrderLD)
                    {
                        // Ledger 
                        StockLedgerLD l = new StockLedgerLD();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = -1; // 1 : in , -1 : out
                        int Remark = 8; // See Library DB - Ledger
                        string Keterangan = r.Invoice; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (d.TipeLokasi == 1) { Lokasi = loc_o.Single(p => p.ID == d.IDLokasi).Nama; }
                        else if (d.TipeLokasi == 2) { Lokasi = loc_e.Single(p => p.ID == d.IDLokasi).Nama; }

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

                        // Update StockActual from 1 to 0
                        var stkact = r.StockProductLD_Stone1B.StockActualLD_Stone1B;
                        stkact.InHand = 0;
                        db.SubmitChanges();
                        Db.Execute("EXEC sp_update_ppn_new " + r.ID + " ,'LD'", Db.Cnn_DB);
                    }
                }
                if (d.SalesOrderPackaging.Count() > 0)
                {
                    foreach (var r in d.SalesOrderPackaging)
                    {
                        // Update Stock Qty (minus) 
                        var x = db.StockActualPackaging.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == r.SalesOrder.TipeLokasi && p.IDLokasi == r.SalesOrder.IDLokasi);

                        //Input
                        x.InHand = x.InHand - r.Qty.Value;

                        //Commit
                        db.SubmitChanges();
                    }
                }
                if (d.SalesOrderSouvenir.Count() > 0)
                {
                    foreach (var r in d.SalesOrderSouvenir)
                    {
                        // Update Stock Qty (minus) 
                        var x = db.StockActualSouvenir.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == r.SalesOrder.TipeLokasi && p.IDLokasi == r.SalesOrder.IDLokasi);

                        //Input
                        x.InHand = x.InHand - r.Qty.Value;

                        //Commit
                        db.SubmitChanges();
                    }
                }

                string SalesOrderNumber = d.Nomor;

                ////////////////////////////////////////////// Update Voucher

                var u = from p in db.SalesVoucher where p.IDSalesOrder == myIDSO select p;
                if (u.Count() > 0)
                {
                    int TipeLokasi;
                    int IDLokasi;
                    string NamaStore = "";

                    TipeLokasi = Convert.ToInt32(d.TipeLokasi);
                    IDLokasi = Convert.ToInt32(d.IDLokasi);

                    foreach (var data in u)
                    {
                        string NoVoucher = data.NomorVoucher;
                        var y = cs.T_voucher.SingleOrDefault(p => p.M_nomor == NoVoucher && (p.M_klaimdoc == null || p.M_klaimdoc == ""));
                        if (y == null) { next_err_2.Visible = true; div_err.Visible = true; }
                        else
                        {
                            if (TipeLokasi == 1)
                            {
                                var l = db.LocOutlet.SingleOrDefault(p => p.ID == IDLokasi);
                                NamaStore = l.Kode;
                            }
                            else
                            {
                                var l = db.LocExhibition.SingleOrDefault(p => p.ID == IDLokasi);
                                NamaStore = l.Kode;
                            }

                            y.M_store2 = NamaStore;
                            y.M_klaimdoc = SalesOrderNumber;
                            y.M_tanggal2 = DateTime.Now;

                            cs.SubmitChanges();
                        }

                    }
                }

                // Go to Payment
                Response.Redirect("/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?d=1&id=" + d.ID);
            }
            else
            {
                lit_error_sap.Text += "<div class='alert alert-danger' role='alert'>" + status_SAP + "</div>";
            }

        }

    }

    protected void Discount_TextChanged(object sender, EventArgs e)
    {
        calc();
        string href = App.RefreshURL(this);
        //Page-Qstr
        href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
        href += "&tb=" + ((TextBox)sender).ClientID;
        Response.Redirect(href);

        //fillForm();
        //upd_form.Update();
    }

    protected void DropDownListPromoBank_Changed(object sender, EventArgs e)
    {
        calc();
        string href = App.RefreshURL(this);
        //Page-Qstr
        href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
        href += "&tb=" + ((DropDownList)sender).ClientID;
        Response.Redirect(href);
    }
    protected void calc()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO && p.Draft);
        var c = cs.Mscustomer.SingleOrDefault(p => p.M_kode == d.DataCustomer.NoCustomer);
        var m = cs.Msmembership.SingleOrDefault(p => p.M_kode == c.M_typemember);

        decimal disc1 = 0m;
        decimal disc2 = 0m;

        if (STAMPS.xActivate)
        {
            List<decimal> Discounts = fillMembershipStamps(d, ref disc1, ref disc2);
            disc1 = Discounts[0]; disc2 = Discounts[1];
        }

        calcDJ(c, m, disc1, disc2);
        calcPG(c, m, disc1, disc2);
        calcGJ(c, m, disc1, disc2);
        calcLD(c, m, disc1, disc2);

        if (d == null) DataError();
        else
        {
            d.Keterangan = keterangan.Text;
            savePackaging(d);
            saveSouvenir(d);
            d.TotalBayarBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalBayar) + d.SalesOrderPG.Sum(p => p.TotalBayar) + d.SalesOrderGJ.Sum(p => p.TotalBayar) + d.SalesOrderLD.Sum(p => p.TotalBayar));
            d.TotalRupiahBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalRupiah) + d.SalesOrderPG.Sum(p => p.TotalRupiah) + d.SalesOrderGJ.Sum(p => p.TotalRupiah) + d.SalesOrderLD.Sum(p => p.TotalRupiah));
            db.SubmitChanges();
        }
    }

    protected void savePackaging(SalesOrder d)
    {
        TextBox txt = new TextBox();
        string hf = "";
        int index = 1;

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfpc" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("pc_" + index.ToString());

                var s = db.SalesOrderPackaging.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (s != null)
                {
                    //var x = db.StockActualPackaging.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == s.IDProduct);
                    //if (x == null) DataError();
                    //else
                    //{
                    //    x.InHand = x.InHand + s.Qty.Value;
                    //    db.SubmitChanges();
                    //}

                    decimal QtyInput = Convert.ToDecimal(txt.Text);
                    decimal Modal = Db.SingleDecimal("SELECT ISNULL(Harga, 0) FROM Packaging WHERE ID = " + s.IDProduct, Db.Cnn_DB);

                    s.Qty = QtyInput;
                    s.ModalRupiah = Modal;
                    s.TotalModalRupiah = QtyInput * Modal;
                    //x.InHand = x.InHand - Convert.ToDecimal(txt.Text);
                    db.SubmitChanges();
                }
            }
            catch (Exception) { }
            index++;
        }
    }

    protected void saveSouvenir(SalesOrder d)
    {
        TextBox txt = new TextBox();
        string hf = "";
        int index = 1;

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfsv" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("sv_" + index.ToString());

                var s = db.SalesOrderSouvenir.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (s != null)
                {
                    //var x = db.StockActualSouvenir.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == s.IDProduct && p.InHand > 0);
                    //if (x == null) DataError();
                    //else
                    //{
                    //    x.InHand = x.InHand + s.Qty.Value;
                    //    db.SubmitChanges();
                    //}

                    decimal QtyInput = Convert.ToDecimal(txt.Text);
                    decimal Modal = Db.SingleDecimal("SELECT ISNULL(Harga, 0) FROM Souvenir WHERE ID = " + s.IDProduct, Db.Cnn_DB);

                    s.Qty = QtyInput;
                    s.ModalRupiah = Modal;
                    s.TotalModalRupiah = QtyInput * Modal;
                    //x.InHand = x.InHand - Convert.ToDecimal(txt.Text);
                    db.SubmitChanges();
                }
            }
            catch (Exception) { }
            index++;
        }
    }

    protected void calcDJ(Mscustomer c, Msmembership m, decimal disc1, decimal disc2)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        TextBox txt3 = new TextBox();
        TextBox txt4 = new TextBox();
        TextBox txt5 = new TextBox();
        DropDownList ddl2 = new DropDownList();
        TextBox txt6 = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            decimal TotalRupiah = 0m;
            decimal TotalBayar = 0m;

            decimal DiscountNormalNominal = 0;
            decimal DiscountNormalPercent = 0;
            decimal DiscountPromoNominal = 0;
            decimal DiscountPromoPercent = 0;
            decimal DiscountBankNominal = 0;
            decimal DiscountMemberNominal = 0;
            decimal DiscountMemberPercent = 0;
            decimal DiscountVoucherNominal = 0;
            decimal DiscountOtherNominal = 0;
            decimal DiscountRoundNominal = 0;
            decimal TotalFeeExtraRingSize = 0;
            int ExtraRingSize = 0;
            int FeeExtraRingSize = 0;
            int TipePromo = 0;
            decimal HargaPromo = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("djhf1_" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("djdn2_" + index.ToString());
                txt2 = (TextBox)row.FindControl("djdb1_" + index.ToString());
                txt3 = (TextBox)row.FindControl("djdv1_" + index.ToString());
                txt4 = (TextBox)row.FindControl("djdo1_" + index.ToString());
                txt5 = (TextBox)row.FindControl("djdr1_" + index.ToString());
                ddl2 = (DropDownList)row.FindControl("djdb1ddl_" + index.ToString());
                txt6 = (TextBox)row.FindControl("djtfrs_" + index.ToString());

                var d = db.SalesOrderDJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        DiscountNormalPercent = Convert.ToDecimal(txt.Text);
                        if (DiscountNormalPercent > 10m) { DiscountNormalPercent = 10m; }
                        else if (DiscountNormalPercent < 0m) { DiscountNormalPercent = 0m; }
                    }
                    catch (Exception) { }
                    try
                    {
                        DiscountBankNominal = Convert.ToDecimal(txt2.Text);
                        if (DiscountBankNominal < 0m) { DiscountBankNominal = 0m; }
                        int idpromo = Convert.ToInt32(ddl2.SelectedValue);
                        if (idpromo > 0)
                        {
                            decimal discount = Db.SingleDecimal("SELECT TOP 1 ISNULL(Discount,0) [Discount] FROM PromoDetail WHERE ID = " + idpromo, Db.Cnn_DB);
                            DiscountBankNominal = discount;

                            Db.Execute("UPDATE SalesOrderDJ SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
                        }
                        else if (idpromo == 0)
                        {
                            DiscountBankNominal = 0m;
                            Db.Execute("UPDATE SalesOrderDJ SET IDPromoDetail = NULL WHERE ID = " + hf, Db.Cnn_DB);
                        }
                        else if (idpromo == -1)
                        {
                            Db.Execute("UPDATE SalesOrderDJ SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
                        }
                    }
                    catch (Exception) { }

                    try
                    {
                        DiscountVoucherNominal = Convert.ToDecimal(txt3.Text);
                    }
                    catch (Exception) { }

                    try
                    {
                        DiscountOtherNominal = Convert.ToDecimal(txt4.Text);
                        //if (DiscountOtherNominal < 0m) { DiscountOtherNominal = 0m; }
                    }
                    catch (Exception) { }

                    TotalRupiah = (Math.Floor((d.HargaRupiah.HasValue ? d.HargaRupiah.Value : 0m) / 1000)) * 1000;

                    DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    DiscountPromoPercent = 0m;

                    DiscountNormalNominal = (Math.Floor(((TotalRupiah - DiscountPromoNominal) * (DiscountNormalPercent / 100)) / 1000)) * 1000;

                    if (d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SETTING") { DiscountNormalNominal = 0; }

                    //DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    //DiscountPromoPercent = 0m;


                    bool statusNoPromoMembership = false;

                    // promo
                    var t = from p in db.PromoDJ_Detail
                            where p.PromoDJ.ApprovalNama != null && p.PromoDJ.TglStart.Value <= DateTime.Today
                            && p.PromoDJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == d.IDProduct
                            && p.PromoDJ.Draft == false
                            select p;
                    if (t.Count() > 0)
                    {
                        var disc = t.First();
                        var discP = disc.PromoDJ;
                        statusNoPromoMembership = (discP.StatusDiscMembership != null) ? discP.StatusDiscMembership.Value : false;

                        ExtraRingSize = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(ExtraRingSize,0) AS INT) [ExtraRingSize] FROM PromoDJ WHERE ID = " + discP.ID, Db.Cnn_DB);

                        FeeExtraRingSize = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(Harga,0) AS INT) [Harga] FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0", Db.Cnn_DB);

                        //lit_test.Text += "<br />______________________calcDJ-0_" + discP.StatusDiscMembership.Value.ToString();
                    }

                    //if (d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SETTING") { statusNoPromoMembership = true; }
                    // berdasarkan meeting AE, pg dan gj tidak ada diskon membership
                    if ((d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SETTING") /*||(d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.NamaKode == "M" && d.StockProductDJ.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode == "PT")*/ || (d.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.NamaKode == "MTG" && !d.StockProductDJ.StockProductDJ_Stone1B.Any() && d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode != "SNI"))
                    { statusNoPromoMembership = true; }
                    if (statusNoPromoMembership)
                    {
                        DiscountMemberPercent = 0m;
                        DiscountMemberNominal = 0m;
                    }
                    else
                    {
                        bool branded = Pricing.isDiamondBranded(d.StockProductDJ.Nomor);
                        bool isGeneric = Pricing.isDiamondGeneric(d.StockProductDJ.Nomor);
						if(branded == false)
							branded = isFancyCertifiedDJ(d.StockProductDJ.ID);
                        bool isDossierNonBranded = (d.StockProductDJ.StockProductDJ_Stone1B.Any() && !branded);

                        if (STAMPS.xActivate)
                        {
                            DiscountMemberPercent = branded ? disc2 : ((isGeneric) ? 0m : disc1);

                        }
                        else if (m != null)
                        {
                            DiscountMemberPercent = branded ? m.M_extradisc2.Value : ((isGeneric) ? 0m : m.M_extradisc.Value);

                        }

                        DiscountMemberNominal = (TotalRupiah - DiscountNormalNominal - DiscountPromoNominal) * DiscountMemberPercent / 100;
                        DiscountMemberNominal = Math.Floor(DiscountMemberNominal);
                    }

                    //lit_test.Text += "<br />______________________calcDJ-1_" + statusNoPromoMembership.ToString();
                    //lit_test.Text += "<br />______________________calcDJ-2_" + DiscountMemberPercent.ToString();
                    //lit_test.Text += "<br />______________________calcDJ-3_" + DiscountMemberNominal.ToString();

                    TotalBayar = TotalRupiah - DiscountNormalNominal - DiscountPromoNominal - DiscountBankNominal - DiscountMemberNominal
                                - DiscountVoucherNominal - DiscountOtherNominal;

                    try
                    {
                        DiscountRoundNominal = Convert.ToDecimal(txt5.Text);
                        if (DiscountRoundNominal == 0m) { DiscountRoundNominal = 0m; }
                        else if (DiscountRoundNominal > (TotalRupiah * 0.001m)) { DiscountRoundNominal = Math.Floor((TotalRupiah * 0.001m) / 1000) * 1000; }
                    }
                    catch (Exception) { }

                    if (DiscountRoundNominal > 0m)
                    {
                        TotalBayar = TotalBayar - DiscountRoundNominal;
                    }

                    try
                    {
                        TotalFeeExtraRingSize = Convert.ToDecimal(txt6.Text);
                    }
                    catch (Exception ex) { }

                    if (TotalFeeExtraRingSize > 0m)
                    {
                        Db.Execute("UPDATE SalesOrderDJ SET TotalFeeExtraRingSize = " + TotalFeeExtraRingSize + ", FeeExtraRingSizePerSize = " + FeeExtraRingSize + ", ExtraRingSize = " + ExtraRingSize + " WHERE ID = " + hf, Db.Cnn_DB);
                        TotalBayar = TotalBayar + TotalFeeExtraRingSize;
                    }

                    string[] ASP = new string[20];
                    ASP = DLC.getASPDJParameter(d.StockProductDJ, d.TotalBayar.Value);

                    int MEPeriodeMin = 0;
                    int MEPeriodeMax = 0;
                    decimal ME_Rumus = 0m;
                    decimal MENominal = 0m;
                    int TI1_PeriodeMin = 0;
                    int TI1_PeriodeMax = 0;
                    decimal TI1_Rumus = 0m;
                    decimal TI1_Nominal = 0m;
                    int TI2_PeriodeMin = 0;
                    int TI2_PeriodeMax = 0;
                    decimal TI2_Rumus = 0m;
                    decimal TI2_Nominal = 0m;
                    int TI3_PeriodeMin = 0;
                    int TI3_PeriodeMax = 0;
                    decimal TI3_Rumus = 0m;
                    decimal TI3_Nominal = 0m;
                    int R1_PeriodeMin = 0;
                    int R1_PeriodeMax = 0;
                    decimal R1_Rumus = 0m;
                    decimal R1_Nominal = 0m;
                    int R2_PeriodeMin = 0;
                    int R2_PeriodeMax = 0;
                    decimal R2_Rumus = 0m;
                    decimal R2_Nominal = 0m;
                    int MEPeriodeMaxDay = 0;

                    try
                    {
                        if (ASP[0] != "")
                        {
                            MEPeriodeMin = Convert.ToInt32(ASP[0]);
                            MEPeriodeMax = Convert.ToInt32(ASP[1]);
                            ME_Rumus = Convert.ToDecimal(ASP[2]);
                            MENominal = 0m;
                            TI1_PeriodeMin = Convert.ToInt32(ASP[3]);
                            TI1_PeriodeMax = Convert.ToInt32(ASP[4]);
                            TI1_Rumus = Convert.ToDecimal(ASP[5]);
                            TI1_Nominal = 0m;
                            TI2_PeriodeMin = Convert.ToInt32(ASP[6]);
                            TI2_PeriodeMax = Convert.ToInt32(ASP[7]);
                            TI2_Rumus = Convert.ToDecimal(ASP[8]);
                            TI2_Nominal = 0m;
                            TI3_PeriodeMin = Convert.ToInt32(ASP[9]);
                            TI3_PeriodeMax = Convert.ToInt32(ASP[10]);
                            TI3_Rumus = Convert.ToDecimal(ASP[11]);
                            TI3_Nominal = 0m;
                            R1_PeriodeMin = Convert.ToInt32(ASP[12]);
                            R1_PeriodeMax = Convert.ToInt32(ASP[13]);
                            R1_Rumus = Convert.ToDecimal(ASP[14]);
                            R1_Nominal = 0m;
                            R2_PeriodeMin = Convert.ToInt32(ASP[15]);
                            R2_PeriodeMax = Convert.ToInt32(ASP[16]);
                            R2_Rumus = Convert.ToDecimal(ASP[17]);
                            R2_Nominal = 0m;
                            MEPeriodeMaxDay = Convert.ToInt32(ASP[18]);
                        }
                    }
                    catch (Exception) { }

                    MENominal = (ME_Rumus / 100) * TotalBayar;
                    TI1_Nominal = (TI1_Rumus / 100) * TotalBayar;
                    TI2_Nominal = (TI2_Rumus / 100) * TotalBayar;
                    TI3_Nominal = (TI3_Rumus / 100) * TotalBayar;
                    R1_Nominal = (R1_Rumus / 100) * TotalBayar;
                    R2_Nominal = (R2_Rumus / 100) * TotalBayar;

                    d.MEPeriodeMin = MEPeriodeMin;
                    d.MEPeriodeMax = MEPeriodeMax;
                    d.MERumus = ME_Rumus;
                    d.MENominal = MENominal;
                    d.TI1_PeriodeMin = TI1_PeriodeMin;
                    d.TI1_PeriodeMax = TI1_PeriodeMax;
                    d.TI1_Rumus = TI1_Rumus;
                    d.TI1_Nominal = TI1_Nominal;
                    d.TI2_PeriodeMin = TI2_PeriodeMin;
                    d.TI2_PeriodeMax = TI2_PeriodeMax;
                    d.TI2_Rumus = TI2_Rumus;
                    d.TI2_Nominal = TI2_Nominal;
                    d.TI3_PeriodeMin = TI3_PeriodeMin;
                    d.TI3_PeriodeMax = TI3_PeriodeMax;
                    d.TI3_Rumus = TI3_Rumus;
                    d.TI3_Nominal = TI3_Nominal;
                    d.R1_PeriodeMin = R1_PeriodeMin;
                    d.R1_PeriodeMax = R1_PeriodeMax;
                    d.R1_Rumus = R1_Rumus;
                    d.R1_Nominal = R1_Nominal;
                    d.R2_PeriodeMin = R2_PeriodeMin;
                    d.R2_PeriodeMax = R2_PeriodeMax;
                    d.R2_Rumus = R2_Rumus;
                    d.R2_Nominal = R2_Nominal;
                    d.MEPeriodeMaxDay = MEPeriodeMaxDay;

                    d.Discount = DiscountNormalPercent;
                    d.DiscountNominal = DiscountNormalNominal;
                    d.DiscountBank = DiscountBankNominal;
                    d.DiscountProgram = DiscountMemberPercent;
                    d.DiscountProgramNominal = DiscountMemberNominal;
                    d.DiscountGift = DiscountVoucherNominal;
                    d.DiscountOther = DiscountOtherNominal;
                    d.DiscountRound = DiscountRoundNominal;
                    d.TotalRupiah = TotalRupiah;
                    d.TotalBayar = TotalBayar;
                    db.SubmitChanges();

                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
    }
    protected void calcPG(Mscustomer c, Msmembership m, decimal disc1, decimal disc2)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        TextBox txt3 = new TextBox();
        TextBox txt4 = new TextBox();
        TextBox txt5 = new TextBox();
        DropDownList ddl2 = new DropDownList();
        TextBox txt6 = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            decimal TotalRupiah = 0m;
            decimal TotalBayar = 0m;

            decimal DiscountNormalNominal = 0;
            decimal DiscountNormalPercent = 0;
            decimal DiscountPromoNominal = 0;
            decimal DiscountPromoPercent = 0;
            decimal DiscountBankNominal = 0;
            decimal DiscountMemberNominal = 0;
            decimal DiscountMemberPercent = 0;
            decimal DiscountVoucherNominal = 0;
            decimal DiscountOtherNominal = 0;
            decimal DiscountRoundNominal = 0;
            decimal TotalFeeExtraRingSize = 0;
            int ExtraRingSize = 0;
            int FeeExtraRingSize = 0;

            try
            {
                hf = ((HiddenField)row.FindControl("pghf1_" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("pgdn2_" + index.ToString());
                txt2 = (TextBox)row.FindControl("pgdb1_" + index.ToString());
                txt3 = (TextBox)row.FindControl("pgdv1_" + index.ToString());
                txt4 = (TextBox)row.FindControl("pgdo1_" + index.ToString());
                txt5 = (TextBox)row.FindControl("pgdr1_" + index.ToString());
                ddl2 = (DropDownList)row.FindControl("pgdb1ddl_" + index.ToString());
                txt6 = (TextBox)row.FindControl("pgtfrs_" + index.ToString());

                var d = db.SalesOrderPG.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        DiscountNormalPercent = Convert.ToDecimal(txt.Text);
                        if (DiscountNormalPercent > 10m) { DiscountNormalPercent = 10m; }
                        else if (DiscountNormalPercent < 0m) { DiscountNormalPercent = 0m; }
                    }
                    catch (Exception) { }
                    try
                    {
                        DiscountBankNominal = Convert.ToDecimal(txt2.Text);
                        if (DiscountBankNominal < 0m) { DiscountBankNominal = 0m; }
                        int idpromo = Convert.ToInt32(ddl2.SelectedValue);
                        if (idpromo > 0)
                        {
                            decimal discount = Db.SingleDecimal("SELECT TOP 1 ISNULL(Discount,0) [Discount] FROM PromoDetail WHERE ID = " + idpromo, Db.Cnn_DB);
                            DiscountBankNominal = discount;

                            Db.Execute("UPDATE SalesOrderPG SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
                        }
                        else if (idpromo == 0)
                        {
                            DiscountBankNominal = 0m;
                            Db.Execute("UPDATE SalesOrderPG SET IDPromoDetail = NULL WHERE ID = " + hf, Db.Cnn_DB);
                        }
                        else if (idpromo == -1)
                        {
                            Db.Execute("UPDATE SalesOrderPG SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
                        }
                    }
                    catch (Exception) { }

                    try
                    {
                        DiscountVoucherNominal = Convert.ToDecimal(txt3.Text);
                    }
                    catch (Exception) { }

                    try
                    {
                        DiscountOtherNominal = Convert.ToDecimal(txt4.Text);
                        //if (DiscountOtherNominal < 0m) { DiscountOtherNominal = 0m; }
                    }
                    catch (Exception) { }

                    TotalRupiah = (Math.Floor((d.HargaRupiah.HasValue ? d.HargaRupiah.Value : 0m) / 1000)) * 1000;

                    DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    DiscountPromoPercent = 0m;

                    DiscountNormalNominal = (Math.Floor(((TotalRupiah - DiscountPromoNominal) * (DiscountNormalPercent / 100)) / 1000)) * 1000;

                    //DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    //DiscountPromoPercent = 0m;

                    bool statusNoPromoMembership = false;

                    // promo
                    var t = from p in db.PromoPG_Detail
                            where p.PromoPG.ApprovalNama != null && p.PromoPG.TglStart.Value <= DateTime.Today
                            && p.PromoPG.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == d.IDProduct
                            && p.PromoPG.Draft == false
                            select p;
                    if (t.Count() > 0)
                    {
                        var disc = t.First();
                        var discP = disc.PromoPG;
                        statusNoPromoMembership = (discP.StatusDiscMembership != null) ? discP.StatusDiscMembership.Value : false;

                        ExtraRingSize = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(ExtraRingSize,0) AS INT) [ExtraRingSize] FROM PromoPG WHERE ID = " + discP.ID, Db.Cnn_DB);

                        FeeExtraRingSize = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(Harga,0) AS INT) [Harga] FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0", Db.Cnn_DB);
                    }

                    statusNoPromoMembership = true; // true utk pg dan gj
                    if (statusNoPromoMembership)
                    {
                        DiscountMemberPercent = 0m;
                        DiscountMemberNominal = 0m;
                    }
                    else
                    {
                        bool branded = false;
                        bool isGeneric = false;
                        bool isDossierNonBranded = false;

                        if (STAMPS.xActivate)
                        {
                            DiscountMemberPercent = branded ? disc2 : ((isGeneric) ? 0m : disc1);

                        }
                        else if (m != null)
                        {
                            DiscountMemberPercent = branded ? m.M_extradisc2.Value : ((isGeneric) ? 0m : m.M_extradisc.Value);

                        }

                        DiscountMemberNominal = (TotalRupiah - DiscountNormalNominal - DiscountPromoNominal) * DiscountMemberPercent / 100;
                        DiscountMemberNominal = Math.Floor(DiscountMemberNominal);
                    }

                    TotalBayar = TotalRupiah - DiscountNormalNominal - DiscountPromoNominal - DiscountBankNominal - DiscountMemberNominal
                                - DiscountVoucherNominal - DiscountOtherNominal;

                    try
                    {
                        DiscountRoundNominal = Convert.ToDecimal(txt5.Text);
                        if (DiscountRoundNominal == 0m) { DiscountRoundNominal = 0m; }
                        else if (DiscountRoundNominal > (TotalRupiah * 0.001m)) { DiscountRoundNominal = Math.Floor((TotalRupiah * 0.001m) / 1000) * 1000; }
                    }
                    catch (Exception) { }

                    if (DiscountRoundNominal > 0m)
                    {
                        TotalBayar = TotalBayar - DiscountRoundNominal;
                    }

                    try
                    {
                        TotalFeeExtraRingSize = Convert.ToDecimal(txt6.Text);
                    }
                    catch (Exception ex) { }

                    if (TotalFeeExtraRingSize > 0m)
                    {
                        Db.Execute("UPDATE SalesOrderPG SET TotalFeeExtraRingSize = " + TotalFeeExtraRingSize + ", FeeExtraRingSizePerSize = " + FeeExtraRingSize + ", ExtraRingSize = " + ExtraRingSize + " WHERE ID = " + hf, Db.Cnn_DB);
                        TotalBayar = TotalBayar + TotalFeeExtraRingSize;
                    }

                    string[] ASP = new string[20];

                    ASP = DLC.getASPPGParameter();

                    int MEPeriodeMin = 0;
                    int MEPeriodeMax = 0;
                    decimal ME_Rumus = 0m;
                    int TI1_PeriodeMin = 0;
                    decimal TI1_Rumus = 0m;
                    int TI2_PeriodeMin = 0;
                    decimal TI2_Rumus = 0m;
                    int R_PeriodeMin = 0;
                    decimal R_Rumus = 0m;

                    try
                    {
                        MEPeriodeMin = Convert.ToInt32(ASP[0]);
                        MEPeriodeMax = Convert.ToInt32(ASP[1]);
                        ME_Rumus = Convert.ToDecimal(ASP[2]);
                        TI1_PeriodeMin = Convert.ToInt32(ASP[3]);
                        TI1_Rumus = Convert.ToDecimal(ASP[4]);
                        TI2_PeriodeMin = Convert.ToInt32(ASP[5]);
                        TI2_Rumus = Convert.ToDecimal(ASP[6]);
                        R_PeriodeMin = Convert.ToInt32(ASP[7]);
                        R_Rumus = Convert.ToDecimal(ASP[8]);
                    }
                    catch (Exception) { }

                    d.MEPeriodeMin = MEPeriodeMin;
                    d.MEPeriodeMax = MEPeriodeMax;
                    d.MERumus = ME_Rumus;
                    d.TI1_PeriodeMin = TI1_PeriodeMin;
                    d.TI1_Rumus = TI1_Rumus;
                    d.TI2_PeriodeMin = TI2_PeriodeMin;
                    d.TI2_Rumus = TI2_Rumus;
                    d.R_PeriodeMin = R_PeriodeMin;
                    d.R_Rumus = R_Rumus;

                    d.Discount = DiscountNormalPercent;
                    d.DiscountNominal = DiscountNormalNominal;
                    d.DiscountBank = DiscountBankNominal;
                    d.DiscountProgram = DiscountMemberPercent;
                    d.DiscountProgramNominal = DiscountMemberNominal;
                    d.DiscountGift = DiscountVoucherNominal;
                    d.DiscountOther = DiscountOtherNominal;
                    d.DiscountRound = DiscountRoundNominal;
                    d.TotalRupiah = TotalRupiah;
                    d.TotalBayar = TotalBayar;
                    db.SubmitChanges();

                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
    }
    protected void calcGJ(Mscustomer c, Msmembership m, decimal disc1, decimal disc2)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        TextBox txt3 = new TextBox();
        TextBox txt4 = new TextBox();
        TextBox txt5 = new TextBox();
        DropDownList ddl2 = new DropDownList();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            decimal TotalRupiah = 0m;
            decimal TotalBayar = 0m;

            decimal DiscountNormalNominal = 0;
            decimal DiscountNormalPercent = 0;
            decimal DiscountPromoNominal = 0;
            decimal DiscountPromoPercent = 0;
            decimal DiscountBankNominal = 0;
            decimal DiscountMemberNominal = 0;
            decimal DiscountMemberPercent = 0;
            decimal DiscountVoucherNominal = 0;
            decimal DiscountOtherNominal = 0;
            decimal DiscountRoundNominal = 0;

            try
            {
                hf = ((HiddenField)row.FindControl("gjhf1_" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("gjdn2_" + index.ToString());
                txt2 = (TextBox)row.FindControl("gjdb1_" + index.ToString());
                txt3 = (TextBox)row.FindControl("gjdv1_" + index.ToString());
                txt4 = (TextBox)row.FindControl("gjdo1_" + index.ToString());
                txt5 = (TextBox)row.FindControl("gjdr1_" + index.ToString());
                ddl2 = (DropDownList)row.FindControl("gjdb1ddl_" + index.ToString());

                var d = db.SalesOrderGJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        DiscountNormalPercent = Convert.ToDecimal(txt.Text);
                        if (DiscountNormalPercent > 10m) { DiscountNormalPercent = 10m; }
                        else if (DiscountNormalPercent < 0m) { DiscountNormalPercent = 0m; }
                    }
                    catch (Exception) { }
                    try
                    {
                        DiscountBankNominal = Convert.ToDecimal(txt2.Text);
                        if (DiscountBankNominal < 0m) { DiscountBankNominal = 0m; }
                        int idpromo = Convert.ToInt32(ddl2.SelectedValue);
                        if (idpromo > 0)
                        {
                            decimal discount = Db.SingleDecimal("SELECT TOP 1 ISNULL(Discount,0) [Discount] FROM PromoDetail WHERE ID = " + idpromo, Db.Cnn_DB);
                            DiscountBankNominal = discount;

                            Db.Execute("UPDATE SalesOrderGJ SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
                        }
                        else if (idpromo == 0)
                        {
                            DiscountBankNominal = 0m;
                            Db.Execute("UPDATE SalesOrderGJ SET IDPromoDetail = NULL WHERE ID = " + hf, Db.Cnn_DB);
                        }
                        else if (idpromo == -1)
                        {
                            Db.Execute("UPDATE SalesOrderGJ SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
                        }
                    }
                    catch (Exception) { }

                    try
                    {
                        DiscountVoucherNominal = Convert.ToDecimal(txt3.Text);
                    }
                    catch (Exception) { }

                    try
                    {
                        DiscountOtherNominal = Convert.ToDecimal(txt4.Text);
                        //if (DiscountOtherNominal < 0m) { DiscountOtherNominal = 0m; }
                    }
                    catch (Exception) { }

                    TotalRupiah = (Math.Floor((d.HargaRupiah.HasValue ? d.HargaRupiah.Value : 0m) / 1000)) * 1000;

                    DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    DiscountPromoPercent = 0m;

                    DiscountNormalNominal = (Math.Floor(((TotalRupiah - DiscountPromoNominal) * (DiscountNormalPercent / 100)) / 1000)) * 1000;

                    //DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    //DiscountPromoPercent = 0m;

                    bool statusNoPromoMembership = false;

                    // promo
                    var t = from p in db.PromoGJ_Detail
                            where p.PromoGJ.ApprovalNama != null && p.PromoGJ.TglStart.Value <= DateTime.Today
                            && p.PromoGJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == d.IDProduct
                            && p.PromoGJ.Draft == false
                            select p;
                    if (t.Count() > 0)
                    {
                        var disc = t.First();
                        var discP = disc.PromoGJ;
                        statusNoPromoMembership = (discP.StatusDiscMembership != null) ? discP.StatusDiscMembership.Value : false;
                    }

                    statusNoPromoMembership = true; // true utk pg dan gj
                    if (statusNoPromoMembership)
                    {
                        DiscountMemberPercent = 0m;
                        DiscountMemberNominal = 0m;
                    }
                    else
                    {
                        bool branded = false;
                        bool isGeneric = false;
                        bool isDossierNonBranded = false;

                        if (STAMPS.xActivate)
                        {
                            DiscountMemberPercent = branded ? disc2 : ((isGeneric) ? 0m : disc1);

                        }
                        else if (m != null)
                        {
                            DiscountMemberPercent = branded ? m.M_extradisc2.Value : ((isGeneric) ? 0m : m.M_extradisc.Value);

                        }

                        DiscountMemberNominal = (TotalRupiah - DiscountNormalNominal - DiscountPromoNominal) * DiscountMemberPercent / 100;
                        DiscountMemberNominal = Math.Floor(DiscountMemberNominal);
                    }

                    TotalBayar = TotalRupiah - DiscountNormalNominal - DiscountPromoNominal - DiscountBankNominal - DiscountMemberNominal
                                - DiscountVoucherNominal - DiscountOtherNominal;

                    try
                    {
                        DiscountRoundNominal = Convert.ToDecimal(txt5.Text);
                        if (DiscountRoundNominal == 0m) { DiscountRoundNominal = 0m; }
                        else if (DiscountRoundNominal > (TotalRupiah * 0.001m)) { DiscountRoundNominal = Math.Floor((TotalRupiah * 0.001m) / 1000) * 1000; }
                    }
                    catch (Exception) { }

                    if (DiscountRoundNominal > 0m)
                    {
                        TotalBayar = TotalBayar - DiscountRoundNominal;
                    }

                    try
                    {
                        string[] ASP = new string[20];

                        bool Solitaire = false;

                        ASP = DLC.getASPGJParameter(d.StockProductGJ.StockProductGJ_CharProduct.ProductCategory, Solitaire);

                        int MEPeriodeMin = 0;
                        int MEPeriodeMax = 0;
                        decimal ME_Rumus = 0m;
                        decimal MENominal = 0m;
                        int TI1_PeriodeMin = 0;
                        int TI1_PeriodeMax = 0;
                        decimal TI1_Rumus = 0m;
                        decimal TI1_Nominal = 0m;
                        int TI2_PeriodeMin = 0;
                        int TI2_PeriodeMax = 0;
                        decimal TI2_Rumus = 0;
                        decimal TI2_Nominal = 0m;
                        int TI3_PeriodeMin = 0;
                        int TI3_PeriodeMax = 0;
                        decimal TI3_Rumus = 0m;
                        decimal TI3_Nominal = 0m;
                        int R1_PeriodeMin = 0;
                        int R1_PeriodeMax = 0;
                        decimal R1_Rumus = 0m;
                        decimal R1_Nominal = 0m;
                        int R2_PeriodeMin = 0;
                        int R2_PeriodeMax = 0;
                        decimal R2_Rumus = 0m;
                        decimal R2_Nominal = 0m;

                        try
                        {
                            MEPeriodeMin = Convert.ToInt32(ASP[0]);
                            MEPeriodeMax = Convert.ToInt32(ASP[1]);
                            ME_Rumus = Convert.ToDecimal(ASP[2]);
                            MENominal = 0m;
                            TI1_PeriodeMin = Convert.ToInt32(ASP[3]);
                            TI1_PeriodeMax = Convert.ToInt32(ASP[4]);
                            TI1_Rumus = Convert.ToDecimal(ASP[5]);
                            TI1_Nominal = 0m;
                            TI2_PeriodeMin = Convert.ToInt32(ASP[6]);
                            TI2_PeriodeMax = Convert.ToInt32(ASP[7]);
                            TI2_Rumus = Convert.ToDecimal(ASP[8]);
                            TI2_Nominal = 0m;
                            TI3_PeriodeMin = Convert.ToInt32(ASP[9]);
                            TI3_PeriodeMax = Convert.ToInt32(ASP[10]);
                            TI3_Rumus = Convert.ToDecimal(ASP[11]);
                            TI3_Nominal = 0m;
                            R1_PeriodeMin = Convert.ToInt32(ASP[12]);
                            R1_PeriodeMax = Convert.ToInt32(ASP[13]);
                            R1_Rumus = Convert.ToDecimal(ASP[14]);
                            R1_Nominal = 0m;
                            R2_PeriodeMin = Convert.ToInt32(ASP[15]);
                            R2_PeriodeMax = Convert.ToInt32(ASP[16]);
                            R2_Rumus = Convert.ToDecimal(ASP[17]);
                            R2_Nominal = 0m;
                        }
                        catch (Exception) { }

                        MENominal = (ME_Rumus / 100) * TotalBayar;
                        TI1_Nominal = (TI1_Rumus / 100) * TotalBayar;
                        TI2_Nominal = (TI2_Rumus / 100) * TotalBayar;
                        TI3_Nominal = (TI3_Rumus / 100) * TotalBayar;
                        R1_Nominal = (R1_Rumus / 100) * TotalBayar;
                        R2_Nominal = (R2_Rumus / 100) * TotalBayar;

                        d.MEPeriodeMin = MEPeriodeMin;
                        d.MEPeriodeMax = MEPeriodeMax;
                        d.MERumus = ME_Rumus;
                        d.MENominal = MENominal;
                        d.TI1_PeriodeMin = TI1_PeriodeMin;
                        d.TI1_PeriodeMax = TI1_PeriodeMax;
                        d.TI1_Rumus = TI1_Rumus;
                        d.TI1_Nominal = TI1_Nominal;
                        d.TI2_PeriodeMin = TI2_PeriodeMin;
                        d.TI2_PeriodeMax = TI2_PeriodeMax;
                        d.TI2_Rumus = TI2_Rumus;
                        d.TI2_Nominal = TI2_Nominal;
                        d.TI3_PeriodeMin = TI3_PeriodeMin;
                        d.TI3_PeriodeMax = TI3_PeriodeMax;
                        d.TI3_Rumus = TI3_Rumus;
                        d.TI3_Nominal = TI3_Nominal;
                        d.R1_PeriodeMin = R1_PeriodeMin;
                        d.R1_PeriodeMax = R1_PeriodeMax;
                        d.R1_Rumus = R1_Rumus;
                        d.R1_Nominal = R1_Nominal;
                        d.R2_PeriodeMin = R2_PeriodeMin;
                        d.R2_PeriodeMax = R2_PeriodeMax;
                        d.R2_Rumus = R2_Rumus;
                        d.R2_Nominal = R2_Nominal;
                    }
                    catch (Exception) { }

                    d.Discount = DiscountNormalPercent;
                    d.DiscountNominal = DiscountNormalNominal;
                    d.DiscountBank = DiscountBankNominal;
                    d.DiscountProgram = DiscountMemberPercent;
                    d.DiscountProgramNominal = DiscountMemberNominal;
                    d.DiscountGift = DiscountVoucherNominal;
                    d.DiscountOther = DiscountOtherNominal;
                    d.DiscountRound = DiscountRoundNominal;
                    d.TotalRupiah = TotalRupiah;
                    d.TotalBayar = TotalBayar;
                    db.SubmitChanges();

                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
    }
    protected void calcLD(Mscustomer c, Msmembership m, decimal disc1, decimal disc2)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        TextBox txt3 = new TextBox();
        TextBox txt4 = new TextBox();
        TextBox txt5 = new TextBox();
        DropDownList ddl2 = new DropDownList();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            decimal TotalRupiah = 0m;
            decimal TotalBayar = 0m;

            decimal DiscountNormalNominal = 0;
            decimal DiscountNormalPercent = 0;
            decimal DiscountPromoNominal = 0;
            decimal DiscountPromoPercent = 0;
            decimal DiscountBankNominal = 0;
            decimal DiscountMemberNominal = 0;
            decimal DiscountMemberPercent = 0;
            decimal DiscountVoucherNominal = 0;
            decimal DiscountOtherNominal = 0;
            decimal DiscountRoundNominal = 0;

            try
            {
                hf = ((HiddenField)row.FindControl("ldhf1_" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("lddn2_" + index.ToString());
                txt2 = (TextBox)row.FindControl("lddb1_" + index.ToString());
                txt3 = (TextBox)row.FindControl("lddv1_" + index.ToString());
                txt4 = (TextBox)row.FindControl("lddo1_" + index.ToString());
                txt5 = (TextBox)row.FindControl("lddr1_" + index.ToString());
                ddl2 = (DropDownList)row.FindControl("lddb1ddl_" + index.ToString());

                var d = db.SalesOrderLD.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        DiscountNormalPercent = Convert.ToDecimal(txt.Text);
                        if (DiscountNormalPercent > 10m) { DiscountNormalPercent = 10m; }
                        else if (DiscountNormalPercent < 0m) { DiscountNormalPercent = 0m; }
                    }
                    catch (Exception) { }
                    try
                    {
                        DiscountBankNominal = Convert.ToDecimal(txt2.Text);
                        if (DiscountBankNominal < 0m) { DiscountBankNominal = 0m; }
                        int idpromo = Convert.ToInt32(ddl2.SelectedValue);
                        if (idpromo > 0)
                        {
                            decimal discount = Db.SingleDecimal("SELECT TOP 1 ISNULL(Discount,0) [Discount] FROM PromoDetail WHERE ID = " + idpromo, Db.Cnn_DB);
                            DiscountBankNominal = discount;

                            Db.Execute("UPDATE SalesOrderLD SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
                        }
                        else if (idpromo == 0)
                        {
                            DiscountBankNominal = 0m;
                            Db.Execute("UPDATE SalesOrderLD SET IDPromoDetail = NULL WHERE ID = " + hf, Db.Cnn_DB);
                        }
                        else if (idpromo == -1)
                        {
                            Db.Execute("UPDATE SalesOrderLD SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
                        }
                    }
                    catch (Exception) { }

                    try
                    {
                        DiscountVoucherNominal = Convert.ToDecimal(txt3.Text);
                    }
                    catch (Exception) { }

                    try
                    {
                        DiscountOtherNominal = Convert.ToDecimal(txt4.Text);
                        //if (DiscountOtherNominal < 0m) { DiscountOtherNominal = 0m; }
                    }
                    catch (Exception) { }

                    TotalRupiah = (Math.Floor((d.HargaRupiah.HasValue ? d.HargaRupiah.Value : 0m) / 1000)) * 1000;

                    DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    DiscountPromoPercent = 0m;

                    DiscountNormalNominal = (Math.Floor(((TotalRupiah - DiscountPromoNominal) * (DiscountNormalPercent / 100)) / 1000)) * 1000;

                    //DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    //DiscountPromoPercent = 0m;

                    bool branded = Pricing.isDiamondBranded(d.StockProductLD_Stone1B.Nomor);
                    bool isGeneric = Pricing.isDiamondGeneric(d.StockProductLD_Stone1B.Nomor);
                    //bool isDossierNonBranded = (d.StockProductLD_Stone1B.StockProductDJ_Stone1B.Any() && !branded);

                    if (STAMPS.xActivate)
                    {
                        DiscountMemberPercent = branded ? disc2 : ((isGeneric) ? 0m : disc1);

                    }
                    else if (m != null)
                    {
                        DiscountMemberPercent = branded ? m.M_extradisc2.Value : ((isGeneric) ? 0m : m.M_extradisc.Value);

                    }

                    DiscountMemberNominal = (TotalRupiah - DiscountNormalNominal - DiscountPromoNominal) * DiscountMemberPercent / 100;
                    DiscountMemberNominal = Math.Floor(DiscountMemberNominal);


                    TotalBayar = TotalRupiah - DiscountNormalNominal - DiscountPromoNominal - DiscountBankNominal - DiscountMemberNominal
                                - DiscountVoucherNominal - DiscountOtherNominal;

                    try
                    {
                        DiscountRoundNominal = Convert.ToDecimal(txt5.Text);
                        if (DiscountRoundNominal == 0m) { DiscountRoundNominal = 0m; }
                        else if (DiscountRoundNominal > (TotalRupiah * 0.001m)) { DiscountRoundNominal = Math.Floor((TotalRupiah * 0.001m) / 1000) * 1000; }
                    }
                    catch (Exception) { }

                    if (DiscountRoundNominal > 0m)
                    {
                        TotalBayar = TotalBayar - DiscountRoundNominal;
                    }

                    string[] ASP = new string[20];

                    bool Solitaire = false;

                    ASP = DLC.getASPLDParameter(1, Solitaire);

                    int MEPeriodeMin = 0;
                    int MEPeriodeMax = 0;
                    decimal ME_Rumus = 0m;
                    decimal MENominal = 0m;
                    int TI1_PeriodeMin = 0;
                    int TI1_PeriodeMax = 0;
                    decimal TI1_Rumus = 0m;
                    decimal TI1_Nominal = 0m;
                    int TI2_PeriodeMin = 0;
                    int TI2_PeriodeMax = 0;
                    decimal TI2_Rumus = 0m;
                    decimal TI2_Nominal = 0m;
                    int TI3_PeriodeMin = 0;
                    int TI3_PeriodeMax = 0;
                    decimal TI3_Rumus = 0m;
                    decimal TI3_Nominal = 0m;
                    int R1_PeriodeMin = 0;
                    int R1_PeriodeMax = 0;
                    decimal R1_Rumus = 0m;
                    decimal R1_Nominal = 0m;
                    int R2_PeriodeMin = 0;
                    int R2_PeriodeMax = 0;
                    decimal R2_Rumus = 0m;
                    decimal R2_Nominal = 0m;

                    try
                    {
                        MEPeriodeMin = Convert.ToInt32(ASP[0]);
                        MEPeriodeMax = Convert.ToInt32(ASP[1]);
                        ME_Rumus = Convert.ToDecimal(ASP[2]);
                        MENominal = 0m;
                        TI1_PeriodeMin = Convert.ToInt32(ASP[3]);
                        TI1_PeriodeMax = Convert.ToInt32(ASP[4]);
                        TI1_Rumus = Convert.ToDecimal(ASP[5]);
                        TI1_Nominal = 0m;
                        TI2_PeriodeMin = Convert.ToInt32(ASP[6]);
                        TI2_PeriodeMax = Convert.ToInt32(ASP[7]);
                        TI2_Rumus = Convert.ToDecimal(ASP[8]);
                        TI2_Nominal = 0m;
                        TI3_PeriodeMin = Convert.ToInt32(ASP[9]);
                        TI3_PeriodeMax = Convert.ToInt32(ASP[10]);
                        TI3_Rumus = Convert.ToDecimal(ASP[11]);
                        TI3_Nominal = 0m;
                        R1_PeriodeMin = Convert.ToInt32(ASP[12]);
                        R1_PeriodeMax = Convert.ToInt32(ASP[13]);
                        R1_Rumus = Convert.ToDecimal(ASP[14]);
                        R1_Nominal = 0m;
                        R2_PeriodeMin = Convert.ToInt32(ASP[15]);
                        R2_PeriodeMax = Convert.ToInt32(ASP[16]);
                        R2_Rumus = Convert.ToDecimal(ASP[17]);
                        R2_Nominal = 0m;
                    }
                    catch (Exception) { }

                    MENominal = (ME_Rumus / 100) * TotalBayar;
                    TI1_Nominal = (TI1_Rumus / 100) * TotalBayar;
                    TI2_Nominal = (TI2_Rumus / 100) * TotalBayar;
                    TI3_Nominal = (TI3_Rumus / 100) * TotalBayar;
                    R1_Nominal = (R1_Rumus / 100) * TotalBayar;
                    R2_Nominal = (R2_Rumus / 100) * TotalBayar;

                    d.MEPeriodeMin = MEPeriodeMin;
                    d.MEPeriodeMax = MEPeriodeMax;
                    d.MERumus = ME_Rumus;
                    d.MENominal = MENominal;
                    d.TI1_PeriodeMin = TI1_PeriodeMin;
                    d.TI1_PeriodeMax = TI1_PeriodeMax;
                    d.TI1_Rumus = TI1_Rumus;
                    d.TI1_Nominal = TI1_Nominal;
                    d.TI2_PeriodeMin = TI2_PeriodeMin;
                    d.TI2_PeriodeMax = TI2_PeriodeMax;
                    d.TI2_Rumus = TI2_Rumus;
                    d.TI2_Nominal = TI2_Nominal;
                    d.TI3_PeriodeMin = TI3_PeriodeMin;
                    d.TI3_PeriodeMax = TI3_PeriodeMax;
                    d.TI3_Rumus = TI3_Rumus;
                    d.TI3_Nominal = TI3_Nominal;
                    d.R1_PeriodeMin = R1_PeriodeMin;
                    d.R1_PeriodeMax = R1_PeriodeMax;
                    d.R1_Rumus = R1_Rumus;
                    d.R1_Nominal = R1_Nominal;
                    d.R2_PeriodeMin = R2_PeriodeMin;
                    d.R2_PeriodeMax = R2_PeriodeMax;
                    d.R2_Rumus = R2_Rumus;
                    d.R2_Nominal = R2_Nominal;

                    d.Discount = DiscountNormalPercent;
                    d.DiscountNominal = DiscountNormalNominal;
                    d.DiscountBank = DiscountBankNominal;
                    d.DiscountProgram = DiscountMemberPercent;
                    d.DiscountProgramNominal = DiscountMemberNominal;
                    d.DiscountGift = DiscountVoucherNominal;
                    d.DiscountOther = DiscountOtherNominal;
                    d.DiscountRound = DiscountRoundNominal;
                    d.TotalRupiah = TotalRupiah;
                    d.TotalBayar = TotalBayar;
                    db.SubmitChanges();

                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
    }
    protected void CalcDiscountRound(decimal TotalHarga, TextBox tb)
    {
        decimal Disc = 0m;

        App rd = new App();

        Disc = TotalHarga - rd.RoundBottom(TotalHarga);
        tb.Text = Disc.ToString("N0");
    }

    #region regionResell

    //Item
    protected void fillItemDJRO(int RO_ID, ref int QTY_RO, ref decimal GTRO, bool isVisible)
    {
        var rs = from p in db.ResellDJ
                 where p.IDForm == RO_ID
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;

        //Sub-Header
        if (iQty > 0)
        {
            Sub_HeaderRO(0);
            foreach (var r in rs)
            {
                bool whitelist = false;
                var w = db.StockProductWhitelist.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeProduct == "DJ" && p.Approval != null && p.ApprovalTgl != null);
                if (w != null) { whitelist = true; }

                string meinfo = me_Info(r, whitelist);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLinkRO(0, r.ID, r.IDProduct, isVisible);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "<p>" + index.ToString() + ".</p>";
                //td.Style.Add("text-align", "center");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ.Nomor
                        + "<br />" + r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama
                        ;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.SalesOrder.TglLunas != null)
                {
                    td.Text = Str.FullDate(r.SalesOrder.TglLunas.Value)
                            + "<br />" + meinfo
                            ;
                }
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DocQC_DJ(r, isVisible);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                Label lblI = new Label();
                lblI.Text = " IDR";
                lblI.Width = 25;

                Label lblha = new Label();
                lblha.Text = " IDR";
                lblha.Width = 25;

                TextBox txtha = new TextBox();
                txtha.Attributes["style"] = "width: 100px; text-align:right;";
                txtha.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
                txtha.Enabled = false;
                td.Controls.Add(txtha);
                td.Controls.Add(lblha);
                tr.Cells.Add(td);

                TextBox txt = new TextBox();
                txt.ID = "djro_" + index.ToString();
                txt.Attributes["style"] = "width: 100px; text-align:right;";
                txt.Text = r.HargaRupiah.HasValue ? r.HargaRupiah.Value.ToString("N0") : r.HargaAcuan.Value.ToString("N0");
                txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                txt.AutoPostBack = true;
                txt.TextChanged += new EventHandler(Price_TextChanged);
                txt.Enabled = isVisible;

                HiddenField hf = new HiddenField();
                hf.ID = "hfdjro" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(txt);
                td.Controls.Add(lblI);
                td.Controls.Add(hf);
                tr.Cells.Add(td);

                tbti.Rows.Add(tr);

                ST += Convert.ToDecimal(r.HargaRupiah);

                index++;
            }
            QTY_RO += iQty;
            GTRO += ST;
            Sub_TotalRO(ST, false);
        }

    }
    protected void fillItemPGRO(int RO_ID, ref int QTY_RO, ref decimal GTRO, bool isVisible)
    {
        var rs = from p in db.ResellPG
                 where p.IDForm == RO_ID
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;

        //Sub-Header
        if (iQty > 0)
        {
            Sub_HeaderRO(1);
            foreach (var r in rs)
            {
                bool whitelist = false;
                var w = db.StockProductWhitelist.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeProduct == "PG" && p.Approval != null && p.ApprovalTgl != null);
                if (w != null) { whitelist = true; }

                string meinfo = me_Info(r, whitelist);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLinkRO(1, r.ID, r.IDProduct, isVisible);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "<p>" + index.ToString() + ".</p>";
                //td.Style.Add("text-align", "center");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.Nomor
                        + "<br />" + r.StockProductPG.CharProductItem.Nama
                        ;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.SalesOrder.TglLunas != null)
                {
                    td.Text = Str.FullDate(r.SalesOrder.TglLunas.Value)
                            + "<br />" + meinfo
                            ;
                }
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

				td = new TableCell();
				td.Text = DocQC_PG(r, isVisible);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                TextBox txt2 = new TextBox();
                txt2.ID = "pg_weight_" + index.ToString();
                txt2.Attributes["style"] = "width: 100px;";
                txt2.Attributes["data-plu"] = r.ID.ToString();
                txt2.Attributes["data-nettweight"] = r.StockProductPG.NettoWeight.Value.ToString();
                //txt2.Attributes["onblur"] = "EnabledDisabledNextStep(this);";
                decimal beratisian = Db.SingleDecimal("SELECT TOP 1 ISNULL(BeratTimbang,0) FROM ResellPG WHERE ID = " + r.ID.ToString() + " ", Db.Cnn_DB);
                decimal weightresell = Convert.ToDecimal(r.StockProductPG.NettoWeight.Value);
                if (beratisian == 0m)
                {
                    Db.Execute("UPDATE ResellPG SET BeratTimbang = '" + weightresell.ToString().Replace(",", ".") + "' WHERE ID = " + r.ID.ToString() + " ", Db.Cnn_DB);
                }
                weightresell = Db.SingleDecimal("SELECT TOP 1 ISNULL(BeratTimbang,0) FROM ResellPG WHERE ID = " + r.ID.ToString() + " ", Db.Cnn_DB);
                txt2.Text = weightresell.ToString("N2");
                txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                txt2.AutoPostBack = true;
                txt2.TextChanged += new EventHandler(WeightPG_TextChanged);

                //Label lbl10 = new Label();
                //lbl10.Text = "Weight";

                Label lbl11 = new Label();
                lbl11.Style.Add("width", "100px");
                lbl11.Text = "<br />Berat harus lebih besar dari 0,00.";
                lbl11.ForeColor = Color.Red;

                td = new TableCell();
                td.Style.Add("width", "100px");
                //td.Controls.Add(lbl10);
                td.Controls.Add(txt2);
                if (weightresell == 0m)
                {
                    td.Controls.Add(lbl11);
                }
                tr.Cells.Add(td);

                decimal TGPNow = Db.SingleDecimal("SELECT TOP 1 ISNULL(TGP,0) FROM ResellPG WHERE ID = " + r.ID.ToString() + " ", Db.Cnn_DB);
                decimal TGP = GetTGPPGBeli(r.StockProductPG);
                if (TGPNow == 0m)
                {
                    Db.Execute("UPDATE ResellPG SET TGP = '" + TGP + "' WHERE ID = " + r.ID.ToString() + " ", Db.Cnn_DB);
                }
                td = new TableCell();
                td.Text = TGP.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                Label lblI = new Label();
                lblI.Text = " IDR";
                lblI.Width = 25;

                Label lblha = new Label();
                lblha.Text = " IDR";
                lblha.Width = 25;

                TextBox txtha = new TextBox();
                txtha.Attributes["style"] = "width: 100px; text-align:right;";
                txtha.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
                txtha.Enabled = false;
                td.Controls.Add(txtha);
                td.Controls.Add(lblha);
                tr.Cells.Add(td);

                TextBox txt = new TextBox();
                txt.ID = "pgro_" + index.ToString();
                txt.Attributes["style"] = "width: 100px; text-align:right;";
                txt.Text = (r.HargaRupiah.Value != null) ? r.HargaRupiah.Value.ToString("N0") : r.HargaAcuan.Value.ToString("N0");
                txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                txt.AutoPostBack = true;
                txt.TextChanged += new EventHandler(Price_TextChanged);
                txt.Enabled = isVisible;

                HiddenField hf = new HiddenField();
                hf.ID = "hfpgro" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(txt);
                td.Controls.Add(lblI);
                td.Controls.Add(hf);
                tr.Cells.Add(td);

                tbti.Rows.Add(tr);

                ST += Convert.ToDecimal(r.HargaRupiah);

                index++;
            }
            QTY_RO += iQty;
            GTRO += ST;
            Sub_TotalRO(ST, false);
        }

    }
    protected void fillItemGJRO(int RO_ID, ref int QTY_RO, ref decimal GTRO, bool isVisible)
    {
        var rs = from p in db.ResellGJ
                 where p.IDForm == RO_ID
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;

        //Sub-Header
        if (iQty > 0)
        {
            Sub_HeaderRO(2);
            foreach (var r in rs)
            {
                bool whitelist = false;
                var w = db.StockProductWhitelist.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeProduct == "GJ" && p.Approval != null && p.ApprovalTgl != null);
                if (w != null) { whitelist = true; }

                string meinfo = me_Info(r, whitelist);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLinkRO(2, r.ID, r.IDProduct, isVisible);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "<p>" + index.ToString() + ".</p>";
                //td.Style.Add("text-align", "center");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.Nomor
                        + "<br />" + r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama
                        ;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.SalesOrder.TglLunas != null)
                {
                    td.Text = Str.FullDate(r.SalesOrder.TglLunas.Value)
                            + "<br />" + meinfo
                            ;
                }
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DocQC_GJ(r, isVisible);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                Label lblI = new Label();
                lblI.Text = " IDR";
                lblI.Width = 25;

                Label lblha = new Label();
                lblha.Text = " IDR";
                lblha.Width = 25;

                TextBox txtha = new TextBox();
                txtha.Attributes["style"] = "width: 100px; text-align:right;";
                txtha.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
                txtha.Enabled = false;
                td.Controls.Add(txtha);
                td.Controls.Add(lblha);
                tr.Cells.Add(td);

                TextBox txt = new TextBox();
                txt.ID = "gjro_" + index.ToString();
                txt.Attributes["style"] = "width: 100px; text-align:right;";
                txt.Text = r.HargaRupiah.HasValue ? r.HargaRupiah.Value.ToString("N0") : r.HargaAcuan.Value.ToString("N0");
                txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                txt.AutoPostBack = true;
                txt.TextChanged += new EventHandler(Price_TextChanged);
                txt.Enabled = isVisible;

                HiddenField hf = new HiddenField();
                hf.ID = "hfgjro" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(txt);
                td.Controls.Add(lblI);
                td.Controls.Add(hf);
                tr.Cells.Add(td);

                tbti.Rows.Add(tr);

                ST += Convert.ToDecimal(r.HargaRupiah);

                index++;

            }
            QTY_RO += iQty;
            GTRO += ST;
            Sub_TotalRO(ST, false);
        }

    }
    protected void fillItemLDRO(int RO_ID, ref int QTY_RO, ref decimal GTRO, bool isVisible)
    {
        var rs = from p in db.ResellLD
                 where p.IDForm == RO_ID
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;

        //Sub-Header
        if (rs.Count() > 0)
        {
            Sub_HeaderRO(3);
            foreach (var r in rs)
            {
                bool whitelist = false;
                var w = db.StockProductWhitelist.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeProduct == "LD" && p.Approval != null && p.ApprovalTgl != null);
                if (w != null) { whitelist = true; }

                string meinfo = me_Info(r, whitelist);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLinkRO(3, r.ID, r.IDProduct, isVisible);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "<p>" + index.ToString() + ".</p>";
                //td.Style.Add("text-align", "center");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductLD_Stone1B.Nomor
                        ;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.SalesOrder.TglLunas != null)
                {
                    td.Text = Str.FullDate(r.SalesOrder.TglLunas.Value)
                            + "<br />" + meinfo
                            ;
                }
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DocQC_LD(r, isVisible);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                Label lblI = new Label();
                lblI.Text = " IDR";
                lblI.Width = 25;

                Label lblha = new Label();
                lblha.Text = " IDR";
                lblha.Width = 25;

                TextBox txtha = new TextBox();
                txtha.Attributes["style"] = "width: 100px; text-align:right;";
                txtha.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
                txtha.Enabled = false;
                td.Controls.Add(txtha);
                td.Controls.Add(lblha);
                tr.Cells.Add(td);

                TextBox txt = new TextBox();
                txt.ID = "ldro_" + index.ToString();
                txt.Attributes["style"] = "width: 100px; text-align:right;";
                txt.Text = r.HargaRupiah.HasValue ? r.HargaRupiah.Value.ToString("N0") : r.HargaAcuan.Value.ToString("N0");
                txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                txt.AutoPostBack = true;
                txt.TextChanged += new EventHandler(Price_TextChanged);
                txt.Enabled = isVisible;

                HiddenField hf = new HiddenField();
                hf.ID = "hfldro" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(txt);
                td.Controls.Add(lblI);
                td.Controls.Add(hf);
                tr.Cells.Add(td);

                tbti.Rows.Add(tr);

                ST += Convert.ToDecimal(r.HargaRupiah);

                index++;

            }
            QTY_RO += iQty;
            GTRO += ST;
            Sub_TotalRO(ST, false);
        }

    }

    protected void Price_TextChanged(object sender, EventArgs e)
    {
        calcRO();

        string href = App.RefreshURL(this);
        //Page-Qstr
        href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
        href += "&tb=" + ((TextBox)sender).ClientID;
        Response.Redirect(href);
    }
	protected void WeightPG_TextChanged(object sender, EventArgs e)
    {
        string weight = ((TextBox)sender).Text.Replace(",", ".");
        string idresellpg = ((TextBox)sender).Attributes["data-plu"];
        string nettweight = ((TextBox)sender).Attributes["data-nettweight"].Replace(",", ".");
        nettweight = nettweight.Remove(nettweight.Length - 2);
        string errorMsg = "";

        decimal output = 0m;
        if (decimal.TryParse(weight, out output))
        {
            decimal a = Convert.ToDecimal(weight);
            decimal b = Convert.ToDecimal(nettweight);
            if (output == 0m)
            {
                errorMsg = "Berat harus lebih dari 0";
            }
            else
            {
                if (a > b)
                {
                    errorMsg = "Berat tidak boleh lebih dari " + nettweight;
                }
                else
                {
                    Db.Execute("UPDATE ResellPG SET BeratTimbang = " + weight + " WHERE ID = " + idresellpg + " ", Db.Cnn_DB);
                }
            }
        }
        else
        {
            errorMsg = "Berat harus menggunakan koma(,) untuk desimal. Tidak boleh diisi alphabet.";
        }
        string href = App.RefreshURL(this);
        //Page-Qstr
        href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
        href += "&tb=" + ((TextBox)sender).ClientID;
        href += "&itempg=" + idresellpg;
        href += "&wg=" + weight;
        href += "&nw=" + nettweight;
        if (errorMsg != "")
        {
            href += "&err=" + errorMsg;
        }
        Response.Redirect(href);
    }
    protected void calcRO()
    {
        calcDJRO();
        calcPGRO();
        calcGJRO();
        calcLDRO();
    }
    protected void calcDJRO()
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";

        foreach (TableRow row in tbti.Rows)
        {
            decimal HargaBeli = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfdjro" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("djro_" + index.ToString());
                var d = db.ResellDJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        var pr = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == d.IDProduct &&
                            p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                        HargaBeli = Convert.ToDecimal(txt.Text);
                        //if (HargaBeli > d.HargaAcuan.Value && d.StockProductDJ.Legacy != 1)
                        //{
                        //if (d.SalesOrder.Tgl.Value.AddMonths(pr.TI2_PeriodeMin.Value) > DateTime.Today)
                        //HargaBeli = d.HargaAcuan.Value;
                        //}

                        d.HargaRupiah = HargaBeli;

                        db.SubmitChanges();
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }
    protected void calcGJRO()
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";


        foreach (TableRow row in tbti.Rows)
        {
            decimal HargaBeli = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfgjro" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("gjro_" + index.ToString());
                var d = db.ResellGJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        var pr = db.SalesOrderGJ.SingleOrDefault(p => p.IDProduct == d.IDProduct &&
                            p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                        HargaBeli = Convert.ToDecimal(txt.Text);
                        //if (HargaBeli > d.HargaAcuan.Value && d.SalesOrder.Tgl.Value.AddMonths(pr.TI2_PeriodeMin.Value) > DateTime.Today)
                        //    HargaBeli = d.HargaAcuan.Value;

                        d.HargaRupiah = HargaBeli;

                        db.SubmitChanges();
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }
    protected void calcLDRO()
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";


        foreach (TableRow row in tbti.Rows)
        {
            decimal HargaBeli = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfldro" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("ldro_" + index.ToString());
                var d = db.ResellLD.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        var pr = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == d.IDProduct &&
                            p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                        HargaBeli = Convert.ToDecimal(txt.Text);
                        //if (HargaBeli > d.HargaAcuan.Value && d.SalesOrder.Tgl.Value.AddMonths(pr.TI2_PeriodeMin.Value) > DateTime.Today)
                        //    HargaBeli = d.HargaAcuan.Value;

                        d.HargaRupiah = HargaBeli;

                        db.SubmitChanges();
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }
    protected void calcPGRO()
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";


        foreach (TableRow row in tbti.Rows)
        {
            decimal HargaBeli = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfpgro" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("pgro_" + index.ToString());
                var d = db.ResellPG.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        //var pr = db.SalesOrderPG.SingleOrDefault(p => p.IDProduct == d.IDProduct &&
                        //p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                        HargaBeli = Convert.ToDecimal(txt.Text);
                        //if (HargaBeli > d.HargaAcuan.Value && d.SalesOrder.Tgl.Value.AddDays((pr.TI2_PeriodeMin.Value*7)) > DateTime.Today)
                        //HargaBeli = d.HargaAcuan.Value;

                        d.HargaRupiah = HargaBeli;

                        db.SubmitChanges();
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }
    //Custom Row
    protected void Sub_HeaderRO(int ItemIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.ItemIndex(ItemIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tbti.Rows.Add(tr);
    }
    protected void Sub_TotalRO(decimal Total, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 7;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-ms\" style=\"font-weight:bold;\">" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);


        tbti.Rows.Add(tr);
    }

    protected string TD_HyperLinkRO(int ItemIndex, int ID, int IDProduct, bool isVisible = true)
    {
        string x = string.Empty;
        string href = string.Empty;


        if (ItemIndex == 0)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-sm btn-info\" href=\"DJ_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <span class=\"glyphicon glyphicon-search\" aria-hidden=\"true\"></span> ";
            x += "  </a>";
        }
        else if (ItemIndex == 1)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-sm btn-info\" href=\"PG_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <span class=\"glyphicon glyphicon-search\" aria-hidden=\"true\"></span> ";
            x += "  </a>";
        }
        else if (ItemIndex == 2)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-sm btn-info\" href=\"GJ_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <span class=\"glyphicon glyphicon-search\" aria-hidden=\"true\"></span> ";
            x += "  </a>";
        }
        else if (ItemIndex == 3)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-sm btn-info\" href=\"LD_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <span class=\"glyphicon glyphicon-search\" aria-hidden=\"true\"></span> ";
            x += "  </a>";
        }
        x += "</div>";

        if (isVisible)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-sm btn-danger\" href=\"javascript:goDeleteResell('" + myID + "', '" + ItemIndex + "','" + ID + "')\">";
            x += "      <span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span> ";
            x += "  </a>";
            x += "</div>";
        }

        return x;
    }

    protected string DocQC_DJ(ResellDJ r, bool isVisible)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-sm btn-warning " + (isVisible ? "" : "disabled") + " \" href=\"Resell_AddDocQC.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=1\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else
        {
            x += "  <a class=\"btn btn-sm btn-info " + (isVisible ? "" : "disabled") + " \" href=\"Resell_AddDocQC.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=1\">";
            x += r.DocQCDJ.Nomor;
            x += "  </a>";
        }

        return x;
    }

    protected string DocQC_PG(ResellPG r, bool isVisible)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-sm btn-warning " + (isVisible ? "" : "disabled") + " \" href=\"Resell_AddDocQC.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=2\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else
        {
            x += "  <a class=\"btn btn-sm btn-info " + (isVisible ? "" : "disabled") + " \" href=\"Resell_AddDocQC.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=2\">";
            x += r.DocQCPG.Nomor;
            x += "  </a>";
        }

        return x;
    }

    protected string DocQC_GJ(ResellGJ r, bool isVisible)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-sm btn-warning " + (isVisible ? "" : "disabled") + " \" href=\"Resell_AddDocQC.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=3\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else
        {
            x += "  <a class=\"btn btn-sm btn-info " + (isVisible ? "" : "disabled") + " \" href=\"Resell_AddDocQC.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=3\">";
            x += r.DocQCGJ.Nomor;
            x += "  </a>";
        }

        return x;
    }
    protected string DocQC_LD(ResellLD r, bool isVisible)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-sm btn-warning " + (isVisible ? "" : "disabled") + " \" href=\"Resell_AddDocQC.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=4\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else
        {
            x += "  <a class=\"btn btn-sm btn-info " + (isVisible ? "" : "disabled") + " \" href=\"Resell_AddDocQC.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=4\">";
            x += r.DocQCLD.Nomor;
            x += "  </a>";
        }

        return x;
    }

    protected decimal GetTGPPGBeliNew(int IDProduct)
    {
        decimal GoldPrice = 0m;

        string query = "sp_get_tgp_jual_beli_plu";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID_PRODUCT", SqlDbType.Int){ Value = IDProduct },
            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "PG" }
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        if (dt.Rows.Count == 1)
        {
            GoldPrice = Convert.ToDecimal(dt.Rows[0]["HARGA_BELI"]);
        }

        return GoldPrice;
    }

    protected decimal GetTGPPGBeli(StockProductPG r)
    {
    	decimal GoldPrice = 0m;
		DataTable dt =  Db.Rs("EXEC sp_get_tgp_jual_beli ", Db.Cnn_DB);
		if(dt.Rows.Count > 0)
		{
            if (r.CharGoldLevel.ID == 21)
            {
                //Preloved Good 37,5 % PLGPG375
                GoldPrice = Convert.ToDecimal(dt.Rows[13][2]);
            }
            else if (r.CharGoldLevel.ID == 22)
            {
                //Preloved Excellent 37,5 % PLEPG375
                GoldPrice = Convert.ToDecimal(dt.Rows[14][2]);
            }
            else if (r.CharGoldLevel.ID == 23)
            {
                //Preloved Good 75 % PLGPG75
                GoldPrice = Convert.ToDecimal(dt.Rows[15][2]);
            }
            else if (r.CharGoldLevel.ID == 24)
            {
                //Preloved Excellent 75 % PLEPG75
                GoldPrice = Convert.ToDecimal(dt.Rows[16][2]);
            }
            else if (r.CharGoldLevel.ID == 25)
            {
                //Preloved Good CZ 37,5 % PLGCZ375
                GoldPrice = Convert.ToDecimal(dt.Rows[17][2]);
            }
            else if (r.CharGoldLevel.ID == 26)
            {
                //Preloved Excellent CZ 37,5 % PLECZ375
                GoldPrice = Convert.ToDecimal(dt.Rows[18][2]);
            }
            else if (r.CharGoldLevel.ID == 27)
            {
                //Preloved Good CZ 75 % PLGCZ75
                GoldPrice = Convert.ToDecimal(dt.Rows[19][2]);
            }
            else if (r.CharGoldLevel.ID == 28)
            {
                //Preloved Excellent CZ 75 % PLECZ75
                GoldPrice = Convert.ToDecimal(dt.Rows[20][2]);
            }
            else if (r.CharGoldLevel.ID == 13)
			{
				//CZC 37,5
				GoldPrice = Convert.ToDecimal(dt.Rows[11][2]);
			}
			/*else if(r.CharGoldLevel.ID == 14)/
			{
				//PGC 37,5
				GoldPrice = Convert.ToDecimal(dt.Rows[10][2]);
			}
			*/
			//Update tgl 19 Oktober 2024, mappingan sama dengan CZC 37,5
			else if(r.CharGoldLevel.ID == 14)
			{ 
				//PGC 37,5
				GoldPrice = Convert.ToDecimal(dt.Rows[11][2]);
			}
			else if (r.CharGoldLevel.ID == 8 && r.CharFrameColor.ID == 2 && r.CharProductItem.ID == 59) // chain white 37.5
			{
				//IDPricing = 36; //PG / CHAIN KADAR 37,5 - Pricing
				GoldPrice = Convert.ToDecimal(dt.Rows[0][2].ToString());
			}
			else if (r.CharGoldLevel.ID == 8 && r.CharProductItem.ID != 59) // non chain 37.5
			{
				//IDPricing = 36; //PG / CHAIN KADAR 37,5 - Pricing
				GoldPrice = Convert.ToDecimal(dt.Rows[12][2].ToString());
			}
			else if (r.CharGoldLevel.ID == 6 || r.CharGoldLevel.ID == 7) // 6 = zirconia 37,5 ; 7 = zirconia 75 
			{
				if (r.CharGoldLevel.ID == 6) 
				{ 
					//IDPricing = 34; 
					GoldPrice = Convert.ToDecimal(dt.Rows[8][2].ToString());
				}
				else if (r.CharGoldLevel.ID == 7) 
				{
					//IDPricing = 35; 
					GoldPrice = Convert.ToDecimal(dt.Rows[6][2].ToString());
				}
			}
			else if (r.CharGoldLevel.ID == 9 || r.CharGoldLevel.ID == 10)
			{
				GoldPrice = Convert.ToDecimal(dt.Rows[7][2].ToString());
			}
			else if (r.KadarLogam >= 99m)
			{
				if (r.ProductItem == 59) // 59 = Chain
					GoldPrice = Convert.ToDecimal(dt.Rows[5][2].ToString());//IDPricing = 15;
				else
					GoldPrice = Convert.ToDecimal(dt.Rows[5][2].ToString());//IDPricing = 16;
			}
			else
			{
				if (r.ProductItem != 59) // 59 = Chain
				{
					if (r.FrameColor == 1)
						GoldPrice = Convert.ToDecimal(dt.Rows[3][2].ToString());//IDPricing = 11;
					else
						GoldPrice = Convert.ToDecimal(dt.Rows[4][2].ToString());//IDPricing = 12;
				}
				else
				{
					if (r.FrameColor == 1)
						GoldPrice = Convert.ToDecimal(dt.Rows[1][2].ToString());//IDPricing = 13;
					else
						GoldPrice = Convert.ToDecimal(dt.Rows[2][2].ToString());//IDPricing = 14;
				}
			}
		}	
		return GoldPrice;
    }

    protected string me_Info(ResellDJ r, bool whitelist = false)
    {
        string meinfo = "";
        bool flag = false;
        var inv = (from p in db.SalesOrderDJ where p.SalesOrder.Unpaid == 0m && p.SalesOrder.TglLunas != null && p.IDProduct == r.IDProduct select p)
                .OrderByDescending(p => p.SalesOrder.TglLunas)
                .FirstOrDefault()
                ;
        int melimit = myTIdayslimit; // default ME 30 days

        try
        {
            if (inv != null)
            {
                if (inv.MEPeriodeMaxDay.HasValue)
                {
                    // jika ada ME by hari
                    melimit = inv.MEPeriodeMaxDay.Value;

                }
                else if (inv.MEPeriodeMax.HasValue)
                {
                    // default ME mengambil satuan bulan di konversi ke hari
                    melimit = inv.MEPeriodeMax.Value * 30;

                }

            }

        }
        catch (Exception) { }

        if (inv != null)
        {
            flag = (Convert.ToDateTime(inv.SalesOrder.TglLunas).AddDays(melimit) < System.DateTime.Now) ? true : false;
            string xcolor = "green";
            string xsign = "ok";

            if (flag || melimit == 0)
            {
                xcolor = "red";
                xsign = "remove";
            }

            if (whitelist)
            {
                meinfo += "<span style='color:blue;' class='glyphicon glyphicon-ok' aria-hidden='true'></span>";
                meinfo += "<span style='color:blue;'>&nbsp;PLU Whitelisted</span><br />";
            }
            else
            {
                if (melimit > 0)
                {
                    meinfo += "<span style='color:black;'>+ " + melimit + " Days</span><br />";
                    meinfo += "<span style='color:" + xcolor + ";' class='glyphicon glyphicon-" + xsign + "' aria-hidden='true'></span>";
                    meinfo += "<span style='color:" + xcolor + ";'>&nbsp;Merchandise Exchange</span>";
                }
                else
                {
                    //meinfo += "<span style='color:red;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>";
                    //meinfo += "<span style='color:red;'>&nbsp;No Resell, No Exchange</span><br />";
                    meinfo += "";
                }
            }

        }

        return meinfo;
    }

    protected string me_Info(ResellPG r, bool whitelist = false)
    {
        string meinfo = "";
        bool flag = false;
        var inv = (from p in db.SalesOrderPG where p.SalesOrder.Unpaid == 0m && p.SalesOrder.TglLunas != null && p.IDProduct == r.IDProduct select p)
                .OrderByDescending(p => p.SalesOrder.TglLunas)
                .FirstOrDefault()
                ;
        int melimit = myTIdayslimit; // default ME 30 days

        try
        {
            if (inv != null)
            {
                //if (inv.MEPeriodeMaxDay != null)
                //{
                //    if (inv.MEPeriodeMaxDay.Value != null)
                //    {
                //        // jika ada ME by hari
                //        melimit = inv.MEPeriodeMaxDay.Value;
                //    }

                //}
                //else 
                if (inv.MEPeriodeMax != null)
                {
                    if (inv.MEPeriodeMax != null)
                    {
                        // default ME mengambil satuan bulan di konversi ke hari
                        melimit = inv.MEPeriodeMax.Value * 30;
                    }

                }

            }

        }
        catch (Exception) { }

        if (inv != null)
        {
            flag = (Convert.ToDateTime(inv.SalesOrder.TglLunas).AddDays(melimit) < System.DateTime.Now) ? true : false;
            string xcolor = "green";
            string xsign = "ok";

            if (flag || melimit == 0)
            {
                xcolor = "red";
                xsign = "remove";
            }

            if (whitelist)
            {
                meinfo += "<span style='color:blue;' class='glyphicon glyphicon-ok' aria-hidden='true'></span>";
                meinfo += "<span style='color:blue;'>&nbsp;PLU Whitelisted</span><br />";
            }
            else
            {
                if (melimit > 0)
                {
                    meinfo += "<span style='color:black;'>+ " + melimit + " Days</span><br />";
                    meinfo += "<span style='color:" + xcolor + ";' class='glyphicon glyphicon-" + xsign + "' aria-hidden='true'></span>";
                    meinfo += "<span style='color:" + xcolor + ";'>&nbsp;Merchandise Exchange</span>";
                }
                else
                {
                    //meinfo += "<span style='color:red;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>";
                    //meinfo += "<span style='color:red;'>&nbsp;No Resell, No Exchange</span><br />";
                    meinfo += "";
                }
            }

        }

        return meinfo;
    }

    protected string me_Info(ResellGJ r, bool whitelist = false)
    {
        string meinfo = "";
        bool flag = false;
        var inv = (from p in db.SalesOrderGJ where p.SalesOrder.Unpaid == 0m && p.SalesOrder.TglLunas != null && p.IDProduct == r.IDProduct select p)
                .OrderByDescending(p => p.SalesOrder.TglLunas)
                .FirstOrDefault()
                ;
        int melimit = myTIdayslimit; // default ME 30 days

        try
        {
            if (inv != null)
            {
                //if (inv.MEPeriodeMaxDay != null)
                //{
                //    if (inv.MEPeriodeMaxDay.Value != null)
                //    {
                //        // jika ada ME by hari
                //        melimit = inv.MEPeriodeMaxDay.Value;
                //    }

                //}
                //else 
                if (inv.MEPeriodeMax != null)
                {
                    if (inv.MEPeriodeMax != null)
                    {
                        // default ME mengambil satuan bulan di konversi ke hari
                        melimit = inv.MEPeriodeMax.Value * 30;
                    }

                }

            }

        }
        catch (Exception) { }

        if (inv != null)
        {
            flag = (Convert.ToDateTime(inv.SalesOrder.TglLunas).AddDays(melimit) < System.DateTime.Now) ? true : false;
            string xcolor = "green";
            string xsign = "ok";

            if (flag || melimit == 0)
            {
                xcolor = "red";
                xsign = "remove";
            }

            if (whitelist)
            {
                meinfo += "<span style='color:blue;' class='glyphicon glyphicon-ok' aria-hidden='true'></span>";
                meinfo += "<span style='color:blue;'>&nbsp;PLU Whitelisted</span><br />";
            }
            else
            {
                if (melimit > 0)
                {
                    meinfo += "<span style='color:black;'>+ " + melimit + " Days</span><br />";
                    meinfo += "<span style='color:" + xcolor + ";' class='glyphicon glyphicon-" + xsign + "' aria-hidden='true'></span>";
                    meinfo += "<span style='color:" + xcolor + ";'>&nbsp;Merchandise Exchange</span>";
                }
                else
                {
                    //meinfo += "<span style='color:red;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>";
                    //meinfo += "<span style='color:red;'>&nbsp;No Resell, No Exchange</span><br />";
                    meinfo += "";
                }
            }

        }

        return meinfo;
    }

    protected string me_Info(ResellLD r, bool whitelist = false)
    {
        string meinfo = "";
        bool flag = false;
        var inv = (from p in db.SalesOrderLD where p.SalesOrder.Unpaid == 0m && p.SalesOrder.TglLunas != null && p.IDProduct == r.IDProduct select p)
                .OrderByDescending(p => p.SalesOrder.TglLunas)
                .FirstOrDefault()
                ;
        int melimit = myTIdayslimit; // default ME 30 days

        try
        {
            if (inv != null)
            {
                //if (inv.MEPeriodeMaxDay != null)
                //{
                //    if (inv.MEPeriodeMaxDay.Value != null)
                //    {
                //        // jika ada ME by hari
                //        melimit = inv.MEPeriodeMaxDay.Value;
                //    }

                //}
                //else 
                if (inv.MEPeriodeMax != null)
                {
                    if (inv.MEPeriodeMax != null)
                    {
                        // default ME mengambil satuan bulan di konversi ke hari
                        melimit = inv.MEPeriodeMax.Value * 30;
                    }

                }

            }

        }
        catch (Exception) { }

        if (inv != null)
        {
            flag = (Convert.ToDateTime(inv.SalesOrder.TglLunas).AddDays(melimit) < System.DateTime.Now) ? true : false;
            string xcolor = "green";
            string xsign = "ok";

            if (flag || melimit == 0)
            {
                xcolor = "red";
                xsign = "remove";
            }

            if (whitelist)
            {
                meinfo += "<span style='color:blue;' class='glyphicon glyphicon-ok' aria-hidden='true'></span>";
                meinfo += "<span style='color:blue;'>&nbsp;PLU Whitelisted</span><br />";
            }
            else
            {
                if (melimit > 0)
                {
                    meinfo += "<span style='color:black;'>+ " + melimit + " Days</span><br />";
                    meinfo += "<span style='color:" + xcolor + ";' class='glyphicon glyphicon-" + xsign + "' aria-hidden='true'></span>";
                    meinfo += "<span style='color:" + xcolor + ";'>&nbsp;Merchandise Exchange</span>";
                }
                else
                {
                    //meinfo += "<span style='color:red;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>";
                    //meinfo += "<span style='color:red;'>&nbsp;No Resell, No Exchange</span><br />";
                    meinfo += "";
                }
            }

        }

        return meinfo;
    }

    #endregion
	
	protected bool isFancyCertifiedDJ(int idproduct)
    {
        bool IsFancyCert = false;
        string query = "DECLARE @ISFANCYCERT BIT = 0 " +
            "IF EXISTS(SELECT 1 FROM StockProductDJ A JOIN StockProductDJ_Stone1B B ON A.ID = B.IDForm " +
            "JOIN Stone1B C ON B.IDStone = C.ID JOIN Parcel106 D ON C.Parcel06 = D.ID " +
            "WHERE (RIGHT(LTRIM(RTRIM(REPLACE(C.Nama, ' (PR)', ''))), 1) IN ('S','B','A') " +
			"AND D.NamaKode = 'NF' " +
			"AND ISNULL(B.GIA,'') <> '' " + 
			") AND A.ID = @ID) " +
            "BEGIN " +
            "SET @ISFANCYCERT = 1 " +
            "END " +
            "SELECT @ISFANCYCERT [ISFANCYCERT] ";
        List<SqlParameter> parameter = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = idproduct }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameter, CommandType.Text);

        if(dt.Rows.Count == 1)
        {
            IsFancyCert = Convert.ToBoolean(dt.Rows[0]["ISFANCYCERT"]);
        }

        return IsFancyCert;
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                cmd.ExecuteNonQuery();
            }
        }
    }

    //Single Decimal
    protected decimal SingleDecimal(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        decimal x = 0;
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                foreach (var y in param)
                {
                    cmd.Parameters.Add(y);
                }
                sqlCnn.Open();
                x = Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
        return x;
    }
}