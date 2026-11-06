using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_PLU_Reborn : System.Web.UI.Page
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
	protected string myPLU { get { return Request.QueryString["PLU"]; } }
	
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        btnReborn.Enabled = false;
        if (!IsPostBack)
        {

            txtPLU.Text = "";
            selectStore.SelectedValue = "0";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }
    protected void fillFilter()
    {
        txtPLU.Text = Request.QueryString["PLU"];
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&PLU=" + txtPLU.Text;

        Response.Redirect(href);
    }
    protected void fillTable(object sender, EventArgs e)
    {
        string query = "EXEC sp_check_plu_reborn '" + txtPLU.Text + "', '" + selectStore.SelectedValue + "' ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        if (!String.IsNullOrEmpty(txtPLU.Text))
        {
            if (dt.Rows.Count == 0)
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.ColumnSpan = 6;
                td.Style.Add("text-align", "center");
                td.Text = "PLU tidak ditemukan.";
                tr.Cells.Add(td);
                tblMasterPLU.Rows.Add(tr);
            }
            else if (dt.Rows.Count >= 2)
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.ColumnSpan = 6;
                td.Style.Add("text-align", "center");
                td.Text = "PLU error. Jumlah PLU lebih dari satu.";
                tr.Cells.Add(td);
                tblMasterPLU.Rows.Add(tr);
            }
            else
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Style.Add("text-align", "center");
                td.Text = dt.Rows[0][0].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Style.Add("text-align", "center");
                td.Text = dt.Rows[0][1].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Style.Add("text-align", "center");
                td.Text = dt.Rows[0][2].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Style.Add("text-align", "center");
                td.Text = dt.Rows[0][3].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Style.Add("text-align", "center");
                td.Text = dt.Rows[0][4].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Style.Add("text-align", "center");
                td.Text = dt.Rows[0][5].ToString();
                tr.Cells.Add(td);

                tblMasterPLU.Rows.Add(tr);
				
				if(dt.Rows[0][5].ToString().ToUpper() == "LOOSE DIAMOND")
				{
					harga.Visible = false;
				}
				else
				{
					if(dt.Rows[0][5].ToString().ToUpper() == "DIAMOND JEWELLERY")
					{
						var d = db.StockProductDJ.SingleOrDefault(p => p.Nomor == txtPLU.Text);
						harga.Text = "Harga : Rp" + Convert.ToDecimal(d.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2") ;
					}
					else if (dt.Rows[0][5].ToString().ToUpper() == "GOLD JEWELLERY")
					{
						var d = db.StockProductGJ.SingleOrDefault(p => p.Nomor == txtPLU.Text);
						harga.Text = "Harga : Rp" + Convert.ToDecimal(d.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N2");
					}
					else if (dt.Rows[0][5].ToString().ToUpper() == "PLAIN GOLD")
					{
						var d = db.StockProductPG.SingleOrDefault(p => p.Nomor == txtPLU.Text);
						//Estimasi Harga
						decimal GoldPrice = GoldPrice = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableTG WHERE ID=3", Db.Cnn_DB);
						decimal Rate = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableUS WHERE ID=2", Db.Cnn_DB);

						decimal MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + d.Model, Db.Cnn_DB);

						decimal TotalBerat = 1 * Convert.ToDecimal(d.GrossWeight.Value);
						decimal Harga = (((Convert.ToDecimal(d.KadarLogam.Value) / 100) * GoldPrice) * Rate) * TotalBerat;
						harga.Text = "Harga : Rp" + Convert.ToDecimal(Harga).ToString("N2");
					}
					harga.Visible = true;
				}
            }

            var StPrDJ = (from p in db.StockProductDJ
                          where p.Nomor == txtPLU.Text
                          select new DataPLU
                          {
                              PLU = p.Nomor,
                              ImagesHref = p.ImgPicture,

                              //Characteristic Product
                              ProductCategoryShow = p.StockProductDJ_CharProduct.CharProductCategory.Nama + " (" + p.StockProductDJ_CharProduct.CharProductCategory.NamaKode + ")",
                              ProductItemShow = p.StockProductDJ_CharProduct.CharProductItem.Nama + " (" + p.StockProductDJ_CharProduct.CharProductItem.NamaKode + ")",
                              ProductLevelShow = p.StockProductDJ_CharProduct.CharProductLevel.Nama + " (" + p.StockProductDJ_CharProduct.CharProductLevel.NamaKode + ")",
                              StoneDistributionShow = p.StockProductDJ_CharProduct.CharStoneDist.Nama + " (" + p.StockProductDJ_CharProduct.CharStoneDist.NamaKode + ")",
                              FrameMaterialShow = p.StockProductDJ_CharProduct.CharFrameMaterial.Nama + " (" + p.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode + ")",
                              FrameFinishingShow = p.StockProductDJ_CharProduct.CharFrameFinishing.Nama + " (" + p.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode + ")",
                              FrameColorShow = p.StockProductDJ_CharProduct.CharFrameColor.Nama + " (" + p.StockProductDJ_CharProduct.CharFrameColor.NamaKode + ")",
                              ProcessConsShow = p.StockProductDJ_CharProduct.CharProcessCons.Nama + " (" + p.StockProductDJ_CharProduct.CharProcessCons.NamaKode + ")",
                              //Weight
                              GrossWeight = Str.Number3(p.StockProductDJ_CharProduct.GrossWeight),
                              StoneCarat = Str.Number3(p.StockProductDJ_CharProduct.StoneCarat),
                              StoneWeight = Str.Number3(p.StockProductDJ_CharProduct.StoneWeight),
                              NettoWeight = Str.Number3(p.StockProductDJ_CharProduct.NettoWeight),
                              //Dimensi
                              DimensiD = Str.Number2(p.StockProductDJ_CharProduct.DimensiD),
                              DimensiP = Str.Number2(p.StockProductDJ_CharProduct.DimensiP),
                              DimensiL = Str.Number2(p.StockProductDJ_CharProduct.DimensiL),
                              DimensiR = Str.Number2(p.StockProductDJ_CharProduct.DimensiR),

                              //Characteristic Design
                              DesignCategory = p.StockProductDJ_CharDesign.CharDesignCategory.Nama + " (" + p.StockProductDJ_CharDesign.CharDesignCategory.NamaKode + ")",
                              DesignConcept = p.StockProductDJ_CharDesign.CharDesignConcept.Nama + " (" + p.StockProductDJ_CharDesign.CharDesignConcept.NamaKode + ")",
                              DesignProcess = p.StockProductDJ_CharDesign.CharDesignProcess.Nama + " (" + p.StockProductDJ_CharDesign.CharDesignProcess.NamaKode + ")",
                              TargetAge = p.StockProductDJ_CharDesign.CharTargetAge.Nama + " (" + p.StockProductDJ_CharDesign.CharTargetAge.NamaKode + ")",
                              TargerGender = p.StockProductDJ_CharDesign.CharTargetGender.Nama + " (" + p.StockProductDJ_CharDesign.CharTargetGender.NamaKode + ")",
                              Naming = p.StockProductDJ_CharDesign.Naming,
                              KeteranganDesign = designKet(p.StockProductDJ_CharDesign),
                              Grafir = p.StockProductDJ_CharDesign.Grafir,
                              Finishing = designFinishing(p)
                          });
            var StPrPG = (from p in db.StockProductPG
                          where p.Nomor == txtPLU.Text
                          select new DataPLU
                          {
                              PLU = p.Nomor,
                              ImagesHref = p.ImgPicture,

                              //Characteristic Product
                              ProductCategoryShow = "-",
                              ProductItemShow = p.CharProductItem.Nama,
                              ProductLevelShow = p.CharGoldLevel.Nama,
                              StoneDistributionShow = "-",
                              FrameMaterialShow = "-",
                              FrameFinishingShow = "-",
                              FrameColorShow = p.CharFrameColor.Nama + " (" + p.CharFrameColor.NamaKode + ")",
                              ProcessConsShow = "-",
                              //Weight
                              GrossWeight = Str.Number3(p.GrossWeight),
                              StoneCarat = "-",
                              StoneWeight = "-",//Str.Number3(p.StoneWeight),
                              NettoWeight = Str.Number3(p.NettoWeight),
                              //Dimensi
                              DimensiD = "-",
                              DimensiP = "-",
                              DimensiL = "-",
                              DimensiR = Str.Number2(p.DimensiR),

                              //Characteristic Design
                              DesignCategory = "-",//p.CharDesignCategory.Nama + "(" + p.StockProductDJ_CharDesign.CharDesignCategory.NamaKode + ")",
                              DesignConcept = "-",//p.CharDesignConcept.Nama + "(" + p.StockProductDJ_CharDesign.CharDesignConcept.NamaKode + ")",
                              DesignProcess = "-",//p.CharDesignProcess.Nama + "(" + p.StockProductDJ_CharDesign.CharDesignProcess.NamaKode + ")",
                              TargetAge = p.CharTargetAge.Nama + " (" + p.CharTargetAge.NamaKode + ")",
                              TargerGender = "-",//p.CharTargetGender.Nama + "(" + p.StockProductDJ_CharDesign.CharTargetGender.NamaKode + ")",
                              Naming = "-",//p.Naming.ToString(),
                              KeteranganDesign = "-",//designKet(p.StockProductDJ_CharDesign),
                              Grafir = "-",//p.Grafir.ToString(),
                              Finishing = "-",//designFinishing(p)
                          });
            var StPrGJ = (from p in db.StockProductGJ
                          where p.Nomor == txtPLU.Text
                          select new DataPLU
                          {
                              PLU = p.Nomor,
                              ImagesHref = p.ImgPicture,

                              //Characteristic Product
                              ProductCategoryShow = p.StockProductGJ_CharProduct.CharProductCategory.Nama,
                              ProductItemShow = p.StockProductGJ_CharProduct.CharProductItem.Nama,
                              ProductLevelShow = p.StockProductGJ_CharProduct.CharProductLevel.Nama,
                              StoneDistributionShow = p.StockProductGJ_CharProduct.CharStoneDist.Nama,
                              FrameMaterialShow = p.StockProductGJ_CharProduct.CharFrameMaterial.Nama + " (" + p.StockProductGJ_CharProduct.CharFrameMaterial.NamaKode + ")",
                              FrameFinishingShow = p.StockProductGJ_CharProduct.CharFrameFinishing.Nama + " (" + p.StockProductGJ_CharProduct.CharFrameMaterial.NamaKode + ")",
                              FrameColorShow = p.StockProductGJ_CharProduct.CharFrameColor.Nama + " (" + p.StockProductGJ_CharProduct.CharFrameColor.NamaKode + ")",
                              ProcessConsShow = p.StockProductGJ_CharProduct.CharProcessCons.Nama + " (" + p.StockProductGJ_CharProduct.CharProcessCons.NamaKode + ")",
                              //Weight
                              GrossWeight = Str.Number3(p.StockProductGJ_CharProduct.GrossWeight),
                              StoneCarat = Str.Number3(p.StockProductGJ_CharProduct.StoneCarat),
                              StoneWeight = Str.Number3(p.StockProductGJ_CharProduct.StoneWeight),
                              NettoWeight = Str.Number3(p.StockProductGJ_CharProduct.NettoWeight),
                              //Dimensi
                              DimensiD = Str.Number2(p.StockProductGJ_CharProduct.DimensiD),
                              DimensiP = Str.Number2(p.StockProductGJ_CharProduct.DimensiP),
                              DimensiL = Str.Number2(p.StockProductGJ_CharProduct.DimensiL),
                              DimensiR = Str.Number2(p.StockProductGJ_CharProduct.DimensiR),

                              //Characteristic Design
                              DesignCategory = p.StockProductGJ_CharDesign.CharDesignCategory.Nama + " (" + p.StockProductGJ_CharDesign.CharDesignCategory.NamaKode + ")",
                              DesignConcept = p.StockProductGJ_CharDesign.CharDesignConcept.Nama + " (" + p.StockProductGJ_CharDesign.CharDesignConcept.NamaKode + ")",
                              DesignProcess = p.StockProductGJ_CharDesign.CharDesignProcess.Nama + " (" + p.StockProductGJ_CharDesign.CharDesignProcess.NamaKode + ")",
                              TargetAge = p.StockProductGJ_CharDesign.CharTargetAge.Nama + " (" + p.StockProductGJ_CharDesign.CharTargetAge.NamaKode + ")",
                              TargerGender = p.StockProductGJ_CharDesign.CharTargetGender.Nama + " (" + p.StockProductGJ_CharDesign.CharTargetGender.NamaKode + ")",
                              Naming = p.StockProductGJ_CharDesign.Naming,
                              KeteranganDesign = "-",
                              Grafir = p.StockProductGJ_CharDesign.Grafir,
                              Finishing = "-"
                          });
            var StPrLD = (from p in db.StockProductLD
                          where p.Nomor == txtPLU.Text
                          select new DataPLU
                          {
                              PLU = p.Nomor,
                              ImagesHref = p.ImgPicture,

                              //Characteristic Product
                              ProductCategoryShow = "-",
                              ProductItemShow = "-",
                              ProductLevelShow = "-",
                              StoneDistributionShow = "-",
                              FrameMaterialShow = "-",
                              FrameFinishingShow = "-",
                              FrameColorShow = "-",
                              ProcessConsShow = "-",
                              //Weight
                              GrossWeight = "-",
                              StoneCarat = "-",
                              StoneWeight = "-",
                              NettoWeight = "-",
                              //Dimensi
                              DimensiD = "-",
                              DimensiP = "-",
                              DimensiL = "-",
                              DimensiR = "-",

                              //Characteristic Design
                              DesignCategory = p.StockProductLD_CharDesign.CharDesignCategory.Nama + "(" + p.StockProductLD_CharDesign.CharDesignCategory.NamaKode + ")",
                              DesignConcept = p.StockProductLD_CharDesign.CharDesignConcept.Nama + "(" + p.StockProductLD_CharDesign.CharDesignConcept.NamaKode + ")",
                              DesignProcess = p.StockProductLD_CharDesign.CharDesignProcess.Nama + "(" + p.StockProductLD_CharDesign.CharDesignProcess.NamaKode + ")",
                              TargetAge = p.StockProductLD_CharDesign.CharTargetAge.Nama + "(" + p.StockProductLD_CharDesign.CharTargetAge.NamaKode + ")",
                              TargerGender = p.StockProductLD_CharDesign.CharTargetGender.Nama + "(" + p.StockProductLD_CharDesign.CharTargetGender.NamaKode + ")",
                              Naming = "-",//p.Naming.ToString(),
                              KeteranganDesign = "-",//designKet(p.StockProductDJ_CharDesign),
                              Grafir = "-",//p.Grafir.ToString(),
                              Finishing = "-",//designFinishing(p)
                          });
            List<DataPLU> StPrs = new List<DataPLU>();

            StPrs.AddRange(StPrDJ);
            StPrs.AddRange(StPrGJ);
            StPrs.AddRange(StPrPG);
            StPrs.AddRange(StPrLD);

            if (StPrs.Count() == 0)
            {

            }
            else if (StPrs.Count() > 1)
            {

            }
            else
            {
                var StPr = StPrs.SingleOrDefault();
                btnReborn.Enabled = true;

                //DetailPLU
                TableHeaderRow thr = new TableHeaderRow();
                thr.BackColor = Color.Blue;
                thr.ForeColor = Color.White;
                TableHeaderCell thd = new TableHeaderCell();
                thd.ColumnSpan = 3;
                thd.Style.Add("text-align", "center");
                thd.Text = "Detail PLU";
                thr.Cells.Add(thd);
                tblDetailPLU.Rows.Add(thr);

                thr = new TableHeaderRow();
                thr.BackColor = Color.LightBlue;
                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Images PLU";
                thr.Cells.Add(thd);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Characteristic Product";
                thr.Cells.Add(thd);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Characteristic Design";
                thr.Cells.Add(thd);
                tblDetailPLU.Rows.Add(thr);

                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Style.Add("text-align", "center");
                td.Style.Add("width", "20%");

                string url = string.Empty;
                url += "/Images.aspx?path=";
                url += StPr.ImagesHref == null ? "" : StPr.ImagesHref;
                td.Text = "<img style=\"width:200px;height:200px\" src=\"" + url + "\">";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Style.Add("width", "55%");
                td.Text += "<div class=\"form-horizontal\" >";
                td.Text = "<div class=\"col-sm-7\">";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Item </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.ProductItemShow + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Category </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.ProductCategoryShow + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Level </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.ProductLevelShow + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Stone Dist. </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.StoneDistributionShow + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Fr. Material </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.FrameMaterialShow + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Fr. Finishing </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.FrameFinishingShow + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Fr. Color </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.FrameColorShow + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Cons Process </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.ProcessConsShow + "</p></div>";
                td.Text += "</div>";

                td.Text += "<div class=\"col-sm-5\">";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-6\"> Gross Weight </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-4\">" + StPr.GrossWeight + " gr</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-6\"> Stone Carat </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-4\">" + StPr.StoneCarat + " carat</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-6\"> Stone Weight </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-4\">" + StPr.StoneWeight + " gr</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-6\"> Netto Weight </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-4\">" + StPr.NettoWeight + " gr</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-6\"> Diameter </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-4\">" + StPr.DimensiD + " mm</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-6\"> Panjang </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-4\">" + StPr.DimensiP + " mm</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-6\"> Lebar </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-4\">" + StPr.DimensiL + " mm</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-6\"> Ring Size </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-4\">" + StPr.DimensiR + " mm</p></div>";
                td.Text += "</div>";
                td.Text += "</div>";
                tr.Cells.Add(td);

                td = new TableCell();
                //td.Style.Add("text-align", "center");
                td.Text = "<div class=\"form-horizontal\" >";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Category </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.DesignCategory + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Concept </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.DesignConcept + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Process </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.DesignProcess + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Target Age </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.TargetAge + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Target Gender </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.TargerGender + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Naming Product </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.Naming + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Keterangan </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.KeteranganDesign + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Grafir </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.Grafir + "</p></div>";
                td.Text += "<div class=\"form-group\"><p class=\"col-sm-5\"> Finishing </p> <p class=\"col-sm-1\">:</p> <p class=\"col-sm-5\">" + StPr.Finishing + "</p></div>";
                td.Text += "</div>";
                tr.Cells.Add(td);
                tblDetailPLU.Rows.Add(tr);

                //StoneTable
                thr = new TableHeaderRow();
                thr.BackColor = Color.Blue;
                thr.ForeColor = Color.White;
                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Stone";
                thd.ColumnSpan = 8;
                thr.Cells.Add(thd);
                tblDetailStone.Rows.Add(thr);


                thr = new TableHeaderRow();
                thr.BackColor = Color.LightBlue;
                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "No";
                thr.Cells.Add(thd);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Shape";
                thr.Cells.Add(thd);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Colour";
                thr.Cells.Add(thd);
                tblDetailPLU.Rows.Add(thr);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Clarity";
                thr.Cells.Add(thd);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Carat/Butir";
                thr.Cells.Add(thd);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Total Butir";
                thr.Cells.Add(thd);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Total Carat";
                thr.Cells.Add(thd);

                thd = new TableHeaderCell();
                thd.Style.Add("text-align", "center");
                thd.Text = "Product Type";
                thr.Cells.Add(thd);
                tblDetailStone.Rows.Add(thr);

                fillStone();
            }
        }
        else
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.ColumnSpan = 6;
            td.Style.Add("text-align", "center");
            td.Text = "PLU tidak boleh kosong";
            tr.Cells.Add(td);
            tblMasterPLU.Rows.Add(tr);
        }
		dt.Dispose();
    }


    protected void test(object sender, EventArgs e)
    {
        txtPLU.Text = selectStore.SelectedItem.Text;
    }

    protected void aasd(object sender, EventArgs e)
    {
        txtPLU.Text = selectStore.SelectedValue;
    }

    protected string designKet(StockProductDJ_CharDesign d)
    {
        string x = string.Empty;

        if (d.SetItem)
        {
            if (x != "") x += ", ";
            x += "Set Item";
        }
        if (d.Special)
        {
            if (x != "") x += ", ";
            x += "Special DJ";
        }

        return x;
    }

    protected string designFinishing(StockProductDJ d)
    {
        string x = string.Empty;

        var rs = from p in d.StockProductDJ_Finishing select p;
        foreach (var r in rs)
        {
            if (x != "") x += "<br />";
            x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
        }

        return x;
    }
    protected void fillStone() {
        string spname = "sp_get_stone_information";
        string query = " exec " + spname + " '"+txtPLU.Text+"'";

        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        if(dt.Rows.Count > 0)
        {
            foreach (DataRow r in dt.Rows)
            {
                if (r["Tipe"].ToString() == "Gold Jewellery" || r["Tipe"].ToString() == "Plain Gold") {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();
                    td.ColumnSpan = dt.Columns.Count;
                    td.Style.Add("text-align", "center");
                    td.Text = "No Stone";
                    tr.Cells.Add(td);
                    tblDetailStone.Rows.Add(tr);
                }
                else
                {
                    TableRow tr = new TableRow();
                    foreach (DataColumn c in dt.Columns)
                    {
                        TableCell td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = r[c].ToString();
                        tr.Cells.Add(td);

                        if(c.ColumnName == "TotalButir")
                        {
                            TotalButir += Convert.ToDecimal(r[c].ToString());
                        }
                        
                        if(c.ColumnName == "TotalCarat")
                        {
                            TotalCarat += Convert.ToDecimal(r[c].ToString());
                        }
                    }
                    tblDetailStone.Rows.Add(tr);
                }
            }

            TableFooterRow tfr = new TableFooterRow();
            tfr.BackColor = Color.LightCyan;
            TableCell tfd = new TableCell();
            tfd.ColumnSpan = 5;
            tfd.Style.Add("text-align", "center");
            tfd.Style.Add("font-family", "consolas");
            tfd.Style.Add("font-weight", "900");
            tfd.Style.Add("font-size", "16px");
            tfd.Text = "Total";
            tfr.Cells.Add(tfd);

            tfd = new TableCell();
            tfd.Style.Add("text-align", "center");
            tfd.Text = TotalButir.ToString();
            tfr.Cells.Add(tfd);

            tfd = new TableCell();
            tfd.Style.Add("text-align", "center");
            tfd.Text = TotalCarat.ToString();
            tfr.Cells.Add(tfd);

            tfd = new TableCell();
            tfd.ColumnSpan = 5;
            tfd.Style.Add("text-align", "center");
            tfd.Text = "";
            tfr.Cells.Add(tfd);
            tblDetailStone.Rows.Add(tfr);
        }
        else
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.ColumnSpan = dt.Columns.Count;
            td.Style.Add("text-align", "center");
            td.Text = "No Stone";
            tr.Cells.Add(td);
            tblDetailStone.Rows.Add(tr);


            TableFooterRow tfr = new TableFooterRow();
            td = new TableCell();
            td.ColumnSpan = 5;
            td.Style.Add("text-align", "center");
            td.Text = "Total";
            tfr.Cells.Add(td);

            td = new TableCell();
            td.Style.Add("text-align", "center");
            td.Text = "";
            tfr.Cells.Add(td);

            td = new TableCell();
            td.Style.Add("text-align", "center");
            td.Text = "";
            tfr.Cells.Add(td);

            td = new TableCell();
            td.ColumnSpan = 5;
            td.Style.Add("text-align", "center");
            td.Text = "";
            tfr.Cells.Add(td);
            tblDetailStone.Rows.Add(tr);
        }
		dt.Dispose();
    }
	
	protected void Save(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(txtPLU.Text))
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Text PLU tidak boleh kosong.')", true);
        }
        else
        {
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.Nomor == txtPLU.Text);
            var h = db.StockReceivePG.SingleOrDefault(p => p.Nomor == txtPLU.Text);
            var f = db.StockReceiveGJ.SingleOrDefault(p => p.Nomor == txtPLU.Text);
            var g = db.StockReceiveLD_Stone1B.SingleOrDefault(p => p.Nomor == txtPLU.Text);
            try
            {
                if (d != null)
                {
                    string Messages = Db.SingleString("EXEC sp_create_plu_reborn '" + txtPLU.Text + "','" + Generate_Nomor(d) + "','DJ','" + App.UserNama + "'", Db.Cnn_DB);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Messages + "')", true);
					//d.Dispose();
                }

                else if (h != null)
                {
                    string Messages = Db.SingleString("EXEC sp_create_plu_reborn '" + txtPLU.Text + "','" + Generate_Nomor(h) + "','PG','" + App.UserNama + "'", Db.Cnn_DB);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Messages + "')", true);
					//h.Dispose();
                }
                else if (f != null)
                {
                    string Messages = Db.SingleString("EXEC sp_create_plu_reborn '" + txtPLU.Text + "','" + Generate_Nomor(d) + "','GJ','" + App.UserNama + "'", Db.Cnn_DB);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Messages + "')", true);
					//d.Dispose();
                }
                else if (g != null)
                {
                    string Messages = Db.SingleString("EXEC sp_create_plu_reborn '" + txtPLU.Text + "','" + Generate_Nomor(d) + "','LD','" + App.UserNama + "'", Db.Cnn_DB);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Messages + "')", true);
					//d.Dispose();
                }
                else
                {
                    //string Messages = Db.SingleString("EXEC sp_create_plu_reborn '" + txtPLU.Text + "','" + Generate_Nomor(d) + "','DJ','" + App.UserID + "'", Db.Cnn_DB);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PLU tidak ditemukan.')", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error : " + ex.Message.ToString() + "')", true);
            }
        }
    }
	
	protected string Generate_Nomor(StockReceiveDJ d)
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
            var s = (from p in db.StockReceiveDJ
                     where p.Tgl.Year == Tahun /*&& p.Tgl.Month == Bulan*/
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(1, 8)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(3, 6)) + 1;
        }
        catch (Exception ex) { }

        string Kode = "D";
        try
        {
            Kode = App.KodeDJ;
        }
        catch (Exception ex) { }

        string PLU = Kode + Tahun.ToString().Remove(0, 2) + Str.KodeFormat(Nomor, 6);

        return PLU;

    }
	
	protected string Generate_Nomor(StockReceivePG d)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

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

    protected class DataPLU
    {
        public string PLU { get; set; }
        public string ImagesHref { get; set; }

        //Characteristic Product
        public string ProductCategoryShow { get; set; }
        public string ProductItemShow { get; set; }
        public string ProductLevelShow { get; set; }
        public string StoneDistributionShow { get; set; }
        public string FrameMaterialShow { get; set; }
        public string FrameFinishingShow { get; set; }
        public string FrameColorShow { get; set; }
        public string ProcessConsShow { get; set; }
        //Weight
        public string GrossWeight { get; set; }
        public string StoneCarat { get; set; }
        public string StoneWeight { get; set; }
        public string NettoWeight { get; set; }
        //Dimensi
        public string DimensiD { get; set; }
        public string DimensiP { get; set; }
        public string DimensiL { get; set; }
        public string DimensiR { get; set; }

        //Characteristic Design
        public string DesignCategory { get; set; }
        public string DesignConcept { get; set; }
        public string DesignProcess { get; set; }
        public string TargetAge { get; set; }
        public string TargerGender { get; set; }
        public string Naming { get; set; }
        public string KeteranganDesign { get; set; }
        public string Grafir { get; set; }
        public string Finishing { get; set; }
    }
}