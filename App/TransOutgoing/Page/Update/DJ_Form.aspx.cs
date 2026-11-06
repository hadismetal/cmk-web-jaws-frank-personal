using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

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

    protected int myID { get { return App.QueryInt(this); } } /// id brand
    protected int vB { get { return App.QueryInt(this, "idb"); } } /// id brand
    protected int vF { get { return App.QueryInt(this, "idf"); } } /// id from
    protected int vT { get { return App.QueryInt(this, "idt"); } } /// id to
    protected int tf { get { return App.QueryInt(this, "tf"); } } /// type from
    protected int tt { get { return App.QueryInt(this, "tt"); } } /// type to

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            namakurir.Focus();

            Fill();

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tglETA.Text = Str.Tgl(DateTime.Today);
        }
    }
    protected void Fill()
    {
        var n = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == myID);
        if (n == null) App.DataNull(this);
        else
        {
            brand.InnerText = n.CompanyBrand.Nama;
            namakurir.Text = n.NamaKurir;
            tglETA.Text = Str.Tgl(n.TglETA);
            keterangan.Text = n.Keterangan;
        }

        //var rsB = d.CompanyBrand.SingleOrDefault(p => p.ID == vB);
        //if (rsB == null) App.DataNull(this);

        lit_type_from.Text = DL.LocationType(tf);
        lit_type_to.Text = DL.LocationType(tt);
        //brand.InnerText = rsB.Nama;

        // boleh lintas brand : missmondial & mondial

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
                locT_W = locT_W.Where(p => p.Disable == false && p.Draft == false && p.ID == vT);
                if (tt == tf) { locT_W = locT_W.Where(p => p.ID != vF); }
                if (locT_W.Count() != 1) { App.DataNull(this); } else { NamaTo = locT_W.First().Nama; }
            }
            else if (tt == 1)
            {
                locT_O = locT_O.Where(p => p.Disable == false && p.Draft == false && p.ID == vT);
                if (tt == tf) { locT_O = locT_O.Where(p => p.ID != vF); }
                if (locT_O.Count() != 1) { App.DataNull(this); } else { NamaTo = locT_O.First().Nama; }
            }
            else if (tt == 2)
            {
                locT_E = locT_E.Where(p => p.Disable == false && p.Draft == false && p.ID == vT);
                if (tt == tf) { locT_E = locT_E.Where(p => p.ID != vF); }
                if (locT_E.Count() != 1) { App.DataNull(this); } else { NamaTo = locT_E.First().Nama; }
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

            string UpdateOutgoingDJ = @"
                            IF EXISTS(SELECT ID FROM StockOutgoingDJ where ID = @ID)
                            BEGIN
                                UPDATE A SET NamaKurir = @NamaKurir, 
                                TglETA = @tglETA, Keterangan = @Keterangan, Tgl = @Tgl,
                                Operator = @Operator, OperatorTgl = GETDATE()
                                FROM (SELECT * FROM StockOutgoingDJ where ID = @ID) A
                                IF @@ROWCOUNT > 0
                                BEGIN
                                    SELECT 'Update successful' AS Result;
                                END
                                ELSE
                                BEGIN
                                    SELECT 'Update failed - no rows affected' AS Result;
                                END
                            END
                            ELSE
                            BEGIN
                                SELECT 'Update failed - ID does not exist' AS Result;
                            END
            ";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(UpdateOutgoingDJ, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", myID);
                    cmd.Parameters.AddWithValue("@NamaKurir", namakurir.Text.Trim());
                    cmd.Parameters.AddWithValue("@Tgl", Convert.ToDateTime(tgl.Text));
                    cmd.Parameters.AddWithValue("@tglETA", Convert.ToDateTime(tglETA.Text));
                    cmd.Parameters.AddWithValue("@Keterangan", keterangan.Text.Trim());
                    cmd.Parameters.AddWithValue("@Operator", App.UserNama);                    

                    conn.Open();
                    SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);

                    DataSet ds1 = new DataSet();
                    adapter1.Fill(ds1);
                    DataTable data = ds1.Tables[0];
                    
                    if (Convert.ToString(data.Rows[0]["Result"]) == "Update successful")
                    {
                        //Redirect
                        Response.Redirect("DJ_Product.aspx?id=" + myID);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + data.Rows[0]["Result"] + "');", true);
                    }
                    conn.Close();
                }
            }


		}
	}



	// protected void save_Click(object sender, EventArgs e) {
    // 	if (valid()) {
    //         var n = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == myID);
    //         if (n == null) App.DataNull(this);
    //         else
    //         {
    //             //Input
    //             n.IDBrand = vB;
    //             n.NamaKurir = namakurir.Text.Trim();
    //             n.TglETA = Convert.ToDateTime(tglETA.Text);
    //             n.TipeAsal = tf;
    //             n.TipeTujuan = tt;
    //             n.IDAsal = vF;
    //             n.IDTujuan = vT;
    //             n.Keterangan = keterangan.Text.Trim();

    //             //System Auto Input
    //             n.StatusTransOut = 0; // 0 : New
    //             n.Tgl = Convert.ToDateTime(tgl.Text);

    //             //User-Info
    //             n.Operator = App.UserNama;
    //             n.OperatorTgl = DateTime.Now;

    //             //Commit
    //             d.SubmitChanges();
    //         }

    // 		//Redirect
    //         Response.Redirect("DJ_Product.aspx?id=" + n.ID);
    // 	}
    // }
}