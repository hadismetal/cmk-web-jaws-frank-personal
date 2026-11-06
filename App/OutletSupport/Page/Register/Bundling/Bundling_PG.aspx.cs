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
            DLC.CharGoldModelAllFilter(model);
            DLC.CharGoldLevel(productlevel);
            DLC.CharProductItem(productitem);

            //Form
            fillForm();
            fillProduct();
        }
    }

    //Exception
    protected void DataError()
    {
        Response.Redirect("Bundling.aspx");
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
        var csel = from p in db.StockProductPG
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

            string t = r.Nomor + " ( " + r.CharProductItem.Nama + " / " + (DateTime.Today - r.Tgl.Value).TotalDays.ToString() + " hari)";
            string v = r.ID.ToString();
            prod.Items.Add(new ListItem(t, v));
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);

        int Model = Convert.ToInt32(model.SelectedValue);
        int Level = Convert.ToInt32(productlevel.SelectedValue);
        int Item = Convert.ToInt32(productitem.SelectedValue);

        var cat = from p in db.CharGoldModel
                  where ((Model > 0) ? p.ID == Model : 1 == 1)
                  select p;
        var br = from p in db.CharProductLevel
                 where ((Level > 0) ? p.ID == Level : 1 == 1)
                 select p;

        var pr = from p in db.CharProductItem
                 where ((Item > 0) ? p.ID == Item : 1 == 1)
                 select p;

        var products = from p in db.StockProductPG
                       where ((Model > 0) ? p.Model == Model : 1 == 1)
                       && ((Level > 0) ? p.ProductLevel == Level : 1 == 1)
                       && ((Item > 0) ? p.ProductItem == Item : 1 == 1)
                       && p.StatusPenjualan == 0 && p.ReturnProduct == false
                       select p;

        int[] loc = new int[100];
        if (all.Checked)
                {
                    foreach (var b in products)
                    {
                        //d.Draft = false;
                        //db.SubmitChanges();

                        Bundle_ItemPasangan n = new Bundle_ItemPasangan();

                        n.IDForm = d.ID;
                        n.IDProduct = b.ID;

                        n.TipeProduct = 3;

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
                            //d.Draft = false;
                            //db.SubmitChanges();

                            var product = db.StockProductPG.SingleOrDefault(p => p.ID == idproduct);

                            Bundle_ItemPasangan n = new Bundle_ItemPasangan();

                            n.IDForm = d.ID;
                            n.IDProduct = product.ID;

                            n.TipeProduct = 3;

                            db.Bundle_ItemPasangan.InsertOnSubmit(n);
                            db.SubmitChanges();
                        }
                    }
                }
            
        //Redirect
        Response.Redirect("Bundling_LD.aspx?id=" + myID);
    }

    protected void all_CheckedChanged(object sender, EventArgs e)
    {
        if (all.Checked == true)
            prod.Enabled = false;
        else
            prod.Enabled = true;
    }

    protected void category_SelectedIndexChanged(object sender, EventArgs e)
    {
        prod.Focus();
        prod.Items.Clear();
        int Model = Convert.ToInt32(model.SelectedValue);
        int Level = Convert.ToInt32(productlevel.SelectedValue);
        int Item = Convert.ToInt32(productitem.SelectedValue);

        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);


        var csel = from p in db.StockProductPG
                   where ((Model > 0) ? p.Model == Model : 1 == 1)
                   && ((Level > 0) ? p.ProductLevel == Level : 1 == 1)
                   && ((Item > 0) ? p.ProductItem == Item : 1 == 1)
                   && p.StatusPenjualan == 0 && p.ReturnProduct == false
                   select p;

        csel = from p in csel
               where !(from z in db.Bundle_ItemPasangan
                       where z.BundleItem.TglStart >= d.TglStart && z.BundleItem.TglExpire < d.TglExpire
                       && z.BundleItem.ApprovalNama != null
                       select z.IDProduct).Contains(p.ID)
               select p;

        foreach (var r in csel)
        {
            string t = r.Nomor + " (" + r.CharProductItem.Nama + ")";
            string v = r.ID.ToString();
            prod.Items.Add(new ListItem(t, v));
        }
    }

    protected void skip_Click(object sender, EventArgs e)
    {
        Response.Redirect("Bundling_LD.aspx?id=" + myID);
    }

}

