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


public partial class Page_Register_CZ_Form : System.Web.UI.Page
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
    protected int myIDOutCZ
    {
        get { return (App.QueryInt(this, "iddo")); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //keterangan.Focus();

            //DLC.CharGoldModel(ddl_Model);
            //DLC.CharGoldLevel(ddl_ProductLevel);
            //DLC.CharFrameColor(ddl_RangkaWarna);
            //DLC.CharProductItem(ddl_ProductItem);
            //DLC.CharTargetAge(ddl_KelompokUsia);
            DLC.DDLSupplier(ddlsupplier);

            GoldPrice = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableTG WHERE ID=3", Db.Cnn_DB);
            goldrate.Text = GoldPrice.ToString("N2");

            //goldrate.Enabled = false;

            //Form
            fillForm();

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tgltjt.Text = Str.Tgl(DateTime.Today);

            btn_add_emas_click(EmasGridView, e);
        }

    }

    //Exception
    protected void DataError() { Response.Redirect("CZ.aspx"); }

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
        //if (ratetype.SelectedValue == "0")
        //{
            //DLC.CharGoldModel(ddl_Model);
            ((DataControlField)EmasGridView.Columns
            .Cast<DataControlField>()
            .Where(fld => fld.HeaderText == "Total Harga Jual")
            .SingleOrDefault()).Visible = false;
        //}
        //else
        //{
        //    DLC.CharGoldModelGJ(ddl_Model);
        //    ((DataControlField)EmasGridView.Columns
        //    .Cast<DataControlField>()
        //    .Where(fld => fld.HeaderText == "Total Harga Jual")
        //    .SingleOrDefault()).Visible = true;
        //}

        CalcHarga();
    }

    protected void save_Click(object sender, EventArgs e)
    {
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
            //decimal DimensiR = 0m;
            string noplu = "";
            int sqty;
            decimal sweight = 0m;

            //try
            //{
            //    DimensiR = Convert.ToDecimal(dimensir.Text);
            //}
            //catch { }



            var gold = db.PricingTableTG.SingleOrDefault(p => p.ID == 3);
            var rate = db.PricingTableUS.SingleOrDefault(p => p.ID == 2);

            GoldPrice = Convert.ToDecimal(goldrate.Text);
            Rate = Convert.ToDecimal(rate.Harga);

          
            int IDWarehouse = myIDWarehouse;

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

            

            for (i = 0; i < EmasGridView.Rows.Count; i++)
            {
                KadarTukaranJual = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtKjual")).Text);

                string pl = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtPlevel")).Text.ToString();
                int ProductLevel = Convert.ToInt32((from p in db.CharGoldLevel where p.Nama == pl select p.ID).FirstOrDefault());

                string model = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtModel")).Text.ToString();
                int IDModel = Convert.ToInt32((from p in db.CharGoldModel where p.Nama == model select p.ID).FirstOrDefault());

                MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + IDModel, Db.Cnn_DB);


                string item = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtPitem")).Text.ToString();
                int ProductItem = Convert.ToInt32((from p in db.CharProductItem where p.Nama == item select p.ID).FirstOrDefault());

                string frame = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtFrame")).Text.ToString();
                int FrameColor = Convert.ToInt32((from p in db.CharFrameColor where p.Nama == frame select p.ID).FirstOrDefault());

                string age = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtTarget")).Text.ToString();
                int TargetAge = Convert.ToInt32((from p in db.CharTargetAge where p.Nama == age select p.ID).FirstOrDefault());

                KadarLogam = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtKlogam")).Text);
                KadarTukaranBeli = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtKbeli")).Text);

                noplu = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtPLU")).Text.ToString();
                Qty = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtQty")).Text);
                Berat = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtBerat")).Text);
                TotalBerat24 = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtTotalBerat24")).Text);

                sqty = Convert.ToInt32(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtStoneqty")).Text);
                sweight = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtStoneweight")).Text);

                string nokirim = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtNokirim")).Text.ToString();
                string pic = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtPicture")).Text.ToString();

                SelisihMargin = (KadarTukaranBeli - KadarTukaranJual > 0) ? KadarTukaranBeli - KadarTukaranJual : 0;
                for (int j = 1; j <= Convert.ToInt32(Qty); j++)
                {                 

                    //string qr = "exec sp_getnocz '" + myIDOutCZ + "'";
                    //string nokirim = Db.SingleString(qr, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
                    
                    //string qr1 = "exec sp_getpic '" + noplu + "'";
                    //string pic = Db.SingleString(qr1, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
                    

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

                    string path = "";

                    path += App.ImagesDir;
                    path += "StockReceivePG" + "\\";


                    string KodeBarang = ((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtKodeBarang")).Text.ToString();
                    string NamaBarang = ((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtNamaBarang")).Text.ToString();

                    decimal dimensir = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtDimensir")).Text);

                    StockReceivePG n = new StockReceivePG();


                    n.KodeBarang = (KodeBarang != "") ? KodeBarang: "" ; 
                    n.NamaBarang = (NamaBarang != "") ? NamaBarang : "";
                    n.Tgl = Tanggal;
                    n.Tgl = Tanggal;
                    n.TglTJT = TanggalTJT;
                    n.SupplierNama = Supplier;
                    n.SupplierSJ = SupplierSJ;
                    n.IDWarehouse = myIDWarehouse;
                    n.NoTTB = "TTB/" + Tahun + "/" + Bulan + "/" + KodeFormat;
                    //n.Nomor = "P" + Tahun + KodeFormatPLU;
                    n.Nomor = noplu;
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
                    //n.ImgPicture = path + ((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtKodeBarang")).Text.ToString() + ".jpg";

                    n.ImgPicture = pic;

                    n.IDSupplier = Convert.ToInt32(ddlsupplier.SelectedValue);
                    n.FixRate = ratetype.SelectedValue == "0" ? false : true;
                    n.HumanStock = (idsk.Text != "") ? Convert.ToInt32(idsk.Text) : 0;
                    n.DimensiR = dimensir;
                    n.NoKirimCZ = nokirim;
                    n.StoneWeight = sweight;
                    n.StoneQty = sqty;

                    db.StockReceivePG.InsertOnSubmit(n);                   
                    db.SubmitChanges();

                    int ID = Db.SingleInteger("SELECT TOP 1 ID FROM StockReceivePG Order By ID DESC", Db.Cnn_DB);
                    var d = db.StockReceivePG.SingleOrDefault(x => x.ID == ID);

                    //SaveStock(IDPG);

                }
            }

        }
        Response.Redirect("CZ.aspx?d=1");
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
        dtMember.Columns.AddRange(new DataColumn[] {new DataColumn("plu"), new DataColumn("kodebarang"), new DataColumn("qty"), new DataColumn("berat"),
            new DataColumn("plevel"), new DataColumn("model"), new DataColumn("pitem"), new DataColumn("fcolor"), new DataColumn("target"), new DataColumn("klogam"),
            new DataColumn("kbeli"), new DataColumn("namabarang"),
            new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"),
            new DataColumn("total_berat24"), new DataColumn("harga24"), new DataColumn("stoneweight"), new DataColumn("stoneqty"),
            new DataColumn("kjual"), new DataColumn("dimensir"), new DataColumn("numb"), new DataColumn("nokirim"), new DataColumn("picture")
        });
      
        string query = "exec sp_display_srcz '" + myIDOutCZ + "'";
        DataTable cek1 = new DataTable();
            cek1 = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        int numb = 1;
        foreach (DataRow cek2 in cek1.Rows)
        {
            drMember = dtMember.NewRow();
            int idcolumn = 1;


            foreach (DataColumn cek3 in cek1.Columns)
            {
               // drMember["number"] = counter;              
                drMember["qty"] = "1";
                //drMember["kjual"] = 0m;

                if (idcolumn == 1) { drMember["plu"] = cek2[cek3]; }
                if (idcolumn == 2) { drMember["berat"] = Convert.ToDecimal(cek2[cek3]).ToString("N2"); }
                if (idcolumn == 3) {
                    string kb = cek2[cek3].ToString();
                    if (kb.Length > 8)
                    {
                        kb.Substring(1, (kb.Length) - 4);
                        drMember["kodebarang"] = kb;
                    }
                    else {
                         drMember["kodebarang"] = kb;
                    }

                                    }
                if (idcolumn == 4)
                {
                    drMember["plevel"] = cek2[cek3];
                    if (drMember["plevel"].ToString() == "Cubic Zirconia 75%")
                    {
                        drMember["kjual"] = 94.5m;
                    }
                    else if (drMember["plevel"].ToString() == "Cubic Zirconia 37,5%")
                    {
                        drMember["kjual"] = 56.5m;
                    }
                }
                if (idcolumn == 5) { drMember["model"] = cek2[cek3]; }
                if (idcolumn == 6) {
                    drMember["pitem"] = cek2[cek3];
                    drMember["namabarang"] = cek2[cek3] + " CZ";
                }
                if (idcolumn == 7) { drMember["fcolor"] = cek2[cek3]; }
                if (idcolumn == 8) { drMember["target"] = cek2[cek3]; }
                if (idcolumn == 9) { drMember["klogam"] = Convert.ToDecimal(cek2[cek3]).ToString("N2"); }
                if (idcolumn == 10){ drMember["kbeli"] = Convert.ToDecimal(cek2[cek3]).ToString("N2");}
                if (idcolumn == 11) { drMember["stoneweight"] = Convert.ToDecimal(cek2[cek3]).ToString("N2"); }
                if (idcolumn == 12) { drMember["stoneqty"] = Convert.ToInt32(cek2[cek3]); }
                if (idcolumn == 13) { drMember["dimensir"] = Convert.ToDecimal(cek2[cek3]).ToString("N2");  }
                if (idcolumn == 14) { drMember["nokirim"] = cek2[cek3]; }
                if(idcolumn == 15) { drMember["picture"] = cek2[cek3]; }

                drMember["total_berat"] ="1";
                   
                drMember["harga"] = "0";
                drMember["ongkos"] = "0";
                drMember["total_harga"] = "0";
                drMember["total_hargajual"] = "0";
                drMember["total_berat24"] = "1";
                drMember["harga24"] = "0";

                drMember["numb"] = numb;
                idcolumn++;
            }
            dtMember.Rows.Add(drMember);
            numb++;
        }

        EmasGridView.DataSource = dtMember;
        EmasGridView.DataBind();
        EmasGridView.Rows[(EmasGridView.Rows.Count - 1)].Cells[1].FindControl("txtKodeBarang").Focus();

        CalcHarga();
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

        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {
            string model = ((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtModel")).Text;
            int IDModel = Convert.ToInt32((from p in db.CharGoldModel where p.Nama == model select p.ID).FirstOrDefault());

            MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + IDModel, Db.Cnn_DB);


            KadarLogam = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtKlogam")).Text);
            KadarTukaranBeli = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtKbeli")).Text);

            KadarTukaranJual = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtKjual")).Text);

            SelisihMargin = (KadarTukaranBeli - KadarTukaranJual > 0) ? KadarTukaranBeli - KadarTukaranJual : 0;

            Qty = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtQty")).Text);
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
                TotalHargaJual = Pricing.CalcGJJual(Convert.ToInt32(ddlsupplier.SelectedValue), IDModel, Berat, false);
                TotalHargaUSD = OngkosMUSD + HargaUSD;
                TotalHargaJualUSD = Pricing.CalcGJJual(Convert.ToInt32(ddlsupplier.SelectedValue), IDModel, Berat, true);

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