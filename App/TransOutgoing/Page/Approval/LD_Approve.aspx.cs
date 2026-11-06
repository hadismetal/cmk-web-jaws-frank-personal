using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Net;
using Library.SAP_ZMMFM_STO_INTRFC;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public partial class LD_Finish : System.Web.UI.Page
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
    protected void Fill()
    {
        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingLD.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = from p in d.StockOutgoingLD_Product where p.IDForm == id select p;

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
        totalhargaM.InnerText = Str.Number0(item.Sum(p => p.StockProductLD_Stone1B.HargaTotal));
        product_item_summary.InnerText = "";

        foreach (var rsItem in group_product_item)
        {
            product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")<br>";
        }

        // Product Status Regroup LD
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
        cbl_Product.Enabled = true;
        StockOutgoingLD nH = new StockOutgoingLD();
        StockOutgoingLD_Product n = new StockOutgoingLD_Product();
        string DocumentNumber = "";

        var rsH = d.StockOutgoingLD.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 1 && p.Approval == null);
        if (rsH != null)
        {
            //if not exists generate dokumen number
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

            rsH.StatusTransOut = 2; // 2 : Approved
            rsH.Approval = App.UserNama;
            rsH.ApprovalTgl = DateTime.Now;
            d.SubmitChanges();

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
                //lit_msg.Text = "<br /><span class=\"alert alert-danger\">Posting Sap gagal !!   (" + validationResponse.Info + ") <br /> </span> <br /> ";

                SAP.InsertStockTransferInvalid(id, "O", rsH.CompanyBrand.Nama, msg);
                
            }
            else
            {
                var rs = from p in d.StockOutgoingLD_Product where p.IDForm == id select p;

                foreach (var item in rs)
                {
                    item.StockActualLD_Stone1B.Substorage = 98; // Product On The Way !!
                    item.StockActualLD_Stone1B.InHand = 0; // InHand out

                    // Ledger 
                    StockLedgerLD l = new StockLedgerLD();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out
                    int Remark = 3; // See Library DB - Ledger
                    string Keterangan = DocumentNumber; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    //Input
                    l.IDProduct = item.IDProduct;
                    l.TipeLokasi = item.StockOutgoingLD.TipeAsal;
                    l.IDLokasi = item.StockOutgoingLD.IDAsal;
                    l.NamaLokasi = item.StockOutgoingLD.NamaAsal;
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


        }
        //Redirect
        string pageurl = "/TransOutgoing/Page/Report/LD_Print.aspx?id=" + id;
        Response.Write("<script>");
        Response.Write("window.open('" + pageurl + "','_blank')");
        Response.Write("</script>");
        Response.Write("<script>window.location.href='LD.aspx?id=1';</script>");
        
        
    }

    protected void update_Click(object sender, EventArgs e)
    {
        string url = string.Empty;

        url = "/TransOutgoing/Page/Update/LD_Product.aspx?id=" + id;

        Response.Redirect(url);
    }

    protected void reject_Click(object sender, EventArgs e)
    {
        cbl_Product.Enabled = true;
        StockOutgoingLD nH = new StockOutgoingLD();
        StockOutgoingLD_Product n = new StockOutgoingLD_Product();

        var rsH = d.StockOutgoingLD.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 1 && p.Approval == null);
        if (rsH != null)
        {
            rsH.StatusTransOut = 3; // 3 : Reject
            rsH.Approval = App.UserNama;
            rsH.ApprovalTgl = DateTime.Now;

            var rs = from p in d.StockOutgoingLD_Product where p.IDForm == id select p;

            foreach (var item in rs)
            {
                item.StockActualLD_Stone1B.Substorage = item.SubStorage; // return SubStorage Value !!!
                item.StockActualLD_Stone1B.InHand = 1;
            }

            //Commit
            d.SubmitChanges();
        }

        //Redirect
        Response.Redirect("LD.aspx?d=1");
    }

    protected string generateDocumentNumber_TransOut()
    {
        string result = "";

        // ex : KRM/13/11/0001
        //      0123456789

        string v1 = "KRM/LD";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2,2);
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2, '0');
        string v4 = "0001";

        var rs = from p in d.StockOutgoingLD
                 where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3 
                 select p;

        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
}