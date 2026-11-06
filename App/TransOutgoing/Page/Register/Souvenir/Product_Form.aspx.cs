using System;
using System.Linq;

public partial class Product_Form : System.Web.UI.Page
{
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
	        return _db;
	    }
	}

    private Map_DB _d;

    private Map_DB d
    {
        get
        {
            if (_d == null)
                _d = new Map_DB();
            return _d;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (d != null)
	        d.Dispose();
	    base.Dispose();
	}

    protected int brAsal { get { return App.QueryInt(this, "brAsal"); } } /// id brand
    protected int vB { get { return App.QueryInt(this, "idb"); } } /// id brand
    protected int vF { get { return App.QueryInt(this, "idf"); } } /// id from
    protected int vT { get { return App.QueryInt(this, "idt"); } } /// id to
    protected int tf { get { return App.QueryInt(this, "tf"); } } /// type from
    protected int tt { get { return App.QueryInt(this, "tt"); } } /// type to
    
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			namakurir.Focus();

            Fill();

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tglETA.Text = Str.Tgl(DateTime.Today);
		}
	}
    
    
    protected void Fill()
    {

        //var rsB = d.CompanyBrand.SingleOrDefault(p => p.ID == vB);
        //if (rsB == null) App.DataNull(this);

        lit_type_from.Text = DL.LocationType(tf);
        lit_type_to.Text = DL.LocationType(tt);
        brand.InnerText = Db.SingleString("select top 1 Nama from CompanyBrand a where ID="+brAsal, Db.Cnn_DB);

        if (tf == 0 && tt == 0) /// WW
        {
            var rsF = d.LocWarehouse.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocWarehouse.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT && p.ID != vF);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }
        else if (tf == 0 && tt == 1) /// WO
        {
            var rsF = d.LocWarehouse.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocOutlet.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }
        else if (tf == 0 && tt == 2) /// WE
        {
            var rsF = d.LocWarehouse.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocExhibition.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }
        else if (tf == 1 && tt == 0) /// OW
        {
            var rsF = d.LocOutlet.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocWarehouse.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }
        else if (tf == 1 && tt == 1) /// OO
        {
            var rsF = d.LocOutlet.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocOutlet.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT && p.ID != vF);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }
        else if (tf == 1 && tt == 2) /// OE
        {
            var rsF = d.LocOutlet.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocExhibition.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }
        else if (tf == 2 && tt == 0) /// EW
        {
            var rsF = d.LocExhibition.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocWarehouse.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }
        else if (tf == 2 && tt == 1) /// EO
        {
            var rsF = d.LocExhibition.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocOutlet.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }
        else if (tf == 2 && tt == 2) /// EE
        {
            var rsF = d.LocExhibition.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF);
            var rsT = d.LocExhibition.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT && p.ID != vF);
            if (rsF == null) App.DataNull(this);
            if (rsT == null) App.DataNull(this);

            from.InnerText = rsF.Nama;
            to.InnerText = rsT.Nama;
        }

    }
	//Save
    protected bool valid()
    {
        bool status = true;
        if (!Fv.isDateTime(tgl, tgl_ERR)) { status = false; }
        if (!Fv.isDateTime(tglETA, tglETA_ERR)) { status = false; }
        return status;
        // return  }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid()) {
            StockOutgoingSouvenir n = new StockOutgoingSouvenir();
            int IDBrand = 1;
            if (tf == 0) { var rs = d.LocWarehouse.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF); n.NamaAsal = rs.Nama; IDBrand = rs.IDBrand;}
            else if (tf == 1) { var rs = d.LocOutlet.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF); n.NamaAsal = rs.Nama; IDBrand = rs.IDBrand; }
            else if (tf == 2) { var rs = d.LocExhibition.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vF); n.NamaAsal = rs.Nama; IDBrand = rs.IDBrand; }

            if (tt == 0) { var rs = d.LocWarehouse.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT); n.NamaTujuan = rs.Nama; }
            else if (tt == 1) { var rs = d.LocOutlet.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT); n.NamaTujuan = rs.Nama; }
            else if (tt == 2) { var rs = d.LocExhibition.SingleOrDefault(p => (vB > 0) ? p.IDBrand == vB : 1 == 1 && p.ID == vT); n.NamaTujuan = rs.Nama; }

            //Input
            //n.IDBrand = vB;
            n.IDBrand = IDBrand;
            n.NamaKurir = namakurir.Text.Trim();
            n.TglETA = Convert.ToDateTime(tglETA.Text);
            n.TipeAsal = tf;
            n.TipeTujuan = tt;
            n.IDAsal = vF;
            n.IDTujuan = vT;
            n.Keterangan = keterangan.Text.Trim();
            
            //System Auto Input
            n.StatusTransOut = 0; // 0 : New
            n.Tgl = Convert.ToDateTime(tgl.Text);

            //User-Info
            n.Operator = App.UserNama;
            n.OperatorTgl = DateTime.Now;

            //Commit
            d.StockOutgoingSouvenir.InsertOnSubmit(n);
            d.SubmitChanges();

			//Redirect
            Response.Redirect("Product_Product.aspx?id=" + n.ID);
		}
	}
}