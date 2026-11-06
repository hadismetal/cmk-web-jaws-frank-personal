using System;
using System.Linq;
using System.Web.UI.WebControls;
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

    protected int id { get { return App.QueryInt(this); } } /// id trans out

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {



            var rs = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id);

            if (rs != null)
            {
                int tf = rs.TipeAsal;
                int vF = rs.IDAsal;
                int tt = rs.TipeTujuan;
                int vT = rs.IDTujuan;


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
            }

            //Form-Default
            Fill1();

        }
    }
    //Fill
    protected void Fill1()
    {
        cbl_Product.Items.Clear();

        string queryGetDetailIncomingDJ = @"
                        select a.ID,AXX.Nama AS NamaBrand, a.NamaAsal,a.NamaTujuan, a.NamaKurir
                        , FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl
                        , FORMAT(CONVERT(DATETIME, a.TglETA, 113), 'dddd, dd MMMM yyyy', 'id-ID') TglETA
                        , b.IDProduct , c.ImgPicture,c.Nomor PLU, caa.Nama as NamaItem,b.SubStorage, a.Keterangan
                        , ca.GrossWeight,ca.NettoWeight,ca.StoneQty, ca.StoneCarat
                        , cb.HargaRupiahRound,a.TipeAsal,a.TipeTujuan
                        FROM StockOutgoingDJ a
                        JOIN StockOutgoingDJ_Product b on b.IDForm=a.ID
                        JOIN StockProductDJ c on c.ID=b.IDProduct
                        JOIN StockProductDJ_CharProduct ca on ca.ID=c.ID
                        JOIN CharProductItem caa on caa.ID=ca.ProductItem
                        JOIN StockProductDJ_PricingMU cb on cb.ID=c.ID
                        JOIN CompanyBrand axx on axx.ID=a.IDBrand
                        WHERE a.ID=@IDForm

                        SELECT x.ID,COUNT(xx.IDProduct) totalPLU, SUM(GrossWeight) totalGrossWeight, SUM(NettoWeight) totalNettoWeight
                        ,SUM(xxx.StoneQty) totalButir, SUM(xxx.StoneCarat) totalCarat, SUM(xss.HargaRupiahRound) totalHarga, xs.Nama,xs.totalJenis
                        FROM StockOutgoingDJ x 
                        JOIN StockOutgoingDJ_Product xx on xx.IDForm=x.ID 
                        JOIN StockProductDJ_CharProduct xxx on xxx.ID=xx.IDProduct
                        JOIN CharProductItem xa on xa.ID=xxx.ProductItem
                        JOIN (
                            SELECT A.IDForm,D.Nama,COUNT(D.Nama) totalJenis
                            FROM StockOutgoingDJ_Product A
                            JOIN StockProductDJ_CharProduct C ON C.ID=A.IDProduct
                            JOIN CharProductItem D ON D.ID=C.ProductItem		
                            GROUP BY A.IDForm, D.Nama
                        )xs on xs.IDForm=x.ID
                        JOIN StockProductDJ_PricingMU xss on xss.ID=xx.IDProduct
                        where x.id=@IDForm
                        GROUP BY x.ID, xs.Nama,xs.totalJenis

                        SELECT A.IDForm,E.Nama,COUNT(E.Nama) totalStatus
                        FROM StockOutgoingDJ_Product A
                        JOIN Substorage E ON E.ID=A.SubStorage
                        WHERE A.IDForm=@IDForm
                        GROUP BY A.IDForm,E.Nama

        ";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(queryGetDetailIncomingDJ, conn))
            {
                cmd.Parameters.AddWithValue("@IDForm", id);
                conn.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);

                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                DataTable data = ds1.Tables[0];
                DataTable data2 = ds1.Tables[1];
                DataTable data3 = ds1.Tables[2];
                conn.Close();

                brand.InnerText = data.Rows[0]["NamaBrand"].ToString();
                lit_type_from.Text = DL.LocationType(Convert.ToInt32(data.Rows[0]["TipeAsal"]));
                lit_type_to.Text = DL.LocationType(Convert.ToInt32(data.Rows[0]["TipeTujuan"]));
                from.InnerText = data.Rows[0]["NamaAsal"].ToString();
                to.InnerText = data.Rows[0]["NamaTujuan"].ToString();

                tgl.InnerText = data.Rows[0]["Tgl"].ToString();
                kurir.InnerText = data.Rows[0]["NamaKurir"].ToString();
                keterangan.InnerText = data.Rows[0]["Keterangan"].ToString();
                eta.InnerText = data.Rows[0]["TglETA"].ToString();

                totalqty.InnerText = Convert.ToInt32(data2.Rows[0]["totalPLU"]).ToString();

                totalgross.InnerText = Str.Number2(Convert.ToDecimal(data2.Rows[0]["totalGrossWeight"]));
                totalnetto.InnerText = Str.Number2(Convert.ToDecimal(data2.Rows[0]["totalNettoWeight"]));
                totalbutir.InnerText = Str.Number0(Convert.ToDecimal(data2.Rows[0]["totalButir"]));
                totalcarat.InnerText = Str.Number3(Convert.ToDecimal(data2.Rows[0]["totalCarat"]));
                // totalhargaM.InnerText = Str.Number0(Convert.ToDecimal(data2.Rows[0]["totalHarga"]));

                product_item_summary.InnerText = "";

                foreach (DataRow rs in data2.Rows)
                {
                    product_item_summary.InnerHtml += rs["Nama"] + " (" + Convert.ToString(rs["totalJenis"]) + ")<br>";
                }

                foreach (DataRow rs in data3.Rows)
                {
                    product_status_summary.InnerHtml += Convert.ToString(rs["Nama"]) + " (" + Convert.ToString(rs["totalStatus"]) + ")<br>";
                }

                // END

                foreach (DataRow rs in data.Rows)
                {
                    string v = Convert.ToInt32(rs["IDProduct"]).ToString();
                    string t = Logistic.ProductList_DJ(
                                Convert.ToString(rs["NamaItem"]),
                                Convert.ToString(rs["PLU"]),
                                Convert.ToString(rs["ImgPicture"]),
                                DL.SubStorage(Convert.ToInt32(rs["SubStorage"])),
                                Str.Number2(Convert.ToDecimal(rs["GrossWeight"])),
                                Str.Number2(Convert.ToDecimal(rs["NettoWeight"])),
                                Str.Number0(Convert.ToDecimal(rs["StoneQty"])),
                                Str.Number3(Convert.ToDecimal(rs["StoneCarat"])),
                                Str.Number0(Convert.ToDecimal(rs["HargaRupiahRound"]))
                                );
                    cbl_Product.Items.Add(new ListItem(t, v));
                    cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                }
            }
        }
        cbl_Product.Enabled = false;
    }
    protected void Fill()
    {
        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

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

        int lastInsertedId = 0;
        int statusInsert = 0;
        string insertDataIncoming = @"

        INSERT INTO [dbo].[StockIncomingDJ] (IDOutgoing, Nomor, Tgl, Keterangan, StatusTransIn, Operator, OperatorTgl)        
        VALUES (@IDOutgoing, @Nomor, GETDATE(), @Keterangan, 0, @Operator, GETDATE())

        select scope_identity() as ID
        ";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(insertDataIncoming, connection))
            {
                command.Parameters.Add(new SqlParameter("@IDOutgoing", id));
                command.Parameters.Add(new SqlParameter("@Nomor", DBNull.Value)); // Nomor can be null
                command.Parameters.Add(new SqlParameter("@Keterangan", Convert.ToString(txketerangan.Text)));
                // command.Parameters.Add(new SqlParameter("@StatusTransIn", 0)); // Draft
                command.Parameters.Add(new SqlParameter("@Operator", App.UserNama));

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

            if (lastInsertedId > 0)
            {
                string getProductsOutgoingDJ = @"                                                
                            INSERT INTO [dbo].[StockIncomingDJ_Product] (IDForm, IDProduct, SubStorageTo, SubStorageResult, Status, Operator, OperatorTgl)
                            SELECT @IDIncoming, IDProduct, SubStorage, NULL, 0, @Operator, GETDATE() FROM StockOutgoingDJ_Product WHERE IDForm = @IDForm
                        ";

                using (SqlCommand command = new SqlCommand(getProductsOutgoingDJ, connection))
                {
                    command.Parameters.Add(new SqlParameter("@IDForm", id));
                    command.Parameters.Add(new SqlParameter("@IDIncoming", lastInsertedId));
                    command.Parameters.Add(new SqlParameter("@Operator", App.UserNama));

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log or handle your error here                    
                        Response.Write("Insert Product error: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                        Response.Redirect("DJ_QC.aspx?id=" + lastInsertedId);                            
                    }
                    // SqlDataAdapter adapter1 = new SqlDataAdapter(command);

                    // DataSet ds1 = new DataSet();
                    // adapter1.Fill(ds1);
                    // DataTable data = ds1.Tables[0];
                    // connection.Close();

                    // foreach (DataRow row in data.Rows)
                    // {
                    //     string InsertProductIncoming = @"
                    //         INSERT INTO [dbo].[StockIncomingDJ_Product] (IDForm, IDProduct, SubStorageTo, SubStorageResult, Status, Operator, OperatorTgl)
                    //         VALUES (@IDForm, @IDProduct, @SubStorageTo, NULL, 0, @Operator, GETDATE())
                    //     ";
                    //     using (SqlCommand cmdProduct = new SqlCommand(InsertProductIncoming, connection))
                    //     {
                    //         cmdProduct.Parameters.Add(new SqlParameter("@IDForm", lastInsertedId));
                    //         cmdProduct.Parameters.Add(new SqlParameter("@IDProduct", Convert.ToInt32(row["IDProduct"])));
                    //         cmdProduct.Parameters.Add(new SqlParameter("@SubStorageTo", Convert.ToInt32(row["SubStorage"])));
                    //         cmdProduct.Parameters.Add(new SqlParameter("@Operator", App.UserNama));

                    //         try
                    //         {
                    //             connection.Open();
                    //             cmdProduct.ExecuteNonQuery();
                    //         }
                    //         catch (Exception ex)
                    //         {
                    //             // Log or handle your error here
                    //             statusInsert += 1;
                    //             Response.Write("Insert Product error: " + ex.Message);
                    //         }
                    //         finally
                    //         {
                    //             connection.Close();
                    //         }
                    //     }

                    // }

                }

                //Redirect                
                
            }
            else
            {
                // Handle the case where the insert failed
                Response.Write("Insert failed, no ID returned.");
                return;
            }

        }        
    }
    protected void save_Click(object sender, EventArgs e)
    {
        StockIncomingDJ nH = new StockIncomingDJ();

        var rs = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

        //Auto-Input Header
        nH.IDOutgoing = id;
        nH.Nomor = null;
        nH.Tgl = System.DateTime.Now;
        nH.Keterangan = null;
        nH.StatusTransIn = 0; // Draft

        //User-Info
        nH.Operator = App.UserNama;
        nH.OperatorTgl = DateTime.Now;
        nH.Keterangan = txketerangan.Text;
        //Commit
        d.StockIncomingDJ.InsertOnSubmit(nH);
        d.SubmitChanges();

        //Auto-Input List Product
        foreach (var item in rs)
        {
            StockIncomingDJ_Product n = new StockIncomingDJ_Product()
            {
                //Input
                IDForm = nH.ID,
                IDProduct = item.IDProduct,
                SubStorageTo = item.SubStorage,
                SubStorageResult = null,
                Status = 0, // New
                Operator = App.UserNama,
                OperatorTgl = DateTime.Now

            };
            d.StockIncomingDJ_Product.InsertOnSubmit(n);

        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("DJ_QC.aspx?id=" + nH.ID);

    }

}