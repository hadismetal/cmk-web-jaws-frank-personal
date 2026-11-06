    using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;
using CrossBrand.Model;

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
    Modelize cross = new Modelize();

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

        var rsH = cross.IncomingDJ(id).SingleOrDefault();
        outgoing.InnerText = rsH.CrossBrand_StockOutgoingDJ.Nomor;
        brand.InnerText = d.CompanyBrand.SingleOrDefault(p => p.ID == rsH.CrossBrand_StockOutgoingDJ.IDBrandAsal).Nama;
        lit_type_from.Text = DL.LocationType(rsH.CrossBrand_StockOutgoingDJ.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.CrossBrand_StockOutgoingDJ.TipeTujuan);
        from.InnerText = rsH.CrossBrand_StockOutgoingDJ.NamaAsal;
        to.InnerText = rsH.CrossBrand_StockOutgoingDJ.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.CrossBrand_StockOutgoingDJ.Tgl);
        kurir.InnerText = rsH.CrossBrand_StockOutgoingDJ.NamaKurir;
        keterangan.InnerText = rsH.CrossBrand_StockOutgoingDJ.Keterangan;
        eta.InnerText = Str.Tgl(rsH.CrossBrand_StockOutgoingDJ.TglETA);

        var item = rsH.CrossBrand_StockIncomingDJ_Product;

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
        var item = cross.IncomingDJ(id).SingleOrDefault().CrossBrand_StockIncomingDJ_Product;
        item = item.Where(p => p.SubStorageResult == p.SubStorageTo).ToList();

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
        var item = cross.IncomingDJ(id).SingleOrDefault().CrossBrand_StockIncomingDJ_Product;
        item = item.Where(p => p.SubStorageResult == 96).ToList();

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
        var item = cross.IncomingDJ(id).SingleOrDefault().CrossBrand_StockIncomingDJ_Product;
        item = item.Where(p => p.SubStorageResult == 99).ToList();

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
        StockIncomingDJ nH = new StockIncomingDJ();
        StockIncomingDJ_Product n = new StockIncomingDJ_Product();
        string DocumentNumber = "";
        DateTime approveDate = DateTime.Now;

        var rsH = cross.IncomingDJ(id).SingleOrDefault();
        string brandName = d.CompanyBrand.SingleOrDefault(p => p.ID == rsH.CrossBrand_StockOutgoingDJ.IDBrandAsal).Nama;
        int TipeTujuan = Convert.ToInt32(rsH.CrossBrand_StockOutgoingDJ.TipeTujuan);
        int IDLokasiTujuan = Convert.ToInt32(rsH.CrossBrand_StockOutgoingDJ.IDTujuan);
        
        if (rsH != null)
        {

            //save dokumen number
            //if not exists generate dokumen number
            if (rsH.Nomor == null || rsH.Nomor == "")
            {
                DocumentNumber = generateDocumentNumber_TransInNew(TipeTujuan,IDLokasiTujuan);
                rsH.Nomor = DocumentNumber;
                rsH.ApprovalTgl = approveDate;
            }
            else
            {
                DocumentNumber = rsH.Nomor;
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
                validationResponse = SAP.GetResponseWsdlTransfer(DocumentNumber, id, "I");
            }

            //lit_msg.Text = validationResponse.Info;
            //return;
            //var validationResponse = new SAP.ValidationResponse();

            // validationResponse.Success = true;

            if (!validationResponse.Success)
            {
                //throw new Exception(validationResponse.Info);

                //lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info + ") <br /> </span> <br />";

                SAP.InsertStockTransferInvalid(id, "I", brandName, "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString());
                return;
            }
            else
            {

                /// PROSES LANJUTAN APPROVE :
                /// 1. Rubah Header Outgoing dari Approved menjadi Delivered (2 -> 4)
                /// 2. Rubah Stock Actual ke Tipe Location Tujuan (W,O,E)
                /// 3. Rubah Stock Actual ke ID Location Tujuan (1,2,3,...)
                /// 4. Rubah Stock Actual ke arah SubStorage Lanjutan (1,2,3,...)

                // 1

                // 2,3,4
                var rs = cross.IncomingDJ(rsH.ID).SingleOrDefault();


                DataTable dt = new DataTable();

                dt.Columns.Add("IDForm", typeof(int));
                dt.Columns.Add("IDProduct", typeof(int));
                dt.Columns.Add("Nomor", typeof(string));
                dt.Columns.Add("SubStorageTo", typeof(int));
                dt.Columns.Add("SubStorageResult", typeof(int));
                dt.Columns.Add("Status", typeof(int));
                dt.Columns.Add("Operator", typeof(string));
                dt.Columns.Add("OperatorTgl", typeof(DateTime));
                dt.Columns.Add("Approval", typeof(string));
                dt.Columns.Add("ApprovalTgl", typeof(DateTime));

                foreach (var item in rs.CrossBrand_StockIncomingDJ_Product)
                {
                    DataRow dr = dt.NewRow();
                    dr["IDForm"] = item.IDForm;
                    dr["IDProduct"] = item.IDProduct;
                    dr["Nomor"] = item.Nomor;
                    dr["SubStorageTo"] = item.SubStorageTo;
                    dr["SubStorageResult"] = item.SubStorageTo;
                    dr["Status"] = 2;
                    dr["Operator"] = item.Operator;
                    dr["OperatorTgl"] = item.OperatorTgl;
                    dr["Approval"] = App.UserNama;
                    dr["ApprovalTgl"] = approveDate;

                    dt.Rows.Add(dr);

                    int SubstorageResult = Convert.ToInt32(item.SubStorageResult);
                    var rsA = d.StockActualDJ.SingleOrDefault(p => p.IDProduct == item.IDProduct);
                    rsA.Substorage = SubstorageResult; // 4

                    // Ledger 
                    StockLedgerDJ l = new StockLedgerDJ();

                    //Input
                    l.IDProduct = item.IDProduct;
                    l.TipeLokasi = rsH.CrossBrand_StockOutgoingDJ.TipeTujuan;
                    l.IDLokasi = rsH.CrossBrand_StockOutgoingDJ.IDTujuan;
                    l.NamaLokasi = rsH.CrossBrand_StockOutgoingDJ.NamaTujuan;

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
                        int brand = 0;
                        if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 0)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocWarehouse WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }
                        else if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 1)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocOutlet WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }
                        else if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 2)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocExhibition WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }

                        // Merubah Lokasi Barang terhadap sistem karena brg rusak tetap HARUS diterima di lokasi tujuan
                        rsA.TipeLokasi = rsH.CrossBrand_StockOutgoingDJ.TipeTujuan; // 2 
                        rsA.IDLokasi = rsH.CrossBrand_StockOutgoingDJ.IDTujuan; // 3
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
                        int brand = 0;
                        if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 0)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocWarehouse WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }
                        else if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 1)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocOutlet WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }
                        else if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 2)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocExhibition WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }

                        rsA.TipeLokasi = rsH.CrossBrand_StockOutgoingDJ.TipeTujuan; // 2 
                        rsA.IDLokasi = rsH.CrossBrand_StockOutgoingDJ.IDTujuan; // 3
                        rsA.IDBrand = brand; // update brand
                        rsA.NamaLokasi = Lokasi;
                        rsA.Substorage = 1;

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
                UpdateIncoming_Product(dt);
                UpdateIncoming(rsH);

            }
        }

        //Redirect
        Response.Redirect("DJ.aspx?d=1");
        //Response.Redirect("/TransIncoming/Page/Report/DJ_Print.aspx?id=" + id);
    }
    protected void approve_Click(object sender, EventArgs e)
    {
        StockIncomingDJ nH = new StockIncomingDJ();
        StockIncomingDJ_Product n = new StockIncomingDJ_Product();
        string DocumentNumber = "";
        DateTime approveDate = DateTime.Now;

        var rsH = cross.IncomingDJ(id).SingleOrDefault();
        string brandName = d.CompanyBrand.SingleOrDefault(p => p.ID == rsH.CrossBrand_StockOutgoingDJ.IDBrandAsal).Nama;
        if (rsH != null)
        {

            //save dokumen number
            //if not exists generate dokumen number
            if (rsH.Nomor == null || rsH.Nomor == "")
            {
                DocumentNumber = generateDocumentNumber_TransIn();
                rsH.Nomor = DocumentNumber;
                rsH.ApprovalTgl = approveDate;
            }
            else
            {
                DocumentNumber = rsH.Nomor;
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
                validationResponse = SAP.GetResponseWsdlTransfer(DocumentNumber, id, "I");
            }

            //lit_msg.Text = validationResponse.Info;
            //return;
            //var validationResponse = new SAP.ValidationResponse();

            // validationResponse.Success = true;

            if (!validationResponse.Success)
            {
                //throw new Exception(validationResponse.Info);

                //lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info + ") <br /> </span> <br />";

                SAP.InsertStockTransferInvalid(id, "I", brandName, "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString());


                return;
            }
            else
            {

                /// PROSES LANJUTAN APPROVE :
                /// 1. Rubah Header Outgoing dari Approved menjadi Delivered (2 -> 4)
                /// 2. Rubah Stock Actual ke Tipe Location Tujuan (W,O,E)
                /// 3. Rubah Stock Actual ke ID Location Tujuan (1,2,3,...)
                /// 4. Rubah Stock Actual ke arah SubStorage Lanjutan (1,2,3,...)

                // 1

                // 2,3,4
                var rs = cross.IncomingDJ(rsH.ID).SingleOrDefault();


                DataTable dt = new DataTable();

                dt.Columns.Add("IDForm", typeof(int));
                dt.Columns.Add("IDProduct", typeof(int));
                dt.Columns.Add("Nomor", typeof(string));
                dt.Columns.Add("SubStorageTo", typeof(int));
                dt.Columns.Add("SubStorageResult", typeof(int));
                dt.Columns.Add("Status", typeof(int));
                dt.Columns.Add("Operator", typeof(string));
                dt.Columns.Add("OperatorTgl", typeof(DateTime));
                dt.Columns.Add("Approval", typeof(string));
                dt.Columns.Add("ApprovalTgl", typeof(DateTime));

                foreach (var item in rs.CrossBrand_StockIncomingDJ_Product)
                {
                    DataRow dr = dt.NewRow();
                    dr["IDForm"] = item.IDForm;
                    dr["IDProduct"] = item.IDProduct;
                    dr["Nomor"] = item.Nomor;
                    dr["SubStorageTo"] = item.SubStorageTo;
                    dr["SubStorageResult"] = item.SubStorageTo;
                    dr["Status"] = 2;
                    dr["Operator"] = item.Operator;
                    dr["OperatorTgl"] = item.OperatorTgl;
                    dr["Approval"] = App.UserNama;
                    dr["ApprovalTgl"] = approveDate;

                    dt.Rows.Add(dr);

                    int SubstorageResult = Convert.ToInt32(item.SubStorageResult);
                    var rsA = d.StockActualDJ.SingleOrDefault(p => p.IDProduct == item.IDProduct);
                    rsA.Substorage = SubstorageResult; // 4

                    // Ledger 
                    StockLedgerDJ l = new StockLedgerDJ();

                    //Input
                    l.IDProduct = item.IDProduct;
                    l.TipeLokasi = rsH.CrossBrand_StockOutgoingDJ.TipeTujuan;
                    l.IDLokasi = rsH.CrossBrand_StockOutgoingDJ.IDTujuan;
                    l.NamaLokasi = rsH.CrossBrand_StockOutgoingDJ.NamaTujuan;

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
                        int brand = 0;
                        if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 0)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocWarehouse WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }
                        else if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 1)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocOutlet WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }
                        else if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 2)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocExhibition WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }

                        // Merubah Lokasi Barang terhadap sistem karena brg rusak tetap HARUS diterima di lokasi tujuan
                        rsA.TipeLokasi = rsH.CrossBrand_StockOutgoingDJ.TipeTujuan; // 2 
                        rsA.IDLokasi = rsH.CrossBrand_StockOutgoingDJ.IDTujuan; // 3
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
                        int brand = 0;
                        if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 0)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocWarehouse WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }
                        else if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 1)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocOutlet WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }
                        else if (rsH.CrossBrand_StockOutgoingDJ.TipeTujuan == 2)
                        {
                            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                            brand = Db.SingleInteger("SELECT IdBrand FROM LocExhibition WHERE ID=" + rsH.CrossBrand_StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
                        }

                        rsA.TipeLokasi = rsH.CrossBrand_StockOutgoingDJ.TipeTujuan; // 2 
                        rsA.IDLokasi = rsH.CrossBrand_StockOutgoingDJ.IDTujuan; // 3
                        rsA.IDBrand = brand; // update brand
                        rsA.NamaLokasi = Lokasi;
                        rsA.Substorage = 1;

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
                UpdateIncoming_Product(dt);
                UpdateIncoming(rsH);
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
            //    var ff = d.StockCrossBrand_StockOutgoingDJ_Product.SingleOrDefault(p => p.StockCrossBrand_StockOutgoingDJ.ID == item.StockIncomingDJ.IDOutgoing && p.IDProduct == item.IDProduct);
            //    item.StockActualDJ.Substorage = ff.SubStorage;
            //    item.Status = 3;
            //    item.StockActualDJ.InHand = 1;
            //    item.Approval = App.UserNama;
            //    item.ApprovalTgl = DateTime.Now;

            //    string DocumentNumber = generateDocumentNumber_TransIn();

            //    StockLedgerDJ l = new StockLedgerDJ();

            //    //Input
            //    l.IDProduct = item.IDProduct;
            //    l.TipeLokasi = item.StockIncomingDJ.StockCrossBrand_StockOutgoingDJ.TipeAsal;
            //    l.IDLokasi = item.StockIncomingDJ.StockCrossBrand_StockOutgoingDJ.IDAsal;
            //    l.NamaLokasi = item.StockIncomingDJ.StockCrossBrand_StockOutgoingDJ.NamaAsal;

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

    private void UpdateIncoming(CrossBrand_StockIncomingDJ rsH)
    {
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
        dr["ID"] = rsH.ID;
        dr["IDOutgoing"] = rsH.IDOutgoing;
        dr["Nomor"] = rsH.Nomor;
        dr["Tgl"] = rsH.Tgl;
        dr["Keterangan"] = rsH.Keterangan;
        dr["StatusTransIn"] = 2;
        dr["Operator"] = rsH.Operator;
        dr["OperatorTgl"] = rsH.OperatorTgl;
        dr["Approval"] = App.UserNama;
        dr["ApprovalTgl"] = (DateTime)rsH.ApprovalTgl;

        dt.Rows.Add(dr);

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_UpdateCrossBrandIncomingDJ"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    private void UpdateIncoming_Product(DataTable dt)
    {
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_UpdateCrossBrandIncomingDJ_Product"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    protected string generateDocumentNumber_TransInNew(int TipeTujuan, int IDLokasiTujuan)
    {
        string result = "";        

        // ex : TRM/13/11/0001
        //      0123456789

        //TRM/DJ/15/1/0001

        DataSet ds = new DataSet();
        string v1 = "TRM/F/"+ TipeTujuan+ "/" + IDLokasiTujuan.ToString().PadLeft(3, '0') +"/DJ";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2,2);
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2, '0');
        string v4 = "0001";


        string queryGetKRM = "SELECT COUNT(id) TOTAL FROM CrossBrand_StockIncomingDJ WHERE LEFT(Nomor,20) = @Nomor ";        
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
        
        // var rs = cross.IncomingDJ().Where(p => p.Nomor.Contains("TRM/F/DJ"));
        // if (rs.Count() > 0) rs = rs.Where(p => p.Nomor != null && p.Nomor.Substring(9, 2) == v2 && p.Nomor.Substring(12, 2) == v3);

        // result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
        //                         : v1 + "/" + v2 + "/" + v3 + "/" + v4
        //                         ;

        return result;
    }
    protected string generateDocumentNumber_TransIn()
    {
        string result = "";        
        DataSet ds = new DataSet();

        // ex : TRM/13/11/0001
        //      0123456789

        //TRM/DJ/15/1/0001

        string v1 = "TRM/F/DJ";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2, 2);
        //string v3 = System.DateTime.Now.Month.ToString();
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2, '0');
        string v4 = "0001";


        string queryGetKRM = "SELECT TOP 1 RIGHT(Nomor,1) Nomor FROM CrossBrand_StockIncomingDJ WHERE LEFT(Nomor,14) = @Nomor order by id desc ";
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

                result = ds.Tables.Count > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((Convert.ToInt32(angka["Nomor"]) + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;
            }
            conn.Close();
        }
        // var rs = cross.IncomingDJ().Where(p => p.Nomor.Contains("TRM/F/DJ"));
        // if (rs.Count() > 0) rs = rs.Where(p => p.Nomor != null && p.Nomor.Substring(9, 2) == v2 && p.Nomor.Substring(12, 2) == v3);

        // result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
        //                         : v1 + "/" + v2 + "/" + v3 + "/" + v4
        //                         ;

        return result;
    }
}