using System;
using System.Data;				  
using System.Linq;

public partial class Page_Register_Resell_Form : System.Web.UI.Page {
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
			btnNextStepCondition();

			/*if (myID == 0)   
			{
				 divtypetransaction.Visible = false;
			}*/
			try
			{				  
				string selectvalue = LoadTypeTransaction(myID);
				typetransaction.SelectedValue = (selectvalue == "CMK") ? "1" : "2" ;
			}
			catch{}
		}
	}

    protected void customerSearch(int xID)
    {
        search_customer.HRef = (STAMPS.xActivate) ? "/OutletSales/Page/Create/DataCustomerStamps.aspx" : search_customer.HRef;
        search_customer.HRef += "?id=" + xID;
        search_customer.HRef += (STAMPS.xActivate) ? "&mod=RESELL" : "";
    }

    //Exception
    protected void DataError() { Response.Redirect("Resell.aspx"); }

	//Form
	protected void fillForm() {
        if (status != 1)
        {
            if (tipe == 1)
            {
                var d = db.LocOutlet.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
                if (d == null) DataError();
                else
                {
                    warehouse.InnerHtml = d.Nama;


                    Resell n = new Resell();

                    //Location
                    n.IDLokasi = myIDLokasi;
                    n.TipeLokasi = tipe;

                    //Status
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;

                    //User-Info
                    n.OperatorNama = App.UserNama;
                    n.OperatorTgl = DateTime.Now;

                    //Commit
                    db.Resell.InsertOnSubmit(n);
                    db.SubmitChanges();

                    string qstr = "?id=" + n.ID;
                    customerSearch(n.ID);
                    search_agent.HRef += qstr;
                    ID = n.ID.ToString();


                }
            }
            else if(tipe == 2)
            {
                var d = db.LocExhibition.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
                if (d == null) DataError();
                else
                {
                    warehouse.InnerHtml = d.Nama;


                    Resell n = new Resell();

                    //Location
                    n.IDLokasi = myIDLokasi;
                    n.TipeLokasi = tipe;

                    //Status
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;

                    //User-Info
                    n.OperatorNama = App.UserNama;
                    n.OperatorTgl = DateTime.Now;

                    //Commit
                    db.Resell.InsertOnSubmit(n);
                    db.SubmitChanges();

                    string qstr = "?id=" + n.ID;
                    customerSearch(n.ID);
                    search_agent.HRef += qstr;
                    ID = n.ID.ToString();


                }
            }
        }
        else
        {
            var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft);
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
                    customer.Text = d.CustomerNama;
                if (d.IDSales != null)
                {
                    var s = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales);
                    agent.Text = s.Nama;
                }
            }
            string qstr = "?id=" + myID;
            search_customer.HRef += qstr;
            search_agent.HRef += qstr;
            ID = myID.ToString();
        }
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
    protected bool validCustomer
    {
        get
        {
            if (customer.Text == "")
                return false;
            else
                return true;
        }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid && validCustomer) {
            var d = db.Resell.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                d.Keterangan = Str.CapFirst(keterangan.Text);
                d.Tgl = Convert.ToDateTime(tgl.Text);
                //d.Nomor = Generate_Nomor(d);
                //d.Nomor = "";
                d.TradeIn = false;

                db.SubmitChanges();

                if (!String.IsNullOrEmpty(txtnameoldcustomercode.Text))
                {
                    var dz = db.DataCustomer.FirstOrDefault(p => p.NoCustomer == txtoldcustomercode.Text);
                    if (dz != null){
                        //Db.Execute("Exec sp_insert_kode_customer_lama '" + d.IDSalesOrder.ToString() + "','" + txtoldcustomercode.Text + "','SO' ", Db.Cnn_DB);
                        //Db.Execute("Exec sp_insert_kode_customer_lama '" + d.ID.ToString() + "','" + txtoldcustomercode.Text + "','TI' ", Db.Cnn_DB);
                        Db.Execute("Exec sp_insert_kode_customer_lama '" + d.ID.ToString() + "','" + txtoldcustomercode.Text + "','RS' ", Db.Cnn_DB);
                    }
                }
            }

            Response.Redirect("Resell_Data.aspx?id=" + myID);
		}
	}
	protected void search_oldcustumercode_ServerClick(object sender, EventArgs e)
    {
        var d = db.DataCustomer.FirstOrDefault(p => p.NoCustomer == txtoldcustomercode.Text);
        if (d != null)
            txtnameoldcustomercode.Text = String.IsNullOrEmpty(d.NoCustomer) ? "" : d.Nama;
        else
            txtnameoldcustomercode.Text = "";
        if (txtnameoldcustomercode.Text == "")
        {
            lblcheckoldcustumer.Visible = true;
            lblcheckoldcustumer.Text = "*Kode Customer Lama tidak ditemukan.";
        }
        else
            lblcheckoldcustumer.Visible = false;
    }
	
    protected void btnNextStepCondition()
    {
        if (customer.Text == "" || agent.Text == "" || tgl.Text == "")
            save.Attributes.Add("disabled", "disabled");
        else
            save.Attributes.Remove("disabled");
    } 

	protected void changetypetransaction_Change(object sender, EventArgs e)
	{
		filltyperesell(myID);
		//LoadTypeTransaction(idresell, "U");
	}
	
	protected void filltyperesell(int idresell)
	{
		string typeresell = "CMK"; //CMK, LE, GT
		if(myID == 0)
		{
			Resell n = new Resell();
			
			//Location
			n.IDLokasi = myIDLokasi;
			n.TipeLokasi = tipe;

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;

			//User-Info
			n.OperatorNama = App.UserNama;
			n.OperatorTgl = DateTime.Now;

			//Commit
			db.Resell.InsertOnSubmit(n);
			db.SubmitChanges();

			idresell = n.ID;
			
			LoadTypeTransaction(idresell, "U");
			
			Response.Redirect("Resell_Form.aspx?id=" + idresell + "&status=1");
		}
		else
		{
			idresell = myID;
			LoadTypeTransaction(idresell, "U");
		}
		//return idresell;
	}
	
	protected string LoadTypeTransaction(int idresell, string action = null)
    {
        string typeresell = "CMK"; //CMK, LE, GT
        DataTable dt = new DataTable();
        if (action == "U")
        {
            string kode = (typetransaction.SelectedValue == "1") ? "CMK" : "GT" ;
            string queryupdate = "UPDATE Resell SET [GroupResell] = '"+kode+"' WHERE ID = " + idresell;
            Db.Execute(queryupdate, Db.Cnn_DB);
        }

        string query = "SELECT TOP 1 ISNULL(GroupResell,'CMK') [GroupResell] FROM Resell WHERE ID = " + idresell;
        dt = Db.Rs(query, Db.Cnn_DB);
        int rowcount = dt.Rows.Count;
        if(rowcount == 1)
        {
            typeresell = dt.Rows[0]["GroupResell"].ToString();
        }
		dt.Dispose();
		typetransaction.SelectedValue = (typeresell == "CMK") ? "1" : "2";																																	   																								
        return typeresell;
    }
}