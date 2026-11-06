using System;
using System.Linq;

public partial class Page_Create_AdminSalesAccount : System.Web.UI.Page {
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

    private Map_AC _ac;

    private Map_AC ac
    {
        get
        {
            if (_ac == null)
                _ac = new Map_AC();
            return _ac;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (ac != null)
	        ac.Dispose();
	    base.Dispose();
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			nama.Focus();

			//Form
			fillForm();

			//Form-Default
			tgl.Text = Str.Tgl(DateTime.Today);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Designer.aspx"); }

	//Form
	protected void fillForm() {
        DLC.User(username);

	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
        string itemlist = Request.Form["outletlist"];
        string[] item = itemlist.Split(',');
        if (item.Count() > 0)
        {

            if (valid)
            {
                DataAreaManager n = new DataAreaManager();

                //Master
                n.IDUser = Convert.ToInt32(username.SelectedValue);

                //Account
                n.Nama = Str.CapTitle(nama.Text);
                n.Keterangan = Str.CapFirst(keterangan.Text);
                n.Tgl = Convert.ToDateTime(tgl.Text);

                //Status
                n.Draft = true;
                n.DraftDate = DateTime.Now;
                n.Disable = true;

                //Commit
                db.DataAreaManager.InsertOnSubmit(n);
                db.SubmitChanges();

                for (int i = 0; i < item.Count(); i++)
                {
                    DataAreaManager_Lokasi l = new DataAreaManager_Lokasi();
                    l.IDForm = n.ID;
                    l.IDLokasi = Convert.ToInt32(item[i]);
                    l.TipeLokasi = 1;
                    db.DataAreaManager_Lokasi.InsertOnSubmit(l);
                    db.SubmitChanges();
                }

                //Redirect
                Response.Redirect("AMAddr.aspx?id=" + n.ID);
            }
        }
	}
}