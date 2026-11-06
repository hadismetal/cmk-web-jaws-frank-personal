using System;
using System.Linq;

public partial class Page_Register_DJ_ProductAddSave : System.Web.UI.Page {
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
	protected int myIDProduct { get { return App.QueryInt(this, "idproduct"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			qty.Focus();

			//Number-Format
			App.NumberFormat(qty);

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("ProductAdjustment.aspx"); }
	protected void LinkError() { Response.Redirect("ProductAdjustmentInfo.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.AdjustmentSouvenir.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
	
		}
	}

	

	//Save
	protected bool valid {
		get {
			bool x = true;

            x = x && Fv.isDecimal(qty, qty_ERR);

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.AdjustmentSouvenir.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
			if (d == null) DataError();
			else {
				//Process
                AdjustmentSouvenir_Product n = new AdjustmentSouvenir_Product();

                n.IDForm = myID;
                n.IDProduct = myIDProduct;

                n.Qty = Convert.ToDecimal(qty.Text);
                

                //Commit
                db.AdjustmentSouvenir_Product.InsertOnSubmit(n);
                db.SubmitChanges();

				//Redirect
				Response.Redirect("ProductAdjustmentInfo.aspx?id=" + myID);
			}
		}
	}
    protected void cancel_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("ProductAdjustmentInfo.aspx?id=" + myID);
    }
}