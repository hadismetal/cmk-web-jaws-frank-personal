using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class DJ_Finish : System.Web.UI.Page
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
            
        }
    }
    //Fill
    protected void Fill()
    {
        
        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.Nomor;
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        print.Visible = (rsH.StatusTransOut == 2) ? true : false;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

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
                                   group p by new { p.SubStorage } into s
                                   select new
                                   {
                                       SubStorage = s.Key.SubStorage,
                                       Qty = s.Count()
                                   };
        foreach (var rsItem in group_product_status)
        {
            product_status_summary.InnerHtml += DL.SubStorage(rsItem.SubStorage) + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }
        // END

        foreach (var rs in item)
        {
            string v = rs.StockProductDJ.ID.ToString();
            string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductDJ.Nomor,
                        rs.StockProductDJ.ImgPicture,
                        DL.SubStorage(rs.SubStorage),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                        );
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
        }
        cbl_Product.Enabled = false;
    }
    
    //Save
    
    protected void back_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("SummaryDJ.aspx");
        
    }

    protected void print_Click(object sender, EventArgs e)
    {
        
        //Redirect
        Response.Redirect("DJ_Print.aspx?id=" + id);
    }
}