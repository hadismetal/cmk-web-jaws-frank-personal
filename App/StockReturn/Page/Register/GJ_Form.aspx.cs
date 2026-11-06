using System;
using System.Linq;

public partial class Page_Register_GJ_Form : System.Web.UI.Page {
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
    protected int myIDWarehouse { get { return App.QueryInt(this, "idwarehouse"); } }
    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected int status { get { return App.QueryInt(this, "status"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			keterangan.Focus();

			//Form
			fillForm();
            nodokumen.Text = Generate_Nomor();
            nodokumen.Enabled = false;
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("GJ.aspx"); }

    //Form
    protected void fillForm()
    {


        if (status != 1)
        {
            var d = db.LocWarehouse.SingleOrDefault(p => p.ID == myIDWarehouse && !p.Disable);
            if (d == null) DataError();
            else
                warehouse.InnerHtml = d.Nama;

            StockReturnGJ n = new StockReturnGJ();

            //Location
            n.IDWarehouse = myIDWarehouse;

            //Trans. Form
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.Tgl = DateTime.Now;
            n.NoDokumen = Generate_Nomor();

            //Status
            n.Draft = true;
            n.DraftDate = DateTime.Now;

            //User-Info
            n.Operator = App.UserNama;
            n.OperatorTgl = DateTime.Now;

            //Commit
            db.StockReturnGJ.InsertOnSubmit(n);
            db.SubmitChanges();

            search_supplier.HRef += "?id=" + n.ID;
        }
        else
        {
            var x = db.StockReturnGJ.SingleOrDefault(p => p.ID == myID);
            nodokumen.Text = x.NoDokumen;
            suppliernama.Text = x.SupplierNama;
        }


        suppliernama.Enabled = false;
    }
    protected bool validSupplier
    {
        get
        {
            if (suppliernama.Text == "")
                return false;
            else
                return true;
        }
    }
    protected void save_Click(object sender, EventArgs e)
    {

        if (validSupplier)
        {
            var n = db.StockReturnGJ.SingleOrDefault(p => p.ID == myID);


            //Trans. Form
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.Tgl = DateTime.Now;

            //Status
            n.Draft = true;
            n.DraftDate = DateTime.Now;

            //User-Info
            n.Operator = App.UserNama;
            n.OperatorTgl = DateTime.Now;

            //Commit
            db.SubmitChanges();

            //Redirect
            Response.Redirect("GJ_List.aspx?id=" + n.ID);
        }
    }
	protected string Generate_Nomor() {
		string x = string.Empty;

		int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;
        string bln = Bulan.ToString().PadLeft(2, '0');

		/*int Nomor = db.StockReturn.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
		Nomor = Nomor + 1;

		return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        /*int Nomor = db.StockReturnGJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;*/

        int Nomor = 1;

        try
        {
            var s = (from p in db.StockReturnGJ
                     where p.Tgl.Year == Tahun && p.Tgl.Month == Bulan
                     orderby p.NoDokumen.Substring(12, 6) descending
                     select p).First();

            if (s.NoDokumen != null)
                Nomor = Convert.ToInt32(s.NoDokumen.Substring(12, 6)) + 1;
        }
        catch (Exception ex) { }

        string PLU = "RTS/G/" + Tahun.ToString().Remove(0, 2) + "/" + bln + "/" + Str.KodeFormat(Nomor, 6);

        return PLU;
        
	}
}