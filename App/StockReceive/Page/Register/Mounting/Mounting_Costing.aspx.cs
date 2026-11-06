using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Page_Register_Mounting_Costing : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
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
        var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            DLC.CharProcessFinishingMountingModal(finishing_tr, d.ID);
        }
	}

	//Exception
	protected void DataError() { Response.Redirect("Mounting.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Mounting_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

            if (d.Sumber != 0)
            {
                unitprice.Text = "0";
                unitprice_Row.Visible = false;
            }

            var mnt = db.PricingTableFM.SingleOrDefault(p => p.IDBrand == d.LocWarehouse.CompanyBrand.ID && p.IDItem == d.StockReceiveMounting_CharProduct.ProductItem && p.IDCons == d.StockReceiveMounting_CharProduct.ProcessCons);
            if(mnt!=null)
                pembagi.Text = (mnt.Pembagi.HasValue) ? Convert.ToDecimal(mnt.Pembagi).ToString("N0")  : "1";
            else
                pembagi.Text = "1";
		}
	}

	//Save
	protected bool valid {
		get {
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
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				//Process
				deleteProcess(d);
				insertProcess(d);

				//Redirect
				//Response.Redirect("Mounting_Picture.aspx?id=" + myID);
                Response.Redirect("Mounting_Submit.aspx?id=" + myID);
			}
		}
	}
	protected void deleteProcess(StockReceiveMounting ms) {
		var d = ms.StockReceiveMounting_Costing;
		if (d != null) {
			//Commit
			db.StockReceiveMounting_Costing.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
	protected void insertProcess(StockReceiveMounting ms) {

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

		StockReceiveMounting_Costing n = new StockReceiveMounting_Costing();

		n.ID = ms.ID;

        decimal Pembagi = (Convert.ToDecimal(pembagi.Text) > 0 && (pembagi.Text !=null)) ? Convert.ToDecimal(pembagi.Text) : 1;

        n.UnitPrice = Convert.ToDecimal(unitprice.Text);
		//n.Frame = Convert.ToDecimal(frame.Text);
		n.MountingNO = Convert.ToDecimal(mountingno.Text) / Pembagi;
        n.MountingRO = Convert.ToDecimal(mountingro.Text);
        n.Pembagi = Convert.ToDecimal(pembagi.Text);
		n.Setting = Convert.ToDecimal(setting.Text);
		//n.Finishing = Convert.ToDecimal(finishing.Text);
        n.OngkosLainCOGS = Convert.ToDecimal(ongkoslaincogs.Text);
        n.OngkosLain = Convert.ToDecimal(ongkoslain.Text);
        n.Finishing = saveFinishing(ms);
		//Total
		n.TotalRupiah = Convert.ToDecimal(n.UnitPrice) + n.MountingNO + n.MountingRO.Value + n.Setting + n.Finishing + Convert.ToDecimal(n.OngkosLainCOGS);
        n.TotalRate = prc_us.Harga;
        n.TotalBiaya = n.TotalRupiah.Value / n.TotalRate.Value;

        //save finishing
        

		//Commit
		db.StockReceiveMounting_Costing.InsertOnSubmit(n);
		db.SubmitChanges();
	}

    protected decimal saveFinishing(StockReceiveMounting ms)
    {

        decimal Finishing = 0m;

        string test = "1";
        var fns = from p in db.StockReceiveMounting_Finishing
                  where p.IDForm == ms.ID
                  select p;
        if (fns != null)
        {
            try
            {
                foreach (var r in fns)
                {
                    decimal val = Convert.ToDecimal(((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text);

                    Finishing += val;

                    var x = db.StockReceiveMounting_Finishing.SingleOrDefault(p => p.ID == r.ID);
                    x.BiayaModal = val;
                    db.SubmitChanges();

                    //test += ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text != "" ? ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text : "0";
                }
            }
            catch (Exception ex) { }
        }

        return Finishing;
    }
}