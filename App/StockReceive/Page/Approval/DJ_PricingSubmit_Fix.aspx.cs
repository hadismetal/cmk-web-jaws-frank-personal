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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
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

            btnApprove.Visible = true;
            btnFix.Visible = false;
            btnPrint.Visible = false;

            // if (d.Approval != null)
            // {
            //     // kondisi jika error approve
            //     var sa = db.StockActualDJ.SingleOrDefault(p => p.Nomor == d.Nomor);
            //     var sp = db.StockProductDJ.SingleOrDefault(p => p.Nomor == d.Nomor);
                
            //     if (sa == null || sp == null)
            //     {
            //         if (d.Tgl.AddDays(7) >= System.DateTime.Now)
            //         {
            //             lit_error_msg.Text = "<span style='color:red;'>Tombol Fix hanya dibuka untuk 7 hari ke depan sejak barang diinput.</span>";
            //             btnFix.Visible = true;
            //         }
            //     }
            //     else
            //     {
            //         btnPrint.Visible = true;
            //     }
                
            // }
            // else
            // {
            //     btnApprove.Visible = true;
            // }


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

            if (ms.Sumber == 2)
            {
                decimal Rate = ms.StockReceiveDJ_Costing.TotalRate.Value;
                decimal UnitPrice = ms.StockReceiveDJ_Costing.UnitPrice.Value;
                decimal UnitPriceUSD = UnitPrice / Rate;
                modal.InnerHtml = Str.Number2(UnitPriceUSD) + " / " + Str.Number2(AllM) + " / " + Str.Number2(d.ModalUSD);
            }
            else
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
			
			string GIA = Db.SingleString("SELECT TOP 1 ISNULL(CertificateNumber,'') FROM StockReceiveDJ_Stone1A WHERE ID = "+r.ID.ToString()+" ", Db.Cnn_DB);
            string tglGIA = Db.SingleString("SELECT TOP 1 ISNULL(TglCertificate,'') FROM StockReceiveDJ_Stone1A WHERE ID = " + r.ID.ToString() + " ", Db.Cnn_DB);

            // td = new TableCell();
            // td.Text = TD_Nama_stone(r.Stone1A.Nama, (Str.Number3(r.Stone1A.Parcel102.SizeMin) + " - " + Str.Number3(r.Stone1A.Parcel102.SizeMax)));
            // td.Style["text-align"] = "left";
            // tr.Cells.Add(td);
			
			td = new TableCell();
            td.Text = TD_Nama_stone(
                r.Stone1A.Nama
                , (Str.Number3(r.Stone1A.Parcel102.SizeMin) + " - " + Str.Number3(r.Stone1A.Parcel102.SizeMax))
                , GIA, tglGIA);
            td.Style["text-align"] = "left";
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
            Status += "<br /><span class=\"text-sm text-silver\">" + r.GIA + "</span>";
            if (r.StatusTerpasang == 1)
                Status += "<br /><span class=\"text-sm text-silver\">Terpasang</span>";
            else
                Status += "<br /><span class=\"text-sm text-silver\">Tidak Terpasang</span>";

            td = new TableCell();
            td.Text = TD_Nama_stone(
                r.Stone1B.Nama
                , (Str.Number3(r.Stone1B.Parcel102.SizeMin) + " - " + Str.Number3(r.Stone1B.Parcel102.SizeMax))
                , r.GIA, Convert.ToInt32(r.StatusTerpasang), (r.TglGIA.HasValue ? Str.Tgl(r.TglGIA) : ""));
            td.Style["text-align"] = "left";
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
            
            if (r.StatusTerpasang == 1)
            {
                ST_Butir += r.TotalButir;
                ST_Harga += r.HargaTotal.Value;
                ST_HargaM += r.HargaTotalM.Value;
            }
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
            td.Text = TD_Nama_stone(r.Stone2.Nama, (Str.Number3(r.Stone2.Parcel203.SizeMin) + " - " + Str.Number3(r.Stone2.Parcel203.SizeMax)));
            td.Style["text-align"] = "left";
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
            td.Text = TD_Nama_stone(
                r.Stone3.Nama,
                (Str.Number3(r.Stone3.Parcel303.SizeMin) + " - " + Str.Number3(r.Stone3.Parcel303.SizeMax))
                , r.GIA, 1, (r.TglGIA.HasValue ? Str.Tgl(r.TglGIA) : ""));
            td.Style["text-align"] = "left";
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
            td.Text = TD_Nama_stone(r.Stone4.Nama, (Str.Number3(r.Stone4.Parcel403.SizeMin) + " - " + Str.Number3(r.Stone4.Parcel403.SizeMax)));
            td.Style["text-align"] = "left";
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
            td.Text = TD_Nama_stone(r.Stone5.Nama, r.Stone5.Parcel506.NamaKode);
            td.Style["text-align"] = "left";
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
        td.ColumnSpan = 4;
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(TotalButir);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "-";
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

    //Fix
    protected void fix_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval != null && p.Nomor != null);
        if (d == null) App.DataNull(this);
        else
        {
            DateTime dtRec = d.Tgl;
            DateTime dtNow = System.DateTime.Now;

            // hanya bisa dijalankan di tgl dan hari yang sama
            if (dtRec.AddDays(7) >= dtNow)
            {

                int Rcv_ID = d.ID;
                string Rcv_NO = d.Nomor;
                int Prd_ID = 0;
                string Prd_ProtoNomor = "";

                update_T_brj(Rcv_ID, Rcv_NO);

                var tbrj = cs.T_brj.SingleOrDefault(p => p.IDReceive == Rcv_ID && p.NoReceive == Rcv_NO);
                var tprd = db.StockProductDJ.Where(p => p.Nomor == Rcv_NO);

                if (tprd.Count() == 1)
                {
                    // jika tepat 1 data product
                    Prd_ID = tprd.First().ID;
                    Prd_ProtoNomor = tprd.First().ProtoNomor;

                    if (Prd_ID > 0)
                    {
                        delete_StockActualDJ(Prd_ID, Rcv_NO);
                        delete_StockLedgerDJ(Prd_ID);
                        delete_StockProductDJ(Prd_ID, Rcv_NO);

                        if(Prd_ProtoNomor != "")
                        {
                            update_ProductOrder(Prd_ProtoNomor);
                        }

                    }
                    
                }
                else if (tprd.Count() > 1)
                {
                    // jika lebih dari 1 data product
                    foreach (var x in tprd)
                    {
                        Prd_ID = x.ID;
                        Prd_ProtoNomor = x.ProtoNomor;

                        if (Prd_ID > 0)
                        {
                            delete_StockActualDJ(Prd_ID, Rcv_NO);
                            delete_StockLedgerDJ(Prd_ID);
                            delete_StockProductDJ(Prd_ID, Rcv_NO);

                            if (Prd_ProtoNomor != "")
                            {
                                update_ProductOrder(Prd_ProtoNomor);
                            }

                        }
                    }

                }

                update_T_brj(Rcv_ID, Rcv_NO);

                d.Nomor = null;
                d.Approval = null;
                d.ApprovalTgl = null;

                db.SubmitChanges();

            }

            Response.Redirect("DJ_Form.aspx?id=" + myID);
        }
    }

    protected void update_ProductOrder(string Prd_ProtoNomor)
    {
        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.KodeKaretDetail == Prd_ProtoNomor);
        if (s != null)
        {
            var x = s.ProductOrder;

            if (x != null)
                x.StatusPenerimaan = 0;
        }
    }
    
    protected void update_T_brj(int Receive_ID, string Receive_NO)
    {
        var d = cs.T_brj.SingleOrDefault(p => p.NoReceive == Receive_NO && p.IDReceive == Receive_ID);

        if (d != null)
        {
            d.NoReceive = null;
            cs.SubmitChanges();
        }
    }

    protected void delete_StockActualDJ(int Prod_ID, string Prod_NO)
    {
        var d = db.StockActualDJ.SingleOrDefault(p => p.Nomor == Prod_NO && p.IDProduct == Prod_ID);

        if (d != null)
        {
            db.StockActualDJ.DeleteOnSubmit(d);
            db.SubmitChanges();
        }

    }

    protected void delete_StockLedgerDJ(int Prod_ID)
    {
        var d = db.StockLedgerDJ.Where(p => p.IDProduct == Prod_ID);

        if (d != null)
        {
            db.StockLedgerDJ.DeleteAllOnSubmit(d);
            db.SubmitChanges();
        }

    }

    protected void delete_StockProductDJ(int Prod_ID, string Prod_NO)
    {
        var d = db.StockProductDJ.Single(p => p.ID == Prod_ID && p.Nomor == Prod_NO);

        if (d != null)
        {
            var chd = d.StockProductDJ_CharDesign;
            var chp = d.StockProductDJ_CharProduct;
            var cos = d.StockProductDJ_Costing;
            var cop = d.StockProductDJ_CostingProduct;

            var fin = d.StockProductDJ_Finishing;
            var prb = d.StockProductDJ_PricingBiaya;
            var pmu = d.StockProductDJ_PricingMU;

            var prp = d.StockProductDJ_PricingProduct;
            var prs = d.StockProductDJ_PricingSegmen;

            var s1a = d.StockProductDJ_Stone1A;
            var s1b = d.StockProductDJ_Stone1B;
            var s2 = d.StockProductDJ_Stone2;
            var s3 = d.StockProductDJ_Stone3;
            var s4 = d.StockProductDJ_Stone4;
            var s5 = d.StockProductDJ_Stone5;

            db.StockProductDJ_Stone1A.DeleteAllOnSubmit(s1a);
            db.StockProductDJ_Stone1B.DeleteAllOnSubmit(s1b);
            db.StockProductDJ_Stone2.DeleteAllOnSubmit(s2);
            db.StockProductDJ_Stone3.DeleteAllOnSubmit(s3);
            db.StockProductDJ_Stone4.DeleteAllOnSubmit(s4);
            db.StockProductDJ_Stone5.DeleteAllOnSubmit(s5);

            db.StockProductDJ_Finishing.DeleteAllOnSubmit(fin);

            db.StockProductDJ_CharDesign.DeleteOnSubmit(chd);
            db.StockProductDJ_CharProduct.DeleteOnSubmit(chp);
            db.StockProductDJ_Costing.DeleteOnSubmit(cos);
            db.StockProductDJ_CostingProduct.DeleteOnSubmit(cop);
            db.StockProductDJ_PricingBiaya.DeleteOnSubmit(prb);
            db.StockProductDJ_PricingMU.DeleteOnSubmit(pmu);
            db.StockProductDJ_PricingProduct.DeleteOnSubmit(prp);
            db.StockProductDJ_PricingSegmen.DeleteOnSubmit(prs);

            db.SubmitChanges();
        }

    }

    //Print
    protected void print_Click(object sender, EventArgs e)
    {
        Response.Redirect("DJ_PricingPrint.aspx?id=" + myID);
    }

    //Save
    protected void approve_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Status
            if (d.ProtoNomor != null)
            {
                var s = db.KodeKaret_Detail.SingleOrDefault(p => p.KodeKaretDetail == "" + d.ProtoNomor + "");
                if (s != null)
                {
                    var x = s.ProductOrder;
                    //var x = db.ProductOrder.SingleOrDefault(p => p.ID == s.IDProductOrder);
                    if (x != null)
                        x.StatusPenerimaan = 1;

                    var kd = s.KodeKaretDJ;
                    //var kd = db.KodeKaretDJ.SingleOrDefault(p => p.ID == s.IDKodeKaret);
                    if (kd != null)
                    {
                        //kd.IDBrand = d.LocWarehouse.IDBrand;
                        //kd.IDDesigner = d.HumanDesigner;
                        kd.IDSegmen = d.StockReceiveDJ_PricingSegmen.ProductSegmen;
                        kd.Keterangan = d.Keterangan;
                        kd.KodeKaretDJ_CharDesign.DesignCategory = d.StockReceiveDJ_CharDesign.DesignCategory;
                        kd.KodeKaretDJ_CharDesign.DesignConcept = d.StockReceiveDJ_CharDesign.DesignConcept;
                        kd.KodeKaretDJ_CharDesign.DesignProcess = d.StockReceiveDJ_CharDesign.DesignProcess;
                        kd.KodeKaretDJ_CharDesign.TargetAge = d.StockReceiveDJ_CharDesign.TargetAge;
                        kd.KodeKaretDJ_CharDesign.TargetGender = d.StockReceiveDJ_CharDesign.TargetGender;
                        kd.KodeKaretDJ_CharDesign.Naming = d.StockReceiveDJ_CharDesign.Naming;
                        kd.KodeKaretDJ_CharDesign.SetItem = d.StockReceiveDJ_CharDesign.SetItem;
                        kd.KodeKaretDJ_CharDesign.Special = d.StockReceiveDJ_CharDesign.Special;
                        kd.KodeKaretDJ_CharDesign.Grafir = d.StockReceiveDJ_CharDesign.Grafir;
                        kd.KodeKaretDJ_CharProduct.ProductItem = d.StockReceiveDJ_CharProduct.ProductItem;
                        kd.KodeKaretDJ_CharProduct.ProductCategory = d.StockReceiveDJ_CharProduct.ProductCategory;
                        kd.KodeKaretDJ_CharProduct.ProductLevel = d.StockReceiveDJ_CharProduct.ProductLevel;
                        kd.KodeKaretDJ_CharProduct.StoneDist = d.StockReceiveDJ_CharProduct.StoneDist;
                        kd.KodeKaretDJ_CharProduct.FrameMaterial = d.StockReceiveDJ_CharProduct.FrameMaterial;
                        kd.KodeKaretDJ_CharProduct.FrameFinishing = d.StockReceiveDJ_CharProduct.FrameFinishing;
                        kd.KodeKaretDJ_CharProduct.FrameColor = d.StockReceiveDJ_CharProduct.FrameColor;
                        kd.KodeKaretDJ_CharProduct.ProcessCons = d.StockReceiveDJ_CharProduct.ProcessCons;
                        kd.KodeKaretDJ_CharProduct.GrossWeight = d.StockReceiveDJ_CharProduct.GrossWeight;
                        kd.KodeKaretDJ_CharProduct.StoneCarat = d.StockReceiveDJ_CharProduct.StoneCarat;
                        kd.KodeKaretDJ_CharProduct.StoneWeight = d.StockReceiveDJ_CharProduct.StoneWeight;
                        kd.KodeKaretDJ_CharProduct.NettoWeight = d.StockReceiveDJ_CharProduct.NettoWeight;
                        kd.KodeKaretDJ_CharProduct.KadarLogam = d.StockReceiveDJ_CharProduct.KadarLogam;
                        kd.KodeKaretDJ_CharProduct.KadarTukaran = d.StockReceiveDJ_CharProduct.KadarTukaran;
                        kd.KodeKaretDJ_CharProduct.DimensiD = d.StockReceiveDJ_CharProduct.DimensiD;
                        kd.KodeKaretDJ_CharProduct.DimensiP = d.StockReceiveDJ_CharProduct.DimensiP;
                        kd.KodeKaretDJ_CharProduct.DimensiL = d.StockReceiveDJ_CharProduct.DimensiL;
                        kd.KodeKaretDJ_CharProduct.DimensiR = d.StockReceiveDJ_CharProduct.DimensiR;

                        db.SubmitChanges();

                        updateFinishing(s.IDKodeKaret, d);
                        updateStone(s.IDKodeKaret, d);

                        

                    }

                }
            }
            if (d.Sumber == 3 && d.IDMounting != null)
            {
                var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting && p.StatusReceive == 0 && p.Draft == false);
                if (mt != null)
                {
                    mt.StatusReceive = 1;
                    var mtp = db.StockProductMounting.SingleOrDefault(p => p.Nomor == mt.Nomor && p.StatusReceive == false);
                    if (mtp != null)
                        mtp.StatusReceive = true;
                    db.SubmitChanges();
                }

                

            }
            

            //User-Info
            // d.Nomor = Generate_Nomor(d);
            // d.Approval = App.UserNama;
            // d.ApprovalTgl = DateTime.Now;

            //Commit
            db.CommandTimeout = 5 * 60;
            db.SubmitChanges();

            StockProductDJ prod = new StockProductDJ();
            prod.IDWarehouse = d.IDWarehouse;
            prod.Sumber = d.Sumber;
            prod.Keterangan = d.Keterangan;
            prod.Tgl = d.Tgl;
            prod.Nomor = d.Nomor;
            prod.Draft = d.Draft;
            prod.DraftDate = d.DraftDate;
            prod.Proto = d.Proto;
            prod.ProtoNomor = d.ProtoNomor;
            prod.ProductOrder = d.ProductOrder;
            prod.ProductOrderNomor = d.ProductOrderNomor;
            prod.Supplier = d.Supplier;
            prod.SupplierNama = d.SupplierNama;
            prod.SupplierNomor = d.SupplierNomor;
            prod.RequestOutlet = d.RequestOutlet;
            prod.RequestOutletNama = d.RequestOutletNama;
            prod.RequestCustomer = d.RequestCustomer;
            prod.RequestCustomerNama = d.RequestCustomerNama;
            prod.HumanDesigner = d.HumanDesigner;
            prod.HumanDesginerNama = d.HumanDesignerNama;
            prod.HumanStock = d.HumanStock;
            prod.HumanStockNama = d.HumanStockNama;
            prod.ExpiredProduct = d.ExpiredProduct;
            prod.ExpiredRequest = d.ExpiredRequest;
            prod.Operator = d.Operator;
            prod.OperatorTgl = d.OperatorTgl;
            prod.Approval = d.Approval;
            prod.ApprovalTgl = d.ApprovalTgl;
            prod.CatatanManager = d.CatatanManager;
            prod.ImgPicture = d.ImgPicture;
            prod.ReturnProduct = false;
            prod.StatusPenjualan = 0;
            prod.StatusLebur = false;

            db.StockProductDJ.InsertOnSubmit(prod);
            db.SubmitChanges();

            //int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM StockProductDJ ORDER BY ID DESC",Db.Cnn_DB);

            int ProdID = prod.ID;

            StockProductDJ_CharDesign dsg = new StockProductDJ_CharDesign();
            dsg.ID = ProdID;
            dsg.DesignCategory = d.StockReceiveDJ_CharDesign.DesignCategory;
            dsg.DesignConcept = d.StockReceiveDJ_CharDesign.DesignConcept;
            dsg.DesignProcess = d.StockReceiveDJ_CharDesign.DesignProcess;
            dsg.TargetAge = d.StockReceiveDJ_CharDesign.TargetAge;
            dsg.TargetGender = d.StockReceiveDJ_CharDesign.TargetGender;
            dsg.Naming = d.StockReceiveDJ_CharDesign.Naming;
            dsg.SetItem = d.StockReceiveDJ_CharDesign.SetItem;
            dsg.Special = d.StockReceiveDJ_CharDesign.Special;
            dsg.Grafir = d.StockReceiveDJ_CharDesign.Grafir;

            db.StockProductDJ_CharDesign.InsertOnSubmit(dsg);
            db.SubmitChanges();

            StockProductDJ_CharProduct pd = new StockProductDJ_CharProduct();
            pd.ID = ProdID;
            pd.ProductItem = d.StockReceiveDJ_CharProduct.ProductItem;
            pd.ProductCategory = d.StockReceiveDJ_CharProduct.ProductCategory;
            pd.ProductLevel = d.StockReceiveDJ_CharProduct.ProductLevel;
            pd.StoneDist = d.StockReceiveDJ_CharProduct.StoneDist;
            pd.FrameColor = d.StockReceiveDJ_CharProduct.FrameColor;
            pd.FrameFinishing = d.StockReceiveDJ_CharProduct.FrameFinishing;
            pd.FrameMaterial = d.StockReceiveDJ_CharProduct.FrameMaterial;
            pd.ProcessCons = d.StockReceiveDJ_CharProduct.ProcessCons;
            pd.GrossWeight = d.StockReceiveDJ_CharProduct.GrossWeight;
            pd.StoneCarat = d.StockReceiveDJ_CharProduct.StoneCarat;
            pd.StoneWeight = d.StockReceiveDJ_CharProduct.StoneWeight;
            pd.NettoWeight = d.StockReceiveDJ_CharProduct.NettoWeight;
            pd.KadarLogam = d.StockReceiveDJ_CharProduct.KadarLogam;
            pd.KadarTukaran = d.StockReceiveDJ_CharProduct.KadarTukaran;
            pd.DimensiD = d.StockReceiveDJ_CharProduct.DimensiD;
            pd.DimensiL = d.StockReceiveDJ_CharProduct.DimensiL;
            pd.DimensiP = d.StockReceiveDJ_CharProduct.DimensiP;
            pd.DimensiR = d.StockReceiveDJ_CharProduct.DimensiR;
            pd.StoneQty = d.StockReceiveDJ_CharProduct.StoneQty;

            db.StockProductDJ_CharProduct.InsertOnSubmit(pd);
            db.SubmitChanges();

            StockProductDJ_Costing cost = new StockProductDJ_Costing();
            cost.ID = ProdID;
            cost.UnitPrice = d.StockReceiveDJ_Costing.UnitPrice;
            cost.Frame = d.StockReceiveDJ_Costing.Frame;
            cost.MountingNO = d.StockReceiveDJ_Costing.MountingNO;
            cost.MountingRO = d.StockReceiveDJ_Costing.MountingRO.Value;
            cost.Pembagi = d.StockReceiveDJ_Costing.Pembagi.Value;
            cost.Setting = d.StockReceiveDJ_Costing.Setting;
            cost.Finishing = d.StockReceiveDJ_Costing.Finishing;
            cost.OngkosLainCOGS = d.StockReceiveDJ_Costing.OngkosLainCOGS;
            cost.OngkosLain = d.StockReceiveDJ_Costing.OngkosLain;
            cost.TotalRate = d.StockReceiveDJ_Costing.TotalRate.Value;
            cost.TotalRupiah = d.StockReceiveDJ_Costing.TotalRupiah.Value;
            cost.TotalBiaya = d.StockReceiveDJ_Costing.TotalBiaya;

            db.StockProductDJ_Costing.InsertOnSubmit(cost);
            db.SubmitChanges();

            StockProductDJ_CostingProduct cp = new StockProductDJ_CostingProduct();
            cp.ID = ProdID;
            cp.GoldRate = d.StockReceiveDJ_CostingProduct.GoldRate;
            cp.Netto = d.StockReceiveDJ_CostingProduct.Netto;
            cp.Kadar = d.StockReceiveDJ_CostingProduct.Kadar;
            cp.TotalProduct = d.StockReceiveDJ_CostingProduct.TotalProduct;
            cp.TotalStone1A = d.StockReceiveDJ_CostingProduct.TotalStone1A;
            cp.TotalStone1B = d.StockReceiveDJ_CostingProduct.TotalStone1B;
            cp.TotalStone2 = d.StockReceiveDJ_CostingProduct.TotalStone2;
            cp.TotalStone3 = d.StockReceiveDJ_CostingProduct.TotalStone3;
            cp.TotalStone4 = d.StockReceiveDJ_CostingProduct.TotalStone4;
            cp.TotalStone5 = d.StockReceiveDJ_CostingProduct.TotalStone5;
            cp.Total = d.StockReceiveDJ_CostingProduct.Total;
            cp.EfektifGold = d.StockReceiveDJ_CostingProduct.EfektifGold;

            db.StockProductDJ_CostingProduct.InsertOnSubmit(cp);
            db.SubmitChanges();

            var l_fns = from p in db.StockReceiveDJ_Finishing
                        where p.IDForm == d.ID
                        select p;
            if (l_fns != null)
            {
                foreach (var r in l_fns)
                {
                    StockProductDJ_Finishing fns = new StockProductDJ_Finishing();
                    fns.IDForm = ProdID;
                    fns.IDFinishing = r.IDFinishing;
                    fns.Biaya = r.Biaya;
					fns.BiayaModal = r.BiayaModal;
                    fns.Efektif = r.Efektif;
                    db.StockProductDJ_Finishing.InsertOnSubmit(fns);
                    db.SubmitChanges();
                }
            }

            StockProductDJ_PricingBiaya by = new StockProductDJ_PricingBiaya();
            by.ID = ProdID;
            by.NamaBrand = d.StockReceiveDJ_PricingBiaya.NamaBrand;
            by.NamaItem = d.StockReceiveDJ_PricingBiaya.NamaItem;
            by.NamaCons = d.StockReceiveDJ_PricingBiaya.NamaCons;
            by.Mounting = d.StockReceiveDJ_PricingBiaya.Mounting;
            by.MountingNO = d.StockReceiveDJ_PricingBiaya.MountingNO;
            by.MountingRO = d.StockReceiveDJ_PricingBiaya.MountingRO;
            by.Setting = d.StockReceiveDJ_PricingBiaya.Setting;
            by.Finishing = d.StockReceiveDJ_PricingBiaya.Finishing;
            by.TotalRupiah = d.StockReceiveDJ_PricingBiaya.TotalRupiah;
            by.TotalRate = d.StockReceiveDJ_PricingBiaya.TotalRate;
            by.Total = d.StockReceiveDJ_PricingBiaya.Total;
            by.EfektifRate = d.StockReceiveDJ_PricingBiaya.EfektifRate;
            by.EfektifMounting = d.StockReceiveDJ_PricingBiaya.EfektifMounting;
            by.EfektifSetting = d.StockReceiveDJ_PricingBiaya.EfektifSetting;

            db.StockProductDJ_PricingBiaya.InsertOnSubmit(by);
            db.SubmitChanges();

            StockProductDJ_PricingMU mu = new StockProductDJ_PricingMU();
            mu.ID = ProdID;
            mu.NamaCategory = d.StockReceiveDJ_PricingMU.NamaCategory;
            mu.NamaLevel = d.StockReceiveDJ_PricingMU.NamaLevel;
            mu.NamaDist = d.StockReceiveDJ_PricingMU.NamaDist;
            mu.ModalUSD = d.StockReceiveDJ_PricingMU.ModalUSD;
            mu.ModalRate = d.StockReceiveDJ_PricingMU.ModalRate;
            mu.ModalRupiah = d.StockReceiveDJ_PricingMU.ModalRupiah;
            mu.MU = d.StockReceiveDJ_PricingMU.MU;
            mu.HargaUSD = d.StockReceiveDJ_PricingMU.HargaUSD;
            mu.HargaRupiah = d.StockReceiveDJ_PricingMU.HargaRupiah;
            mu.HargaRupiahRound = d.StockReceiveDJ_PricingMU.HargaRupiahRound;
            mu.EfektifRate = d.StockReceiveDJ_PricingMU.EfektifRate;

            db.StockProductDJ_PricingMU.InsertOnSubmit(mu);
            db.SubmitChanges();

            StockProductDJ_PricingProduct prc = new StockProductDJ_PricingProduct();
            prc.ID = ProdID;
            prc.GoldRate = d.StockReceiveDJ_PricingProduct.GoldRate;
            prc.Netto = d.StockReceiveDJ_PricingProduct.Netto;
            prc.Kadar = d.StockReceiveDJ_PricingProduct.Kadar;
            prc.TotalProduct = d.StockReceiveDJ_PricingProduct.TotalProduct;
            prc.TotalStone1A = d.StockReceiveDJ_PricingProduct.TotalStone1A;
            prc.TotalStone1B = d.StockReceiveDJ_PricingProduct.TotalStone1B;
            prc.TotalStone2 = d.StockReceiveDJ_PricingProduct.TotalStone2;
            prc.TotalStone3 = d.StockReceiveDJ_PricingProduct.TotalStone3;
            prc.TotalStone4 = d.StockReceiveDJ_PricingProduct.TotalStone4;
            prc.TotalStone5 = d.StockReceiveDJ_PricingProduct.TotalStone5;
            prc.Total = d.StockReceiveDJ_PricingProduct.Total;
            prc.EfektifGold = d.StockReceiveDJ_PricingProduct.EfektifGold;

            db.StockProductDJ_PricingProduct.InsertOnSubmit(prc);
            db.SubmitChanges();

            StockProductDJ_PricingSegmen sg = new StockProductDJ_PricingSegmen();
            sg.ID = ProdID;
            sg.ProductSegmen = d.StockReceiveDJ_PricingSegmen.ProductSegmen;
            sg.NamaCategory = d.StockReceiveDJ_PricingSegmen.NamaCategory;
            sg.NamaLevel = d.StockReceiveDJ_PricingSegmen.NamaLevel;
            sg.NamaDist = d.StockReceiveDJ_PricingSegmen.NamaDist;
            sg.SegmenSistem = d.StockReceiveDJ_PricingSegmen.SegmenSistem;
            sg.SegmenMinimum = d.StockReceiveDJ_PricingSegmen.SegmenMinimum;
            sg.TotalCarat = d.StockReceiveDJ_PricingSegmen.TotalCarat;
            sg.HargaUSD = d.StockReceiveDJ_PricingSegmen.HargaUSD;
            sg.EfektifSegmen = d.StockReceiveDJ_PricingSegmen.EfektifSegmen;

            db.StockProductDJ_PricingSegmen.InsertOnSubmit(sg);
            db.SubmitChanges();

            // Ledger 
            StockLedgerDJ l = new StockLedgerDJ();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = 1; // 1 : in , -1 : out
            int Remark = 4; // See Library DB - Ledger
            string Keterangan = d.Nomor; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

            //Input
            l.IDProduct = ProdID;
            l.TipeLokasi = 0;
            l.IDLokasi = d.IDWarehouse;
            l.NamaLokasi = d.LocWarehouse.Nama;
            l.Value = Value;
            l.Remark = Remark;
            l.Keterangan = Keterangan;

            //User-Info
            l.Operator = d.Approval;
            l.OperatorTgl = (DateTime) d.ApprovalTgl;

            //Commit
            db.StockLedgerDJ.InsertOnSubmit(l);
            db.SubmitChanges();

            var s1A = from p in db.StockReceiveDJ_Stone1A
                        where p.IDForm == d.ID
                        select p;
            if (s1A != null)
            {
                foreach (var r in s1A)
                {
                    StockProductDJ_Stone1A stone = new StockProductDJ_Stone1A();
                    stone.IDForm = ProdID;
                    stone.IDStone = r.IDStone;
                    stone.TotalButir = r.TotalButir;
                    stone.TotalCarat = r.TotalCarat;
                    stone.CaratButir = r.CaratButir;
                    stone.DimensiL = r.DimensiL;
                    stone.DimensiP = r.DimensiP;
                    stone.DimensiT = r.DimensiT;
                    stone.HargaSatuan = r.HargaSatuan;
                    stone.HargaTotal = r.HargaTotal;
                    stone.HargaSatuanM = r.HargaSatuanM;
                    stone.HargaTotalM = r.HargaTotalM;
                    stone.Efektif = r.Efektif;
                    stone.Setting = r.Setting;
                    db.StockProductDJ_Stone1A.InsertOnSubmit(stone);
                    db.SubmitChanges();

                    int IDNew = Db.SingleInteger("SELECT TOP 1 ID FROM StockProductDJ_Stone1A WHERE IDForm = '"+ ProdID.ToString() + "' AND IDStone = '"+ r.IDStone.ToString() +"' ORDER BY ID DESC ", Db.Cnn_DB);
                    string CertificateNumber = Db.SingleString("SELECT TOP 1 ISNULL(CertificateNumber,'') FROM StockReceiveDJ_Stone1A WHERE ID = '"+ r.ID +"' ", Db.Cnn_DB);
                    Db.Execute("UPDATE StockProductDJ_Stone1A SET CertificateNumber =  '"+ CertificateNumber +"' WHERE IDForm = '"+ ProdID.ToString() +"' AND IDStone = '"+ r.IDStone.ToString() +"' AND ID = '"+IDNew.ToString()+"'", Db.Cnn_DB);
                }
            }

            var s1B = from p in db.StockReceiveDJ_Stone1B
                      where p.IDForm == d.ID
                      select p;
            if (s1B != null)
            {
                foreach (var r in s1B)
                {
                    StockProductDJ_Stone1B stone = new StockProductDJ_Stone1B();
                    stone.IDForm = ProdID;
                    stone.IDStone = r.IDStone;
                    stone.TotalButir = r.TotalButir;
                    stone.TotalCarat = r.TotalCarat;
                    stone.CaratButir = r.CaratButir;
                    stone.DimensiL = r.DimensiL;
                    stone.DimensiP = r.DimensiP;
                    stone.DimensiT = r.DimensiT;
                    stone.HargaSatuan = r.HargaSatuan;
                    stone.HargaTotal = r.HargaTotal;
                    stone.GIA = r.GIA;
                    stone.StatusTerpasang = r.StatusTerpasang;
                    stone.HargaInputH = r.HargaInputH;
                    stone.HargaInputP = r.HargaInputP;
                    stone.HargaSatuanM = r.HargaSatuanM;
                    stone.HargaTotalM = r.HargaTotalM;
                    stone.Efektif = r.Efektif;
                    stone.Setting = r.Setting;
                    stone.HargaMInputH = r.HargaMInputH;
                    stone.HargaMInputP = r.HargaMInputP;
                    db.StockProductDJ_Stone1B.InsertOnSubmit(stone);
                    db.SubmitChanges();
                }
            }

            var s2 = from p in db.StockReceiveDJ_Stone2
                      where p.IDForm == d.ID
                      select p;
            if (s2 != null)
            {
                foreach (var r in s2)
                {
                    StockProductDJ_Stone2 stone = new StockProductDJ_Stone2();
                    stone.IDForm = ProdID;
                    stone.IDStone = r.IDStone;
                    stone.TotalButir = r.TotalButir;
                    stone.TotalCarat = r.TotalCarat;
                    stone.CaratButir = r.CaratButir;
                    stone.DimensiL = r.DimensiL;
                    stone.DimensiP = r.DimensiP;
                    stone.DimensiT = r.DimensiT;
                    stone.HargaSatuan = r.HargaSatuan;
                    stone.HargaTotal = r.HargaTotal;
                    stone.HargaSatuanM = r.HargaSatuanM;
                    stone.HargaTotalM = r.HargaTotalM;
                    stone.Efektif = r.Efektif;
                    stone.Setting = r.Setting;
                    //stone.GIA = r.GIA;
                    db.StockProductDJ_Stone2.InsertOnSubmit(stone);
                    db.SubmitChanges();
                }
            }

            var s3 = from p in db.StockReceiveDJ_Stone3
                     where p.IDForm == d.ID
                     select p;
            if (s3 != null)
            {
                foreach (var r in s3)
                {
                    StockProductDJ_Stone3 stone = new StockProductDJ_Stone3();
                    stone.IDForm = ProdID;
                    stone.IDStone = r.IDStone;
                    stone.TotalButir = r.TotalButir;
                    stone.TotalCarat = r.TotalCarat;
                    stone.CaratButir = r.CaratButir;
                    stone.DimensiL = r.DimensiL;
                    stone.DimensiP = r.DimensiP;
                    stone.DimensiT = r.DimensiT;
                    stone.HargaSatuan = r.HargaSatuan;
                    stone.HargaTotal = r.HargaTotal;
                    stone.HargaSatuanM = r.HargaSatuanM;
                    stone.HargaTotalM = r.HargaTotalM;
                    stone.Efektif = r.Efektif;
                    stone.Setting = r.Setting;
                    stone.GIA = r.GIA;
                    db.StockProductDJ_Stone3.InsertOnSubmit(stone);
                    db.SubmitChanges();
                }
            }

            var s4 = from p in db.StockReceiveDJ_Stone4
                     where p.IDForm == d.ID
                     select p;
            if (s4 != null)
            {
                foreach (var r in s4)
                {
                    StockProductDJ_Stone4 stone = new StockProductDJ_Stone4();
                    stone.IDForm = ProdID;
                    stone.IDStone = r.IDStone;
                    stone.TotalButir = r.TotalButir;
                    stone.TotalCarat = r.TotalCarat;
                    stone.CaratButir = r.CaratButir;
                    stone.DimensiL = r.DimensiL;
                    stone.DimensiP = r.DimensiP;
                    stone.DimensiT = r.DimensiT;
                    stone.HargaSatuan = r.HargaSatuan;
                    stone.HargaTotal = r.HargaTotal;
                    stone.HargaSatuanM = r.HargaSatuanM;
                    stone.HargaTotalM = r.HargaTotalM;
                    stone.Efektif = r.Efektif;
                    stone.Setting = r.Setting;
                    //stone.GIA = r.GIA;
                    db.StockProductDJ_Stone4.InsertOnSubmit(stone);
                    db.SubmitChanges();
                }
            }


            var s5 = from p in db.StockReceiveDJ_Stone5
                     where p.IDForm == d.ID
                     select p;
            if (s5 != null)
            {
                foreach (var r in s5)
                {
                    StockProductDJ_Stone5 stone = new StockProductDJ_Stone5();
                    stone.IDForm = ProdID;
                    stone.IDStone = r.IDStone;
                    stone.TotalButir = r.TotalButir;
                    stone.TotalCarat = r.TotalCarat;
                    stone.CaratButir = r.CaratButir;
                    stone.DimensiL = r.DimensiL;
                    stone.DimensiP = r.DimensiP;
                    stone.DimensiT = r.DimensiT;
                    stone.HargaSatuan = r.HargaSatuan;
                    stone.HargaTotal = r.HargaTotal;
                    stone.HargaSatuanM = r.HargaSatuanM;
                    stone.HargaTotalM = r.HargaTotalM;
                    stone.Efektif = r.Efektif;
                    stone.Setting = r.Setting;
                    //stone.GIA = r.GIA;
                    db.StockProductDJ_Stone5.InsertOnSubmit(stone);
                    db.SubmitChanges();
                }
            }

            StockActualDJ stock = new StockActualDJ();
            stock.IDProduct = ProdID;
            stock.NamaLokasi = d.LocWarehouse.Nama;
            stock.IDBrand = d.LocWarehouse.CompanyBrand.ID;
            stock.TipeLokasi = 0;
            stock.IDLokasi = d.IDWarehouse;
            stock.Substorage = 1;
            stock.Keterangan = d.Keterangan;
            stock.Tgl = d.Tgl;
            stock.Nomor = d.Nomor;
            stock.InHand = 1;
            stock.Operator = d.Operator;
            stock.OperatorTgl = d.OperatorTgl;
            db.StockActualDJ.InsertOnSubmit(stock);
            db.SubmitChanges();

            try
            {
                //add nomor to t_brj
                string NoRec = d.Nomor;
                var brj_item = cs.T_brj.Single(p => p.IDReceive == myID);
                brj_item.NoReceive = NoRec;
                cs.SubmitChanges();
            }
            catch (Exception ex) { }

            //Redirect
            App.ReloadClose(this);
        }
    }

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

    protected void updateFinishing(int IDKodeKaret, StockReceiveDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Finishing WHERE IDForm = " + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockReceiveDJ_Finishing;

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

    protected void updateStone(int IDKodeKaret, StockReceiveDJ d)
    {
        updateStone1A(IDKodeKaret, d);
        updateStone1B(IDKodeKaret, d);
        updateStone2(IDKodeKaret, d);
        updateStone3(IDKodeKaret, d);
        updateStone4(IDKodeKaret, d);
        updateStone5(IDKodeKaret, d);
    }
    protected void updateStone1A(int IDKodeKaret, StockReceiveDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone1A WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockReceiveDJ_Stone1A;
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
				
				int IDNew = Db.SingleInteger("SELECT TOP 1 ID FROM KodeKaretDJ_Stone1A WHERE IDForm = '"+ IDKodeKaret.ToString() + "' AND IDStone = '"+ r.IDStone.ToString() +"' ORDER BY ID DESC ", Db.Cnn_DB);
                string CertificateNumber = Db.SingleString("SELECT TOP 1 ISNULL(CertificateNumber,'') FROM StockReceiveDJ_Stone1A WHERE ID = '"+ r.ID +"' ", Db.Cnn_DB);
                Db.Execute("UPDATE KodeKaretDJ_Stone1A SET CertificateNumber =  '"+ CertificateNumber +"' WHERE IDForm = '"+ IDKodeKaret.ToString() +"' AND IDStone = '"+ r.IDStone.ToString() +"' AND ID = '"+IDNew.ToString()+"'", Db.Cnn_DB);
            }
        }
    }

    protected void updateStone1B(int IDKodeKaret, StockReceiveDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone1B WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockReceiveDJ_Stone1B;
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
                stone.StatusTerpasang = r.StatusTerpasang;
                db.KodeKaretDJ_Stone1B.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }
    }
    protected void updateStone2(int IDKodeKaret, StockReceiveDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone2 WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockReceiveDJ_Stone2;
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
    protected void updateStone3(int IDKodeKaret, StockReceiveDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone3 WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockReceiveDJ_Stone3;
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
                stone.GIA = r.GIA;
                db.KodeKaretDJ_Stone3.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }
    }
    protected void updateStone4(int IDKodeKaret, StockReceiveDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone4 WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockReceiveDJ_Stone4;
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

    protected void updateStone5(int IDKodeKaret, StockReceiveDJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Stone5 WHERE IDForm=" + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockReceiveDJ_Stone5;
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

    protected string Generate_Nomor(StockReceiveDJ d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Year;
        int Bulan = d.Tgl.Month;

        /*int Nomor = db.StockReceiveDJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;

        return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        int Nomor = 1;

        try
        {
            var s = (from p in db.StockReceiveDJ
                     where p.Tgl.Year == Tahun /*&& p.Tgl.Month == Bulan*/
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(1, 8)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(3, 6)) + 1;
        }
        catch (Exception ex) { }

        string Kode = "D";
        try
        {
            Kode = App.KodeDJ;
        }
        catch (Exception ex) { }

        string Kode2 = convertCharPLU(Tahun.ToString().Remove(0, 2));

        string PLU = Kode + Kode2 + Str.KodeFormat(Nomor, 6);

        return PLU;

    }

    private string convertCharPLU(string number)
    {

        char alph1 = Convert.ToInt32(number.Substring(0, 1)) == 0 ? '0' : (char)(Convert.ToInt32(number.Substring(0, 1)) + 64);
        char alph2 = Convert.ToInt32(number.Substring(1, 1)) == 0 ? '0' : (char)(Convert.ToInt32(number.Substring(1, 1)) + 64);
        return alph1.ToString() + alph2.ToString();
    }

    //Display Column
    protected string TD_Nama_stone(string Nama, string size)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong> " + "<span class=\"text-muted text-sm\">(" + size + ")</span></p>";

        return x;
    }
    protected string TD_Nama_stone(string Nama, string size, string GIA, string tglGIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong> " + "<span class=\"text-muted text-sm\">(" + size + ")</span></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\">GIA : " + GIA + (tglGIA == "" ? "" : (" (" + tglGIA + ")")) + "</p>";

        return x;
    }
	protected string TD_Nama_stone(string Nama, string size, string GIA, int Status, string tglGIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong> " + "<span class=\"text-muted text-sm\">(" + size + ")</span></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\">GIA : " + GIA + (tglGIA == "" ? "" : (" (" + tglGIA + ")")) + "</p>";
        if (Status == 1)
            x += "<p class=\"text-muted text-sm\">Terpasang</p>";
        else
            x += "<p class=\"text-muted text-sm\">Tidak Terpasang</p>";

        return x;
    }
}