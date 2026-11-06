using System;
using System.Linq;
using Library.SAP_ZMMFM_STO_INTRFC;
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

    protected int brAsal { get { return App.QueryInt(this, "brAsal"); } } /// id brand
    protected int brTujuan { get { return App.QueryInt(this, "brTujuan"); } } /// id brand
    protected int vF { get { return App.QueryInt(this, "idf"); } } /// id from
    protected int vT { get { return App.QueryInt(this, "idt"); } } /// id to
    protected int tf { get { return App.QueryInt(this, "tf"); } } /// type from
    protected int tt { get { return App.QueryInt(this, "tt"); } } /// type to
    
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			namakurir.Focus();


            bool checkSuccess = false;
            bool SAPActivate = false;
            SAPActivate = SAP.xActivate;


            //check if sap integration false
            if (SAPActivate == true && (checkSuccess))
            {
                if (!SAP.TransferValidation(tf, vF, tt, vT))
                {

                    //Redirect
                    Response.Redirect("DJ.aspx?id=1&valid=0");
                }
            }

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
        brand.InnerText = Db.SingleString("select top 1 Nama from CompanyBrand a where ID="+brAsal, Db.Cnn_DB);
        //brand.InnerText = rsB.Nama;

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
        Response.Write(tf);
		if (valid()) {            
            int IDBrand = 1;
            int lastInsertedId = 0;
            string NamaAsal = "";
            string NamaTujuan = "";
            string getLokasi = @"select * 
                            FROM (
                            	select IDBrand,0 TipeLokasi, ID IDLokasi, Nama NamaLokasi, IDGroup, AddrAlamat from LocWarehouse where Draft = 0 and Disable = 0
                            	UNION
                            	select IDBrand,1 TipeLokasi, ID IDLokasi, Nama NamaLokasi, IDGroup, AddrAlamat from LocOutlet where Draft = 0 and Disable = 0
                            	UNION
                            	select IDBrand,2 TipeLokasi, ID IDLokasi, Nama NamaLokasi, IDGroup, AddrAlamat from LocExhibition where Draft = 0 and Disable = 0
                            )AX
                            where IDLokasi = @IDLokasi AND TipeLokasi = @TipeLokasi AND IDBrand = @IDBrand
                            ";

            string inserKRM = @"INSERT INTO [dbo].[StockOutgoingDJ] 
                                (IDBrand, NamaKurir, TglETA, TipeAsal, TipeTujuan, NamaAsal, NamaTujuan, IDAsal, IDTujuan, Keterangan, StatusTransOut, Tgl, Operator, OperatorTgl)
                                VALUES
                                (@IDBrand, @NamaKurir, @TglETA, @TipeAsal, @TipeTujuan, @NamaAsal, @NamaTujuan, @IDAsal, @IDTujuan, @Keterangan, @StatusTransOut, @Tgl, @Operator, @OperatorTgl)
                                
                                SELECT SCOPE_IDENTITY()";                                
            
                try
                {                                    
                    using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
                    {
                        using (SqlCommand command = new SqlCommand(getLokasi, connection))
                        {                                
                            command.Parameters.Add(new SqlParameter("@IDLokasi", vF));
                            command.Parameters.Add(new SqlParameter("@TipeLokasi", tf));
                            command.Parameters.Add(new SqlParameter("@IDBrand", brAsal));

                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);                    
                            
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow r = ds.Tables[0].Rows[0];                                                         
                                NamaAsal = r["NamaLokasi"].ToString();
                                IDBrand = Convert.ToInt32(r["IDBrand"]);
                            }
                        }
                        
                        using (SqlCommand command = new SqlCommand(getLokasi, connection))
                        {                                
                            command.Parameters.Add(new SqlParameter("@IDLokasi", vT));
                            command.Parameters.Add(new SqlParameter("@TipeLokasi", tt));
                            command.Parameters.Add(new SqlParameter("@IDBrand", brTujuan));

                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);                    
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow r = ds.Tables[0].Rows[0];                        
                                NamaTujuan = r["NamaLokasi"].ToString();                                    
                            }
                        }

                        using (SqlCommand command = new SqlCommand(inserKRM, connection))
                        {                            
                            command.Parameters.Add(new SqlParameter("@IDBrand", IDBrand));
                            command.Parameters.Add(new SqlParameter("@NamaKurir", namakurir.Text.Trim()));
                            command.Parameters.Add(new SqlParameter("@TglETA", Convert.ToDateTime(tglETA.Text)));
                            command.Parameters.Add(new SqlParameter("@TipeAsal", tf));
                            command.Parameters.Add(new SqlParameter("@TipeTujuan", tt));
                            command.Parameters.Add(new SqlParameter("@NamaAsal", NamaAsal));
                            command.Parameters.Add(new SqlParameter("@NamaTujuan", NamaTujuan));
                            command.Parameters.Add(new SqlParameter("@IDAsal", vF));
                            command.Parameters.Add(new SqlParameter("@IDTujuan", vT));
                            command.Parameters.Add(new SqlParameter("@Keterangan", keterangan.Text.Trim()));
                            command.Parameters.Add(new SqlParameter("@StatusTransOut", Convert.ToInt32(0))); // 0 : New
                            command.Parameters.Add(new SqlParameter("@Tgl", Convert.ToDateTime(tgl.Text)));
                            command.Parameters.Add(new SqlParameter("@Operator", App.UserNama));
                            command.Parameters.Add(new SqlParameter("@OperatorTgl", DateTime.Now));
                            
                            try
                            {
                                connection.Open();
                                lastInsertedId = Convert.ToInt32(command.ExecuteScalar());
                            }
                            catch (Exception ex)
                            {
                                // Log or handle your error here
                                Response.Write("Insert error: " + ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }            
			            
                        //Redirect
                        if (lastInsertedId > 0)            
                        {                
                            Response.Redirect("DJ_Product.aspx?id=" + lastInsertedId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
		}
	
    }    

        
        public static void GetDataTujuan(int IDLokasi, int TipeLokasi,int brandTujuan, string query, out string NamaTujuan)
        {   
            NamaTujuan = string.Empty;
            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@IDLokasi", IDLokasi));
                    command.Parameters.Add(new SqlParameter("@TipeLokasi", TipeLokasi));
                    command.Parameters.Add(new SqlParameter("@IDBrand", brandTujuan));

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Close();
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);                    
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {                        
                        NamaTujuan = r["NamaLokasi"].ToString();            
                    }
                }
            }            
        }
	// protected void save_Click(object sender, EventArgs e) {
	// 	if (valid()) {
    //         StockOutgoingDJ n = new StockOutgoingDJ();
    //         int IDBrand = 1;
    //         if (tf == 0) { 
    //             var rs = d.LocWarehouse.SingleOrDefault(p => (vB > 0 ? p.IDBrand == vB : 1 == 1) && p.ID == vF); 
    //             n.NamaAsal = rs.Nama; 
    //             IDBrand = rs.IDBrand;
    //         }
    //         else if (tf == 1) { var rs = d.LocOutlet.SingleOrDefault(p => (vB > 0 ? p.IDBrand == vB : 1 == 1) && p.ID == vF); n.NamaAsal = rs.Nama; IDBrand = rs.IDBrand; }
    //         else if (tf == 2) { var rs = d.LocExhibition.SingleOrDefault(p => (vB > 0 ? p.IDBrand == vB : 1 == 1) && p.ID == vF); n.NamaAsal = rs.Nama; IDBrand = rs.IDBrand; }

    //         if (tt == 0) { var rs = d.LocWarehouse.SingleOrDefault(p => (vB > 0 ? p.IDBrand == vB : 1 == 1) && p.ID == vT); n.NamaTujuan = rs.Nama; }
    //         else if (tt == 1) { var rs = d.LocOutlet.SingleOrDefault(p => (vB > 0 ? p.IDBrand == vB : 1 == 1) && p.ID == vT); n.NamaTujuan = rs.Nama; }
    //         else if (tt == 2) { var rs = d.LocExhibition.SingleOrDefault(p => (vB > 0 ? p.IDBrand == vB : 1 == 1) && p.ID == vT); n.NamaTujuan = rs.Nama; }

    //         //Input
    //         //n.IDBrand = vB;
    //         n.IDBrand = IDBrand;
    //         n.NamaKurir = namakurir.Text.Trim();
    //         n.TglETA = Convert.ToDateTime(tglETA.Text);
    //         n.TipeAsal = tf;
    //         n.TipeTujuan = tt;
    //         n.IDAsal = vF;
    //         n.IDTujuan = vT;
    //         n.Keterangan = keterangan.Text.Trim();
            
    //         //System Auto Input
    //         n.StatusTransOut = 0; // 0 : New
    //         n.Tgl = Convert.ToDateTime(tgl.Text);

    //         //User-Info
    //         n.Operator = App.UserNama;
    //         n.OperatorTgl = DateTime.Now;

    //         //Commit
    //         d.StockOutgoingDJ.InsertOnSubmit(n);
    //         d.SubmitChanges();

	// 		//Redirect
    //         Response.Redirect("DJ_Product.aspx?id=" + n.ID);
	// 	}
	// }
}