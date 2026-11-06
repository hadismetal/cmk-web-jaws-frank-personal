using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class DJ_Approve : System.Web.UI.Page
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

    protected int id { get { return App.QueryInt(this); } } /// id trans in

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
            FillIncomingDJ_new();
            // Fill();
            // Fill1();
            // Fill2();
            // Fill3();

            Calculate_QC_Summary();

            cbl_Product.Enabled = false;
            cbl_Product_2.Enabled = false;
            cbl_Product_3.Enabled = false;
        }
    }
    //Fill
    protected void FillIncomingDJ_new()
    {
        cbl_Product.Items.Clear();

        string detailOutgoing = @"
                                select a.ID, A.Nomor,AXX.Nama AS NamaBrand, a.NamaAsal,a.NamaTujuan, CONVERT(varchar(10),a.Tgl,105) tgl, a.NamaKurir
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
                                JOIN StockIncomingDJ xx on xx.IDOutgoing=a.ID
                                WHERE xx.ID=@IDForm

                                SELECT xz.ID,COUNT(xx.IDProduct) totalPLU, SUM(GrossWeight) totalGrossWeight, SUM(NettoWeight) totalNettoWeight
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
                                JOIN StockIncomingDJ xz on xz.IDOutgoing=x.ID
                                where xz.id=@IDForm
                                GROUP BY xz.ID, xs.Nama,xs.totalJenis

                                SELECT B.ID ,E.Nama,COUNT(E.Nama) totalStatus
                                FROM StockOutgoingDJ_Product A
                                JOIN Substorage E ON E.ID=A.SubStorage
                                JOIN StockIncomingDJ B ON B.IDOutgoing=A.IDForm
                                WHERE B.ID=@IDForm
                                GROUP BY B.ID,E.Nama";
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

                outgoing.InnerText = Convert.ToString(data.Rows[0]["Nomor"]);
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
    protected void Fill()
    {

        var rsH = d.StockIncomingDJ.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingDJ.Nomor;
        brand.InnerText = rsH.StockOutgoingDJ.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingDJ.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingDJ.TipeTujuan);
        from.InnerText = rsH.StockOutgoingDJ.NamaAsal;
        to.InnerText = rsH.StockOutgoingDJ.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.StockOutgoingDJ.Tgl);
        kurir.InnerText = rsH.StockOutgoingDJ.NamaKurir;
        keterangan.InnerText = rsH.StockOutgoingDJ.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingDJ.TglETA);

        var item = from p in d.StockIncomingDJ_Product where p.IDForm == id select p;

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
                                   group p by new { p.SubStorageTo } into s
                                   select new
                                   {
                                       SubStorage = s.Key.SubStorageTo,
                                       Qty = s.Count()
                                   };
        foreach (var rsItem in group_product_status)
        {
            product_status_summary.InnerHtml += DL.SubStorage(rsItem.SubStorage) + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }
        // END
    }
    protected void Fill1()
    {
        var item = from p in d.StockIncomingDJ_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == p.SubStorageTo);

        if (item.Count() > 0)
        {
            foreach (var rs in item)
            {
                string v = rs.StockProductDJ.ID.ToString();
                string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductDJ.Nomor,
                        rs.StockProductDJ.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Fill2()
    {
        var item = from p in d.StockIncomingDJ_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == 96);

        if (item.Count() > 0)
        {
            foreach (var rs in item)
            {
                string v = rs.StockProductDJ.ID.ToString();
                string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductDJ.Nomor,
                        rs.StockProductDJ.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                        );
                cbl_Product_2.Items.Add(new ListItem(t, v));
                cbl_Product_2.Items[cbl_Product_2.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Fill3()
    {
        var item = from p in d.StockIncomingDJ_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == 99);

        if (item.Count() > 0)
        {
            foreach (var rs in item)
            {
                string v = rs.StockProductDJ.ID.ToString();
                string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.StockProductDJ.Nomor,
                        rs.StockProductDJ.ImgPicture,
                        DL.SubStorage(rs.SubStorageTo),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
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

    // approve : rubah status 
    protected void approve_Click1(object sender, EventArgs e)
    {
        // StockIncomingDJ nH = new StockIncomingDJ();
        // StockIncomingDJ_Product n = new StockIncomingDJ_Product();
        string DocumentNumber = "";
        string message = "";
        int IDTRM = 0;
        int IDOutgoing = 0;
        string NamaBrand = "";


        string getQueryIncomingDJ = @"SELECT a.ID AS IDTrm, a.IDOutgoing, ba.Nama as NamaBrand, ISNULL(a.Nomor,'') NomorTRM, b.TipeAsal, b.TipeTujuan, b.NamaAsal, b.NamaTujuan, a.StatusTransIn
                                    FROM StockIncomingDJ a
                                    JOIN StockOutgoingDJ b on b.ID=a.IDOutgoing
                                    JOIN CompanyBrand ba on ba.ID=b.IDBrand
                                    WHERE a.ID=@IDTrm and a.StatusTransIn=1 and a.approval is null";

        string queryUpdateIncomingStatus = @"UPDATE A SET Nomor = @NomorTRM, StatusTransIn = @Status, Approval = @Approver, ApprovalTgl = GETDATE() 
                                             FROM(
                                                    SELECT * FROM StockIncomingDJ WHERE ID = @IDTrm and Approval IS NULL and StatusTransIn = 1
                                                )A 
        ";

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(getQueryIncomingDJ, conn))
            {
                cmd.Parameters.AddWithValue("@IDTrm", id);
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);

                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                DataTable data = ds1.Tables[0];
                conn.Close();
                if (data.Rows.Count > 0)
                {
                    if (Convert.ToString(data.Rows[0]["NomorTRM"]) == "")
                    {
                        DocumentNumber = generateDocumentNumber_TransIn();
                    }
                    else
                    {
                        DocumentNumber = Convert.ToString(data.Rows[0]["NomorTRM"]);
                    }

                    NamaBrand = Convert.ToString(data.Rows[0]["NamaBrand"]);
                    IDTRM = Convert.ToInt32(data.Rows[0]["IDTrm"]);
                    IDOutgoing = Convert.ToInt32(data.Rows[0]["IDOutgoing"]);

                }
            }

            using (SqlCommand cmd = new SqlCommand(queryUpdateIncomingStatus, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@NomorTRM", DocumentNumber);
                cmd.Parameters.AddWithValue("@IDTrm", IDTRM);
                cmd.Parameters.AddWithValue("@Approver", App.UserNama);
                cmd.Parameters.AddWithValue("@Status", 2); // 1 : Status Incoming Approved / On The Way
                cmd.ExecuteNonQuery();
                conn.Close();
            }

           // bool checkSuccess = false;
            // bool SAPActivate = false;
            // SAPActivate = SAP.xActivate;

            // var validationResponse = new SAP.ValidationResponse();

            // //check if sap integration false
            // if (SAPActivate == false || (!checkSuccess))
            // {
            //     validationResponse.Success = true;
            //     validationResponse.Info = "Sap Integration Value : " + SAPActivate.ToString();
            // }
            // else //hit wsdl sap new design
            // {
            //     //firman 1 Januari 2018 get sap response transfer sto        
            //     validationResponse = SAP.GetResponseWsdlTransfer(DocumentNumber, id, "I");
            // }

            // if (!validationResponse.Success)
            // {
            //     lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info + ") <br /> </span> <br />";

            //     SAP.InsertStockTransferInvalid(id, "I", NamaBrand, "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString());
            //     return;
            // }
            // else
            // {
            //DI KOMEN ATAS PERMINTAAN PATRICK 09-10-2025
                string queryApprovalIncomingDJ = @"      
                --JANGAN DI KILL TRANSACTION YANG SEDANG BERJALAN BY PATRICK 09-10-2025    
                                            BEGIN TRY
                                                BEGIN TRANSACTION 
                                                IF EXISTS (SELECT 1 FROM StockIncomingDJ a JOIN StockOutgoingDJ b on b.ID = a.IDOutgoing WHERE b.ID = @IDOutgoing and a.ID <> @IDTrm)
                                                BEGIN
                                                    UPDATE A SET StatusTransIn = 3, Approval = @Approver, ApprovalTgl = GETDATE() 
                                                    FROM (SELECT A.StatusTransIn, A.Approval, A.ApprovalTgl 
                                                          FROM StockIncomingDJ a 
                                                          JOIN StockOutgoingDJ b on b.ID = a.IDOutgoing 
                                                          WHERE b.ID = @IDOutgoing and a.ID <> @IDTrm
                                                        ) A
                                                END;                                                
                                                SAVE TRANSACTION Header

                                                IF EXISTS(select 1 from StockOutgoingDJ where id=@IDOutgoing and StatusTransOut=2)
                                                BEGIN
                                                    UPDATE StockOutgoingDJ SET StatusTransOut = 4/*, Approval = @Approver, ApprovalTgl = GETDATE()*/ 
                                                    WHERE ID = @IDOutgoing and StatusTransOut = 2
                                                END
                                                SAVE TRANSACTION Outgoing
                                              
                                                UPDATE aa SET 
                                                    aa.Status = 2,
                                                    aa.Approval = @Approver,
                                                    aa.ApprovalTgl = GETDATE()
                                                FROM StockIncomingDJ_Product aa                                                
                                                JOIN StockActualDJ bb on bb.IDProduct = aa.IDProduct
                                                WHERE aa.IDForm = @IDTrm and bb.Substorage = 98;
                                                SAVE TRANSACTION INCOMING

                                                UPDATE b SET 
                                                     b.Substorage = ax.SubStorageResult
                                                    , b.InHand = 1, b.TipeLokasi = ax.TipeTujuan 
                                                    , b.IDLokasi = ax.IDTujuan, b.NamaLokasi = ax.NamaTujuan
                                                FROM StockActualDJ b                                                
                                                JOIN (
                                                    SELECT 
                                                        b.IDProduct, aa.Status, a.Approval, a.ApprovalTgl, b.Substorage, aa.SubStorageResult, c.TipeTujuan
                                                        , c.IDTujuan, c.NamaTujuan, b.InHand, b.TipeLokasi, b.IDLokasi, b.NamaLokasi
                                                    FROM StockIncomingDJ a
                                                    JOIN StockIncomingDJ_Product aa ON aa.IDForm = a.ID
                                                    JOIN StockActualDJ b ON b.IDProduct = aa.IDProduct
                                                    JOIN StockOutgoingDJ c ON c.ID = a.IDOutgoing
                                                    WHERE a.ID = @IDTrm AND b.Substorage = 98
                                                ) ax ON ax.IDProduct = b.IDProduct;
                                                SAVE TRANSACTION Actual                                                                                               
                                                
                                                INSERT INTO StockLedgerDJ (IDProduct,TipeLokasi,IDLokasi, NamaLokasi, Value, Remark, Keterangan, Operator, OperatorTgl)
                                                SELECT aa.IDProduct,c.TipeTujuan,c.IDTujuan,c.NamaTujuan,1 Value, 4 Remark, @DocumentNumber Keterangan, @Approver, GETDATE()
                                                FROM StockIncomingDJ a
                                                JOIN StockIncomingDJ_Product aa on aa.IDForm=a.ID		
                                                JOIN StockOutgoingDJ c on c.ID=a.IDOutgoing	
                                                where a.ID=@IDTrm
                                                SAVE TRANSACTION Ledger

                                                COMMIT TRANSACTION;
                                                SELECT 'Transaction Succeed: ' + @DocumentNumber AS ErrorMessage;
                                            END TRY
                                            BEGIN CATCH
                                                IF XACT_STATE() <> 0
                                                    ROLLBACK TRANSACTION;                                            
                                                SELECT 'Transaction failed: ' + ERROR_MESSAGE() AS ErrorMessage;
                                            END CATCH
                ";
                
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(queryApprovalIncomingDJ, conn))
                    {
                        cmd.Parameters.AddWithValue("@DocumentNumber", DocumentNumber);
                        cmd.Parameters.AddWithValue("@IDOutgoing", IDOutgoing);
                        cmd.Parameters.AddWithValue("@IDTrm", id);
                        cmd.Parameters.AddWithValue("@Approver", App.UserNama);
                        cmd.Parameters.AddWithValue("@Status", 2); // 1 : Status Incoming Approved / On The Way                        

                        cmd.ExecuteNonQuery();                        
                        
                        message = "Transaction Succeed: " + DocumentNumber;
                        conn.Close();
                    }
                    // conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    message = "Transaction failed: " + ex.Message;
                }


            // }
        }
        // Response.Write(message);

        //Redirect
        if (message.Contains("Transaction Succeed"))
        {
            lit_msg.Text = "<br /><span class=\"alert alert-success\">" + message + "</span> <br />";
            Response.Redirect("DJ.aspx?d=1");
        }
        else
        {
            lit_msg.Text = "<br /><span class=\"alert alert-danger\">" + message + "</span> <br />";
        }
        
        //Response.Redirect("/TransIncoming/Page/Report/DJ_Print.aspx?id=" + id);
    }
    protected void approve_Click(object sender, EventArgs e)
    {
        StockIncomingDJ nH = new StockIncomingDJ();
        StockIncomingDJ_Product n = new StockIncomingDJ_Product();
        string DocumentNumber = "";

        var rsH = d.StockIncomingDJ.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 1 && p.Approval == null);
        if (rsH != null)
        {
            //save dokumen number
            //if not exists generate dokumen number
            if (rsH.Nomor == null || rsH.Nomor == "")
            {
                DocumentNumber = generateDocumentNumber_TransIn();
                rsH.Nomor = DocumentNumber;
                d.SubmitChanges();
            }
            else
            {
                DocumentNumber = rsH.Nomor;
            }
            
            rsH.StatusTransIn = 2;
            rsH.Approval = App.UserNama;
            rsH.ApprovalTgl = DateTime.Now;
            d.SubmitChanges();
			
			Lib.InsertToSAPStaging(DocumentNumber, "F", "DJ", false);

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
                validationResponse = SAP.GetResponseWsdlTransfer(DocumentNumber, id, "I");
            }
            
            if (!validationResponse.Success)
            {
                lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info + ") <br /> </span> <br />";

                SAP.InsertStockTransferInvalid(id, "I", rsH.StockOutgoingDJ.CompanyBrand.Nama, "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString());


                return;
            }
            else
            {
                var rsi = from p in d.StockIncomingDJ
                          where p.StockOutgoingDJ.ID == rsH.IDOutgoing
                          && p.ID != rsH.ID
                          select p;

                foreach (var r in rsi)
                {
                    r.StatusTransIn = 3;
                    r.Approval = App.UserNama;
                    r.ApprovalTgl = DateTime.Now;
                    db.SubmitChanges();
                }

                /// PROSES LANJUTAN APPROVE :
                /// 1. Rubah Header Outgoing dari Approved menjadi Delivered (2 -> 4)
                /// 2. Rubah Stock Actual ke Tipe Location Tujuan (W,O,E)
                /// 3. Rubah Stock Actual ke ID Location Tujuan (1,2,3,...)
                /// 4. Rubah Stock Actual ke arah SubStorage Lanjutan (1,2,3,...)

                // 1
                var rsHout = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == rsH.IDOutgoing && p.StatusTransOut == 2); // 2 : Status Out Approved / On The Way
                rsHout.StatusTransOut = 4; // 4 : Delivered

                // 2,3,4
                var rs = from p in d.StockIncomingDJ_Product where p.IDForm == id && p.StockActualDJ.Substorage == 98 select p;

                foreach (var item in rs)
                {
                    item.Status = 2;
                    item.Approval = App.UserNama;
                    item.ApprovalTgl = DateTime.Now;

                    int SubstorageResult = Convert.ToInt32(item.SubStorageResult);
                    var rsOutItem = d.StockOutgoingDJ_Product.SingleOrDefault(p => p.IDProduct == item.IDProduct && p.IDForm == rsH.IDOutgoing);
                    var rsA = d.StockActualDJ.SingleOrDefault(p => p.IDProduct == rsOutItem.IDProduct);
                    rsA.Substorage = SubstorageResult; // 4

                    // Ledger 
                    StockLedgerDJ l = new StockLedgerDJ();

                    //Input
                    l.IDProduct = item.IDProduct;
                    l.TipeLokasi = item.StockIncomingDJ.StockOutgoingDJ.TipeTujuan;
                    l.IDLokasi = item.StockIncomingDJ.StockOutgoingDJ.IDTujuan;
                    l.NamaLokasi = item.StockIncomingDJ.StockOutgoingDJ.NamaTujuan;

                    // Value (See Library DB - Ledger for Value and Remark)
                    if (SubstorageResult == 99) // (99 : Lost / Trouble , See Library DB - SubStorage)
                    {
                        //rsA.InHand = 0;

                        rsA.InHand = 1;

                        decimal Value = -1; // 1 : in , -1 : out
                        int Remark = 6; // See Library DB - Ledger
                        string Keterangan = DocumentNumber; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        // Tidak Merubah Lokasi Barang terhadap sistem karena brg hilang pengakuannya tetap ada di lokasi asal
                        //rsA.TipeLokasi = rsHout.TipeTujuan; // 2 
                        //rsA.IDLokasi = rsHout.IDTujuan; // 3

                        l.Value = Value;
                        l.Remark = Remark;
                        l.Keterangan = Keterangan;
                    }
                    else if (SubstorageResult == 96) // (96 : Broken , See Library DB - SubStorage)
                    {
                        rsA.InHand = 1;

                        decimal Value = +1; // +1 : in , -1 : out
                        int Remark = 5; // See Library DB - Ledger
                        string Keterangan = DocumentNumber; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (rsHout.TipeTujuan == 0)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);
                        else if (rsHout.TipeTujuan == 1)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);
                        else if (rsHout.TipeTujuan == 2)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);

                        int brand = 0;
                        if (rsHout.TipeTujuan == 0)
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocWarehouse WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);
                        else if (rsHout.TipeTujuan == 1)
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocOutlet WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);
                        else if (rsHout.TipeTujuan == 2)
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocExhibition WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);

                        // Merubah Lokasi Barang terhadap sistem karena brg rusak tetap HARUS diterima di lokasi tujuan
                        rsA.TipeLokasi = rsHout.TipeTujuan; // 2 
                        rsA.IDLokasi = rsHout.IDTujuan; // 3
                        rsA.IDBrand = brand; // update brand
                        rsA.NamaLokasi = Lokasi;

                        l.Value = Value;
                        l.Remark = Remark;
                        l.Keterangan = Keterangan;
                    }
                    else
                    {
                        rsA.InHand = 1;

                        decimal Value = +1; // 1 : in , -1 : out
                        int Remark = 4; // See Library DB - Ledger
                        string Keterangan = DocumentNumber; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        string Lokasi = "";
                        if (rsHout.TipeTujuan == 0)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);
                        else if (rsHout.TipeTujuan == 1)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);
                        else if (rsHout.TipeTujuan == 2)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);

                        int brand = 0;
                        if (rsHout.TipeTujuan == 0)
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocWarehouse WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);
                        else if (rsHout.TipeTujuan == 1)
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocOutlet WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);
                        else if (rsHout.TipeTujuan == 2)
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocExhibition WHERE ID=" + rsHout.IDTujuan, Db.Cnn_DB);

                        rsA.TipeLokasi = rsHout.TipeTujuan; // 2 
                        rsA.IDLokasi = rsHout.IDTujuan; // 3
                        rsA.IDBrand = brand; // update brand
                        rsA.NamaLokasi = Lokasi;

                        l.Value = Value;
                        l.Remark = Remark;
                        l.Keterangan = Keterangan;
                    }

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
        Response.Redirect("DJ.aspx?d=1");
        //Response.Redirect("/TransIncoming/Page/Report/DJ_Print.aspx?id=" + id);
    }


    protected void reject_Click(object sender, EventArgs e)
    {
        StockIncomingDJ nH = new StockIncomingDJ();
        StockIncomingDJ_Product n = new StockIncomingDJ_Product();

        var rsH = d.StockIncomingDJ.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 1 && p.Approval == null);
        if (rsH != null)
        {
            rsH.StatusTransIn = 3;
            rsH.Approval = App.UserNama;
            rsH.ApprovalTgl = DateTime.Now;

            //var rs = from p in d.StockIncomingDJ_Product where p.IDForm == id && p.StockActualDJ.Substorage == 98 select p;
            var rs = from p in d.StockIncomingDJ_Product where p.IDForm == id select p;

            //foreach (var item in rs)
            //{
            //    var ff = d.StockOutgoingDJ_Product.SingleOrDefault(p => p.StockOutgoingDJ.ID == item.StockIncomingDJ.IDOutgoing && p.IDProduct == item.IDProduct);
            //    item.StockActualDJ.Substorage = ff.SubStorage;
            //    item.Status = 3;
            //    item.StockActualDJ.InHand = 1;
            //    item.Approval = App.UserNama;
            //    item.ApprovalTgl = DateTime.Now;

            //    string DocumentNumber = generateDocumentNumber_TransIn();

            //    StockLedgerDJ l = new StockLedgerDJ();

            //    //Input
            //    l.IDProduct = item.IDProduct;
            //    l.TipeLokasi = item.StockIncomingDJ.StockOutgoingDJ.TipeAsal;
            //    l.IDLokasi = item.StockIncomingDJ.StockOutgoingDJ.IDAsal;
            //    l.NamaLokasi = item.StockIncomingDJ.StockOutgoingDJ.NamaAsal;

            //    decimal Value = +1; // 1 : in , -1 : out
            //    int Remark = 4; // See Library DB - Ledger
            //    string Keterangan = DocumentNumber; // Commonly filled by document number or id 
            //    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

            //    // Tidak Merubah Lokasi Barang terhadap sistem karena brg hilang pengakuannya tetap ada di lokasi asal
            //    //rsA.TipeLokasi = rsHout.TipeTujuan; // 2 
            //    //rsA.IDLokasi = rsHout.IDTujuan; // 3

            //    l.Value = Value;
            //    l.Remark = Remark;
            //    l.Keterangan = Keterangan;

            //    //User-Info
            //    l.Operator = App.UserNama;
            //    l.OperatorTgl = DateTime.Now;

            //    //Commit
            //    d.StockLedgerDJ.InsertOnSubmit(l);
            //}

            //Commit
            d.SubmitChanges();
        }

        //Redirect
        Response.Redirect("DJ.aspx?d=1");
    }

    protected string generateDocumentNumber_TransIn()
    {
        string result = "";

        // ex : TRM/13/11/0001
        //      0123456789

        //TRM/DJ/15/1/0001

        string v1 = "TRM/DJ";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2, 2);
        //string v3 = System.DateTime.Now.Month.ToString();
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2,'0');
        string v4 = "0001";

        var rs = from p in d.StockIncomingDJ
                 where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3
                 select p;

        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
}