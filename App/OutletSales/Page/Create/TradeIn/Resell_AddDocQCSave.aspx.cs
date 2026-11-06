using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

public partial class Page_Register_Resell_ItemAddSave : System.Web.UI.Page {
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
    protected int myItemID { get { return App.QueryInt(this, "itemid"); } }
    protected int myResellID { get { return App.QueryInt(this, "rid"); } }
    protected int myDocID { get { return App.QueryInt(this, "docid"); } }
    protected int myTipeID { get { return App.QueryInt(this, "tipe"); } }
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}

        Save();
	}

	//Exception
	protected void DataError() { Response.Redirect("TradeIn.aspx"); }

	
	protected void Save() {
        var d = db.TradeIn.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            if (myTipeID == 1)
            {
                var x = db.ResellDJ.SingleOrDefault(p => p.ID == myResellID);
                if (x != null)
                {
                    x.IDDocQC = myDocID;
                    db.SubmitChanges();
                }
            }
            else if (myTipeID == 2)
            {
                var x = db.ResellPG.SingleOrDefault(p => p.ID == myResellID);
                if (x != null)
                {
                    x.IDDocQC = myDocID;
                    db.SubmitChanges();

                    UpdateBeratTimbangPG(x.ID);
                }
            }
            else if (myTipeID == 3)
            {
                var x = db.ResellGJ.SingleOrDefault(p => p.ID == myResellID);
                if (x != null)
                {
                    x.IDDocQC = myDocID;
                    db.SubmitChanges();
                }
            }
            else if (myTipeID == 4)
            {
                var x = db.ResellLD.SingleOrDefault(p => p.ID == myResellID);
                if (x != null)
                {
                    x.IDDocQC = myDocID;
                    db.SubmitChanges();
                }
            }
            Response.Redirect("TradeIn_Data.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO);
        }	
	}

    protected void UpdateBeratTimbangPG(int ID)
    {
        DocQCPG pg = db.DocQCPG.SingleOrDefault(p => p.ID == myDocID);
        if (pg != null)
        {
            string normalize = pg.NetWeight.Replace(".", "").Replace(',', '.');
            decimal weight = decimal.Parse(normalize, CultureInfo.InvariantCulture);
            ResellPG rpg = db.ResellPG.SingleOrDefault(p => p.ID == myResellID);
            if (rpg != null)
            {
                decimal TgpBeli = GetTGPPGBeliNew(rpg.IDProduct);
                string query = "UPDATE ResellPG SET BeratTimbang = @BeratTimbang, TGP = @Tgp, HargaRupiah = @BeratTimbang * @Tgp WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@BeratTimbang", SqlDbType.Decimal) { Value = weight },
                    new SqlParameter("@Tgp", SqlDbType.Money) { Value = TgpBeli },
                    new SqlParameter("@ID", SqlDbType.Int) { Value = ID }
                };

                Execute(query, Db.Cnn_DB, parameters, CommandType.Text);
            }
        }
    }

    protected decimal GetTGPPGBeliNew(int IDProduct)
    {
        decimal GoldPrice = 0m;

        string query = "sp_get_tgp_jual_beli_plu";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID_PRODUCT", SqlDbType.Int){ Value = IDProduct },
            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "PG" }
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        if (dt.Rows.Count == 1)
        {
            GoldPrice = Convert.ToDecimal(dt.Rows[0]["HARGA_BELI"]);
        }

        return GoldPrice;
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                foreach (var x in param)
                {
                    cmd.Parameters.Add(x);
                }
                cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                sqlCnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}