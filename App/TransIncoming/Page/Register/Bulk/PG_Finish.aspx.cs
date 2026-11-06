using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PG_Finish : System.Web.UI.Page
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
            cbl_Product.Enabled = true;
            cbl_Product_2.Enabled = true;
            cbl_Product_3.Enabled = true;

            cbl_Product.Items.Clear();
            cbl_Product_2.Items.Clear();
            cbl_Product_3.Items.Clear();

            //Form-Default
            Fill();
            Fill1();
            Fill2();
            Fill3();

            Calculate_QC_Summary();

            cbl_Product.Enabled = false;
            cbl_Product_2.Enabled = false;
            cbl_Product_3.Enabled = false;
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
            product_status_summary.InnerHtml += DL.SubStorage(Convert.ToInt32(rsItem.SubStorage)) + " (" + rsItem.Qty.ToString("0") + ")<br>";
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
    protected void Fill1()
    {
        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == p.SubStorageTo);

        if (item.Count() > 0)
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
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Fill2()
    {
        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == 96);

        if (item.Count() > 0)
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
                cbl_Product_2.Items.Add(new ListItem(t, v));
                cbl_Product_2.Items[cbl_Product_2.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Fill3()
    {
        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == 99);

        if (item.Count() > 0)
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
                cbl_Product_3.Items.Add(new ListItem(t, v));
                cbl_Product_3.Items[cbl_Product_3.Items.Count - 1].Selected = true;
            }
        }
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

    // save !!!
    protected void save_Click(object sender, EventArgs e)
    {
        StockIncomingPG nH = new StockIncomingPG();
        StockIncomingPG_Product n = new StockIncomingPG_Product();

        var rsH = d.StockIncomingPG.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 0);
        rsH.StatusTransIn = 1;

        var rs = from p in d.StockIncomingPG_Product where p.IDForm == id && p.StockActualPG.Substorage == 98 select p;

        foreach (var item in rs)
        {
            item.Status = 1;
        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("PG.aspx?d=1");
    }
}