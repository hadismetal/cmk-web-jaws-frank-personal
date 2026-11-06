using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

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

    protected int myID { get { return App.QueryInt(this); } }
    protected string myConfirm { get { return Request.QueryString["confirm"]; } }

    protected string hrefmain { get { return "Sales.aspx"; } }
    protected string hrefmain2 { get { return "Sales_Data.aspx"; } }
    protected string hrefmainfull { get { return "Sales/Sales.aspx"; } }
    protected string hrefpattern { get { return "?id=" + myID; } }
    protected string hrefpattern2 { get { return "&id=" + myID; } }

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

        Voucher.isDoubleRewardText(lit_promo_membership);

        div_unfinished.Visible = false;
        div_finished.Visible = false;
        redirect_payment.Visible = false;
        redirect_register.Visible = false;
        div_btn_keterangan.Visible = true;

        var x = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (x != null)
        {
            if (x.Draft)
            {
                div_unfinished.Visible = true;
                div_finished.Visible = false;

                if (!IsPostBack)
                {
                    //HyperLink
                    addpck.HRef += hrefpattern;
                    addsv.HRef += hrefpattern;

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
                        if (c != null)
                        {
                            Membership = c.NoMember; // tiering membership update from STAMPS

                            try
                            {
                                Db.Execute("EXEC sp_get_membership_customer 'UPDATE', '" + x.ID.ToString() + "' ",Db.Cnn_DB);
                            }
                            catch{}
                        }
                        // c null (SO tanpa DataCustomer) -> Membership tetap default "Non-Member"
                    }
                    else if (x.DataCustomer != null)
                    {
                        var c = cs.Mscustomer.SingleOrDefault(p => p.M_kode == x.DataCustomer.NoCustomer);
                        if (c != null)
                        {
                            var m = cs.Msmembership.SingleOrDefault(p => p.M_kode == c.M_typemember);

                            if (m != null) { Membership = m.M_nama + " (" + m.M_kode + ")"; }
                        }
                    }
                    // x.DataCustomer null -> Membership tetap default "Non-Member"

                    string Lokasi = "";
                    string BrandCompany = "";
                    if (x.TipeLokasi == 1)
                    {
                        var loc = db.LocOutlet.SingleOrDefault(p => p.ID == x.IDLokasi);
                        if (loc != null)
                        {
                            Lokasi = loc.Nama;
                            BrandCompany = (loc.CompanyBrand != null) ? loc.CompanyBrand.Nama : "-";
                        }
                        else
                        {
                            Lokasi = "-";
                            BrandCompany = "-";
                        }
                    }
                    else if (x.TipeLokasi == 2)
                    {
                        var loc = db.LocExhibition.SingleOrDefault(p => p.ID == x.IDLokasi);
                        if (loc != null)
                        {
                            Lokasi = loc.Nama;
                            BrandCompany = (loc.CompanyBrand != null) ? loc.CompanyBrand.Nama : "-";
                        }
                        else
                        {
                            Lokasi = "-";
                            BrandCompany = "-";
                        }
                    }

                    brand.InnerHtml = BrandCompany;
                    membership.InnerHtml = Membership;
                    location.InnerHtml = Lokasi;
                    description.InnerHtml = Str.HtmlBreak(x.Keterangan);
                    jpjr.InnerHtml = (x.DataSales != null) ? x.DataSales.Nama : "-";
                    customer.InnerHtml = (x.DataCustomer != null) ? (x.DataCustomer.Nama + " (" + x.DataCustomer.NoCustomer + ")") : "-";
                    operator_input.InnerHtml = x.Operator + " (" + Str.TglJam(x.DraftDate) + ")";
                    tgl.InnerHtml = Str.Tgl(x.Tgl);
                    txtReceiver.InnerHtml = (String.IsNullOrEmpty(x.CustomerReferenceNama) ? "" : x.CustomerReferenceNama);
                }

                fillTable(sender, e, x);
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
    protected void fillTable(object sender, EventArgs e, SalesOrder x)
    {
        int QTY = 0;
        decimal GT = 0m;
        decimal GT2 = 0m;

        // fillMembership dan fillItemDJ/PG/GJ/LD sudah null-tolerant terhadap c/m (lihat
        // "if (member != null)" di fillMembership dan "else if (m != null)" di fillItemDJ dkk),
        // jadi aman membiarkan keduanya null kalau x.DataCustomer/c tidak ditemukan.
        var c = (x.DataCustomer != null) ? cs.Mscustomer.SingleOrDefault(p => p.M_kode == x.DataCustomer.NoCustomer) : null;
        var m = (c != null) ? cs.Msmembership.SingleOrDefault(p => p.M_kode == c.M_typemember) : null;

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
        fillItemDJ(myID, ref QTY, ref GT, ref GT2, m, disc1, disc2);
        fillItemPG(myID, ref QTY, ref GT, ref GT2, m, disc1, disc2);
        fillItemGJ(myID, ref QTY, ref GT, ref GT2, m, disc1, disc2);
        fillItemLD(myID, ref QTY, ref GT, ref GT2, m, disc1, disc2);
        fillPackaging(x);
        fillSouvenir(x);
        fillSouvenirGJ();
        fillSouvenirDJ();
        fillSouvenirLD();
        fillSouvenirPG();

        TextBox txtbox = new TextBox();

        //Grand-Total
        if (QTY == 0)
        {
            review.Disabled = true;
            App.TableNoResult(tb);
        }
        else
        {
            Sub_Total(GT, GT2, true);
            review.Disabled = false;
        }

        tb.Focus();
        // calc() (calcDJ/PG/GJ/LD + savePackaging/saveSouvenir + SubmitChanges) sebelumnya
        // dipanggil TANPA SYARAT di sini - jalan di SETIAP request ke halaman ini termasuk GET
        // biasa dan reload berulang, bukan cuma saat ada edit yang benar-benar perlu disimpan.
        // Terbukti aman di-skip saat myConfirm=="1" (mode tampilan konfirmasi, read-only):
        //   1. Grand Total yang tampil ke user dihitung SEPENUHNYA oleh fillItemDJ/PG/GJ/LD
        //      (lewat Sub_Total(), baris di atas) SEBELUM titik ini - calc() tidak pernah
        //      menghasilkan apa pun yang terlihat di layar, jadi skip di sini tidak mengubah
        //      tampilan sama sekali.
        //   2. Semua TextBox diskon di-set Enabled=false saat myConfirm=="1" (lihat pola
        //      "(myConfirm == "1") ? false : ..." di TD_Disc_* dkk) - tidak ada yang bisa
        //      diedit di mode ini, jadi tidak ada "perubahan postback" yang perlu di-capture.
        //   3. review_Click (tombol yang membawa user ke mode confirm) SUDAH memanggil calc()
        //      sendiri sebelum redirect ke "?confirm=1" - data sudah tersimpan benar sebelum
        //      halaman confirm pernah dilihat.
        // Direplikasi dari perbaikan performa/deadlock di Palace (calc() mencakup cascade
        // SubmitChanges yang mahal saat dipanggil berulang tanpa alasan di halaman konfirmasi).
        if (myConfirm != "1")
        {
            calc();
        }
        checkValid();
    }

    protected void checkValid()
    {
        bool disableReviewButton = false;
        bool xvalid_so = checkItemSales(myID);
        bool xvalid_nominal_so = checkNilaiSales(myID);

        if (!(xvalid_so && xvalid_nominal_so)) { disableReviewButton = true; div_err.Visible = true; }
        if (!xvalid_so) { next_err_nosales.Visible = true; }
        if (!xvalid_nominal_so) { next_err_nilaisales.Visible = true; }

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

    protected List<decimal> fillMembershipStamps(SalesOrder d, ref decimal disc1, ref decimal disc2)
    {
        string spname = "[SP_STAMPS_GET_MASTER_MEMBERSHIP_TIERING]";
        string query = " exec " + spname
                     + Str.isString(d.DataCustomer.NoMember, false)
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_IN);

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
    // Kelas & helper bantu untuk N+1 fix di fillItemDJ/PG/GJ/LD — prefetch data Promo & Bundle
    // Souvenir SEKALI per SO (bukan per baris item, seperti sebelumnya). PromoDJ/PG/GJ punya
    // struktur kolom identik jadi dipakai bersama lewat 1 kelas. TIDAK ada logic kalkulasi
    // diskon/promo/souvenir yang diubah — cuma sumber datanya, dari query-per-baris jadi
    // lookup-dari-dictionary.
    private class PromoInfo
    {
        public bool? StatusDiscNormal;
        public bool? StatusDiscMembership;
        public DateTime? TglStart;
        public DateTime? TglExpire;
        public int? Tipe;
        public string Nama;
        public decimal? PromoHarga;
        public decimal? PoinReward;
        public decimal? DiscountNominal;
        public decimal? DiscountPersen;
        public int? StatusASP;
        public int ExtraRingSize;
    }

    private class BundleSouvenirInfo
    {
        public string Nama;
        public DateTime? TglStart;
        public DateTime? TglExpire;
    }

    // Prefetch untuk rantai lazy-load r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem/Category/Level
    // + StockProductDJ_Stone1B.Any() — dipakai berulang di fillItemDJ dan calcDJ. Setiap navigasi ini
    // sebelumnya trigger query tambahan PER BARIS (N+1 "known-family" yang sudah dikonfirmasi sebelumnya).
    private class CharProductInfoDJ
    {
        public string ItemNama;
        public string CategoryNamaKode;
        public string LevelNamaKode;
        public bool HasStone1B;
    }
    private Dictionary<int, CharProductInfoDJ> PrefetchCharProductInfoDJ(List<int> productIds)
    {
        Dictionary<int, CharProductInfoDJ> result = new Dictionary<int, CharProductInfoDJ>();
        if (productIds == null || productIds.Count == 0) return result;

        string idList = string.Join(",", productIds.Distinct());
        string query = @"
            SELECT cp.ID AS IDProduct, ci.Nama AS ItemNama, cc.NamaKode AS CategoryNamaKode, cl.NamaKode AS LevelNamaKode,
                CAST(CASE WHEN EXISTS(SELECT 1 FROM StockProductDJ_Stone1B s WHERE s.IDForm = cp.ID) THEN 1 ELSE 0 END AS BIT) AS HasStone1B
            FROM StockProductDJ_CharProduct cp
            LEFT JOIN CharProductItem ci ON ci.ID = cp.ProductItem
            LEFT JOIN CharProductCategory cc ON cc.ID = cp.Productcategory
            LEFT JOIN CharProductLevel cl ON cl.ID = cp.ProductLevel
            WHERE cp.ID IN (" + idList + ")";
        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>(), CommandType.Text);
        foreach (DataRow row in dt.Rows)
        {
            result[Convert.ToInt32(row["IDProduct"])] = new CharProductInfoDJ
            {
                ItemNama = (row["ItemNama"] == DBNull.Value) ? null : row["ItemNama"].ToString(),
                CategoryNamaKode = (row["CategoryNamaKode"] == DBNull.Value) ? null : row["CategoryNamaKode"].ToString(),
                LevelNamaKode = (row["LevelNamaKode"] == DBNull.Value) ? null : row["LevelNamaKode"].ToString(),
                HasStone1B = Convert.ToBoolean(row["HasStone1B"])
            };
        }
        return result;
    }

    // Sama seperti CharProductInfoDJ, untuk rantai r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem
    // (fillItemGJ) dan StockProductGJ_CharProduct.ProductCategory (calcGJ, dipakai DLC.getASPGJParameter).
    private class CharProductInfoGJ
    {
        public string ItemNama;
        public int? ProductCategory;
    }
    private Dictionary<int, CharProductInfoGJ> PrefetchCharProductInfoGJ(List<int> productIds)
    {
        Dictionary<int, CharProductInfoGJ> result = new Dictionary<int, CharProductInfoGJ>();
        if (productIds == null || productIds.Count == 0) return result;

        string idList = string.Join(",", productIds.Distinct());
        string query = @"
            SELECT cp.ID AS IDProduct, ci.Nama AS ItemNama, cp.ProductCategory
            FROM StockProductGJ_CharProduct cp
            LEFT JOIN CharProductItem ci ON ci.ID = cp.ProductItem
            WHERE cp.ID IN (" + idList + ")";
        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>(), CommandType.Text);
        foreach (DataRow row in dt.Rows)
        {
            result[Convert.ToInt32(row["IDProduct"])] = new CharProductInfoGJ
            {
                ItemNama = (row["ItemNama"] == DBNull.Value) ? null : row["ItemNama"].ToString(),
                ProductCategory = (row["ProductCategory"] == DBNull.Value) ? (int?)null : Convert.ToInt32(row["ProductCategory"])
            };
        }
        return result;
    }

    private Dictionary<int, PromoInfo> PrefetchPromoInfo(string detailTable, string promoTable, bool filterDraft)
    {
        Dictionary<int, PromoInfo> result = new Dictionary<int, PromoInfo>();
        string draftFilter = filterDraft ? " AND pj.Draft = 0" : "";
        string query = "SELECT pd.IDProduct, pj.StatusDiscNormal, pj.StatusDiscMembership, pj.TglStart, pj.TglExpire, "
                     + "pj.Tipe, pj.Nama, pj.PromoHarga, pj.PoinReward, pj.DiscountNominal, pj.DiscountPersen, pj.StatusASP, ISNULL(pj.ExtraRingSize, 0) AS ExtraRingSize "
                     + "FROM " + detailTable + " pd JOIN " + promoTable + " pj ON pj.ID = pd.IDForm "
                     + "WHERE pj.ApprovalNama IS NOT NULL AND pj.TglStart <= @Today AND pj.TglExpire >= @Today" + draftFilter;
        List<SqlParameter> param = new List<SqlParameter>() { new SqlParameter("@Today", SqlDbType.DateTime) { Value = DateTime.Today } };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);
        foreach (DataRow pr in dt.Rows)
        {
            int idProduct = Convert.ToInt32(pr["IDProduct"]);
            // t.First() di kode asli cuma ambil salah satu match kalau lebih dari satu —
            // konsisten, ambil yang pertama ditemukan di sini juga.
            if (!result.ContainsKey(idProduct))
            {
                result[idProduct] = new PromoInfo
                {
                    StatusDiscNormal = (pr["StatusDiscNormal"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(pr["StatusDiscNormal"]),
                    StatusDiscMembership = (pr["StatusDiscMembership"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(pr["StatusDiscMembership"]),
                    TglStart = (pr["TglStart"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(pr["TglStart"]),
                    TglExpire = (pr["TglExpire"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(pr["TglExpire"]),
                    Tipe = (pr["Tipe"] == DBNull.Value) ? (int?)null : Convert.ToInt32(pr["Tipe"]),
                    Nama = pr["Nama"].ToString(),
                    PromoHarga = (pr["PromoHarga"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(pr["PromoHarga"]),
                    PoinReward = (pr["PoinReward"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(pr["PoinReward"]),
                    DiscountNominal = (pr["DiscountNominal"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(pr["DiscountNominal"]),
                    DiscountPersen = (pr["DiscountPersen"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(pr["DiscountPersen"]),
                    StatusASP = (pr["StatusASP"] == DBNull.Value) ? (int?)null : Convert.ToInt32(pr["StatusASP"]),
                    ExtraRingSize = Convert.ToInt32(pr["ExtraRingSize"])
                };
            }
        }
        return result;
    }

    private Dictionary<int, BundleSouvenirInfo> PrefetchBundleSouvenirInfo(int tipeProduct)
    {
        Dictionary<int, BundleSouvenirInfo> result = new Dictionary<int, BundleSouvenirInfo>();
        string query = @"
            SELECT bp.IDProduct, bi.Nama, bi.TglStart, bi.TglExpire
            FROM Bundle_ItemPasangan bp
            JOIN BundleItem bi ON bi.ID = bp.IDForm
            WHERE bi.TglExpire > @Today AND bi.StatusActive = 1 AND bp.TipeProduct = @TipeProduct";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@Today", SqlDbType.DateTime) { Value = DateTime.Today },
            new SqlParameter("@TipeProduct", SqlDbType.Int) { Value = tipeProduct }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);
        foreach (DataRow sr in dt.Rows)
        {
            int idProduct = Convert.ToInt32(sr["IDProduct"]);
            if (!result.ContainsKey(idProduct))
            {
                result[idProduct] = new BundleSouvenirInfo
                {
                    Nama = sr["Nama"].ToString(),
                    TglStart = (sr["TglStart"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(sr["TglStart"]),
                    TglExpire = (sr["TglExpire"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(sr["TglExpire"])
                };
            }
        }
        return result;
    }

    // batched version of isFancyCertifiedDJ() - was called once per row (each opening its own
    // connection); this checks all of a SO's distinct products in a single round-trip instead.
    private Dictionary<int, bool> PrefetchFancyCertifiedDJ(List<int> productIds)
    {
        Dictionary<int, bool> result = new Dictionary<int, bool>();
        if (productIds == null || productIds.Count == 0) return result;

        string idList = string.Join(",", productIds.Distinct());
        string query = "SELECT DISTINCT A.ID FROM StockProductDJ A JOIN StockProductDJ_Stone1B B ON A.ID = B.IDForm " +
            "JOIN Stone1B C ON B.IDStone = C.ID JOIN Parcel106 D ON C.Parcel06 = D.ID " +
            "WHERE (RIGHT(LTRIM(RTRIM(REPLACE(C.Nama, ' (PR)', ''))), 1) IN ('S','B','A') " +
            "AND D.NamaKode = 'NF' " +
            "AND ISNULL(B.GIA,'') <> '' " +
            ") AND A.ID IN (" + idList + ")";
        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>(), CommandType.Text);
        foreach (DataRow row in dt.Rows)
        {
            result[Convert.ToInt32(row["ID"])] = true;
        }
        return result;
    }

    protected void fillItemDJ(int SO_ID, ref int QTY, ref decimal GT, ref decimal GT2, Msmembership m, decimal disc1, decimal disc2)
    {
        var rs = from p in db.SalesOrderDJ
                 where p.IDForm == SO_ID
                 orderby p.ID ascending
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;
        decimal ST2 = 0m;

        if (iQty > 0)
        {
            // Dulu IQueryable tak dimaterialisasi — vcr.Count()/vcrdtl.Count()/foreach(vcrdtl) di bawah
            // masing-masing jadi round-trip DB tersendiri PER BARIS. Materialize sekali di sini,
            // sisanya jadi LINQ-to-Objects di memori (predikat/hasil tetap sama).
            var vcr = (from p in db.SalesVoucher
                      where p.IDSalesOrder == SO_ID && p.ProductType == 1 && p.ProductID != null
                      select p).ToList();

            Dictionary<int, PromoInfo> promoByProduct = PrefetchPromoInfo("PromoDJ_Detail", "PromoDJ", true);
            Dictionary<int, BundleSouvenirInfo> souvenirByProduct = PrefetchBundleSouvenirInfo(1);
            var djProductIds = db.SalesOrderDJ.Where(p => p.IDForm == SO_ID).Select(p => p.IDProduct).Distinct().ToList();
            Dictionary<int, bool> fancyCertByProduct = PrefetchFancyCertifiedDJ(djProductIds);
            // N+1 "known-family" — lihat komentar di PrefetchCharProductInfoDJ.
            Dictionary<int, CharProductInfoDJ> charProductByProductDJ = PrefetchCharProductInfoDJ(djProductIds);
            // Query ini TIDAK bergantung pada baris sama sekali (WHERE-nya fixed) — diambil sekali,
            // sama seperti sudah dilakukan di calcDJ.
            string feeExtraRingSizeShared = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(Harga,0) AS INT) [Harga] FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0", Db.Cnn_DB).ToString();

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
				if (isBranded == false)
				{
					bool isFancyCert;
					fancyCertByProduct.TryGetValue(r.IDProduct, out isFancyCert);
					isBranded = isFancyCert;
				}
                //bool isRegular = Pricing.isDiamondRegular(r.StockProductDJ.Nomor);
                bool isNett = Pricing.isNett(r.StockProductDJ.Nomor);

                //lit_test_text.Text += (r.StockProductDJ.Nomor + " - " + ((isNett) ? "True" : "False") + "<br />"); // test

                bool statusNoPromoNormal = false;
                bool statusNoPromoMembership = false;
				string extra_ring_size = "0";
				int TipePromo = 0;
				decimal HargaPromo = 0m;

                // promo dj — lookup dari prefetch, bukan query per baris lagi.
                PromoInfo discP;
                if (promoByProduct.TryGetValue(r.IDProduct, out discP))
                {
                    isPromo = true;
                    statusNoPromoNormal = (discP.StatusDiscNormal != null) ? discP.StatusDiscNormal.Value : false;
                    statusNoPromoMembership = (discP.StatusDiscMembership != null) ? discP.StatusDiscMembership.Value : false;

                    extra_ring_size = discP.ExtraRingSize.ToString();

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

                // souvenir — lookup dari prefetch, bukan query per baris lagi.
                BundleSouvenirInfo souv;
                if (souvenirByProduct.TryGetValue(r.IDProduct, out souv))
                {
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

                // N+1 "known-family" - lihat komentar di PrefetchCharProductInfoDJ.
                CharProductInfoDJ charProductInfo;
                charProductByProductDJ.TryGetValue(r.IDProduct, out charProductInfo);

                td = new TableCell();
                string xPLU = r.StockProductDJ.Nomor;
                string xItem = (charProductInfo != null) ? charProductInfo.ItemNama : null;
                string bundlingld = CheckKombinasiDJLD(r.IDProduct,1);
                td.Text = TD_PLU(0, xPLU, xItem, statusPromo, statusSouvenir);
				td.Text += "<label class=\"text-sm alert-info\">" + bundlingld + "</label>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p class='text-ms text-price' style='font-weight:bold;'>" + Convert.ToDecimal(HargaPLU).ToString("N0") + "</p>";
                tr.Cells.Add(td);

                // disc normal
                decimal disc_nd_p = 0m;
                decimal disc_nd_n = 0m;

                if (tipeproduk == "gj" || (tipeproduk == "dj" && charProductInfo != null && charProductInfo.CategoryNamaKode == "SETTING"))
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
						if(TipePromo == 3)
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
                if (tipeproduk == "pg" || tipeproduk == "gj" || (tipeproduk == "dj" && charProductInfo != null && charProductInfo.CategoryNamaKode == "SETTING") || (tipeproduk == "dj" && charProductInfo != null && charProductInfo.LevelNamaKode == "MTG" && !charProductInfo.HasStone1B && charProductInfo.CategoryNamaKode != "SNI"))
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
                td = Td_Disc_Bank(tipeproduk, index, r.ID, isPromo, val_disc_bank_n, val_disc_bank_p,HargaPLU - disc_nd_n - DiscountPromoNominal - disc_ms_n - disc_vc_n);
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

                if (r.DiscountRound == null) { r.DiscountRound = 0m; }

                if (r.DiscountRound == 0m) { }
                else if (r.DiscountRound > 0m)
                {
                    if (r.DiscountRound < 1000 && (totalHargaPLU_wo_floor % 1000 != 0))
                    {
                        disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                        disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p);

                        r.DiscountRound = disc_rn_n;
                    }
                    else if (r.DiscountRound > 1000m)
                    {
                        if ((totalHargaPLU_wo_floor - r.DiscountRound) % 1000 != 0)
                        {
                            decimal sthousand = Math.Floor((r.DiscountRound.Value) / 1000) * 1000;
                            disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                            disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p) + sthousand;

                            r.DiscountRound = disc_rn_n;
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
				string fee_extra_ring_size = feeExtraRingSizeShared;
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
                 orderby p.ID ascending
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;
        decimal ST2 = 0m;

        if (iQty > 0)
        {
            // Dulu IQueryable tak dimaterialisasi — vcr.Count()/vcrdtl.Count()/foreach(vcrdtl) di bawah
            // masing-masing jadi round-trip DB tersendiri PER BARIS. Materialize sekali di sini,
            // sisanya jadi LINQ-to-Objects di memori (predikat/hasil tetap sama).
            var vcr = (from p in db.SalesVoucher
                      where p.IDSalesOrder == SO_ID && p.ProductType == 2 && p.ProductID != null
                      select p).ToList();

            // Beda dari DJ: query asli PromoPG_Detail TIDAK filter Draft — jadi filterDraft=false
            // di sini supaya hasilnya identik dengan sebelumnya.
            Dictionary<int, PromoInfo> promoByProduct = PrefetchPromoInfo("PromoPG_Detail", "PromoPG", false);
            // Souvenir PG pakai TipeProduct == 3 (bukan 2 — dikonfirmasi dari kode asli).
            Dictionary<int, BundleSouvenirInfo> souvenirByProduct = PrefetchBundleSouvenirInfo(3);
            // Query ini TIDAK bergantung pada baris sama sekali (WHERE-nya fixed) — diambil sekali,
            // sama seperti sudah dilakukan di calcPG.
            string feeExtraRingSizeShared = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(Harga,0) AS INT) [Harga] FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0", Db.Cnn_DB).ToString();

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

                // promo pg — lookup dari prefetch, bukan query per baris lagi.
                PromoInfo discP;
                if (promoByProduct.TryGetValue(r.IDProduct, out discP))
                {
                    isPromo = true;
                    statusNoPromoNormal = (discP.StatusDiscNormal != null) ? discP.StatusDiscNormal.Value : false;
                    statusNoPromoMembership = (discP.StatusDiscMembership != null) ? discP.StatusDiscMembership.Value : false;
					extra_ring_size = discP.ExtraRingSize.ToString();

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

                // souvenir — lookup dari prefetch, bukan query per baris lagi.
                BundleSouvenirInfo souv;
                if (souvenirByProduct.TryGetValue(r.IDProduct, out souv))
                {
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

                //td = new TableCell();
                string xPLU = r.StockProductPG.Nomor;
                string xItem = r.StockProductPG.CharProductItem.Nama;
                decimal xWeight = Convert.ToDecimal(r.StockProductPG.NettoWeight);
                decimal xTimbang = Db.SingleDecimal("SELECT TOP 1 ISNULL(BERATTIMBANG,0.00) FROM SALESORDERPG WHERE ID = '" + r.ID + "' ", Db.Cnn_DB);
                decimal xWeightTimbang = (xTimbang == 0m) ? xWeight : Convert.ToDecimal(xTimbang);
                td = TD_PLU_PG_Cell(0, xPLU, xItem, statusPromo, statusSouvenir, xWeight, xWeightTimbang, r.ID, Convert.ToBoolean(r.StockProductPG.FixRate));
                //td.Text = TD_PLU(0, xPLU, xItem, statusPromo, statusSouvenir);
                //td.Text = TD_PLU_PG_Cell(0, xPLU, xItem, statusPromo, statusSouvenir, xWeight, xWeightTimbang, r.ID);
                //td.Style.Add("text-align", "left");
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
                td = Td_Disc_Bank(tipeproduk, index, r.ID, isPromo, val_disc_bank_n, val_disc_bank_p,HargaPLU - disc_nd_n - DiscountPromoNominal - disc_ms_n - disc_vc_n);
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

                if (r.DiscountRound == null) { r.DiscountRound = 0m; }

                if (r.DiscountRound == 0m) { }
                else if (r.DiscountRound > 0m)
                {
                    if (r.DiscountRound < 1000 && (totalHargaPLU_wo_floor % 1000 != 0))
                    {
                        disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                        disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p);

                        r.DiscountRound = disc_rn_n;
                    }
                    else if (r.DiscountRound > 1000m)
                    {
                        if ((totalHargaPLU_wo_floor - r.DiscountRound) % 1000 != 0)
                        {
                            decimal sthousand = Math.Floor((r.DiscountRound.Value) / 1000) * 1000;
                            disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                            disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p) + sthousand;

                            r.DiscountRound = disc_rn_n;
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
				string fee_extra_ring_size = feeExtraRingSizeShared;
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
                 orderby p.ID ascending
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;
        decimal ST2 = 0m;

        if (iQty > 0)
        {
            // Dulu IQueryable tak dimaterialisasi — vcr.Count()/vcrdtl.Count()/foreach(vcrdtl) di bawah
            // masing-masing jadi round-trip DB tersendiri PER BARIS. Materialize sekali di sini,
            // sisanya jadi LINQ-to-Objects di memori (predikat/hasil tetap sama).
            var vcr = (from p in db.SalesVoucher
                      where p.IDSalesOrder == SO_ID && p.ProductType == 3 && p.ProductID != null
                      select p).ToList();

            // Sama seperti PG: query asli PromoGJ_Detail TIDAK filter Draft.
            Dictionary<int, PromoInfo> promoByProduct = PrefetchPromoInfo("PromoGJ_Detail", "PromoGJ", false);
            // Souvenir GJ pakai TipeProduct == 2 (dikonfirmasi dari kode asli).
            Dictionary<int, BundleSouvenirInfo> souvenirByProduct = PrefetchBundleSouvenirInfo(2);
            // N+1 "known-family" — lihat komentar di PrefetchCharProductInfoGJ.
            var gjProductIds = db.SalesOrderGJ.Where(p => p.IDForm == SO_ID).Select(p => p.IDProduct).Distinct().ToList();
            Dictionary<int, CharProductInfoGJ> charProductByProductGJ = PrefetchCharProductInfoGJ(gjProductIds);

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

                // promo gj — lookup dari prefetch, bukan query per baris lagi.
                PromoInfo discP;
                if (promoByProduct.TryGetValue(r.IDProduct, out discP))
                {
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

                // souvenir — lookup dari prefetch, bukan query per baris lagi.
                BundleSouvenirInfo souv;
                if (souvenirByProduct.TryGetValue(r.IDProduct, out souv))
                {
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

                // N+1 "known-family" — lihat komentar di PrefetchCharProductInfoGJ.
                CharProductInfoGJ charProductInfoGJ;
                charProductByProductGJ.TryGetValue(r.IDProduct, out charProductInfoGJ);

                td = new TableCell();
                string xPLU = r.StockProductGJ.Nomor;
                string xItem = (charProductInfoGJ != null) ? charProductInfoGJ.ItemNama : null;
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
                td = Td_Disc_Bank(tipeproduk, index, r.ID, isPromo, val_disc_bank_n, val_disc_bank_p,HargaPLU - disc_nd_n - DiscountPromoNominal - disc_ms_n - disc_vc_n);
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

                if (r.DiscountRound == null) { r.DiscountRound = 0m; }

                if (r.DiscountRound == 0m) { }
                else if (r.DiscountRound > 0m)
                {
                    if (r.DiscountRound < 1000 && (totalHargaPLU_wo_floor % 1000 != 0))
                    {
                        disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                        disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p);

                        r.DiscountRound = disc_rn_n;
                    }
                    else if (r.DiscountRound > 1000m)
                    {
                        if ((totalHargaPLU_wo_floor - r.DiscountRound) % 1000 != 0)
                        {
                            decimal sthousand = Math.Floor((r.DiscountRound.Value) / 1000) * 1000;
                            disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                            disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p) + sthousand;

                            r.DiscountRound = disc_rn_n;
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
                 orderby p.ID ascending
                 select p;

        int index = 1;
        int iQty = rs.Count();
        decimal ST = 0;
        decimal ST2 = 0;

        if (iQty > 0)
        {
            // Dulu IQueryable tak dimaterialisasi — vcr.Count()/vcrdtl.Count()/foreach(vcrdtl) di bawah
            // masing-masing jadi round-trip DB tersendiri PER BARIS. Materialize sekali di sini,
            // sisanya jadi LINQ-to-Objects di memori (predikat/hasil tetap sama).
            var vcr = (from p in db.SalesVoucher
                      where p.IDSalesOrder == SO_ID && p.ProductType == 4 && p.ProductID != null
                      select p).ToList();

            // LD tidak punya fitur promo (lihat komentar "belum ada fitur promo ld" di bawah),
            // jadi cuma prefetch souvenir. Souvenir LD pakai TipeProduct == 4.
            Dictionary<int, BundleSouvenirInfo> souvenirByProduct = PrefetchBundleSouvenirInfo(4);

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

                // souvenir — lookup dari prefetch, bukan query per baris lagi.
                BundleSouvenirInfo souv;
                if (souvenirByProduct.TryGetValue(r.IDProduct, out souv))
                {
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
                var DJLD = from x in db.StockProductDJLD
                where x.IDProductLD == r.IDProduct
                select x;

                if (DJLD.Any()) // This will execute the query
                {
                    lt.Text = "";
                }
                else
                {
                    lt.Text = TD_HyperLink(3, r.ID, r.IDProduct);
                }

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
                string bundlingdj = CheckKombinasiDJLD(r.IDProduct,4);
                td.Text = TD_PLU(0, xPLU, xItem, statusPromo, statusSouvenir);
				td.Text += "<label class=\"text-sm alert-info\">" + bundlingdj + "</label>";
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
                td = Td_Disc_Bank(tipeproduk, index, r.ID, isPromo, val_disc_bank_n, val_disc_bank_p,HargaPLU - disc_nd_n - DiscountPromoNominal - disc_ms_n - disc_vc_n);
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

                if (r.DiscountRound == null) { r.DiscountRound = 0m; }

                if (r.DiscountRound == 0m) { }
                else if (r.DiscountRound > 0m)
                {
                    if (r.DiscountRound < 1000 && (totalHargaPLU_wo_floor % 1000 != 0))
                    {
                        disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                        disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p);

                        r.DiscountRound = disc_rn_n;
                    }
                    else if (r.DiscountRound > 1000m)
                    {
                        if ((totalHargaPLU_wo_floor - r.DiscountRound) % 1000 != 0)
                        {
                            decimal sthousand = Math.Floor((r.DiscountRound.Value) / 1000) * 1000;
                            disc_rn_p = Math.Floor(Convert.ToDecimal(totalHargaPLU_wo_floor) / 1000) * 1000;
                            disc_rn_n = (Convert.ToDecimal(totalHargaPLU_wo_floor) - disc_rn_p) + sthousand;

                            r.DiscountRound = disc_rn_n;
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
        // Raw parameterized query (bukan SP) — JOIN ke Packaging + StockActualPackaging
        // dilakukan sekali di DB (bukan lazy-load per baris seperti versi LINQ sebelumnya, yang
        // menghasilkan N+1: r.Packaging.Nama/.Kode dan
        // r.Packaging.StockActualPackaging.SingleOrDefault(...) masing-masing 1 query per baris).
        // Query di-tulis SEKALI di sini (bukan string-builder dinamis) supaya teksnya konsisten
        // karakter-per-karakter — plan cache SQL Server tetap bisa dipakai ulang antar-panggilan.
        string query = @"
            SELECT sop.ID, sop.IDProduct, sop.Qty, pkg.Nama, pkg.Kode, ISNULL(sap.InHand, 0) AS InHand
            FROM SalesOrderPackaging sop
            JOIN Packaging pkg ON pkg.ID = sop.IDProduct
            LEFT JOIN StockActualPackaging sap
                ON sap.IDProduct = sop.IDProduct AND sap.TipeLokasi = @TipeLokasi AND sap.IDLokasi = @IDLokasi
            WHERE sop.IDForm = @IDForm
            ORDER BY sop.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID },
            new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = x.TipeLokasi },
            new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = x.IDLokasi }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);

        int index = 1;

        //Sub-Header
        if (dt.Rows.Count > 0)
        {
            Sub_HeaderPackaging();

            foreach (DataRow r in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(9, Convert.ToInt32(r["ID"]), Convert.ToInt32(r["IDProduct"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["Nama"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r["Kode"] == DBNull.Value) ? "" : r["Kode"].ToString();
                tr.Cells.Add(td);

                // SP pakai LEFT JOIN + ISNULL(InHand,0) — beda dari LINQ asli yang pakai
                // SingleOrDefault(...) lalu langsung .InHand tanpa null-check (akan NRE kalau
                // baris StockActualPackaging untuk lokasi ini tidak ada). Ini perbaikan
                // null-handling yang disengaja, bukan cuma migrasi apa adanya.
                string maxQtyInput = Convert.ToInt32(r["InHand"]).ToString();

                td = new TableCell();
                td.Text = maxQtyInput;
                tr.Cells.Add(td);

                bool hasQty = r["Qty"] != DBNull.Value;
                decimal qty = hasQty ? Convert.ToDecimal(r["Qty"]) : 0m;

                TextBox txt = new TextBox();
                txt.ID = "pc_" + index.ToString();
                txt.Attributes["style"] = "width:50px; text-align:center;";
                txt.Attributes["type"] = "number";
                txt.Attributes["min"] = "1";
                txt.Attributes["max"] = maxQtyInput;
                txt.Text = hasQty ? qty.ToString("N0") : "1";
                //txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                //txt.AutoPostBack = true;
                //txt.TextChanged += new EventHandler(Discount_TextChanged);
                txt.Enabled = (myConfirm == "1") ? false : true;

                HiddenField hf = new HiddenField();
                hf.ID = "hfpc" + index.ToString();
                hf.Value = r["ID"].ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                //td.Controls.Add(txt);
                // Sama seperti LINQ asli (r.Qty.Value.ToString("N0") tanpa null-check di titik
                // ini) — tapi di sini pakai fallback 0m kalau null, bukan crash.
                td.Text = qty.ToString("N0");
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
        // Raw parameterized query — sama alasannya dengan fillPackaging: eliminasi N+1 dari
        // r.Souvenir.Nama/.Kode dan r.Souvenir.StockActualSouvenir.SingleOrDefault(...) per baris.
        string query = @"
            SELECT sos.ID, sos.IDProduct, sos.Qty, sv.Nama, sv.Kode, ISNULL(sas.InHand, 0) AS InHand
            FROM SalesOrderSouvenir sos
            JOIN Souvenir sv ON sv.ID = sos.IDProduct
            LEFT JOIN StockActualSouvenir sas
                ON sas.IDProduct = sos.IDProduct AND sas.TipeLokasi = @TipeLokasi AND sas.IDLokasi = @IDLokasi
            WHERE sos.IDForm = @IDForm AND (sos.TipeProduct = 0 OR sos.TipeProduct IS NULL)
            ORDER BY sos.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID },
            new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = x.TipeLokasi },
            new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = x.IDLokasi }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);

        int index = 1;

        //Sub-Header
        if (dt.Rows.Count > 0)
        {
            Sub_HeaderSouvenir();

            foreach (DataRow r in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(10, Convert.ToInt32(r["ID"]), Convert.ToInt32(r["IDProduct"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["Nama"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r["Kode"] == DBNull.Value) ? "" : r["Kode"].ToString();
                tr.Cells.Add(td);

                // LEFT JOIN + ISNULL(InHand,0) di SP — beda null-handling dari LINQ asli, lihat
                // catatan yang sama di fillPackaging.
                string maxQtyInput = Convert.ToInt32(r["InHand"]).ToString();

                td = new TableCell();
                td.Text = maxQtyInput;
                tr.Cells.Add(td);

                bool hasQty = r["Qty"] != DBNull.Value;
                decimal qty = hasQty ? Convert.ToDecimal(r["Qty"]) : 0m;

                TextBox txt = new TextBox();
                txt.ID = "sv_" + index.ToString();
                txt.Attributes["style"] = "width:50px; text-align:center;";
                txt.Attributes["type"] = "number";
                txt.Attributes["min"] = "1";
                txt.Attributes["max"] = maxQtyInput;
                txt.Text = hasQty ? qty.ToString("N0") : "1";
                //txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                //txt.AutoPostBack = true;
                //txt.TextChanged += new EventHandler(Discount_TextChanged);
                txt.Enabled = (myConfirm == "1") ? false : true;

                HiddenField hf = new HiddenField();
                hf.ID = "hfsv" + index.ToString();
                hf.Value = r["ID"].ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                //td.Controls.Add(txt);
                td.Text = qty.ToString("N0");
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
        // Raw parameterized query. Null-handling: LEFT JOIN + ISNULL(...,'-') supaya kalau baris
        // char-product/CharProductItem tidak ada, item TETAP muncul di list (fallback "-", sama
        // pola dengan fillSouvenirLD) — bukan diam-diam hilang (INNER JOIN) atau NRE crash
        // (perilaku LINQ asli yang akses r.q.StockProductGJ_CharProduct.CharProductItem.Nama
        // tanpa null-check).
        string query = @"
            SELECT sos.ID, sos.IDProductSouvenir, sos.Qty, q.Nomor, ISNULL(ci.Nama, '-') AS ItemNama
            FROM SalesOrderSouvenir sos
            JOIN StockProductGJ q ON q.ID = sos.IDProductSouvenir
            LEFT JOIN StockProductGJ_CharProduct cp ON cp.ID = q.ID
            LEFT JOIN CharProductItem ci ON ci.ID = cp.ProductItem
            WHERE sos.IDForm = @IDForm AND sos.TipeProduct = 2
            ORDER BY sos.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);

        int index = 1;

        //Sub-Header
        if (dt.Rows.Count > 0) Sub_HeaderSouvenirGJ();

        foreach (DataRow r in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(11, Convert.ToInt32(r["ID"]), Convert.ToInt32(r["IDProductSouvenir"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Nomor"].ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["ItemNama"].ToString();
            tr.Cells.Add(td);

            bool hasQty = r["Qty"] != DBNull.Value;

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
            txt.Text = hasQty ? Convert.ToDecimal(r["Qty"]).ToString("N0") : "1";
            //txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt.AutoPostBack = true;
            //txt.TextChanged += new EventHandler(Discount_TextChanged);
            txt.Enabled = (myConfirm == "1") ? false : true;

            HiddenField hf = new HiddenField();
            hf.ID = "hfsvgj" + index.ToString();
            hf.Value = r["ID"].ToString();
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
        // Raw parameterized query. Catatan null-handling sama seperti fillSouvenirGJ — LEFT
        // JOIN + ISNULL(...,'-') supaya item tetap muncul walau char-product tidak ada.
        string query = @"
            SELECT sos.ID, sos.IDProductSouvenir, sos.Qty, q.Nomor, ISNULL(ci.Nama, '-') AS ItemNama
            FROM SalesOrderSouvenir sos
            JOIN StockProductDJ q ON q.ID = sos.IDProductSouvenir
            LEFT JOIN StockProductDJ_CharProduct cp ON cp.ID = q.ID
            LEFT JOIN CharProductItem ci ON ci.ID = cp.ProductItem
            WHERE sos.IDForm = @IDForm AND sos.TipeProduct = 1
            ORDER BY sos.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);

        int index = 1;

        //Sub-Header
        if (dt.Rows.Count > 0) Sub_HeaderSouvenirDJ();

        foreach (DataRow r in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(12, Convert.ToInt32(r["ID"]), Convert.ToInt32(r["IDProductSouvenir"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Nomor"].ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["ItemNama"].ToString();
            tr.Cells.Add(td);

            bool hasQty = r["Qty"] != DBNull.Value;

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
            txt.Text = hasQty ? Convert.ToDecimal(r["Qty"]).ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.Enabled = (myConfirm == "1") ? false : true;

            HiddenField hf = new HiddenField();
            hf.ID = "hfsvdj" + index.ToString();
            hf.Value = r["ID"].ToString();
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
        // Raw parameterized query. Tidak perlu JOIN tambahan untuk Nama (kolom itu memang
        // hardcode "-" di versi asli), jadi tidak ada N+1 di sini sebenarnya — dikonversi cuma
        // untuk konsistensi pola dengan GJ/DJ/PG.
        string query = @"
            SELECT sos.ID, sos.IDProductSouvenir, sos.Qty, q.Nomor
            FROM SalesOrderSouvenir sos
            JOIN StockProductLD_Stone1B q ON q.ID = sos.IDProductSouvenir
            WHERE sos.IDForm = @IDForm AND sos.TipeProduct = 4
            ORDER BY sos.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);

        int index = 1;

        //Sub-Header
        if (dt.Rows.Count > 0) Sub_HeaderSouvenirLD();

        foreach (DataRow r in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(13, Convert.ToInt32(r["ID"]), Convert.ToInt32(r["IDProductSouvenir"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Nomor"].ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "-";
            tr.Cells.Add(td);

            bool hasQty = r["Qty"] != DBNull.Value;

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
            txt.Text = hasQty ? Convert.ToDecimal(r["Qty"]).ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.Enabled = (myConfirm == "1") ? false : true;

            HiddenField hf = new HiddenField();
            hf.ID = "hfsvld" + index.ToString();
            hf.Value = r["ID"].ToString();
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
        // Raw parameterized query. StockProductPG punya FK langsung ke CharProductItem (tidak
        // lewat tabel _CharProduct terpisah seperti GJ/DJ), jadi cuma 1 JOIN tambahan.
        // Null-handling: LEFT JOIN + ISNULL(...,'-'), sama seperti catatan di fillSouvenirGJ/DJ.
        string query = @"
            SELECT sos.ID, sos.IDProductSouvenir, sos.Qty, q.Nomor, ISNULL(ci.Nama, '-') AS ItemNama
            FROM SalesOrderSouvenir sos
            JOIN StockProductPG q ON q.ID = sos.IDProductSouvenir
            LEFT JOIN CharProductItem ci ON ci.ID = q.ProductItem
            WHERE sos.IDForm = @IDForm AND sos.TipeProduct = 3
            ORDER BY sos.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);

        int index = 1;

        //Sub-Header
        if (dt.Rows.Count > 0) Sub_HeaderSouvenirPG();

        foreach (DataRow r in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(14, Convert.ToInt32(r["ID"]), Convert.ToInt32(r["IDProductSouvenir"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Nomor"].ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["ItemNama"].ToString();
            tr.Cells.Add(td);

            bool hasQty = r["Qty"] != DBNull.Value;

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
            txt.Text = hasQty ? Convert.ToDecimal(r["Qty"]).ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.Enabled = (myConfirm == "1") ? false : true;

            HiddenField hf = new HiddenField();
            hf.ID = "hfsvpg" + index.ToString();
            hf.Value = r["ID"].ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }

    //Custom Row
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
    //Custom Row
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
    //Custom Row
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


    //Custom Row

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
		// ddlpromo1/ddlpromo2 dulu masing-masing panggil PromoBankList sendiri dengan hargarupiah
		// yang sama persis - 2 round-trip identik per baris. Query sekali, isi kedua dropdown dari
		// DataTable yang sama.
		DataTable dtPromoBank = PromoBankTable(hargarupiah);
		FillPromoBankList(ddlpromo1, dtPromoBank);

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
		FillPromoBankList(ddlpromo2, dtPromoBank);

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
		
		if(tipeproduk == "dj")
		{
			int promo = Db.SingleInteger("SELECT TOP 1 ISNULL(IDPromoDetail,0) [IDPromoDetail] FROM SalesOrderDJ WHERE ID = " + ID.ToString(), Db.Cnn_DB);
			ddlpromo1.SelectedValue = promo.ToString();
			ddlpromo2.SelectedValue = promo.ToString();
		}
		else if(tipeproduk == "pg")
		{
			int promo = Db.SingleInteger("SELECT TOP 1 ISNULL(IDPromoDetail,0) [IDPromoDetail] FROM SalesOrderPG WHERE ID = " + ID.ToString(), Db.Cnn_DB);
			ddlpromo1.SelectedValue = promo.ToString();
			ddlpromo2.SelectedValue = promo.ToString();
		}
		else if(tipeproduk == "gj")
		{
			int promo = Db.SingleInteger("SELECT TOP 1 ISNULL(IDPromoDetail,0) [IDPromoDetail] FROM SalesOrderGJ WHERE ID = " + ID.ToString(), Db.Cnn_DB);
			ddlpromo1.SelectedValue = promo.ToString();
			ddlpromo2.SelectedValue = promo.ToString();
		}
		else if(tipeproduk == "ld")
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
	
	private DataTable PromoBankTable(decimal hargarupiah = 0m)
	{
		string query = "SELECT A.ID, A.Nama, MinValue, MaxValue, Discount, B.Nama [PromoName] FROM PromoDetail A " +
			"JOIN Promo B ON A.IDForm = B.ID " +
			"WHERE B.Draft = 0 AND B.Disable = 0 AND GETDATE() BETWEEN StartDate AND EndDate AND " + Convert.ToInt64(hargarupiah).ToString() + " BETWEEN MinValue AND MaxValue ";
        return Db.Rs(query, Db.Cnn_DB);
	}
	private void FillPromoBankList(DropDownList c, DataTable dt)
	{
		c.Items.Add(new ListItem("No Promo","0"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString() + " (" + item["PromoName"] + " : " + Convert.ToDecimal(item["Discount"]).ToString("N0") + ")";
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
		c.Items.Add(new ListItem("Lain Lain","-1"));
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
        /// Revisi dari CRM Add Membership Redemption bisa berlaku untuk seluruh jenis barang (2018-12-27) kecuali pg (2022-03-04)
        if (tipeproduk == "dj" || /*tipeproduk == "pg" ||*/ tipeproduk == "gj" || tipeproduk == "ld")
        {
            int itemtype = 0;
            if (tipeproduk == "dj") { itemtype = 1; }
            //if (tipeproduk == "pg") { itemtype = 2; }
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
                url1 = "Sales_VoucherStampsAdd.aspx?id=" + myID + "&itemtype=" + itemtype + "&itemid=" + ID;
                url2 = "Sales_VoucherStampsVoid.aspx?id=" + myID + "&itemtype=" + itemtype + "&itemid=" + ID;

            }
            else
            {
                url1 = "Sales_VoucherAdd.aspx?id=" + myID + "&itemtype=" + itemtype + "&itemid=" + ID;
                url2 = "Sales_VoucherVoid.aspx?id=" + myID + "&itemtype=" + itemtype + "&itemid=" + ID;

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
	
	protected string TD_PLU_PG(int ItemIndex, string PLU, string Item, string Promo, string Souvenir, string WeightPG, string WeightTimbangPG, int IdSalesOrderPG)
    {
        string x = string.Empty;
        
        x += "<div>";
        x += "<span class='text-bold pull-left' style='font-size:15px;'>" + PLU + "</span>";
        x += "</div>";
        x += "<div class='pull-right'>" + Promo + Souvenir + "</div>";
        x += "<div style='clear: both; border-bottom:solid 1px silver;'></div>";
        x += "<div>";
        x += "<span class='text-muted pull-left'>" + Str.CapUpper(Item) + "</span>";
        x += "</div>";
        
		/*x += "<div class='form-horizontal'><div class='form-group'>";
        x += "<label for='beratnett' class='control-label col-md-4'>Berat Barcode:</label>";
        x += "<div class='col-md-5'><input type='text' id='beratnett"+PLU+"' style='weight:50px' disabled class='form-control' value='" + WeightPG + "' /><p>gr</p></div>";
        x += "</div><div class='form-group'>";
        x += "<label for='berattimbang' class='control-label col-md-4'>Berat Timbang:</label>";
        x += "<div class='col-md-5'><input type='text' id='berattimbang"+PLU+"' style='weight:50px' class='form-control' value='" + WeightTimbangPG + "' onblur='onBlurWeight"+PLU+"(this);' /><p>gr</p></div>";
		x += "<div class=' col-md-offset-1 col-md-10'><small style='display:none;color:red' id='warningweightpg"+PLU+"'>*Berat Timbang di luar batas toleransi +- 0.02 gr.</small></div>";
        x += "</div></div>";*/
		
		x += "<table class='pull-right'><tbody>";
		x += "<tr><td>Berat Barcode</td><td>:</td><td><input type='text' id='beratnett" + PLU + "' style='width:50px' disabled value='" + WeightPG + "' /></td><td><p>gr</p></td></tr>";
		x += "<tr><td>Berat Timbang</td><td>:</td><td><input type='text' id='berattimbang" + PLU + "' style='width:50px' value='" + WeightTimbangPG + "' onblur='onBlurWeight"+PLU.Replace("-","_")+"(this);' /></td><td><p>gr</p></td></tr>";
		x += "</tbody></table>";
        
		x += "<script type='text/javascript'>";
        x += "function onBlurWeight"+PLU.Replace("-","_")+"(e){ var koma = e.value.split(',').pop().length; var nilaiberat = e.value; var pembanding = document.getElementById('beratnett"+PLU+"').value; if(isNaN(e.value.replace(',','.'))) {alert('Harus Angka / Decimal.');} else{ if(koma >= 4 ){alert('Maksimal 3 digit dibelakang koma.');} else{ if(nilaiberat != pembanding){ if( (parseFloat(nilaiberat.replace(',','.')) <= parseFloat(pembanding.replace(',','.')) + 0.12 ) && (parseFloat(nilaiberat.replace(',','.')) >= parseFloat(pembanding.replace(',','.')) - 0.12  )){ window.location.href = \"Sales_DataPGBeratTimbangSave.aspx?id="+myID+ "&idsopg="+IdSalesOrderPG+ "&weightpg=\"+nilaiberat+\"  \"; } else{ document.getElementById('warningweightpg"+PLU+"').style.display = 'block'; document.getElementById('berattimbang"+PLU+"').value = pembanding;  window.location.href = \"Sales_DataPGBeratTimbangSave.aspx?id="+myID+ "&idsopg="+IdSalesOrderPG+ "&weightpg=\"+pembanding+\"  \";}} else{ window.location.href = \"Sales_DataPGBeratTimbangSave.aspx?id="+myID+ "&idsopg="+IdSalesOrderPG+ "&weightpg=\"+pembanding+\"  \"; }}; }}";
        x += "</script>";

        return x;
    }

    protected TableCell TD_PLU_PG_Cell(int ItemIndex, string PLU, string Item, string Promo, string Souvenir, decimal WeightPG, decimal WeightTimbangPG, int IdSalesOrderPG, bool FixRate)
    {
        TableCell td = new TableCell();
        td.Style.Add("text-align", "left");

        // --- Bagian atas: PLU + badge (pure Literal, aman) ---
        Literal litHeader = new Literal();
        litHeader.Text = "<div>";
        litHeader.Text += "<span class='text-bold pull-left' style='font-size:15px;'>" + PLU + "</span>";
        litHeader.Text += "</div>";
        litHeader.Text += "<div class='pull-left'>" + Promo + Souvenir + "</div>";
        litHeader.Text += "<div style='clear:both; border-bottom:solid 1px silver;'></div>";
        litHeader.Text += "<div><span class='text-muted pull-left'>" + Str.CapUpper(Item) + "</span></div>";
        td.Controls.Add(litHeader);

        if (FixRate)
        {
            Literal litFixedRate = new Literal();
            litFixedRate.Text = "<br />";
            litFixedRate.Text += "<div style='color:#d9534f; font-size:11px; text-align:left; clear:both;'>Fix Rate - Berat Timbang tidak dapat diubah. Jika Beda Berat Silakan Hubungi Pusat.</div>";
            td.Controls.Add(litFixedRate);
        }
        else
        {
            // --- Berat Barcode (readonly, full HTML aman) ---
            Literal litBarcode = new Literal();
            litBarcode.Text = "<table class='pull-left'><tbody>";
            litBarcode.Text += "<tr>";
            litBarcode.Text += "<td>Berat Barcode</td><td>:</td>";
            litBarcode.Text += "<td><input type='number' step='0.01' min = '0' id='beratnett" + PLU + "' style='width:50px' disabled value='" + WeightPG.ToString("N2").Replace(",", ".") + "' /></td>";
            litBarcode.Text += "<td>gr</td>";
            litBarcode.Text += "</tr>";
            litBarcode.Text += "</tbody></table>";
            td.Controls.Add(litBarcode);

            // --- Baris Berat Timbang: Panel bungkus supaya tidak split HTML ---
            Panel pnlTimbang = new Panel();
            pnlTimbang.Attributes["style"] = "display:table; float:left;";

            Literal litLabelTimbang = new Literal();
            litLabelTimbang.Text = "<table class='pull-left'><tbody><tr><td>Berat Timbang</td><td>:</td><td>";
            pnlTimbang.Controls.Add(litLabelTimbang);

            // TextBox berat timbang — AutoPostBack, TANPA onblur JS
            TextBox txtBerat = new TextBox();
            string safeId = PLU.Replace("-", "_").Replace(".", "_");
            txtBerat.ID = "berattimbang_" + safeId;                // unik per baris
            txtBerat.Text = WeightTimbangPG.ToString("N2").Replace(",", ".");
            txtBerat.Attributes["style"] = "width:50px;";
            txtBerat.Attributes["type"] = "number";
            txtBerat.Attributes["step"] = "0.01";
            txtBerat.Attributes["min"] = "0";

            bool isReadOnly = (myConfirm == "1") || FixRate;
            txtBerat.Enabled = !isReadOnly;

            if (!isReadOnly)
            {
                txtBerat.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                txtBerat.AutoPostBack = true;
                txtBerat.Attributes["onchange"] = "showOverlay();";
                txtBerat.TextChanged += new EventHandler(WeightPGTimbang_TextChanged);
            }
            pnlTimbang.Controls.Add(txtBerat);

            // HiddenField konteks: "idSalesOrderPG|beratBarcode"
            HiddenField hfCtx = new HiddenField();
            hfCtx.ID = "hfBeratCtx_" + safeId;
            hfCtx.Value = IdSalesOrderPG + "|" + WeightPG.ToString("N2").Replace(",", ".");
            hfCtx.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            pnlTimbang.Controls.Add(hfCtx);

            Literal litSuffix = new Literal();
            litSuffix.Text = "</td><td>gr</td></tr></tbody></table>";
            pnlTimbang.Controls.Add(litSuffix);

            td.Controls.Add(pnlTimbang);

            if (FixRate)
            {
                Literal litFixRate = new Literal();
                litFixRate.Text = "<div style='color:#d9534f; font-size:11px; text-align:left; clear:both;'>Fix Rate - Berat Timbang tidak dapat diubah.</div>";
                td.Controls.Add(litFixRate);
            }
        }

        return td;   // <-- TableCell siap, JANGAN set .Text sesudah ini
    }

    protected void WeightPGTimbang_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        string hfId = txt.ID.Replace("berattimbang_", "hfBeratCtx_");
        HiddenField hf = txt.Parent.FindControl(hfId) as HiddenField;
        if (hf == null) return;

        string[] parts = hf.Value.Split('|');
        if (parts.Length != 2) return;

        int idSoPG;
        if (!int.TryParse(parts[0], out idSoPG)) return;

        // Parse input safely: support "3,60" and "3.60"
        decimal beratTimbang;
        string raw = (txt.Text ?? "").Trim().Replace(",", ".");

        if (!decimal.TryParse(
                raw,
                System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign,
                System.Globalization.CultureInfo.InvariantCulture,
                out beratTimbang))
        {
            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "alert",
                "alert('Format berat tidak valid. Contoh benar: 3,60');", true);
            return;
        }

        // Optional: 3 decimal max
        if (decimal.Round(beratTimbang, 3) != beratTimbang)
        {
            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "alert",
                "alert('Maksimal 3 digit desimal.');", true);
            return;
        }

        string query = "sp_update_berattimbang_sales_order_pg";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int) { Value = idSoPG },
            new SqlParameter("@BERATTIMBANG", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 3,
                Value = beratTimbang // pass decimal, NOT txt.Text
            },
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        bool success = dt.Rows.Count > 0 && Convert.ToBoolean(dt.Rows[0]["Success"]);
        string error = success ? "" : dt.Rows[0]["Error"].ToString();

        if (!success)
        {
            DataTable dtback = Rs("SELECT TOP 1 ISNULL(BeratTimbang, 0) [BeratTimbang] FROM SalesOrderPG WHERE ID = @ID", Db.Cnn_DB,
            new List<SqlParameter>
            {
                new SqlParameter("@ID", SqlDbType.Int){ Value = idSoPG }
            }, CommandType.Text);

            txt.Text = Convert.ToDecimal(dtback.Rows[0]["BeratTimbang"]).ToString("N2").Replace(",", ".");

            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "alert",
                "alert('Error: " + error + "');", true);
            return;
        }

        string weightParam = beratTimbang.ToString();
        Response.Redirect("Sales_DataPGBeratTimbangSave.aspx?id=" + myID + "&idsopg=" + idSoPG + "&weightpg=" + weightParam);
    }

    protected decimal GetPGPricingMyAPPSNewBanget(int IDProduct, decimal WeightPG)
    {
        decimal GoldPrice = 0m;
        decimal TotalRupiah = 0m;

        string query = "sp_get_tgp_jual_beli_plu";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID_PRODUCT", SqlDbType.Int){ Value = IDProduct },
            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "PG" }
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        if (dt.Rows.Count == 1)
        {
            GoldPrice = Convert.ToDecimal(dt.Rows[0]["HARGA_JUAL"]);
        }

        TotalRupiah = GoldPrice * Convert.ToDecimal(WeightPG);
        return TotalRupiah;
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
            x += "  <a class=\"btn btn-xs btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + ItemIndex + "','" + ID + "')\">";
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
        calc();

        Response.Redirect("Sales_Data.aspx?id=" + myID + "&confirm=1#content_div_confirm");
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
        int item = 0;
        int iditem = 0;
        bool isFound = false;
		
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
		if (d != null)
		{
            string plu = txt_plu.Text.Trim();
			string error_blacklist = validasi_BlackList(plu);
			if (string.IsNullOrEmpty(error_blacklist))
			{
				int tipelokasi = d.TipeLokasi ?? 0;
				int idlokasi = d.IDLokasi ?? 0;
				// Search DJ
				if (!isFound)
				{
					/*Db.Execute("EXEC sp_solve_bug_plu_for_sales_order '" + plu + "','DJ'," + myID, Db.Cnn_DB);
					var rs = from p in db.StockProductDJ
							 where p.Draft == false && p.StatusPenjualan == 0 && p.ReturnProduct == false
							 && p.StockActualDJ.IDLokasi == d.IDLokasi && p.StockActualDJ.TipeLokasi == d.TipeLokasi && p.StockActualDJ.Substorage == 1
							 && !(from z in db.SalesOrderDJ
								  where z.SalesOrder.StatusVoid == false
								  select z.IDProduct).Contains(p.ID)
							 && p.Nomor == plu
							 select p;*/
					string query = "EXEC sp_solve_bug_plu_for_sales_order @NOMOR, @TIPE_PRODUCT, @ID_SO " +
						"IF EXISTS(SELECT 1 FROM StockProductDJ p " +
						"INNER JOIN StockActualDJ sa ON p.ID = sa.IDProduct " +
						"WHERE p.Draft = 0 AND p.StatusPenjualan = 0 AND p.ReturnProduct = 0 " +
						"AND sa.IDLokasi = @ID_LOKASI AND sa.TipeLokasi = @TIPE_LOKASI " +
						"AND sa.Substorage = 1 AND p.Nomor = @NOMOR " +
						"AND NOT EXISTS(SELECT 1 FROM SalesOrderDJ sodj INNER JOIN SalesOrder so ON sodj.IDForm = so.ID " +
						"WHERE so.StatusVoid = 0 " +
						"AND sodj.IDProduct = p.ID)) " +
						"BEGIN SELECT 1 [RESULT], ISNULL((SELECT TOP 1 ID FROM StockProductDJ WHERE Nomor = @NOMOR AND Draft = 0 AND ApprovalTgl IS NOT NULL),0) [ID] END ELSE BEGIN SELECT 0[RESULT], 0[ID] END ";
					List<SqlParameter> parametersdj = new List<SqlParameter>
					{
						new SqlParameter("@NOMOR", SqlDbType.VarChar){ Value = plu },
						new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "DJ" },
						new SqlParameter("@ID_SO", SqlDbType.Int){ Value = myID },
						new SqlParameter("@ID_LOKASI", SqlDbType.Int) { Value = idlokasi },
						new SqlParameter("@TIPE_LOKASI", SqlDbType.Int){ Value = tipelokasi }
					};

					DataTable dt = Rs(query, Db.Cnn_DB, parametersdj, CommandType.Text);
					int result = Convert.ToInt32(dt.Rows[0]["RESULT"]);
					int id = Convert.ToInt32(dt.Rows[0]["ID"]);

					if (result == 1)
					{
						item = 1;
						iditem = id;
						isFound = true;
					}
				}

				// Search PG
				if (!isFound)
				{
					/*Db.Execute("EXEC sp_solve_bug_plu_for_sales_order '" + plu + "','PG'," + myID, Db.Cnn_DB);
					var rs = from p in db.StockProductPG
							 where p.Draft == false && p.StatusPenjualan == 0 && p.StockActualPG.TipeLokasi == d.TipeLokasi
							 && p.StockActualPG.IDLokasi == d.IDLokasi && p.StockActualPG.TipeLokasi == d.TipeLokasi && p.StockActualPG.Substorage == 1
							 && p.CharGoldLevel.NamaKode != "VLEI"
							 && !(from z in db.SalesOrderPG
								  where z.SalesOrder.StatusVoid == false
								  select z.IDProduct).Contains(p.ID)
							 && p.Nomor == plu
							 select p;*/

					string query = "EXEC sp_solve_bug_plu_for_sales_order @NOMOR, @TIPE_PRODUCT, @ID_SO " +
						"IF EXISTS(SELECT 1 FROM StockProductPG p " +
						"INNER JOIN StockActualPG sa ON p.ID = sa.IDProduct " +
						"WHERE p.Draft = 0 AND p.StatusPenjualan = 0 AND p.ReturnProduct = 0 " +
						"AND sa.IDLokasi = @ID_LOKASI AND sa.TipeLokasi = @TIPE_LOKASI " +
						"AND sa.Substorage = 1 AND p.Nomor = @NOMOR " +
						"AND NOT EXISTS(SELECT 1 FROM SalesOrderPG sopg INNER JOIN SalesOrder so ON sopg.IDForm = so.ID " +
						"WHERE so.StatusVoid = 0 " +
						"AND sopg.IDProduct = p.ID)) " +
						"BEGIN SELECT 1 [RESULT], ISNULL((SELECT TOP 1 ID FROM StockProductPG WHERE Nomor = @NOMOR AND Draft = 0 AND ApprovalTgl IS NOT NULL),0) [ID] END ELSE BEGIN SELECT 0[RESULT], 0[ID] END ";
					List<SqlParameter> parameterspg = new List<SqlParameter>
					{
						new SqlParameter("@NOMOR", SqlDbType.VarChar){ Value = plu },
						new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "PG" },
						new SqlParameter("@ID_SO", SqlDbType.Int){ Value = myID },
						new SqlParameter("@ID_LOKASI", SqlDbType.Int) { Value = idlokasi },
						new SqlParameter("@TIPE_LOKASI", SqlDbType.Int){ Value = tipelokasi }
					};

					DataTable dt = Rs(query, Db.Cnn_DB, parameterspg, CommandType.Text);
					int result = Convert.ToInt32(dt.Rows[0]["RESULT"]);
					int id = Convert.ToInt32(dt.Rows[0]["ID"]);

					if (result == 1)
					{
						item = 2;
						iditem = id;
						isFound = true;
					}
				}

				// Search GJ
				if (!isFound)
				{
					/*Db.Execute("EXEC sp_solve_bug_plu_for_sales_order '" + plu + "','GJ'," + myID, Db.Cnn_DB);
					var rs = from p in db.StockProductGJ
							 where p.Draft == false && p.StatusPenjualan == 0 && p.ReturnProduct == false
							 && p.StockActualGJ.IDLokasi == d.IDLokasi && p.StockActualGJ.TipeLokasi == d.TipeLokasi && p.StockActualGJ.Substorage == 1
							 && !(from z in db.SalesOrderGJ
								  where z.SalesOrder.StatusVoid == false
								  select z.IDProduct).Contains(p.ID)
							 && p.Nomor == plu
							 select p;*/

					string query = "EXEC sp_solve_bug_plu_for_sales_order @NOMOR, @TIPE_PRODUCT, @ID_SO " +
						"IF EXISTS (SELECT 1 FROM StockProductGJ p " +
						"INNER JOIN StockActualGJ sa ON p.ID = sa.IDProduct " +
						"WHERE p.Draft = 0 AND p.StatusPenjualan = 0 AND p.ReturnProduct = 0 " +
						"AND sa.IDLokasi = @ID_LOKASI AND sa.TipeLokasi = @TIPE_LOKASI " +
						"AND sa.Substorage = 1 AND p.Nomor = @NOMOR " +
						"AND NOT EXISTS (SELECT 1 FROM SalesOrderGJ sojg INNER JOIN SalesOrder so ON sojg.IDForm  = so.ID " +
						"WHERE so.StatusVoid = 0 " +
						"AND sojg.IDProduct = p.ID)) " +
						"BEGIN SELECT 1 [RESULT], ISNULL((SELECT TOP 1 ID FROM StockProductGJ WHERE Nomor = @NOMOR AND Draft = 0 AND ApprovalTgl IS NOT NULL),0) [ID] END ELSE BEGIN SELECT 0 [RESULT], 0 [ID] END ";
					List<SqlParameter> parametersgj = new List<SqlParameter>()
					{
						new SqlParameter("@NOMOR", SqlDbType.VarChar){ Value = plu },
						new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "GJ" },
						new SqlParameter("@ID_SO", SqlDbType.Int){ Value = myID },
						new SqlParameter("@ID_LOKASI", SqlDbType.Int) { Value = idlokasi },
						new SqlParameter("@TIPE_LOKASI", SqlDbType.Int){ Value = tipelokasi }
					};

					DataTable dt = Rs(query, Db.Cnn_DB, parametersgj, CommandType.Text);
					int result = Convert.ToInt32(dt.Rows[0]["RESULT"]);
					int id = Convert.ToInt32(dt.Rows[0]["ID"]);

					if (result > 0)
					{
						item = 3;
						iditem = id;
						isFound = true;
					}
				}

				// Search LD
				if (!isFound)
				{
					string query = "EXEC sp_solve_bug_plu_for_sales_order @NOMOR, @TIPE_PRODUCT, @ID_SO " +
						"IF EXISTS (SELECT 1 FROM StockProductLD_Stone1B p " +
						"JOIN StockProductLD p1 ON p.IDForm = p1.ID " +
						"INNER JOIN StockActualLD_Stone1B sa ON p.ID = sa.IDProduct " +
						"WHERE p1.Draft = 0 AND p.StatusPenjualan = 0 AND p.ReturnProduct = 0 " +
						"AND sa.IDLokasi = @ID_LOKASI AND sa.TipeLokasi = @TIPE_LOKASI " +
						"AND sa.Substorage = 1 AND p.Nomor = @NOMOR " +
						"AND NOT EXISTS (SELECT 1 FROM SalesOrderLD sold INNER JOIN SalesOrder so ON sold.IDForm  = so.ID " +
						"WHERE so.StatusVoid = 0 " +
						"AND sold.IDProduct = p.ID)) " +
						"BEGIN SELECT 1 [RESULT], ISNULL((SELECT TOP 1 A.ID FROM StockProductLD_Stone1B A JOIN StockProductLD B ON A.IDForm = B.ID WHERE A.Nomor = @NOMOR AND Draft = 0 AND ApprovalTgl IS NOT NULL),0) [ID] END ELSE BEGIN SELECT 0 [RESULT], 0 [ID] END ";
					List<SqlParameter> parametersld = new List<SqlParameter>()
					{
						new SqlParameter("@NOMOR", SqlDbType.VarChar){ Value = plu },
						new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "LD" },
						new SqlParameter("@ID_SO", SqlDbType.Int){ Value = myID },
						new SqlParameter("@ID_LOKASI", SqlDbType.Int) { Value = idlokasi },
						new SqlParameter("@TIPE_LOKASI", SqlDbType.Int){ Value = tipelokasi }
					};

					DataTable dt = Rs(query, Db.Cnn_DB, parametersld, CommandType.Text);
					int result = Convert.ToInt32(dt.Rows[0]["RESULT"]);
					int id = Convert.ToInt32(dt.Rows[0]["ID"]);

					if (result == 1)
					{
						item = 4;
						iditem = id;
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
			}
			if (isFound)
			{
				Response.Redirect("Sales_ItemAddSave.aspx?id=" + myID + "&item=" + item + "&iditem=" + iditem);
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
	
    //update_keterangan_Click
    protected void update_keterangan_Click(object sender, EventArgs e)
    {
        //Response.Redirect("Sales_Data.aspx?id=" + myID + "&oweifjowfj=234234");
        //Response.Write("asdasdad");

        var x = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (x == null) DataError();
        {
            x.Keterangan = keterangan.Text;
            db.SubmitChanges();
            Response.Redirect("Sales_Data.aspx" + hrefpattern);
        }
    }

    //Cancel
    protected void cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Sales_Data.aspx" + hrefpattern);
    }

    //Redirect Document Sales Order
    protected void redirect_payment_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && !p.Draft); // no-draft !!!
        if (d == null) { DataError(); }
        else
        {
            Response.Redirect("/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?id=" + d.ID);
        }
    }

    //Redirect Document Register
    protected void redirect_register_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && !p.Draft); // no-draft !!!
        if (d == null) { DataError(); }
        else
        {
            Response.Redirect("/OutletSales/Page/Create/" + hrefmainfull);
        }
    }
    //Save New
    protected void save_Click(object sender, EventArgs e)
    {
        string status_SAP = string.Empty;
        string status_STAMPS = string.Empty;

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) { DataError(); }
        else
        {
            // Generate Document Number
            if (d.Nomor == null || d.Nomor == "")
            {
                string NewSalesOrderNumber = Document.Generate_Nomor_SalesOrder(d);
                d.Nomor = NewSalesOrderNumber;
                db.SubmitChanges();

                List<SqlParameter> paramsa = new List<SqlParameter>
                {
                    new SqlParameter("@IDSO", SqlDbType.Int){ Value = myID }
                };
                Execute("sp_solve_duplicate_so_number", Db.Cnn_DB, paramsa, CommandType.StoredProcedure);
            }

            // Validation and SAP Posting
            try
            {
                if (SAP.xActivate)
                {
                    status_SAP += ((status_SAP == "") ? SAPM.prePostSalesOrderIntegration(d) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.isValidLocationSAP(d) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.postCustomerToSAP(d) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.postSalesGoodsToSAP(d, false) : "");
                }
            }
            catch (Exception ex)
            {
                status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (SAP Error message : " + ex.Message + ")";
            }

            status_SAP = ""; // for dev only, disable enable SAP validation
            if (status_SAP == "")
            {
                string query = "sp_SaveSalesOrder";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ID", SqlDbType.Int){ Value = myID },
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
                lit_error_sap.Text += (status_SAP == "") ? "" : "<div class='alert alert-danger' role='alert'>" + status_SAP + "</div>";
            }
        }
    }

    //Save Old
    protected void save_ClickOld(object sender, EventArgs e)
    {
        string status_SAP = "";
        string status_STAMPS = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) { DataError(); }
        else
        {
            // Generate Document Number
            if (d.Nomor == null || d.Nomor == "")
            {
                string NewSalesOrderNumber = Document.Generate_Nomor_SalesOrder(d);
                d.Nomor = NewSalesOrderNumber;
                db.SubmitChanges();
				
				Db.Execute("EXEC sp_solve_duplicate_so_number " + myID, Db.Cnn_DB);
            }

            // Validation and SAP Posting
            try
            {
                if (SAP.xActivate)
                {
                    status_SAP += ((status_SAP == "") ? SAPM.prePostSalesOrderIntegration(d) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.isValidLocationSAP(d) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.postCustomerToSAP(d) : "");
                    status_SAP += ((status_SAP == "") ? SAPM.postSalesGoodsToSAP(d, false) : "");
                }
            }
            catch (Exception ex)
            {
                status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (SAP Error message : "
                    + ex.Message
                    //+ ex.StackTrace 
                    //+ ex.InnerException 
                    + ")";
            }

            status_SAP = ""; // for dev only, disable enable SAP validation
            if (status_SAP == "")
            {
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

                d.ModalRupiah = dModalRupiah;
                d.HargaRupiah = dHargaRupiah;
                d.TotalBayarBeforeDisc = dTotalBayar;
                d.TotalRupiahBeforeDisc = dTotalRupiah;
                d.Discount = 0m;
                d.DiscountNominal = dDiscNormal;
                d.DiscountGift = dDiscGift;
                d.DiscountProgram = 0m;
                d.DiscountProgramNominal = dDiscProgram;
                d.DiscountRound = dDiscRound;
                d.TotalBayar = dTotalBayar;
                d.TotalRupiah = dTotalRupiah;
                d.TotalResell = 0m;
                d.Unpaid = dTotalBayar;
                d.Paid = 0m;
                d.Draft = false;
                d.DraftDate = DateTime.Now;
                d.Keterangan = keterangan.Text;
                d.StatusPembayaran = (dTotalBayar == 0m ? true : false);
                db.SubmitChanges();

                string SalesOrderNumber = d.Nomor;

                var u = from p in db.SalesVoucher where p.IDSalesOrder == myID select p;
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
                        if (y == null) next_err_2.Visible = true;
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
                lit_error_sap.Text += (status_SAP == "") ? "" : "<div class='alert alert-danger' role='alert'>" + status_SAP + "</div>";
            }
            
        }

    }

    protected void Discount_TextChanged(object sender, EventArgs e)
    {
        calc();
        string href = App.RefreshURL(this);
        //Page-Qstr
        href += "&id=" + myID;
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
        href += "&tb=" + ((DropDownList)sender).ClientID;
		Response.Redirect(href);
	}
    protected void calc()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
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
        // Kumpulkan (ID, Qty) dari kontrol UI dulu — bagian ini TIDAK berubah dari versi asli
        // (masih toleran per-baris: FindControl yang gagal untuk index tertentu di-skip diam-diam).
        List<Tuple<int, decimal>> updates = new List<Tuple<int, decimal>>();
        TextBox txt = new TextBox();
        string hf = "";
        int index = 1;

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfpc" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("pc_" + index.ToString());
                updates.Add(Tuple.Create(Convert.ToInt32(hf), Convert.ToDecimal(txt.Text)));
            }
            catch (Exception) { }
            index++;
        }

        if (updates.Count == 0) return;

        // Sebelumnya per baris: 1 SingleOrDefault(SalesOrderPackaging) + 1 raw SQL query
        // terpisah ke Packaging.Harga (kolom ini TIDAK ada di mapping LINQ Packaging — sama
        // seperti temuan TotalFeeExtraRingSize sebelumnya) + 1 SubmitChanges SENDIRI-SENDIRI —
        // N baris = N transaksi independen (kalau baris ke-3 gagal, baris 1-2 SUDAH ter-commit,
        // tidak atomic). Sekarang: 1 UPDATE ber-JOIN per baris (Harga diambil langsung, tanpa
        // round-trip terpisah), semua baris dalam SATU transaksi — gagal satu, rollback semua.
        // Exception tetap ditangkap di paling luar supaya perilaku ke caller (calc()) sama
        // seperti sebelumnya: gagal di sini tidak menjatuhkan halaman.
        try
        {
            using (SqlConnection cnn = new SqlConnection(Db.Cnn_DB))
            {
                cnn.Open();
                using (SqlTransaction tran = cnn.BeginTransaction())
                {
                    try
                    {
                        foreach (var u in updates)
                        {
                            using (SqlCommand cmd = new SqlCommand(
                                @"UPDATE sop SET
                                    sop.Qty = @Qty,
                                    sop.ModalRupiah = ISNULL(pkg.Harga, 0),
                                    sop.TotalModalRupiah = @Qty * ISNULL(pkg.Harga, 0)
                                  FROM SalesOrderPackaging sop
                                  JOIN Packaging pkg ON pkg.ID = sop.IDProduct
                                  WHERE sop.ID = @ID", cnn, tran))
                            {
                                cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Money) { Value = u.Item2 });
                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = u.Item1 });
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (Exception) { }
    }

    protected void saveSouvenir(SalesOrder d)
    {
        // Sama pola dan alasannya dengan savePackaging di atas.
        List<Tuple<int, decimal>> updates = new List<Tuple<int, decimal>>();
        TextBox txt = new TextBox();
        string hf = "";
        int index = 1;

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfsv" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("sv_" + index.ToString());
                updates.Add(Tuple.Create(Convert.ToInt32(hf), Convert.ToDecimal(txt.Text)));
            }
            catch (Exception) { }
            index++;
        }

        if (updates.Count == 0) return;

        try
        {
            using (SqlConnection cnn = new SqlConnection(Db.Cnn_DB))
            {
                cnn.Open();
                using (SqlTransaction tran = cnn.BeginTransaction())
                {
                    try
                    {
                        foreach (var u in updates)
                        {
                            using (SqlCommand cmd = new SqlCommand(
                                @"UPDATE sos SET
                                    sos.Qty = @Qty,
                                    sos.ModalRupiah = ISNULL(sv.Harga, 0),
                                    sos.TotalModalRupiah = @Qty * ISNULL(sv.Harga, 0)
                                  FROM SalesOrderSouvenir sos
                                  JOIN Souvenir sv ON sv.ID = sos.IDProduct
                                  WHERE sos.ID = @ID", cnn, tran))
                            {
                                cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Money) { Value = u.Item2 });
                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = u.Item1 });
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (Exception) { }
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

        // N+1 fix (scope terbatas, TIDAK menyentuh logic kalkulasi diskon): query PromoDJ_Detail
        // + 2 raw SQL lookup di bawah sebelumnya dieksekusi ULANG untuk SETIAP baris item —
        // padahal kondisi WHERE-nya (ApprovalNama/TglStart/TglExpire/Draft) SAMA untuk semua
        // baris, cuma IDProduct yang beda. Di-prefetch sekali di sini untuk semua produk aktif,
        // di-lookup dari memory per baris. Kondisi "cuma diisi kalau ada match" (dan konsekuensi
        // downstream-nya) TETAP PERSIS SAMA seperti sebelumnya — tidak ada logic yang diubah.
        Dictionary<int, Tuple<bool?, int>> promoMatchByProduct = new Dictionary<int, Tuple<bool?, int>>();
        {
            string promoQuery = @"
                SELECT pd.IDProduct, pj.StatusDiscMembership, ISNULL(pj.ExtraRingSize, 0) AS ExtraRingSize
                FROM PromoDJ_Detail pd
                JOIN PromoDJ pj ON pj.ID = pd.IDForm
                WHERE pj.ApprovalNama IS NOT NULL AND pj.TglStart <= @Today AND pj.TglExpire >= @Today AND pj.Draft = 0";
            List<SqlParameter> promoParam = new List<SqlParameter>()
            {
                new SqlParameter("@Today", SqlDbType.DateTime) { Value = DateTime.Today }
            };
            DataTable promoDt = Rs(promoQuery, Db.Cnn_DB, promoParam, CommandType.Text);
            foreach (DataRow pr in promoDt.Rows)
            {
                int idProduct = Convert.ToInt32(pr["IDProduct"]);
                // t.First() di kode asli cuma ambil salah satu match kalau lebih dari satu —
                // konsisten, ambil yang pertama ditemukan di sini juga.
                if (!promoMatchByProduct.ContainsKey(idProduct))
                {
                    bool? statusDiscMembership = (pr["StatusDiscMembership"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(pr["StatusDiscMembership"]);
                    int extraRingSize = Convert.ToInt32(pr["ExtraRingSize"]);
                    promoMatchByProduct[idProduct] = Tuple.Create(statusDiscMembership, extraRingSize);
                }
            }
        }
        // Query ini TIDAK bergantung pada baris/promo sama sekali (WHERE-nya fixed), tapi
        // sebelumnya di-query ulang tiap baris yang kebetulan match promo. Diambil sekali;
        // TETAP cuma dipakai kalau ada promo match (lihat di bawah), sama seperti sebelumnya.
        int feeExtraRingSizeSharedValue = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(Harga,0) AS INT) [Harga] FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0", Db.Cnn_DB);

        // Dulu tiap baris query db.SalesOrderDJ.SingleOrDefault(ID) sendiri-sendiri - N round-trip
        // untuk N baris. Bulk-fetch sekali di sini (scope sama: IDForm == myID), lookup per baris
        // dari dictionary di memori. SubmitChanges() juga dipindah ke luar loop (1x untuk semua
        // baris, bukan 1x per baris) - tidak ada raw-SQL yang membaca ulang kolom SalesOrderDJ
        // ini di antara baris, jadi urutan commit per-baris tidak signifikan di sini.
        // ExecuteQuery<T>() (raw SQL -> entity ter-track penuh, aman untuk SubmitChanges
        // sesudahnya) - BUKAN .Where(). File ini punya "using System.Linq.Dynamic;" yang bikin
        // SETIAP pemanggilan method "Where" (lambda ATAU query-syntax) salah resolve ke overload
        // Func<T,int,bool> (indexed) -> CS1593, walau lambda-nya cuma 1 parameter.
        List<SalesOrderDJ> djRowsForByID = db.ExecuteQuery<SalesOrderDJ>("SELECT * FROM SalesOrderDJ WHERE IDForm = {0}", myID).ToList();
        Dictionary<int, SalesOrderDJ> djByID = djRowsForByID.ToDictionary(p => p.ID);
        // Cache Discount-by-IDPromoDetail juga - beberapa baris sering pakai promo bank yang sama.
        Dictionary<int, decimal> discountByPromoId = new Dictionary<int, decimal>();
        // N+1 "known-family" - lihat komentar di PrefetchCharProductInfoDJ.
        Dictionary<int, CharProductInfoDJ> charProductByProductDJ = PrefetchCharProductInfoDJ(djByID.Values.Select(p => p.IDProduct).Distinct().ToList());

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

                SalesOrderDJ d;
                djByID.TryGetValue(Convert.ToInt32(hf), out d);
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
							decimal discount;
							if (!discountByPromoId.TryGetValue(idpromo, out discount))
							{
								discount = Db.SingleDecimal("SELECT TOP 1 ISNULL(Discount,0) [Discount] FROM PromoDetail WHERE ID = " + idpromo, Db.Cnn_DB);
								discountByPromoId[idpromo] = discount;
							}
							DiscountBankNominal = discount;
							
							Db.Execute("UPDATE SalesOrderDJ SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
						}
						else if(idpromo == 0) 
						{
							DiscountBankNominal = 0m;
							Db.Execute("UPDATE SalesOrderDJ SET IDPromoDetail = NULL WHERE ID = " + hf, Db.Cnn_DB);
						}
						else if(idpromo == -1)
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

                    // N+1 "known-family" - lihat komentar di PrefetchCharProductInfoDJ.
                    CharProductInfoDJ charProductInfo;
                    charProductByProductDJ.TryGetValue(d.IDProduct, out charProductInfo);

                    if (charProductInfo != null && charProductInfo.CategoryNamaKode == "SETTING") { DiscountNormalNominal = 0; }

                    bool statusNoPromoMembership = false;

                    // promo — lookup dari prefetch di atas, bukan query per baris lagi.
                    Tuple<bool?, int> promoMatch;
                    if (promoMatchByProduct.TryGetValue(d.IDProduct, out promoMatch))
                    {
                        statusNoPromoMembership = promoMatch.Item1.HasValue ? promoMatch.Item1.Value : false;
                        ExtraRingSize = promoMatch.Item2;
                        FeeExtraRingSize = feeExtraRingSizeSharedValue;
                    }

					// berdasarkan meeting AE, pg dan gj tidak ada diskon membership
					if (charProductInfo != null && ((charProductInfo.CategoryNamaKode == "SETTING") || (charProductInfo.LevelNamaKode == "MTG" && !charProductInfo.HasStone1B && charProductInfo.CategoryNamaKode != "SNI")))
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
                        bool isDossierNonBranded = (charProductInfo != null && charProductInfo.HasStone1B && !branded);

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
					catch (Exception ex) {}
					
					if(TotalFeeExtraRingSize > 0m)
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

                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
        db.SubmitChanges();
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

        // N+1 fix (scope terbatas, sama seperti calcDJ) — lihat komentar di calcDJ untuk
        // penjelasan lengkap. Catatan tambahan khusus di sini: statusNoPromoMembership hasil
        // lookup ini SELALU ditimpa "= true" tak lama setelah dipakai (baris "statusNoPromoMembership
        // = true; // true utk pg dan gj") — jadi nilainya sebenarnya tidak berpengaruh ke hasil
        // akhir. Tetap dihitung di sini untuk menjaga struktur identik dengan calcDJ dan supaya
        // ExtraRingSize/FeeExtraRingSize (yang MEMANG dipakai downstream) tetap benar.
        Dictionary<int, Tuple<bool?, int>> promoMatchByProduct = new Dictionary<int, Tuple<bool?, int>>();
        {
            string promoQuery = @"
                SELECT pd.IDProduct, pj.StatusDiscMembership, ISNULL(pj.ExtraRingSize, 0) AS ExtraRingSize
                FROM PromoPG_Detail pd
                JOIN PromoPG pj ON pj.ID = pd.IDForm
                WHERE pj.ApprovalNama IS NOT NULL AND pj.TglStart <= @Today AND pj.TglExpire >= @Today AND pj.Draft = 0";
            List<SqlParameter> promoParam = new List<SqlParameter>()
            {
                new SqlParameter("@Today", SqlDbType.DateTime) { Value = DateTime.Today }
            };
            DataTable promoDt = Rs(promoQuery, Db.Cnn_DB, promoParam, CommandType.Text);
            foreach (DataRow pr in promoDt.Rows)
            {
                int idProduct = Convert.ToInt32(pr["IDProduct"]);
                if (!promoMatchByProduct.ContainsKey(idProduct))
                {
                    bool? statusDiscMembership = (pr["StatusDiscMembership"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(pr["StatusDiscMembership"]);
                    int extraRingSize = Convert.ToInt32(pr["ExtraRingSize"]);
                    promoMatchByProduct[idProduct] = Tuple.Create(statusDiscMembership, extraRingSize);
                }
            }
        }
        int feeExtraRingSizeSharedValue = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(Harga,0) AS INT) [Harga] FROM PricingTableRingSize WHERE Draft = 0 AND Disable = 0", Db.Cnn_DB);

        // Dulu tiap baris query db.SalesOrderPG.SingleOrDefault(ID) sendiri-sendiri - N round-trip
        // untuk N baris. Bulk-fetch sekali di sini (scope sama: IDForm == myID), lookup per baris
        // dari dictionary di memori. SubmitChanges() juga dipindah ke luar loop (1x untuk semua
        // baris, bukan 1x per baris) - sama seperti calcDJ.
        // ExecuteQuery<T>() (raw SQL), bukan .Where() - lihat komentar di djByID (fillItemDJ)
        // untuk penjelasan CS1593.
        List<SalesOrderPG> pgRowsForByID = db.ExecuteQuery<SalesOrderPG>("SELECT * FROM SalesOrderPG WHERE IDForm = {0}", myID).ToList();
        Dictionary<int, SalesOrderPG> pgByID = pgRowsForByID.ToDictionary(p => p.ID);
        // Cache Discount-by-IDPromoDetail juga - beberapa baris sering pakai promo bank yang sama.
        Dictionary<int, decimal> discountByPromoId = new Dictionary<int, decimal>();

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

                SalesOrderPG d;
                pgByID.TryGetValue(Convert.ToInt32(hf), out d);
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
							decimal discount;
							if (!discountByPromoId.TryGetValue(idpromo, out discount))
							{
								discount = Db.SingleDecimal("SELECT TOP 1 ISNULL(Discount,0) [Discount] FROM PromoDetail WHERE ID = " + idpromo, Db.Cnn_DB);
								discountByPromoId[idpromo] = discount;
							}
							DiscountBankNominal = discount;
							
							Db.Execute("UPDATE SalesOrderPG SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
						}
						else if(idpromo == 0)
						{
							DiscountBankNominal = 0m;
							Db.Execute("UPDATE SalesOrderPG SET IDPromoDetail = NULL WHERE ID = " + hf, Db.Cnn_DB);
						}
						else if(idpromo == -1)
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

                    // promo — lookup dari prefetch di atas, bukan query per baris lagi.
                    Tuple<bool?, int> promoMatch;
                    if (promoMatchByProduct.TryGetValue(d.IDProduct, out promoMatch))
                    {
                        statusNoPromoMembership = promoMatch.Item1.HasValue ? promoMatch.Item1.Value : false;
                        ExtraRingSize = promoMatch.Item2;
                        FeeExtraRingSize = feeExtraRingSizeSharedValue;
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
					catch (Exception ex) {}
					
					if(TotalFeeExtraRingSize > 0m)
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

                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
        db.SubmitChanges();
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

        // Dulu tiap baris query db.SalesOrderGJ.SingleOrDefault(ID) sendiri-sendiri - N round-trip
        // untuk N baris. Bulk-fetch sekali di sini (scope sama: IDForm == myID), lookup per baris
        // dari dictionary di memori. SubmitChanges() juga dipindah ke luar loop (1x untuk semua
        // baris, bukan 1x per baris) - sama seperti calcDJ.
        // ExecuteQuery<T>() (raw SQL), bukan .Where() - lihat komentar di djByID (fillItemDJ)
        // untuk penjelasan CS1593.
        List<SalesOrderGJ> gjRowsForByID = db.ExecuteQuery<SalesOrderGJ>("SELECT * FROM SalesOrderGJ WHERE IDForm = {0}", myID).ToList();
        Dictionary<int, SalesOrderGJ> gjByID = gjRowsForByID.ToDictionary(p => p.ID);
        // Cache Discount-by-IDPromoDetail juga - beberapa baris sering pakai promo bank yang sama.
        Dictionary<int, decimal> discountByPromoId = new Dictionary<int, decimal>();
        // N+1 "known-family" - lihat komentar di PrefetchCharProductInfoGJ.
        Dictionary<int, CharProductInfoGJ> charProductByProductGJ = PrefetchCharProductInfoGJ(gjByID.Values.Select(p => p.IDProduct).Distinct().ToList());

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

                SalesOrderGJ d;
                gjByID.TryGetValue(Convert.ToInt32(hf), out d);
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
							decimal discount;
							if (!discountByPromoId.TryGetValue(idpromo, out discount))
							{
								discount = Db.SingleDecimal("SELECT TOP 1 ISNULL(Discount,0) [Discount] FROM PromoDetail WHERE ID = " + idpromo, Db.Cnn_DB);
								discountByPromoId[idpromo] = discount;
							}
							DiscountBankNominal = discount;
							
							Db.Execute("UPDATE SalesOrderGJ SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
						}
						else if(idpromo == 0)
						{
							DiscountBankNominal = 0m;
							Db.Execute("UPDATE SalesOrderGJ SET IDPromoDetail = NULL WHERE ID = " + hf, Db.Cnn_DB);
						}
						else if(idpromo == -1)
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

                    // N+1 fix: query PromoGJ_Detail per baris DIHAPUS (bukan diprefetch, murni
                    // dihapus) — hasilnya (statusNoPromoMembership) SELALU ditimpa "= true" tepat
                    // di baris berikutnya, dan tidak ada ExtraRingSize/FeeExtraRingSize yang
                    // dipakai downstream di calcGJ (beda dengan calcDJ/calcPG) — jadi query ini
                    // provably dead code, aman dihapus tanpa mengubah hasil apa pun.
                    bool statusNoPromoMembership = true; // true utk pg dan gj
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

                        // N+1 "known-family" - lihat komentar di PrefetchCharProductInfoGJ.
                        CharProductInfoGJ charProductInfoGJ;
                        charProductByProductGJ.TryGetValue(d.IDProduct, out charProductInfoGJ);
                        int productCategoryGJ = (charProductInfoGJ != null && charProductInfoGJ.ProductCategory.HasValue) ? charProductInfoGJ.ProductCategory.Value : 0;
                        ASP = DLC.getASPGJParameter(productCategoryGJ, Solitaire);

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

                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
        db.SubmitChanges();
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

        // Dulu tiap baris query db.SalesOrderLD.SingleOrDefault(ID) sendiri-sendiri - N round-trip
        // untuk N baris. Bulk-fetch sekali di sini (scope sama: IDForm == myID), lookup per baris
        // dari dictionary di memori. SubmitChanges() juga dipindah ke luar loop (1x untuk semua
        // baris, bukan 1x per baris) - sama seperti calcDJ.
        // ExecuteQuery<T>() (raw SQL), bukan .Where() - lihat komentar di djByID (fillItemDJ)
        // untuk penjelasan CS1593.
        List<SalesOrderLD> ldRowsForByID = db.ExecuteQuery<SalesOrderLD>("SELECT * FROM SalesOrderLD WHERE IDForm = {0}", myID).ToList();
        Dictionary<int, SalesOrderLD> ldByID = ldRowsForByID.ToDictionary(p => p.ID);
        // Cache Discount-by-IDPromoDetail juga - beberapa baris sering pakai promo bank yang sama.
        Dictionary<int, decimal> discountByPromoId = new Dictionary<int, decimal>();

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

                SalesOrderLD d;
                ldByID.TryGetValue(Convert.ToInt32(hf), out d);
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
							decimal discount;
							if (!discountByPromoId.TryGetValue(idpromo, out discount))
							{
								discount = Db.SingleDecimal("SELECT TOP 1 ISNULL(Discount,0) [Discount] FROM PromoDetail WHERE ID = " + idpromo, Db.Cnn_DB);
								discountByPromoId[idpromo] = discount;
							}
							DiscountBankNominal = discount;
							
							Db.Execute("UPDATE SalesOrderLD SET IDPromoDetail = " + ddl2.SelectedValue + " WHERE ID = " + hf, Db.Cnn_DB);
						}
						else if(idpromo == 0)
						{
							DiscountBankNominal = 0m;
							Db.Execute("UPDATE SalesOrderLD SET IDPromoDetail = NULL WHERE ID = " + hf, Db.Cnn_DB);
						}
						else if(idpromo == -1)
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

                    DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    DiscountPromoPercent = 0m;

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

                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
        db.SubmitChanges();
    }
    protected void CalcDiscountRound(decimal TotalHarga, TextBox tb)
    {
        decimal Disc = 0m;

        App rd = new App();

        Disc = TotalHarga - rd.RoundBottom(TotalHarga);
        tb.Text = Disc.ToString("N0");
    }

    protected string CheckKombinasiDJLD(int idproduct, int tipeproduct)
    {
        string result = "";
        using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
        {
            string query = "sp_check_kombinasi_dj_ld";
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = query;
            cmd.Parameters.Add(new SqlParameter("@ID_PRODUCT", SqlDbType.Int) { Value = idproduct });
            cmd.Parameters.Add(new SqlParameter("@TIPE_PRODUCT", SqlDbType.Int) { Value = tipeproduct });
            conn.OpenAsync();
            result = (string)cmd.ExecuteScalar();
            conn.Close();
        }
        return result;
    }
	
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
}