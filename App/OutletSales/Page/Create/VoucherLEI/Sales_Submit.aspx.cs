using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_Submit : System.Web.UI.Page
{
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
    string Module = "OutletSales"; // OutletSales , Accounting

	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Trans. Form
            string Brand = "";
            if(d.TipeLokasi == 1)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi,Db.Cnn_DB);
            else if(d.TipeLokasi == 2)
                Brand = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi,Db.Cnn_DB);

            brand.InnerHtml = Brand;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            customer.InnerHtml = d.DataCustomer.Nama;
			tgl.InnerHtml = Str.Tgl(d.Tgl);

            totalbeforedisc.Text = Convert.ToDecimal(d.TotalBayarBeforeDisc.Value).ToString("N0");
            totalbayar.Text = Convert.ToDecimal(d.TotalBayarBeforeDisc.Value).ToString("N0");
			
			//Stone
			fillItem();

		}
	}

	//Stone
	protected void fillItem() {
        decimal GT = 0m;
        decimal GT2 = 0m;
        //Item
        fillItemDJ(ref GT, ref GT2);
        fillItemPG(ref GT, ref GT2);
        fillItemGJ(ref GT, ref GT2);
        fillItemLD(ref GT, ref GT2);

        //Grand-Total
        Sub_Total(GT, GT2, true);
	}

    //Item
    protected void fillItemDJ(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderDJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(0);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST,ST2, false);
        }

    }
    protected void fillItemPG(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderPG
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(1);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT += ST;

            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemGJ(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderGJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(0);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductGJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }

    protected void fillItemLD(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderLD
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(0);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductLD_Stone1B.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.StockProductLD_Stone1B.HargaTotal.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.StockProductLD_Stone1B.HargaTotal.Value);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }

    //Custom Row
    protected void Sub_Header(int ItemIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.ItemIndex(ItemIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(decimal Total, decimal TotalBayar, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 4;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalBayar) + "</p>";
        td.CssClass = "text-right";
        tr.Cells.Add(td);


        tb.Rows.Add(tr);
    }

    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }

    protected void CalcTotalBayar(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            decimal Discount = 0m;
            decimal DiscountProgram = 0m;
            decimal DiscountGift = 0m;
            decimal DiscountRound = 0m;
            decimal NoDiscount = 0m;
            decimal NoDiscountProgram = 0m;
            decimal TotalBayar = 0m;
            decimal TotalRupiah = 0m;
            decimal TotalHarga = 0m;

            if (Convert.ToDecimal(discount.Text) > 10)
                discount.Text = "10";

            Discount = Convert.ToDecimal(discount.Text);

            if (Convert.ToDecimal(discountprogram.Text) > 5)
                discountprogram.Text = "5";

            DiscountProgram = Convert.ToDecimal(discountprogram.Text);

            DiscountGift = Convert.ToDecimal(discountgift.Text);

            TotalHarga = d.TotalBayarBeforeDisc.Value;
            NoDiscount = Math.Ceiling(TotalHarga * Discount / 100);
            TotalBayar = TotalHarga - NoDiscount;
            NoDiscountProgram = Math.Ceiling(TotalBayar * DiscountProgram / 100);
            TotalBayar = TotalBayar - NoDiscountProgram;
            TotalRupiah = TotalBayar;
            TotalBayar = TotalBayar - DiscountGift;

            CalcDiscountRound(TotalBayar, discountround);

            DiscountRound = Convert.ToDecimal(discountround.Text);

            
            TotalBayar = TotalBayar - DiscountRound;

            totalbayar.Text = TotalBayar.ToString("N0");
            totaldiscount.Text = (NoDiscount + NoDiscountProgram + DiscountGift + DiscountRound).ToString("N0");
        }

        save.Focus();
    }

    protected void CalcTotalBayar2(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            decimal Discount = 0m;
            decimal DiscountProgram = 0m;
            decimal DiscountGift = 0m;
            decimal DiscountRound = 0m;
            decimal NoDiscount = 0m;
            decimal NoDiscountProgram = 0m;
            decimal TotalBayar = 0m;
            decimal TotalRupiah = 0m;
            decimal TotalHarga = 0m;

            if (Convert.ToDecimal(discount.Text) > 10)
                discount.Text = "10";

            Discount = Convert.ToDecimal(discount.Text);

            if (Convert.ToDecimal(discountprogram.Text) > 5)
                discountprogram.Text = "5";

            DiscountProgram = Convert.ToDecimal(discountprogram.Text);

            DiscountGift = Convert.ToDecimal(discountgift.Text);

            TotalHarga = d.TotalBayarBeforeDisc.Value;
            NoDiscount = Math.Ceiling(TotalHarga * Discount / 100);
            TotalBayar = TotalHarga - NoDiscount;
            NoDiscountProgram = Math.Ceiling(TotalBayar * DiscountProgram / 100);
            TotalBayar = TotalBayar - NoDiscountProgram;
            TotalRupiah = TotalBayar;
            TotalBayar = TotalBayar - DiscountGift;

            DiscountRound = Convert.ToDecimal(discountround.Text);


            TotalBayar = TotalBayar - DiscountRound;

            totalbayar.Text = TotalBayar.ToString("N0");
            totaldiscount.Text = (NoDiscount + NoDiscountProgram + DiscountGift + DiscountRound).ToString("N0");
        }

        save.Focus();
    }

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
            decimal Discount = 0m;
            decimal DiscountProgram = 0m;
            decimal DiscountGift = 0m;
            decimal DiscountRound = 0m;
            decimal NoDiscount = 0m;
            decimal NoDiscountProgram = 0m;
            decimal TotalBayar = 0m;
            decimal TotalRupiah = 0m;
            decimal TotalHarga = 0m;

            if (Convert.ToDecimal(discount.Text) > 10)
                discount.Text = "10";

            Discount = Convert.ToDecimal(discount.Text);

            if (Convert.ToDecimal(discountprogram.Text) > 5)
                discountprogram.Text = "5";

            DiscountProgram = Convert.ToDecimal(discountprogram.Text);

            DiscountGift = Convert.ToDecimal(discountgift.Text);

            TotalHarga = d.TotalBayarBeforeDisc.Value;
            NoDiscount = Math.Ceiling(TotalHarga * Discount / 100);
            TotalBayar = TotalHarga - NoDiscount;
            NoDiscountProgram = Math.Ceiling(TotalBayar * DiscountProgram / 100);
            TotalBayar = TotalBayar - NoDiscountProgram;
            TotalRupiah = TotalBayar;
            TotalBayar = TotalBayar - DiscountGift;

            DiscountRound = Convert.ToDecimal(discountround.Text);

            TotalBayar = TotalBayar - DiscountRound;

			//Status
			d.Draft = false;
			d.DraftDate = null;

            d.ModalRupiah = d.SalesOrderDJ.Sum(p => p.ModalRupiah) + d.SalesOrderPG.Sum(p => p.ModalRupiah) + d.SalesOrderPG.Sum(p => p.ModalRupiah);
            d.HargaRupiah = d.SalesOrderDJ.Sum(p => p.HargaRupiah) + d.SalesOrderPG.Sum(p => p.HargaRupiah);
            d.TotalBayarBeforeDisc = d.SalesOrderDJ.Sum(p => p.TotalBayar) + d.SalesOrderPG.Sum(p => p.TotalBayar);
            d.TotalRupiahBeforeDisc = d.SalesOrderDJ.Sum(p => p.TotalRupiah) + d.SalesOrderPG.Sum(p => p.TotalRupiah);
            d.Discount = Discount;
            d.DiscountNominal = NoDiscount;
            d.DiscountGift = DiscountGift;
            d.DiscountProgram = DiscountProgram;
            d.DiscountProgramNominal = NoDiscountProgram;
            d.DiscountRound = DiscountRound;
            d.TotalBayar = TotalBayar;
            d.TotalRupiah = TotalRupiah;
            d.Unpaid = TotalBayar;
            d.Paid = 0m;

			//Commit
			db.SubmitChanges();

            foreach (var r in d.SalesOrderDJ)
            {
                // Ledger 
                StockLedgerDJ l = new StockLedgerDJ();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = -1; // 1 : in , -1 : out
                int Remark = 8; // See Library DB - Ledger
                string Keterangan = d.Nomor; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                else if (d.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                //Input
                l.IDProduct = r.IDProduct;
                l.TipeLokasi = d.TipeLokasi.Value;
                l.IDLokasi = d.IDLokasi.Value;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerDJ.InsertOnSubmit(l);
                db.SubmitChanges();
            }

            foreach (var r in d.SalesOrderPG)
            {
                // Ledger 
                StockLedgerPG l = new StockLedgerPG();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = -1; // 1 : in , -1 : out
                int Remark = 8; // See Library DB - Ledger
                string Keterangan = d.Nomor; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                else if (d.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                //Input
                l.IDProduct = r.IDProduct;
                l.TipeLokasi = d.TipeLokasi.Value;
                l.IDLokasi = d.IDLokasi.Value;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerPG.InsertOnSubmit(l);
                db.SubmitChanges();
            }

            foreach (var r in d.SalesOrderGJ)
            {
                // Ledger 
                StockLedgerGJ l = new StockLedgerGJ();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = -1; // 1 : in , -1 : out
                int Remark = 8; // See Library DB - Ledger
                string Keterangan = d.Nomor; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                else if (d.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                //Input
                l.IDProduct = r.IDProduct;
                l.TipeLokasi = d.TipeLokasi.Value;
                l.IDLokasi = d.IDLokasi.Value;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerGJ.InsertOnSubmit(l);
                db.SubmitChanges();
            }

			//Redirect
			Response.Redirect("/" + Module + "/Page/Create/Payment/Payment_ItemList.aspx?d=1&id=" + d.ID);
		}
        //Response.Redirect("DJ_Pricing.aspx?id=" + myID);
	}

    protected void CalcDiscountRound(decimal TotalHarga, TextBox tb)
    {
        decimal Disc = 0m;

        App rd = new App();

        Disc = TotalHarga - rd.RoundBottom(TotalHarga);
        tb.Text = Disc.ToString("N0");
    }
}