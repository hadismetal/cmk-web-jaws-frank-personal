using System;
using System.Linq;

public partial class AP_DiaryCreate : System.Web.UI.Page {
	private Map_AP _db;

	private Map_AP db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AP();
	        return _db;
	    }
	}

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    base.Dispose();
	}
	protected string myModule { get { return Request.QueryString["module"]; } }
	protected string myDataTable { get { return Request.QueryString["datatable"]; } }
	protected int myDataID { get { return App.QueryInt(this, "dataid"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(RoleAP.Create(myModule));

		if (!IsPostBack) {
			back.HRef = BackLink;
			catatan.Focus();
		}
	}
	protected string BackLink {
		get {
			string href = string.Empty;

			href += "Diary.aspx";
			href += "?module=" + myModule;
			href += "&datatable=" + myDataTable;
			href += "&dataid=" + myDataID;

			return href;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		Diary n = new Diary();

		n.Module = myModule;
		n.DataTable = myDataTable;
		n.DataID = myDataID;

		n.Tgl = DateTime.Now;
		n.Author = App.UserNama;

		n.Catatan = Str.CapFirst(catatan.Text);

		//Commit
		db.Diary.InsertOnSubmit(n);
		db.SubmitChanges();

		//Redirect
		Response.Redirect(BackLink + "&d=1");
	}
}