using System;
using System.Linq;

public partial class Page_Register_Repair_Form : System.Web.UI.Page {
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

    string ID = "";

    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected int myIDLokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected int status { get { return App.QueryInt(this, "status"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			keterangan.Focus();

            if (tipe == 0) { lit_tipelocation.Text = "Warehouse"; }
            else if (tipe == 1) { lit_tipelocation.Text = "Outlet"; }
            else if (tipe == 2) { lit_tipelocation.Text = "Exhibition"; }

			//Form
			fillForm();
            
			//Form-Default
            tglselesai.Text = Str.Tgl(DateTime.Today);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("RepairInvoice.aspx"); }

	//Form
	protected void fillForm() {
        if (status != 1)
        {
            var d = db.LocOutlet.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
            if (d == null) DataError();
            else
            {
                locationname.InnerHtml = d.Nama;

                DocRepairResult n = new DocRepairResult();

                //Location
                n.IDLokasi = myIDLokasi;
                n.TipeLokasi = tipe;

                //Status
                n.Draft = true;
                n.DraftDate = DateTime.Now;

                //User-Info
                n.Operator = App.UserNama;
                n.OperatorTgl = DateTime.Now;
                n.StatusRepair = 0;

                //Commit
                db.DocRepairResult.InsertOnSubmit(n);
                db.SubmitChanges();

                string qstr = "?id=" + n.ID;
                search_doc.HRef += qstr;
                ID = n.ID.ToString();
                
            }
        }
        else
        {
            var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) DataError();
            else
            {
                string NamaLokasi = "";

                if (d.TipeLokasi == 0) { lit_tipelocation.Text = "Warehouse"; NamaLokasi = db.LocWarehouse.SingleOrDefault(p => p.ID == d.IDLokasi).Nama; }
                else if (d.TipeLokasi == 1) { lit_tipelocation.Text = "Outlet"; NamaLokasi = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).Nama; }
                else if (d.TipeLokasi == 2) { lit_tipelocation.Text = "Exhibition"; NamaLokasi = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).Nama; }

                locationname.InnerText = NamaLokasi;

                if (d.IDWorkOrder != null)
                    doc.Text = d.DocRepair.Nomor;
            }
            string qstr = "?id=" + myID;
            search_doc.HRef += qstr;
            ID = myID.ToString();
        }
	}

    protected bool validDoc
    {
        get
        {
            if (doc.Text == "")
                return false;
            else
                return true;
        }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (validDoc) {
            var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                var s = db.DocRepair.SingleOrDefault(p => p.ID == d.IDWorkOrder);

                d.Keterangan = Str.CapFirst(keterangan.Text);
                d.Tgl = s.Tgl;
                d.TglSelesai = Convert.ToDateTime(tglselesai.Text);
                //d.Nomor = Generate_Nomor(d);
                d.Nomor = "";
                d.Sumber = s.Sumber;
                if (s.IDProduct.HasValue)
                    d.IDProduct = s.IDProduct.Value;
                if (s.IDProductTitipan.HasValue)
                    d.IDProductTitipan = s.IDProductTitipan.Value;

                db.SubmitChanges();

                moveItem(d);
            }

            Response.Redirect("RepairInvoice_Data.aspx?id=" + myID);
		}
	}

    protected void moveItem(DocRepairResult d)
    {
        var z = db.DocRepairResult_Repair.Where(p => p.IDForm == d.ID);
        if (z != null)
        {
            db.DocRepairResult_Repair.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var item = from p in d.DocRepair.DocRepair_Repair
                    orderby p.CharProcessRepair.Nama
                    select p;
        if (item != null)
        {
            foreach (var r in item)
            {
                DocRepairResult_Repair n = new DocRepairResult_Repair();

                n.IDForm = d.ID;
                n.Biaya = r.Biaya;
                n.Efektif = r.Efektif;
                n.IDRepair = r.IDRepair;
                n.Keterangan = r.Keterangan;
                

                //Commit
                db.DocRepairResult_Repair.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
}