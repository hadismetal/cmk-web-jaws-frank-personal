using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_SPK_CharDesign02 : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this,"tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			naming.Focus();

			

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx"); }
	protected void LinkError() { Response.Redirect("SPK_CharDesign01.aspx?id=" + myID + "&tipe=" + tipe); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
            if (tipe == 1)
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.ProductDesignGJ.Nomor + "</a>";
            }
            else
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.KodeKaretGJ.Nomor + "</a>";
            }
		}
        if (tipe == 1)
        {
            var dsg = db.ProductDesignGJ_CharDesign.SingleOrDefault(p => p.ID == d.IDProductDesign);
            if (dsg == null) DataError();
            else
            {
                naming.Text = dsg.Naming;
                setitem.Checked = (dsg.SetItem == true) ? true : false;
                special.Checked = (dsg.Special == true) ? true : false;
                grafir.Text = dsg.Grafir;

                //DLC
                DLC.CharProcessFinishingSPKGJ(cb, Convert.ToInt32(d.IDProductDesign));
            }
        }
        else
        {
            var dsg = db.KodeKaretGJ_CharDesign.SingleOrDefault(p => p.ID == d.IDKodeKaret);
            if (dsg == null) DataError();
            else
            {
                naming.Text = dsg.Naming;
                setitem.Checked = (dsg.SetItem == true) ? true : false;
                special.Checked = (dsg.Special == true) ? true : false;
                grafir.Text = dsg.Grafir;

                //DLC
                DLC.CharProcessFinishingSPKKaretGJ(cb, Convert.ToInt32(d.IDKodeKaret));
            }
        }
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {

            var product = d.ProductOrderGJ_CharProduct;

			//Process
			saveData(d);
			saveFinishing(d);

			//Redirect
			Response.Redirect("SPK_Submit.aspx?id=" + myID + "&tipe=" + tipe);
		}
	}

	//Data
	protected void saveData(ProductOrderGJ ms) {
		var d = ms.ProductOrderGJ_CharDesign;
		if (d == null) LinkError();
		else {
			d.Naming = Str.CapTitle(naming.Text);
            d.Grafir = grafir.Text; // Str.CapUpper(grafir.Text);

			d.SetItem = setitem.Checked;
			d.Special = special.Checked;

			//Commit
			db.SubmitChanges();
		}
	}

	//Finishing
	protected void saveFinishing(ProductOrderGJ d) {
		var rs = from p in cb.Items.Cast<ListItem>()
				 select p;

		foreach (var r in rs) {
			int myIDFinishing = Convert.ToInt32(r.Value);
			if (r.Selected)
				insertProcess(d, myIDFinishing);
			else
				deleteProcess(d, myIDFinishing);
		}
	}
	protected void insertProcess(ProductOrderGJ ms, int myIDFinishing) {
		var d = ms.ProductOrderGJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
		if (d == null) {
			ProductOrderGJ_Finishing n = new ProductOrderGJ_Finishing();

			n.IDForm = ms.ID;
			n.IDFinishing = myIDFinishing;

			//Commit
			db.ProductOrderGJ_Finishing.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected void deleteProcess(ProductOrderGJ ms, int myIDFinishing) {
		var d = ms.ProductOrderGJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
		if (d != null) {
			//Commit
			db.ProductOrderGJ_Finishing.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
}