using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page {
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

        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        if(kw.Text!="")
            rs = rs.Where(p=>p.Nomor==""+kw.Text+"");
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0) rs = rs.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        rs = rs.Where(p => p.StockActualDJ.InHand == 1);

        if(orderby.SelectedValue=="1")
            rs.OrderBy(p=>p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);

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
                td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                var det = (from p in db.StockProductDJ_Stone1A
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.StockProductDJ_Stone1B
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.StockProductDJ_Stone2
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.StockProductDJ_Stone3
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.StockProductDJ_Stone4
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.StockProductDJ_Stone5
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                decimal TotalButir = 0m;
                try
                {
                    TotalButir = det.Sum(p => p.TotalButir);
                }
                catch (Exception ex) { }
                decimal TotalCarat = 0m;
                try
                {
                    TotalCarat = det.Sum(p => p.TotalCarat);
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = TotalButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharStoneDist.Nama;
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
                    var d = db.StockProductDJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf) && p.StockActualDJ.InHand==1 && p.ReturnProduct == false && p.StatusPenjualan == 0);
                    if (d == null) App.DataNull(this);
                    else
                    {
                        var x = db.RevalItemDJ.SingleOrDefault(p => p.IDProduct == d.ID);
                        if(x==null)
                        {

                            RevalItemDJ rv = new RevalItemDJ();

                            rv.IDProduct = d.ID;
                            db.RevalItemDJ.InsertOnSubmit(rv);
                            db.SubmitChanges();


                            RevalProductDJ prod = new RevalProductDJ();
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

                            db.RevalProductDJ.InsertOnSubmit(prod);
                            db.SubmitChanges();

                            int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM RevalProductDJ ORDER BY ID DESC", Db.Cnn_DB);

                            RevalProductDJ_CharDesign dsg = new RevalProductDJ_CharDesign();
                            dsg.ID = ProdID;
                            dsg.DesignCategory = d.StockProductDJ_CharDesign.DesignCategory;
                            dsg.DesignConcept = d.StockProductDJ_CharDesign.DesignConcept;
                            dsg.DesignProcess = d.StockProductDJ_CharDesign.DesignProcess;
                            dsg.TargetAge = d.StockProductDJ_CharDesign.TargetAge;
                            dsg.TargetGender = d.StockProductDJ_CharDesign.TargetGender;
                            dsg.Naming = d.StockProductDJ_CharDesign.Naming;
                            dsg.SetItem = d.StockProductDJ_CharDesign.SetItem;
                            dsg.Special = d.StockProductDJ_CharDesign.Special;
                            dsg.Grafir = d.StockProductDJ_CharDesign.Grafir;

                            db.RevalProductDJ_CharDesign.InsertOnSubmit(dsg);
                            db.SubmitChanges();

                            RevalProductDJ_CharProduct pd = new RevalProductDJ_CharProduct();
                            pd.ID = ProdID;
                            pd.ProductItem = d.StockProductDJ_CharProduct.ProductItem;
                            pd.ProductCategory = d.StockProductDJ_CharProduct.ProductCategory;
                            pd.ProductLevel = d.StockProductDJ_CharProduct.ProductLevel;
                            pd.StoneDist = d.StockProductDJ_CharProduct.StoneDist;
                            pd.FrameColor = d.StockProductDJ_CharProduct.FrameColor;
                            pd.FrameFinishing = d.StockProductDJ_CharProduct.FrameFinishing;
                            pd.FrameMaterial = d.StockProductDJ_CharProduct.FrameMaterial;
                            pd.ProcessCons = d.StockProductDJ_CharProduct.ProcessCons;
                            pd.GrossWeight = d.StockProductDJ_CharProduct.GrossWeight;
                            pd.StoneCarat = d.StockProductDJ_CharProduct.StoneCarat;
                            pd.StoneWeight = d.StockProductDJ_CharProduct.StoneWeight;
                            pd.NettoWeight = d.StockProductDJ_CharProduct.NettoWeight;
                            pd.KadarLogam = d.StockProductDJ_CharProduct.KadarLogam;
                            pd.KadarTukaran = d.StockProductDJ_CharProduct.KadarTukaran;
                            pd.DimensiD = d.StockProductDJ_CharProduct.DimensiD;
                            pd.DimensiL = d.StockProductDJ_CharProduct.DimensiL;
                            pd.DimensiP = d.StockProductDJ_CharProduct.DimensiP;
                            pd.DimensiR = d.StockProductDJ_CharProduct.DimensiR;
                            pd.StoneQty = d.StockProductDJ_CharProduct.StoneQty;

                            db.RevalProductDJ_CharProduct.InsertOnSubmit(pd);
                            db.SubmitChanges();

                            RevalProductDJ_Costing cost = new RevalProductDJ_Costing();
                            cost.ID = ProdID;
                            cost.UnitPrice = d.StockProductDJ_Costing.UnitPrice;
                            cost.Frame = d.StockProductDJ_Costing.Frame;
                            cost.MountingNO = d.StockProductDJ_Costing.MountingNO;
                            cost.MountingRO = d.StockProductDJ_Costing.MountingRO;
                            cost.Pembagi = d.StockProductDJ_Costing.Pembagi;
                            cost.Setting = d.StockProductDJ_Costing.Setting;
                            cost.Finishing = d.StockProductDJ_Costing.Finishing;
                            cost.OngkosLainCOGS = d.StockProductDJ_Costing.OngkosLainCOGS;
                            cost.OngkosLain = d.StockProductDJ_Costing.OngkosLain;
                            cost.TotalRate = d.StockProductDJ_Costing.TotalRate;
                            cost.TotalRupiah = d.StockProductDJ_Costing.TotalRupiah;
                            cost.TotalBiaya = d.StockProductDJ_Costing.TotalBiaya;

                            db.RevalProductDJ_Costing.InsertOnSubmit(cost);
                            db.SubmitChanges();

                            RevalProductDJ_CostingProduct cp = new RevalProductDJ_CostingProduct();
                            cp.ID = ProdID;
                            cp.GoldRate = d.StockProductDJ_CostingProduct.GoldRate;
                            cp.Netto = d.StockProductDJ_CostingProduct.Netto;
                            cp.Kadar = d.StockProductDJ_CostingProduct.Kadar;
                            cp.TotalProduct = d.StockProductDJ_CostingProduct.TotalProduct;
                            cp.TotalStone1A = d.StockProductDJ_CostingProduct.TotalStone1A;
                            cp.TotalStone1B = d.StockProductDJ_CostingProduct.TotalStone1B;
                            cp.TotalStone2 = d.StockProductDJ_CostingProduct.TotalStone2;
                            cp.TotalStone3 = d.StockProductDJ_CostingProduct.TotalStone3;
                            cp.TotalStone4 = d.StockProductDJ_CostingProduct.TotalStone4;
                            cp.TotalStone5 = d.StockProductDJ_CostingProduct.TotalStone5;
                            cp.Total = d.StockProductDJ_CostingProduct.Total;
                            cp.EfektifGold = d.StockProductDJ_CostingProduct.EfektifGold;

                            db.RevalProductDJ_CostingProduct.InsertOnSubmit(cp);
                            db.SubmitChanges();

                            var l_fns = from p in db.StockProductDJ_Finishing
                                        where p.IDForm == d.ID
                                        select p;
                            if (l_fns != null)
                            {
                                foreach (var r in l_fns)
                                {
                                    RevalProductDJ_Finishing fns = new RevalProductDJ_Finishing();
                                    fns.IDForm = ProdID;
                                    fns.IDFinishing = r.IDFinishing;
                                    fns.Biaya = r.Biaya;
                                    fns.BiayaModal = r.BiayaModal;
                                    fns.Efektif = r.Efektif;
                                    db.RevalProductDJ_Finishing.InsertOnSubmit(fns);
                                    db.SubmitChanges();
                                }
                            }

                            RevalProductDJ_PricingBiaya by = new RevalProductDJ_PricingBiaya();
                            by.ID = ProdID;
                            by.NamaBrand = d.StockProductDJ_PricingBiaya.NamaBrand;
                            by.NamaItem = d.StockProductDJ_PricingBiaya.NamaItem;
                            by.NamaCons = d.StockProductDJ_PricingBiaya.NamaCons;
                            by.Mounting = d.StockProductDJ_PricingBiaya.Mounting;
                            by.MountingNO = d.StockProductDJ_PricingBiaya.MountingNO;
                            by.MountingRO = d.StockProductDJ_PricingBiaya.MountingRO;
                            by.Setting = d.StockProductDJ_PricingBiaya.Setting;
                            by.Finishing = d.StockProductDJ_PricingBiaya.Finishing;
                            by.TotalRupiah = d.StockProductDJ_PricingBiaya.TotalRupiah;
                            by.TotalRate = d.StockProductDJ_PricingBiaya.TotalRate;
                            by.Total = d.StockProductDJ_PricingBiaya.Total;
                            by.EfektifRate = d.StockProductDJ_PricingBiaya.EfektifRate;
                            by.EfektifMounting = d.StockProductDJ_PricingBiaya.EfektifMounting;
                            by.EfektifSetting = d.StockProductDJ_PricingBiaya.EfektifSetting;

                            db.RevalProductDJ_PricingBiaya.InsertOnSubmit(by);
                            db.SubmitChanges();

                            RevalProductDJ_PricingMU mu = new RevalProductDJ_PricingMU();
                            mu.ID = ProdID;
                            mu.NamaCategory = d.StockProductDJ_PricingMU.NamaCategory;
                            mu.NamaLevel = d.StockProductDJ_PricingMU.NamaLevel;
                            mu.NamaDist = d.StockProductDJ_PricingMU.NamaDist;
                            mu.ModalUSD = d.StockProductDJ_PricingMU.ModalUSD;
                            mu.ModalRate = d.StockProductDJ_PricingMU.ModalRate;
                            mu.ModalRupiah = d.StockProductDJ_PricingMU.ModalRupiah;
                            mu.MU = d.StockProductDJ_PricingMU.MU;
                            mu.HargaUSD = d.StockProductDJ_PricingMU.HargaUSD;
                            mu.HargaRupiah = d.StockProductDJ_PricingMU.HargaRupiah;
                            mu.HargaRupiahRound = d.StockProductDJ_PricingMU.HargaRupiahRound;
                            mu.EfektifRate = d.StockProductDJ_PricingMU.EfektifRate;

                            db.RevalProductDJ_PricingMU.InsertOnSubmit(mu);
                            db.SubmitChanges();

                            RevalProductDJ_PricingProduct prc = new RevalProductDJ_PricingProduct();
                            prc.ID = ProdID;
                            prc.GoldRate = d.StockProductDJ_PricingProduct.GoldRate;
                            prc.Netto = d.StockProductDJ_PricingProduct.Netto;
                            prc.Kadar = d.StockProductDJ_PricingProduct.Kadar;
                            prc.TotalProduct = d.StockProductDJ_PricingProduct.TotalProduct;
                            prc.TotalStone1A = d.StockProductDJ_PricingProduct.TotalStone1A;
                            prc.TotalStone1B = d.StockProductDJ_PricingProduct.TotalStone1B;
                            prc.TotalStone2 = d.StockProductDJ_PricingProduct.TotalStone2;
                            prc.TotalStone3 = d.StockProductDJ_PricingProduct.TotalStone3;
                            prc.TotalStone4 = d.StockProductDJ_PricingProduct.TotalStone4;
                            prc.TotalStone5 = d.StockProductDJ_PricingProduct.TotalStone5;
                            prc.Total = d.StockProductDJ_PricingProduct.Total;
                            prc.EfektifGold = d.StockProductDJ_PricingProduct.EfektifGold;

                            db.RevalProductDJ_PricingProduct.InsertOnSubmit(prc);
                            db.SubmitChanges();

                            RevalProductDJ_PricingSegmen sg = new RevalProductDJ_PricingSegmen();
                            sg.ID = ProdID;
                            sg.ProductSegmen = d.StockProductDJ_PricingSegmen.ProductSegmen;
                            sg.NamaCategory = d.StockProductDJ_PricingSegmen.NamaCategory;
                            sg.NamaLevel = d.StockProductDJ_PricingSegmen.NamaLevel;
                            sg.NamaDist = d.StockProductDJ_PricingSegmen.NamaDist;
                            sg.SegmenSistem = d.StockProductDJ_PricingSegmen.SegmenSistem;
                            sg.SegmenMinimum = d.StockProductDJ_PricingSegmen.SegmenMinimum;
                            sg.TotalCarat = d.StockProductDJ_PricingSegmen.TotalCarat;
                            sg.HargaUSD = d.StockProductDJ_PricingSegmen.HargaUSD;
                            sg.EfektifSegmen = d.StockProductDJ_PricingSegmen.EfektifSegmen;

                            db.RevalProductDJ_PricingSegmen.InsertOnSubmit(sg);
                            db.SubmitChanges();

                            var s1A = from p in db.StockProductDJ_Stone1A
                                      where p.IDForm == d.ID
                                      select p;
                            if (s1A != null)
                            {
                                foreach (var r in s1A)
                                {
                                    RevalProductDJ_Stone1A stone = new RevalProductDJ_Stone1A();
                                    stone.IDForm = ProdID;
                                    stone.IDStone = r.IDStone;
                                    stone.TotalButir = r.TotalButir;
                                    stone.TotalCarat = r.TotalCarat;
                                    stone.CaratButir = r.CaratButir;
                                    stone.DimensiL = r.DimensiL;
                                    stone.DimensiP = r.DimensiP;
                                    stone.DimensiT = r.DimensiT;
                                    stone.HargaSatuan = r.HargaSatuan;
                                    stone.HargaTotal = r.HargaTotal;
                                    stone.HargaSatuanM = r.HargaSatuanM;
                                    stone.HargaTotalM = r.HargaTotalM;
                                    stone.Efektif = r.Efektif;
                                    stone.Setting = r.Setting;
                                    db.RevalProductDJ_Stone1A.InsertOnSubmit(stone);
                                    db.SubmitChanges();
                                }
                            }

                            var s1B = from p in db.StockProductDJ_Stone1B
                                      where p.IDForm == d.ID
                                      select p;
                            if (s1B != null)
                            {
                                foreach (var r in s1B)
                                {
                                    RevalProductDJ_Stone1B stone = new RevalProductDJ_Stone1B();
                                    stone.IDForm = ProdID;
                                    stone.IDStone = r.IDStone;
                                    stone.TotalButir = r.TotalButir;
                                    stone.TotalCarat = r.TotalCarat;
                                    stone.CaratButir = r.CaratButir;
                                    stone.DimensiL = r.DimensiL;
                                    stone.DimensiP = r.DimensiP;
                                    stone.DimensiT = r.DimensiT;
                                    stone.HargaSatuan = r.HargaSatuan;
                                    stone.HargaTotal = r.HargaTotal;
                                    stone.GIA = r.GIA;
                                    stone.HargaInputH = r.HargaInputH;
                                    stone.HargaInputP = r.HargaInputP;
                                    stone.HargaSatuanM = r.HargaSatuanM;
                                    stone.HargaTotalM = r.HargaTotalM;
                                    stone.Efektif = r.Efektif;
                                    stone.Setting = r.Setting;
                                    db.RevalProductDJ_Stone1B.InsertOnSubmit(stone);
                                    db.SubmitChanges();
                                }
                            }

                            var s2 = from p in db.StockProductDJ_Stone2
                                     where p.IDForm == d.ID
                                     select p;
                            if (s2 != null)
                            {
                                foreach (var r in s2)
                                {
                                    RevalProductDJ_Stone2 stone = new RevalProductDJ_Stone2();
                                    stone.IDForm = ProdID;
                                    stone.IDStone = r.IDStone;
                                    stone.TotalButir = r.TotalButir;
                                    stone.TotalCarat = r.TotalCarat;
                                    stone.CaratButir = r.CaratButir;
                                    stone.DimensiL = r.DimensiL;
                                    stone.DimensiP = r.DimensiP;
                                    stone.DimensiT = r.DimensiT;
                                    stone.HargaSatuan = r.HargaSatuan;
                                    stone.HargaTotal = r.HargaTotal;
                                    stone.HargaSatuanM = r.HargaSatuanM;
                                    stone.HargaTotalM = r.HargaTotalM;
                                    stone.Efektif = r.Efektif;
                                    stone.Setting = r.Setting;
                                    db.RevalProductDJ_Stone2.InsertOnSubmit(stone);
                                    db.SubmitChanges();
                                }
                            }

                            var s3 = from p in db.StockProductDJ_Stone3
                                     where p.IDForm == d.ID
                                     select p;
                            if (s3 != null)
                            {
                                foreach (var r in s3)
                                {
                                    RevalProductDJ_Stone3 stone = new RevalProductDJ_Stone3();
                                    stone.IDForm = ProdID;
                                    stone.IDStone = r.IDStone;
                                    stone.TotalButir = r.TotalButir;
                                    stone.TotalCarat = r.TotalCarat;
                                    stone.CaratButir = r.CaratButir;
                                    stone.DimensiL = r.DimensiL;
                                    stone.DimensiP = r.DimensiP;
                                    stone.DimensiT = r.DimensiT;
                                    stone.HargaSatuan = r.HargaSatuan;
                                    stone.HargaTotal = r.HargaTotal;
                                    stone.HargaSatuanM = r.HargaSatuanM;
                                    stone.HargaTotalM = r.HargaTotalM;
                                    stone.Efektif = r.Efektif;
                                    stone.Setting = r.Setting;
                                    db.RevalProductDJ_Stone3.InsertOnSubmit(stone);
                                    db.SubmitChanges();
                                }
                            }

                            var s4 = from p in db.StockProductDJ_Stone4
                                     where p.IDForm == d.ID
                                     select p;
                            if (s4 != null)
                            {
                                foreach (var r in s4)
                                {
                                    RevalProductDJ_Stone4 stone = new RevalProductDJ_Stone4();
                                    stone.IDForm = ProdID;
                                    stone.IDStone = r.IDStone;
                                    stone.TotalButir = r.TotalButir;
                                    stone.TotalCarat = r.TotalCarat;
                                    stone.CaratButir = r.CaratButir;
                                    stone.DimensiL = r.DimensiL;
                                    stone.DimensiP = r.DimensiP;
                                    stone.DimensiT = r.DimensiT;
                                    stone.HargaSatuan = r.HargaSatuan;
                                    stone.HargaTotal = r.HargaTotal;
                                    stone.HargaSatuanM = r.HargaSatuanM;
                                    stone.HargaTotalM = r.HargaTotalM;
                                    stone.Efektif = r.Efektif;
                                    stone.Setting = r.Setting;
                                    db.RevalProductDJ_Stone4.InsertOnSubmit(stone);
                                    db.SubmitChanges();
                                }
                            }


                            var s5 = from p in db.StockProductDJ_Stone5
                                     where p.IDForm == d.ID
                                     select p;
                            if (s5 != null)
                            {
                                foreach (var r in s5)
                                {
                                    RevalProductDJ_Stone5 stone = new RevalProductDJ_Stone5();
                                    stone.IDForm = ProdID;
                                    stone.IDStone = r.IDStone;
                                    stone.TotalButir = r.TotalButir;
                                    stone.TotalCarat = r.TotalCarat;
                                    stone.CaratButir = r.CaratButir;
                                    stone.DimensiL = r.DimensiL;
                                    stone.DimensiP = r.DimensiP;
                                    stone.DimensiT = r.DimensiT;
                                    stone.HargaSatuan = r.HargaSatuan;
                                    stone.HargaTotal = r.HargaTotal;
                                    stone.HargaSatuanM = r.HargaSatuanM;
                                    stone.HargaTotalM = r.HargaTotalM;
                                    stone.Efektif = r.Efektif;
                                    stone.Setting = r.Setting;
                                    db.RevalProductDJ_Stone5.InsertOnSubmit(stone);
                                    db.SubmitChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            index++;
        }
        Response.Redirect("DJ_Register.aspx?d=1");
    }
}