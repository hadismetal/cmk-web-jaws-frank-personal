using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Repair_CharProduct02 : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            grossweight.Focus();

            //Number-Format
            App.NumberFormat(grossweight);
            //App.NumberFormat(kadarlogam);
            //App.NumberFormat(kadartukaran);
            App.NumberFormat(dimensid);
            App.NumberFormat(dimensip);
            App.NumberFormat(dimensil);
            App.NumberFormat(dimensir);
            //App.NumberFormat(stoneqty);

            //Form-Default
            //kadarlogam.Text = Str.Number2(CV.KadarLogam);
            //kadartukaran.Text = Str.Number2(CV.KadarTukaran);

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Repair.aspx"); }
    protected void LinkError() { Response.Redirect("Repair_Data.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            //Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

            if (d.IDProduct != null)
            {
                var dj = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProduct);
                if (dj != null)
                {
                    //lit_customer.Text = d.DataCustomer.Nama;
                    //lit_jpjr.Text = d.DataSales.Nama;
                    //lit_tgl.Text = Str.FullDate(d.Tgl);
                    //lit_tgl2.Text = Str.FullDate(d.TglSelesai);
                    //lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);

                    //if (d.IDProduct != null) { lit_plu.Text = d.StockProductDJ.Nomor; }
                    //else if (d.IDProductTitipan != null) { lit_plu.Text = d.StockProductDJCustomer.Nomor; }

                    //var d_item_list = from p in db.DocRepair_Repair where p.IDForm == myID select p;

                    //int index = 1;

                    //foreach (var r in d_item_list)
                    //{
                    //    TableRow tr = new TableRow();
                    //    TableCell td;

                    //    td = new TableCell();
                    //    td.Text = index.ToString() + ".";
                    //    tr.Cells.Add(td);

                    //    td = new TableCell();
                    //    td.Text = r.CharProcessRepair.Nama;
                    //    tr.Cells.Add(td);

                    //    tbl_repairitem.Rows.Add(tr);

                    //    index++;

                    //}

                    grossweightold.Text = Str.Number2(dj.StockProductDJ_CharProduct.GrossWeight);
                    dimensidold.Text = Str.Number2(dj.StockProductDJ_CharProduct.DimensiD);
                    dimensipold.Text = Str.Number2(dj.StockProductDJ_CharProduct.DimensiP);
                    dimensilold.Text = Str.Number2(dj.StockProductDJ_CharProduct.DimensiL);
                    dimensirold.Text = Str.Number2(dj.StockProductDJ_CharProduct.DimensiR);

                    decimal dgrossweight = 0m;
                    decimal ddimensid = 0m;
                    decimal ddimensip = 0m;
                    decimal ddimensil = 0m;
                    decimal ddimensir = 0m;

                    decimal dgrossweightresult = 0m;
                    decimal ddimensidresult = 0m;
                    decimal ddimensipresult = 0m;
                    decimal ddimensilresult = 0m;
                    decimal ddimensirresult = 0m;

                    if (db.DocRepair_CharProduct.Any(p => p.ID == d.IDWorkOrder))
                    {
                        dgrossweight = (d.DocRepair.DocRepair_CharProduct.GrossWeight != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.GrossWeight) : 0m);
                        ddimensid = (d.DocRepair.DocRepair_CharProduct.DimensiD != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiD) : 0m);
                        ddimensip = (d.DocRepair.DocRepair_CharProduct.DimensiP != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiP) : 0m);
                        ddimensil = (d.DocRepair.DocRepair_CharProduct.DimensiL != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiL) : 0m);
                        ddimensir = (d.DocRepair.DocRepair_CharProduct.DimensiR != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiR) : 0m);

                        dgrossweightresult = (d.DocRepair.DocRepair_CharProduct.GrossWeight != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.GrossWeight) : 0m);
                        ddimensidresult = (d.DocRepair.DocRepair_CharProduct.DimensiD != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiD) : 0m);
                        ddimensipresult = (d.DocRepair.DocRepair_CharProduct.DimensiP != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiP) : 0m);
                        ddimensilresult = (d.DocRepair.DocRepair_CharProduct.DimensiL != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiL) : 0m);
                        ddimensirresult = (d.DocRepair.DocRepair_CharProduct.DimensiR != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiR) : 0m);
                    }
                    else
                    {
                        dgrossweight = (d.StockProductDJ.StockProductDJ_CharProduct.GrossWeight != null ? Convert.ToDecimal(d.StockProductDJ.StockProductDJ_CharProduct.GrossWeight) : 0m);
                    }

                    grossweight.Text = Str.Number2(dgrossweight);
                    dimensid.Text = Str.Number2(ddimensid);
                    dimensip.Text = Str.Number2(ddimensip);
                    dimensil.Text = Str.Number2(ddimensil);
                    dimensir.Text = Str.Number2(ddimensir);

                    grossweightresult.Text = Str.Number2(dgrossweightresult);
                    dimensidresult.Text = Str.Number2(ddimensidresult);
                    dimensipresult.Text = Str.Number2(ddimensipresult);
                    dimensilresult.Text = Str.Number2(ddimensilresult);
                    dimensirresult.Text = Str.Number2(ddimensirresult);
                }
            }
            else if (d.IDProductTitipan != null)
            {
                var dj = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == d.IDProductTitipan);
                if (dj != null)
                {
                    //lit_customer.Text = d.DataCustomer.Nama;
                    //lit_jpjr.Text = d.DataSales.Nama;
                    //lit_tgl.Text = Str.FullDate(d.Tgl);
                    //lit_tgl2.Text = Str.FullDate(d.TglSelesai);
                    //lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);

                    //if (d.IDProduct != null) { lit_plu.Text = d.StockProductDJ.Nomor; }
                    //else if (d.IDProductTitipan != null) { lit_plu.Text = d.StockProductDJCustomer.Nomor; }

                    //var d_item_list = from p in db.DocRepairResult_Repair where p.IDForm == myID select p;

                    //int index = 1;

                    //foreach (var r in d_item_list)
                    //{
                    //    TableRow tr = new TableRow();
                    //    TableCell td;

                    //    td = new TableCell();
                    //    td.Text = index.ToString() + ".";
                    //    tr.Cells.Add(td);

                    //    td = new TableCell();
                    //    td.Text = r.CharProcessRepair.Nama;
                    //    tr.Cells.Add(td);

                    //    tbl_repairitem.Rows.Add(tr);

                    //    index++;

                    //}

                    grossweightold.Text = Str.Number2(dj.StockProductDJCustomer_CharProduct.GrossWeight);
                    dimensidold.Text = Str.Number2(dj.StockProductDJCustomer_CharProduct.DimensiD);
                    dimensipold.Text = Str.Number2(dj.StockProductDJCustomer_CharProduct.DimensiP);
                    dimensilold.Text = Str.Number2(dj.StockProductDJCustomer_CharProduct.DimensiL);
                    dimensirold.Text = Str.Number2(dj.StockProductDJCustomer_CharProduct.DimensiR);

                    decimal dgrossweight = 0m;
                    decimal ddimensid = 0m;
                    decimal ddimensip = 0m;
                    decimal ddimensil = 0m;
                    decimal ddimensir = 0m;

                    decimal dgrossweightresult = 0m;
                    decimal ddimensidresult = 0m;
                    decimal ddimensipresult = 0m;
                    decimal ddimensilresult = 0m;
                    decimal ddimensirresult = 0m;

                    if (db.DocRepair_CharProduct.Any(p => p.ID == d.IDWorkOrder))
                    {
                        dgrossweight = (d.DocRepair.DocRepair_CharProduct.GrossWeight != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.GrossWeight) : 0m);
                        ddimensid = (d.DocRepair.DocRepair_CharProduct.DimensiD != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiD) : 0m);
                        ddimensip = (d.DocRepair.DocRepair_CharProduct.DimensiP != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiP) : 0m);
                        ddimensil = (d.DocRepair.DocRepair_CharProduct.DimensiL != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiL) : 0m);
                        ddimensir = (d.DocRepair.DocRepair_CharProduct.DimensiR != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiR) : 0m);

                        dgrossweightresult = (d.DocRepair.DocRepair_CharProduct.GrossWeight != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.GrossWeight) : 0m);
                        ddimensidresult = (d.DocRepair.DocRepair_CharProduct.DimensiD != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiD) : 0m);
                        ddimensipresult = (d.DocRepair.DocRepair_CharProduct.DimensiP != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiP) : 0m);
                        ddimensilresult = (d.DocRepair.DocRepair_CharProduct.DimensiL != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiL) : 0m);
                        ddimensirresult = (d.DocRepair.DocRepair_CharProduct.DimensiR != null ? Convert.ToDecimal(d.DocRepair.DocRepair_CharProduct.DimensiR) : 0m);
                    }
                    else
                    {
                        dgrossweight = (d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.GrossWeight != null ? Convert.ToDecimal(d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.GrossWeight) : 0m);
                    }

                    grossweight.Text = Str.Number2(dgrossweight);
                    dimensid.Text = Str.Number2(ddimensid);
                    dimensip.Text = Str.Number2(ddimensip);
                    dimensil.Text = Str.Number2(ddimensil);
                    dimensir.Text = Str.Number2(ddimensir);

                    grossweightresult.Text = Str.Number2(dgrossweightresult);
                    dimensidresult.Text = Str.Number2(ddimensidresult);
                    dimensipresult.Text = Str.Number2(ddimensipresult);
                    dimensilresult.Text = Str.Number2(ddimensilresult);
                    dimensirresult.Text = Str.Number2(ddimensirresult);
                }
            }


        }
    }

    //Valid
    protected bool valid
    {
        get
        {
            bool x = true;

            x = x && Fv.isDecimal(grossweight, grossweight_ERR);
            //x = x && Fv.isDecimal(kadarlogam, kadarlogam_ERR);
            //x = x && Fv.isDecimal(kadartukaran, kadartukaran_ERR);
            x = x && Fv.isDecimal(dimensid, dimensid_ERR);
            x = x && Fv.isDecimal(dimensip, dimensip_ERR);
            x = x && Fv.isDecimal(dimensil, dimensil_ERR);
            x = x && Fv.isDecimal(dimensir, dimensir_ERR, true);

            //Value
            x = x && valid_GrossWeight;
            //x = x && valid_KadarLogam;
            //x = x && valid_KadarTukaran;

            return x;
        }
    }
    protected bool valid_GrossWeight
    {
        get
        {
            decimal dv = Convert.ToDecimal(grossweight.Text);

            bool x = dv > 0;
            grossweight_VAL.Visible = !x;

            return x;
        }
    }
    //protected bool valid_KadarLogam
    //{
    //    get
    //    {
    //        decimal dv = Convert.ToDecimal(kadarlogam.Text);

    //        bool x = dv > 0;
    //        kadarlogam_VAL.Visible = !x;

    //        return x;
    //    }
    //}
    //protected bool valid_KadarTukaran
    //{
    //    get
    //    {
    //        decimal dv = Convert.ToDecimal(kadartukaran.Text);

    //        bool x = dv > 0;
    //        kadartukaran_VAL.Visible = !x;

    //        return x;
    //    }
    //}
    protected void save_Click(object sender, EventArgs e)
    {
        if (valid)
        {
            var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) DataError();
            else
            {
                //Process
                saveData(d);
                moveStone(d.DocRepair, d);

                //Recal Weight
                //Lib.RecalWeight(db,d,d.DocRepairResult_CharProduct);

                //Redirect
                Response.Redirect("RepairInvoice_DataStone.aspx?id=" + myID);
            }
        }
    }
    protected void saveData(DocRepairResult ms)
    {
        var d = db.DocRepairResult_CharProduct.SingleOrDefault(p=>p.ID == ms.ID);
        if (d == null)
        {
            
            DocRepairResult_CharProduct s = new DocRepairResult_CharProduct();

            s.ID = ms.ID;

            s.ProductItem = ms.DocRepair.DocRepair_CharProduct.ProductItem;
            if (ms.IDProduct.HasValue)
            {
                s.ProductCategory = ms.DocRepair.DocRepair_CharProduct.ProductCategory.Value;
                s.ProductLevel = ms.DocRepair.DocRepair_CharProduct.ProductLevel.Value;
                s.StoneDist = ms.DocRepair.DocRepair_CharProduct.StoneDist.Value;
                s.FrameMaterial = ms.DocRepair.DocRepair_CharProduct.FrameMaterial.Value;
                s.FrameFinishing = ms.DocRepair.DocRepair_CharProduct.FrameFinishing.Value;
                s.FrameColor = ms.DocRepair.DocRepair_CharProduct.FrameColor.Value;
                s.ProcessCons = ms.DocRepair.DocRepair_CharProduct.ProcessCons.Value;
            }
            
            //s.StoneCarat = ms.DocRepair.DocRepair_CharProduct.StoneCarat.Value;
            //s.StoneWeight = ms.DocRepair.DocRepair_CharProduct.StoneWeight.Value;
            //s.StoneQty = ms.DocRepair.DocRepair_CharProduct.StoneQty;
            //s.StoneQty = Convert.ToDecimal(stoneqty.Text);
            s.GrossWeight = Convert.ToDecimal(grossweightresult.Text);
            //s.NettoWeight = ms.DocRepair.DocRepair_CharProduct.GrossWeight.Value;
            //s.KadarLogam = Convert.ToDecimal(kadarlogam.Text);
            //s.KadarTukaran = Convert.ToDecimal(kadartukaran.Text);

            //Dimensi
            s.DimensiD = Convert.ToDecimal(dimensidresult.Text);
            s.DimensiP = Convert.ToDecimal(dimensipresult.Text);
            s.DimensiL = Convert.ToDecimal(dimensilresult.Text);
            s.SelisihBerat = Convert.ToDecimal(ms.DocRepair.DocRepair_CharProduct.GrossWeight) - Convert.ToDecimal(grossweightresult.Text);

            //Ring-Size
            if (dimensir.Text != "")
                s.DimensiR = Convert.ToDecimal(dimensirresult.Text);
            else
                s.DimensiR = 0;

            //Commit
            db.DocRepairResult_CharProduct.InsertOnSubmit(s);
            db.SubmitChanges();
        }
        else
        {
            d.GrossWeight = Convert.ToDecimal(grossweightresult.Text);
            //d.NettoWeight = d.GrossWeight;
            //d.KadarLogam = Convert.ToDecimal(kadarlogam.Text);
            //d.KadarTukaran = Convert.ToDecimal(kadartukaran.Text);
            //d.StoneQty = Convert.ToDecimal(stoneqty.Text);
            d.SelisihBerat = Convert.ToDecimal(grossweightresult.Text) - d.DocRepairResult.DocRepair.DocRepair_CharProduct.GrossWeight;

            //Dimensi
            d.DimensiD = Convert.ToDecimal(dimensidresult.Text);
            d.DimensiP = Convert.ToDecimal(dimensipresult.Text);
            d.DimensiL = Convert.ToDecimal(dimensilresult.Text);

            //Ring-Size
            if (dimensir.Text != "")
                d.DimensiR = Convert.ToDecimal(dimensirresult.Text);
            else
                d.DimensiR = 0;

            //Commit
            db.SubmitChanges();
        }
    }

    //Move Stone
    protected void moveStone(DocRepair ms, DocRepairResult x)
    {
        moveStone1A(ms, x);
        moveStone1B(ms, x);
        moveStone2(ms, x);
        moveStone3(ms, x);
        moveStone4(ms, x);
        moveStone5(ms, x);
    }
    protected void moveStone1A(DocRepair ms, DocRepairResult x)
    {
        var z = db.DocRepairResult_Stone1A.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.DocRepairResult_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in ms.DocRepair_Stone1A
                    orderby p.Stone1A.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairResult_Stone1A n = new DocRepairResult_Stone1A();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                // update harga terbaru dari work order -> invoice
                var y = db.Stone1A.SingleOrDefault(p => p.ID == r.IDStone && !p.Disable);
                var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                        p.ID == CV.US_General_Pricing &&
                        p.Harga.HasValue &&
                        p.ApprovalTgl.HasValue);

                n.HargaSatuan = y.StonePricing1A.Harga.Value;
                n.HargaSatuanM = y.StoneCosting1A.Harga;
                n.HargaTotal = ((y.StonePricing1A.Harga.Value * r.TotalCarat));
                n.HargaTotalM = y.StoneCosting1A.Harga.Value * r.TotalCarat;
                n.HargaTotalRupiah = ((y.StonePricing1A.Harga.Value * r.TotalCarat) * prc_us.Harga * 1.9m) + r.Setting;

                //n.HargaSatuan = r.HargaSatuan;
                //n.HargaTotal = r.HargaTotal;
                //n.HargaSatuanM = r.HargaSatuanM;
                //n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;
                //n.HargaTotalRupiah = r.HargaTotalRupiah;

                //Commit
                db.DocRepairResult_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone1B(DocRepair ms, DocRepairResult x)
    {
        var z = db.DocRepairResult_Stone1B.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.DocRepairResult_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in ms.DocRepair_Stone1B
                    orderby p.Stone1B.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairResult_Stone1B n = new DocRepairResult_Stone1B();

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

                // update harga terbaru dari work order -> invoice
                var y = db.Stone1B.SingleOrDefault(p => p.ID == r.IDStone && !p.Disable);
                var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                        p.ID == CV.US_General_Pricing &&
                        p.Harga.HasValue &&
                        p.ApprovalTgl.HasValue);

                n.HargaSatuan = y.StonePricing1B.Harga.Value;
                n.HargaSatuanM = y.StoneCosting1B.Harga;
                n.HargaTotal = (y.StonePricing1B.Harga.Value * r.TotalCarat) + ((y.StonePricing1B.Harga.Value * r.TotalCarat) * y.StonePricing1B.MarkUp / 100);
                n.HargaTotal = (y.StonePricing1B.Harga.Value * r.TotalCarat);
                n.HargaTotalM = y.StoneCosting1B.Harga.Value * r.TotalCarat;
                n.HargaTotalRupiah = (y.StonePricing1B.Harga.Value * r.TotalCarat) + ((y.StonePricing1B.Harga.Value * r.TotalCarat) * y.StonePricing1B.MarkUp / 100) * prc_us.Harga + r.Setting;

                //n.HargaSatuan = r.HargaSatuan;
                //n.HargaTotal = r.HargaTotal;
                //n.HargaSatuanM = r.HargaSatuanM;
                //n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;
                //n.HargaTotalRupiah = r.HargaTotalRupiah;

                //Commit
                db.DocRepairResult_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone2(DocRepair ms, DocRepairResult x)
    {
        var z = db.DocRepairResult_Stone2.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.DocRepairResult_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in ms.DocRepair_Stone2
                    orderby p.Stone2.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairResult_Stone2 n = new DocRepairResult_Stone2();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                // update harga terbaru dari work order -> invoice
                var y = db.Stone2.SingleOrDefault(p => p.ID == r.IDStone && !p.Disable);
                var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                        p.ID == CV.US_General_Pricing &&
                        p.Harga.HasValue &&
                        p.ApprovalTgl.HasValue);

                n.HargaSatuan = y.StonePricing2.Harga.Value;
                n.HargaSatuanM = y.StoneCosting2.Harga;
                n.HargaTotal = (y.StonePricing2.Harga.Value * r.TotalCarat);
                n.HargaTotalM = y.StoneCosting2.Harga.Value * r.TotalCarat;
                n.HargaTotalRupiah = ((y.StonePricing2.Harga.Value * r.TotalCarat)) + r.Setting;

                //n.HargaSatuan = r.HargaSatuan;
                //n.HargaTotal = r.HargaTotal;
                //n.HargaSatuanM = r.HargaSatuanM;
                //n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;
                //n.HargaTotalRupiah = r.HargaTotalRupiah;

                //Commit
                db.DocRepairResult_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone3(DocRepair ms, DocRepairResult x)
    {
        var z = db.DocRepairResult_Stone3.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.DocRepairResult_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in ms.DocRepair_Stone3
                    orderby p.Stone3.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairResult_Stone3 n = new DocRepairResult_Stone3();

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

                // update harga terbaru dari work order -> invoice
                var y = db.Stone3.SingleOrDefault(p => p.ID == r.IDStone && !p.Disable);
                var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                        p.ID == CV.US_General_Pricing &&
                        p.Harga.HasValue &&
                        p.ApprovalTgl.HasValue);

                n.HargaSatuan = y.StonePricing3.Harga.Value;
                n.HargaSatuanM = y.StoneCosting3.Harga;
                n.HargaTotal = (y.StonePricing3.Harga.Value * r.TotalCarat);
                n.HargaTotalM = y.StoneCosting3.Harga.Value * r.TotalCarat;
                n.HargaTotalRupiah = ((y.StonePricing3.Harga.Value * r.TotalCarat)) + r.Setting;

                //n.HargaSatuan = r.HargaSatuan;
                //n.HargaTotal = r.HargaTotal;
                //n.HargaSatuanM = r.HargaSatuanM;
                //n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;
                //n.HargaTotalRupiah = r.HargaTotalRupiah;

                //Commit
                db.DocRepairResult_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone4(DocRepair ms, DocRepairResult x)
    {
        var z = db.DocRepairResult_Stone4.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.DocRepairResult_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in ms.DocRepair_Stone4
                    orderby p.Stone4.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairResult_Stone4 n = new DocRepairResult_Stone4();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                // update harga terbaru dari work order -> invoice
                var y = db.Stone4.SingleOrDefault(p => p.ID == r.IDStone && !p.Disable);
                var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                        p.ID == CV.US_General_Pricing &&
                        p.Harga.HasValue &&
                        p.ApprovalTgl.HasValue);

                n.HargaSatuan = y.StonePricing4.Harga.Value;
                n.HargaSatuanM = y.StoneCosting4.Harga;
                n.HargaTotal = (y.StonePricing4.Harga.Value * r.TotalCarat);
                n.HargaTotalM = y.StoneCosting4.Harga.Value * r.TotalCarat;
                n.HargaTotalRupiah = ((y.StonePricing4.Harga.Value * r.TotalCarat)) + r.Setting;

                //n.HargaSatuan = r.HargaSatuan;
                //n.HargaTotal = r.HargaTotal;
                //n.HargaSatuanM = r.HargaSatuanM;
                //n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;
                //n.HargaTotalRupiah = r.HargaTotalRupiah;

                //Commit
                db.DocRepairResult_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone5(DocRepair ms, DocRepairResult x)
    {
        var z = db.DocRepairResult_Stone5.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.DocRepairResult_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in ms.DocRepair_Stone5
                    orderby p.Stone5.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepairResult_Stone5 n = new DocRepairResult_Stone5();

                n.IDForm = x.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                // update harga terbaru dari work order -> invoice
                var y = db.Stone5.SingleOrDefault(p => p.ID == r.IDStone && !p.Disable);
                var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                        p.ID == CV.US_General_Pricing &&
                        p.Harga.HasValue &&
                        p.ApprovalTgl.HasValue);

                n.HargaSatuan = y.StonePricing5.Harga.Value;
                n.HargaSatuanM = y.StoneCosting5.Harga;
                n.HargaTotal = (y.StonePricing5.Harga.Value * r.TotalCarat);
                n.HargaTotalM = y.StoneCosting5.Harga.Value * r.TotalCarat;
                n.HargaTotalRupiah = ((y.StonePricing5.Harga.Value * r.TotalCarat)) + r.Setting;

                //n.HargaSatuan = r.HargaSatuan;
                //n.HargaTotal = r.HargaTotal;
                //n.HargaSatuanM = r.HargaSatuanM;
                //n.HargaTotalM = r.HargaTotalM;
                n.Efektif = r.Efektif;
                n.Setting = r.Setting;
                //n.HargaTotalRupiah = r.HargaTotalRupiah;

                //Commit
                db.DocRepairResult_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
}