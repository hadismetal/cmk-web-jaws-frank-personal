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
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            fillStone();
            fillProduct(d);
            
            fillBiaya(d);
            fillFinishing();
            fillMU(d);
            fillSegmen(d);
        }
    }

    protected void fillProduct(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_PricingProduct;
        var x = ms.StockReceiveDJ_CostingProduct;
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
    protected void fillBiaya(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_PricingBiaya;
        var x = ms.StockReceiveDJ_Costing;
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
    protected void fillMU(StockReceiveDJ ms)
    {
        decimal AllM = 0m;

        var d = ms.StockReceiveDJ_PricingMU;
        var x = ms.StockReceiveDJ_CostingProduct;
        var z = ms.StockReceiveDJ_Costing;

        AllM = (x.Total + z.TotalBiaya);

        if (d != null)
        {
            cat1.InnerHtml = d.NamaCategory;
            lvl1.InnerHtml = d.NamaLevel;
            dist1.InnerHtml = d.NamaDist;

            modal.InnerHtml = Str.Number2(AllM) + " / " + Str.Number2(d.ModalUSD);
            //modalrate.InnerHtml = Str.Number2(z.TotalRate) + " / " + Str.Number2(d.ModalRate);
            modalrate.InnerHtml = Str.Number2(d.ModalRate);
            //modalrupiah.InnerHtml = Str.Number2(AllM * z.TotalRate) + " / " + Str.Number2(d.ModalRupiah);

            murate.InnerHtml = Str.Tgl(d.EfektifRate);

            mu_persen.InnerHtml = Str.Number2(d.MU);
            hargaidr.InnerHtml = Str.Number0(d.HargaRupiahRound);
            hargausd.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }
    protected void fillSegmen(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_PricingSegmen;
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
        var rs = from p in db.StockReceiveDJ_Stone1A
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
        var rs = from p in db.StockReceiveDJ_Stone1B
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

            string Status = "";
            if (r.StatusTerpasang == 1)
                Status = "<br /><span class=\"text-sm text-silver\">Terpasang</span>";
            else
                Status = "<br /><span class=\"text-sm text-silver\">Tidak Terpasang</span>";

            td = new TableCell();
            td.Text = r.Stone1B.Nama + Status;
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
        var rs = from p in db.StockReceiveDJ_Stone2
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
        var rs = from p in db.StockReceiveDJ_Stone3
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
        var rs = from p in db.StockReceiveDJ_Stone4
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
        var rs = from p in db.StockReceiveDJ_Stone5
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
        td.Text = x; td.Font.Size = 12;
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
        var rs = from p in db.StockReceiveDJ_Finishing
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
}