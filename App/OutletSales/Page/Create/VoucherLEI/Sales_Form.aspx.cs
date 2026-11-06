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
    string Module = "pos"; // pos, acc

    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected int myIDLokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected int status { get { return App.QueryInt(this, "status"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack)
        {
			keterangan.Focus();

            //Form
            fillForm();

            if(myID > 0)
            {
                customerSearch(myID);
            }
            
            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tgl.Enabled = (Module == "acc") ? true : false;
			
			Event(ddlevent);
			if(myID > 0)
				fillSalesCrossBrand(myID);
			btnNextStepCondition();
        }
	}

    protected void customerSearch(int xID)
    {
        search_customer.HRef = (STAMPS.xActivate) ? "DataCustomerStamps.aspx" : search_customer.HRef;
        search_customer.HRef += "?id=" + xID;
        search_customer.HRef += (STAMPS.xActivate) ? "&mod=SALES" : "";
    }

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

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
                    location.InnerHtml = d.Nama;

                    SalesOrder n = new SalesOrder();

                    //Location
                    n.IDLokasi = myIDLokasi;
                    n.TipeLokasi = tipe;

                    //Status
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;

                    //User-Info
                    n.Operator = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.StatusVoid = false;
                    n.StatusPembayaran = false;

                    //Commit
                    db.SalesOrder.InsertOnSubmit(n);
                    db.SubmitChanges();

                    string qstr = "?id=" + n.ID;
                    customerSearch(n.ID);
                    search_agent.HRef += qstr;
					search_agentcb.HRef += qstr;


                    ID = n.ID.ToString();
					
					if(d.Kode == "CRP" || d.Kode == "CRL" || d.Kode == "CRS")
                    {
                        rownpwp.Visible = true;
                    }
                    else
                    {
                        rownpwp.Visible = false;
                    }

                }
            }
            else if (tipe == 2)
            {
                var d = db.LocExhibition.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
                if (d == null) DataError();
                else
                {
                    location.InnerHtml = d.Nama;


                    SalesOrder n = new SalesOrder();

                    //Location
                    n.IDLokasi = myIDLokasi;
                    n.TipeLokasi = tipe;

                    //Status
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;

                    //User-Info
                    n.Operator = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.StatusVoid = false;
                    n.StatusPembayaran = false;

                    //Commit
                    db.SalesOrder.InsertOnSubmit(n);
                    db.SubmitChanges();

                    string qstr = "?id=" + n.ID;
                    customerSearch(n.ID);
                    search_agent.HRef += qstr;
					search_agentcb.HRef += qstr;

                    ID = n.ID.ToString();


                }
            }
        }
        else
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) DataError();
            else
            {
                string Brand = "";
                if (d.TipeLokasi == 1)
                {
                    var k = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
					
					if(k.Kode == "CRP" || k.Kode == "CRL" || k.Kode == "CRS")
					{
						rownpwp.Visible = true;
					}
					else
					{
						rownpwp.Visible = false;
					}
                }
                else if (d.TipeLokasi == 2)
                {
                    var k = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }

                location.InnerHtml = Brand;
                if(d.IDCustomer !=null)
                    customer.Text = d.DataCustomer.Nama;
                if (d.IDSales != null)
                {
                    var s = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales);
                    agent.Text = s.Nama;
                }
				if (!String.IsNullOrEmpty(d.CustomerReferenceNama))
                {
                    customer.Text = d.CustomerReferenceNama;
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
			search_agentcb.HRef += qstr;
            ID = myID.ToString();
        }
	}

	//Save
    protected bool validDate
    {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
    protected bool validDate2
    {
        get
        {
            bool result = false;

            try
            {
                if (Convert.ToDateTime(tgl.Text) <= System.DateTime.Today) { result = true; }
            }
            catch (Exception ex) { }

            return result;
        }
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
	protected void save_Click(object sender, EventArgs e) 
    {
        if (validDate && validDate2 && validCustomer && validJPJR)
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                d.Keterangan = Str.CapFirst(keterangan.Text);
				d.Tgl = DateTime.Now;
                //d.Tgl = (Module == "acc") ? Convert.ToDateTime(tgl.Text) : System.DateTime.Today;
                //d.Nomor = Generate_Nomor(d);
                d.Nomor = "";
				d.CustomerReferenceNama = (givento.Text == "" ? customer.Text : givento.Text );
                d.TradeIn = false;

                db.SubmitChanges();
				
				Db.Execute("UPDATE SalesOrder SET IDPromo = " + (ddlevent.SelectedValue == "0" ? "NULL" : ddlevent.SelectedValue ) + " WHERE ID = " + myID, Db.Cnn_DB);
				
				if (rownpwp.Visible == true)
                {
                    if(Convert.ToInt16(isanynpwp.SelectedValue) == 1)
                    {
                        Db.Execute("UPDATE SalesOrder SET NPWP = 1 WHERE ID=" + myID,Db.Cnn_DB);
                    }
                }
            }

            Response.Redirect("Sales_Data.aspx?id=" + myID);
		}
	}
	protected void btnNextStepCondition()
    {
        if (customer.Text == "" || agent.Text == "" || tgl.Text == "")
            save.Attributes.Add("disabled","disabled");
        else
            save.Attributes.Remove("disabled");
    }
	
    protected void fillSalesCrossBrand(int idsalesorder)
	{
		try
		{
			DataTable dt = new DataTable(); 
			int tipeloc = 0; 
			int idloc = 0; 
			string brandcross = ""; 
			string idsalescross = "";
			
			dt = Db.Rs("SELECT TOP 1 KodeBrandCross, TipeLokasiCross, IDLokasiCross, IDSalesCross FROM SalesOrder WHERE ID = " + idsalesorder, Db.Cnn_DB);
			brandcross = dt.Rows[0]["KodeBrandCross"].ToString();
			tipeloc = Convert.ToInt32(dt.Rows[0]["TipeLokasiCross"]);
			idloc = Convert.ToInt32(dt.Rows[0]["IDLokasiCross"]);
			idsalescross = dt.Rows[0]["IDSalesCross"].ToString();
			
			if(brandcross == "S" || brandcross == "M")
			{
				string queryloc = "";
				if(tipeloc == 1)
				{
					queryloc = "SELECT TOP 1 Kode FROM mondial_db_real.dbo.LocOutlet WHERE ID = " + idloc;
				}
				else
				{
					queryloc = "SELECT TOP 1 Kode FROM mondial_db_real.dbo.LocExhibition WHERE ID = " + idloc;
				}
				agentcb.Text = Db.SingleString("SELECT TOP 1 Nama FROM mondial_db_real.dbo.DataSales WHERE ID = " + idsalescross, Db.Cnn_DB);
				storecb.Text = Db.SingleString(queryloc, Db.Cnn_DB);
				if(brandcross == "S")
					sbucb.Text = "MISS MONDIAL";
				else if(brandcross == "M")
					sbucb.Text = "MONDIAL";
			}
			else if(brandcross == "F")
			{
				string queryloc = "";
				if(tipeloc == 1)
				{
					queryloc = "SELECT TOP 1 Kode FROM frank_db.dbo.LocOutlet WHERE ID = " + idloc;
				}
				else
				{
					queryloc = "SELECT TOP 1 Kode FROM frank_db.dbo.LocExhibition WHERE ID = " + idloc;
				}
				agentcb.Text = Db.SingleString("SELECT TOP 1 Nama FROM frank_db.dbo.DataSales WHERE ID = " + idsalescross, Db.Cnn_DB);
				storecb.Text = Db.SingleString(queryloc, Db.Cnn_DB);
				sbucb.Text = "FRANK";
			}
			else
			{
				string queryloc = "";
				if(tipeloc == 1)
				{
					queryloc = "SELECT TOP 1 Kode FROM palace_db_real.dbo.LocOutlet WHERE ID = " + idloc;
				}
				else
				{
					queryloc = "SELECT TOP 1 Kode FROM palace_db_real.dbo.LocExhibition WHERE ID = " + idloc;
				}
				agentcb.Text = Db.SingleString("SELECT TOP 1 Nama FROM palace_db_real.dbo.DataSales WHERE ID = " + idsalescross, Db.Cnn_DB);
				storecb.Text = Db.SingleString(queryloc, Db.Cnn_DB);
				sbucb.Text = "THE PALACE";
			}
		}
		catch{}
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