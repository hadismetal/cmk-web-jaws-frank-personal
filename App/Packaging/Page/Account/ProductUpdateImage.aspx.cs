using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


public partial class Page_Account_PackagingUpdateAddr : System.Web.UI.Page {
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

		if (!IsPostBack) {
			

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;

            //Form-Default
            kb.InnerHtml = (App.MaxUpload / 1000).ToString("N0") + " mb";
            imgpicture_format.InnerHtml = Str.ComaExt(Upload.ImageFormat);
		}
	}

	protected void fillForm() {
		var d = db.Packaging.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            h1_st.Text = d.Nama;
            h1_id.Text = Str.ID_Master(d.ID);
        }
		
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
        var d = db.Packaging.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Process
            bool finish = finishUpload(d);

            //Redirect
            if (finish) Response.Redirect("ProductUpdateImage.aspx?id=" + myID);
        }
	}

    protected bool finishUpload(Packaging d)
    {
        bool x = true;

        x = x && saveImage(d, imgpicture, imgpicture_ERR, "Picture");

        return x;
    }

    //Process
    protected bool saveImage(Packaging d, FileUpload file, HtmlGenericControl lb, string name)
    {
        if (!Upload.HasFile(file))
        {
            //Commit
            //d.ImgPicture = null;
            //db.SubmitChanges();

            return true;
        }
        else
        {
            if (!Upload.FormatValid(file, lb, Upload.ImageFormat)) return false;
            else
            {
                string path = string.Empty;

                path += App.ImagesDir;
                path += "Packaging" + "\\";

                if (Upload.CreateDir(path))
                {
                    path += name + "_";
                    path += myID + Upload.GetExt(file.PostedFile.FileName);

                    //Upload
                    file.PostedFile.SaveAs(path);

                    //Commit
                    d.ImgPicture = path;
                    db.SubmitChanges();

                    return true;
                }
                else
                {
                    dir_ERR.InnerHtml += path;
                    dir_ERR.Visible = true;

                    return false;
                }
            }
        }
    }
}