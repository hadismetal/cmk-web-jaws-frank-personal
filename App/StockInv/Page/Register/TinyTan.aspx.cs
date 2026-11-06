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

        if (cbl_Product.Items.Count > 0)
        {

            for (int i = 0; i < cbl_Product.Items.Count; i++)
            {
                if (cbl_Product.Items[i].Selected)
                {
                    int isExist = 0;
                    try
                    {
                        isExist = Db.SingleInteger("SELECT 1 FROM mstinytan WHERE m_karet = '" + cbl_Product.Items[i].Value + "'", Db.Cnn_CS);
                    }
                    catch { }
                    if (isExist == 0)
                    {
                        Db.Rs("INSERT INTO mstinytan VALUES ('" + cbl_Product.Items[i].Value + "', '" + App.UserNama + "' , GETDATE())", Db.Cnn_CS);
                        index++;
                    }
                }
            }

            Response.Redirect("TinyTan.aspx?d=1");
        }
        else
            lit_Message.Text = "<span style='color:red;'>Please input Kode Karet</span>";

    }
}