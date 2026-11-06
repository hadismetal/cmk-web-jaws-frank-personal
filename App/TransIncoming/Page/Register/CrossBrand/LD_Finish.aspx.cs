using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;

public partial class PG_Finish : System.Web.UI.Page
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
            cbl_Product.Enabled = true;
            cbl_Product_2.Enabled = true;
            cbl_Product_3.Enabled = true;

            cbl_Product.Items.Clear();
            cbl_Product_2.Items.Clear();
            cbl_Product_3.Items.Clear();

            //Form-Default
            Fill();
            Fill1();
            Fill2();
            Fill3();

            Calculate_QC_Summary();

            cbl_Product.Enabled = false;
            cbl_Product_2.Enabled = false;
            cbl_Product_3.Enabled = false;
        }
    }
    //Fill
    protected void Fill()
    {

        var rsH = cross.IncomingLD(id).SingleOrDefault();
        outgoing.InnerText = rsH.CrossBrand_StockOutgoingLD.Nomor;
        brand.InnerText = d.CompanyBrand.SingleOrDefault(p => p.ID == rsH.CrossBrand_StockOutgoingLD.IDBrandAsal).Nama;
        lit_type_from.Text = DL.LocationType(rsH.CrossBrand_StockOutgoingLD.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.CrossBrand_StockOutgoingLD.TipeTujuan);
        from.InnerText = rsH.CrossBrand_StockOutgoingLD.NamaAsal;
        to.InnerText = rsH.CrossBrand_StockOutgoingLD.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.CrossBrand_StockOutgoingLD.Tgl);
        kurir.InnerText = rsH.CrossBrand_StockOutgoingLD.NamaKurir;
        keterangan.InnerText = rsH.CrossBrand_StockOutgoingLD.Keterangan;
        eta.InnerText = Str.Tgl(rsH.CrossBrand_StockOutgoingLD.TglETA);
        tdKeterangan.InnerText = rsH.Keterangan;
        var item = rsH.CrossBrand_StockIncomingLD_Product;

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
            product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")";
        }

        // Product Status Regroup DJ
        var group_product_status = from p in item
                                   group p by new { p.SubStorageTo } into s
                                   select new
                                   {
                                       SubStorage = s.Key.SubStorageTo,
                                       Qty = s.Count()
                                   };
        foreach (var rsItem in group_product_status)
        {
            product_status_summary.InnerHtml += DL.SubStorage(Convert.ToInt32(rsItem.SubStorage)) + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }
        // END
    }
    protected void Fill1()
    {
        var item = cross.IncomingLD(id).SingleOrDefault().CrossBrand_StockIncomingLD_Product;
        item = item.Where(p => p.SubStorageResult == p.SubStorageTo).ToList();

        if (item.Count() > 0)
        {
            foreach (var rs in item)
            {
                string v = rs.StockProductLD_Stone1B.ID.ToString();
                string t = Logistic.ProductList_LD(
                            rs.StockProductLD_Stone1B.Nomor,
                            rs.StockProductLD_Stone1B.ImgPicture,
                            DL.SubStorage(rs.SubStorageTo),
                            Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)
                            );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Fill2()
    {
        var item = cross.IncomingLD(id).SingleOrDefault().CrossBrand_StockIncomingLD_Product;
        item = item.Where(p => p.SubStorageResult == 96).ToList();

        if (item.Count() > 0)
        {
            foreach (var rs in item)
            {
                string v = rs.StockProductLD_Stone1B.ID.ToString();
                string t = Logistic.ProductList_LD(
                            rs.StockProductLD_Stone1B.Nomor,
                            rs.StockProductLD_Stone1B.ImgPicture,
                            DL.SubStorage(rs.SubStorageTo),
                            Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)
                            );
                cbl_Product_2.Items.Add(new ListItem(t, v));
                cbl_Product_2.Items[cbl_Product_2.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Fill3()
    {
        var item = cross.IncomingLD(id).SingleOrDefault().CrossBrand_StockIncomingLD_Product;
        item = item.Where(p => p.SubStorageResult == 99).ToList();

        if (item.Count() > 0)
        {
            foreach (var rs in item)
            {
                string v = rs.StockProductLD_Stone1B.ID.ToString();
                string t = Logistic.ProductList_LD(
                            rs.StockProductLD_Stone1B.Nomor,
                            rs.StockProductLD_Stone1B.ImgPicture,
                            DL.SubStorage(rs.SubStorageTo),
                            Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)
                            );
                cbl_Product_3.Items.Add(new ListItem(t, v));
                cbl_Product_3.Items[cbl_Product_3.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Calculate_QC_Summary()
    {
        bool flag_cbl1 = cbl_Product.Enabled;
        bool flag_cbl2 = cbl_Product_2.Enabled;
        bool flag_cbl3 = cbl_Product_3.Enabled;

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = true;

        int cbl1 = 0;
        int cbl2 = 0;
        int cbl3 = 0;

        for (int i = 0; i < cbl_Product.Items.Count; i++) { if (cbl_Product.Items[i].Selected == true) cbl1++; }
        for (int i = 0; i < cbl_Product_2.Items.Count; i++) { if (cbl_Product_2.Items[i].Selected == true) cbl2++; }
        for (int i = 0; i < cbl_Product_3.Items.Count; i++) { if (cbl_Product_3.Items[i].Selected == true) cbl3++; }
        
        accepted.InnerText = cbl1.ToString();
        broken.InnerText = cbl2.ToString();
        troublelost.InnerText = cbl3.ToString();

        cbl_Product.Enabled = flag_cbl1;
        cbl_Product_2.Enabled = flag_cbl2;
        cbl_Product_3.Enabled = flag_cbl3;
    }

    // save !!!
    protected void save_Click(object sender, EventArgs e)
    {
        var rs = cross.IncomingLD(id).SingleOrDefault();

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
        dr["ID"] = rs.ID;
        dr["IDOutgoing"] = rs.IDOutgoing;
        dr["Tgl"] = rs.Tgl;
        dr["Keterangan"] = rs.Keterangan;
        dr["StatusTransIn"] = 1;
        dr["Operator"] = rs.Operator;
        dr["OperatorTgl"] = rs.OperatorTgl;

        dt.Rows.Add(dr);

        int ID = 0;
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_UpdateCrossBrandIncomingLD"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

            conn.Open();
            cmd.ExecuteNonQuery();
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
        foreach (var item in rs.CrossBrand_StockIncomingLD_Product)
        {

            DataRow dr2 = dt2.NewRow();
            dr2["IDForm"] = item.IDForm;
            dr2["IDProduct"] = item.IDProduct;
            dr2["Nomor"] = item.Nomor;
            dr2["SubStorageTo"] = item.SubStorageTo;
            dr2["SubStorageResult"] = item.SubStorageResult;
            dr2["Status"] = 1;
            dr2["Operator"] = item.Operator;
            dr2["OperatorTgl"] = item.OperatorTgl;


            dt2.Rows.Add(dr2);
        }

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_UpdateCrossBrandIncomingLD_Product"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt2); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Redirect
        Response.Redirect("LD.aspx?d=1");
    }
}