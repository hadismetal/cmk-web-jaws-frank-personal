using System;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_Form : System.Web.UI.Page {
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
		}
	}

    protected void customerSearch(int xID)
    {
        search_customer.HRef = (STAMPS.xActivate) ? "/OutletSales/Page/Create/DataCustomerStamps.aspx" : search_customer.HRef;
        search_customer.HRef += "?id=" + xID;
        search_customer.HRef += (STAMPS.xActivate) ? "&mod=TRADEIN" : "";
    }

    //Exception
    protected void DataError() { Response.Redirect("TradeIn.aspx"); }

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

                    SalesOrder so = new SalesOrder();

                    //Location
                    so.IDLokasi = myIDLokasi;
                    so.TipeLokasi = tipe;

                    //Status
                    so.Draft = true;
                    so.DraftDate = DateTime.Now;

                    //User-Info
                    so.Operator = App.UserNama;
                    so.OperatorTgl = DateTime.Now;
                    so.TradeIn = true;
                    so.StatusPembayaran = false;
                    so.StatusVoid = false;

                    //Commit
                    db.SalesOrder.InsertOnSubmit(so);
                    db.SubmitChanges();

                    Resell ro = new Resell();

                    //Location
                    ro.IDLokasi = myIDLokasi;
                    ro.TipeLokasi = tipe;

                    //Status
                    ro.Draft = true;
                    ro.DraftDate = DateTime.Now;

                    //User-Info
                    ro.OperatorNama = App.UserNama;
                    ro.OperatorTgl = DateTime.Now;

                    //Commit
                    db.Resell.InsertOnSubmit(ro);
                    db.SubmitChanges();

                    TradeIn n = new TradeIn();

                    //Location
                    n.IDLokasi = myIDLokasi;
                    n.TipeLokasi = tipe;

                    //Status
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;

                    //User-Info
                    n.OperatorNama = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.IDSalesOrder = so.ID;
                    n.IDResell = ro.ID;

                    //Commit
                    db.TradeIn.InsertOnSubmit(n);
                    db.SubmitChanges();

                    string qstr = "?id=" + n.ID;
                    customerSearch(n.ID);
                    search_agent.HRef += qstr;
                    ID = n.ID.ToString();


                }
            }
            else if (tipe == 2)
            {
                var d = db.LocExhibition.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
                if (d == null) DataError();
                else
                {
                    warehouse.InnerHtml = d.Nama;

                    SalesOrder so = new SalesOrder();

                    //Location
                    so.IDLokasi = myIDLokasi;
                    so.TipeLokasi = tipe;

                    //Status
                    so.Draft = true;
                    so.DraftDate = DateTime.Now;

                    //User-Info
                    so.Operator = App.UserNama;
                    so.OperatorTgl = DateTime.Now;
                    so.TradeIn = true;
                    so.StatusPembayaran = false;
                    so.StatusVoid = false;

                    //Commit
                    db.SalesOrder.InsertOnSubmit(so);
                    db.SubmitChanges();

                    Resell ro = new Resell();

                    //Location
                    ro.IDLokasi = myIDLokasi;
                    ro.TipeLokasi = tipe;

                    //Status
                    ro.Draft = true;
                    ro.DraftDate = DateTime.Now;

                    //User-Info
                    ro.OperatorNama = App.UserNama;
                    ro.OperatorTgl = DateTime.Now;

                    //Commit
                    db.Resell.InsertOnSubmit(ro);
                    db.SubmitChanges();

                    TradeIn n = new TradeIn();

                    //Location
                    n.IDLokasi = myIDLokasi;
                    n.TipeLokasi = tipe;

                    //Status
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;

                    //User-Info
                    n.OperatorNama = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.IDSalesOrder = so.ID;
                    n.IDResell = ro.ID;

                    //Commit
                    db.TradeIn.InsertOnSubmit(n);
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
            var d = db.TradeIn.SingleOrDefault(p => p.ID == myID && p.Draft);
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
                if (d.IDSales != null)
                {
                    var s = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales);
                    agent.Text = s.Nama;
                }
				if(!String.IsNullOrEmpty(d.SalesOrder.CustomerReferenceNama))
				{
					givento.Text = d.SalesOrder.CustomerReferenceNama;
				}
                //if (d.IDSales2 != null)
                //{
                //    var s = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales2);
                //    agent2.Text = s.Nama;
                //}
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
    protected bool validJPJR
    {
        get
        {
            if (agent.Text == "")
                return false;
            else
                return true;
        }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid && validCustomer && validJPJR) {
            var d = db.TradeIn.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                d.SalesOrder.IDSales = d.IDSales;
                d.SalesOrder.SalesNama = d.SalesNama;
                d.SalesOrder.Tgl = Convert.ToDateTime(tgl.Text);
				//d.SalesOrder.Tgl = DateTime.Now;
                d.SalesOrder.IDCustomer = d.IDCustomer;
                d.SalesOrder.CustomerNama = d.CustomerNama;
				d.SalesOrder.CustomerReferenceNama = ((givento.Text == "") ? d.CustomerNama : givento.Text);
				d.SalesOrder.Nomor = "";
				
                d.Resell.IDCustomer = d.IDCustomer;
                d.Resell.CustomerNama = d.CustomerNama;
                d.Resell.Tgl = Convert.ToDateTime(tgl.Text);
				//d.Resell.Tgl = DateTime.Now;
                d.Keterangan = Str.CapFirst(keterangan.Text);
                d.Tgl = Convert.ToDateTime(tgl.Text);
				//d.Tgl = DateTime.Now;
                d.Nomor = null;

                var so = d.SalesOrder;
                so.Keterangan = Str.CapFirst(keterangan.Text);

                db.SubmitChanges();
				if (!String.IsNullOrEmpty(txtnameoldcustomercode.Text))
                {
                    var dz = db.DataCustomer.FirstOrDefault(p => p.NoCustomer == txtoldcustomercode.Text);
                    if (dz != null) {
                        Db.Execute("Exec sp_insert_kode_customer_lama '" + d.IDSalesOrder.ToString() + "','" + txtoldcustomercode.Text + "','SO' ", Db.Cnn_DB);
                        Db.Execute("Exec sp_insert_kode_customer_lama '" + d.ID.ToString() + "','" + txtoldcustomercode.Text + "','TI' ", Db.Cnn_DB);
                        Db.Execute("Exec sp_insert_kode_customer_lama '" + d.IDResell.ToString() + "','" + txtoldcustomercode.Text + "','RS' ", Db.Cnn_DB);
                    }
                }												   
				save_Ereceipt();
            }

            Response.Redirect("TradeIn_Data.aspx?id=" + myID + "&so=" + d.IDSalesOrder.ToString() + "&ro=" + d.IDResell.ToString());
		}
	}

	protected void save_Ereceipt()
	{
		var t = db.TradeIn.SingleOrDefault(p => p.ID == myID);
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == t.IDSalesOrder);
		string queryupdate = "UPDATE SalesOrder SET "; 
		if(rdo_ereceipt_no.Checked)
		{
			queryupdate += "eReceiptHP = NULL, eReceiptEmail = NULL, IsSentEreceipt = 0, EreceiptType = '" + ddl_erecipt.SelectedValue + "', EreceiptKeterangan = '" + ereceipt_light.Text + "'";
		}
		else if(rdo_ereceipt_yes.Checked)
		{
			queryupdate += "eReceiptHP = '" + d.DataCustomer.AddrNoTelp + "', eReceiptEmail = '" + d.DataCustomer.AddrEmail + "', IsSentEreceipt = 1, EreceiptType = NULL, EreceiptKeterangan = NULL";
		}
		queryupdate += " WHERE ID = " + d.ID.ToString();
		Db.Execute(queryupdate, Db.Cnn_DB);
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
            save.Attributes.Add("disabled","disabled");
        else
            save.Attributes.Remove("disabled");
    }
	protected void Event(DropDownList c)
	{
		DataTable dt = Db.Rs("SELECT Nama, ID FROM Promo WHERE Disable = 0 AND Draft = 0 AND GETDATE() BETWEEN StartDate AND ENDDATE AND Tipe = 2;", Db.Cnn_DB);
        c.Items.Add(new ListItem("No Event", "0"));
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow r in dt.Rows)
            {
                string t = r["Nama"].ToString();
                string v = r["ID"].ToString();
                c.Items.Add(new ListItem(t, v));
            }
        }
	}
}