using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Repair_Submit : System.Web.UI.Page {
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
    protected bool isRepairStock = false; // true : repair stock, false : repair toko
        
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
            App.NumberFormat(discount);
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("RepairInvoice.aspx"); }

	//Form
	protected void fillForm() 
    {
        decimal SelisihBerat = 0m;

		var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

            lit_customer.Text = d.DataCustomer.Nama;
            lit_jpjr.Text = d.DataSales.Nama;
            lit_tgl.Text = Str.FullDate(d.Tgl);
            lit_tgl2.Text = Str.FullDate(d.TglSelesai);
            lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);

            if (d.IDProduct != null) { span_plu.InnerText = d.StockProductDJ.Nomor; }
            else if (d.IDProductTitipan != null) { span_plu.InnerText = d.StockProductDJCustomer.Nomor; }

            var d_item_list = from p in db.DocRepair_Repair where p.IDForm == myID select p;

            var product = d.DocRepair.DocRepair_CharProduct;
            var productresult = d.DocRepairResult_CharProduct;

            span_productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";

            //Picture Repair
            Upload.Image(imgpicturerepair, product.DocRepair.ImgPicture);

            // insert jumlah total batu yang lama ke tabel result repair old stone qty
            d.DocRepairResult_CharProduct.StoneCaratOld = (d.IDProduct != null ? d.StockProductDJ.StockProductDJ_CharProduct.StoneCarat : d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.StoneCarat);
            d.DocRepairResult_CharProduct.StoneQtyOld = (d.IDProduct != null ? d.StockProductDJ.StockProductDJ_CharProduct.StoneQty : d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.StoneQty);
            db.SubmitChanges();

            // hitung ulang netto weight untuk harga emas pada table result repair
            RecalWeight.RecalWeightRepairResult_PoS(db, d, d.DocRepairResult_CharProduct);
            db.SubmitChanges();

            if (d.DocRepair.IDProduct != null)
            {
                var productold = d.StockProductDJ.StockProductDJ_CharProduct;
                if (product == null) save.Disabled = true;
                else
                {
                    // kalkulasi untuk netto weight khusus repair toko
                    productresult.NettoWeight = productresult.GrossWeight - productresult.StoneWeight;
                    db.SubmitChanges();

                    SelisihBerat = (decimal)d.DocRepairResult_CharProduct.NettoWeight - (decimal)productold.NettoWeight;
                    productresult.SelisihBerat = SelisihBerat;
                    db.SubmitChanges();

                    span_grossweight.InnerHtml = Str.Number2(productold.GrossWeight);
                    span_diameter.InnerHtml = Str.Number2(productold.DimensiD);
                    span_panjang.InnerHtml = Str.Number2(productold.DimensiP);
                    span_lebar.InnerHtml = Str.Number2(productold.DimensiL);
                    span_ringsize.InnerHtml = Str.Number2(productold.DimensiR);

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
                    // kalkulasi untuk netto weight khusus repair toko
                    productresult.NettoWeight = productresult.GrossWeight - productresult.StoneWeight;
                    db.SubmitChanges();

                    SelisihBerat = (decimal)d.DocRepairResult_CharProduct.NettoWeight - (decimal)productold.NettoWeight;
                    productresult.SelisihBerat = SelisihBerat;
                    db.SubmitChanges();

                    span_grossweight.InnerHtml = Str.Number2(productold.GrossWeight);
                    span_diameter.InnerHtml = Str.Number2(productold.DimensiD);
                    span_panjang.InnerHtml = Str.Number2(productold.DimensiP);
                    span_lebar.InnerHtml = Str.Number2(productold.DimensiL);
                    span_ringsize.InnerHtml = Str.Number2(productold.DimensiR);

                    //Picture Before
                    Upload.Image(imgpicturebefore, productold.StockProductDJCustomer.ImgPicture);
                }
            }

            // for result info
            span_grossweight.InnerHtml += (d.DocRepairResult_CharProduct.GrossWeight != d.DocRepair.DocRepair_CharProduct.GrossWeight ? "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" + Str.Number2(d.DocRepairResult_CharProduct.GrossWeight) : "");
            span_diameter.InnerHtml += (d.DocRepairResult_CharProduct.DimensiD != d.DocRepair.DocRepair_CharProduct.DimensiD ? "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" + Str.Number2(d.DocRepairResult_CharProduct.DimensiD) : "");
            span_panjang.InnerHtml += (d.DocRepairResult_CharProduct.DimensiP != d.DocRepair.DocRepair_CharProduct.DimensiP ? "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" + Str.Number2(d.DocRepairResult_CharProduct.DimensiP) : "");
            span_lebar.InnerHtml += (d.DocRepairResult_CharProduct.DimensiL != d.DocRepair.DocRepair_CharProduct.DimensiL ? "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" + Str.Number2(d.DocRepairResult_CharProduct.DimensiL) : "");
            span_ringsize.InnerHtml += (d.DocRepairResult_CharProduct.DimensiR != d.DocRepair.DocRepair_CharProduct.DimensiR ? "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" + Str.Number2(d.DocRepairResult_CharProduct.DimensiR) : "");

            if (productresult == null) save.Disabled = true;
            else
            {
                decimal TotalBiaya = 0m;
                decimal OngkosRepair = 0m;
                decimal HargaBatu = 0m;
                decimal HargaEmas = 0m;

                if (d.DocRepairResult_Stone1A.Count() > 0) { var dr = db.DocRepair_Stone1A.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepairResult_Stone1B.Count() > 0) { var dr = db.DocRepairResult_Stone1B.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepairResult_Stone2.Count() > 0) { var dr = db.DocRepairResult_Stone2.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepairResult_Stone3.Count() > 0) { var dr = db.DocRepairResult_Stone3.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepairResult_Stone4.Count() > 0) { var dr = db.DocRepairResult_Stone4.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
                if (d.DocRepairResult_Stone5.Count() > 0) { var dr = db.DocRepairResult_Stone5.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }

                // looping pembulatan harga batu
                var x1A = d.DocRepairResult_Stone1A; foreach (var r1A in x1A) { r1A.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r1A.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x1B = d.DocRepairResult_Stone1B; foreach (var r1B in x1B) { r1B.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r1B.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x2 = d.DocRepairResult_Stone2; foreach (var r2 in x2) { r2.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r2.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x3 = d.DocRepairResult_Stone3; foreach (var r3 in x3) { r3.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r3.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x4 = d.DocRepairResult_Stone4; foreach (var r4 in x4) { r4.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r4.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }
                var x5 = d.DocRepairResult_Stone5; foreach (var r5 in x5) { r5.HargaTotalRupiah = (Math.Floor((Math.Ceiling(Convert.ToDecimal(r5.HargaTotalRupiah)) / 1000))) * 1000; db.SubmitChanges(); }

                if (d.DocRepairResult_Stone1A.Count() > 0) { HargaBatu += d.DocRepairResult_Stone1A.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepairResult_Stone1B.Count() > 0) { HargaBatu += d.DocRepairResult_Stone1B.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepairResult_Stone2.Count() > 0) { HargaBatu += d.DocRepairResult_Stone2.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepairResult_Stone3.Count() > 0) { HargaBatu += d.DocRepairResult_Stone3.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepairResult_Stone4.Count() > 0) { HargaBatu += d.DocRepairResult_Stone4.Sum(p => p.HargaTotalRupiah.Value); }
                if (d.DocRepairResult_Stone5.Count() > 0) { HargaBatu += d.DocRepairResult_Stone5.Sum(p => p.HargaTotalRupiah.Value); }

                var prc_rpr = db.PricingTableTG.SingleOrDefault(p =>
                    p.ID == CV.Repair_General_Pricing &&
                    p.Harga.HasValue &&
                    p.ApprovalTgl.HasValue);

                bool is_InvalidItem = Pricing.RepairInvoicePricing_InvalidProduct(isRepairStock, d);
                bool is_SmallItem = Pricing.RepairInvoicePricing_SmallBigProduct(isRepairStock, d);

                if (is_InvalidItem == true && !isRepairStock) { save.Disabled = true; lit_msg.Text = "<p class='bg-danger'>Repair tidak bisa dilakukan untuk Product Item : " + product.CharProductItem.Nama + "</p>"; } // barang termasuk dalam daftar yang tidak boleh di repair

                //repairproductitem.InnerText = (is_InvalidItem && !isRepairStock) ? "Invalid" : ((is_SmallItem) ? "Small" : "Big");

                if (is_SmallItem) { HargaEmas = (SelisihBerat - 0.5m) > 0 ? ((SelisihBerat - 0.5m) * prc_rpr.Harga.Value) : 0m; }
                else { HargaEmas = (SelisihBerat - 0.5m) > 0 ? ((SelisihBerat - 0.5m) * prc_rpr.Harga.Value) : 0m; }

                OngkosRepair = d.DocRepairResult_Repair.Sum(p => p.Biaya.Value);

                OngkosRepair = (Math.Floor((Math.Floor(OngkosRepair) / 1000))) * 1000;
                HargaBatu = (Math.Floor((Math.Floor(HargaBatu) / 1000))) * 1000;
                HargaEmas = (Math.Floor((Math.Floor(HargaEmas) / 1000))) * 1000;

                TotalBiaya = OngkosRepair + HargaBatu + HargaEmas;
                //TotalBiaya = (Math.Floor((Math.Floor(TotalBiaya) / 1000))) * 1000; // pembulatan 1000 ke bawah

                d.DocRepairResult_CharProduct.HargaEmas = HargaEmas;
                d.DocRepairResult_CharProduct.Rate = prc_rpr.Harga.Value;
                db.SubmitChanges();

                lit_pricetotal_jasa.Text = Str.Number0(OngkosRepair);
                lit_pricetotal_batu.Text = Str.Number0(HargaBatu);
                lit_pricetotal_emas.Text = Str.Number0(HargaEmas);
                lit_pricetotal.Text = Str.Number0(TotalBiaya);

                //estimasi.InnerHtml = Str.Number0(TotalBiaya);

                // update repair estimasi harga
                var r = db.DocRepair.SingleOrDefault(p => p.ID == myID);
                r.EstimasiHarga = Math.Floor(TotalBiaya);
                db.SubmitChanges();

            }

            //    //Weight
            //grossweight.InnerHtml = Str.Number3(product.GrossWeight);
            //totalbutir.InnerHtml = Str.Number0(product.StoneQty);
            //totalharga.InnerHtml = (Str.Number0(TotalBiaya));
            //selisihberat.InnerHtml = Str.Number2(product.SelisihBerat);
            //}

			//Stone
			fillStone();

            //Repair Item
            fillRepairItem();

		}
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
            GT += rs.Sum(p=>p.Biaya.Value);
        }
        Sub_Total(GT);
    }

	//Stone
	protected void fillStone() {
		int GT_Butir = 0;
		decimal GT_Carat = 0;
        decimal GT_Harga = 0;

		//Stone
        //fillStone(0, ref GT_Butir, ref GT_Carat, ref GT_Harga);
        //Stone Repair
        fillStone1A(0, ref GT_Butir, ref GT_Carat);
        fillStone1B(1, ref GT_Butir, ref GT_Carat);
        fillStone2(2, ref GT_Butir, ref GT_Carat);
        fillStone3(3, ref GT_Butir, ref GT_Carat);
        fillStone4(4, ref GT_Butir, ref GT_Carat);
        fillStone5(5, ref GT_Butir, ref GT_Carat);

		//Grand-Total
        Sub_Total(GT_Butir, GT_Carat, GT_Harga, true);
	}

	//Stone
	protected void fillStone(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_Harga) {
        ////SELECT
        //var rs = from p in db.DocRepairResult_StoneRepair
        //         where p.IDForm == myID
        //         orderby p.StoneRepair.Nama
        //         select p;

        ////Sub-Total
        //int ST_Butir = 0;
        //decimal ST_Carat = 0;
        //decimal ST_Harga = 0;

        ////Sub-Header
        //if (rs.Count() > 0) Sub_Header(StoneIndex);

        ////RESULT
        //int index = 1;
        //foreach (var r in rs) {
        //    if (!Response.IsClientConnected) break;

        //    TableRow tr = new TableRow();
        //    TableCell td;

        //    td = new TableCell();
        //    td.Text = "<p>" + index++.ToString() + ".</p>";
        //    tr.Cells.Add(td);

        //    td = new TableCell();
        //    td.Text = TD_Nama(r.StoneRepair.Nama);
        //    tr.Cells.Add(td);

        //    td = new TableCell();
        //    td.Text = TD_CaratButir(r.CaratButir);
        //    td.CssClass = "text-right";
        //    tr.Cells.Add(td);

        //    td = new TableCell();
        //    td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
        //    td.CssClass = "text-center";
        //    tr.Cells.Add(td);

        //    td = new TableCell();
        //    td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
        //    td.CssClass = "text-center";
        //    tr.Cells.Add(td);

        //    td = new TableCell();
        //    td.Text = Convert.ToDecimal(r.HargaTotalRupiah.Value).ToString("N0");
        //    td.CssClass = "text-center";
        //    tr.Cells.Add(td);

        //    td = new TableCell();
        //    td.Text = r.Keterangan;
        //    tr.Cells.Add(td);

        //    ST_Harga += Convert.ToDecimal(r.HargaTotalRupiah.Value);

        //    tb.Rows.Add(tr);
        //}

        ////Total
        //if (rs.Count() > 0) {
        //    GT_Butir += ST_Butir;
        //    GT_Carat += ST_Carat;
        //    GT_Harga += ST_Harga;

        //    Sub_Total(ST_Butir, ST_Carat, ST_Harga, false);
        //}
	}

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepairResult_Stone1A
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
        var rs = from p in db.DocRepairResult_Stone1B
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
        var rs = from p in db.DocRepairResult_Stone2
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
        var rs = from p in db.DocRepairResult_Stone3
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
        var rs = from p in db.DocRepairResult_Stone4
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
        var rs = from p in db.DocRepairResult_Stone5
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
    //protected void Sub_Header(int StoneIndex) {
    //    TableRow tr = new TableRow();
    //    TableCell td;

    //    tr.CssClass = "sub-header";
    //    string x = string.Empty;

    //    x += "<br />";
    //    x += DL.StoneIndex(StoneIndex);

    //    td = new TableCell();
    //    td.Text = x;
    //    td.CssClass = "text-lg text-warning";
    //    td.ColumnSpan = App.TableColspan(tb);
    //    tr.Cells.Add(td);

    //    tb.Rows.Add(tr);
    //}
    protected void Sub_Total(int TotalButir, decimal TotalCarat, decimal TotalHarga, bool GrandTotal)
    {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 4;
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
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_Nama(string Nama, string GIA) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";
		if (GIA != "")
			x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

		return x;
	}
	protected string TD_Dimensi(decimal? Dimensi) {
		string x = string.Empty;

		x += "<p>" + Str.Number2(Dimensi) + "</p>";

		return x;
	}
	protected string TD_CaratButir(decimal? CaratButir) {
		string x = string.Empty;

		x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

		return x;
	}
	protected string TD_TotalButir(int? TotalButir, ref int ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

		//Sub-Total
		if (TotalButir.HasValue) ST += TotalButir.Value;

		return x;
	}
	protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

		//Sub-Total
		if (TotalCarat.HasValue) ST += TotalCarat.Value;

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e)
    {
        decimal SelisihBerat = 0m;

		var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else 
        {
			//Status
			d.Draft = false;
            d.Nomor = Generate_Nomor(d);
			d.DraftDate = null;
            if(d.IDProduct.HasValue)
                d.StockProductDJ.StockActualDJ.Substorage = 5;
            d.DocRepair.StatusRepair = 2;
            
            decimal OngkosRepair = 0m;
            decimal HargaEmas = 0m;
            decimal TotalBiaya = 0m;
            decimal HargaBatu = 0m;

            var prc_rpr = db.PricingTableTG.SingleOrDefault(p =>
                p.ID == CV.Repair_General_Pricing &&
                p.Harga.HasValue &&
                p.ApprovalTgl.HasValue);

            bool is_InvalidItem = Pricing.RepairInvoicePricing_InvalidProduct(isRepairStock, d);
            bool is_SmallItem = Pricing.RepairInvoicePricing_SmallBigProduct(isRepairStock, d);

            if (is_InvalidItem == true && !isRepairStock) { save.Disabled = true; lit_msg.Text = "<p class='bg-danger'>Repair tidak bisa dilakukan untuk Product Item : " + d.DocRepairResult_CharProduct.CharProductItem.Nama + "</p>"; } // barang termasuk dalam daftar yang tidak boleh di repair

            //repairproductitem.InnerText = (is_InvalidItem && !isRepairStock) ? "Invalid" : ((is_SmallItem) ? "Small" : "Big");

            var product = d.DocRepairResult_CharProduct;

            // insert jumlah total batu yang lama ke tabel result repair old stone qty
            product.StoneCaratOld = (d.IDProduct != null ? d.StockProductDJ.StockProductDJ_CharProduct.StoneCarat : d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.StoneCarat);
            product.StoneQtyOld = (d.IDProduct != null ? d.StockProductDJ.StockProductDJ_CharProduct.StoneQty : d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.StoneQty);
            db.SubmitChanges();

            // hitung ulang netto weight untuk harga emas pada table result repair
            RecalWeight.RecalWeightRepairResult_PoS(db, d, d.DocRepairResult_CharProduct);
            db.SubmitChanges();

            if (d.DocRepair.IDProduct != null)
            {
                var productold = d.StockProductDJ.StockProductDJ_CharProduct;
                if (product == null) save.Disabled = true;
                else
                {
                    SelisihBerat = (decimal)d.DocRepairResult_CharProduct.NettoWeight - (decimal)productold.NettoWeight;
                }
            }
            else if (d.IDProductTitipan != null)
            {
                var productold = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct;
                if (product == null) save.Disabled = true;
                else
                {
                    SelisihBerat = (decimal)d.DocRepairResult_CharProduct.NettoWeight - (decimal)productold.NettoWeight;
                }
            }

            if (is_SmallItem) { HargaEmas = (SelisihBerat - 0.5m) > 0 ? ((SelisihBerat - 0.5m) * prc_rpr.Harga.Value) : 0m; }
            else { HargaEmas = (SelisihBerat - 0.5m) > 0 ? ((SelisihBerat - 0.5m) * prc_rpr.Harga.Value) : 0m; }

            OngkosRepair = d.DocRepairResult_Repair.Sum(p => p.Biaya.Value);

            if (d.DocRepairResult_Stone1A.Count() > 0) { var dr = db.DocRepair_Stone1A.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
            if (d.DocRepairResult_Stone1B.Count() > 0) { var dr = db.DocRepairResult_Stone1B.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
            if (d.DocRepairResult_Stone2.Count() > 0) { var dr = db.DocRepairResult_Stone2.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
            if (d.DocRepairResult_Stone3.Count() > 0) { var dr = db.DocRepairResult_Stone3.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
            if (d.DocRepairResult_Stone4.Count() > 0) { var dr = db.DocRepairResult_Stone4.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }
            if (d.DocRepairResult_Stone5.Count() > 0) { var dr = db.DocRepairResult_Stone5.Where(p => p.IDForm == myID && p.HargaTotalRupiah == null).ToList(); dr.ForEach(p => p.HargaTotalRupiah = 0m); db.SubmitChanges(); }

            if (d.DocRepairResult_Stone1A.Count() > 0) { HargaBatu += d.DocRepairResult_Stone1A.Sum(p => p.HargaTotalRupiah.Value); }
            if (d.DocRepairResult_Stone1B.Count() > 0) { HargaBatu += d.DocRepairResult_Stone1B.Sum(p => p.HargaTotalRupiah.Value); }
            if (d.DocRepairResult_Stone2.Count() > 0) { HargaBatu += d.DocRepairResult_Stone2.Sum(p => p.HargaTotalRupiah.Value); }
            if (d.DocRepairResult_Stone3.Count() > 0) { HargaBatu += d.DocRepairResult_Stone3.Sum(p => p.HargaTotalRupiah.Value); }
            if (d.DocRepairResult_Stone4.Count() > 0) { HargaBatu += d.DocRepairResult_Stone4.Sum(p => p.HargaTotalRupiah.Value); }
            if (d.DocRepairResult_Stone5.Count() > 0) { HargaBatu += d.DocRepairResult_Stone5.Sum(p => p.HargaTotalRupiah.Value); }

            OngkosRepair = (Math.Floor((Math.Floor(OngkosRepair) / 1000))) * 1000;
            HargaBatu = (Math.Floor((Math.Floor(HargaBatu) / 1000))) * 1000;
            HargaEmas = (Math.Floor((Math.Floor(HargaEmas) / 1000))) * 1000;

            TotalBiaya = OngkosRepair + HargaBatu + HargaEmas;
            TotalBiaya = (Math.Floor((Math.Floor(TotalBiaya) / 1000))) * 1000; // pembulatan 1000 ke bawah

            bool is_ValidDiscount = (Math.Floor(TotalBiaya) - Math.Floor(Convert.ToDecimal(discount.Text)) >= 0m) ? true : false;
            if (is_ValidDiscount)
            {
                d.TotalHarga = TotalBiaya;
                db.SubmitChanges();

                decimal TotalBayar = 0m;
                decimal Discount = (discount.Text.Trim() == "") ? 0m : Convert.ToDecimal(discount.Text.Trim());
                TotalBayar = Math.Floor(TotalBiaya) - Discount;

                SalesOrderRepair n = new SalesOrderRepair();

                n.IDRepair = d.ID;
                n.TipeLokasi = d.TipeLokasi.Value;
                n.IDLokasi = d.IDLokasi.Value;
                if (d.IDCustomer.HasValue)
                {
                    n.IDCustomer = d.IDCustomer.Value;
                    n.CustomerNama = d.CustomerNama;
                }
                if (d.IDSales.HasValue)
                    n.IDSales = d.IDSales.Value;
                n.Operator = d.Operator;
                n.OperatorTgl = d.OperatorTgl;
                n.SalesNama = d.SalesNama;

                n.IDProduct = (d.IDProduct.HasValue) ? d.IDProduct : null;
                n.IDProductTitipan = (d.IDProductTitipan.HasValue) ? d.IDProductTitipan : null;

                n.HargaRupiah = TotalBiaya;
                n.TotalBayarBeforeDisc = TotalBiaya;
                n.TotalRupiahBeforeDisc = TotalBiaya;
                n.Discount = 0m;
                n.DiscountNominal = Discount;
                n.DiscountGift = 0m;
                n.DiscountProgram = 0m;
                n.DiscountProgramNominal = 0m;
                n.DiscountRound = 0m;
                n.TotalBayar = TotalBayar;
                n.TotalRupiah = TotalBayar;
                n.Unpaid = TotalBayar;
                n.Paid = 0m;
                n.Draft = false;
                n.DraftDate = DateTime.Now;
                n.Nomor = Generate_Nomor(n);
                n.Tgl = DateTime.Today;
                db.SalesOrderRepair.InsertOnSubmit(n);

                if (d.IDProduct.HasValue) 
                {
                    var rslt = from p in db.DocTitipan_Product
                               where p.IDProduct.Value == d.IDProduct
                               select p;

                    // status titipan 3 : barang sudah dikembalikan ke toko
                    foreach (var r in rslt)
                    {
                        r.DocTitipan.StatusTitipan = 3;
                        db.SubmitChanges();
                    }
                }
                else if (d.IDProductTitipan.HasValue) 
                {
                    var rslt = from p in db.DocTitipan_Product
                               where p.IDProductTitipan.Value == d.IDProductTitipan
                               select p;

                    // status titipan 3 : barang sudah dikembalikan ke toko
                    foreach (var r in rslt)
                    {
                        r.DocTitipan.StatusTitipan = 3;
                        db.SubmitChanges();
                    }
                }

                //Commit
                db.SubmitChanges();

                //Redirect
                Response.Redirect("RepairInvoice_Print.aspx?id=" + myID);
            }
            else
            {
                lit_msg.Text = "<p class='bg-danger'>Discount tidak Valid</p>";
            }
		}
        //Response.Redirect("Repair_Pricing.aspx?id=" + myID);
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

        string docid = "INV/RPR/" + Tahun.ToString().Remove(0, 2) + "/" + Bulan.ToString().PadLeft(2,'0') + "/" + Str.KodeFormat(Nomor, 6);

        return docid;
    }

    protected string Generate_Nomor(SalesOrderRepair j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.SalesOrder.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;

        int Nomor = 1;

        try
        {
            var s = (from p in db.SalesOrderRepair
                     where p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan
                     && p.Nomor.Substring(3, 1) == "" + j.TipeLokasi.Value.ToString() + ""
                     && p.Nomor.Substring(5, 3) == "" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + ""
                     orderby p.ID descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(19,4)) + 1;
        }
        catch (Exception ex) { }

        return "SO/RPR/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }

    //Display Column
    protected string TD_HyperLink(int StoneIndex, int TableID)
    {
        string x = string.Empty;
        string href = string.Empty;

        return x;
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
        td.ColumnSpan = 4;
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

        tb.Rows.Add(tr);
    }
}