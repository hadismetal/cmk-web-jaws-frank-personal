using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ_PricingSubmit : System.Web.UI.Page
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
        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml = d.Nomor;
                //"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                //+ d.Nomor + "</a>";

            fillStone();
            fillProduct(d);
            
            fillBiaya(d);
            fillFinishing();
            fillMU(d);
            fillSegmen(d);
        }
    }

    protected void fillProduct(StockProductDJ ms)
    {
        var d = ms.StockProductDJ_PricingProduct;
        var x = ms.StockProductDJ_CostingProduct;
        if (d != null && x != null)
        {
            goldrate.InnerHtml = Str.Number2(x.GoldRate) + " / " + Str.Number2(d.GoldRate);
            netto.InnerHtml = Str.Number2(x.Netto) + " / " + Str.Number2(d.Netto);
            kadar.InnerHtml = Str.Number2(x.Kadar) + " / " + Str.Number2(d.Kadar);
            totalproduct.InnerHtml = Str.Number2(x.TotalProduct) + " / " + Str.Number2(d.TotalProduct);

            sum_frame.InnerHtml = Str.Number2(x.TotalProduct)  + " / " + Str.Number2(d.TotalProduct);
            sum_stone.InnerHtml = Str.Number2(x.TotalStone1A + x.TotalStone1B + x.TotalStone2 + x.TotalStone3 + x.TotalStone4 + x.TotalStone5) + " / " + Str.Number2(d.TotalStone1A + d.TotalStone1B + d.TotalStone2 + d.TotalStone3 + d.TotalStone4 + d.TotalStone5);
            sum_total.InnerHtml = Str.Number2(x.Total) + " / " + Str.Number2(d.Total);

            efektifgold.InnerHtml = Str.Tgl(d.EfektifGold);
        }
    }
    protected void fillBiaya(StockProductDJ ms)
    {
        var d = ms.StockProductDJ_PricingBiaya;
        var x = ms.StockProductDJ_Costing;
        if (d != null)
        {
            brand1.InnerHtml = d.NamaBrand;
            item1.InnerHtml = d.NamaItem;
            cons1.InnerHtml = d.NamaCons;

            no1.InnerHtml = Str.Number0(x.MountingNO) + " / " + Str.Number0(d.MountingNO);
            ro1.InnerHtml = Str.Number0(x.MountingRO) + " / " + Str.Number0(d.MountingRO);
            total1.InnerHtml = Str.Number0(x.MountingNO + x.MountingRO) + " / " + Str.Number0(d.Mounting);

            efektifmounting.InnerHtml = Str.Tgl(d.EfektifMounting);

            fee10.InnerHtml = Str.Number0(x.MountingNO + x.MountingRO) + " / " + Str.Number0(d.Mounting);
            fee20.InnerHtml = Str.Number0(x.Setting) + " / " + Str.Number0(d.Setting);
            fee30.InnerHtml = Str.Number0(x.Finishing) + " / " + Str.Number0(d.Finishing);
            fee31.InnerHtml = Str.Number0(x.OngkosLainCOGS) + " / " + Str.Number0(x.OngkosLain);

            fee11.InnerHtml = Str.Tgl(d.EfektifMounting);
            fee21.InnerHtml = Str.Tgl(d.EfektifSetting);

            feerupiah.InnerHtml = Str.Number0(x.TotalRupiah) + " / " + Str.Number0(d.TotalRupiah + x.OngkosLain);
            feerate.InnerHtml = Str.Number2(x.TotalRate)  + " / " + Str.Number2(d.TotalRate);
            feetotal.InnerHtml = Str.Number2(x.TotalBiaya) + " / " + Str.Number2(d.Total);

            efektifrate.InnerHtml = Str.Tgl(d.EfektifRate);

            if (ms.Sumber == 2)
                unitprice.InnerText = Str.Number0(x.UnitPrice);
            else
                unitbox.Visible = false;
        }
    }
    protected void fillMU(StockProductDJ ms)
    {
        decimal AllM = 0m;

        var d = ms.StockProductDJ_PricingMU;
        var x = ms.StockProductDJ_CostingProduct;
        var z = ms.StockProductDJ_Costing;

        AllM = (x.Total + z.TotalBiaya);

        if (d != null)
        {
            cat1.InnerHtml = d.NamaCategory;
            lvl1.InnerHtml = d.NamaLevel;
            dist1.InnerHtml = d.NamaDist;

            modal.InnerHtml = Str.Number2(AllM) + " / " + Str.Number2(d.ModalUSD);
            modalrate.InnerHtml = Str.Number2(z.TotalRate) + " / " + Str.Number2(d.ModalRate);
            modalrupiah.InnerHtml = Str.Number2(AllM * z.TotalRate) + " / " + Str.Number2(d.ModalRupiah);

            murate.InnerHtml = Str.Tgl(d.EfektifRate);

            mu_persen.InnerHtml = Str.Number2(d.MU);
            hargaidr.InnerHtml = Str.Number0(d.HargaRupiahRound);
            hargausd.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }
    protected void fillSegmen(StockProductDJ ms)
    {
        var d = ms.StockProductDJ_PricingSegmen;
        if (d != null)
        {
            cat2.InnerHtml = d.NamaCategory;
            lvl2.InnerHtml = d.NamaLevel;
            dist2.InnerHtml = d.NamaDist;

            segmensistem.InnerHtml = DL.SegmenSistem(d.SegmenSistem);
            if (d.SegmenSistem == 0)
                minimum.InnerHtml = Str.Number2(d.SegmenMinimum);
            else
                minimum.InnerHtml = Str.Number3(d.SegmenMinimum);
            efektifsegmen.InnerHtml = Str.Tgl(d.EfektifSegmen);

            segmen.InnerHtml = d.CharProductSegmen.Nama;

            carat.InnerHtml = Str.Number3(d.TotalCarat);
            hargasegmen.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }

    //Stone
    protected void fillStone()
    {
        int GT_Butir = 0;
        decimal GT_Harga = 0;
        

        //Stone
        fillStone1A(0, ref GT_Butir, ref GT_Harga, ref GT_HargaM);
        fillStone1B(1, ref GT_Butir, ref GT_Harga, ref GT_HargaM);
        fillStone2(2, ref GT_Butir, ref GT_Harga, ref GT_HargaM);
        fillStone3(3, ref GT_Butir, ref GT_Harga, ref GT_HargaM);
        fillStone4(4, ref GT_Butir, ref GT_Harga, ref GT_HargaM);
        fillStone5(5, ref GT_Butir, ref GT_Harga, ref GT_HargaM);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Harga, GT_HargaM, true);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_CaratM)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1A
                 where p.IDForm == myID
                 orderby p.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;
        decimal ST_HargaM = 0;

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
            td.Text = Str.Number2(r.HargaSatuanM) + " / " + Str.Number2(r.HargaSatuan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.HargaTotalM) + " / " + Str.Number2(r.HargaTotal);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Harga += r.HargaTotal.Value;
            ST_HargaM += r.HargaTotalM.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;
            GT_CaratM += ST_HargaM;

            Sub_Total(ST_Butir, ST_Harga, ST_HargaM, false);
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_CaratM)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1B
                 where p.IDForm == myID
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;
        decimal ST_HargaM = 0;

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
            td.Text = Str.Number2(r.HargaSatuanM) + " / " + Str.Number2(r.HargaSatuan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.HargaTotalM) + " / " + Str.Number2(r.HargaTotal);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Harga += r.HargaTotal.Value;
            ST_HargaM += r.HargaTotalM.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;
            GT_CaratM += ST_HargaM;

            Sub_Total(ST_Butir, ST_Harga, ST_HargaM, false);
        }
    }
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_CaratM)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone2
                 where p.IDForm == myID
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;
        decimal ST_HargaM = 0;

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
            td.Text = Str.Number2(r.HargaSatuanM) + " / " + Str.Number2(r.HargaSatuan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.HargaTotalM) + " / " + Str.Number2(r.HargaTotal);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Harga += r.HargaTotal.Value;
            ST_HargaM += r.HargaTotalM.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;
            GT_CaratM += ST_HargaM;

            Sub_Total(ST_Butir, ST_Harga, ST_HargaM, false);
        }
    }
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_CaratM)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone3
                 where p.IDForm == myID
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;
        decimal ST_HargaM = 0;

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
            td.Text = Str.Number2(r.HargaSatuanM) + " / " + Str.Number2(r.HargaSatuan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.HargaTotalM) + " / " + Str.Number2(r.HargaTotal);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Harga += r.HargaTotal.Value;
            ST_HargaM += r.HargaTotalM.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;
            GT_CaratM += ST_HargaM;

            Sub_Total(ST_Butir, ST_Harga, ST_HargaM, false);
        }
    }
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_CaratM)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone4
                 where p.IDForm == myID
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;
        decimal ST_HargaM = 0;

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
            td.Text = Str.Number2(r.HargaSatuanM) + " / " + Str.Number2(r.HargaSatuan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.HargaTotalM) + " / " + Str.Number2(r.HargaTotal);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Harga += r.HargaTotal.Value;
            ST_HargaM += r.HargaTotalM.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;
            GT_CaratM += ST_HargaM;

            Sub_Total(ST_Butir, ST_Harga, ST_HargaM, false);
        }
    }
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_CaratM)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone5
                 where p.IDForm == myID
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;
        decimal ST_HargaM = 0;

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
            td.Text = Str.Number2(r.HargaSatuanM) + " / " + Str.Number2(r.HargaSatuan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.HargaTotalM) + " / " + Str.Number2(r.HargaTotal);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Harga += r.HargaTotal.Value;
            ST_HargaM += r.HargaTotalM.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;
            GT_CaratM += ST_HargaM;

            Sub_Total(ST_Butir, ST_Harga, ST_HargaM, false);
        }
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
    protected void Sub_Total(int TotalButir, decimal TotalHarga, decimal TotalHargaM, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 5;
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(TotalButir);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number2(TotalHargaM) + " / " + Str.Number2(TotalHarga);
        td.CssClass = "text-right text-bold";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void fillFinishing()
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Finishing
                 where p.IDForm == myID
                 orderby p.CharProcessFinishing.SN
                 select p;

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
            td.Text = r.CharProcessFinishing.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.BiayaModal) + " / " + Str.Number2(r.Biaya);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tbff.Rows.Add(tr);
        }
    }


    //Save
    //protected void save_Click(object sender, EventArgs e)
    //{
    //    var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
    //    if (d == null) App.DataNull(this);
    //    else
    //    {
    //        //Status
    //        if (d.ProtoNomor != null)
    //        {
    //            var s = db.KodeKaret_Detail.SingleOrDefault(p => p.KodeKaretDetail == "" + d.ProtoNomor + "");
    //            if (s != null)
    //            {
    //                var x = db.ProductOrder.SingleOrDefault(p => p.ID == s.IDProductOrder);
    //                if (x != null)
    //                    x.StatusPenerimaan = 1;

    //                var kd = db.KodeKaretDJ.SingleOrDefault(p => p.ID == s.IDKodeKaret);
    //                if (kd != null)
    //                {
    //                    //kd.IDBrand = d.LocWarehouse.IDBrand;
    //                    //kd.IDDesigner = d.HumanDesigner;
    //                    kd.IDSegmen = d.StockProductDJ_PricingSegmen.ProductSegmen;
    //                    kd.Keterangan = d.Keterangan;
    //                    kd.KodeKaretDJ_CharDesign.DesignCategory = d.StockProductDJ_CharDesign.DesignCategory;
    //                    kd.KodeKaretDJ_CharDesign.DesignConcept = d.StockProductDJ_CharDesign.DesignConcept;
    //                    kd.KodeKaretDJ_CharDesign.DesignProcess = d.StockProductDJ_CharDesign.DesignProcess;
    //                    kd.KodeKaretDJ_CharDesign.TargetAge = d.StockProductDJ_CharDesign.TargetAge;
    //                    kd.KodeKaretDJ_CharDesign.TargetGender = d.StockProductDJ_CharDesign.TargetGender;
    //                    kd.KodeKaretDJ_CharDesign.Naming = d.StockProductDJ_CharDesign.Naming;
    //                    kd.KodeKaretDJ_CharDesign.SetItem = d.StockProductDJ_CharDesign.SetItem;
    //                    kd.KodeKaretDJ_CharDesign.Special = d.StockProductDJ_CharDesign.Special;
    //                    kd.KodeKaretDJ_CharDesign.Grafir = d.StockProductDJ_CharDesign.Grafir;
    //                    kd.KodeKaretDJ_CharProduct.ProductItem = d.StockProductDJ_CharProduct.ProductItem;
    //                    kd.KodeKaretDJ_CharProduct.ProductCategory = d.StockProductDJ_CharProduct.ProductCategory;
    //                    kd.KodeKaretDJ_CharProduct.ProductLevel = d.StockProductDJ_CharProduct.ProductLevel;
    //                    kd.KodeKaretDJ_CharProduct.StoneDist = d.StockProductDJ_CharProduct.StoneDist;
    //                    kd.KodeKaretDJ_CharProduct.FrameMaterial = d.StockProductDJ_CharProduct.FrameMaterial;
    //                    kd.KodeKaretDJ_CharProduct.FrameFinishing = d.StockProductDJ_CharProduct.FrameFinishing;
    //                    kd.KodeKaretDJ_CharProduct.FrameColor = d.StockProductDJ_CharProduct.FrameColor;
    //                    kd.KodeKaretDJ_CharProduct.ProcessCons = d.StockProductDJ_CharProduct.ProcessCons;
    //                    kd.KodeKaretDJ_CharProduct.GrossWeight = d.StockProductDJ_CharProduct.GrossWeight;
    //                    kd.KodeKaretDJ_CharProduct.StoneCarat = d.StockProductDJ_CharProduct.StoneCarat;
    //                    kd.KodeKaretDJ_CharProduct.StoneWeight = d.StockProductDJ_CharProduct.StoneWeight;
    //                    kd.KodeKaretDJ_CharProduct.NettoWeight = d.StockProductDJ_CharProduct.NettoWeight;
    //                    kd.KodeKaretDJ_CharProduct.KadarLogam = d.StockProductDJ_CharProduct.KadarLogam;
    //                    kd.KodeKaretDJ_CharProduct.KadarTukaran = d.StockProductDJ_CharProduct.KadarTukaran;
    //                    kd.KodeKaretDJ_CharProduct.DimensiD = d.StockProductDJ_CharProduct.DimensiD;
    //                    kd.KodeKaretDJ_CharProduct.DimensiP = d.StockProductDJ_CharProduct.DimensiP;
    //                    kd.KodeKaretDJ_CharProduct.DimensiL = d.StockProductDJ_CharProduct.DimensiL;
    //                    kd.KodeKaretDJ_CharProduct.DimensiR = d.StockProductDJ_CharProduct.DimensiR;

    //                    db.SubmitChanges();

    //                    updateFinishing(s.IDKodeKaret, d);
    //                    updateStone(s.IDKodeKaret, d);

                        

    //                }

    //            }
    //        }
    //        //if (d.Sumber == 3 && d.IDMounting != null)
    //        //{
    //        //    var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting && p.StatusReceive == 0 && p.Draft == false);
    //        //    if (mt != null)
    //        //    {
    //        //        mt.StatusReceive = 1;
    //        //        var mtp = db.StockProductMounting.SingleOrDefault(p => p.Nomor == mt.Nomor && p.StatusReceive == false);
    //        //        if (mtp != null)
    //        //            mtp.StatusReceive = true;
    //        //        db.SubmitChanges();
    //        //    }

                

    //        //}
            

    //        //User-Info
    //        //d.Nomor = Generate_Nomor(d);
    //        d.Approval = App.UserNama;
    //        d.ApprovalTgl = DateTime.Now;

    //        //Commit
    //        db.SubmitChanges();

    //        StockProductDJ prod = new StockProductDJ();
    //        prod.IDWarehouse = d.IDWarehouse;
    //        prod.Sumber = d.Sumber;
    //        prod.Keterangan = d.Keterangan;
    //        prod.Tgl = d.Tgl;
    //        prod.Nomor = d.Nomor;
    //        prod.Draft = d.Draft;
    //        prod.DraftDate = d.DraftDate;
    //        prod.Proto = d.Proto;
    //        prod.ProtoNomor = d.ProtoNomor;
    //        prod.ProductOrder = d.ProductOrder;
    //        prod.ProductOrderNomor = d.ProductOrderNomor;
    //        prod.Supplier = d.Supplier;
    //        prod.SupplierNama = d.SupplierNama;
    //        prod.SupplierNomor = d.SupplierNomor;
    //        prod.RequestOutlet = d.RequestOutlet;
    //        prod.RequestOutletNama = d.RequestOutletNama;
    //        prod.RequestCustomer = d.RequestCustomer;
    //        prod.RequestCustomerNama = d.RequestCustomerNama;
    //        prod.HumanDesigner = d.HumanDesigner;
    //        //prod.HumanDesginerNama = d.HumanDesignerNama;
    //        prod.HumanStock = d.HumanStock;
    //        prod.HumanStockNama = d.HumanStockNama;
    //        prod.ExpiredProduct = d.ExpiredProduct;
    //        prod.ExpiredRequest = d.ExpiredRequest;
    //        prod.Operator = d.Operator;
    //        prod.OperatorTgl = d.OperatorTgl;
    //        prod.Approval = d.Approval;
    //        prod.ApprovalTgl = d.ApprovalTgl;
    //        prod.CatatanManager = d.CatatanManager;
    //        prod.ImgPicture = d.ImgPicture;
    //        prod.ReturnProduct = false;
    //        prod.StatusPenjualan = 0;
    //        prod.StatusLebur = false;

    //        db.StockProductDJ.InsertOnSubmit(prod);
    //        db.SubmitChanges();

    //        int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM StockProductDJ ORDER BY ID DESC",Db.Cnn_DB);

    //        StockProductDJ_CharDesign dsg = new StockProductDJ_CharDesign();
    //        dsg.ID = ProdID;
    //        dsg.DesignCategory = d.StockProductDJ_CharDesign.DesignCategory;
    //        dsg.DesignConcept = d.StockProductDJ_CharDesign.DesignConcept;
    //        dsg.DesignProcess = d.StockProductDJ_CharDesign.DesignProcess;
    //        dsg.TargetAge = d.StockProductDJ_CharDesign.TargetAge;
    //        dsg.TargetGender = d.StockProductDJ_CharDesign.TargetGender;
    //        dsg.Naming = d.StockProductDJ_CharDesign.Naming;
    //        dsg.SetItem = d.StockProductDJ_CharDesign.SetItem;
    //        dsg.Special = d.StockProductDJ_CharDesign.Special;
    //        dsg.Grafir = d.StockProductDJ_CharDesign.Grafir;

    //        db.StockProductDJ_CharDesign.InsertOnSubmit(dsg);
    //        db.SubmitChanges();

    //        StockProductDJ_CharProduct pd = new StockProductDJ_CharProduct();
    //        pd.ID = ProdID;
    //        pd.ProductItem = d.StockProductDJ_CharProduct.ProductItem;
    //        pd.ProductCategory = d.StockProductDJ_CharProduct.ProductCategory;
    //        pd.ProductLevel = d.StockProductDJ_CharProduct.ProductLevel;
    //        pd.StoneDist = d.StockProductDJ_CharProduct.StoneDist;
    //        pd.FrameColor = d.StockProductDJ_CharProduct.FrameColor;
    //        pd.FrameFinishing = d.StockProductDJ_CharProduct.FrameFinishing;
    //        pd.FrameMaterial = d.StockProductDJ_CharProduct.FrameMaterial;
    //        pd.ProcessCons = d.StockProductDJ_CharProduct.ProcessCons;
    //        pd.GrossWeight = d.StockProductDJ_CharProduct.GrossWeight;
    //        pd.StoneCarat = d.StockProductDJ_CharProduct.StoneCarat;
    //        pd.StoneWeight = d.StockProductDJ_CharProduct.StoneWeight;
    //        pd.NettoWeight = d.StockProductDJ_CharProduct.NettoWeight;
    //        pd.KadarLogam = d.StockProductDJ_CharProduct.KadarLogam;
    //        pd.KadarTukaran = d.StockProductDJ_CharProduct.KadarTukaran;
    //        pd.DimensiD = d.StockProductDJ_CharProduct.DimensiD;
    //        pd.DimensiL = d.StockProductDJ_CharProduct.DimensiL;
    //        pd.DimensiP = d.StockProductDJ_CharProduct.DimensiP;
    //        pd.DimensiR = d.StockProductDJ_CharProduct.DimensiR;
    //        pd.StoneQty = d.StockProductDJ_CharProduct.StoneQty;

    //        db.StockProductDJ_CharProduct.InsertOnSubmit(pd);
    //        db.SubmitChanges();

    //        StockProductDJ_Costing cost = new StockProductDJ_Costing();
    //        cost.ID = ProdID;
    //        cost.UnitPrice = d.StockProductDJ_Costing.UnitPrice;
    //        cost.Frame = d.StockProductDJ_Costing.Frame;
    //        cost.MountingNO = d.StockProductDJ_Costing.MountingNO;
    //        //cost.MountingRO = d.StockProductDJ_Costing.MountingRO.Value;
    //        cost.Pembagi = d.StockProductDJ_Costing.Pembagi.Value;
    //        cost.Setting = d.StockProductDJ_Costing.Setting;
    //        cost.Finishing = d.StockProductDJ_Costing.Finishing;
    //        cost.OngkosLainCOGS = d.StockProductDJ_Costing.OngkosLainCOGS;
    //        cost.OngkosLain = d.StockProductDJ_Costing.OngkosLain;
    //        cost.TotalRate = d.StockProductDJ_Costing.TotalRate.Value;
    //        cost.TotalRupiah = d.StockProductDJ_Costing.TotalRupiah.Value;
    //        cost.TotalBiaya = d.StockProductDJ_Costing.TotalBiaya;

    //        db.StockProductDJ_Costing.InsertOnSubmit(cost);
    //        db.SubmitChanges();

    //        StockProductDJ_CostingProduct cp = new StockProductDJ_CostingProduct();
    //        cp.ID = ProdID;
    //        cp.GoldRate = d.StockProductDJ_CostingProduct.GoldRate;
    //        cp.Netto = d.StockProductDJ_CostingProduct.Netto;
    //        cp.Kadar = d.StockProductDJ_CostingProduct.Kadar;
    //        cp.TotalProduct = d.StockProductDJ_CostingProduct.TotalProduct;
    //        cp.TotalStone1A = d.StockProductDJ_CostingProduct.TotalStone1A;
    //        cp.TotalStone1B = d.StockProductDJ_CostingProduct.TotalStone1B;
    //        cp.TotalStone2 = d.StockProductDJ_CostingProduct.TotalStone2;
    //        cp.TotalStone3 = d.StockProductDJ_CostingProduct.TotalStone3;
    //        cp.TotalStone4 = d.StockProductDJ_CostingProduct.TotalStone4;
    //        cp.TotalStone5 = d.StockProductDJ_CostingProduct.TotalStone5;
    //        cp.Total = d.StockProductDJ_CostingProduct.Total;
    //        cp.EfektifGold = d.StockProductDJ_CostingProduct.EfektifGold;

    //        db.StockProductDJ_CostingProduct.InsertOnSubmit(cp);
    //        db.SubmitChanges();

    //        var l_fns = from p in db.StockProductDJ_Finishing
    //                    where p.IDForm == d.ID
    //                    select p;
    //        if (l_fns != null)
    //        {
    //            foreach (var r in l_fns)
    //            {
    //                StockProductDJ_Finishing fns = new StockProductDJ_Finishing();
    //                fns.IDForm = ProdID;
    //                fns.IDFinishing = r.IDFinishing;
    //                fns.Biaya = r.Biaya;
    //                fns.BiayaModal = r.BiayaModal;
    //                fns.Efektif = r.Efektif;
    //                db.StockProductDJ_Finishing.InsertOnSubmit(fns);
    //                db.SubmitChanges();
    //            }
    //        }

    //        StockProductDJ_PricingBiaya by = new StockProductDJ_PricingBiaya();
    //        by.ID = ProdID;
    //        by.NamaBrand = d.StockProductDJ_PricingBiaya.NamaBrand;
    //        by.NamaItem = d.StockProductDJ_PricingBiaya.NamaItem;
    //        by.NamaCons = d.StockProductDJ_PricingBiaya.NamaCons;
    //        by.Mounting = d.StockProductDJ_PricingBiaya.Mounting;
    //        by.MountingNO = d.StockProductDJ_PricingBiaya.MountingNO;
    //        by.MountingRO = d.StockProductDJ_PricingBiaya.MountingRO;
    //        by.Setting = d.StockProductDJ_PricingBiaya.Setting;
    //        by.Finishing = d.StockProductDJ_PricingBiaya.Finishing;
    //        by.TotalRupiah = d.StockProductDJ_PricingBiaya.TotalRupiah;
    //        by.TotalRate = d.StockProductDJ_PricingBiaya.TotalRate;
    //        by.Total = d.StockProductDJ_PricingBiaya.Total;
    //        by.EfektifRate = d.StockProductDJ_PricingBiaya.EfektifRate;
    //        by.EfektifMounting = d.StockProductDJ_PricingBiaya.EfektifMounting;
    //        by.EfektifSetting = d.StockProductDJ_PricingBiaya.EfektifSetting;

    //        db.StockProductDJ_PricingBiaya.InsertOnSubmit(by);
    //        db.SubmitChanges();

    //        StockProductDJ_PricingMU mu = new StockProductDJ_PricingMU();
    //        mu.ID = ProdID;
    //        mu.NamaCategory = d.StockProductDJ_PricingMU.NamaCategory;
    //        mu.NamaLevel = d.StockProductDJ_PricingMU.NamaLevel;
    //        mu.NamaDist = d.StockProductDJ_PricingMU.NamaDist;
    //        mu.ModalUSD = d.StockProductDJ_PricingMU.ModalUSD;
    //        mu.ModalRate = d.StockProductDJ_PricingMU.ModalRate;
    //        mu.ModalRupiah = d.StockProductDJ_PricingMU.ModalRupiah;
    //        mu.MU = d.StockProductDJ_PricingMU.MU;
    //        mu.HargaUSD = d.StockProductDJ_PricingMU.HargaUSD;
    //        mu.HargaRupiah = d.StockProductDJ_PricingMU.HargaRupiah;
    //        mu.HargaRupiahRound = d.StockProductDJ_PricingMU.HargaRupiahRound;
    //        mu.EfektifRate = d.StockProductDJ_PricingMU.EfektifRate;

    //        db.StockProductDJ_PricingMU.InsertOnSubmit(mu);
    //        db.SubmitChanges();

    //        StockProductDJ_PricingProduct prc = new StockProductDJ_PricingProduct();
    //        prc.ID = ProdID;
    //        prc.GoldRate = d.StockProductDJ_PricingProduct.GoldRate;
    //        prc.Netto = d.StockProductDJ_PricingProduct.Netto;
    //        prc.Kadar = d.StockProductDJ_PricingProduct.Kadar;
    //        prc.TotalProduct = d.StockProductDJ_PricingProduct.TotalProduct;
    //        prc.TotalStone1A = d.StockProductDJ_PricingProduct.TotalStone1A;
    //        prc.TotalStone1B = d.StockProductDJ_PricingProduct.TotalStone1B;
    //        prc.TotalStone2 = d.StockProductDJ_PricingProduct.TotalStone2;
    //        prc.TotalStone3 = d.StockProductDJ_PricingProduct.TotalStone3;
    //        prc.TotalStone4 = d.StockProductDJ_PricingProduct.TotalStone4;
    //        prc.TotalStone5 = d.StockProductDJ_PricingProduct.TotalStone5;
    //        prc.Total = d.StockProductDJ_PricingProduct.Total;
    //        prc.EfektifGold = d.StockProductDJ_PricingProduct.EfektifGold;

    //        db.StockProductDJ_PricingProduct.InsertOnSubmit(prc);
    //        db.SubmitChanges();

    //        StockProductDJ_PricingSegmen sg = new StockProductDJ_PricingSegmen();
    //        sg.ID = ProdID;
    //        sg.ProductSegmen = d.StockProductDJ_PricingSegmen.ProductSegmen;
    //        sg.NamaCategory = d.StockProductDJ_PricingSegmen.NamaCategory;
    //        sg.NamaLevel = d.StockProductDJ_PricingSegmen.NamaLevel;
    //        sg.NamaDist = d.StockProductDJ_PricingSegmen.NamaDist;
    //        sg.SegmenSistem = d.StockProductDJ_PricingSegmen.SegmenSistem;
    //        sg.SegmenMinimum = d.StockProductDJ_PricingSegmen.SegmenMinimum;
    //        sg.TotalCarat = d.StockProductDJ_PricingSegmen.TotalCarat;
    //        sg.HargaUSD = d.StockProductDJ_PricingSegmen.HargaUSD;
    //        sg.EfektifSegmen = d.StockProductDJ_PricingSegmen.EfektifSegmen;

    //        db.StockProductDJ_PricingSegmen.InsertOnSubmit(sg);
    //        db.SubmitChanges();

    //        var s1A = from p in db.StockProductDJ_Stone1A
    //                    where p.IDForm == d.ID
    //                    select p;
    //        if (s1A != null)
    //        {
    //            foreach (var r in s1A)
    //            {
    //                StockProductDJ_Stone1A stone = new StockProductDJ_Stone1A();
    //                stone.IDForm = ProdID;
    //                stone.IDStone = r.IDStone;
    //                stone.TotalButir = r.TotalButir;
    //                stone.TotalCarat = r.TotalCarat;
    //                stone.CaratButir = r.CaratButir;
    //                stone.DimensiL = r.DimensiL;
    //                stone.DimensiP = r.DimensiP;
    //                stone.DimensiT = r.DimensiT;
    //                stone.HargaSatuan = r.HargaSatuan;
    //                stone.HargaTotal = r.HargaTotal;
    //                stone.HargaSatuanM = r.HargaSatuanM;
    //                stone.HargaTotalM = r.HargaTotalM;
    //                stone.Efektif = r.Efektif;
    //                stone.Setting = r.Setting;
    //                db.StockProductDJ_Stone1A.InsertOnSubmit(stone);
    //                db.SubmitChanges();
    //            }
    //        }

    //        var s1B = from p in db.StockProductDJ_Stone1B
    //                  where p.IDForm == d.ID
    //                  select p;
    //        if (s1B != null)
    //        {
    //            foreach (var r in s1B)
    //            {
    //                StockProductDJ_Stone1B stone = new StockProductDJ_Stone1B();
    //                stone.IDForm = ProdID;
    //                stone.IDStone = r.IDStone;
    //                stone.TotalButir = r.TotalButir;
    //                stone.TotalCarat = r.TotalCarat;
    //                stone.CaratButir = r.CaratButir;
    //                stone.DimensiL = r.DimensiL;
    //                stone.DimensiP = r.DimensiP;
    //                stone.DimensiT = r.DimensiT;
    //                stone.HargaSatuan = r.HargaSatuan;
    //                stone.HargaTotal = r.HargaTotal;
    //                stone.GIA = r.GIA;
    //                stone.HargaInputH = r.HargaInputH;
    //                stone.HargaInputP = r.HargaInputP;
    //                stone.HargaSatuanM = r.HargaSatuanM;
    //                stone.HargaTotalM = r.HargaTotalM;
    //                stone.Efektif = r.Efektif;
    //                stone.Setting = r.Setting;
    //                db.StockProductDJ_Stone1B.InsertOnSubmit(stone);
    //                db.SubmitChanges();
    //            }
    //        }

    //        var s2 = from p in db.StockProductDJ_Stone2
    //                  where p.IDForm == d.ID
    //                  select p;
    //        if (s2 != null)
    //        {
    //            foreach (var r in s2)
    //            {
    //                StockProductDJ_Stone2 stone = new StockProductDJ_Stone2();
    //                stone.IDForm = ProdID;
    //                stone.IDStone = r.IDStone;
    //                stone.TotalButir = r.TotalButir;
    //                stone.TotalCarat = r.TotalCarat;
    //                stone.CaratButir = r.CaratButir;
    //                stone.DimensiL = r.DimensiL;
    //                stone.DimensiP = r.DimensiP;
    //                stone.DimensiT = r.DimensiT;
    //                stone.HargaSatuan = r.HargaSatuan;
    //                stone.HargaTotal = r.HargaTotal;
    //                stone.HargaSatuanM = r.HargaSatuanM;
    //                stone.HargaTotalM = r.HargaTotalM;
    //                stone.Efektif = r.Efektif;
    //                stone.Setting = r.Setting;
    //                db.StockProductDJ_Stone2.InsertOnSubmit(stone);
    //                db.SubmitChanges();
    //            }
    //        }

    //        var s3 = from p in db.StockProductDJ_Stone3
    //                 where p.IDForm == d.ID
    //                 select p;
    //        if (s3 != null)
    //        {
    //            foreach (var r in s3)
    //            {
    //                StockProductDJ_Stone3 stone = new StockProductDJ_Stone3();
    //                stone.IDForm = ProdID;
    //                stone.IDStone = r.IDStone;
    //                stone.TotalButir = r.TotalButir;
    //                stone.TotalCarat = r.TotalCarat;
    //                stone.CaratButir = r.CaratButir;
    //                stone.DimensiL = r.DimensiL;
    //                stone.DimensiP = r.DimensiP;
    //                stone.DimensiT = r.DimensiT;
    //                stone.HargaSatuan = r.HargaSatuan;
    //                stone.HargaTotal = r.HargaTotal;
    //                stone.HargaSatuanM = r.HargaSatuanM;
    //                stone.HargaTotalM = r.HargaTotalM;
    //                stone.Efektif = r.Efektif;
    //                stone.Setting = r.Setting;
    //                db.StockProductDJ_Stone3.InsertOnSubmit(stone);
    //                db.SubmitChanges();
    //            }
    //        }

    //        var s4 = from p in db.StockProductDJ_Stone4
    //                 where p.IDForm == d.ID
    //                 select p;
    //        if (s4 != null)
    //        {
    //            foreach (var r in s4)
    //            {
    //                StockProductDJ_Stone4 stone = new StockProductDJ_Stone4();
    //                stone.IDForm = ProdID;
    //                stone.IDStone = r.IDStone;
    //                stone.TotalButir = r.TotalButir;
    //                stone.TotalCarat = r.TotalCarat;
    //                stone.CaratButir = r.CaratButir;
    //                stone.DimensiL = r.DimensiL;
    //                stone.DimensiP = r.DimensiP;
    //                stone.DimensiT = r.DimensiT;
    //                stone.HargaSatuan = r.HargaSatuan;
    //                stone.HargaTotal = r.HargaTotal;
    //                stone.HargaSatuanM = r.HargaSatuanM;
    //                stone.HargaTotalM = r.HargaTotalM;
    //                stone.Efektif = r.Efektif;
    //                stone.Setting = r.Setting;
    //                db.StockProductDJ_Stone4.InsertOnSubmit(stone);
    //                db.SubmitChanges();
    //            }
    //        }


    //        var s5 = from p in db.StockProductDJ_Stone5
    //                 where p.IDForm == d.ID
    //                 select p;
    //        if (s5 != null)
    //        {
    //            foreach (var r in s5)
    //            {
    //                StockProductDJ_Stone5 stone = new StockProductDJ_Stone5();
    //                stone.IDForm = ProdID;
    //                stone.IDStone = r.IDStone;
    //                stone.TotalButir = r.TotalButir;
    //                stone.TotalCarat = r.TotalCarat;
    //                stone.CaratButir = r.CaratButir;
    //                stone.DimensiL = r.DimensiL;
    //                stone.DimensiP = r.DimensiP;
    //                stone.DimensiT = r.DimensiT;
    //                stone.HargaSatuan = r.HargaSatuan;
    //                stone.HargaTotal = r.HargaTotal;
    //                stone.HargaSatuanM = r.HargaSatuanM;
    //                stone.HargaTotalM = r.HargaTotalM;
    //                stone.Efektif = r.Efektif;
    //                stone.Setting = r.Setting;
    //                db.StockProductDJ_Stone5.InsertOnSubmit(stone);
    //                db.SubmitChanges();
    //            }
    //        }

    //        StockActualDJ stock = new StockActualDJ();
    //        stock.IDProduct = ProdID;
    //        stock.NamaLokasi = d.LocWarehouse.Nama;
    //        stock.IDBrand = d.LocWarehouse.CompanyBrand.ID;
    //        stock.TipeLokasi = 0;
    //        //stock.IDLokasi = d.IDWarehouse;
    //        stock.Substorage = 1;
    //        stock.Keterangan = d.Keterangan;
    //        //stock.Tgl = d.Tgl;
    //        stock.Nomor = d.Nomor;
    //        stock.InHand = 1;
    //        stock.Operator = d.Operator;
    //        //stock.OperatorTgl = d.OperatorTgl;
    //        db.StockActualDJ.InsertOnSubmit(stock);
    //        db.SubmitChanges();


    //        //Redirect
    //        App.ReloadClose(this);
    //    }
    //}

    protected void updateFinishing(int IDKodeKaret, StockReceiveMounting mt)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Finishing WHERE IDForm = " + IDKodeKaret);
        db.SubmitChanges();

        var rs = mt.StockReceiveMounting_Finishing;

        foreach (var r in rs)
        {
            KodeKaretDJ_Finishing fns = new KodeKaretDJ_Finishing();
            fns.IDForm = IDKodeKaret;
            fns.IDFinishing = r.IDFinishing;
            fns.Biaya = r.Biaya;
            fns.Efektif = r.Efektif;
            db.KodeKaretDJ_Finishing.InsertOnSubmit(fns);
            db.SubmitChanges();
        }
    }

    protected void updateFinishing(int IDKodeKaret, StockProductDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Finishing WHERE IDForm = " + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockProductDJ_Finishing;

        foreach (var r in rs)
        {
            KodeKaretDJ_Finishing fns = new KodeKaretDJ_Finishing();
            fns.IDForm = IDKodeKaret;
            fns.IDFinishing = r.IDFinishing;
            fns.Biaya = r.Biaya;
            fns.Efektif = r.Efektif;
            db.KodeKaretDJ_Finishing.InsertOnSubmit(fns);
            db.SubmitChanges();
        }
    }

    protected void updateStone(int IDKodeKaret, StockProductDJ d)
    {
        updateStone1A(IDKodeKaret, d);
        updateStone1B(IDKodeKaret, d);
        updateStone2(IDKodeKaret, d);
        updateStone3(IDKodeKaret, d);
        updateStone4(IDKodeKaret, d);
        updateStone5(IDKodeKaret, d);
    }
    protected void updateStone1A(int IDKodeKaret, StockProductDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone1A WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockProductDJ_Stone1A;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                KodeKaretDJ_Stone1A stone = new KodeKaretDJ_Stone1A();
                stone.IDForm = IDKodeKaret;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                db.KodeKaretDJ_Stone1A.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }
    }

    protected void updateStone1B(int IDKodeKaret, StockProductDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone1B WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockProductDJ_Stone1B;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                KodeKaretDJ_Stone1B stone = new KodeKaretDJ_Stone1B();
                stone.IDForm = IDKodeKaret;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.GIA = r.GIA;
                db.KodeKaretDJ_Stone1B.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }
    }
    protected void updateStone2(int IDKodeKaret, StockProductDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone2 WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockProductDJ_Stone2;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                KodeKaretDJ_Stone2 stone = new KodeKaretDJ_Stone2();
                stone.IDForm = IDKodeKaret;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                db.KodeKaretDJ_Stone2.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }
    }
    protected void updateStone3(int IDKodeKaret, StockProductDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone3 WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockProductDJ_Stone3;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                KodeKaretDJ_Stone3 stone = new KodeKaretDJ_Stone3();
                stone.IDForm = IDKodeKaret;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                db.KodeKaretDJ_Stone3.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }
    }
    protected void updateStone4(int IDKodeKaret, StockProductDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone4 WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockProductDJ_Stone4;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                KodeKaretDJ_Stone4 stone = new KodeKaretDJ_Stone4();
                stone.IDForm = IDKodeKaret;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                db.KodeKaretDJ_Stone4.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }
    }

    protected void updateStone5(int IDKodeKaret, StockProductDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone5 WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockProductDJ_Stone5;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                KodeKaretDJ_Stone5 stone = new KodeKaretDJ_Stone5();
                stone.IDForm = IDKodeKaret;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                db.KodeKaretDJ_Stone5.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }
    }

    //protected string Generate_Nomor(StockProductDJ d)
    //{
    //    string x = string.Empty;

    //    //int Tahun = d.Tgl.Year;
    //    //int Bulan = d.Tgl.Month;

    //    int Tahun = System.DateTime.Now.Year;
    //    int Bulan = System.DateTime.Now.Month;

    //    /*int Nomor = db.StockProductDJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
    //    Nomor = Nomor + 1;

    //    return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
    //    int Nomor = 1;

    //    try
    //    {
    //        var s = (from p in db.StockProductDJ
    //                 //where p.Tgl.Year == Tahun /*&& p.Tgl.Month == Bulan*/
    //                 && p.Nomor != null
    //                 orderby (p.Nomor.Substring(1, 8)) descending
    //                 select p).First();

    //        if (s.Nomor != null)
    //            Nomor = Convert.ToInt32(s.Nomor.Substring(3, 6)) + 1;
    //    }
    //    catch (Exception ex) { }

    //    string PLU = "D" + Tahun.ToString().Remove(0, 2) + Str.KodeFormat(Nomor, 6);

    //    return PLU;

    //}
}