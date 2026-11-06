using System;
using System.Linq;

public partial class Page_Register_SPK_CharProduct01 : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			productitem.Focus();

			//DLC
			DLC.CharProductItem(productitem);
			DLC.CharProductCategory(productcategory);
			DLC.CharProductLevel(productlevel);
			DLC.CharFrameMaterial(framematerial);
			DLC.CharFrameFinishing(framefinishing);
			DLC.CharFrameColor(framecolor);
			DLC.CharProcessCons(processcons);

			//DLC-Control
			App.DropDown(save, productitem, productitem_ERR);
			App.DropDown(save, productcategory, productcategory_ERR);
			App.DropDown(save, productlevel, productlevel_ERR);
			App.DropDown(save, framematerial, framematerial_ERR);
			App.DropDown(save, framefinishing, framefinishing_ERR);
			App.DropDown(save, framecolor, framecolor_ERR);
			App.DropDown(save, processcons, processcons_ERR);

            //Numberformat
            App.NumberFormat(komponen);
            App.NumberFormat(poin);
            App.NumberFormat(bebansusut);
            App.NumberFormat(pk1);
            App.NumberFormat(pk2);
            App.NumberFormat(pk3);
            App.NumberFormat(pk4);
            App.NumberFormat(pk5);
            App.NumberFormat(pk6);

			//Form
			fillForm();
			fillDist(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx"); }

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
            var dsg = db.ProductDesignGJ_CharProduct.SingleOrDefault(p => p.ID == d.IDProductDesign);
            if (dsg == null) DataError();
            else
            {
                productitem.SelectedValue = dsg.ProductItem.ToString();
                productcategory.SelectedValue = dsg.ProductCategory.ToString();
                productlevel.SelectedValue = dsg.ProductLevel.ToString();
                framematerial.SelectedValue = dsg.FrameMaterial.ToString();
                framefinishing.SelectedValue = dsg.FrameFinishing.ToString();
                framecolor.SelectedValue = dsg.FrameColor.ToString();
                processcons.SelectedValue = dsg.ProcessCons.ToString();
            }
        }
        else if (tipe == 2)
        {
            var ro = db.KodeKaretGJ_CharProduct.SingleOrDefault(p => p.ID == d.IDKodeKaret);
            if (ro == null) DataError();
            else
            {
                productitem.SelectedValue = ro.ProductItem.ToString();
                productcategory.SelectedValue = ro.ProductCategory.ToString();
                productlevel.SelectedValue = ro.ProductLevel.ToString();
                framematerial.SelectedValue = ro.FrameMaterial.ToString();
                framefinishing.SelectedValue = ro.FrameFinishing.ToString();
                framecolor.SelectedValue = ro.FrameColor.ToString();
                processcons.SelectedValue = ro.ProcessCons.ToString();
                bebansusut.Text = Convert.ToDecimal(ro.BebanSusut).ToString("N2");
                komponen.Text = Convert.ToDecimal(ro.Komponen).ToString("N2");
                poin.Text = Convert.ToDecimal(ro.PoinKomponen).ToString("N2");
                komponen1.Text = ro.KodeKomponen1;
                komponen2.Text = ro.KodeKomponen2;
                komponen3.Text = ro.KodeKomponen3;
                komponen4.Text = ro.KodeKomponen4;
                komponen5.Text = ro.KodeKomponen5;
                komponen6.Text = ro.KodeKomponen6;
                pk1.Text = Convert.ToDecimal(ro.JlhKomponen1).ToString("N2");
                pk2.Text = Convert.ToDecimal(ro.JlhKomponen2).ToString("N2");
                pk3.Text = Convert.ToDecimal(ro.JlhKomponen3).ToString("N2");
                pk4.Text = Convert.ToDecimal(ro.JlhKomponen4).ToString("N2");
                pk5.Text = Convert.ToDecimal(ro.JlhKomponen5).ToString("N2");
                pk6.Text = Convert.ToDecimal(ro.JlhKomponen6).ToString("N2");
            }
        }
	}
	protected void fillDist(object sender, EventArgs e) {
		//Clear-Selection
		stonedist_ERR.Visible = false;
		stonedist.ClearSelection();
		stonedist.Items.Clear();

		//DLC
		if (productcategory.Items.Count > 0 && productlevel.Items.Count > 0) {
			int IDCategory = Convert.ToInt32(productcategory.SelectedValue);
			int IDLevel = Convert.ToInt32(productlevel.SelectedValue);
			DLC.SettingsStoneDist(stonedist, IDCategory, IDLevel);
		}
	}

	//Save
	protected bool valid_Dist {
		get {
			bool x = stonedist.Items.Count > 0;
			stonedist_ERR.Visible = !x;
			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid_Dist) {
			var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				//Process
				deleteProcess(d);
				insertProcess(d);

				//Redirect
				Response.Redirect("SPK_CharProduct02.aspx?id=" + myID + "&tipe=" + tipe);
			}
		}
	}
	protected void deleteProcess(ProductOrderGJ ms) {
		var d = ms.ProductOrderGJ_CharProduct;
		if (d != null) {
			//Commit
			db.ProductOrderGJ_CharProduct.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
	protected void insertProcess(ProductOrderGJ ms) {
		ProductOrderGJ_CharProduct n = new ProductOrderGJ_CharProduct();

		n.ID = ms.ID;

		n.ProductItem = Convert.ToInt32(productitem.SelectedValue);
		n.ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
		n.ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
		n.StoneDist = Convert.ToInt32(stonedist.SelectedValue);
		n.FrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
		n.FrameFinishing = Convert.ToInt32(framefinishing.SelectedValue);
		n.FrameColor = Convert.ToInt32(framecolor.SelectedValue);
		n.ProcessCons = Convert.ToInt32(processcons.SelectedValue);
        n.KodeKomponen1 = komponen1.Text;
        n.KodeKomponen2 = komponen2.Text;
        n.KodeKomponen3 = komponen3.Text;
        n.KodeKomponen4 = komponen4.Text;
        n.KodeKomponen5 = komponen5.Text;
        n.KodeKomponen6 = komponen6.Text;
        n.JlhKomponen1 = (pk1.Text!="") ? Convert.ToDecimal(pk1.Text) : 0; 
        n.JlhKomponen2 = (pk2.Text!="") ? Convert.ToDecimal(pk2.Text) : 0; 
        n.JlhKomponen3 = (pk3.Text!="") ? Convert.ToDecimal(pk3.Text) : 0; 
        n.JlhKomponen4 = (pk4.Text!="") ? Convert.ToDecimal(pk4.Text) : 0; 
        n.JlhKomponen5 = (pk5.Text!="") ? Convert.ToDecimal(pk5.Text) : 0; 
        n.JlhKomponen6 = (pk6.Text!="") ? Convert.ToDecimal(pk6.Text) : 0; 
        n.Komponen = (komponen.Text!="") ? Convert.ToDecimal(komponen.Text) : 0;
        n.PoinKomponen = (poin.Text != "") ? Convert.ToDecimal(poin.Text) : 0;
        n.BebanSusut = (bebansusut.Text != "") ? Convert.ToDecimal(bebansusut.Text) : 0; 

		//Commit
		db.ProductOrderGJ_CharProduct.InsertOnSubmit(n);
		db.SubmitChanges();
	}
}