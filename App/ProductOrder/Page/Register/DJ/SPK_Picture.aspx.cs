using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Page_Register_SPK_Picture : System.Web.UI.Page
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
	protected int myID { get { return App.QueryInt(this,"id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int status { get { return App.QueryInt(this, "status"); } }

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
	protected void DataError() { Response.Redirect("SPK.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
            //Header
            if (tipe == 1 && status != 1)
            {
                //header_nomor.InnerHtml =
                //    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                //    + d.ProductDesign.Nomor + "</a>";

                //Picture
                Upload.Image(imgpicture, d.ProductDesign.ImgPicture);
            }
            else if (tipe != 1 && status != 1)
            {
                //header_nomor.InnerHtml =
                //    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                //    + d.KodeKaretDJ.Nomor + "</a>";

                //Picture
                var lastro = db.ProductOrder.OrderByDescending(p => p.ID).FirstOrDefault(p => p.IDKodeKaret == d.IDKodeKaret && p.Approval != null && p.StatusOrder != null && p.KodeKaretLama != null);
                Upload.Image(imgpicture, lastro.ImgPicture);
            }

              else if (status == 1)
            {
                lb_status.Visible = true;

                if (tipe == 1)
                {
                    //header_nomor.InnerHtml =
                    //  "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    //  + d.ProductDesign.Nomor + "</a>";
                    Upload.Image(imgpicture, d.ProductDesign.ImgPicture);
                }
                else
                {
                   // header_nomor.InnerHtml =
                   //"<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                   //+ d.KodeKaretDJ.Nomor + "</a>";
                    Upload.Image(imgpicture, d.KodeKaretDJ.ImgPicture);
                }
                //Picture
                Upload.Image(imgreplace, d.ImgPicture);
            }
        }
	}
    protected void review_click(object sender, EventArgs e)
    {
        var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            bool finish = finishUpload(d);
            db.SubmitChanges();
            if (finish)
            {
                string href = App.RefreshURL(this);

                //Customize-Qstr
                href += "&id=" + myID;
                href += "&tipe=" + tipe;
                href += "&status=1";
                Response.Redirect(href);
            }
        }

    }
    //Save
    protected void save_Click(object sender, EventArgs e)
    {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else
        {
            if (status == 1)
            {
                Response.Redirect("SPK_Submit.aspx?id=" + myID + "&tipe=" + tipe);
            }

            //Process
            
            else
            {
                //bool finish = finishUpload(d);

                if (tipe == 1 )
                {
                    d.ImgPicture = d.ProductDesign.ImgPicture;
                }
                else if (tipe == 2)
                {
                    var lastro = db.ProductOrder.OrderByDescending(p => p.ID).FirstOrDefault(p => p.IDKodeKaret == d.IDKodeKaret && p.Approval != null && p.StatusOrder != null && p.KodeKaretLama != null);
                    d.ImgPicture = lastro.ImgPicture;
                    //d.ImgPicture = d.KodeKaretDJ.ImgPicture;
                }
            }
            
            db.SubmitChanges();

			//Redirect
			//if (finish) 
            Response.Redirect("SPK_Submit.aspx?id=" + myID + "&tipe=" + tipe);
		}
	}
	protected bool finishUpload(ProductOrder d) {
		bool x = true;
 
		x = x && saveImage(d, imgpicture_up, imgpicture_ERR, "Picture");

		return x;
	}

	//Process
	protected bool saveImage(ProductOrder d, FileUpload file, HtmlGenericControl lb, string name)
    {
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

				if (Upload.CreateDir(path))
                {
					path += name + "_";
					path += myID + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + Upload.GetExt(file.PostedFile.FileName);

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