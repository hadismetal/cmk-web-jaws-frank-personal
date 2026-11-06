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
            FillProduct();
        }
    }
    //Fill
    protected void Fill()
    {
        save.Visible = false;

        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingGJ.SingleOrDefault(p => p.ID == id);
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
        /// update form jika produk sudah tidak ada di lokasi asal maka dihapus dari daftar
        var item = from p in d.StockOutgoingGJ_Product
                   where p.IDForm == id && p.StockOutgoingGJ.StatusTransOut == 0
                   select p;

        var inhand0 = from p in item where p.StockActualGJ.InHand != 1 select p;
        var inhand1 = from p in item where p.StockActualGJ.InHand == 1 select p;

        foreach (var s in inhand0) { d.StockOutgoingGJ_Product.DeleteOnSubmit(s); db.SubmitChanges(); }

        foreach (var r in inhand1)
        {
            //var rs = d.StockActualGJ.SingleOrDefault(p => p.StockProductGJ.Nomor == r.StockProductGJ.Nomor && p.TipeLokasi == r.StockOutgoingGJ.TipeAsal && p.InHand >= 1 );

            var rs = r.StockActualGJ;

            string v = rs.StockProductGJ.ID.ToString();
            string t = Logistic.ProductList_GJ(
                        rs.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductGJ.Nomor,
                        rs.StockProductGJ.ImgPicture,
                        DL.SubStorage(rs.Substorage),
                        Str.Number2(rs.StockProductGJ.StockProductGJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductGJ.StockProductGJ_CharProduct.GrossWeight)
                        );
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

        }

        inputproduct.Text = "";
        inputproduct.Focus();

        save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        ////////////////

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