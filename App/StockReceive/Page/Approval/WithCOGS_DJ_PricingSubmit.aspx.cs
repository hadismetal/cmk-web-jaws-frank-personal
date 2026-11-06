using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ_PricingSubmit : System.Web.UI.Page {
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
		App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
			fillForm();
		}
	}

	//Form
	protected void fillForm() {
		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			fillProduct(d);
			fillStone();
			fillBiaya(d);
			fillFinishing();
			fillMU(d);
			fillSegmen(d);
		}
	}

	protected void fillProduct(StockReceiveDJ ms) {
		var d = ms.StockReceiveDJ_PricingProduct;
		if (d != null) {
			goldrate.InnerHtml = Str.Number2(d.GoldRate);
			netto.InnerHtml = Str.Number2(d.Netto);
			kadar.InnerHtml = Str.Number2(d.Kadar);
			totalproduct.InnerHtml = Str.Number2(d.TotalProduct);

			sum_frame.InnerHtml = Str.Number2(d.TotalProduct);
            sum_stone.InnerHtml = Str.Number2(d.TotalStone1A + d.TotalStone1B + d.TotalStone2 + d.TotalStone3 + d.TotalStone4 + d.TotalStone5);
			sum_total.InnerHtml = Str.Number2(d.Total);

			efektifgold.InnerHtml = Str.Tgl(d.EfektifGold);
		}
	}
	protected void fillBiaya(StockReceiveDJ ms) {
		var d = ms.StockReceiveDJ_PricingBiaya;
		if (d != null) {
			brand1.InnerHtml = d.NamaBrand;
			item1.InnerHtml = d.NamaItem;
			cons1.InnerHtml = d.NamaCons;

			no1.InnerHtml = Str.Number0(d.MountingNO);
			ro1.InnerHtml = Str.Number0(d.MountingRO);
			total1.InnerHtml = Str.Number0(d.Mounting);

			efektifmounting.InnerHtml = Str.Tgl(d.EfektifMounting);

			fee10.InnerHtml = Str.Number0(d.Mounting);
			fee20.InnerHtml = Str.Number0(d.Setting);
			fee30.InnerHtml = Str.Number0(d.Finishing);
			
			fee11.InnerHtml = Str.Tgl(d.EfektifMounting);
			fee21.InnerHtml = Str.Tgl(d.EfektifSetting);

			feerupiah.InnerHtml = Str.Number0(d.TotalRupiah);
			feerate.InnerHtml = Str.Number2(d.TotalRate);
			feetotal.InnerHtml = Str.Number2(d.Total);

			efektifrate.InnerHtml = Str.Tgl(d.EfektifRate);
		}
	}
	protected void fillMU(StockReceiveDJ ms) {
		var d = ms.StockReceiveDJ_PricingMU;
		if (d != null) {
			cat1.InnerHtml = d.NamaCategory;
			lvl1.InnerHtml = d.NamaLevel;
			dist1.InnerHtml = d.NamaDist;

			modal.InnerHtml = Str.Number2(d.ModalUSD);
			modalrate.InnerHtml = Str.Number2(d.ModalRate);
			modalrupiah.InnerHtml = Str.Number2(d.ModalRupiah);

			murate.InnerHtml = Str.Tgl(d.EfektifRate);

			mu_persen.InnerHtml = Str.Number2(d.MU);
			hargaidr.InnerHtml = Str.Number2(d.HargaRupiahRound);
			hargausd.InnerHtml = Str.Number2(d.HargaUSD);
		}
	}
	protected void fillSegmen(StockReceiveDJ ms) {
		var d = ms.StockReceiveDJ_PricingSegmen;
		if (d != null) {
			cat2.InnerHtml = d.NamaCategory;
			lvl2.InnerHtml = d.NamaLevel;
			dist2.InnerHtml = d.NamaDist;

			segmensistem.InnerHtml = DL.SegmenSistem(d.SegmenSistem);
			if (d.SegmenSistem == 0)
				minimum.InnerHtml = Str.Number2(d.SegmenMinimum);
			else
				minimum.InnerHtml = Str.Number3(d.SegmenMinimum);
			efektifsegmen.InnerHtml = Str.Tgl(d.EfektifSegmen);

			segmen.InnerHtml = d.CharProductSegmen.Nama;

			carat.InnerHtml = Str.Number3(d.TotalCarat);
			hargasegmen.InnerHtml = Str.Number2(d.HargaUSD);
		}
	}

	//Stone
	protected void fillStone() {
		int GT_Butir = 0;
		decimal GT_HargaM = 0;
        decimal GT_Harga = 0;

		//Stone
        fillStone1A(0, ref GT_Butir, ref GT_HargaM, ref GT_Harga);
        fillStone1B(1, ref GT_Butir, ref GT_HargaM, ref GT_Harga);
        fillStone2(2, ref GT_Butir, ref GT_HargaM, ref GT_Harga);
        fillStone3(3, ref GT_Butir, ref GT_HargaM, ref GT_Harga);
        fillStone4(4, ref GT_Butir, ref GT_HargaM, ref GT_Harga);
        fillStone5(5, ref GT_Butir, ref GT_HargaM, ref GT_Harga);

		//Grand-Total
		Sub_Total(GT_Butir,GT_HargaM, GT_Harga, true);
	}

	//Stone
	protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_CaratM, ref decimal GT_Carat) {
		//SELECT
		var rs = from p in db.StockReceiveDJ_Stone1A
				 where p.IDForm == myID
				 orderby p.Stone1A.Nama
				 select p;

		//Sub-Total
		int ST_Butir = 0;
        decimal ST_HargaM = 0;
		decimal ST_Harga = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index++.ToString() + ".";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone1A.Nama;
			td.CssClass = "text-bold";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Tgl(r.Efektif);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaSatuan);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Integer(r.TotalButir);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.Stone1A.StoneCosting1A.Harga*r.TotalButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaTotal);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Total
			ST_Butir += r.TotalButir;
            ST_HargaM += Convert.ToDecimal(r.Stone1A.StoneCosting1A.Harga * r.TotalButir);
			ST_Harga += r.HargaTotal.Value;
		}

		//Total
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
            GT_CaratM += ST_HargaM;
			GT_Carat += ST_Harga;

			Sub_Total(ST_Butir,ST_HargaM, ST_Harga, false);
		}
	}
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_CaratM, ref decimal GT_Carat)
    {
		//SELECT
		var rs = from p in db.StockReceiveDJ_Stone1B
				 where p.IDForm == myID
				 orderby p.Stone1B.Nama
				 select p;

		//Sub-Total
		int ST_Butir = 0;
        decimal ST_HargaM = 0;
		decimal ST_Harga = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index++.ToString() + ".";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone1B.Nama;
			td.CssClass = "text-bold";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Tgl(r.Efektif);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaSatuan);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Integer(r.TotalButir);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.Stone1B.StoneCosting1B.Harga*r.TotalButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaTotal);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Total
			ST_Butir += r.TotalButir;
            ST_HargaM += Convert.ToDecimal(r.Stone1B.StoneCosting1B.Harga*r.TotalButir);
			ST_Harga += r.HargaTotal.Value;
		}

		//Total
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
            GT_CaratM += ST_HargaM;
			GT_Carat += ST_Harga;

			Sub_Total(ST_Butir,ST_HargaM, ST_Harga, false);
		}
	}
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_CaratM, ref decimal GT_Carat)
    {
		//SELECT
		var rs = from p in db.StockReceiveDJ_Stone2
				 where p.IDForm == myID
				 orderby p.Stone2.Nama
				 select p;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_HargaM = 0;
        decimal ST_Harga = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index++.ToString() + ".";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone2.Nama;
			td.CssClass = "text-bold";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Tgl(r.Efektif);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaSatuan);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Integer(r.TotalButir);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.Stone2.StoneCosting2.Harga*r.TotalButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaTotal);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Total
			ST_Butir += r.TotalButir;
            ST_HargaM += Convert.ToDecimal(r.Stone2.StoneCosting2.Harga * r.TotalButir);
			ST_Harga += r.HargaTotal.Value;
		}

		//Total
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
            GT_CaratM += ST_HargaM;
			GT_Carat += ST_Harga;

			Sub_Total(ST_Butir,ST_HargaM, ST_Harga, false);
		}
	}
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_CaratM, ref decimal GT_Carat)
    {
		//SELECT
		var rs = from p in db.StockReceiveDJ_Stone3
				 where p.IDForm == myID
				 orderby p.Stone3.Nama
				 select p;

		//Sub-Total
		int ST_Butir = 0;
        decimal ST_HargaM = 0;
		decimal ST_Harga = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index++.ToString() + ".";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone3.Nama;
			td.CssClass = "text-bold";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Tgl(r.Efektif);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaSatuan);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Integer(r.TotalButir);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.Stone3.StoneCosting3.Harga*r.TotalButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaTotal);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Total
			ST_Butir += r.TotalButir;
            ST_HargaM += Convert.ToDecimal(r.Stone3.StoneCosting3.Harga*r.TotalButir);
			ST_Harga += r.HargaTotal.Value;
		}

		//Total
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
            GT_CaratM += ST_HargaM;
			GT_Carat += ST_Harga;

			Sub_Total(ST_Butir,ST_HargaM, ST_Harga, false);
		}
	}
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_CaratM, ref decimal GT_Carat)
    {
		//SELECT
		var rs = from p in db.StockReceiveDJ_Stone4
				 where p.IDForm == myID
				 orderby p.Stone4.Nama
				 select p;

		//Sub-Total
		int ST_Butir = 0;
        decimal ST_HargaM = 0;
		decimal ST_Harga = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index++.ToString() + ".";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone4.Nama;
			td.CssClass = "text-bold";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Tgl(r.Efektif);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaSatuan);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Integer(r.TotalButir);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.Stone4.StoneCosting4.Harga*r.TotalButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaTotal);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Total
			ST_Butir += r.TotalButir;
            ST_HargaM += Convert.ToDecimal(r.Stone4.StoneCosting4.Harga*r.TotalButir);
			ST_Harga += r.HargaTotal.Value;
		}

		//Total
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
            GT_CaratM += ST_HargaM;
			GT_Carat += ST_Harga;

			Sub_Total(ST_Butir,ST_HargaM, ST_Harga, false);
		}
	}
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_CaratM, ref decimal GT_Carat)
    {
		//SELECT
		var rs = from p in db.StockReceiveDJ_Stone5
				 where p.IDForm == myID
				 orderby p.Stone5.Nama
				 select p;

		//Sub-Total
		int ST_Butir = 0;
        decimal ST_HargaM = 0;
		decimal ST_Harga = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index++.ToString() + ".";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone5.Nama;
			td.CssClass = "text-bold";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Tgl(r.Efektif);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaSatuan);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Integer(r.TotalButir);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.Stone5.StoneCosting5.Harga*r.TotalButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.HargaTotal);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Total
			ST_Butir += r.TotalButir;
            ST_Harga += Convert.ToDecimal(r.Stone5.StoneCosting5.Harga*r.TotalButir);
			ST_Harga += r.HargaTotal.Value;
		}

		//Total
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
            GT_CaratM += ST_HargaM;
			GT_Carat += ST_Harga;

			Sub_Total(ST_Butir,ST_HargaM, ST_Harga, false);
		}
	}
	protected void Sub_Header(int StoneIndex) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += DL.StoneIndex(StoneIndex);

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected void Sub_Total(int TotalButir, decimal TotalHargaM, decimal TotalHarga, bool GrandTotal) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 4;
		td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = Str.Integer(TotalButir);
		td.CssClass = "text-center";
		tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number2(TotalHargaM);
        td.CssClass = "text-right text-bold";
        tr.Cells.Add(td);

		td = new TableCell();
		td.Text = Str.Number2(TotalHarga);
		td.CssClass = "text-right text-bold";
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}

	protected void fillFinishing() {
		//SELECT
		var rs = from p in db.StockReceiveDJ_Finishing
				 where p.IDForm == myID
				 orderby p.CharProcessFinishing.SN
				 select p;

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index++.ToString() + ".";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.CharProcessFinishing.Nama;
			td.CssClass = "text-bold";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Tgl(r.Efektif);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Str.Number2(r.Biaya);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tbff.Rows.Add(tr);
		}
	}
	

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Status
			

			//User-Info
			d.Approval = App.UserNama;
			d.ApprovalTgl = DateTime.Now;

			//Commit
			db.SubmitChanges();

			//Redirect
			App.ReloadClose(this);
		}
	}
}