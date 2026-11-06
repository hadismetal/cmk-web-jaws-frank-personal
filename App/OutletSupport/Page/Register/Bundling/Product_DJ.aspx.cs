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
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductItem(productitem);

            fillProduct();

            //Form
            fillForm();
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
        var csel = from p in db.StockProductDJ
                   where p.StatusPenjualan == 0 && p.ReturnProduct == false && p.Draft.Value == false && p.Legacy == null 
                   select p;

        csel = from p in csel
               where !(from z in db.Bundle_ItemMaster
                       where z.BundleItem.TglStart >= d.TglStart && z.BundleItem.TglExpire < d.TglExpire
                       && z.BundleItem.ApprovalNama != null
                       select z.IDProduct).Contains(p.ID)
               select p;

        foreach (var r in csel)
        {
            string ProtoNomor = string.Empty;
            ProtoNomor = (r.ProtoNomor.Contains("-") ? r.ProtoNomor.Substring(0, r.ProtoNomor.IndexOf('-')) : r.ProtoNomor);
            ProtoNomor = (ProtoNomor == "") ? "-" : ProtoNomor;

            string t = r.Nomor + " (" + ProtoNomor + " / " + r.StockProductDJ_CharProduct.CharProductItem.Nama + " / " + (DateTime.Today - r.Tgl.Value).TotalDays.ToString() + " hari)";
            string v = r.ID.ToString();
            prod.Items.Add(new ListItem(t, v));
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);

        int Category = Convert.ToInt32(productcategory.SelectedValue);
        int Level = Convert.ToInt32(productlevel.SelectedValue);
        int Item = Convert.ToInt32(productitem.SelectedValue);


        var cat = from p in db.CharProductCategory
                  where ((Category > 0) ? p.ID == Category : 1 == 1)
                  select p;
        var br = from p in db.CharProductLevel
                 where ((Level > 0) ? p.ID == Level : 1 == 1)
                 select p;

        var pr = from p in db.CharProductItem
                 where ((Item > 0) ? p.ID == Item : 1 == 1)
                 select p;

        var products = from p in db.StockProductDJ
                       where ((Category > 0) ? p.StockProductDJ_CharProduct.ProductCategory == Category : 1 == 1)
                       && ((Level > 0) ? p.StockProductDJ_CharProduct.ProductLevel == Level : 1 == 1)
                       && ((Item > 0) ? p.StockProductDJ_CharProduct.ProductItem == Item : 1 == 1)
                       && p.StatusPenjualan == 0 && p.ReturnProduct == false && p.Draft.Value == false && p.Legacy == null 
                       select p;

        int[] loc = new int[100];

        if (all.Checked)
        {
            foreach (var b in products)
            {
                //d.Draft = false;
                //db.SubmitChanges();

                Bundle_ItemMaster n = new Bundle_ItemMaster();

                n.IDForm = d.ID;
                n.IDProduct = b.ID;

                n.TipeProduct = 1;

                db.Bundle_ItemMaster.InsertOnSubmit(n);
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
                    
                    var product = db.StockProductDJ.SingleOrDefault(p => p.ID == idproduct);
                    
                    Bundle_ItemMaster n = new Bundle_ItemMaster();

                    n.IDForm = d.ID;
                    n.IDProduct = product.ID;

                    n.TipeProduct = 1;

                    db.Bundle_ItemMaster.InsertOnSubmit(n);
                    db.SubmitChanges();
                }
            }
        }

        Response.Redirect("Product_GJ.aspx?id=" + myID);
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
        try
        {
            ((DropDownList)sender).Focus();
        }
        catch (Exception ex)
        {
            prod.Focus();
        }

        prod.Items.Clear();
        int Category = Convert.ToInt32(productcategory.SelectedValue);
        int Level = Convert.ToInt32(productlevel.SelectedValue);
        int Item = Convert.ToInt32(productitem.SelectedValue);


        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);

        var csel = from p in db.StockProductDJ
                   where ((Category > 0) ? p.StockProductDJ_CharProduct.ProductCategory == Category : 1 == 1)
                   && ((Level > 0) ? p.StockProductDJ_CharProduct.ProductLevel == Level : 1 == 1)
                   && ((Item > 0) ? p.StockProductDJ_CharProduct.ProductItem == Item : 1 == 1)

                   && p.StatusPenjualan == 0 && p.ReturnProduct == false && p.Draft.Value == false && p.Legacy == null 
                   select p;

        csel = from p in csel
               where !(from z in db.Bundle_ItemMaster
                       where z.BundleItem.TglStart >= d.TglStart && z.BundleItem.TglExpire < d.TglExpire
                       && z.BundleItem.ApprovalNama != null
                       select z.IDProduct).Contains(p.ID)
               select p;

        foreach (var r in csel)
        {
            string ProtoNomor = string.Empty;
            ProtoNomor = (r.ProtoNomor.Contains("-") ? r.ProtoNomor.Substring(0, r.ProtoNomor.IndexOf('-')) : r.ProtoNomor);
            ProtoNomor = (ProtoNomor == "") ? "-" : ProtoNomor;

            string t = r.Nomor + " (" + ProtoNomor + " / " + r.StockProductDJ_CharProduct.CharProductItem.Nama + " / " + (DateTime.Today - r.Tgl.Value).TotalDays.ToString() + " hari)";
            string v = r.ID.ToString();
            prod.Items.Add(new ListItem(t, v));
        }
    }

    protected void skip_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product_GJ.aspx?id=" + myID);
    }

}

