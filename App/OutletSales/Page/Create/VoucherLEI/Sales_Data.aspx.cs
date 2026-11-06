using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_Item : System.Web.UI.Page
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
                    //addpck.HRef += hrefpattern;
                    //addsv.HRef += hrefpattern;

                    if (myConfirm == "1")
                    {
                        div_review.Visible = false;
                        div_confirm.Visible = true;
                        update_keterangan.Visible = false;
                        div_btn_keterangan.Visible = false;

                        //addpck.Disabled = true;
                        //addsv.Disabled = true;
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

    //Exception
    protected void DataError() { Response.Redirect(hrefmain); }

    //Display Table
    protected void fillTable(object sender, EventArgs e, SalesOrder x)
    {
        int QTY = 0;
        decimal GT = 0m;
        decimal GT2 = 0m;

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
        fillItemPG(myID, ref QTY, ref GT, ref GT2, m, disc1, disc2);

        TextBox txtbox = new TextBox();

        string txt = Request.QueryString["tb"];
        if (txt != "")
        {
            Page.RegisterStartupScript("af", "<script>$(function() {$(\"#" + txt + "\").focus();});</script>");
        }

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
        calc();
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
                bool isBranded = false; // Pricing.isDiamondBranded(r.StockProductDJ.Nomor);
                bool isNett = Pricing.isNett(r.StockProductPG.Nomor);

                bool statusNoPromoNormal = false;
                bool statusNoPromoMembership = false;

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
				string xWeight = Str.Number3(r.StockProductPG.NettoWeight);
                decimal xTimbang = Db.SingleDecimal("SELECT TOP 1 ISNULL(BERATTIMBANG,0.000) FROM SALESORDERPG WHERE ID = '"+r.ID+"' ", Db.Cnn_DB);
                if(xTimbang == 0)
                {
                    Db.Execute("UPDATE SALESORDERPG SET BERATTIMBANG = '" + xWeight.Replace(",", ".") + "'  WHERE ID = '" + r.ID + "' ", Db.Cnn_DB);
                }
                string xWeightTimbang = (xTimbang == 0m ) ? xWeight : Str.Number3(xTimbang)  ;
                //td.Text = TD_PLU(0, xPLU, xItem, statusPromo, statusSouvenir);
                td.Text = TD_PLU_PG(0, xPLU, xItem, statusPromo, statusSouvenir, xWeight, xWeightTimbang, r.ID);
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

                // total bayar
                decimal totalHargaPLU = Math.Floor(HargaPLU - disc_nd_n - DiscountPromoNominal - DiscountProgramBank - disc_ms_n - disc_vc_n - DiscountLainLain - disc_rn_n);
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
        td.ColumnSpan = 7;
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
		PromoBankList(ddlpromo1,hargarupiah);

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
		PromoBankList(ddlpromo2,hargarupiah);

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
	
	private void PromoBankList(DropDownList c, decimal hargarupiah = 0m)
	{
		string query = "SELECT A.ID, A.Nama, MinValue, MaxValue, Discount, B.Nama [PromoName] FROM PromoDetail A " +
			"JOIN Promo B ON A.IDForm = B.ID " +
			"WHERE B.Draft = 0 AND B.Disable = 0 AND GETDATE() BETWEEN StartDate AND EndDate AND " + Convert.ToInt64(hargarupiah).ToString() + " BETWEEN MinValue AND MaxValue ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
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
        x += "<span class='text-muted'>" + Str.CapUpper(Item) + "</span>";
        x += "</div>";
        x += "<div class='form-horizontal' style='display:none'><div class='form-group'>";
        x += "<label for='beratnett' class='control-label col-md-4'>Berat Barcode:</label>";
        x += "<div class='col-md-5'><input type='text' id='beratnett' style='weight:50px' disabled class='form-control' value='" + WeightPG + "' /><p>gr</p></div>";
        x += "</div><div class='form-group'>";
        x += "<label for='berattimbang' class='control-label col-md-4'>Berat Timbang:</label>";
        x += "<div class='col-md-5'><input type='text' id='berattimbang' style='weight:50px' class='form-control' value='" + WeightTimbangPG + "' onblur='onBlurWeight(this);' /><p>gr</p></div>";
        x += "</div></div>";
        x += "<script type='text/javascript'>";
        x += "function onBlurWeight(e){ var koma = e.value.split(',').pop().length; var nilaiberat = e.value; if(isNaN(e.value.replace(',','.'))) {alert('Harus Angka / Decimal.');} else{ if(koma >= 4 ){alert('Maksimal 3 digit dibelakang koma.');} else{ window.location.href = \"Sales_DataPGBeratTimbangSave.aspx?id="+myID+ "&idsopg="+IdSalesOrderPG+ "&weightpg=\"+nilaiberat+\"  \"; }}; }";
        x += "</script>";

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

    //add_plu_Click
    protected void add_plu_Click(object sender, EventArgs e)
    {
        string plu = txt_plu.Text.Trim();
        int item = 0;
        int iditem = 0;
        bool isFound = false;
		
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
		if (d != null)
		{
			// Search PG
			if (!isFound)
			{
				var rs = from p in db.StockProductPG
						 where p.Draft == false && p.StatusPenjualan == 0 && p.StockActualPG.TipeLokasi == d.TipeLokasi
						 && p.StockActualPG.IDLokasi == d.IDLokasi && p.StockActualPG.TipeLokasi == d.TipeLokasi && p.StockActualPG.Substorage == 1
						 && p.CharGoldLevel.NamaKode == "VLEI"
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
    //Save
    protected void save_Click(object sender, EventArgs e)
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
                string NewSalesOrderNumber = Document.Generate_Nomor_SalesOrderVoucherLEI(d);
                d.Nomor = NewSalesOrderNumber;
                db.SubmitChanges();
				
				//Db.Execute("EXEC sp_solve_duplicate_so_number " + myID, Db.Cnn_DB);
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

        calcPG(c, m, disc1, disc2);

        if (d == null) DataError();
        else
        {
            d.Keterangan = keterangan.Text;
            d.TotalBayarBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalBayar) + d.SalesOrderPG.Sum(p => p.TotalBayar) + d.SalesOrderGJ.Sum(p => p.TotalBayar) + d.SalesOrderLD.Sum(p => p.TotalBayar));
            d.TotalRupiahBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalRupiah) + d.SalesOrderPG.Sum(p => p.TotalRupiah) + d.SalesOrderGJ.Sum(p => p.TotalRupiah) + d.SalesOrderLD.Sum(p => p.TotalRupiah));
            db.SubmitChanges();
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
                hf = ((HiddenField)row.FindControl("pghf1_" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("pgdn2_" + index.ToString());
                txt2 = (TextBox)row.FindControl("pgdb1_" + index.ToString());
                txt3 = (TextBox)row.FindControl("pgdv1_" + index.ToString());
                txt4 = (TextBox)row.FindControl("pgdo1_" + index.ToString());
                txt5 = (TextBox)row.FindControl("pgdr1_" + index.ToString());
				ddl2 = (DropDownList)row.FindControl("pgdb1ddl_" + index.ToString());

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
                    DiscountNormalNominal = (Math.Floor((TotalRupiah * (DiscountNormalPercent / 100)) / 1000)) * 1000;

                    DiscountPromoNominal = (d.DiscountPromo.HasValue ? d.DiscountPromo.Value : 0m);
                    DiscountPromoPercent = 0m;

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
    
    protected void CalcDiscountRound(decimal TotalHarga, TextBox tb)
    {
        decimal Disc = 0m;

        App rd = new App();

        Disc = TotalHarga - rd.RoundBottom(TotalHarga);
        tb.Text = Disc.ToString("N0");
    }
}