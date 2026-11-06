using System;
using System.Linq;
using System.Web.UI.WebControls;

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

        if (!IsPostBack)
        {
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
            //fillDist(sender, e);

            //if (status == 1)
            //{
            //    LoadDist(sender, e);
            //}
            //else
            //{

            //}

        }
        
	}

	//Exception
    protected void DataError(string errorcode = "000") { Response.Redirect("SPK_Data02.aspx?id=" + myID + "&tipe=" + tipe + "&err=" + errorcode); }

	//Form
	protected void fillForm()
    {
        var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError("001");
        else
        {
            if (tipe == 1)
            {
                var dsg = db.ProductDesign_CharProduct.SingleOrDefault(p => p.ID == d.IDProductDesign);
                if (dsg == null) DataError("002");
                else
                {
                    productitem.SelectedValue = dsg.ProductItem.ToString();
                    productcategory.SelectedValue = dsg.ProductCategory.ToString();
                    productlevel.SelectedValue = dsg.ProductLevel.ToString();
                    framematerial.SelectedValue = dsg.FrameMaterial.ToString();
                    framefinishing.SelectedValue = dsg.FrameFinishing.ToString();
                    framecolor.SelectedValue = dsg.FrameColor.ToString();
                    processcons.SelectedValue = dsg.ProcessCons.ToString();

                    fillDistInit(dsg);
                }
            }
            else if (tipe == 2)
            {
                var ro_kkp = db.ProductOrder.Where(p => p.KodeKaretLama == d.KodeKaretLama && p.Approval != null).OrderByDescending(p => p.ID).FirstOrDefault();
                var ro_kkj = db.ProductOrder.Where(p => p.KodeKaret == d.KodeKaret && p.Approval != null).OrderByDescending(p => p.ID).FirstOrDefault();
                var ro_kkm = db.ProductOrder.Where(p => p.KodeKaretDJ.Nomor == d.KodeKaret && p.Approval != null).OrderByDescending(p => p.ID).FirstOrDefault();

                if (ro_kkp != null)
                {
                    productitem.SelectedValue = ro_kkp.ProductOrder_CharProduct.ProductItem.ToString();
                    productcategory.SelectedValue = ro_kkp.ProductOrder_CharProduct.ProductCategory.ToString();
                    productlevel.SelectedValue = ro_kkp.ProductOrder_CharProduct.ProductLevel.ToString();
                    framematerial.SelectedValue = ro_kkp.ProductOrder_CharProduct.FrameMaterial.ToString();
                    framefinishing.SelectedValue = ro_kkp.ProductOrder_CharProduct.FrameFinishing.ToString();
                    framecolor.SelectedValue = ro_kkp.ProductOrder_CharProduct.FrameColor.ToString();
                    processcons.SelectedValue = ro_kkp.ProductOrder_CharProduct.ProcessCons.ToString();

                    fillDistInit(ro_kkp);

                    stonedist.SelectedValue = ro_kkp.ProductOrder_CharProduct.StoneDist.ToString();
                    bebansusut.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.BebanSusut).ToString("N2");
                    komponen.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.Komponen).ToString("N2");
                    poin.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.PoinKomponen).ToString("N2");
                    komponen1.Text = ro_kkp.ProductOrder_CharProduct.KodeKomponen1;
                    komponen2.Text = ro_kkp.ProductOrder_CharProduct.KodeKomponen2;
                    komponen3.Text = ro_kkp.ProductOrder_CharProduct.KodeKomponen3;
                    komponen4.Text = ro_kkp.ProductOrder_CharProduct.KodeKomponen4;
                    komponen5.Text = ro_kkp.ProductOrder_CharProduct.KodeKomponen5;
                    komponen6.Text = ro_kkp.ProductOrder_CharProduct.KodeKomponen6;
                    pk1.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.JlhKomponen1).ToString("N2");
                    pk2.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.JlhKomponen2).ToString("N2");
                    pk3.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.JlhKomponen3).ToString("N2");
                    pk4.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.JlhKomponen4).ToString("N2");
                    pk5.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.JlhKomponen5).ToString("N2");
                    pk6.Text = Convert.ToDecimal(ro_kkp.ProductOrder_CharProduct.JlhKomponen6).ToString("N2");

                }
                else if (ro_kkj != null)
                {
                    productitem.SelectedValue = ro_kkj.ProductOrder_CharProduct.ProductItem.ToString();
                    productcategory.SelectedValue = ro_kkj.ProductOrder_CharProduct.ProductCategory.ToString();
                    productlevel.SelectedValue = ro_kkj.ProductOrder_CharProduct.ProductLevel.ToString();
                    framematerial.SelectedValue = ro_kkj.ProductOrder_CharProduct.FrameMaterial.ToString();
                    framefinishing.SelectedValue = ro_kkj.ProductOrder_CharProduct.FrameFinishing.ToString();
                    framecolor.SelectedValue = ro_kkj.ProductOrder_CharProduct.FrameColor.ToString();
                    processcons.SelectedValue = ro_kkj.ProductOrder_CharProduct.ProcessCons.ToString();

                    fillDistInit(ro_kkj);

                    stonedist.SelectedValue = ro_kkj.ProductOrder_CharProduct.StoneDist.ToString();
                    bebansusut.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.BebanSusut).ToString("N2");
                    komponen.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.Komponen).ToString("N2");
                    poin.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.PoinKomponen).ToString("N2");
                    komponen1.Text = ro_kkj.ProductOrder_CharProduct.KodeKomponen1;
                    komponen2.Text = ro_kkj.ProductOrder_CharProduct.KodeKomponen2;
                    komponen3.Text = ro_kkj.ProductOrder_CharProduct.KodeKomponen3;
                    komponen4.Text = ro_kkj.ProductOrder_CharProduct.KodeKomponen4;
                    komponen5.Text = ro_kkj.ProductOrder_CharProduct.KodeKomponen5;
                    komponen6.Text = ro_kkj.ProductOrder_CharProduct.KodeKomponen6;
                    pk1.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.JlhKomponen1).ToString("N2");
                    pk2.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.JlhKomponen2).ToString("N2");
                    pk3.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.JlhKomponen3).ToString("N2");
                    pk4.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.JlhKomponen4).ToString("N2");
                    pk5.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.JlhKomponen5).ToString("N2");
                    pk6.Text = Convert.ToDecimal(ro_kkj.ProductOrder_CharProduct.JlhKomponen6).ToString("N2");

                }
                else if (ro_kkm != null)
                {
                    productitem.SelectedValue = ro_kkm.ProductOrder_CharProduct.ProductItem.ToString();
                    productcategory.SelectedValue = ro_kkm.ProductOrder_CharProduct.ProductCategory.ToString();
                    productlevel.SelectedValue = ro_kkm.ProductOrder_CharProduct.ProductLevel.ToString();
                    framematerial.SelectedValue = ro_kkm.ProductOrder_CharProduct.FrameMaterial.ToString();
                    framefinishing.SelectedValue = ro_kkm.ProductOrder_CharProduct.FrameFinishing.ToString();
                    framecolor.SelectedValue = ro_kkm.ProductOrder_CharProduct.FrameColor.ToString();
                    processcons.SelectedValue = ro_kkm.ProductOrder_CharProduct.ProcessCons.ToString();

                    fillDistInit(ro_kkm);

                    stonedist.SelectedValue = ro_kkm.ProductOrder_CharProduct.StoneDist.ToString();
                    bebansusut.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.BebanSusut).ToString("N2");
                    komponen.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.Komponen).ToString("N2");
                    poin.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.PoinKomponen).ToString("N2");
                    komponen1.Text = ro_kkm.ProductOrder_CharProduct.KodeKomponen1;
                    komponen2.Text = ro_kkm.ProductOrder_CharProduct.KodeKomponen2;
                    komponen3.Text = ro_kkm.ProductOrder_CharProduct.KodeKomponen3;
                    komponen4.Text = ro_kkm.ProductOrder_CharProduct.KodeKomponen4;
                    komponen5.Text = ro_kkm.ProductOrder_CharProduct.KodeKomponen5;
                    komponen6.Text = ro_kkm.ProductOrder_CharProduct.KodeKomponen6;
                    pk1.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.JlhKomponen1).ToString("N2");
                    pk2.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.JlhKomponen2).ToString("N2");
                    pk3.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.JlhKomponen3).ToString("N2");
                    pk4.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.JlhKomponen4).ToString("N2");
                    pk5.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.JlhKomponen5).ToString("N2");
                    pk6.Text = Convert.ToDecimal(ro_kkm.ProductOrder_CharProduct.JlhKomponen6).ToString("N2");

                }
                else
                {
                    DataError("003");
                }
            }
        }

	}

    protected void fillDistInit(ProductDesign_CharProduct d)
    {
        //DLC
        if (productcategory.Items.Count > 0 && productlevel.Items.Count > 0)
        {
            int IDCategory = Convert.ToInt32(productcategory.SelectedValue);
            int IDLevel = Convert.ToInt32(productlevel.SelectedValue);
            //DLC.SettingsStoneDist(stonedist, IDCategory, IDLevel);

            var cek = from p in db.SettingsStoneDist
                      where
                         p.IDCategory == IDCategory &&
                         p.IDLevel == IDLevel &&
                         p.CharStoneDist.Disable == false
                      orderby p.CharStoneDist.SN
                      select p;

            foreach (var r in cek)
            {
                string t = r.CharStoneDist.Nama + " (" + r.CharStoneDist.NamaKode + ")";
                string v = r.IDDist.ToString();
                stonedist.Items.Add(new ListItem(t, v));
            }
        }

        stonedist.SelectedValue = d.StoneDist.ToString();
    }

    protected void fillDistInit(ProductOrder d)
    {
        //DLC
        if (productcategory.Items.Count > 0 && productlevel.Items.Count > 0)
        {
            int IDCategory = Convert.ToInt32(productcategory.SelectedValue);
            int IDLevel = Convert.ToInt32(productlevel.SelectedValue);
            //DLC.SettingsStoneDist(stonedist, IDCategory, IDLevel);

            var cek = from p in db.SettingsStoneDist
                      where
                         p.IDCategory == IDCategory &&
                         p.IDLevel == IDLevel &&
                         p.CharStoneDist.Disable == false
                      orderby p.CharStoneDist.SN
                      select p;

            foreach (var r in cek)
            {
                string t = r.CharStoneDist.Nama + " (" + r.CharStoneDist.NamaKode + ")";
                string v = r.IDDist.ToString();
                stonedist.Items.Add(new ListItem(t, v));
            }
        }

        stonedist.SelectedValue = d.ProductOrder_CharProduct.StoneDist.ToString();
    }

	protected void fillDist(object sender, EventArgs e)
    {
		//Clear-Selection
		stonedist_ERR.Visible = false;
		stonedist.ClearSelection();
		stonedist.Items.Clear();

		//DLC
		if (productcategory.Items.Count > 0 && productlevel.Items.Count > 0)
        {
            int IDCategory = Convert.ToInt32(productcategory.SelectedValue);
			int IDLevel = Convert.ToInt32(productlevel.SelectedValue);
            //DLC.SettingsStoneDist(stonedist, IDCategory, IDLevel);

            var cek = from p in db.SettingsStoneDist
                     where
                        p.IDCategory == IDCategory &&
                        p.IDLevel == IDLevel &&
                        p.CharStoneDist.Disable == false
                     orderby p.CharStoneDist.SN
                     select p;

            foreach (var r in cek)
            {
                string t = r.CharStoneDist.Nama + " (" + r.CharStoneDist.NamaKode + ")";
                string v = r.IDDist.ToString();
                stonedist.Items.Add(new ListItem(t, v));
            }
        }
        //lbstatus.Text = "2";   
	}

    protected void LoadDist(object sender, EventArgs e)
    {
        //Clear-Selection
        stonedist_ERR.Visible = false;
        stonedist.ClearSelection();
        stonedist.Items.Clear();

        //DLC
        if (productcategory.Items.Count > 0 && productlevel.Items.Count > 0)
        {
            if (tipe == 2)
            {
                var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
                if (d != null)
                {
                    var ro = db.ProductOrder.OrderByDescending(p => p.ID).FirstOrDefault(p => p.KodeKaret == d.KodeKaret && p.Approval != null);
                    productcategory.SelectedValue = ro.ProductOrder_CharProduct.ProductCategory.ToString();
                    productlevel.SelectedValue = ro.ProductOrder_CharProduct.ProductLevel.ToString();
                    stonedist.SelectedValue = ro.ProductOrder_CharProduct.StoneDist.ToString();
                }
            }
            int IDCategory = Convert.ToInt32(productcategory.SelectedValue);
            int IDLevel = Convert.ToInt32(productlevel.SelectedValue);
            //DLC.SettingsStoneDist(stonedist, IDCategory, IDLevel);

            var cek = from p in db.SettingsStoneDist
                        where
                            p.IDCategory == IDCategory &&
                            p.IDLevel == IDLevel &&
                            p.CharStoneDist.Disable == false
                        orderby p.CharStoneDist.SN
                        select p;

            foreach (var r in cek)
            {
                string t = r.CharStoneDist.Nama + " (" + r.CharStoneDist.NamaKode + ")";
                string v = r.IDDist.ToString();
                stonedist.Items.Add(new ListItem(t, v));
            }
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
	protected void save_Click(object sender, EventArgs e)
    {
		if (valid_Dist)
        {
			var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) DataError("004");
			else
            {
				//Process
				deleteProcess(d);
				insertProcess(d);
				//Redirect
				Response.Redirect("SPK_CharProduct02.aspx?id=" + myID + "&tipe=" + tipe);
			}
		}
	}
	protected void deleteProcess(ProductOrder ms) {
		var d = ms.ProductOrder_CharProduct;
		if (d != null) {
			//Commit
			db.ProductOrder_CharProduct.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
	protected void insertProcess(ProductOrder ms)
    {
		ProductOrder_CharProduct n = new ProductOrder_CharProduct();

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
		db.ProductOrder_CharProduct.InsertOnSubmit(n);
		db.SubmitChanges();
	}
}