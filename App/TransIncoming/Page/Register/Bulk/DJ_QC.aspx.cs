using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class DJ_QC : System.Web.UI.Page
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
        
        var rsH = d.StockIncomingDJ.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingDJ.Nomor;
        brand.InnerText = rsH.StockOutgoingDJ.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingDJ.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingDJ.TipeTujuan);
        from.InnerText = rsH.StockOutgoingDJ.NamaAsal;
        to.InnerText = rsH.StockOutgoingDJ.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.StockOutgoingDJ.Tgl);
        kurir.InnerText = rsH.StockOutgoingDJ.NamaKurir;
        keterangan.InnerText = rsH.StockOutgoingDJ.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingDJ.TglETA);

        var item = from p in d.StockIncomingDJ_Product where p.IDForm == id select p;

        var group_product_item = from p in item
                                 group p by new { p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama } into s
                                 select new
                                 {
                                     ProductItem = s.Key.Nama,
                                     Qty = s.Count()
                                 };

        totalqty.InnerText = item.Count().ToString();

        totalgross.InnerText = Str.Number2(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight));
        totalnetto.InnerText = Str.Number2(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight));
        totalbutir.InnerText = Str.Number0(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty));
        totalcarat.InnerText = Str.Number3(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat));
        product_item_summary.InnerText = "";

        foreach (var rsItem in group_product_item)
        {
            product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }

        // Product Status Regroup DJ
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

        var item = from p in d.StockIncomingDJ_Product where p.IDForm == id select p;

        if (!reverse)
        {
            foreach (var rs in item)
            {
                string v = rs.StockProductDJ.ID.ToString();
                string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductDJ.Nomor,
                        rs.StockProductDJ.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
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
                    var rs = d.StockIncomingDJ_Product.SingleOrDefault(p => p.StockProductDJ.ID == Convert.ToInt32(cbl_Product.Items[i].Value) && p.StockIncomingDJ.ID == Convert.ToInt32(id));

                    string v = rs.StockProductDJ.ID.ToString();
                    string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductDJ.Nomor,
                        rs.StockProductDJ.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                        );
                    cbl_Product_2.Items.Add(new ListItem(t, v));
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
                var rs = d.StockIncomingDJ_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product_2.Items[i].Value) && p.StockIncomingDJ.ID == Convert.ToInt32(id));

                string v = rs.StockProductDJ.ID.ToString();
                string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductDJ.Nomor,
                        rs.StockProductDJ.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
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
        //StockIncomingDJ nH = new StockIncomingDJ();
        StockIncomingDJ_Product n = new StockIncomingDJ_Product();

        //var rsH = d.StockIncomingDJ.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 0);
        //rsH.StatusTransIn = 1;

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = true;

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected == true)
            {
                var rs = d.StockIncomingDJ_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value) && p.StockIncomingDJ.ID == Convert.ToInt32(id));
                rs.SubStorageResult = rs.SubStorageTo;
            }
        }
        for (int i = 0; i < cbl_Product_2.Items.Count; i++)
        {
            if (cbl_Product_2.Items[i].Selected == true)
            {
                var rs = d.StockIncomingDJ_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product_2.Items[i].Value) && p.StockIncomingDJ.ID == Convert.ToInt32(id));
                rs.SubStorageResult = 96; // 96 : broken
            }
        }
        for (int i = 0; i < cbl_Product_3.Items.Count; i++)
        {
            if (cbl_Product_3.Items[i].Selected == true)
            {
                var rs = d.StockIncomingDJ_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product_3.Items[i].Value) && p.StockIncomingDJ.ID == Convert.ToInt32(id));
                rs.SubStorageResult = 99; // 99 : lost / trouble
            }
        }
        //var rs = from p in d.StockIncomingDJ_Product where p.IDForm == id && p.StockActualDJ.Substorage == 98 select p;

        //foreach (var item in rs)
        //{
        //    item.StockActualDJ.Substorage = 98;
        //}

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("DJ_Finish.aspx?id=" + id);
    }
}