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
            fillForm();
        }
    }

    //Form
    protected void fillForm()
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


            if (d.Status == 3 || d.Status == 2)
            {
                go.Visible = false;
            }
            ////Product Char.
            var product = d.StockProductDJ.StockProductDJ_CharProduct;
            if (product == null) { }
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
            if (design == null) { }
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

            fillStone(d);
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
            netto.InnerHtml = Str.Number2(x.Netto);
            kadar.InnerHtml = Str.Number2(Math.Ceiling(x.Kadar));
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
    protected void fillStone(StockRetireDJ s)
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;
        int GT_ButirTerima = 0;
        decimal GT_CaratTerima = 0m;
        int GT_ButirRusak = 0;
        int GT_ButirSelisih = 0;
        decimal GT_CaratSelisih = 0m;

        //Stone
        fillStone1A(0, ref GT_Butir, ref GT_Carat, ref GT_ButirTerima, ref GT_CaratTerima, ref GT_ButirRusak, ref GT_ButirSelisih, ref GT_CaratSelisih, s);
        fillStone1B(1, ref GT_Butir, ref GT_Carat, ref GT_ButirTerima, ref GT_CaratTerima, ref GT_ButirRusak, ref GT_ButirSelisih, ref GT_CaratSelisih, s);
        fillStone2(2, ref GT_Butir, ref GT_Carat, ref GT_ButirTerima, ref GT_CaratTerima, ref GT_ButirRusak, ref GT_ButirSelisih, ref GT_CaratSelisih, s);
        fillStone3(3, ref GT_Butir, ref GT_Carat, ref GT_ButirTerima, ref GT_CaratTerima, ref GT_ButirRusak, ref GT_ButirSelisih, ref GT_CaratSelisih, s);
        fillStone4(4, ref GT_Butir, ref GT_Carat, ref GT_ButirTerima, ref GT_CaratTerima, ref GT_ButirRusak, ref GT_ButirSelisih, ref GT_CaratSelisih, s);
        fillStone5(5, ref GT_Butir, ref GT_Carat, ref GT_ButirTerima, ref GT_CaratTerima, ref GT_ButirRusak, ref GT_ButirSelisih, ref GT_CaratSelisih, s);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Carat, GT_ButirTerima, GT_CaratTerima, GT_ButirRusak, GT_ButirSelisih, GT_CaratSelisih, true);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref int GT_ButirTerima, ref decimal GT_CaratTerima, ref int GT_ButirRusak, ref int GT_ButirSelisih, ref decimal GT_CaratSelisih, StockRetireDJ s)
    {
        //SELECT
        var rs = from p in db.StockRetireDJ_Stone1A
                 where p.IDForm == s.ID
                 orderby p.StockProductDJ_Stone1A.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        int ST_ButirTerima = 0;
        decimal ST_CaratTerima = 0;
        int ST_ButirRusak = 0;
        int ST_ButirSelisih = 0;
        decimal ST_CaratSelisih = 0;


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
            td.Text = r.StockProductDJ_Stone1A.Stone1A.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone1A.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone1A.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone1A.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalRusak);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone1A.TotalButir;
            ST_Carat += r.StockProductDJ_Stone1A.TotalCarat;
            ST_ButirTerima += r.TotalTerima;
            ST_CaratTerima += r.CaratTerima;
            ST_ButirRusak += r.TotalRusak;
            ST_ButirSelisih += r.TotalSelisih;
            ST_CaratSelisih += r.CaratSelisih;

        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;
            GT_ButirTerima += ST_ButirTerima;
            GT_CaratTerima += ST_CaratTerima;
            GT_ButirRusak += ST_ButirRusak;
            GT_ButirSelisih += ST_ButirSelisih;
            GT_CaratSelisih += ST_CaratSelisih;

            Sub_Total(ST_Butir, ST_Carat, ST_ButirTerima, ST_CaratTerima, ST_ButirRusak, ST_ButirSelisih, ST_CaratSelisih, false);
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref int GT_ButirTerima, ref decimal GT_CaratTerima, ref int GT_ButirRusak, ref int GT_ButirSelisih, ref decimal GT_CaratSelisih, StockRetireDJ s)
    {
        //SELECT
        var rs = from p in db.StockRetireDJ_Stone1B
                 where p.IDForm == s.ID
                 orderby p.StockProductDJ_Stone1B.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        int ST_ButirTerima = 0;
        decimal ST_CaratTerima = 0;
        int ST_ButirRusak = 0;
        int ST_ButirSelisih = 0;
        decimal ST_CaratSelisih = 0;

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
            td.Text = r.StockProductDJ_Stone1B.Stone1B.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone1B.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone1B.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone1B.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalRusak);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone1B.TotalButir;
            ST_Carat += r.StockProductDJ_Stone1B.TotalCarat;
            ST_ButirTerima += r.TotalTerima;
            ST_CaratTerima += r.CaratTerima;
            ST_ButirRusak += r.TotalRusak;
            ST_ButirSelisih += r.TotalSelisih;
            ST_CaratSelisih += r.CaratSelisih;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;
            GT_ButirTerima += ST_ButirTerima;
            GT_CaratTerima += ST_CaratTerima;
            GT_ButirRusak += ST_ButirRusak;
            GT_ButirSelisih += ST_ButirSelisih;
            GT_CaratSelisih += ST_CaratSelisih;

            Sub_Total(ST_Butir, ST_Carat, ST_ButirTerima, ST_CaratTerima, ST_ButirRusak, ST_ButirSelisih, ST_CaratSelisih, false);
        }
    }

    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref int GT_ButirTerima, ref decimal GT_CaratTerima, ref int GT_ButirRusak, ref int GT_ButirSelisih, ref decimal GT_CaratSelisih, StockRetireDJ s)
    {
        //SELECT
        var rs = from p in db.StockRetireDJ_Stone2
                 where p.IDForm == s.ID
                 orderby p.StockProductDJ_Stone2.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        int ST_ButirTerima = 0;
        decimal ST_CaratTerima = 0;
        int ST_ButirRusak = 0;
        int ST_ButirSelisih = 0;
        decimal ST_CaratSelisih = 0;

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
            td.Text = r.StockProductDJ_Stone2.Stone2.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone2.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone2.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone2.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalRusak);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.CaratSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone2.TotalButir;
            ST_Carat += r.StockProductDJ_Stone2.TotalCarat;
            ST_ButirTerima += r.TotalTerima;
            ST_CaratTerima += r.CaratTerima;
            ST_ButirRusak += r.TotalRusak;
            ST_ButirSelisih += r.TotalSelisih;
            ST_CaratSelisih += r.CaratSelisih;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;
            GT_ButirTerima += ST_ButirTerima;
            GT_CaratTerima += ST_CaratTerima;
            GT_ButirRusak += ST_ButirRusak;
            GT_ButirSelisih += ST_ButirSelisih;
            GT_CaratSelisih += ST_CaratSelisih;

            Sub_Total(ST_Butir, ST_Carat, ST_ButirTerima, ST_CaratTerima, ST_ButirRusak, ST_ButirSelisih, ST_CaratSelisih, false);
        }
    }

    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref int GT_ButirTerima, ref decimal GT_CaratTerima, ref int GT_ButirRusak, ref int GT_ButirSelisih, ref decimal GT_CaratSelisih,StockRetireDJ s)
    {
        //SELECT
        var rs = from p in db.StockRetireDJ_Stone3
                 where p.IDForm == s.ID
                 orderby p.StockProductDJ_Stone3.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        int ST_ButirTerima = 0;
        decimal ST_CaratTerima = 0;
        int ST_ButirRusak = 0;
        int ST_ButirSelisih = 0;
        decimal ST_CaratSelisih = 0;

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
            td.Text = r.StockProductDJ_Stone3.Stone3.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone3.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone3.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone3.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalRusak);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone3.TotalButir;
            ST_Carat += r.StockProductDJ_Stone3.TotalCarat;
            ST_ButirTerima += r.TotalTerima;
            ST_CaratTerima += r.CaratTerima;
            ST_ButirRusak += r.TotalRusak;
            ST_ButirSelisih += r.TotalSelisih;
            ST_CaratSelisih += r.CaratSelisih;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;
            GT_ButirTerima += ST_ButirTerima;
            GT_CaratTerima += ST_CaratTerima;
            GT_ButirRusak += ST_ButirRusak;
            GT_ButirSelisih += ST_ButirSelisih;
            GT_CaratSelisih += ST_CaratSelisih;

            Sub_Total(ST_Butir, ST_Carat, ST_ButirTerima, ST_CaratTerima, ST_ButirRusak, ST_ButirSelisih, ST_CaratSelisih, false);
        }
    }

    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref int GT_ButirTerima, ref decimal GT_CaratTerima, ref int GT_ButirRusak, ref int GT_ButirSelisih, ref decimal GT_CaratSelisih, StockRetireDJ s)
    {
        //SELECT
        var rs = from p in db.StockRetireDJ_Stone4
                 where p.IDForm == s.ID
                 orderby p.StockProductDJ_Stone4.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        int ST_ButirTerima = 0;
        decimal ST_CaratTerima = 0;
        int ST_ButirRusak = 0;
        int ST_ButirSelisih = 0;
        decimal ST_CaratSelisih = 0;

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
            td.Text = r.StockProductDJ_Stone4.Stone4.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone4.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone4.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone4.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);


            td = new TableCell();
            td.Text = Str.Number0(r.TotalRusak);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone4.TotalButir;
            ST_Carat += r.StockProductDJ_Stone4.TotalCarat;
            ST_ButirTerima += r.TotalTerima;
            ST_CaratTerima += r.CaratTerima;
            ST_ButirRusak += r.TotalRusak;
            ST_ButirSelisih += r.TotalSelisih;
            ST_CaratSelisih += r.CaratSelisih;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;
            GT_ButirTerima += ST_ButirTerima;
            GT_CaratTerima += ST_CaratTerima;
            GT_ButirRusak += ST_ButirRusak;
            GT_ButirSelisih += ST_ButirSelisih;
            GT_CaratSelisih += ST_CaratSelisih;

            Sub_Total(ST_Butir, ST_Carat, ST_ButirTerima, ST_CaratTerima, ST_ButirRusak, ST_ButirSelisih, ST_CaratSelisih, false);
        }
    }

    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref int GT_ButirTerima, ref decimal GT_CaratTerima, ref int GT_ButirRusak, ref int GT_ButirSelisih, ref decimal GT_CaratSelisih, StockRetireDJ s)
    {
        //SELECT
        var rs = from p in db.StockRetireDJ_Stone5
                 where p.IDForm == s.ID
                 orderby p.StockProductDJ_Stone5.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        int ST_ButirTerima = 0;
        decimal ST_CaratTerima = 0;
        int ST_ButirRusak = 0;
        int ST_ButirSelisih = 0;
        decimal ST_CaratSelisih = 0;

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
            td.Text = r.StockProductDJ_Stone5.Stone5.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone5.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone5.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone5.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratTerima);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalRusak);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.TotalSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.CaratSelisih);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone5.TotalButir;
            ST_Carat += r.StockProductDJ_Stone5.TotalCarat;
            ST_ButirTerima += r.TotalTerima;
            ST_CaratTerima += r.CaratTerima;
            ST_ButirRusak += r.TotalRusak;
            ST_ButirSelisih += r.TotalSelisih;
            ST_CaratSelisih += r.CaratSelisih;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;
            GT_ButirTerima += ST_ButirTerima;
            GT_CaratTerima += ST_CaratTerima;
            GT_ButirRusak += ST_ButirRusak;
            GT_ButirSelisih += ST_ButirSelisih;
            GT_CaratSelisih += ST_CaratSelisih;


            Sub_Total(ST_Butir, ST_Carat, ST_ButirTerima, ST_CaratTerima, ST_ButirRusak, ST_ButirSelisih, ST_CaratSelisih, false);
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
    protected void Sub_Total(int TotalButir, decimal TotalCarat, int ButirTerima, decimal CaratTerima, int ButirRusak, int ButirSelisih, decimal CaratSelisih, bool GrandTotal)
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

        td = new TableCell();
        td.Text = Str.Integer(ButirTerima);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number3(CaratTerima);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(ButirRusak);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(ButirSelisih);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number3(CaratSelisih);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "SPL_DJ.aspx?id=" + myID;

        Response.Redirect(href);
    }
}