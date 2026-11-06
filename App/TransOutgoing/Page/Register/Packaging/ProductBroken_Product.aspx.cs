using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Product_Product : System.Web.UI.Page
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
        save.Visible = false;

        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
        //brand.InnerText = rsH.Company.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        //inputproduct.Text = "13/11/0018";
        //inputproduct.Focus();
        FillPackaging();

    }
    //InputProduct
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
        int vB = rsH.IDBrand.Value;
        int vF = rsH.TipeAsal;

        lit_Message.Text = "";
        //string input = inputproduct.Text.Trim();
        String input = ddl_ProductCode.SelectedValue;

        try
        {
            var rs = d.StockActualPackaging.SingleOrDefault(p => p.Packaging.Kode == input && p.TipeLokasi == vF && p.Damaged >= 1 && p.IDLokasi == rsH.IDAsal);

            string err_msg = valid(input, cbl_Product);
            if (err_msg == "")
            {
                string v = rs.Packaging.ID.ToString();
                string t = Logistic.ProductList_Packaging(
                        rs.Packaging.Nama, qty.Text, rs.Packaging.ID.ToString());
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                //upl.Update();

                //inputproduct.Text = "";
                qty.Text = "";
                //inputproduct.Focus();
            }
            else
            {
                // error message
                lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
            }

            save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        }
        catch
        {
            // error message
            lit_Message.Text = "<span style='color:red;'>Invalid Product Code !!!</span>";
        }
    }
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.Packaging.SingleOrDefault(p => p.Kode == "" + input + "");

        if (rs1 == null) { message = "Invalid Product"; }
        else
        {
            // 2. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
            }
        }

        if (!Fv.isInt(qty)) message = "Please input quantity";

        // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

        return message;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        
        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
            int vB = rsH.IDBrand.Value;
            int vF = rsH.TipeAsal;

            decimal qty = 0m;
            if (cbl_Product.Items[i].Selected)
            {
                qty = Convert.ToDecimal(Request.Form["qty_" + cbl_Product.Items[i].Value]);

                var rs = d.StockActualPackaging.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value) && p.TipeLokasi == vF && p.IDLokasi == rsH.IDAsal);

                StockOutgoingPackaging_Product n = new StockOutgoingPackaging_Product()
                {
                    //Input
                    IDForm = id,
                    IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value),
                    Qty = qty
                    //SubStorage = rs.Substorage

                };
                d.StockOutgoingPackaging_Product.InsertOnSubmit(n);

                //Commit
                d.SubmitChanges();
            }
        }

        

        //Redirect
        Response.Redirect("ProductBroken_Finish.aspx?id=" + id);
        
    }
	
	protected void FillPackaging()
    {
        var rs = from p in d.Packaging
                 where !p.Draft && !p.Disable //&& ((p.Nama.Contains(query)) || p.Kode.Contains(query))
                    
                 select new { id = p.Kode, label = p.Nama };

        ddl_ProductCode.Items.Clear();
        foreach (var r in rs)
        {
            string t = r.label + " (" + r.id + ")";
            string v = r.id;
            ListItem item = new ListItem(t, v);

            ddl_ProductCode.Items.Add(item);
        }
    }
}