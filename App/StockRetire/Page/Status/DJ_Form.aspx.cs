using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ_Retirement : System.Web.UI.Page
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
        if (d == null) App.DataNull(this);
        else
        {
            petugaslebur.Text = d.Nama;
            keterangan.Text = d.Keterangan;
            alasan.Text = d.Alasan;
            //Header
            header_nomor.InnerHtml = d.Nomor;

            susutbongkar.Text = Str.Number2(d.SusutBongkar);
            nettobongkar.Text = Str.Number2(d.NettoSetelahBongkar);
            susutlebur.Text = Str.Number2(d.SusutLebur);
            beratakhir.Text = Str.Number2(d.BeratAkhir);
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.StockRetireDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
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
            fillProduct(d.StockProductDJ);

            fillMU(d.StockProductDJ);

        }
    }

    protected void fillProduct(StockProductDJ ms)
    {
        var d = ms.StockProductDJ_PricingProduct;
        var x = ms.StockProductDJ_CostingProduct;
        if (d != null && x != null)
        {
            netto.InnerHtml = Str.Number2(ms.StockProductDJ_CharProduct.NettoWeight);
            kadar.InnerHtml = Str.Number2(Math.Ceiling(ms.StockProductDJ_CharProduct.KadarTukaran));
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
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1A.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s1A_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //
            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s1A_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S1A_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);
            //

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s1A_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S1A_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt_selisih = new TextBox();
            txt_selisih.ID = "s1A_selisih" + index.ToString();
            txt_selisih.Attributes["style"] = "width: 50px;";
            txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfs1A" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
        ((TextBox)sender).Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs1a" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s1A_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s1A_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s1A_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s1A_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone1A.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                    
                        Butir = Convert.ToInt32(d.TotalButir);                       
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) {  }
            index++;
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
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
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
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s1B_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S1B_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //
            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s1B_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S1B_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);
            //

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s1B_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S1B_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt_selisih = new TextBox();
            txt_selisih.ID = "s1B_selisih" + index.ToString();
            txt_selisih.Attributes["style"] = "width: 50px;";
            txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfs1B" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
        ((TextBox)sender).Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfs1B" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s1B_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s1B_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s1B_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s1B_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone1B.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
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
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
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
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s2_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S2_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //
            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s2_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S2_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);
            //

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s2_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S2_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt_selisih = new TextBox();
            txt_selisih.ID = "s2_selisih" + index.ToString();
            txt_selisih.Attributes["style"] = "width: 50px;";
            txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfs2" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
        ((TextBox)sender).Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfs2" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s2_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s2_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s2_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s2_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone2.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();

                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
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
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
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
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s3_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S3_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //
            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s3_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S3_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);
            //

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s3_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S3_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt_selisih = new TextBox();
            txt_selisih.ID = "s3_selisih" + index.ToString();
            txt_selisih.Attributes["style"] = "width: 50px;";
            txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfs3" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
        ((TextBox)sender).Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs3" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s3_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s3_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s3_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s3_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone3.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
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
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
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
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s4_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S4_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //
            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s4_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S4_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);
            //

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s4_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S4_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt_selisih = new TextBox();
            txt_selisih.ID = "s4_selisih" + index.ToString();
            txt_selisih.Attributes["style"] = "width: 50px;";
            txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfs4" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
        ((TextBox)sender).Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs4" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s4_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s4_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s4_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s4_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone4.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();

                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
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
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
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
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            TextBox txt_terima = new TextBox();
            txt_terima.ID = "s5_terima" + index.ToString();
            txt_terima.Attributes["style"] = "width: 50px;";
            txt_terima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_terima.Text = Str.Integer(r.TotalButir);
            txt_terima.AutoPostBack = true;
            txt_terima.TextChanged += new EventHandler(S5_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_terima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            //
            TextBox txt_cterima = new TextBox();
            txt_cterima.ID = "s5_cterima" + index.ToString();
            txt_cterima.Attributes["style"] = "width: 50px;";
            txt_cterima.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_cterima.Text = Str.Number3(r.TotalCarat);
            txt_cterima.AutoPostBack = true;
            txt_cterima.TextChanged += new EventHandler(S5_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_cterima);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);
            //

            TextBox txt_rusak = new TextBox();
            txt_rusak.ID = "s5_rusak" + index.ToString();
            txt_rusak.Attributes["style"] = "width: 50px;";
            txt_rusak.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_rusak.Text = "0";
            txt_rusak.AutoPostBack = true;
            txt_rusak.TextChanged += new EventHandler(S5_TextChanged);

            td = new TableCell();
            td.Controls.Add(txt_rusak);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt_selisih = new TextBox();
            txt_selisih.ID = "s5_selisih" + index.ToString();
            txt_selisih.Attributes["style"] = "width: 50px;";
            txt_selisih.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt_selisih.Text = "0";
            //txt_terima.AutoPostBack = true;
            //txt_terima.TextChanged += new EventHandler(S1A_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfs5" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt_selisih);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
        ((TextBox)sender).Focus();

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfs5" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s5_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s5_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s5_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s5_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone5.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();

                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
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
        var d = db.StockRetireDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            saveStone(d.StockProductDJ);

            d.NettoWeight = d.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;
            d.SusutBongkar = Convert.ToDecimal(susutbongkar.Text);
            d.NettoSetelahBongkar = Convert.ToDecimal(nettobongkar.Text);
            d.SusutLebur = Convert.ToDecimal(susutlebur.Text);
            d.BeratAkhir = Convert.ToDecimal(beratakhir.Text);
            d.Nama = petugaslebur.Text;
            d.Alasan = alasan.Text;
            d.Keterangan = keterangan.Text;
            db.SubmitChanges();

            //Redirect
            App.ReloadClose(this);
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
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();

        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            decimal Cselisih = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs1a" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s1A_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s1A_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s1A_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s1A_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone1A.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {                        
                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                        Cselisih = d.TotalCarat - Cterima;
                        //
                        StockRetireDJ_Stone1A j = new StockRetireDJ_Stone1A();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratTerima = Cterima;
                        j.CaratSelisih = Cselisih;

                        db.StockRetireDJ_Stone1A.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) {  }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }

    protected void saveStone1B(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone1B WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();

        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            decimal Cselisih = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs1B" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s1B_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s1B_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s1B_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s1B_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone1B.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {                        
                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                        Cselisih = d.TotalCarat - Cterima;

                        StockRetireDJ_Stone1B j = new StockRetireDJ_Stone1B();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratTerima = Cterima;
                        j.CaratSelisih = Cselisih;

                        db.StockRetireDJ_Stone1B.InsertOnSubmit(j);
                        db.SubmitChanges();
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }

    protected void saveStone2(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone2 WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();

        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            decimal Cselisih = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs2" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s2_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s2_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s2_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s2_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone2.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        
                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                        Cselisih = d.TotalCarat - Cterima;

                        StockRetireDJ_Stone2 j = new StockRetireDJ_Stone2();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratTerima = Cterima;
                        j.CaratSelisih = Cselisih;

                        db.StockRetireDJ_Stone2.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }

    protected void saveStone3(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone3 WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();

        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            decimal Cselisih = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs3" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s3_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s3_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s3_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s3_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone3.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {                        
                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                        Cselisih = d.TotalCarat - Cterima;

                        StockRetireDJ_Stone3 j = new StockRetireDJ_Stone3();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratTerima = Cterima;
                        j.CaratSelisih = Cselisih;

                        db.StockRetireDJ_Stone3.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }

    protected void saveStone4(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone4 WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            decimal Cselisih = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs4" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s4_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s4_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s4_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s4_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone4.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {                        
                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                        Cselisih = d.TotalCarat - Cterima;

                        StockRetireDJ_Stone4 j = new StockRetireDJ_Stone4();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.TotalSelisih = Selisih;
                        j.CaratTerima = Cterima;
                        j.CaratSelisih = Cselisih;

                        db.StockRetireDJ_Stone4.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }

    protected void saveStone5(StockProductDJ s)
    {
        Db.Execute("DELETE FROM StockRetireDJ_Stone5 WHERE IDForm=" + myID, Db.Cnn_DB);

        int index = 1;

        TextBox txt_terima = new TextBox();
        TextBox txt_rusak = new TextBox();
        TextBox txt_selisih = new TextBox();
        TextBox txt_cterima = new TextBox();

        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            int Butir = 0;
            int Terima = 0;
            int Rusak = 0;
            int Selisih = 0;
            decimal Cterima = 0m;
            decimal Cselisih = 0m;

            try
            {
                hf = ((HiddenField)row.FindControl("hfs5" + index.ToString())).Value;
                txt_terima = (TextBox)row.FindControl("s5_terima" + index.ToString());
                txt_rusak = (TextBox)row.FindControl("s5_rusak" + index.ToString());
                txt_selisih = (TextBox)row.FindControl("s5_selisih" + index.ToString());
                txt_cterima = (TextBox)row.FindControl("s5_cterima" + index.ToString());

                var d = db.StockProductDJ_Stone5.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {                        
                        Butir = Convert.ToInt32(d.TotalButir);
                        Cterima = Convert.ToDecimal(txt_cterima.Text);
                        Terima = Convert.ToInt32(txt_terima.Text);
                        Rusak = Convert.ToInt32(txt_rusak.Text);
                        Selisih = Butir - Terima - Rusak;
                        txt_selisih.Text = Selisih.ToString();
                        Cselisih = d.TotalCarat - Cterima;

                        StockRetireDJ_Stone5 j = new StockRetireDJ_Stone5();

                        j.IDForm = myID;
                        j.IDStone = d.ID;
                        j.TotalButir = d.TotalButir;
                        j.TotalTerima = Terima;
                        j.TotalRusak = Rusak;
                        j.CaratTerima = Cterima;
                        j.CaratSelisih = Cselisih;

                        db.StockRetireDJ_Stone5.InsertOnSubmit(j);
                        db.SubmitChanges();

                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
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