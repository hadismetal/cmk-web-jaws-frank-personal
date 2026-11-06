using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Web.Script.Services;


public partial class DOKRM_index : System.Web.UI.Page
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

    protected string myV { get { return Request.QueryString["valid"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object ReportKRM(string keyword, string start, string end, string loccode, string statusdo)
    {
        string[] start1 = start.Split('-');
        string[] end1 = end.Split('-');
        string xDateFrom = start1[2] + "-" + start1[1] + "-" + start1[0];
        string xDateTo = end1[2] + "-" + end1[1] + "-" + end1[0];

        int xUserID = App.UserID;
        int xisAM = 0;
        int xisAE = 0;
        string listStore = "";

        //check area manager
		using (Map_DB db = new Map_DB())
		{
			var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
			int amcount = am.Count();
			if (amcount > 0)
			{
				int z = 0;
				foreach (var x in am)
				{
					int j = 0;
					var lokasi = x.DataAreaManager_Lokasi
						.GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
						.Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
					foreach (var y in lokasi)
					{
						listStore = "";
						if (y.TipeLokasi == 1)
							listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
						else
							listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
						j++;
						listStore += ",";
					}
					z++;
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
					xisAE = 1;
				}
			}
		}

        string xKeyloc = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;
        loccode = xKeyloc;

        string query = "exec sp_report_dokrm_detail_new '" + keyword.Trim() + "', '" + xDateFrom + "', " +
        "'" + xDateTo + "','" + loccode + "','" + statusdo + "','1','20','1'";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        Krm[] listkrm = new Krm[dt.Rows.Count];
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            Krm krm = new Krm();
            krm.no = Convert.ToInt32(dr["NO"]);
            krm.id = Convert.ToInt32(dr["ID"]);
            krm.nodokrm =  dr["NOMOR_DO_KRM"].ToString();
            krm.nokrm = dr["NO_KRM"].ToString().Replace(",","<br />");
            krm.kurir = dr["NAMA_KURIR"].ToString();
            krm.statusdo = dr["STATUS_DO"].ToString();
			krm.approvaloutgoingnama = dr["OPERATOR_APPROVED"].ToString();
            krm.approvaloutgoingtgl = Convert.ToDateTime(dr["TGL_APPROVED"]).ToString("dddd, dd MMMM yyyy hh:mm");
			krm.approvalincomingnama = (dr["OPERATOR_APPROVED_TERIMA"] == DBNull.Value) ? "" : dr["OPERATOR_APPROVED_TERIMA"].ToString();
            krm.approvalincomingtgl = (dr["TGL_APPROVED_TERIMA"] == DBNull.Value) ? "" :Convert.ToDateTime(dr["TGL_APPROVED_TERIMA"]).ToString("dddd, dd MMMM yyyy hh:mm");
            krm.etd = Convert.ToDateTime(dr["ETD"]).ToString("dddd, dd MMMM yyyy");
            krm.keterangan = dr["KETERANGAN"].ToString();
			krm.tujuan = dr["TUJUAN"].ToString();
			krm.qty = Convert.ToInt32(dr["QTY"]).ToString();
			krm.estimasi_harga = Convert.ToDecimal(dr["ESTIMASI_HARGA"]).ToString("N0");
			krm.estimasi_harga_int = Convert.ToInt64(dr["ESTIMASI_HARGA"]);
            listkrm[i] = krm;
            i++;
        }
        return listkrm;
    }

    public class Krm
    {
        public int no { get; set; }
        public int id { get; set; }
        public string nodokrm { get; set; }
        public string nokrm { get; set; }
        public string lokasiasal { get; set; }
        public string lokasitujuan { get; set; }
        public string kurir { get; set; }
        public string statusdo { get; set; }
        public string approvaloutgoingnama { get; set; }
        public string approvaloutgoingtgl { get; set; }
		public string approvalincomingnama { get; set; }
        public string approvalincomingtgl { get; set; }
        public string etd { get; set; }
        public string keterangan { get; set; }
		public string tujuan { get; set; }
		public string qty { get;set; }
		public string estimasi_harga { get; set; }
		public long estimasi_harga_int { get; set; }
    }
}