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

        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.Nomor;
        //brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        print.Visible = (rsH.StatusTransOut == 2) ? true : false;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = from p in d.StockOutgoingPackaging_Product where p.IDForm == id select p;


        foreach (var rs in item)
        {
            string v = rs.Packaging.ID.ToString();
            string t = Logistic.ProductList_Packaging(
                        rs.Packaging.Nama, Convert.ToDecimal(rs.Qty).ToString("N0")
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
        Response.Redirect("Product.aspx");
        
    }

    protected void print_Click(object sender, EventArgs e)
    {
        
        //Redirect
        Response.Redirect("Product_Print.aspx?id=" + id);
    }
}