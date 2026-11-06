using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class DJ_Product : System.Web.UI.Page
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

        inputproduct.Focus();

    }
    //InputProduct
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        var rsH = d.StockProductDJLD.SingleOrDefault(p => p.ID == id);

        lit_Message.Text = "";
        string input = inputproduct.Text.Trim();


        string err_msg = "";
        if (err_msg == "")
        {

            string v = input;
            string t = input;
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
            inputproduct.Text = "";
            inputproduct.Focus();
        }

        save.Visible = cbl_Product.Items.Count > 0 ? true : false;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        int index = 1;
        var rs = d.StockProductDJLD.SingleOrDefault(p => p.ID == id);

        if (cbl_Product.Items.Count > 0)
        {

            for (int i = 0; i < cbl_Product.Items.Count; i++)
            {
                if (cbl_Product.Items[i].Selected)
                {
                    var rsh = d.StockBasic.SingleOrDefault(p => p.ProtoNomor.Equals(cbl_Product.Items[i].Value));
                    if (rsh == null)
                    {
                        StockBasic s = new StockBasic();

                        s.ProtoNomor = Str.Trim(cbl_Product.Items[i].Value);
                        s.Operator = App.UserNama;
                        s.OperatorTgl = DateTime.Now;

                        d.StockBasic.InsertOnSubmit(s);
                        d.SubmitChanges();
                        index++;
                    }
                }
            }

            Response.Redirect("StockBasic.aspx?d=1");
        }
        else
            lit_Message.Text = "<span style='color:red;'>Please input Kode Karet</span>";

    }
}