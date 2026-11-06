using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_GJ : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            DLC.LocationList(cabang, tipe);

            //Generate
            fillFilter();
            
		}
        if (App.Refresh(this)) fillTable(sender, e);
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        var rs = from p in db.StockProductGJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        if(kw.Text!="")
            rs = rs.Where(p=>p.Nomor==""+kw.Text+"");
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0) rs = rs.Where(p => p.StockActualGJ.IDLokasi == Cabang && p.StockActualGJ.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualGJ.TipeLokasi == tipe);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductGJ_CharProduct.ProductCategory == ProductCategory);

        rs = rs.Where(p => p.StockActualGJ.InHand == 1);

        if(orderby.SelectedValue=="1")
            rs.OrderBy(p=>p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.StockProductGJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.StockProductGJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.StockProductGJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int index = 1;

            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                CheckBox cb = new CheckBox();

                cb.ID = "cb_po" + index.ToString();
                cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                HiddenField hf = new HiddenField();
                hf.ID = "hf" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(cb);
                td.Controls.Add(hf);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor!=null) ? r.ProtoNomor : "";

                

                td = new TableCell();
                td.Text = "<small>" + KodeKaret + "</small>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_PricingMU.HargaUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;

            }

            
        }


    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            orderby.SelectedValue = Request.QueryString["orderby"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&tipe=" + tipe;
        href += "&order=" + orderby.SelectedValue;

        Response.Redirect(href);
    }

    protected void Save(object sender, EventArgs e)
    {
        int index = 1;

        CheckBox cb = new CheckBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                cb = (CheckBox)row.FindControl("cb_po" + index.ToString());
                if (cb.Checked == true)
                {
                    var d = db.StockProductGJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf) && p.StockActualGJ.InHand==1 && p.ReturnProduct == false && p.StatusPenjualan == 0);
                    if (d == null) App.DataNull(this);
                    else
                    {
                        var x = db.RevalItemGJ.SingleOrDefault(p => p.IDProduct == d.ID);
                        if(x==null)
                        {

                            RevalItemGJ rv = new RevalItemGJ();

                            rv.IDProduct = d.ID;
                            db.RevalItemGJ.InsertOnSubmit(rv);
                            db.SubmitChanges();


                            RevalProductGJ prod = new RevalProductGJ();
                            prod.IDReval = rv.ID;
                            prod.IDWarehouse = d.IDWarehouse;
                            prod.Sumber = d.Sumber;
                            prod.Keterangan = d.Keterangan;
                            prod.Tgl = d.Tgl;
                            prod.Nomor = d.Nomor;
                            prod.Draft = d.Draft;
                            prod.DraftDate = d.DraftDate;
                            prod.Proto = d.Proto;
                            prod.ProtoNomor = d.ProtoNomor;
                            prod.ProductOrder = d.ProductOrder;
                            prod.ProductOrderNomor = d.ProductOrderNomor;
                            prod.Supplier = d.Supplier;
                            prod.SupplierNama = d.SupplierNama;
                            prod.SupplierNomor = d.SupplierNomor;
                            prod.RequestOutlet = d.RequestOutlet;
                            prod.RequestOutletNama = d.RequestOutletNama;
                            prod.RequestCustomer = d.RequestCustomer;
                            prod.RequestCustomerNama = d.RequestCustomerNama;
                            prod.HumanDesigner = d.HumanDesigner;
                            prod.HumanStock = d.HumanStock;
                            prod.HumanStockNama = d.HumanStockNama;
                            prod.ExpiredProduct = d.ExpiredProduct;
                            prod.ExpiredRequest = d.ExpiredRequest;
                            //prod.Operator = d.Operator;
                            //prod.OperatorTgl = d.OperatorTgl;
                            prod.Operator = App.UserNama;
                            prod.OperatorTgl = DateTime.Now;
                            prod.Approval = d.Approval;
                            prod.ApprovalTgl = d.ApprovalTgl;
                            prod.CatatanManager = d.CatatanManager;
                            prod.ImgPicture = d.ImgPicture;

                            db.RevalProductGJ.InsertOnSubmit(prod);
                            db.SubmitChanges();

                            int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM RevalProductGJ ORDER BY ID DESC", Db.Cnn_DB);

                            RevalProductGJ_CharDesign dsg = new RevalProductGJ_CharDesign();
                            dsg.ID = ProdID;
                            dsg.DesignCategory = d.StockProductGJ_CharDesign.DesignCategory;
                            dsg.DesignConcept = d.StockProductGJ_CharDesign.DesignConcept;
                            dsg.DesignProcess = d.StockProductGJ_CharDesign.DesignProcess;
                            dsg.TargetAge = d.StockProductGJ_CharDesign.TargetAge;
                            dsg.TargetGender = d.StockProductGJ_CharDesign.TargetGender;
                            dsg.Naming = d.StockProductGJ_CharDesign.Naming;
                            dsg.SetItem = d.StockProductGJ_CharDesign.SetItem;
                            dsg.Special = d.StockProductGJ_CharDesign.Special;
                            dsg.Grafir = d.StockProductGJ_CharDesign.Grafir;

                            db.RevalProductGJ_CharDesign.InsertOnSubmit(dsg);
                            db.SubmitChanges();

                            RevalProductGJ_CharProduct pd = new RevalProductGJ_CharProduct();
                            pd.ID = ProdID;
                            pd.ProductItem = d.StockProductGJ_CharProduct.ProductItem;
                            pd.ProductCategory = d.StockProductGJ_CharProduct.ProductCategory;
                            pd.ProductLevel = d.StockProductGJ_CharProduct.ProductLevel;
                            pd.StoneDist = d.StockProductGJ_CharProduct.StoneDist;
                            pd.FrameColor = d.StockProductGJ_CharProduct.FrameColor;
                            pd.FrameFinishing = d.StockProductGJ_CharProduct.FrameFinishing;
                            pd.FrameMaterial = d.StockProductGJ_CharProduct.FrameMaterial;
                            pd.ProcessCons = d.StockProductGJ_CharProduct.ProcessCons;
                            pd.GrossWeight = d.StockProductGJ_CharProduct.GrossWeight;
                            pd.StoneCarat = d.StockProductGJ_CharProduct.StoneCarat;
                            pd.StoneWeight = d.StockProductGJ_CharProduct.StoneWeight;
                            pd.NettoWeight = d.StockProductGJ_CharProduct.NettoWeight;
                            pd.KadarLogam = d.StockProductGJ_CharProduct.KadarLogam;
                            pd.KadarTukaran = d.StockProductGJ_CharProduct.KadarTukaran;
                            pd.DimensiD = d.StockProductGJ_CharProduct.DimensiD;
                            pd.DimensiL = d.StockProductGJ_CharProduct.DimensiL;
                            pd.DimensiP = d.StockProductGJ_CharProduct.DimensiP;
                            pd.DimensiR = d.StockProductGJ_CharProduct.DimensiR;
                            pd.StoneQty = d.StockProductGJ_CharProduct.StoneQty;

                            db.RevalProductGJ_CharProduct.InsertOnSubmit(pd);
                            db.SubmitChanges();

                            RevalProductGJ_Costing cost = new RevalProductGJ_Costing();
                            cost.ID = ProdID;
                            cost.UnitPrice = d.StockProductGJ_Costing.UnitPrice;
                            cost.Frame = d.StockProductGJ_Costing.Frame;
                            cost.MountingNO = d.StockProductGJ_Costing.MountingNO;
                            cost.MountingRO = d.StockProductGJ_Costing.MountingRO;
                            cost.Pembagi = d.StockProductGJ_Costing.Pembagi;
                            cost.Setting = d.StockProductGJ_Costing.Setting;
                            cost.Finishing = d.StockProductGJ_Costing.Finishing;
                            cost.OngkosLainCOGS = d.StockProductGJ_Costing.OngkosLainCOGS;
                            cost.OngkosLain = d.StockProductGJ_Costing.OngkosLain;
                            cost.TotalRate = d.StockProductGJ_Costing.TotalRate;
                            cost.TotalRupiah = d.StockProductGJ_Costing.TotalRupiah;
                            cost.TotalBiaya = d.StockProductGJ_Costing.TotalBiaya;

                            db.RevalProductGJ_Costing.InsertOnSubmit(cost);
                            db.SubmitChanges();

                            RevalProductGJ_CostingProduct cp = new RevalProductGJ_CostingProduct();
                            cp.ID = ProdID;
                            cp.GoldRate = d.StockProductGJ_CostingProduct.GoldRate;
                            cp.Netto = d.StockProductGJ_CostingProduct.Netto;
                            cp.Kadar = d.StockProductGJ_CostingProduct.Kadar;
                            cp.TotalProduct = d.StockProductGJ_CostingProduct.TotalProduct;
                            cp.TotalStone1A = d.StockProductGJ_CostingProduct.TotalStone1A;
                            cp.TotalStone1B = d.StockProductGJ_CostingProduct.TotalStone1B;
                            cp.TotalStone2 = d.StockProductGJ_CostingProduct.TotalStone2;
                            cp.TotalStone3 = d.StockProductGJ_CostingProduct.TotalStone3;
                            cp.TotalStone4 = d.StockProductGJ_CostingProduct.TotalStone4;
                            cp.TotalStone5 = d.StockProductGJ_CostingProduct.TotalStone5;
                            cp.Total = d.StockProductGJ_CostingProduct.Total;
                            cp.EfektifGold = d.StockProductGJ_CostingProduct.EfektifGold;

                            db.RevalProductGJ_CostingProduct.InsertOnSubmit(cp);
                            db.SubmitChanges();

                            var l_fns = from p in db.StockProductGJ_Finishing
                                        where p.IDForm == d.ID
                                        select p;
                            if (l_fns != null)
                            {
                                foreach (var r in l_fns)
                                {
                                    RevalProductGJ_Finishing fns = new RevalProductGJ_Finishing();
                                    fns.IDForm = ProdID;
                                    fns.IDFinishing = r.IDFinishing;
                                    fns.Biaya = r.Biaya;
                                    fns.BiayaModal = r.BiayaModal;
                                    fns.Efektif = r.Efektif;
                                    db.RevalProductGJ_Finishing.InsertOnSubmit(fns);
                                    db.SubmitChanges();
                                }
                            }

                            RevalProductGJ_PricingBiaya by = new RevalProductGJ_PricingBiaya();
                            by.ID = ProdID;
                            by.NamaBrand = d.StockProductGJ_PricingBiaya.NamaBrand;
                            by.NamaItem = d.StockProductGJ_PricingBiaya.NamaItem;
                            by.NamaCons = d.StockProductGJ_PricingBiaya.NamaCons;
                            by.Mounting = d.StockProductGJ_PricingBiaya.Mounting;
                            by.MountingNO = d.StockProductGJ_PricingBiaya.MountingNO;
                            by.MountingRO = d.StockProductGJ_PricingBiaya.MountingRO;
                            by.Setting = d.StockProductGJ_PricingBiaya.Setting;
                            by.Finishing = d.StockProductGJ_PricingBiaya.Finishing;
                            by.TotalRupiah = d.StockProductGJ_PricingBiaya.TotalRupiah;
                            by.TotalRate = d.StockProductGJ_PricingBiaya.TotalRate;
                            by.Total = d.StockProductGJ_PricingBiaya.Total;
                            by.EfektifRate = d.StockProductGJ_PricingBiaya.EfektifRate;
                            by.EfektifMounting = d.StockProductGJ_PricingBiaya.EfektifMounting;
                            by.EfektifSetting = d.StockProductGJ_PricingBiaya.EfektifSetting;

                            db.RevalProductGJ_PricingBiaya.InsertOnSubmit(by);
                            db.SubmitChanges();

                            RevalProductGJ_PricingMU mu = new RevalProductGJ_PricingMU();
                            mu.ID = ProdID;
                            mu.NamaCategory = d.StockProductGJ_PricingMU.NamaCategory;
                            mu.NamaLevel = d.StockProductGJ_PricingMU.NamaLevel;
                            mu.NamaDist = d.StockProductGJ_PricingMU.NamaDist;
                            mu.ModalUSD = d.StockProductGJ_PricingMU.ModalUSD;
                            mu.ModalRate = d.StockProductGJ_PricingMU.ModalRate;
                            mu.ModalRupiah = d.StockProductGJ_PricingMU.ModalRupiah;
                            mu.MU = d.StockProductGJ_PricingMU.MU;
                            mu.HargaUSD = d.StockProductGJ_PricingMU.HargaUSD;
                            mu.HargaRupiah = d.StockProductGJ_PricingMU.HargaRupiah;
                            mu.HargaRupiahRound = d.StockProductGJ_PricingMU.HargaRupiahRound;
                            mu.EfektifRate = d.StockProductGJ_PricingMU.EfektifRate;

                            db.RevalProductGJ_PricingMU.InsertOnSubmit(mu);
                            db.SubmitChanges();

                            RevalProductGJ_PricingProduct prc = new RevalProductGJ_PricingProduct();
                            prc.ID = ProdID;
                            prc.GoldRate = d.StockProductGJ_PricingProduct.GoldRate;
                            prc.Netto = d.StockProductGJ_PricingProduct.Netto;
                            prc.Kadar = d.StockProductGJ_PricingProduct.Kadar;
                            prc.TotalProduct = d.StockProductGJ_PricingProduct.TotalProduct;
                            prc.TotalStone1A = d.StockProductGJ_PricingProduct.TotalStone1A;
                            prc.TotalStone1B = d.StockProductGJ_PricingProduct.TotalStone1B;
                            prc.TotalStone2 = d.StockProductGJ_PricingProduct.TotalStone2;
                            prc.TotalStone3 = d.StockProductGJ_PricingProduct.TotalStone3;
                            prc.TotalStone4 = d.StockProductGJ_PricingProduct.TotalStone4;
                            prc.TotalStone5 = d.StockProductGJ_PricingProduct.TotalStone5;
                            prc.Total = d.StockProductGJ_PricingProduct.Total;
                            prc.EfektifGold = d.StockProductGJ_PricingProduct.EfektifGold;

                            db.RevalProductGJ_PricingProduct.InsertOnSubmit(prc);
                            db.SubmitChanges();

                            RevalProductGJ_PricingSegmen sg = new RevalProductGJ_PricingSegmen();
                            sg.ID = ProdID;
                            sg.ProductSegmen = d.StockProductGJ_PricingSegmen.ProductSegmen;
                            sg.NamaCategory = d.StockProductGJ_PricingSegmen.NamaCategory;
                            sg.NamaLevel = d.StockProductGJ_PricingSegmen.NamaLevel;
                            sg.NamaDist = d.StockProductGJ_PricingSegmen.NamaDist;
                            sg.SegmenSistem = d.StockProductGJ_PricingSegmen.SegmenSistem;
                            sg.SegmenMinimum = d.StockProductGJ_PricingSegmen.SegmenMinimum;
                            sg.TotalCarat = d.StockProductGJ_PricingSegmen.TotalCarat;
                            sg.HargaUSD = d.StockProductGJ_PricingSegmen.HargaUSD;
                            sg.EfektifSegmen = d.StockProductGJ_PricingSegmen.EfektifSegmen;

                            db.RevalProductGJ_PricingSegmen.InsertOnSubmit(sg);
                            db.SubmitChanges();

                        }
                    }
                }
            }
            catch (Exception ex) { }

            index++;
        }
        Response.Redirect("GJ_Register.aspx?d=1");
    }
}