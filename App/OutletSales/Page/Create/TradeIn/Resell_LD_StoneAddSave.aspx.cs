using System;
using System.Linq;

public partial class Page_Register_DJ_StoneAddSave : System.Web.UI.Page
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
    protected int myStoneIndex { get { return App.QueryInt(this, "si"); } }
    protected int myIDStone { get { return App.QueryInt(this, "idstone"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            totalbutir.Focus();

            //Number-Format
            App.NumberFormat(totalbutir);
            App.NumberFormat(totalcarat);
            App.NumberFormat(dimensip);
            App.NumberFormat(dimensil);
            App.NumberFormat(dimensit);

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJ.aspx"); }
    protected void LinkError() { Response.Redirect("DJ_Stone.aspx?id=" + myID); }
    protected void ProductError() { Response.Redirect("DJ_CharProduct01.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.Resell.SingleOrDefault(p => p.ID == myIDRO && p.Draft);
        if (d == null) DataError();
        else
        {


            //Cancel-Link
            cancel.HRef += "?id=" + myID;

            fillStone1B();
        }
    }


    protected void fillStone1B()
    {
        var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Parcel101.Nama + " " + d.Parcel102.Nama + " " + d.Parcel103.Nama;

            //Size Control
            if (!d.Parcel102.SizeMin.HasValue || !d.Parcel102.SizeMax.HasValue) sizeError();
            else
                sizeDisplay(
                    d.Parcel102.SizeMin,
                    d.Parcel102.SizeMax
                    );

            //GIA
            //div_gia.Visible = true;
        }
    }

    //Size Control
    protected void sizeError()
    {
        save.Disabled = true;

        //Belum Setup
        sizecontrol.Attributes["class"] = "text-red";
        sizecontrol.InnerHtml = "<i class=\"icon-warning-sign\"></i> Belum Setup";
    }
    protected void sizeDisplay(decimal? SizeMin, decimal? SizeMax)
    {
        string x = string.Empty;

        x += Str.Number3(SizeMin);
        x += " - ";
        x += Str.Number3(SizeMax);

        sizecontrol.InnerHtml = x;
    }

    //Validasi Carat
    protected bool carat1B(int TotalButir, decimal TotalCarat)
    {
        var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) { LinkError(); return false; }
        else
        {
            decimal Min = d.Parcel102.SizeMin.Value;
            decimal Max = d.Parcel102.SizeMax.Value;

            decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
            return (CB >= Min && CB <= Max);
        }
    }

    //Save
    protected bool valid
    {
        get
        {
            bool x = true;

            x = x && Fv.isDecimal(totalbutir, totalbutir_ERR);
            x = x && Fv.isDecimal(totalcarat, totalcarat_ERR);
            x = x && Fv.isDecimal(dimensip, dimensip_ERR);
            x = x && Fv.isDecimal(dimensil, dimensil_ERR, true);
            x = x && Fv.isDecimal(dimensit, dimensit_ERR, true);

            //Value
            x = x && valid_TotalButir;
            x = x && valid_TotalCarat;

            //Size-Control
            x = x && valid_CaratButir;

            return x;
        }
    }
    protected bool valid_TotalButir
    {
        get
        {
            int dv = Convert.ToInt32(totalbutir.Text.Replace(".", ""));

            bool x = dv > 0;
            totalbutir_VAL.Visible = !x;

            return x;
        }
    }
    protected bool valid_TotalCarat
    {
        get
        {
            decimal dv = Convert.ToDecimal(totalcarat.Text);

            bool x = dv > 0;
            totalcarat_VAL.Visible = !x;

            return x;
        }
    }
    protected bool valid_CaratButir
    {
        get
        {
            bool x = true;

            int TotalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
            decimal TotalCarat = Convert.ToDecimal(totalcarat.Text);

            carat1B(TotalButir, TotalCarat);
            caratbutir_ERR.Visible = !x;
            return x;
        }
    }

    protected string isValid()
    {
        string result = string.Empty;
        if (string.IsNullOrEmpty(plu.Text))
            result = "PLU tidak boleh kosong.";
        else if (db.StockProductLD_Stone1B.Any(p => p.Nomor.Equals(plu.Text)))
            result = "PLU sudah terdaftar.";
        return result;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        string isValidate = isValid();
        if (string.IsNullOrEmpty(isValidate))
        {
            if (valid)
            {
                string PLU = plu.Text;

                var d = db.Resell.SingleOrDefault(p => p.ID == myID);
                if (d == null) DataError();
                else
                {
                    var rslt = db.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor.Equals(PLU) && !p.StockProductLD.Draft.Value);
                    if (rslt == null)
                    {

                        StockProductLD prod = new StockProductLD();
                        prod.Keterangan = "Non Jaws";
                        prod.Tgl = d.Tgl;
                        prod.Nomor = "";
                        prod.Supplier = 0;
                        prod.SupplierNama = "";
                        prod.SupplierNomor = "";
                        prod.RequestOutlet = 0;
                        prod.RequestOutletNama = "";
                        prod.RequestCustomer = 0;
                        prod.RequestCustomerNama = "";
                        prod.HumanStock = 0;
                        prod.HumanStockNama = "";
                        prod.Operator = d.Operator;
                        prod.OperatorTgl = d.OperatorTgl;
                        prod.Approval = d.Operator;
                        prod.ApprovalTgl = d.Tgl;
                        prod.CatatanManager = "";
                        prod.ImgPicture = "";
                        prod.ReturnProduct = false;
                        prod.StatusPenjualan = 2;
                        prod.Draft = false;

                        db.StockProductLD.InsertOnSubmit(prod);
                        db.SubmitChanges();


                        StockProductLD_Stone1B stone = new StockProductLD_Stone1B();
                        stone.Nomor = plu.Text.Trim();
                        stone.IDForm = prod.ID;
                        stone.IDStone = myIDStone;
                        stone.TotalButir = Convert.ToInt32(totalbutir.Text);
                        stone.TotalCarat = Convert.ToDecimal(totalcarat.Text);
                        stone.CaratButir = Convert.ToDecimal(totalcarat.Text) / Convert.ToInt32(totalbutir.Text);
                        stone.DimensiL = Convert.ToDecimal(dimensil.Text);
                        stone.DimensiP = Convert.ToDecimal(dimensip.Text);
                        stone.DimensiT = Convert.ToDecimal(dimensit.Text);
                        stone.HargaSatuan = 0m;
                        stone.HargaTotal = 0m;
                        stone.GIA = gia.Text;
                        stone.HargaInputH = 0m;
                        stone.HargaInputP = 0m;
                        stone.HargaSatuanM = 0m;
                        stone.HargaTotalM = 0m;
                        stone.Efektif = d.Tgl;
                        stone.Setting = 0m;
                        stone.ReturnProduct = false;
                        stone.StatusPenjualan = 2;
                        stone.ProductItem = DLC.ProductItem();
                        db.StockProductLD_Stone1B.InsertOnSubmit(stone);
                        db.SubmitChanges();

                        StockActualLD_Stone1B stock = new StockActualLD_Stone1B();
                        stock.IDProduct = stone.ID;

                        string Lokasi = "";
                        if (d.TipeLokasi.Value == 1)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                        else if (d.TipeLokasi.Value == 2)
                            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                        int IDBrand = 1;
                        if (d.TipeLokasi == 1)
                        {
                            var dd = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi.Value);
                            IDBrand = dd.IDBrand;
                        }
                        if (d.TipeLokasi == 2)
                        {
                            var dd = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi.Value);
                            IDBrand = dd.IDBrand;
                        }

                        stock.NamaLokasi = Lokasi;
                        stock.IDBrand = IDBrand;
                        stock.TipeLokasi = d.TipeLokasi.Value;
                        stock.IDLokasi = d.IDLokasi.Value;
                        stock.Substorage = 1;
                        stock.Keterangan = "Barang NON Jaws";
                        stock.Tgl = d.Tgl.HasValue ? d.Tgl.Value : DateTime.Today;
                        stock.Nomor = plu.Text.Trim();
                        stock.InHand = 1;
                        stock.Operator = d.Operator;
                        stock.OperatorTgl = d.OperatorTgl.Value;
                        db.StockActualLD_Stone1B.InsertOnSubmit(stock);
                        db.SubmitChanges();

                        // Ledger 
                        StockLedgerLD l = new StockLedgerLD();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = 1; // 1 : in , -1 : out
                        int Remark = 11; // See Library DB - Ledger
                        string Keterangan = plu.Text; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        //Input
                        l.IDProduct = stone.ID;
                        l.TipeLokasi = d.TipeLokasi.Value;
                        l.IDLokasi = d.IDLokasi.Value;
                        l.NamaLokasi = Lokasi;
                        l.Value = Value;
                        l.Remark = Remark;
                        l.Keterangan = Keterangan;

                        //User-Info
                        l.Operator = App.UserNama;
                        l.OperatorTgl = DateTime.Now;

                        //Commit
                        db.StockLedgerLD.InsertOnSubmit(l);
                        db.SubmitChanges();

                        SalesOrder n = new SalesOrder();

                        //Location
                        n.IDLokasi = d.IDLokasi;
                        n.TipeLokasi = d.TipeLokasi;
                        n.IDCustomer = d.IDCustomer;
                        n.CustomerNama = d.CustomerNama;
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

                        SalesOrderLD nsodtl = new SalesOrderLD();

                        nsodtl.IDForm = n.ID;
                        nsodtl.IDProduct = stone.ID;
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

                        db.SalesOrderLD.InsertOnSubmit(nsodtl);
                        db.SubmitChanges();

                        var prnsodtl = db.SalesOrderLD.SingleOrDefault(p => p.ID == nsodtl.ID);
                        prnsodtl.Invoice = Document.Generate_Nomor_SalesOrder_LD(prnsodtl);
                        db.SubmitChanges();

                        //Redirect
                        Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=4&iditem=" + stone.ID + "&ro=" + myIDRO + "&so=" + myIDSO);
                    }
                    else
                        err_plu.InnerText = "PLU sudah terdaftar";
                }
            }
        }
        else
        {
            err_plu.InnerText = isValidate;
        }
    }
}