using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Data.SqlClient;

public partial class Page_DJ_List_v2 : System.Web.UI.Page
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
    protected int act { get { return App.QueryInt(this, "act"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);



            int Tipe = 0;
            if (Request.QueryString["tipelokasi"] != "All" && Request.QueryString["tipelokasi"] != "-1")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            DLC.LocationList(lokasi, Tipe);

            //Generate
            fillFilter();

            string error = HttpUtility.UrlDecode(Request.QueryString["error"]);
            if (!String.IsNullOrEmpty(error))
            {
                err.Text = "<span class=\"text-danger\">PLU Error : " + error + "</span>";
            }
            //if (error != "")


        }
        if (App.Refresh(this)) fillTable(sender, e);
        //if (act == 1) Apply(sender, e);

        Reval_M = (reval_M.Checked == true) ? 1 : 0;
        Reval_R = (reval_R.Checked == true) ? 1 : 0;
        Reval_G = (reval_G.Checked == true) ? 1 : 0;
        Reval_SK = (reval_SK.Checked == true) ? 1 : 0;
        Reval_SB = (reval_SB.Checked == true) ? 1 : 0;
        Reval_MF = (reval_MF.Checked == true) ? 1 : 0;
        Reval_MU = (reval_MU.Checked == true) ? 1 : 0;
        Reval_Rate = (reval_Rate.Checked == true) ? 1 : 0;
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        /*var rs = from p in db.RevalProductDJ

                 select p;

        rs = rs.OrderBy(p => p.RevalProductDJ_CharProduct.ProductItem);

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductCategory == ProductCategory);*/

        DataTable dt = new DataTable();

        try
        {
            string spname = "sp_getExecuteReval ";
            string query = " exec " + spname
                         + " " + tipelokasi.SelectedValue + ", "
                         + lokasi.SelectedValue + ", "
                         + productitem.SelectedValue + ", "
                         + productcategory.SelectedValue + ", "
                         + productlevel.SelectedValue + ", "
                         + parcel104a.SelectedValue + ", "
                         + orderby.SelectedValue + ", '"
                         + kw.Text + "'";

            dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
        }

        if (dt.Rows.Count != 0)
        {
            int index = 1;

            foreach (DataRow r in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                CheckBox cb = new CheckBox();

                cb.ID = "cb_po" + index.ToString();
                cb.Style.Add("display", "block");
                cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                HiddenField hf = new HiddenField();
                hf.ID = "hf" + index.ToString();
                hf.Value = r["ID"].ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Style.Add("text-align", "center");
                td.Controls.Add(cb);
                td.Controls.Add(hf);

                tr.Cells.Add(td);

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
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["Nomor"].ToString();
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = r["ProtoNomor"].ToString();
                td = new TableCell();
                td.Text = KodeKaret;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["ProductItem"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["HargaRupiahRound"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["HargaRupiahRound_Reval"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Mutasi(Convert.ToDecimal(r["HargaRupiahRound"]), Convert.ToDecimal(r["HargaRupiahRound_Reval"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["M_Awal"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["M"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["M_Reval"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["R"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["R_Reval"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["NettoWeight"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["GrossWeight"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["TotalButir"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["TotalCarat"]).ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["Segment"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["StoneDist"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["ProductLevel"].ToString();
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
        if (OldPrice == 0) mutasi = ((NewPrice - OldPrice) / 1) * 100;
        else mutasi = ((NewPrice - OldPrice) / OldPrice) * 100;

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
            reval_SK.Checked = (Request.QueryString["reval_sk"] == "True") ? true : false;
            reval_SB.Checked = (Request.QueryString["reval_sb"] == "True") ? true : false;
            reval_MF.Checked = (Request.QueryString["reval_mf"] == "True") ? true : false;
            reval_MU.Checked = (Request.QueryString["reval_mu"] == "True") ? true : false;
            reval_Rate.Checked = (Request.QueryString["reval_rate"] == "True") ? true : false;
            parcel104a.SelectedValue = Request.QueryString["parcel104a"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];

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
        href += "&parcel104a=" + parcel104a.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Redirect(href);
    }
    protected void ExportList(object sender, EventArgs e)
    {
        string href = "DJ_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&parcel104a=" + parcel104a.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&parcel104a=" + parcel104a.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Redirect(href);
    }
    protected void Execute(object sender, EventArgs e)
    {
        /*var rs = from p in db.RevalProductDJ

                 select p;

        rs = rs.OrderBy(p => p.RevalProductDJ_CharProduct.ProductItem);

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductCategory == ProductCategory);*/


        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        string o = "1=1";
        if (ProductItem > 0)
            o += " AND RevalProductDJ_CharProduct.ProductItem=" + ProductItem;
        if (ProductLevel > 0)
            o += " AND RevalProductDJ_CharProduct.ProductLevel=" + ProductLevel;
        if (ProductCategory > 0)
            o += " AND RevalProductDJ_CharProduct.ProductCategory=" + ProductCategory;

        var rs = (from p in db.RevalProductDJ.Where(o)
                  join r in db.RevalItemDJ on p.IDReval equals r.ID
                  join q in db.StockActualDJ on r.IDProduct equals q.IDProduct
                  select new { p, q }).ToList();

        if (parcel104a.SelectedValue == "1")
        {
            rs = (from p in rs
                  where !p.p.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4") && !z.Stone1A.Nama.Contains("B4"))
                  select p).ToList();
        }
        else if (parcel104a.SelectedValue == "2")
        {
            rs = rs.Where(p => p.p.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4"))).ToList();
        }
        else if (parcel104a.SelectedValue == "3")
        {
            rs = rs.Where(p => p.p.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("B4"))).ToList();
        }

        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);
        if (Tipe == 0 || Tipe == 1 || Tipe == 2)
        {
            rs = rs.Where(p => ((Lokasi > 0) ? p.q.IDLokasi == Lokasi : 1 == 1) && p.q.TipeLokasi == Tipe).ToList();
        }

        if (kw.Text != "")
            rs = rs.Where(p => p.p.Nomor == kw.Text.ToUpper()).ToList();

        if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.p.RevalProductDJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.p.RevalProductDJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.p.RevalProductDJ_CharProduct.CharProductItem.Nama);


        string error = "";
        if (rs != null)
        {

            foreach (var d in rs)
            {
                var product = d.p.RevalProductDJ_CharProduct;

                if (reval_rate_usd.Checked == true)
                {
                    try { saveRevalRateDolar(d.p, product); }
                    catch (Exception ex) { error += "RRate-" + d.p.Nomor + ","; }
                }
                else
                {
                   

                    Setting = 0m;

                    try { saveRevalStone(d.p); }
                    catch (Exception ex) { error += "RSt-" + d.p.Nomor + ","; }
                    try { saveRevalFinishing(d.p, product); }
                    catch (Exception ex) { error += "RFi-" + d.p.Nomor + ","; }
                    try { saveRevalTotalStone(d.p); }
                    catch (Exception ex) { error += "RTSt-" + d.p.Nomor + ","; }
                    try { saveRevalProduct(d.p, product); }
                    catch (Exception ex) { error += "RPr-" + d.p.Nomor + ","; }
                    try { saveRevalBiaya(d.p, product); }
                    catch (Exception ex) { error += "RBy-" + d.p.Nomor + ","; }
                    try { saveRevalMU(d.p, product); }
                    catch (Exception ex) { error += "RMU-" + d.p.Nomor + ","; }
                    try { saveRevalSegmen(d.p, product); }
                    catch (Exception ex) { error += "RSg-" + d.p.Nomor + ","; }
                }

                //try
                //{
                //    saveRevalStone(d.p);
                //    saveRevalFinishing(d.p, product);

                //    saveRevalTotalStone(d.p);

                //    saveRevalProduct(d.p, product);
                //    saveRevalBiaya(d.p, product);



                //    saveRevalMU(d.p, product);
                //    saveRevalSegmen(d.p, product);
                //}
                //catch(Exception ex)
                //{
                //    error += d.p.Nomor + ",";
                //    //Response.Write(ex.ToString());
                //    //Response.End();
                //}
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
        href += "&reval_sk=" + reval_SK.Checked.ToString();
        href += "&reval_sb=" + reval_SB.Checked.ToString();
        href += "&reval_mf=" + reval_MF.Checked.ToString();
        href += "&reval_mu=" + reval_MU.Checked.ToString();
        href += "&reval_rate=" + reval_Rate.Checked.ToString();
        href += "&error=" + HttpUtility.UrlEncode(error);
        href += "&parcel104a=" + parcel104a.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        Response.Redirect(href);


    }

    //Stone Pricing
    protected void saveRevalStone(RevalProductDJ d)
    {
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SK == 1)
            saveRevalStone1A(d);
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
            saveRevalStone1B(d);
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
            saveRevalStone2(d);
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
            saveRevalStone3(d);
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
            saveRevalStone4(d);
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
            saveRevalStone5(d);

    }
    protected void saveRevalStone1A(RevalProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;

        var rs = from p in db.RevalProductDJ_Stone1A
                 where p.IDForm == d.ID
                 select p;

        foreach (var r in rs)
        {

            StonePricing1A prc = r.Stone1A.StonePricing1A;
            StoneCosting1A cost = r.Stone1A.StoneCosting1A;
            if (prc == null || cost == null)
            {
            }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    //Harga
                    if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                    {
                        r.HargaSatuan = prc.Harga.Value;
                        r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                    }

                    if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                    {
                        r.HargaSatuanM = cost.Harga.Value;
                        r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                    }

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                    {
                        //Setting
                        OngkosPB = price.CalculateOngkosPB(d.RevalProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.RevalProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                        r.Setting = OngkosPB;

                        Setting += OngkosPB;
                        db.SubmitChanges();
                    }

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveRevalStone1B(RevalProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

        var rs = from p in db.RevalProductDJ_Stone1B
                 where
                     p.StatusTerpasang == 1
                     && p.IDForm == d.ID
                 select p;
        foreach (var r in rs)
        {

            StonePricing1B prc = r.Stone1B.StonePricing1B;
            StoneCosting1B cost = r.Stone1B.StoneCosting1B;
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue || !prc.InputH.HasValue || !prc.InputP.HasValue) { }
                else
                {
                    //Harga
                    if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                    {
                        r.HargaSatuan = prc.Harga.Value;
                        r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                    }

                    //Rapaport
                    r.HargaInputH = prc.InputH.Value;
                    r.HargaInputP = prc.InputP.Value;

                    if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                    {
                        r.HargaSatuanM = cost.Harga.Value;
                        r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                    }

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                    {
                        //Setting
                        OngkosPB = price.CalculateOngkosPB(d.RevalProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.RevalProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                        r.Setting = OngkosPB;

                        Setting += OngkosPB;
                        db.SubmitChanges();
                    }

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveRevalStone2(RevalProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(2);

        decimal OngkosPB = 0m;

        var rs = from p in db.RevalProductDJ_Stone2
                 where p.IDForm == d.ID
                 select p;
        foreach (var r in rs)
        {

            StonePricing2 prc = r.Stone2.StonePricing2;
            StoneCosting2 cost = r.Stone2.StoneCosting2;
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    //Harga
                    if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                    {
                        r.HargaSatuan = prc.Harga.Value;
                        r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                    }

                    if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                    {
                        r.HargaSatuanM = cost.Harga.Value;
                        r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                    }

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                    {
                        //Setting
                        OngkosPB = price.CalculateOngkosPB(d.RevalProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.RevalProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                        r.Setting = OngkosPB;

                        Setting += OngkosPB;
                        db.SubmitChanges();
                    }

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveRevalStone3(RevalProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(3);

        decimal OngkosPB = 0m;

        var rs = from p in db.RevalProductDJ_Stone3
                 where p.IDForm == d.ID
                 select p;

        foreach (var r in rs)
        {

            StonePricing3 prc = r.Stone3.StonePricing3;
            StoneCosting3 cost = r.Stone3.StoneCosting3;
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    //Harga
                    if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                    {
                        r.HargaSatuan = prc.Harga.Value;
                        r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                    }

                    if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                    {
                        r.HargaSatuanM = cost.Harga.Value;
                        r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                    }

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                    {
                        //Setting
                        OngkosPB = price.CalculateOngkosPB(d.RevalProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.RevalProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                        r.Setting = OngkosPB;

                        Setting += OngkosPB;
                        db.SubmitChanges();
                    }

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveRevalStone4(RevalProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(4);

        decimal OngkosPB = 0m;

        var rs = from p in db.RevalProductDJ_Stone4
                 where p.IDForm == d.ID
                 select p;
        foreach (var r in rs)
        {

            StonePricing4 prc = r.Stone4.StonePricing4;
            StoneCosting4 cost = r.Stone4.StoneCosting4;
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    //Harga
                    if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                    {
                        r.HargaSatuan = prc.Harga.Value;
                        r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                    }

                    if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                    {
                        r.HargaSatuanM = cost.Harga.Value;
                        r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                    }

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                    {
                        //Setting
                        OngkosPB = price.CalculateOngkosPB(d.RevalProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.RevalProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                        r.Setting = OngkosPB;

                        Setting += OngkosPB;
                        db.SubmitChanges();
                    }

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveRevalStone5(RevalProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(5);

        decimal OngkosPB = 0m;

        var rs = from p in db.RevalProductDJ_Stone5
                 where p.IDForm == d.ID
                 select p;
        foreach (var r in rs)
        {
            //StonePricing5 prc = r.Stone5.StonePricing5;
            //StoneCosting5 cost = r.Stone5.StoneCosting5;

            StonePricing5 prc = db.StonePricing5.SingleOrDefault(p => p.ID == r.IDStone);
            StoneCosting5 cost = db.StoneCosting5.SingleOrDefault(p => p.ID == r.IDStone);
            if (prc == null || cost == null) { }
            else
            {
                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    //Harga
                    if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                    {
                        r.HargaSatuan = prc.Harga.Value;
                        r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                    }

                    if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                    {
                        r.HargaSatuanM = cost.Harga.Value;
                        r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                    }

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                    {
                        //Setting
                        try
                        {
                            OngkosPB = price.CalculateOngkosPB(d.RevalProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.RevalProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                        }
                        catch (Exception ex) { }
                        r.Setting = OngkosPB;

                        Setting += OngkosPB;
                        db.SubmitChanges();
                    }

                    ////Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveRevalFinishing(RevalProductDJ d, RevalProductDJ_CharProduct product)
    {
        var rs = from p in d.RevalProductDJ_Finishing select p;
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
    protected void saveRevalProduct(RevalProductDJ d, RevalProductDJ_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing

        int IDTG_Costing = CV.TG_PG1_Costing;
        int IDTG_Pricing = CV.TG_PG1_Pricing;
        if (d.RevalProductDJ_CharProduct.CharFrameMaterial.NamaKode == "AU")
        {
            IDTG_Costing = CV.TG_PG1_Costing;
            IDTG_Pricing = CV.TG_PG1_Pricing;
        }
        else if (d.RevalProductDJ_CharProduct.CharFrameMaterial.NamaKode == "PL")
        {
            IDTG_Costing = CV.TG_PG_Paladium;
            IDTG_Pricing = CV.TG_PG_Paladium2;
        }
        else if (d.RevalProductDJ_CharProduct.CharFrameMaterial.NamaKode == "PT")
        {
            IDTG_Costing = CV.TG_PG_Platina;
            IDTG_Pricing = CV.TG_PG_Platina2;
        }
        else if (d.RevalProductDJ_CharProduct.CharFrameMaterial.NamaKode == "SIL")
        {
            IDTG_Costing = CV.TG_PG_Silver;
            IDTG_Pricing = CV.TG_PG_Silver2;
        }

        var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == IDTG_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == IDTG_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_tg == null || prc_tgM == null) { ok = false; sebab = "Today Gold Price"; }

        //Process
        if (ok || Reval_R == 1 || Reval_M == 1)
        {
            /// Pricing (R)
            if (Reval_R == 1)
            {
                var n = db.RevalProductDJ_PricingProduct.SingleOrDefault(p => p.ID == d.ID);

                //Gold Price
                if (Reval_G == 1)
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
                if (Reval_SK == 1) { n.TotalStone1A = Convert.ToDecimal(TotalRevalStone1A(d)); }
                if (Reval_SB == 1) { n.TotalStone1B = Convert.ToDecimal(TotalRevalStone1B(d)); }
                n.TotalStone2 = Convert.ToDecimal(TotalRevalStone2(d));
                n.TotalStone3 = Convert.ToDecimal(TotalRevalStone3(d));
                n.TotalStone4 = Convert.ToDecimal(TotalRevalStone4(d));
                n.TotalStone5 = Convert.ToDecimal(TotalRevalStone5(d));
                n.Total = n.TotalProduct + Convert.ToDecimal(n.TotalStone1A) + Convert.ToDecimal(n.TotalStone1B) + Convert.ToDecimal(n.TotalStone2) + Convert.ToDecimal(n.TotalStone3) + Convert.ToDecimal(n.TotalStone4) + Convert.ToDecimal(n.TotalStone5);

                //Commit
                db.SubmitChanges();
            }

            /// Costing (M)
            if (Reval_M == 1)
            {

                var m = db.RevalProductDJ_CostingProduct.SingleOrDefault(p => p.ID == d.ID);

                //Gold Price
                if (Reval_G == 1)
                {
                    m.GoldRate = prc_tgM.Harga.Value;
                    m.EfektifGold = prc_tgM.ApprovalTgl.Value;
                    db.SubmitChanges();
                }

                //Informasi Berat
                m.Netto = product.NettoWeight;
                m.Kadar = product.KadarTukaran;

                //Total
                m.TotalProduct = m.GoldRate * m.Netto * (m.Kadar / (decimal)100);
                if (Reval_SK == 1) { m.TotalStone1A = Convert.ToDecimal(TotalMRevalStone1A(d)); }
                if (Reval_SB == 1) { m.TotalStone1B = Convert.ToDecimal(TotalMRevalStone1B(d)); }
                m.TotalStone2 = Convert.ToDecimal(TotalMRevalStone2(d));
                m.TotalStone3 = Convert.ToDecimal(TotalMRevalStone3(d));
                m.TotalStone4 = Convert.ToDecimal(TotalMRevalStone4(d));
                m.TotalStone5 = Convert.ToDecimal(TotalMRevalStone5(d));
                m.Total = m.TotalProduct + Convert.ToDecimal(m.TotalStone1A) + Convert.ToDecimal(m.TotalStone1B) + Convert.ToDecimal(m.TotalStone2) + Convert.ToDecimal(m.TotalStone3) + Convert.ToDecimal(m.TotalStone4) + Convert.ToDecimal(m.TotalStone5);

                //Commit
                db.SubmitChanges();
            }
        }
    }

    //Biaya
    protected void saveRevalBiaya(RevalProductDJ d, RevalProductDJ_CharProduct product)
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
        if (ok)
        {

            var n = db.RevalProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == d.ID);

            //n.ID = d.ID;

            //Informasi
            n.NamaBrand = d.LocWarehouse.CompanyBrand.Nama;
            n.NamaItem = product.CharProductItem.Nama;
            n.NamaCons = product.CharProcessCons.Nama;

            //Mounting
            if (Reval_MF == 1)
            {
                n.Mounting = prc_fm.Biaya.Value;
                n.MountingNO = prc_fm.InputNO.Value;
                n.MountingRO = prc_fm.InputRO.Value;
                n.EfektifMounting = prc_fm.ApprovalTgl.Value;

                //Setting
                //n.Setting = prc_fs.Biaya.Value;
                /*Sementara di Hardcode*/

                Setting = 0m;
                Setting += n.RevalProductDJ.RevalProductDJ_Stone1A.Count() > 0 ? Convert.ToDecimal(n.RevalProductDJ.RevalProductDJ_Stone1A.Sum(p => p.Setting)) : 0m;
                Setting += n.RevalProductDJ.RevalProductDJ_Stone1B.Count() > 0 ? Convert.ToDecimal(n.RevalProductDJ.RevalProductDJ_Stone1B.Sum(p => p.Setting)) : 0m;
                Setting += n.RevalProductDJ.RevalProductDJ_Stone2.Count() > 0 ? Convert.ToDecimal(n.RevalProductDJ.RevalProductDJ_Stone2.Sum(p => p.Setting)) : 0m;
                Setting += n.RevalProductDJ.RevalProductDJ_Stone3.Count() > 0 ? Convert.ToDecimal(n.RevalProductDJ.RevalProductDJ_Stone3.Sum(p => p.Setting)) : 0m;
                Setting += n.RevalProductDJ.RevalProductDJ_Stone4.Count() > 0 ? Convert.ToDecimal(n.RevalProductDJ.RevalProductDJ_Stone4.Sum(p => p.Setting)) : 0m;
                Setting += n.RevalProductDJ.RevalProductDJ_Stone5.Count() > 0 ? Convert.ToDecimal(n.RevalProductDJ.RevalProductDJ_Stone5.Sum(p => p.Setting)) : 0m;

                n.Setting = Setting;
                n.EfektifSetting = prc_fs.ApprovalTgl.Value;
                db.SubmitChanges();
            }

            /*end hardcode*/



            //Finishing
            n.Finishing = TotalRevalFinishing(d);

            //Total
            n.TotalRupiah = n.Mounting + n.Setting + n.Finishing;

            if (Reval_Rate == 1)
            {
                n.TotalRate = prc_us.Harga.Value;
                n.EfektifRate = prc_us.ApprovalTgl.Value;
                db.SubmitChanges();
            }

            n.Total = n.TotalRupiah / n.TotalRate;

            //Tgl. Efektif



            //Commit
            //db.RevalProductDJ_PricingBiaya.InsertOnSubmit(n);
            db.SubmitChanges();

        }
    }
    protected int FS_ID(RevalProductDJ d, RevalProductDJ_CharProduct product)
    {
        return 1; //Masih di-hardcoded. BELUM SELESAI !
    }

    //Mark-Up
    protected void saveRevalMU(RevalProductDJ d, RevalProductDJ_CharProduct product)
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
        if (ok)
        {
            var n = db.RevalProductDJ_PricingMU.SingleOrDefault(p => p.ID == d.ID);

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            if (Reval_R == 1)
            {
                /// R
                n.ModalUSD = TotalRevalModal(d.ID);

                n.ModalRupiah = n.ModalUSD * n.ModalRate;
                db.SubmitChanges();
            }

            if (Reval_MU == 1)
            {
                //Mark-Up
                n.MU = prc_mu.Persen.Value;
                db.SubmitChanges();
            }

            /// Ongkos
            decimal OngkosLain = 0m;

            try
            {
                OngkosLain = d.RevalProductDJ_Costing.OngkosLain.Value;
            }
            catch (Exception ex)
            {
                OngkosLain = 0m;
            }

            decimal rate = prc_us.Harga.Value;
            DateTime efektif_rate = prc_us.ApprovalTgl.Value;
            if(Db.SingleBool("SELECT dbo.fx_isFashionRate(" + product.ID + ", 1) ", Db.Cnn_DB))
            {
                rate = Db.SingleDecimal("SELECT Harga From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
                efektif_rate = Db.SingleTime("SELECT ApprovalTgl From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
            }

            if (Reval_M == 0 && Reval_R == 0 && Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 1)
            {
                n.ModalRate = rate;
                n.HargaRupiah = n.HargaUSD * n.ModalRate;
                n.HargaRupiahRound = price.RoundTop(n.HargaRupiah);
                n.EfektifRate = efektif_rate;

            }
            else if (Reval_M == 1 || Reval_R == 1)
            {
                n.ModalRate = rate;
                n.HargaUSD = TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID) + TotalRevalStone3NMU(d.ID) + TotalRevalStone2(d) + TotalRevalStone4(d) + TotalRevalStone5(d) + (OngkosLain / d.RevalProductDJ_Costing.TotalRate.Value);
                n.HargaRupiah = (TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID) + TotalRevalStone3NMU(d.ID) + TotalRevalStone2(d) + TotalRevalStone4(d) + TotalRevalStone5(d)) * n.ModalRate + OngkosLain;
                n.HargaRupiahRound = price.RoundTop((TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID) + TotalRevalStone3NMU(d.ID) + TotalRevalStone2(d) + TotalRevalStone4(d) + TotalRevalStone5(d)) * n.ModalRate + OngkosLain);
                n.EfektifRate = efektif_rate;
            }

            //Commit
            //db.RevalProductDJ_PricingMU.InsertOnSubmit(n);
            db.SubmitChanges();

        }
    }


    protected void saveRevalRateDolar(RevalProductDJ d, RevalProductDJ_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (ok)
        {
            var n = db.RevalProductDJ_PricingMU.SingleOrDefault(p => p.ID == d.ID);

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;


            /// Ongkos
            decimal OngkosLain = 0m;

            try
            {
                OngkosLain = d.RevalProductDJ_Costing.OngkosLain.Value;
            }
            catch (Exception ex)
            {
                OngkosLain = 0m;
            }
            n.ModalRate = prc_us.Harga.Value;
            DateTime efektif_rate = prc_us.ApprovalTgl.Value;
            if(Db.SingleBool("SELECT dbo.fx_isFashionRate(" + product.ID + ", 1) ", Db.Cnn_DB))
            {
                n.ModalRate = Db.SingleDecimal("SELECT Harga From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
                efektif_rate = Db.SingleTime("SELECT ApprovalTgl From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
            }
            n.HargaRupiah = n.HargaUSD * n.ModalRate;
            n.HargaRupiahRound = price.RoundTop(n.HargaRupiah);
            n.EfektifRate = efektif_rate;


            //Commit
            //db.RevalProductDJ_PricingMU.InsertOnSubmit(n);
            db.SubmitChanges();

        }
    }
    //Segmen System
    protected void saveRevalSegmen(RevalProductDJ d, RevalProductDJ_CharProduct product)
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
            try
            {
                carat = (from p in db.RevalProductDJ_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                        (from p in db.RevalProductDJ_Stone1B where p.IDForm == d.ID && p.StatusTerpasang == 1 select new { Carat = p.CaratButir }).Union
                        (from p in db.RevalProductDJ_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                        (from p in db.RevalProductDJ_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                        (from p in db.RevalProductDJ_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                        (from p in db.RevalProductDJ_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
            }
            catch (Exception ex) { }

            //Parameter
            int IDSegmen = 1;
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

            var u = db.RevalProductDJ_PricingSegmen.SingleOrDefault(p => p.ID == d.ID);
            db.RevalProductDJ_PricingSegmen.DeleteOnSubmit(u);
            db.SubmitChanges();

            RevalProductDJ_PricingSegmen n = new RevalProductDJ_PricingSegmen();

            n.ID = d.ID;
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
            db.RevalProductDJ_PricingSegmen.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected bool checkRevalSegmen(RevalProductDJ_CharProduct product)
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
    protected decimal TotalRevalStone(RevalProductDJ d)
    {
        decimal total = 0;

        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SK == 1)
        {
            var rs_1A = from p in d.RevalProductDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1A) total += r;
        }

        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
        {
            var rs_1B = from p in d.RevalProductDJ_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
            foreach (var r in rs_1B) total += r;
        }

        var rs_2 = from p in d.RevalProductDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.RevalProductDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.RevalProductDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.RevalProductDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    //Sub-Total
    protected decimal TotalRevalStone1A(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.RevalProductDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected decimal TotalRevalStone1B(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in d.RevalProductDJ_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected decimal TotalRevalStone2(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in d.RevalProductDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected decimal TotalRevalStone3(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in d.RevalProductDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected decimal TotalRevalStone4(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in d.RevalProductDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected decimal TotalRevalStone5(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in d.RevalProductDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    protected decimal TotalMRevalStone1A(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.RevalProductDJ_Stone1A where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected decimal TotalMRevalStone1B(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in d.RevalProductDJ_Stone1B where p.HargaTotalM.HasValue && p.StatusTerpasang == 1 select p.HargaTotalM.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected decimal TotalMRevalStone2(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in d.RevalProductDJ_Stone2 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected decimal TotalMRevalStone3(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in d.RevalProductDJ_Stone3 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected decimal TotalMRevalStone4(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in d.RevalProductDJ_Stone4 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected decimal TotalMRevalStone5(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in d.RevalProductDJ_Stone5 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    protected decimal TotalRevalFinishing(RevalProductDJ d)
    {
        decimal total = 0;

        var rs = from p in d.RevalProductDJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }
    protected decimal TotalRevalModal(int ID)
    {
        decimal total = 0;

        var product = db.RevalProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.RevalProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
        total += biaya.Total;

        return total;
    }
    protected decimal TotalRevalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.RevalProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.RevalProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        var stone3 = from p in db.RevalProductDJ_Stone3
                     where p.IDForm == ID && (p.GIA == "" || p.GIA == null)
                     select p;
        decimal Stone3ToMarkUp = 0m;
        try
        {
            Stone3ToMarkUp = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(Stone3ToMarkUp);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalRevalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.RevalProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.RevalProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
    protected decimal TotalRevalStone3NMU(int ID)
    {
        decimal total = 0;

        var stone3 = from p in db.RevalProductDJ_Stone3
                     where p.IDForm == ID && p.GIA != ""
                     select p;
        try
        {
            total = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal HargaRevalUSD(int ID)
    {
        var mu = db.RevalProductDJ_PricingMU.SingleOrDefault(p => p.ID == ID);
        return mu.HargaUSD;
    }
    protected void saveRevalTotalStone(RevalProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.RevalProductDJ_Stone1A select p.TotalButir;
        foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.RevalProductDJ_Stone1B where p.StatusTerpasang == 1 select p.TotalButir;
        foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.RevalProductDJ_Stone2 select p.TotalButir;
        foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.RevalProductDJ_Stone3 select p.TotalButir;
        foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.RevalProductDJ_Stone4 select p.TotalButir;
        foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.RevalProductDJ_Stone5 select p.TotalButir;
        foreach (var r in rs_5) total += r;

        d.RevalProductDJ_CharProduct.StoneQty = total;
        db.SubmitChanges();

    }

    protected void Delete(object sender, EventArgs e)
    {
        int index = 1;

        CheckBox cb = new CheckBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {

            try
            {

                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                cb = (CheckBox)row.FindControl("cb_po" + index.ToString());
                if (cb.Checked == true)
                {
                    var d = db.RevalProductDJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                    if (d == null) App.DataNull(this);
                    else
                    {
                        try
                        {
                            deleteProcess(d);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            index++;
        }
        string href = "DJ_List_v2.aspx?refreshPage=1&d=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&parcel104a=" + parcel104a.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        Response.Redirect(href);
    }

    protected void Apply(object sender, EventArgs e)
    {
        /// NEW APPLY FUNCTION : APPLY CHECKED ITEM ONLY

        int index = 1;
        int count_success = 0;

        CheckBox cb = new CheckBox();
        string hf = "";

        string errmsg = "";

        //int test_index = 1;
        //string test_msg = "";

        string msg_plu_error = "";

        foreach (TableRow row in tb.Rows)
        {
            //test_msg += test_index.ToString();
            //test_index++;

            if (index < tb.Rows.Count)
            {
                hf = ((HiddenField)row.FindControl("hf" + index)).Value;
                cb = (CheckBox)row.FindControl("cb_po" + index);

                //hf = ((HiddenField)row.FindControl("hf1")).Value;
                //cb = (CheckBox)row.FindControl("cb_po1");

                if (cb.Checked == true)
                {
                    //errmsg += hf + ". ";

                    //var d = db.StockProductDJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf) && p.ReturnProduct == false && p.StatusPenjualan == 0);
                    //if (d != null && asd == 1)
                    //{


                    var x = db.RevalProductDJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                    if (x != null)
                    {
                        try
                        {
                            #region longprocess

                            var d = db.StockProductDJ.SingleOrDefault(p => p.Nomor == x.Nomor && p.ReturnProduct == false && p.StatusPenjualan == 0 && !p.Draft.Value);

                            //////////////////////////////////////////////////////////////////////////

                            /// Insert LogReval buat dapetin master id nya
                            int RevalProdID = insertProduct(d);
                            db.SubmitChanges();

                            //////////////////////////////////////////////////////////////////////////

                            /// Costing
                            /// Costing : Update Stock
                            var sp_co = db.StockProductDJ_Costing.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor);
                            sp_co.UnitPrice = x.RevalProductDJ_Costing.UnitPrice;
                            sp_co.Frame = x.RevalProductDJ_Costing.Frame;
                            sp_co.MountingNO = x.RevalProductDJ_Costing.MountingNO;
                            sp_co.MountingRO = x.RevalProductDJ_Costing.MountingRO;
                            sp_co.Pembagi = x.RevalProductDJ_Costing.Pembagi;
                            sp_co.Setting = x.RevalProductDJ_Costing.Setting;
                            sp_co.Finishing = x.RevalProductDJ_Costing.Finishing;
                            sp_co.OngkosLainCOGS = x.RevalProductDJ_Costing.OngkosLainCOGS;
                            sp_co.OngkosLain = x.RevalProductDJ_Costing.OngkosLain;
                            sp_co.TotalRate = x.RevalProductDJ_Costing.TotalRate;
                            sp_co.TotalRupiah = x.RevalProductDJ_Costing.TotalRupiah;
                            sp_co.TotalBiaya = x.RevalProductDJ_Costing.TotalBiaya;
                            db.SubmitChanges();

                            /// Costing : Update LogReval
                            var rlp_co = db.RevalLogProductDJ_Costing.SingleOrDefault(p => p.ID == RevalProdID);
                            rlp_co.UnitPrice = x.RevalProductDJ_Costing.UnitPrice;
                            rlp_co.Frame = x.RevalProductDJ_Costing.Frame;
                            rlp_co.MountingNO = x.RevalProductDJ_Costing.MountingNO;
                            rlp_co.MountingRO = x.RevalProductDJ_Costing.MountingRO;
                            rlp_co.Pembagi = x.RevalProductDJ_Costing.Pembagi;
                            rlp_co.Setting = x.RevalProductDJ_Costing.Setting;
                            rlp_co.Finishing = x.RevalProductDJ_Costing.Finishing;
                            rlp_co.OngkosLainCOGS = x.RevalProductDJ_Costing.OngkosLainCOGS;
                            rlp_co.OngkosLain = x.RevalProductDJ_Costing.OngkosLain;
                            rlp_co.TotalRate = x.RevalProductDJ_Costing.TotalRate;
                            rlp_co.TotalRupiah = x.RevalProductDJ_Costing.TotalRupiah;
                            rlp_co.TotalBiaya = x.RevalProductDJ_Costing.TotalBiaya;
                            db.SubmitChanges();

                            //////////////////////////////////////////////////////////////////////////

                            /// CostingProduct
                            /// CostingProduct : Update Stock
                            var sp_cop = db.StockProductDJ_CostingProduct.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor);
                            sp_cop.GoldRate = x.RevalProductDJ_CostingProduct.GoldRate;
                            sp_cop.TotalProduct = x.RevalProductDJ_CostingProduct.TotalProduct;
                            sp_cop.TotalStone1A = x.RevalProductDJ_CostingProduct.TotalStone1A;
                            sp_cop.TotalStone1B = x.RevalProductDJ_CostingProduct.TotalStone1B;
                            sp_cop.TotalStone2 = x.RevalProductDJ_CostingProduct.TotalStone2;
                            sp_cop.TotalStone3 = x.RevalProductDJ_CostingProduct.TotalStone3;
                            sp_cop.TotalStone4 = x.RevalProductDJ_CostingProduct.TotalStone4;
                            sp_cop.TotalStone5 = x.RevalProductDJ_CostingProduct.TotalStone5;
                            sp_cop.Total = x.RevalProductDJ_CostingProduct.Total;
                            sp_cop.EfektifGold = x.RevalProductDJ_CostingProduct.EfektifGold;
                            db.SubmitChanges();

                            /// CostingProduct : Update LogReval
                            var rlp_cop = db.RevalLogProductDJ_CostingProduct.SingleOrDefault(p => p.ID == RevalProdID);
                            rlp_cop.GoldRate = x.RevalProductDJ_CostingProduct.GoldRate;
                            rlp_cop.TotalProduct = x.RevalProductDJ_CostingProduct.TotalProduct;
                            rlp_cop.TotalStone1A = x.RevalProductDJ_CostingProduct.TotalStone1A;
                            rlp_cop.TotalStone1B = x.RevalProductDJ_CostingProduct.TotalStone1B;
                            rlp_cop.TotalStone2 = x.RevalProductDJ_CostingProduct.TotalStone2;
                            rlp_cop.TotalStone3 = x.RevalProductDJ_CostingProduct.TotalStone3;
                            rlp_cop.TotalStone4 = x.RevalProductDJ_CostingProduct.TotalStone4;
                            rlp_cop.TotalStone5 = x.RevalProductDJ_CostingProduct.TotalStone5;
                            rlp_cop.Total = x.RevalProductDJ_CostingProduct.Total;
                            rlp_cop.EfektifGold = x.RevalProductDJ_CostingProduct.EfektifGold;
                            db.SubmitChanges();

                            //////////////////////////////////////////////////////////////////////////

                            /// Finishing
                            var sph_fin = from p in db.RevalProductDJ_Finishing where p.IDForm == RevalProdID select p;
                            if (sph_fin != null)
                            {
                                foreach (var r in sph_fin)
                                {
                                    /// Finishing : Update Stock
                                    var sp_fin = db.StockProductDJ_Finishing.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor && p.IDFinishing == r.IDFinishing);
                                    sp_fin.Biaya = r.Biaya;
                                    sp_fin.BiayaModal = r.BiayaModal;
                                    sp_fin.Efektif = r.Efektif;
                                    db.SubmitChanges();

                                    /// Finishing : Update LogReval
                                    var rlp_fin = db.RevalLogProductDJ_Finishing.SingleOrDefault(p => p.ID == RevalProdID && p.IDFinishing == r.IDFinishing);
                                    rlp_fin.Biaya = r.Biaya;
                                    rlp_fin.BiayaModal = r.BiayaModal;
                                    rlp_fin.Efektif = r.Efektif;
                                    db.SubmitChanges();

                                }
                            }

                            //////////////////////////////////////////////////////////////////////////

                            /// PricingBiaya
                            /// PricingBiaya : Update Stock
                            var sp_pb = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor);
                            sp_pb.Mounting = x.RevalProductDJ_PricingBiaya.Mounting;
                            sp_pb.MountingNO = x.RevalProductDJ_PricingBiaya.MountingNO;
                            sp_pb.MountingRO = x.RevalProductDJ_PricingBiaya.MountingRO;
                            sp_pb.Setting = x.RevalProductDJ_PricingBiaya.Setting;
                            sp_pb.Finishing = x.RevalProductDJ_PricingBiaya.Finishing;
                            sp_pb.TotalRupiah = x.RevalProductDJ_PricingBiaya.TotalRupiah;
                            sp_pb.TotalRate = x.RevalProductDJ_PricingBiaya.TotalRate;
                            sp_pb.Total = x.RevalProductDJ_PricingBiaya.Total;
                            sp_pb.EfektifRate = x.RevalProductDJ_PricingBiaya.EfektifRate;
                            sp_pb.EfektifMounting = x.RevalProductDJ_PricingBiaya.EfektifMounting;
                            sp_pb.EfektifSetting = x.RevalProductDJ_PricingBiaya.EfektifSetting;
                            db.SubmitChanges();

                            /// PricingBiaya : Update LogReval
                            var rlp_pb = db.RevalLogProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == RevalProdID);
                            rlp_pb.Mounting = x.RevalProductDJ_PricingBiaya.Mounting;
                            rlp_pb.MountingNO = x.RevalProductDJ_PricingBiaya.MountingNO;
                            rlp_pb.MountingRO = x.RevalProductDJ_PricingBiaya.MountingRO;
                            rlp_pb.Setting = x.RevalProductDJ_PricingBiaya.Setting;
                            rlp_pb.Finishing = x.RevalProductDJ_PricingBiaya.Finishing;
                            rlp_pb.TotalRupiah = x.RevalProductDJ_PricingBiaya.TotalRupiah;
                            rlp_pb.TotalRate = x.RevalProductDJ_PricingBiaya.TotalRate;
                            rlp_pb.Total = x.RevalProductDJ_PricingBiaya.Total;
                            rlp_pb.EfektifRate = x.RevalProductDJ_PricingBiaya.EfektifRate;
                            rlp_pb.EfektifMounting = x.RevalProductDJ_PricingBiaya.EfektifMounting;
                            rlp_pb.EfektifSetting = x.RevalProductDJ_PricingBiaya.EfektifSetting;
                            db.SubmitChanges();

                            //////////////////////////////////////////////////////////////////////////

                            /// PricingMU
                            /// PricingMU : Update Stock
                            var sp_mu = db.StockProductDJ_PricingMU.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor);
                            sp_mu.ModalUSD = x.RevalProductDJ_PricingMU.ModalUSD;
                            sp_mu.ModalRate = x.RevalProductDJ_PricingMU.ModalRate;
                            sp_mu.ModalRupiah = x.RevalProductDJ_PricingMU.ModalRupiah;
                            sp_mu.MU = x.RevalProductDJ_PricingMU.MU;
                            sp_mu.HargaUSD = x.RevalProductDJ_PricingMU.HargaUSD;
                            sp_mu.HargaRupiah = x.RevalProductDJ_PricingMU.HargaRupiah;
                            sp_mu.HargaRupiahRound = x.RevalProductDJ_PricingMU.HargaRupiahRound;
                            sp_mu.EfektifRate = x.RevalProductDJ_PricingMU.EfektifRate;
                            db.SubmitChanges();

                            /// PricingMU : Update LogReval
                            var rlp_mu = db.RevalLogProductDJ_PricingMU.SingleOrDefault(p => p.ID == RevalProdID);
                            rlp_mu.ModalUSD = x.RevalProductDJ_PricingMU.ModalUSD;
                            rlp_mu.ModalRate = x.RevalProductDJ_PricingMU.ModalRate;
                            rlp_mu.ModalRupiah = x.RevalProductDJ_PricingMU.ModalRupiah;
                            rlp_mu.MU = x.RevalProductDJ_PricingMU.MU;
                            rlp_mu.HargaUSD = x.RevalProductDJ_PricingMU.HargaUSD;
                            rlp_mu.HargaRupiah = x.RevalProductDJ_PricingMU.HargaRupiah;
                            rlp_mu.HargaRupiahRound = x.RevalProductDJ_PricingMU.HargaRupiahRound;
                            rlp_mu.EfektifRate = x.RevalProductDJ_PricingMU.EfektifRate;
                            db.SubmitChanges();

                            //////////////////////////////////////////////////////////////////////////

                            /// PricingProduct
                            /// PricingProduct : Update Stock
                            var sp_pp = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor);
                            sp_pp.GoldRate = x.RevalProductDJ_PricingProduct.GoldRate;
                            sp_pp.TotalProduct = x.RevalProductDJ_PricingProduct.TotalProduct;
                            sp_pp.TotalStone1A = x.RevalProductDJ_PricingProduct.TotalStone1A;
                            sp_pp.TotalStone1B = x.RevalProductDJ_PricingProduct.TotalStone1B;
                            sp_pp.TotalStone2 = x.RevalProductDJ_PricingProduct.TotalStone2;
                            sp_pp.TotalStone3 = x.RevalProductDJ_PricingProduct.TotalStone3;
                            sp_pp.TotalStone4 = x.RevalProductDJ_PricingProduct.TotalStone4;
                            sp_pp.TotalStone5 = x.RevalProductDJ_PricingProduct.TotalStone5;
                            sp_pp.Total = x.RevalProductDJ_PricingProduct.Total;
                            sp_pp.EfektifGold = x.RevalProductDJ_PricingProduct.EfektifGold;
                            db.SubmitChanges();

                            /// PricingProduct : Update LogReval
                            var rlp_pp = db.RevalLogProductDJ_PricingProduct.SingleOrDefault(p => p.ID == RevalProdID);
                            rlp_pp.GoldRate = x.RevalProductDJ_PricingProduct.GoldRate;
                            rlp_pp.TotalProduct = x.RevalProductDJ_PricingProduct.TotalProduct;
                            rlp_pp.TotalStone1A = x.RevalProductDJ_PricingProduct.TotalStone1A;
                            rlp_pp.TotalStone1B = x.RevalProductDJ_PricingProduct.TotalStone1B;
                            rlp_pp.TotalStone2 = x.RevalProductDJ_PricingProduct.TotalStone2;
                            rlp_pp.TotalStone3 = x.RevalProductDJ_PricingProduct.TotalStone3;
                            rlp_pp.TotalStone4 = x.RevalProductDJ_PricingProduct.TotalStone4;
                            rlp_pp.TotalStone5 = x.RevalProductDJ_PricingProduct.TotalStone5;
                            rlp_pp.Total = x.RevalProductDJ_PricingProduct.Total;
                            rlp_pp.EfektifGold = x.RevalProductDJ_PricingProduct.EfektifGold;
                            db.SubmitChanges();

                            //////////////////////////////////////////////////////////////////////////

                            /// PricingSegmen
                            /// PricingSegmen : Update Stock
                            var sp_ps = db.StockProductDJ_PricingSegmen.Where(p => p.StockProductDJ.Nomor == x.Nomor && p.StockProductDJ.Draft == false);

                            int v_ID_PK = sp_ps.SingleOrDefault().ID;
                            int v_IDProductSegmen_FK = sp_ps.SingleOrDefault().ProductSegmen;
                            string v_NamaCategory = sp_ps.SingleOrDefault().NamaCategory;
                            string v_NamaLevel = sp_ps.SingleOrDefault().NamaLevel;
                            string v_NamaDist = sp_ps.SingleOrDefault().NamaDist;
                            int v_SegmenSistem = sp_ps.SingleOrDefault().SegmenSistem;
                            decimal v_SegmenMinimum = sp_ps.SingleOrDefault().SegmenMinimum;
                            decimal v_TotalCarat = sp_ps.SingleOrDefault().TotalCarat;
                            decimal v_HargaUSD = sp_ps.SingleOrDefault().HargaUSD;
                            DateTime v_EfektifSegmen = sp_ps.SingleOrDefault().EfektifSegmen;

                            if (sp_ps != null)
                            {
                                db.StockProductDJ_PricingSegmen.DeleteAllOnSubmit(sp_ps);
                            }

                            v_IDProductSegmen_FK = x.RevalProductDJ_PricingSegmen.ProductSegmen;
                            v_SegmenSistem = x.RevalProductDJ_PricingSegmen.SegmenSistem;
                            v_SegmenMinimum = x.RevalProductDJ_PricingSegmen.SegmenMinimum;
                            v_HargaUSD = x.RevalProductDJ_PricingSegmen.HargaUSD;
                            v_EfektifSegmen = x.RevalProductDJ_PricingSegmen.EfektifSegmen;

                            StockProductDJ_PricingSegmen n = new StockProductDJ_PricingSegmen();

                            n.ID = v_ID_PK;
                            n.ProductSegmen = v_IDProductSegmen_FK;
                            n.NamaCategory = v_NamaCategory;
                            n.NamaLevel = v_NamaLevel;
                            n.NamaDist = v_NamaDist;
                            n.SegmenSistem = v_SegmenSistem;
                            n.SegmenMinimum = v_SegmenMinimum;
                            n.TotalCarat = v_TotalCarat;
                            n.HargaUSD = v_HargaUSD;
                            n.EfektifSegmen = System.DateTime.Now;

                            //Commit
                            db.StockProductDJ_PricingSegmen.InsertOnSubmit(n);
                            db.SubmitChanges();

                            /// PricingSegmen : Update LogReval
                            //var rlp_ps = db.RevalLogProductDJ_PricingSegmen.SingleOrDefault(p => p.ID == RevalProdID);
                            var rlp_ps = (from z in db.RevalLogProductDJ_PricingSegmen
                                          where z.ID == RevalProdID
                                          select z).FirstOrDefault();
                            rlp_ps.ProductSegmen = x.RevalProductDJ_PricingSegmen.ProductSegmen;
                            //rlp_ps.ProductSegmen = 2;
                            rlp_ps.SegmenSistem = x.RevalProductDJ_PricingSegmen.SegmenSistem;
                            rlp_ps.SegmenMinimum = x.RevalProductDJ_PricingSegmen.SegmenMinimum;
                            rlp_ps.HargaUSD = x.RevalProductDJ_PricingSegmen.HargaUSD;
                            rlp_ps.EfektifSegmen = x.RevalProductDJ_PricingSegmen.EfektifSegmen;
                            db.SubmitChanges();

                            //////////////////////////////////////////////////////////////////////////

                            /// Stone1A
                            var sph_s1a = from p in db.RevalProductDJ_Stone1A where p.IDForm == RevalProdID select p;
                            if (sph_s1a != null)
                            {
                                foreach (var r in sph_s1a)
                                {
                                    /// Stone1A : Update Stock
                                    var sp_s1a = db.StockProductDJ_Stone1A.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor && p.IDStone == r.IDStone);
                                    sp_s1a.HargaSatuan = r.HargaSatuan;
                                    sp_s1a.HargaTotal = r.HargaTotal;
                                    sp_s1a.HargaSatuanM = r.HargaSatuanM;
                                    sp_s1a.HargaTotalM = r.HargaTotalM;
                                    sp_s1a.Efektif = r.Efektif;
                                    sp_s1a.Setting = r.Setting;
                                    db.SubmitChanges();

                                    /// Stone1A : Update LogReval
                                    var rlp_s1a = db.RevalLogProductDJ_Stone1A.SingleOrDefault(p => p.ID == RevalProdID && p.IDStone == r.IDStone);
                                    rlp_s1a.HargaSatuan = r.HargaSatuan;
                                    rlp_s1a.HargaTotal = r.HargaTotal;
                                    rlp_s1a.HargaSatuanM = r.HargaSatuanM;
                                    rlp_s1a.HargaTotalM = r.HargaTotalM;
                                    rlp_s1a.Efektif = r.Efektif;
                                    rlp_s1a.Setting = r.Setting;
                                    db.SubmitChanges();

                                }
                            }

                            //////////////////////////////////////////////////////////////////////////

                            /// Stone1B
                            var sph_s1b = from p in db.RevalProductDJ_Stone1B where p.IDForm == RevalProdID select p;
                            if (sph_s1b != null)
                            {
                                foreach (var r in sph_s1b)
                                {
                                    /// Stone1A : Update Stock
                                    var sp_s1b = db.StockProductDJ_Stone1B.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor && p.IDStone == r.IDStone);
                                    sp_s1b.HargaSatuan = r.HargaSatuan;
                                    sp_s1b.HargaTotal = r.HargaTotal;
                                    sp_s1b.HargaInputH = r.HargaInputH;
                                    sp_s1b.HargaSatuanM = r.HargaSatuanM;
                                    sp_s1b.HargaTotalM = r.HargaTotalM;
                                    sp_s1b.HargaInputP = r.HargaInputP;
                                    sp_s1b.Efektif = r.Efektif;
                                    sp_s1b.Setting = r.Setting;
                                    db.SubmitChanges();

                                    /// Stone1A : Update LogReval
                                    var rlp_s1b = db.RevalLogProductDJ_Stone1B.SingleOrDefault(p => p.ID == RevalProdID && p.IDStone == r.IDStone);
                                    rlp_s1b.HargaSatuan = r.HargaSatuan;
                                    rlp_s1b.HargaTotal = r.HargaTotal;
                                    rlp_s1b.HargaInputH = r.HargaInputH;
                                    rlp_s1b.HargaSatuanM = r.HargaSatuanM;
                                    rlp_s1b.HargaTotalM = r.HargaTotalM;
                                    rlp_s1b.HargaInputP = r.HargaInputP;
                                    rlp_s1b.Efektif = r.Efektif;
                                    rlp_s1b.Setting = r.Setting;
                                    db.SubmitChanges();

                                }
                            }

                            //////////////////////////////////////////////////////////////////////////

                            /// Stone2
                            var sph_s2 = from p in db.RevalProductDJ_Stone2 where p.IDForm == RevalProdID select p;
                            if (sph_s2 != null)
                            {
                                foreach (var r in sph_s2)
                                {
                                    /// Stone2 : Update Stock
                                    var sp_s2 = db.StockProductDJ_Stone2.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor && p.IDStone == r.IDStone);
                                    sp_s2.HargaSatuan = r.HargaSatuan;
                                    sp_s2.HargaTotal = r.HargaTotal;
                                    sp_s2.HargaSatuanM = r.HargaSatuanM;
                                    sp_s2.HargaTotalM = r.HargaTotalM;
                                    sp_s2.Efektif = r.Efektif;
                                    sp_s2.Setting = r.Setting;
                                    db.SubmitChanges();

                                    /// Stone2 : Update LogReval
                                    var rlp_s2 = db.RevalLogProductDJ_Stone2.SingleOrDefault(p => p.ID == RevalProdID && p.IDStone == r.IDStone);
                                    rlp_s2.HargaSatuan = r.HargaSatuan;
                                    rlp_s2.HargaTotal = r.HargaTotal;
                                    rlp_s2.HargaSatuanM = r.HargaSatuanM;
                                    rlp_s2.HargaTotalM = r.HargaTotalM;
                                    rlp_s2.Efektif = r.Efektif;
                                    rlp_s2.Setting = r.Setting;
                                    db.SubmitChanges();

                                }
                            }

                            //////////////////////////////////////////////////////////////////////////

                            /// Stone3
                            var sph_s3 = from p in db.RevalProductDJ_Stone3 where p.IDForm == RevalProdID select p;
                            if (sph_s3 != null)
                            {
                                foreach (var r in sph_s3)
                                {
                                    /// Stone3 : Update Stock
                                    var sp_s3 = db.StockProductDJ_Stone3.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor && p.IDStone == r.IDStone);
                                    sp_s3.HargaSatuan = r.HargaSatuan;
                                    sp_s3.HargaTotal = r.HargaTotal;
                                    sp_s3.HargaSatuanM = r.HargaSatuanM;
                                    sp_s3.HargaTotalM = r.HargaTotalM;
                                    sp_s3.Efektif = r.Efektif;
                                    sp_s3.Setting = r.Setting;
                                    db.SubmitChanges();

                                    /// Stone3 : Update LogReval
                                    var rlp_s3 = db.RevalLogProductDJ_Stone3.SingleOrDefault(p => p.ID == RevalProdID && p.IDStone == r.IDStone);
                                    rlp_s3.HargaSatuan = r.HargaSatuan;
                                    rlp_s3.HargaTotal = r.HargaTotal;
                                    rlp_s3.HargaSatuanM = r.HargaSatuanM;
                                    rlp_s3.HargaTotalM = r.HargaTotalM;
                                    rlp_s3.Efektif = r.Efektif;
                                    rlp_s3.Setting = r.Setting;
                                    db.SubmitChanges();

                                }
                            }

                            //////////////////////////////////////////////////////////////////////////

                            /// Stone4
                            var sph_s4 = from p in db.RevalProductDJ_Stone4 where p.IDForm == RevalProdID select p;
                            if (sph_s4 != null)
                            {
                                foreach (var r in sph_s4)
                                {
                                    /// Stone4 : Update Stock
                                    var sp_s4 = db.StockProductDJ_Stone4.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor && p.IDStone == r.IDStone);
                                    sp_s4.HargaSatuan = r.HargaSatuan;
                                    sp_s4.HargaTotal = r.HargaTotal;
                                    sp_s4.HargaSatuanM = r.HargaSatuanM;
                                    sp_s4.HargaTotalM = r.HargaTotalM;
                                    sp_s4.Efektif = r.Efektif;
                                    sp_s4.Setting = r.Setting;
                                    db.SubmitChanges();

                                    /// Stone4 : Update LogReval
                                    var rlp_s4 = db.RevalLogProductDJ_Stone4.SingleOrDefault(p => p.ID == RevalProdID && p.IDStone == r.IDStone);
                                    rlp_s4.HargaSatuan = r.HargaSatuan;
                                    rlp_s4.HargaTotal = r.HargaTotal;
                                    rlp_s4.HargaSatuanM = r.HargaSatuanM;
                                    rlp_s4.HargaTotalM = r.HargaTotalM;
                                    rlp_s4.Efektif = r.Efektif;
                                    rlp_s4.Setting = r.Setting;
                                    db.SubmitChanges();

                                }
                            }

                            //////////////////////////////////////////////////////////////////////////

                            /// Stone5
                            var sph_s5 = from p in db.RevalProductDJ_Stone5 where p.IDForm == RevalProdID select p;
                            if (sph_s5 != null)
                            {
                                foreach (var r in sph_s5)
                                {
                                    /// Stone5 : Update Stock
                                    var sp_s5 = db.StockProductDJ_Stone5.SingleOrDefault(p => p.StockProductDJ.Nomor == x.Nomor && p.IDStone == r.IDStone);
                                    sp_s5.HargaSatuan = r.HargaSatuan;
                                    sp_s5.HargaTotal = r.HargaTotal;
                                    sp_s5.HargaSatuanM = r.HargaSatuanM;
                                    sp_s5.HargaTotalM = r.HargaTotalM;
                                    sp_s5.Efektif = r.Efektif;
                                    sp_s5.Setting = r.Setting;
                                    db.SubmitChanges();

                                    /// Stone5 : Update LogReval
                                    var rlp_s5 = db.RevalLogProductDJ_Stone5.SingleOrDefault(p => p.ID == RevalProdID && p.IDStone == r.IDStone);
                                    rlp_s5.HargaSatuan = r.HargaSatuan;
                                    rlp_s5.HargaTotal = r.HargaTotal;
                                    rlp_s5.HargaSatuanM = r.HargaSatuanM;
                                    rlp_s5.HargaTotalM = r.HargaTotalM;
                                    rlp_s5.Efektif = r.Efektif;
                                    rlp_s5.Setting = r.Setting;
                                    db.SubmitChanges();

                                }
                            }

                            //////////////////////////////////////////////////////////////////////////

                            /// delete item reval selected
                            //try
                            //{
                            deleteProcess(x);
                            db.SubmitChanges();

                            count_success++;
                            //}
                            //catch (Exception ex)
                            //{

                            //}

                            //////////////////////////////////////////////////////////////////////////




                            db.SubmitChanges();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            msg_plu_error += x.Nomor + ", ";
                        }
                    }
                    // close if not null
                }
                // close cb checked
            }
            // close looping checkbox

            //catch (Exception ex) { Response.Write(ex.ToString()); }

            index++;
        }

        if (msg_plu_error != "")
        {
            errmsg += " PLU " + msg_plu_error + " error atau sudah terjual";

            err.Text = errmsg;
        }

        // clear data row
        //for (int i = 1; i < tb.Rows.Count; i++)
        //{
        //    tb.Rows.RemoveAt(i);
        //}
        //tb.Rows.Clear();

        //fillTable(sender, e);

        //string href = App.RefreshURL(this);
        //string href = "DJ_List_v2.aspx?refreshPage=1&d=1&totalproduct=" + index;

        //Customize-Qstr

        //href += "&refreshPage=1&d=1&totalproduct=" + count_success;
        //href += "&kw=" + kw.Text;
        //href += "&productitem=" + productitem.SelectedValue;
        //href += "&productcategory=" + productcategory.SelectedValue;
        //href += "&productlevel=" + productlevel.SelectedValue;
        //href += "&order=" + orderby.SelectedValue;
        //href += "&parcel104a=" + parcel104a.SelectedValue;
        //href += "&tipelokasi=" + tipelokasi.Text;
        //href += "&lokasi=" + lokasi.Text;
        //Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "ExportDJ.aspx?t=expport";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&reval_m=" + reval_M.Checked.ToString();
        href += "&reval_r=" + reval_R.Checked.ToString();
        href += "&reval_g=" + reval_G.Checked.ToString();
        href += "&reval_sk=" + reval_SK.Checked.ToString();
        href += "&reval_sb=" + reval_SB.Checked.ToString();
        href += "&reval_mf=" + reval_MF.Checked.ToString();
        href += "&reval_mu=" + reval_MU.Checked.ToString();
        href += "&reval_rate=" + reval_Rate.Checked.ToString();
        href += "&parcel104a=" + parcel104a.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Write("<script>window.open('" + href + "');</script>");
    }

    //Apply Reval
    //insert product log
    protected int insertProduct(StockProductDJ d)
    {
        RevalLogProductDJ prod = new RevalLogProductDJ();

        prod.Nomor = d.Nomor;

        //prod.Operator = d.Operator;
        prod.Operator = App.UserNama;
        prod.OperatorTgl = DateTime.Now;
        //prod.Approval = d.Approval;
        prod.Approval = App.UserNama;
        prod.ApprovalTgl = DateTime.Now;
        prod.ImgPicture = d.ImgPicture;

        db.RevalLogProductDJ.InsertOnSubmit(prod);
        db.SubmitChanges();

        int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM RevalLogProductDJ ORDER BY ID DESC", Db.Cnn_DB);

        var cek1 = db.RevalLogProductDJ_CharProduct.SingleOrDefault(p => p.ID == ProdID);

        if (cek1 == null)
        {
            RevalLogProductDJ_CharProduct pd = new RevalLogProductDJ_CharProduct();
            pd.ID = ProdID;
            pd.ProductItem = d.StockProductDJ_CharProduct.ProductItem;
            pd.ProductCategory = d.StockProductDJ_CharProduct.ProductCategory;
            pd.ProductLevel = d.StockProductDJ_CharProduct.ProductLevel;
            pd.StoneDist = d.StockProductDJ_CharProduct.StoneDist;
            pd.FrameColor = d.StockProductDJ_CharProduct.FrameColor;
            pd.FrameFinishing = d.StockProductDJ_CharProduct.FrameFinishing;
            pd.FrameMaterial = d.StockProductDJ_CharProduct.FrameMaterial;
            pd.ProcessCons = d.StockProductDJ_CharProduct.ProcessCons;
            pd.GrossWeight = d.StockProductDJ_CharProduct.GrossWeight;
            pd.StoneCarat = d.StockProductDJ_CharProduct.StoneCarat;
            pd.StoneWeight = d.StockProductDJ_CharProduct.StoneWeight;
            pd.NettoWeight = d.StockProductDJ_CharProduct.NettoWeight;
            pd.KadarLogam = d.StockProductDJ_CharProduct.KadarLogam;
            pd.KadarTukaran = d.StockProductDJ_CharProduct.KadarTukaran;
            pd.DimensiD = d.StockProductDJ_CharProduct.DimensiD;
            pd.DimensiL = d.StockProductDJ_CharProduct.DimensiL;
            pd.DimensiP = d.StockProductDJ_CharProduct.DimensiP;
            pd.DimensiR = d.StockProductDJ_CharProduct.DimensiR;
            pd.StoneQty = d.StockProductDJ_CharProduct.StoneQty;

            db.RevalLogProductDJ_CharProduct.InsertOnSubmit(pd);
            db.SubmitChanges();
        }

        var cek2 = db.RevalLogProductDJ_Costing.SingleOrDefault(p => p.ID == ProdID);

        if (cek2 == null)
        {

            RevalLogProductDJ_Costing cost = new RevalLogProductDJ_Costing();
            cost.ID = ProdID;
            cost.UnitPriceOld = d.StockProductDJ_Costing.UnitPrice;
            cost.FrameOld = d.StockProductDJ_Costing.Frame;
            cost.MountingNOOld = d.StockProductDJ_Costing.MountingNO;
            cost.MountingROOld = d.StockProductDJ_Costing.MountingRO;
            cost.Pembagi = d.StockProductDJ_Costing.Pembagi.Value; /// no old
            cost.SettingOld = d.StockProductDJ_Costing.Setting;
            cost.FinishingOld = d.StockProductDJ_Costing.Finishing;
            cost.OngkosLainCOGSOld = d.StockProductDJ_Costing.OngkosLainCOGS;
            cost.OngkosLainOld = d.StockProductDJ_Costing.OngkosLain;
            cost.TotalRateOld = d.StockProductDJ_Costing.TotalRate.Value;
            cost.TotalRupiahOld = d.StockProductDJ_Costing.TotalRupiah.Value;
            cost.TotalBiayaOld = d.StockProductDJ_Costing.TotalBiaya;

            db.RevalLogProductDJ_Costing.InsertOnSubmit(cost);
            db.SubmitChanges();
        }

        var cek3 = db.RevalLogProductDJ_CostingProduct.SingleOrDefault(p => p.ID == ProdID);

        if (cek3 == null)
        {

            RevalLogProductDJ_CostingProduct cp = new RevalLogProductDJ_CostingProduct();
            cp.ID = ProdID;
            cp.GoldRateOld = d.StockProductDJ_CostingProduct.GoldRate;
            cp.Netto = d.StockProductDJ_CostingProduct.Netto; /// no old
            cp.Kadar = d.StockProductDJ_CostingProduct.Kadar; /// no old
            cp.TotalProductOld = d.StockProductDJ_CostingProduct.TotalProduct;
            cp.TotalStone1AOld = d.StockProductDJ_CostingProduct.TotalStone1A;
            cp.TotalStone1BOld = d.StockProductDJ_CostingProduct.TotalStone1B;
            cp.TotalStone2Old = d.StockProductDJ_CostingProduct.TotalStone2;
            cp.TotalStone3Old = d.StockProductDJ_CostingProduct.TotalStone3;
            cp.TotalStone4Old = d.StockProductDJ_CostingProduct.TotalStone4;
            cp.TotalStone5Old = d.StockProductDJ_CostingProduct.TotalStone5;
            cp.TotalOld = d.StockProductDJ_CostingProduct.Total;
            cp.EfektifGoldOld = d.StockProductDJ_CostingProduct.EfektifGold;

            db.RevalLogProductDJ_CostingProduct.InsertOnSubmit(cp);
            db.SubmitChanges();
        }

        var l_fns = from p in db.StockProductDJ_Finishing
                    where p.IDForm == d.ID
                    select p;
        if (l_fns != null)
        {
            foreach (var r in l_fns)
            {
                RevalLogProductDJ_Finishing fns = new RevalLogProductDJ_Finishing();
                fns.IDForm = ProdID; /// no old
                fns.IDFinishing = r.IDFinishing; /// no old
                fns.BiayaOld = r.Biaya;
                fns.BiayaModalOld = r.BiayaModal;
                fns.EfektifOld = r.Efektif;
                db.RevalLogProductDJ_Finishing.InsertOnSubmit(fns);
                db.SubmitChanges();
            }
        }

        var cek4 = db.RevalLogProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ProdID);

        if (cek4 == null)
        {

            RevalLogProductDJ_PricingBiaya by = new RevalLogProductDJ_PricingBiaya();
            by.ID = ProdID;
            by.NamaBrand = d.StockProductDJ_PricingBiaya.NamaBrand; /// no old
            by.NamaItem = d.StockProductDJ_PricingBiaya.NamaItem; /// no old
            by.NamaCons = d.StockProductDJ_PricingBiaya.NamaCons; /// no old
            by.MountingOld = d.StockProductDJ_PricingBiaya.Mounting;
            by.MountingNOOld = d.StockProductDJ_PricingBiaya.MountingNO;
            by.MountingROOld = d.StockProductDJ_PricingBiaya.MountingRO;
            by.SettingOld = d.StockProductDJ_PricingBiaya.Setting;
            by.FinishingOld = d.StockProductDJ_PricingBiaya.Finishing;
            by.TotalRupiahOld = d.StockProductDJ_PricingBiaya.TotalRupiah;
            by.TotalRateOld = d.StockProductDJ_PricingBiaya.TotalRate;
            by.TotalOld = d.StockProductDJ_PricingBiaya.Total;
            by.EfektifRateOld = d.StockProductDJ_PricingBiaya.EfektifRate;
            by.EfektifMountingOld = d.StockProductDJ_PricingBiaya.EfektifMounting;
            by.EfektifSettingOld = d.StockProductDJ_PricingBiaya.EfektifSetting;

            db.RevalLogProductDJ_PricingBiaya.InsertOnSubmit(by);
            db.SubmitChanges();
        }

        var cek5 = db.RevalLogProductDJ_PricingMU.SingleOrDefault(p => p.ID == ProdID);

        if (cek5 == null)
        {

            RevalLogProductDJ_PricingMU mu = new RevalLogProductDJ_PricingMU();
            mu.ID = ProdID;
            mu.NamaCategory = d.StockProductDJ_PricingMU.NamaCategory;
            mu.NamaLevel = d.StockProductDJ_PricingMU.NamaLevel;
            mu.NamaDist = d.StockProductDJ_PricingMU.NamaDist;
            mu.ModalUSDOld = d.StockProductDJ_PricingMU.ModalUSD;
            mu.ModalRateOld = d.StockProductDJ_PricingMU.ModalRate;
            mu.ModalRupiahOld = d.StockProductDJ_PricingMU.ModalRupiah;
            mu.MUOld = d.StockProductDJ_PricingMU.MU;
            mu.HargaUSDOld = d.StockProductDJ_PricingMU.HargaUSD;
            mu.HargaRupiahOld = d.StockProductDJ_PricingMU.HargaRupiah;
            mu.HargaRupiahRoundOld = d.StockProductDJ_PricingMU.HargaRupiahRound;
            mu.EfektifRateOld = d.StockProductDJ_PricingMU.EfektifRate;

            db.RevalLogProductDJ_PricingMU.InsertOnSubmit(mu);
            db.SubmitChanges();
        }

        var cek6 = db.RevalLogProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ProdID);

        if (cek6 == null)
        {

            RevalLogProductDJ_PricingProduct prc = new RevalLogProductDJ_PricingProduct();
            prc.ID = ProdID;
            prc.GoldRateOld = d.StockProductDJ_PricingProduct.GoldRate;
            prc.Netto = d.StockProductDJ_PricingProduct.Netto;
            prc.Kadar = d.StockProductDJ_PricingProduct.Kadar;
            prc.TotalProductOld = d.StockProductDJ_PricingProduct.TotalProduct;
            prc.TotalStone1AOld = d.StockProductDJ_PricingProduct.TotalStone1A;
            prc.TotalStone1BOld = d.StockProductDJ_PricingProduct.TotalStone1B;
            prc.TotalStone2Old = d.StockProductDJ_PricingProduct.TotalStone2;
            prc.TotalStone3Old = d.StockProductDJ_PricingProduct.TotalStone3;
            prc.TotalStone4Old = d.StockProductDJ_PricingProduct.TotalStone4;
            prc.TotalStone5Old = d.StockProductDJ_PricingProduct.TotalStone5;
            prc.TotalOld = d.StockProductDJ_PricingProduct.Total;
            prc.EfektifGoldOld = d.StockProductDJ_PricingProduct.EfektifGold;

            db.RevalLogProductDJ_PricingProduct.InsertOnSubmit(prc);
            db.SubmitChanges();
        }

        var cek7 = db.RevalLogProductDJ_PricingSegmen.SingleOrDefault(p => p.ID == ProdID);

        if (cek7 == null)
        {

            RevalLogProductDJ_PricingSegmen sg = new RevalLogProductDJ_PricingSegmen();
            sg.ID = ProdID;
            sg.ProductSegmen = d.StockProductDJ_PricingSegmen.ProductSegmen;
            sg.NamaCategory = d.StockProductDJ_PricingSegmen.NamaCategory;
            sg.NamaLevel = d.StockProductDJ_PricingSegmen.NamaLevel;
            sg.NamaDist = d.StockProductDJ_PricingSegmen.NamaDist;
            sg.SegmenSistemOld = d.StockProductDJ_PricingSegmen.SegmenSistem;
            sg.SegmenMinimumOld = d.StockProductDJ_PricingSegmen.SegmenMinimum;
            sg.TotalCarat = d.StockProductDJ_PricingSegmen.TotalCarat; /// no old
            sg.HargaUSDOld = d.StockProductDJ_PricingSegmen.HargaUSD;
            sg.EfektifSegmenOld = d.StockProductDJ_PricingSegmen.EfektifSegmen;

            db.RevalLogProductDJ_PricingSegmen.InsertOnSubmit(sg);
            db.SubmitChanges();
        }

        var s1A = from p in db.StockProductDJ_Stone1A
                  where p.IDForm == d.ID
                  select p;
        if (s1A != null)
        {
            foreach (var r in s1A)
            {
                RevalLogProductDJ_Stone1A stone = new RevalLogProductDJ_Stone1A();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone1A.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        var s1B = from p in db.StockProductDJ_Stone1B
                  where p.IDForm == d.ID && p.StatusTerpasang == 1
                  select p;
        if (s1B != null)
        {
            foreach (var r in s1B)
            {
                RevalLogProductDJ_Stone1B stone = new RevalLogProductDJ_Stone1B();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.GIA = r.GIA;
                stone.StatusTerpasang = r.StatusTerpasang; /// no old
                stone.HargaInputHOld = r.HargaInputH;
                stone.HargaInputPOld = r.HargaInputP;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone1B.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        var s2 = from p in db.StockProductDJ_Stone2
                 where p.IDForm == d.ID
                 select p;
        if (s2 != null)
        {
            foreach (var r in s2)
            {
                RevalLogProductDJ_Stone2 stone = new RevalLogProductDJ_Stone2();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone2.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        var s3 = from p in db.StockProductDJ_Stone3
                 where p.IDForm == d.ID
                 select p;
        if (s3 != null)
        {
            foreach (var r in s3)
            {
                RevalLogProductDJ_Stone3 stone = new RevalLogProductDJ_Stone3();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone3.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        var s4 = from p in db.StockProductDJ_Stone4
                 where p.IDForm == d.ID
                 select p;
        if (s4 != null)
        {
            foreach (var r in s4)
            {
                RevalLogProductDJ_Stone4 stone = new RevalLogProductDJ_Stone4();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone4.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }


        var s5 = from p in db.StockProductDJ_Stone5
                 where p.IDForm == d.ID
                 select p;
        if (s5 != null)
        {
            foreach (var r in s5)
            {
                RevalLogProductDJ_Stone5 stone = new RevalLogProductDJ_Stone5();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone5.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        return ProdID;
    }

    protected void deleteProcess(RevalProductDJ d)
    {
        if (d.RevalProductDJ_CharDesign != null)
            db.RevalProductDJ_CharDesign.DeleteOnSubmit(
                d.RevalProductDJ_CharDesign);

        if (d.RevalProductDJ_CharProduct != null)
            db.RevalProductDJ_CharProduct.DeleteOnSubmit(
                d.RevalProductDJ_CharProduct);

        if (d.RevalProductDJ_Costing != null)
            db.RevalProductDJ_Costing.DeleteOnSubmit(
                d.RevalProductDJ_Costing);

        if (d.RevalProductDJ_CostingProduct != null)
            db.RevalProductDJ_CostingProduct.DeleteOnSubmit(
                d.RevalProductDJ_CostingProduct);

        var s1A = db.RevalProductDJ_Stone1A.Where(p => p.IDForm == d.ID);
        if (s1A != null)
        {
            db.RevalProductDJ_Stone1A.DeleteAllOnSubmit(s1A);
            db.SubmitChanges();
        }

        var s1B = db.RevalProductDJ_Stone1B.Where(p => p.IDForm == d.ID);
        if (s1B != null)
        {
            db.RevalProductDJ_Stone1B.DeleteAllOnSubmit(s1B);
            db.SubmitChanges();
        }

        var s2 = db.RevalProductDJ_Stone2.Where(p => p.IDForm == d.ID);
        if (s2 != null)
        {
            db.RevalProductDJ_Stone2.DeleteAllOnSubmit(s2);
            db.SubmitChanges();
        }

        var s3 = db.RevalProductDJ_Stone3.Where(p => p.IDForm == d.ID);
        if (s3 != null)
        {
            db.RevalProductDJ_Stone3.DeleteAllOnSubmit(s3);
            db.SubmitChanges();
        }

        var s4 = db.RevalProductDJ_Stone4.Where(p => p.IDForm == d.ID);
        if (s4 != null)
        {
            db.RevalProductDJ_Stone4.DeleteAllOnSubmit(s4);
            db.SubmitChanges();
        }

        var s5 = db.RevalProductDJ_Stone5.Where(p => p.IDForm == d.ID);
        if (s5 != null)
        {
            db.RevalProductDJ_Stone5.DeleteAllOnSubmit(s5);
            db.SubmitChanges();
        }

        var fn = db.RevalProductDJ_Finishing.Where(p => p.IDForm == d.ID);
        if (fn != null)
        {
            db.RevalProductDJ_Finishing.DeleteAllOnSubmit(fn);
            db.SubmitChanges();
        }

        if (d.RevalProductDJ_PricingBiaya != null)
            db.RevalProductDJ_PricingBiaya.DeleteOnSubmit(
                d.RevalProductDJ_PricingBiaya);

        if (d.RevalProductDJ_PricingMU != null)
            db.RevalProductDJ_PricingMU.DeleteOnSubmit(
                d.RevalProductDJ_PricingMU);

        if (d.RevalProductDJ_PricingProduct != null)
            db.RevalProductDJ_PricingProduct.DeleteOnSubmit(
                d.RevalProductDJ_PricingProduct);

        if (d.RevalProductDJ_PricingSegmen != null)
            db.RevalProductDJ_PricingSegmen.DeleteOnSubmit(
                d.RevalProductDJ_PricingSegmen);

        if (d != null)
            db.RevalProductDJ.DeleteOnSubmit(d);

        if (d.RevalItemDJ != null)
            db.RevalItemDJ.DeleteOnSubmit(d.RevalItemDJ);

        //Commit
        db.SubmitChanges();
    }

    //Stone Pricing
    protected void saveStone(StockProductDJ d, int ProdID)
    {
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SK == 1)
            saveStone1A(d, ProdID);
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
            saveStone1B(d, ProdID);
        saveStone2(d, ProdID);
        saveStone3(d, ProdID);
        saveStone4(d, ProdID);
        saveStone5(d, ProdID);

    }
    protected void saveStone1A(StockProductDJ d, int ProdID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone1A
                 where p.IDForm == d.ID
                 select p;
        foreach (var r in rs)
        {

            StonePricing1A prc = r.Stone1A.StonePricing1A;
            StoneCosting1A cost = r.Stone1A.StoneCosting1A;
            if (prc == null || cost == null)
            {
            }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));

                    //var st = db.RevalLogProductDJ_Stone1A.SingleOrDefault(p => p.IDForm == ProdID && p.IDStone == r.Stone1A.ID);
                    var tt = from p in db.RevalLogProductDJ_Stone1A
                             where p.IDForm == ProdID && p.IDStone == r.Stone1A.ID
                             select p;

                    foreach (var tt_d in tt)
                    {
                        var st = db.RevalLogProductDJ_Stone1A.SingleOrDefault(p => p.ID == tt_d.ID);
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            st.HargaSatuanOld = r.HargaSatuan;
                            st.HargaSatuan = prc.Harga.Value;
                            st.HargaTotalOld = r.HargaTotal;
                            st.HargaTotal = prc.Harga.Value * r.TotalCarat;
                            //Tgl. Efektif
                            r.Efektif = prc.ApprovalTgl.Value;
                            st.EfektifOld = r.Efektif;
                            st.Efektif = prc.ApprovalTgl.Value;
                            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                            {
                                st.SettingOld = r.Setting;
                                st.Setting = OngkosPB;
                                db.SubmitChanges();
                            }
                            db.SubmitChanges();
                        }

                        if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                        {
                            st.HargaSatuanMOld = r.HargaSatuanM;
                            st.HargaSatuanM = cost.Harga.Value;
                            st.HargaTotalMOld = r.HargaTotalM;
                            st.HargaSatuanM = cost.Harga.Value * r.TotalCarat;
                            db.SubmitChanges();
                        }
                    }


                    if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                    {
                        //Harga
                        r.HargaSatuan = prc.Harga.Value;
                        r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                        db.SubmitChanges();
                    }

                    if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                    {
                        r.HargaSatuanM = cost.Harga.Value;
                        r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                        db.SubmitChanges();
                    }


                    if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                    {
                        //Setting

                        r.Setting = OngkosPB;

                        Setting += OngkosPB;
                        db.SubmitChanges();
                    }

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone1B(StockProductDJ d, int ProdID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone1B where p.IDForm == d.ID && p.StatusTerpasang == 1 select p;
        foreach (var r in rs)
        {

            StonePricing1B prc = r.Stone1B.StonePricing1B;
            StoneCosting1B cost = r.Stone1B.StoneCosting1B;
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue || !prc.InputH.HasValue || !prc.InputP.HasValue) { }
                else
                {
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));

                    //var st = db.RevalLogProductDJ_Stone1B.SingleOrDefault(p => p.IDForm == ProdID && p.IDStone == r.Stone1B.ID);
                    var tt = from p in db.RevalLogProductDJ_Stone1B
                             where p.IDForm == ProdID && p.IDStone == r.Stone1B.ID
                             select p;

                    foreach (var tt_d in tt)
                    {
                        var st = db.RevalLogProductDJ_Stone1B.SingleOrDefault(p => p.ID == tt_d.ID);
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                st.HargaSatuanOld = r.HargaSatuan;
                                st.HargaSatuan = prc.Harga.Value;
                                st.HargaTotalOld = r.HargaTotal;
                                st.HargaTotal = prc.Harga.Value * r.TotalCarat;
                                st.HargaInputHOld = r.HargaInputH;
                                st.HargaInputH = prc.InputH.Value;
                                st.HargaInputPOld = r.HargaInputP;
                                st.HargaInputP = prc.InputP.Value;
                                st.EfektifOld = r.Efektif;
                                st.Efektif = prc.ApprovalTgl.Value;
                                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                                {
                                    st.SettingOld = r.Setting;
                                    st.Setting = OngkosPB;
                                    db.SubmitChanges();
                                }
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                st.HargaSatuanMOld = r.HargaSatuanM;
                                st.HargaSatuanM = cost.Harga.Value;
                                st.HargaTotalMOld = r.HargaTotalM;
                                st.HargaSatuanM = cost.Harga.Value * r.TotalCarat;
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                //Harga
                                r.HargaSatuan = prc.Harga.Value;
                                r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                                //Tgl. Efektif
                                r.Efektif = prc.ApprovalTgl.Value;
                                db.SubmitChanges();
                            }

                            //Rapaport
                            r.HargaInputH = prc.InputH.Value;
                            r.HargaInputP = prc.InputP.Value;

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                                db.SubmitChanges();
                            }


                            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                            {
                                //Setting

                                r.Setting = OngkosPB;

                                Setting += OngkosPB;
                                db.SubmitChanges();
                            }

                            //Commit
                            db.SubmitChanges();
                        }
                    }
                }
            }
        }
    }
    protected void saveStone2(StockProductDJ d, int ProdID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(2);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone2 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {

            StonePricing2 prc = r.Stone2.StonePricing2;
            StoneCosting2 cost = r.Stone2.StoneCosting2;
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));

                    //var st = db.RevalLogProductDJ_Stone2.SingleOrDefault(p => p.IDForm == ProdID && p.IDStone == r.Stone2.ID);

                    var tt = from p in db.RevalLogProductDJ_Stone2
                             where p.IDForm == ProdID && p.IDStone == r.Stone2.ID
                             select p;

                    foreach (var tt_d in tt)
                    {
                        var st = db.RevalLogProductDJ_Stone2.SingleOrDefault(p => p.ID == tt_d.ID);
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                st.HargaSatuanOld = r.HargaSatuan;
                                st.HargaSatuan = prc.Harga.Value;
                                st.HargaTotalOld = r.HargaTotal;
                                st.HargaTotal = prc.Harga.Value * r.TotalCarat;
                                st.EfektifOld = r.Efektif;
                                st.Efektif = prc.ApprovalTgl.Value;
                                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                                {
                                    st.SettingOld = r.Setting;
                                    st.Setting = OngkosPB;
                                    db.SubmitChanges();
                                }
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                st.HargaSatuanMOld = r.HargaSatuanM;
                                st.HargaSatuanM = cost.Harga.Value;
                                st.HargaTotalMOld = r.HargaTotalM;
                                st.HargaSatuanM = cost.Harga.Value * r.TotalCarat;

                                db.SubmitChanges();
                            }

                            //Harga
                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                r.HargaSatuan = prc.Harga.Value;
                                r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                                //Tgl. Efektif
                                r.Efektif = prc.ApprovalTgl.Value;
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                                db.SubmitChanges();
                            }

                            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                            {
                                //Setting

                                r.Setting = OngkosPB;

                                Setting += OngkosPB;
                                db.SubmitChanges();
                            }

                            //Commit
                            db.SubmitChanges();
                        }
                    }
                }
            }
        }
    }
    protected void saveStone3(StockProductDJ d, int ProdID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(3);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone3 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {

            StonePricing3 prc = r.Stone3.StonePricing3;
            StoneCosting3 cost = r.Stone3.StoneCosting3;
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));

                    //var st = db.RevalLogProductDJ_Stone2.SingleOrDefault(p => p.IDForm == ProdID && p.IDStone == r.Stone3.ID);
                    var tt = from p in db.RevalLogProductDJ_Stone3
                             where p.IDForm == ProdID && p.IDStone == r.Stone3.ID
                             select p;

                    foreach (var tt_d in tt)
                    {
                        var st = db.RevalLogProductDJ_Stone3.SingleOrDefault(p => p.ID == tt_d.ID);
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                st.HargaSatuanOld = r.HargaSatuan;
                                st.HargaSatuan = prc.Harga.Value;
                                st.HargaTotalOld = r.HargaTotal;
                                st.HargaTotal = prc.Harga.Value * r.TotalCarat;
                                st.EfektifOld = r.Efektif;
                                st.Efektif = prc.ApprovalTgl.Value;
                                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                                {
                                    st.SettingOld = r.Setting;
                                    st.Setting = OngkosPB;
                                    db.SubmitChanges();
                                }
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                st.HargaSatuanMOld = r.HargaSatuanM;
                                st.HargaSatuanM = cost.Harga.Value;
                                st.HargaTotalMOld = r.HargaTotalM;
                                st.HargaSatuanM = cost.Harga.Value * r.TotalCarat;

                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                //Harga
                                r.HargaSatuan = prc.Harga.Value;
                                r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                                //Tgl. Efektif
                                r.Efektif = prc.ApprovalTgl.Value;
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                                db.SubmitChanges();
                            }



                            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                            {
                                //Setting

                                r.Setting = OngkosPB;

                                Setting += OngkosPB;
                                db.SubmitChanges();
                            }

                            //Commit
                            db.SubmitChanges();
                        }
                    }
                }
            }
        }
    }
    protected void saveStone4(StockProductDJ d, int ProdID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(4);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone4 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {

            StonePricing4 prc = r.Stone4.StonePricing4;
            StoneCosting4 cost = r.Stone4.StoneCosting4;
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));

                    //var st = db.RevalLogProductDJ_Stone2.SingleOrDefault(p => p.IDForm == ProdID && p.IDStone == r.Stone4.ID);
                    var tt = from p in db.RevalLogProductDJ_Stone4
                             where p.IDForm == ProdID && p.IDStone == r.Stone4.ID
                             select p;

                    foreach (var tt_d in tt)
                    {
                        var st = db.RevalLogProductDJ_Stone4.SingleOrDefault(p => p.ID == tt_d.ID);
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                st.HargaSatuanOld = r.HargaSatuan;
                                st.HargaSatuan = prc.Harga.Value;
                                st.HargaTotalOld = r.HargaTotal;
                                st.HargaTotal = prc.Harga.Value * r.TotalCarat;
                                st.EfektifOld = r.Efektif;
                                st.Efektif = prc.ApprovalTgl.Value;
                                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                                {
                                    st.SettingOld = r.Setting;
                                    st.Setting = OngkosPB;
                                    db.SubmitChanges();
                                }
                                db.SubmitChanges();
                            }
                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                st.HargaSatuanMOld = r.HargaSatuanM;
                                st.HargaSatuanM = cost.Harga.Value;
                                st.HargaTotalMOld = r.HargaTotalM;
                                st.HargaSatuanM = cost.Harga.Value * r.TotalCarat;
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                //Harga
                                r.HargaSatuan = prc.Harga.Value;
                                r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                                //Tgl. Efektif
                                r.Efektif = prc.ApprovalTgl.Value;
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                                db.SubmitChanges();
                            }



                            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                            {
                                //Setting

                                r.Setting = OngkosPB;

                                Setting += OngkosPB;
                                db.SubmitChanges();
                            }

                            //Commit
                            db.SubmitChanges();
                        }
                    }
                }
            }
        }
    }
    protected void saveStone5(StockProductDJ d, int ProdID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(5);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone5 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {

            StonePricing5 prc = db.StonePricing5.SingleOrDefault(p => p.ID == r.IDStone);
            StoneCosting5 cost = db.StoneCosting5.SingleOrDefault(p => p.ID == r.IDStone);
            if (prc == null || cost == null) { }
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                else
                {
                    try
                    {
                        OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    }
                    catch (Exception ex) { }

                    //var st = db.RevalLogProductDJ_Stone2.SingleOrDefault(p => p.IDForm == ProdID && p.IDStone == r.Stone5.ID);
                    var tt = from p in db.RevalLogProductDJ_Stone5
                             where p.IDForm == ProdID && p.IDStone == r.Stone5.ID
                             select p;

                    foreach (var tt_d in tt)
                    {
                        var st = db.RevalLogProductDJ_Stone5.SingleOrDefault(p => p.ID == tt_d.ID);
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                st.HargaSatuanOld = r.HargaSatuan;
                                st.HargaSatuan = prc.Harga.Value;
                                st.HargaTotalOld = r.HargaTotal;
                                st.HargaTotal = prc.Harga.Value * r.TotalCarat;
                                st.EfektifOld = r.Efektif;
                                st.Efektif = prc.ApprovalTgl.Value;
                                if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                                {
                                    st.SettingOld = r.Setting;
                                    st.Setting = OngkosPB;
                                    db.SubmitChanges();
                                }
                                db.SubmitChanges();
                            }
                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                st.HargaSatuanMOld = r.HargaSatuanM;
                                st.HargaSatuanM = cost.Harga.Value;
                                st.HargaTotalMOld = r.HargaTotalM;
                                st.HargaSatuanM = cost.Harga.Value * r.TotalCarat;

                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                            {
                                //Harga
                                r.HargaSatuan = prc.Harga.Value;
                                r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                                //Tgl. Efektif
                                r.Efektif = prc.ApprovalTgl.Value;
                                db.SubmitChanges();
                            }

                            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                            {
                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                                db.SubmitChanges();
                            }

                            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                            {
                                //Setting

                                r.Setting = OngkosPB;

                                Setting += OngkosPB;
                                db.SubmitChanges();
                            }

                            //Commit
                            db.SubmitChanges();
                        }
                    }
                }
            }
        }
    }

    //Finishing
    protected void saveFinishing(StockProductDJ d, StockProductDJ_CharProduct product, int ProdID)
    {
        var rs = from p in d.StockProductDJ_Finishing select p;
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
                var fn = db.RevalLogProductDJ_Finishing.SingleOrDefault(p => p.IDForm == ProdID && p.IDFinishing == r.IDFinishing);

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
    protected void saveProduct(StockProductDJ d, StockProductDJ_CharProduct product, int ProdID)
    {
        string sebab = "";
        bool ok = true;

        //Pricing

        int IDTG_Costing = CV.TG_PG1_Costing;
        int IDTG_Pricing = CV.TG_PG1_Pricing;
        if (d.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode == "AU")
        {
            IDTG_Costing = CV.TG_PG1_Costing;
            IDTG_Pricing = CV.TG_PG1_Pricing;
        }
        else if (d.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode == "PL")
        {
            IDTG_Costing = CV.TG_PG_Paladium;
            IDTG_Pricing = CV.TG_PG_Paladium2;
        }
        else if (d.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode == "PT")
        {
            IDTG_Costing = CV.TG_PG_Platina;
            IDTG_Pricing = CV.TG_PG_Platina2;
        }
        else if (d.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode == "SIL")
        {
            IDTG_Costing = CV.TG_PG_Silver;
            IDTG_Pricing = CV.TG_PG_Silver2;
        }

        var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == IDTG_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == IDTG_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_tg == null || prc_tgM == null) { ok = false; sebab = "Today Gold Price"; }

        //Process
        if (!ok) { }
        else
        {
            var pr = db.RevalLogProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ProdID);

            //StockProductDJ_PricingProduct n = new StockProductDJ_PricingProduct();

            var n = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == d.ID);

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
            //db.StockProductDJ_PricingProduct.InsertOnSubmit(n);
            db.SubmitChanges();
            //}

            //Costing
            //StockProductDJ_CostingProduct m = new StockProductDJ_CostingProduct();

            var cs = db.RevalLogProductDJ_CostingProduct.SingleOrDefault(p => p.ID == ProdID);

            var m = db.StockProductDJ_CostingProduct.SingleOrDefault(p => p.ID == d.ID);

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
                //cs.Total = (prc_tgM.Harga.Value * n.Netto * (n.Kadar / (decimal)100)) + Convert.ToDecimal(TotalMStone1A(d)) + Convert.ToDecimal(TotalMStone1B(d)) + Convert.ToDecimal(TotalMStone2(d)) + Convert.ToDecimal(TotalMStone3(d)) + Convert.ToDecimal(TotalMStone4(d)) + Convert.ToDecimal(TotalMStone5(d));
                cs.Total = m.TotalProduct + Convert.ToDecimal(m.TotalStone1A) + Convert.ToDecimal(m.TotalStone1B) + Convert.ToDecimal(m.TotalStone2) + Convert.ToDecimal(m.TotalStone3) + Convert.ToDecimal(m.TotalStone4) + Convert.ToDecimal(m.TotalStone5);


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
                //db.StockProductDJ_CostingProduct.InsertOnSubmit(m);
                db.SubmitChanges();
            }
        }
    }

    //Biaya
    protected void saveBiaya(StockProductDJ d, StockProductDJ_CharProduct product, int ProdID)
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
            //StockProductDJ_PricingBiaya n = new StockProductDJ_PricingBiaya();

            var by = db.RevalLogProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ProdID);

            var n = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == d.ID);

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

            //Tgl. Efektif

            if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_Rate == 1)
            {
                n.TotalRate = prc_us.Harga.Value;
                n.EfektifRate = prc_us.ApprovalTgl.Value;
                db.SubmitChanges();
            }

            n.Total = n.TotalRupiah / n.TotalRate;



            //Commit
            //db.StockProductDJ_PricingBiaya.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected int FS_ID(StockProductDJ d, StockProductDJ_CharProduct product)
    {
        return 1; //Masih di-hardcoded. BELUM SELESAI !
    }

    //Mark-Up
    protected void saveMU(StockProductDJ d, StockProductDJ_CharProduct product, int ProdID)
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
            //StockProductDJ_PricingMU n = new StockProductDJ_PricingMU();

            var n = db.StockProductDJ_PricingMU.SingleOrDefault(p => p.ID == d.ID);

            var mu = db.RevalLogProductDJ_PricingMU.SingleOrDefault(p => p.ID == ProdID);

            var revmu = db.RevalProductDJ_PricingMU.SingleOrDefault(p => p.ID == ProdID);


            decimal OngkosLain = 0m;

            try
            {
                OngkosLain = d.StockProductDJ_Costing.OngkosLain.Value;
            }
            catch (Exception ex)
            {
                OngkosLain = 0m;
            }

            decimal rate = prc_us.Harga.Value;
            DateTime efektif_rate = prc_us.ApprovalTgl.Value;
            if(product.ProductCategory == 1 || product.ProductCategory == 8)
            {
                rate = Db.SingleDecimal("SELECT Harga From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
                efektif_rate = Db.SingleTime("SELECT ApprovalTgl From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
            }

            if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
            {
                mu.ModalUSDOld = n.ModalUSD;
                mu.ModalUSD = TotalModal(d.ID);
                mu.ModalRupiahOld = n.ModalRupiah;
                mu.ModalRupiah = TotalModal(d.ID) * rate;
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
                mu.ModalRate = rate;
                mu.EfektifRateOld = n.EfektifRate;
                mu.EfektifRate = efektif_rate;
                mu.ModalRupiahOld = n.ModalRupiah;
                mu.ModalRupiah = n.ModalUSD * rate;
                mu.HargaRupiahOld = n.HargaRupiah;
                mu.HargaRupiah = mu.HargaUSD * rate;
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

            if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
            {
                //Response.Redirect(mu.MU.ToString());
                //Response.End();
                //Response.Redirect((price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID)) * n.ModalRate + OngkosLain)).ToString());
                //Response.End();
                mu.HargaUSDOld = n.HargaUSD;
                //mu.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (mu.MU / (decimal)100)) + TotalStone1B(d.ID) + (OngkosLain / d.StockProductDJ_Costing.TotalRate.Value);
                mu.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (mu.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone2(d) + TotalStone3NMU(d.ID) + TotalStone4(d) + TotalStone5(d) + (OngkosLain / d.StockProductDJ_Costing.TotalRate.Value);
                mu.HargaRupiahOld = n.HargaRupiah;
                mu.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (mu.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone2(d) + TotalStone3NMU(d.ID) + TotalStone4(d) + TotalStone5(d)) * mu.ModalRate + OngkosLain;
                mu.HargaRupiahRoundOld = n.HargaRupiahRound;
                mu.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (mu.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone2(d) + TotalStone3NMU(d.ID) + TotalStone4(d) + TotalStone5(d)) * mu.ModalRate + OngkosLain);
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
                n.ModalRate = rate;
                n.ModalRupiah = n.ModalUSD * n.ModalRate;
                n.HargaRupiah = n.HargaUSD * n.ModalRate;
                n.HargaRupiahRound = price.RoundTop(n.HargaRupiah);
                //Tgl. Efektif
                n.EfektifRate = efektif_rate;
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
                //n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + (OngkosLain / d.StockProductDJ_Costing.TotalRate.Value);

                /// APPLY : Harga Final
                n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d) + (OngkosLain / d.StockProductDJ_Costing.TotalRate.Value);
                n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * n.ModalRate + OngkosLain;
                n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * n.ModalRate + OngkosLain);

                /// APPLY : Harga Reval Rate saja
                //n.HargaUSD = revmu.HargaUSD;
                //n.HargaRupiah = n.HargaUSD * n.ModalRate;
                //n.HargaRupiahRound = price.RoundTop(n.HargaRupiah);

                db.SubmitChanges();
            }



            //Commit
            //db.StockProductDJ_PricingMU.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }

    //Segmen System
    protected void saveSegmen(StockProductDJ d, StockProductDJ_CharProduct product, int ProdID)
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
            try
            {
                carat = (from p in db.StockProductDJ_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                        (from p in db.StockProductDJ_Stone1B where p.IDForm == d.ID && p.StatusTerpasang == 1 select new { Carat = p.CaratButir }).Union
                        (from p in db.StockProductDJ_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                        (from p in db.StockProductDJ_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                        (from p in db.StockProductDJ_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                        (from p in db.StockProductDJ_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
            }
            catch (Exception ex) { }

            //Parameter
            int IDSegmen = 1;
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

            var n = db.StockProductDJ_PricingSegmen.SingleOrDefault(p => p.ID == d.ID);

            var sg = db.RevalLogProductDJ_PricingSegmen.SingleOrDefault(p => p.ID == ProdID);

            //StockProductDJ_PricingSegmen n = new StockProductDJ_PricingSegmen();

            //n.ID = d.ID;

            sg.SegmenSistemOld = n.SegmenSistem;
            sg.SegmenSistem = SegmenSistem;
            sg.SegmenMinimumOld = n.SegmenMinimum;
            sg.SegmenMinimum = SegmenMinimum;
            sg.HargaUSDOld = n.HargaUSD;
            sg.HargaUSD = harga;
            sg.EfektifSegmenOld = n.EfektifSegmen;
            sg.EfektifSegmen = EfektifSegmen;

            try
            {
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
            }
            catch { }

            ///Commit
            //db.StockProductDJ_PricingSegmen.InsertOnSubmit(n);
            db.SubmitChanges();

        }
    }
    protected bool checkSegmen(StockProductDJ_CharProduct product)
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
    protected decimal TotalStone(StockProductDJ d)
    {
        decimal total = 0;

        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SK == 1)
        {
            var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1A) total += r;
        }

        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1)
        {
            var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
            foreach (var r in rs_1B) total += r;
        }

        var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    //Sub-Total
    protected decimal TotalStone1A(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected decimal TotalStone1B(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected decimal TotalStone2(StockProductDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected decimal TotalStone3(StockProductDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected decimal TotalStone4(StockProductDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected decimal TotalStone5(StockProductDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    protected decimal TotalMStone1A(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected decimal TotalMStone1B(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotalM.HasValue && p.StatusTerpasang == 1 select p.HargaTotalM.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected decimal TotalMStone2(StockProductDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected decimal TotalMStone3(StockProductDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected decimal TotalMStone4(StockProductDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected decimal TotalMStone5(StockProductDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    protected decimal TotalFinishing(StockProductDJ d)
    {
        decimal total = 0;

        var rs = from p in d.StockProductDJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }
    protected decimal TotalModal(int ID)
    {
        decimal total = 0;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
        total += biaya.Total;

        return total;
    }
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        var stone3 = from p in db.StockProductDJ_Stone3
                     where p.IDForm == ID && (p.GIA == "" || p.GIA == null)
                     select p;
        decimal Stone3ToMarkUp = 0m;
        try
        {
            Stone3ToMarkUp = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(Stone3ToMarkUp);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
    protected decimal TotalStone3NMU(int ID)
    {
        decimal total = 0;

        var stone3 = from p in db.StockProductDJ_Stone3
                     where p.IDForm == ID && p.GIA != ""
                     select p;
        try
        {
            total = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal HargaUSD(int ID)
    {
        var mu = db.StockProductDJ_PricingMU.SingleOrDefault(p => p.ID == ID);
        return mu.HargaUSD;
    }
    protected void saveTotalStone(StockProductDJ d, int ProdID)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A select p.TotalButir;
        foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.StockProductDJ_Stone1B where p.StatusTerpasang == 1 select p.TotalButir;
        foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.StockProductDJ_Stone2 select p.TotalButir;
        foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.StockProductDJ_Stone3 select p.TotalButir;
        foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.StockProductDJ_Stone4 select p.TotalButir;
        foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.StockProductDJ_Stone5 select p.TotalButir;
        foreach (var r in rs_5) total += r;

        d.StockProductDJ_CharProduct.StoneQty = total;
        db.SubmitChanges();

    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 0 || Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            string t = "All";
            string v = "0";
            ListItem item = new ListItem(t, v);
            lokasi.Items.Add(item);
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }

    protected void ApplyAll(object sender, EventArgs e)
    {
        string LocationType = tipelokasi.SelectedValue == "-1" ? "9" : tipelokasi.SelectedValue;
        string IDLokasi = lokasi.SelectedValue == "-1" ? "0" : lokasi.SelectedValue;
        string Clarity = parcel104a.SelectedValue == "0" ? "''" : parcel104a.Text;
        Db.Execute("sp_reval_apply 'DJ', '0', '" + kw.Text + "', " + LocationType + ", " + IDLokasi + ", " + productitem.SelectedValue + ", " + productcategory.SelectedValue + ", " + productlevel.SelectedValue + ", 0, " + Clarity + ", " + orderby.SelectedValue + ", 0, 1, '" + App.UserNama +"'", Db.Cnn_DB);

        refreshPage(sender, e);
    }
	
	protected void exportExcel(object sender, EventArgs e)
	{
		string href = "DJ_List_V2_Export.aspx?";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        /*href += "&reval_m=" + reval_M.Checked.ToString();
        href += "&reval_r=" + reval_R.Checked.ToString();
        href += "&reval_g=" + reval_G.Checked.ToString();
        href += "&reval_sk=" + reval_SK.Checked.ToString();
        href += "&reval_sb=" + reval_SB.Checked.ToString();
        href += "&reval_mf=" + reval_MF.Checked.ToString();
        href += "&reval_mu=" + reval_MU.Checked.ToString();
        href += "&reval_rate=" + reval_Rate.Checked.ToString();*/
        href += "&parcel104a=" + parcel104a.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Write("<script>window.open('" + href + "');</script>");
	}
}