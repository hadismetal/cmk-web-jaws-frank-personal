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
    decimal Setting = 0m;
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
	protected void DataError(string err = "") { Response.Redirect("SPK.aspx?err=" + err); }
    
    protected void LinkError() { Response.Redirect("SPK_CharDesign01.aspx?id=" + myID + "&tipe=" + tipe); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError("001");
		else {
			//Header
            //if (tipe == 1)
            //{
            //    header_nomor.InnerHtml =
            //        "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //        + d.ProductDesign.Nomor + "</a>";
            //}
            //else
            //{
            //    header_nomor.InnerHtml =
            //        "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //        + d.KodeKaretDJ.Nomor + "</a>";
            //}
		}
        if (tipe == 1)
        {
            var dsg = db.ProductDesign_CharDesign.SingleOrDefault(p => p.ID == d.IDProductDesign);
            if (dsg == null) DataError("002");
            else
            {
                naming.Text = dsg.Naming;
                setitem.Checked = (dsg.SetItem == true) ? true : false;
                special.Checked = (dsg.Special == true) ? true : false;
                grafir.Text = dsg.Grafir;

                //DLC
                DLC.CharProcessFinishingSPK(cb, Convert.ToInt32(d.IDProductDesign));
            }
        }
        else
        {
            var dsg = db.KodeKaretDJ_CharDesign.SingleOrDefault(p => p.ID == d.IDKodeKaret);
            if (dsg == null) DataError("003");
            else
            {
                naming.Text = dsg.Naming;
                setitem.Checked = (dsg.SetItem == true) ? true : false;
                special.Checked = (dsg.Special == true) ? true : false;
                grafir.Text = dsg.Grafir;

                //DLC
                DLC.CharProcessFinishingSPKKaret(cb, Convert.ToInt32(d.IDKodeKaret));
            }
        }
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError("004");
		else {

            var product = d.ProductOrder_CharProduct;

			//Process
			saveData(d);
			saveFinishing(d);
            moveStone(d);

            //Recal Weight
            Lib.RecalWeight(db, d, product);

			//Redirect
			Response.Redirect("SPK_Stone.aspx?id=" + myID + "&tipe=" + tipe);
		}
	}

	//Data
	protected void saveData(ProductOrder ms) {
		var d = ms.ProductOrder_CharDesign;
		if (d == null) LinkError();
		else {
            string strgrafirtype = "";
            if (rdo1_ic.Checked) { strgrafirtype = rdo1_ic.Value; }
            else if (rdo1_bt.Checked) { strgrafirtype = rdo1_bt.Value; }
            else if (rdo1_bc.Checked) { strgrafirtype = rdo1_bc.Value; }
            else if (rdo1_mc.Checked) { strgrafirtype = rdo1_mc.Value; }
            else if (rdo1_vv.Checked) { strgrafirtype = rdo1_vv.Value; }
            else if (rdo1_bs.Checked) { strgrafirtype = rdo1_bs.Value; }
            else if (rdo1_ale.Checked) { strgrafirtype = rdo1_ale.Value; }

            d.Naming = Str.CapTitle(naming.Text);
            d.Grafir = grafir.Text.Trim(); // Str.CapUpper(grafir.Text);
            d.Grafir2 = grafir2.Text.Trim(); // Str.CapUpper(grafir2.Text);
            d.GrafirType = strgrafirtype;

			d.SetItem = setitem.Checked;
			d.Special = special.Checked;

			//Commit
			db.SubmitChanges();
		}
	}

    //Move Stone
    protected void moveStone(ProductOrder ms)
    {
        if (tipe == 1)
        {
            moveStone1A(ms);
            moveStone1B(ms);
            moveStone2(ms);
            moveStone3(ms);
            moveStone4(ms);
            moveStone5(ms);
        }
        else
        {
            moveKaretStone1A(ms);
            moveKaretStone1B(ms);
            moveKaretStone2(ms);
            moveKaretStone3(ms);
            moveKaretStone4(ms);
            moveKaretStone5(ms);
        }
    }
    protected void moveStone1A(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone1A.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.ProductDesign_Stone1A
                    where p.IDForm == ms.IDProductDesign
                    orderby p.Stone1A.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing1A prc = r.Stone1A.StonePricing1A;
                StoneCosting1A cost = r.Stone1A.StoneCosting1A;

                ProductOrder_Stone1A n = new ProductOrder_Stone1A();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;
                
                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone1B(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone1B.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.ProductDesign_Stone1B
                    where p.IDForm == ms.IDProductDesign
                    orderby p.Stone1B.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing1B prc = r.Stone1B.StonePricing1B;
                StoneCosting1B cost = r.Stone1B.StoneCosting1B;

                ProductOrder_Stone1B n = new ProductOrder_Stone1B();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;
                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;
                //n.StatusTerpasang = r.StatusTerpasang;
                n.StatusTerpasang = 1;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone2(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone2.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.ProductDesign_Stone2
                    where p.IDForm == ms.IDProductDesign
                    orderby p.Stone2.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing2 prc = r.Stone2.StonePricing2;
                StoneCosting2 cost = r.Stone2.StoneCosting2;

                ProductOrder_Stone2 n = new ProductOrder_Stone2();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone3(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone3.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.ProductDesign_Stone3
                    where p.IDForm == ms.IDProductDesign
                    orderby p.Stone3.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing3 prc = r.Stone3.StonePricing3;
                StoneCosting3 cost = r.Stone3.StoneCosting3;

                ProductOrder_Stone3 n = new ProductOrder_Stone3();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone4(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone4.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.ProductDesign_Stone4
                    where p.IDForm == ms.IDProductDesign
                    orderby p.Stone4.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing4 prc = r.Stone4.StonePricing4;
                StoneCosting4 cost = r.Stone4.StoneCosting4;

                ProductOrder_Stone4 n = new ProductOrder_Stone4();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone5(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone5.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.ProductDesign_Stone5
                    where p.IDForm == ms.IDProductDesign
                    orderby p.Stone5.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing5 prc = r.Stone5.StonePricing5;
                StoneCosting5 cost = r.Stone5.StoneCosting5;

                ProductOrder_Stone5 n = new ProductOrder_Stone5();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected void moveKaretStone1A(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone1A.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.KodeKaretDJ_Stone1A
                    where p.IDForm == ms.IDKodeKaret
                    orderby p.Stone1A.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing1A prc = r.Stone1A.StonePricing1A;
                StoneCosting1A cost = r.Stone1A.StoneCosting1A;

                ProductOrder_Stone1A n = new ProductOrder_Stone1A();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveKaretStone1B(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone1B.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.KodeKaretDJ_Stone1B
                    where p.IDForm == ms.IDKodeKaret
                    orderby p.Stone1B.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing1B prc = r.Stone1B.StonePricing1B;
                StoneCosting1B cost = r.Stone1B.StoneCosting1B;

                ProductOrder_Stone1B n = new ProductOrder_Stone1B();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;
                n.GIA = r.GIA;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                n.StatusTerpasang = 1;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveKaretStone2(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone2.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.KodeKaretDJ_Stone2
                    where p.IDForm == ms.IDKodeKaret
                    orderby p.Stone2.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing2 prc = r.Stone2.StonePricing2;
                StoneCosting2 cost = r.Stone2.StoneCosting2;

                ProductOrder_Stone2 n = new ProductOrder_Stone2();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveKaretStone3(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone3.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.KodeKaretDJ_Stone3
                    where p.IDForm == ms.IDKodeKaret
                    orderby p.Stone3.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing3 prc = r.Stone3.StonePricing3;
                StoneCosting3 cost = r.Stone3.StoneCosting3;

                ProductOrder_Stone3 n = new ProductOrder_Stone3();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveKaretStone4(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone4.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.KodeKaretDJ_Stone4
                    where p.IDForm == ms.IDKodeKaret
                    orderby p.Stone4.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing4 prc = r.Stone4.StonePricing4;
                StoneCosting4 cost = r.Stone4.StoneCosting4;

                ProductOrder_Stone4 n = new ProductOrder_Stone4();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveKaretStone5(ProductOrder ms)
    {
        decimal OngkosPB = 0m;
        var z = db.ProductOrder_Stone5.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.ProductOrder_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }
        var stone = from p in db.KodeKaretDJ_Stone5
                    where p.IDForm == ms.IDKodeKaret
                    orderby p.Stone5.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StonePricing5 prc = r.Stone5.StonePricing5;
                StoneCosting5 cost = r.Stone5.StoneCosting5;

                ProductOrder_Stone5 n = new ProductOrder_Stone5();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Harga
                n.HargaSatuan = prc.Harga.Value;
                n.HargaTotal = n.HargaSatuan * n.TotalCarat;

                n.HargaSatuanM = cost.Harga.Value;
                n.HargaTotalM = n.HargaSatuanM * n.TotalCarat;

                //Tgl. Efektif
                n.Efektif = prc.ApprovalTgl.Value;

                //Commit
                db.ProductOrder_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

	//Finishing
	protected void saveFinishing(ProductOrder d) {
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
	protected void insertProcess(ProductOrder ms, int myIDFinishing) {
		var d = ms.ProductOrder_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
		if (d == null) {
			ProductOrder_Finishing n = new ProductOrder_Finishing();

			n.IDForm = ms.ID;
			n.IDFinishing = myIDFinishing;

			//Commit
			db.ProductOrder_Finishing.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected void deleteProcess(ProductOrder ms, int myIDFinishing) {
		var d = ms.ProductOrder_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
		if (d != null) {
			//Commit
			db.ProductOrder_Finishing.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
}