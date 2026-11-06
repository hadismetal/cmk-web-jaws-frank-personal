using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_PG_PricingSubmit : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
			DLC.CharFrameColor(framecolor);
			Ddl.FisikBarangQC(fisik);
			Ddl.GrafirLogoQC(logo);
			Ddl.GrafirKadarEmasQC(kadaremas);
			kadarkaratimeter.SelectedValue = "1";
            TipeLocation(tipe_lokasi);
            Location(lokasi, Convert.ToInt32(tipe_lokasi.SelectedValue));
            fillForm();
        }
    }

    protected string getLocationUser()
    {
        string result = "";
        int xUserID = App.UserID;
        int xisAM = 0;
        string listStore = "";
        //check area manager
        var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
        int amcount = am.Count();
        if (amcount > 0)
        {
            int i = 0;
            foreach (var x in am)
            {
                int j = 0;
                var lokasi = x.DataAreaManager_Lokasi
                    .GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
                    .Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
                foreach (var y in lokasi)
                {
                    //listStore = "";
                    if (y.TipeLokasi == 1)
                        listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
                    else
                        listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
                    j++;
                    listStore += ",";
                }
                i++;
            }
            xisAM = 1;
        }

        if (xisAM == 0)
        {
            //check admin sales
            var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
            if (ad.Count() > 0)
            {
                int k = 0;
                foreach (var x in ad)
                {
                    listStore = "";
                    if (x.TipeLokasi == 1)
                        listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
                    else
                        listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
                    k++;
                    listStore += ",";
                }
            }
        }

        result = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;

        return result;
    }

    protected void Location(DropDownList ddl, int tipelokasi)
    {
        string keyloc = getLocationUser();
        string query = string.Empty;
        if (tipelokasi == -1)
        {
            query = "SELECT A.* FROM (" +
                "SELECT TOP 0 ID, Kode, UPPER(Nama) [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
                ") A";
        }
        else
        if (tipelokasi == 1)
        {
            query = "SELECT A.* FROM(" +
                "SELECT ID, Kode, UPPER(Nama) [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',','))))" +
                ") A";
        }
        else if (tipelokasi == 2)
        {
            query = "SELECT A.* FROM(" +
                "SELECT ID, Kode, UPPER(Nama) [Nama] FROM LocExhibition WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',','))))" +
                ") A";
        }
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("All", "-1"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Kode"].ToString() + " (" + item["Nama"].ToString() + ")";
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }

    protected void Location_Changed(object sender, EventArgs e)
    {
        Location(lokasi, Convert.ToInt32(tipe_lokasi.SelectedValue));
    }

    protected void TipeLocation(DropDownList ddl)
    {
        string keyloc = getLocationUser();

        string query = "SELECT A.* FROM (" +
            "SELECT TOP 1 1 [Tipe], 'Outlet' [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
            "UNION ALL " +
            "SELECT TOP 1 2 [Tipe], 'Exhibition' [Nama] FROM LocExhibition WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
            ") A";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("Pilih Tipe Lokasi", "-1"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["Tipe"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.StockProductPG.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml = d.Nomor;
			
			string pqc = "";
			using (Map_AC w = new Map_AC())
			{
				var user = w.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
				if (user != null)
					pqc = user.Nama + " (" + user.UserNumber +")"; 
			}
			
			petugasqc.Text = pqc;

            fillProduct(d);

        }
    }

    protected void fillProduct(StockProductPG ms)
    {
        netto.InnerHtml = Str.Number2(ms.NettoWeight);
        kadar.InnerHtml = Str.Number2(Math.Ceiling(ms.KadarLogam.Value));
        frame.InnerHtml = ms.CharFrameColor.Nama;
        netweighthidden.Value = Str.Number2(ms.NettoWeight);
        framecolor.SelectedValue = ms.CharFrameColor.ID.ToString();
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
		string msg = validateFillQC();
		if(string.IsNullOrEmpty(msg))
		{
			var d = db.StockProductPG.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else
			{

				DocQCPG x = new DocQCPG();

				x.QCNama = petugasqc.Text;
				x.Keterangan = keterangan.Text;
				x.Nomor = Document.Generate_Nomor_QCPG(x);
				x.IDProduct = d.ID;
				x.Logo = logo.Text;
				x.KadarLogam = kadaremas.Text;
				//x.Merk = merk.Text;
				x.FisikKeseluruhan = fisik.Text;
				x.NetWeight = String.Format("{0:0.00}", netweight.Text.Trim());
				x.WarnaEmas = framecolor.SelectedItem.Text;
				x.OperatorNama = App.UserNama;
				x.OperatorTgl = DateTime.Now;
				x.Draft = false;
				x.DraftDate = null;

				db.DocQCPG.InsertOnSubmit(x);
				db.SubmitChanges();

                string query = "UPDATE DocQCPG SET TipeLokasi = @TipeLokasi, IDLokasi = @IDLokasi, KadarKaratimeter = @KadarKaratimeter WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@TipeLokasi", SqlDbType.Int){ Value = Convert.ToInt32(tipe_lokasi.SelectedValue) },
                    new SqlParameter("@IDLokasi", SqlDbType.Int){ Value = Convert.ToInt32(lokasi.SelectedValue) },
                    new SqlParameter("@KadarKaratimeter", SqlDbType.VarChar){ Value = kadarkaratimeter.SelectedItem.Text },
                    new SqlParameter("@ID", SqlDbType.Int){ Value = Convert.ToInt32(x.ID) }
                };
                Execute(query, Db.Cnn_DB, parameters, CommandType.Text);

                //Db.Execute("UPDATE DocQCPG SET KadarKaratimeter = '"+ kadarkaratimeter.SelectedItem.Text +"'  WHERE ID = " + x.ID, Db.Cnn_DB);

				//Redirect
				//App.ReloadClose(this);
				//Response.Redirect("PG_Detail.aspx?id=" + x.ID);

                // Alert + Redirect
                string url = "../../Report/QCPG.aspx?refreshPage=1&kw=" + x.Nomor + "&dari=" + Convert.ToDateTime(x.OperatorTgl).ToString("dd-MM-yyyy") + "&sampai=" + Convert.ToDateTime(x.OperatorTgl).ToString("dd-MM-yyyy");
                string script = "alert('Data saved successfully! Please Approve QC PG before use on '); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert", script, true);
            }
			lblwarning.Visible = false;
            lblwarning.InnerText = "";
		}
		else
		{
			lblwarning.Visible = true;
            lblwarning.InnerText = msg;
		}
    }
	
	protected void btnSearchNIK_Click(object sender, EventArgs e)
    {
		lblwarning.Visible = false;
        string nik = petugasqcsearch.Text;
        DataTable dt = new DataTable();
        dt = Db.Rs("SELECT TOP 100 m_nik, m_nama FROM dbhrd.dbo.mskaryawan WHERE m_nik = '" + nik + "' ", Db.Cnn_CS);

        int rowcount = dt.Rows.Count;
        if(rowcount == 1)
        {
            petugasqc.Text = dt.Rows[0]["m_nama"].ToString().ToUpper() + " (" + dt.Rows[0]["m_nik"].ToString() + ")";
            lblerrorpetugasqc.InnerText = "";
            lblerrorpetugasqc.Visible = false;
        }
        else if(rowcount == 0)
        {
            petugasqc.Text = "";
            lblerrorpetugasqc.InnerText = "NIK tidak ditemukan.";
            lblerrorpetugasqc.Visible = true;
        }
        else
        {
            petugasqc.Text = "";
            lblerrorpetugasqc.InnerText = "NIK duplikasi.";
            lblerrorpetugasqc.Visible = true;
        }
    }
	
	protected string validateFillQC()
    {
        string message = "";
        if (string.IsNullOrEmpty(petugasqc.Text))
        {
            message = "Petugas QC wajib diinput.";
            petugasqc.Focus();
        }

        string netweightactual = String.Format("{0:0.00}", netweight.Text.Trim());
        decimal netweight1 = 0m;
        bool isnumber = decimal.TryParse(netweightactual, out netweight1);
        if (!isnumber)
        {
            message = "Berat harus berupa angka / nominal.";
            netweight.Focus();
        }

        if (string.IsNullOrEmpty(message))
        {
            if (!(Convert.ToInt32(tipe_lokasi.SelectedValue) >= 0))
            {
                message = "Tipe Lokasi wajib dipilih";
                tipe_lokasi.Focus();
            }
        }

        if (string.IsNullOrEmpty(message))
        {
            if (!(Convert.ToInt32(lokasi.SelectedValue) > 0))
            {
                message = "Lokasi wajib dipilih";
                lokasi.Focus();
            }
        }

        return message; 
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = cmdType;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
            cmd.ExecuteNonQuery();
        }
    }
}