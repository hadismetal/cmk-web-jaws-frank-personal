using System;
using System.Linq;

public partial class Page_Register_DJ_Form : System.Web.UI.Page {
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
	protected int myProductDesignGJ { get { return App.QueryInt(this, "idproductdesign"); } }
    protected int myKodeKaret { get { return App.QueryInt(this, "idkodekaret"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
            DLC.CompanyBrand(spkbrand);
			keterangan.Focus();

			//Form
            if (tipe == 1)
            {
                var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myProductDesignGJ && p.Draft == false && p.StatusSPK == false);
                if (d == null) DataError();
                noref.InnerText = d.DesignKode;
                designer.InnerText = d.DataHuman.Nama + " (" + d.DataHuman.CompanyBrand.Nama + ")";
                jlhcopy.Enabled = false;
            }
            else if (tipe == 2)
            {
                var d = db.KodeKaretGJ.SingleOrDefault(p => p.ID == myKodeKaret);
                if (d == null) DataError();
                noref.InnerText = d.Nomor;
                designer.InnerText = d.DataHuman.Nama + " (" + d.DataHuman.CompanyBrand.Nama + ")";
            }
            else
            {
                DataError();
            }
            

			//Form-Default
			tglrangka.Text = Str.Tgl(DateTime.Today);
            tglsetting.Text = Str.Tgl(DateTime.Today);
            tgldj.Text = Str.Tgl(DateTime.Today);
            tglkonsumen.Text = Str.Tgl(DateTime.Today);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx?d=2"); }

	//Save
	protected bool validRangka {
		get { return Fv.isDateTime(tglrangka, tglrangka_ERR); }
	}
    protected bool validSetting {
        get { return Fv.isDateTime(tglsetting, tglsetting_ERR); }
	}
    protected bool validDJ {
        get { return Fv.isDateTime(tgldj, tgldj_ERR); }
	}
    protected bool validKonsumen {
        get { return Fv.isDateTime(tglkonsumen, tglkonsumen_ERR); }
	}
    protected bool validJlhCopy
    {
        get { return Fv.isInt(jlhcopy, jlhcopy_ERR); }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (validRangka && validSetting && validDJ && validKonsumen && validJlhCopy) {
			ProductOrderGJ n = new ProductOrderGJ();

			//Location

			//Trans. Form
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = DateTime.Today;
            n.TanggalJatuhTempoRangka = Convert.ToDateTime(tglrangka.Text);
            n.TanggalJatuhTempoSetting = Convert.ToDateTime(tglsetting.Text);
            n.TanggalJatuhTempoDJ = Convert.ToDateTime(tgldj.Text);
            n.TanggalJatuhTempoKonsumen = Convert.ToDateTime(tglkonsumen.Text);
            n.StatusFPS = 0;
            n.StatusPenerimaan = 0;
            n.NoInvoice = noinvoice.Text;
            n.NoPO = nopo.Text;
            n.NoTitipan = notitipan.Text;
            n.JlhCopy = Convert.ToInt32(jlhcopy.Text);

            int idBrandCompany = 0;

            if (tipe == 1)
            {
                var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myProductDesignGJ && p.Draft == false && p.StatusSPK == false);
                if (d == null) DataError();

                idBrandCompany = d.DataHuman.CompanyBrand.ID;
                try { idBrandCompany = Convert.ToInt32(spkbrand.SelectedItem.Value); }
                catch (Exception ex) { }

                n.IDBrand = idBrandCompany;
                n.IDSegmen = d.ProductDesignGJ_PricingSegmen.ProductSegmen;
                n.IDDesigner = d.DataHuman.ID;
            }
            else if (tipe == 2)
            {
                var d = db.KodeKaretGJ.SingleOrDefault(p => p.ID == myKodeKaret);
                if (d == null) DataError();

                idBrandCompany = d.IDBrand.Value;
                try { idBrandCompany = Convert.ToInt32(spkbrand.SelectedItem.Value); }
                catch (Exception ex) { }

                n.IDBrand = idBrandCompany;
                n.IDSegmen = d.IDSegmen;
                n.IDDesigner = d.IDDesigner;
            }

            
            if (tipe == 1)
            {
                n.IDProductDesign = myProductDesignGJ;
            }
            else
            {
                string KodeKaret = Db.SingleString("SELECT TOP 1 Nomor FROM KodeKaretGJ WHERE ID="+myKodeKaret, Db.Cnn_DB);
                n.KodeKaret = KodeKaret;
                n.IDKodeKaret = Convert.ToInt32(myKodeKaret);
            }

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
            n.StatusOrder = (tipe == 1) ? 1 : 2;
            n.KodeKaretLama = kodekaretlama.Text;

			//User-Info
			n.Operator = App.UserNama;
			n.OperatorTgl = DateTime.Now;

			//Commit
			db.ProductOrderGJ.InsertOnSubmit(n);
			db.SubmitChanges();

			//Redirect
			Response.Redirect("SPK_Data01.aspx?id=" + n.ID + "&tipe=" + tipe);
		}
	}
}