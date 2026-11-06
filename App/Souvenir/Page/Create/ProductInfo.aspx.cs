using System;
using System.Linq;

public partial class Page_Create_SouvenirAddr : System.Web.UI.Page
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
            kode.Focus();

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Product.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.Souvenir.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nama.InnerHtml =
                "<a href=\"../Account/ProductFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nama + "</a>";
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.Souvenir.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
			string rslt = ValidateKodeSouvenir(kode.Text);
            if (rslt == "")
            {
                d.Kode = kode.Text;
                d.Satuan = satuan.Text;
                d.Tgl = DateTime.Now;

                //Commit
                db.SubmitChanges();
                string Harga = string.IsNullOrEmpty(harga.Text) ? "0" : harga.Text.Replace(".", "");
                Db.Execute("UPDATE Souvenir SET Harga = " + Harga + " WHERE ID = " + d.ID.ToString(), Db.Cnn_DB);

                //Redirect
                Response.Redirect("ProductImage.aspx?id=" + myID);
            }
            else
            {
                string script = "alert('" + rslt + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "InjectAlert", script, true);
            }
        }
    }

    protected string ValidateKodeSouvenir(string kode)
    {
        string msg = "";
        bool d = db.Souvenir.Any(p => p.Kode.ToLower() == kode.TrimEnd().TrimStart().ToLower() && !p.Draft && !p.Disable);
        if (d) { msg = "Kode souvenir tersebut sudah terdaftar di database. Silakan cari kode yang lain."; }
        return msg;
    }
}