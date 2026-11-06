using System;
using System.Linq;

public partial class Page_Create_CustomerAddr : System.Web.UI.Page {
	private Map_DB _db;
	private Map_CS _cs;

	private Map_DB db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_DB();
	        return _db;
	    }
	}

	private Map_CS cs
	{
	    get
	    {
	        if (_cs == null)
	            _cs = new Map_CS();
	        return _cs;
	    }
	}

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (cs != null)
	        cs.Dispose();
	    base.Dispose();
	}
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			addralamat.Focus();

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Customer.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DataCustomer.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/CustomerFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.DataCustomer.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
            string NoCustomer = d.NoCustomer;

            var u = cs.Mscustomer.SingleOrDefault(p=>p.M_kode.Equals(d.NoCustomer));

            string MKode = Db.SingleString("DECLARE	@return_value int," +
            "@m_kode varchar(20)" +
            "SELECT	@m_kode = N''" +
            "EXEC @return_value = sp_savecustomer " +
            "@m_kode = N'" + NoCustomer + "'," +
            "@m_group = @m_kode," +
            "@m_nama = N'" + d.Nama + "'," +
            "@m_alamat = N'" + Str.CapTitle(addralamat.Text) + "'," +
            "@m_kota = N''," +
            "@m_telepon1 = N'" + Str.CapUpper(addrnotelp1.Text) + "'," +
            "@m_telepon2 = N'" + Str.CapUpper(addrnotelp2.Text) + "'," +
            "@m_fax = N'" + Str.CapUpper(addrnofax.Text) + "'," +
            "@m_email = N'" + Str.CapLower(addremail.Text) + "'," +
            "@m_npwp = N''," +
            "@m_status = N'00'," +
            "@m_tmplahir = N''," +
            "@m_tgllahir = N'" + d.TglLahir + "'," +
            "@m_agama = N'" + d.Agama + "'," +
            "@m_cabang = N'" + u.M_cabang + "'," +
            "@m_kodesales = N''," +
            "@m_referensi = N'" + u.M_kode +"'," +
            "@m_pinbb = N'" + Str.CapUpper(pinbb.Text) + "'," +
            "@m_tanggal = N'" + u.M_tanggal + "'," +
            "@m_bulan = N'" + u.M_bulan + "'," +
            "@m_tahun = N'" + u.M_tahun + "'," +
            "@m_ktp = N''," +
            "@m_validktp = N''," +
            "@m_email2 = N''," +
            "@loginid = N''" +
            "SELECT	@m_kode as N'@m_kode'" +
            "SELECT	'Return Value' = @return_value", Db.Cnn_CS);

			d.AddrAlamat = Str.CapTitle(addralamat.Text);
			d.AddrNoTelp = Str.CapUpper(addrnotelp1.Text);
            d.AddrNoTelp2 = Str.CapUpper(addrnotelp2.Text);
            d.AddrNoTelp3 = Str.CapUpper(addrnotelp3.Text);
            d.AddrMobile = Str.CapUpper(addrnohp.Text);
			d.AddrNoFax = Str.CapUpper(addrnofax.Text);
			d.AddrEmail = Str.CapLower(addremail.Text);
            d.PinBB = Str.CapUpper(pinbb.Text);

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("CustomerAddInfo.aspx?id=" + myID);
		}
	}
}