using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Page_Register_SPK_Picture : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			//Form
			fillForm();

			//Form-Default
			//kb.InnerHtml = (App.MaxUpload / 1000).ToString("N0") + " mb";
			//imgpicture_format.InnerHtml = Str.ComaExt(Upload.ImageFormat);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
            //Header
            if (tipe == 1)
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.ProductDesign.Nomor + "</a>";

                //Picture
                Upload.Image(imgpicture, d.ProductDesign.ImgPicture);
            }
            else
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.KodeKaretDJ.Nomor + "</a>";

                //Picture
                Upload.Image(imgpicture, d.KodeKaretDJ.ImgPicture);
            }
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Process
			//bool finish = finishUpload(d);

            if (tipe == 1)
                d.ImgPicture = d.ProductDesign.ImgPicture;
            else if (tipe == 2)
                d.ImgPicture = d.KodeKaretDJ.ImgPicture;

            db.SubmitChanges();

			//Redirect
			//if (finish) 
            Response.Redirect("SPK_Submit.aspx?id=" + myID + "&tipe=" + tipe);
		}
	}
	/*protected bool finishUpload(ProductOrder d) {
		bool x = true;

		x = x && saveImage(d, imgpicture, imgpicture_ERR, "Picture");

		return x;
	}

	//Process
	protected bool saveImage(ProductOrder d, FileUpload file, HtmlGenericControl lb, string name) {
		if (!Upload.HasFile(file)) {
			//Commit
			d.ImgPicture = null;
			db.SubmitChanges();

			return true;
		}
		else {
			if (!Upload.FormatValid(file, lb, Upload.ImageFormat)) return false;
			else {
				string path = string.Empty;

				path += App.ImagesDir;
				path += "ProductOrder" + "\\";

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
	}*/
}