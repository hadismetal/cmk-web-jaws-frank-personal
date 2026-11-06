using System;
using System.Linq;

public partial class Page_Register_Design_CharProduct02 : System.Web.UI.Page {
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
			grossweight.Focus();

			//Number-Format
			App.NumberFormat(grossweight);
			App.NumberFormat(kadarlogam);
			App.NumberFormat(kadartukaran);
			App.NumberFormat(dimensid);
			App.NumberFormat(dimensip);
			App.NumberFormat(dimensil);
			App.NumberFormat(dimensir);

			//Form-Default
			kadarlogam.Text = Str.Number2(CV.KadarLogam);
			kadartukaran.Text = Str.Number2(CV.KadarTukaran);

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Design.aspx"); }
	protected void LinkError() { Response.Redirect("Design_CharProduct01.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";
		}
	}

	//Save
	protected bool valid {
		get {
			bool x = true;

			x = x && Fv.isDecimal(grossweight, grossweight_ERR);
			x = x && Fv.isDecimal(kadarlogam, kadarlogam_ERR);
			x = x && Fv.isDecimal(kadartukaran, kadartukaran_ERR);
			x = x && Fv.isDecimal(dimensid, dimensid_ERR);
			x = x && Fv.isDecimal(dimensip, dimensip_ERR);
			x = x && Fv.isDecimal(dimensil, dimensil_ERR);
			x = x && Fv.isDecimal(dimensir, dimensir_ERR, true);

			//Value
			x = x && valid_GrossWeight;
			x = x && valid_KadarLogam;
			x = x && valid_KadarTukaran;

			return x;
		}
	}
	protected bool valid_GrossWeight {
		get {
			decimal dv = Convert.ToDecimal(grossweight.Text);

			bool x = dv > 0;
			grossweight_VAL.Visible = !x;

			return x;
		}
	}
	protected bool valid_KadarLogam {
		get {
			decimal dv = Convert.ToDecimal(kadarlogam.Text);

			bool x = dv > 0;
			kadarlogam_VAL.Visible = !x;

			return x;
		}
	}
	protected bool valid_KadarTukaran {
		get {
			decimal dv = Convert.ToDecimal(kadartukaran.Text);

			bool x = dv > 0;
			kadartukaran_VAL.Visible = !x;

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				//Process
				saveData(d);

				//Redirect
				Response.Redirect("Design_CharDesign01.aspx?id=" + myID);
			}
		}
	}
	protected void saveData(ProductDesignGJ ms) {
		var d = ms.ProductDesignGJ_CharProduct;
		if (d == null) LinkError();
		else {
			d.GrossWeight = Convert.ToDecimal(grossweight.Text);
			d.NettoWeight = d.GrossWeight;
			d.KadarLogam = Convert.ToDecimal(kadarlogam.Text);
			d.KadarTukaran = Convert.ToDecimal(kadartukaran.Text);

			//Dimensi
			d.DimensiD = Convert.ToDecimal(dimensid.Text);
			d.DimensiP = Convert.ToDecimal(dimensip.Text);
			d.DimensiL = Convert.ToDecimal(dimensil.Text);

			//Ring-Size
			if (dimensir.Text != "")
				d.DimensiR = Convert.ToDecimal(dimensir.Text);
			else
				d.DimensiR = null;

			//Commit
			db.SubmitChanges();
		}
	}
}