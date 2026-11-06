using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using CrossBrand;
using CrossBrand.Model;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data.SqlClient;
public partial class PG_Product : System.Web.UI.Page
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

    protected int id { get { return App.QueryInt(this); } } /// id trans out

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            //Form-Default
            Fill();

        }
    }
    //Fill
    // protected void Fill()
    // {
    //     save.Visible = false;

    //     cbl_Product.Items.Clear();

    //     Modelize cross = new Modelize();
    //     CrossBrand_StockOutgoingPG rsH = cross.OutgoingPG(id).SingleOrDefault();

    //     brandFrom.InnerText = d.CompanyBrand.SingleOrDefault(c => c.ID == rsH.IDBrandAsal).Nama;
    //     brandTo.InnerText = d.CompanyBrand.SingleOrDefault(c => c.ID == rsH.IDBrandTujuan).Nama;
    //     lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
    //     lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
    //     from.InnerText = rsH.NamaAsal;
    //     to.InnerText = rsH.NamaTujuan;

    //     //inputproduct.Text = "13/11/0018";
    //     inputproduct.Focus();

    // }

    protected void Fill()
    {
        save.Visible = false;

        cbl_Product.Items.Clear();

        Modelize cross = new Modelize();
        CrossBrand_StockOutgoingPG rsH = cross.OutgoingPG(id).SingleOrDefault();

        brandFrom.InnerText = d.CompanyBrand.SingleOrDefault(c => c.ID == rsH.IDBrandAsal).Nama;
        brandTo.InnerText = d.CompanyBrand.SingleOrDefault(c => c.ID == rsH.IDBrandTujuan).Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;        
        IDAsal.Text = Convert.ToString(rsH.IDAsal);
        TipeAsal.Text = Convert.ToString(rsH.TipeAsal);

        //inputproduct.Text = "13/11/0018";
        inputproduct.Focus();

    }
	protected string validasi_BlackList(string nomor)
    {
        string query = "DECLARE @ERROR VARCHAR(100) = ''; " +
            "IF EXISTS (SELECT 1 FROM StockProductBlackList WHERE PLU = @PLU AND STATUS = 1) " +
            "BEGIN " +
            "   SET @ERROR = 'This product is blacklisted.'; " +
            "END " +
            "SELECT @ERROR AS ErrorMessage;";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@PLU", SqlDbType.VarChar) { Value = nomor }
        };
        DataTable dt = Rs(query, Db.Cnn_IN, parameters, CommandType.Text);
        return dt.Rows[0][0].ToString();
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
                cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }
    //InputProduct
    protected void btn_inputproduct_Click1(object sender, EventArgs e)
	{
		// string PLU = inputproduct.Text.Trim();
		// string TipeLokasiAsal = TipeAsal.Text.Trim();
		// string IDLokasiAsal = IDAsal.Text.Trim();
		TextBox pinputproduct = inputproduct;
		Literal plit_Message = lit_Message;
		System.Web.UI.HtmlControls.HtmlButton psave = save;
		CheckBoxList pcbl_Product = cbl_Product;
		int pid = id;

		string error_blacklist = validasi_BlackList(pinputproduct.Text.Trim());

		if(string.IsNullOrEmpty(error_blacklist))
		{
			Lib.FormRegisterUpdateDokumenDraft_PG_CrossBrandV3(plit_Message, pinputproduct, psave, pcbl_Product, pid);
		}
		else
		{
			lit_Message.Text = "<label class=\"text-danger\">PLU Tidak dapat di Cross Brand karena diblacklist. Harap hubungi pusat.</label>";
		}
	}
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        Literal plit_Message = lit_Message;
        TextBox pinputproduct = inputproduct;
        System.Web.UI.HtmlControls.HtmlButton psave = save;
        CheckBoxList pcbl_Product = cbl_Product;
        int pid = id;

        Lib.FormRegisterUpdateDokumenDraft_PG_CrossBrand(plit_Message, pinputproduct, psave, pcbl_Product, pid);
    }
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductPG.SingleOrDefault(p => p.Nomor == input && !p.Draft.Value);

        if (rs1 == null) { message = "Invalid Product"; }
        else
        {
            // 2. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
            }

            if (rs1.ProductLevel != 11) message = "Barang yang dapat dicross brand hanya barang PGE";
        }

        // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

        return message;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        Db.Execute("DELETE FROM CrossBrand_StockOutgoingPG_Product WHERE IDForm=" + id, Db.Cnn_DB);

        DataTable dt = new DataTable();

        dt.Columns.Add("IDForm", typeof(int));
        dt.Columns.Add("IDProduct", typeof(int));
        dt.Columns.Add("Nomor", typeof(string));
        dt.Columns.Add("Substorage", typeof(int));

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                var rs = d.StockActualPG.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value));

                DataRow dr = dt.NewRow();

                dr["IDForm"] = id;
                dr["IDProduct"] = Convert.ToInt32(cbl_Product.Items[i].Value);
                dr["Nomor"] = rs.Nomor;
                dr["Substorage"] = rs.Substorage;

                dt.Rows.Add(dr);
            }
        }

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_InsertCrossBrandOutgoingPG_Product"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Redirect
        Response.Redirect("PG_Finish.aspx?id=" + id);

    }
    

    [WebMethod]
    public static object CheckDataExistence(string PLU, string IDAsal, string TipeLokasi)
    {
        // List to hold values with their counts
        List<object> Data = new List<object>();

        string CHECKPLU = @"
            DECLARE @MESSAGE varchar(100) = 'Data siap digunakan', @STATUS int = 200, @IDProduct int
            SELECT @IDProduct = ID FROM StockProductPG where Nomor = @Nomor

            IF NOT EXISTS(SELECT 1 FROM StockActualPG a where IDLokasi = @IDLokasi and TipeLokasi = @TipeLokasi and Nomor = @Nomor and Substorage = @SubStorage and InHand >= @InHand)
            BEGIN                    
                SELECT @MESSAGE = 'Data tidak ada di lokasi tersebut', @STATUS = 404
            END             
            ELSE
            BEGIN
                IF EXISTS(SELECT 1 FROM SalesOrderPG a JOIN SalesOrder b on b.ID=a.IDForm where a.IDProduct = @IDProduct and b.StatusPembayaran = 0 and b.Draft = 0)
                BEGIN
                    SELECT @MESSAGE = 'Data berstatus waiting for payment pada nomor SO '+b.Nomor, @STATUS = 404 
                    FROM SalesOrderPG a JOIN SalesOrder b on b.ID=a.IDForm where a.IDProduct = @IDProduct and b.StatusPembayaran = 0 and b.Draft = 0
                END
                ELSE IF EXISTs(SELECT 1 FROM SalesOrderPG a JOIN SalesOrder b on b.ID=a.IDForm where a.IDProduct = @IDProduct and b.Draft = 1)
                BEGIN
                    SELECT @MESSAGE = 'Data berstatus draft pada penjualan', @STATUS = 404
                END
                ELSE IF EXISTS(SELECT 1 FROM StockOutgoingPG a JOIN StockOutgoingPG_product b on b.IDForm = a.ID WHERE b.IDProduct = @IDProduct and a.StatusTransOut <> 4 and a.TipeAsal = @TipeLokasi and a.IDAsal = @IDLokasi)
                BEGIN                    
                    SELECT @MESSAGE = 'Barang sudah terinput pada menu outgoing '+ IIF(a.StatusTransOut = 0 , 'dan berstatus draft.', 'dengan status waiting for approval.'), @STATUS = 404 FROM StockOutgoingPG a JOIN StockOutgoingPG_product b on b.IDForm = a.ID WHERE b.IDProduct = @IDProduct and a.StatusTransOut <> 4 and a.TipeAsal = @TipeLokasi and a.IDAsal = @IDLokasi
                END
                ELSE IF EXISTS(SELECT 1 FROM CrossBrand_StockOutgoingPG a JOIN CrossBrand_StockOutgoingPG_Product b on a.ID = b.IDForm WHERE b.IDProduct = @IDProduct and a.StatusTransOut <> 4 and a.TipeAsal = @TipeLokasi and a.IDAsal = @IDLokasi)
                BEGIN                    
                    SELECT @MESSAGE = 'Barang sudah terinput pada menu outgoing crossbrand'+ IIF(a.StatusTransOut = 0, ' dan berstatus draft.', 'dengan status waiting for approval.'), @STATUS = 404 FROM CrossBrand_StockOutgoingPG a JOIN CrossBrand_StockOutgoingPG_Product b on a.ID = b.IDForm WHERE b.IDProduct = @IDProduct and a.StatusTransOut <> 4 and a.TipeAsal = @TipeLokasi and a.IDAsal = @IDLokasi
                END
            END                               
            SELECT @MESSAGE AS [Message], @STATUS AS [Status]
            
        ";

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(CHECKPLU, conn))
            {
                // command.CommandType = CommandType.StoredProcedure;                
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = PLU });
                command.Parameters.Add(new SqlParameter("@SubStorage", SqlDbType.Int) { Value = 1 });
                // command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = 1 });
                // command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = 1 });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDAsal });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@InHand", SqlDbType.Int) { Value = 1 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                var datas = dt.Rows[0];
                // return ds;

                Data.Add(new
                { 
                    Message = (string)datas["Message"],
                    Status = datas["Status"].ToString(),
                });
            }
        }

        return Data[0];        
    }
}