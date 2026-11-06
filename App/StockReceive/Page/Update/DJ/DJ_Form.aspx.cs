using System;
using System.Linq;

public partial class Page_Register_DJ_Form : System.Web.UI.Page {
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
    protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			keterangan.Focus();

			

			//DLC
			DLC.SumberStockReceive(sumber);

            //Form
            fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
        if (d == null) DataError();
        else
        {
            keterangan.Text = d.Keterangan;
            sumber.SelectedIndex = d.Sumber;
            tgl.Text = Str.Tgl(d.Tgl);
            warehouse.InnerText = d.LocWarehouse.Nama;
        }
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
            var n = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID);

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

            //n.ImgPicture = "D:\\Images\\StockReceiveDJ\\" + Generate_Nomor(n) + ".jpg";

			db.SubmitChanges();

                Response.Redirect("DJ_Data01.aspx?id=" + n.ID);
		}
	}
	
}