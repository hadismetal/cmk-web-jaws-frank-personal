using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Net;
using Library.SAP_ZMMFM_STO_INTRFC;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using CrossBrand;
using CrossBrand.Model;
using System.Drawing.Printing;
using System.Security.Cryptography;
using System.Drawing;

public partial class DJ_Finish : System.Web.UI.Page
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
    protected int IDLokasi { get; set; }
    protected int TipeLokasi { get; set; }
    

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
    protected void Fill()
    {
        cbl_Product.Items.Clear();

        var rsH = cross.OutgoingDJ(id).SingleOrDefault();
        brand.InnerText = d.CompanyBrand.SingleOrDefault(c => c.ID == rsH.IDBrandAsal).Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);
        IDLokasi = Convert.ToInt32(rsH.IDAsal);
        TipeLokasi = Convert.ToInt32(rsH.TipeAsal);

        var item = rsH.CrossBrand_StockOutgoingDJ_Product;

        var group_product_item = from p in item
                                 group p by new { p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama } into s
                                 select new
                                 {
                                     ProductItem = s.Key.Nama,
                                     Qty = s.Count()
                                 };

        totalqty.InnerText = item.Count().ToString();

        totalgross.InnerText = Str.Number2(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight));
        totalnetto.InnerText = Str.Number2(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight));
        totalbutir.InnerText = Str.Number0(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty));
        totalcarat.InnerText = Str.Number3(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat));
        totalhargaM.InnerText = Str.Number0(item.Sum(p => p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound));
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
            string v = rs.StockProductDJ.ID.ToString();
            string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductDJ.Nomor,
                        rs.StockProductDJ.ImgPicture,
                        DL.SubStorage(rs.SubStorage),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
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
        var rs1 = d.StockProductDJ.SingleOrDefault(p => p.Nomor == input);

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

    protected void save_Click1(object sender, EventArgs e)
    {
        cbl_Product.Enabled = false;
        string DocumentNumber = "";
        string brandAsal = "";
        string dbName = "";
        int IDBrandAsal = 0;
        int IDBrandTujuan = 0;

        string queryCheckPLU = @"
        
            DECLARE @tabelBarang table(PLU varchar(150))
            IF EXISTS(select 1 from CrossBrand_StockOutgoingDJ_Product a JOIN SalesOrderDJ b on b.IDProduct=a.IDProduct JOIN SalesOrder c on c.ID=b.IDForm where a.IDForm=@ID and StatusPembayaran=1 and statusvoid=0 and draft=0)
            BEGIN
                INSERT INTO @tabelBarang
                select a.Nomor from CrossBrand_StockOutgoingDJ_Product a JOIN SalesOrderDJ b on b.IDProduct=a.IDProduct JOIN SalesOrder c on c.ID=b.IDForm where a.IDForm=@ID and StatusPembayaran=1 and statusvoid=0 and draft=0
            END

            IF EXISTS(select 1 from CrossBrand_StockOutgoingDJ_Product a JOIN SalesOrderSouvenir b on b.IDProductDJ=a.IDProduct JOIN SalesOrder c on c.ID=b.IDForm where a.IDForm=@ID and StatusPembayaran=1 and statusvoid=0 and draft=0)
            BEGIN
                INSERT INTO @tabelBarang
                select a.Nomor from CrossBrand_StockOutgoingDJ_Product a JOIN SalesOrderDJ b on b.IDProduct=a.IDProduct JOIN SalesOrder c on c.ID=b.IDForm where a.IDForm=@ID and StatusPembayaran=1 and statusvoid=0 and draft=0
            END


            IF EXISTS(SELECT 1 FROM @tabelBarang)
            BEGIN
                SELECT STRING_AGG(PLU,', ') PLU From @tabelBarang
            END
        ";


        string strSql = "sp_GetCrossBrandOutgoingDJ";
        string queryUpdate = @"UPDATE A SET Nomor = @Nomor, StatusTransOut = 2, Approval = @Approver, ApprovalTgl = @ApprovalTgl 
                              FROM (SELECT ID, Nomor, StatusTransOut, Approval, ApprovalTgl FROM CrossBrand_StockOutgoingDJ WHERE ID = @IDOutgoing)A
                             ";
        string queryGetBrand = "select Nama as NamaBrandAsal FROM CompanyBrand WHERE ID = @IDBrandAsal ";        
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();


        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(queryCheckPLU, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds2);
            }

            if (ds2.Tables.Count > 0)
            {
                DataTable dtt = ds2.Tables[0];
                save.Disabled = true;                
                lit_msg.Text = "<br /><span class=\"alert alert-danger\">Approve gagal, karena PLU berikut : " + Convert.ToString(dtt.Rows[0]["PLU"]) + " sudah dalam status terjual. Harap klik update atau pengajuan tersebut.<br /></span> <br /> ";
                return;
            }
            else
            {

                using (SqlCommand cmd = new SqlCommand(strSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    int totalRecord = 0;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                }


                if (ds.Tables.Count > 0)
                {
                    DataTable dt1 = ds.Tables[0];
                    IDBrandAsal = Convert.ToInt32(dt1.Rows[dt1.Rows.Count - 1]["IDBrandTujuan"]);

                    var rs = dt1.Rows[dt1.Rows.Count - 1];
                    IDBrandTujuan = (int)rs["IDBrandTujuan"];

                    dbName = new[] { 3, 4 }.Contains(Convert.ToInt32(IDBrandTujuan)) ? "mondial_db_real.dbo." : "palace_db_real.dbo.";

                    var Nomor = Convert.ToString(rs["Nomor"]);
                    DocumentNumber = string.IsNullOrEmpty(Nomor) ? generateDocumentNumber_TransOutNew(Convert.ToInt32(rs["TipeAsal"]), Convert.ToInt32(rs["IDAsal"])) : Nomor;

                    using (SqlCommand cmd2 = new SqlCommand(queryGetBrand, conn))
                    {
                        cmd2.Parameters.AddWithValue("@IDBrandAsal", IDBrandAsal);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd2);
                        adapter.Fill(ds);
                    }

                    using (SqlCommand cmd1 = new SqlCommand(queryUpdate, conn))
                    {
                        cmd1.Parameters.AddWithValue("@IDOutgoing", id);
                        cmd1.Parameters.AddWithValue("@Nomor", DocumentNumber);
                        cmd1.Parameters.AddWithValue("@Approver", App.UserNama);
                        cmd1.Parameters.AddWithValue("@ApprovalTgl", DateTime.Now);

                        cmd1.ExecuteNonQuery();
                    }

                    //get sap integration value
                    bool checkSuccess = false;
                    bool SAPActivate = false;
                    SAPActivate = SAP.xActivate;

                    var validationResponse = new SAP.ValidationResponse();

                    //check if sap integration false
                    if (SAPActivate == false || (!checkSuccess))
                    {
                        validationResponse.Success = true;
                        validationResponse.Info = "Sap Integration Value : " + SAPActivate.ToString();
                    }
                    else //hit wsdl sap new design
                    {
                        //firman 1 Januari 2018 get sap response transfer sto        
                        validationResponse = SAP.GetResponseWsdlTransfer(DocumentNumber, id, "O");
                    }

                    if (!validationResponse.Success)
                    {

                        string msg = "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString();
                        // throw new Exception(validationResponse.Info);
                        lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info + ") <br /> </span> <br /> ";

                        SAP.InsertStockTransferInvalid(id, "O", brandAsal, msg);


                        return;
                    }
                    else
                    {
                        // var rs = rsH.CrossBrand_StockOutgoingDJ_Product;
                        string queryUpdateProduct = @"       
                            DECLARE @RemarkName varchar(100)
                            select @RemarkName = Nama FROM LedgerRemark WHERE ID = @Remark

                            UPDATE a SET Substorage=@Substorage, InHand = @InHand                         	 					
                            FROM StockActualDJ a
                            JOIN CrossBrand_StockOutgoingDJ_Product XA ON XA.IDProduct = a.IDProduct
                            JOIN CrossBrand_StockOutgoingDJ XAA ON XAA.ID=XA.IDForm
                            WHERE xaa.ID = @IDOutgoing


                            INSERT INTO StockLedgerDJ (IDProduct, TipeLokasi, IDLokasi, NamaLokasi, Value, Remark, Keterangan, Operator, OperatorTgl)
                            SELECT xa.IDProduct, XAA.TipeAsal,xaa.IDAsal, xaa.NamaAsal, @Value, @Remark, IIF(@Keterangan IS NULL OR @Keterangan = '', @RemarkName, @Keterangan), @Operator, GETDATE()
                            FROM StockActualDJ a
                            JOIN CrossBrand_StockOutgoingDJ_Product XA ON XA.IDProduct = a.IDProduct
                            JOIN CrossBrand_StockOutgoingDJ XAA ON XAA.ID=XA.IDForm                         
                            WHERE xaa.ID = @IDOutgoing
                            
                        ";

                        decimal Value = -1; // 1 : in , -1 : out
                        int Remark = 3; // See Library DB - Ledger
                        string Keterangan = DocumentNumber; // Commonly filled by document number or id 
                        // if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        using (SqlCommand cmd2 = new SqlCommand(queryUpdateProduct, conn))
                        {
                            cmd2.Parameters.AddWithValue("@IDOutgoing", id);
                            cmd2.Parameters.AddWithValue("@Substorage", 98);
                            cmd2.Parameters.AddWithValue("@InHand", 0);
                            cmd2.Parameters.AddWithValue("@Value", Value);
                            cmd2.Parameters.AddWithValue("@Remark", Remark);
                            cmd2.Parameters.AddWithValue("@Keterangan", Keterangan);
                            cmd2.Parameters.AddWithValue("@Operator", App.UserNama);

                            cmd2.ExecuteNonQuery();
                        }

                        // using (SqlCommand cmd5 = new SqlCommand(dbName + queryInsert, conn))
                        // {
                        //     cmd5.Parameters.AddWithValue("@ID", id);
                        //     cmd5.Parameters.AddWithValue("@IDBrandAsal", Convert.ToInt32(IDBrandAsal));
                        //     cmd5.Parameters.AddWithValue("@IDBrandTujuan", Convert.ToInt32(IDBrandTujuan));
                        //     cmd5.Parameters.AddWithValue("@TipeProduct", "DJ");

                        //     cmd5.ExecuteNonQuery();
                        // }
                    }
                }
                conn.Close();

                string pageurl = "/TransOutgoing/Page/Report/DJCrossBrand_Print.aspx?id=" + id;
                Response.Write("<script>");
                Response.Write("window.open('" + pageurl + "','_blank')");
                Response.Write("</script>");
                Response.Write("<script>window.location.href='DJCrossBrand.aspx?id=1';</script>");
            }
        }

        //Redirect
       

    }

    protected void save_Click(object sender, EventArgs e)
    {
        cbl_Product.Enabled = true;
        StockOutgoingDJ nH = new StockOutgoingDJ();
        StockOutgoingDJ_Product n = new StockOutgoingDJ_Product();
        string DocumentNumber = "";

        var rsH = cross.OutgoingDJ(id).SingleOrDefault();
        string brand = d.CompanyBrand.SingleOrDefault(p => p.ID == rsH.IDBrandAsal).Nama;
        if (rsH != null)
        {
            if (rsH.Nomor == null || rsH.Nomor == "")
            {
                DocumentNumber = generateDocumentNumber_TransOut();
                rsH.Nomor = DocumentNumber;
                d.SubmitChanges();
            }
            else
            {

                DocumentNumber = rsH.Nomor;
            }
            //save dokumen number
            //if not exists generate dokumen number

            UpdateOutgoing(rsH);

            //get sap integration value
            bool checkSuccess = false;
            bool SAPActivate = false;
            SAPActivate = SAP.xActivate;

            var validationResponse = new SAP.ValidationResponse();

            //check if sap integration false
            if (SAPActivate ==  false || (!checkSuccess))
            {
                validationResponse.Success = true;
                validationResponse.Info = "Sap Integration Value : " + SAPActivate.ToString();
            }
            else //hit wsdl sap new design
            {
                //firman 1 Januari 2018 get sap response transfer sto        
                validationResponse = SAP.GetResponseWsdlTransfer(DocumentNumber, id, "O");
            }

            // lit_msg.Text = validationResponse.Success.ToString() + " - " + validationResponse.Info;

            //var validationResponse = new SAP.ValidationResponse();


            //validationResponse.Success = true;

            //  string msg2 = "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString();

            //lit_msg.Text = id.ToString() + "-" + "O" + "-" + rsH.CompanyBrand.Nama + "-" + msg2;
            //return;

            if (!validationResponse.Success)
            {

                string msg = "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString();
                // throw new Exception(validationResponse.Info);
                lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info  + ") <br /> </span> <br /> ";

                SAP.InsertStockTransferInvalid(id, "O" ,brand, msg);


                return;
            } 
            else
            {
                var rs = rsH.CrossBrand_StockOutgoingDJ_Product;

                foreach (var item in rs)
                {
                    var actual = d.StockActualDJ.SingleOrDefault(p => p.IDProduct == item.IDProduct);
                    actual.Substorage = 98; // Product On The Way !!
                    actual.InHand = 0;

                    // Ledger 
                    StockLedgerDJ l = new StockLedgerDJ();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out
                    int Remark = 3; // See Library DB - Ledger
                    string Keterangan = DocumentNumber; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    //Input
                    l.IDProduct = item.IDProduct;
                    l.TipeLokasi = rsH.TipeAsal;
                    l.IDLokasi = rsH.IDAsal;
                    l.NamaLokasi = rsH.NamaAsal;
                    l.Value = Value;
                    l.Remark = Remark;
                    l.Keterangan = Keterangan;

                    //User-Info
                    l.Operator = App.UserNama;
                    l.OperatorTgl = DateTime.Now;

                    //Commit
                    d.StockLedgerDJ.InsertOnSubmit(l);
                }

                //Commit
                d.SubmitChanges();

            }

        }
        //Redirect
        string pageurl = "/TransOutgoing/Page/Report/DJCrossBrand_Print.aspx?id=" + id;
        Response.Write("<script>");
        Response.Write("window.open('" + pageurl + "','_blank')");
        Response.Write("</script>");
        Response.Write("<script>window.location.href='DJCrossBrand.aspx?id=1';</script>");
        
        
    }

    protected void reject_Click(object sender, EventArgs e)
    {
        cbl_Product.Enabled = true;
        StockOutgoingDJ nH = new StockOutgoingDJ();
        StockOutgoingDJ_Product n = new StockOutgoingDJ_Product();

        var rsH = cross.OutgoingDJ(id).SingleOrDefault(p => p.StatusTransOut == 1 && p.Approval == null);
        if (rsH != null)
        {
            rsH.StatusTransOut = 3; // 3 : Reject
            rsH.Approval = App.UserNama;
            rsH.ApprovalTgl = DateTime.Now;

            UpdateOutgoing(rsH);

            List<int> idProduct = (from r in rsH.CrossBrand_StockOutgoingDJ_Product
                                  select r.IDProduct
                                  ).ToList();

            var rs = d.StockActualDJ.Where(p => idProduct.Contains(p.IDProduct));

            foreach (var item in rs)
            {
                item.Substorage = rsH.CrossBrand_StockOutgoingDJ_Product.SingleOrDefault(p => p.IDProduct == item.IDProduct).SubStorage; // return SubStorage Value !!!
            }

            //Commit
            d.SubmitChanges();
        }

        //Redirect
        Response.Redirect("DJCrossBrand.aspx?d=1");
    }

    protected void update_Click(object sender, EventArgs e)
    {
        string url = string.Empty;

        url = "/TransOutgoing/Page/Update/CrossBrand/DJ_Product.aspx?id=" + id;

        Response.Redirect(url);
    }
    
    protected string generateDocumentNumber_TransOutNew(int TipeLokasi, int IDLokasi)
    {
        string result = "";
        DataSet ds = new DataSet();

        // ex : KRM/13/11/0001
        //      0123456789

        string v1 = "KRM/F/"+ TipeLokasi+ "/" + IDLokasi.ToString().PadLeft(3, '0') +"/DJ";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2,2);
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2, '0');
        string v4 = "0001";


        string queryGetKRM = "SELECT COUNT(id) TOTAL FROM CrossBrand_StockOutgoingDJ WHERE LEFT(Nomor,20) = @Nomor ";
        // string queryGetKRM = "SELECT TOP 1 RIGHT(Nomor,4) Nomor FROM CrossBrand_StockOutgoingDJ WHERE NOMOR IS NOT NULL order by id desc ";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(queryGetKRM, conn))
            {
                cmd.Parameters.AddWithValue("@Nomor", v1 + "/" + v2 + "/" + v3);
                // cmd.Parameters.AddWithValue("@Nomor", v1 + "/" + v2 + "/" + v3); // kombinasi nomor yang lama

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);

                DataTable dt1 = ds.Tables[0];
                var angka = dt1.Rows[0];

                result = ds.Tables.Count > 0 ? v1 +  "/" + v2 + "/" + v3 + "/" + ((Convert.ToInt32(angka["TOTAL"]) + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;
                
            }
        }

        // var rs = cross.OutgoingPG().Where(p => p.Nomor.Contains("KRM/F/PG"));
        // if (rs.Count() > 0) rs = rs.Where(p => p.Nomor != null && p.Nomor.Substring(9, 2) == v2 && p.Nomor.Substring(12, 2) == v3);

        // result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
        //                         : v1 + "/" + v2 + "/" + v3 + "/" + v4
        //                         ;

        return result;
    }

    protected string generateDocumentNumber_TransOut()
    {
        string result = "";
        DataSet ds = new DataSet();

        // ex : KRM/13/11/0001
        //      0123456789

        string v1 = "KRM/F/DJ";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2, 2);
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2, '0');
        string v4 = "0001";

        string queryGetKRM = "SELECT COUNT(id) TOTAL FROM CrossBrand_StockOutgoingDJ WHERE LEFT(Nomor,14) = @Nomor ";
        // string queryGetKRM = "SELECT TOP 1 RIGHT(Nomor,4) Nomor FROM CrossBrand_StockOutgoingDJ WHERE NOMOR IS NOT NULL order by id desc ";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(queryGetKRM, conn))
            {
                cmd.Parameters.AddWithValue("@Nomor", v1 + "/" + v2 + "/" + v3);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);

                DataTable dt1 = ds.Tables[0];
                var angka = dt1.Rows[0];

                result = ds.Tables.Count > 0 ? v1 + "/" + TipeLokasi + "/" + IDLokasi + "/" + v2 + "/" + v3 + "/" + ((Convert.ToInt32(angka["TOTAL"]) + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + TipeLokasi + "/" + IDLokasi + "/" + v2 + "/" + v3 + "/" + v4
                                ;
            }
        }
        // var rs = cross.OutgoingDJ().Where(p => p.Nomor.Contains("KRM/F/DJ"));
        // if (rs.Count() > 0) rs = rs.Where(p => p.Nomor != null && p.Nomor.Substring(9, 2) == v2 && p.Nomor.Substring(12, 2) == v3);

        // result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
        //                         : v1 + "/" + v2 + "/" + v3 + "/" + v4
        //                         ;

        return result;
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

        dr["ID"] = id;
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
        dr["StatusTransOut"] = 2;
        dr["Operator"] = rsH.Operator;
        dr["OperatorTgl"] = rsH.OperatorTgl;
        dr["Approval"] = App.UserNama;
        dr["ApprovalTgl"] = DateTime.Now;

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