using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Linq.Dynamic;
using System.Web.Configuration;
using System.Globalization;

public partial class Page_Register_DJ_CharProduct01 : System.Web.UI.Page
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

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    protected int myID { get { return App.QueryInt(this); } }
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            DLC.CharGoldModel(ddl_Model);
            DLC.CharGoldLevel(ddl_ProductLevel);
            DLC.CharFrameColor(ddl_RangkaWarna);
            DLC.CharProductItem(ddl_ProductItem);
            DLC.CharTargetAge(ddl_KelompokUsia);

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJ.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.Resell.SingleOrDefault(p => p.ID == myIDRO);
        if (d == null) DataError();
        else
        {

            DataTable dtMember = new DataTable("AdditionalMember");

            DataRow drMember;
            dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nocert"), new DataColumn("id"), new DataColumn("berat") });
            var counter = 1;

            drMember = dtMember.NewRow();
            drMember["number"] = counter;
            drMember["nocert"] = "";
            drMember["id"] = "";
            drMember["berat"] = "";

            dtMember.Rows.Add(drMember);

            EmasGridView.DataSource = dtMember;
            EmasGridView.DataBind();

        }
    }

    protected string isValid()
    {
        string result = string.Empty;
        if (string.IsNullOrEmpty(plu.Text))
            result = "PLU tidak boleh kosong.";
        else if (db.StockProductPG.Any(p => p.Nomor.Equals(plu.Text)))
            result = "PLU sudah terdaftar.";
        else if (string.IsNullOrEmpty(tgl.Text))
            result = "Tanggal tidak boleh kosong.";
        else if (string.IsNullOrEmpty(harga.Text))
            result = "Harga tidak boleh kosong.";
        return result;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        string isValidate = isValid();
        if (string.IsNullOrEmpty(isValidate))
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.Nomor.Equals(plu.Text) && !p.Draft.Value);
            if (x != null)
            {
                if (x.StockActualPG.InHand == 0)
                {
                    var res = db.Resell.SingleOrDefault(p => p.ID == myID);

                    var inv = from p in db.SalesOrderPG
                              where p.IDProduct == x.ID
                              select p;
                    if (inv.Count() > 0)
                    {
                        var so = db.SalesOrder.SingleOrDefault(p => p.ID == inv.First().IDForm);

                        db.SalesOrder.DeleteOnSubmit(so);
                        db.SalesOrderPG.DeleteAllOnSubmit(inv);
                        db.SubmitChanges();


                    }
                    insertPG(x, res, true);
                    Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=2&iditem=" + x.ID + "&ro=" + myIDRO + "&so=" + myIDSO);
                }
                else
                    err_plu.InnerText = "PLU sudah terdaftar";
            }
            else
            {
                var d = db.Resell.SingleOrDefault(p => p.ID == myID);
                decimal MarginPG = 0m;
                decimal TotalHarga = 0m;
                decimal TotalHargaUSD = 0m;
                decimal TotalHargaJual = 0m;
                decimal TotalHargaJualUSD = 0m;
                decimal TotalBerat = 0m;
                decimal Harga = 0m;
                decimal HargaUSD = 0m;
                decimal OngkosM = 0m;
                decimal OngkosR = 0m;
                decimal OngkosMUSD = 0m;
                decimal OngkosRUSD = 0m;
                decimal Berat = 0m;
                decimal KadarLogam = 0m;
                decimal KadarTukaranJual = 0m;
                decimal KadarTukaranBeli = 0m;
                decimal TotalBerat24 = 0m;
                decimal TotalHarga24 = 0m;
                decimal SelisihMargin = 0m;
                decimal OngkosPGFixed = 0m;
                decimal Rate = 0m;
                decimal GoldPrice = 0m;
                int IDSupplier = 1;

                var gold = db.PricingTableTG.SingleOrDefault(p => p.ID == 3);
                var rate = db.PricingTableUS.SingleOrDefault(p => p.ID == 2);

                GoldPrice = gold.Harga.Value;
                Rate = Convert.ToDecimal(rate.Harga);

                MarginPG = 0m;

                int ProductLevel = Convert.ToInt32(ddl_ProductLevel.SelectedValue);
                int IDModel = Convert.ToInt32(ddl_Model.SelectedValue);
                int ProductItem = Convert.ToInt32(ddl_ProductItem.SelectedValue);
                int FrameColor = Convert.ToInt32(ddl_RangkaWarna.SelectedValue);
                int TargetAge = Convert.ToInt32(ddl_KelompokUsia.SelectedValue);

                KadarLogam = Convert.ToDecimal(kadar_logam.Text);
                KadarTukaranBeli = 82m;
                KadarTukaranJual = 82m;

                SelisihMargin = (KadarTukaranBeli - KadarTukaranJual > 0) ? KadarTukaranBeli - KadarTukaranJual : 0;

                DateTime Tanggal = DateTime.Today;
                DateTime TanggalTJT = DateTime.Today;
                string Supplier = "";
                string SupplierSJ = "";
                string Keterangan = (keterangan.Text != "") ? keterangan.Text : "";


                string NoCert = "";
                for (int i = 0; i < EmasGridView.Rows.Count; i++)
                {

                    NoCert = ((TextBox)EmasGridView.Rows[i].Cells[0].FindControl("txtNoCert")).Text;

                    Berat = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[1].FindControl("txtBerat")).Text);

                    TotalBerat = 1 * Berat;
                    OngkosR = ((MarginPG + SelisihMargin) / 100) * OngkosPGFixed * TotalBerat;
                    OngkosM = ((KadarTukaranBeli - KadarLogam) / 100) * GoldPrice * TotalBerat * Rate;
                    OngkosRUSD = (((MarginPG + SelisihMargin) / 100) * OngkosPGFixed * TotalBerat) / Rate;
                    OngkosMUSD = ((KadarTukaranBeli - KadarLogam) / 100) * GoldPrice * TotalBerat;
                    Harga = (((KadarLogam / 100) * GoldPrice) * Rate) * TotalBerat;
                    HargaUSD = (((KadarLogam / 100) * GoldPrice)) * TotalBerat;
                    TotalHarga = OngkosM + Harga;
                    TotalHargaJual = OngkosR + Harga;
                    TotalHargaUSD = OngkosMUSD + HargaUSD;
                    TotalHargaJualUSD = OngkosRUSD + HargaUSD;

                    TotalBerat24 = TotalBerat * (KadarTukaranBeli / 100);
                    TotalHarga24 = TotalBerat24 * GoldPrice * Rate;

                    StockProductPG n = new StockProductPG();

                    n.Nomor = plu.Text.Trim();
                    n.NomorSertifikat = NoCert;
                    n.KodeBarang = "NON JAWS";
                    n.NamaBarang = "NON JAWS";
                    n.Tgl = Tanggal;
                    n.TglTJT = TanggalTJT;
                    n.SupplierNama = Supplier;
                    n.SupplierSJ = SupplierSJ;
                    int IDBrand = 1;
                    if (d.TipeLokasi == 1)
                    {
                        var dd = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                        IDBrand = dd.IDBrand;
                    }
                    if (d.TipeLokasi == 2)
                    {
                        var dd = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                        IDBrand = dd.IDBrand;
                    }
                    n.IDBrand = IDBrand;

                    n.Keterangan = Keterangan;

                    n.ProductLevel = ProductLevel;
                    n.Model = IDModel;
                    n.ProductItem = ProductItem;
                    n.FrameColor = FrameColor;
                    n.TargetAge = TargetAge;
                    n.KadarLogam = KadarLogam;
                    n.KadarTukaranJual = KadarTukaranJual;
                    n.KadarTukaranBeli = KadarTukaranBeli;
                    n.GrossWeight = TotalBerat;
                    n.NettoWeight = TotalBerat;//TotalBerat24;
                    n.MountingM = OngkosM;
                    n.MountingR = OngkosR;
                    n.MountingMUSD = OngkosMUSD;
                    n.MountingRUSD = OngkosRUSD;
                    n.GoldRate = GoldPrice;
                    n.EfektifGold = Convert.ToDateTime(gold.ApprovalTgl);
                    n.TotalRate = Convert.ToDecimal(rate.Harga);
                    n.EfektifTotalRate = Convert.ToDateTime(rate.ApprovalTgl);
                    n.Harga = Harga;
                    n.HargaUSD = HargaUSD;
                    n.TotalHarga = TotalHarga;
                    n.TotalHargaUSD = TotalHargaUSD;
                    n.TotalHargaJual = TotalHargaJual;
                    n.TotalHargaJualUSD = TotalHargaJualUSD;
                    n.TotalHarga24 = TotalHarga24;
                    n.HumanStockNama = "NON JAWS";
                    n.ImgPicture = "";
                    n.IDSupplier = IDSupplier;
                    n.FixRate = false;
                    n.Draft = false;
                    n.Operator = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.Approval = App.UserNama;
                    n.ApprovalTgl = DateTime.Now;
                    n.Legacy = 1;
                    n.StatusPenjualan = 2;
                    n.StatusLebur = false;
                    n.ReturnProduct = false;
                    n.NoTTB = "";

                    db.StockProductPG.InsertOnSubmit(n);
                    db.SubmitChanges();

                    insertPG(n, d, false);

                    Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=2&iditem=" + n.ID + "&ro=" + myIDRO + "&so=" + myIDSO);
                }
            }
        }
        else
        {
            err_plu.InnerText = isValidate;
        }
    }
    protected void insertPG(StockProductPG d, Resell res, bool Exists)
    {
        // Ledger 
        StockLedgerPG l = new StockLedgerPG();

        // Value (See Library DB - Ledger for Value and Remark)
        decimal Value = 1; // 1 : in , -1 : out
        int Remark = 4; // See Library DB - Ledger
        string Keterangan = d.Nomor; // Commonly filled by document number or id 
        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

        string Lokasi = "";
        int IDBrand = 1;
        if (res.TipeLokasi == 0)
        {
            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocWarehouse WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
        }
        else if (res.TipeLokasi == 1)
        {
            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocOutlet WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
        }
        else if (res.TipeLokasi == 2)
        {
            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocExhibition WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
        }



        //Input
        l.IDProduct = d.ID;
        l.TipeLokasi = res.TipeLokasi;
        l.IDLokasi = res.IDLokasi;
        l.NamaLokasi = Lokasi;
        l.Value = Value;
        l.Remark = Remark;
        l.Keterangan = Keterangan;

        //User-Info
        l.Operator = App.UserNama;
        l.OperatorTgl = DateTime.Now;

        //Commit
        db.StockLedgerPG.InsertOnSubmit(l);
        db.SubmitChanges();

        if (Exists == false)
        {

            StockActualPG stock = new StockActualPG();
            stock.IDProduct = d.ID;
            stock.NamaLokasi = Lokasi;
            stock.IDBrand = IDBrand;
            stock.TipeLokasi = res.TipeLokasi;
            stock.IDLokasi = res.IDLokasi;
            stock.Substorage = 1;
            stock.Keterangan = "Barang Non JAWS";
            stock.Tgl = d.Tgl.Value;
            stock.Nomor = d.Nomor;
            stock.InHand = 1;
            stock.Operator = d.Operator;
            stock.OperatorTgl = d.OperatorTgl.Value;
            db.StockActualPG.InsertOnSubmit(stock);
            db.SubmitChanges();
        }

        SalesOrder n = new SalesOrder();

        //Location
        n.IDLokasi = res.IDLokasi;
        n.TipeLokasi = res.TipeLokasi;
        n.IDCustomer = res.IDCustomer;
        n.CustomerNama = res.CustomerNama;
        n.Tgl = Convert.ToDateTime(tgl.Text);

        //Status
        n.Draft = false;
        n.DraftDate = null;

        //User-Info
        n.Operator = App.UserNama;
        n.OperatorTgl = DateTime.Now;
        n.StatusVoid = false;
        n.StatusPembayaran = true;
        n.TotalBayar = Convert.ToDecimal(harga.Text);

        //Commit
        db.SalesOrder.InsertOnSubmit(n);
        db.SubmitChanges();

        var pr = db.SalesOrder.SingleOrDefault(p => p.ID == n.ID);
        pr.Nomor = Document.Generate_Nomor_SalesOrder(pr);
        db.SubmitChanges();

        SalesOrderPG nsodtl = new SalesOrderPG();

        nsodtl.IDForm = n.ID;
        nsodtl.IDProduct = d.ID;
        nsodtl.ModalRupiah = 0m;
        nsodtl.TotalBayar = 0m;
        nsodtl.TotalRupiah = 0m;
        nsodtl.HargaRupiah = 0m;
        nsodtl.Discount = 0m;
        nsodtl.DiscountGift = 0m;
        nsodtl.DiscountNominal = 0m;
        nsodtl.DiscountProgram = 0m;
        nsodtl.DiscountProgramNominal = 0m;
        nsodtl.DiscountPromo = 0m;
        nsodtl.DiscountRound = 0m;
        nsodtl.MEPeriodeMax = 0;
        nsodtl.MEPeriodeMin = 0;
        nsodtl.MERumus = 0m;
        nsodtl.TI1_PeriodeMin = 0;
        nsodtl.TI1_Rumus = 0;
        nsodtl.TI2_PeriodeMin = 0;
        nsodtl.TI2_Rumus = 0;
        nsodtl.StatusResell = 0;
        nsodtl.TotalBayar = Convert.ToDecimal(harga.Text);

        db.SalesOrderPG.InsertOnSubmit(nsodtl);
        db.SubmitChanges();

        var prnsodtl = db.SalesOrderPG.SingleOrDefault(p => p.ID == nsodtl.ID);
        prnsodtl.Invoice = Document.Generate_Nomor_SalesOrder_PG(prnsodtl);
        db.SubmitChanges();
    }

}