using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Resell_Submit : System.Web.UI.Page {
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
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}
        fillForm();
	}

	//Exception
	protected void DataError() { Response.Redirect("Resell.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft);
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
			
			//Stone
			fillItem();

		}
	}

	//Stone
	protected void fillItem() {
        decimal GT = 0m;
        //Item
        fillItemDJ(ref GT);
        fillItemPG(ref GT);
        fillItemGJ(ref GT);

        //Grand-Total
        Sub_Total(GT, true);
	}

    //Item
    protected void fillItemDJ(ref decimal GT)
    {
        var rs = from p in db.ResellDJ
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
            td.Text = r.StockProductDJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;

            Sub_Total(ST, false);
        }

    }
    protected void fillItemPG(ref decimal GT)
    {
        var rs = from p in db.ResellPG
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
            td.Text = r.StockProductPG.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT += ST;


            Sub_Total(ST, false);
        }
    }
    protected void fillItemGJ(ref decimal GT)
    {

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
    protected void Sub_Total(decimal Total, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 3;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
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

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {

            var s = from p in db.ResellDJ
                    where p.IDForm == d.ID
                    select p;
            foreach (var r in s)
            {
                var pd = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.IDForm == r.IDSalesOrder);
                if (pd != null)
                {
                    pd.StatusResell = 1;
                    db.SubmitChanges();

                    // Ledger 
                    StockLedgerDJ l = new StockLedgerDJ();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = 1; // 1 : in , -1 : out
                    int Remark = 11; // See Library DB - Ledger
                    string Keterangan = d.Nomor; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (d.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                    else if (d.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                    //Input
                    l.IDProduct = r.IDProduct;
                    l.TipeLokasi = d.TipeLokasi;
                    l.IDLokasi = d.IDLokasi;
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
            }

            var t = from p in db.ResellPG
                    where p.IDForm == d.ID
                    select p;
            foreach (var r in t)
            {
                var pd = db.SalesOrderPG.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.IDForm == r.IDSalesOrder);
                if (pd != null)
                {
                    pd.StatusResell = 1;
                    db.SubmitChanges();

                    // Ledger 
                    StockLedgerPG l = new StockLedgerPG();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = 1; // 1 : in , -1 : out
                    int Remark = 11; // See Library DB - Ledger
                    string Keterangan = d.Nomor; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (d.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                    else if (d.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                    //Input
                    l.IDProduct = r.IDProduct;
                    l.TipeLokasi = d.TipeLokasi;
                    l.IDLokasi = d.IDLokasi;
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
            }

			//Status
			d.Draft = false;
            d.DraftDate = null;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("/Resell.aspx?d=1");
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