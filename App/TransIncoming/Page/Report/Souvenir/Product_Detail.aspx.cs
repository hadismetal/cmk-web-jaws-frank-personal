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

        var rsH = d.StockIncomingSouvenir.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.Nomor;
        //brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingSouvenir.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingSouvenir.TipeTujuan);
        from.InnerText = rsH.StockOutgoingSouvenir.NamaAsal;
        to.InnerText = rsH.StockOutgoingSouvenir.NamaTujuan;

        print.Visible = (rsH.StatusTransIn == 2) ? true : false;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.StockOutgoingSouvenir.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingSouvenir.TglETA);

        var item = from p in d.StockIncomingSouvenir_Product where p.IDForm == id select p;


        foreach (var rs in item)
        {
            string v = rs.Souvenir.ID.ToString();
            string t = Logistic.ProductList_Souvenir(
                        rs.Souvenir.Kode,
                        rs.Souvenir.ID.ToString(),
                        Str.Number0(rs.Qty != null ? rs.Qty.Value : 0),
                        Str.Number0(rs.Received != null ? rs.Received.Value : 0),
                        Str.Number0(rs.Damaged != null ? rs.Damaged.Value : 0),
                        Str.Number0(rs.Qty - (rs.Received + rs.Damaged)),
                        true,
                        true);
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