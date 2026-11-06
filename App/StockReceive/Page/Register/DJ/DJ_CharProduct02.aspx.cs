using System;
using System.Linq;

public partial class Page_Register_DJ_CharProduct02 : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
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

            // ApplyConditionalValidation();
            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJ.aspx"); }
    protected void LinkError() { Response.Redirect("DJ_CharProduct01.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            if (d.Proto != null)
            {
                var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (proto != null)
                {
                    kadarlogam.Text = Str.Number2(proto.ProductOrder.ProductOrder_CharProduct.KadarLogam);
                    kadartukaran.Text = Str.Number2(proto.ProductOrder.ProductOrder_CharProduct.KadarTukaran);
                    grossweight.Text = Str.Number2(proto.ProductOrder.ProductOrder_CharProduct.GrossWeight);
                    dimensid.Text = Str.Number2(proto.ProductOrder.ProductOrder_CharProduct.DimensiD);
                    dimensip.Text = Str.Number2(proto.ProductOrder.ProductOrder_CharProduct.DimensiP);
                    dimensil.Text = Str.Number2(proto.ProductOrder.ProductOrder_CharProduct.DimensiL);
                    dimensir.Text = Str.Number2(proto.ProductOrder.ProductOrder_CharProduct.DimensiR);
                }
            }
            if (d.IDMounting != null)
            {
                var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting);
                if (mt != null)
                {
                    kadarlogam.Text = Str.Number2(mt.StockReceiveMounting_CharProduct.KadarLogam);
                    kadartukaran.Text = Str.Number2(mt.StockReceiveMounting_CharProduct.KadarTukaran);
                    grossweight.Text = Str.Number2(mt.StockReceiveMounting_CharProduct.GrossWeight);
                    dimensid.Text = Str.Number2(mt.StockReceiveMounting_CharProduct.DimensiD);
                    dimensip.Text = Str.Number2(mt.StockReceiveMounting_CharProduct.DimensiP);
                    dimensil.Text = Str.Number2(mt.StockReceiveMounting_CharProduct.DimensiL);
                    dimensir.Text = Str.Number2(mt.StockReceiveMounting_CharProduct.DimensiR);
                }
            }
        }
    }

    //Save
    protected bool valid
    {
        get
        {
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

			var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);

			if (d != null && d.StockReceiveDJ_CharProduct != null)
			{
				int productItem = d.StockReceiveDJ_CharProduct.ProductItem;

				var ringTypes = new[] { 54, 73, 88, 64, 91, 84, 92 };
				bool isRingType = ringTypes.Contains(productItem);

				if (isRingType)
				{
					bool ring1Empty = string.IsNullOrEmpty(dimensir.Text) || dimensir.Text == "0";
					bool ring2Empty = string.IsNullOrEmpty(dimensir2.Text) || dimensir2.Text == "0";

					// validasi untuk ring size: minimal salah satu harus diisi
					if (ring1Empty && ring2Empty)
					{
						dimensir_ERR.InnerText = "Required (fill one)";
						dimensir_ERR.Visible = true;

						dimensir2_ERR.InnerText = "Required (fill one)";
						dimensir2_ERR.Visible = true;

						x = false;
					}
				}
				else
				{
					if (dimensid.Text == "")
					{
                        dimensid_ERR.InnerText = "Required";
						dimensid_ERR.Visible = true;
						x = false;
					}

					if (dimensip.Text == "")
					{
						dimensip_ERR.InnerText = "Required";
						dimensip_ERR.Visible = true;
						x = false;
					}

					if (dimensil.Text == "")
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
    protected bool valid_GrossWeight
    {
        get
        {
            decimal dv = Convert.ToDecimal(grossweight.Text);

            bool x = dv > 0;
            grossweight_VAL.Visible = !x;

            return x;
        }
    }
    protected bool valid_KadarLogam
    {
        get
        {
            decimal dv = Convert.ToDecimal(kadarlogam.Text);

            bool x = dv > 0;
            kadarlogam_VAL.Visible = !x;

            return x;
        }
    }
    protected bool valid_KadarTukaran
    {
        get
        {
            decimal dv = Convert.ToDecimal(kadartukaran.Text);

            bool x = dv > 0;
            kadartukaran_VAL.Visible = !x;

            return x;
        }
    }
    protected void save_Click(object sender, EventArgs e)
    {
        if (!valid)
			return;

        if (valid)
        {
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) DataError();
            else
            {
                //Process
                saveData(d);

                //Redirect
                Response.Redirect("DJ_CharDesign01.aspx?id=" + myID);
            }
        }
    }
    protected void saveData(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_CharProduct;
        if (d == null) LinkError();
        else
        {
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
			"UPDATE StockReceiveDJ_CharProduct SET DimensiR2 = {0} WHERE ID = {1}",
			string.IsNullOrEmpty(dimensir2.Text)
				? (object)DBNull.Value
				: Convert.ToDecimal(dimensir2.Text),
			myID
		);
	}
}