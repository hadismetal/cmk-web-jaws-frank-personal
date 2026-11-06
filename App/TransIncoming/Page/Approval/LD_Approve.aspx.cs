using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class LD_Approve : System.Web.UI.Page
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
        
        var rsH = d.StockIncomingLD.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingLD.Nomor;
        brand.InnerText = rsH.StockOutgoingLD.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingLD.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingLD.TipeTujuan);
        from.InnerText = rsH.StockOutgoingLD.NamaAsal;
        to.InnerText = rsH.StockOutgoingLD.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.StockOutgoingLD.Tgl);
        kurir.InnerText = rsH.StockOutgoingLD.NamaKurir;
        keterangan.InnerText = rsH.StockOutgoingLD.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingLD.TglETA);

        var item = from p in d.StockIncomingLD_Product where p.IDForm == id select p;

        var group_product_item = from p in item
                                 group p by new { p.StockProductLD_Stone1B.Stone1B.Nama } into s
                                 select new
                                 {
                                     ProductItem = s.Key.Nama,
                                     Qty = s.Count()
                                 };

        totalqty.InnerText = item.Count().ToString();

        totalbutir.InnerText = Str.Number0(item.Sum(p => p.StockProductLD_Stone1B.TotalButir));
        totalcarat.InnerText = Str.Number3(item.Sum(p => p.StockProductLD_Stone1B.TotalCarat));
        product_item_summary.InnerText = "";

        foreach (var rsItem in group_product_item)
        {
            product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }

        // Product Status Regroup LD
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
        var item = from p in d.StockIncomingLD_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == p.SubStorageTo);

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
        var item = from p in d.StockIncomingLD_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == 96);

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
        var item = from p in d.StockIncomingLD_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == 99);

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

    // approve : rubah status 
    protected void approve_Click(object sender, EventArgs e)
    {
        StockIncomingLD nH = new StockIncomingLD();
        StockIncomingLD_Product n = new StockIncomingLD_Product();
        string DocumentNumber = "";

        var rsH = d.StockIncomingLD.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 1 && p.Approval == null);
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
			
			Lib.InsertToSAPStaging(DocumentNumber, "F", "LD", false);

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
                //lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info + ") <br /> </span> <br />";

                SAP.InsertStockTransferInvalid(id, "I", rsH.StockOutgoingLD.CompanyBrand.Nama, "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString());


                return;
            }
            else
            {

                var rsi = from p in d.StockIncomingLD
                          where p.StockOutgoingLD.ID == rsH.IDOutgoing
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
                var rsHout = d.StockOutgoingLD.SingleOrDefault(p => p.ID == rsH.IDOutgoing && p.StatusTransOut == 2); // 2 : Status Out Approved / On The Way
                rsHout.StatusTransOut = 4; // 4 : Delivered

                // 2,3,4
                var rs = from p in d.StockIncomingLD_Product where p.IDForm == id && p.StockActualLD_Stone1B.Substorage == 98 select p;

                foreach (var item in rs)
                {
                    item.Status = 2;
                    item.Approval = App.UserNama;
                    item.ApprovalTgl = DateTime.Now;

                    int SubstorageResult = Convert.ToInt32(item.SubStorageResult);
                    var rsOutItem = d.StockOutgoingLD_Product.SingleOrDefault(p => p.IDProduct == item.IDProduct && p.IDForm == rsH.IDOutgoing);
                    var rsA = d.StockActualLD_Stone1B.SingleOrDefault(p => p.IDProduct == rsOutItem.IDProduct);
                    rsA.Substorage = SubstorageResult; // 4

                    // Ledger 
                    StockLedgerLD l = new StockLedgerLD();

                    //Input
                    l.IDProduct = item.IDProduct;
                    l.TipeLokasi = item.StockIncomingLD.StockOutgoingLD.TipeTujuan;
                    l.IDLokasi = item.StockIncomingLD.StockOutgoingLD.IDTujuan;
                    l.NamaLokasi = item.StockIncomingLD.StockOutgoingLD.NamaTujuan;

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
                    d.StockLedgerLD.InsertOnSubmit(l);
                }
                
                //Commit
                d.SubmitChanges();
            }

        }
        
        //Redirect
        Response.Redirect("LD.aspx?d=1");
        //Response.Redirect("/TransIncoming/Page/Report/LD_Print.aspx?id=" + id);
    }

    protected void reject_Click(object sender, EventArgs e)
    {
        StockIncomingLD nH = new StockIncomingLD();
        StockIncomingLD_Product n = new StockIncomingLD_Product();

        var rsH = d.StockIncomingLD.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 1 && p.Approval == null);
        if (rsH != null)
        {
            rsH.StatusTransIn = 3;
            rsH.Approval = App.UserNama;
            rsH.ApprovalTgl = DateTime.Now;

            //var rs = from p in d.StockIncomingLD_Product where p.IDForm == id && p.StockActualLD_Stone1B.Substorage == 98 select p;
            var rs = from p in d.StockIncomingLD_Product where p.IDForm == id select p;

            foreach (var item in rs)
            {
                var ff = d.StockOutgoingLD_Product.SingleOrDefault(p => p.StockOutgoingLD.ID == item.StockIncomingLD.IDOutgoing && p.IDProduct == item.IDProduct);
                item.StockActualLD_Stone1B.Substorage = ff.SubStorage;
                item.StockActualLD_Stone1B.InHand = 1;
                item.Status = 3;
                item.Approval = App.UserNama;
                item.ApprovalTgl = DateTime.Now;

                string DocumentNumber = generateDocumentNumber_TransIn();

                StockLedgerLD l = new StockLedgerLD();

                //Input
                l.IDProduct = item.IDProduct;
                l.TipeLokasi = item.StockIncomingLD.StockOutgoingLD.TipeAsal;
                l.IDLokasi = item.StockIncomingLD.StockOutgoingLD.IDAsal;
                l.NamaLokasi = item.StockIncomingLD.StockOutgoingLD.NamaAsal;

                decimal Value = +1; // 1 : in , -1 : out
                int Remark = 4; // See Library DB - Ledger
                string Keterangan = DocumentNumber; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                // Tidak Merubah Lokasi Barang terhadap sistem karena brg hilang pengakuannya tetap ada di lokasi asal
                //rsA.TipeLokasi = rsHout.TipeTujuan; // 2 
                //rsA.IDLokasi = rsHout.IDTujuan; // 3

                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                d.StockLedgerLD.InsertOnSubmit(l);
            }

            //Commit
            d.SubmitChanges();
        }
        //Redirect
        Response.Redirect("LD.aspx?d=1");
    }

    protected string generateDocumentNumber_TransIn()
    {
        string result = "";

        // ex : TRM/13/11/0001
        //      0123456789

        string v1 = "TRM/LD";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2, 2);
        //string v3 = System.DateTime.Now.Month.ToString();
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2, '0');
        string v4 = "0001";

        var rs = from p in d.StockIncomingLD
                 where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3
                 select p;

        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
}