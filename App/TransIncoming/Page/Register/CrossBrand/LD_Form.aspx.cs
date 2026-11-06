using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;
using CrossBrand.Model;

public partial class PG_Form : System.Web.UI.Page
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

    protected int id { get { return App.QueryInt(this); } } /// id trans out

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {



            var rs = cross.OutgoingDJ(id).SingleOrDefault();

            if (rs != null)
            {
                int tf = rs.TipeAsal;
                int vF = rs.IDAsal;
                int tt = rs.TipeTujuan;
                int vT = rs.IDTujuan;
                int bF = rs.IDBrandAsal;
                int bT = rs.IDBrandTujuan;


                //if (!Lib.TransferValidationCrossBrand(tf, vF, tt, vT, bF, bT))
                //{
                //    Redirect
                //    Response.Redirect("DJ.aspx?id=1&valid=0");
                //}
            }

            //Form-Default
            Fill();

        }
    }
    //Fill
    protected void Fill()
    {
        cbl_Product.Items.Clear();

        var rsH = cross.OutgoingLD(id).SingleOrDefault();
        brand.InnerText = d.CompanyBrand.SingleOrDefault(p => p.ID == rsH.IDBrandAsal).Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = rsH.CrossBrand_StockOutgoingLD_Product;

        var group_product_item = from p in item
                                 group p by new { p.StockProductLD_Stone1B.Stone1B.Nama } into s
                                 select new
                                 {
                                     ProductItem = s.Key.Nama,
                                     Qty = s.Count()
                                 };

        totalqty.InnerText = item.Count().ToString();

        product_item_summary.InnerText = "";

        foreach (var rsItem in group_product_item)
        {
            product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }

        // Product Status Regroup DJ
        var group_product_status = from p in item
                                   group p by new { p.SubStorage } into s
                                   select new
                                   {
                                       SubStorage = s.Key.SubStorage,
                                       Qty = s.Count()
                                   };
        foreach (var rsItem in group_product_status)
        {
            product_status_summary.InnerHtml += DL.SubStorage(rsItem.SubStorage) + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }
        // END

        foreach (var rs in item)
        {
            string v = rs.StockProductLD_Stone1B.ID.ToString();
            string t = Logistic.ProductList_LD(
                        rs.StockProductLD_Stone1B.Nomor,
                        rs.StockProductLD_Stone1B.ImgPicture,
                        DL.SubStorage(rs.SubStorage),
                        Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)
                        );
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
        }
        cbl_Product.Enabled = false;
    }
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == input);

        if (rs1 == null) { message = "Invalid Product Code"; }
        else
        {
            // 2. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
            }
        }

        // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

        return message;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        var rs = cross.OutgoingLD(id).SingleOrDefault();

        DataTable dt = new DataTable();

        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("IDOutgoing", typeof(int));
        dt.Columns.Add("Nomor", typeof(string));
        dt.Columns.Add("Tgl", typeof(DateTime));
        dt.Columns.Add("Keterangan", typeof(string));
        dt.Columns.Add("StatusTransIn", typeof(int));
        dt.Columns.Add("Operator", typeof(string));
        dt.Columns.Add("OperatorTgl", typeof(DateTime));
        dt.Columns.Add("Approval", typeof(string));
        dt.Columns.Add("ApprovalTgl", typeof(DateTime));


        DataRow dr = dt.NewRow();
        dr["ID"] = DBNull.Value;
        dr["IDOutgoing"] = rs.ID;
        dr["Tgl"] = DateTime.Now;
        dr["Keterangan"] = txketerangan.Text;
        dr["StatusTransIn"] = 0;
        dr["Operator"] = App.UserNama;
        dr["OperatorTgl"] = DateTime.Now;

        dt.Rows.Add(dr);

        int ID = 0;
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_InsertCrossBrandIncomingLD"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

            SqlParameter RuturnValue = new SqlParameter("@retValue", SqlDbType.Int);
            RuturnValue.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(RuturnValue);

            conn.Open();
            cmd.ExecuteNonQuery();
            ID = (int)cmd.Parameters["@retValue"].Value;
            conn.Close();
        }

        DataTable dt2 = new DataTable();

        dt2.Columns.Add("IDForm", typeof(int));
        dt2.Columns.Add("IDProduct", typeof(int));
        dt2.Columns.Add("Nomor", typeof(string));
        dt2.Columns.Add("SubStorageTo", typeof(int));
        dt2.Columns.Add("SubStorageResult", typeof(int));
        dt2.Columns.Add("Status", typeof(int));
        dt2.Columns.Add("Operator", typeof(string));
        dt2.Columns.Add("OperatorTgl", typeof(DateTime));
        dt2.Columns.Add("Approval", typeof(string));
        dt2.Columns.Add("ApprovalTgl", typeof(DateTime));



        //Auto-Input List Product
        foreach (var item in rs.CrossBrand_StockOutgoingLD_Product)
        {

            DataRow dr2 = dt2.NewRow();
            dr2["IDForm"] = ID;
            dr2["IDProduct"] = item.IDProduct;
            dr2["Nomor"] = item.Nomor;
            dr2["SubStorageTo"] = 1;
            dr2["Status"] = 0;
            dr2["Operator"] = App.UserNama;
            dr2["OperatorTgl"] = DateTime.Now;


            dt2.Rows.Add(dr2);
        }

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_InsertCrossBrandIncomingLD_Product"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt2); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Redirect
        Response.Redirect("LD_QC.aspx?id=" + ID);

    }
}