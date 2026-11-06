using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_ProductAddr : System.Web.UI.Page
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

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            save.Disabled = true;
            div_expire_all.Visible = false;

            //Form
            fillHeader(sender, e);
            fillForm();
        }
    }

    //Display Header
    protected void fillHeader(object sender, EventArgs e)
    {
        var head = db.PromoDJ.SingleOrDefault(p => p.ID == myID);

        DateTime myDate1 = Convert.ToDateTime(head.TglExpire);
        DateTime myDate2 = DateTime.Now;
        TimeSpan timeSpanResult;
        timeSpanResult = myDate1 - myDate2;
        double days = Math.Ceiling(timeSpanResult.TotalDays);
        string result = "";
        if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
        else if (days == 0) { result = "Today is the last day!"; }
        else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }
		
		int extraringsize = Db.SingleInteger("SELECT CASt(ISNULL(ExtraRingSize,0) AS INT) [E] FROM PromoDJ WHERE ID = " + myID, Db.Cnn_DB);

        lit_header1.Text += "<table style='border:none;'>"
                    + "<tr><td>Nama Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Nama + "</td></tr>"
                    + "<tr><td>Keterangan</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Keterangan + "</td></tr>"
                    + "<tr><td>Tgl Start</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglStart).ToString("dd-MM-yyyy") + "</td></tr>"
                    + "<tr><td>Tgl Expire</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglExpire).ToString("dd-MM-yyyy") + " (" + result + ")</td></tr>"
                    + "<tr><td valign='top'>Modifikasi</td>" + "<td valign='top'>&nbsp;:&nbsp;</td><td>"
                    + (head.StatusASP == 1 ? "- No Exchange / No Resell (ASP)<br />" : "")
                    + (head.StatusDiscMembership == true ? "- No Discount Membership<br />" : "")
                    + (head.StatusDiscNormal == true ? "- No Discount Normal<br />" : "")
					+ (extraringsize > 0 ? ("+" + extraringsize.ToString() + " Ring Size") : "")
                    + "</td></tr>" 
                    + "</table>";
        lit_header2.Text += "<table style='border:none;'>"
                    + "<tr><td>Tipe Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + (head.Tipe == 3 ? "Promo 1 Harga" : (head.Tipe == 2 ? "Promo Poin" : "Discount")) + "</td></tr>"
                    + "<tr><td>Harga Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PromoHarga).ToString("#,##0") + "</td></tr>"
                    + "<tr><td>Discount Nominal</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountNominal).ToString("#,##0") + "</td></tr>"
                    + "<tr><td>Discount Persen</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountPersen).ToString("#,##0.00") + "</td></tr>"
                    + "<tr><td>Poin Reward</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PoinReward).ToString("#,##0") + "</td></tr>"
                    
                    + "</table>";
    }
    
    //Exception
    protected void DataError()
    {
        Response.Redirect("DJ.aspx");
    }

    //Form
    protected void fillForm()
    {
        var d = db.PromoDJ.SingleOrDefault(p => p.ID == myID && p.Draft == true);
        if (d == null) DataError();
    }
    protected void fillProduct()
    {

    }

    //btn_expire_all_yes_Click
    protected void btn_expire_all_yes_Click(object sender, EventArgs e)
    {
        string mode = "1";
        string kkmode = "2";
        string input = inputproduct.Text.Trim();

        string spname = "sp_get_promo_information_by_kode_karet";
        string query = " exec " + spname
                     + "  " + mode + ""
                     + ", " + kkmode + ""
                     + ", '" + input + "'"

                     ;
        //try
        //{
            Db.Execute(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
            inputData();
        //}
        //catch (Exception) { }
    }

    protected void btn_cancelproduct_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        inputData();
    }

    protected void inputData()
    {
        int countvaliddata = 0;

        string err_msg = "";
        string err_msg2 = "";

        bool isPromoActive = false;
        bool isPromoUpcoming = false;

        var rsH = db.PromoDJ.SingleOrDefault(p => p.ID == myID);

        lit_Message.Text = "";
        string input = inputproduct.Text.Trim();

        int mode = 1; // 0: plu, 1: kode karet
        int kkmode = 0;

        try
        {
            ////////////////////////////


            string spname = "sp_get_promo_information_by_kode_karet";
            string query = " exec " + spname
                         + "  " + mode + ""
                         + ", " + kkmode + ""
                         + ", '" + input + "'"

                         ;
            try
            {
                DataTable dt = new DataTable();
                dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                if (dt.Rows.Count > 0)
                {
                    cbl_Product.Items.Clear();

                    lit_ProductHeader.Text = TableList.ProductHeader_DJ("List PLU Promo Input");

                    int idx = 1;

                    string vSubStorage = "";

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        vSubStorage =dtRow["SubStorage"].ToString();

                        isPromoActive = (dtRow["StatusPromo"].ToString() == "ONGOING");
                        isPromoUpcoming = (dtRow["StatusPromo"].ToString() == "UPCOMING");

                        if (!isPromoActive && !isPromoUpcoming && vSubStorage == "1") { countvaliddata++; }

                        string v = dtRow["IDProduct"].ToString();
                        string t = TableList.ProductList_DJ(
                                idx,
                                dtRow["ProductItem"].ToString(),
                                dtRow["Nomor"].ToString(),
                                dtRow["ImgPicture"].ToString(),
                                DL.SubStorage(Convert.ToInt32(vSubStorage)),
                                Str.Number2(Convert.ToDecimal(dtRow["NettoWeight"].ToString())),
                                Str.Number2(Convert.ToDecimal(dtRow["GrossWeight"].ToString())),
                                Str.Number0(Convert.ToDecimal(dtRow["StoneQty"].ToString())),
                                Str.Number3(Convert.ToDecimal(dtRow["StoneCarat"].ToString())),
                                Str.Number0(Convert.ToDecimal(dtRow["HargaRupiahRound"].ToString())),
                                dtRow["NamaLokasi"].ToString(),
                                err_msg2,
                                isPromoActive,
                                isPromoUpcoming
                                );
                        cbl_Product.Items.Add(new ListItem(t, v));

                        if(vSubStorage == "1")
                        {
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Enabled = (!isPromoActive && !isPromoUpcoming);
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = (!isPromoActive && !isPromoUpcoming);
                        }
                        else
                        {
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Enabled = false;
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = false;
                        }
                        idx++;
                    }

                }
                else
                {
                    err_msg2 = "<span>There were no items matching your search. Try again with different keywords.</span>";
                }
            }
            catch (Exception) { }


            // lit_PromoDocumentAffected

            kkmode = 1;

            spname = "sp_get_promo_information_by_kode_karet";
            query = " exec " + spname
                    + "  " + mode + ""
                    + ", " + kkmode + ""
                    + ", '" + input + "'"

                    ;

            int flag_id_promo = 0;
            int flag_id_promo_next = 0;

            try
            {
                DataTable dt = new DataTable();
                dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                if (dt.Rows.Count > 0)
                {
                    div_expire_all.Visible = true;

                    lit_PromoDocumentAffected.Text = "";
                    lit_DocumentPromoAffectedHeader.Text = TableList.ProductHeader_DJ("List Document Promo Affected");


                    int idx = 1;
                    int idxdata = 1;

                    string NamaPromo = "";
                    string ApprovalNama = "";
                    string ApprovalTgl = "";
                    string TglStart = "";
                    string TglExpire = "";
                    string TipePromo = "";
                    string HargaPromo = "";
                    string DiscountNominal = "";
                    string DiscountPersen = "";
                    string StatusPromo = "";
                    string IDPromo = "";

                    string PLU = "";
                    string ProtoNomor = "";
                    string ProductItem = "";
                    string StatusBarang = "";
                    string Flag = "";

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        //dt.Rows.Count.ToString() + "<br />" + 
                        //lit_PromoDocumentAffected.Text += dt.Rows.Count.ToString() + "<br />";

                        //header
                        flag_id_promo_next = Convert.ToInt32(dtRow["IDPromo"].ToString());

                        if (idx == 1 || (idx > 1 && (flag_id_promo != flag_id_promo_next)))
                        {
                            if (idx > 1 && (flag_id_promo != flag_id_promo_next))
                            {
                                // footer to header change
                                flag_id_promo = Convert.ToInt32(dtRow["IDPromo"].ToString());
                                flag_id_promo_next = Convert.ToInt32(dtRow["IDPromo"].ToString());

                                idxdata = 1;

                                lit_PromoDocumentAffected.Text += TableList.ProductAffected_Footer_DJ();
                            }

                            if (idx == 1)
                            {
                                flag_id_promo = Convert.ToInt32(dtRow["IDPromo"].ToString());
                                flag_id_promo_next = Convert.ToInt32(dtRow["IDPromo"].ToString());
                            }

                            NamaPromo = dtRow["NamaPromo"].ToString();
                            ApprovalNama = dtRow["ApprovalNama"].ToString();
                            ApprovalTgl = Convert.ToDateTime(dtRow["ApprovalTgl"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            TglStart = Convert.ToDateTime(dtRow["TglStart"].ToString()).ToString("dd-MM-yyyy");
                            TglExpire = Convert.ToDateTime(dtRow["TglExpire"].ToString()).ToString("dd-MM-yyyy");
                            TipePromo = dtRow["TipePromo"].ToString();
                            TipePromo = (TipePromo == "3" ? "Promo 1 Harga" : (TipePromo == "2" ? "Promo Poin" : "Discount"));
                            HargaPromo = Str.Number0(Convert.ToDecimal(dtRow["PromoSatuHarga"].ToString()));
                            DiscountNominal = Str.Number0(Convert.ToDecimal(dtRow["DiscNominal"].ToString()));
                            DiscountPersen = Str.Number0(Convert.ToDecimal(dtRow["DiscPersen"].ToString()));
                            StatusPromo = dtRow["StatusPromo"].ToString();
                            IDPromo = dtRow["IDPromo"].ToString();

                            lit_PromoDocumentAffected.Text += TableList.ProductAffected_Header_DJ(
                                NamaPromo,
                                ApprovalNama,
                                ApprovalTgl,
                                TglStart,
                                TglExpire,
                                TipePromo,
                                HargaPromo,
                                DiscountNominal,
                                DiscountPersen,
                                StatusPromo,
                                IDPromo
                                );
                        }

                        // data
                        PLU = dtRow["Nomor"].ToString();
                        ProtoNomor = dtRow["ProtoNomor"].ToString();
                        ProductItem = dtRow["ProductItem"].ToString();
                        StatusBarang = dtRow["StatusJual"].ToString();
                        Flag = dtRow["Flag"].ToString();

                        lit_PromoDocumentAffected.Text += TableList.ProductAffected_Data_DJ(
                            idxdata.ToString(),
                            PLU,
                            ProtoNomor,
                            ProductItem,
                            StatusBarang,
                            Flag
                            );

                        // footer
                        if (idx == dt.Rows.Count || (idx > 1 && (flag_id_promo != flag_id_promo_next)))
                        {
                            if((idx > 1 && (flag_id_promo != flag_id_promo_next)))
                            {
                                flag_id_promo = Convert.ToInt32(dtRow["IDPromo"].ToString());
                                flag_id_promo_next = Convert.ToInt32(dtRow["IDPromo"].ToString());
                            }

                            lit_PromoDocumentAffected.Text += TableList.ProductAffected_Footer_DJ();
                        }

                        idx++;
                        idxdata++;
                    }

                }
                else
                {
                    
                }
            }
            catch (Exception) { }





            ////////////////////////////


            //// query untuk inputan kode karet
            //var rsk = from p in db.StockProductDJ
            //          where (p.ProtoNomor.Contains("-") ? p.ProtoNomor.Substring(0, p.ProtoNomor.IndexOf('-')).Equals(input) : p.ProtoNomor.Equals(input))

            //          select p;

            //// jika ditemukan inputan kode karet
            //if (rsk.Count() > 0)
            //{
            //    cbl_Product.Items.Clear();

            //    //var rsn = rsk.Select(p => p.Nomor);

            //    //// 2, Check apakah produk masih dalam status promo aktif
            //    //var rs2 = from t in db.PromoDJ_Detail
            //    //          where rsn.Contains(t.StockProductDJ.Nomor)
            //    //              && t.PromoDJ.ApprovalTgl != null
            //    //              && t.PromoDJ.ApprovalNama != null
            //    //              && Convert.ToDateTime(t.PromoDJ.TglExpire).AddDays(1) > System.DateTime.Now
            //    //              && (Convert.ToInt32(t.PromoDJ.TglStart.Value.Year.ToString().PadLeft(4, '0')
            //    //                    + t.PromoDJ.TglStart.Value.Month.ToString().PadLeft(2, '0')
            //    //                    + t.PromoDJ.TglStart.Value.Day.ToString().PadLeft(2, '0'))
            //    //                    <
            //    //                    Convert.ToInt32(System.DateTime.Now.Year.ToString().PadLeft(4, '0')
            //    //                    + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
            //    //                    + System.DateTime.Now.Day.ToString().PadLeft(2, '0'))
            //    //                    )
            //    //          select t;

            //    //// 3. Check Apakah product termasuk dalam salah satu dokumen upcoming
            //    //var rs3 = from t in db.PromoDJ_Detail
            //    //          where rsn.Contains(t.StockProductDJ.Nomor)
            //    //              && t.PromoDJ.ApprovalTgl != null
            //    //              && t.PromoDJ.ApprovalNama != null
            //    //              && Convert.ToDateTime(t.PromoDJ.TglExpire).AddDays(1) > System.DateTime.Now
            //    //              && (Convert.ToInt32(t.PromoDJ.TglStart.Value.Year.ToString().PadLeft(4, '0')
            //    //                    + t.PromoDJ.TglStart.Value.Month.ToString().PadLeft(2, '0')
            //    //                    + t.PromoDJ.TglStart.Value.Day.ToString().PadLeft(2, '0'))
            //    //                    >
            //    //                    Convert.ToInt32(System.DateTime.Now.Year.ToString().PadLeft(4, '0')
            //    //                    + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
            //    //                    + System.DateTime.Now.Day.ToString().PadLeft(2, '0'))
            //    //                    )
            //    //          select t;

            //    //var rsn2 = rs2.Select(p => p.StockProductDJ.Nomor); // promo aktif
            //    //var rsn3 = rs3.Select(p => p.StockProductDJ.Nomor); // promo upcoming



            //    

            //    int index = 1;

            //    foreach (var r in rsk)
            //    {
            //        isPromoActive = false;
            //        isPromoUpcoming = false;

            //        err_msg2 = "";

            //        var rs = db.StockProductDJ.SingleOrDefault(p => p.Nomor.Equals(r.Nomor) 
            //            //&& p.StockActualDJ.Substorage == 1
            //            && p.StatusPenjualan == 0
            //            && p.StatusLebur == false
            //            && p.ReturnProduct == false
            //            && p.Legacy == null
            //            && !(from z in db.Bundle_ItemPasangan where z.TipeProduct == 1 select z.IDProduct).Contains(p.ID)
            //            );

            //        if (rs != null)
            //        {
            //            //isPromoActive = rsn2.Contains(rs.Nomor);
            //            //isPromoUpcoming = rsn3.Contains(rs.Nomor);

            //            //err_msg2 = valid(rs.Nomor, cbl_Product);

            //            //if(err_msg2.Trim() == "")
            //            //{

            //                string v = rs.ID.ToString();
            //                string t = TableList.ProductList_DJ(
            //                        index,
            //                        rs.StockProductDJ_CharProduct.CharProductItem.Nama,
            //                        rs.Nomor,
            //                        rs.ImgPicture,
            //                        DL.SubStorage(rs.StockActualDJ.Substorage),
            //                        Str.Number2(rs.StockProductDJ_CharProduct.NettoWeight),
            //                        Str.Number2(rs.StockProductDJ_CharProduct.GrossWeight),
            //                        Str.Number0(rs.StockProductDJ_CharProduct.StoneQty),
            //                        Str.Number3(rs.StockProductDJ_CharProduct.StoneCarat),
            //                        Str.Number0(rs.StockProductDJ_PricingMU.HargaRupiahRound),
            //                        rs.StockActualDJ.NamaLokasi,
            //                        err_msg2,
            //                        isPromoActive,
            //                        isPromoUpcoming
            //                        );
            //                cbl_Product.Items.Add(new ListItem(t, v));

            //                cbl_Product.Items[cbl_Product.Items.Count - 1].Enabled = (!isPromoActive && !isPromoUpcoming);
            //                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = (!isPromoActive && !isPromoUpcoming);

            //            //}
            //            //else
            //            //{
            //            //    err_msg += err_msg2 + "<br />";
            //            //}

            //            //upl.Update();

            //            index++;


            //            inputproduct.Focus();
            //        }
            //    }

            //    // error message
            //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";

            //    save.Visible = cbl_Product.Items.Count > 0 ? true : false;

            //    btn_inputproduct.Visible = false;
            //    btn_cancelproduct.Visible = true;
            //    inputproduct.Enabled = false;
            //}
            //else // selain inputan kode karet
            //{
            //    string err_msg3 = "";
            //    int index = 1;

            //    var rs = db.StockProductDJ.SingleOrDefault(p => p.Nomor.Equals(input) 
            //            //&& p.StockActualDJ.Substorage == 1
            //            && p.StatusPenjualan == 0
            //            && p.StatusLebur == false
            //            && p.ReturnProduct == false
            //            && p.Legacy == null
            //            && !(from z in db.Bundle_ItemPasangan where z.TipeProduct == 1 select z.IDProduct).Contains(p.ID)
            //            );

            //    if(rs != null)
            //    {
            //        lit_ProductHeader.Text = TableList.ProductHeader_DJ();

            //        string curFile = @"" + rs.ImgPicture;
            //        err_msg3 = valid(input, cbl_Product);

            //        //if (!File.Exists(curFile))
            //        //{
            //        //    err_msg = "Gambar Product tidak ada.";
            //        //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
            //        //}
            //        //else
            //        //{

            //        if (err_msg3 == "")
            //        {
            //            string v = rs.ID.ToString();
            //            string t = TableList.ProductList_DJ(
            //                    index++,
            //                    rs.StockProductDJ_CharProduct.CharProductItem.Nama,
            //                    rs.Nomor,
            //                    rs.ImgPicture,
            //                    DL.SubStorage(rs.StockActualDJ.Substorage),
            //                    Str.Number2(rs.StockProductDJ_CharProduct.NettoWeight),
            //                    Str.Number2(rs.StockProductDJ_CharProduct.GrossWeight),
            //                    Str.Number0(rs.StockProductDJ_CharProduct.StoneQty),
            //                    Str.Number3(rs.StockProductDJ_CharProduct.StoneCarat),
            //                    Str.Number0(rs.StockProductDJ_PricingMU.HargaRupiahRound),
            //                    rs.StockActualDJ.NamaLokasi
            //                    );
            //            cbl_Product.Items.Add(new ListItem(t, v));
            //            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

            //            //upl.Update();

            //            inputproduct.Focus();
            //        }
            //    }

            //    // error message
            //    lit_Message.Text = "<span style='color:red;'>" + err_msg3 + "</span>";

            //    save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            //    //}
            //}

            save.Disabled = !(countvaliddata > 0);

        }
        catch
        {
            // error message
            lit_Message.Text = "<span style='color:red;'>Invalid Product Code !!!</span>";
        }
    }

    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = db.StockProductDJ.SingleOrDefault(p => p.Nomor == input && !p.Draft.Value);

        if (rs1 == null) { message = "Invalid Product"; }
        else
        {

            // 2, Check apakah produk masih dalam status promo aktif
            var rs2 = from t in db.PromoDJ_Detail
                      where t.StockProductDJ.Nomor == input
                          && t.PromoDJ.ApprovalTgl != null
                          && t.PromoDJ.ApprovalNama != null
                          && Convert.ToDateTime(t.PromoDJ.TglExpire).AddDays(1) > System.DateTime.Now 
                          && (  Convert.ToInt32(t.PromoDJ.TglStart.Value.Year.ToString().PadLeft(4,'0') 
                                + t.PromoDJ.TglStart.Value.Month.ToString().PadLeft(2,'0') 
                                + t.PromoDJ.TglStart.Value.Day.ToString().PadLeft(2,'0'))
                                <
                                Convert.ToInt32(System.DateTime.Now.Year.ToString().PadLeft(4,'0') 
                                + System.DateTime.Now.Month.ToString().PadLeft(2,'0') 
                                + System.DateTime.Now.Day.ToString().PadLeft(2,'0'))
                                )
                      select t;
            if (rs2.Count() > 0) 
            { 
                message = "PLU " + input + " masih termasuk dalam promo aktif : <br />";
                foreach (var data in rs2)
                {
                    DateTime myDate1 = Convert.ToDateTime(data.PromoDJ.TglExpire);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate1 - myDate2;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
                    else if (days == 0) { result = "Today is the last day!"; }
                    else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

                    message += "<table style='border:none;'>"
                            + "<tr><td>Nama Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + data.PromoDJ.Nama + "</td></tr>"
                            + "<tr><td>Approval Nama</td>" + "<td>&nbsp;:&nbsp;</td><td>" + data.PromoDJ.ApprovalNama + "</td></tr>"
                            + "<tr><td>Approval Tgl</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(data.PromoDJ.ApprovalTgl).ToString("dd-MM-yyyy") + "</td></tr>"
                            + "<tr><td>Tgl Start</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(data.PromoDJ.TglStart).ToString("dd-MM-yyyy") + "</td></tr>"
                            + "<tr><td>Tgl Expire</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(data.PromoDJ.TglExpire).ToString("dd-MM-yyyy") + " (" + result + ")</td></tr>"
                            + "</table>";
                }
            }

            // 3. Check Apakah product termasuk dalam salah satu dokumen upcoming
            var rs3 = from t in db.PromoDJ_Detail
                      where t.StockProductDJ.Nomor == input
                          && t.PromoDJ.ApprovalTgl != null
                          && t.PromoDJ.ApprovalNama != null
                          && Convert.ToDateTime(t.PromoDJ.TglExpire).AddDays(1) > System.DateTime.Now
                          && (Convert.ToInt32(t.PromoDJ.TglStart.Value.Year.ToString().PadLeft(4, '0')
                                + t.PromoDJ.TglStart.Value.Month.ToString().PadLeft(2, '0')
                                + t.PromoDJ.TglStart.Value.Day.ToString().PadLeft(2, '0'))
                                >
                                Convert.ToInt32(System.DateTime.Now.Year.ToString().PadLeft(4, '0')
                                + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Day.ToString().PadLeft(2, '0'))
                                )
                      select t;
            if (rs3.Count() > 0)
            {
                message = "PLU " + input + " termasuk dalam promo upcoming : <br />";
                foreach (var data in rs3)
                {
                    DateTime myDate1 = Convert.ToDateTime(data.PromoDJ.TglStart);
                    DateTime myDate2 = DateTime.Now;
                    TimeSpan timeSpanResult;
                    timeSpanResult = myDate2 - myDate1;
                    double days = Math.Ceiling(timeSpanResult.TotalDays);
                    string result = "";
                    if (days > 0) { result = days.ToString("#,##0") + " more " + ((days > 1 ? " days" : " days")); }
                    else if (days == 0) { result = "Tomorrow"; }
                    else if (days == -1) { result = "Today"; }
                    else if (days < -1) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + "</span>"; }

                    message += "<table style='border:none;'>"
                            + "<tr><td>Nama Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + data.PromoDJ.Nama + "</td></tr>"
                            + "<tr><td>Approval Nama</td>" + "<td>&nbsp;:&nbsp;</td><td>" + data.PromoDJ.ApprovalNama + "</td></tr>"
                            + "<tr><td>Approval Tgl</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(data.PromoDJ.ApprovalTgl).ToString("dd-MM-yyyy") + "</td></tr>"
                            + "<tr><td>Tgl Start</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(data.PromoDJ.TglStart).ToString("dd-MM-yyyy") + " (" + result + ")</td></tr>"
                            + "<tr><td>Tgl Expire</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(data.PromoDJ.TglExpire).ToString("dd-MM-yyyy") + "</td></tr>"
                            + "</table>";
                }
            }

            // 4. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
            }


            // 5. Check Apakah product valid status substoragenya untuk dikirim ke gudang


        }

        return message;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                var d = db.StockProductDJ.SingleOrDefault(p=>p.ID == Convert.ToInt32(cbl_Product.Items[i].Value));

                PromoDJ_Detail n = new PromoDJ_Detail();

                n.IDForm = myID;
                n.IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value);
                n.IDCategory = d.StockProductDJ_CharProduct.ProductCategory;
                n.IDItem = d.StockProductDJ_CharProduct.ProductItem;
                n.IDLevel = d.StockProductDJ_CharProduct.ProductLevel;
                n.IDLokasi = d.StockActualDJ.IDLokasi;
                n.TipeLokasi = d.StockActualDJ.TipeLokasi;

                db.PromoDJ_Detail.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        var rsH = db.PromoDJ.SingleOrDefault(p => p.ID == myID);
        rsH.Draft = false;
        //Commit
        db.SubmitChanges();

        //Redirect
        Response.Redirect("DJ.aspx?d=1");

    }

}

