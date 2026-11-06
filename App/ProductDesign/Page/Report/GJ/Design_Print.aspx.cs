using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Design_Design : System.Web.UI.Page {
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
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Summary.aspx"); }

	//Form
    protected void fillForm()
    {
        decimal TotalBatu = 0m;
        decimal TotalKarat = 0m;

        var rs = db.ProductDesignGJ.SingleOrDefault(x => x.ID == Convert.ToInt32(myID));

        if (rs != null)
        {
            tgl.Text = Str.FullDate(rs.Tgl);

            estimasi_harga.Text = (Convert.ToDecimal(rs.ProductDesignGJ_PricingMU.HargaRupiahRound).ToString("N0"));
            Kode.Text = rs.Nomor;
            NamaDesigner.Text = rs.DataHuman.Nama;
            ApprovedBy.Text = rs.Approval;
            Toko.Text = rs.DataHuman.CompanyBrand.Nama;
            customernama.Text = rs.RequestCustomerNama;

            Upload.Image(imgpicture, rs.ImgPicture);
        }
    }
}