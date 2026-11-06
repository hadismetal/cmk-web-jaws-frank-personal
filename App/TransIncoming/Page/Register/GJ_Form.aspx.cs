using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class GJ_Form : System.Web.UI.Page
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

        var rsH = d.StockOutgoingGJ.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = from p in d.StockOutgoingGJ_Product where p.IDForm == id select p;

        var group_product_item = from p in item
                                 group p by new { p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama } into s
                                 select new
                                 {
                                     ProductItem = s.Key.Nama,
                                     Qty = s.Count()
                                 };

        totalqty.InnerText = item.Count().ToString();

        totalgross.InnerText = Str.Number2(item.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.GrossWeight));
        totalnetto.InnerText = Str.Number2(item.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight));
        totalbutir.InnerText = Str.Number0(item.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty));
        totalcarat.InnerText = Str.Number3(item.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat));
        product_item_summary.InnerText = "";

        foreach (var rsItem in group_product_item)
        {
            product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }

        // Product Status Regroup GJ
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
            string v = rs.StockProductGJ.ID.ToString();
            string t = Logistic.ProductList_GJ(
                        rs.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductGJ.Nomor,
                        rs.StockProductGJ.ImgPicture,
                        DL.SubStorage(rs.SubStorage),
                        Str.Number2(rs.StockProductGJ.StockProductGJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductGJ.StockProductGJ_CharProduct.GrossWeight)
                        );
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
        }
        cbl_Product.Enabled = false;
    }
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductGJ.SingleOrDefault(p => p.Nomor == input);

        if (rs1 == null) { message = "Invalid Product Code"; }
        else
        {
            // 2. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
            }
        }

        // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

        return message;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        StockIncomingGJ nH = new StockIncomingGJ();

        var rs = from p in d.StockOutgoingGJ_Product where p.IDForm == id select p;

        //Auto-Input Header
        nH.IDOutgoing = id;
        nH.Nomor = null;
        nH.Tgl = System.DateTime.Now;
        nH.Keterangan = txketerangan.Text;
        nH.StatusTransIn = 0; // Draft

        //User-Info
        nH.Operator = App.UserNama;
        nH.OperatorTgl = DateTime.Now;

        //Commit
        d.StockIncomingGJ.InsertOnSubmit(nH);
        d.SubmitChanges();

        //Auto-Input List Product
        foreach (var item in rs)
        {
            StockIncomingGJ_Product n = new StockIncomingGJ_Product()
            {
                //Input
                IDForm = nH.ID,
                IDProduct = item.IDProduct,
                SubStorageTo = item.SubStorage,
                SubStorageResult = null,
                Status = 0, // New
                Operator = App.UserNama,
                OperatorTgl = DateTime.Now
            };
            d.StockIncomingGJ_Product.InsertOnSubmit(n);

        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("GJ_QC.aspx?id=" + nH.ID);
        
    }
}