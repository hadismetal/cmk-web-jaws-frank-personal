using System;
using System.Linq;

public partial class Page_Account_SouvenirUpdateAddr : System.Web.UI.Page
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

		if (!IsPostBack)
		{
			kode.Focus();

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;
		}
	}
	protected void fillForm()
	{
		var d = db.Souvenir.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else
		{
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Master(d.ID);

			kode.Text = d.Kode;
			satuan.Text = d.Satuan;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e)
	{
		var d = db.Souvenir.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else
		{
			if (ValidateKodeSouvenir(kode.Text) == "")
			{
				d.Kode = kode.Text;
				d.Satuan = satuan.Text;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.Reload(this, myID);
			}
			else
			{
				string script = "alert('" + ValidateKodeSouvenir(kode.Text) + "');";
				ClientScript.RegisterStartupScript(this.GetType(), "InjectAlert", script, true);
			}
		}
	}

	protected string ValidateKodeSouvenir(string kode)
	{
		string msg = "";

		string kodeSouvenirLama = Db.SingleString("SELECT Kode FROM Souvenir WHERE ID =" + myID, Db.Cnn_DB);
		if (kodeSouvenirLama != kode)
		{
			bool d = db.Souvenir.Any(p => p.Kode.ToLower() == kode.TrimEnd().TrimStart().ToLower() && !p.Draft && !p.Disable);
			if (d) { msg = "Kode souvenir tersebut sudah terdaftar di database. Silakan cari kode yang lain."; }
		}
		return msg;
	}
}