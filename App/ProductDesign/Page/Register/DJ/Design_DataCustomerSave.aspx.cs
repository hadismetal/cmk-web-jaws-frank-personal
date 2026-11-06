using System;
using System.Linq;

public partial class Page_Register_Design_DataDesignerSave : System.Web.UI.Page {
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (cs != null)
	        cs.Dispose();
	    base.Dispose();
	}
	protected int myID { get { return App.QueryInt(this); } }
	protected string myLink { get { return Request.QueryString["link"]; } }
   

	//Exception
	protected void DataError() { Response.Redirect("Design.aspx"); }
	protected void LinkError() { Response.Redirect("Design_DataCustomer.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			saveLink(d);

			//Redirect
			Response.Redirect("Design_Data01.aspx?id=" + myID + "&status=1");
		}
	}
	protected void saveLink(ProductDesign data)
    {
        var des = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
        var d = db.DataCustomer.SingleOrDefault(
			p =>
			p.NoCustomer == myLink && !p.Disable
			);

        if (d == null)
        {
            var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode == myLink);
            DataCustomer n = new DataCustomer();
            n.AddrAlamat = cust.M_alamat;
            n.AddrEmail = cust.M_email;
            n.AddrMobile = cust.M_telepon1;
            n.AddrNoFax = cust.M_fax;
            n.AddrNoTelp2 = cust.M_telepon2;
            n.Agama = Convert.ToInt32(cust.M_agama);
            n.Disable = false;
            n.Draft = false;
            n.NoCustomer = cust.M_kode;
            n.NoMember = cust.M_member;
            n.NoNPWP = cust.M_npwp;
            n.PinBB = cust.M_pinbb;
            n.Nama = cust.M_nama;
            n.TglLahir = cust.M_tgllahir;
            n.Tgl = Convert.ToDateTime(cust.Createdate);
            n.Keterangan = "";
            
            n.IDBrand = des.DataHuman.CompanyBrand.ID;
            db.DataCustomer.InsertOnSubmit(n);
            db.SubmitChanges();

            data.RequestCustomer = n.ID;
            data.RequestCustomerNama = n.Nama;
            db.SubmitChanges();
        }
        else
        {
            data.RequestCustomer = d.ID;
            data.RequestCustomerNama = d.Nama;

            //Commit
            db.SubmitChanges();
        }
	}
}