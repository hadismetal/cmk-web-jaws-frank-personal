using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Page_Register_DJ_Costing : System.Web.UI.Page
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
            unitprice.Focus(); //frame.Focus();

            //Number-Format
            App.NumberFormat(unitprice);
            //App.NumberFormat(frame);
            App.NumberFormat(mountingno);
            App.NumberFormat(mountingro);
            App.NumberFormat(setting);
            //App.NumberFormat(finishing);
            App.NumberFormat(ongkoslaincogs);
            App.NumberFormat(ongkoslain);

            //Form
            fillForm();
        }
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
        if (d == null) DataError();
        else
        {
            DLC.CharProcessFinishingModal(finishing_tr, d.ID);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJ.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveDJ_Costing.SingleOrDefault(p => p.ID == myID && p.StockReceiveDJ.Approval==null);
        if (d == null)
        {
            var ms = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + ms.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + ms.Nomor + "</a>";

            var mnt = db.PricingTableFM.SingleOrDefault(p => p.IDBrand == ms.LocWarehouse.CompanyBrand.ID && p.IDItem == ms.StockReceiveDJ_CharProduct.ProductItem && p.IDCons == ms.StockReceiveDJ_CharProduct.ProcessCons);
            pembagi.Text = Convert.ToDecimal(mnt.Pembagi).ToString("N0");
        }
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.StockReceiveDJ.Nomor + "</a>";

            if (d.StockReceiveDJ.Sumber != 2)
            {
                unitprice.Text = "0";
                unitprice_Row.Visible = false;
            }

            frame.Text = Convert.ToDecimal(d.Frame).ToString("N0");
            pembagi.Text = Convert.ToDecimal(d.Pembagi).ToString("N0");
            mountingno.Text = Convert.ToDecimal(d.MountingNO * d.Pembagi).ToString("N0");
            mountingro.Text = Convert.ToDecimal(d.MountingRO).ToString("N0");
            setting.Text = Convert.ToDecimal(d.Setting).ToString("N0");
            ongkoslaincogs.Text = Convert.ToDecimal(d.OngkosLainCOGS).ToString("N0");
            ongkoslain.Text = Convert.ToDecimal(d.OngkosLain).ToString("N0");
        }
    }

    //Save
    protected bool valid
    {
        get
        {
            bool x = true;

            //x = x && Fv.isDecimal(frame, frame_ERR);
            x = x && Fv.isDecimal(mountingno, mountingno_ERR);
            x = x && Fv.isDecimal(mountingro, mountingro_ERR);
            x = x && Fv.isInt(pembagi, pembagi_ERR);
            x = x && Fv.isDecimal(setting, setting_ERR);
            //x = x && Fv.isDecimal(finishing, finishing_ERR);

            return x;
        }
    }
    protected void save_Click(object sender, EventArgs e)
    {
        if (valid)
        {
            var d = db.StockReceiveDJ_Costing.SingleOrDefault(p => p.ID == myID && p.StockReceiveDJ.Approval==null);
            if (d == null)
            {
                var ms = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
                insertProcess(ms);
                //updateChar(d.StockReceiveDJ); //// di-disable sementara karena belum siap
                Response.Redirect("DJ_Picture.aspx?id=" + myID);
            }
            else
            {
                //Process
                var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                p.ID == CV.US_General_Costing &&
                p.Harga.HasValue &&
                p.ApprovalTgl.HasValue);

                decimal Pembagi = (Convert.ToDecimal(pembagi.Text) > 0 && (pembagi.Text != null)) ? Convert.ToDecimal(pembagi.Text) : 1;

                d.UnitPrice = Convert.ToDecimal(unitprice.Text);
                d.Frame = Convert.ToDecimal(frame.Text);
                d.MountingNO = Convert.ToDecimal(mountingno.Text) / Pembagi;
                d.MountingRO = Convert.ToDecimal(mountingro.Text);
                d.Pembagi = Convert.ToDecimal(pembagi.Text);
                d.Setting = Convert.ToDecimal(setting.Text);
                //n.Finishing = Convert.ToDecimal(finishing.Text);
                d.OngkosLainCOGS = Convert.ToDecimal(ongkoslaincogs.Text);
                d.OngkosLain = Convert.ToDecimal(ongkoslain.Text);

                //Total
                d.TotalRupiah = Convert.ToDecimal(d.UnitPrice) + d.MountingNO + d.MountingRO.Value + d.Setting + d.Finishing + Convert.ToDecimal(d.OngkosLainCOGS);
                d.TotalRate = prc_us.Harga;
                d.TotalBiaya = d.TotalRupiah.Value / d.TotalRate.Value;

                //save finishing
                d.Finishing = saveFinishing(d.StockReceiveDJ);

                //Commit
                db.SubmitChanges();

                //updateChar(d.StockReceiveDJ); //// didisable sementara karena belum siap

                //Redirect
                Response.Redirect("DJ_Picture.aspx?id=" + myID);
                //Response.Redirect("DJ_Submit.aspx?id=" + myID);
            }
        }
    }
    protected void insertProcess(StockReceiveDJ ms)
    {

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        StockReceiveDJ_Costing n = new StockReceiveDJ_Costing();

        n.ID = ms.ID;

        decimal Pembagi = (Convert.ToDecimal(pembagi.Text) > 0 && (pembagi.Text != null)) ? Convert.ToDecimal(pembagi.Text) : 1;

        n.UnitPrice = Convert.ToDecimal(unitprice.Text);
        n.Frame = Convert.ToDecimal(frame.Text);
        n.MountingNO = Convert.ToDecimal(mountingno.Text) / Pembagi;
        n.MountingRO = Convert.ToDecimal(mountingro.Text);
        n.Pembagi = Convert.ToDecimal(pembagi.Text);
        n.Setting = Convert.ToDecimal(setting.Text);
        //n.Finishing = Convert.ToDecimal(finishing.Text);
        n.OngkosLainCOGS = Convert.ToDecimal(ongkoslaincogs.Text);
        n.OngkosLain = Convert.ToDecimal(ongkoslain.Text);

        //Total
        n.TotalRupiah = Convert.ToDecimal(n.UnitPrice) + n.MountingNO + n.MountingRO.Value + n.Setting + n.Finishing + Convert.ToDecimal(n.OngkosLainCOGS);
        n.TotalRate = prc_us.Harga;
        n.TotalBiaya = n.TotalRupiah.Value / n.TotalRate.Value;

        //save finishing
        n.Finishing = saveFinishing(ms);

        //Commit
        db.StockReceiveDJ_Costing.InsertOnSubmit(n);
        db.SubmitChanges();
    }

    protected decimal saveFinishing(StockReceiveDJ ms)
    {

        decimal Finishing = 0m;

        string test = "1";
        var fns = from p in db.StockReceiveDJ_Finishing
                  where p.IDForm == ms.ID
                  select p;
        if (fns != null)
        {
            foreach (var r in fns)
            {
                decimal val = Convert.ToDecimal(((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text);

                Finishing += val;

                var x = db.StockReceiveDJ_Finishing.SingleOrDefault(p => p.ID == r.ID);
                x.BiayaModal = val;
                db.SubmitChanges();

                //test += ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text != "" ? ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text : "0";
            }
        }

        return Finishing;
    }
    protected void updateChar(StockReceiveDJ d)
    {
        decimal stone = 0m;
        int IDCategory = 1;
        int currCategory = d.StockReceiveDJ_CharProduct.ProductCategory;
        if (currCategory == 1 || currCategory == 6 || currCategory == 5)
        {
            stone = (from p in d.StockReceiveDJ_Stone1A
                     select new
                     {
                         CaratButir = p.CaratButir
                     }).Union(from p in d.StockReceiveDJ_Stone1B
                              select new
                              {
                                  CaratButir = p.CaratButir
                              }).Union(from p in d.StockReceiveDJ_Stone2
                                       select new
                                       {
                                           CaratButir = p.CaratButir
                                       }).Union(from p in d.StockReceiveDJ_Stone3
                                                select new
                                                {
                                                    CaratButir = p.CaratButir
                                                }).Union(from p in d.StockReceiveDJ_Stone4
                                                         select new
                                                         {
                                                             CaratButir = p.CaratButir
                                                         }).Union(from p in d.StockReceiveDJ_Stone5
                                                                  select new
                                                                  {
                                                                      CaratButir = p.CaratButir
                                                                  }).Max(p => p.CaratButir);
            if (stone <= 0.079m)
            {
                IDCategory = 1;
            }
            else if (stone > 0.079m && stone <= 0.899m)
            {
                IDCategory = 6;
            }
            else if (stone >= 0.9m)
            {
                IDCategory = 5;
            }
            d.StockReceiveDJ_CharProduct.ProductCategory = IDCategory;
            db.SubmitChanges();
        }
    }
}