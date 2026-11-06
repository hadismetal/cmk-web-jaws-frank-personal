using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PG_Product : System.Web.UI.Page
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
            FillProduct();
        }
    }
    //Fill
    protected void Fill()
    {
        save.Visible = false;

        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingPG.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        //inputproduct.Text = "13/11/0018";
        inputproduct.Focus();

    }

    protected void FillProduct()
    {
        var dj = from p in d.StockOutgoingPG_Product
                 where p.IDForm == id
                 select p;
        foreach (var r in dj)
        {
            var rs = d.StockActualPG.SingleOrDefault(p => p.StockProductPG.Nomor == r.StockProductPG.Nomor 
                && p.TipeLokasi == r.StockOutgoingPG.TipeAsal && p.InHand >= 1);

            if (rs == null)
            {
                //db.ExecuteCommand("DELETE FROM StockOutgoingPG_Product WHERE IDProduct =" + rs.IDProduct + " AND IDForm=" + r.ID);
                //db.SubmitChanges();
            }
            else
            {
                string v = rs.StockProductPG.ID.ToString();
                string t = Logistic.ProductList_PG(
                        rs.StockProductPG.CharProductItem.Nama,
                        rs.StockProductPG.Nomor,
                        rs.StockProductPG.ImgPicture,
                        DL.SubStorage(rs.Substorage),
                        Str.Number2(rs.StockProductPG.NettoWeight),
                        Str.Number2(rs.StockProductPG.GrossWeight),
                        rs.StockProductPG.CharGoldModel.Nama
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                //upl.Update();

                inputproduct.Text = "";
                inputproduct.Focus();
            }
            save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        }
    }

    //InputProduct
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        Literal plit_Message = lit_Message;
        TextBox pinputproduct = inputproduct;
        System.Web.UI.HtmlControls.HtmlButton psave = save;
        CheckBoxList pcbl_Product = cbl_Product;
        int pid = id;
        string Mode = "update";

        Lib.FormRegisterUpdateDokumenDraft_DJ(plit_Message, pinputproduct, psave, pcbl_Product, pid, Mode);
    }
    
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductPG.SingleOrDefault(p => p.Nomor == input);

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
        Db.Execute("DELETE FROM StockOutgoingPG_Product WHERE IDForm=" + id, Db.Cnn_DB);
        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                var rs = d.StockActualPG.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value));

                StockOutgoingPG_Product n = new StockOutgoingPG_Product()
                {
                    //Input
                    IDForm = id,
                    IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value),
                    SubStorage = rs.Substorage

                };
                d.StockOutgoingPG_Product.InsertOnSubmit(n);
            }
        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("PG_Finish.aspx?id=" + id);
        
    }
}