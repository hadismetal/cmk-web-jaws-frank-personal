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

    protected void fillForm()
    {
        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID && p.Draft == true);
        if (d == null) DataError();
    }

    //Exception
    protected void DataError()
    {
        Response.Redirect("Souvenir.aspx");
    }

    protected void fillProduct()
    {
        var rsk = from p in db.Bundle_ItemPasangan
                  join q in db.StockProductPG on p.IDProduct equals q.ID
                  where p.TipeProduct == 3 && p.BundleItem.ID == myID
                  select new { p, q };
        if (rsk.Count() > 0)
        {
            foreach (var r in rsk)
            {
                var rs = db.StockProductPG.SingleOrDefault(p => p.Nomor.Equals(r.q.Nomor) && p.StockActualPG.Substorage == 1
                    && !(from z in db.Bundle_ItemPasangan where z.TipeProduct == 3 && z.IDForm != myID select z.IDProduct).Contains(p.ID)
                    
                             );

                if (rs != null)
                {
                    string v = rs.ID.ToString();
                    string t = Logistic.ProductList_PG(
                            rs.CharProductItem.Nama,
                            rs.Nomor,
                            rs.ImgPicture,
                            DL.SubStorage(rs.StockActualPG.Substorage),
                            Str.Number2(rs.NettoWeight),
                            Str.Number2(rs.GrossWeight),
                            rs.CharGoldModel.Nama
                            );
                    cbl_Product.Items.Add(new ListItem(t, v));
                    cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                    //upl.Update();

                    inputproduct.Text = "";
                    inputproduct.Focus();
                }
            }
            save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        }
    }
    //InputProduct
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        var rsH = db.BundleItem.SingleOrDefault(p => p.ID == myID);

        lit_Message.Text = "";
        string input = inputproduct.Text.Trim();

        try
        {
            //var rs = db.StockProductPG.SingleOrDefault(p => p.Nomor.Equals(input) && p.StockActualPG.Substorage==1);

            var rs = db.StockProductPG.SingleOrDefault(p => p.Nomor.Equals(input) && p.StockActualPG.Substorage == 1
                        && !(from z in db.Bundle_ItemPasangan where z.TipeProduct == 3 select z.IDProduct).Contains(p.ID)
                                 );

            if (rs != null)
            {
                string err_msg = valid(input, cbl_Product);
                if (err_msg == "")
                {
                    string v = rs.ID.ToString();
                    string t = Logistic.ProductList_PG(
                            rs.CharProductItem.Nama,
                            rs.Nomor,
                            rs.ImgPicture,
                            DL.SubStorage(rs.StockActualPG.Substorage),
                            Str.Number2(rs.NettoWeight),
                            Str.Number2(rs.GrossWeight),
                            rs.CharGoldModel.Nama
                            );
                    cbl_Product.Items.Add(new ListItem(t, v));
                    cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                    //upl.Update();

                    inputproduct.Text = "";
                    inputproduct.Focus();
                }
                else
                {
                    // error message
                    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                }
            }
            else
            {
                // error message
                lit_Message.Text = "<span style='color:red;'>PLU sudah terdaftar</span>";
            }
            save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
            // error message
            lit_Message.Text = "<span style='color:red;'>Invalid Product Code !!!</span>";
        }
    }

    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = db.StockProductPG.SingleOrDefault(p => p.Nomor == input);

        if (rs1 == null) { message = "Invalid Product"; }
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
        var rs = from p in db.Bundle_ItemPasangan
                 where p.IDForm == myID && p.TipeProduct == 3
                 select p;
        db.Bundle_ItemPasangan.DeleteAllOnSubmit(rs);
        db.SubmitChanges();

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                Bundle_ItemPasangan n = new Bundle_ItemPasangan();

                n.IDForm = myID;
                n.IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value);

                n.TipeProduct = 3;

                db.Bundle_ItemPasangan.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);
        //d.Draft = false;
        //d.DraftDate = null;

        //Commit
        db.SubmitChanges();

        //Redirect
        Response.Redirect("Souvenir_LD.aspx?id=" + myID);

    }

    protected void skip_Click(object sender, EventArgs e)
    {
        Response.Redirect("Souvenir_LD.aspx?id=" + myID);
    }

}

