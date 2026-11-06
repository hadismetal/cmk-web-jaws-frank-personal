using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Repair_Submit2 : System.Web.UI.Page
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
        bool isRepairStock = true; // true : repair stock, false : repair toko
        decimal SelisihBerat = 0m;

        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            //lit_customer.Text = d.DataCustomer.Nama;
            //lit_jpjr.Text = d.DataSales.Nama;
            lit_tgl.Text = Str.FullDate(d.Tgl);
            lit_tgl2.Text = Str.FullDate(d.TglSelesai);
            lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);

            if (d.IDProduct != null) { lit_plu.Text = d.StockProductDJ.Nomor; }
            else if (d.IDProductTitipan != null) { lit_plu.Text = d.StockProductDJCustomer.Nomor; }

            var d_item_list = from p in db.DocRepair_Repair where p.IDForm == myID select p;

            var product = d.DocRepair_CharProduct;

            productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";

            //Picture Repair
            Upload.Image(imgpicturerepair, product.DocRepair.ImgPicture);

            if (d.IDProduct != null)
            {
                var productold = d.StockProductDJ.StockProductDJ_CharProduct;
                if (product == null) save.Disabled = true;
                else
                {
                    SelisihBerat = (decimal)product.GrossWeight.Value - (decimal)productold.GrossWeight;

                    grossweight.InnerHtml = (product.GrossWeight != productold.GrossWeight ? Str.Number2(productold.GrossWeight) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.GrossWeight);
                    diameter.InnerHtml = (product.DimensiD != productold.DimensiD ? Str.Number2(productold.DimensiD) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiD);
                    panjang.InnerHtml = (product.DimensiP != productold.DimensiP ? Str.Number2(productold.DimensiP) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiP);
                    lebar.InnerHtml = (product.DimensiL != productold.DimensiL ? Str.Number2(productold.DimensiL) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiL);
                    ringsize.InnerHtml = (product.DimensiR != productold.DimensiR ? Str.Number2(productold.DimensiR) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiR);

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

                    grossweight.InnerHtml = (product.GrossWeight != productold.GrossWeight ? Str.Number2(productold.GrossWeight) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.GrossWeight);
                    diameter.InnerHtml = (product.DimensiD != productold.DimensiD ? Str.Number2(productold.DimensiD) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiD);
                    panjang.InnerHtml = (product.DimensiP != productold.DimensiP ? Str.Number2(productold.DimensiP) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiP);
                    lebar.InnerHtml = (product.DimensiL != productold.DimensiL ? Str.Number2(productold.DimensiL) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiL);
                    ringsize.InnerHtml = (product.DimensiR != productold.DimensiR ? Str.Number2(productold.DimensiR) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiR);

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

                repairproductitem.InnerText = (is_InvalidItem && !isRepairStock) ? "Invalid" : ((is_SmallItem) ? "Small" : "Big");

                if (is_SmallItem) { HargaEmas = (SelisihBerat - 0.5m) > 0 ? ((SelisihBerat - 0.5m) * prc_rpr.Harga.Value) : 0m; }
                else { HargaEmas = (SelisihBerat - 1m) > 0 ? ((SelisihBerat - 1m) * prc_rpr.Harga.Value) : 0m; }

                OngkosRepair = d.DocRepair_Repair.Sum(p => p.Biaya.Value);

                TotalBiaya = OngkosRepair + HargaBatu + HargaEmas;

                estimasi.InnerHtml = Str.Number0(TotalBiaya);

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

    protected void ProductError() { Response.Redirect("Repair_Data.aspx?id=" + myID); }

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
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            saveRepair(d, true);

            //Status
            d.Draft = false;
            d.Nomor = Generate_Nomor(d);
            d.DraftDate = null;
            //if (d.IDProduct.HasValue)
            //    d.StockProductDJ.StockActualDJ.Substorage = 5;

            //Commit
            db.SubmitChanges();

            //Redirect
            Response.Redirect("Repair_Print.aspx?id=" + myID);
        }
        //Response.Redirect("Repair_Pricing.aspx?id=" + myID);
    }
	//Save
    protected void save1_Click(object sender, EventArgs e)
    {
		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            saveRepair(d, false);

            //Status
            d.Draft = false;
            d.Nomor = Generate_Nomor(d);
            d.DraftDate = null;
            //if (d.IDProduct.HasValue)
            //    d.StockProductDJ.StockActualDJ.Substorage = 5;

            //Commit
            db.SubmitChanges();

            //Redirect
            //Response.Redirect("Repair_Print.aspx?id=" + myID);
            string pageurl2 = "/StockRepair/Page/Report/Repair_Print2.aspx?id=" + myID;
            Response.Write("<script>");
            Response.Write("window.open('" + pageurl2 + "','_blank')");
            Response.Write("</script>");

            string pageurl = "/StockRepair/Page/Report/Repair_Print.aspx?id=" + myID;
            Response.Write("<script>");
            Response.Write("window.location.href='" + pageurl + "'");
            Response.Write("</script>");
            //Response.Redirect("Repair_Print.aspx?id=" + myID);

        }
        //Response.Redirect("Repair_Pricing.aspx?id=" + myID);
	}

    protected void saveRepair(DocRepair d, bool issavetoStock = true)
    {
        /// 1. Hitung ongkos
        /// 2. DocRepair & StockProduct -> DocRepairResult
        /// 3. Stock Ledger
        /// 4. Move Stone
        /// 5. Recal Weight
        /// 6. Reval Price

        /// 1. Hitung ongkos
        decimal OngkosRepair = 0m;
        try
        {
            OngkosRepair = d.DocRepair_Repair.Sum(p => p.Biaya.Value);
        }
        catch (Exception ex) { }

        /// 2. DocRepair -> DocRepairResult
        DocRepairResult n = new DocRepairResult();

        //Location
        n.IDLokasi = d.IDLokasi;
        n.TipeLokasi = d.TipeLokasi;
        n.IDProduct = d.IDProduct;

        n.Tgl = d.Tgl;
        n.Draft = false;
        n.TglSelesai = d.TglSelesai;
		if(issavetoStock == true){
			n.StatusRepair = 2;
		}
		else{
			n.StatusRepair = 1;
		}
        n.ImgPicture = d.ImgPicture;
        n.IDWorkOrder = d.ID;
        n.TotalHarga = OngkosRepair;
        n.Sumber = d.Sumber;

        //User-Info
        n.Operator = App.UserNama;
        n.OperatorTgl = DateTime.Now;
        //n.StatusRepair = 2;

        //Commit
        db.DocRepairResult.InsertOnSubmit(n);
        db.SubmitChanges();

        // Ongkos
        moveItem(n);

        var s = db.DocRepair_CharProduct.SingleOrDefault(p => p.ID == myID);

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

        m.StoneCaratOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.StoneCarat;
        m.StoneWeightOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.StoneWeight;
        m.StoneQtyOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.StoneQty;
        m.GrossWeightOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.GrossWeight;
        m.NettoWeightOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;
        m.KadarLogamOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.KadarLogam;
        m.DimensiDOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.DimensiD;
        m.DimensiPOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.DimensiP;
        m.DimensiLOld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.DimensiL;
        m.DimensiROld = s.DocRepair.StockProductDJ.StockProductDJ_CharProduct.DimensiR;

        //Commit
        db.DocRepairResult_CharProduct.InsertOnSubmit(m);
        db.SubmitChanges();

        n.Nomor = Generate_Nomor(n);
        db.SubmitChanges();

        /// 3. Stock Ledger
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

        /// select repair result
        var o = db.DocRepairResult.SingleOrDefault(p => p.ID == m.ID);
        var q = db.DocRepairResult_CharProduct.SingleOrDefault(p => p.ID == m.ID);

        // Recal RepairResult
        //Lib.RecalWeightRepairResult(db, o, q); // id RepairResult
        //db.SubmitChanges();
		
		if(issavetoStock == true)
		{
			/// 4. Move CharProduct
			var x = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProduct);
			var y = x.StockProductDJ_CharProduct;
			y.GrossWeight = Convert.ToDecimal(m.GrossWeight);
			y.StoneCarat = Convert.ToDecimal(m.StoneCarat);
			y.StoneWeight = Convert.ToDecimal(m.StoneWeight);
			y.StoneQty = Convert.ToDecimal(m.StoneQty);
			y.NettoWeight = Convert.ToDecimal(m.NettoWeight);
			y.KadarLogam = Convert.ToDecimal(m.KadarLogam);
			y.KadarTukaran = Convert.ToDecimal(m.KadarTukaran);
			y.DimensiD = Convert.ToDecimal(m.DimensiD);
			y.DimensiL = Convert.ToDecimal(m.DimensiL);
			y.DimensiP = Convert.ToDecimal(m.DimensiP);
			y.DimensiR = Convert.ToDecimal(m.DimensiR);
			db.SubmitChanges();

			/// 5. Move Stone : Pemindahan batu dari dokumen repair ke stock
			moveStone(d, x, o);
		}
		else
		{
			/// 4. Move CharProduct
			var x = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProduct);
			moveStoneLog(x,d);
		}

        /// 6. Save Stone & Reval Product Price : Hitung ulang keseluruhan harga barang pada tabel stock
        Pricing.RepairPricingDJ(d.StockProductDJ.ID); // New Function 2016-01-11
        
    }

    protected void moveStone(DocRepair ms, StockProductDJ x, DocRepairResult n)
    {
        moveStone1A(ms, x, n);
        moveStone1B(ms, x, n);
        moveStone2(ms, x, n);
        moveStone3(ms, x, n);
        moveStone4(ms, x, n);
        moveStone5(ms, x, n);
    }
    protected void moveStone1A(DocRepair ms, StockProductDJ x, DocRepairResult nrr)
    {
        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        // 2. DocRepair_Stone -> DocRepairResult_Stone
        // 3. DocRepair_Stone -> StockProductDJ_Stone

        var y = db.DocQCDJ_Stone1A.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone1A.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }

        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        var product = from p in ms.StockProductDJ.StockProductDJ_Stone1A
                    orderby p.Stone1A.Nama
                    select p;
        if (product != null)
        {
            foreach (var r in product)
            {
                DocRepairLog_Stone1A n = new DocRepairLog_Stone1A();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.DocRepairLog_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 2. DocRepair_Stone -> DocRepairResult_Stone
        var repair = from p in ms.DocRepair_Stone1A
                      orderby p.Stone1A.Nama
                      select p;
        if (repair != null)
        {
            foreach (var r in repair)
            {
                DocRepairResult_Stone1A n = new DocRepairResult_Stone1A();

                n.IDForm = nrr.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;


                //Commit
                db.DocRepairResult_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 3. DocRepair_Stone -> StockProductDJ_Stone
        var z = db.StockProductDJ_Stone1A.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var result = from p in ms.DocRepair_Stone1A
                    orderby p.Stone1A.Nama
                    select p;
        if (result != null)
        {
            foreach (var r in result)
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

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;


                //Commit
                db.StockProductDJ_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone1B(DocRepair ms, StockProductDJ x, DocRepairResult nrr)
    {
        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        // 2. DocRepair_Stone -> DocRepairResult_Stone
        // 3. DocRepair_Stone -> StockProductDJ_Stone

        var y = db.DocQCDJ_Stone1B.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone1B.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }

        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        var product = from p in ms.StockProductDJ.StockProductDJ_Stone1B
                      orderby p.Stone1B.Nama
                      select p;
        if (product != null)
        {
            foreach (var r in product)
            {
                DocRepairLog_Stone1B n = new DocRepairLog_Stone1B();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                n.HargaInputH = r.HargaInputH;
                n.HargaInputP = r.HargaInputP;

                //Commit
                db.DocRepairLog_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 2. DocRepair_Stone -> DocRepairResult_Stone
        var repair = from p in ms.DocRepair_Stone1B
                     orderby p.Stone1B.Nama
                     select p;
        if (repair != null)
        {
            foreach (var r in repair)
            {
                DocRepairResult_Stone1B n = new DocRepairResult_Stone1B();

                n.IDForm = nrr.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                n.HargaInputH = r.HargaInputH;
                n.HargaInputP = r.HargaInputP;

                //Commit
                db.DocRepairResult_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 3. DocRepair_Stone -> StockProductDJ_Stone
        var z = db.StockProductDJ_Stone1B.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var result = from p in ms.DocRepair_Stone1B
                     orderby p.Stone1B.Nama
                     select p;
        if (result != null)
        {
            foreach (var r in result)
            {
                StockProductDJ_Stone1B n = new StockProductDJ_Stone1B();

                n.GIA = r.GIA;

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                n.HargaInputH = r.HargaInputH;
                n.HargaInputP = r.HargaInputP;

                n.StatusTerpasang = r.StatusTerpasang;


                //Commit
                db.StockProductDJ_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone2(DocRepair ms, StockProductDJ x, DocRepairResult nrr)
    {
        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        // 2. DocRepair_Stone -> DocRepairResult_Stone
        // 3. DocRepair_Stone -> StockProductDJ_Stone

        var y = db.DocQCDJ_Stone2.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone2.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }

        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        var product = from p in ms.StockProductDJ.StockProductDJ_Stone2
                      orderby p.Stone2.Nama
                      select p;
        if (product != null)
        {
            foreach (var r in product)
            {
                DocRepairLog_Stone2 n = new DocRepairLog_Stone2();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;


                //Commit
                db.DocRepairLog_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 2. DocRepair_Stone -> DocRepairResult_Stone
        var repair = from p in ms.DocRepair_Stone2
                     orderby p.Stone2.Nama
                     select p;
        if (repair != null)
        {
            foreach (var r in repair)
            {
                DocRepairResult_Stone2 n = new DocRepairResult_Stone2();

                n.IDForm = nrr.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;


                //Commit
                db.DocRepairResult_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 3. DocRepair_Stone -> StockProductDJ_Stone
        var z = db.StockProductDJ_Stone2.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var result = from p in ms.DocRepair_Stone2
                     orderby p.Stone2.Nama
                     select p;
        if (result != null)
        {
            foreach (var r in result)
            {
                StockProductDJ_Stone2 n = new StockProductDJ_Stone2();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.StockProductDJ_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone3(DocRepair ms, StockProductDJ x, DocRepairResult nrr)
    {
        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        // 2. DocRepair_Stone -> DocRepairResult_Stone
        // 3. DocRepair_Stone -> StockProductDJ_Stone

        var y = db.DocQCDJ_Stone3.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone3.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }

        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        var product = from p in ms.StockProductDJ.StockProductDJ_Stone3
                      orderby p.Stone3.Nama
                      select p;
        if (product != null)
        {
            foreach (var r in product)
            {
                DocRepairLog_Stone3 n = new DocRepairLog_Stone3();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.DocRepairLog_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 2. DocRepair_Stone -> DocRepairResult_Stone
        var repair = from p in ms.DocRepair_Stone3
                     orderby p.Stone3.Nama
                     select p;
        if (repair != null)
        {
            foreach (var r in repair)
            {
                DocRepairResult_Stone3 n = new DocRepairResult_Stone3();

                n.IDForm = nrr.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.DocRepairResult_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 3. DocRepair_Stone -> StockProductDJ_Stone
        var z = db.StockProductDJ_Stone3.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var result = from p in ms.DocRepair_Stone3
                     orderby p.Stone3.Nama
                     select p;
        if (result != null)
        {
            foreach (var r in result)
            {
                StockProductDJ_Stone3 n = new StockProductDJ_Stone3();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.StockProductDJ_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone4(DocRepair ms, StockProductDJ x, DocRepairResult nrr)
    {
        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        // 2. DocRepair_Stone -> DocRepairResult_Stone
        // 3. DocRepair_Stone -> StockProductDJ_Stone

        var y = db.DocQCDJ_Stone4.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone4.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }

        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        var product = from p in ms.StockProductDJ.StockProductDJ_Stone4
                      orderby p.Stone4.Nama
                      select p;
        if (product != null)
        {
            foreach (var r in product)
            {
                DocRepairLog_Stone4 n = new DocRepairLog_Stone4();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.DocRepairLog_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 2. DocRepair_Stone -> DocRepairResult_Stone
        var repair = from p in ms.DocRepair_Stone4
                     orderby p.Stone4.Nama
                     select p;
        if (repair != null)
        {
            foreach (var r in repair)
            {
                DocRepairResult_Stone4 n = new DocRepairResult_Stone4();

                n.IDForm = nrr.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.DocRepairResult_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 3. DocRepair_Stone -> StockProductDJ_Stone
        var z = db.StockProductDJ_Stone4.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var result = from p in ms.DocRepair_Stone4
                     orderby p.Stone4.Nama
                     select p;
        if (result != null)
        {
            foreach (var r in result)
            {
                StockProductDJ_Stone4 n = new StockProductDJ_Stone4();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.StockProductDJ_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone5(DocRepair ms, StockProductDJ x, DocRepairResult nrr)
    {
        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        // 2. DocRepair_Stone -> DocRepairResult_Stone
        // 3. DocRepair_Stone -> StockProductDJ_Stone

        var y = db.DocQCDJ_Stone5.Where(p => p.DocQCDJ.IDProduct == ms.IDProduct);
        if (y != null)
        {
            db.DocQCDJ_Stone5.DeleteAllOnSubmit(y);
            db.SubmitChanges();
        }

        // 1. StockProductDJ_Stone -> DocRepairLog_Stone
        var product = from p in ms.StockProductDJ.StockProductDJ_Stone5
                      orderby p.Stone5.Nama
                      select p;
        if (product != null)
        {
            foreach (var r in product)
            {
                DocRepairLog_Stone5 n = new DocRepairLog_Stone5();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.DocRepairLog_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 2. DocRepair_Stone -> DocRepairResult_Stone
        var repair = from p in ms.DocRepair_Stone5
                     orderby p.Stone5.Nama
                     select p;
        if (repair != null)
        {
            foreach (var r in repair)
            {
                DocRepairResult_Stone5 n = new DocRepairResult_Stone5();

                n.IDForm = nrr.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.DocRepairResult_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        // 3. DocRepair_Stone -> StockProductDJ_Stone
        var z = db.StockProductDJ_Stone5.Where(p => p.IDForm == ms.IDProduct);
        if (z != null)
        {
            db.StockProductDJ_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var result = from p in ms.DocRepair_Stone5
                     orderby p.Stone5.Nama
                     select p;
        if (result != null)
        {
            foreach (var r in result)
            {
                StockProductDJ_Stone5 n = new StockProductDJ_Stone5();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                n.HargaSatuan = r.HargaSatuan;
                n.HargaTotal = r.HargaTotal;
                n.HargaSatuanM = r.HargaSatuanM;
                n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;

                //Commit
                db.StockProductDJ_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
	
	protected void moveStoneLog(StockProductDJ ms, DocRepair x)
    {
        moveLogStone1A(ms, x);
        moveLogStone1B(ms, x);
        moveLogStone2(ms, x);
        moveLogStone3(ms, x);
        moveLogStone4(ms, x);
        moveLogStone5(ms, x);
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

    protected string Generate_Nomor(DocRepair d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Value.Year;
        int Bulan = d.Tgl.Value.Month;

        int Nomor = 1;

        try
        {
            var s = (from p in db.DocRepair
                     where p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(10, 6)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(10, 6)) + 1;
        }
        catch (Exception ex) { }

        string docid = "RPR/" + Tahun.ToString().Remove(0, 2) + "/" + Bulan.ToString().PadLeft(2, '0') + "/" + Str.KodeFormat(Nomor, 6);

        return docid;

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
}