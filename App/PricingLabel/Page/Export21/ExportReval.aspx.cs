using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class Page_Export : System.Web.UI.Page
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
    EncryptDecrypt enc = new EncryptDecrypt();

    protected int limitdata = 750;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //datepicker.Text = Str.Tgl(DateTime.Now.AddDays(1));
            div_table.Visible = false;

            //DLC.LocationList(lokasi, tipe);
            DLC.CharProductItem(ddl_productitem);
            DLC.CharProductCategory(ddl_productcategory);
            DLC.CharProductLevel(ddl_productlevel);
            ddlLocation(ddl_location);

            Fill();
        }


    }

    protected void Fill()
    {
        string xmessage = "";
        string xtipeproduk = "";
        string xproductitem = "";
        string xproductcategory = "";
        string xproductlevel = "";
        string xlocationtipe = "";
        string xlocationid = "";
        string xpludari = "";
        string xplusampai = "";

        xmessage = Request.QueryString["msg"];
        xtipeproduk = Request.QueryString["v"];
        xproductitem = Request.QueryString["it"];
        xproductcategory = Request.QueryString["ct"];
        xproductlevel = Request.QueryString["lv"];
        xlocationtipe = Request.QueryString["tp"];
        xlocationid = Request.QueryString["cb"];
        xpludari = Request.QueryString["fr"];
        xplusampai = Request.QueryString["to"];

        if (xmessage == "0") { DataError("PLU tidak ditemukan"); }
        else if (xmessage == limitdata.ToString()) { DataError("Jumlah lebih dari " + limitdata.ToString() + " PLU"); }

        if (xtipeproduk != "") { ddl_tipeproduk.SelectedValue = xtipeproduk; }

        if (xproductitem != "") { ddl_productitem.SelectedValue = xproductitem; }

        if (xproductcategory != "") { ddl_productcategory.SelectedValue = xproductcategory; }

        if (xproductlevel != "") { ddl_productlevel.SelectedValue = xproductlevel; }

        if (xlocationtipe != "" && xlocationtipe != "9") { ddl_location.SelectedValue = (xlocationtipe + xlocationid); }

    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        FillTable();
    }
    protected void FillTable()
    {
        err.Visible = false;
        lit_err.Text = "";
        div_table.Visible = false;

        string xtipeproduk = ddl_tipeproduk.SelectedValue;

        btnPrint1.Visible = false;

        tbrv_valid.Visible = false;
        tbrv_invalid.Visible = false;
        tbrv_promo.Visible = false;
        tbrv_pending.Visible = false;
        tbrv_sold.Visible = false;

        if (xtipeproduk == "DJ") { SetDJ(xtipeproduk); }
        else if (xtipeproduk == "PG") { SetPG(xtipeproduk); }

    }

    // btnPrint1_Click
    protected void btnPrint1_Click(object sender, EventArgs e)
    {
        err.Visible = false;

        string xtipeproduk = ddl_tipeproduk.SelectedValue;

        if (xtipeproduk == "DJ") { exportDJ(); }
        else if (xtipeproduk == "PG") { exportPG(); }

        //lit_text_test.Text = dt.ToString();
    }

    protected void exportDJ(string SingleNomorPLU = "")
    {
        err.Visible = false;
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        string Level = ddl_productlevel.SelectedValue;
        string Clarity = ddl_clarity.SelectedValue;

        if (Cabang == "") { Cabang = "0"; }

        if (rdo_loc_w.Checked) { TipeLokasi = "0"; Cabang = "0"; }

        string error_msg = "";

        try
        {
            //if (Cabang == "0") { error_msg = "Cabang harus dipilih."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportRevalDJ.aspx?productitem=" + Item + "&tipe=" + TipeLokasi + "&cabang=" + Cabang + "&productcategory=" + Category + "&productlevel=" + Level + "&clarity=" + Clarity);
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

    protected void exportPG(string SingleNomorPLU = "")
    {
        err.Visible = false;
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;

        if (rdo_loc_w.Checked) { TipeLokasi = "0"; Cabang = "0"; }

        string error_msg = "";

        try
        {
            //if (Cabang == "0") { error_msg = "Cabang harus dipilih."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportRevalPG.aspx?productitem=" + Item + "&tipe=" + TipeLokasi + "&cabang=" + Cabang);
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

    protected void DataError(string msg)
    {
        //err.InnerText = msg;
        //err.Attributes["class"] = "text-danger";
        err.Visible = true;

        lit_err.Text = msg;
    }

    public static void ddlPromoDJ(DropDownList c, DateTime dt)
    {
        c.Items.Clear();
        using (Map_DB db = new Map_DB())
        {
        var rs = from p in db.PromoDJ where p.TglStart == dt orderby p.Nama select p;
        foreach (var r in rs)
        {
            string t = r.Nama + " (" + r.PromoDJ_Detail.Count() + ")";
            string v = r.ID.ToString();
            c.Items.Add(new ListItem(t, v));
        }
        }
    }

    public static void ddlLocation(DropDownList c)
    {
        c.Items.Clear();
        c.Items.Add(new ListItem("...", "9"));

        using (Map_DB db = new Map_DB())
        {
        var rs1 = from p in db.LocWarehouse where !p.Draft && !p.Disable orderby p.Nama select p;
        foreach (var r in rs1)
        {
            string t = r.Nama + " (Warehouse)";
            string v = "0" + r.ID.ToString();
            c.Items.Add(new ListItem(t, v));
        }
        var rs2 = from p in db.LocOutlet where !p.Draft && !p.Disable orderby p.Nama select p;
        foreach (var r in rs2)
        {
            string t = r.Kode + " - " + r.Nama + " (Outlet)";
            string v = "1" + r.ID.ToString();
            c.Items.Add(new ListItem(t, v));
        }
        var rs3 = from p in db.LocExhibition where !p.Draft && !p.Disable orderby p.Nama select p;
        foreach (var r in rs3)
        {
            string t = r.Kode + " - " + r.Nama + " (Exhibition)";
            string v = "2" + r.ID.ToString();
            c.Items.Add(new ListItem(t, v));
        }
        }
    }

    protected void SetDJ(string TipeProduk)
    {
        tbrv_valid.Visible = true;
        tbrv_invalid.Visible = true;
        tbrv_promo.Visible = true;
        tbrv_pending.Visible = true;
        tbrv_sold.Visible = true;

        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        string Level = ddl_productlevel.SelectedValue;
        string Clarity = ddl_clarity.SelectedValue;

        int tipe = 0;
        int cabang = 0;
        int ProductItem = 0;
        int ProductCategory = 0;
        int ProductLevel = 0;

        try
        {
            tipe = Convert.ToInt32(TipeLokasi);
        }
        catch (Exception) { }

        try
        {
            cabang = Convert.ToInt32(Cabang);
        }
        catch (Exception) { }
        try
        {
            ProductItem = Convert.ToInt32(Item);
        }
        catch (Exception) { }
        try
        {
            ProductCategory = Convert.ToInt32(Category);
        }
        catch (Exception) { }
        try
        {
            ProductLevel = Convert.ToInt32(Level);
        }
        catch (Exception) { }
        string xTest = "0";

        try
        {
            var rv = from p in db.StockProductDJ
                     join q in db.StockActualDJ on p.ID equals q.IDProduct
                     join r in db.RevalItemDJ on q.IDProduct equals r.IDProduct
                     join s in db.RevalProductDJ on r.ID equals s.IDReval
                     where (p.Legacy == null || p.Legacy == 0) && p.Draft == false
                     select new { p, q, r, s };

            // clarity filter
            if (Clarity == "1")
            {
                //rv = from p in rv
                //     where !p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4") && !z.Stone1A.Nama.Contains("B4"))
                //     select p;

                rv = rv.Where(p => !p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4"))
                            && !p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("B4"))

                            );
            }
            else
            if (Clarity == "2")
            {
                // W4 = F VS = VS
                rv = rv.Where(p => p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4")));
            }
            else if (Clarity == "3")
            {
                // B4 = H VS = RED
                rv = rv.Where(p => p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("B4")));
            }


            if (rdo_loc_w.Checked)
            {
                rv = rv.Where(p => p.q.TipeLokasi == 0);
            }

            if (ProductItem > 0)
            {
                rv = rv.Where(p => p.p.StockProductDJ_CharProduct.ProductItem == ProductItem);
            }

            if (ProductCategory > 0)
            {
                rv = rv.Where(p => p.p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
            }

            if (ProductLevel > 0)
            {
                rv = rv.Where(p => p.p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
            }

            xTest = "4C";

            if ((tipe >= 0 || tipe <= 2) && cabang > 0)
            {
                rv = rv.Where(p => p.p.StockActualDJ.TipeLokasi == tipe && p.p.StockActualDJ.IDLokasi == cabang);
            }

            var x = from p in rv
                    select new
                    {
                        IDProduct = p.p.ID,
                        PLU = p.p.Nomor,
                        ProductItem = p.p.StockProductDJ_CharProduct.CharProductItem.Nama,
                        ProductCategory = p.p.StockProductDJ_CharProduct.CharProductCategory.Nama,
                        Location = p.q.NamaLokasi,
                        Substorage = p.q.Substorage,
                        Harga = p.p.StockProductDJ_PricingMU.HargaRupiahRound,
                        HargaReval = p.s.RevalProductDJ_PricingMU.HargaRupiahRound,
                        StatusJual = p.p.StatusPenjualan
                    }
                    ;

            var w = (from p in db.PromoDJ_Detail
                     join q in db.PromoDJ on p.IDForm equals q.ID
                     where (from y in rv select y.p.ID).Contains(p.IDProduct.Value)
                         && p.PromoDJ.TglExpire >= System.DateTime.Now
                     select new { xPromoHarga = q.PromoHarga, xIDProduct = p.IDProduct })
                    ;

            var m = from p in x
                    select new
                    {
                        IDProduct = p.IDProduct,
                        PLU = p.PLU,
                        ProductItem = p.ProductItem,
                        ProductCategory = p.ProductCategory,
                        Location = p.Location,
                        SubStorage = p.Substorage,
                        Harga = p.Harga,
                        HargaReval = p.HargaReval,
                        HargaPromo = (w.FirstOrDefault(q => q.xIDProduct == p.IDProduct).xPromoHarga),
                        StatusJual = p.StatusJual
                    }
                    ;

            var rs1 = m.Where(p => p.StatusJual == 0 && p.HargaPromo == null && (decimal)p.HargaReval != (decimal)p.Harga);
            var rs2 = m.Where(p => p.StatusJual == 0 && p.HargaPromo == null && p.HargaReval == null);
            var rs3 = m.Where(p => p.StatusJual == 0 && p.HargaPromo != null);
            var rs4 = m.Where(p => p.StatusJual == 0 && p.HargaPromo == null && (decimal)p.HargaReval == (decimal)p.Harga);
            var rs5 = m.Where(p => p.StatusJual == 1);

            rs1 = rs1.OrderBy(p => p.PLU);
            rs2 = rs2.OrderBy(p => p.PLU);
            rs3 = rs3.OrderBy(p => p.PLU);
            rs4 = rs4.OrderBy(p => p.PLU);
            rs5 = rs5.OrderBy(p => p.PLU);

            int count = 1;
            int totalcountdata = 0;

            totalcountdata = rs1.Count();
            if (totalcountdata > 0)
            {
                btnPrint1.Visible = true;

                lit_rv_valid.Text = "<h3><span class='label label-info'>Valid</span></h3>";

                foreach (var rslt in rs1)
                {
                    xTest = "7";

                    string xIDProduct = "";
                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xIDProduct = rslt.IDProduct.ToString();
                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xCategory = rslt.ProductCategory;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.SubStorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    //if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = (xReval != "0") ? TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), ProductLevel.ToString(), tipe.ToString(), cabang.ToString(), xIDProduct) : "";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    tbrv_valid.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }
            }
            else
            {
                tbrv_valid.Visible = false;
            }
            lit_count_valid.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            count = 1;
            totalcountdata = rs2.Count();
            if (totalcountdata > 0)
            {
                lit_rv_invalid.Text = "<h3><span class='label label-danger'>Invalid</span></h3>";

                foreach (var rslt in rs2)
                {
                    xTest = "7";

                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xCategory = rslt.ProductCategory;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.SubStorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xPromo;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "red");
                    tr.Cells.Add(td);

                    tbrv_invalid.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }

            }
            else
            {
                tbrv_invalid.Visible = false;
            }
            lit_count_invalid.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            count = 1;
            totalcountdata = rs3.Count();
            if (totalcountdata > 0)
            {
                lit_rv_promo.Text = "<h3><span class='label label-warning'>Promo</span></h3>";

                foreach (var rslt in rs3)
                {
                    xTest = "7";

                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xCategory = rslt.ProductCategory;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.SubStorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    //if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xPromo;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "red");
                    tr.Cells.Add(td);

                    tbrv_promo.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }

            }
            else
            {
                tbrv_promo.Visible = false;
            }
            lit_count_promo.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            count = 1;
            totalcountdata = rs4.Count();
            if (totalcountdata > 0)
            {
                lit_rv_pending.Text = "<h3><span class='label label-default'>Pending</span></h3>";

                foreach (var rslt in rs4)
                {
                    xTest = "7";

                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xCategory = rslt.ProductCategory;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.SubStorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xPromo;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "red");
                    tr.Cells.Add(td);

                    tbrv_pending.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }

            }
            else
            {
                tbrv_pending.Visible = false;
            }
            lit_count_pending.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            count = 1;
            totalcountdata = rs5.Count();
            if (totalcountdata > 0)
            {
                lit_rv_sold.Text = "<h3><span class='label label-default'>Sold</span></h3>";

                foreach (var rslt in rs5)
                {
                    xTest = "7";

                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xCategory = rslt.ProductCategory;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.SubStorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xPromo;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "red");
                    tr.Cells.Add(td);

                    tbrv_sold.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }

            }
            else
            {
                tbrv_sold.Visible = false;
            }
            lit_count_sold.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            string groupitem1 = "";
            string groupitem2 = "";
            string groupitem3 = "";
            string groupitem4 = "";
            string groupitem5 = "";

            ///////// 1
            var group_product_item1 = from p in rs1
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item1)
            {
                groupitem1 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }


            ///////// 2
            var group_product_item2 = from p in rs2
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item2)
            {
                groupitem2 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }


            ///////// 3
            var group_product_item3 = from p in rs3
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item3)
            {
                groupitem3 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }


            ///////// 4
            var group_product_item4 = from p in rs4
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item4)
            {
                groupitem4 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }

            ///////// 5
            var group_product_item5 = from p in rs5
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item5)
            {
                groupitem5 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }


            SetHeaderCategory(rs1.Count(), groupitem1, lit_rv_valid, "Valid", "info");
            SetHeaderCategory(rs2.Count(), groupitem2, lit_rv_invalid, "Invalid", "danger");
            SetHeaderCategory(rs3.Count(), groupitem3, lit_rv_promo, "Promo", "warning");
            SetHeaderCategory(rs4.Count(), groupitem4, lit_rv_pending, "Pending", "default");
            SetHeaderCategory(rs5.Count(), groupitem5, lit_rv_sold, "Sold", "danger");

            div_table.Visible = true;
            SetHeader();

        }
        catch (Exception ex)
        {
            xTest = "Error Code : 0x" + xTest.PadLeft(3, '0')
                  + "<br /><br />Error Message : <span>" + ex.Message.ToString() + "</span>"
                  + "<br /><br />Please contact IT Administrator within the error code number and message."
                  ;

            DataError(xTest);
        }
    }

    protected void SetPG(string TipeProduk)
    {
        tbrv_valid.Visible = true;
        tbrv_invalid.Visible = true;
        tbrv_promo.Visible = true;
        tbrv_pending.Visible = true;
        tbrv_sold.Visible = true;

        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        string Level = ddl_productlevel.SelectedValue;
        string Clarity = ddl_clarity.SelectedValue;

        int tipe = 0;
        int cabang = 0;
        int ProductItem = 0;
        int ProductCategory = 0;
        int ProductLevel = 0;

        List<PGReval> list = new List<PGReval>();

        try
        {
            tipe = Convert.ToInt32(TipeLokasi);
        }
        catch (Exception) { }

        try
        {
            cabang = Convert.ToInt32(Cabang);
        }
        catch (Exception) { }
        try
        {
            ProductItem = Convert.ToInt32(Item);
        }
        catch (Exception) { }
        try
        {
            ProductCategory = Convert.ToInt32(Category);
        }
        catch (Exception) { }
        try
        {
            ProductLevel = Convert.ToInt32(Level);
        }
        catch (Exception) { }

        string xTest = "0";

        try
        {
            string spname = "sp_getLabel_reval_pg ";
            string query = " exec " + spname
                         + cabang + ", "
                         + tipe + ", "
                         + ProductItem;

            DataTable dt = Db.Rs(query, Db.Cnn_DB);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    decimal? HargaPromo;
                    if (row["HargaPromo"] == DBNull.Value) HargaPromo = null;
                    else HargaPromo = Convert.ToDecimal(row["HargaPromo"]);
                    list.Add(new PGReval
                    {
                        IDProduct = Convert.ToInt32(row["IDProduct"]),
                        PLU = row["Nomor"].ToString(),
                        ProductItem = row["ProductItem"].ToString(),
                        LocationType = Convert.ToInt32(row["LocationType"]),
                        Location = row["Location"].ToString(),
                        Substorage = Convert.ToInt32(row["Substorage"]),
                        Harga = Convert.ToDecimal(row["Harga"]),
                        HargaReval = Convert.ToDecimal(row["HargaReval"]),
                        HargaPromo = HargaPromo,
                        StatusPenjualan = Convert.ToInt32(row["StatusPenjualan"]),
                    });
                }
            }

            var rs1 = list.Where(p => p.StatusPenjualan == 0 && p.HargaPromo == null && (decimal)p.HargaReval != (decimal)p.Harga);
            var rs2 = list.Where(p => p.StatusPenjualan == 0 && p.HargaPromo == null && p.HargaReval == null);
            var rs3 = list.Where(p => p.StatusPenjualan == 0 && p.HargaPromo != null);
            var rs4 = list.Where(p => p.StatusPenjualan == 0 && p.HargaPromo == null && (decimal)p.HargaReval == (decimal)p.Harga);
            var rs5 = list.Where(p => p.StatusPenjualan == 1);

            rs1 = rs1.OrderBy(p => p.PLU);
            rs2 = rs2.OrderBy(p => p.PLU);
            rs3 = rs3.OrderBy(p => p.PLU);
            rs4 = rs4.OrderBy(p => p.PLU);
            rs5 = rs5.OrderBy(p => p.PLU);

            int count = 1;
            int totalcountdata = 0;

            totalcountdata = rs1.Count();
            if (totalcountdata > 0)
            {
                btnPrint1.Visible = true;

                lit_rv_valid.Text = "<h3><span class='label label-info'>Valid</span></h3>";

                foreach (var rslt in rs1)
                {
                    xTest = "7";

                    string xIDProduct = "";
                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xIDProduct = rslt.IDProduct.ToString();
                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.Substorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    //if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = (xReval != "0") ? TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), ProductLevel.ToString(), tipe.ToString(), cabang.ToString(), xIDProduct) : "";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    tbrv_valid.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }
            }
            else
            {
                tbrv_valid.Visible = false;
            }
            lit_count_valid.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            count = 1;
            totalcountdata = rs2.Count();
            if (totalcountdata > 0)
            {
                lit_rv_invalid.Text = "<h3><span class='label label-danger'>Invalid</span></h3>";

                foreach (var rslt in rs2)
                {
                    xTest = "7";

                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.Substorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xPromo;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "red");
                    tr.Cells.Add(td);

                    tbrv_invalid.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }

            }
            else
            {
                tbrv_invalid.Visible = false;
            }
            lit_count_invalid.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            count = 1;
            totalcountdata = rs3.Count();
            if (totalcountdata > 0)
            {
                lit_rv_promo.Text = "<h3><span class='label label-warning'>Promo</span></h3>";

                foreach (var rslt in rs3)
                {
                    xTest = "7";

                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.Substorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    //if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xPromo;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "red");
                    tr.Cells.Add(td);

                    tbrv_promo.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }

            }
            else
            {
                tbrv_promo.Visible = false;
            }
            lit_count_promo.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            count = 1;
            totalcountdata = rs4.Count();
            if (totalcountdata > 0)
            {
                lit_rv_pending.Text = "<h3><span class='label label-default'>Pending</span></h3>";

                foreach (var rslt in rs4)
                {
                    xTest = "7";

                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.Substorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xPromo;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "red");
                    tr.Cells.Add(td);

                    tbrv_pending.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }

            }
            else
            {
                tbrv_pending.Visible = false;
            }
            lit_count_pending.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            count = 1;
            totalcountdata = rs5.Count();
            if (totalcountdata > 0)
            {
                lit_rv_sold.Text = "<h3><span class='label label-default'>Sold</span></h3>";

                foreach (var rslt in rs5)
                {
                    xTest = "7";

                    string xNomor = "";
                    string xItem = "";
                    string xCategory = "";
                    string xLocation = "";
                    string xSubStorage = "";
                    string xHarga = "";
                    string xReval = "";
                    string xPromo = "";

                    xNomor = rslt.PLU;
                    xItem = rslt.ProductItem;
                    xLocation = rslt.Location;
                    xSubStorage = DL.SubStorage(rslt.Substorage);
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xReval = Convert.ToDecimal(rslt.HargaReval).ToString("N0");
                    xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                    xPromo = xPromo == "0" ? "" : xPromo;

                    xTest = "7A";

                    TableRow tr = new TableRow();
                    TableCell td;

                    if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                    td = new TableCell();
                    td.Text = count.ToString() + ".";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xNomor;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xItem;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xCategory;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xLocation + "<br />Status : " + (xSubStorage != "Jual" ? ("<span style='color:red;'>" + xSubStorage + "</span>") : xSubStorage);
                    td.Style.Add("text-align", "left");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xHarga;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "blue");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xReval + TD_Mutasi(Convert.ToDecimal(xHarga), Convert.ToDecimal(xReval));
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = xPromo;
                    td.Style.Add("text-align", "right");
                    td.Style.Add("vertical-align", "top");
                    td.Style.Add("color", "red");
                    tr.Cells.Add(td);

                    tbrv_sold.Rows.Add(tr);


                    xTest = "8";

                    xTest = "9";

                    count++;
                }

            }
            else
            {
                tbrv_sold.Visible = false;
            }
            lit_count_sold.Text = "<span class='badge'>" + Str.Number0(totalcountdata) + "</span>";

            string groupitem1 = "";
            string groupitem2 = "";
            string groupitem3 = "";
            string groupitem4 = "";
            string groupitem5 = "";

            ///////// 1
            var group_product_item1 = from p in rs1
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item1)
            {
                groupitem1 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }


            ///////// 2
            var group_product_item2 = from p in rs2
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item2)
            {
                groupitem2 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }


            ///////// 3
            var group_product_item3 = from p in rs3
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item3)
            {
                groupitem3 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }


            ///////// 4
            var group_product_item4 = from p in rs4
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item4)
            {
                groupitem4 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }

            ///////// 5
            var group_product_item5 = from p in rs5
                                      group p by new { p.ProductItem } into s
                                      select new
                                      {
                                          ProductItem = s.Key.ProductItem,
                                          Qty = s.Count()
                                      };
            foreach (var rsItem in group_product_item5)
            {
                groupitem5 += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
            }


            SetHeaderCategory(rs1.Count(), groupitem1, lit_rv_valid, "Valid", "info");
            SetHeaderCategory(rs2.Count(), groupitem2, lit_rv_invalid, "Invalid", "danger");
            SetHeaderCategory(rs3.Count(), groupitem3, lit_rv_promo, "Promo", "warning");
            SetHeaderCategory(rs4.Count(), groupitem4, lit_rv_pending, "Pending", "default");
            SetHeaderCategory(rs5.Count(), groupitem5, lit_rv_sold, "Sold", "danger");

            div_table.Visible = true;
            SetHeader();

        }
        catch (Exception ex)
        {
            xTest = "Error Code : 0x" + xTest.PadLeft(3, '0')
                  + "<br /><br />Error Message : <span>" + ex.Message.ToString() + "</span>"
                  + "<br /><br />Please contact IT Administrator within the error code number and message."
                  ;

            DataError(xTest);
        }
    }

    protected void SetHeader()
    {
        lit_header1.Text = "";
        lit_header2.Text = "";

        string xqty = "";
        string xproducttype = "";
        string xlocation = "";
        string xproductitem = "";
        string xproductcategory = "";
        string xproductlevel = "";
        string xclarity = "";

        xproducttype = ddl_tipeproduk.SelectedItem.Text;
        xlocation = (rdo_loc_w.Checked) ? "All Warehouse" : ddl_location.SelectedItem.Text;
        xproductitem = ddl_productitem.SelectedItem.Text;
        xproductcategory = ddl_productcategory.SelectedItem.Text;
        xproductlevel = ddl_productlevel.SelectedItem.Text;
        xclarity = ddl_clarity.SelectedItem.Text;

        heading_tipeproduk.Text = xproducttype;

        lit_header1.Text += "<table style='border:none;'>"
                    + "<tr><td style='vertical-align:top;'>Location</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xlocation + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>Product Item</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xproductitem + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>Product Category (DJ/GJ only)</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xproductcategory + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>Product Level (DJ/GJ only)</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xproductlevel + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>Stone Clarity (DJ only)</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xclarity + "</td></tr>"
                    + "</table>";
        //lit_header2.Text += "<table style='border:none;'>"
        //            + "<tr><td style='vertical-align:top;'>Qty</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xqty + "</td></tr>"
        //            + "<tr><td style='vertical-align:top;'>Item Summary</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xgroupitem + "</td></tr>"
        //            + "</table>";

    }


    protected void SetHeaderCategory(int qty, string xgroupitem, Literal lit, string xcategory, string xtype = "default")
    {
        lit.Text = "";


        lit.Text += "<div class='row'>";
        lit.Text += "<div class='col-xs-12 col-sm-6'>";
        lit.Text += "<table style='border:none;'>";
        lit.Text += "<tr><td style='vertical-align:top;'>Status</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + "<span class='label label-" + xtype + "'>" + xcategory + "</span>" + "</td></tr>";
        lit.Text += "<tr><td style='vertical-align:top;'>Qty</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + Str.Number0(qty) + "</td></tr>";
        lit.Text += "</table>";
        lit.Text += "</div>";
        lit.Text += "<div class='col-xs-12 col-sm-6'>";
        lit.Text += "<table style='border:none;'>";
        lit.Text += "<tr><td style='vertical-align:top;'>Item Summary</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xgroupitem + "</td></tr>";
        lit.Text += "</table>";
        lit.Text += "</div>";
        lit.Text += "</div>";

    }


    protected string TD_HyperLink(string TipeProduk, string From, string To, string Item, string Category, string Level, string TipeLokasi, string Cabang, string IDProduct)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info btn-xs\" target=\"_blank\" href=\"../Export21/ExportReval" + TipeProduk + ".aspx?productitem=" + Item + "&productcategory=" + Category + "&productlevel=" + Level + "&tipe=" + TipeLokasi + "&idproduct=" + IDProduct + "&cabang=" + Cabang + "\">";
        x += "  <i class=\"icon-download-alt\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }


    protected string TD_Mutasi(decimal OldPrice, decimal NewPrice)
    {
        string x = string.Empty;
        decimal mutasi = 0m;
        if (OldPrice != 0m)
        {
            mutasi = ((NewPrice - OldPrice) / OldPrice) * 100;
        }

        string sgn = mutasi > 0 ? "+" : "";
        string css = mutasi > 10 || mutasi < -10 ? "text-red" : "";

        x += "<p class=\"" + css + " text-lg\">";
        x += sgn;
        x += Str.Number2(mutasi);
        x += "%";
        x += "</p>";

        return x;
    }

    class PGReval
    {
        public int IDProduct { get; set; }
        public string PLU { get; set; }
        public string ProductItem { get; set; }
        public int LocationType { get; set; }
        public string Location { get; set; }
        public int Substorage { get; set; }
        public decimal Harga { get; set; }
        public decimal HargaReval { get; set; }
        public Nullable<decimal> HargaPromo { get; set; }
        public int StatusPenjualan { get; set; }
    }
}