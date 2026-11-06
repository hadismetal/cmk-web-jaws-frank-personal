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
            Fill();
            Fill1(false);
            //Form-Default
            
        }
        
    }
    //Fill
    protected void Fill()
    {
        
        var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingPackaging.Nomor;
        brand.InnerText = rsH.StockOutgoingPackaging.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingPackaging.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingPackaging.TipeTujuan);
        from.InnerText = rsH.StockOutgoingPackaging.NamaAsal;
        to.InnerText = rsH.StockOutgoingPackaging.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.StockOutgoingPackaging.Tgl);
        kurir.InnerText = rsH.StockOutgoingPackaging.NamaKurir;
        keterangan.InnerText = rsH.StockOutgoingPackaging.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingPackaging.TglETA);

        var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;

    }
    protected void Fill1(bool reverse)
    {
        if (!reverse) { cbl_Product.Items.Clear(); }

        var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;

        if (!reverse)
        {
            foreach (var rs in item)
            {
                string v = rs.Packaging.ID.ToString();
                string t = Logistic.ProductList_Packaging(
                        rs.Packaging.Kode,
                        Str.Number0(rs.Qty != null ? rs.Qty.Value : 0),
                        rs.Packaging.ID.ToString()
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true; // default harus manual check
				cbl_Product.Items[cbl_Product.Items.Count - 1].Enabled = false;
            }
        }
    }

    // save !!!
    protected void nextF_Click(object sender, EventArgs e)
    {
        //StockIncomingPackaging nH = new StockIncomingPackaging();
        StockIncomingPackaging_Product n = new StockIncomingPackaging_Product();

        //var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 0);
        //rsH.StatusTransIn = 1;

        //Redirect
        Response.Redirect("ProductBroken_Finish.aspx?id=" + id);
    }
}