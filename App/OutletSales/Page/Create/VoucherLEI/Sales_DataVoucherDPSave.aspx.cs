using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Sales_Sales_DataVoucherDPSave : System.Web.UI.Page
{
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
    protected string PLU { get { return Request.QueryString["PLU"]; } }
    protected string VoucherCode { get { return Request.QueryString["vc"]; } }
    protected int myItemType { get { return App.QueryInt(this, "itemtype"); } }
    protected int myItemID { get { return App.QueryInt(this, "itemID"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

        }

        Save(myID, PLU, VoucherCode);
    }
    protected void Save(int IDSalesOrder, string plunumber, string vc)
    {
        if (IDSalesOrder != 0 && !string.IsNullOrEmpty(plunumber) && !string.IsNullOrEmpty(vc))
        {
            var idproductdj = db.StockProductDJ.Where(p => p.Nomor == plunumber).Select(p => p.ID);
            var idproductpg = db.StockProductPG.Where(p => p.Nomor == plunumber).Select(p => p.ID);
            var idproductgj = db.StockProductGJ.Where(p => p.Nomor == plunumber).Select(p => p.ID);
            var idproductld = db.StockProductLD_Stone1B.Where(p => p.Nomor == plunumber).Select(p => p.StockProductLD.ID);
            var idproduct = (idproductdj).Union(idproductgj).Union(idproductld).Union(idproductpg);
            if (idproduct.Count() == 1)
            {
                if (vc == "-")
                {

                }
                else
                {
                    var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
                    string xNoSalesOrder = d.Nomor; // no sales order masih kosong karena belum terbentuk ketika voucher discount diinput
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

                    var x = cs.T_voucher.SingleOrDefault(p => p.M_nomor == VoucherCode);
                    var y = cs.Tb_type_voucher.SingleOrDefault(p => p.Ckodetype == x.M_type);
                    if (x != null)
                    {
                        int zIDSalesOrder = myID;
                        string zIDTypeVoucher = x.M_type;
                        string zTypeVoucher = y.Cnamatype;
                        string zNomorVoucher = VoucherCode;
                        string zKeterangan = x.M_keterangan;
                        decimal zNominal = x.M_value;
                        string zIssuer = x.M_issuer;
                        DateTime zDateStartVoucher = Convert.ToDateTime(x.M_tanggal.ToString());

                        SalesVoucher s = new SalesVoucher();

                        s.IDSalesOrder = zIDSalesOrder;
                        s.IDTypeVoucher = zIDTypeVoucher;
                        s.TypeVoucher = zTypeVoucher;
                        s.NomorVoucher = zNomorVoucher;
                        s.Keterangan = zKeterangan;
                        s.Issuer = zIssuer;
                        s.Operator = App.UserNama;
                        s.OperatorTgl = DateTime.Now;
                        s.ProductType = myItemType;

                        decimal dDoubleReward = 1m;

                        if (myItemType == 1)
                        {
                            string xCategory = "";
                            bool xDossier03CaratUpExists = false;

                            var m = db.SalesOrderDJ.SingleOrDefault(p => p.ID == myItemID);

                            xCategory = m.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
                            xDossier03CaratUpExists = m.StockProductDJ.StockProductDJ_Stone1B.Any(p => p.CaratButir >= 0.3m);

                            // hardcode promo nasional 2019 Februari : double reward membership redemption
                            if (System.DateTime.Now >= Convert.ToDateTime("2019-02-01 00:00:00.000")
                                && System.DateTime.Now < Convert.ToDateTime("2019-04-01 00:00:00.000")
                                && zTypeVoucher == "MEMBERSHIP REDEMPTION"
                                && zIDTypeVoucher == "07"
                                && zDateStartVoucher >= Convert.ToDateTime("2019-02-01 00:00:00.000")
                                && zDateStartVoucher < Convert.ToDateTime("2019-04-01 00:00:00.000")
                                && !(xDossier03CaratUpExists && xCategory == "SNI")
                                && !(xCategory == "SI")
                                )
                            {
                                dDoubleReward = 2m;
                            }

                            zNominal = zNominal * dDoubleReward;

                            m.DiscountGift += zNominal;
                            db.SubmitChanges();

                            s.Nominal = zNominal;
                            s.ProductID = m.IDProduct;
                        }
                        else if (myItemType == 2)
                        {
                            zNominal = zNominal * dDoubleReward;

                            var m = db.SalesOrderPG.SingleOrDefault(p => p.ID == myItemID);
                            m.DiscountGift += zNominal;
                            db.SubmitChanges();

                            s.Nominal = zNominal;
                            s.ProductID = m.IDProduct;
                        }
                        else if (myItemType == 3)
                        {
                            zNominal = zNominal * dDoubleReward;

                            var m = db.SalesOrderGJ.SingleOrDefault(p => p.ID == myItemID);
                            m.DiscountGift += zNominal;
                            db.SubmitChanges();

                            s.Nominal = zNominal;
                            s.ProductID = m.IDProduct;
                        }
                        else if (myItemType == 4)
                        {
                            zNominal = zNominal * dDoubleReward;

                            var m = db.SalesOrderLD.SingleOrDefault(p => p.ID == myItemID);
                            m.DiscountGift += zNominal;
                            db.SubmitChanges();

                            s.Nominal = zNominal;
                            s.ProductID = m.IDProduct;
                        }

                        db.SalesVoucher.InsertOnSubmit(s);
                        db.SubmitChanges();

                        // update t_voucher
                        x.M_klaimdoc = xNoSalesOrder;
                        x.M_tanggal2 = DateTime.Now;
                        x.M_store2 = xStore;

                        cs.SubmitChanges();

                    }

                    Response.Redirect("Sales_Data.aspx?id=" + myID);
                }
            }
        }
        Response.Redirect("Sales_Data.aspx?id="+myID+"");
    }
}