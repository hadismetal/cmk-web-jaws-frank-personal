using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class GJ_Product : System.Web.UI.Page
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
        //save.Visible = false;

        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingGJ.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        int vB = rsH.IDBrand;
        int vF = rsH.TipeAsal;
        int LokasiAsal = rsH.IDAsal;

        var rs = from p in d.StockActualGJ 
                 where p.IDBrand == vB && p.TipeLokasi == vF
                && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductGJ.StatusLebur == false && p.StockProductGJ.ReturnProduct == false
                && !(from z in d.StockOutgoingGJ_Product
                     where (z.StockOutgoingGJ.StatusTransOut == 2 || z.StockOutgoingGJ.StatusTransOut == 1)
                     && z.StockOutgoingGJ.IDAsal == LokasiAsal && z.StockOutgoingGJ.TipeAsal == vF
                     select z.IDProduct).Contains(p.IDProduct)
                select p;

        if (rs.Count() > 0)
        {
            foreach (var r in rs)
            {
                string v = r.StockProductGJ.ID.ToString();
                string t = Logistic.ProductList_GJ(
                        r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                        r.StockProductGJ.Nomor,
                        r.StockProductGJ.ImgPicture,
                        DL.SubStorage(r.Substorage),
                        Str.Number2(r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight),
                        Str.Number2(r.StockProductGJ.StockProductGJ_CharProduct.GrossWeight)
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                //upl.Update();
            }
        }

    }
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductGJ.SingleOrDefault(p => p.Nomor == input);

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
        Db.Execute("DELETE FROM StockOutgoingGJ_Product WHERE IDForm=" + id,Db.Cnn_DB);

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                var rs = d.StockActualGJ.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value));

                StockOutgoingGJ_Product n = new StockOutgoingGJ_Product()
                {
                    //Input
                    IDForm = id,
                    IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value),
                    SubStorage = rs.Substorage

                };
                d.StockOutgoingGJ_Product.InsertOnSubmit(n);
            }
        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("GJ_Finish.aspx?id=" + id);
        
    }
}