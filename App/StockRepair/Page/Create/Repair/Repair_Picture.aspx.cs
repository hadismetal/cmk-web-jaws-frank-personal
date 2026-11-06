using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Page_Register_Repair_Picture : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Repair.aspx"); }

	//Form
	protected void fillForm() {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

            lit_plu.Text = d.StockProductDJ.Nomor;
            lit_tgl.Text = Str.FullDate(d.Tgl);
            lit_tgl2.Text = Str.FullDate(d.TglSelesai);
            lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);

            var d_item_list = from p in db.DocRepair_Repair where p.IDForm == myID select p;

            int index = 1;

            foreach (var r in d_item_list)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharProcessRepair.Nama;
                tr.Cells.Add(td);

                tbl_repairitem.Rows.Add(tr);

                index++;

            }

            var product = d.DocRepair_CharProduct;
            var productold = d.StockProductDJ.StockProductDJ_CharProduct;
            if (product == null) ProductError();
            else
            {
                grossweight.InnerHtml = (product.GrossWeight != productold.GrossWeight ? Str.Number2(productold.GrossWeight) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.GrossWeight);
                //kadarlogam.InnerHtml = (product.KadarLogam != productold.KadarLogam ? Str.Number2(productold.KadarLogam) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.KadarLogam);
                diameter.InnerHtml = (product.DimensiD != productold.DimensiD ? Str.Number2(productold.DimensiD) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiD);
                panjang.InnerHtml = (product.DimensiP != productold.DimensiP ? Str.Number2(productold.DimensiP) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiP);
                lebar.InnerHtml = (product.DimensiL != productold.DimensiL ? Str.Number2(productold.DimensiL) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiL);
                ringsize.InnerHtml = (product.DimensiR != productold.DimensiR ? Str.Number2(productold.DimensiR) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiR);
            }
		}
	}

    protected void ProductError() { Response.Redirect("Repair_Data.aspx?id=" + myID); }

	//Save
	protected void save_Click(object sender, EventArgs e) {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Process
			bool finish = finishUpload(d);

			//Redirect
			if (finish) Response.Redirect("Repair_Submit.aspx?id=" + myID);
		}
	}
	protected bool finishUpload(DocRepair d) {
		bool x = true;

		x = x && saveImage(d, imgpicture, imgpicture_ERR, "Picture");

		return x;
	}

	//Process
	protected bool saveImage(DocRepair d, FileUpload file, HtmlGenericControl lb, string name) {
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
				path += "DocRepair" + "\\";

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