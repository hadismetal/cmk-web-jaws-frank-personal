using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
    protected int myID { get { return App.QueryInt(this); } } /// id brand
    protected int vBf { get { return App.QueryInt(this, "idb"); } } /// id brand
    protected int vF { get { return App.QueryInt(this, "idf"); } } /// id from
    protected int vT { get { return App.QueryInt(this, "idt"); } } /// id to
    protected int tf { get { return App.QueryInt(this, "tf"); } } /// type from
    protected int tt { get { return App.QueryInt(this, "tt"); } } /// type to
    protected int vBt { get { return App.QueryInt(this, "idbt"); } } /// id brand

    protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			namakurir.Focus();

            Fill();

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tglETA.Text = Str.Tgl(DateTime.Today);
		}
	}
    protected void Fill()
    {
        var n = cross.OutgoingDJ(myID).SingleOrDefault();
        if (n == null) App.DataNull(this);
        else
        {
            namakurir.Text = n.NamaKurir;
            tglETA.Text = Str.Tgl(n.TglETA);
            keterangan.Text = n.Keterangan;
        }

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
            var n = cross.OutgoingDJ(myID).SingleOrDefault();
            if (n == null) App.DataNull(this);
            else
            {
                //Input
                n.NamaKurir = namakurir.Text.Trim();
                n.TglETA = Convert.ToDateTime(tglETA.Text);
                n.TipeAsal = tf;
                n.TipeTujuan = tt;
                n.IDAsal = vF;
                n.IDTujuan = vT;
                n.Keterangan = keterangan.Text.Trim();

                //System Auto Input
                n.StatusTransOut = 0; // 0 : New
                n.Tgl = Convert.ToDateTime(tgl.Text);

                //User-Info
                n.Operator = App.UserNama;
                n.OperatorTgl = DateTime.Now;

                //Commit
                UpdateOutgoing(n);
            }

			//Redirect
            Response.Redirect("DJ_Product.aspx?id=" + n.ID);
		}
	}

    private void UpdateOutgoing(CrossBrand_StockOutgoingDJ rsH)
    {
        DataTable dt = new DataTable();

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

        dr["ID"] = rsH.ID;
        dr["IDBrandAsal"] = rsH.IDBrandAsal;
        dr["IDBrandTujuan"] = rsH.IDBrandTujuan;
        dr["Nomor"] = rsH.Nomor;
        dr["NamaKurir"] = rsH.NamaKurir;
        dr["TglETA"] = rsH.TglETA;
        dr["TipeAsal"] = rsH.TipeAsal;
        dr["TipeTujuan"] = rsH.TipeTujuan;
        dr["NamaAsal"] = rsH.NamaAsal;
        dr["NamaTujuan"] = rsH.NamaTujuan;
        dr["IDAsal"] = rsH.IDAsal;
        dr["IDTujuan"] = rsH.IDTujuan;
        dr["Keterangan"] = rsH.Keterangan;
        dr["Tgl"] = rsH.Tgl;
        dr["StatusTransOut"] = rsH.StatusTransOut;
        dr["Operator"] = rsH.Operator;
        dr["OperatorTgl"] = rsH.OperatorTgl;

        dt.Rows.Add(dr);

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_UpdateCrossBrandOutgoingDJ"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}