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

public partial class Page_Register_PG_Form : System.Web.UI.Page {
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

	protected string NoTTB { get { return Request.QueryString["nottb"]; } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
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
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("PG.aspx"); }

	//Form
	protected void fillForm() {
        var d = (from p in db.StockReceivePG
                where p.NoTTB == "" + NoTTB + "" && p.Approval == null
                select p).First();
        if (d == null)
            Response.Redirect("PG.aspx");
        else
        {
            ddl_Model.SelectedValue = d.Model.ToString();
            ddl_ProductLevel.SelectedValue = d.ProductLevel.ToString();
            ddl_RangkaWarna.SelectedValue = d.FrameColor.ToString();
            ddl_ProductItem.SelectedValue = d.ProductItem.ToString();
            ddl_KelompokUsia.SelectedValue = d.TargetAge.ToString();
            if(d.IDSupplier.HasValue)
                ddlsupplier.SelectedValue = Convert.ToInt32(d.IDSupplier.Value).ToString();
            ratetype.SelectedValue = (d.FixRate == true) ? "1" : "0";

            if (d.FixRate == false)
                DLC.CharGoldModel(ddl_Model);
            else
                DLC.CharGoldModelGJ(ddl_Model);
                ddl_Model.SelectedValue = d.Model.ToString();

            tgl.Text = Str.Tgl(d.Tgl);
            tgltjt.Text = Str.Tgl(d.TglTJT);
            supplier.Text = d.SupplierNama;
            sjsupplier.Text = d.SupplierSJ;
            stockkeeper.Text = d.HumanStockNama;
            keterangan.Text = d.Keterangan;
            NamaBarang.Text = d.NamaBarang;
            KodeBarang.Text = d.KodeBarang;
            kadar_tukaran.Text = d.KadarTukaranBeli.ToString("N2");
            kadar_jual.Text = d.KadarTukaranJual.ToString("N2");
            kadar_logam.Text = d.KadarLogam.ToString("N2");
            dimensir.Text = d.DimensiR != null ? Convert.ToDecimal(d.DimensiR).ToString("N2") : "";
			
			KodeMaterial.Text =  Db.SingleString("SELECT TOP 1 KodeMaterial FROM StockReceivePG WHERE NoTTB = '" + NoTTB + "'", Db.Cnn_DB);
            NomorValidasi.Text = Db.SingleString("SELECT TOP 1 NomorValidasi FROM StockReceivePG WHERE NoTTB = '" + NoTTB + "'", Db.Cnn_DB);

            if(File.Exists(d.ImgPicture))
            {
                Lib.Image(img, d.ImgPicture);
            }
            else Lib.Image(img, "D:/ImagesPalace/No-Image.png");

            LoadItem(NoTTB);
        }
	}

	//Save
	protected bool validTgl {
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
            DLC.CharGoldModel(ddl_Model);
        else
            DLC.CharGoldModelGJ(ddl_Model);

        CalcHarga();
    }

