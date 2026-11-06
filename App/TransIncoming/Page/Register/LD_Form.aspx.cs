using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class LD_Form : System.Web.UI.Page
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

        var rsH = d.StockOutgoingLD.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);
        
        var item = from p in d.StockOutgoingLD_Product where p.IDForm == id select p;

        var group_product_item = from p in item
                                 group p by new { p.StockProductLD_Stone1B.Stone1B.Nama } into s
                                 select new
                                 {
                                     ProductItem = s.Key.Nama,
                                     Qty = s.Count()
                                 };

        totalqty.InnerText = item.Count().ToString();

        totalbutir.InnerText = Str.Number0(item.Sum(p => p.StockProductLD_Stone1B.TotalButir));
        totalcarat.InnerText = Str.Number3(item.Sum(p => p.StockProductLD_Stone1B.TotalCarat));
        product_item_summary.InnerText = "";

        foreach (var rsItem in group_product_item)
        {
            product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }

        // Product Status Regroup LD
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
            string v = rs.StockProductLD_Stone1B.ID.ToString();
            string t = Logistic.ProductList_LD(
                        rs.StockProductLD_Stone1B.Nomor,
                        rs.StockProductLD_Stone1B.ImgPicture,
                        DL.SubStorage(rs.SubStorage),
                        Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)
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
        var rs1 = d.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == input);

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
        StockIncomingLD nH = new StockIncomingLD();

        var rs = from p in d.StockOutgoingLD_Product where p.IDForm == id select p;

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
        d.StockIncomingLD.InsertOnSubmit(nH);
        d.SubmitChanges();

        //Auto-Input List Product
        foreach (var item in rs)
        {
            StockIncomingLD_Product n = new StockIncomingLD_Product()
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
            d.StockIncomingLD_Product.InsertOnSubmit(n);

        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("LD_QC.aspx?id=" + nH.ID);
        
    }
}