using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Register_Repair_Submit : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    decimal Setting = 0m;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Exception
    protected void PricingError(string sebab)
    {
        Response.Redirect("Repair_Submit.aspx?id=" + myID + "&prcerror=" + sebab);
    }

    //Exception
    protected void DataError() { Response.Redirect("Repair.aspx"); }

    //Form
    protected void fillForm()
    {
        bool isRepairStock = false; // true : repair stock, false : repair toko
        decimal SelisihBerat = 0m;

        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            lit_customer.Text = d.DataCustomer.Nama;
            lit_jpjr.Text = d.DataSales.Nama;
            lit_tgl.Text = Str.FullDate(d.Tgl);
            lit_tgl2.Text = Str.FullDate(d.TglSelesai);
            lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);

            if (d.IDProduct != null) { span_plu.InnerText = d.StockProductDJ.Nomor; }
            else if (d.IDProductTitipan != null) { span_plu.InnerText = d.StockProductDJCustomer.Nomor; }

            var d_item_list = from p in db.DocRepair_Repair where p.IDForm == myID select p;

            var product = d.DocRepair_CharProduct;

            span_productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";

            //Picture Repair
            Upload.Image(imgpicturerepair, product.DocRepair.ImgPicture);

            if (d.IDProduct != null)
            {
                var productold = d.StockProductDJ.StockProductDJ_CharProduct;
                if (product == null) save.Disabled = true;
                else
                {
                    SelisihBerat = (decimal)product.GrossWeight.Value - (decimal)productold.GrossWeight;

                    span_grossweight.InnerHtml = (product.GrossWeight != productold.GrossWeight ? Str.Number2(productold.GrossWeight) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.GrossWeight);
                    span_diameter.InnerHtml = (product.DimensiD != productold.DimensiD ? Str.Number2(productold.DimensiD) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiD);
                    span_panjang.InnerHtml = (product.DimensiP != productold.DimensiP ? Str.Number2(productold.DimensiP) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiP);
                    span_lebar.InnerHtml = (product.DimensiL != productold.DimensiL ? Str.Number2(productold.DimensiL) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiL);
                    span_ringsize.InnerHtml = (product.DimensiR != productold.DimensiR ? Str.Number2(productold.DimensiR) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiR);

                    //Picture Before
                    Upload.Image(imgpicturebefore, productold.StockProductDJ.ImgPicture);
                }
            }
            else if (d.IDProductTitipan != null)
            {
                var productold = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct;
                if (product == null) save.Disabled = true;
                else
                {
                    SelisihBerat = (decimal)product.GrossWeight - (decimal)productold.GrossWeight;

                    span_grossweight.InnerHtml = (product.GrossWeight != productold.GrossWeight ? Str.Number2(productold.GrossWeight) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.GrossWeight);
                    span_diameter.InnerHtml = (product.DimensiD != productold.DimensiD ? Str.Number2(productold.DimensiD) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiD);
                    span_panjang.InnerHtml = (product.DimensiP != productold.DimensiP ? Str.Number2(productold.DimensiP) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiP);
                    span_lebar.InnerHtml = (product.DimensiL != productold.DimensiL ? Str.Number2(productold.DimensiL) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiL);
                    span_ringsize.InnerHtml = (product.DimensiR != productold.DimensiR ? Str.Number2(productold.DimensiR) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiR);

                    //Picture Before
                    Upload.Image(imgpicturebefore, productold.StockProductDJCustomer.ImgPicture);
                }
            }

            if (product == null) save.Disabled = true;
            else
            {
                decimal TotalBiaya = 0m;
                decimal OngkosRepair = 0m;
                decimal HargaBatu = 0m;
                decimal HargaEmas = 0m;

                if (d.DocRepair_Stone1A.Count() > 0) { var dr = db.DocRepair_Stone1A.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepair_Stone1B.Count() > 0) { var dr = db.DocRepair_Stone1B.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepair_Stone2.Count() > 0) { var dr = db.DocRepair_Stone2.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepair_Stone3.Count() > 0) { var dr = db.DocRepair_Stone3.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepair_Stone4.Count() > 0) { var dr = db.DocRepair_Stone4.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepair_Stone5.Count() > 0) { var dr = db.DocRepair_Stone5.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }

                // looping pembulatan harga batu
                var x1A = d.DocRepair_Stone1A; foreach (var r1A in x1A) { r1A.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r1A.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x1B = d.DocRepair_Stone1B; foreach (var r1B in x1B) { r1B.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r1B.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x2 = d.DocRepair_Stone2; foreach (var r2 in x2) { r2.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r2.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x3 = d.DocRepair_Stone3; foreach (var r3 in x3) { r3.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r3.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x4 = d.DocRepair_Stone4; foreach (var r4 in x4) { r4.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r4.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x5 = d.DocRepair_Stone5; foreach (var r5 in x5) { r5.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r5.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }

                if (d.DocRepair_Stone1A.Count() > 0) { HargaBatu += d.DocRepair_Stone1A.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepair_Stone1B.Count() > 0) { HargaBatu += d.DocRepair_Stone1B.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepair_Stone2.Count() > 0) { HargaBatu += d.DocRepair_Stone2.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepair_Stone3.Count() > 0) { HargaBatu += d.DocRepair_Stone3.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepair_Stone4.Count() > 0) { HargaBatu += d.DocRepair_Stone4.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepair_Stone5.Count() > 0) { HargaBatu += d.DocRepair_Stone5.Sum(p => p.HargaTotalRupiah.Value); }

                var prc_rpr = db.PricingTableTG.SingleOrDefault(p =>
                    p.ID == CV.Repair_General_Pricing &&
                    p.Harga.HasValue &&
                    p.ApprovalTgl.HasValue);

                bool is_InvalidItem = false;
                bool is_SmallItem = false;

                // invalid item condition
                if (!isRepairStock)
                {
                    if (d.IDProduct != null)
                    {
                        var x = d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem;
                        is_InvalidItem = (x.Nama.ToLower().Contains("lm")
                                        || x.Nama.ToLower().Contains("loose diamond")
                                        || x.Nama.ToLower().Contains("chain")
                                        ) ? true : false;
						var xo = d.StockProductDJ.StockProductDJ_CharProduct;
						if(xo.ProductItem == 90)
							is_InvalidItem = false;
                    }
                    else if (d.IDProductTitipan != null)
                    {
                        var x = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem;
                        is_InvalidItem = (x.Nama.ToLower().Contains("lm")
                                        || x.Nama.ToLower().Contains("loose diamond")
                                        || x.Nama.ToLower().Contains("chain")
                                        ) ? true : false;
                    }
					
                }

                // small item condition
                if (d.IDProduct != null)
                {
                    var x = d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem;
                    is_SmallItem = (x.Nama.ToLower().Contains("pendant")
                                 || x.Nama.ToLower().Contains("ladies ring")
                                 || x.Nama.ToLower().Contains("earring")
                                 || x.Nama.ToLower().Contains("wedding ring")
                                 || x.Nama.ToLower().Contains("aretino")
                                 || x.Nama.ToLower().Contains("baby")
                                 ) ? true : false;
                }
                else if (d.IDProductTitipan != null)
                {
                    var x = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem;
                    is_SmallItem = (x.Nama.ToLower().Contains("pendant")
                                 || x.Nama.ToLower().Contains("ladies ring")
                                 || x.Nama.ToLower().Contains("earring")
                                 || x.Nama.ToLower().Contains("wedding ring")
                                 || x.Nama.ToLower().Contains("aretino")
                                 || x.Nama.ToLower().Contains("baby")
                                 ) ? true : false;
                }

                if (is_InvalidItem == true && !isRepairStock) { save.Disabled = true; lit_msg.Text = "<p class='bg-danger'>Repair tidak bisa dilakukan untuk Product Item : " + product.CharProductItem.Nama + "</p>"; } // barang termasuk dalam daftar yang tidak boleh di repair

                //repairproductitem.InnerText = (is_InvalidItem && !isRepairStock) ? "Invalid" : ((is_SmallItem) ? "Small" : "Big");

                if (is_SmallItem) { HargaEmas = (SelisihBerat - 0.5m) > 0 ? ((SelisihBerat - 0.5m) * prc_rpr.Harga.Value) : 0m; }
                else { HargaEmas = (SelisihBerat - 0.5m) > 0 ? ((SelisihBerat - 0.5m) * prc_rpr.Harga.Value) : 0m; }

                OngkosRepair = d.DocRepair_Repair.Sum(p => p.Biaya.Value);

                OngkosRepair = (Math.Floor((Math.Floor(OngkosRepair) / 1000))) * 1000;
                HargaBatu = (Math.Floor((Math.Floor(HargaBatu) / 1000))) * 1000;
                HargaEmas = (Math.Floor((Math.Floor(HargaEmas) / 1000))) * 1000;

                TotalBiaya = OngkosRepair + HargaBatu + HargaEmas;
                //TotalBiaya = (Math.Floor((Math.Floor(TotalBiaya) / 1000))) * 1000; // pembulatan 1000 ke bawah

                lit_pricetotal_jasa.Text = Str.Number0(OngkosRepair);
                lit_pricetotal_batu.Text = Str.Number0(HargaBatu);
                lit_pricetotal_emas.Text = Str.Number0(HargaEmas);
                lit_pricetotal.Text = Str.Number0(TotalBiaya);

                //estimasi.InnerHtml = Str.Number0(TotalBiaya);

                // update repair estimasi harga
                var r = db.DocRepair.SingleOrDefault(p => p.ID == myID);
                r.EstimasiHarga = TotalBiaya;
                db.SubmitChanges();

            }

            if (d.IDProduct.HasValue)
            {
                //Stone
                fillStone();
            }

            //Repair Item
            fillRepairItem();
        }
    }

    //Repair Item
    protected void fillRepairItem()
    {
        decimal GT = 0;

        var rs = from p in db.DocRepair_Repair
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

        //Stone
        fillStone1A(0, ref GT_Butir, ref GT_Carat);
        fillStone1B(1, ref GT_Butir, ref GT_Carat);
        fillStone2(2, ref GT_Butir, ref GT_Carat);
        fillStone3(3, ref GT_Butir, ref GT_Carat);
        fillStone4(4, ref GT_Butir, ref GT_Carat);
        fillStone5(5, ref GT_Butir, ref GT_Carat);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Carat, true);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone1A
                 where p.IDForm == myID
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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone1A.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

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
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.HtmlBreak(r.Keterangan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone1B
                 where p.IDForm == myID
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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone1B.Nama, r.GIA);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StatusTerpasang == 1
                    ? "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>"
                    : "<div class='text-center'><span class='glyphicon glyphicon-remove' aria-hidden='true' style='color:red; font-size:25px;'></span></div>"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

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
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.HtmlBreak(r.Keterangan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone2
                 where p.IDForm == myID
                 orderby p.Stone2.Nama
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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone2.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

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
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.HtmlBreak(r.Keterangan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone3
                 where p.IDForm == myID
                 orderby p.Stone3.Nama
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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone3.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

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
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.HtmlBreak(r.Keterangan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone4
                 where p.IDForm == myID
                 orderby p.Stone4.Nama
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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone4.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

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
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.HtmlBreak(r.Keterangan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone5
                 where p.IDForm == myID
                 orderby p.Stone5.Nama
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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone5.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

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
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.HtmlBreak(r.Keterangan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
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
        td.ColumnSpan = 7;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Integer(TotalButir) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number3(TotalCarat) + "</p>";
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
        td.Text = "<p class=\"text-lg\">" + Str.Number0(Total) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
    }

    //Display Column
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

        x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

        //Sub-Total
        if (TotalButir.HasValue) ST += TotalButir.Value;

        return x;
    }
    protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

        //Sub-Total
        if (TotalCarat.HasValue) ST += TotalCarat.Value;

        return x;
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            if (d.IDProductTitipan != null)
            {
                var titipanrepair = d.StockProductDJCustomer;
                var docttp = db.DocTitipan_Product.SingleOrDefault(p => p.IDProductTitipan == d.IDProductTitipan).DocTitipan;

                docttp.StatusTitipan = 2;

                db.SubmitChanges();

                saveRepairTitipan(d); // Untuk Repair Stock Titipan
            }
            else if (d.IDProduct != null)
            {
                DocTitipan n = new DocTitipan();

                //Location
                n.IDLokasi = d.IDLokasi;
                n.TipeLokasi = d.TipeLokasi;

                //Status
                n.Draft = false;
                n.DraftDate = null;

                //User-Info
                n.Operator = App.UserNama;
                n.OperatorTgl = DateTime.Now;

                n.IDCustomer = d.IDCustomer;
                n.CustomerNama = d.CustomerNama;
                n.IDSales = d.IDSales;
                n.SalesNama = d.SalesNama;
                n.StatusTitipan = 2;
                n.TglSelesai = d.Tgl.Value.AddMonths(1);
                n.Nomor = Generate_Nomor(n);
                n.Tgl = d.Tgl;
                db.DocTitipan.InsertOnSubmit(n);
                db.SubmitChanges();

                int IDForm = n.ID;

                DocTitipan_Product u = new DocTitipan_Product();

                u.IDForm = IDForm;
                u.IDProduct = d.IDProduct;
                db.DocTitipan_Product.InsertOnSubmit(u);
                db.SubmitChanges();

                saveRepair(d); // Untuk Repair Stock Biasa
            }

            //saveRepair(d);

            //Status
            d.Draft = false;
            // d.Nomor = Generate_Nomor(d);
            d.Nomor = Generate_NomorNew(Convert.ToInt32(d.Tgl.Value.Year), Convert.ToInt32(d.Tgl.Value.Month), Convert.ToInt32(d.TipeLokasi), Convert.ToInt32(d.IDLokasi));
            d.DraftDate = null;
            //if (d.IDProduct.HasValue)
            //    d.StockProductDJ.StockActualDJ.Substorage = 5;

            //Commit
            db.SubmitChanges();
			
			try
			{ 
				Db.Execute("exec sp_get_repair_myapps 'FC','"+ d.Nomor +"' ",Db.Cnn_CS);
			}
			catch{}


            //Redirect
			if(d.IDProduct != null)
			{
				string pageurl2 = "/OutletSales/Page/Report/Repair_Print2.aspx?id=" + myID + "&idstock="+ d.StockProductDJ.ID +"";
				Response.Write("<script>");
				Response.Write("window.open('" + pageurl2 + "','_blank')");
				Response.Write("</script>");

				string pageurl = "/OutletSales/Page/Report/Repair_Print.aspx?id=" + myID + "&idstock="+ d.StockProductDJ.ID +"";
				Response.Write("<script>");
				Response.Write("window.location.href='" + pageurl + "'");
				Response.Write("</script>");
				//Response.Redirect("Repair_Print.aspx?id=" + myID);
			}
			else
			{
				
				string pageurl2 = "/OutletSales/Page/Report/Repair_Print2.aspx?id=" + myID ;
				Response.Write("<script>");
				Response.Write("window.open('" + pageurl2 + "','_blank')");
				Response.Write("</script>");

				string pageurl = "/OutletSales/Page/Report/Repair_Print.aspx?id=" + myID ;
				Response.Write("<script>");
				Response.Write("window.location.href='" + pageurl + "'");
				Response.Write("</script>");
				//Response.Redirect("Repair_Print.aspx?id=" + myID);
			}





        }
        //Response.Redirect("Repair_Pricing.aspx?id=" + myID);
    }

    protected void saveRepair(DocRepair d)
    {
        decimal OngkosRepair = 0m;
        try
        {
            OngkosRepair = d.DocRepair_Repair.Sum(p => p.Biaya.Value);
        }
        catch (Exception ex) { }

        DocRepairResult n = new DocRepairResult();

        //Location
        n.IDLokasi = d.IDLokasi;
        n.TipeLokasi = d.TipeLokasi;
        n.IDProduct = d.IDProduct;
        n.IDProductTitipan = n.IDProductTitipan;

        n.Tgl = d.Tgl;
        n.Draft = false;
        n.TglSelesai = d.TglSelesai;
        n.StatusRepair = 2;
        n.ImgPicture = d.ImgPicture;
        n.IDWorkOrder = d.ID;
        n.TotalHarga = OngkosRepair;
        n.Sumber = d.Sumber;

        //User-Info
        n.Operator = App.UserNama;
        n.OperatorTgl = DateTime.Now;
        n.StatusRepair = 2;

        //Commit
        db.DocRepairResult.InsertOnSubmit(n);
        db.SubmitChanges();

        var s = d.DocRepair_CharProduct;

        DocRepairResult_CharProduct m = new DocRepairResult_CharProduct();

        m.ID = n.ID;
        m.ProductItem = s.ProductItem;
        m.ProductCategory = s.ProductCategory;
        m.ProductLevel = s.ProductLevel;
        m.StoneDist = s.StoneDist;
        m.FrameMaterial = s.FrameMaterial;
        m.FrameFinishing = s.FrameFinishing;
        m.FrameColor = s.FrameColor;
        m.ProcessCons = s.ProcessCons;
        m.GrossWeight = s.GrossWeight;
        m.StoneCarat = s.StoneCarat;
        m.StoneWeight = s.StoneWeight;
        m.StoneQty = s.StoneQty;
        m.NettoWeight = s.NettoWeight;
        m.KadarLogam = s.KadarLogam;
        m.KadarTukaran = s.KadarTukaran;
        m.DimensiD = s.DimensiD;
        m.DimensiP = s.DimensiP;
        m.DimensiL = s.DimensiL;
        m.DimensiR = s.DimensiR;

        //Commit
        db.DocRepairResult_CharProduct.InsertOnSubmit(m);
        db.SubmitChanges();

        n.Nomor = Generate_Nomor(n);
        db.SubmitChanges();

        // Ledger 
        StockLedgerDJ l = new StockLedgerDJ();

        // Value (See Library DB - Ledger for Value and Remark)
        decimal Value = +1; // 1 : in , -1 : out
        int Remark = 19; // See Library DB - Ledger
        string Keterangan = n.Nomor; // Commonly filled by document number or id 
        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

        string Lokasi = "";
        if (d.TipeLokasi == 1)
            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
        else if (d.TipeLokasi == 2)
            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

        //Input
        l.IDProduct = d.StockProductDJ.ID;
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
        db.StockLedgerDJ.InsertOnSubmit(l);
        db.SubmitChanges();
        
        moveItem(n);

        //d.StockProductDJ.StockProductDJ_CharProduct.GrossWeight = d.DocRepair_CharProduct.GrossWeight.Value;

        //Lib.RecalWeight(db, d.StockProductDJ, d.StockProductDJ.StockProductDJ_CharProduct);
        db.SubmitChanges();

        moveStoneLog(d.StockProductDJ, d);
        //moveStone(d, d.StockProductDJ);

        // PricingStock.RepairPricingDJ(d.StockProductDJ.ID); // New Function 2016-01-11
    }

    protected void saveRepairTitipan(DocRepair d)
    {

    }

    protected void RevalProductDJ(StockProductDJ d)
    {
        deleteProcess(d);

        var product = d.StockProductDJ_CharProduct;
        saveStone(d);
        saveFinishing(d, product);

        saveProduct(d, product);
        saveBiaya(d, product);

        saveMU(d, product);
        saveSegmen(d, product);
        saveTotalStone(d);
    }

    protected void moveStoneLog(StockProductDJ ms, DocRepair x)
    {
		try{
        moveLogStone1A(ms, x);
        moveLogStone1B(ms, x);
        moveLogStone2(ms, x);
        moveLogStone3(ms, x);
        moveLogStone4(ms, x);
        moveLogStone5(ms, x);
		}
		catch{}
    }

    protected void moveLogStone1A(StockProductDJ ms, DocRepair x)
    {
        var stone = from p in ms.StockProductDJ_Stone1A
                    orderby p.Stone1A.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairLog_Stone1A n = new DocRepairLog_Stone1A();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.DocRepairLog_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveLogStone1B(StockProductDJ ms, DocRepair x)
    {

        var stone = from p in ms.StockProductDJ_Stone1B
                    orderby p.Stone1B.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairLog_Stone1B n = new DocRepairLog_Stone1B();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;
                n.GIA = r.GIA;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.DocRepairLog_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveLogStone2(StockProductDJ ms, DocRepair x)
    {
        var stone = from p in ms.StockProductDJ_Stone2
                    orderby p.Stone2.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairLog_Stone2 n = new DocRepairLog_Stone2();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.DocRepairLog_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveLogStone3(StockProductDJ ms, DocRepair x)
    {
        var stone = from p in ms.StockProductDJ_Stone3
                    orderby p.Stone3.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairLog_Stone3 n = new DocRepairLog_Stone3();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;
                n.GIA = r.GIA;

                //Commit
                db.DocRepairLog_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveLogStone4(StockProductDJ ms, DocRepair x)
    {
        var stone = from p in ms.StockProductDJ_Stone4
                    orderby p.Stone4.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairLog_Stone4 n = new DocRepairLog_Stone4();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.DocRepairLog_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveLogStone5(StockProductDJ ms, DocRepair x)
    {
        var stone = from p in ms.StockProductDJ_Stone5
                    orderby p.Stone5.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairLog_Stone5 n = new DocRepairLog_Stone5();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.DocRepairLog_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected void moveStone(DocRepair ms, StockProductDJ x)
    {
        moveStone1A(ms, x);
        moveStone1B(ms, x);
        moveStone2(ms, x);
        moveStone3(ms, x);
        moveStone4(ms, x);
        moveStone5(ms, x);
    }

    protected void moveStone1A(DocRepair ms, StockProductDJ x)
    {
        var y = db.DocQCDJ_Stone1A.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone1A.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }
        var z = db.StockProductDJ_Stone1A.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }


        var stone = from p in ms.DocRepair_Stone1A
                    orderby p.Stone1A.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockProductDJ_Stone1A n = new StockProductDJ_Stone1A();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockProductDJ_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone1B(DocRepair ms, StockProductDJ x)
    {
        var y = db.DocQCDJ_Stone1B.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone1B.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }
        var z = db.StockProductDJ_Stone1B.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }


        var stone = from p in ms.DocRepair_Stone1B
                    orderby p.Stone1B.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockProductDJ_Stone1B n = new StockProductDJ_Stone1B();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;
                n.GIA = r.GIA;
                n.StatusTerpasang = 1;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockProductDJ_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone2(DocRepair ms, StockProductDJ x)
    {
        var y = db.DocQCDJ_Stone2.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone2.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }
        var z = db.StockProductDJ_Stone2.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.DocRepair_Stone2
                    orderby p.Stone2.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockProductDJ_Stone2 n = new StockProductDJ_Stone2();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockProductDJ_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone3(DocRepair ms, StockProductDJ x)
    {
        var y = db.DocQCDJ_Stone3.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone3.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }
        var z = db.StockProductDJ_Stone3.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.DocRepair_Stone3
                    orderby p.Stone3.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockProductDJ_Stone3 n = new StockProductDJ_Stone3();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;
                n.GIA = r.GIA;

                //Commit
                db.StockProductDJ_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone4(DocRepair ms, StockProductDJ x)
    {
        var y = db.DocQCDJ_Stone4.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone4.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }
        var z = db.StockProductDJ_Stone4.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.DocRepair_Stone4
                    orderby p.Stone4.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockProductDJ_Stone4 n = new StockProductDJ_Stone4();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockProductDJ_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone5(DocRepair ms, StockProductDJ x)
    {
        var y = db.DocQCDJ_Stone5.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone5.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }
        var z = db.StockProductDJ_Stone5.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.DocRepair_Stone5
                    orderby p.Stone5.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockProductDJ_Stone5 n = new StockProductDJ_Stone5();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockProductDJ_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected string Generate_NomorNew(int Tahun, int Bulan, int TipeLokasi, int IDLokasi )
    {
        string x = string.Empty;        
        int Nomor = 1;

        try
        {

            string queryGetNomor = @"                    

                    select TOP 1 CAST(SUBSTRING(Nomor, 17,6) AS INT)+1 Nomor from DocRepair 
                    where YEAR(Tgl) = @Tahun and month(Tgl) = @Bulan 
                    and Flag = 1 and Nomor IS NOT NULL
                    ORDER BY SUBSTRING(Nomor, 17, 6) DESC                  
                    ";
            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                using (SqlCommand command = new SqlCommand(queryGetNomor, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@Tahun", SqlDbType.Int) { Value = Tahun });
                    command.Parameters.Add(new SqlParameter("@Bulan", SqlDbType.Int) { Value = Bulan });                    

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow item in dt.Rows)
                    {
                        Nomor = Convert.ToInt32(item["Nomor"]);
                    }
                }
                
            }
        
            // if (s.Nomor != null)
            //     Nomor = Convert.ToInt32(s.Nomor.Substring(16, 6)) + 1;
        }
        catch (Exception ex) { }

        string kodeBrandSBU = "CMK";
        string kodeLocation = "0";
        if (TipeLokasi == 0)
        {
            var y = db.LocWarehouse.SingleOrDefault(p => p.ID == IDLokasi);
            kodeBrandSBU = y.CompanyBrand.NamaKode;
            kodeLocation = y.ID.ToString().PadLeft(3, '0');
        }
        else if (TipeLokasi == 1)
        {
            var y = db.LocOutlet.SingleOrDefault(p => p.ID == IDLokasi);
            kodeBrandSBU = y.CompanyBrand.NamaKode;
            kodeLocation = y.ID.ToString().PadLeft(3, '0');
        }
        else if (TipeLokasi == 2)
        {
            var y = db.LocExhibition.SingleOrDefault(p => p.ID == IDLokasi);
            kodeBrandSBU = y.CompanyBrand.NamaKode;
            kodeLocation = y.ID.ToString().PadLeft(3, '0');
        }

        string docid = "RPR"
                     + "/" + kodeBrandSBU // 1+1
                     + "/" + kodeLocation // 1+3
                     + "/" + Tahun.ToString().Remove(0, 2) 
                     + "/" + Bulan.ToString().PadLeft(2, '0') 
                     + "/" + Str.KodeFormat(Nomor, 6)
                     ;

        return docid;

    }

    // protected string Generate_Nomor(DocRepair d)
    // {
    //     string x = string.Empty;

    //     int Tahun = d.Tgl.Value.Year;
    //     int Bulan = d.Tgl.Value.Month;

    //     int Nomor = 1;

    //     try
    //     {
    //         var s = (from p in db.DocRepair
    //                  where p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan
    //                  && p.Nomor != null
    //                  orderby (p.Nomor.Substring(16, 6)) descending
    //                  select p).First();

    //         if (s.Nomor != null)
    //             Nomor = Convert.ToInt32(s.Nomor.Substring(16, 6)) + 1;
    //     }
    //     catch (Exception ex) { }

    //     string kodeBrandSBU = "CMK";
    //     string kodeLocation = "0";
    //     if (d.TipeLokasi == 0)
    //     {
    //         var y = db.LocWarehouse.SingleOrDefault(p => p.ID == d.IDLokasi);
    //         kodeBrandSBU = y.CompanyBrand.NamaKode;
    //         kodeLocation = y.ID.ToString().PadLeft(3, '0');
    //     }
    //     else if (d.TipeLokasi == 1)
    //     {
    //         var y = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
    //         kodeBrandSBU = y.CompanyBrand.NamaKode;
    //         kodeLocation = y.ID.ToString().PadLeft(3, '0');
    //     }
    //     else if (d.TipeLokasi == 2)
    //     {
    //         var y = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
    //         kodeBrandSBU = y.CompanyBrand.NamaKode;
    //         kodeLocation = y.ID.ToString().PadLeft(3, '0');
    //     }

    //     string docid = "RPR"
    //                  + "/" + kodeBrandSBU // 1+1
    //                  + "/" + kodeLocation // 1+3
    //                  + "/" + Tahun.ToString().Remove(0, 2) 
    //                  + "/" + Bulan.ToString().PadLeft(2, '0') 
    //                  + "/" + Str.KodeFormat(Nomor, 6)
    //                  ;

    //     return docid;

    // }


    //REVAL Product

    //Delete
    protected void deleteProcess(StockProductDJ d)
    {
        if (d.StockProductDJ_PricingProduct != null)
            db.StockProductDJ_PricingProduct.DeleteOnSubmit(
                d.StockProductDJ_PricingProduct);

        if (d.StockProductDJ_CostingProduct != null)
            db.StockProductDJ_CostingProduct.DeleteOnSubmit(
                d.StockProductDJ_CostingProduct);

        if (d.StockProductDJ_PricingBiaya != null)
            db.StockProductDJ_PricingBiaya.DeleteOnSubmit(
                d.StockProductDJ_PricingBiaya);

        if (d.StockProductDJ_PricingMU != null)
            db.StockProductDJ_PricingMU.DeleteOnSubmit(
                d.StockProductDJ_PricingMU);

        if (d.StockProductDJ_PricingSegmen != null)
            db.StockProductDJ_PricingSegmen.DeleteOnSubmit(
                d.StockProductDJ_PricingSegmen);

        //Commit
        db.SubmitChanges();
    }

    //Stone Pricing
    protected void saveStone(StockProductDJ d)
    {
        saveStone1A(d);
        saveStone1B(d);
        saveStone2(d);
        saveStone3(d);
        saveStone4(d);
        saveStone5(d);
    }
    protected void saveStone1A(StockProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;
        
        var rs = from p in db.StockProductDJ_Stone1A 
                 where p.IDForm == d.ID
                 select p;
        foreach (var r in rs)
        {

            StonePricing1A prc = r.Stone1A.StonePricing1A;
            StoneCosting1A cost = r.Stone1A.StoneCosting1A;
            if (prc == null || cost == null) PricingError(sebab);
            else
            {
                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone1A.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if(t!=null)
                            {
                            //Harga
                            r.HargaSatuan = t.HargaSatuan;
                            r.HargaTotal = t.HargaTotal;

                            r.HargaSatuanM = cost.Harga.Value;
                            r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                            //Tgl. Efektif
                            r.Efektif = r.Efektif;

                            //Setting
                            r.Setting = r.Setting;
                                }
                        }
                    }*/

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone1B(StockProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone1B where p.IDForm == d.ID && p.StatusTerpasang == 1 select p;
        foreach (var r in rs)
        {

            StonePricing1B prc = r.Stone1B.StonePricing1B;
            StoneCosting1B cost = r.Stone1B.StoneCosting1B;
            if (prc == null || cost == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue || !prc.InputH.HasValue || !prc.InputP.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Rapaport
                    r.HargaInputH = prc.InputH.Value;
                    r.HargaInputP = prc.InputP.Value;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone1B.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                //Rapaport
                                r.HargaInputH = t.HargaInputH;
                                r.HargaInputP = t.HargaInputP;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone2(StockProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(2);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone2 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {
            StonePricing2 prc = r.Stone2.StonePricing2;
            StoneCosting2 cost = r.Stone2.StoneCosting2;
            if (prc == null || cost == null) PricingError(sebab);
            else
            {
                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone2.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone3(StockProductDJ d)
    {

        //Exception-Cause
        string sebab = DL.StoneIndex(3);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone3 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {

            StonePricing3 prc = r.Stone3.StonePricing3;
            StoneCosting3 cost = r.Stone3.StoneCosting3;
            if (prc == null || cost == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone3.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone4(StockProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(4);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone4 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {

            StonePricing4 prc = r.Stone4.StonePricing4;
            StoneCosting4 cost = r.Stone4.StoneCosting4;
            if (prc == null || cost == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone4.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone5(StockProductDJ d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(5);

        decimal OngkosPB = 0m;

        var rs = from p in db.StockProductDJ_Stone5 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {

            StonePricing5 prc = r.Stone5.StonePricing5;
            StoneCosting5 cost = r.Stone5.StoneCosting5;
            if (prc == null || cost == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone5.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }

    //Finishing
    protected void saveFinishing(StockProductDJ d, StockProductDJ_CharProduct product)
    {
        var rs = from p in d.StockProductDJ_Finishing select p;
        foreach (var r in rs)
        {
            string sebab = "";
            bool ok = true;

            //Pricing
            var prc_ff = db.PricingTableFF.SingleOrDefault(p =>
                p.IDItem == product.ProductItem &&
                p.IDFinishing == r.IDFinishing &&
                p.Biaya.HasValue &&
                p.ApprovalTgl.HasValue
                );

            //Pricing - Validasi
            if (prc_ff == null) { ok = false; sebab = "Finishing Fee"; }

            //Process
            if (!ok) PricingError(sebab);
            else
            {
                r.Biaya = prc_ff.Biaya.Value;
                r.Efektif = prc_ff.ApprovalTgl.Value;

                /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                {
                    var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                    if (s != null)
                    {
                        var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Finishing.SingleOrDefault(p => p.IDFinishing == r.IDFinishing);
                        if (t != null)
                        {
                            r.Biaya = t.Biaya;
                            r.Efektif = t.Efektif;
                        }
                    }
                }*/

                //Commit
                db.SubmitChanges();
            }
        }
    }

    //Product
    protected void saveProduct(StockProductDJ d, StockProductDJ_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_PG1_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_PG1_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_tg == null || prc_tgM == null) { ok = false; sebab = "Today Gold Price"; }

        //Process
        if (!ok) PricingError(sebab);
        else
        {
            StockProductDJ_PricingProduct n = new StockProductDJ_PricingProduct();

            n.ID = d.ID;

            //Gold Price
            n.GoldRate = prc_tg.Harga.Value;
            n.EfektifGold = prc_tg.ApprovalTgl.Value;

            //Informasi Berat
            n.Netto = product.NettoWeight;
            n.Kadar = product.KadarTukaran;

            //Total
            n.TotalProduct = n.GoldRate * n.Netto * (n.Kadar / (decimal)100);
            n.TotalStone1A = Convert.ToDecimal(TotalStone1A(d));
            n.TotalStone1B = Convert.ToDecimal(TotalStone1B(d));
            n.TotalStone2 = Convert.ToDecimal(TotalStone2(d));
            n.TotalStone3 = Convert.ToDecimal(TotalStone3(d));
            n.TotalStone4 = Convert.ToDecimal(TotalStone4(d));
            n.TotalStone5 = Convert.ToDecimal(TotalStone5(d));
            n.Total = n.TotalProduct + Convert.ToDecimal(n.TotalStone1A) + Convert.ToDecimal(n.TotalStone1B) + Convert.ToDecimal(n.TotalStone2) + Convert.ToDecimal(n.TotalStone3) + Convert.ToDecimal(n.TotalStone4) + Convert.ToDecimal(n.TotalStone5);

            //Commit
            db.StockProductDJ_PricingProduct.InsertOnSubmit(n);
            db.SubmitChanges();

            /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
            {
                var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (s != null)
                {
                    var u = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == n.ID);
                    //Gold Price
                    n.GoldRate = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingProduct.GoldRate;
                    n.EfektifGold = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingProduct.EfektifGold;
                }
            }*/

            //Costing
            StockProductDJ_CostingProduct m = new StockProductDJ_CostingProduct();

            m.ID = d.ID;

            //Gold Price
            m.GoldRate = prc_tgM.Harga.Value;
            m.EfektifGold = prc_tgM.ApprovalTgl.Value;

            //Informasi Berat
            m.Netto = product.NettoWeight;
            m.Kadar = product.KadarTukaran;

            //Total
            m.TotalProduct = m.GoldRate * n.Netto * (n.Kadar / (decimal)100);
            m.TotalStone1A = Convert.ToDecimal(TotalMStone1A(d));
            m.TotalStone1B = Convert.ToDecimal(TotalMStone1B(d));
            m.TotalStone2 = Convert.ToDecimal(TotalMStone2(d));
            m.TotalStone3 = Convert.ToDecimal(TotalMStone3(d));
            m.TotalStone4 = Convert.ToDecimal(TotalMStone4(d));
            m.TotalStone5 = Convert.ToDecimal(TotalMStone5(d));
            m.Total = m.TotalProduct + Convert.ToDecimal(m.TotalStone1A) + Convert.ToDecimal(m.TotalStone1B) + Convert.ToDecimal(m.TotalStone2) + Convert.ToDecimal(m.TotalStone3) + Convert.ToDecimal(m.TotalStone4) + Convert.ToDecimal(m.TotalStone5);

            //Commit
            db.StockProductDJ_CostingProduct.InsertOnSubmit(m);
            db.SubmitChanges();
        }
    }

    //Biaya
    protected void saveBiaya(StockProductDJ d, StockProductDJ_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
            p.IDBrand == d.LocWarehouse.IDBrand &&
            p.IDItem == product.ProductItem &&
            p.IDCons == product.ProcessCons &&
            p.Biaya.HasValue &&
            p.Pembagi.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_fs = db.PricingTableFS.SingleOrDefault(p =>
            p.ID == FS_ID(d, product) &&
            p.Biaya.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_fm == null) { ok = false; sebab = "Mounting Fee"; }
        if (prc_fs == null) { ok = false; sebab = "Setting Fee"; }
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (!ok) PricingError(sebab);
        else
        {
            StockProductDJ_PricingBiaya n = new StockProductDJ_PricingBiaya();

            n.ID = d.ID;

            //Informasi
            n.NamaBrand = d.LocWarehouse.CompanyBrand.Nama;
            n.NamaItem = product.CharProductItem.Nama;
            n.NamaCons = product.CharProcessCons.Nama;

            //Mounting
            n.Mounting = prc_fm.Biaya.Value;
            n.MountingNO = prc_fm.InputNO.Value;
            n.MountingRO = prc_fm.InputRO.Value;

            //Setting
            //n.Setting = prc_fs.Biaya.Value;
            /*Sementara di Hardcode*/
            n.Setting = Setting;

            /*end hardcode*/



            //Finishing
            n.Finishing = TotalFinishing(d);

            //Total
            n.TotalRupiah = n.Mounting + n.Setting + n.Finishing;
            n.TotalRate = prc_us.Harga.Value;
            n.Total = n.TotalRupiah / n.TotalRate;

            //Tgl. Efektif
            n.EfektifMounting = prc_fm.ApprovalTgl.Value;
            n.EfektifSetting = prc_fs.ApprovalTgl.Value;
            n.EfektifRate = prc_us.ApprovalTgl.Value;

            /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
            {
                var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (s != null)
                {
                    n.Mounting = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.Mounting;
                    n.MountingNO = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.MountingNO;
                    n.MountingRO = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.MountingRO;

                    //Setting
                    //n.Setting = prc_fs.Biaya.Value;
                    n.Setting = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.Setting;



                    //Finishing
                    n.Finishing = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.Finishing;

                    //Total
                    n.TotalRupiah = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.TotalRupiah;
                    n.TotalRate = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.TotalRate;
                    n.Total = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.Total;

                    //Tgl. Efektif
                    n.EfektifMounting = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.EfektifMounting;
                    n.EfektifSetting = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.EfektifSetting;
                    n.EfektifRate = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.EfektifRate;
                }
            }*/


            //Commit
            db.StockProductDJ_PricingBiaya.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected int FS_ID(StockProductDJ d, StockProductDJ_CharProduct product)
    {
        return 1; //Masih di-hardcoded. BELUM SELESAI !
    }

    //Mark-Up
    protected void saveMU(StockProductDJ d, StockProductDJ_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
            p.IDCategory == product.ProductCategory &&
            p.IDLevel == product.ProductLevel &&
            p.IDDist == product.StoneDist &&
            p.Persen.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_mu == null) { ok = false; sebab = "Diamond Jewelry MU"; }
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (!ok) PricingError(sebab);
        else
        {
            StockProductDJ_PricingMU n = new StockProductDJ_PricingMU();

            n.ID = d.ID;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            //Modal
            n.ModalUSD = TotalModal(d.ID);
            
            decimal rate = prc_us.Harga.Value;
            DateTime efektif_rate = prc_us.ApprovalTgl.Value;
            if(Db.SingleBool("SELECT dbo.fx_isFashionRate(" + product.ID + ", 0) ", Db.Cnn_DB))
            {
                rate = Db.SingleDecimal("SELECT Harga From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
                efektif_rate = Db.SingleTime("SELECT ApprovalTgl From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
            }
            n.ModalRate = rate;
			n.ModalRupiah = n.ModalUSD * n.ModalRate;

            //Mark-Up
            n.MU = prc_mu.Persen.Value;

            decimal OngkosLain = 0m;

            try
            {
                OngkosLain = d.StockProductDJ_Costing.OngkosLain.Value;
            }
            catch (Exception ex)
            {
                OngkosLain = 0m;
            }

            if(product.ProductCategory == 1 || product.ProductCategory == 8)
            {
                n.ModalRate = Db.SingleDecimal("SELECT Harga From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
                n.EfektifRate = Db.SingleTime("SELECT ApprovalTgl From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
            }

            //Harga Final
            //n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + (OngkosLain / d.StockProductDJ_Costing.TotalRate.Value);
            n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d) + (OngkosLain / d.StockProductDJ_Costing.TotalRate.Value);
            n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * n.ModalRate + OngkosLain;
            n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * n.ModalRate + OngkosLain);

            //Tgl. Efektif
            n.EfektifRate = prc_us.ApprovalTgl.Value;
            /*if (d.Proto != null && d.Proto!=0 && d.RequestCustomerNama != null)
            {
                var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (s != null)
                {
                    n.HargaUSD = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingMU.HargaUSD;
                    n.HargaRupiah = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingMU.HargaUSD;
                    n.HargaRupiahRound = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingMU.HargaRupiahRound;
					//Tgl. Efektif
					n.EfektifRate = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingMU.EfektifRate;
                }
            }*/





            //Commit
            db.StockProductDJ_PricingMU.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }

    //Segmen System
    protected void saveSegmen(StockProductDJ d, StockProductDJ_CharProduct product)
    {
        string sebab = "Segmen System";
        bool ok = true;

        //Pricing - Validasi
        ok = checkSegmen(product);

        //Process
        if (!ok) PricingError(sebab);
        else
        {
            //Product Value
            decimal harga = HargaUSD(d.ID);
            //decimal carat = product.StoneCarat;

            decimal carat = 0m;

            try
            {
                carat = (from p in db.StockProductDJ_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone1B where p.IDForm == d.ID && p.StatusTerpasang == 1 select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
            }
            catch (Exception ex) { }

            //Parameter
            int IDSegmen = 0;
            int SegmenSistem = 0;
            decimal SegmenMinimum = 0;
            DateTime EfektifSegmen = DateTime.Now;

            //SELECT
            var rs = from p in db.PricingTableSS
                     where
                         p.IDCategory == product.ProductCategory &&
                         p.IDLevel == product.ProductLevel &&
                         p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                     orderby p.Minimum
                     select p;

            foreach (var r in rs)
            {
                if ((r.SegmenSistem == 0 && harga >= r.Minimum.Value) ||
                    (r.SegmenSistem == 1 && carat >= r.Minimum.Value))
                {

                    IDSegmen = r.IDSegmen;
                    SegmenSistem = r.SegmenSistem;
                    SegmenMinimum = r.Minimum.Value;
                    EfektifSegmen = r.ApprovalTgl.Value;
                }
            }

            StockProductDJ_PricingSegmen n = new StockProductDJ_PricingSegmen();

            n.ID = d.ID;
            n.ProductSegmen = IDSegmen;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            //Segmen
            n.SegmenSistem = SegmenSistem;
            n.SegmenMinimum = SegmenMinimum;

            //Harga dan Carat
            n.HargaUSD = harga;
            n.TotalCarat = carat;

            //Tgl. Efektif
            n.EfektifSegmen = EfektifSegmen;

            ///Commit
            db.StockProductDJ_PricingSegmen.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected bool checkSegmen(StockProductDJ_CharProduct product)
    {
        bool ok = true;

        var rs = from p in db.PricingTableSS
                 where
                     p.IDCategory == product.ProductCategory &&
                     p.IDLevel == product.ProductLevel &&
                     p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                 select p;

        foreach (var r in rs)
        {
            if (!r.Minimum.HasValue || !r.ApprovalTgl.HasValue)
            {
                ok = false;
                break;
            }
        }

        return ok;
    }

    //Sub-Total
    protected decimal TotalStone(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
        foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    //Sub-Total
    protected decimal TotalStone1A(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_1A = from p in db.StockProductDJ_Stone1A where p.IDForm == d.ID && p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1A) total += r;
        }
        catch (Exception ex) { }
        return total;
    }
    protected decimal TotalStone1B(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_1B = from p in db.StockProductDJ_Stone1B where p.IDForm == d.ID && p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
            foreach (var r in rs_1B) total += r;
        }
        catch (Exception ex) { }
        return total;
    }
    protected decimal TotalStone2(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_2 = from p in db.StockProductDJ_Stone2 where p.IDForm == d.ID && p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_2) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone3(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_3 = from p in db.StockProductDJ_Stone3 where p.IDForm == d.ID && p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_3) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone4(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_4 = from p in db.StockProductDJ_Stone4 where p.IDForm == d.ID && p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_4) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone5(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_5 = from p in db.StockProductDJ_Stone5 where p.IDForm == d.ID && p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_5) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalMStone1A(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in db.StockProductDJ_Stone1A where p.IDForm == d.ID && p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected decimal TotalMStone1B(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in db.StockProductDJ_Stone1B where p.IDForm == d.ID && p.HargaTotalM.HasValue && p.StatusTerpasang == 1 select p.HargaTotalM.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected decimal TotalMStone2(StockProductDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in db.StockProductDJ_Stone2 where p.IDForm == d.ID && p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected decimal TotalMStone3(StockProductDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in db.StockProductDJ_Stone3 where p.IDForm == d.ID && p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected decimal TotalMStone4(StockProductDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in db.StockProductDJ_Stone4 where p.IDForm == d.ID && p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected decimal TotalMStone5(StockProductDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in db.StockProductDJ_Stone5 where p.IDForm == d.ID && p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    protected decimal TotalFinishing(StockProductDJ d)
    {
        decimal total = 0;

        var rs = from p in db.StockProductDJ_Finishing where p.IDForm == d.ID && p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }
    protected decimal TotalModal(int ID)
    {
        decimal total = 0;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
        total += biaya.Total;

        return total;
    }
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        var stone3 = from p in db.StockProductDJ_Stone3
                     where p.IDForm == ID && (p.GIA == "" || p.GIA == null)
                     select p;
        decimal Stone3ToMarkUp = 0m;
        try
        {
            Stone3ToMarkUp = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp);
        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(Stone3ToMarkUp);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
    protected decimal TotalStone3NMU(int ID)
    {
        decimal total = 0;

        var stone3 = from p in db.StockProductDJ_Stone3
                     where p.IDForm == ID && p.GIA != ""
                     select p;
        try
        {
            total = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal HargaUSD(int ID)
    {
        var mu = db.StockProductDJ_PricingMU.SingleOrDefault(p => p.ID == ID);
        return mu.HargaUSD;
    }
    protected void saveTotalStone(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A select p.TotalButir;
        foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.StockProductDJ_Stone1B where p.StatusTerpasang == 1 select p.TotalButir;
        foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.StockProductDJ_Stone2 select p.TotalButir;
        foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.StockProductDJ_Stone3 select p.TotalButir;
        foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.StockProductDJ_Stone4 select p.TotalButir;
        foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.StockProductDJ_Stone5 select p.TotalButir;
        foreach (var r in rs_5) total += r;

        d.StockProductDJ_CharProduct.StoneQty = total;
        db.SubmitChanges();

    }

    protected void moveItem(DocRepairResult d)
    {
        var z = db.DocRepairResult_Repair.Where(p => p.IDForm == d.IDWorkOrder);
        if (z != null)
        {
            db.DocRepairResult_Repair.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var item = from p in d.DocRepair.DocRepair_Repair
                   orderby p.CharProcessRepair.Nama
                   select p;
        if (item != null)
        {
            foreach (var r in item)
            {
                DocRepairResult_Repair n = new DocRepairResult_Repair();

                n.IDForm = d.ID;
                n.Biaya = r.Biaya;
                n.Efektif = r.Efektif;
                n.IDRepair = r.IDRepair;
                n.Keterangan = r.Keterangan;

                //Commit
                db.DocRepairResult_Repair.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected string Generate_Nomor(DocRepairResult d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Value.Year;
        int Bulan = d.Tgl.Value.Month;

        int Nomor = 1;

        try
        {
            var s = (from p in db.DocRepairResult
                     where p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(14, 6)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(14, 6)) + 1;
        }
        catch (Exception ex) { }

        string docid = "INV/RPR/" + Tahun.ToString().Remove(0, 2) + "/" + Bulan.ToString().PadLeft(2, '0') + "/" + Str.KodeFormat(Nomor, 6);

        return docid;

    }

    protected string Generate_Nomor(DocTitipan j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.Titipan.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;

        int Nomor = 1;

        try
        {
            var s = (from p in db.DocTitipan
                     where p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan
                     && p.Nomor.Substring(3, 1) == "" + j.TipeLokasi.ToString() + ""
                     && p.Nomor.Substring(5, 3) == "" + j.IDLokasi.ToString().PadLeft(3, '0') + ""
                     orderby p.ID descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(15, 4)) + 1;
        }
        catch (Exception ex) { }

        return "TK/" + j.TipeLokasi.ToString() + "/" + j.IDLokasi.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }
}