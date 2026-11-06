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

			//Form
			fillForm();

            if (myID > 0)
            {
                customerSearch(myID);
            }

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tgl.Enabled = false;
            tglselesai.Text = Str.Tgl(DateTime.Today.AddMonths(1));
            btnNextStepCondition();

        }
	}

    protected void customerSearch(int xID)
    {
        search_customer.HRef = (STAMPS.xActivate) ? "/OutletSales/Page/Create/DataCustomerStamps.aspx" : search_customer.HRef;
        search_customer.HRef += "?id=" + xID;
        search_customer.HRef += (STAMPS.xActivate) ? "&mod=REPAIR" : "";
    }

    //Exception
    protected void DataError() { Response.Redirect("Repair.aspx"); }

	//Form
	protected void fillForm() {
        if (status != 1)
        {
			if(tipe == 1)
			{
				var d = db.LocOutlet.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
				if (d == null) DataError();
				else
				{
					warehouse.InnerHtml = d.Nama;


					DocRepair n = new DocRepair();

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
					n.Sumber = 0;

					//Commit
					db.DocRepair.InsertOnSubmit(n);
					db.SubmitChanges();

					string qstr = "?id=" + n.ID;
					customerSearch(n.ID);
					search_plu.HRef += qstr;
					search_agent.HRef += qstr;
					search_qc.HRef += qstr;
					ID = n.ID.ToString();
					

				}
			}
			else
			{
				var d = db.LocExhibition.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
				if (d == null) DataError();
				else
				{
					warehouse.InnerHtml = d.Nama;


					DocRepair n = new DocRepair();

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
					n.Sumber = 0;

					//Commit
					db.DocRepair.InsertOnSubmit(n);
					db.SubmitChanges();

					string qstr = "?id=" + n.ID;
					customerSearch(n.ID);
					search_plu.HRef += qstr;
					search_agent.HRef += qstr;
					search_qc.HRef += qstr;
					ID = n.ID.ToString();
					

				}
			}
        }
        else
        {
            var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) DataError();
            else
            {
                string Brand = "";
                if (d.TipeLokasi == 1)
                {
                    var k = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }
                else if (d.TipeLokasi == 2)
                {
                    var k = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }

                warehouse.InnerHtml = Brand;
                if(d.IDCustomer !=null)
                    customer.Text = d.DataCustomer.Nama;
                if (d.IDProduct != null)
                    plu.Text = d.StockProductDJ.Nomor;
                if(d.IDProductTitipan != null)
                    plu.Text = d.StockProductDJCustomer.Nomor;
                if (d.IDSales != null)
                    agent.Text = d.SalesNama;
                if (d.IDQC != null)
                    qc.Text = db.DocQCDJ.SingleOrDefault(p => p.ID == d.IDQC).Nomor;
                if (d.IDQCTitipan != null)
                    qc.Text = db.DocQCDJCustomer.SingleOrDefault(p => p.ID == d.IDQCTitipan).Nomor;
            }
            string qstr = "?id=" + myID;
            search_customer.HRef += qstr;
            search_plu.HRef += qstr;
            search_agent.HRef += qstr;
            search_qc.HRef += qstr;
            ID = myID.ToString();
        }
	}

	//Save
	protected bool valid {
		get {
            bool x = true;
            if (Fv.isDateTime(tgl, tgl_ERR) == false)
                x= false;
            if (Fv.isDateTime(tglselesai, tglselesai_ERR) == false)
                x= false;
            return x; 
        }
	}
    protected bool validPLU
    {
        get
        {
            if (plu.Text == "")
                return false;
            else
                return true;
        }
    }
    protected bool validQC
    {
        get
        {
            if (qc.Text == "")
                return false;
            else
                return true;
        }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid && validPLU && validQC) {
            var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                d.Keterangan = Str.CapFirst(keterangan.Text);
                //d.Tgl = Convert.ToDateTime(tgl.Text);
                d.Tgl = System.DateTime.Now;
                d.TglSelesai = Convert.ToDateTime(tglselesai.Text);
                //d.Nomor = Generate_Nomor(d);
                d.Nomor = "";

                db.SubmitChanges();
            }
            // db.Rs("UPDATE DocRepair SET Flag = 1 WHERE ID=" + myID,db.Cnn_DB);
            db.ExecuteCommand(
                "UPDATE DocRepair SET Flag = 1 WHERE ID = {0}", myID
            );

            Response.Redirect("Repair_Picture.aspx?id=" + myID + "&md=" + "1");
            //Response.Redirect("Repair_Data.aspx?id=" + myID);
		}
	}
    protected void btnNextStepCondition()
    {
        if (customer.Text == "" || agent.Text == "" || tgl.Text == "" || plu.Text == "" || qc.Text == "")
            save.Attributes.Add("disabled", "disabled");
        else
            save.Attributes.Remove("disabled");
    }

}