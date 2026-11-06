using System;
using System.Linq;

public partial class Page_Register_Titipan_CharProduct01 : System.Web.UI.Page
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
    protected int myTitipanID { get { return App.QueryInt(this,"idtitipan"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            productitem.Focus();
            App.NumberFormat(grossweight);

            //DLC
            DLC.CharProductItem(productitem);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Titipan_Data.aspx?id=" + myTitipanID); }

    //Form
    protected void fillForm()
    {
        var d = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Titipan_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            
        }
    }

    protected bool valid_GrossWeight
    {
        get
        {
            decimal dv = Convert.ToDecimal(grossweight.Text);

            bool x = dv > 0;
            grossweight_VAL.Visible = !x;

            return x;
        }
    }
    protected void save_Click(object sender, EventArgs e)
    {
        if (valid_GrossWeight)
        {
            var d = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) DataError();
            else
            {
                //Process
                deleteProcess(d);
                insertProcess(d);

                //Redirect
                Response.Redirect("Titipan_CustomerStone.aspx?id=" + myID + "&idtitipan=" + myTitipanID);
            }
        }
    }
    protected void deleteProcess(StockProductDJCustomer ms)
    {
        var d = ms.StockProductDJCustomer_CharProduct;
        if (d != null)
        {
            //Commit
            db.StockProductDJCustomer_CharProduct.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void insertProcess(StockProductDJCustomer ms)
    {
        StockProductDJCustomer_CharProduct n = new StockProductDJCustomer_CharProduct();

        n.ID = ms.ID;

        n.ProductItem = Convert.ToInt32(productitem.SelectedValue);
        n.GrossWeight = Convert.ToDecimal(grossweight.Text);
        n.StoneCarat = 0m;
        n.StoneQty = 0m;
        n.StoneWeight = 0m;

        //Commit
        db.StockProductDJCustomer_CharProduct.InsertOnSubmit(n);
        db.SubmitChanges();
    }
}