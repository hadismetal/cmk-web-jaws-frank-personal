using System;
using System.Linq;

public partial class Page_Create_Packaging : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			nama.Focus();
            DLC.CompanyBrand(brand);
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		Packaging n = new Packaging();

		//Account
		n.Nama = Str.CapTitle(nama.Text);
		n.Keterangan = Str.CapFirst(keterangan.Text);
        n.IDBrand = Convert.ToInt32(brand.SelectedValue);

		//Status
		n.Draft = true;
		n.DraftDate = DateTime.Now;
		n.Disable = true;

		//Commit
		db.Packaging.InsertOnSubmit(n);
		db.SubmitChanges();

		//Redirect
		Response.Redirect("ProductInfo.aspx?id=" + n.ID);
	}
}