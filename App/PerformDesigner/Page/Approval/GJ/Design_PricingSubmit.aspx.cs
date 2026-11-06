using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_Design_PricingSubmit : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            fillProduct(d);
            fillBiaya(d);
            fillFinishing();
            fillMU(d);
            fillSegmen(d);
        }
    }

    protected void fillProduct(ProductDesignGJ ms)
    {
        var d = ms.ProductDesignGJ_PricingProduct;
        if (d != null)
        {
            goldrate.InnerHtml = Str.Number2(d.GoldRate);
            netto.InnerHtml = Str.Number2(d.Netto);
            kadar.InnerHtml = Str.Number2(d.Kadar);
            totalproduct.InnerHtml = Str.Number2(d.TotalProduct);

            sum_frame.InnerHtml = Str.Number2(d.TotalProduct);
            sum_total.InnerHtml = Str.Number2(d.Total);

            efektifgold.InnerHtml = Str.Tgl(d.EfektifGold);
        }
    }
    protected void fillBiaya(ProductDesignGJ ms)
    {
        var d = ms.ProductDesignGJ_PricingBiaya;
        if (d != null)
        {
            brand1.InnerHtml = d.NamaBrand;
            item1.InnerHtml = d.NamaItem;
            cons1.InnerHtml = d.NamaCons;

            no1.InnerHtml = Str.Number0(d.MountingNO);
            ro1.InnerHtml = Str.Number0(d.MountingRO);
            total1.InnerHtml = Str.Number0(d.Mounting);

            efektifmounting.InnerHtml = Str.Tgl(d.EfektifMounting);

            fee10.InnerHtml = Str.Number0(d.Mounting);
            fee20.InnerHtml = Str.Number0(d.Setting);
            fee30.InnerHtml = Str.Number0(d.Finishing);

            fee11.InnerHtml = Str.Tgl(d.EfektifMounting);
            fee21.InnerHtml = Str.Tgl(d.EfektifSetting);

            feerupiah.InnerHtml = Str.Number0(d.TotalRupiah);
            feerate.InnerHtml = Str.Number2(d.TotalRate);
            feetotal.InnerHtml = Str.Number2(d.Total);

            efektifrate.InnerHtml = Str.Tgl(d.EfektifRate);
        }
    }
    protected void fillMU(ProductDesignGJ ms)
    {
        var d = ms.ProductDesignGJ_PricingMU;
        if (d != null)
        {
            cat1.InnerHtml = d.NamaCategory;
            lvl1.InnerHtml = d.NamaLevel;
            dist1.InnerHtml = d.NamaDist;

            modal.InnerHtml = Str.Number2(d.ModalUSD);
            modalrate.InnerHtml = Str.Number2(d.ModalRate);
            modalrupiah.InnerHtml = Str.Number2(d.ModalRupiah);

            murate.InnerHtml = Str.Tgl(d.EfektifRate);

            mu_persen.InnerHtml = Str.Number2(d.MU);
            hargaidr.InnerHtml = Str.Number2(d.HargaRupiahRound);
            hargausd.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }
    protected void fillSegmen(ProductDesignGJ ms)
    {
        var d = ms.ProductDesignGJ_PricingSegmen;
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
        var rs = from p in db.ProductDesignGJ_Finishing
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
            td.Text = Str.Number2(r.Biaya);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tbff.Rows.Add(tr);
        }
    }


    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Status


            //User-Info
            d.Nomor = Generate_Nomor(d);
            d.DesignKode = Generate_Nomor(d);
            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;

            //Commit
            db.SubmitChanges();

            //Redirect
            App.ReloadClose(this);
        }
    }

    protected string Generate_Nomor(ProductDesignGJ d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Year;
        int Bulan = d.Tgl.Month;

        /*int Nomor = db.ProductDesignGJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;*/

        int Nomor = 1;

        try
        {
            var s = (from p in db.ProductDesignGJ
                     where p.Tgl.Year == Tahun && p.Tgl.Month == Bulan
                     && p.Nomor!=null
                     orderby (p.Nomor.Substring(13, 4)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(13, 4)) + 1;
        }
        catch (Exception ex)
        {

        }

        return "DSG/GJ/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }
}