using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAddSave : System.Web.UI.Page {
	private Map_DB _db;

	private Map_DB db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_DB();
	        return _db;
	    }
	}

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (cs != null)
	        cs.Dispose();
	    base.Dispose();
	}
    App gp = new App();
	protected int myID { get { return App.QueryInt(this); } }
	protected string myIDItem { get { return Request.QueryString["iditem"]; } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}

        Save();
	}

	//Exception
	protected void DataError() { Response.Redirect("Payment.aspx"); }
    protected void DataInvalidResponseWrite(string err) { Response.Write(err); }

    protected void Save()
    {
        string errMsg = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {

            // redeem stamps
            /*try
            {
                int id_tipelokasi = d.TipeLokasi.Value;
                int id_lokasi = d.IDLokasi.Value;
                int id_brand = 0;
                string kode_brand = "";

                if (id_tipelokasi == 1)
                {
                    var loc = db.LocOutlet.Single(p => p.ID == id_lokasi);

                    id_brand = loc.IDBrand;
                    kode_brand = loc.CompanyBrand.NamaKode;
                }
                else if (id_tipelokasi == 2)
                {
                    var loc = db.LocExhibition.Single(p => p.ID == id_lokasi);

                    id_brand = loc.IDBrand;
                    kode_brand = loc.CompanyBrand.NamaKode;
                }

                //errmsg = "";


                if (STAMPS.xActivate)
                {
                    string plu = "";

                    errMsg += ((errMsg == "") ? STAMPSM.RedeemVoucherCode(d,
                        myIDItem,
                        kode_brand,
                        plu
                        ) : "");
                }
            }
            catch (Exception) { }*/
			
			if (IsUsed(d))
            {
                errMsg = "Kode Voucher Sudah Digunakan di SO Lain.";
            }

            if(errMsg != "")
            {
                DataInvalidResponseWrite(errMsg);
            }
            else
            {
                var x = cs.T_voucher.SingleOrDefault(p => p.M_nomor == myIDItem);
                if (x != null)
                {
                    var x2 = cs.Tb_type_voucher.Single(p => p.Ckodetype == x.M_type);
                    decimal TotalBayar = x.M_value;
                    SalesReceipt s = new SalesReceipt();

                    s.IDSalesOrder = myID;
                    s.NoSalesOrder = d.Nomor;
                    s.OperatorNama = App.UserNama;
                    s.OperatorTgl = DateTime.Now;
                    s.TotalBayar = x.M_value;

                    db.SalesReceipt.InsertOnSubmit(s);
                    db.SubmitChanges();

                    int IDSalesReceipt = s.ID;

                    SalesReceiptDetail sd = new SalesReceiptDetail();

                    sd.IDSalesReceipt = IDSalesReceipt;

                    sd.IDPaymentType = 5;
                    sd.CCNumber = x.M_nomor;
                    int lengthpocer = x2.Cnamatype.Length;
					if(lengthpocer > 50)
					{
						sd.CCName = x.M_nomor;
					}
					else
					{
						sd.CCName = x2.Cnamatype;
					}
                    sd.Nominal = x.M_value;
                    sd.MDR = 0;
                    sd.OperatorNama = App.UserNama;
                    sd.OperatorTgl = DateTime.Now;

                    db.SalesReceiptDetail.InsertOnSubmit(sd);
                    db.SubmitChanges();

                    // sales order information update
                    var z = db.SalesReceipt.Where(p => p.IDSalesOrder == d.ID).Sum(p => p.TotalBayar);
                    d.Paid = z;
                    d.Unpaid = d.TotalBayar - z;
                    db.SubmitChanges();

                    var y = db.SalesReceipt.SingleOrDefault(p => p.ID == IDSalesReceipt);

                    string NoInvoice = Document.Generate_Nomor_Invoice(d);
                    string NoDP = Document.Generate_Nomor_DownPayment(d);

                    if (d.Unpaid <= 0m)
                    {
                        //d.TglLunas = System.DateTime.Now;
                        //d.StatusPembayaran = true;
                        y.TotalBayar = TotalBayar;
                        y.NomorInvoice = NoInvoice;

                        decimal SisaDP = TotalBayar;

                        var dj = d.SalesOrderDJ;
                        foreach (var r in dj)
                        {
                            if (d.TradeIn == true)
                                r.StockProductDJ.StockActualDJ.Substorage = 3;
                            r.StockProductDJ.StatusPenjualan = 2;

                            decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                            if (SisaDP > 0)
                            {
                                if (TotalDP < r.TotalBayar)
                                {
                                    if (SisaDP > r.TotalBayar)
                                    {
                                        SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                        r.TotalDP = r.TotalBayar.Value;
                                    }
                                    else
                                    {
                                        r.TotalDP = TotalDP + SisaDP;
                                        SisaDP = 0m;
                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }
                        var pg = d.SalesOrderPG;
                        foreach (var r in pg)
                        {
                            if (d.TradeIn == true)
                                r.StockProductPG.StockActualPG.Substorage = 3;
                            r.StockProductPG.StatusPenjualan = 2;

                            decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                            if (SisaDP > 0)
                            {
                                if (TotalDP < r.TotalBayar)
                                {
                                    if (SisaDP > r.TotalBayar)
                                    {
                                        SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                        r.TotalDP = r.TotalBayar.Value;
                                    }
                                    else
                                    {
                                        r.TotalDP = TotalDP + SisaDP;
                                        SisaDP = 0m;
                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }
                        var gj = d.SalesOrderGJ;
                        foreach (var r in gj)
                        {
                            if (d.TradeIn == true)
                                r.StockProductGJ.StockActualGJ.Substorage = 3;
                            r.StockProductGJ.StatusPenjualan = 2;

                            decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                            if (SisaDP > 0)
                            {
                                if (TotalDP < r.TotalBayar)
                                {
                                    if (SisaDP > r.TotalBayar)
                                    {
                                        SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                        r.TotalDP = r.TotalBayar.Value;
                                    }
                                    else
                                    {
                                        r.TotalDP = TotalDP + SisaDP;
                                        SisaDP = 0m;
                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }
                        var ld = d.SalesOrderLD;
                        foreach (var r in ld)
                        {
                            if (d.TradeIn == true)
                                r.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 3;
                            r.StockProductLD_Stone1B.StatusPenjualan = 2;

                            decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                            if (SisaDP > 0)
                            {
                                if (TotalDP < r.TotalBayar)
                                {
                                    if (SisaDP > r.TotalBayar)
                                    {
                                        SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                        r.TotalDP = r.TotalBayar.Value;
                                    }
                                    else
                                    {
                                        r.TotalDP = TotalDP + SisaDP;
                                        SisaDP = 0m;
                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }
                        //Commit
                        db.SubmitChanges();
                    }
                    else
                    {
                        x.M_value = TotalBayar;
                        //x.NomorDP = NoDP;

                        decimal SisaDP = TotalBayar;

                        var dj = d.SalesOrderDJ;
                        foreach (var r in dj)
                        {
                            r.StockProductDJ.StatusPenjualan = 3;

                            decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                            if (SisaDP > 0)
                            {
                                if (TotalDP < r.TotalBayar)
                                {
                                    if (SisaDP > r.TotalBayar)
                                    {
                                        SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                        r.TotalDP = r.TotalBayar.Value;
                                    }
                                    else
                                    {
                                        r.TotalDP = TotalDP + SisaDP;
                                        SisaDP = 0m;
                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }
                        var pg = d.SalesOrderPG;
                        foreach (var r in pg)
                        {
                            r.StockProductPG.StatusPenjualan = 3;

                            decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                            if (SisaDP > 0)
                            {
                                if (TotalDP < r.TotalBayar)
                                {
                                    if (SisaDP > r.TotalBayar)
                                    {
                                        SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                        r.TotalDP = r.TotalBayar.Value;
                                    }
                                    else
                                    {
                                        r.TotalDP = TotalDP + SisaDP;
                                        SisaDP = 0m;
                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }

                        var gj = d.SalesOrderGJ;
                        foreach (var r in gj)
                        {
                            r.StockProductGJ.StatusPenjualan = 3;

                            decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                            if (SisaDP > 0)
                            {
                                if (TotalDP < r.TotalBayar)
                                {
                                    if (SisaDP > r.TotalBayar)
                                    {
                                        SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                        r.TotalDP = r.TotalBayar.Value;
                                    }
                                    else
                                    {
                                        r.TotalDP = TotalDP + SisaDP;
                                        SisaDP = 0m;
                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }

                        var ld = d.SalesOrderLD;
                        foreach (var r in ld)
                        {
                            r.StockProductLD_Stone1B.StatusPenjualan = 3;

                            decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                            if (SisaDP > 0)
                            {
                                if (TotalDP < r.TotalBayar)
                                {
                                    if (SisaDP > r.TotalBayar)
                                    {
                                        SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                        r.TotalDP = r.TotalBayar.Value;
                                    }
                                    else
                                    {
                                        r.TotalDP = TotalDP + SisaDP;
                                        SisaDP = 0m;
                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }
                        //Commit
                        db.SubmitChanges();

                    }

                    string xNoSalesOrder = d.Nomor;
                    string xNoCustomer = d.DataCustomer.NoCustomer;
                    string xNamaCustomer = d.DataCustomer.Nama;
                    string xStore = "";
                    if (d.TipeLokasi == 1)
                    {
                        xStore = Db.SingleString("SELECT Kode FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                    }
                    else if (d.TipeLokasi == 2)
                    {
                        xStore = Db.SingleString("SELECT Kode FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                    }

                    // update t_voucher
                    x.M_klaimdoc = xNoSalesOrder;
                    x.M_tanggal2 = DateTime.Now;
                    x.M_store2 = xStore;

                    cs.SubmitChanges();
                    db.SubmitChanges();
                }

                Response.Redirect("Payment_ItemList.aspx?id=" + myID + "#divrowpayment");
            }

        }	
	}
	
	protected bool IsUsed(SalesOrder d)
    {
        bool isused = false;
        isused = d.SalesReceipt.Any(p => p.SalesReceiptDetail.Any(q => q.IDPaymentType == 5 && q.CCNumber == myIDItem));
        if (!isused)
        {
            isused = db.SalesReceipt.Any(p => p.SalesReceiptDetail.Any(q => q.IDPaymentType == 5 
			&& q.CCNumber == myIDItem 
			&& (q.SalesReceipt.SalesOrder.StatusPembayaran == true || q.SalesReceipt.SalesOrder.StatusPembayaran == false)
			&& q.SalesReceipt.SalesOrder.StatusVoid == false
			&& q.SalesReceipt.SalesOrder.Draft == false));
        }

        return isused;
    }
}