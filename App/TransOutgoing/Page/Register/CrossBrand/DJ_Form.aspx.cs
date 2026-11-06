using System;
using System.Linq;
using Library.SAP_ZMMFM_STO_INTRFC;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using CrossBrand;
using CrossBrand.Model;

public partial class DJ_Form : System.Web.UI.Page
{
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
	        return _db;
	    }
	}

    private Map_DB _d;

    private Map_DB d
    {
        get
        {
            if (_d == null)
                _d = new Map_DB();
            return _d;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (d != null)
	        d.Dispose();
	    base.Dispose();
	}
    Modelize cross = new Modelize();
    List<LocWarehouse> allWarehouse = new List<LocWarehouse>();
    List<LocOutlet> allOutlet = new List<LocOutlet>();
    List<LocExhibition> allExhibition = new List<LocExhibition>();

    protected int vBf { get { return App.QueryInt(this, "idbf"); } } /// id brand
    protected int vF { get { return App.QueryInt(this, "idf"); } } /// id from
    protected int vT { get { return App.QueryInt(this, "idt"); } } /// id to
    protected int tf { get { return App.QueryInt(this, "tf"); } } /// type from
    protected int tt { get { return App.QueryInt(this, "tt"); } } /// type to
    protected int vBt { get { return App.QueryInt(this, "idbt"); } } /// id brand

    protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			namakurir.Focus();


                //if (!SAP.TransferValidation(tf, vF, tt, vT))
                //{

                //    //Redirect
                //    Response.Redirect("DJ.aspx?id=1&valid=0");
                //}
           

            Fill();

            

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tglETA.Text = Str.Tgl(DateTime.Today);
		}
	}
    
   

    protected void Fill()
    {
        
        //var rsB = d.CompanyBrand.SingleOrDefault(p => p.ID == vB);
        //if (rsB == null) App.DataNull(this);

        lit_type_from.Text = DL.LocationType(tf);
        lit_type_to.Text = DL.LocationType(tt);
        brandFrom.InnerText = d.CompanyBrand.SingleOrDefault(p => p.ID == vBf).Nama;
        brandTo.InnerText = d.CompanyBrand.SingleOrDefault(p => p.ID == vBt).Nama;

        var locF_W = from p in d.LocWarehouse select p;
        var locF_O = from p in d.LocOutlet select p;
        var locF_E = from p in d.LocExhibition select p;

        var locT_W = locF_W;
        var locT_O = locF_O;
        var locT_E = locF_E;

        string NamaFrom = "";
        string NamaTo = "";

        try
        {
            if (tf == 0)
            {
                locF_W = locF_W.Where(p => p.Disable == false && p.Draft == false && p.ID == vF);
                if (locF_W.Count() != 1) { App.DataNull(this); } else { NamaFrom = locF_W.First().Nama; }
            }
            else if (tf == 1)
            {
                locF_O = locF_O.Where(p => p.Disable == false && p.Draft == false && p.ID == vF);
                if (locF_O.Count() != 1) { App.DataNull(this); } else { NamaFrom = locF_O.First().Nama; }
            }
            else if (tf == 2)
            {
                locF_E = locF_E.Where(p => p.Disable == false && p.Draft == false && p.ID == vF);
                if (locF_E.Count() != 1) { App.DataNull(this); } else { NamaFrom = locF_E.First().Nama; }
            }
        }
        catch { }
        try
        {
            if (tt == 0)
            {
                var loc = cross.Warehouses(vBt, vT).Where(p => p.Draft == 0 && p.Disable == 0);
                if (tt == tf) { loc = loc.Where(p => p.ID != vF || p.IDBrand != vBf); }
                if (loc.Count() != 1) { App.DataNull(this); } else { NamaTo = loc.First().Nama; }
            }
            else if (tt == 1)
            {
                var loc = cross.Outlets(vBt, vT).Where(p => p.Draft == 0 && p.Disable == 0);
                if (tt == tf) { loc = loc.Where(p => p.ID != vF || p.IDBrand != vBf); }
                if (loc.Count() != 1) { App.DataNull(this); } else { NamaTo = loc.First().Nama; }
            }
            else if (tt == 2)
            {
                var loc = cross.Exhibitions(vBt, vT).Where(p => p.Draft == 0 && p.Disable == 0);
                if (tt == tf) { loc = loc.Where(p => p.ID != vF || p.IDBrand != vBf); }
                if (loc.Count() != 1) { App.DataNull(this); } else { NamaTo = loc.First().Nama; }
            }
        }
        catch { }

        if (NamaFrom == "") App.DataNull(this);
        if (NamaTo == "") App.DataNull(this);

        from.InnerText = NamaFrom;
        to.InnerText = NamaTo;

    }
	//Save
    protected bool valid()
    {
        bool status = true;
        if (!Fv.isDateTime(tgl, tgl_ERR)) { status = false; }
        if (!Fv.isDateTime(tglETA, tglETA_ERR)) { status = false; }
        return status;
        // return  }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid()) {

            DataTable dt = new DataTable();
            DataTable dataAsal = new DataTable();
            DataTable dataTujuan = new DataTable();

            if (vF > 0)
            {
                dataAsal = Db.Rs("sp_getLocationBrand " + vBf + ", " + tf + ", " + vF, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
            }
            if (vT > 0)
            {
                dataTujuan = Db.Rs("sp_getLocationBrand " + vBt + ", " + tt + ", " + vT, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
            }

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("IDBrandAsal", typeof(int));
            dt.Columns.Add("IDBrandTujuan", typeof(int));
            dt.Columns.Add("Nomor", typeof(string));
            dt.Columns.Add("NamaKurir", typeof(string));
            dt.Columns.Add("TglETA", typeof(DateTime));
            dt.Columns.Add("TipeAsal", typeof(int));
            dt.Columns.Add("TipeTujuan", typeof(int));
            dt.Columns.Add("NamaAsal", typeof(string));
            dt.Columns.Add("NamaTujuan", typeof(string));
            dt.Columns.Add("IDAsal", typeof(int));
            dt.Columns.Add("IDTujuan", typeof(int));
            dt.Columns.Add("Keterangan", typeof(string));
            dt.Columns.Add("Tgl", typeof(DateTime));
            dt.Columns.Add("StatusTransOut", typeof(int));
            dt.Columns.Add("Operator", typeof(string));
            dt.Columns.Add("OperatorTgl", typeof(DateTime));
            dt.Columns.Add("Approval", typeof(string));
            dt.Columns.Add("ApprovalTgl", typeof(DateTime));

            DataRow dr = dt.NewRow();

            dr["IDBrandAsal"] = vBf;
            dr["IDBrandTujuan"] = vBt;
            dr["NamaKurir"] = namakurir.Text;
            dr["TglETA"] = Convert.ToDateTime(tglETA.Text);
            dr["TipeAsal"] = tf;
            dr["TipeTujuan"] = tt;
            dr["NamaAsal"] = dataAsal.Rows[0]["text"];
            dr["NamaTujuan"] = dataTujuan.Rows[0]["text"];
            dr["IDAsal"] = vF;
            dr["IDTujuan"] = vT;
            dr["Keterangan"] = keterangan.Text;
            dr["Tgl"] = Convert.ToDateTime(tgl.Text);
            dr["StatusTransOut"] = 0;
            dr["Operator"] = App.UserNama;
            dr["OperatorTgl"] = DateTime.Now;

            dt.Rows.Add(dr);

            int ID = 0;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "dbo.sp_InsertCrossBrandOutgoingDJ"; //The name of the above mentioned stored procedure.  
                SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

                SqlParameter RuturnValue = new SqlParameter("@retValue", SqlDbType.Int);
                RuturnValue.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(RuturnValue);

                conn.Open();
                cmd.ExecuteNonQuery();
                ID = (int)cmd.Parameters["@retValue"].Value; 
                conn.Close();
            }

			//Redirect
            Response.Redirect("DJ_Product.aspx?id=" + ID);
		}
	}
}