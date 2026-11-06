using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Page_Register_GJ_Picture : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			//Form
			fillForm();

			//Form-Default
			kb.InnerHtml = (App.MaxUpload / 1000).ToString("N0") + " mb";
			imgpicture_format.InnerHtml = Str.ComaExt(Upload.ImageFormat);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("GJ.aspx"); }

	//Form
	protected void fillForm() {
        var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/GJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
        var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) DataError();
		else {
			//Process
			bool finish = finishUpload(d);

			//Redirect
			if (finish) Response.Redirect("GJ_Submit.aspx?id=" + myID);
		}
	}
	protected bool finishUpload(ProductDesignGJ d) {
		bool x = true;

		x = x && saveImage(d, imgpicture, imgpicture_ERR, "Picture");

		return x;
	}

	//Process
	protected bool saveImage(ProductDesignGJ d, FileUpload file, HtmlGenericControl lb, string name) {
		if (!Upload.HasFile(file)) {
			//Commit
			//d.ImgPicture = null;
			//db.SubmitChanges();

			return true;
		}
		else {
			if (!Upload.FormatValid(file, lb, Upload.ImageFormat)) return false;
			else {
				string path = string.Empty;

				path += App.ImagesDir;
				path += "ProductDesignGJ" + "\\";

				if (Upload.CreateDir(path)) {
					path += name + "_";
					path += myID + Upload.GetExt(file.PostedFile.FileName);

					//Upload
					file.PostedFile.SaveAs(path);

					//Commit
					d.ImgPicture = path;
					db.SubmitChanges();

					return true;
				}
				else {
					dir_ERR.InnerHtml += path;
					dir_ERR.Visible = true;

					return false;
				}
			}
		}
	}
}