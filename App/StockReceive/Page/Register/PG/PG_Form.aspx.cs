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

public partial class Page_Register_PG_Form : System.Web.UI.Page
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
    App price = new App();

    decimal GoldPrice = 0m;
    static int i = 1;

    protected int myIDWarehouse { get { return App.QueryInt(this, "IDWarehouse"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //keterangan.Focus();

            DLC.CharGoldModel(ddl_Model);
            DLC.CharGoldLevel(ddl_ProductLevel);
            DLC.CharFrameColor(ddl_RangkaWarna);
            DLC.CharProductItem(ddl_ProductItem);
            DLC.CharTargetAge(ddl_KelompokUsia);
            DLC.DDLSupplier(ddlsupplier);

            GoldPrice = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableTG WHERE ID=41", Db.Cnn_DB);
            goldrate.Text = GoldPrice.ToString("N2");
            //goldrate.Enabled = false;

            //Form
            fillForm();

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tgltjt.Text = Str.Tgl(DateTime.Today);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("PG.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.LocWarehouse.SingleOrDefault(p => p.ID == myIDWarehouse && !p.Disable);
        if (d == null) DataError();
        else
            warehouse.InnerHtml = d.Nama;
    }

    //Save
    protected bool validTgl
    {
        get { return Fv.isDateTime(tgl, tgl_ERR); }
    }
    protected bool validTglTJT
    {
        get { return Fv.isDateTime(tgltjt, tgltjt_ERR); }
    }

    protected void LoadModel(object sender, EventArgs e)
    {
        ((RadioButtonList)sender).Focus();
        if (ratetype.SelectedValue == "0")
        {
            DLC.CharGoldModel(ddl_Model);
            ((DataControlField)EmasGridView.Columns
            .Cast<DataControlField>()
            .Where(fld => fld.HeaderText == "Total Harga Jual")
            .SingleOrDefault()).Visible = false;
        }
        else
        {
            DLC.CharGoldModelGJ(ddl_Model);
            ((DataControlField)EmasGridView.Columns
            .Cast<DataControlField>()
            .Where(fld => fld.HeaderText == "Total Harga Jual")
            .SingleOrDefault()).Visible = true;
        }

        CalcHarga();
    }

    protected void save_Click(object sender, EventArgs e) {
        if (validTgl && validTglTJT)
        {
            idsk.Text = idskhidden.Value;

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
            decimal Qty = 0m;
            decimal KadarLogam = 0m;
            decimal KadarTukaranJual = 0m;
            decimal KadarTukaranBeli = 0m;
            decimal TotalBerat24 = 0m;
            decimal TotalHarga24 = 0m;
            decimal SelisihMargin = 0m;
            //decimal OngkosPGFixed = 565000m;
            decimal OngkosPGFixed = 0m;
            decimal Rate = 0m;
            decimal DimensiR = 0m;
            
            try{
                DimensiR = Convert.ToDecimal(dimensir.Text);    
            }
            catch{}

            var gold = db.PricingTableTG.SingleOrDefault(p => p.ID == 41);
            var rate = db.PricingTableUS.SingleOrDefault(p => p.ID == 2);

            GoldPrice = Convert.ToDecimal(goldrate.Text);
            Rate = Convert.ToDecimal(rate.Harga);

            MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + Convert.ToInt32(ddl_Model.SelectedValue), Db.Cnn_DB);

            int IDWarehouse = myIDWarehouse;
            int ProductLevel = Convert.ToInt32(ddl_ProductLevel.SelectedValue);
            int IDModel = Convert.ToInt32(ddl_Model.SelectedValue);
            int ProductItem = Convert.ToInt32(ddl_ProductItem.SelectedValue);
            int FrameColor = Convert.ToInt32(ddl_RangkaWarna.SelectedValue);
            int TargetAge = Convert.ToInt32(ddl_KelompokUsia.SelectedValue);

            KadarLogam = Convert.ToDecimal(kadar_logam.Text);
            KadarTukaranBeli = Convert.ToDecimal(kadar_tukaran.Text);
            KadarTukaranJual = Convert.ToDecimal(kadar_jual.Text);

            SelisihMargin = (KadarTukaranBeli - KadarTukaranJual > 0) ? KadarTukaranBeli - KadarTukaranJual : 0;

            DateTime Tanggal = Convert.ToDateTime(tgl.Text);
            DateTime TanggalTJT = Convert.ToDateTime(tgltjt.Text);
            string Supplier = (supplier.Text != "") ? supplier.Text : "";
            string SupplierSJ = (sjsupplier.Text != "") ? sjsupplier.Text : "";
            string Keterangan = (keterangan.Text != "") ? keterangan.Text : "";

            int LastID = 1;

            try
            {
                LastID = Db.SingleInteger("SELECT COUNT(*) FROM StockReceivePG WHERE SUBSTRING(NoTTB,5,2) = CONVERT(VARCHAR(2), GETDATE(), 2) AND SUBSTRING(NoTTB,8,2) = CONVERT(VARCHAR(2), GETDATE(), 1)", Db.Cnn_DB);
                LastID = LastID + 1;
            }
            catch (Exception ex)
            {
                LastID = 1;
            }

            string Bulan = DateTime.Now.ToString("MM");
            string Tahun = DateTime.Now.ToString("yy");

            string KodeFormat = Str.KodeFormat(LastID, 4);

            string NoCert = "";

            for (i = 0; i < EmasGridView.Rows.Count; i++)
            {

                NoCert = ((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtNoCert")).Text;
                Qty = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtQty")).Text);
                Berat = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtBerat")).Text);
                TotalBerat24 = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtTotalBerat24")).Text);

                for (int j = 1; j <= Convert.ToInt32(Qty); j++)
                {
                    int LastPLUID = 1;

                    try
                    {
                        //LastPLUID = Db.SingleInteger("SELECT COUNT(*) FROM StockReceivePG WHERE SUBSTRING(Nomor,2,2) = CONVERT(VARCHAR(2), GETDATE(), 2) AND SUBSTRING(Nomor,4,2) = CONVERT(VARCHAR(2), GETDATE(), 1)", Db.Cnn_DB);
                        LastPLUID = Db.SingleInteger("SELECT COUNT(*) FROM StockReceivePG WHERE SUBSTRING(Nomor,2,2) = CONVERT(VARCHAR(2), GETDATE(), 2) ", Db.Cnn_DB);
                        LastPLUID = LastPLUID + 1;
                    }
                    catch (Exception ex)
                    {
                        LastPLUID = 1;
                    }

                    string KodeFormatPLU = Str.KodeFormat(LastPLUID, 6);

                    if (ratetype.SelectedValue == "0")
                    {
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

                    }
                    else
                    {
                        TotalBerat = 1 * Berat;
                        OngkosR = 0m;
                        OngkosM = 0m;
                        OngkosRUSD = 0m;
                        OngkosMUSD = 0m;
                        //Harga = GoldPrice * Rate * TotalBerat;
                        //Harga = price.RoundTop(GoldPrice * Rate * TotalBerat);
                        Harga = GoldPrice * Rate * TotalBerat;
                        HargaUSD = GoldPrice * TotalBerat;
                        TotalHarga = OngkosM + Harga;
                        TotalHargaJual = price.RoundTop(Lib.CalcGJJual(Convert.ToInt32(ddlsupplier.SelectedValue), Convert.ToInt32(ddl_Model.SelectedValue), Berat, false));
                        TotalHargaUSD = OngkosMUSD + HargaUSD;
                        TotalHargaJualUSD = Lib.CalcGJJual(Convert.ToInt32(ddlsupplier.SelectedValue), Convert.ToInt32(ddl_Model.SelectedValue), Berat, true);

                        TotalBerat24 = TotalBerat * (KadarTukaranBeli / 100);
                        TotalHarga24 = TotalBerat24 * GoldPrice * Rate;

                    }

                    string path = "";

                    path += App.ImagesDir;
                    path += "StockReceivePG" + "\\";

                    StockReceivePG n = new StockReceivePG();

                    n.NomorSertifikat = NoCert;
                    n.KodeBarang = (KodeBarang.Text != "") ? KodeBarang.Text : "";
                    n.NamaBarang = (NamaBarang.Text != "") ? NamaBarang.Text : "";
                    n.Tgl = Tanggal;
                    n.TglTJT = TanggalTJT;
                    n.SupplierNama = Supplier;
                    n.SupplierSJ = SupplierSJ;
                    n.IDWarehouse = myIDWarehouse;
                    n.NoTTB = "TTB/" + Tahun + "/" + Bulan + "/" + KodeFormat;
                    //n.Nomor = "P" + Tahun + KodeFormatPLU;
                    n.Nomor = Generate_Nomor(n);
                    n.Keterangan = Keterangan;
                    n.Draft = false;
                    n.DraftDate = DateTime.Now;
                    n.Operator = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.ProductLevel = ProductLevel;
                    n.Model = IDModel;
                    n.ProductItem = ProductItem;
                    n.FrameColor = FrameColor;
                    n.TargetAge = TargetAge;
                    n.KadarLogam = KadarLogam;
                    n.KadarTukaranJual = KadarTukaranJual;
                    n.KadarTukaranBeli = KadarTukaranBeli;
                    n.GrossWeight = TotalBerat;
                    n.NettoWeight = TotalBerat; // revisi berat netto -> harus sama dengan gross
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
                    n.HumanStockNama = stockkeeper.Text;
                    n.ImgPicture = path + KodeBarang.Text + ".jpg";
                    n.IDSupplier = Convert.ToInt32(ddlsupplier.SelectedValue);
                    n.FixRate = ratetype.SelectedValue == "0" ? false : true;
                    n.HumanStock = (idsk.Text!="") ? Convert.ToInt32(idsk.Text) : 0;
                    n.DimensiR = DimensiR;

                    db.StockReceivePG.InsertOnSubmit(n);
                    db.SubmitChanges();

                    Db.Execute("UPDATE StockReceivePG SET NomorValidasi = '" + NomorValidasi.Text + "', KodeMaterial = '" + KodeMaterial.Text + "' WHERE ID = " + n.ID.ToString(), Db.Cnn_DB);

                    //SaveStock(IDPG);

                }
            }

        }
            Response.Redirect("PG.aspx?d=1");
	}
    protected void calculatePriceModel(object sender, EventArgs e)
    {
        ((DropDownList)sender).Focus();
        CalcHarga();
    }
    protected void calculatePrice(object sender, EventArgs e)
    {
        ((TextBox)sender).Focus();
        CalcHarga();
    }
    protected void btn_add_emas_click(object sender, EventArgs e)
    {
        idsk.Text = idskhidden.Value;
        DataTable dtMember = new DataTable("AdditionalMember");

        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nocert"), new DataColumn("qty"), new DataColumn("berat"), new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"), new DataColumn("total_berat24"), new DataColumn("harga24") });
        var counter = 1;

        for (var i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];
            //var COIHOSB = Convert.ToDouble(((TextBox)gridViewRowTemp.Cells[2].FindControl("TextBoxAdditionalInsuranceCostHOSB")).Text);

            drMember = dtMember.NewRow();
            drMember["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember["nocert"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNoCert")).Text;
            drMember["qty"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtQty")).Text;

            drMember["berat"] = ((TextBox)gridViewRowTemp.Cells[1].FindControl("txtBerat")).Text;
            drMember["total_berat"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat")).Text;

            drMember["harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga")).Text;
            drMember["ongkos"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtOngkos")).Text;
            drMember["total_harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHarga")).Text;
            drMember["total_hargajual"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHargaJual")).Text;
            drMember["total_berat24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat24")).Text;
            drMember["harga24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga24")).Text;

            dtMember.Rows.Add(drMember);

            counter++;
        }



        drMember = dtMember.NewRow();
        drMember["number"] = counter;
        drMember["nocert"] = "";
        drMember["qty"] = "1";

        drMember["berat"] = "1";
        drMember["total_berat"] = "1";

        drMember["harga"] = "0";
        drMember["ongkos"] = "0";
        drMember["total_harga"] = "0";
        drMember["total_hargajual"] = "0";
        drMember["total_berat24"] = "1";
        drMember["harga24"] = "0";
        dtMember.Rows.Add(drMember);


        EmasGridView.DataSource = dtMember;
        EmasGridView.DataBind();
        EmasGridView.Rows[(EmasGridView.Rows.Count - 1)].Cells[1].FindControl("txtQty").Focus();

        CalcHarga();
    }

    protected void EmasGridView_onRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        idsk.Text = idskhidden.Value;

        DataTable dtMember = new DataTable("AdditionalMember");
        DataTable dtMember2 = new DataTable("AdditionalMember");

        DataRow drMember, drMember2;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nocert"), new DataColumn("qty"), new DataColumn("berat"), new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"), new DataColumn("total_berat24"), new DataColumn("harga24") });
        dtMember2.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nocert"), new DataColumn("qty"), new DataColumn("berat"), new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"), new DataColumn("total_berat24"), new DataColumn("harga24") });

        var counter = 1;
        int i = 0;
        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];


            drMember = dtMember.NewRow();
            drMember["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember["nocert"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNoCert")).Text;
            drMember["qty"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtQty")).Text;

            drMember["berat"] = ((TextBox)gridViewRowTemp.Cells[1].FindControl("txtBerat")).Text;
            drMember["total_berat"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat")).Text;

            drMember["harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga")).Text;
            drMember["ongkos"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtOngkos")).Text;
            drMember["total_harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHarga")).Text;
            drMember["total_hargajual"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHargaJual")).Text;
            drMember["total_berat24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat24")).Text;
            drMember["harga24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga24")).Text;

            dtMember.Rows.Add(drMember);
            counter++;
        }



        dtMember.Rows[e.RowIndex].Delete();

        EmasGridView.DataSource = dtMember;
        EmasGridView.DataBind();

        counter = 1;
        decimal totalbutir = 0;
        decimal totalongkospb = 0;
        decimal totalhargaparcel = 0;
        i = 0;
        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];

            drMember2 = dtMember2.NewRow();
            drMember2["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember2["nocert"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNoCert")).Text;
            drMember2["qty"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtQty")).Text;

            drMember2["berat"] = ((TextBox)gridViewRowTemp.Cells[1].FindControl("txtBerat")).Text;
            drMember2["total_berat"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat")).Text;

            drMember2["harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga")).Text;
            drMember2["ongkos"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtOngkos")).Text;
            drMember2["total_harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHarga")).Text;
            drMember2["total_hargajual"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHargaJual")).Text;
            drMember2["total_berat24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat24")).Text;
            drMember2["harga24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga24")).Text;

            dtMember2.Rows.Add(drMember2);
            counter++;
        }



        EmasGridView.DataSource = dtMember2;
        EmasGridView.DataBind();
        if (i > 0)
            EmasGridView.Rows[(EmasGridView.Rows.Count - 1)].Cells[1].FindControl("txtQty").Focus();


        CalcHarga();
    }

    protected string Generate_Nomor(StockReceivePG d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Year;
        int Bulan = d.Tgl.Month;

        /*int Nomor = db.StockReceiveDJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;

        return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        int Nomor = 1;

        try
        {
            var s = (from p in db.StockReceivePG
                     where p.Tgl.Year == Tahun && p.Nomor.StartsWith("B") /*&& p.Tgl.Month == Bulan*/
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(1, 8)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(3, 6)) + 1;
        }
        catch (Exception ex) { }

        string Kode = "P";
        try
        {
            Kode = App.KodePG;
        }
        catch (Exception ex) { }

        string Kode2 = convertCharPLU(Tahun.ToString().Remove(0, 2));

        string PLU = Kode + Kode2 + Str.KodeFormat(Nomor, 6);

        return PLU;

    }

    private string convertCharPLU(string number)
    {

        char alph1 = Convert.ToInt32(number.Substring(0, 1)) == 0 ? '0' : (char)(Convert.ToInt32(number.Substring(0, 1)) + 64);
        char alph2 = Convert.ToInt32(number.Substring(1, 1)) == 0 ? '0' : (char)(Convert.ToInt32(number.Substring(1, 1)) + 64);
        return alph1.ToString() + alph2.ToString();
    }

    protected void CalcHarga()
    {
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
        decimal Qty = 0m;
        decimal KadarLogam = 0m;
        decimal KadarTukaranJual = 0m;
        decimal KadarTukaranBeli = 0m;
        decimal TotalBerat24 = 0m;
        decimal TotalHarga24 = 0m;
        decimal SelisihMargin = 0m;
        //decimal OngkosPGFixed = 565000m;
        decimal OngkosPGFixed = 0m;
        decimal Rate = 0m;
        decimal SumQty = 0m;
        decimal SumBerat = 0m;
        decimal SumTotalBerat = 0m;
        decimal SumHarga = 0m;
        decimal SumOngkos = 0m;
        decimal SumTotalHarga = 0m;
        decimal SumTotalHargaJual = 0m;
        decimal SumTotalBerat24 = 0m;
        decimal SumTotalHarga24 = 0m;

        GoldPrice = Convert.ToDecimal(goldrate.Text);
        Rate = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableUS WHERE ID=3", Db.Cnn_DB);

        MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + Convert.ToInt32(ddl_Model.SelectedValue), Db.Cnn_DB);

        KadarLogam = Convert.ToDecimal(kadar_logam.Text);
        KadarTukaranBeli = Convert.ToDecimal(kadar_tukaran.Text);
        KadarTukaranJual = Convert.ToDecimal(kadar_jual.Text);

        SelisihMargin = (KadarTukaranBeli - KadarTukaranJual > 0) ? KadarTukaranBeli - KadarTukaranJual : 0;

        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {

            Qty = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtQty")).Text);
            Berat = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtBerat")).Text);

            if (ratetype.SelectedValue == "0")
            {
                TotalBerat = Qty * Berat;
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

                SumQty += Qty;
                SumBerat += Berat;
                SumHarga += Harga;
                SumOngkos += OngkosM;
                SumTotalBerat += TotalBerat;
                SumTotalHarga += TotalHarga;
                SumTotalHargaJual += TotalHargaJual;
                SumTotalBerat24 += TotalBerat24;
                SumTotalHarga24 += TotalHarga24;
            }
            else
            {
                TotalBerat = Qty * Berat;
                OngkosR = 0m;
                OngkosM = 0m;
                OngkosRUSD = 0m;
                OngkosMUSD = 0m;
                //Harga = GoldPrice * Rate  * TotalBerat;
                Harga = price.RoundTop(GoldPrice * Rate * TotalBerat);
                HargaUSD = GoldPrice * TotalBerat;
                TotalHarga = OngkosM + Harga;
                TotalHargaJual = price.RoundTop(Lib.CalcGJJual(Convert.ToInt32(ddlsupplier.SelectedValue), Convert.ToInt32(ddl_Model.SelectedValue), Berat, false));
                TotalHargaUSD = OngkosMUSD + HargaUSD;
                TotalHargaJualUSD = Lib.CalcGJJual(Convert.ToInt32(ddlsupplier.SelectedValue), Convert.ToInt32(ddl_Model.SelectedValue), Berat, true);

                TotalBerat24 = TotalBerat * (KadarTukaranBeli / 100);
                TotalHarga24 = TotalBerat24 * GoldPrice * Rate;

                SumQty += Qty;
                SumBerat += Berat;
                SumHarga += Harga;
                SumOngkos += OngkosM;
                SumTotalBerat += TotalBerat;
                SumTotalHarga += TotalHarga;
                SumTotalHargaJual += TotalHargaJual;
                SumTotalBerat24 += TotalBerat24;
                SumTotalHarga24 += TotalHarga24;
            }


            ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtTotalBerat")).Text = TotalBerat.ToString("N2");
            ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtHarga")).Text = (Harga).ToString("N0");
            ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtOngkos")).Text = (OngkosM).ToString("N0");
            ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtTotalHarga")).Text = (TotalHarga).ToString("N0");
            ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtTotalHargaJual")).Text = (TotalHargaJual).ToString("N0");
            ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtTotalBerat24")).Text = (TotalBerat24).ToString("N2");
            ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtHarga24")).Text = (TotalHarga24).ToString("N0");
        }

        var gridViewFooterRowTemp = EmasGridView.FooterRow;
        if (gridViewFooterRowTemp != null)
        {
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumQty")).Text = SumQty.ToString("N0");
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumBerat")).Text = SumBerat.ToString("N2");
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumTotalBerat")).Text = SumTotalBerat.ToString("N2");
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumHarga")).Text = SumHarga.ToString("N0");
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumOngkos")).Text = SumOngkos.ToString("N0");
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumTotalHarga")).Text = SumTotalHarga.ToString("N0");
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumTotalHargaJual")).Text = SumTotalHargaJual.ToString("N0");
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumTotalBerat24")).Text = SumTotalBerat24.ToString("N2");
            ((Literal)gridViewFooterRowTemp.Cells[4].FindControl("SumTotalHarga24")).Text = SumTotalHarga24.ToString("N0");
        }
    }
}