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

            //Form-Default
            kadarlogam.Text = Str.Number2(CV.KadarLogam);
            kadartukaran.Text = Str.Number2(CV.KadarTukaran);

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
        var d = db.ProductDesign_CharProduct.SingleOrDefault(p => p.ID == myID && p.ProductDesign.Approval == null);
        if (d == null)
        {
            var x = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Approval==null);
            if (x == null) DataError();
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + x.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + x.Nomor + "</a>";
        }
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.ProductDesign.Nomor + "</a>";

            
            kadarlogam.Text = Str.Number2(d.KadarLogam);
            kadartukaran.Text = Str.Number2(d.KadarTukaran);
            grossweight.Text = Str.Number2(d.GrossWeight);
            dimensid.Text = Str.Number2(d.DimensiD);
            dimensip.Text = Str.Number2(d.DimensiP);
            dimensil.Text = Str.Number2(d.DimensiL);
            dimensir.Text = Str.Number2(d.DimensiR);
            
            var dimR2 = db.ExecuteQuery<decimal?>(
					"SELECT DimensiR2 FROM ProductDesign_CharProduct WHERE ID = {0}",
					myID
				).FirstOrDefault();

            dimensir2.Text = dimR2.HasValue ? Str.Number2(dimR2.Value) : "0";
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
        if (valid)
        {
            var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Approval==null);
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
    protected void saveData(ProductDesign ms)
    {
        var d = ms.ProductDesign_CharProduct;
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
            UpdateDimensiR2();
            db.SubmitChanges();
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