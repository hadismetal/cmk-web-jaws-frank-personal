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
			App.NumberFormat(dimensir2);

			//Form-Default
			kadarlogam.Text = Str.Number2(CV.KadarLogam);
			kadartukaran.Text = Str.Number2(CV.KadarTukaran);

			//Form
			fillForm();
			ApplyConditionalValidation();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Design.aspx"); }
	protected void LinkError() { Response.Redirect("Design_CharProduct01.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";
		}
	}

	//Save
	// protected bool valid {
	// 	get {
	// 		bool x = true;

	// 		x = x && Fv.isDecimal(grossweight, grossweight_ERR);
	// 		x = x && Fv.isDecimal(kadarlogam, kadarlogam_ERR);
	// 		x = x && Fv.isDecimal(kadartukaran, kadartukaran_ERR);
	// 		x = x && Fv.isDecimal(dimensid, dimensid_ERR);
	// 		x = x && Fv.isDecimal(dimensip, dimensip_ERR);
	// 		x = x && Fv.isDecimal(dimensil, dimensil_ERR);
	// 		x = x && Fv.isDecimal(dimensir, dimensir_ERR, true);
	// 		x = x && Fv.isDecimal(dimensir2, dimensir2_ERR, true);

	// 		//Value
	// 		x = x && valid_GrossWeight;
	// 		x = x && valid_KadarLogam;
	// 		x = x && valid_KadarTukaran;

	// 		return x;
	// 	}
	// }
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
			x = x && Fv.isDecimal(dimensir2, dimensir2_ERR, true);

			dimensir_ERR.Visible = false;
			dimensir2_ERR.Visible = false;

			var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);

			// if (d != null && d.ProductDesign_CharProduct != null)
			// {
			// 	int productItem = d.ProductDesign_CharProduct.ProductItem;

			// 	var ringTypes = new[] { 54, 73, 88, 64, 91, 84, 92 };
			// 	bool isRingType = ringTypes.Contains(productItem);

			// 	if (isRingType)
			// 	{
			// 		bool empty1 = string.IsNullOrWhiteSpace(dimensir.Text);
			// 		bool empty2 = string.IsNullOrWhiteSpace(dimensir2.Text);

			// 		if (empty1 || empty2)
			// 		{
			// 			dimensir_ERR.InnerText = "Cannot be empty (use 0 if not used)";
			// 			dimensir2_ERR.InnerText = "Cannot be empty (use 0 if not used)";

			// 			dimensir_ERR.Visible = true;
			// 			dimensir2_ERR.Visible = true;

			// 			x = false;
			// 			return;
			// 		}

			// 		decimal r1 = 0, r2 = 0;

			// 		bool valid1 = decimal.TryParse(dimensir.Text, out r1);
			// 		bool valid2 = decimal.TryParse(dimensir2.Text, out r2);

			// 		if (!valid1 || !valid2)
			// 		{
			// 			dimensir_ERR.InnerText = "Invalid number";
			// 			dimensir2_ERR.InnerText = "Invalid number";

			// 			dimensir_ERR.Visible = true;
			// 			dimensir2_ERR.Visible = true;

			// 			x = false;
			// 			return;
			// 		}

			// 		if (r1 == 0 && r2 == 0)
			// 		{
			// 			dimensir_ERR.InnerText = "One must be filled (not both 0)";
			// 			dimensir2_ERR.InnerText = "One must be filled (not both 0)";

			// 			dimensir_ERR.Visible = true;
			// 			dimensir2_ERR.Visible = true;

			// 			x = false;
			// 		}
			// 		else if (r1 > 0 && r2 > 0)
			// 		{
			// 			dimensir_ERR.InnerText = "Only one allowed, other must be 0";
			// 			dimensir2_ERR.InnerText = "Only one allowed, other must be 0";

			// 			dimensir_ERR.Visible = true;
			// 			dimensir2_ERR.Visible = true;

			// 			x = false;
			// 		}
			// 	}
			// 	else
			// 	{
			// 		if (dimensid.Text == "")
			// 		{
			// 			dimensid_ERR.InnerText = "Required";
			// 			dimensid_ERR.Visible = true;
			// 			x = false;
			// 		}

			// 		if (dimensip.Text == "")
			// 		{
			// 			dimensip_ERR.InnerText = "Required";
			// 			dimensip_ERR.Visible = true;
			// 			x = false;
			// 		}

			// 		if (dimensil.Text == "")
			// 		{
			// 			dimensil_ERR.InnerText = "Required";
			// 			dimensil_ERR.Visible = true;
			// 			x = false;
			// 		}
			// 	}
			// }
			if (d != null && d.ProductDesign_CharProduct != null)
			{
				int productItem = d.ProductDesign_CharProduct.ProductItem;

				var ringTypes = new[] { 54, 73, 88, 64, 91, 84, 92 };
				bool isRingType = ringTypes.Contains(productItem);

				// reset error dulu
				dimensir_ERR.Visible = false;
				dimensir2_ERR.Visible = false;
				dimensid_ERR.Visible = false;
				dimensip_ERR.Visible = false;
				dimensil_ERR.Visible = false;

				if (isRingType)
				{
					bool empty1 = string.IsNullOrWhiteSpace(dimensir.Text);
					bool empty2 = string.IsNullOrWhiteSpace(dimensir2.Text);

					if (empty1 || empty2)
					{
						dimensir_ERR.InnerText = "Cannot be empty (use 0 if not used)";
						dimensir2_ERR.InnerText = "Cannot be empty (use 0 if not used)";

						dimensir_ERR.Visible = true;
						dimensir2_ERR.Visible = true;

						x = false;
					}
					else
					{
						decimal r1, r2;

						bool valid1 = decimal.TryParse(dimensir.Text, out r1);
						bool valid2 = decimal.TryParse(dimensir2.Text, out r2);

						if (!valid1 || !valid2)
						{
							dimensir_ERR.InnerText = "Invalid number";
							dimensir2_ERR.InnerText = "Invalid number";

							dimensir_ERR.Visible = true;
							dimensir2_ERR.Visible = true;

							x = false;
						}
						else if (r1 == 0 && r2 == 0)
						{
							dimensir_ERR.InnerText = "One must be filled (not both 0)";
							dimensir2_ERR.InnerText = "One must be filled (not both 0)";

							dimensir_ERR.Visible = true;
							dimensir2_ERR.Visible = true;

							x = false;
						}
						else if (r1 > 0 && r2 > 0)
						{
							dimensir_ERR.InnerText = "Only one allowed, other must be 0";
							dimensir2_ERR.InnerText = "Only one allowed, other must be 0";

							dimensir_ERR.Visible = true;
							dimensir2_ERR.Visible = true;

							x = false;
						}
					}
				}
				else
				{
					if (string.IsNullOrWhiteSpace(dimensid.Text))
					{
						dimensid_ERR.InnerText = "Required";
						dimensid_ERR.Visible = true;
						x = false;
					}

					if (string.IsNullOrWhiteSpace(dimensip.Text))
					{
						dimensip_ERR.InnerText = "Required";
						dimensip_ERR.Visible = true;
						x = false;
					}

					if (string.IsNullOrWhiteSpace(dimensil.Text))
					{
						dimensil_ERR.InnerText = "Required";
						dimensil_ERR.Visible = true;
						x = false;
					}
				}
			}
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
	// protected void save_Click(object sender, EventArgs e) {
	// 	if (valid) {
	// 		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
	// 		var c = d.ProductDesignCharProduct;
	// 		if (d == null) DataError();
	// 		else {
	// 			//Process
	// 			saveData(d);

	// 			//Redirect
	// 			Response.Redirect("Design_CharDesign01.aspx?id=" + myID);
	// 		}
	// 	}
	// }
	protected void save_Click(object sender, EventArgs e)
	{
		if (!valid)
			return;

		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);

		if (d == null)
		{
			DataError();
			return;
		}

		saveData(d);

		Response.Redirect("Design_CharDesign01.aspx?id=" + myID);
	}

	private void ApplyConditionalValidation()
	{
		var d = db.ProductDesign
				.SingleOrDefault(p => p.ID == myID && p.Draft);

		if (d == null)
			return;

		var c = d.ProductDesign_CharProduct;

		if (c == null)
			return;

		int productItem = c.ProductItem; 

		var ringTypes = new[] { 54, 73, 88, 64, 91, 84, 92};
		bool isRingType = ringTypes.Contains(productItem);

		dimensir_RF.Enabled = isRingType;
		dimensir2_RF.Enabled = isRingType;
		dimensid_RF.Enabled = !isRingType;
		dimensip_RF.Enabled = !isRingType;
		dimensil_RF.Enabled = !isRingType;
	}

	protected void saveData(ProductDesign ms) {
		var d = ms.ProductDesign_CharProduct;
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
			UpdateDimensiR2();
		}
	}

	private void UpdateDimensiR2()
	{
		db.ExecuteCommand(
			"UPDATE ProductDesign_CharProduct SET DimensiR2 = {0} WHERE ID = {1}",
			string.IsNullOrEmpty(dimensir2.Text)
				? (object)DBNull.Value
				: Convert.ToDecimal(dimensir2.Text),
			myID
		);
	}
}