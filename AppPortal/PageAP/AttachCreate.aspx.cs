using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public partial class AP_AttachCreate : System.Web.UI.Page {
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
			kb.InnerHtml = (App.MaxUpload / 1000).ToString("N0") + " mb";
		}
	}
	protected string BackLink {
		get {
			string href = string.Empty;

			href += "Attach.aspx";
			href += "?module=" + myModule;
			href += "&datatable=" + myDataTable;
			href += "&dataid=" + myDataID;

			return href;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		//Process
		saveAttach(file_1, nama_1);
		saveAttach(file_2, nama_2);
		saveAttach(file_3, nama_3);
		saveAttach(file_4, nama_4);
		saveAttach(file_5, nama_5);

		//Redirect
		Response.Redirect(BackLink + "&d=1");
	}
	protected void saveAttach(FileUpload file, TextBox nama) {
		if (Upload.HasFile(file)) {
			Attach n = new Attach();
			HttpPostedFile f = file.PostedFile;

			n.Module = myModule;
			n.DataTable = myDataTable;
			n.DataID = myDataID;

			n.Tgl = DateTime.Now;
			n.Author = App.UserNama;

			//Nama File
			string NamaFile = nama.Text != "" ?
				Str.Trim(nama.Text) + Upload.GetExt(Str.Trim(f.FileName)) :
				Str.Trim(f.FileName);

			n.NamaFile = NamaFile;
			n.ContentType = f.ContentType;
			n.ContentLength = f.ContentLength;

			//Commit
			db.Attach.InsertOnSubmit(n);
			db.SubmitChanges();

			//Save Binary
			saveBinary(file, n.ID);
		}
	}
	protected void saveBinary(FileUpload file, int newID) {
		AttachBinary n = new AttachBinary();
		HttpPostedFile f = file.PostedFile;

		//Binary Data
		byte[] data = new byte[f.InputStream.Length + 1];
		f.InputStream.Read(data, 0, data.Length);

		n.ID = newID;
		n.Data = data;

		//Commit
		db.AttachBinary.InsertOnSubmit(n);
		db.SubmitChanges();
	}

}