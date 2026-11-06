using System;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_Form : System.Web.UI.Page
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

    string ID = "";
    string Module = "pos"; // pos, acc

    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int myIDLokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected int status { get { return App.QueryInt(this, "status"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            keterangan.Focus();

            //Form
            fillForm();

            if (myID > 0)
            {
                customerSearch(myID);
            }

            //Form-Default
			Event(ddlevent);
            tgl.Text = Str.Tgl(DateTime.Today);
            tgl.Enabled = (Module == "acc") ? true : false;
			
        }
    }

    protected void customerSearch(int xID)
    {
        search_customer.HRef = (STAMPS.xActivate) ? "/OutletSales/Page/Create/DataCustomerStamps.aspx" : search_customer.HRef;
        search_customer.HRef += "?id=" + xID;
        search_customer.HRef += (STAMPS.xActivate) ? "&mod=PACKAGING" : "";
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form
    protected void fillForm()
    {
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
					search_so.HRef += qstr;
                    ID = n.ID.ToString();


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
					search_so.HRef += qstr;
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
                }
                else if (d.TipeLokasi == 2)
                {
                    var k = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }

                location.InnerHtml = Brand;
                if (d.IDCustomer != null)
                    customer.Text = d.DataCustomer.Nama;
                if (d.IDSales != null)
                {
                    var s = db.DataSales.SingleOrDefault(p => p.ID == d.IDSales);
                    agent.Text = s.Nama;
                }
				string pkso = Db.SingleString("SELECT TOP 1 ISNULL(LinkPackagingSalesOrder,'') [LPSO] FROM SalesOrder WHERE ID = " + d.ID, Db.Cnn_DB);
				if(!string.IsNullOrEmpty(pkso))
				{
					so.Text = pkso;
				}
				string pkdp = Db.SingleString("SELECT TOP 1 ISNULL(LinkPackagingDownPayment,'') [LPDP] FROM SalesOrder WHERE ID = " + d.ID,Db.Cnn_DB);
				if(!string.IsNullOrEmpty(pkdp))
				{
					so.Text = pkdp;
				}
				string pkrep = Db.SingleString("SELECT TOP 1 ISNULL(LinkPackagingRepair,'') [LPRP] FROM SalesOrder WHERE ID = " + d.ID, Db.Cnn_DB);
				if(!string.IsNullOrEmpty(pkrep))
				{
					so.Text = pkrep;
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
			search_so.HRef += qstr;
            //search_agent2.HRef += qstr + "&tipe=2";
            ID = myID.ToString();
        }
		//validateall();
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
                d.Tgl = (Module == "acc") ? Convert.ToDateTime(tgl.Text) : System.DateTime.Today;
                //d.Nomor = Generate_Nomor(d);
                d.Nomor = "";
                d.TradeIn = false;

                db.SubmitChanges();
				
				Db.Execute("UPDATE SalesOrder SET IDPromo = " + (ddlevent.SelectedValue == "0" ? "NULL" : ddlevent.SelectedValue ) + " WHERE ID = " + myID, Db.Cnn_DB);
				
				if(is_using_reference.Checked == false)
				{
					Db.Execute("UPDATE SalesOrder SET LinkPackagingSalesOrder = null, LinkPackagingRepair = null, LinkPackagingDownPayment = null WHERE ID = " + myID, Db.Cnn_DB);
				}
            }

            Response.Redirect("Sales_Data.aspx?id=" + myID);
        }
    }
	
	protected bool validateTransactionNumber
	{
		get
		{
			if(string.IsNullOrEmpty(so.Text))
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
	
	protected void validateall()
	{
		bool validate = false;
		validate = validCustomer;
		
		if(validate == true)
			validate = validJPJR;
		
		if(validate == true)
			validate = validateTransactionNumber;
		
		if(validate == true)
			save.Disabled = false;
		else
			save.Disabled = true;
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