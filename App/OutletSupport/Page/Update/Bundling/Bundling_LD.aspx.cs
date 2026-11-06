using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_ProductAddr : System.Web.UI.Page
{
    private Map_DB _db;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //Form
            fillForm();
            fillProduct();
        }
    }

    //Exception
    protected void DataError()
    {
        Response.Redirect("Home.aspx");
    }

    //Form
    protected void fillForm()
    {
        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID && p.Draft == true);
        if (d == null) DataError();
    }

    protected void fillProduct()
    {
        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID && p.Draft == true);
        var csel = from p in db.StockProductLD_Stone1B
                   where p.StatusPenjualan == 0 && p.ReturnProduct == false
                   select p;

        csel = from p in csel
               where !(from z in db.Bundle_ItemPasangan
                       where z.BundleItem.TglStart >= d.TglStart && z.BundleItem.TglExpire < d.TglExpire
                       && z.BundleItem.ApprovalNama != null
                       select z.IDProduct).Contains(p.ID)
               select p;

        foreach (var r in csel)
        {
            string t = r.Nomor;
            string v = r.ID.ToString();
            prod.Items.Add(new ListItem(t, v));
        }

        var rs = from p in db.Bundle_ItemPasangan
                 where p.IDForm == myID && p.TipeProduct == 4
                 select p;

        foreach (var r in rs)
        {
            string n = r.IDProduct.ToString();
            if (rs.Count() != 0) prod.Items.FindByValue(n).Selected = true;
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);

        var products = from p in db.StockProductLD_Stone1B
                       where p.StatusPenjualan == 0 && p.ReturnProduct == false
                       select p;

        var rs = from p in db.Bundle_ItemPasangan
                 where p.IDForm == myID && p.TipeProduct == 4
                 select p;

        db.Bundle_ItemPasangan.DeleteAllOnSubmit(rs);
        db.SubmitChanges();

        //int[] loc = new int[100];

        if (all.Checked)
        {
            foreach (var b in products)
            {
                d.Draft = false;
                db.SubmitChanges();

                Bundle_ItemPasangan n = new Bundle_ItemPasangan();

                n.IDForm = d.ID;
                n.IDProduct = b.ID;

                n.TipeProduct = 4;

                db.Bundle_ItemPasangan.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
        else
        {
            int idproduct = 0;
            for (int a = 0; a < prod.Items.Count; a++)
            {
                if (prod.Items[a].Selected)
                    idproduct = Convert.ToInt32(prod.Items[a].Value);
                else
                    idproduct = 0;
                if (idproduct != 0)
                {
                    d.Draft = false;
                    db.SubmitChanges();

                    var product = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == idproduct);

                    Bundle_ItemPasangan n = new Bundle_ItemPasangan();

                    n.IDForm = d.ID;
                    n.IDProduct = product.ID;

                    n.TipeProduct = 4;

                    db.Bundle_ItemPasangan.InsertOnSubmit(n);
                    db.SubmitChanges();
                }
            }
        }

        d.Draft = false;
        db.SubmitChanges();

        Response.Redirect("Home.aspx?d=1");
    }

    protected void all_CheckedChanged(object sender, EventArgs e)
    {
        if (all.Checked == true)
            prod.Enabled = false;
        else
            prod.Enabled = true;
    }
}

