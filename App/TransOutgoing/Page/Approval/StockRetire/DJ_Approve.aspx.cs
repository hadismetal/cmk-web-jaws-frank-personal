using System;
using System.Linq;
using System.Web.UI.WebControls;

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
            Fill();
            
        }
    }
    //Fill
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

    protected void save_Click(object sender, EventArgs e)
    {
        cbl_Product.Enabled = true;
        StockOutgoingDJ nH = new StockOutgoingDJ();
        StockOutgoingDJ_Product n = new StockOutgoingDJ_Product();

        string DocumentNumber = generateDocumentNumber_TransOut();

        var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 5);
        rsH.StatusTransOut = 6; // 6 : Approved
        rsH.Nomor = DocumentNumber;
        rsH.Approval = App.UserNama;
        rsH.ApprovalTgl = DateTime.Now;

        var rs = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

        foreach (var item in rs)
        {
            item.StockActualDJ.Substorage = 95; // Product On The Way !!
            item.StockActualDJ.InHand = 0; // InHand out

            // Ledger 
            StockLedgerDJ l = new StockLedgerDJ();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = -1; // 1 : in , -1 : out
            int Remark = 3; // See Library DB - Ledger
            string Keterangan = DocumentNumber; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

            //Input
            l.IDProduct = item.IDProduct;
            l.TipeLokasi = item.StockOutgoingDJ.TipeAsal;
            l.IDLokasi = item.StockOutgoingDJ.IDAsal;
            l.NamaLokasi = item.StockOutgoingDJ.NamaAsal;
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

        //Redirect
        Response.Redirect("DJ.aspx?d=1");
        
    }

    /*
    protected void reject_Click(object sender, EventArgs e)
    {
        cbl_Product.Enabled = true;
        StockOutgoingDJ nH = new StockOutgoingDJ();
        StockOutgoingDJ_Product n = new StockOutgoingDJ_Product();

        var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 5);
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

        //Redirect
        Response.Redirect("DJ.aspx?d=1");
    }
    */

    protected string generateDocumentNumber_TransOut()
    {
        string result = "";

        // ex : KRM/13/11/0001
        //      0123456789

        string v1 = "KRM/DJ";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2,2);
        string v3 = System.DateTime.Now.Month.ToString().PadLeft(2, '0');
        string v4 = "0001";

        var rs = from p in d.StockOutgoingDJ
                 where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3 
                 select p;

        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
}