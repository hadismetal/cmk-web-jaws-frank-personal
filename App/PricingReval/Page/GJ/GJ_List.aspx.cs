using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_GJ : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

    int Reval_M = 0;
    int Reval_R = 0;
    int Reval_G = 0;
    int Reval_SK = 0;
    int Reval_SB = 0;
    int Reval_MF = 0;
    int Reval_MU = 0;
    int Reval_Rate = 0;

    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            //Generate
            fillFilter();

        }
        if (App.Refresh(this)) fillTable(sender, e);

        Reval_M = (reval_M.Checked == true) ? 1 : 0;
        Reval_R = (reval_R.Checked == true) ? 1 : 0;
        Reval_G = (reval_G.Checked == true) ? 1 : 0;
        Reval_MF = (reval_MF.Checked == true) ? 1 : 0;
        Reval_MU = (reval_MU.Checked == true) ? 1 : 0;
        Reval_Rate = (reval_Rate.Checked == true) ? 1 : 0;
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        var rs = from p in db.RevalProductGJ

                 select p;

        rs = rs.OrderBy(p => p.RevalProductGJ_CharProduct.ProductItem);

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductCategory == ProductCategory);

        if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int index = 1;

            foreach (var r in rs)
            {
                /*decimal AllM = 0m;

                var y = r.RevalProductGJ_CostingProduct;
                var z = r.RevalProductGJ_Costing;

                AllM = (y.Total + z.TotalBiaya);*/

                var x = db.StockProductGJ.SingleOrDefault(p => p.Nomor == "" + r.Nomor + "");

                TableRow tr = new TableRow();
                TableCell td;

                /*CheckBox cb = new CheckBox();

                cb.ID = "cb_po" + index.ToString();
                cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                HiddenField hf = new HiddenField();
                hf.ID = "hf" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(cb);
                td.Controls.Add(hf);
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";



                td = new TableCell();
                td.Text = "<small>" + KodeKaret + "</small>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalProductGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(x.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0") + " / " + Convert.ToDecimal(r.RevalProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Mutasi(Convert.ToDecimal(x.StockProductGJ_PricingMU.HargaRupiahRound), Convert.ToDecimal(r.RevalProductGJ_PricingMU.HargaRupiahRound));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(x.StockProductGJ_PricingMU.ModalUSD).ToString("N2") + " / " + Convert.ToDecimal(r.RevalProductGJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(x.StockProductGJ_PricingMU.HargaUSD).ToString("N2") + " / " + Convert.ToDecimal(r.RevalProductGJ_PricingMU.HargaUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalProductGJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalProductGJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalProductGJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalProductGJ_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;

            }


        }


    }

    protected string TD_Mutasi(decimal OldPrice, decimal NewPrice)
    {
        string x = string.Empty;
        decimal mutasi = 0m;

        mutasi = ((NewPrice - OldPrice) / OldPrice) * 100;

        string sgn = mutasi > 0 ? "+" : "";
        string css = mutasi > 10 || mutasi < -10 ? "text-red" : "";

        x += "<p class=\"" + css + " text-lg\">";
        x += sgn;
        x += Str.Number2(mutasi);
        x += "%";
        x += "</p>";

        return x;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            reval_M.Checked = (Request.QueryString["reval_m"] == "True") ? true : false;
            reval_R.Checked = (Request.QueryString["reval_r"] == "True") ? true : false;
            reval_G.Checked = (Request.QueryString["reval_g"] == "True") ? true : false;
            reval_MF.Checked = (Request.QueryString["reval_mf"] == "True") ? true : false;
            reval_MU.Checked = (Request.QueryString["reval_mu"] == "True") ? true : false;
            reval_Rate.Checked = (Request.QueryString["reval_rate"] == "True") ? true : false;
            

        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;

        Response.Redirect(href);
    }
    protected void Execute(object sender, EventArgs e)
    {
        var rs = from p in db.RevalProductGJ
                 select p;

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductCategory == ProductCategory);

        if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            foreach (var d in rs)
            {
                var product = d.RevalProductGJ_CharProduct;

                saveRevalFinishing(d, product);

                saveRevalProduct(d, product);
                saveRevalBiaya(d, product);

                saveRevalMU(d, product);
                saveRevalSegmen(d, product);
                saveRevalTotalStone(d);
            }
        }

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&reval_m=" + reval_M.Checked.ToString();
        href += "&reval_r=" + reval_R.Checked.ToString();
        href += "&reval_g=" + reval_G.Checked.ToString();
        href += "&reval_mf=" + reval_MF.Checked.ToString();
        href += "&reval_mu=" + reval_MU.Checked.ToString();
        href += "&reval_rate=" + reval_Rate.Checked.ToString();

        Response.Redirect(href);


    }

    //Finishing
    protected void saveRevalFinishing(RevalProductGJ d, RevalProductGJ_CharProduct product)
    {
        var rs = from p in d.RevalProductGJ_Finishing select p;
        foreach (var r in rs)
        {
            string sebab = "";
            bool ok = true;

            //Pricing
            var prc_ff = db.PricingTableFF.SingleOrDefault(p =>
                p.IDItem == product.ProductItem &&
                p.IDFinishing == r.IDFinishing &&
                p.Biaya.HasValue &&
                p.ApprovalTgl.HasValue
                );

            //Pricing - Validasi
            if (prc_ff == null) { ok = false; sebab = "Finishing Fee"; }

            //Process
            if (!ok) { }
            else
            {
                if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                {
                    r.Biaya = prc_ff.Biaya.Value;
                    r.Efektif = prc_ff.ApprovalTgl.Value;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }

    //Product
    protected void saveRevalProduct(RevalProductGJ d, RevalProductGJ_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_GJ_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_GJ_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_tg == null || prc_tgM == null) { ok = false; sebab = "Today Gold Price"; }

        //Process
        if (!ok) { }
        else
        {
            //RevalProductGJ_PricingProduct n = new RevalProductGJ_PricingProduct();


            var n = db.RevalProductGJ_PricingProduct.SingleOrDefault(p => p.ID == d.ID);

            //n.ID = d.ID;

            //if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
            //{
                //Gold Price
                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_G == 1)
                {
                    n.GoldRate = prc_tg.Harga.Value;
                    n.EfektifGold = prc_tg.ApprovalTgl.Value;
                    db.SubmitChanges();
                }

                //Informasi Berat
                n.Netto = product.NettoWeight;
                n.Kadar = product.KadarTukaran;

                //Total
                n.TotalProduct = n.GoldRate * n.Netto * (n.Kadar / (decimal)100);
                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SK == 1)
                    n.TotalStone1A = Convert.ToDecimal(TotalRevalStone1A(d));
                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
                    n.TotalStone1B = Convert.ToDecimal(TotalRevalStone1B(d));
                n.TotalStone2 = Convert.ToDecimal(TotalRevalStone2(d));
                n.TotalStone3 = Convert.ToDecimal(TotalRevalStone3(d));
                n.TotalStone4 = Convert.ToDecimal(TotalRevalStone4(d));
                n.TotalStone5 = Convert.ToDecimal(TotalRevalStone5(d));
                n.Total = n.TotalProduct + Convert.ToDecimal(n.TotalStone1A) + Convert.ToDecimal(n.TotalStone1B) + Convert.ToDecimal(n.TotalStone2) + Convert.ToDecimal(n.TotalStone3) + Convert.ToDecimal(n.TotalStone4) + Convert.ToDecimal(n.TotalStone5);

                //Commit
                //db.RevalProductGJ_PricingProduct.InsertOnSubmit(n);
                db.SubmitChanges();
            //}

            //Costing
            //RevalProductGJ_CostingProduct m = new RevalProductGJ_CostingProduct();

            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
            {

                var m = db.RevalProductGJ_CostingProduct.SingleOrDefault(p => p.ID == d.ID);

                //m.ID = d.ID;

                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_G == 1)
                {
                    //Gold Price
                    m.GoldRate = prc_tgM.Harga.Value;
                    m.EfektifGold = prc_tgM.ApprovalTgl.Value;
                    db.SubmitChanges();
                }

                //Informasi Berat
                m.Netto = product.NettoWeight;
                m.Kadar = product.KadarTukaran;

                //Total
                m.TotalProduct = m.GoldRate * n.Netto * (n.Kadar / (decimal)100);
                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SK == 1)
                    m.TotalStone1A = Convert.ToDecimal(TotalMRevalStone1A(d));
                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
                {
                    m.TotalStone1B = Convert.ToDecimal(TotalMRevalStone1B(d));
                }
                m.TotalStone2 = Convert.ToDecimal(TotalMRevalStone2(d));
                m.TotalStone3 = Convert.ToDecimal(TotalMRevalStone3(d));
                m.TotalStone4 = Convert.ToDecimal(TotalMRevalStone4(d));
                m.TotalStone5 = Convert.ToDecimal(TotalMRevalStone5(d));
                m.Total = m.TotalProduct + Convert.ToDecimal(m.TotalStone1A) + Convert.ToDecimal(m.TotalStone1B) + Convert.ToDecimal(m.TotalStone2) + Convert.ToDecimal(m.TotalStone3) + Convert.ToDecimal(m.TotalStone4) + Convert.ToDecimal(m.TotalStone5);

                //Commit
                //db.RevalProductGJ_CostingProduct.InsertOnSubmit(m);
                db.SubmitChanges();
            }
        }
    }

    //Biaya
    protected void saveRevalBiaya(RevalProductGJ d, RevalProductGJ_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
            p.IDBrand == d.LocWarehouse.IDBrand &&
            p.IDItem == product.ProductItem &&
            p.IDCons == product.ProcessCons &&
            p.Biaya.HasValue &&
            p.Pembagi.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_fs = db.PricingTableFS.SingleOrDefault(p =>
            p.ID == FS_ID(d, product) &&
            p.Biaya.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_fm == null) { ok = false; sebab = "Mounting Fee"; }
        if (prc_fs == null) { ok = false; sebab = "Setting Fee"; }
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (!ok) { }
        else
        {
            //RevalProductGJ_PricingBiaya n = new RevalProductGJ_PricingBiaya();

            //if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
            //{
            var n = db.RevalProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == d.ID);

            //n.ID = d.ID;

            //Informasi
            n.NamaBrand = d.LocWarehouse.CompanyBrand.Nama;
            n.NamaItem = product.CharProductItem.Nama;
            n.NamaCons = product.CharProcessCons.Nama;

            //Mounting
            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
            {
                n.Mounting = prc_fm.Biaya.Value;
                n.MountingNO = prc_fm.InputNO.Value;
                n.MountingRO = prc_fm.InputRO.Value;
                n.EfektifMounting = prc_fm.ApprovalTgl.Value;

                //Setting
                //n.Setting = prc_fs.Biaya.Value;
                /*Sementara di Hardcode*/

                n.Setting = Setting;
                n.EfektifSetting = prc_fs.ApprovalTgl.Value;
                db.SubmitChanges();
            }

            /*end hardcode*/



            //Finishing
            n.Finishing = TotalRevalFinishing(d);

            //Total
            n.TotalRupiah = n.Mounting + n.Setting + n.Finishing;

            n.Total = n.TotalRupiah / n.TotalRate;

            //Tgl. Efektif

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_Rate == 1)
            {
                n.TotalRate = prc_us.Harga.Value;
                n.EfektifRate = prc_us.ApprovalTgl.Value;
                db.SubmitChanges();
            }

            //Commit
            //db.RevalProductGJ_PricingBiaya.InsertOnSubmit(n);
            db.SubmitChanges();
            //}
        }
    }
    protected int FS_ID(RevalProductGJ d, RevalProductGJ_CharProduct product)
    {
        return 1; //Masih di-hardcoded. BELUM SELESAI !
    }

    //Mark-Up
    protected void saveRevalMU(RevalProductGJ d, RevalProductGJ_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
            p.IDCategory == product.ProductCategory &&
            p.IDLevel == product.ProductLevel &&
            p.IDDist == product.StoneDist &&
            p.Persen.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_mu == null) { ok = false; sebab = "Diamond Jewelry MU"; }
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (!ok) { }
        else
        {
            //RevalProductGJ_PricingMU n = new RevalProductGJ_PricingMU();


            var n = db.RevalProductGJ_PricingMU.SingleOrDefault(p => p.ID == d.ID);

            //n.ID = d.ID;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
            {
                //Modal
                n.ModalUSD = TotalRevalModal(d.ID);

                n.ModalRupiah = n.ModalUSD * n.ModalRate;
                db.SubmitChanges();
            }

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_Rate == 1)
            {
                n.ModalRate = prc_us.Harga.Value;
                db.SubmitChanges();
            }

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MU == 1)
            {
                //Mark-Up
                n.MU = prc_mu.Persen.Value;
                db.SubmitChanges();
            }

            decimal OngkosLain = 0m;

            try
            {
                OngkosLain = d.RevalProductGJ_Costing.OngkosLain.Value;
            }
            catch (Exception ex)
            {
                OngkosLain = 0m;
            }

            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
            {
                //Harga Final
                n.HargaUSD = TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID) + (OngkosLain / d.RevalProductGJ_Costing.TotalRate.Value);
                n.HargaRupiah = (TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID)) * n.ModalRate + OngkosLain;
                n.HargaRupiahRound = price.RoundTop((TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID)) * n.ModalRate + OngkosLain);

                //Tgl. Efektif
                n.EfektifRate = prc_us.ApprovalTgl.Value;
            }

            //Commit
            //db.RevalProductGJ_PricingMU.InsertOnSubmit(n);
            db.SubmitChanges();

        }
    }

    //Segmen System
    protected void saveRevalSegmen(RevalProductGJ d, RevalProductGJ_CharProduct product)
    {
        string sebab = "Segmen System";
        bool ok = true;

        //Pricing - Validasi
        ok = checkRevalSegmen(product);

        //Process
        if (!ok) { }
        else
        {
            //Product Value
            decimal harga = HargaRevalUSD(d.ID);
            //decimal carat = product.StoneCarat;

            decimal carat = 0m;

            //Parameter
            int IDSegmen = 0;
            int SegmenSistem = 0;
            decimal SegmenMinimum = 0;
            DateTime EfektifSegmen = DateTime.Now;

            //SELECT
            var rs = from p in db.PricingTableSS
                     where
                         p.IDCategory == product.ProductCategory &&
                         p.IDLevel == product.ProductLevel &&
                         p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                     orderby p.Minimum
                     select p;

            foreach (var r in rs)
            {
                if ((r.SegmenSistem == 0 && harga >= r.Minimum.Value) ||
                    (r.SegmenSistem == 1 && carat >= r.Minimum.Value))
                {

                    IDSegmen = r.IDSegmen;
                    SegmenSistem = r.SegmenSistem;
                    SegmenMinimum = r.Minimum.Value;
                    EfektifSegmen = r.ApprovalTgl.Value;
                }
            }

            var n = db.RevalProductGJ_PricingSegmen.SingleOrDefault(p => p.ID == d.ID);

            //RevalProductGJ_PricingSegmen n = new RevalProductGJ_PricingSegmen();

            //n.ID = d.ID;
            /*n.ProductSegmen = IDSegmen;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            //Segmen
            n.SegmenSistem = SegmenSistem;
            n.SegmenMinimum = SegmenMinimum;

            //Harga dan Carat
            n.HargaUSD = harga;
            n.TotalCarat = carat;

            //Tgl. Efektif
            n.EfektifSegmen = EfektifSegmen;

            ///Commit
            //db.RevalProductGJ_PricingSegmen.InsertOnSubmit(n);*/
            db.SubmitChanges();
        }
    }
    protected bool checkRevalSegmen(RevalProductGJ_CharProduct product)
    {
        bool ok = true;

        var rs = from p in db.PricingTableSS
                 where
                     p.IDCategory == product.ProductCategory &&
                     p.IDLevel == product.ProductLevel &&
                     p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                 select p;

        foreach (var r in rs)
        {
            if (!r.Minimum.HasValue || !r.ApprovalTgl.HasValue)
            {
                ok = false;
                break;
            }
        }

        return ok;
    }

    //Sub-Total
    protected decimal TotalRevalStone(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    //Sub-Total
    protected decimal TotalRevalStone1A(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalRevalStone1B(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalRevalStone2(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalRevalStone3(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalRevalStone4(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalRevalStone5(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMRevalStone1A(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMRevalStone1B(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMRevalStone2(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMRevalStone3(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMRevalStone4(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMRevalStone5(RevalProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalRevalFinishing(RevalProductGJ d)
    {
        decimal total = 0;

        var rs = from p in d.RevalProductGJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }
    protected decimal TotalRevalModal(int ID)
    {
        decimal total = 0;

        var product = db.RevalProductGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.RevalProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
        total += biaya.Total;

        return total;
    }
    protected decimal TotalRevalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.RevalProductGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.RevalProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalRevalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.RevalProductGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.RevalProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
    protected decimal HargaRevalUSD(int ID)
    {
        var mu = db.RevalProductGJ_PricingMU.SingleOrDefault(p => p.ID == ID);
        return mu.HargaUSD;
    }
    protected void saveRevalTotalStone(RevalProductGJ d)
    {
        decimal total = 0;

        d.RevalProductGJ_CharProduct.StoneQty = total;
        db.SubmitChanges();

    }

    protected void Apply(object sender, EventArgs e)
    {
        var rs = from p in db.RevalProductGJ
                 select p;

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalProductGJ_CharProduct.ProductCategory == ProductCategory);

        if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.RevalProductGJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            foreach (var r in rs)
            {
                var d = db.StockProductGJ.SingleOrDefault(p => p.Nomor == "" + r.Nomor + "");

                var product = d.StockProductGJ_CharProduct;

                int ProdID = insertProduct(d);

                saveFinishing(d, product, ProdID);

                saveProduct(d, product, ProdID);
                saveBiaya(d, product, ProdID);

                saveMU(d, product, ProdID);
                saveSegmen(d, product, ProdID);
                saveTotalStone(d, ProdID);
                deleteProcess(r);
            }
        }

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&reval_m=" + reval_M.Checked.ToString();
        href += "&reval_r=" + reval_R.Checked.ToString();
        href += "&reval_g=" + reval_G.Checked.ToString();
        href += "&reval_mf=" + reval_MF.Checked.ToString();
        href += "&reval_mu=" + reval_MU.Checked.ToString();
        href += "&reval_rate=" + reval_Rate.Checked.ToString();

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "ExportGJ.aspx?t=expport";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&reval_m=" + reval_M.Checked.ToString();
        href += "&reval_r=" + reval_R.Checked.ToString();
        href += "&reval_g=" + reval_G.Checked.ToString();
        href += "&reval_mf=" + reval_MF.Checked.ToString();
        href += "&reval_mu=" + reval_MU.Checked.ToString();
        href += "&reval_rate=" + reval_Rate.Checked.ToString();

        Response.Write("<script>window.open('" + href + "');</script>");
    }

    //Apply Reval
    //insert product log
    protected int insertProduct(StockProductGJ d)
    {
        RevalLogProductGJ prod = new RevalLogProductGJ();

        prod.Nomor = d.Nomor;

        //prod.Operator = d.Operator;
        prod.Operator = App.UserNama;
        prod.OperatorTgl = DateTime.Now;
        //prod.Approval = d.Approval;
        prod.Approval = App.UserNama;
        prod.ApprovalTgl = DateTime.Now;
        prod.ImgPicture = d.ImgPicture;

        db.RevalLogProductGJ.InsertOnSubmit(prod);
        db.SubmitChanges();

        int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM RevalLogProductGJ ORDER BY ID DESC", Db.Cnn_DB);

        RevalLogProductGJ_CharProduct pd = new RevalLogProductGJ_CharProduct();
        pd.ID = ProdID;
        pd.ProductItem = d.StockProductGJ_CharProduct.ProductItem;
        pd.ProductCategory = d.StockProductGJ_CharProduct.ProductCategory;
        pd.ProductLevel = d.StockProductGJ_CharProduct.ProductLevel;
        pd.StoneDist = d.StockProductGJ_CharProduct.StoneDist;
        pd.FrameColor = d.StockProductGJ_CharProduct.FrameColor;
        pd.FrameFinishing = d.StockProductGJ_CharProduct.FrameFinishing;
        pd.FrameMaterial = d.StockProductGJ_CharProduct.FrameMaterial;
        pd.ProcessCons = d.StockProductGJ_CharProduct.ProcessCons;
        pd.GrossWeight = d.StockProductGJ_CharProduct.GrossWeight;
        pd.StoneCarat = d.StockProductGJ_CharProduct.StoneCarat;
        pd.StoneWeight = d.StockProductGJ_CharProduct.StoneWeight;
        pd.NettoWeight = d.StockProductGJ_CharProduct.NettoWeight;
        pd.KadarLogam = d.StockProductGJ_CharProduct.KadarLogam;
        pd.KadarTukaran = d.StockProductGJ_CharProduct.KadarTukaran;
        pd.DimensiD = d.StockProductGJ_CharProduct.DimensiD;
        pd.DimensiL = d.StockProductGJ_CharProduct.DimensiL;
        pd.DimensiP = d.StockProductGJ_CharProduct.DimensiP;
        pd.DimensiR = d.StockProductGJ_CharProduct.DimensiR;
        pd.StoneQty = d.StockProductGJ_CharProduct.StoneQty;

        db.RevalLogProductGJ_CharProduct.InsertOnSubmit(pd);
        db.SubmitChanges();

        RevalLogProductGJ_Costing cost = new RevalLogProductGJ_Costing();
        cost.ID = ProdID;
        cost.UnitPrice = d.StockProductGJ_Costing.UnitPrice;
        cost.Frame = d.StockProductGJ_Costing.Frame;
        cost.MountingNO = d.StockProductGJ_Costing.MountingNO;
        cost.MountingRO = d.StockProductGJ_Costing.MountingRO;
        cost.Pembagi = d.StockProductGJ_Costing.Pembagi.Value;
        cost.Setting = d.StockProductGJ_Costing.Setting;
        cost.Finishing = d.StockProductGJ_Costing.Finishing;
        cost.OngkosLainCOGS = d.StockProductGJ_Costing.OngkosLainCOGS;
        cost.OngkosLain = d.StockProductGJ_Costing.OngkosLain;
        cost.TotalRate = d.StockProductGJ_Costing.TotalRate.Value;
        cost.TotalRupiah = d.StockProductGJ_Costing.TotalRupiah.Value;
        cost.TotalBiaya = d.StockProductGJ_Costing.TotalBiaya;

        db.RevalLogProductGJ_Costing.InsertOnSubmit(cost);
        db.SubmitChanges();

        RevalLogProductGJ_CostingProduct cp = new RevalLogProductGJ_CostingProduct();
        cp.ID = ProdID;
        cp.GoldRate = d.StockProductGJ_CostingProduct.GoldRate;
        cp.Netto = d.StockProductGJ_CostingProduct.Netto;
        cp.Kadar = d.StockProductGJ_CostingProduct.Kadar;
        cp.TotalProduct = d.StockProductGJ_CostingProduct.TotalProduct;
        cp.TotalStone1A = d.StockProductGJ_CostingProduct.TotalStone1A;
        cp.TotalStone1B = d.StockProductGJ_CostingProduct.TotalStone1B;
        cp.TotalStone2 = d.StockProductGJ_CostingProduct.TotalStone2;
        cp.TotalStone3 = d.StockProductGJ_CostingProduct.TotalStone3;
        cp.TotalStone4 = d.StockProductGJ_CostingProduct.TotalStone4;
        cp.TotalStone5 = d.StockProductGJ_CostingProduct.TotalStone5;
        cp.Total = d.StockProductGJ_CostingProduct.Total;
        cp.EfektifGold = d.StockProductGJ_CostingProduct.EfektifGold;

        db.RevalLogProductGJ_CostingProduct.InsertOnSubmit(cp);
        db.SubmitChanges();

        var l_fns = from p in db.StockProductGJ_Finishing
                    where p.IDForm == d.ID
                    select p;
        if (l_fns != null)
        {
            foreach (var r in l_fns)
            {
                RevalLogProductGJ_Finishing fns = new RevalLogProductGJ_Finishing();
                fns.IDForm = ProdID;
                fns.IDFinishing = r.IDFinishing;
                fns.Biaya = r.Biaya;
                fns.BiayaModal = r.BiayaModal;
                fns.Efektif = r.Efektif;
                db.RevalLogProductGJ_Finishing.InsertOnSubmit(fns);
                db.SubmitChanges();
            }
        }

        RevalLogProductGJ_PricingBiaya by = new RevalLogProductGJ_PricingBiaya();
        by.ID = ProdID;
        by.NamaBrand = d.StockProductGJ_PricingBiaya.NamaBrand;
        by.NamaItem = d.StockProductGJ_PricingBiaya.NamaItem;
        by.NamaCons = d.StockProductGJ_PricingBiaya.NamaCons;
        by.Mounting = d.StockProductGJ_PricingBiaya.Mounting;
        by.MountingNO = d.StockProductGJ_PricingBiaya.MountingNO;
        by.MountingRO = d.StockProductGJ_PricingBiaya.MountingRO;
        by.Setting = d.StockProductGJ_PricingBiaya.Setting;
        by.Finishing = d.StockProductGJ_PricingBiaya.Finishing;
        by.TotalRupiah = d.StockProductGJ_PricingBiaya.TotalRupiah;
        by.TotalRate = d.StockProductGJ_PricingBiaya.TotalRate;
        by.Total = d.StockProductGJ_PricingBiaya.Total;
        by.EfektifRate = d.StockProductGJ_PricingBiaya.EfektifRate;
        by.EfektifMounting = d.StockProductGJ_PricingBiaya.EfektifMounting;
        by.EfektifSetting = d.StockProductGJ_PricingBiaya.EfektifSetting;

        db.RevalLogProductGJ_PricingBiaya.InsertOnSubmit(by);
        db.SubmitChanges();

        RevalLogProductGJ_PricingMU mu = new RevalLogProductGJ_PricingMU();
        mu.ID = ProdID;
        mu.NamaCategory = d.StockProductGJ_PricingMU.NamaCategory;
        mu.NamaLevel = d.StockProductGJ_PricingMU.NamaLevel;
        mu.NamaDist = d.StockProductGJ_PricingMU.NamaDist;
        mu.ModalUSD = d.StockProductGJ_PricingMU.ModalUSD;
        mu.ModalRate = d.StockProductGJ_PricingMU.ModalRate;
        mu.ModalRupiah = d.StockProductGJ_PricingMU.ModalRupiah;
        mu.MU = d.StockProductGJ_PricingMU.MU;
        mu.HargaUSD = d.StockProductGJ_PricingMU.HargaUSD;
        mu.HargaRupiah = d.StockProductGJ_PricingMU.HargaRupiah;
        mu.HargaRupiahRound = d.StockProductGJ_PricingMU.HargaRupiahRound;
        mu.EfektifRate = d.StockProductGJ_PricingMU.EfektifRate;

        db.RevalLogProductGJ_PricingMU.InsertOnSubmit(mu);
        db.SubmitChanges();

        RevalLogProductGJ_PricingProduct prc = new RevalLogProductGJ_PricingProduct();
        prc.ID = ProdID;
        prc.GoldRate = d.StockProductGJ_PricingProduct.GoldRate;
        prc.Netto = d.StockProductGJ_PricingProduct.Netto;
        prc.Kadar = d.StockProductGJ_PricingProduct.Kadar;
        prc.TotalProduct = d.StockProductGJ_PricingProduct.TotalProduct;
        prc.TotalStone1A = d.StockProductGJ_PricingProduct.TotalStone1A;
        prc.TotalStone1B = d.StockProductGJ_PricingProduct.TotalStone1B;
        prc.TotalStone2 = d.StockProductGJ_PricingProduct.TotalStone2;
        prc.TotalStone3 = d.StockProductGJ_PricingProduct.TotalStone3;
        prc.TotalStone4 = d.StockProductGJ_PricingProduct.TotalStone4;
        prc.TotalStone5 = d.StockProductGJ_PricingProduct.TotalStone5;
        prc.Total = d.StockProductGJ_PricingProduct.Total;
        prc.EfektifGold = d.StockProductGJ_PricingProduct.EfektifGold;

        db.RevalLogProductGJ_PricingProduct.InsertOnSubmit(prc);
        db.SubmitChanges();

        RevalLogProductGJ_PricingSegmen sg = new RevalLogProductGJ_PricingSegmen();
        sg.ID = ProdID;
        sg.ProductSegmen = d.StockProductGJ_PricingSegmen.ProductSegmen;
        sg.NamaCategory = d.StockProductGJ_PricingSegmen.NamaCategory;
        sg.NamaLevel = d.StockProductGJ_PricingSegmen.NamaLevel;
        sg.NamaDist = d.StockProductGJ_PricingSegmen.NamaDist;
        sg.SegmenSistem = d.StockProductGJ_PricingSegmen.SegmenSistem;
        sg.SegmenMinimum = d.StockProductGJ_PricingSegmen.SegmenMinimum;
        sg.TotalCarat = d.StockProductGJ_PricingSegmen.TotalCarat;
        sg.HargaUSD = d.StockProductGJ_PricingSegmen.HargaUSD;
        sg.EfektifSegmen = d.StockProductGJ_PricingSegmen.EfektifSegmen;

        db.RevalLogProductGJ_PricingSegmen.InsertOnSubmit(sg);
        db.SubmitChanges();

        return ProdID;
    }

    protected void deleteProcess(RevalProductGJ d)
    {
        if (d.RevalProductGJ_CharDesign != null)
            db.RevalProductGJ_CharDesign.DeleteOnSubmit(
                d.RevalProductGJ_CharDesign);

        if (d.RevalProductGJ_CharProduct != null)
            db.RevalProductGJ_CharProduct.DeleteOnSubmit(
                d.RevalProductGJ_CharProduct);

        if (d.RevalProductGJ_Costing != null)
            db.RevalProductGJ_Costing.DeleteOnSubmit(
                d.RevalProductGJ_Costing);

        if (d.RevalProductGJ_CostingProduct != null)
            db.RevalProductGJ_CostingProduct.DeleteOnSubmit(
                d.RevalProductGJ_CostingProduct);

        var fn = db.RevalProductGJ_Finishing.Where(p => p.IDForm == d.ID);
        if (fn != null)
        {
            db.RevalProductGJ_Finishing.DeleteAllOnSubmit(fn);
            db.SubmitChanges();
        }

        if (d.RevalProductGJ_PricingBiaya != null)
            db.RevalProductGJ_PricingBiaya.DeleteOnSubmit(
                d.RevalProductGJ_PricingBiaya);

        if (d.RevalProductGJ_PricingMU != null)
            db.RevalProductGJ_PricingMU.DeleteOnSubmit(
                d.RevalProductGJ_PricingMU);

        if (d.RevalProductGJ_PricingProduct != null)
            db.RevalProductGJ_PricingProduct.DeleteOnSubmit(
                d.RevalProductGJ_PricingProduct);

        if (d.RevalProductGJ_PricingSegmen != null)
            db.RevalProductGJ_PricingSegmen.DeleteOnSubmit(
                d.RevalProductGJ_PricingSegmen);

        if (d != null)
            db.RevalProductGJ.DeleteOnSubmit(d);

        if (d.RevalItemGJ != null)
            db.RevalItemGJ.DeleteOnSubmit(d.RevalItemGJ);

        //Commit
        db.SubmitChanges();
    }

    //Finishing
    protected void saveFinishing(StockProductGJ d, StockProductGJ_CharProduct product, int ProdID)
    {
        var rs = from p in d.StockProductGJ_Finishing select p;
        foreach (var r in rs)
        {
            string sebab = "";
            bool ok = true;

            //Pricing
            var prc_ff = db.PricingTableFF.SingleOrDefault(p =>
                p.IDItem == product.ProductItem &&
                p.IDFinishing == r.IDFinishing &&
                p.Biaya.HasValue &&
                p.ApprovalTgl.HasValue
                );

            //Pricing - Validasi
            if (prc_ff == null) { ok = false; sebab = "Finishing Fee"; }

            //Process
            if (!ok) { }
            else
            {
                var fn = db.RevalLogProductGJ_Finishing.SingleOrDefault(p => p.IDForm == ProdID && p.IDFinishing == r.IDFinishing);

                if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                {
                    fn.BiayaOld = r.Biaya;
                    fn.Biaya = prc_ff.Biaya.Value;
                    fn.EfektifOld = r.Efektif;
                    fn.Efektif = prc_ff.ApprovalTgl.Value;

                    r.Biaya = prc_ff.Biaya.Value;
                    r.Efektif = prc_ff.ApprovalTgl.Value;
                }
                //Commit
                db.SubmitChanges();
            }
        }
    }

    //Product
    protected void saveProduct(StockProductGJ d, StockProductGJ_CharProduct product, int ProdID)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_GJ_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_GJ_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_tg == null || prc_tgM == null) { ok = false; sebab = "Today Gold Price"; }

        //Process
        if (!ok) { }
        else
        {
            var pr = db.RevalLogProductGJ_PricingProduct.SingleOrDefault(p => p.ID == ProdID);

            //StockProductGJ_PricingProduct n = new StockProductGJ_PricingProduct();

            var n = db.StockProductGJ_PricingProduct.SingleOrDefault(p => p.ID == d.ID);

            //if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
            //{
                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_G == 1)
                {
                    pr.GoldRateOld = n.GoldRate;
                    pr.GoldRate = prc_tg.Harga.Value;
                    pr.EfektifGoldOld = n.EfektifGold;
                    pr.EfektifGold = prc_tg.ApprovalTgl.Value;
                    db.SubmitChanges();
                }

                pr.TotalProductOld = n.TotalProduct;
                pr.TotalProduct = prc_tg.Harga.Value * n.Netto * (n.Kadar / (decimal)100);
                pr.TotalStone1AOld = n.TotalStone1A;
                pr.TotalStone1A = Convert.ToDecimal(TotalStone1A(d));
                pr.TotalStone1BOld = n.TotalStone1B;
                pr.TotalStone1B = Convert.ToDecimal(TotalStone1B(d));
                pr.TotalStone2Old = n.TotalStone2;
                pr.TotalStone2 = Convert.ToDecimal(TotalStone2(d));
                pr.TotalStone3Old = n.TotalStone3;
                pr.TotalStone3 = Convert.ToDecimal(TotalStone3(d));
                pr.TotalStone4Old = n.TotalStone4;
                pr.TotalStone4 = Convert.ToDecimal(TotalStone4(d));
                pr.TotalStone5Old = n.TotalStone5;
                pr.TotalStone5 = Convert.ToDecimal(TotalStone5(d));
                pr.TotalOld = n.Total;
                pr.Total = (n.GoldRate * n.Netto * (n.Kadar / (decimal)100)) + Convert.ToDecimal(TotalStone1B(d)) + Convert.ToDecimal(TotalStone2(d)) + Convert.ToDecimal(TotalStone3(d)) + Convert.ToDecimal(TotalStone4(d)) + Convert.ToDecimal(TotalStone5(d));

                db.SubmitChanges();

                //n.ID = d.ID;

                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_G == 1)
                {
                    //Gold Price
                    n.GoldRate = prc_tg.Harga.Value;
                    n.EfektifGold = prc_tg.ApprovalTgl.Value;
                    db.SubmitChanges();
                }

                //Informasi Berat
                n.Netto = product.NettoWeight;
                n.Kadar = product.KadarTukaran;

                //Total
                n.TotalProduct = n.GoldRate * n.Netto * (n.Kadar / (decimal)100);
                n.TotalStone1A = Convert.ToDecimal(TotalStone1A(d));
                n.TotalStone1B = Convert.ToDecimal(TotalStone1B(d));
                n.TotalStone2 = Convert.ToDecimal(TotalStone2(d));
                n.TotalStone3 = Convert.ToDecimal(TotalStone3(d));
                n.TotalStone4 = Convert.ToDecimal(TotalStone4(d));
                n.TotalStone5 = Convert.ToDecimal(TotalStone5(d));
                n.Total = n.TotalProduct + Convert.ToDecimal(n.TotalStone1A) + Convert.ToDecimal(n.TotalStone1B) + Convert.ToDecimal(n.TotalStone2) + Convert.ToDecimal(n.TotalStone3) + Convert.ToDecimal(n.TotalStone4) + Convert.ToDecimal(n.TotalStone5);

                //Commit
                //db.StockProductGJ_PricingProduct.InsertOnSubmit(n);
                db.SubmitChanges();
            //}

            //Costing
            //StockProductGJ_CostingProduct m = new StockProductGJ_CostingProduct();

            var cs = db.RevalLogProductGJ_CostingProduct.SingleOrDefault(p => p.ID == ProdID);

            var m = db.StockProductGJ_CostingProduct.SingleOrDefault(p => p.ID == d.ID);

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_M == 1)
            {
                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_G == 1)
                {
                    cs.GoldRateOld = m.GoldRate;
                    cs.GoldRate = prc_tgM.Harga.Value;
                    cs.EfektifGoldOld = m.EfektifGold;
                    cs.EfektifGold = prc_tgM.ApprovalTgl.Value;
                    db.SubmitChanges();
                }
                cs.TotalProductOld = m.TotalProduct;
                cs.TotalProduct = prc_tgM.Harga.Value * n.Netto * (n.Kadar / (decimal)100);
                cs.TotalStone1AOld = m.TotalStone1A;
                cs.TotalStone1A = Convert.ToDecimal(TotalMStone1A(d));
                cs.TotalStone1BOld = m.TotalStone1B;
                cs.TotalStone1B = Convert.ToDecimal(TotalMStone1B(d));
                cs.TotalStone2Old = m.TotalStone2;
                cs.TotalStone2 = Convert.ToDecimal(TotalMStone2(d));
                cs.TotalStone3Old = m.TotalStone3;
                cs.TotalStone3 = Convert.ToDecimal(TotalMStone3(d));
                cs.TotalStone4Old = m.TotalStone4;
                cs.TotalStone4 = Convert.ToDecimal(TotalMStone4(d));
                cs.TotalStone5Old = m.TotalStone5;
                cs.TotalStone5 = Convert.ToDecimal(TotalMStone5(d));
                cs.TotalOld = m.Total;
                cs.Total = (prc_tgM.Harga.Value * n.Netto * (n.Kadar / (decimal)100)) + Convert.ToDecimal(TotalMStone1A(d)) + Convert.ToDecimal(TotalMStone1B(d)) + Convert.ToDecimal(TotalMStone2(d)) + Convert.ToDecimal(TotalMStone3(d)) + Convert.ToDecimal(TotalMStone4(d)) + Convert.ToDecimal(TotalMStone5(d));

                //m.ID = d.ID;

                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_G == 1)
                {
                    //Gold Price
                    m.GoldRate = prc_tgM.Harga.Value;
                    m.EfektifGold = prc_tgM.ApprovalTgl.Value;
                    db.SubmitChanges();
                }

                //Informasi Berat
                m.Netto = product.NettoWeight;
                m.Kadar = product.KadarTukaran;

                //Total
                m.TotalProduct = m.GoldRate * n.Netto * (n.Kadar / (decimal)100);
                m.TotalStone1A = Convert.ToDecimal(TotalMStone1A(d));
                m.TotalStone1B = Convert.ToDecimal(TotalMStone1B(d));
                m.TotalStone2 = Convert.ToDecimal(TotalMStone2(d));
                m.TotalStone3 = Convert.ToDecimal(TotalMStone3(d));
                m.TotalStone4 = Convert.ToDecimal(TotalMStone4(d));
                m.TotalStone5 = Convert.ToDecimal(TotalMStone5(d));
                m.Total = m.TotalProduct + Convert.ToDecimal(m.TotalStone1A) + Convert.ToDecimal(m.TotalStone1B) + Convert.ToDecimal(m.TotalStone2) + Convert.ToDecimal(m.TotalStone3) + Convert.ToDecimal(m.TotalStone4) + Convert.ToDecimal(m.TotalStone5);

                //Commit
                //db.StockProductGJ_CostingProduct.InsertOnSubmit(m);
                db.SubmitChanges();
            }
        }
    }

    //Biaya
    protected void saveBiaya(StockProductGJ d, StockProductGJ_CharProduct product, int ProdID)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
            p.IDBrand == d.LocWarehouse.IDBrand &&
            p.IDItem == product.ProductItem &&
            p.IDCons == product.ProcessCons &&
            p.Biaya.HasValue &&
            p.Pembagi.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_fs = db.PricingTableFS.SingleOrDefault(p =>
            p.ID == FS_ID(d, product) &&
            p.Biaya.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_fm == null) { ok = false; sebab = "Mounting Fee"; }
        if (prc_fs == null) { ok = false; sebab = "Setting Fee"; }
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (!ok) { }
        else
        {
            //StockProductGJ_PricingBiaya n = new StockProductGJ_PricingBiaya();

            var by = db.RevalLogProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == ProdID);

            var n = db.StockProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == d.ID);

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
            {
                by.MountingOld = n.Mounting;
                by.Mounting = prc_fm.Biaya.Value;
                by.MountingNOOld = n.MountingNO;
                by.MountingNO = prc_fm.InputNO.Value;
                by.MountingROOld = n.MountingRO;
                by.MountingRO = prc_fm.InputRO.Value;
                by.SettingOld = n.Setting;
                by.Setting = Setting;
                db.SubmitChanges();
            }
            by.FinishingOld = n.Finishing;
            by.Finishing = TotalFinishing(d);
            by.TotalRateOld = n.TotalRate;
            by.TotalRate = prc_us.Harga.Value;
            by.TotalRupiahOld = n.TotalRupiah;
            by.TotalRupiah = prc_fm.Biaya.Value + Setting + TotalFinishing(d);
            by.EfektifMountingOld = n.EfektifMounting;
            by.EfektifMounting = prc_fm.ApprovalTgl.Value;
            by.EfektifSettingOld = n.EfektifSetting;
            by.EfektifSetting = prc_fs.ApprovalTgl.Value;
            by.EfektifRateOld = n.EfektifRate;
            by.EfektifRate = prc_us.ApprovalTgl.Value;

            //n.ID = d.ID;

            //Informasi
            n.NamaBrand = d.LocWarehouse.CompanyBrand.Nama;
            n.NamaItem = product.CharProductItem.Nama;
            n.NamaCons = product.CharProcessCons.Nama;

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
            {
                //Mounting
                n.Mounting = prc_fm.Biaya.Value;
                n.MountingNO = prc_fm.InputNO.Value;
                n.MountingRO = prc_fm.InputRO.Value;


                //Setting
                //n.Setting = prc_fs.Biaya.Value;
                /*Sementara di Hardcode*/
                n.Setting = Setting;

                n.EfektifMounting = prc_fm.ApprovalTgl.Value;
                n.EfektifSetting = prc_fs.ApprovalTgl.Value;
                db.SubmitChanges();
            }

            /*end hardcode*/

            //Finishing
            n.Finishing = TotalFinishing(d);

            //Total
            n.TotalRupiah = n.Mounting + n.Setting + n.Finishing;
            
            n.Total = n.TotalRupiah / n.TotalRate;

            //Tgl. Efektif

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_Rate == 1)
            {
                n.TotalRate = prc_us.Harga.Value;
                n.EfektifRate = prc_us.ApprovalTgl.Value;
                db.SubmitChanges();
            }

            //Commit
            //db.StockProductGJ_PricingBiaya.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected int FS_ID(StockProductGJ d, StockProductGJ_CharProduct product)
    {
        return 1; //Masih di-hardcoded. BELUM SELESAI !
    }

    //Mark-Up
    protected void saveMU(StockProductGJ d, StockProductGJ_CharProduct product, int ProdID)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
            p.IDCategory == product.ProductCategory &&
            p.IDLevel == product.ProductLevel &&
            p.IDDist == product.StoneDist &&
            p.Persen.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_mu == null) { ok = false; sebab = "Diamond Jewelry MU"; }
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (!ok) { }
        else
        {
            //StockProductGJ_PricingMU n = new StockProductGJ_PricingMU();

            var n = db.StockProductGJ_PricingMU.SingleOrDefault(p => p.ID == d.ID);

            var mu = db.RevalLogProductGJ_PricingMU.SingleOrDefault(p => p.ID == ProdID);


            decimal OngkosLain = 0m;

            try
            {
                OngkosLain = d.StockProductGJ_Costing.OngkosLain.Value;
            }
            catch (Exception ex)
            {
                OngkosLain = 0m;
            }

            if ((Reval_M == 0 && Reval_R == 0)  || Reval_M == 1)
            {
                mu.ModalUSDOld = n.ModalUSD;
                mu.ModalUSD = TotalModal(d.ID);
                mu.ModalRupiahOld = n.ModalRupiah;
                mu.ModalRupiah = TotalModal(d.ID) * prc_us.Harga.Value;
                if (Reval_M == 1)
                {
                    mu.HargaUSDOld = n.HargaUSD;
                    mu.HargaRupiahOld = n.HargaRupiah;
                    mu.HargaRupiahRoundOld = n.HargaRupiahRound;
                }

                db.SubmitChanges();
            }
            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_Rate == 1)
            {
				mu.ModalUSDOld = n.ModalUSD;
                mu.ModalUSD = mu.ModalUSD;
				mu.HargaUSDOld = n.HargaUSD;
                mu.HargaUSD = mu.HargaUSD;
                mu.ModalRateOld = n.ModalRate;
                mu.ModalRate = prc_us.Harga.Value;
                mu.EfektifRateOld = n.EfektifRate;
                mu.EfektifRate = prc_us.ApprovalTgl.Value;
				mu.ModalRupiahOld = n.ModalRupiah;
				mu.ModalRupiah = n.ModalUSD * prc_us.Harga.Value;
				mu.HargaRupiahOld = n.HargaRupiah;
                mu.HargaRupiah = mu.HargaUSD * prc_us.Harga.Value;
                mu.HargaRupiahRoundOld = n.HargaRupiahRound;
                mu.HargaRupiahRound = price.RoundTop(mu.HargaRupiah);
                db.SubmitChanges();
            }
            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MU == 1)
            {
				//Response.Redirect(prc_mu.Persen.Value.ToString());
				//Response.End();
                mu.MUOld = n.MU;
                mu.MU = prc_mu.Persen.Value;
				db.SubmitChanges();
            }

            if ((Reval_M == 0 && Reval_R == 0)  || Reval_R == 1)
            {
				//Response.Redirect(mu.MU.ToString());
				//Response.End();
				//Response.Redirect((price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID)) * n.ModalRate + OngkosLain)).ToString());
				//Response.End();
                mu.HargaUSDOld = n.HargaUSD;
                mu.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (mu.MU / (decimal)100)) + TotalStone1B(d.ID) + (OngkosLain / d.StockProductGJ_Costing.TotalRate.Value);
                mu.HargaRupiahOld = n.HargaRupiah;
                mu.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (mu.MU / (decimal)100)) + TotalStone1B(d.ID)) * mu.ModalRate + OngkosLain;
                mu.HargaRupiahRoundOld = n.HargaRupiahRound;
                mu.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (mu.MU / (decimal)100)) + TotalStone1B(d.ID)) * mu.ModalRate + OngkosLain);
                if (Reval_R == 1)
                {
                    mu.ModalUSDOld = n.ModalUSD;
                    mu.ModalRupiahOld = n.ModalRupiah;
                }
                db.SubmitChanges();
            }
            

            //n.ID = d.ID;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
            {
                //Modal
                n.ModalUSD = TotalModal(d.ID);
                
                n.ModalRupiah = n.ModalUSD * n.ModalRate;
				
                db.SubmitChanges();
            }
            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_Rate == 1)
            {
                n.ModalRate = prc_us.Harga.Value;
				n.ModalRupiah = n.ModalUSD * n.ModalRate;
				n.HargaRupiah = n.HargaUSD * n.ModalRate;
                n.HargaRupiahRound = price.RoundTop(n.HargaRupiah);
                //Tgl. Efektif
                n.EfektifRate = prc_us.ApprovalTgl.Value;
                db.SubmitChanges();
            }

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MU == 1)
            {
                //Mark-Up
             
                n.MU = prc_mu.Persen.Value;
                db.SubmitChanges();
            }
            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
            {
                //Harga Final
                n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + (OngkosLain / d.StockProductGJ_Costing.TotalRate.Value);
                n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID)) * n.ModalRate + OngkosLain;
                n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID)) * n.ModalRate + OngkosLain);
                db.SubmitChanges();
            }

            

            //Commit
            //db.StockProductGJ_PricingMU.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }

    //Segmen System
    protected void saveSegmen(StockProductGJ d, StockProductGJ_CharProduct product, int ProdID)
    {
        string sebab = "Segmen System";
        bool ok = true;

        //Pricing - Validasi
        ok = checkSegmen(product);

        //Process
        if (!ok) { }
        else
        {
            //Product Value
            decimal harga = HargaUSD(d.ID);
            //decimal carat = product.StoneCarat;

            decimal carat = 0m;

            //Parameter
            int IDSegmen = 0;
            int SegmenSistem = 0;
            decimal SegmenMinimum = 0;
            DateTime EfektifSegmen = DateTime.Now;

            //SELECT
            var rs = from p in db.PricingTableSS
                     where
                         p.IDCategory == product.ProductCategory &&
                         p.IDLevel == product.ProductLevel &&
                         p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                     orderby p.Minimum
                     select p;

            foreach (var r in rs)
            {
                if ((r.SegmenSistem == 0 && harga >= r.Minimum.Value) ||
                    (r.SegmenSistem == 1 && carat >= r.Minimum.Value))
                {

                    IDSegmen = r.IDSegmen;
                    SegmenSistem = r.SegmenSistem;
                    SegmenMinimum = r.Minimum.Value;
                    EfektifSegmen = r.ApprovalTgl.Value;
                }
            }

            var n = db.StockProductGJ_PricingSegmen.SingleOrDefault(p => p.ID == d.ID);

            var sg = db.RevalLogProductGJ_PricingSegmen.SingleOrDefault(p => p.ID == ProdID);

            //StockProductGJ_PricingSegmen n = new StockProductGJ_PricingSegmen();

            //n.ID = d.ID;

            sg.SegmenSistemOld = n.SegmenSistem;
            sg.SegmenSistem = SegmenSistem;
            sg.SegmenMinimumOld = n.SegmenMinimum;
            sg.SegmenMinimum = SegmenMinimum;
            sg.HargaUSDOld = n.HargaUSD;
            sg.HargaUSD = harga;
            sg.EfektifSegmenOld = n.EfektifSegmen;
            sg.EfektifSegmen = EfektifSegmen;

            n.ProductSegmen = IDSegmen;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            //Segmen
            n.SegmenSistem = SegmenSistem;
            n.SegmenMinimum = SegmenMinimum;

            //Harga dan Carat
            n.HargaUSD = harga;
            n.TotalCarat = carat;

            //Tgl. Efektif
            n.EfektifSegmen = EfektifSegmen;

            ///Commit
            //db.StockProductGJ_PricingSegmen.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected bool checkSegmen(StockProductGJ_CharProduct product)
    {
        bool ok = true;

        var rs = from p in db.PricingTableSS
                 where
                     p.IDCategory == product.ProductCategory &&
                     p.IDLevel == product.ProductLevel &&
                     p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                 select p;

        foreach (var r in rs)
        {
            if (!r.Minimum.HasValue || !r.ApprovalTgl.HasValue)
            {
                ok = false;
                break;
            }
        }

        return ok;
    }

    //Sub-Total
    protected decimal TotalStone(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    //Sub-Total
    protected decimal TotalStone1A(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone1B(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone2(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone3(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone4(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone5(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMStone1A(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMStone1B(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMStone2(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMStone3(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMStone4(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalMStone5(StockProductGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalFinishing(StockProductGJ d)
    {
        decimal total = 0;

        var rs = from p in d.StockProductGJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }
    protected decimal TotalModal(int ID)
    {
        decimal total = 0;

        var product = db.StockProductGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
        total += biaya.Total;

        return total;
    }
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.StockProductGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.StockProductGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
    protected decimal HargaUSD(int ID)
    {
        var mu = db.StockProductGJ_PricingMU.SingleOrDefault(p => p.ID == ID);
        return mu.HargaUSD;
    }
    protected void saveTotalStone(StockProductGJ d, int ProdID)
    {
        decimal total = 0;

        d.StockProductGJ_CharProduct.StoneQty = total;
        db.SubmitChanges();

    }
}