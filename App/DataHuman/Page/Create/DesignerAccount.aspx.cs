using System;
using System.Linq;
using System.Data;

public partial class Page_Create_DesignerAccount : System.Web.UI.Page {
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

    private Map_AC _ac;

    private Map_AC ac
    {
        get
        {
            if (_ac == null)
                _ac = new Map_AC();
            return _ac;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (ac != null)
	        ac.Dispose();
	    base.Dispose();
	}
	protected int myIDGroup { get { return App.QueryInt(this, "idgroup"); } }
	protected int myIDBrand { get { return App.QueryInt(this, "idbrand"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			nama.Focus();

			//Form
			fillForm();

			//Form-Default
			//tgl.Text = Str.Tgl(DateTime.Today);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Designer.aspx"); }

	//Form
	protected void fillForm() {
        //DLC.User(username);

		var d1 = db.DataHumanGroup.SingleOrDefault(p => p.ID == myIDGroup && !p.Disable);
		if (d1 == null) DataError();
		else
			group.InnerHtml = d1.Nama + " (" + d1.NamaKode + ")";

		var d2 = db.CompanyBrand.SingleOrDefault(p => p.ID == myIDBrand && !p.Disable);
		if (d2 == null) DataError();
		else
			brand.InnerHtml = d2.Nama + " (" + d2.NamaKode + ")";
	}

	//Save
    protected bool valid
    {
        //get { return Fv.isDateTime(tgl, tgl_ERR); }

        get { return true; }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {

            string NIK = txt_nik.Text.Trim().ToUpper();
            string NAMA = nama.Text.Trim().ToUpper();
            string KETERANGAN = keterangan.Text.Trim().ToUpper();

            try
            {
                if (NIK != "")
                {
                    string[] infokaryawan = getNamaKaryawan(NIK).Split('-');
                    NAMA = infokaryawan[0];
                    KETERANGAN = infokaryawan[1];
                }
            }
            catch (Exception ex) { }

			DataHuman n = new DataHuman();

			//Master
			n.IDGroup = myIDGroup;
			n.IDBrand = myIDBrand;
            //n.IDUser = Convert.ToInt32(username.SelectedValue);

			//Account
            n.Keterangan = Str.CapFirst(KETERANGAN);
            n.Tgl = System.DateTime.Today; // Convert.ToDateTime(tgl.Text);
            n.NIK = NIK;
            n.Nama = Str.CapTitle(NAMA);

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
			n.Disable = true;

			//Commit
			db.DataHuman.InsertOnSubmit(n);
			db.SubmitChanges();

			//Redirect
			Response.Redirect("DesignerAddr.aspx?id=" + n.ID);
		}
	}

    //btn_clear
    protected void btn_clear_Click(object sender, EventArgs e)
    {
        nama.Text = "";
        keterangan.Text = "";

        nama.Enabled = true;
        keterangan.Enabled = true;
    }

    protected void btn_search_nik_Click(object sender, EventArgs e)
    {
        try
        {
            string nikkaryawan = txt_nik.Text.Trim();

            string[] infokaryawan = getNamaKaryawan(nikkaryawan).Split('-');

            nama.Text = infokaryawan[0];
            keterangan.Text = infokaryawan[1];

            nama.Enabled = false;
            keterangan.Enabled = false;
        }
        catch (Exception ex) { }
    }

    protected string getNamaKaryawan(string nik)
    {
        string result = "";
        string namakaryawan = "";
        string jabatankaryawan = "";
        string emailkaryawan = "";
        string spname = "sp_getdatakaryawan";
        string query = " exec " + spname
                     + " '" + nik + "'"
                     ;
        try
        {
            DataTable dt = new DataTable();
            dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    namakaryawan = dtRow["m_nama"].ToString();
                    jabatankaryawan = dtRow["m_jabatan"].ToString();
                    emailkaryawan = dtRow["m_emailkantor"].ToString();
                    result = namakaryawan
                           + "-" + jabatankaryawan
                           + "-" + emailkaryawan
                           ;
                }
            }
        }
        catch (Exception) { }

        return result;
    }
}