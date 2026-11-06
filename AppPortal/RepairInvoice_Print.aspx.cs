using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Repair_Invoice : System.Web.UI.Page {
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

    protected void Page_Load(object sender, EventArgs e)
    {
        //App.Protect();
        //App.ProtectRole(Role.Operator);

        var repairdata = db.DocRepairResult.SingleOrDefault(p => p.ID == myID);
        var locationbrand_outlet = db.LocOutlet;
        var locationbrand_exhibition = db.LocExhibition;

        string CB = ""; // mondial, franknco, missmondial, thepalace
        if (repairdata.TipeLokasi == 1) { CB = locationbrand_outlet.SingleOrDefault(p => p.ID == repairdata.IDLokasi).CompanyBrand.NamaKode; }
        else if (repairdata.TipeLokasi == 2) { CB = locationbrand_exhibition.SingleOrDefault(p => p.ID == repairdata.IDLokasi).CompanyBrand.NamaKode; }

        CB = CB.ToUpper();

        if (CB == "M" || CB == "S") { img_logo_web.Src = "https://image-james.cmk.co.id/api/Image?img=D:/Web/Images/web/logo_web_mondial.jpg"; }
        else if (CB == "P") { img_logo_web.Src = "https://image-james.cmk.co.id/api/Image?img=D:/Web/Images/web/logo_web_thepalace.jpg"; }
        else if (CB == "F") { img_logo_web.Src = "https://image-james.cmk.co.id/api/Image?img=D:/Web/Images/web/logo_web_franknco.jpg"; }
        else { img_logo_web.Src = "https://image-james.cmk.co.id/api/Image?img=D:/Web/Images/web/logo_web_franknco.jpg"; }

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Repair.aspx"); }

    //Form
    protected void fillForm()
    {
        bool isRepairStock = false; // true : repair stock, false : repair toko

        var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            //Header
            //header_nomor.InnerHtml = d.Nomor;
            lit_nomor_repair.Text = d.Nomor;

            string Cabang = "";
            //if (d.TipeLokasi == 1)
            //    Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi,Db.Cnn_DB);
            //else
            //    Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

            string AlamatCabang = "";
            if (d.TipeLokasi == 0 && isRepairStock)
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                AlamatCabang = Db.SingleString("SELECT AddrAlamat FROM LocWarehouse WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }
            else if (d.TipeLokasi == 1)
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                AlamatCabang = Db.SingleString("SELECT AddrAlamat FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }
            else if (d.TipeLokasi == 2)
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                AlamatCabang = Db.SingleString("SELECT AddrAlamat FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }

            cabang.Text = Str.HtmlBreak(AlamatCabang);

            tgl.InnerText = Str.Tgl(d.Tgl);
            tglselesai.InnerText = Str.Tgl(d.TglSelesai);
            ket.InnerText = d.Keterangan;

            customer.InnerText = (!isRepairStock) ? d.CustomerNama : "-";
            jpjr.InnerText = (!isRepairStock) ? d.SalesNama : "-";

            if (d.IDProduct != null)
            {
                noplu.InnerText = d.StockProductDJ.Nomor;
                productitem.InnerText = d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            }
            else if (d.IDProductTitipan != null)
            {
                noplu.InnerText = d.StockProductDJCustomer.Nomor;
                productitem.InnerText = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama;
            }

            //Upload.Image(imgpicture, d.ImgPicture);
            string Img = "";

            Img = d.ImgPicture;
            string str_image = "";
            //imgpicture.ImageUrl = Img;

            str_image = "<img src='/Images.aspx?path=" + Img + "' style='width: 100px;' border='0' /></div>";

            //lit_image_product.Text = str_image;

            ////Product Char.
            var product = d.DocRepairResult_CharProduct;
            if (product == null) { }
            else
            {
                //Picture Repair
                Upload.Image(imgpicturerepair, product.DocRepairResult.ImgPicture);

                if (d.IDProduct != null)
                {
                    //Picture Before
                    Upload.Image(imgpicturebefore, product.DocRepairResult.StockProductDJ.ImgPicture);
                }
                else if (d.IDProductTitipan != null)
                {
                    //Picture Before
                    Upload.Image(imgpicturebefore, product.DocRepairResult.StockProductDJCustomer.ImgPicture);
                }

                //Weight
                var product_ori = d.DocRepair.DocRepair_CharProduct;

                td_grossweight.InnerHtml = Str.Number2(product_ori.GrossWeight) + " &#8594; " + Str.Number2(product.GrossWeight);
                totalbutir.InnerHtml = Str.Number2(product.StoneQty);

                dimensid.InnerHtml = Str.Number2(product_ori.DimensiD) + " &#8594; " + Str.Number2(product.DimensiD);
                dimensip.InnerHtml = Str.Number2(product_ori.DimensiP) + " &#8594; " + Str.Number2(product.DimensiP);
                dimensil.InnerHtml = Str.Number2(product_ori.DimensiL) + " &#8594; " + Str.Number2(product.DimensiL);
                dimensir.InnerHtml = Str.Number2(product_ori.DimensiR) + " &#8594; " + Str.Number2(product.DimensiR);

                var salesorderrepair = db.SalesOrderRepair.SingleOrDefault(p => p.IDRepair == myID);

                span_harga.InnerHtml = Str.Number0(salesorderrepair.HargaRupiah);
                span_discount.InnerHtml = Str.Number0(salesorderrepair.DiscountNominal);
                span_totalharga.InnerHtml = Str.Number0(salesorderrepair.TotalRupiah);

                //customer2.Text = (!isRepairStock) ? d.CustomerNama : "-";
                jp.Text = (!isRepairStock) ? d.SalesNama : "-";

            }

            var prc_rpr = db.PricingTableTG.SingleOrDefault(p =>
                p.ID == CV.Repair_General_Pricing &&
                p.Harga.HasValue &&
                p.ApprovalTgl.HasValue);

            // harga emas
            decimal HargaEmas = 0m;
            decimal SelisihBerat = 0m;

            bool is_InvalidItem = Pricing.RepairInvoicePricing_InvalidProduct(isRepairStock, d);
            bool is_SmallItem = Pricing.RepairInvoicePricing_SmallBigProduct(isRepairStock, d);

            if (d.DocRepair.IDProduct != null)
            {
                var productold = d.StockProductDJ.StockProductDJ_CharProduct;
                if (product == null) { }
                else
                {
                    SelisihBerat = (decimal)d.DocRepairResult_CharProduct.NettoWeight - (decimal)productold.NettoWeight;
                }
            }
            else if (d.IDProductTitipan != null)
            {
                var productold = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct;
                if (product == null) { }
                else
                {
                    SelisihBerat = (decimal)d.DocRepairResult_CharProduct.NettoWeight - (decimal)productold.NettoWeight;
                }
            }

            HargaEmas = (d.DocRepairResult_CharProduct.HargaEmas) != null ? Convert.ToDecimal(d.DocRepairResult_CharProduct.HargaEmas) : 0m;
            
            if (is_SmallItem) { HargaEmas = (SelisihBerat - 0.5m) > 0 ? HargaEmas : 0m; }
            else { HargaEmas = (SelisihBerat - 0.5m) > 0 ? HargaEmas : 0m; }

            HargaEmas = (Math.Floor((Math.Floor(HargaEmas) / 1000))) * 1000;

            SelisihBerat = (SelisihBerat - 0.5m > 0) ? SelisihBerat - 0.5m : 0m;

            fillEmas(HargaEmas, SelisihBerat);

            //Stone
            fillStone();

            //Repair Item
            fillRepairItem();

        }
    }

    protected void fillEmas(decimal HargaEmas, decimal SelisihBerat)
    {
        TableRow tr = new TableRow();
        TableCell td;

        td = new TableCell();
        td.Text = Str.Number2(SelisihBerat) + " gram";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number0(HargaEmas);
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }

    //Repair Item
    protected void fillRepairItem()
    {
        decimal GT = 0;

        var rs = from p in db.DocRepairResult_Repair
                 where p.IDForm == myID
                 orderby p.CharProcessRepair.Nama
                 select p;

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.CharProcessRepair.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Biaya.Value).ToString("N0");
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT += rs.Sum(p => p.Biaya.Value);
        }
        Sub_Total(GT);
    }

    //Stone
    protected void fillStone()
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;
        decimal GT_Harga = 0;

        //Stone
        fillStone1A(0, ref GT_Butir, ref GT_Carat, ref GT_Harga);
        fillStone1B(1, ref GT_Butir, ref GT_Carat, ref GT_Harga);
        fillStone2(2, ref GT_Butir, ref GT_Carat, ref GT_Harga);
        fillStone3(3, ref GT_Butir, ref GT_Carat, ref GT_Harga);
        fillStone4(4, ref GT_Butir, ref GT_Carat, ref GT_Harga);
        fillStone5(5, ref GT_Butir, ref GT_Carat, ref GT_Harga);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Carat, true, GT_Harga);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_Harga)
    {
        //SELECT
        var rs = from p in db.DocRepairResult_Stone1A
                 where p.IDForm == myID
                 orderby p.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        decimal ST_Harga = 0;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone1A.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalHarga(r.HargaTotalRupiah, ref ST_Harga);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false, ST_Harga);
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_Harga)
    {
        //SELECT
        var rs = from p in db.DocRepairResult_Stone1B
                 where p.IDForm == myID
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        decimal ST_Harga = 0;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone1B.Nama, r.GIA);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalHarga(r.HargaTotalRupiah, ref ST_Harga);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false, ST_Harga);
        }
    }
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_Harga)
    {
        //SELECT
        var rs = from p in db.DocRepairResult_Stone2
                 where p.IDForm == myID
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        decimal ST_Harga = 0;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone2.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalHarga(r.HargaTotalRupiah, ref ST_Harga);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false, ST_Harga);
        }
    }
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_Harga)
    {
        //SELECT
        var rs = from p in db.DocRepairResult_Stone3
                 where p.IDForm == myID
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        decimal ST_Harga = 0;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone3.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalHarga(r.HargaTotalRupiah, ref ST_Harga);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false, ST_Harga);
        }
    }
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_Harga)
    {
        //SELECT
        var rs = from p in db.DocRepairResult_Stone4
                 where p.IDForm == myID
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        decimal ST_Harga = 0;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone4.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalHarga(r.HargaTotalRupiah, ref ST_Harga);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false, ST_Harga);
        }
    }
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_Harga)
    {
        //SELECT
        var rs = from p in db.DocRepairResult_Stone5
                 where p.IDForm == myID
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;
        decimal ST_Harga = 0;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone5.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalHarga(r.HargaTotalRupiah, ref ST_Harga);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false, ST_Harga);
        }
    }

    //Custom Row
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
        td.CssClass = "text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal, decimal TotalHarga)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 2;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-sm\">" + Str.Integer(TotalButir) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-sm\">" + Str.Number3(TotalCarat) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-sm\">" + Str.Number0(TotalHarga) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void Sub_Total(decimal Total)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "text-grandtotal";

        td = new TableCell();
        td.ColumnSpan = 2;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += "Grand Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-sm\">" + Str.Number0(Total) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
    }

    //Display Column
    //TD_TotalHarga
    protected string TD_TotalHarga(decimal? Harga, ref decimal ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm text-info\">" + Str.Number0(Harga) + "</p>";

        //Sub-Total
        if (Harga.HasValue) ST += Harga.Value;

        return x;
    }
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }
    protected string TD_Dimensi(decimal? Dimensi)
    {
        string x = string.Empty;

        x += "<p>" + Str.Number2(Dimensi) + "</p>";

        return x;
    }
    protected string TD_CaratButir(decimal? CaratButir)
    {
        string x = string.Empty;

        x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

        return x;
    }
    protected string TD_TotalButir(int? TotalButir, ref int ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm text-success\">" + Str.Integer(TotalButir) + "</p>";

        //Sub-Total
        if (TotalButir.HasValue) ST += TotalButir.Value;

        return x;
    }
    protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm text-info\">" + Str.Number3(TotalCarat) + "</p>";

        //Sub-Total
        if (TotalCarat.HasValue) ST += TotalCarat.Value;

        return x;
    }
}

