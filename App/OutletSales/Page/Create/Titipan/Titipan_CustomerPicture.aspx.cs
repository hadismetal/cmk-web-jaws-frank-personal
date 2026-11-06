using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Page_Register_Titipan_Picture : System.Web.UI.Page {
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
    protected int myTitipanID { get { return App.QueryInt(this,"idtitipan"); } }

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
	protected void DataError() { Response.Redirect("Titipan.aspx"); }

	//Form
	protected void fillForm() {
        var d = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Titipan_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Process
			bool finish = finishUpload(d);

			//Redirect
			if (finish) Response.Redirect("Titipan_Data.aspx?id=" + myTitipanID);
		}
	}
	protected bool finishUpload(StockProductDJCustomer d) {
		bool x = true;

		x = x && saveImage(d, imgpicture, imgpicture_ERR, "Picture");

		return x;
	}

	//Process
	protected bool saveImage(StockProductDJCustomer d, FileUpload file, HtmlGenericControl lb, string name) {
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
				path += "StockProductDJCustomer" + "\\";

				if (Upload.CreateDir(path)) {
					path += name + "_";
					path += myID + Upload.GetExt(file.PostedFile.FileName);

					//Upload
					file.PostedFile.SaveAs(path);

					//Commit
					d.ImgPicture = path;
                    d.Nomor = Generate_Nomor(d);
                    d.Draft = false;
                    d.DraftDate = null;
					db.SubmitChanges();

                    DocTitipan_Product s = new DocTitipan_Product();

                    s.IDProductTitipan = d.ID;
                    s.IDForm = myTitipanID;

                    db.DocTitipan_Product.InsertOnSubmit(s);
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

    protected string Generate_Nomor(StockProductDJCustomer d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Value.Year;
        int Bulan = d.Tgl.Value.Month;

        /*int Nomor = db.StockProductDJCustomer.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;

        return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        int Nomor = 1;

        try
        {
            var s = (from p in db.StockProductDJCustomer
                     where p.Tgl.Value.Year == Tahun /*&& p.Tgl.Month == Bulan*/
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(1, 8)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(3, 6)) + 1;
        }
        catch (Exception ex) { }

        string PLU = "T" + Tahun.ToString().Remove(0, 2) + Str.KodeFormat(Nomor, 6);

        return PLU;

    }
}