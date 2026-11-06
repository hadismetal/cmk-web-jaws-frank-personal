using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PG_QC : System.Web.UI.Page
{
    private Map_AC _db;

    private Map_AC db
    {
        get
        {
            if (_db == null)
                _db = new Map_AC();
            return _db;
        }
    }

    private Map_DB _d;

    private Map_DB d
    {
        get
        {
            if (_d == null)
                _d = new Map_DB();
            return _d;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (d != null)
            d.Dispose();
        base.Dispose();
    }

    protected int id { get { return App.QueryInt(this); } } /// id trans out

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            //Form-Default
            Fill();
            Fill1(false);
        }
    }
    //Fill
    protected void Fill()
    {
        
        var rsH = d.StockIncomingPG.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingPG.Nomor;
        brand.InnerText = rsH.StockOutgoingPG.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingPG.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingPG.TipeTujuan);
        from.InnerText = rsH.StockOutgoingPG.NamaAsal;
        to.InnerText = rsH.StockOutgoingPG.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.StockOutgoingPG.Tgl);
        kurir.InnerText = rsH.StockOutgoingPG.NamaKurir;
        keterangan.InnerText = rsH.StockOutgoingPG.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingPG.TglETA);

        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;

        var group_product_item = from p in item
                                 group p by new { p.StockProductPG.CharProductItem.Nama } into s
                                 select new
                                 {
                                     ProductItem = s.Key.Nama,
                                     Qty = s.Count()
                                 };

        totalqty.InnerText = item.Count().ToString();

        totalgross.InnerText = Str.Number2(item.Sum(p => p.StockProductPG.GrossWeight));
        //totalnetto.InnerText = Str.Number2(item.Sum(p => p.StockProductPG.NettoWeight));
        //totalbutir.InnerText = "";// CalculateTotalButir(id).ToString();
        //totalcarat.InnerText = item.Sum(p => p.StockProductPG.StockProductPG_CharProduct.StoneCarat).ToString();
        product_item_summary.InnerText = "";

        foreach (var rsItem in group_product_item)
        {
            product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }

        // Product Status Regroup PG
        var group_product_status = from p in item
                                   group p by new { p.SubStorageTo } into s
                                   select new
                                   {
                                       SubStorage = s.Key.SubStorageTo,
                                       Qty = s.Count()
                                   };
        foreach (var rsItem in group_product_status)
        {
            product_status_summary.InnerHtml += DL.SubStorage(rsItem.SubStorage) + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }
        // END
        // Product Model Regroup PG
        var group_product_model = from p in item
                                  group p by new { p.StockProductPG.CharGoldModel.Nama } into s
                                  select new
                                  {
                                      Model = s.Key.Nama,
                                      Qty = s.Count()
                                  };
        foreach (var rsItem in group_product_model)
        {
            product_model_summary.InnerHtml += rsItem.Model + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }
        // END
    }
    protected void Fill1(bool reverse)
    {
        next2.Visible = true; // 1
        back1.Visible = false; // 2
        next3.Visible = false; // 2
        back2.Visible = false; // 3
        nextF.Visible = false; // 3

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = true;

        if (!reverse) { cbl_Product.Items.Clear(); }
        cbl_Product_2.Items.Clear();
        cbl_Product_3.Items.Clear();

        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;

        if (!reverse)
        {
            foreach (var rs in item)
            {
                string v = rs.StockProductPG.ID.ToString();
                string t = Logistic.ProductList_PG(
                        rs.StockProductPG.CharProductItem.Nama,
                        rs.StockProductPG.Nomor,
                        rs.StockProductPG.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductPG.NettoWeight),
                        Str.Number2(rs.StockProductPG.GrossWeight),
                        rs.StockProductPG.CharGoldModel.Nama
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                //cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true; // default harus manual check
            }
        }

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = false;
        cbl_Product_3.Enabled = false;

        Calculate_QC_Summary();
    }
    protected void Fill2CheckSkip(bool reverse)
    {
        // logic untuk skip Fill2 ke Fill3 jika Fill1 tercentang semua
        int flag = 0;
        for (int i = 0; i < cbl_Product.Items.Count; i++) { if (cbl_Product.Items[i].Selected == true) { flag++; } }

        if (!reverse)
        {
            if (flag == cbl_Product.Items.Count) { Fill3(); } else { Fill2(reverse); }
        }
        else
        {
            if (flag == cbl_Product.Items.Count) { Fill1(reverse); } else { Fill2(reverse); }
        }
    }
    protected void Fill2(bool reverse)
    {
        next2.Visible = false; // 1
        back1.Visible = true; // 2
        next3.Visible = true; // 2
        back2.Visible = false; // 3
        nextF.Visible = false; // 3

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = true;

        //cbl_Product.Items.Clear();
        if (!reverse) { cbl_Product_2.Items.Clear(); }
        cbl_Product_3.Items.Clear();

        if (!reverse)
        {
            for (int i = 0; i < cbl_Product.Items.Count; i++)
            {
                if (cbl_Product.Items[i].Selected == false)
                {
                    var rs = d.StockIncomingPG_Product.SingleOrDefault(p => p.StockProductPG.ID == Convert.ToInt32(cbl_Product.Items[i].Value) && p.StockIncomingPG.ID == Convert.ToInt32(id));

                    string v = rs.StockProductPG.ID.ToString();
                    string t = Logistic.ProductList_PG(
                        rs.StockProductPG.CharProductItem.Nama,
                        rs.StockProductPG.Nomor,
                        rs.StockProductPG.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductPG.NettoWeight),
                        Str.Number2(rs.StockProductPG.GrossWeight),
                        rs.StockProductPG.CharGoldModel.Nama
                        );
                    cbl_Product_2.Items.Add(new ListItem(t, v));
                    cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                    //cbl_Product.Items[i].Selected = true; // default harus manual check
                }
            }
        }

        cbl_Product.Enabled = false;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = false;

        Calculate_QC_Summary();
    }
    protected void Fill3()
    {
        next2.Visible = false; // 1
        back1.Visible = false; // 2
        next3.Visible = false; // 2
        back2.Visible = true; // 3
        nextF.Visible = true; // 3

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = true;

        //cbl_Product.Items.Clear();
        //cbl_Product_2.Items.Clear();
        cbl_Product_3.Items.Clear();

        for (int i = 0; i < cbl_Product_2.Items.Count; i++)
        {
            if (cbl_Product_2.Items[i].Selected == false)
            {
                var rs = d.StockIncomingPG_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product_2.Items[i].Value) && p.StockIncomingPG.ID == Convert.ToInt32(id));

                string v = rs.StockProductPG.ID.ToString();
                string t = Logistic.ProductList_PG(
                        rs.StockProductPG.CharProductItem.Nama,
                        rs.StockProductPG.Nomor,
                        rs.StockProductPG.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductPG.NettoWeight),
                        Str.Number2(rs.StockProductPG.GrossWeight),
                        rs.StockProductPG.CharGoldModel.Nama
                        );
                cbl_Product_3.Items.Add(new ListItem(t, v));
                
            }
        }
        for (int i = 0; i < cbl_Product_3.Items.Count; i++)
        {
            cbl_Product_3.Items[i].Selected = true; // default auto-checked karena last step
        }

        cbl_Product.Enabled = false;
        cbl_Product_2.Enabled = false;
        cbl_Product_3.Enabled = false;

        Calculate_QC_Summary();
    }
    protected void Calculate_QC_Summary()
    {
        bool flag_cbl1 = cbl_Product.Enabled;
        bool flag_cbl2 = cbl_Product_2.Enabled;
        bool flag_cbl3 = cbl_Product_3.Enabled;

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = true;

        int cbl1 = 0;
        int cbl2 = 0;
        int cbl3 = 0;

        for (int i = 0; i < cbl_Product.Items.Count; i++) { if (cbl_Product.Items[i].Selected == true) cbl1++; }
        for (int i = 0; i < cbl_Product_2.Items.Count; i++) { if (cbl_Product_2.Items[i].Selected == true) cbl2++; }
        for (int i = 0; i < cbl_Product_3.Items.Count; i++) { if (cbl_Product_3.Items[i].Selected == true) cbl3++; }
        
        accepted.InnerText = cbl1.ToString();
        broken.InnerText = cbl2.ToString();
        troublelost.InnerText = cbl3.ToString();

        cbl_Product.Enabled = flag_cbl1;
        cbl_Product_2.Enabled = flag_cbl2;
        cbl_Product_3.Enabled = flag_cbl3;
    }
    protected void next2_Click(object sender, EventArgs e)
    {
        //Fill2(false);
        Fill2CheckSkip(false);
    }

    protected void back1_Click(object sender, EventArgs e)
    {
        Fill1(true);
    }

    protected void next3_Click(object sender, EventArgs e)
    {
        Fill3();
    }

    protected void back2_Click(object sender, EventArgs e)
    {
        //Fill2(true);
        Fill2CheckSkip(true);
    }

    // save !!!
    protected void nextF_Click(object sender, EventArgs e)
    {
        //StockIncomingPG nH = new StockIncomingPG();
        StockIncomingPG_Product n = new StockIncomingPG_Product();

        //var rsH = d.StockIncomingPG.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 0);
        //rsH.StatusTransIn = 1;

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = true;

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected == true)
            {
                var rs = d.StockIncomingPG_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value) && p.StockIncomingPG.ID == Convert.ToInt32(id));
                rs.SubStorageResult = rs.SubStorageTo;
            }
        }
        for (int i = 0; i < cbl_Product_2.Items.Count; i++)
        {
            if (cbl_Product_2.Items[i].Selected == true)
            {
                var rs = d.StockIncomingPG_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product_2.Items[i].Value) && p.StockIncomingPG.ID == Convert.ToInt32(id));
                rs.SubStorageResult = 96; // 96 : broken
            }
        }
        for (int i = 0; i < cbl_Product_3.Items.Count; i++)
        {
            if (cbl_Product_3.Items[i].Selected == true)
            {
                var rs = d.StockIncomingPG_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product_3.Items[i].Value) && p.StockIncomingPG.ID == Convert.ToInt32(id));
                rs.SubStorageResult = 99; // 99 : lost / trouble
            }
        }
        //var rs = from p in d.StockIncomingPG_Product where p.IDForm == id && p.StockActualPG.Substorage == 98 select p;

        //foreach (var item in rs)
        //{
        //    item.StockActualPG.Substorage = 98;
        //}

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("PG_Finish.aspx?id=" + id);
    }
}