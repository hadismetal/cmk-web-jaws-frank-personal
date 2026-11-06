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

        var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id);
        //brand.InnerText = rsH.CompanyBrand.Nama;
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
            string t = Logistic.ProductList_Souvenir(
                        rs.Souvenir.Kode,
                        Convert.ToDecimal(rs.Qty).ToString("N0")
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
        var rs1 = d.Souvenir.SingleOrDefault(p => p.Kode == input);

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

        string DocumentNumber = generateDocumentNumber_TransOut();

        var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 6);
        rsH.StatusTransOut = 7; // 7 : Approved
        rsH.Nomor = DocumentNumber;
        rsH.Approval = App.UserNama;
        rsH.ApprovalTgl = DateTime.Now;

        var rs = from p in d.StockOutgoingSouvenir_Product where p.IDForm == id select p;

        foreach (var item in rs)
        {
            /*var x = d.StockActualSouvenir.SingleOrDefault(p => p.IDProduct == item.IDProduct && p.TipeLokasi == item.StockOutgoingSouvenir.TipeAsal && p.IDLokasi == item.StockOutgoingSouvenir.IDAsal);
            if (x != null)
            {
                x.InHand = x.InHand - item.Qty.Value;
                x.InTransit = (x.InTransit.HasValue ? x.InTransit.Value : 0) + item.Qty.Value;
            }*/

            //item.StockActualSouvenirDJ.Substorage = 98; // Product On The Way !!
            //item.StockActualSouvenirDJ.InHand = 0; // InHand out

        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("ProductBroken.aspx?d=1");
        
    }

    //protected void reject_Click(object sender, EventArgs e)
    //{
    //    cbl_Product.Enabled = true;
    //    StockOutgoingSouvenir nH = new StockOutgoingSouvenir();
    //    StockOutgoingSouvenir_Product n = new StockOutgoingSouvenir_Product();

    //    var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 6);
    //    rsH.StatusTransOut = 8; // 8 : Reject
    //    rsH.Approval = App.UserNama;
    //    rsH.ApprovalTgl = DateTime.Now;

    //    var rs = from p in d.StockOutgoingSouvenir_Product where p.IDForm == id select p;

    //    //foreach (var item in rs)
    //    //{
    //    //    item.StockActualSouvenirDJ.Substorage = item.SubStorage; // return SubStorage Value !!!
    //    //}

    //    //Commit
    //    d.SubmitChanges();

    //    //Redirect
    //    Response.Redirect("ProductBroken.aspx?d=1");
    //}

    protected string generateDocumentNumber_TransOut()
    {
        string result = "";

        // ex : KRM/13/11/0001
        //      0123456789

        string v1 = "KRM/SB";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2,2);
        string v3 = System.DateTime.Now.Month.ToString("d2").PadLeft(2,'0');
        string v4 = "0001";

        var rs = from p in d.StockOutgoingSouvenir
                 where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3 
                 select p;

        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
}