using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Status_DJForm : System.Web.UI.Page
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
    decimal GT_HargaM = 0;

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);


        if (!IsPostBack)
        {    
            fillHeader();
            App.NumberFormat(susutbongkar);
            App.NumberFormat(susutlebur);
            
        }
        fillForm();
    }

    protected void fillHeader()
    {
        var d = db.StockRetireDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null)
        {
            App.DataNull(this);
            susutbongkar.Text = "0"; 
            susutlebur.Text = "0";
            DLC.AlasanLebur(ddalasan);
        }
        else
        {
            DLC.AlasanLebur(ddalasan);
            petugaslebur.Text = d.Nama;
            keterangan.Text = d.Keterangan;
            ddalasan.SelectedValue = d.Reason.ToString();
            //alasan.Text = d.Alasan;
            //Header
            header_nomor.InnerHtml = d.Nomor;
            if (d.SusutBongkar != null)
            {
                susutbongkar.Text = Str.Number2(d.SusutBongkar);
            }
            else
            {
                susutbongkar.Text = "0";
            }

            if (d.NettoSetelahBongkar != null)
            {
                nettobongkar.Text = Str.Number2(d.NettoSetelahBongkar);
            }
            else
            {
                nettobongkar.Text = d.StockProductDJ.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
            }

            if (d.SusutLebur != null)
            {
                susutlebur.Text = Str.Number2(d.SusutLebur);
            }
            else
            {
                susutlebur.Text = "0";
            }

            if (d.BeratAkhir != null)
            {
                beratakhir.Text = Str.Number2(d.BeratAkhir);
            }
            else
            {
                beratakhir.Text = d.StockProductDJ.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
            }
            //CalcAll();
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.StockRetireDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null)
        {
            App.DataNull(this);
            susutbongkar.Text = "0";
            susutlebur.Text = "0";
        }
        else
        {

            //Trans. Data
            protonomor.InnerHtml = d.StockProductDJ.ProtoNomor;
            productordernomor.InnerHtml = d.StockProductDJ.ProductOrderNomor;
            suppliernama.InnerHtml = d.StockProductDJ.SupplierNama;
            suppliernomor.InnerHtml = d.StockProductDJ.SupplierNomor;
            requestoutletnama.InnerHtml = d.StockProductDJ.RequestOutletNama;
            requestcustomernama.InnerHtml = d.StockProductDJ.RequestCustomerNama;
            humanstocknama.InnerHtml = d.StockProductDJ.HumanStockNama;
            expiredproduct.InnerHtml = Str.Tgl(d.StockProductDJ.ExpiredProduct);
            expiredrequest.InnerHtml = Str.Tgl(d.StockProductDJ.ExpiredRequest);

            ////Product Char.
            var product = d.StockProductDJ.StockProductDJ_CharProduct;
            if (product == null) save.Disabled = true;
            else
            {
                productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";
                productcategory.InnerHtml = product.CharProductCategory.Nama + " (" + product.CharProductCategory.NamaKode + ")";
                productlevel.InnerHtml = product.CharProductLevel.Nama + " (" + product.CharProductLevel.NamaKode + ")";
                stonedist.InnerHtml = product.CharStoneDist.Nama + " (" + product.CharStoneDist.NamaKode + ")";
                framematerial.InnerHtml = product.CharFrameMaterial.Nama + " (" + product.CharFrameMaterial.NamaKode + ")";
                framefinishing.InnerHtml = product.CharFrameFinishing.Nama + " (" + product.CharFrameFinishing.NamaKode + ")";
                framecolor.InnerHtml = product.CharFrameColor.Nama + " (" + product.CharFrameColor.NamaKode + ")";
                processcons.InnerHtml = product.CharProcessCons.Nama + " (" + product.CharProcessCons.NamaKode + ")";

                //Weight
                grossweight.InnerHtml = Str.Number3(product.GrossWeight);
                stonecarat.InnerHtml = Str.Number3(product.StoneCarat);
                stoneweight.InnerHtml = Str.Number3(product.StoneWeight);
                nettoweight.InnerHtml = Str.Number3(product.NettoWeight);

                //Dimensi
                dimensid.InnerHtml = Str.Number2(product.DimensiD);
                dimensip.InnerHtml = Str.Number2(product.DimensiP);
                dimensil.InnerHtml = Str.Number2(product.DimensiL);
                dimensir.InnerHtml = Str.Number2(product.DimensiR);
            }

            //Design Char.
            var design = d.StockProductDJ.StockProductDJ_CharDesign;
            if (design == null) save.Disabled = true;
            else
            {
                designcategory.InnerHtml = design.CharDesignCategory.Nama + " (" + design.CharDesignCategory.NamaKode + ")";
                designconcept.InnerHtml = design.CharDesignConcept.Nama + " (" + design.CharDesignConcept.NamaKode + ")";
                designprocess.InnerHtml = design.CharDesignProcess.Nama + " (" + design.CharDesignProcess.NamaKode + ")";
                targetage.InnerHtml = design.CharTargetAge.Nama + " (" + design.CharTargetAge.NamaKode + ")";
                targetgender.InnerHtml = design.CharTargetGender.Nama + " (" + design.CharTargetGender.NamaKode + ")";

                naming.InnerHtml = design.Naming;
                designket.InnerHtml = designKet(design);
                grafir.InnerHtml = design.Grafir;
                finishing.InnerHtml = designFinishing(d.StockProductDJ);
            }

            //Picture
            Upload.Image(imgpicture, d.StockProductDJ.ImgPicture);

            fillStone(d.StockProductDJ);
            fillProduct(d.StockProductDJ, myID);

            fillMU(d.StockProductDJ);

        }


        //string txt = Request.QueryString["tb"];
        //if (txt != "")
        //{
        //    Page.RegisterStartupScript("af", "<script>$(function() {$(\"#" + txt + "\").focus();});</script>");
        //}
    }

    protected void fillProduct(StockProductDJ ms, int myID)
    {
        var d = ms.StockProductDJ_PricingProduct;
        var x = ms.StockProductDJ_CostingProduct;
        var a = db.StockRetireDJ.SingleOrDefault(p => p.ID == myID);
        if (d != null && x != null)
        {
            netto.InnerHtml = Str.Number2(ms.StockProductDJ_CharProduct.NettoWeight);
            kadar.InnerHtml = Str.Number2(Convert.ToDecimal(a.KadarTukaran));
        }
    }
    protected void fillMU(StockProductDJ ms)
    {
        decimal AllM = 0m;

        var d = ms.StockProductDJ_PricingMU;
        if (d != null)
        {

            hargaidr.InnerHtml = Str.Number0(d.HargaRupiahRound);
        }
    }

    //Stone
    protected void fillStone(StockProductDJ s)
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;

        TableHeaderRow thr = new TableHeaderRow();

        TableHeaderCell th1= new TableHeaderCell();

        th1 = new TableHeaderCell();
        th1.Text = "No";
        th1.RowSpan = 2;
        th1.CssClass = "text-center";
        th1.Width = 40;
        thr.Cells.Add(th1);

        th1 = new TableHeaderCell();
        th1.Text = "Stone";
        th1.RowSpan = 2;
        th1.CssClass = "text-center";
        th1.Width = 200;
        thr.Cells.Add(th1);

        th1 = new TableHeaderCell();
        th1.Text = "Tgl";
        th1.RowSpan = 2;
        th1.CssClass = "text-center";
        th1.Width = 90;
        thr.Cells.Add(th1);

        th1 = new TableHeaderCell();
        th1.Text = "Total Butir";
        th1.CssClass = "text-center";
        th1.Width = 130;
        thr.Cells.Add(th1);

        th1 = new TableHeaderCell();
        th1.Text = "Total Butir Diterima";
        th1.CssClass = "text-center";
        th1.Width = 135;
        thr.Cells.Add(th1);

        th1 = new TableHeaderCell();
        th1.Text = "Total Butir Rusak";
        th1.CssClass = "text-center";
        th1.Width = 130;
        thr.Cells.Add(th1);

        th1 = new TableHeaderCell();
        th1.Text = "Total Butir Selisih";
        th1.CssClass = "text-center";
        th1.Width = 130;
        thr.Cells.Add(th1);

        th1 = new TableHeaderCell();
        th1.Text = "Keterangan";
        th1.RowSpan = 2;
        th1.CssClass = "text-center";
        th1.Width = 250;
        thr.Cells.Add(th1);

        tb.Rows.Add(thr);

        TableHeaderRow thr2 = new TableHeaderRow();

        TableHeaderCell thc1 = new TableHeaderCell();

        thc1 = new TableHeaderCell();
        thc1.Text = "Total Carat";
        thc1.CssClass = "text-center";
        thr2.Cells.Add(thc1);

        thc1 = new TableHeaderCell();
        thc1.Text = "Total Carat Diterima";
        thc1.CssClass = "text-center";
        thr2.Cells.Add(thc1);

        thc1 = new TableHeaderCell();
        thc1.Text = "Total Carat Rusak";
        thc1.CssClass = "text-center";
        thr2.Cells.Add(thc1);

        thc1 = new TableHeaderCell();
        thc1.Text = "Total Carat Selisih";
        thc1.CssClass = "text-center";
        thr2.Cells.Add(thc1);

        tb.Rows.Add(thr2);

        //Stone
        fillStone1A(0, ref GT_Butir, ref GT_Carat, s);
        fillStone1B(1, ref GT_Butir, ref GT_Carat, s);
        fillStone2(2, ref GT_Butir, ref GT_Carat, s);
        fillStone3(3, ref GT_Butir, ref GT_Carat, s);
        fillStone4(4, ref GT_Butir, ref GT_Carat, s);
        fillStone5(5, ref GT_Butir, ref GT_Carat, s);

        //Grand-Total
        //Sub_Total(GT_Butir, GT_Carat, true);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ s)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1A
                    where p.IDForm == s.ID
                    orderby p.Stone1A.Nama
                    select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {

            var cek = r.StockRetireDJ_Stone1A.SingleOrDefault(p => p.IDStone == r.ID);

            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1A.Nama + " " ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            //td.Text = Str.Integer(r.TotalButir);
            td.Text = Str.Integer(r.TotalButir) + "<br />" + Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number3(r.TotalCarat);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s1A_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_terima.Text = Str.Integer(cek.TotalTerima);
            }
            else
            { txt_terima.Text = Str.Integer(r.TotalButir); }  
                              
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s1A_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            if (cek != null)
            {
                txt_cterima.Text = Str.Number3(cek.CaratTerima);
            }
            else
            {
                txt_cterima.Text = Str.Number3(r.TotalCarat);
            }
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S1A_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_cterima);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s1A_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_rusak.Text = Str.Integer(cek.TotalRusak);
            }
            else { txt_rusak.Text = "0"; }
            
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S1A_TextChanged);

            TextBox txt_crusak = new TextBox();
            txt_crusak.ID = "s1A_crusak" + index.ToString();
            txt_crusak.Attributes["style"] = "width: 50px;";
            txt_crusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_crusak.Text = Str.Number3(cek.CaratRusak);
            }
            else { txt_crusak.Text = "0"; }
            
            txt_crusak.AutoPostBack = true;
            txt_crusak.TextChanged += new EventHandler(S1A_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_crusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);



            //td = new TableCell();
            //td.Controls.Add(txt_crusak);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            //TextBox txt_selisih = new TextBox();
            //txt_selisih.ID = "s1A_selisih" + index.ToString();
            //txt_selisih.Attributes["style"] = "width: 50px;";
            //txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            Label lb_selisih = new Label();
            lb_selisih.ID = "s1A_selisih" + index.ToString();
            lb_selisih.Attributes["style"] = "width: 50px;";
            lb_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            { lb_selisih.Text = "<span>"+ Str.Integer(cek.TotalSelisih) + "<br /> "+ Str.Number3(cek.CaratSelisih) +"</span>";
            }
            else
            { lb_selisih.Text = "<span>0" + "<br />0 </span>"; }

            HiddenField hf = new HiddenField();
            hf.ID = "hfs1A" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            //HiddenField fl = new HiddenField();
            //fl.ID = "fl1A";
            //fl.Value = "Stone1A";
            //fl.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(lb_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            //td.Controls.Add(fl);
            tr.Cells.Add(td);


            TextBox txket = new TextBox();
            txket.ID = "s1A_ket" + index.ToString();
            txket.Attributes["style"] = "width: 250px;";
            txket.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txket.AutoPostBack = true;
            //txket.TextChanged += new EventHandler(S1A_TextChanged);
            if (cek != null)
            {
                txket.Text = cek.Keterangan;
            }
            else { txket.Text = ""; }

            td = new TableCell();
            td.Controls.Add(txket);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            //Sub_Total(ST_Butir, ST_Carat, false);
        }
    }


    protected void S1A_TextChanged(object sender, EventArgs e)
    {
        //string txt = ((TextBox)sender).ClientID;
        //TextBox1.Focus();
        //if (txt != "")
        //{
        //    Page.RegisterClientScriptBlock("af", "<script>$(function() {$(\"#" + txt + "\").focus();});</script>");
        //}


        TextBox tx = ((TextBox)sender);
        tx.Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";
        //string f = "";

        foreach (TableRow row in tb.Rows )
        {            
            //f = ((HiddenField)row.FindControl("fl1A")).Value;
            //if (f == "Stone1A")
            //{

                int Butir = 0;
                int Terima = 0;
                int Rusak = 0;
                int Selisih = 0;
                decimal Carat = 0m;
                decimal Cterima = 0m;
                decimal Crusak = 0m;
                decimal Cselisih = 0m;
                string tket = ""; 

            //lit_test.Text += "<br />______xxx_" + index.ToString();

            if (((HiddenField)row.FindControl("hfs1A" + index.ToString()))!= null)
            {
                hf = ((HiddenField)row.FindControl("hfs1A" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s1A_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s1A_rusak" + index.ToString());
            //txt_selisih = (TextBox)row.FindControl("s1A_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s1A_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s1A_crusak" + index.ToString());
            lb_selisih = (Label)row.FindControl("s1A_selisih" + index.ToString());
            txket = (TextBox)row.FindControl("s1A_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone1A.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {


                    Butir = Convert.ToInt32(d.TotalButir);
                    Terima = Convert.ToInt32(txt_terima.Text);
                    Rusak = Convert.ToInt32(txt_rusak.Text);
                    Selisih = Butir - Terima - Rusak;
                    //txt_selisih.Text = Selisih.ToString();
                    Carat = Convert.ToDecimal(d.TotalCarat);
                    Cterima = Convert.ToDecimal(txt_cterima.Text);
                    Crusak = Convert.ToDecimal(txt_crusak.Text);
                    Cselisih = Carat - Cterima - Crusak;
                    lb_selisih.Text = "<span>" + Str.Integer(Selisih) + "<br />" + Str.Number3(Cselisih) + "</span>";
                    tket = txket.Text;
                    txket.Text = tket;

                }
            
            index++;
            }
            //}
        }


        
    }

    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ s)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1B
                 where p.IDForm == s.ID
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            var cek = r.StockRetireDJ_Stone1B.SingleOrDefault(p => p.IDStone == r.ID);

            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1B.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            //td.Text = Str.Integer(r.TotalButir);
            td.Text = Str.Integer(r.TotalButir) + "<br />" + Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number3(r.TotalCarat);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s1B_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_terima.Text = Str.Integer(cek.TotalTerima);
            }
            else { txt_terima.Text = Str.Integer(r.TotalButir); ; }
            //txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S1B_TextChanged);

            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s1B_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_cterima.Text = Str.Number3(cek.CaratTerima);
            }
            else { txt_cterima.Text = Str.Number3(r.TotalCarat); }
            //txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S1B_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_cterima);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s1B_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_rusak.Text = Str.Integer(cek.TotalRusak);
            }
            else { txt_rusak.Text = "0"; }
            //txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S1B_TextChanged);

            TextBox txt_crusak = new TextBox();
            txt_crusak.ID = "s1B_crusak" + index.ToString();
            txt_crusak.Attributes["style"] = "width: 50px;";
            txt_crusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_crusak.Text = Str.Number3(cek.CaratRusak);
            }
            else { txt_crusak.Text = "0"; }
            //txt_crusak.Text = "0";
            txt_crusak.AutoPostBack = true;
            txt_crusak.TextChanged += new EventHandler(S1B_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_crusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);


            //td = new TableCell();
            //td.Controls.Add(txt_crusak);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            //TextBox txt_selisih = new TextBox();
            //txt_selisih.ID = "s1B_selisih" + index.ToString();
            //txt_selisih.Attributes["style"] = "width: 50px;";
            //txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            Label lb_selisih = new Label();
            lb_selisih.ID = "s1B_selisih" + index.ToString();
            lb_selisih.Attributes["style"] = "width: 50px;";
            lb_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                lb_selisih.Text = "<span>" + Str.Integer(cek.TotalSelisih) + "<br /> " + Str.Number3(cek.CaratSelisih) + "</span>";
            }
            else
            { lb_selisih.Text = "<span>0" + "<br />0 </span>"; }
            //lb_selisih.Text = "<span>0" + "<br />0 </span>";
            HiddenField hf = new HiddenField();
            hf.ID = "hfs1B" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(lb_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            TextBox txket = new TextBox();
            txket.ID = "s1B_ket" + index.ToString();
            txket.Attributes["style"] = "width: 250px;";
            txket.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txket.AutoPostBack = true;
            //txket.TextChanged += new EventHandler(S1B_TextChanged);
            if (cek != null)
            {
                txket.Text = cek.Keterangan;
            }
            else { txket.Text = ""; }

            td = new TableCell();
            td.Controls.Add(txket);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            //Sub_Total(ST_Butir, ST_Carat, false);
        }
    }

    protected void S1B_TextChanged(object sender, EventArgs e)
    {
        TextBox tx = ((TextBox)sender);
        tx.Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

                if (((HiddenField)row.FindControl("hfs1B" + index.ToString())) != null)
                {
                    hf = ((HiddenField)row.FindControl("hfs1B" + index.ToString())).Value;
                }
                else
                {
                    hf = "";
                }
                txt_terima = (TextBox)row.FindControl("s1B_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s1B_rusak" + index.ToString());
                //txt_selisih = (TextBox)row.FindControl("s1B_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s1B_cterima" + index.ToString());
                txt_crusak = (TextBox)row.FindControl("s1B_crusak" + index.ToString());
                lb_selisih = (Label)row.FindControl("s1B_selisih" + index.ToString());
                txket = (TextBox)row.FindControl("s1B_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone1B.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {

                    Butir = Convert.ToInt32(d.TotalButir);
                    Terima = Convert.ToInt32(txt_terima.Text);
                    Rusak = Convert.ToInt32(txt_rusak.Text);
                    Selisih = Butir - Terima - Rusak;
                    //txt_selisih.Text = Selisih.ToString();
                    Carat = Convert.ToDecimal(d.TotalCarat);
                    Cterima = Convert.ToDecimal(txt_cterima.Text);
                    Crusak = Convert.ToDecimal(txt_crusak.Text);
                    Cselisih = Carat - Cterima - Crusak;
                    lb_selisih.Text = "<span>" + Str.Integer(Selisih) + "<br />" + Str.Number3(Cselisih) + "</span>";
                    tket = txket.Text;
                    txket.Text = tket;
                }
                index++;
            }
        }

    }

    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ s)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone2
                 where p.IDForm == s.ID
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            var cek = r.StockRetireDJ_Stone2.SingleOrDefault(p => p.IDStone == r.ID);
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone2.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            // td.Text = Str.Integer(r.TotalButir);
            td.Text = Str.Integer(r.TotalButir) + "<br />" + Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number3(r.TotalCarat);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s2_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_terima.Text = Str.Integer(cek.TotalTerima);
            }
            else { txt_terima.Text = Str.Integer(r.TotalButir); }
            //txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S2_TextChanged);

            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s2_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_cterima.Text = Str.Number3(cek.CaratTerima); ;
            }
            else { txt_cterima.Text = Str.Number3(r.TotalCarat); }
            //txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S2_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_cterima);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s2_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_rusak.Text = Str.Integer(cek.TotalRusak);
            }
            else { txt_rusak.Text = "0"; }
            //txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S2_TextChanged);

            TextBox txt_crusak = new TextBox();
            txt_crusak.ID = "s2_crusak" + index.ToString();
            txt_crusak.Attributes["style"] = "width: 50px;";
            txt_crusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_crusak.Text = Str.Number3(cek.CaratRusak);
            }
            else { txt_crusak.Text = "0"; }
            //txt_crusak.Text = "0";
            txt_crusak.AutoPostBack = true;
            txt_crusak.TextChanged += new EventHandler(S2_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_crusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_crusak);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            //TextBox txt_selisih = new TextBox();
            //txt_selisih.ID = "s2_selisih" + index.ToString();
            //txt_selisih.Attributes["style"] = "width: 50px;";
            //txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);


            Label lb_selisih = new Label();
            lb_selisih.ID = "s2_selisih" + index.ToString();
            lb_selisih.Attributes["style"] = "width: 50px;";
            lb_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                lb_selisih.Text = "<span>" + Str.Integer(cek.TotalSelisih) + "<br /> " + Str.Number3(cek.CaratSelisih) + "</span>";
            }
            else
            { lb_selisih.Text = "<span>0" + "<br />0 </span>"; }
            //lb_selisih.Text = "<span>0"  + "<br />0 </span>";

            HiddenField hf = new HiddenField();
            hf.ID = "hfs2" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(lb_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            TextBox txket = new TextBox();
            txket.ID = "s2_ket" + index.ToString();
            txket.Attributes["style"] = "width: 250px;";
            txket.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txket.AutoPostBack = true;
            //txket.TextChanged += new EventHandler(S2_TextChanged);
            if (cek != null)
            {
                txket.Text = cek.Keterangan;
            }
            else { txket.Text = ""; }

            td = new TableCell();
            td.Controls.Add(txket);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            //Sub_Total(ST_Butir, ST_Carat, false);
        }
    }

    protected void S2_TextChanged(object sender, EventArgs e)
    {
        TextBox tx = ((TextBox)sender);
        tx.Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

            if (((HiddenField)row.FindControl("hfs2" + index.ToString())) != null)
            {
                hf = ((HiddenField)row.FindControl("hfs2" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s2_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s2_rusak" + index.ToString());
            //txt_selisih = (TextBox)row.FindControl("s2_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s2_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s2_crusak" + index.ToString());
            lb_selisih = (Label)row.FindControl("s2_selisih" + index.ToString());
            txket = (TextBox)row.FindControl("s2_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone2.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    Butir = Convert.ToInt32(d.TotalButir);
                    Terima = Convert.ToInt32(txt_terima.Text);
                    Rusak = Convert.ToInt32(txt_rusak.Text);
                    Selisih = Butir - Terima - Rusak;
                    //txt_selisih.Text = Selisih.ToString();
                    Carat = Convert.ToDecimal(d.TotalCarat);
                    Cterima = Convert.ToDecimal(txt_cterima.Text);
                    Crusak = Convert.ToDecimal(txt_crusak.Text);
                    Cselisih = Carat - Cterima - Crusak;
                    lb_selisih.Text = "<span>" + Str.Integer(Selisih) + "<br />" + Str.Number3(Cselisih) + "</span>";
                    tket = txket.Text;
                    txket.Text = tket;
                }


                index++;
            }
        }
    }

    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ s)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone3
                 where p.IDForm == s.ID
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            var cek = r.StockRetireDJ_Stone3.SingleOrDefault(p => p.IDStone == r.ID);

            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone3.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            //td.Text = Str.Integer(r.TotalButir);
            td.Text = Str.Integer(r.TotalButir) + "<br />" + Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number3(r.TotalCarat);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s3_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_terima.Text = Str.Integer(cek.TotalTerima);
            }
            else { txt_terima.Text = Str.Integer(r.TotalButir); }

            //txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S3_TextChanged);

            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s3_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_cterima.Text = Str.Number3(cek.CaratTerima);
            }
            else { txt_cterima.Text = Str.Number3(r.TotalCarat); }
            //txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S3_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_cterima);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s3_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_rusak.Text = Str.Integer(cek.TotalRusak);
            }
            else { txt_rusak.Text = "0"; }

            //txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S3_TextChanged);

            TextBox txt_crusak = new TextBox();
            txt_crusak.ID = "s3_crusak" + index.ToString();
            txt_crusak.Attributes["style"] = "width: 50px;";
            txt_crusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_rusak.Text = Str.Number3(cek.CaratRusak);
            }
            else { txt_rusak.Text = "0"; }
            //txt_crusak.Text = "0";
            txt_crusak.AutoPostBack = true;
            txt_crusak.TextChanged += new EventHandler(S3_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_crusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //TextBox txt_selisih = new TextBox();
            //txt_selisih.ID = "s3_selisih" + index.ToString();
            //txt_selisih.Attributes["style"] = "width: 50px;";
            //txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            Label lb_selisih = new Label();
            lb_selisih.ID = "s3_selisih" + index.ToString();
            lb_selisih.Attributes["style"] = "width: 50px;";
            lb_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                lb_selisih.Text = "<span>" + Str.Integer(cek.TotalSelisih) + "<br /> " + Str.Number3(cek.CaratSelisih) + "</span>";
            }
            else
            { lb_selisih.Text = "<span>0" + "<br />0 </span>"; }
            //lb_selisih.Text = "<span>0"  + "<br />0 </span>";

            HiddenField hf = new HiddenField();
            hf.ID = "hfs3" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(lb_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            TextBox txket = new TextBox();
            txket.ID = "s3_ket" + index.ToString();
            txket.Attributes["style"] = "width: 250px;";
            txket.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txket.AutoPostBack = true;
            //txket.TextChanged += new EventHandler(S3_TextChanged);
            if (cek != null)
            {
                txket.Text = cek.Keterangan;
            }
            else { txket.Text = ""; }

            td = new TableCell();
            td.Controls.Add(txket);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);
            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            //Sub_Total(ST_Butir, ST_Carat, false);
        }
    }

    protected void S3_TextChanged(object sender, EventArgs e)
    {
        TextBox tx = ((TextBox)sender);
        tx.Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

                if (((HiddenField)row.FindControl("hfs3" + index.ToString())) != null)
                {
                    hf = ((HiddenField)row.FindControl("hfs3" + index.ToString())).Value;
                }
                else
                {
                    hf = "";
                }
                txt_terima = (TextBox)row.FindControl("s3_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s3_rusak" + index.ToString());
                //txt_selisih = (TextBox)row.FindControl("s3_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s3_cterima" + index.ToString());
                txt_crusak = (TextBox)row.FindControl("s3_crusak" + index.ToString());
                lb_selisih = (Label)row.FindControl("s3_selisih" + index.ToString());
                txket = (TextBox)row.FindControl("s3_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone3.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    Butir = Convert.ToInt32(d.TotalButir);
                    Terima = Convert.ToInt32(txt_terima.Text);
                    Rusak = Convert.ToInt32(txt_rusak.Text);
                    Selisih = Butir - Terima - Rusak;
                    //txt_selisih.Text = Selisih.ToString();
                    Carat = Convert.ToDecimal(d.TotalCarat);
                    Cterima = Convert.ToDecimal(txt_cterima.Text);
                    Crusak = Convert.ToDecimal(txt_crusak.Text);
                    Cselisih = Carat - Cterima - Crusak;
                    lb_selisih.Text = "<span>" + Str.Integer(Selisih) + "<br />" + Str.Number3(Cselisih) + "</span>";
                    tket = txket.Text;
                    txket.Text = tket;
                }

                index++;
            }
        }
    }

    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ s)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone4
                 where p.IDForm == s.ID
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            var cek = r.StockRetireDJ_Stone4.SingleOrDefault(p => p.IDStone == r.ID);
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone4.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            //td.Text = Str.Integer(r.TotalButir);
            td.Text = Str.Integer(r.TotalButir) + "<br />" + Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number3(r.TotalCarat);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s4_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_terima.Text = Str.Integer(cek.TotalTerima);
            }
            else { txt_terima.Text = Str.Integer(r.TotalButir); }
            //txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S4_TextChanged);

            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s4_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_cterima.Text = Str.Number3(cek.CaratTerima);
            }
            else { txt_cterima.Text = Str.Number3(r.TotalCarat); }
            //txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S4_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_cterima);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s4_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_rusak.Text = Str.Integer(cek.TotalRusak);
            }
            else { txt_rusak.Text = "0"; }
            //txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S4_TextChanged);

            TextBox txt_crusak = new TextBox();
            txt_crusak.ID = "s4_crusak" + index.ToString();
            txt_crusak.Attributes["style"] = "width: 50px;";
            txt_crusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_crusak.Text = Str.Number3(cek.CaratRusak);
            }
            else { txt_crusak.Text = "0"; }
            //txt_crusak.Text = "0";
            txt_crusak.AutoPostBack = true;
            txt_crusak.TextChanged += new EventHandler(S4_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_crusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_crusak);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            //TextBox txt_selisih = new TextBox();
            //txt_selisih.ID = "s4_selisih" + index.ToString();
            //txt_selisih.Attributes["style"] = "width: 50px;";
            //txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            Label lb_selisih = new Label();
            lb_selisih.ID = "s4_selisih" + index.ToString();
            lb_selisih.Attributes["style"] = "width: 50px;";
            lb_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                lb_selisih.Text = "<span>" + Str.Integer(cek.TotalSelisih) + "<br /> " + Str.Number3(cek.CaratSelisih) + "</span>";
            }
            else
            { lb_selisih.Text = "<span>0" + "<br />0 </span>"; }
            //lb_selisih.Text = "<span>0" + "<br />0 </span>";

            HiddenField hf = new HiddenField();
            hf.ID = "hfs4" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(lb_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            TextBox txket = new TextBox();
            txket.ID = "s4_ket" + index.ToString();
            txket.Attributes["style"] = "width: 250px;";
            txket.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txket.AutoPostBack = true;
            //txket.TextChanged += new EventHandler(S4_TextChanged);
            if (cek != null)
            {
                txket.Text = cek.Keterangan;
            }
            else { txket.Text = ""; }


            td = new TableCell();
            td.Controls.Add(txket);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            //Sub_Total(ST_Butir, ST_Carat, false);
        }
    }

    protected void S4_TextChanged(object sender, EventArgs e)
    {
        TextBox tx = ((TextBox)sender);
        tx.Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

            if (((HiddenField)row.FindControl("hfs4" + index.ToString())) != null)
            {
                hf = ((HiddenField)row.FindControl("hfs4" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s4_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s4_rusak" + index.ToString());
            //txt_selisih = (TextBox)row.FindControl("s4_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s4_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s4_crusak" + index.ToString());
            lb_selisih = (Label)row.FindControl("s4_selisih" + index.ToString());
            txket = (TextBox)row.FindControl("s4_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone4.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    Butir = Convert.ToInt32(d.TotalButir);
                    Terima = Convert.ToInt32(txt_terima.Text);
                    Rusak = Convert.ToInt32(txt_rusak.Text);
                    Selisih = Butir - Terima - Rusak;
                    //txt_selisih.Text = Selisih.ToString();
                    Carat = Convert.ToDecimal(d.TotalCarat);
                    Cterima = Convert.ToDecimal(txt_cterima.Text);
                    Crusak = Convert.ToDecimal(txt_crusak.Text);
                    Cselisih = Carat - Cterima - Crusak;
                    lb_selisih.Text = "<span>" + Str.Integer(Selisih) + "<br />" + Str.Number3(Cselisih) + "</span>";
                    tket = txket.Text;
                    txket.Text = tket;
                }

                index++;
            }
        }
    }

    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ s)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone5
                 where p.IDForm == s.ID
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            var cek = r.StockRetireDJ_Stone5.SingleOrDefault(p => p.IDStone == r.ID);
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone5.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            //td.Text = Str.Integer(r.TotalButir);
            td.Text = Str.Integer(r.TotalButir) + "<br />" + Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number3(r.TotalCarat);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s5_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_terima.Text = Str.Integer(cek.TotalTerima);
            }
            else { txt_terima.Text = Str.Integer(r.TotalButir); }
            //txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S5_TextChanged);

            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s5_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_cterima.Text = Str.Number3(cek.CaratTerima);
            }
            else { txt_cterima.Text = Str.Number3(r.TotalCarat); }
            //txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S5_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_cterima);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s5_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_rusak.Text = Str.Integer(cek.TotalRusak);
            }
            else { txt_rusak.Text = "0"; }
            //txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S5_TextChanged);

            TextBox txt_crusak = new TextBox();
            txt_crusak.ID = "s5_crusak" + index.ToString();
            txt_crusak.Attributes["style"] = "width: 50px;";
            txt_crusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                txt_crusak.Text = Str.Number3(cek.CaratRusak);
            }
            else { txt_crusak.Text = "0"; }
            //txt_crusak.Text = "0";
            txt_crusak.AutoPostBack = true;
            txt_crusak.TextChanged += new EventHandler(S5_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.Controls.Add(new LiteralControl("<br />"));
            td.Controls.Add(txt_crusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Controls.Add(txt_crusak);
            //td.HorizontalAlign = HorizontalAlign.Center;
            //tr.Cells.Add(td);

            //TextBox txt_selisih = new TextBox();
            //txt_selisih.ID = "s5_selisih" + index.ToString();
            //txt_selisih.Attributes["style"] = "width: 50px;";
            //txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            Label lb_selisih = new Label();
            lb_selisih.ID = "s5_selisih" + index.ToString();
            lb_selisih.Attributes["style"] = "width: 50px;";
            lb_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (cek != null)
            {
                lb_selisih.Text = "<span>" + Str.Integer(cek.TotalSelisih) + "<br /> " + Str.Number3(cek.CaratSelisih) + "</span>";
            }
            else
            { lb_selisih.Text = "<span>0" + "<br />0 </span>"; }
            //lb_selisih.Text = "<span>0"  + "<br />0 </span>";

            HiddenField hf = new HiddenField();
            hf.ID = "hfs5" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(lb_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            TextBox txket = new TextBox();
            txket.ID = "s5_ket" + index.ToString();
            txket.Attributes["style"] = "width: 250px;";
            txket.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txket.AutoPostBack = true;
            //txket.TextChanged += new EventHandler(S5_TextChanged);
            if (cek != null)
            {
                lb_selisih.Text = "<span>" + Str.Integer(cek.TotalSelisih) + "<br /> " + Str.Number3(cek.CaratSelisih) + "</span>";
            }
            else
            { lb_selisih.Text = "<span>0" + "<br />0 </span>"; }

            td = new TableCell();
            td.Controls.Add(txket);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            //Sub_Total(ST_Butir, ST_Carat, false);
        }

    }

    protected void S5_TextChanged(object sender, EventArgs e)
    {
        TextBox tx = ((TextBox)sender);
        tx.Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

                if (((HiddenField)row.FindControl("hfs5" + index.ToString())) != null)
                {
                    hf = ((HiddenField)row.FindControl("hfs5" + index.ToString())).Value;
                }
                else
                {
                    hf = "";
                }
                txt_terima = (TextBox)row.FindControl("s5_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s5_rusak" + index.ToString());
                //txt_selisih = (TextBox)row.FindControl("s5_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s5_cterima" + index.ToString());
                txt_crusak = (TextBox)row.FindControl("s5_crusak" + index.ToString());
                lb_selisih = (Label)row.FindControl("s5_selisih" + index.ToString());
                txket = (TextBox)row.FindControl("s5_ket" + index.ToString());

            var d = db.StockProductDJ_Stone5.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    Butir = Convert.ToInt32(d.TotalButir);
                    Terima = Convert.ToInt32(txt_terima.Text);
                    Rusak = Convert.ToInt32(txt_rusak.Text);
                    Selisih = Butir - Terima - Rusak;
                    //txt_selisih.Text = Selisih.ToString();
                    Carat = Convert.ToDecimal(d.TotalCarat);
                    Cterima = Convert.ToDecimal(txt_cterima.Text);
                    Crusak = Convert.ToDecimal(txt_crusak.Text);
                    Cselisih = Carat - Cterima - Crusak;
                    lb_selisih.Text = "<span>" + Str.Integer(Selisih) + "<br />" + Str.Number3(Cselisih) + "</span>";
                    tket = txket.Text;
                    txket.Text = tket;
                }

            index++;
        }
    }

    //Design Info
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
            x += "Special Design";
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

    protected void Sub_Header(int StoneIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.StoneIndex(StoneIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 3;
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(TotalButir);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number3(TotalCarat);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        if (susutlebur.Text != null && susutlebur.Text != "" && susutbongkar.Text != null && susutbongkar.Text !="")
        {
            var d = db.StockRetireDJ.SingleOrDefault(p => p.ID == myID);
            if (d == null) App.DataNull(this);
            else
            {
                //if (d.Status == 3)
                //{
                    StockLedgerDJ l = new StockLedgerDJ();
                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out , 0 for register retire
                    int Remark = 20; // See Library DB - Ledger
                    string Keterangan = d.Nomor; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (d.StockProductDJ.StockActualDJ.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockProductDJ.StockActualDJ.IDLokasi, Db.Cnn_DB);
                    else if (d.StockProductDJ.StockActualDJ.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockProductDJ.StockActualDJ.IDLokasi, Db.Cnn_DB);

                    //Input
                    l.IDProduct = d.ID;
                    l.TipeLokasi = d.StockProductDJ.StockActualDJ.TipeLokasi;
                    l.IDLokasi = d.StockProductDJ.StockActualDJ.IDLokasi;
                    l.NamaLokasi = Lokasi;
                    l.Value = Value;
                    l.Remark = Remark;
                    l.Keterangan = d.Nomor;

                    //User-Info
                    l.Operator = App.UserNama;
                    l.OperatorTgl = DateTime.Now;

                    //Commit
                    db.StockLedgerDJ.InsertOnSubmit(l);
                    db.SubmitChanges();
                //}

                saveStone(d.StockProductDJ);

                d.NettoWeight = d.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;
                d.SusutBongkar = Convert.ToDecimal(susutbongkar.Text);
                d.NettoSetelahBongkar = Convert.ToDecimal(nettobongkar.Text);
                d.SusutLebur = Convert.ToDecimal(susutlebur.Text);
                d.BeratAkhir = Convert.ToDecimal(beratakhir.Text);
                d.Nama = petugaslebur.Text;
                //d.Alasan = 
                d.Reason = Convert.ToInt32(ddalasan.SelectedValue);

                d.Alasan = ddalasan.SelectedItem.Text;
                //d.Alasan = alasan.Text;
                d.Keterangan = keterangan.Text;
                d.Status = 4; // status after answer = 4
                db.SubmitChanges();

                //Redirect
                App.ReloadClose(this);
            }
        }
        else
        {
            susutbongkar.Focus();
            if (susutbongkar.Text == null || susutbongkar.Text == "")
            {
                lbbongkar.Visible = true;
            }
            if (susutlebur.Text == null || susutlebur.Text == "")
            {
                lblebur.Visible = true;
            }
        }
    }

    protected void saveStone(StockProductDJ d)
    {
        saveStone1A(d);
        saveStone1B(d);
        saveStone2(d);
        saveStone3(d);
        saveStone4(d);
        saveStone5(d);
    }

    protected void saveStone1A(StockProductDJ s)
    {
        int index = 1;

        Db.Execute("DELETE FROM StockRetireDJ_Stone1A WHERE IDForm=" + myID, Db.Cnn_DB);

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

            if (((HiddenField)row.FindControl("hfs1a" + index.ToString())) != null)
            {
                hf = ((HiddenField)row.FindControl("hfs1a" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s1A_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s1A_rusak" + index.ToString());
            //txt_selisih = (TextBox)row.FindControl("s1A_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s1A_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s1A_crusak" + index.ToString());
            txket = (TextBox)row.FindControl("s1A_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone1A.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        Butir = Convert.ToInt32(d.TotalButir);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        //txt_selisih.Text = Selisih.ToString();
                        Carat = Convert.ToDecimal(d.TotalCarat);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Crusak = Convert.ToDecimal(txt_crusak.Text);
                        Cselisih = d.TotalCarat - Cterima - Crusak;
                        tket = txket.Text;

                        StockRetireDJ_Stone1A j = new StockRetireDJ_Stone1A();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratButir = d.TotalCarat;
                        j.CaratTerima = Cterima;
                        j.CaratRusak = Crusak;
                        j.CaratSelisih = Cselisih;
                        j.Keterangan = tket;

                        db.StockRetireDJ_Stone1A.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }

                index++;
            }
        }
    }

    protected void saveStone1B(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone1B WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

            if (((HiddenField)row.FindControl("hfs1B" + index.ToString())) != null)
            {
                hf = ((HiddenField)row.FindControl("hfs1B" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s1B_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s1B_rusak" + index.ToString());
            //txt_selisih = (TextBox)row.FindControl("s1B_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s1B_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s1B_crusak" + index.ToString());
            txket = (TextBox)row.FindControl("s1B_ket" + index.ToString());

            if(hf != "")
            { 
                var d = db.StockProductDJ_Stone1B.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        Butir = Convert.ToInt32(d.TotalButir);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        //txt_selisih.Text = Selisih.ToString();
                        Carat = Convert.ToDecimal(d.TotalCarat);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Crusak = Convert.ToDecimal(txt_crusak.Text);
                        Cselisih = d.TotalCarat - Cterima - Crusak;
                        tket = txket.Text;

                        StockRetireDJ_Stone1B j = new StockRetireDJ_Stone1B();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratButir = d.TotalCarat;
                        j.CaratTerima = Cterima;
                        j.CaratRusak = Crusak;
                        j.CaratSelisih = Cselisih;
                        j.Keterangan = tket;
                        db.StockRetireDJ_Stone1B.InsertOnSubmit(j);
                        db.SubmitChanges();
                    }
                    catch (Exception ex) { }
                }

            index++;
            }
        }
    }

    protected void saveStone2(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone2 WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

            if (((HiddenField)row.FindControl("hfs2" + index.ToString())) != null)
            {
                hf = ((HiddenField)row.FindControl("hfs2" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s2_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s2_rusak" + index.ToString());
            // txt_selisih = (TextBox)row.FindControl("s2_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s2_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s2_crusak" + index.ToString());
            txket = (TextBox)row.FindControl("s2_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone2.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        Butir = Convert.ToInt32(d.TotalButir);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        //txt_selisih.Text = Selisih.ToString();
                        Carat = Convert.ToDecimal(d.TotalCarat);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Crusak = Convert.ToDecimal(txt_crusak.Text);
                        Cselisih = d.TotalCarat - Cterima - Crusak;
                        tket = txket.Text;
                        StockRetireDJ_Stone2 j = new StockRetireDJ_Stone2();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratButir = d.TotalCarat;
                        j.CaratTerima = Cterima;
                        j.CaratRusak = Crusak;
                        j.CaratSelisih = Cselisih;
                        j.Keterangan = tket;
                        db.StockRetireDJ_Stone2.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }
                index++;
            }
        }
    }

    protected void saveStone3(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone3 WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

            if (((HiddenField)row.FindControl("hfs3" + index.ToString())) != null)
            {
                hf = ((HiddenField)row.FindControl("hfs3" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s3_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s3_rusak" + index.ToString());
            //txt_selisih = (TextBox)row.FindControl("s3_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s3_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s3_crusak" + index.ToString());
            txket = (TextBox)row.FindControl("s3_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone3.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        Butir = Convert.ToInt32(d.TotalButir);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        //txt_selisih.Text = Selisih.ToString();
                        Carat = Convert.ToDecimal(d.TotalCarat);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Crusak = Convert.ToDecimal(txt_crusak.Text);
                        Cselisih = d.TotalCarat - Cterima - Crusak;
                        tket = txket.Text;

                        StockRetireDJ_Stone3 j = new StockRetireDJ_Stone3();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratButir = d.TotalCarat;
                        j.CaratTerima = Cterima;
                        j.CaratRusak = Crusak;
                        j.CaratSelisih = Cselisih;
                        j.Keterangan = tket;
                        db.StockRetireDJ_Stone3.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }

                index++;
            }
        }
    }

    protected void saveStone4(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone4 WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";

            if (((HiddenField)row.FindControl("hfs4" + index.ToString())) != null)
            {
                hf = ((HiddenField)row.FindControl("hfs4" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s4_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s4_rusak" + index.ToString());
            //txt_selisih = (TextBox)row.FindControl("s4_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s4_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s4_crusak" + index.ToString());
            txket = (TextBox)row.FindControl("s4_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone4.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        Butir = Convert.ToInt32(d.TotalButir);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        //txt_selisih.Text = Selisih.ToString();
                        Carat = Convert.ToDecimal(d.TotalCarat);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Crusak = Convert.ToDecimal(txt_crusak.Text);
                        Cselisih = d.TotalCarat - Cterima - Crusak;
                        tket = txket.Text;

                        StockRetireDJ_Stone4 j = new StockRetireDJ_Stone4();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratButir = d.TotalCarat;
                        j.CaratTerima = Cterima;
                        j.CaratRusak = Crusak;
                        j.CaratSelisih = Cselisih;
                        j.Keterangan = tket;
                        db.StockRetireDJ_Stone4.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }
                index++;
            }
        }
    }

    protected void saveStone5(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone5 WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        //TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        TextBox txt_crusak = new TextBox();
        Label lb_selisih = new Label();
        TextBox txket = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Carat = 0m;
            decimal Cterima = 0m;
            decimal Crusak = 0m;
            decimal Cselisih = 0m;
            string tket = "";


            if (((HiddenField)row.FindControl("hfs5" + index.ToString())) != null)
            {
                hf = ((HiddenField)row.FindControl("hfs5" + index.ToString())).Value;
            }
            else
            {
                hf = "";
            }
            txt_terima = (TextBox)row.FindControl("s5_terima" + index.ToString());
            txt_rusak = (TextBox)row.FindControl("s5_rusak" + index.ToString());
            //txt_selisih = (TextBox)row.FindControl("s5_selisih" + index.ToString());
            txt_cterima = (TextBox)row.FindControl("s5_cterima" + index.ToString());
            txt_crusak = (TextBox)row.FindControl("s5_selisih" + index.ToString());
            txket = (TextBox)row.FindControl("s5_ket" + index.ToString());

            if (hf != "")
            {
                var d = db.StockProductDJ_Stone5.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        Butir = Convert.ToInt32(d.TotalButir);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        //txt_selisih.Text = Selisih.ToString();
                        Carat = Convert.ToDecimal(d.TotalCarat);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Crusak = Convert.ToDecimal(txt_crusak.Text);
                        Cselisih = d.TotalCarat - Cterima - Crusak;
                        tket = txket.Text;
                        StockRetireDJ_Stone5 j = new StockRetireDJ_Stone5();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratButir = d.TotalCarat;
                        j.CaratTerima = Cterima;
                        j.CaratRusak = Crusak;
                        j.CaratSelisih = Cselisih;
                        j.Keterangan = tket;
                        db.StockRetireDJ_Stone5.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }
                index++;
            }
        }
    }

    protected void CalcSusutBongkar(object sender, EventArgs e)
    {
        ((TextBox)sender).Focus();
        CalcAll();
    }

    protected void CalcSusutLebur(object sender, EventArgs e)
    {
        ((TextBox)sender).Focus();
        CalcAll();
    }
    protected void CalcAll()
    {
        decimal NettoWeight = 0m;
        decimal SusutBongkar = 0m;
        decimal NettoBongkar = 0m;
        decimal SusutLebur = 0m;
        decimal BeratAkhir = 0m;

        try
        {
            NettoWeight = Convert.ToDecimal(netto.InnerText);
        }
        catch (Exception ex) { }
        try
        {
            SusutBongkar = Convert.ToDecimal(susutbongkar.Text);
        }
        catch (Exception ex) { }
        try
        {
            SusutLebur = Convert.ToDecimal(susutlebur.Text);
        }
        catch (Exception ex) { }

        NettoBongkar = NettoWeight - SusutBongkar;
        BeratAkhir = NettoBongkar - SusutLebur;

        nettobongkar.Text = NettoBongkar.ToString("N2");
        beratakhir.Text = BeratAkhir.ToString("N2");
    }
}