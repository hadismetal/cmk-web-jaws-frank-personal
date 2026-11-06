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

public partial class DJ_Product : System.Web.UI.Page
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

            //Form-Default
            Fill();
            FillProduct();
        }
    }
    //Fill
    protected void Fill()
    {
        save.Visible = false;

        cbl_Product.Items.Clear();

        Modelize cross = new Modelize();
        CrossBrand_StockOutgoingDJ rsH = cross.OutgoingDJ(id).SingleOrDefault();

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
    // protected void Fill()
    // {
    //     save.Visible = false;

    //     cbl_Product.Items.Clear();

    //     var rsH = cross.OutgoingDJ(id).SingleOrDefault();
    //     brand.InnerText = d.CompanyBrand.SingleOrDefault(p => p.ID == rsH.IDBrandAsal).Nama;
    //     lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
    //     lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
    //     from.InnerText = rsH.NamaAsal;
    //     to.InnerText = rsH.NamaTujuan;

    //     //inputproduct.Text = "13/11/0018";
    //     inputproduct.Focus();

    // }

    protected void FillProduct()
    {
        /// update form jika produk sudah tidak ada di lokasi asal maka dihapus dari daftar
        var item = cross.OutgoingDJ(id).SingleOrDefault().CrossBrand_StockOutgoingDJ_Product;

        var inhand0 = from p in item where p.StockProductDJ.StockActualDJ.InHand != 1 select p;
        var inhand1 = from p in item where p.StockProductDJ.StockActualDJ.InHand == 1 select p;

        foreach (var s in inhand0) { Db.Execute("DELETE FROM CrossBrand_StockOutgoingDJ_Product WHERE IDProduct=" + s.IDProduct + " AND IDForm=" + id, Db.Cnn_DB); }

        foreach (var r in inhand1)
        {
            //var rs = d.StockActualDJ.SingleOrDefault(p => p.StockProductDJ.Nomor == r.StockProductDJ.Nomor && p.TipeLokasi == r.StockOutgoingDJ.TipeAsal && p.InHand >= 1 );

            string v = r.IDProduct.ToString();
            string t = Logistic.ProductList_DJ(
                    r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                    r.StockProductDJ.Nomor,
                    r.StockProductDJ.ImgPicture,
                    DL.SubStorage(r.SubStorage),
                    Str.Number2(r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                    Str.Number2(r.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                    Str.Number0(r.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                    Str.Number3(r.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                    Str.Number0(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                    );
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

        }

        inputproduct.Text = "";
        inputproduct.Focus();

        save.Visible = cbl_Product.Items.Count > 0 ? true : false;

    }

    [WebMethod]
    public static object CheckDataExistence(string PLU, string IDAsal, string TipeLokasi)
    {
        // List to hold values with their counts
        List<object> Data = new List<object>();

        string CHECKPLU = @"
            DECLARE @MESSAGE varchar(100) = 'Data siap digunakan', @STATUS int = 200, @IDProduct int
            SELECT @IDProduct = ID FROM StockProductDJ where Nomor = @Nomor

            IF NOT EXISTS(SELECT 1 FROM StockActualDJ a where IDLokasi = @IDLokasi and TipeLokasi = @TipeLokasi and Nomor = @Nomor and Substorage = @SubStorage and InHand >= @InHand)
            BEGIN                    
                SELECT @MESSAGE = 'Data tidak ada di lokasi tersebut', @STATUS = 404
            END             
            ELSE
            BEGIN
                IF EXISTS(SELECT 1 FROM SalesOrderDJ a JOIN SalesOrder b on b.ID=a.IDForm where a.IDProduct = @IDProduct and b.StatusPembayaran = 0 and b.Draft = 0)
                BEGIN
                    SELECT @MESSAGE = 'Data berstatus waiting for payment pada nomor SO '+b.Nomor, @STATUS = 404 
                    FROM SalesOrderDJ a JOIN SalesOrder b on b.ID=a.IDForm where a.IDProduct = @IDProduct and b.StatusPembayaran = 0 and b.Draft = 0
                END
                ELSE IF EXISTs(SELECT 1 FROM SalesOrderDJ a JOIN SalesOrder b on b.ID=a.IDForm where a.IDProduct = @IDProduct and b.Draft = 1)
                BEGIN
                    SELECT @MESSAGE = 'Data berstatus draft pada penjualan', @STATUS = 404
                END
                ELSE IF EXISTS(SELECT 1 FROM StockOutgoingDJ a JOIN StockOutgoingDJ_product b on b.IDForm = a.ID WHERE b.IDProduct = @IDProduct and a.StatusTransOut <> 4 and a.TipeAsal = @TipeLokasi and a.IDAsal = @IDLokasi)
                BEGIN
                    SELECT @MESSAGE = 'Barang sudah terinput pada menu outgoing'+ IIF(a.StatusTransOut = 0, ' dan berstatus draft.', ' dengan status waiting for approval.') , @STATUS = 404 FROM StockOutgoingDJ a JOIN StockOutgoingDJ_product b on b.IDForm = a.ID WHERE b.IDProduct = @IDProduct and a.StatusTransOut <> 4 and a.TipeAsal = @TipeLokasi and a.IDAsal = @IDLokasi
                END
                ELSE IF EXISTS(SELECT 1 FROM CrossBrand_StockOutgoingDJ a JOIN CrossBrand_StockOutgoingDJ_Product b on a.ID = b.IDForm WHERE b.IDProduct = @IDProduct and a.StatusTransOut <> 4 and a.TipeAsal = @TipeLokasi and a.IDAsal = @IDLokasi)
                BEGIN
                    SELECT @MESSAGE = 'Barang sudah terinput pada menu outgoing crossbrand '+IIF(a.StatusTransOut = 0, 'dan berstatus draft', 'dengan status waiting for approval'), @STATUS = 404 FROM CrossBrand_StockOutgoingDJ a JOIN CrossBrand_StockOutgoingDJ_Product b on a.ID = b.IDForm WHERE b.IDProduct = @IDProduct and a.StatusTransOut <> 4 and a.TipeAsal = @TipeLokasi and a.IDAsal = @IDLokasi                    
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

        Lib.FormRegisterUpdateDokumenDraft_DJ_CrossBrandV3(plit_Message, pinputproduct, psave, pcbl_Product, pid);
        // DataSet dataDJ = getPLUDJ(PLU, TipeLokasiAsal, IDLokasiAsal);
    }

    //InputProduct
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        Literal plit_Message = lit_Message;
        TextBox pinputproduct = inputproduct;
        System.Web.UI.HtmlControls.HtmlButton psave = save;
        CheckBoxList pcbl_Product = cbl_Product;
        int pid = id;
        string Mode = "update";

        Lib.FormRegisterUpdateDokumenDraft_DJ_CrossBrand(plit_Message, pinputproduct, psave, pcbl_Product, pid, Mode);
    }
    protected decimal CalculateTotalButir(int idproduct)
    {
        decimal result = 0m;

        var rs1a = from p in d.StockProductDJ_Stone1A where p.StockProductDJ.ID == idproduct select p;
        var rs1b = from p in d.StockProductDJ_Stone1B where p.StockProductDJ.ID == idproduct select p;
        var rs2 = from p in d.StockProductDJ_Stone2 where p.StockProductDJ.ID == idproduct select p;
        var rs3 = from p in d.StockProductDJ_Stone3 where p.StockProductDJ.ID == idproduct select p;
        var rs4 = from p in d.StockProductDJ_Stone4 where p.StockProductDJ.ID == idproduct select p;
        var rs5 = from p in d.StockProductDJ_Stone5 where p.StockProductDJ.ID == idproduct select p;

        foreach (var i1a in rs1a) { result += i1a.TotalButir; }
        foreach (var i1b in rs1b) { result += i1b.TotalButir; }
        foreach (var i2 in rs2) { result += i2.TotalButir; }
        foreach (var i3 in rs3) { result += i3.TotalButir; }
        foreach (var i4 in rs4) { result += i4.TotalButir; }
        foreach (var i5 in rs5) { result += i5.TotalButir; }

        return result;
    }
    protected decimal CalculateTotalCarat(int idproduct)
    {
        decimal result = 0m;

        var rs1a = from p in d.StockProductDJ_Stone1A where p.StockProductDJ.ID == idproduct select p;
        var rs1b = from p in d.StockProductDJ_Stone1B where p.StockProductDJ.ID == idproduct select p;
        var rs2 = from p in d.StockProductDJ_Stone2 where p.StockProductDJ.ID == idproduct select p;
        var rs3 = from p in d.StockProductDJ_Stone3 where p.StockProductDJ.ID == idproduct select p;
        var rs4 = from p in d.StockProductDJ_Stone4 where p.StockProductDJ.ID == idproduct select p;
        var rs5 = from p in d.StockProductDJ_Stone5 where p.StockProductDJ.ID == idproduct select p;

        foreach (var i1a in rs1a) { result += i1a.TotalCarat; }
        foreach (var i1b in rs1b) { result += i1b.TotalCarat; }
        foreach (var i2 in rs2) { result += i2.TotalCarat; }
        foreach (var i3 in rs3) { result += i3.TotalCarat; }
        foreach (var i4 in rs4) { result += i4.TotalCarat; }
        foreach (var i5 in rs5) { result += i5.TotalCarat; }

        return result;
    }
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductDJ.SingleOrDefault(p => p.Nomor == input);

        if (rs1 == null) { message = "Invalid Product"; }
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
        Db.Execute("DELETE FROM CrossBrand_StockOutgoingDJ_Product WHERE IDForm=" + id,Db.Cnn_DB);

        DataTable dt = new DataTable();

        dt.Columns.Add("IDForm", typeof(int));
        dt.Columns.Add("IDProduct", typeof(int));
        dt.Columns.Add("Nomor", typeof(string));
        dt.Columns.Add("Substorage", typeof(int));

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                var rs = d.StockActualDJ.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value));

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
            cmd.CommandText = "dbo.sp_InsertCrossBrandOutgoingDJ_Product"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Redirect
        Response.Redirect("DJ_Finish.aspx?id=" + id);
        
    }
}