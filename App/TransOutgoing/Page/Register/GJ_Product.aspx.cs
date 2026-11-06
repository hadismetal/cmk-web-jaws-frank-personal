using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;

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
    //InputProduct
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        Literal plit_Message = lit_Message;
        TextBox pinputproduct = inputproduct;
        System.Web.UI.HtmlControls.HtmlButton psave = save;
        CheckBoxList pcbl_Product = cbl_Product;
        int pid = id;

        Lib.FormRegisterUpdateDokumenDraft_GJ(plit_Message, pinputproduct, psave, pcbl_Product, pid);
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