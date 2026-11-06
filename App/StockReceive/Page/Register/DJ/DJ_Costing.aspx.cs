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
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            if (d.IDMounting == null)
                DLC.CharProcessFinishingModal(finishing_tr, d.ID);
            else
                DLC.CharProcessFinishingMountingModal(finishing_tr, d.IDMounting.Value);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJ.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            var mnt = db.PricingTableFM.SingleOrDefault(p => p.IDBrand == d.LocWarehouse.CompanyBrand.ID && p.IDItem == d.StockReceiveDJ_CharProduct.ProductItem && p.IDCons == d.StockReceiveDJ_CharProduct.ProcessCons);
			if(mnt!=null)
				pembagi.Text = Convert.ToDecimal(mnt.Pembagi.HasValue ? mnt.Pembagi.Value : 1).ToString("N0");
			else
				pembagi.Text = "1";

            if (d.Sumber != 2)
            {
                unitprice.Text = "0";
                unitprice_Row.Visible = false;
            }
            if (d.Sumber == 3 && d.IDMounting != null)
            {
                var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting);
                if (mt != null)
                {
                    mountingno.Text = Str.Number0(mt.StockReceiveMounting_Costing.MountingNO);
                    mountingro.Text = Str.Number0(mt.StockReceiveMounting_Costing.MountingRO);
                    pembagi.Text = Str.Number0(mt.StockReceiveMounting_Costing.Pembagi);
                    setting.Text = Str.Number0(mt.StockReceiveMounting_Costing.Setting);
                    ongkoslaincogs.Text = Str.Number0(mt.StockReceiveMounting_Costing.OngkosLainCOGS);
                    ongkoslain.Text = Str.Number0(mt.StockReceiveMounting_Costing.OngkosLain);

                    
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
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) DataError();
            else
            {
                //Process
                deleteProcess(d);
                insertProcess(d);

                //updateChar(d); // disable sementara karena belum siap

                //Redirect
                Response.Redirect("DJ_Picture.aspx?id=" + myID);
                //Response.Redirect("DJ_Submit.aspx?id=" + myID);
            }
        }
    }
    protected void deleteProcess(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_Costing;
        if (d != null)
        {
            //Commit
            db.StockReceiveDJ_Costing.DeleteOnSubmit(d);
            db.SubmitChanges();
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
        //n.Frame = Convert.ToDecimal(frame.Text);
        n.MountingNO = Convert.ToDecimal(mountingno.Text) / Pembagi;
        n.MountingRO = Convert.ToDecimal(mountingro.Text);
        n.Pembagi = Convert.ToDecimal(pembagi.Text);
        n.Setting = Convert.ToDecimal(setting.Text);
        n.Finishing = saveFinishing(ms);
        //n.Finishing = Convert.ToDecimal(finishing.Text);
        n.OngkosLainCOGS = Convert.ToDecimal(ongkoslaincogs.Text);
        n.OngkosLain = Convert.ToDecimal(ongkoslain.Text);

        //Total
        //n.TotalRupiah = Convert.ToDecimal(n.UnitPrice) + n.MountingNO + n.MountingRO.Value + n.Setting + n.Finishing + Convert.ToDecimal(n.OngkosLainCOGS);
        n.TotalRupiah = n.MountingNO + n.MountingRO.Value + n.Setting + n.Finishing + Convert.ToDecimal(n.OngkosLainCOGS);
        n.TotalRate = prc_us.Harga;
        n.TotalBiaya = n.TotalRupiah.Value / n.TotalRate.Value;

        //save finishing
        

        //Commit
        db.StockReceiveDJ_Costing.InsertOnSubmit(n);
        db.SubmitChanges();
    }

    protected decimal saveFinishing(StockReceiveDJ ms)
    {

        decimal Finishing = 0m;

        string test = "1";

        if (ms.Sumber != 3)
        {
            var fns = from p in db.StockReceiveDJ_Finishing
                      where p.IDForm == ms.ID
                      select p;
            if (fns.Count() > 0)
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
        }
        else if (ms.Sumber == 3)
        {
            var fns = from p in db.StockReceiveMounting_Finishing
                      where p.IDForm == ms.IDMounting
                      select p;
            if (fns.Count() > 0)
            {
                foreach (var r in fns)
                {
                    decimal val = 0;

                    val = Convert.ToDecimal(((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text);

                    Finishing += val;

                    var x = db.StockReceiveMounting_Finishing.SingleOrDefault(p => p.ID == r.ID);
                    x.BiayaModal = val;
                    db.SubmitChanges();

                    //test += ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text != "" ? ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text : "0";
                }
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