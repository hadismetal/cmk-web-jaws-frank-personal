using System;
using System.Linq;
using System.Web.UI.WebControls;

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

            //Form-Default
            Fill();
            Fill1();
        }
    }
    //Fill
    protected void Fill()
    {
        
        var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingPackaging.Nomor;
        brand.InnerText = rsH.StockOutgoingPackaging.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingPackaging.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingPackaging.TipeTujuan);
        from.InnerText = rsH.StockOutgoingPackaging.NamaAsal;
        to.InnerText = rsH.StockOutgoingPackaging.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.StockOutgoingPackaging.Tgl);
        kurir.InnerText = rsH.StockOutgoingPackaging.NamaKurir;
        keterangan.InnerText = rsH.StockOutgoingPackaging.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingPackaging.TglETA);

        var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;
    }
    protected void Fill1()
    {
        var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;
        //item = item.Where(p => p.SubStorageResult == p.SubStorageTo);

        if (item.Count() > 0)
        {
            foreach (var rs in item)
            {
                string v = rs.Packaging.ID.ToString();
                string t = Logistic.ProductList_Packaging(
                        rs.Packaging.Kode,
                        
                        Str.Number0(rs.Qty != null ? rs.Qty.Value : 0),
                        rs.Packaging.ID.ToString()
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
            }
        }
    }

    // approve : rubah status 
    protected void approve_Click(object sender, EventArgs e)
    {
        StockIncomingPackaging nH = new StockIncomingPackaging();
        StockIncomingPackaging_Product n = new StockIncomingPackaging_Product();

        string DocumentNumber = generateDocumentNumber_TransIn();

        var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 6);
        rsH.StatusTransIn = 7;
        rsH.Nomor = DocumentNumber;
        rsH.Approval = App.UserNama;
        rsH.ApprovalTgl = DateTime.Now;

        /// PROSES LANJUTAN APPROVE :
        /// 1. Rubah Header Outgoing dari Approved menjadi Delivered (2 -> 4)
        /// 2. Rubah Stock Actual ke Tipe Location Tujuan (W,O,E)
        /// 3. Rubah Stock Actual ke ID Location Tujuan (1,2,3,...)
        /// 4. Rubah Stock Actual ke arah SubStorage Lanjutan (1,2,3,...)
        
        // 1
        var rsHout = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == rsH.IDOutgoing && p.StatusTransOut == 7); // 2 : Status Out Approved / On The Way
        if(rsHout!=null)
            rsHout.StatusTransOut = 9; // 9 : Delivered
        
        // 2,3,4
        var rs = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;

        foreach (var item in rs)
        {
            var x = d.StockActualPackaging.SingleOrDefault(p => p.IDProduct == item.IDProduct && p.TipeLokasi == item.StockIncomingPackaging.StockOutgoingPackaging.TipeAsal && p.IDLokasi == item.StockIncomingPackaging.StockOutgoingPackaging.IDAsal);
            if (x != null)
            {
                
                x.Damaged = x.Damaged - item.Qty;
                //x.InHand = x.InHand + item.NeverArrived.Value;
                db.SubmitChanges();
            }

            var z = d.StockActualPackaging.SingleOrDefault(p => p.IDProduct == item.IDProduct && p.TipeLokasi == item.StockIncomingPackaging.StockOutgoingPackaging.TipeTujuan && p.IDLokasi == item.StockIncomingPackaging.StockOutgoingPackaging.IDTujuan);
            if (z != null)
            {
                
                
                z.Damaged = z.Damaged + item.Qty;
                db.SubmitChanges();
            }
            else
            {
                StockActualPackaging stock = new StockActualPackaging();
                stock.IDProduct = item.IDProduct;
                //stock.NamaLokasi = x.P;
                //stock.IDBrand = d.LocWarehouse.CompanyBrand.ID;
                stock.TipeLokasi = item.StockIncomingPackaging.StockOutgoingPackaging.TipeTujuan ;
                stock.IDLokasi = item.StockIncomingPackaging.StockOutgoingPackaging.IDTujuan;
                stock.Substorage = 1;
                stock.Keterangan = (item.StockIncomingPackaging.Keterangan!=null) ? item.StockIncomingPackaging.Keterangan : "";
                stock.Tgl = item.StockIncomingPackaging.Tgl.Value;
                stock.Nomor = item.Packaging.Kode;
                stock.InHand = 0m;
                //z.Receive = q.Receive - (Convert.ToDecimal(rcv.Text) + Convert.ToDecimal(dmg.Text) + Convert.ToDecimal(lst.Text));
                stock.Damaged = item.Qty.Value;
                stock.NeverArrived = 0;
                stock.Operator = item.StockIncomingPackaging.Operator;
                stock.OperatorTgl = item.StockIncomingPackaging.OperatorTgl;
                d.StockActualPackaging.InsertOnSubmit(stock);
                d.SubmitChanges();
            }
            
            item.Status = 2;
            item.Approval = App.UserNama;
            item.ApprovalTgl = DateTime.Now;
        }


        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("ProductBroken.aspx?d=1");
    }

    //protected void reject_Click(object sender, EventArgs e)
    //{
    //    StockIncomingPackaging nH = new StockIncomingPackaging();
    //    StockIncomingPackaging_Product n = new StockIncomingPackaging_Product();

    //    var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 6);
    //    rsH.StatusTransIn = 8;
    //    rsH.Approval = App.UserNama;
    //    rsH.ApprovalTgl = DateTime.Now;

    //    var rs = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;

    //    foreach (var item in rs)
    //    {
    //        item.Status = 3;
    //        item.Approval = App.UserNama;
    //        item.ApprovalTgl = DateTime.Now;
    //    }

    //    //Commit
    //    d.SubmitChanges();

    //    //Redirect
    //    Response.Redirect("ProductBroken.aspx?d=1");
    //}

    protected string generateDocumentNumber_TransIn()
    {
        string result = "";

        // ex : TRM/13/11/0001
        //      0123456789

        string v1 = "TRM/PB";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2, 2);
        string v3 = System.DateTime.Now.Month.ToString("d2").PadLeft(2,'0');
        string v4 = "0001";

        var rs = from p in d.StockIncomingPackaging
                 where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3
                 select p;

        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
}