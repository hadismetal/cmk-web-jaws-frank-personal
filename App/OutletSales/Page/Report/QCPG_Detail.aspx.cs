using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Report_QCPG_Detail : System.Web.UI.Page
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

        RedirectToApprove();
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

        string CB = brand; // mondial, franknco, missmondial, thepalace
        string Alamat = alamat;

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

    protected void RedirectToApprove()
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
            };
        DataTable dt = Rs("SELECT ApprovalNama, ApprovalTgl, OperatorTgl FROM DocQCPG WHERE ID = @ID", Db.Cnn_DB, parameters, CommandType.Text);
        bool IsApproved = dt.Rows[0]["ApprovalNama"] == DBNull.Value ? false : true;
        DateTime TglQC = Convert.ToDateTime(dt.Rows[0]["OperatorTgl"]);
        DateTime TglCutOff = Convert.ToDateTime("2025-12-07");

        if (TglCutOff <= TglQC)
        {
            if (!IsApproved)
            {
                Response.Redirect("QCPG_Approve.aspx?id=" + myID);
            }
        }
    }

    //Form
    protected void fillForm()
    {
        var q = db.DocQCPG.SingleOrDefault(p => p.ID == myID);
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
                checkedby.InnerText = q.OperatorNama.ToUpper();
                approvedby.InnerText = Db.SingleString("SELECT ISNULL(ApprovalNama,'') FROM DocQCPG WHERE ID = " + q.ID, Db.Cnn_DB);
                string apprvtgl = Db.SingleTime("SELECT ISNULL(ApprovalTgl,'1900-01-01') FROM DocQCPG WHERE ID = " + q.ID, Db.Cnn_DB).ToString("yyyy-MM-dd");
                string apprvtglshow = Db.SingleTime("SELECT ISNULL(ApprovalTgl,'1900-01-01') FROM DocQCPG WHERE ID = " + q.ID, Db.Cnn_DB).ToString("dd MMMM yyyy HH:mm");
                approvedtgl.InnerText = apprvtgl == "1900-01-01" ? string.Empty : apprvtglshow;
                string url = ConfigurationManager.AppSettings["ImageAPI"];
                imgpicture.ImageUrl = url + q.StockProductPG.ImgPicture;
                //Upload.Image(imgpicture, q.StockProductPG.ImgPicture);
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
                checkedby.InnerText = q.OperatorNama.ToUpper();
                approvedby.InnerText = Db.SingleString("SELECT ISNULL(ApprovalNama,'') FROM DocQCPG WHERE ID = " + q.ID, Db.Cnn_DB);
                string apprvtgl = Db.SingleTime("SELECT ISNULL(ApprovalTgl,'1900-01-01') FROM DocQCPG WHERE ID = " + q.ID, Db.Cnn_DB).ToString("yyyy-MM-dd");
                string apprvtglshow = Db.SingleTime("SELECT ISNULL(ApprovalTgl,'1900-01-01') FROM DocQCPG WHERE ID = " + q.ID, Db.Cnn_DB).ToString("dd MMMM yyyy HH:mm");
                approvedtgl.InnerText = apprvtgl == "1900-01-01" ? string.Empty : apprvtglshow;
                string url = ConfigurationManager.AppSettings["ImageAPI"];
                imgpicture.ImageUrl = url + q.StockProductPG.ImgPicture;
                //Upload.Image(imgpicture, q.StockProductPG.ImgPicture);
            }
        }
    }

    protected void RedirectToHome()
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs("SELECT RejectNama, RejectTgl, OperatorTgl FROM DocQCPG WHERE ID = @ID", Db.Cnn_DB, parameters, CommandType.Text);
        bool IsRejected = dt.Rows[0]["RejectNama"] == DBNull.Value ? false : true;
        DateTime TglQC = Convert.ToDateTime(dt.Rows[0]["OperatorTgl"]);
        DateTime TglCutOff = Convert.ToDateTime("2025-12-07");

        if (TglCutOff <= TglQC)
        {
            if (IsRejected)
            {
                // Alert + Redirect
                string url = "QCPG.aspx";
                string script = "alert('Data sudah rejected! Tolong Buat QC Ulang'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert", script, true);
            }
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
}