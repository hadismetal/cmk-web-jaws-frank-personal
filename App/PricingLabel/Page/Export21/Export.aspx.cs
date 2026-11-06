using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

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

    protected int limitdata = 500;

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
            DLC.DDLSupplier(ddl_supplier);
            DLC.CharGoldLevel(ddl_productlevel);

            ddlLocation(ddl_location);
            cb_skip_pwp.Checked = true;

            Fill();
        }

    }

    protected void Fill()
    {
        string xmessage = "";
        string xtipeproduk = "";
        string xproductitem = "";
        string xproductcategory = "";
        string xlocationtipe = "";
        string xlocationid = "";
        string xpludari = "";
        string xplusampai = "";
        string xskippwp = "";

        xmessage = Request.QueryString["msg"];
        xtipeproduk = Request.QueryString["v"];
        xproductitem = Request.QueryString["it"];
        xproductcategory = Request.QueryString["ct"];
        xlocationtipe = Request.QueryString["tp"];
        xlocationid = Request.QueryString["cb"];
        xpludari = Request.QueryString["fr"];
        xplusampai = Request.QueryString["to"];
        xskippwp = Request.QueryString["skip_pwp"];

        if (xmessage == "0") { DataError("PLU tidak ditemukan"); }
        else if (xmessage == limitdata.ToString()) { DataError("Jumlah lebih dari " + limitdata.ToString() + " PLU"); }

        if (xtipeproduk != "") { ddl_tipeproduk.SelectedValue = xtipeproduk; }

        if (xproductitem != "") { ddl_productitem.SelectedValue = xproductitem; }

        if (xproductcategory != "") { ddl_productcategory.SelectedValue = xproductcategory; }

        if (xlocationtipe != "" && xlocationtipe != "9") { ddl_location.SelectedValue = (xlocationtipe + xlocationid); }

        if (xpludari != "") { txt_from.Text = xpludari; }

        if (xplusampai != "") { txt_to.Text = xplusampai; }

        if (App.Refresh(this))
        {
            if (xskippwp == "1") { cb_skip_pwp.Checked = true; } else { cb_skip_pwp.Checked = false; }
        }
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

        tbdj.Visible = false;
        tbpg.Visible = false;
        tbgj.Visible = false;
        tbld.Visible = false;

        Common.RemoveTableData(tbdj);
        Common.RemoveTableData(tbpg);
        Common.RemoveTableData(tbgj);
        Common.RemoveTableData(tbld);

        if (xtipeproduk == "DJ") { SetDJ(xtipeproduk); }
        else if (xtipeproduk == "PG") { SetPG(xtipeproduk); }
        else if (xtipeproduk == "GJ") { SetGJ(xtipeproduk); }
        else if (xtipeproduk == "LD") { SetLD(xtipeproduk); }

    }

    // btnPrint1_Click
    protected void btnPrint1_Click(object sender, EventArgs e)
    {
        err.Visible = false;

        string xtipeproduk = ddl_tipeproduk.SelectedValue;

        if (xtipeproduk == "DJ") { exportDJ(); }
        else if (xtipeproduk == "PG") { exportPG(); }
        else if (xtipeproduk == "GJ") { exportGJ(); }
        else if (xtipeproduk == "LD") { exportLD(); }

        //lit_text_test.Text = dt.ToString();
    }

    protected void exportDJ(string SingleNomorPLU = "")
    {
        err.Visible = false;
        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        if (SingleNomorPLU != "") { From = SingleNomorPLU; To = SingleNomorPLU; }

        if (Cabang == "") { Cabang = "0"; }

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (ddl_productitem.SelectedValue == "0" || ddl_productitem.SelectedValue == "")) { error_msg = "No. PLU / Product Item harus diisi."; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0, 3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = "Format PLU tidak valid."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportDJ.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&tipe=" + TipeLokasi + "&cabang=" + Cabang + "&productcategory=" + Category + "&skip_pwp=" + SkipPWP);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }
    protected void exportGJ(string SingleNomorPLU = "")
    {
        err.Visible = false;
        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        if (SingleNomorPLU != "") { From = SingleNomorPLU; To = SingleNomorPLU; }

        if (Cabang == "") { Cabang = "0"; }

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (ddl_productitem.SelectedValue == "0" || ddl_productitem.SelectedValue == "")) { error_msg = "No. PLU / Product Item harus diisi."; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0, 3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = "Format PLU tidak valid."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportGJ.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&tipe=" + TipeLokasi + "&cabang=" + Cabang + "&productcategory=" + Category + "&skip_pwp=" + SkipPWP);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }
    protected void exportPG(string SingleNomorPLU = "")
    {
        err.Visible = false;
        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        string Level = ddl_productlevel.SelectedValue;
        string Supplier = ddl_supplier.SelectedValue;
        string KodeBarang = txt_kodeBarang.Text;
        string TgpFixRate = ddl_tgp_fixrate.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        if (SingleNomorPLU != "") { From = SingleNomorPLU; To = SingleNomorPLU; }

        if (Cabang == "") { Cabang = "0"; }

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (ddl_productitem.SelectedValue == "0" || ddl_productitem.SelectedValue == "")) { error_msg = ""; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0, 3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = ""; }

            if (error_msg == "")
            {
                Response.Redirect("ExportPG.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&supplier=" + Supplier + "&tipe=" + TipeLokasi + "&cabang=" + Cabang + "&productcategory=" + Category + "&kobar=" + KodeBarang + "&level=" + Level + "&tgpfixrate=" + TgpFixRate + "&skip_pwp=" + SkipPWP);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }
    protected void exportLD(string SingleNomorPLU = "")
    {
        err.Visible = false;
        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        if (SingleNomorPLU != "") { From = SingleNomorPLU; To = SingleNomorPLU; }

        if (Cabang == "") { Cabang = "0"; }

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (ddl_productitem.SelectedValue == "0" || ddl_productitem.SelectedValue == "")) { error_msg = "No. PLU / Product Item harus diisi."; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0, 3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = "Format PLU tidak valid."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportLD.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&tipe=" + TipeLokasi + "&cabang=" + Cabang + "&productcategory=" + Category + "&skip_pwp=" + SkipPWP);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
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

    public static void ddlPromoPG(DropDownList c, DateTime dt)
    {
        c.Items.Clear();
        using (Map_DB db = new Map_DB())
        {
            var rs = from p in db.PromoPG where p.TglStart == dt orderby p.Nama select p;
            foreach (var r in rs)
            {
                string t = r.Nama + " (" + r.PromoPG_Detail.Count() + ")";
                string v = r.ID.ToString();
                c.Items.Add(new ListItem(t, v));
            }
        }
    }

    public static void ddlPromoGJ(DropDownList c, DateTime dt)
    {
        c.Items.Clear();
        using (Map_DB db = new Map_DB())
        {
            var rs = from p in db.PromoGJ where p.TglStart == dt orderby p.Nama select p;
            foreach (var r in rs)
            {
                string t = r.Nama + " (" + r.PromoGJ_Detail.Count() + ")";
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

    /*protected void SetDJ(string TipeProduk)
    {
        tbdj.Visible = true;

        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        int tipe = 0;
        int cabang = 0;
        int ProductItem = 0;
        int ProductCategory = 0;

        try
        {
            tipe = Convert.ToInt32(TipeLokasi);
        }
        catch (Exception ex) { }

        try
        {
            cabang = Convert.ToInt32(Cabang);
        }
        catch (Exception ex) { }
        try
        {
            ProductItem = Convert.ToInt32(Item);
        }
        catch (Exception ex) { }
        try
        {
            ProductCategory = Convert.ToInt32(Category);
        }
        catch (Exception ex) { }
        string xTest = "0";

        //lit_test.Text = ProductItem.ToString() + " - " + ProductCategory.ToString();

        if (To == "" && From != "") { To = From; }
        if (From == "" && To != "") { From = To; }
        if (From == "" || To == "" || From == null || To == null)
        {
            DataError("PLU harus diinput");
        }
        if ((From.Length != To.Length))
        {
            DataError("Format PLU dari & sampai tidak sama.");
        }

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                string YearFrom = "";
                string YearTo = "";
                int Start = 1;
                int End = 1;

                xTest = "1";

                // (Old / Avoris) : with plu, 10 digit
                if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }
                // (Old / Speedware) : with first character letter, 6 digit
                else if ((From.Substring(0, 1) == "A"
                        || From.Substring(0, 1) == "B"
                        || From.Substring(0, 1) == "G"
                        || From.Substring(0, 1) == "P"
                        || From.Substring(0, 1) == "R"
                        || From.Substring(0, 1) == "Q"
                    ) && From.Length == 6)
                {
                    Start = Convert.ToInt32(From.Substring(1, 5));
                    End = Convert.ToInt32(To.Substring(1, 5));
                }
                // (Old / Speedware) : without letter / all number, 6 digit
                else if (From.Length == 6)
                {
                    Start = Convert.ToInt32(From);
                    End = Convert.ToInt32(To);
                }
                // (New / JAWS) : with first letter, 9 digit
                else if (From.Length == 9)
                {
                    YearFrom = From.Substring(1, 2);
                    YearTo = To.Substring(1, 2);
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }

                xTest = "2";

                // Validasi Tahun agar tidak terlalu banyak bandroll
                if (YearFrom != YearTo)
                {
                    DataError("Format tahun dari & sampai tidak sama.");
                }
                else
                {
                    xTest = "3";

                    if (Start > End)
                    {
                        DataError("Format increment dari harus lebih kecil");
                    }
                    else
                    {
                        xTest = "4";

                        var dj = from p in db.StockProductDJ
                                 join q in db.StockActualDJ on p.ID equals q.IDProduct
                                 where (p.Legacy == null || p.Legacy == 0) && p.Draft == false
                                    && p.Nomor.Length == From.Length
                                 select new { p, q };

                        xTest = "4A";

                        // (Old / Avoris) : with plu, 10 digit
                        if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                        {
                            // PLU0023827
                            dj = dj.Where(p => p.p.Nomor.Length == 10);
                            dj = dj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 7)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 7)) <= End);
                            dj = dj.Where(p => p.p.Nomor.Substring(0, 3) == "PLU");
                        }
                        // (Old / Speedware) : with first character letter, 6 digit
                        else if ((From.Substring(0, 1) == "A"
                               || From.Substring(0, 1) == "B"
                               || From.Substring(0, 1) == "G"
                               || From.Substring(0, 1) == "P"
                               || From.Substring(0, 1) == "R"
                               || From.Substring(0, 1) == "Q"
                                ) && From.Length == 6)
                        {
                            xTest = "4A1";

                            //throw new Exception();

                            // P43172
                            dj = dj.Where(p => p.p.Nomor.Length == 6);
                            dj = dj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 5)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 5)) <= End);
                            dj = dj.Where(p => (p.p.Nomor.Substring(0, 1) == "A" // frank
                                              || p.p.Nomor.Substring(0, 1) == "B" // frank
                                              || p.p.Nomor.Substring(0, 1) == "G" // frank
                                              || p.p.Nomor.Substring(0, 1) == "P" // frank, palace, mondial
                                              || p.p.Nomor.Substring(0, 1) == "R" // frank
                                              || p.p.Nomor.Substring(0, 1) == "Q" // mondial
                                             ));

                        }
                        // (Old / Speedware) : without letter / all number, 6 digit
                        else if (From.Length == 6)
                        {
                            xTest = "4A2";

                            // 555698
                            dj = dj.Where(p => p.p.Nomor.Length == 6);

                            dj = dj.Where(p => p.p.Nomor.Equals(Start.ToString())); // harus sama persis

                            dj = dj.Where(p => !p.p.Nomor.Contains("A")); // frank
                            dj = dj.Where(p => !p.p.Nomor.Contains("B")); // frank
                            dj = dj.Where(p => !p.p.Nomor.Contains("G")); // frank
                            dj = dj.Where(p => !p.p.Nomor.Contains("P")); // frank, palace, mondial
                            dj = dj.Where(p => !p.p.Nomor.Contains("R")); // frank
                            dj = dj.Where(p => !p.p.Nomor.Contains("Q")); // mondial

                            //Regex regex = new Regex("^[0-9]+$");

                            //dj = dj.Where(p => !p.p.Nomor.Substring(0, 1).Contains("A"));

                            //dj = dj.Where(p => regex.IsMatch(p.p.Nomor));

                            //dj = dj.Where(p => !p.p.Nomor.StartsWith("A"));

                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Equals("0")
                            //                || p.p.Nomor.Substring(0, 1).Equals("1")
                            //                || p.p.Nomor.Substring(0, 1).Equals("2")
                            //                || p.p.Nomor.Substring(0, 1).Equals("3")
                            //                || p.p.Nomor.Substring(0, 1).Equals("4")
                            //                || p.p.Nomor.Substring(0, 1).Equals("5")
                            //                || p.p.Nomor.Substring(0, 1).Equals("6")
                            //                || p.p.Nomor.Substring(0, 1).Equals("7")
                            //                || p.p.Nomor.Substring(0, 1).Equals("8")
                            //                || p.p.Nomor.Substring(0, 1).Equals("9")
                            //    );



                            //dj = dj.Where(p => p.p.Nomor.StartsWith("5"));
                            //dj = dj.Where(p => p.p.Nomor.StartsWith("6"));
                            //dj = dj.Where(p => p.p.Nomor.StartsWith("7"));
                            //dj = dj.Where(p => p.p.Nomor.StartsWith("8"));
                            //dj = dj.Where(p => p.p.Nomor.StartsWith("9"));

                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("1"));
                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("2"));
                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("3"));
                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("4"));
                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("5"));
                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("6"));
                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("7"));
                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("8"));
                            //dj = dj.Where(p => p.p.Nomor.Substring(0, 1).Contains("9"));

                            //dj = dj.Where(p => Convert.ToInt32("1" + p.p.Nomor.Substring(0, 6)) >= Convert.ToInt32("1" + Start.ToString().PadLeft(6, '0')));
                            //dj = dj.Where(p => Convert.ToInt32("1" + p.p.Nomor.Substring(0, 6)) <= Convert.ToInt32("1" + End.ToString().PadLeft(6, '0')));


                            //lit_test.Text = "";

                            //dj = dj.OrderBy(p => p.p.Nomor);

                            //foreach (var data in dj)
                            //{
                            //    lit_test.Text += data.p.Nomor + "-" + xTest + "<br />";
                            //}

                        }
                        // (New / JAWS) : with first letter, 9 digit
                        else if (From.Length == 9)
                        {
                            // H15002681
                            dj = dj.Where(p => p.p.Nomor.Substring(1, 2) == YearFrom.ToString()); // A17020031
                            dj = dj.Where(p => p.p.Nomor.Length == 9);
                            dj = dj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 6)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 6)) <= End);
                            //dj = dj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                        }

                        xTest = "4B";

                        if (ProductItem > 0)
                        {
                            dj = dj.Where(p => p.p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                        }

                        if (ProductCategory > 0)
                        {
                            dj = dj.Where(p => p.p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                        }

                        xTest = "4C";

                        if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                        {
                            dj = dj.Where(p => p.p.StockActualDJ.TipeLokasi == tipe && p.p.StockActualDJ.IDLokasi == cabang);
                        }

                        xTest = "4D";

                        //if (clarity == 1)
                        //{
                        //    dj = dj.Where(p => p.p.StockProductDJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama == "SI"));
                        //}
                        //else if(clarity == 2)
                        //{
                        //    dj = dj.Where(p => p.p.StockProductDJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama != "SI"));
                        //}

                        xTest = "5";

                        // urut by nomor plu
                        dj = dj.OrderBy(p => p.p.Nomor);

                        xTest = "5A";

                        //var test = dj.ToList();

                        //int temp;
                        //test = test.Where(p => int.TryParse(test["Nomor"], out temp));


                        bool isValid = false;

                        isValid = dj.Any(s => s.p.Nomor.Contains(From) || s.p.Nomor.Contains(To));

                        isValid = true;

                        if (isValid && dj.Count() > 0 && dj.Count() <= limitdata)
                        {

                            div_table.Visible = true;

                            xTest = "6";

                            var x = from p in dj
                                    select new
                                    {
                                        IDProduct = p.p.ID,
                                        PLU = p.p.Nomor,
                                        ProductItem = p.p.StockProductDJ_CharProduct.CharProductItem.Nama,
                                        ProductCategory = p.p.StockProductDJ_CharProduct.CharProductCategory.Nama,
                                        Location = p.q.NamaLokasi,
                                        Harga = p.p.StockProductDJ_PricingMU.HargaRupiahRound
                                    }
                                    ;

                            xTest = "6A";

                            //var w = (from p in db.PromoDJ_Detail
                            //         join q in db.PromoDJ on p.IDForm equals q.ID
                            //         where (from y in dj select y.p.ID).Contains(p.IDProduct.Value)
                            //             && p.PromoDJ.TglExpire >= System.DateTime.Now
                            //         select new { xPromoHarga = q.PromoHarga, xIDProduct = p.IDProduct })
                            //        ;

                            xTest = "6B";

                            //var m = from p in x
                            //        select new
                            //        {
                            //            IDProduct = p.IDProduct,
                            //            PLU = p.PLU,
                            //            ProductItem = p.ProductItem,
                            //            ProductCategory = p.ProductCategory,
                            //            Location = p.Location,
                            //            Harga = p.Harga,
                            //            HargaPromo = (w.FirstOrDefault(q => q.xIDProduct == p.IDProduct).xPromoHarga)
                            //        }
                            //        ;

                            var m = x;

                            xTest = "6C";

                            int count = 1;
                            int countskip = 1;

                            TableRow tr;
                            TableCell td;

                            lit_test.Text = "";

                            string xNomor = "";
                            string xItem = "";
                            string xCategory = "";
                            string xLocation = "";
                            string xHarga = "";
                            string xPromo = "";
                            bool xIsPWP = false;
                            decimal xHargaPWP = 0m;

                            foreach (var rslt in m)
                            {
                                tr = new TableRow();

                                xTest = "7";

                                xNomor = rslt.PLU;
                                xItem = rslt.ProductItem;
                                xCategory = rslt.ProductCategory;
                                xLocation = rslt.Location;
                                xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                                xPromo = "0"; // Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                                xPromo = xPromo == "0" ? "" : xPromo;

                                string querypwp = "DECLARE @ISPWP BIT = 0 " +
                                    "DECLARE @HARGAPWP MONEY = 0 " +
                                    "IF EXISTS (SELECT 1 FROM PromoPWP A JOIN PromoPWP_Detail B ON A.ID = B.IDForm " +
                                    "WHERE B.IDProduct = @IDPRODUCT AND B.TipeProduct = 1 AND A.Status = 1 " +
                                    "AND A.TglStart <= CAST(GETDATE() AS DATE) AND A.TglExpired >= CAST(GETDATE() AS DATE)) " +
                                    "BEGIN " +
                                    "SET @ISPWP = 1 " +
                                    "SET @HARGAPWP = (SELECT TOP 1 ISNULL(B.HargaPWP,0) FROM PromoPWP A JOIN PromoPWP_Detail B ON A.ID = B.IDForm " +
                                    "WHERE B.IDProduct = @IDPRODUCT AND B.TipeProduct = 1 AND A.Status = 1 " +
                                    "AND A.TglStart <= CAST(GETDATE() AS DATE) AND A.TglExpired >= CAST(GETDATE() AS DATE)) " +
                                    "END " +
                                    "SELECT @ISPWP [ISPWP], @HARGAPWP [HARGAPWP] ";
                                List<SqlParameter> parameters = new List<SqlParameter>()
                                {
                                    new SqlParameter("@IDPRODUCT", SqlDbType.Int){ Value = rslt.IDProduct }
                                };
                                DataTable dt = Rs(querypwp, Db.Cnn_DB, parameters, CommandType.Text);
                                xIsPWP = Convert.ToBoolean(dt.Rows[0]["ISPWP"]);
                                xHargaPWP = Convert.ToDecimal(dt.Rows[0]["HARGAPWP"]);

                                xTest = "7A";

                                if (cb_skip_pwp.Checked)
                                {
                                    if (!xIsPWP)
                                    {
                                        if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                                        td = new TableCell();
                                        td.Text = TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), tipe.ToString(), cabang.ToString());
                                        td.Style.Add("text-align", "center");
                                        td.Style.Add("vertical-align", "middle");
                                        tr.Cells.Add(td);

                                        td = new TableCell();
                                        td.Text = "<p>" + countskip.ToString() + ".</p>";
                                        td.Style.Add("text-align", "center");
                                        td.Style.Add("vertical-align", "middle");
                                        tr.Cells.Add(td);

                                        td = new TableCell();
                                        td.Text = xNomor;
                                        td.Style.Add("text-align", "center");
                                        td.Style.Add("vertical-align", "middle");
                                        tr.Cells.Add(td);

                                        td = new TableCell();
                                        td.Text = xItem;
                                        td.Style.Add("text-align", "center");
                                        td.Style.Add("vertical-align", "middle");
                                        tr.Cells.Add(td);

                                        td = new TableCell();
                                        td.Text = xCategory;
                                        td.Style.Add("text-align", "center");
                                        td.Style.Add("vertical-align", "middle");
                                        tr.Cells.Add(td);

                                        td = new TableCell();
                                        td.Text = xLocation;
                                        td.Style.Add("text-align", "left");
                                        td.Style.Add("vertical-align", "middle");
                                        tr.Cells.Add(td);

                                        td = new TableCell();
                                        td.Text = xHarga;
                                        td.Style.Add("text-align", "right");
                                        td.Style.Add("vertical-align", "middle");
                                        td.Style.Add("color", "blue");
                                        tr.Cells.Add(td);

                                        td = new TableCell();
                                        td.Text = xPromo;
                                        td.Style.Add("text-align", "right");
                                        td.Style.Add("vertical-align", "middle");
                                        td.Style.Add("color", "red");
                                        tr.Cells.Add(td);

                                        tbdj.Rows.Add(tr);

                                        countskip++;
                                    }
                                }
                                else
                                {
                                    if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                                    td = new TableCell();
                                    td.Text = TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), tipe.ToString(), cabang.ToString());
                                    td.Style.Add("text-align", "center");
                                    td.Style.Add("vertical-align", "middle");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = "<p>" + count.ToString() + ".</p>";
                                    td.Style.Add("text-align", "center");
                                    td.Style.Add("vertical-align", "middle");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = xNomor;
                                    td.Style.Add("text-align", "center");
                                    td.Style.Add("vertical-align", "middle");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = xItem;
                                    td.Style.Add("text-align", "center");
                                    td.Style.Add("vertical-align", "middle");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = xCategory;
                                    td.Style.Add("text-align", "center");
                                    td.Style.Add("vertical-align", "middle");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = xLocation;
                                    td.Style.Add("text-align", "left");
                                    td.Style.Add("vertical-align", "middle");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = xHarga;
                                    td.Style.Add("text-align", "right");
                                    td.Style.Add("vertical-align", "middle");
                                    td.Style.Add("color", "blue");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = xPromo;
                                    td.Style.Add("text-align", "right");
                                    td.Style.Add("vertical-align", "middle");
                                    td.Style.Add("color", "red");
                                    tr.Cells.Add(td);

                                    tbdj.Rows.Add(tr);
                                }

                                xTest = "8";

                                count++;
                            }

                            //Response.Write(builder.ToString());

                            string groupitem = "";

                            var group_product_item = from p in m
                                                     group p by new { p.ProductItem } into s
                                                     select new
                                                     {
                                                         ProductItem = s.Key.ProductItem,
                                                         Qty = s.Count()
                                                     };

                            foreach (var rsItem in group_product_item)
                            {
                                groupitem += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
                            }

                            SetHeader(m.Count(), groupitem);

                        }
                        else if (isValid && dj.Count() > 0 && dj.Count() > limitdata)
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=" + limitdata.ToString();
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }
                        else
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=0";
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }


                    }
                }
            }


        }
        catch (Exception ex)
        {
            xTest = "Error Code : 0x" + xTest.PadLeft(3, '0')
                  + "<br /><br />Error Message : <span>" + ex.Message.ToString() + "</span>"
                  + "<br /><br />Please contact IT Administrator within the error code number and message."
                  ;

            DataError(xTest);
        }
    }*/

    protected void SetDJ(string TipeProduk)
    {
        tbdj.Visible = true;

        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        int tipe = 0;
        int cabang = 0;
        int ProductItem = 0;
        int ProductCategory = 0;

        try
        {
            tipe = Convert.ToInt32(TipeLokasi);
        }
        catch (Exception ex) { }

        try
        {
            cabang = Convert.ToInt32(Cabang);
        }
        catch (Exception ex) { }
        try
        {
            ProductItem = Convert.ToInt32(Item);
        }
        catch (Exception ex) { }
        try
        {
            ProductCategory = Convert.ToInt32(Category);
        }
        catch (Exception ex) { }

        string query = "sp_export_label_dj_standard";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@FROM", SqlDbType.VarChar){ Value = From },
            new SqlParameter("@TO", SqlDbType.VarChar){ Value = To },
            new SqlParameter("@ITEM", SqlDbType.Int){ Value = Item },
            new SqlParameter("@CATEGORY", SqlDbType.Int){ Value = Category },
            new SqlParameter("@TIPE_LOKASI", SqlDbType.Int){ Value = tipe },
            new SqlParameter("@ID_LOKASI", SqlDbType.Int){ Value = cabang },
            new SqlParameter("@SKIP_PWP", SqlDbType.Bit){ Value = SkipPWP }
        };

        //Response.Write(TipeLokasi + "#" + Cabang + "#" + From + "#" + To + "#" + Item + "#" + Category + "#" + SkipPWP);
        //return;
        DataSet ds = Ds(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        DataTable dt = ds.Tables[0];
        DataTable dtsummary = ds.Tables[1];

        string xTest = "0";

        //lit_test.Text = ProductItem.ToString() + " - " + ProductCategory.ToString();

        if (To == "" && From != "") { To = From; }
        if (From == "" && To != "") { From = To; }
        if (From == "" || To == "" || From == null || To == null)
        {
            DataError("PLU harus diinput");
        }
        if ((From.Length != To.Length))
        {
            DataError("Format PLU dari & sampai tidak sama.");
        }

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                string YearFrom = "";
                string YearTo = "";
                int Start = 1;
                int End = 1;

                xTest = "1";

                // (Old / Avoris) : with plu, 10 digit
                if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }
                // (Old / Speedware) : with first character letter, 6 digit
                else if ((From.Substring(0, 1) == "A"
                        || From.Substring(0, 1) == "B"
                        || From.Substring(0, 1) == "G"
                        || From.Substring(0, 1) == "P"
                        || From.Substring(0, 1) == "R"
                        || From.Substring(0, 1) == "Q"
                    ) && From.Length == 6)
                {
                    Start = Convert.ToInt32(From.Substring(1, 5));
                    End = Convert.ToInt32(To.Substring(1, 5));
                }
                // (Old / Speedware) : without letter / all number, 6 digit
                else if (From.Length == 6)
                {
                    Start = Convert.ToInt32(From);
                    End = Convert.ToInt32(To);
                }
                // (New / JAWS) : with first letter, 9 digit
                else if (From.Length == 9)
                {
                    YearFrom = From.Substring(1, 2);
                    YearTo = To.Substring(1, 2);
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }

                xTest = "2";

                // Validasi Tahun agar tidak terlalu banyak bandroll
                if (YearFrom != YearTo)
                {
                    DataError("Format tahun dari & sampai tidak sama.");
                }
                else
                {
                    xTest = "3";

                    if (Start > End)
                    {
                        DataError("Format increment dari harus lebih kecil");
                    }
                    else
                    {
                        xTest = "4";

                        if (dt.Rows.Count > 0 && dt.Rows.Count <= limitdata)
                        {

                            div_table.Visible = true;

                            TableRow tr;
                            TableCell td;

                            lit_test.Text = "";

                            string xNo = "";
                            string xNomor = "";
                            string xItem = "";
                            string xCategory = "";
                            string xLocation = "";
                            string xHarga = "";
                            string xPromo = "";
                            string xHargaPWP = "";

                            foreach (DataRow dr in dt.Rows)
                            {
                                tr = new TableRow();

                                xNo = Convert.ToInt32(dr["NO"]).ToString("N0");
                                xNomor = dr["PLU"].ToString();
                                xItem = dr["PRODUCT_ITEM"].ToString();
                                xCategory = dr["PRODUCT_CATEGORY"].ToString();
                                xLocation = dr["LOCATION"].ToString();
                                xHarga = Convert.ToDecimal(dr["HARGA"]).ToString("N2");
                                xPromo = Convert.ToDecimal(dr["HARGA_PROMO"]) == 0m ? "" : Convert.ToDecimal(dr["HARGA_PROMO"]).ToString("N2");
                                xHargaPWP = Convert.ToDecimal(dr["HARGAPWP"]) == 0m ? "" : Convert.ToDecimal(dr["HARGAPWP"]).ToString("N2");

                                if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                                td = new TableCell();
                                td.Text = TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), tipe.ToString(), cabang.ToString());
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "<p>" + xNo + ".</p>";
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xNomor;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xItem;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xCategory;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xLocation;
                                td.Style.Add("text-align", "left");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xHarga;
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                td.Style.Add("color", "blue");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xPromo;
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                td.Style.Add("color", "red");
                                tr.Cells.Add(td);

                                tbdj.Rows.Add(tr);
                            }

                            string groupitem = "";

                            foreach (DataRow dr in dtsummary.Rows)
                            {
                                groupitem += dr["PRODUCT_ITEM"].ToString() + " (" + Convert.ToInt32(dr["QTY"]).ToString("0") + ")<br>";
                            }

                            SetHeader(dt.Rows.Count, groupitem);
                        }
                        else if (dt.Rows.Count > 0 && dt.Rows.Count > limitdata)
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=" + limitdata.ToString();
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }
                        else
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=0";
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {

        }
    }

    protected void SetPG(string TipeProduk)
    {
        tbpg.Visible = true;

        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Level = ddl_productlevel.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        string SelectedTgpFixRate = ddl_tgp_fixrate.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        int tipe = 0;
        int cabang = 0;
        int ProductItem = 0;
        int ProductCategory = 0;
        int ProductLevel = 0;
        int TgpFixRate = 0;
        int Supplier = 0;

        try
        {
            tipe = Convert.ToInt32(TipeLokasi);
        }
        catch (Exception ex) { }

        try
        {
            cabang = Convert.ToInt32(Cabang);
        }
        catch (Exception ex) { }
        try
        {
            ProductItem = Convert.ToInt32(Item);
        }
        catch (Exception ex) { }

        try
        {
            ProductCategory = Convert.ToInt32(Category);
        }
        catch (Exception ex) { }

        try
        {
            Supplier = Convert.ToInt32(ddl_supplier.SelectedValue);
        }
        catch (Exception ex) { }

        try
        {
            ProductLevel = Convert.ToInt32(Level);
        }
        catch (Exception ex) { }

        try
        {
            TgpFixRate = Convert.ToInt32(SelectedTgpFixRate);
        }
        catch (Exception ex) { }

        string xTest = "0";

        if (To == "" && From != "") { To = From; }
        if (From == "" && To != "") { From = To; }
        if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1) && (ProductLevel == null || ProductLevel < 1) && (TgpFixRate == null || TgpFixRate < 1))
        {
            DataError("PLU harus di input atau isi salah satu filter berikut: Product Item, Product Level, & TGP / Fix Rate");
            return;
        }
        if ((From.Length != To.Length))
        {
            DataError("Format PLU dari & sampai tidak sama.");
            return;
        }

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                string YearFrom = "";
                string YearTo = "";
                int Start = 1;
                int End = 1;

                xTest = "1";
                if (From.Substring(0, 2).ToUpper() != "ZN" && From.Substring(0, 3).ToUpper() != "VRS" && From.Substring(0, 2).ToUpper() != "GP")
                {
                    // (Old / Avoris) : with plu, 10 digit
                    if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                    {
                        Start = Convert.ToInt32(From.Substring(3, 7));
                        End = Convert.ToInt32(To.Substring(3, 7));
                    }
                    else if (From.StartsWith("TH"))
                    {
                        Start = Convert.ToInt32(From.Substring(2, 8));
                        End = Convert.ToInt32(To.Substring(2, 8));
                    }
                    // (Old / Speedware) : with first character letter, 6 digit
                    else if ((From.Substring(0, 1) == "A"
                            || From.Substring(0, 1) == "B"
                            || From.Substring(0, 1) == "G"
                            || From.Substring(0, 1) == "P"
                            || From.Substring(0, 1) == "R"
                            || From.Substring(0, 1) == "Q"
                        ) && From.Length == 6)
                    {
                        Start = Convert.ToInt32(From.Substring(1, 5));
                        End = Convert.ToInt32(To.Substring(1, 5));
                    }
                    // (Old / Speedware) : without letter / all number, 6 digit
                    else if (From.Length == 6)
                    {
                        Start = Convert.ToInt32(From);
                        End = Convert.ToInt32(To);
                    }
                    // (New / JAWS) : with first letter, 9 digit
                    else if (From.Length == 9)
                    {
                        YearFrom = From.Substring(1, 2);
                        YearTo = To.Substring(1, 2);
                        Start = Convert.ToInt32(From.Substring(3, 6));
                        End = Convert.ToInt32(To.Substring(3, 6));
                    }
                }

                xTest = "2";

                // Validasi Tahun agar tidak terlalu banyak bandroll
                if (YearFrom != YearTo)
                {
                    DataError("Format tahun dari & sampai tidak sama.");
                }
                else
                {
                    xTest = "3";

                    if (Start > End)
                    {
                        DataError("Format increment dari harus lebih kecil");
                    }
                    else
                    {
                        xTest = "4";

                        var pg = from p in db.StockProductPG
                                 join q in db.StockActualPG on p.ID equals q.IDProduct
                                 where (p.Legacy == null || p.Legacy == 0) && p.Draft == false
                                    && p.Nomor.Length == From.Length && p.KodeBarang.Contains(txt_kodeBarang.Text)
                                 select new { p, q };

                        xTest = "4A";
                        if (From.Substring(0, 2).ToUpper() != "ZN" && From.Substring(0, 3).ToUpper() != "VRS" && From.Substring(0, 2).ToUpper() != "GP")
                        {
                            // (Old / Avoris) : with plu, 10 digit
                            if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                            {
                                // PLU0023827
                                pg = pg.Where(p => p.p.Nomor.Length == 10);
                                pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 7)) >= Start);
                                pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 7)) <= End);
                                pg = pg.Where(p => p.p.Nomor.Substring(0, 3) == "PLU");
                            }
                            else if (From.StartsWith("TH"))
                            {
                                pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(2, 8)) >= Start);
                                pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(2, 8)) <= End);
                                pg = pg.Where(p => p.p.Nomor.Substring(0, 2) == "TH");
                            }
                            // (Old / Speedware) : with first character letter, 6 digit
                            else if ((From.Substring(0, 1) == "A"
                                || From.Substring(0, 1) == "B"
                                || From.Substring(0, 1) == "G"
                                || From.Substring(0, 1) == "P"
                                || From.Substring(0, 1) == "R"
                                || From.Substring(0, 1) == "Q"
                                    ) && From.Length == 6)
                            {
                                xTest = "4A1";

                                //throw new Exception();

                                // P43172
                                pg = pg.Where(p => p.p.Nomor.Substring(0, 1) == From.Substring(0, 1));
                                pg = pg.Where(p => p.p.Nomor.Length == 6);
                                pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 5)) >= Start);
                                pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 5)) <= End);
                                pg = pg.Where(p => (p.p.Nomor.Substring(0, 1) == "A" // frank
                                                || p.p.Nomor.Substring(0, 1) == "B" // frank
                                                || p.p.Nomor.Substring(0, 1) == "G" // frank
                                                || p.p.Nomor.Substring(0, 1) == "P" // frank, palace, mondial
                                                || p.p.Nomor.Substring(0, 1) == "R" // frank
                                                || p.p.Nomor.Substring(0, 1) == "Q" // mondial
                                                ));

                            }
                            // (Old / Speedware) : without letter / all number, 6 digit
                            else if (From.Length == 6)
                            {
                                xTest = "4A2";

                                // 555698
                                pg = pg.Where(p => p.p.Nomor.Length == 6);

                                pg = pg.Where(p => !p.p.Nomor.Contains("A")); // frank
                                pg = pg.Where(p => !p.p.Nomor.Contains("B")); // frank
                                pg = pg.Where(p => !p.p.Nomor.Contains("G")); // frank
                                pg = pg.Where(p => !p.p.Nomor.Contains("P")); // frank, palace, mondial
                                pg = pg.Where(p => !p.p.Nomor.Contains("R")); // frank
                                pg = pg.Where(p => !p.p.Nomor.Contains("Q")); // mondial

                                pg = pg.Where(p => Convert.ToInt32("1" + p.p.Nomor.Substring(0, 6)) >= Convert.ToInt32("1" + Start.ToString().PadLeft(6, '0')));
                                pg = pg.Where(p => Convert.ToInt32("1" + p.p.Nomor.Substring(0, 6)) <= Convert.ToInt32("1" + End.ToString().PadLeft(6, '0')));
                            }
                            // (New / JAWS) : with first letter, 9 digit
                            else if (From.Length == 9)
                            {
                                // H15002681
                                pg = pg.Where(p => p.p.Nomor.Substring(1, 2) == YearFrom.ToString()); // A17020031
                                pg = pg.Where(p => p.p.Nomor.Length == 9);
                                pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 6)) >= Start);
                                pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 6)) <= End);
                                //pg = pg.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                            }
                        }
                        else if (From.Substring(0, 2).ToUpper() == "ZN" || From.Substring(0, 3).ToUpper() == "VRS" || From.Substring(0, 2).ToUpper() == "GP")
                        {
                            pg = pg.Where(p => p.p.Nomor.Contains(From));
                        }

                        xTest = "4B";

                        if (ProductItem > 0)
                        {
                            pg = pg.Where(p => p.p.ProductItem == ProductItem);
                        }

                        if (Supplier > 0)
                        {
                            pg = pg.Where(p => p.p.IDSupplier == Supplier);
                        }

                        if (ProductLevel > 0)
                        {
                            pg = pg.Where(p => p.p.ProductLevel == ProductLevel);
                        }

                        if (TgpFixRate > 0)
                        {
                            if (TgpFixRate == 1)
                            {
                                pg = pg.Where(p => p.p.FixRate == true);
                            }
                            else
                            {
                                pg = pg.Where(p => p.p.FixRate == false);
                            }
                        }

                        xTest = "4C";

                        if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                        {
                            pg = pg.Where(p => p.p.StockActualPG.TipeLokasi == tipe && p.p.StockActualPG.IDLokasi == cabang);
                        }

                        xTest = "4D";

                        //if (clarity == 1)
                        //{
                        //    pg = pg.Where(p => p.p.StockProductpg_Stone1A.Any(s => s.Stone1A.Parcel104.Nama == "SI"));
                        //}
                        //else if(clarity == 2)
                        //{
                        //    pg = pg.Where(p => p.p.StockProductpg_Stone1A.Any(s => s.Stone1A.Parcel104.Nama != "SI"));
                        //}

                        xTest = "5";

                        // urut by nomor plu
                        pg = pg.OrderBy(p => p.p.Nomor);

                        xTest = "5A";

                        bool isValid = false;

                        isValid = pg.Any(s => s.p.Nomor.Contains(From) || s.p.Nomor.Contains(To));

                        xTest = "5B";

                        if (isValid && pg.Count() > 0 && pg.Count() <= limitdata)
                        {
                            xTest = "6";

                            var x = from p in pg
                                    select new
                                    {
                                        IDProduct = p.p.ID,
                                        PLU = p.p.Nomor,
                                        ProductItem = p.p.CharProductItem.Nama,
                                        ProductCategory = "PG",
                                        Location = p.q.NamaLokasi,
                                        Harga = p.p.TotalHargaJual.Value,
                                        FixedPrice = (p.p.FixRate.Value == true) ? true : false
                                    }
                                    ;

                            xTest = "6A";

                            var w = (from p in db.PromoPG_Detail
                                     join q in db.PromoPG on p.IDForm equals q.ID
                                     where (from y in pg select y.p.ID).Contains(p.IDProduct.Value)
                                         && p.PromoPG.TglExpire >= System.DateTime.Now
                                     select new { xPromoHarga = q.PromoHarga, xIDProduct = p.IDProduct })
                                    ;

                            xTest = "6B";

                            var m = from p in x
                                    select new
                                    {
                                        IDProduct = p.IDProduct,
                                        PLU = p.PLU,
                                        ProductItem = p.ProductItem,
                                        ProductCategory = p.ProductCategory,
                                        Location = p.Location,
                                        Harga = p.Harga,
                                        HargaPromo = (w.FirstOrDefault(q => q.xIDProduct == p.IDProduct).xPromoHarga),
                                        FixedPrice = p.FixedPrice,
                                    }
                                    ;

                            xTest = "6C";



                            int count = 1;

                            foreach (var rslt in m)
                            {
                                string xNomor = "";
                                string xItem = "";
                                string xCategory = "";
                                string xLocation = "";
                                string xHarga = "";
                                string xPromo = "";
                                string xFixed = "";

                                xNomor = rslt.PLU;
                                xItem = rslt.ProductItem;
                                xCategory = rslt.ProductCategory;
                                xLocation = rslt.Location;
                                xHarga = rslt.FixedPrice ? Convert.ToDecimal(rslt.Harga).ToString("N0") : "";
                                xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");
                                xPromo = xPromo == "0" ? "" : xPromo;

                                xFixed = rslt.FixedPrice ? "Fixed Price" : "Today Gold Price";

                                xTest = "7";

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                                td = new TableCell();
                                td.Text = TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), Supplier.ToString(), txt_kodeBarang.Text, tipe.ToString(), cabang.ToString());
                                td.Style.Add("text-align", "center");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "<p>" + count.ToString() + ".</p>";
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xNomor;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xItem;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xLocation;
                                td.Style.Add("text-align", "left");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xHarga;
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                td.Style.Add("color", "blue");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xFixed;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xPromo;
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                td.Style.Add("color", "red");
                                tr.Cells.Add(td);

                                tbpg.Rows.Add(tr);


                                xTest = "8";

                                xTest = "9";

                                count++;
                            }

                            //Response.Write(builder.ToString());


                            string groupitem = "";

                            var group_product_item = from p in m
                                                     group p by new { p.ProductItem } into s
                                                     select new
                                                     {
                                                         ProductItem = s.Key.ProductItem,
                                                         Qty = s.Count()
                                                     };

                            foreach (var rsItem in group_product_item)
                            {
                                groupitem += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
                            }

                            div_table.Visible = true;
                            SetHeader(m.Count(), groupitem);

                        }
                        else if (isValid && pg.Count() > 0 && pg.Count() > limitdata)
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=" + limitdata;
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }
                        else
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=0";
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }

                    }
                }
            }
            //misal PLU From dan PLU To kosong
            else
            {
                var pg = from p in db.StockProductPG
                         join q in db.StockActualPG on p.ID equals q.IDProduct
                         where (p.Legacy == null || p.Legacy == 0) && p.Draft == false && q.StockProductPG.ReturnProduct == false && q.StockProductPG.StatusLebur == false && !q.StockProductPG.Draft.Value
                            && !(from s in db.SalesOrderPG
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(q.IDProduct)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                 select s.IDProductSouvenir).Contains(q.IDProduct)
                         select new { p, q };

                if (ProductItem > 0)
                {
                    pg = pg.Where(p => p.p.ProductItem == ProductItem);
                }

                if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                {
                    pg = pg.Where(p => p.p.StockActualPG.TipeLokasi == tipe && p.p.StockActualPG.IDLokasi == cabang);
                }
                if (Supplier > 0)
                {
                    pg = pg.Where(p => p.p.IDSupplier == Supplier);
                }

                if (ProductLevel > 0)
                {
                    pg = pg.Where(p => p.p.ProductLevel == ProductLevel);
                }

                if (TgpFixRate > 0)
                {
                    if (TgpFixRate == 1)
                    {
                        pg = pg.Where(p => p.p.FixRate == true);
                    }
                    else
                    {
                        pg = pg.Where(p => p.p.FixRate == false);
                    }
                }

                //if (clarity == 1)
                //{
                //    pg = pg.Where(p => p.p.StockProductpg_Stone1A.Any(s => s.Stone1A.Parcel104.Nama == "SI"));
                //}
                //else if(clarity == 2)
                //{
                //    pg = pg.Where(p => p.p.StockProductpg_Stone1A.Any(s => s.Stone1A.Parcel104.Nama != "SI"));
                //}

                // urut by nomor plu
                pg = pg.OrderBy(p => p.p.Nomor);

                bool isValid = true;

                if (isValid && pg.Count() > 0 /*&& pg.Count() <= limitdata*/)
                {
                    xTest = "6";

                    var x = from p in pg
                            select new
                            {
                                IDProduct = p.p.ID,
                                PLU = p.p.Nomor,
                                ProductItem = p.p.CharProductItem.Nama,
                                ProductCategory = "PG",
                                Location = p.q.NamaLokasi,
                                Harga = p.p.TotalHargaJual.Value,
                                FixedPrice = (p.p.FixRate.Value == true) ? true : false
                            }
                            ;

                    xTest = "6A";

                    var w = (from p in db.PromoPG_Detail
                             join q in db.PromoPG on p.IDForm equals q.ID
                             where (from y in pg select y.p.ID).Contains(p.IDProduct.Value)
                                 && p.PromoPG.TglExpire >= System.DateTime.Now
                             select new { xPromoHarga = q.PromoHarga, xIDProduct = p.IDProduct })
                            ;

                    xTest = "6B";

                    var m = from p in x
                            select new
                            {
                                IDProduct = p.IDProduct,
                                PLU = p.PLU,
                                ProductItem = p.ProductItem,
                                ProductCategory = p.ProductCategory,
                                Location = p.Location,
                                Harga = p.Harga,
                                HargaPromo = (w.FirstOrDefault(q => q.xIDProduct == p.IDProduct).xPromoHarga),
                                FixedPrice = p.FixedPrice,
                            }
                            ;

                    xTest = "6C";



                    int count = 1;

                    foreach (var rslt in m)
                    {
                        string xNomor = "";
                        string xItem = "";
                        string xCategory = "";
                        string xLocation = "";
                        string xHarga = "";
                        string xPromo = "";
                        string xFixed = "";

                        xNomor = rslt.PLU;
                        xItem = rslt.ProductItem;
                        xCategory = rslt.ProductCategory;
                        xLocation = rslt.Location;
                        xHarga = rslt.FixedPrice ? Convert.ToDecimal(rslt.Harga).ToString("N0") : "";
                        xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");
                        xPromo = xPromo == "0" ? "" : xPromo;

                        xFixed = rslt.FixedPrice ? "Fixed Price" : "Today Gold Price";

                        xTest = "7";

                        TableRow tr = new TableRow();
                        TableCell td;

                        if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                        td = new TableCell();
                        td.Text = TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), Supplier.ToString(), txt_kodeBarang.Text, tipe.ToString(), cabang.ToString());
                        td.Style.Add("text-align", "center");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + count.ToString() + ".</p>";
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "middle");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xNomor;
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "middle");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xItem;
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "middle");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xLocation;
                        td.Style.Add("text-align", "left");
                        td.Style.Add("vertical-align", "middle");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xHarga;
                        td.Style.Add("text-align", "right");
                        td.Style.Add("vertical-align", "middle");
                        td.Style.Add("color", "blue");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xFixed;
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "middle");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xPromo;
                        td.Style.Add("text-align", "right");
                        td.Style.Add("vertical-align", "middle");
                        td.Style.Add("color", "red");
                        tr.Cells.Add(td);

                        tbpg.Rows.Add(tr);


                        xTest = "8";

                        xTest = "9";

                        count++;
                    }

                    //Response.Write(builder.ToString());


                    string groupitem = "";

                    var group_product_item = from p in m
                                             group p by new { p.ProductItem } into s
                                             select new
                                             {
                                                 ProductItem = s.Key.ProductItem,
                                                 Qty = s.Count()
                                             };

                    foreach (var rsItem in group_product_item)
                    {
                        groupitem += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
                    }

                    div_table.Visible = true;
                    SetHeader(m.Count(), groupitem);

                }
                /*else if (isValid && pg.Count() > 0 && pg.Count() > limitdata)
                {
                    string xmsg = "";
                    xmsg = "Export.aspx?msg=" + limitdata;
                    xmsg += "&v=" + TipeProduk;
                    xmsg += "&it=" + ProductItem;
                    xmsg += "&ct=" + ProductCategory;
                    xmsg += "&fr=" + From;
                    xmsg += "&to=" + To;
                    xmsg += "&tp=" + tipe;
                    xmsg += "&cb=" + cabang;

                    Response.Redirect(xmsg);
                }*/
                else
                {
                    string xmsg = "";
                    xmsg = "Export.aspx?msg=0";
                    xmsg += "&v=" + TipeProduk;
                    xmsg += "&it=" + ProductItem;
                    xmsg += "&ct=" + ProductCategory;
                    xmsg += "&fr=" + From;
                    xmsg += "&to=" + To;
                    xmsg += "&tp=" + tipe;
                    xmsg += "&cb=" + cabang;
                    xmsg += "&skip_pwp=" + SkipPWP;

                    Response.Redirect(xmsg);
                }
            }

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

    protected void SetGJ(string TipeProduk)
    {
        tbgj.Visible = true;

        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        int tipe = 0;
        int cabang = 0;
        int ProductItem = 0;
        int ProductCategory = 0;

        try
        {
            tipe = Convert.ToInt32(TipeLokasi);
        }
        catch (Exception ex) { }

        try
        {
            cabang = Convert.ToInt32(Cabang);
        }
        catch (Exception ex) { }
        try
        {
            ProductItem = Convert.ToInt32(Item);
        }
        catch (Exception ex) { }
        try
        {
            ProductCategory = Convert.ToInt32(Category);
        }
        catch (Exception ex) { }
        string xTest = "0";

        if (To == "" && From != "") { To = From; }
        if (From == "" && To != "") { From = To; }
        if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1))
        {
            DataError("PLU harus diinput");
        }
        if ((From.Length != To.Length))
        {
            DataError("Format PLU dari & sampai tidak sama.");
        }

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                string YearFrom = "";
                string YearTo = "";
                int Start = 1;
                int End = 1;

                xTest = "1";

                // (Old / Avoris) : with plu, 10 digit
                if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }
                // (Old / Speedware) : with first character letter, 6 digit
                else if ((From.Substring(0, 1) == "A"
                        || From.Substring(0, 1) == "B"
                        || From.Substring(0, 1) == "G"
                        || From.Substring(0, 1) == "P"
                        || From.Substring(0, 1) == "R"
                        || From.Substring(0, 1) == "Q"
                    ) && From.Length == 6)
                {
                    Start = Convert.ToInt32(From.Substring(1, 5));
                    End = Convert.ToInt32(To.Substring(1, 5));
                }
                // (Old / Speedware) : without letter / all number, 6 digit
                else if (From.Length == 6)
                {
                    Start = Convert.ToInt32(From);
                    End = Convert.ToInt32(To);
                }
                // (New / JAWS) : with first letter, 9 digit
                else if (From.Length == 9)
                {
                    YearFrom = From.Substring(1, 2);
                    YearTo = To.Substring(1, 2);
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }

                xTest = "2";

                // Validasi Tahun agar tidak terlalu banyak bandroll
                if (YearFrom != YearTo)
                {
                    DataError("Format tahun dari & sampai tidak sama.");
                }
                else
                {
                    xTest = "3";

                    if (Start > End)
                    {
                        DataError("Format increment dari harus lebih kecil");
                    }
                    else
                    {
                        xTest = "4";

                        var gj = from p in db.StockProductGJ
                                 join q in db.StockActualGJ on p.ID equals q.IDProduct
                                 where p.Draft == false
                                    && p.Nomor.Length == From.Length
                                 select new { p, q };

                        xTest = "4A";

                        // (Old / Avoris) : with plu, 10 digit
                        if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                        {
                            // PLU0023827
                            gj = gj.Where(p => p.p.Nomor.Length == 10);
                            gj = gj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 7)) >= Start);
                            gj = gj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 7)) <= End);
                            gj = gj.Where(p => p.p.Nomor.Substring(0, 3) == "PLU");
                        }
                        // (Old / Speedware) : with first character letter, 6 digit
                        else if ((From.Substring(0, 1) == "A"
                               || From.Substring(0, 1) == "B"
                               || From.Substring(0, 1) == "G"
                               || From.Substring(0, 1) == "P"
                               || From.Substring(0, 1) == "R"
                               || From.Substring(0, 1) == "Q"
                                ) && From.Length == 6)
                        {
                            xTest = "4A1";

                            //throw new Exception();

                            // P43172
                            gj = gj.Where(p => p.p.Nomor.Length == 6);
                            gj = gj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 5)) >= Start);
                            gj = gj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 5)) <= End);
                            gj = gj.Where(p => (p.p.Nomor.Substring(0, 1) == "A" // frank
                                              || p.p.Nomor.Substring(0, 1) == "B" // frank
                                              || p.p.Nomor.Substring(0, 1) == "G" // frank
                                              || p.p.Nomor.Substring(0, 1) == "P" // frank, palace, mondial
                                              || p.p.Nomor.Substring(0, 1) == "R" // frank
                                              || p.p.Nomor.Substring(0, 1) == "Q" // mondial
                                             ));

                        }
                        // (Old / Speedware) : without letter / all number, 6 digit
                        else if (From.Length == 6)
                        {
                            xTest = "4A2";

                            // 555698
                            gj = gj.Where(p => p.p.Nomor.Length == 6);

                            gj = gj.Where(p => !p.p.Nomor.Contains("A")); // frank
                            gj = gj.Where(p => !p.p.Nomor.Contains("B")); // frank
                            gj = gj.Where(p => !p.p.Nomor.Contains("G")); // frank
                            gj = gj.Where(p => !p.p.Nomor.Contains("P")); // frank, palace, mondial
                            gj = gj.Where(p => !p.p.Nomor.Contains("R")); // frank
                            gj = gj.Where(p => !p.p.Nomor.Contains("Q")); // mondial

                            gj = gj.Where(p => Convert.ToInt32("1" + p.p.Nomor.Substring(0, 6)) >= Convert.ToInt32("1" + Start.ToString().PadLeft(6, '0')));
                            gj = gj.Where(p => Convert.ToInt32("1" + p.p.Nomor.Substring(0, 6)) <= Convert.ToInt32("1" + End.ToString().PadLeft(6, '0')));
                        }
                        // (New / JAWS) : with first letter, 9 digit
                        else if (From.Length == 9)
                        {
                            // H15002681
                            gj = gj.Where(p => p.p.Nomor.Substring(1, 2) == YearFrom.ToString()); // A17020031
                            gj = gj.Where(p => p.p.Nomor.Length == 9);
                            gj = gj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 6)) >= Start);
                            gj = gj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 6)) <= End);
                            //gj = gj.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                        }

                        xTest = "4B";

                        if (ProductItem > 0)
                        {
                            gj = gj.Where(p => p.p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                        }

                        if (ProductCategory > 0)
                        {
                            gj = gj.Where(p => p.p.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
                        }

                        xTest = "4C";

                        if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                        {
                            gj = gj.Where(p => p.p.StockActualGJ.TipeLokasi == tipe && p.p.StockActualGJ.IDLokasi == cabang);
                        }

                        xTest = "4D";

                        //if (clarity == 1)
                        //{
                        //    GJ = GJ.Where(p => p.p.StockProductGJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama == "SI"));
                        //}
                        //else if(clarity == 2)
                        //{
                        //    GJ = GJ.Where(p => p.p.StockProductGJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama != "SI"));
                        //}

                        xTest = "5";

                        // urut by nomor plu
                        gj = gj.OrderBy(p => p.p.Nomor);

                        xTest = "5A";

                        xTest = "5B";

                        bool isValid = false;

                        isValid = gj.Any(s => s.p.Nomor.Contains(From) || s.p.Nomor.Contains(To));

                        if (isValid && gj.Count() > 0 && gj.Count() <= limitdata)
                        {
                            var x = from p in gj
                                    select new
                                    {
                                        IDProduct = p.p.ID,
                                        PLU = p.p.Nomor,
                                        ProductItem = p.p.StockProductGJ_CharProduct.CharProductItem.Nama,
                                        ProductCategory = p.p.StockProductGJ_CharProduct.CharProductCategory.Nama,
                                        Location = p.q.NamaLokasi,
                                        Harga = p.p.StockProductGJ_PricingMU.HargaRupiahRound
                                    }
                                    ;

                            var w = (from p in db.PromoGJ_Detail
                                     join q in db.PromoGJ on p.IDForm equals q.ID
                                     where (from y in gj select y.p.ID).Contains(p.IDProduct.Value)
                                         && p.PromoGJ.TglExpire >= System.DateTime.Now
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
                                        Harga = p.Harga,
                                        HargaPromo = (w.FirstOrDefault(q => q.xIDProduct == p.IDProduct).xPromoHarga)
                                    }
                                    ;



                            xTest = "6";

                            int count = 1;

                            foreach (var rslt in m)
                            {
                                string xNomor = "";
                                string xItem = "";
                                string xCategory = "";
                                string xLocation = "";
                                string xHarga = "";
                                string xPromo = "";

                                xNomor = rslt.PLU;
                                xItem = rslt.ProductItem;
                                xCategory = rslt.ProductCategory;
                                xLocation = rslt.Location;
                                xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                                xPromo = Convert.ToDecimal(rslt.HargaPromo).ToString("N0");

                                xPromo = xPromo == "0" ? "" : xPromo;

                                xTest = "7";

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (xPromo != "") { tr.Style.Add("background-color", "#FFEEEE"); }

                                td = new TableCell();
                                td.Text = TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), tipe.ToString(), cabang.ToString());
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "<p>" + count.ToString() + ".</p>";
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xNomor;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xItem;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xCategory;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xLocation;
                                td.Style.Add("text-align", "left");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xHarga;
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                td.Style.Add("color", "blue");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xPromo;
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                td.Style.Add("color", "red");
                                tr.Cells.Add(td);

                                tbgj.Rows.Add(tr);


                                xTest = "8";

                                xTest = "9";

                                count++;
                            }

                            //Response.Write(builder.ToString());


                            string groupitem = "";

                            var group_product_item = from p in m
                                                     group p by new { p.ProductItem } into s
                                                     select new
                                                     {
                                                         ProductItem = s.Key.ProductItem,
                                                         Qty = s.Count()
                                                     };

                            foreach (var rsItem in group_product_item)
                            {
                                groupitem += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
                            }

                            div_table.Visible = true;
                            SetHeader(m.Count(), groupitem);

                        }
                        else if (isValid && gj.Count() > 0 && gj.Count() > limitdata)
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=" + limitdata.ToString();
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }
                        else
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=0";
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }


                    }
                }
            }


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

    protected void SetLD(string TipeProduk)
    {
        tbld.Visible = true;

        string From = txt_from.Text.Trim();
        string To = txt_to.Text.Trim();
        string TipeLokasi = ddl_location.SelectedValue.Substring(0, 1);
        string Cabang = ddl_location.SelectedValue.Substring(1);
        string Item = ddl_productitem.SelectedValue;
        string Category = ddl_productcategory.SelectedValue;
        int SkipPWP = cb_skip_pwp.Checked ? 1 : 0;

        int tipe = 0;
        int cabang = 0;
        int ProductItem = 0;
        int ProductCategory = 0;

        try
        {
            tipe = Convert.ToInt32(TipeLokasi);
        }
        catch (Exception ex) { }

        try
        {
            cabang = Convert.ToInt32(Cabang);
        }
        catch (Exception ex) { }
        try
        {
            ProductItem = Convert.ToInt32(Item);
        }
        catch (Exception ex) { }
        string xTest = "0";

        if (To == "" && From != "") { To = From; }
        if (From == "" && To != "") { From = To; }
        if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1))
        {
            DataError("PLU harus diinput");
        }
        if ((From.Length != To.Length))
        {
            DataError("Format PLU dari & sampai tidak sama.");
        }

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                string YearFrom = "";
                string YearTo = "";
                int Start = 1;
                int End = 1;

                xTest = "1";

                // (Old / Avoris) : with plu, 10 digit
                if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }
                // (Old / Speedware) : with first character letter, 6 digit
                else if ((From.Substring(0, 1) == "A"
                        || From.Substring(0, 1) == "B"
                        || From.Substring(0, 1) == "G"
                        || From.Substring(0, 1) == "P"
                        || From.Substring(0, 1) == "R"
                        || From.Substring(0, 1) == "Q"
                    ) && From.Length == 6)
                {
                    Start = Convert.ToInt32(From.Substring(1, 5));
                    End = Convert.ToInt32(To.Substring(1, 5));
                }
                // (Old / Speedware) : without letter / all number, 6 digit
                else if (From.Length == 6)
                {
                    Start = Convert.ToInt32(From);
                    End = Convert.ToInt32(To);
                }
                // (New / JAWS) : with first letter, 9 digit
                else if (From.Length == 9)
                {
                    YearFrom = From.Substring(1, 2);
                    YearTo = To.Substring(1, 2);
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }

                xTest = "2";

                // Validasi Tahun agar tidak terlalu banyak bandroll
                if (YearFrom != YearTo)
                {
                    DataError("Format tahun dari & sampai tidak sama.");
                }
                else
                {
                    xTest = "3";

                    if (Start > End)
                    {
                        DataError("Format increment dari harus lebih kecil");
                    }
                    else
                    {
                        xTest = "4";

                        var ld = from p in db.StockProductLD_Stone1B
                                 join q in db.StockActualLD_Stone1B on p.ID equals q.IDProduct
                                 where p.Nomor.Length == From.Length
                                 select new { p, q };

                        xTest = "4A";

                        // (Old / Avoris) : with plu, 10 digit
                        if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                        {
                            // PLU0023827
                            ld = ld.Where(p => p.p.Nomor.Length == 10);
                            ld = ld.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 7)) >= Start);
                            ld = ld.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 7)) <= End);
                            ld = ld.Where(p => p.p.Nomor.Substring(0, 3) == "PLU");
                        }
                        // (Old / Speedware) : with first character letter, 6 digit
                        else if ((From.Substring(0, 1) == "A"
                               || From.Substring(0, 1) == "B"
                               || From.Substring(0, 1) == "G"
                               || From.Substring(0, 1) == "P"
                               || From.Substring(0, 1) == "R"
                               || From.Substring(0, 1) == "Q"
                                ) && From.Length == 6)
                        {
                            xTest = "4A1";

                            //throw new Exception();

                            // P43172
                            ld = ld.Where(p => p.p.Nomor.Length == 6);
                            ld = ld.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 5)) >= Start);
                            ld = ld.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 5)) <= End);
                            ld = ld.Where(p => (p.p.Nomor.Substring(0, 1) == "A" // frank
                                              || p.p.Nomor.Substring(0, 1) == "B" // frank
                                              || p.p.Nomor.Substring(0, 1) == "G" // frank
                                              || p.p.Nomor.Substring(0, 1) == "P" // frank, palace, mondial
                                              || p.p.Nomor.Substring(0, 1) == "R" // frank
                                              || p.p.Nomor.Substring(0, 1) == "Q" // mondial
                                             ));

                        }
                        // (Old / Speedware) : without letter / all number, 6 digit
                        else if (From.Length == 6)
                        {
                            xTest = "4A2";

                            // 555698
                            ld = ld.Where(p => p.p.Nomor.Length == 6);

                            ld = ld.Where(p => !p.p.Nomor.Contains("A")); // frank
                            ld = ld.Where(p => !p.p.Nomor.Contains("B")); // frank
                            ld = ld.Where(p => !p.p.Nomor.Contains("G")); // frank
                            ld = ld.Where(p => !p.p.Nomor.Contains("P")); // frank, palace, mondial
                            ld = ld.Where(p => !p.p.Nomor.Contains("R")); // frank
                            ld = ld.Where(p => !p.p.Nomor.Contains("Q")); // mondial

                            ld = ld.Where(p => Convert.ToInt32("1" + p.p.Nomor.Substring(0, 6)) >= Convert.ToInt32("1" + Start.ToString().PadLeft(6, '0')));
                            ld = ld.Where(p => Convert.ToInt32("1" + p.p.Nomor.Substring(0, 6)) <= Convert.ToInt32("1" + End.ToString().PadLeft(6, '0')));
                        }
                        // (New / JAWS) : with first letter, 9 digit
                        else if (From.Length == 9)
                        {
                            // H15002681
                            ld = ld.Where(p => p.p.Nomor.Substring(1, 2) == YearFrom.ToString()); // A17020031
                            ld = ld.Where(p => p.p.Nomor.Length == 9);
                            ld = ld.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 6)) >= Start);
                            ld = ld.Where(p => Convert.ToInt32(p.p.Nomor.Substring(3, 6)) <= End);
                            //ld = ld.Where(p => Convert.ToInt32(p.p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                        }

                        xTest = "4B";

                        //if (ProductItem > 0)
                        //{
                        //    ld = ld.Where(p => p.p.StockProductLD_Sto_CharProduct.ProductItem == ProductItem);
                        //}

                        xTest = "4C";

                        if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                        {
                            ld = ld.Where(p => p.p.StockActualLD_Stone1B.TipeLokasi == tipe && p.p.StockActualLD_Stone1B.IDLokasi == cabang);
                        }

                        xTest = "4D";

                        //if (clarity == 1)
                        //{
                        //    LD = LD.Where(p => p.p.StockProductLD_Stone1A.Any(s => s.Stone1A.Parcel104.Nama == "SI"));
                        //}
                        //else if(clarity == 2)
                        //{
                        //    LD = LD.Where(p => p.p.StockProductLD_Stone1A.Any(s => s.Stone1A.Parcel104.Nama != "SI"));
                        //}

                        xTest = "5";

                        // urut by nomor plu
                        ld = ld.OrderBy(p => p.p.Nomor);

                        xTest = "5A";

                        xTest = "5B";

                        bool isValid = false;

                        isValid = ld.Any(s => s.p.Nomor.Contains(From) || s.p.Nomor.Contains(To));
                        //isValid = true;

                        if (isValid && ld.Count() > 0 && ld.Count() <= limitdata)
                        {
                            var x = from p in ld
                                    select new
                                    {
                                        IDProduct = p.p.ID,
                                        PLU = p.p.Nomor,
                                        ProductItem = "LD",
                                        ProductCategory = "LD",
                                        Location = p.q.NamaLokasi,
                                        Shape = p.p.Stone1B.Parcel101.NamaKode,
                                        Colour = p.p.Stone1B.Parcel103.NamaKode,
                                        Clarity = p.p.Stone1B.Parcel104.NamaKode,
                                        Brand = p.p.Stone1B.Parcel106.NamaKode,
                                        GIA = p.p.GIA,
                                    }
                                    ;



                            xTest = "6";

                            int count = 1;

                            foreach (var rslt in x)
                            {
                                string xNomor = "";
                                string xItem = "";
                                string xCategory = "";
                                string xLocation = "";
                                string xShape = "";
                                string xColour = "";
                                string xClarity = "";
                                string xBrand = "";
                                string xGIA = "";

                                xNomor = rslt.PLU;
                                xItem = rslt.ProductItem;
                                xCategory = rslt.ProductCategory;
                                xLocation = rslt.Location;
                                xShape = rslt.Shape;
                                xColour = rslt.Colour;
                                xClarity = rslt.Clarity;
                                xBrand = rslt.Brand;
                                xGIA = rslt.GIA;

                                xTest = "7";

                                TableRow tr = new TableRow();
                                TableCell td;

                                td = new TableCell();
                                td.Text = TD_HyperLink(TipeProduk, xNomor, xNomor, ProductItem.ToString(), ProductCategory.ToString(), tipe.ToString(), cabang.ToString());
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "<p>" + count.ToString() + ".</p>";
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xNomor;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xItem;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xCategory;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xLocation;
                                td.Style.Add("text-align", "left");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xShape;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xColour;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xClarity;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xBrand;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = xGIA;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                tbld.Rows.Add(tr);


                                xTest = "8";

                                xTest = "9";

                                count++;
                            }

                            //Response.Write(builder.ToString());


                            string groupitem = "";

                            var group_product_item = from p in x
                                                     group p by new { p.ProductItem } into s
                                                     select new
                                                     {
                                                         ProductItem = s.Key.ProductItem,
                                                         Qty = s.Count()
                                                     };

                            foreach (var rsItem in group_product_item)
                            {
                                groupitem += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
                            }

                            div_table.Visible = true;
                            SetHeader(x.Count(), groupitem);

                        }
                        else if (isValid && ld.Count() > 0 && ld.Count() > limitdata)
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=" + limitdata.ToString();
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }
                        else
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=0";
                            xmsg += "&v=" + TipeProduk;
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&ct=" + ProductCategory;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;
                            xmsg += "&skip_pwp=" + SkipPWP;

                            Response.Redirect(xmsg);
                        }


                    }
                }
            }


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

    protected void SetHeader(int qty, string xgroupitem)
    {
        lit_header1.Text = "";
        lit_header2.Text = "";

        string xqty = "";
        string xproducttype = "";
        string xlocation = "";
        string xproductitem = "";
        string xproductcategory = "";
        string xplustart = "";
        string xpluuntil = "";

        if (qty > limitdata)
        {
            if (ddl_tipeproduk.SelectedItem.Value != "PG")
            {
                btnPrint1.Visible = false;
                xqty = "<span style='color:red;'>" + qty.ToString() + "</span>";
            }
            else
            {
                btnPrint1.Visible = true;
                xqty = qty.ToString();
            }
        }
        else
        {
            btnPrint1.Visible = true;
            xqty = qty.ToString();
        }
        xproducttype = ddl_tipeproduk.SelectedItem.Text;
        xlocation = ddl_location.SelectedItem.Text;
        xproductitem = ddl_productitem.SelectedItem.Text;
        xproductcategory = ddl_productcategory.SelectedItem.Text;
        xplustart = txt_from.Text.Trim();
        xpluuntil = txt_to.Text.Trim();

        heading_tipeproduk.Text = xproducttype;

        lit_header1.Text += "<table style='border:none;'>"
                    + "<tr><td style='vertical-align:top;'>Location</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xlocation + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>Product Item</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xproductitem + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>Product Category (DJ/GJ only)</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xproductcategory + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>PLU Start</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xplustart + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>PLU End</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xpluuntil + "</td></tr>"
                    + "</table>";
        lit_header2.Text += "<table style='border:none;'>"
                    + "<tr><td style='vertical-align:top;'>Qty</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xqty + "</td></tr>"
                    + "<tr><td style='vertical-align:top;'>Item Summary</td>" + "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + xgroupitem + "</td></tr>"
                    + "</table>";

    }


    protected string TD_HyperLink(string TipeProduk, string From, string To, string Item, string Category, string TipeLokasi, string Cabang)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info btn-xs\" target=\"_blank\" href=\"../Export21/Export" + TipeProduk + ".aspx?from=" + From + "&to=" + To + "&productcategory=" + Category + "&productitem=" + Item + "&tipe=" + TipeLokasi + "&cabang=" + Cabang + "\">";
        x += "  <i class=\"icon-download-alt\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }

    protected string TD_HyperLink(string TipeProduk, string From, string To, string Item, string Category, string Supplier, string KodeBarang, string TipeLokasi, string Cabang)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info btn-xs\" target=\"_blank\" href=\"../Export21/Export" + TipeProduk + ".aspx?from=" + From + "&to=" + To + "&productcategory=" + Category + "&productitem=" + Item + "&supplier=" + Supplier + "&kobar=" + KodeBarang + "&tipe=" + TipeLokasi + "&cabang=" + Cabang + "\">";
        x += "  <i class=\"icon-download-alt\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }

    //Data Table Builder
    protected DataSet Ds(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataSet rs = new DataSet();
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
}