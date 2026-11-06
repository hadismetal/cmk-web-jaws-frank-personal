using System;
using System.Linq;

public partial class Page_Register_SPK_CharProduct02 : System.Web.UI.Page {
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
			grossweight.Focus();

			//Number-Format
			App.NumberFormat(grossweight);
			App.NumberFormat(kadarlogam);
			App.NumberFormat(kadartukaran);
			//App.NumberFormat(dimensid);
			App.NumberFormat(dimensip);
			App.NumberFormat(dimensil);
			App.NumberFormat(dimensir);
			App.NumberFormat(dimensir2);

			//Form-Default
			kadarlogam.Text = Str.Number2(CV.KadarLogam);
			kadartukaran.Text = Str.Number2(CV.KadarTukaran);

			//Form
			fillForm();
			// ApplyConditionalValidation();

		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx"); }
	protected void LinkError() { Response.Redirect("SPK_CharProduct01.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
            //if (tipe == 1)
            //{
            //    header_nomor.InnerHtml =
            //        "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //        + d.ProductDesign.Nomor + "</a>";
            //}
            //else
            //{
            //    header_nomor.InnerHtml =
            //        "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //        + d.KodeKaretDJ.Nomor + "</a>";
            //}
		}

        if (tipe == 1)
        {
            var dsg = db.ProductDesign_CharProduct.SingleOrDefault(p => p.ID == d.IDProductDesign);
            if (dsg == null) DataError();
            else
            {
                grossweight.Text = dsg.GrossWeight.ToString("N2");
                kadarlogam.Text = dsg.KadarLogam.ToString("N2");
                kadartukaran.Text = dsg.KadarTukaran.ToString("N2");
                //dimensid.Text = Convert.ToDecimal(dsg.DimensiD).ToString("N2");
                dimensip.Text = Convert.ToDecimal(dsg.DimensiP).ToString("N2");
                dimensil.Text = Convert.ToDecimal(dsg.DimensiL).ToString("N2");
                dimensir.Text = Convert.ToDecimal(dsg.DimensiR).ToString("N2");
                dimensir2.Text = Convert.ToDecimal(0).ToString("N2");
            }
        }
        else if (tipe == 2)
        {
            var ro_kkp = db.ProductOrder.Where(p => p.KodeKaretLama == d.KodeKaretLama && p.Approval != null).OrderByDescending(p => p.ID).FirstOrDefault();
            var ro_kkj = db.ProductOrder.Where(p => p.KodeKaret == d.KodeKaret && p.Approval != null).OrderByDescending(p => p.ID).FirstOrDefault();
            var ro_kkm = db.ProductOrder.Where(p => p.KodeKaretDJ.Nomor == d.KodeKaret && p.Approval != null).OrderByDescending(p => p.ID).FirstOrDefault();

            if (ro_kkp != null)
            {
                grossweight.Text = ro_kkp.ProductOrder_CharProduct.GrossWeight.ToString("N2");
                kadarlogam.Text = ro_kkp.ProductOrder_CharProduct.KadarLogam.ToString("N2");
                kadartukaran.Text = ro_kkp.ProductOrder_CharProduct.KadarTukaran.ToString("N2");
                //dimensid.Text = ro_kkp.ProductOrder_CharProduct.DimensiD.ToString("N2");
                dimensip.Text = ro_kkp.ProductOrder_CharProduct.DimensiP.ToString("N2");
                dimensil.Text = ro_kkp.ProductOrder_CharProduct.DimensiL.ToString("N2");
                dimensir.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.DimensiR).ToString("N2");
                dimensir2.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.DimensiR2).ToString("N2");
            }
            else if (ro_kkj != null)
            {
                grossweight.Text = ro_kkj.ProductOrder_CharProduct.GrossWeight.ToString("N2");
                kadarlogam.Text = ro_kkj.ProductOrder_CharProduct.KadarLogam.ToString("N2");
                kadartukaran.Text = ro_kkj.ProductOrder_CharProduct.KadarTukaran.ToString("N2");
                //dimensid.Text = ro_kkj.ProductOrder_CharProduct.DimensiD.ToString("N2");
                dimensip.Text = ro_kkj.ProductOrder_CharProduct.DimensiP.ToString("N2");
                dimensil.Text = ro_kkj.ProductOrder_CharProduct.DimensiL.ToString("N2");
                dimensir.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.DimensiR).ToString("N2");
                dimensir2.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.DimensiR2).ToString("N2");
            }
            else if (ro_kkm != null)
            {
                grossweight.Text = ro_kkm.ProductOrder_CharProduct.GrossWeight.ToString("N2");
                kadarlogam.Text = ro_kkm.ProductOrder_CharProduct.KadarLogam.ToString("N2");
                kadartukaran.Text = ro_kkm.ProductOrder_CharProduct.KadarTukaran.ToString("N2");
                //dimensid.Text = ro_kkm.ProductOrder_CharProduct.DimensiD.ToString("N2");
                dimensip.Text = ro_kkm.ProductOrder_CharProduct.DimensiP.ToString("N2");
                dimensil.Text = ro_kkm.ProductOrder_CharProduct.DimensiL.ToString("N2");
                dimensir.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.DimensiR).ToString("N2");
                dimensir2.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.DimensiR2).ToString("N2");
            }

        }
	}

	//Save
	// protected bool valid {
	// 	get {
	// 		bool x = true;

	// 		x = x && Fv.isDecimal(grossweight, grossweight_ERR);
	// 		x = x && Fv.isDecimal(kadarlogam, kadarlogam_ERR);
	// 		x = x && Fv.isDecimal(kadartukaran, kadartukaran_ERR);
	// 		//x = x && Fv.isDecimal(dimensid, dimensid_ERR);
	// 		x = x && Fv.isDecimal(dimensip, dimensip_ERR);
	// 		x = x && Fv.isDecimal(dimensil, dimensil_ERR);
	// 		x = x && Fv.isDecimal(dimensir, dimensir_ERR, true);

	// 		//Value
	// 		x = x && valid_GrossWeight;
	// 		x = x && valid_KadarLogam;
	// 		x = x && valid_KadarTukaran;

	// 		return x;
	// 	}
	// }
	// protected bool valid {
	// 	get {
	// 		bool x = true;

	// 		x = x && Fv.isDecimal(grossweight, grossweight_ERR);
	// 		x = x && Fv.isDecimal(kadarlogam, kadarlogam_ERR);
	// 		x = x && Fv.isDecimal(kadartukaran, kadartukaran_ERR);
	// 		x = x && Fv.isDecimal(dimensip, dimensip_ERR);
	// 		x = x && Fv.isDecimal(dimensil, dimensil_ERR);
	// 		x = x && Fv.isDecimal(dimensir, dimensir_ERR, true);
	// 		x = x && Fv.isDecimal(dimensir2, dimensir2_ERR, true);

	// 		dimensir_ERR.Visible = false;
	// 		dimensir2_ERR.Visible = false;

	// 		var order = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);

	// 		if (order != null && order.ProductOrder_CharProduct != null)
	// 		{
	// 			int productItem = order.ProductOrder_CharProduct.ProductItem;

	// 			var ringTypes = new[] { 73, 88, 64, 91, 84, 92, 89 };
	// 			bool isRingType = ringTypes.Contains(productItem);

	// 			if (isRingType)
	// 			{
	// 				bool ring1Empty = string.IsNullOrEmpty(dimensir.Text) || dimensir.Text == "0";
	// 				bool ring2Empty = string.IsNullOrEmpty(dimensir2.Text) || dimensir2.Text == "0";

	// 				if (ring1Empty && ring2Empty)
	// 				{
	// 					dimensir_ERR.InnerText = "Required (fill one)";
	// 					dimensir_ERR.Visible = true;

	// 					dimensir2_ERR.InnerText = "Required (fill one)";
	// 					dimensir2_ERR.Visible = true;

	// 					x = false;
	// 				}
	// 			}
	// 			else
	// 			{
	// 				// if (dimensid.Text == "0")
	// 				// {
    //                 //     dimensid_ERR.InnerText = "Required";
	// 				// 	dimensid_ERR.Visible = true;
	// 				// 	x = false;
	// 				// }

	// 				if (dimensip.Text == "0")
	// 				{
    //                     dimensip_ERR.InnerText = "Required";
	// 					dimensip_ERR.Visible = true;
	// 					x = false;
	// 				}

	// 				if (dimensil.Text == "0")
	// 				{
	// 					dimensil_ERR.InnerText = "Required";
	// 					dimensil_ERR.Visible = true;
	// 					x = false;
	// 				}
	// 			}
	// 		}

	// 		// Value checks
	// 		x = x && valid_GrossWeight;
	// 		x = x && valid_KadarLogam;
	// 		x = x && valid_KadarTukaran;

	// 		return x;
	// 	}
	// }

	protected bool valid
	{
		get
		{
			bool x = true;

			x = x && Fv.isDecimal(grossweight, grossweight_ERR);
			x = x && Fv.isDecimal(kadarlogam, kadarlogam_ERR);
			x = x && Fv.isDecimal(kadartukaran, kadartukaran_ERR);
			x = x && Fv.isDecimal(dimensip, dimensip_ERR);
			x = x && Fv.isDecimal(dimensil, dimensil_ERR);
			x = x && Fv.isDecimal(dimensir, dimensir_ERR);
			x = x && Fv.isDecimal(dimensir2, dimensir2_ERR);

			x = x && valid_GrossWeight;
			x = x && valid_KadarLogam;
			x = x && valid_KadarTukaran;

			var order = db.ProductOrder
				.SingleOrDefault(p => p.ID == myID && p.Draft);

			if (order != null && order.ProductOrder_CharProduct != null)
			{
				int productItem = order.ProductOrder_CharProduct.ProductItem;

				var ringTypes = new[] { 54, 73, 88, 64, 91, 84, 92};
				bool isRingType = ringTypes.Contains(productItem);

				if (isRingType)
				{
					decimal r1 = Convert.ToDecimal(dimensir.Text);
					decimal r2 = Convert.ToDecimal(dimensir2.Text);

					bool ringValid = (r1 > 0 || r2 > 0);

					dimensir_ERR.InnerText = "Required";
					dimensir2_ERR.InnerText = "Required";

					dimensir_ERR.Visible = !ringValid;
					dimensir2_ERR.Visible = !ringValid;

					x = x && ringValid;
				}
				else
				{
					bool pEmpty = string.IsNullOrEmpty(dimensip.Text.Trim());
					bool lEmpty = string.IsNullOrEmpty(dimensil.Text.Trim());

					dimensip_ERR.InnerText = "Required";
					dimensil_ERR.InnerText = "Required";

					dimensip_ERR.Visible = pEmpty;
					dimensil_ERR.Visible = lEmpty;

					x = x && !pEmpty && !lEmpty;
				}
			}

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
			var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				//Process
				saveData(d);

				//Redirect
				Response.Redirect("SPK_CharDesign01.aspx?id=" + myID + "&tipe=" + tipe);
			}
		}
	}
	protected void saveData(ProductOrder ms) {
		var d = ms.ProductOrder_CharProduct;
		if (d == null) LinkError();
		else {
			d.GrossWeight = Convert.ToDecimal(grossweight.Text);
			d.NettoWeight = d.GrossWeight;
			d.KadarLogam = Convert.ToDecimal(kadarlogam.Text);
			d.KadarTukaran = Convert.ToDecimal(kadartukaran.Text);

			//Dimensi
			d.DimensiD = Convert.ToDecimal(0); // dimensi dinonaktifkan
			d.DimensiP = Convert.ToDecimal(dimensip.Text);
			d.DimensiL = Convert.ToDecimal(dimensil.Text);

			//Ring-Size
			if (dimensir.Text != "")
				d.DimensiR = Convert.ToDecimal(dimensir.Text);
			else
				d.DimensiR = null;

            //Ring-Size 2
            if (dimensir2.Text != "")
                d.DimensiR2 = Convert.ToDecimal(dimensir2.Text);
            else
                d.DimensiR2 = null;

			//Commit
			db.SubmitChanges();
		}
	}
}