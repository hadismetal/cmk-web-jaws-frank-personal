using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Report_QCPG_Approve : System.Web.UI.Page
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
    protected DateTime TglCutOff = Convert.ToDateTime("2026-03-05");

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
		
		if (!IsPostBack)
		{
			RedirectToHome();

            //var data = db.DocQCPG.SingleOrDefault(p => p.ID == myID).StockProductPG.StockActualPG;

            DataTable dt = Rs("IF EXISTS (SELECT 1 FROM DocQCPG WHERE ID = @ID AND TipeLokasi IS NOT NULL AND IDLokasi IS NOT NULL) " +
                "BEGIN " +
                "SELECT TOP 1 A.ID, A.TipeLokasi, A.IDLokasi, C.Nama, C.Brand, C.AddrAlamat FROM DocQCPG A " +
                "JOIN (SELECT A.ID, 1 Tipe, A.Nama, A.Kode, B.Nama [Brand], A.AddrAlamat FROM LocOutlet A " +
                "JOIN CompanyBrand B ON A.IDBrand = B.ID " +
                "UNION ALL " +
                "SELECT A.ID, 2 Tipe, A.Nama, A.Kode, B.Nama [Brand], A. AddrAlamat FROM LocExhibition A " +
                "JOIN CompanyBrand B ON A.IDBrand = B.ID) C ON A.IDLokasi = C.ID AND A.TipeLokasi = C.Tipe " +
                "WHERE A.ID = @ID " +
                "END " +
                "ELSE " +
                "BEGIN " +
                "SELECT TOP 1 A.ID, A.TipeLokasi, A.IDLokasi, C.Nama, C.Brand, C.AddrAlamat FROM DocQCPG A " +
                "JOIN StockActualPG B ON A.IDProduct = B.IDProduct " +
                "JOIN (SELECT A.ID, 1 Tipe, A.Nama, A.Kode, B.Nama [Brand], A.AddrAlamat FROM LocOutlet A " +
                "JOIN CompanyBrand B ON A.IDBrand = B.ID " +
                "UNION ALL " +
                "SELECT A.ID, 2 Tipe, A.Nama, A.Kode, B.Nama [Brand], A.AddrAlamat FROM LocExhibition A " +
                "JOIN CompanyBrand B ON A.IDBrand = B.ID) C ON B.IDLokasi = C.ID AND B.TipeLokasi = C.Tipe " +
                "WHERE A.ID = @ID " +
                "END", Db.Cnn_DB, new List<SqlParameter>() { new SqlParameter("@ID", SqlDbType.Int) { Value = myID } }, CommandType.Text);
            //int TipeLokasi = Convert.ToInt32(dt.Rows[0]["TipeLokasi"]);
            //int IDLokasi = Convert.ToInt32(dt.Rows[0]["IDLokasi"]);
            string brand = dt.Rows[0]["Brand"].ToString();
            string alamat = dt.Rows[0]["AddrAlamat"].ToString();

            string CB = ""; // mondial, franknco, missmondial, thepalace
			string Alamat = "";
			

			CB = CB.ToLower();

			if (CB == "mondial") { img_logo_web.Src = "/Images/web/logo_web_mondial.jpg"; }
			else if (CB == "miss mondial") { img_logo_web.Src = "/Images/web/logo_web_missmondial.jpg"; }
			else if (CB == "the palace") { img_logo_web.Src = "/Images/web/logo_web_thepalace.jpg"; }
			else if (CB == "frank & co") { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }
			else { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }

			cabang.Text = Alamat;
			lit_nomor_repair.Text = db.DocQCPG.SingleOrDefault(p => p.ID == myID).Nomor;

			fillForm();
		}
    }

    //Form
    protected void fillForm()
    {
        var q = db.DocQCPG.SingleOrDefault(p => p.ID == myID);
        int tipelokasi = Convert.ToInt32(Db.Rs("SELECT TOP 1 ISNULL(TipeLokasi,-1) TipeLokasi FROM DocQCPG WHERE ID = " + myID, Db.Cnn_DB).Rows[0]["TipeLokasi"]);
        int idlokasi = Convert.ToInt32(Db.Rs("SELECT TOP 1 ISNULL(IDLokasi,-1) IDLokasi FROM DocQCPG WHERE ID = " + myID, Db.Cnn_DB).Rows[0]["IDLokasi"]);
        if (q == null) App.DataNull(this);
        else
        {
            try
            {
                //Header
                nodocqc.InnerText = q.Nomor;
                tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
                plu.InnerText = q.StockProductPG.Nomor;
                netto.InnerText = Convert.ToDecimal(q.StockProductPG.NettoWeight).ToString("N2") + " gram";
                frame.InnerText = q.StockProductPG.CharFrameColor.Nama + " (" + q.StockProductPG.CharFrameColor.NamaKode + ")";
                kadar.InnerText = Convert.ToDecimal(q.StockProductPG.KadarLogam).ToString("N0") + " %";
                petugasqc.InnerHtml = "<span style='font-size:11px;'>" + q.QCNama.ToUpper() + "</span>";
                logo.InnerText = q.Logo;
                kadaremas.InnerText = q.KadarLogam;
                fisik.InnerText = q.FisikKeseluruhan;
                netweight.InnerText = Convert.ToDecimal(q.NetWeight).ToString("N2") + " gram";
                kadarkaratimeter.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(KadarKaratimeter,'') [KadarKaratimeter] FROM DocQCPG WHERE ID = " + myID, Db.Cnn_DB);
                framecoloractual.InnerText = q.WarnaEmas;
                keterangan.InnerText = q.Keterangan;
                //Upload.Image(imgpicture, q.StockProductPG.ImgPicture);
                string url = ConfigurationManager.AppSettings["ImageAPI"];
                imgpicture.ImageUrl = url + q.StockProductPG.ImgPicture;
            }
            catch
            {
                //Header
                nodocqc.InnerText = q.Nomor;
                tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
                plu.InnerText = q.StockProductPG.Nomor;
                netto.InnerText = Convert.ToDecimal(q.StockProductPG.NettoWeight).ToString("N2") + " gram";
                frame.InnerText = q.StockProductPG.CharFrameColor.Nama + " (" + q.StockProductPG.CharFrameColor.NamaKode + ")";
                kadar.InnerText = Convert.ToDecimal(q.StockProductPG.KadarLogam).ToString("N0") + " %";
                petugasqc.InnerHtml = "<span style='font-size:11px;'>" + q.QCNama.ToUpper() + "</span>";
                logo.InnerText = q.Logo;
                kadaremas.InnerText = q.KadarLogam;
                fisik.InnerText = q.FisikKeseluruhan;
                netweight.InnerText = q.NetWeight + " gram";
                kadarkaratimeter.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(KadarKaratimeter,'') [KadarKaratimeter] FROM DocQCPG WHERE ID = " + myID, Db.Cnn_DB);
                framecoloractual.InnerText = q.WarnaEmas;
                keterangan.InnerText = q.Keterangan;
                //Upload.Image(imgpicture, q.StockProductPG.ImgPicture);
                string url = ConfigurationManager.AppSettings["ImageAPI"];
                imgpicture.ImageUrl = url + q.StockProductPG.ImgPicture;
            }

            string warning = string.Empty;
            if (IsStoreManager(idlokasi, tipelokasi, ref warning))
            {
                reject.Visible = true;
                approve.Visible = true;
                lbl_warning.Visible = false;
                lbl_warning.InnerText = string.Empty;
            }
            else
            {
                reject.Visible = false;
                approve.Visible = false;
                lbl_warning.Visible = true;
                lbl_warning.InnerText = warning;
            }
        }
    }

    protected bool IsStoreManager(int idlokasi, int tipelokasi, ref string warning)
    {
        bool IsSM = false;
        using (Map_AC ac = new Map_AC())
        {
            UserAccount user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (Role.Manager)
            {
                if (idlokasi >= 0 && tipelokasi > -1)
                {
                    string kodelokasi = string.Empty;
                    if (tipelokasi == 1)
                        kodelokasi = db.LocOutlet.SingleOrDefault(p => p.ID == idlokasi).Kode;
                    else if(tipelokasi == 2)
                        kodelokasi = db.LocExhibition.SingleOrDefault(p => p.ID == idlokasi).Kode;
                    string jabatan = user.Keterangan.ToString();
                    if (jabatan.Contains("JEWELLERY REPRESENTATIVE") || jabatan.Contains("GOLD REPRESENTATIVE") || jabatan.Contains("ADMINISTRATIVE EXECUTIVE"))
                    {
                        IsSM = false;
                        warning = "Only SM / AM Who Can Approve / Reject.";
                    }
                    else if (jabatan.Contains("STORE MANAGER") || jabatan.Contains("AREA MANAGER") || jabatan.Contains("JEWELLERY COORDINATOR"))
                    {
                        if (tipelokasi != 2)
                        {
                            IsSM = getLocationUser(kodelokasi);
                            if (!IsSM)
                                warning = "SM / AM Not In This Store.";
                        }
                        else
                        {
                            IsSM = true;
                        }
                    }
                    else
                    {
                        IsSM = true;
                    }
                }
                else
                {
                    IsSM = true;
                }
            }
        }
        return IsSM;
    }

    protected bool getLocationUser(string kodelokasi)
    {
        bool isSM = false;
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

        if (result.Contains(kodelokasi))
            isSM = true;

        return isSM;
    }

    protected void Approve_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs("SELECT TOP 1 Nomor, RejectNama, RejectTgl, OperatorTgl FROM DocQCPG WHERE ID = @ID", Db.Cnn_DB, parameters, CommandType.Text);
        bool IsRejected = dt.Rows[0]["RejectNama"] == DBNull.Value ? false : true;
        DateTime TglQC = Convert.ToDateTime(dt.Rows[0]["OperatorTgl"]);
        string NomorQC = dt.Rows[0]["Nomor"].ToString();

        if (TglCutOff <= TglQC)
        {
            if (IsRejected)
            {
                // Alert + Redirect
                string url = "QCPG.aspx";
                string script = "alert('QC " + NomorQC + " Sudah Rejected! Tolong Buat QC Ulang'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert", script, true);
            }
            else
            {
                string appnama = App.UserNama;
                string queryupadte = "UPDATE DocQCPG SET ApprovalNama = @ApprovalNama, ApprovalTgl = @ApprovalTgl WHERE ID = @ID";
                List<SqlParameter> parameters2 = new List<SqlParameter>()
                {
                    new SqlParameter("@ApprovalNama", SqlDbType.VarChar){ Value = appnama },
                    new SqlParameter("@ApprovalTgl", SqlDbType.DateTime){ Value = DateTime.Now },
                    new SqlParameter("@ID", SqlDbType.VarChar){ Value = myID }
                };
                Execute(queryupadte, Db.Cnn_DB, parameters2, CommandType.Text);

                // Alert + Redirect
                string url = "QCPG_Detail.aspx?id=" + myID;
                string script = "alert('Data success approved!'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert2", script, true);
            }
        }
        else
        {
            string appnama = App.UserNama;
            string queryupadte = "UPDATE DocQCPG SET ApprovalNama = @ApprovalNama, ApprovalTgl = @ApprovalTgl WHERE ID = @ID";
            List<SqlParameter> parameters2 = new List<SqlParameter>()
            {
                new SqlParameter("@ApprovalNama", SqlDbType.VarChar){ Value = appnama },
                new SqlParameter("@ApprovalTgl", SqlDbType.DateTime){ Value = DateTime.Now },
                new SqlParameter("@ID", SqlDbType.VarChar){ Value = myID }
            };
            Execute(queryupadte, Db.Cnn_DB, parameters2, CommandType.Text);

            // Alert + Redirect
            string url = "QCPG_Detail.aspx?id=" + myID;
            string script = "alert('Data success approved!'); window.location='" + url + "';";
            ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert2", script, true);
        }
    }

    protected void Rejected_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs("SELECT TOP 1 Nomor, ApprovalNama, ApprovalTgl, OperatorTgl FROM DocQCPG WHERE ID = @ID", Db.Cnn_DB, parameters, CommandType.Text);
        bool IsApproved = dt.Rows[0]["ApprovalNama"] == DBNull.Value ? false : true;
        DateTime TglQC = Convert.ToDateTime(dt.Rows[0]["OperatorTgl"]);
        string NomorQC = dt.Rows[0]["Nomor"].ToString();

        if (TglCutOff <= TglQC)
        {
            if (IsApproved)
            {
                // Alert + Redirect
                string url = "QCPG.aspx";
                string script = "alert('QC " + NomorQC + " Sudah Approved! Data Tidak Bisa Direject.'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert3", script, true);
            }
            else
            {
                string appnama = App.UserNama;
                string queryreject = "UPDATE DocQCPG SET RejectNama = @RejectNama, RejectTgl = @RejectTgl, RejectKeterangan = @RejectKeterangan WHERE ID = @ID";
                List<SqlParameter> parameters2 = new List<SqlParameter>()
            {
                new SqlParameter("@RejectNama", SqlDbType.VarChar){ Value = appnama },
                new SqlParameter("@RejectTgl", SqlDbType.DateTime){ Value = DateTime.Now },
                new SqlParameter("@RejectKeterangan", SqlDbType.VarChar){ Value = keterangan_reject.Value },
                new SqlParameter("@ID", SqlDbType.VarChar){ Value = myID }
            };
                Execute(queryreject, Db.Cnn_DB, parameters2, CommandType.Text);

                // Alert + Redirect
                string url = "QCPG.aspx";
                string script = "alert('Data success rejected!'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert4", script, true);
            }
        }
        else
        {
            string appnama = App.UserNama;
            string queryreject = "UPDATE DocQCPG SET RejectNama = @RejectNama, RejectTgl = @RejectTgl, RejectKeterangan = @RejectKeterangan WHERE ID = @ID";
            List<SqlParameter> parameters2 = new List<SqlParameter>()
            {
                new SqlParameter("@RejectNama", SqlDbType.VarChar){ Value = appnama },
                new SqlParameter("@RejectTgl", SqlDbType.DateTime){ Value = DateTime.Now },
                new SqlParameter("@RejectKeterangan", SqlDbType.VarChar){ Value = keterangan_reject.Value },
                new SqlParameter("@ID", SqlDbType.VarChar){ Value = myID }
            };
            Execute(queryreject, Db.Cnn_DB, parameters2, CommandType.Text);

            // Alert + Redirect
            string url = "QCPG.aspx";
            string script = "alert('Data success rejected!'); window.location='" + url + "';";
            ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert4", script, true);
        }
    }

    protected DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
        }
        return dt;
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

    protected void RedirectToHome()
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs("SELECT TOP 1 Nomor, RejectNama, RejectTgl, ApprovalNama, ApprovalTgl, OperatorTgl FROM DocQCPG WHERE ID = @ID", Db.Cnn_DB, parameters, CommandType.Text);
        bool IsRejected = dt.Rows[0]["RejectNama"] == DBNull.Value ? false : true;
        bool IsApprove = dt.Rows[0]["ApprovalNama"] == DBNull.Value ? false : true;
        DateTime TglQC = Convert.ToDateTime(dt.Rows[0]["OperatorTgl"]);
        string NomorQC = dt.Rows[0]["Nomor"].ToString();

        if (TglCutOff <= TglQC)
        {
            if (IsRejected)
            {
                // Alert + Redirect
                string url = "QCPG.aspx";
                string script = "alert('QC " + NomorQC + " Sudah Rejected! Tolong Buat QC Ulang'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert6", script, true);
            }
            else if (IsApprove)
            {
                // Alert + Redirect
                string url = "QCPG_Detail.aspx?id=" + myID;
                string script = "alert('Data success approved!'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert5", script, true);
            }
        }
    }
}