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
	protected int myIDWarehouse { get { return App.QueryInt(this, "IDWarehouse"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			keterangan.Focus();

			//Form
			fillForm();

			//DLC
			DLC.SumberStockReceive(sumber);

			//Form-Default
			tgl.Text = Str.Tgl(DateTime.Today);
			sumber.SelectedIndex = 0;
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("GJ.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.LocWarehouse.SingleOrDefault(p => p.ID == myIDWarehouse && !p.Disable);
		if (d == null) DataError();
		else
			warehouse.InnerHtml = d.Nama;
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			StockReceiveGJ n = new StockReceiveGJ();

			//Location
			n.IDWarehouse = myIDWarehouse;

			//Trans. Form
			n.Sumber = sumber.SelectedIndex;
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = Convert.ToDateTime(tgl.Text);
			//n.Nomor = Generate_Nomor(n);

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;

			//User-Info
			n.Operator = App.UserNama;
			n.OperatorTgl = DateTime.Now;

            //n.ImgPicture = "D:\\Images\\StockReceiveGJ\\" + Generate_Nomor(n) + ".jpg";

			//Commit
			db.StockReceiveGJ.InsertOnSubmit(n);
			db.SubmitChanges();

			//Redirect
            if(sumber.SelectedValue=="1")
                Response.Redirect("GJ_KodeKaretList.aspx?id=" + n.ID);
            else
                Response.Redirect("GJ_Data01.aspx?id=" + n.ID);
		}
	}
	
}