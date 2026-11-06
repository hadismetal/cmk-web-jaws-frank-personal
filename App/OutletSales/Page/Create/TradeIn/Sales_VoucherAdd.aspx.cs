using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAdd : System.Web.UI.Page
{
    private Map_DB _db;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int myItemID { get { return App.QueryInt(this, "itemid"); } }
    protected int myItemType { get { return App.QueryInt(this, "itemtype"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        int IDSO = myIDSO;
        int IDTI = myID;
        int IDRS = myIDRO;
        int ITEMTP = myItemType;
        int ITEMID = myItemID;

        if (!IsPostBack)
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == IDSO && p.Draft);
            if (d == null) DataError("1");
            else
            {
                kw.Focus();
                fillForm(d, ITEMTP, ITEMID);
                fillFilter();
                if (App.Refresh(this)) fillTable(d, sender, e);

                //Cancel-Link
                cancel.HRef = "TradeIn_Data.aspx";
                cancel.HRef += "?id=" + IDTI;
                cancel.HRef += "&so=" + IDSO;
                cancel.HRef += "&ro=" + IDRS;
            }

        }
    }

    //Exception
    protected void DataError(string err) { Response.Redirect("TradeIn.aspx?err=" + err); }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
        href += "&itemtype=" + myItemType;
        href += "&itemid=" + myItemID;
        href += "&kw=" + kw.Text;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(string NomorVoucher)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Sales_VoucherAddSave.aspx";
        href += "?id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
        href += "&itemtype=" + myItemType;
        href += "&itemid=" + myItemID;
        href += "&NomorVoucher=" + NomorVoucher;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected void fillForm(SalesOrder d, int ITEMTP, int ITEMID)
    {

        string xpluimg = "";
        string xplutype = "";
        string xplunumber = "";
        string xpluitem = "";

        if (ITEMTP == 1)
        {
            xplutype = "Diamond Jewellery";

            var x = db.SalesOrderDJ.SingleOrDefault(p => p.ID == ITEMID);
            if (x != null)
            {
                xpluimg = x.StockProductDJ.ImgPicture;
                xplunumber = x.StockProductDJ.Nomor;
                xpluitem = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            }

        }
        else if (ITEMTP == 2)
        {
            xplutype = "Plain Gold";

            var x = db.SalesOrderPG.SingleOrDefault(p => p.ID == ITEMID);
            if (x != null)
            {
                xpluimg = x.StockProductPG.ImgPicture;
                xplunumber = x.StockProductPG.Nomor;
                xpluitem = x.StockProductPG.CharProductItem.Nama;
            }
        }
        else if (ITEMTP == 3)
        {
            xplutype = "Gold Jewellery";

            var x = db.SalesOrderGJ.SingleOrDefault(p => p.ID == ITEMID);
            if (x != null)
            {
                xpluimg = x.StockProductGJ.ImgPicture;
                xplunumber = x.StockProductGJ.Nomor;
                xpluitem = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
            }
        }
        else if (ITEMTP == 4)
        {
            xplutype = "Loose Diamond";

            var x = db.SalesOrderLD.SingleOrDefault(p => p.ID == ITEMID);
            if (x != null)
            {
                xplunumber = x.StockProductLD_Stone1B.Nomor;
                xpluitem = "Loose Diamond";
            }
        }

        lit_img.Text = xpluimg != "" ? "<img class='img-thumbnail' src=\"/Images.aspx?path=" + xpluimg + "\" style='max-height:60px;' />" : "";

        span_plu_type.InnerText = Str.CapUpper(xplutype);
        span_plu_number.InnerText = Str.CapUpper(xplunumber);
        span_plu_item.InnerText = Str.CapUpper(xpluitem);

    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
        }
    }
    protected void fillTable(SalesOrder d, object sender, EventArgs e)
    {
        string voucherTypeAllowed = "DISCOUNT"; // DISCOUNT or PAYMENT
        string keyword = kw.Text.Trim();
        string errmsg = "";

        div_panel_voucher.Visible = false;

        if (keyword != "")
        {
            int id_tipelokasi = d.TipeLokasi.Value;
            int id_lokasi = d.IDLokasi.Value;
            int id_brand = 0;

            if (id_tipelokasi == 1)
            {
                id_brand = db.LocOutlet.Single(p => p.ID == id_lokasi).IDBrand;
            }
            else if (id_tipelokasi == 2)
            {
                id_brand = db.LocExhibition.Single(p => p.ID == id_lokasi).IDBrand;
            }

            try
            {
                errmsg = Voucher.isValidVoucher(voucherTypeAllowed, keyword, d.ID, id_brand, d.DataCustomer.NoCustomer, myItemType);
            }
            catch (Exception) { }
            
            if (errmsg != "")
            {
                lit_msg.Text = "<div class='alert alert-warning' role='alert'>" + errmsg + "</div>";
            }
            else
            {
                div_panel_voucher.Visible = true;

                var rs = (from p in cs.T_voucher
                          join q in cs.Tb_type_voucher on p.M_type equals q.Ckodetype
                          where p.M_nomor.Equals(keyword) && q.Cjenisvoucher == voucherTypeAllowed
                          select new { p, q }).FirstOrDefault();

                if (rs != null)
                {
                    string xbrand = "";

                    if (rs.p.M_brand == null || rs.p.M_brand == "")
                    {
                        xbrand = "ALL";
                    }
                    else
                    {
                        var bd = cs.Msmaster.SingleOrDefault(p => p.M_type == "BRAND" && p.M_kode == rs.p.M_brand);
                        if (bd != null) { xbrand = bd.M_nama; } else { xbrand = "UNKNOWN"; }
                    }

                    string xjenisvoucher = rs.q.Cnamatype;
                    string xkodecustomer = rs.p.M_kodecust;
                    string xnamacustomer = rs.p.M_nama;

                    string xstartdate = Str.FullDateJam(Convert.ToDateTime(rs.p.M_tanggal));
                    string xexpiredate = Str.FullDateJam(Convert.ToDateTime(rs.p.M_expiredate));
                    string xketerangan1 = rs.p.M_keterangan;
                    string xketerangan2 = rs.p.M_keterangan2;

                    string xkodevoucher = rs.p.M_nomor;
                    string xnominalvoucher = Str.Number0(rs.p.M_value);

                    string xinfo_left = "";
                    string xinfo_right = "";

                    xinfo_left += "<table>";
                    xinfo_left += "<tr><td style='vertical-align:top;'>Jenis Voucher</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xjenisvoucher + "</td></tr>";
                    xinfo_left += "<tr><td style='vertical-align:top;'>SBU</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xbrand + "</td></tr>";
                    xinfo_left += "<tr><td style='vertical-align:top;'>Kode Customer</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xkodecustomer + "</td></tr>";
                    xinfo_left += "<tr><td style='vertical-align:top;'>Nama Customer</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xnamacustomer + "</td></tr>";
                    xinfo_left += "</table>";

                    xinfo_right += "<table>";
                    xinfo_right += "<tr><td style='vertical-align:top;'>Tanggal Terbit</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xstartdate + "</td></tr>";
                    xinfo_right += "<tr><td style='vertical-align:top;'>Tanggal Expire</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xexpiredate + "</td></tr>";
                    xinfo_right += "<tr><td style='vertical-align:top;'>Keterangan 1</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xketerangan1 + "</td></tr>";
                    xinfo_right += "<tr><td style='vertical-align:top;'>Keterangan 2</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xketerangan2 + "</td></tr>";
                    xinfo_right += "</table>";

                    spn_voucher_nomorkode.InnerText = xkodevoucher;
                    spn_voucher_nominal.InnerText = xnominalvoucher;

                    lit_voucher_link.Text = TD_HyperLink(xkodevoucher);
                    lit_voucher_info_left.Text = xinfo_left;
                    lit_voucher_info_right.Text = xinfo_right;

                }

            }
        }
    }

}