	protected void save_Click(object sender, EventArgs e) {
        if (validTgl && validTglTJT)
        {
            int myID;

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

            var gold = db.PricingTableTG.SingleOrDefault(p => p.ID == 41);
            var rate = db.PricingTableUS.SingleOrDefault(p => p.ID == 2);

            GoldPrice = Convert.ToDecimal(goldrate.Text);
            Rate = Convert.ToDecimal(rate.Harga);

            MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + Convert.ToInt32(ddl_Model.SelectedValue), Db.Cnn_DB);

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
            decimal dimensi = string.IsNullOrEmpty(dimensir.Text) ? 0 : Convert.ToDecimal(dimensir.Text);


            string NoCert = "";
            for (i = 0; i < EmasGridView.Rows.Count; i++)
            {
                myID = Convert.ToInt32(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtID")).Text);
                NoCert = ((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtNoCert")).Text;
                
                Berat = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[4].FindControl("txtBerat")).Text);
                TotalBerat24 = Convert.ToDecimal(((Literal)EmasGridView.Rows[i].Cells[4].FindControl("txtTotalBerat24")).Text);
                
                var n = db.StockReceivePG.SingleOrDefault(p => p.ID == myID);
                if (n != null)
                {

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

                    n.NomorSertifikat = NoCert;
                    n.KodeBarang = (KodeBarang.Text != "") ? KodeBarang.Text : "";
                    n.NamaBarang = (NamaBarang.Text != "") ? NamaBarang.Text : "";
                    n.Tgl = Tanggal;
                    n.TglTJT = TanggalTJT;
                    n.SupplierNama = Supplier;
                    n.SupplierSJ = SupplierSJ;
                    
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
                    if (dimensi > 0) n.DimensiR = dimensi;

                    db.SubmitChanges();
					Db.Execute("UPDATE StockReceivePG SET NomorValidasi = '" + NomorValidasi.Text + "', KodeMaterial = '" + KodeMaterial.Text + "' WHERE ID = " + n.ID, Db.Cnn_DB);
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
    
    protected void LoadItem(string NoTTB)
    {
        var rs = from p in db.StockReceivePG
                 where p.NoTTB == "" + NoTTB + "" && p.Approval == null
                 select p;

        DataTable dtMember = new DataTable("AdditionalMember");

        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nocert"), new DataColumn("id"), new DataColumn("qty"), new DataColumn("berat"), new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"), new DataColumn("total_berat24"), new DataColumn("harga24") });
        var counter = 1;

        foreach(var r in rs)
        {
            drMember = dtMember.NewRow();
            drMember["number"] = counter;
            drMember["nocert"] = r.NomorSertifikat;
            drMember["id"] = r.ID.ToString();
            drMember["berat"] = r.GrossWeight.ToString("N2");
            drMember["total_berat"] = r.GrossWeight.ToString("N2");

            drMember["harga"] = ((decimal) r.Harga).ToString("N0");
            drMember["ongkos"] = r.MountingM.ToString("N0");
            drMember["total_harga"] = r.TotalHarga.ToString("N0");
            drMember["total_hargajual"] = r.TotalHargaJual.Value.ToString("N0");
            drMember["total_berat24"] = r.NettoWeight.ToString("N2");
            drMember["harga24"] = r.TotalHarga24.ToString("N0");
            dtMember.Rows.Add(drMember);

            EmasGridView.DataSource = dtMember;
            EmasGridView.DataBind();

            counter++;
        }
        

        CalcHarga();
    }

	protected string Generate_Nomor(StockReceivePG d) {
		string x = string.Empty;

		int Tahun = d.Tgl.Year;
		int Bulan = d.Tgl.Month;

		int Nomor = db.StockReceivePG.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
		Nomor = Nomor + 1;

		return Str.KodeFormat(Tahun, Bulan, Nomor, 4);
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

        GoldPrice = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableTG WHERE ID=41", Db.Cnn_DB);
        Rate = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableUS WHERE ID=3", Db.Cnn_DB);

        MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + Convert.ToInt32(ddl_Model.SelectedValue), Db.Cnn_DB);

        KadarLogam = Convert.ToDecimal(kadar_logam.Text);
        KadarTukaranBeli = Convert.ToDecimal(kadar_tukaran.Text);
        KadarTukaranJual = Convert.ToDecimal(kadar_jual.Text);

        SelisihMargin = (KadarTukaranBeli - KadarTukaranJual > 0) ? KadarTukaranBeli - KadarTukaranJual : 0;

        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {

            Qty = 1;
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

    protected void uploadImg_ServerClick(object sender, EventArgs e)
    {
        if(SaveImage())
        {
            Response.Redirect(App.CurrentURL);
        }
    }

    private bool SaveImage()
    {
        var d = db.StockReceivePG.Where(p => p.NoTTB.Equals(NoTTB)).First();
        if (!Upload.FormatValid(imgpicture, imgpicture_ERR, Upload.ImageFormat)) return false;
        else
        {
            string fileName = d.ImgPicture.Split('\\').Last();
            string path = d.ImgPicture.Replace(fileName, "");
            if (Upload.CreateDir(path))
            {
                path += imgpicture.FileName;
                //Upload
                imgpicture.SaveAs(d.ImgPicture);

                return true;
            }
            else
            {
                imgpicture_ERR.InnerHtml += path;
                imgpicture_ERR.Visible = true;

                return false;
            }
        }
    }
}