using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Product_Finish : System.Web.UI.Page
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

        var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id);
        //brand.InnerText = rsH.Company.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = from p in d.StockOutgoingSouvenir_Product where p.IDForm == id select p;

        foreach (var rs in item)
        {
            string v = rs.Souvenir.ID.ToString();
            string t = Logistic.ProductList_Souvenir(rs.Souvenir.Kode,
                        Convert.ToDecimal(rs.Qty).ToString("N0"));
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
        var rs1 = d.Souvenir.SingleOrDefault(p => p.Kode == "" + input + "");

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
        cbl_Product.Enabled = true;
        StockOutgoingSouvenir nH = new StockOutgoingSouvenir();
        StockOutgoingSouvenir_Product n = new StockOutgoingSouvenir_Product();

        var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 5);
        rsH.StatusTransOut = 6;

        /*var rs = from p in d.StockOutgoingSouvenir_Product where p.IDForm == id && p.StockActual.Substorage == 97 select p;

        foreach (var item in rs)
        {
            item.StockActual.Substorage = 98;
        }*/

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("ProductBroken.aspx?d=1");
        
    }
}