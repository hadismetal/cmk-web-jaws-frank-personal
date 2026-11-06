using System;
using System.Linq;

public partial class Page_Create_CustomerAccount : System.Web.UI.Page {
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
	protected int myIDBrand { get { return App.QueryInt(this, "idbrand"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			nama.Focus();

			//Form
			fillForm();
            DLC.OutletExhibitionList(location);

			//Form-Default
			tgl.Text = Str.Tgl(DateTime.Today);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Customer.aspx"); }

	//Form
	protected void fillForm() {


		var d2 = db.CompanyBrand.SingleOrDefault(p => p.ID == myIDBrand && !p.Disable);
		if (d2 == null) DataError();
		else
			brand.InnerHtml = d2.Nama + " (" + d2.NamaKode + ")";
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {

            string Kode = location.SelectedValue;
            int IDBrand = myIDBrand;

            string HariLahir = "";
            string BulanLahir = "";
            string TahunLahir = "";
            if (tgllahir.Text != "")
            {
                DateTime TglLahir = Convert.ToDateTime(tgllahir.Text);
                HariLahir = TglLahir.Day.ToString();
                BulanLahir = TglLahir.Month.ToString();
                TahunLahir = TglLahir.Year.ToString();
            }

            string MKode = Db.SingleString("DECLARE	@return_value int," +
            "@m_kode varchar(20)" +
            "SELECT	@m_kode = N''" +
            "EXEC @return_value = sp_savecustomer " +
            "@m_kode = @m_kode OUTPUT," +
            "@m_group = @m_kode," +
            "@m_nama = N'" + nama.Text + "'," +
            "@m_alamat = N''," +
            "@m_kota = N''," +
            "@m_telepon1 = N''," +
            "@m_telepon2 = N''," +
            "@m_fax = N''," +
            "@m_email = N''," +
            "@m_npwp = N''," +
            "@m_status = N'00'," +
            "@m_tmplahir = N''," +
            ((tgllahir.Text != "") ? "@m_tgllahir = N'" + Convert.ToDateTime(tgllahir.Text) + "'," : "@m_tgllahir = N'',") +
            "@m_agama = N'" + agama.SelectedValue + "'," +
            "@m_cabang = N'" + Kode + "'," +
            "@m_kodesales = N''," +
            "@m_referensi = @m_kode," +
            "@m_pinbb = N''," +
            ((tgllahir.Text != "") ? "@m_tanggal = " + HariLahir + "," : "@m_tanggal = 0,") +
            ((tgllahir.Text != "") ? "@m_bulan = " + BulanLahir + "," : "@m_bulan = 0,") +
            ((tgllahir.Text != "") ? "@m_tahun = " + TahunLahir + "," : "@m_tahun = 0,") +
            "@m_ktp = N''," +
            "@m_validktp = N''," +
            "@m_email2 = N''," +
            "@loginid = N''" +
            "SELECT	@m_kode as N'@m_kode'" +
            "SELECT	'Return Value' = @return_value", Db.Cnn_CS);

            var cst = cs.Mscustomer.SingleOrDefault(p => p.M_kode.Equals(MKode));

            DataCustomer n = new DataCustomer();

            //Master
            n.IDBrand = IDBrand;

            //Account
            n.Nama = Str.CapTitle(nama.Text);
            n.Keterangan = "";
            n.Tgl = Convert.ToDateTime(tgl.Text);
            if (tgllahir.Text != "")
                n.TglLahir = Convert.ToDateTime(tgllahir.Text);
            n.Agama = Convert.ToInt32(agama.SelectedValue);
            n.NoCustomer = MKode;

            //Status
            n.Draft = true;
            n.DraftDate = DateTime.Now;
            n.Disable = false;
            n.Prospect = true;

            //Commit
            db.DataCustomer.InsertOnSubmit(n);
            db.SubmitChanges();

			//Redirect
			Response.Redirect("CustomerAddr.aspx?id=" + n.ID);
		}
	}

    protected string Generate_Nomor(string Kode)
    {
        string x = string.Empty;

        int Tahun = DateTime.Today.Year;
        int Bulan = DateTime.Today.Month;

        /*int Nomor = db.StockReceiveDJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;

        return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        int Nomor = 1;

        try
        {
            /*var s = (from p in db.DataCustomer
                     where p.Tgl.Year == Tahun 
                     && p.NoCustomer != null
                     orderby p.NoCustomer.Remove(3).EndsWith(Kode) descending
                     select p).First();*/

            Nomor = Db.SingleInteger("SELECT TOP 1 CAST(RIGHT(NoCustomer,6) AS INT) FROM DataCustomer WHERE NoCustomer is not null "
                        + "AND LEFT(NoCustomer,3) = '" + Kode + "'"
                        + "AND LEN(NoCustomer) = 11" 
                        + "ORDER BY RIGHT(NoCustomer,6) DESC", Db.Cnn_DB);

            //if (s.NoCustomer != null)
            Nomor = Nomor + 1;
        }
        catch (Exception ex) { }

        string PLU = Kode + Tahun.ToString().Remove(0, 2) + Str.KodeFormat(Nomor, 6);

        return PLU;

    }
}