using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

public partial class Page_Export : System.Web.UI.Page
{
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
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
    EncryptDecrypt enc = new EncryptDecrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            ddl_promo.Enabled = false;
            btnDisplay.Disabled = true;

            datepicker.Text = Str.Tgl(DateTime.Now.AddDays(1));
            
            div_table.Visible = false;

        }
    }

    protected void exportRedirect(string xtipeproduk, int ipromoid, string xloc)
    {
        err.Visible = false;
        string error_msg = "";

        try
        {
            if (ipromoid <= 0) { error_msg = "Data tidak ditemukan"; }

            if (error_msg == "")
            {
                Response.Redirect("ExportPromo" + xtipeproduk + ".aspx?tipe=" + xtipeproduk + "&id=" + ipromoid.ToString() + "&setlocid=" + xloc);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }

    // btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {
        int ipromoid = 0;

        string xtipeproduk = hf_promo_tipe.Value;
        string xpromoid = hf_promo_id.Value;
        string xlocation = "";

        try
        {
            ipromoid = Convert.ToInt32(xpromoid);
        }
        catch (Exception) { }

        xlocation = ddl_location.SelectedValue;
        
        exportRedirect(xtipeproduk, ipromoid, xlocation);

        Display_Action(xtipeproduk, ipromoid);
    }

    // btnDisplay_Click
    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        int ipromoid = 0;

        string xtipeproduk = ddl_tipeproduk2.SelectedValue;
        string xpromoid = ddl_promo.SelectedValue;

        try
        {
            ipromoid = Convert.ToInt32(xpromoid);
        }
        catch (Exception) { }

        Display_Action(xtipeproduk, ipromoid);
    }

    protected void Display_Action(string xtipeproduk, int ipromoid)
    {
        div_table.Visible = true;
        set_table(tb, xtipeproduk, ipromoid);
        ddlLocation(ddl_location, ipromoid, xtipeproduk);
    }

    // btnRetrieve_Click
    protected void btnRetrieve_Click(object sender, EventArgs e)
    {
        err.Visible = false;

        bool status = false;

        div_table.Visible = false;

        string xtipeproduk = ddl_tipeproduk2.SelectedValue;
        DateTime dt = Convert.ToDateTime(datepicker.Text);

        if (xtipeproduk == "DJ") { status = ddlPromoDJ(ddl_promo, dt); }
        else if (xtipeproduk == "PG") { status = ddlPromoPG(ddl_promo, dt); }
        else if (xtipeproduk == "GJ") { status = ddlPromoGJ(ddl_promo, dt); }

        ddl_promo.Enabled = status;
        btnDisplay.Disabled = !status;

        if (!status) { DataError("Tidak ada Promo"); }
    }

    // btnRetrievePLU_Click
    protected void btnRetrievePLU_Click(object sender, EventArgs e)
    {
        err.Visible = false;

        bool status = false;

        div_table.Visible = false;

        string xtipeproduk = ddl_tipeproduk2.SelectedValue;
        string xplu = txt_plu.Text.Trim();
        DateTime dt = Convert.ToDateTime(datepicker.Text);

        if (xtipeproduk == "DJ") { status = ddlPromoDJ(ddl_promo, dt, xplu); }
        else if (xtipeproduk == "PG") { status = ddlPromoPG(ddl_promo, dt, xplu); }
        else if (xtipeproduk == "GJ") { status = ddlPromoGJ(ddl_promo, dt, xplu); }

        ddl_promo.Enabled = status;
        btnDisplay.Disabled = !status;

        if (!status) { DataError("Tidak ada Promo"); }
    }

    protected void set_table(Table tbl, string tp, int pid)
    {
        if (pid > 0)
        {
            hf_promo_tipe.Value = tp;
            hf_promo_id.Value = pid.ToString();

            fillHeader(pid, tp);
            fillTable(pid, tp);
        }
        else
        {
            DataError("Data tidak dapat ditemukan.");
        }

    }

    protected void fillHeader(int PromoID, string TipeProduk)
    {
        lit_header1.Text = "";
        lit_header2.Text = "";
        string xheadingtipeproduk = "";

        btnExport.Visible = false;
        div_ddl_location.Visible = false;

        if (TipeProduk == "DJ")
        {
            xheadingtipeproduk = "Diamond Jewellery";
            var head = db.PromoDJ.SingleOrDefault(p => p.ID == PromoID);

            DateTime myDate1 = Convert.ToDateTime(head.TglExpire);
            DateTime myDate2 = DateTime.Now;
            TimeSpan timeSpanResult;
            timeSpanResult = myDate1 - myDate2;
            double days = Math.Ceiling(timeSpanResult.TotalDays);
            string result = "";
            if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
            else if (days == 0) { result = "<span style='color:red;'>Last day promo</span>"; }
            else if (days < 0) { result = "<span style='color:red;'>Expired since " + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " day")) + " ago</span>"; }
			
			int extraringsize = Db.SingleInteger("SELECT CASt(ISNULL(ExtraRingSize,0) AS INT) [E] FROM PromoDJ WHERE ID = " + PromoID, Db.Cnn_DB);

            if (days >= 0)
            {
                btnExport.Visible = true;
                div_ddl_location.Visible = true;
            }

            lit_header1.Text += "<table style='border:none;'>"
                        + "<tr><td>Nama Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Nama + "</td></tr>"
                        + "<tr><td>Keterangan</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Keterangan + "</td></tr>"
                        + "<tr><td>Approval Nama</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.ApprovalNama + "</td></tr>"
                        + "<tr><td>Approval Tgl</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.ApprovalTgl).ToString("dd-MM-yyyy") + "</td></tr>"
                        + "<tr><td>Tgl Start</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglStart).ToString("dd-MM-yyyy") + "</td></tr>"
                        + "<tr><td>Tgl Expire</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglExpire).ToString("dd-MM-yyyy") + " (" + result + ")</td></tr>"
                        + "</table>";
            lit_header2.Text += "<table style='border:none;'>"
                        + "<tr><td>Tipe Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + (head.Tipe == 3 ? "Promo 1 Harga" : (head.Tipe == 2 ? "Promo Poin" : "Discount")) + "</td></tr>"
                        + "<tr><td>Harga Promo</td>" + "<td>&nbsp;:&nbsp;</td><td><span style='color:blue;'>" + Convert.ToDecimal(head.PromoHarga).ToString("#,##0") + "</span></td></tr>"
                        + "<tr><td>Discount Nominal</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountNominal).ToString("#,##0") + "</td></tr>"
                        + "<tr><td>Discount Persen</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountPersen).ToString("#,##0.00") + "</td></tr>"
                        + "<tr><td>Poin Reward</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PoinReward).ToString("#,##0") + "</td></tr>"
                        + "<tr><td valign='top'>Modifikasi</td>" + "<td valign='top'>&nbsp;:&nbsp;</td><td>"
                        + (head.StatusASP == 1 ? "- No Exchange / No Resell (ASP)<br />" : "")
                        + (head.StatusDiscMembership == true ? "- No Discount Membership<br />" : "")
                        + (head.StatusDiscNormal == true ? "- No Discount Normal<br />" : "")
						+ (extraringsize > 0 ? ("+" + extraringsize.ToString() + " Ring Size") : "")
                        + "</td></tr>"
                        + "</table>";
        }
        else if (TipeProduk == "PG")
        {
            xheadingtipeproduk = "Plain Gold";
            var head = db.PromoPG.SingleOrDefault(p => p.ID == PromoID);

            DateTime myDate1 = Convert.ToDateTime(head.TglExpire);
            DateTime myDate2 = DateTime.Now;
            TimeSpan timeSpanResult;
            timeSpanResult = myDate1 - myDate2;
            double days = Math.Ceiling(timeSpanResult.TotalDays);
            string result = "";
            if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
            else if (days == 0) { result = "<span style='color:red;'>Last day promo</span>"; }
            else if (days < 0) { result = "<span style='color:red;'>Expired since " + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " day")) + " ago</span>"; }
			
			int extraringsize = Db.SingleInteger("SELECT CASt(ISNULL(ExtraRingSize,0) AS INT) [E] FROM PromoPG WHERE ID = " + PromoID, Db.Cnn_DB);

            if (days >= 0)
            {
                btnExport.Visible = true;
                div_ddl_location.Visible = true;
            }

            lit_header1.Text += "<table style='border:none;'>"
                        + "<tr><td>Nama Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Nama + "</td></tr>"
                        + "<tr><td>Keterangan</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Keterangan + "</td></tr>"
                        + "<tr><td>Approval Nama</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.ApprovalNama + "</td></tr>"
                        + "<tr><td>Approval Tgl</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.ApprovalTgl).ToString("dd-MM-yyyy") + "</td></tr>"
                        + "<tr><td>Tgl Start</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglStart).ToString("dd-MM-yyyy") + "</td></tr>"
                        + "<tr><td>Tgl Expire</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglExpire).ToString("dd-MM-yyyy") + " (" + result + ")</td></tr>"
                        + "</table>";
            lit_header2.Text += "<table style='border:none;'>"
                        + "<tr><td>Tipe Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + (head.Tipe == 3 ? "Promo 1 Harga" : (head.Tipe == 2 ? "Promo Poin" : "Discount")) + "</td></tr>"
                        + "<tr><td>Harga Promo</td>" + "<td>&nbsp;:&nbsp;</td><td><span style='color:blue;'>" + Convert.ToDecimal(head.PromoHarga).ToString("#,##0") + "</span></td></tr>"
                        + "<tr><td>Discount Nominal</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountNominal).ToString("#,##0") + "</td></tr>"
                        + "<tr><td>Discount Persen</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountPersen).ToString("#,##0.00") + "</td></tr>"
                        + "<tr><td>Poin Reward</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PoinReward).ToString("#,##0") + "</td></tr>"
                        + "<tr><td valign='top'>Modifikasi</td>" + "<td valign='top'>&nbsp;:&nbsp;</td><td>"
                        + (head.StatusASP == 1 ? "- No Exchange / No Resell (ASP)<br />" : "")
                        + (head.StatusDiscMembership == true ? "- No Discount Membership<br />" : "")
                        + (head.StatusDiscNormal == true ? "- No Discount Normal<br />" : "")
						+ (extraringsize > 0 ? ("+" + extraringsize.ToString() + " Ring Size") : "")
                        + "</td></tr>"
                        + "</table>";
        }
        else if (TipeProduk == "GJ")
        {
            xheadingtipeproduk = "Gold Jewellery";
            var head = db.PromoGJ.SingleOrDefault(p => p.ID == PromoID);

            DateTime myDate1 = Convert.ToDateTime(head.TglExpire);
            DateTime myDate2 = DateTime.Now;
            TimeSpan timeSpanResult;
            timeSpanResult = myDate1 - myDate2;
            double days = Math.Ceiling(timeSpanResult.TotalDays);
            string result = "";
            if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
            else if (days == 0) { result = "<span style='color:red;'>Last day promo</span>"; }
            else if (days < 0) { result = "<span style='color:red;'>Expired since " + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " day")) + " ago</span>"; }

            if (days >= 0) { btnExport.Visible = true; }

            lit_header1.Text += "<table style='border:none; vertical-align:top;'>"
                        + "<tr><td>Nama Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Nama + "</td></tr>"
                        + "<tr><td>Keterangan</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Keterangan + "</td></tr>"
                        + "<tr><td>Approval Nama</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.ApprovalNama + "</td></tr>"
                        + "<tr><td>Approval Tgl</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.ApprovalTgl).ToString("dd-MM-yyyy") + "</td></tr>"
                        + "<tr><td>Tgl Start</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglStart).ToString("dd-MM-yyyy") + "</td></tr>"
                        + "<tr><td>Tgl Expire</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglExpire).ToString("dd-MM-yyyy") + " (" + result + ")</td></tr>"
                        + "</table>";
            lit_header2.Text += "<table style='border:none; vertical-align:top;'>"
                        + "<tr><td>Tipe Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + (head.Tipe == 3 ? "Promo 1 Harga" : (head.Tipe == 2 ? "Promo Poin" : "Discount")) + "</td></tr>"
                        + "<tr><td>Harga Promo</td>" + "<td>&nbsp;:&nbsp;</td><td><span style='color:blue;'>" + Convert.ToDecimal(head.PromoHarga).ToString("#,##0") + "</span></td></tr>"
                        + "<tr><td>Discount Nominal</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountNominal).ToString("#,##0") + "</td></tr>"
                        + "<tr><td>Discount Persen</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountPersen).ToString("#,##0.00") + "</td></tr>"
                        + "<tr><td>Poin Reward</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PoinReward).ToString("#,##0") + "</td></tr>"
                        + "<tr><td valign='top'>Modifikasi</td>" + "<td valign='top'>&nbsp;:&nbsp;</td><td>"
                        + (head.StatusASP == 1 ? "- No Exchange / No Resell (ASP)<br />" : "")
                        + (head.StatusDiscMembership == true ? "- No Discount Membership<br />" : "")
                        + (head.StatusDiscNormal == true ? "- No Discount Normal<br />" : "")
                        + "</td></tr>"
                        + "</table>";
        }

        heading_tipeproduk.Text = xheadingtipeproduk;


    }

    protected void fillTable(int PromoID, string TipeProduk)
    {
        var items = new Dictionary<string, int>();

        //tb.Rows.Clear();
        for (int i = 1; i < tb.Rows.Count; i++)
        {
            tb.Rows.RemoveAt(i);
        }

        if (TipeProduk == "DJ")
        {
            var so = db.PromoDJ.SingleOrDefault(p => p.ID == PromoID);

            if (so != null)
            {
                /// Master Brand
                var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocWarehouse
                var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocOutlet
                var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocExhibition
                var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

                var rs = from p in db.PromoDJ_Detail
                         where p.IDForm == so.ID && p.PromoDJ.ApprovalTgl != null
                         orderby p.PromoDJ.DraftDate ascending
                         select p;

                int index = 1;
                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    string xcolor = "black";
                    if (r.StockProductDJ.Nomor == txt_plu.Text.Trim()) { tr.Style.Add("background-color", "#EEEEFF"); xcolor = "blue"; }

                    td = new TableCell();
                    td.Text = r.PromoDJ.TglExpire.Value.AddDays(1) < System.DateTime.Now ? "" : TD_HyperLink(PromoID, TipeProduk, r.IDProduct.Value);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = index.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.Nomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    td.Style.Add("color", xcolor);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CharProductItem.Nama;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CharProductCategory.NamaKode;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CharProductLevel.NamaKode;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    string StatusBarang = "";
                    StatusBarang = DL.SubStorage(r.StockProductDJ.StockActualDJ.Substorage);

                    td = new TableCell();
                    td.Text = StatusBarang;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    string NamaLokasi = "";
                    if (r.StockProductDJ.StockActualDJ.TipeLokasi == 0) { try { NamaLokasi = rsWarehouse[r.StockProductDJ.StockActualDJ.IDLokasi]; } catch { NamaLokasi = r.StockProductDJ.StockActualDJ.NamaLokasi; } }
                else if (r.StockProductDJ.StockActualDJ.TipeLokasi == 1) { try { NamaLokasi = rsOutlet[r.StockProductDJ.StockActualDJ.IDLokasi]; } catch { NamaLokasi = r.StockProductDJ.StockActualDJ.NamaLokasi; } }
                else if (r.StockProductDJ.StockActualDJ.TipeLokasi == 2) { try { NamaLokasi = rsExhibition[r.StockProductDJ.StockActualDJ.IDLokasi]; } catch { NamaLokasi = r.StockProductDJ.StockActualDJ.NamaLokasi; } }

                    td = new TableCell();
                    td.Text = NamaLokasi;
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    if (items.ContainsKey(NamaLokasi))
                    {
                        items[NamaLokasi]++;
                    }
                    else
                    {
                        items.Add(NamaLokasi, 1);
                    }

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }
        else if (TipeProduk == "PG")
        {
            var so = db.PromoPG.SingleOrDefault(p => p.ID == PromoID);

            if (so != null)
            {
                /// Master Brand
                var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocWarehouse
                var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocOutlet
                var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocExhibition
                var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

                var rs = from p in db.PromoPG_Detail
                         where p.IDForm == so.ID && p.PromoPG.ApprovalTgl != null
                         orderby p.PromoPG.DraftDate ascending
                         select p;

                int index = 1;
                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.PromoPG.TglExpire.Value.AddDays(1) < System.DateTime.Now ? "" : TD_HyperLink(PromoID, TipeProduk, r.IDProduct.Value);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = index.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.Nomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CharProductItem.Nama;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "PG";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CharGoldLevel.NamaKode;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    string StatusBarang = "";
                    StatusBarang = DL.SubStorage(r.StockProductPG.StockActualPG.Substorage);

                    td = new TableCell();
                    td.Text = StatusBarang;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    string NamaLokasi = "";
                    if (r.StockProductPG.StockActualPG.TipeLokasi == 0) { NamaLokasi = rsWarehouse[r.StockProductPG.StockActualPG.IDLokasi]; }
                    else if (r.StockProductPG.StockActualPG.TipeLokasi == 1) { NamaLokasi = rsOutlet[r.StockProductPG.StockActualPG.IDLokasi]; }
                    else if (r.StockProductPG.StockActualPG.TipeLokasi == 2) { NamaLokasi = rsExhibition[r.StockProductPG.StockActualPG.IDLokasi]; }

                    td = new TableCell();
                    td.Text = NamaLokasi;
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    if (items.ContainsKey(NamaLokasi))
                    {
                        items[NamaLokasi]++;
                    }
                    else
                    {
                        items.Add(NamaLokasi, 1);
                    }

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }
        else if (TipeProduk == "GJ")
        {
            var so = db.PromoGJ.SingleOrDefault(p => p.ID == PromoID);

            if (so != null)
            {
                /// Master Brand
                var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocWarehouse
                var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocOutlet
                var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

                /// Master Lokasi LocExhibition
                var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

                var rs = from p in db.PromoGJ_Detail
                         where p.IDForm == so.ID && p.PromoGJ.ApprovalTgl != null
                         orderby p.PromoGJ.DraftDate ascending
                         select p;

                int index = 1;
                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.PromoGJ.TglExpire.Value.AddDays(1) < System.DateTime.Now ? "" : TD_HyperLink(PromoID, TipeProduk, r.IDProduct.Value);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = index.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductGJ.Nomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CharProductItem.Nama;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CharProductCategory.NamaKode;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CharProductLevel.NamaKode;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    string StatusBarang = "";
                    StatusBarang = DL.SubStorage(r.StockProductGJ.StockActualGJ.Substorage);

                    td = new TableCell();
                    td.Text = StatusBarang;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    string NamaLokasi = "";
                    if (r.StockProductGJ.StockActualGJ.TipeLokasi == 0) { NamaLokasi = rsWarehouse[r.StockProductGJ.StockActualGJ.IDLokasi]; }
                    else if (r.StockProductGJ.StockActualGJ.TipeLokasi == 1) { NamaLokasi = rsOutlet[r.StockProductGJ.StockActualGJ.IDLokasi]; }
                    else if (r.StockProductGJ.StockActualGJ.TipeLokasi == 2) { NamaLokasi = rsExhibition[r.StockProductGJ.StockActualGJ.IDLokasi]; }

                    td = new TableCell();
                    td.Text = NamaLokasi;
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    if (items.ContainsKey(NamaLokasi))
                    {
                        items[NamaLokasi]++;
                    }
                    else
                    {
                        items.Add(NamaLokasi, 1);
                    }

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }

        lit_location_summary.Text = "";

        // Acquire keys and sort them.
        var list = items.Keys.ToList();
        list.Sort();

        int idxloc = 1;

        foreach (var key in list)
        {
            lit_location_summary.Text += "<div>" + idxloc++.ToString() + ". " + key.ToString() + " (" + Str.Number0(items[key]) + ")</div>";
        }

    }

    protected void DataError(string msg)
    {
        //err.InnerText = msg;
        //err.Attributes["class"] = "text-danger";
        err.Visible = true;

        lit_err.Text = msg;
    }

    public static bool ddlPromoDJ(DropDownList c, DateTime dt, string plu = "")
    {
        bool result = false;

        c.Items.Clear();
        using (Map_DB db = new Map_DB())
        {

        if(plu != "")
        {

            var pl = from p in db.PromoDJ_Detail
                     where p.StockProductDJ.Nomor == plu
                        && p.PromoDJ.TglExpire > System.DateTime.Now
                        && p.PromoDJ.Draft == false
                        && p.PromoDJ.ApprovalNama != null
                        && p.PromoDJ.ApprovalTgl != null
                     select p.PromoDJ;

            var rs = from p in db.PromoDJ
                     join q in pl on p.ID equals q.ID
                     where p.TglStart == (plu == "" ? dt : p.TglStart)
                        && p.Draft == false
                        && p.ApprovalNama != null
                        && p.ApprovalTgl != null
                     orderby p.Nama
                     select p;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    // tampilkan hanya yang promo yang terdapat item
                    if(r.PromoDJ_Detail.Count() > 0)
                    {
                        string t = r.Nama
                             + " - " + r.ApprovalNama
                             + " (" + r.PromoDJ_Detail.Count() + ")"
                             ;
                        string v = r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                    
                }

                result = true;
            }

        }
        else
        {
            var rs = from p in db.PromoDJ
                     where p.TglStart == (plu == "" ? dt : p.TglStart)
                        && p.Draft == false
                        && p.ApprovalNama != null
                        && p.ApprovalTgl != null
                     orderby p.Nama
                     select p;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    // tampilkan hanya yang promo yang terdapat item
                    if (r.PromoDJ_Detail.Count() > 0)
                    {
                        string t = r.Nama
                             + " - " + r.ApprovalNama
                             + " (" + r.PromoDJ_Detail.Count() + ")"
                             ;
                        string v = r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                }

                result = true;
            }

        }

        return result;
        }
    }

    public static bool ddlPromoPG(DropDownList c, DateTime dt, string plu = "")
    {
        bool result = false;

        c.Items.Clear();
        using (Map_DB db = new Map_DB())
        {

        if (plu != "")
        {

            var pl = from p in db.PromoPG_Detail
                     where p.StockProductPG.Nomor == plu
                        && p.PromoPG.TglExpire > System.DateTime.Now
                        && p.PromoPG.Draft == false
                        && p.PromoPG.ApprovalNama != null
                        && p.PromoPG.ApprovalTgl != null
                     select p.PromoPG;

            var rs = from p in db.PromoPG
                     join q in pl on p.ID equals q.ID
                     where p.TglStart == (plu == "" ? dt : p.TglStart)
                        && p.Draft == false
                        && p.ApprovalNama != null
                        && p.ApprovalTgl != null
                     orderby p.Nama
                     select p;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    // tampilkan hanya yang promo yang terdapat item
                    if (r.PromoPG_Detail.Count() > 0)
                    {
                        string t = r.Nama
                             + " - " + r.ApprovalNama
                             + " (" + r.PromoPG_Detail.Count() + ")"
                             ;
                        string v = r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                        
                }

                result = true;
            }

        }
        else
        {
            var rs = from p in db.PromoPG
                     where p.TglStart == (plu == "" ? dt : p.TglStart)
                        && p.Draft == false
                        && p.ApprovalNama != null
                        && p.ApprovalTgl != null
                     orderby p.Nama
                     select p;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    // tampilkan hanya yang promo yang terdapat item
                    if (r.PromoPG_Detail.Count() > 0)
                    {
                        string t = r.Nama
                             + " - " + r.ApprovalNama
                             + " (" + r.PromoPG_Detail.Count() + ")"
                             ;
                        string v = r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                        
                }

                result = true;
            }

        }

        return result;
        }
    }

    public static bool ddlPromoGJ(DropDownList c, DateTime dt, string plu = "")
    {
        bool result = false;

        c.Items.Clear();
        using (Map_DB db = new Map_DB())
        {

        if (plu != "")
        {

            var pl = from p in db.PromoGJ_Detail
                     where p.StockProductGJ.Nomor == plu
                        && p.PromoGJ.TglExpire > System.DateTime.Now
                        && p.PromoGJ.Draft == false
                        && p.PromoGJ.ApprovalNama != null
                        && p.PromoGJ.ApprovalTgl != null
                     select p.PromoGJ;

            var rs = from p in db.PromoGJ
                     join q in pl on p.ID equals q.ID
                     where p.TglStart == (plu == "" ? dt : p.TglStart)
                        && p.Draft == false
                        && p.ApprovalNama != null
                        && p.ApprovalTgl != null
                     orderby p.Nama
                     select p;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    // tampilkan hanya yang promo yang terdapat item
                    if (r.PromoGJ_Detail.Count() > 0)
                    {
                        string t = r.Nama
                             + " - " + r.ApprovalNama
                             + " (" + r.PromoGJ_Detail.Count() + ")"
                             ;
                        string v = r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                        
                }

                result = true;
            }

        }
        else
        {
            var rs = from p in db.PromoGJ
                     where p.TglStart == (plu == "" ? dt : p.TglStart)
                        && p.Draft == false
                        && p.ApprovalNama != null
                        && p.ApprovalTgl != null
                     orderby p.Nama
                     select p;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    // tampilkan hanya yang promo yang terdapat item
                    if (r.PromoGJ_Detail.Count() > 0)
                    {
                        string t = r.Nama
                             + " - " + r.ApprovalNama
                             + " (" + r.PromoGJ_Detail.Count() + ")"
                             ;
                        string v = r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                        
                }

                result = true;
            }

        }

        return result;
        }
    }

    public static void ddlLocation(DropDownList c, int promoid, string tipeproduk)
    {
        c.Items.Clear();
        c.Items.Add(new ListItem("...", "-1"));
        
        using (Map_DB db = new Map_DB())
        {
        
        if(tipeproduk == "DJ")
        {
            var x = db.PromoDJ_Detail.Where(p => p.PromoDJ.ID == promoid);
            var xw = from p in x where p.StockProductDJ.StockActualDJ.TipeLokasi == 0 select p.StockProductDJ.StockActualDJ.IDLokasi;
            var xo = from p in x where p.StockProductDJ.StockActualDJ.TipeLokasi == 1 select p.StockProductDJ.StockActualDJ.IDLokasi;
            var xe = from p in x where p.StockProductDJ.StockActualDJ.TipeLokasi == 2 select p.StockProductDJ.StockActualDJ.IDLokasi;


            var rs1 = from p in db.LocWarehouse where !p.Draft && !p.Disable && xw.Contains(p.ID) orderby p.Nama select p;
            foreach (var r in rs1)
            {
                string t = r.Nama + " (Warehouse)";
                string v = "0" + r.ID.ToString();
                c.Items.Add(new ListItem(t, v));
            }
            var rs2 = from p in db.LocOutlet where !p.Draft && !p.Disable && xo.Contains(p.ID) orderby p.Nama select p;
            foreach (var r in rs2)
            {
                string t = r.Kode + " - " + r.Nama + " (Outlet)";
                string v = "1" + r.ID.ToString();
                c.Items.Add(new ListItem(t, v));
            }
            var rs3 = from p in db.LocExhibition where !p.Draft && !p.Disable && xe.Contains(p.ID) orderby p.Nama select p;
            foreach (var r in rs3)
            {
                string t = r.Kode + " - " + r.Nama + " (Exhibition)";
                string v = "2" + r.ID.ToString();
                c.Items.Add(new ListItem(t, v));
            }
            
        }
        if(tipeproduk == "PG")
        {
            var x = db.PromoPG_Detail.Where(p => p.PromoPG.ID == promoid);
            var xw = from p in x where p.StockProductPG.StockActualPG.TipeLokasi == 0 select p.StockProductPG.StockActualPG.IDLokasi;
            var xo = from p in x where p.StockProductPG.StockActualPG.TipeLokasi == 1 select p.StockProductPG.StockActualPG.IDLokasi;
            var xe = from p in x where p.StockProductPG.StockActualPG.TipeLokasi == 2 select p.StockProductPG.StockActualPG.IDLokasi;


            var rs1 = from p in db.LocWarehouse where !p.Draft && !p.Disable && xw.Contains(p.ID) orderby p.Nama select p;
            foreach (var r in rs1)
            {
                string t = r.Nama + " (Warehouse)";
                string v = "0" + r.ID.ToString();
                c.Items.Add(new ListItem(t, v));
            }
            var rs2 = from p in db.LocOutlet where !p.Draft && !p.Disable && xo.Contains(p.ID) orderby p.Nama select p;
            foreach (var r in rs2)
            {
                string t = r.Kode + " - " + r.Nama + " (Outlet)";
                string v = "1" + r.ID.ToString();
                c.Items.Add(new ListItem(t, v));
            }
            var rs3 = from p in db.LocExhibition where !p.Draft && !p.Disable && xe.Contains(p.ID) orderby p.Nama select p;
            foreach (var r in rs3)
            {
                string t = r.Kode + " - " + r.Nama + " (Exhibition)";
                string v = "2" + r.ID.ToString();
                c.Items.Add(new ListItem(t, v));
            }
        }
        }
    }

    protected string TD_HyperLink(int PromoID, string PromoTipe, int IDProduct)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info btn-xs\" target=\"_blank\" href=\"../Export21/ExportPromo" + PromoTipe + ".aspx?id=" + PromoID + "&pid=" + IDProduct + "\">";
        x += "  <i class=\"icon-download-alt\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
}