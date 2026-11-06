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
        var d = db.StockReceiveGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            fillProduct(d);
            
            fillBiaya(d);
            fillFinishing();
            fillMU(d);
            fillSegmen(d);
        }
    }

    protected void fillProduct(StockReceiveGJ ms)
    {
        var d = ms.StockReceiveGJ_PricingProduct;
        var x = ms.StockReceiveGJ_CostingProduct;
        if (d != null && x != null)
        {
            goldrate.InnerHtml = Str.Number2(x.GoldRate) + " / " + Str.Number2(d.GoldRate);
            netto.InnerHtml = Str.Number2(x.Netto) + " / " + Str.Number2(d.Netto);
            kadar.InnerHtml = Str.Number2(x.Kadar) + " / " + Str.Number2(d.Kadar);
            totalproduct.InnerHtml = Str.Number2(x.TotalProduct) + " / " + Str.Number2(d.TotalProduct);

            sum_frame.InnerHtml = Str.Number2(x.TotalProduct)  + " / " + Str.Number2(d.TotalProduct);
            sum_total.InnerHtml = Str.Number2(x.Total) + " / " + Str.Number2(d.Total);

            efektifgold.InnerHtml = Str.Tgl(d.EfektifGold);
        }
    }
    protected void fillBiaya(StockReceiveGJ ms)
    {
        var d = ms.StockReceiveGJ_PricingBiaya;
        var x = ms.StockReceiveGJ_Costing;
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
    protected void fillMU(StockReceiveGJ ms)
    {
        decimal AllM = 0m;

        var d = ms.StockReceiveGJ_PricingMU;
        var x = ms.StockReceiveGJ_CostingProduct;
        var z = ms.StockReceiveGJ_Costing;

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
    protected void fillSegmen(StockReceiveGJ ms)
    {
        var d = ms.StockReceiveGJ_PricingSegmen;
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

            hargasegmen.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }

    protected void fillFinishing()
    {
        //SELECT
        var rs = from p in db.StockReceiveGJ_Finishing
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

    protected void updateFinishing(int IDKodeKaret, StockReceiveGJ d)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Finishing WHERE IDForm = " + IDKodeKaret);
        db.SubmitChanges();

        var rs = d.StockReceiveGJ_Finishing;

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
}