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
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
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
                    lit_plu.Text = d.StockProductDJ.Nomor;
                    lit_tgl.Text = Str.FullDate(d.Tgl);
                    lit_tgl2.Text = Str.FullDate(d.TglSelesai);
                    lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);

                    var d_item_list = from p in db.DocRepair_Repair where p.IDForm == myID select p;

                    int index = 1;

                    foreach (var r in d_item_list)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.CharProcessRepair.Nama;
                        tr.Cells.Add(td);

                        tbl_repairitem.Rows.Add(tr);

                        index++;

                    }

                    //kadarlogamold.Text = Str.Number2(dj.StockProductDJ_CharProduct.KadarLogam);
                    grossweightold.Text = Str.Number2(dj.StockProductDJ_CharProduct.GrossWeight);
                    dimensidold.Text = Str.Number2(dj.StockProductDJ_CharProduct.DimensiD);
                    dimensipold.Text = Str.Number2(dj.StockProductDJ_CharProduct.DimensiP);
                    dimensilold.Text = Str.Number2(dj.StockProductDJ_CharProduct.DimensiL);
                    dimensirold.Text = Str.Number2(dj.StockProductDJ_CharProduct.DimensiR);

                    //decimal dkadarlogam = 0m;
                    decimal dgrossweight = 0m;
                    decimal ddimensid = 0m;
                    decimal ddimensip = 0m;
                    decimal ddimensil = 0m;
                    decimal ddimensir = 0m;

                    if (db.DocRepair_CharProduct.Any(p => p.ID == myID))
                    {
                        //dkadarlogam = (d.DocRepair_CharProduct.KadarLogam != null ? Convert.ToDecimal(d.DocRepair_CharProduct.KadarLogam) : 0m);
                        dgrossweight = (d.DocRepair_CharProduct.GrossWeight != null ? Convert.ToDecimal(d.DocRepair_CharProduct.GrossWeight) : 0m);
                        ddimensid = (d.DocRepair_CharProduct.DimensiD != null ? Convert.ToDecimal(d.DocRepair_CharProduct.DimensiD) : 0m);
                        ddimensip = (d.DocRepair_CharProduct.DimensiP != null ? Convert.ToDecimal(d.DocRepair_CharProduct.DimensiP) : 0m);
                        ddimensil = (d.DocRepair_CharProduct.DimensiL != null ? Convert.ToDecimal(d.DocRepair_CharProduct.DimensiL) : 0m);
                        ddimensir = (d.DocRepair_CharProduct.DimensiR != null ? Convert.ToDecimal(d.DocRepair_CharProduct.DimensiR) : 0m);
                    }
                    else
                    {
                        //dkadarlogam = (d.StockProductDJ.StockProductDJ_CharProduct.KadarLogam != null ? Convert.ToDecimal(d.StockProductDJ.StockProductDJ_CharProduct.KadarLogam) : 0m);
                        dgrossweight = (d.StockProductDJ.StockProductDJ_CharProduct.GrossWeight != null ? Convert.ToDecimal(d.StockProductDJ.StockProductDJ_CharProduct.GrossWeight) : 0m);
                    }

                    //kadarlogam.Text = Str.Number2(dkadarlogam);
                    grossweight.Text = Str.Number2(dgrossweight);
                    dimensid.Text = Str.Number2(ddimensid);
                    dimensip.Text = Str.Number2(ddimensip);
                    dimensil.Text = Str.Number2(ddimensil);
                    dimensir.Text = Str.Number2(ddimensir);
                }
            }
            



        }
    }

    //Save
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
            var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
            if (d == null) DataError();
            else
            {
                //Process
                saveData(d);
                moveStone(d.StockProductDJ, d);

                //Recal Weight
                //Lib.RecalWeight(db,d,d.DocRepair_CharProduct);

                //Redirect
                Response.Redirect("Repair_DataStone.aspx?id=" + myID);
            }
        }
    }
    protected void saveData(DocRepair ms)
    {
        var x = db.StockProductDJ.SingleOrDefault(p => p.ID == ms.IDProduct);
        // Recal StockProduct
        Lib.RecalWeightStockResult(db, x, x.StockProductDJ_CharProduct); // id Product
        db.SubmitChanges();

        var d = db.DocRepair_CharProduct.SingleOrDefault(p=>p.ID == ms.ID);
        if (d == null)
        {
            DocRepair_CharProduct s = new DocRepair_CharProduct();

            s.ID = ms.ID;
            
            s.StoneQty = Convert.ToDecimal(0);
            s.GrossWeight = Convert.ToDecimal(grossweight.Text);

            //Dimensi
            s.DimensiD = Convert.ToDecimal(dimensid.Text);
            s.DimensiP = Convert.ToDecimal(dimensip.Text);
            s.DimensiL = Convert.ToDecimal(dimensil.Text);

            if (ms.IDProduct.HasValue)
            {
                s.KadarLogam = ms.StockProductDJ.StockProductDJ_CharProduct.KadarLogam;
                s.KadarTukaran = ms.StockProductDJ.StockProductDJ_CharProduct.KadarTukaran;

                s.ProductItem = ms.StockProductDJ.StockProductDJ_CharProduct.ProductItem;
                s.ProductCategory = ms.StockProductDJ.StockProductDJ_CharProduct.ProductCategory;
                s.ProductLevel = ms.StockProductDJ.StockProductDJ_CharProduct.ProductLevel;
                s.StoneDist = ms.StockProductDJ.StockProductDJ_CharProduct.StoneDist;
                s.FrameMaterial = ms.StockProductDJ.StockProductDJ_CharProduct.FrameMaterial;
                s.FrameFinishing = ms.StockProductDJ.StockProductDJ_CharProduct.FrameFinishing;
                s.FrameColor = ms.StockProductDJ.StockProductDJ_CharProduct.FrameColor;
                s.ProcessCons = ms.StockProductDJ.StockProductDJ_CharProduct.ProcessCons;
                s.StoneCarat = ms.StockProductDJ.StockProductDJ_CharProduct.StoneCarat;
                s.StoneWeight = ms.StockProductDJ.StockProductDJ_CharProduct.StoneWeight;
                //s.StoneQty = ms.StockProductDJ.StockProductDJ_CharProduct.StoneQty;
                
                s.NettoWeight = ms.StockProductDJ.StockProductDJ_CharProduct.GrossWeight;
                
            }
            else if (ms.IDProductTitipan.HasValue)
            {
                
                s.ProductItem = ms.StockProductDJCustomer.StockProductDJCustomer_CharProduct.ProductItem;
                
                s.StoneCarat = ms.StockProductDJCustomer.StockProductDJCustomer_CharProduct.StoneCarat;
                s.StoneWeight = ms.StockProductDJCustomer.StockProductDJCustomer_CharProduct.StoneWeight;
                s.NettoWeight = ms.StockProductDJCustomer.StockProductDJCustomer_CharProduct.GrossWeight;
                
                
            }

            //Ring-Size
            if (dimensir.Text != "")
                s.DimensiR = Convert.ToDecimal(dimensir.Text);
            else
                s.DimensiR = null;

            //Commit
            db.DocRepair_CharProduct.InsertOnSubmit(s);
            db.SubmitChanges();
        }
        else
        {
            d.GrossWeight = Convert.ToDecimal(grossweight.Text);
            d.NettoWeight = d.GrossWeight;
            //d.KadarLogam = Convert.ToDecimal(kadarlogam.Text);
            //d.KadarTukaran = Convert.ToDecimal(kadartukaran.Text);
            d.StoneQty = Convert.ToDecimal(0);

            //Dimensi
            d.DimensiD = Convert.ToDecimal(dimensid.Text);
            d.DimensiP = Convert.ToDecimal(dimensip.Text);
            d.DimensiL = Convert.ToDecimal(dimensil.Text);

            //Ring-Size
            if (dimensir.Text != "")
                d.DimensiR = Convert.ToDecimal(dimensir.Text);
            else
                d.DimensiR = null;

            //Commit
            db.SubmitChanges();
        }
    }

    //Move Stone
    protected void moveStone(StockProductDJ ms, DocRepair x)
    {
        moveStone1A(ms, x);
        moveStone1B(ms, x);
        moveStone2(ms, x);
        moveStone3(ms, x);
        moveStone4(ms, x);
        moveStone5(ms, x);
    }
    protected void moveStone1A(StockProductDJ ms, DocRepair x)
    {
        var z = db.DocRepair_Stone1A.Where(p => p.IDForm == x.ID);
        if (z != null)
        {
            db.DocRepair_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.StockProductDJ_Stone1A
                    orderby p.Stone1A.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepair_Stone1A n = new DocRepair_Stone1A();

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
                db.DocRepair_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone1B(StockProductDJ ms, DocRepair x)
    {
        var z = db.DocRepair_Stone1B.Where(p => p.IDForm == x.ID);
        if (z != null)
        {
            db.DocRepair_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.StockProductDJ_Stone1B
                    orderby p.Stone1B.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepair_Stone1B n = new DocRepair_Stone1B();

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

                n.GIA = r.GIA;
                n.StatusTerpasang = (int)((r.StatusTerpasang != null) ? r.StatusTerpasang : 1);

                n.HargaInputH = r.HargaInputH;
                n.HargaInputP = r.HargaInputP;

                //Commit
                db.DocRepair_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone2(StockProductDJ ms, DocRepair x)
    {
        var z = db.DocRepair_Stone2.Where(p => p.IDForm == x.ID);
        if (z != null)
        {
            db.DocRepair_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.StockProductDJ_Stone2
                    orderby p.Stone2.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepair_Stone2 n = new DocRepair_Stone2();

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

                n.GIA = r.GIA;

                //Commit
                db.DocRepair_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone3(StockProductDJ ms, DocRepair x)
    {
        var z = db.DocRepair_Stone3.Where(p => p.IDForm == x.ID);
        if (z != null)
        {
            db.DocRepair_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.StockProductDJ_Stone3
                    orderby p.Stone3.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepair_Stone3 n = new DocRepair_Stone3();

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

                n.GIA = r.GIA;

                //Commit
                db.DocRepair_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone4(StockProductDJ ms, DocRepair x)
    {
        var z = db.DocRepair_Stone4.Where(p => p.IDForm == x.ID);
        if (z != null)
        {
            db.DocRepair_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.StockProductDJ_Stone4
                    orderby p.Stone4.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepair_Stone4 n = new DocRepair_Stone4();

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

                n.GIA = r.GIA;

                //Commit
                db.DocRepair_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone5(StockProductDJ ms, DocRepair x)
    {
        var z = db.DocRepair_Stone5.Where(p => p.IDForm == x.ID);
        if (z != null)
        {
            db.DocRepair_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in ms.StockProductDJ_Stone5
                    orderby p.Stone5.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                DocRepair_Stone5 n = new DocRepair_Stone5();

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

                n.GIA = r.GIA;

                //Commit
                db.DocRepair_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
}