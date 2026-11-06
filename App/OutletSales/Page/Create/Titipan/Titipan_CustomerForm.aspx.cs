using System;
using System.Linq;

public partial class Page_Register_Titipan_Form : System.Web.UI.Page {
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
	protected int myTitipanID { get { return App.QueryInt(this, "id"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			keterangan.Focus();

		}
	}

	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }

	
	protected void save_Click(object sender, EventArgs e) {
			StockProductDJCustomer n = new StockProductDJCustomer();

			
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = DateTime.Today;
			//n.Nomor = Generate_Nomor(n);

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;

			//User-Info
			n.Operator = App.UserNama;
			n.OperatorTgl = DateTime.Now;

            //n.ImgPicture = "D:\\Images\\StockReceiveDJ\\" + Generate_Nomor(n) + ".jpg";

			//Commit
			db.StockProductDJCustomer.InsertOnSubmit(n);
			db.SubmitChanges();

            Response.Redirect("Titipan_CustomerCharProduct.aspx?id=" + n.ID + "&idtitipan=" + myTitipanID);
	}
	
}