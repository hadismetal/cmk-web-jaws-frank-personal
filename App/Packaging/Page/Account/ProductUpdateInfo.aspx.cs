using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;

public partial class Page_Account_PackagingUpdateAddr : System.Web.UI.Page
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
		
		DDLTipe(ddltipe);

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
		var d = db.Packaging.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else
		{
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Master(d.ID);

			kode.Text = d.Kode;
			satuan.Text = d.Satuan;
			ddltipe.SelectedValue = Db.SingleInteger("SELECT TOP 1 Tipe FROM Packaging WHERE ID = " + d.ID, Db.Cnn_DB).ToString();
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e)
	{
		var d = db.Packaging.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else
		{
			if (ValidateKodePackaging(kode.Text) == "")
			{
				d.Kode = kode.Text;
				d.Satuan = satuan.Text;

				//Commit
				db.SubmitChanges();
				Db.Execute("UPDATE Packaging SET Tipe = '" + ddltipe.SelectedValue + "' WHERE ID = " + d.ID.ToString(), Db.Cnn_DB);

				//Redirect
				App.Reload(this, myID);
			}
			else
			{
				string script = "alert('" + ValidateKodePackaging(kode.Text) + "');";
				ClientScript.RegisterStartupScript(this.GetType(), "InjectAlert", script, true);
			}
		}
	}

	protected string ValidateKodePackaging(string kode)
	{
		string msg = "";


		string kodePackagingLama = Db.SingleString("SELECT Kode FROM Packaging WHERE ID =" + myID, Db.Cnn_DB);
		if (kodePackagingLama != kode)
		{
			bool d = db.Packaging.Any(p => p.Kode.ToLower() == kode.TrimEnd().TrimStart().ToLower() && !p.Draft && !p.Disable);
			if (d) { msg = "Kode packaging tersebut sudah terdaftar di database. Silakan cari kode yang lain."; }
		}
		return msg;
	}
	
	protected void DDLTipe(DropDownList c)
	{
		string query = "SELECT ID, Nama FROM PackagingType";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
	}
}