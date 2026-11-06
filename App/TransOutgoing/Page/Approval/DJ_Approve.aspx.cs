using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Net;
using Library.SAP_ZMMFM_STO_INTRFC;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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

    protected int id { get { return App.QueryInt(this); } } /// id trans out

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            //Form-Default
            Fill1();      
        }
    }
    //Fill
    // protected void Fill()
    // {
    //     cbl_Product.Items.Clear();

    //     var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id);
    //     brand.InnerText = rsH.CompanyBrand.Nama;
    //     lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
    //     lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
    //     from.InnerText = rsH.NamaAsal;
    //     to.InnerText = rsH.NamaTujuan;

    //     tgl.InnerText = Str.Tgl(rsH.Tgl);
    //     kurir.InnerText = rsH.NamaKurir;
    //     keterangan.InnerText = rsH.Keterangan;
    //     eta.InnerText = Str.Tgl(rsH.TglETA);

    //     var item = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

    //     var group_product_item = from p in item
    //                              group p by new { p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama } into s
    //                              select new
    //                              {  
    //                                  ProductItem = s.Key.Nama,
    //                                  Qty = s.Count()
    //                              };

    //     totalqty.InnerText = item.Count().ToString();

    //     totalgross.InnerText = Str.Number2(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight));
    //     totalnetto.InnerText = Str.Number2(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight));
    //     totalbutir.InnerText = Str.Number0(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty));
    //     totalcarat.InnerText = Str.Number3(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat));
    //     totalhargaM.InnerText = Str.Number0(item.Sum(p => p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound));
    //     product_item_summary.InnerText = "";

    //     foreach (var rsItem in group_product_item)
    //     {
    //         product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
    //     }

    //     // Product Status Regroup DJ
    //     var group_product_status = from p in item
    //                              group p by new { p.SubStorage } into s
    //                              select new
    //                              {
    //                                  SubStorage = s.Key.SubStorage,
    //                                  Qty = s.Count()
    //                              };
    //     foreach (var rsItem in group_product_status)
    //     {
    //         product_status_summary.InnerHtml += DL.SubStorage(rsItem.SubStorage) + " (" + rsItem.Qty.ToString("0") + ")<br>";
    //     }
    //     // END

    //     foreach (var rs in item)
    //     {
    //         string v = rs.StockProductDJ.ID.ToString();
    //         string t = Logistic.ProductList_DJ(
    //                     rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
    //                     rs.StockProductDJ.Nomor,
    //                     rs.StockProductDJ.ImgPicture,
    //                     DL.SubStorage(rs.SubStorage),
    //                     Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
    //                     Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
    //                     Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
    //                     Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
    //                     Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
    //                     );
    //         cbl_Product.Items.Add(new ListItem(t, v));
    //         cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
    //     }
    //     cbl_Product.Enabled = false;
    // }
    
    protected void Fill1()
    {
        cbl_Product.Items.Clear();

        string detailOutgoing = @"
                                select a.ID,AXX.Nama AS NamaBrand, a.NamaAsal,a.NamaTujuan, CONVERT(varchar(10),a.Tgl,105) tgl, a.NamaKurir
                                ,a.Keterangan, CONVERT(varchar(10),a.TglETA,105) tglETA,b.IDProduct , c.ImgPicture,c.Nomor PLU, caa.Nama as NamaItem,b.SubStorage
                                ,ca.GrossWeight,ca.NettoWeight,ca.StoneQty, ca.StoneCarat
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
                                GROUP BY A.IDForm,E.Nama";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(detailOutgoing, conn))
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

                brand.InnerText = Convert.ToString(data.Rows[0]["NamaBrand"]);
                lit_type_from.Text = DL.LocationType(Convert.ToInt32(data.Rows[0]["TipeAsal"]));
                lit_type_to.Text = DL.LocationType(Convert.ToInt32(data.Rows[0]["TipeTujuan"]));
                from.InnerText = Convert.ToString(data.Rows[0]["NamaAsal"]);
                to.InnerText = Convert.ToString(data.Rows[0]["NamaTujuan"]);

                tgl.InnerText = Convert.ToString(data.Rows[0]["tgl"]);
                kurir.InnerText = Convert.ToString(data.Rows[0]["NamaKurir"]);
                keterangan.InnerText = Convert.ToString(data.Rows[0]["Keterangan"]);
                eta.InnerText = Convert.ToString(data.Rows[0]["tglETA"]);


                totalqty.InnerText = Convert.ToInt32(data2.Rows[0]["totalPLU"]).ToString();

                totalgross.InnerText = Str.Number2(Convert.ToDecimal(data2.Rows[0]["totalGrossWeight"]));
                totalnetto.InnerText = Str.Number2(Convert.ToDecimal(data2.Rows[0]["totalNettoWeight"]));
                totalbutir.InnerText = Str.Number0(Convert.ToDecimal(data2.Rows[0]["totalButir"]));
                totalcarat.InnerText = Str.Number3(Convert.ToDecimal(data2.Rows[0]["totalCarat"]));
                totalhargaM.InnerText = Str.Number0(Convert.ToDecimal(data2.Rows[0]["totalHarga"]));

                product_item_summary.InnerText = "";

                foreach (DataRow rs in data2.Rows)
                {
                    product_item_summary.InnerHtml += rs["Nama"] + " (" + Convert.ToString(rs["totalJenis"])+ ")<br>";
                }

                foreach (DataRow rs in data3.Rows)
                {
                    product_status_summary.InnerHtml += Convert.ToString(rs["Nama"]) + " (" + Convert.ToString(rs["totalStatus"]) + ")<br>";
                }
                

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
                cbl_Product.Enabled = false;

            }
        }        
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

    protected void save_Click(object sender, EventArgs e)
    {
        cbl_Product.Enabled = true;
        StockOutgoingDJ nH = new StockOutgoingDJ();
        StockOutgoingDJ_Product n = new StockOutgoingDJ_Product();
        string DocumentNumber = "";
        string CompanyBrand = "";



        // var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 1 && p.Approval == null);        

        string getDataOutgoingDJ = @"SELECT ISNULL(a.Nomor,'') Nomor,b.Nama as NamaBrand
                        FROM StockOutgoingDJ a
                        JOIN CompanyBrand b on b.ID = a.IDBrand
                        WHERE a.ID = @ID and a.StatusTransOut = 1 and a.Approval IS NULL";
        int totalData = 0;

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(getDataOutgoingDJ, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);
                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);


                DataTable data = ds1.Tables[0];

                if (data.Rows.Count > 0)
                {
                    totalData = data.Rows.Count;
                }

                foreach (DataRow rs in data.Rows)
                {
                    DocumentNumber = Convert.ToString(rs["Nomor"]);
                    CompanyBrand = Convert.ToString(rs["NamaBrand"]);
                }

                conn.Close();
            }
        }

        if (totalData > 0)
        {
            //if not exists generate dokumen number
            if (DocumentNumber == null || DocumentNumber == "")
            {
                DocumentNumber = generateDocumentNumber_TransOut();
            }

            string Approved = @"UPDATE StockOutgoingDJ set Nomor = @NomorKRM, StatusTransOut = @statusTransOut, Approval = @Approver, ApprovalTgl = GETDATE() WHERE ID=@ID";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Approved, conn))
                {
                    cmd.Parameters.AddWithValue("@NomorKRM", DocumentNumber);
                    cmd.Parameters.AddWithValue("@statusTransOut", "2");
                    cmd.Parameters.AddWithValue("@Approver", App.UserNama);
                    cmd.Parameters.AddWithValue("@ID", id);                    
                    conn.Open();
                    SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);
                    DataSet ds1 = new DataSet();
                    adapter1.Fill(ds1);                        
                    conn.Close();
                }
            }

            // rsH.StatusTransOut = 2; // 2 : Approved
            // rsH.Approval = App.UserNama;
            // rsH.ApprovalTgl = DateTime.Now;
            // d.SubmitChanges();

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

                SAP.InsertStockTransferInvalid(id, "O", CompanyBrand, msg);

            }
            else
            {
                // item.StockActualDJ.Substorage = 98; // Product On The Way !!
                // item.StockActualDJ.InHand = 0; // InHand out
                string OutgoingDJ_Detail = @"UPDATE A SET A.Substorage = 98, A.InHand = 0
                                            FROM (
                                                    select * from StockActualDJ where IDProduct in (select IDProduct from StockOutgoingDJ_Product where IDForm=@ID)
                                                 )A

                                            select a.IDProduct, b.* from StockOutgoingDJ_Product a
                                            JOIN StockOutgoingDJ b on a.IDForm = b.ID
                                            where IDForm=@ID
                                            ";
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(OutgoingDJ_Detail, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        conn.Open();
                        SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);

                        DataSet ds1 = new DataSet();
                        adapter1.Fill(ds1);
                        DataTable data = ds1.Tables[0];
                        conn.Close();


                        decimal Value = -1; // 1 : in , -1 : out
                        int Remark = 3; // See Library DB - Ledger
                        string Keterangan = DocumentNumber; // Commonly filled by document number or id 

                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!
                        foreach (DataRow rs in data.Rows)
                        {                        
                            string insertLedgerDJ = @"INSERT INTO StockLedgerDJ (IDProduct, TipeLokasi, IDLokasi, NamaLokasi, Value, Remark, Keterangan, Operator, OperatorTgl)
                                                      VALUES (@IDProduct, @TipeLokasi, @IDLokasi, @NamaLokasi, @Value, @Remark, @Keterangan, @Operator, GETDATE())";
                            using (SqlCommand cmd1 = new SqlCommand(insertLedgerDJ, conn))
                            {
                                cmd1.Parameters.AddWithValue("@IDProduct", Convert.ToInt32(rs["IDProduct"]));
                                cmd1.Parameters.AddWithValue("@TipeLokasi", Convert.ToInt32(rs["TipeAsal"]));
                                cmd1.Parameters.AddWithValue("@IDLokasi", Convert.ToInt32(rs["IDAsal"]));
                                cmd1.Parameters.AddWithValue("@NamaLokasi", Convert.ToString(rs["NamaAsal"]));
                                cmd1.Parameters.AddWithValue("@Value", Value);
                                cmd1.Parameters.AddWithValue("@Remark", Remark);
                                cmd1.Parameters.AddWithValue("@Keterangan", Keterangan);
                                cmd1.Parameters.AddWithValue("@Operator", App.UserNama);                                

                                conn.Open();
                                cmd1.ExecuteNonQuery();
                                conn.Close();
                            }                            
                        }                                                        
                    }
                }
                // var rs = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

                // foreach (var item in rs)
                // {
                //     item.StockActualDJ.Substorage = 98; // Product On The Way !!
                //     item.StockActualDJ.InHand = 0; // InHand out

                //     // Ledger 
                //     StockLedgerDJ l = new StockLedgerDJ();

                //     // Value (See Library DB - Ledger for Value and Remark)
                //     decimal Value = -1; // 1 : in , -1 : out
                //     int Remark = 3; // See Library DB - Ledger
                //     string Keterangan = DocumentNumber; // Commonly filled by document number or id 
                //     if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                //     //Input
                //     l.IDProduct = item.IDProduct;
                //     l.TipeLokasi = item.StockOutgoingDJ.TipeAsal;
                //     l.IDLokasi = item.StockOutgoingDJ.IDAsal;
                //     l.NamaLokasi = item.StockOutgoingDJ.NamaAsal;
                //     l.Value = Value;
                //     l.Remark = Remark;
                //     l.Keterangan = Keterangan;

                //     //User-Info
                //     l.Operator = App.UserNama;
                //     l.OperatorTgl = DateTime.Now;

                //     //Commit
                //     d.StockLedgerDJ.InsertOnSubmit(l);
                // }

                //Commit
                // d.SubmitChanges();

            }

        }
        //Redirect
        string pageurl = "/TransOutgoing/Page/Report/DJ_Print.aspx?id=" + id;
        Response.Write("<script>");
        Response.Write("window.open('" + pageurl + "','_blank')");
        Response.Write("</script>");
        Response.Write("<script>window.location.href='DJ.aspx?id=1';</script>");


    }
    // protected void save_Click(object sender, EventArgs e)
    // {
    //     cbl_Product.Enabled = true;
    //     StockOutgoingDJ nH = new StockOutgoingDJ();
    //     StockOutgoingDJ_Product n = new StockOutgoingDJ_Product();
    //     string DocumentNumber = ""; 

    //     var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 1 && p.Approval == null);        
    //     if (rsH != null)
    //     {
    //         //if not exists generate dokumen number
    //         if (rsH.Nomor == null || rsH.Nomor == "")
    //         {
    //             DocumentNumber = generateDocumentNumber_TransOut();
    //             rsH.Nomor = DocumentNumber;
    //             d.SubmitChanges();
    //         }
    //         else
    //         {
    //             DocumentNumber = rsH.Nomor;
    //         }
            
    //         rsH.StatusTransOut = 2; // 2 : Approved
    //         rsH.Approval = App.UserNama;
    //         rsH.ApprovalTgl = DateTime.Now;
    //         d.SubmitChanges();

    //         //get sap integration value
    //         bool checkSuccess = false;
    //         bool SAPActivate = false;
    //         SAPActivate = SAP.xActivate;

    //         var validationResponse = new SAP.ValidationResponse();

    //         //check if sap integration false
    //         if (SAPActivate == false || (!checkSuccess))
    //         {
    //             validationResponse.Success = true;
    //             validationResponse.Info = "Sap Integration Value : " + SAPActivate.ToString();
    //         }
    //         else //hit wsdl sap new design
    //         {
    //             //firman 1 Januari 2018 get sap response transfer sto        
    //             validationResponse = SAP.GetResponseWsdlTransfer(DocumentNumber, id, "O");
    //         }
            
    //         if (!validationResponse.Success)
    //         {

    //             string msg = "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString();
    //             // throw new Exception(validationResponse.Info);
    //             lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info  + ") <br /> </span> <br /> ";

    //             SAP.InsertStockTransferInvalid(id, "O" ,rsH.CompanyBrand.Nama, msg);
                
    //         } 
    //         else
    //         {
    //             var rs = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

    //             foreach (var item in rs)
    //             {
    //                 item.StockActualDJ.Substorage = 98; // Product On The Way !!
    //                 item.StockActualDJ.InHand = 0; // InHand out

    //                 // Ledger 
    //                 StockLedgerDJ l = new StockLedgerDJ();

    //                 // Value (See Library DB - Ledger for Value and Remark)
    //                 decimal Value = -1; // 1 : in , -1 : out
    //                 int Remark = 3; // See Library DB - Ledger
    //                 string Keterangan = DocumentNumber; // Commonly filled by document number or id 
    //                 if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

    //                 //Input
    //                 l.IDProduct = item.IDProduct;
    //                 l.TipeLokasi = item.StockOutgoingDJ.TipeAsal;
    //                 l.IDLokasi = item.StockOutgoingDJ.IDAsal;
    //                 l.NamaLokasi = item.StockOutgoingDJ.NamaAsal;
    //                 l.Value = Value;
    //                 l.Remark = Remark;
    //                 l.Keterangan = Keterangan;

    //                 //User-Info
    //                 l.Operator = App.UserNama;
    //                 l.OperatorTgl = DateTime.Now;

    //                 //Commit
    //                 d.StockLedgerDJ.InsertOnSubmit(l);
    //             }

    //             //Commit
    //             d.SubmitChanges();

    //         }

    //     }
    //     //Redirect
    //     string pageurl = "/TransOutgoing/Page/Report/DJ_Print.aspx?id=" + id;
    //     Response.Write("<script>");
    //     Response.Write("window.open('" + pageurl + "','_blank')");
    //     Response.Write("</script>");
    //     Response.Write("<script>window.location.href='DJ.aspx?id=1';</script>");
        
        
    // }

    protected void reject_Click(object sender, EventArgs e)
    {
        cbl_Product.Enabled = true;
        StockOutgoingDJ nH = new StockOutgoingDJ();
        StockOutgoingDJ_Product n = new StockOutgoingDJ_Product();

        var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 1 && p.Approval == null);
        if (rsH != null)
        {
            rsH.StatusTransOut = 3; // 3 : Reject
            rsH.Approval = App.UserNama;
            rsH.ApprovalTgl = DateTime.Now;

            var rs = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

            foreach (var item in rs)
            {
                item.StockActualDJ.Substorage = item.SubStorage; // return SubStorage Value !!!
            }

            //Commit
            d.SubmitChanges();
        }

        //Redirect
        Response.Redirect("DJ.aspx?d=1");
    }

    protected void update_Click(object sender, EventArgs e)
    {
        string url = string.Empty;

        url = "/TransOutgoing/Page/Update/DJ_Product.aspx?id=" + id;

        Response.Redirect(url);
    }

    protected string generateDocumentNumber_TransOut()
    {
        string result = "";
        string kodebrand = "";
        int idlokasi = 0;
        int TipeLokasi = 0;

        // ex : KRM/13/11/0001
        //      0123456789

        // string query = @"SELECT c.NamaKode as KodeBrand,a.TipeAsal,a.IDAsal FROM StockOutgoingDJ a                         
        //                 JOIN CompanyBrand c on c.ID=a.IDBrand
        //                  WHERE a.ID = @ID";

        // using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        // {
        //     using (SqlCommand cmd = new SqlCommand(query, conn))
        //     {                                                                
        //         cmd.Parameters.AddWithValue("@ID", id);
        //         conn.Open();                                
        //         using (SqlDataReader reader = cmd.ExecuteReader())
        //         {
        //             // Loop through the results
        //             while (reader.Read())
        //             {                                                             
        //                 idlokasi = reader.GetInt32(reader.GetOrdinal("IDAsal"));
        //                 kodebrand = Convert.ToString(reader["KodeBrand"]);
        //                 TipeLokasi = reader.GetInt32(reader.GetOrdinal("TipeAsal"));
        //             }
        //         }
        //         conn.Close(); 
        //     }
        // }     

        // ex : KRM/13/11/0001
        //      0123456789

        string v1 = "KRM"+"/DJ";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2,2);
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2,'0');
        string v4 = "0001";

        var rs = from p in d.StockOutgoingDJ
                 where p.Nomor != null && p.Nomor.Contains(v1 + "/" + v2 + "/" + v3)
                 select p;
        // var rs = from p in d.StockOutgoingDJ
        //          where p.Nomor != null && p.Nomor.Substring(0, 12) == v1 &&p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3 
        //          select p;


        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
    // protected string generateDocumentNumber_TransOut()
    // {
    //     string result = "";

    //     // ex : KRM/13/11/0001
    //     //      0123456789

    //     string v1 = "KRM/DJ";
    //     string v2 = System.DateTime.Now.Year.ToString().Substring(2,2);
    //     string v3 = System.DateTime.Now.Month.ToString().PadLeft(2,'0');
    //     string v4 = "0001";

    //     var rs = from p in d.StockOutgoingDJ
    //              where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3 
    //              select p;


    //     result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((Convert.ToInt32(rs.OrderByDescending(p => p.Nomor).FirstOrDefault().Nomor.Substring(13, 4)) + 1).ToString()).PadLeft(4, '0')
    //                             : v1 + "/" + v2 + "/" + v3 + "/" + v4
    //                             ;

    //     return result;
    // }
}