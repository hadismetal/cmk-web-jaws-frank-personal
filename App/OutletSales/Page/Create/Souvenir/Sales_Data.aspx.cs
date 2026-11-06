using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Register_Sales_Item : System.Web.UI.Page
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
    App gp = new App();
	int index = 1;
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //HyperLink          
            addsv.HRef += "?id=" + myID;

            //Form
            fillForm();

            //Generate
        }
        fillTable(sender, e);
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Sales_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //Item        
        fillSouvenir();
        fillSouvenirGJ();
        fillSouvenirDJ();
        fillSouvenirLD();
        fillSouvenirPG();

        TextBox txtbox = new TextBox();
    }

    protected void fillSouvenir()
    {
        var rs = from p in db.SalesOrderSouvenir
                 where p.IDForm == myID && p.TipeProduct == 0
                 select p;

        //int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenir();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(0, r.ID, r.IDProduct.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Souvenir.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Souvenir.Kode;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
			txt.Attributes["type"] = "number";
            txt.Text = r.Qty.HasValue ? r.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
			txt.TextChanged += new EventHandler(OnKeyPressQty);

            HiddenField hf = new HiddenField();
            hf.ID = "hfsv" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }

    protected void fillSouvenirGJ()
    {
        var rs = from p in db.SalesOrderSouvenir
                 join q in db.StockProductGJ on p.IDProductSouvenir equals q.ID
                 where p.IDForm == myID && p.TipeProduct == 2
                 select new { p, q };

        //int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenir();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(2, r.p.ID, r.p.IDProductSouvenir.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.StockProductGJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
			txt.Attributes["type"] = "number";
			txt.Attributes["max"] = "1";
			txt.Attributes["min"] = "1";
			txt.Attributes["disabled"] = "disabled";
            txt.Text = r.p.Qty.HasValue ? r.p.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
			txt.TextChanged += new EventHandler(OnKeyPressQtyProduct);

            HiddenField hf = new HiddenField();
            hf.ID = "hfsv" + index.ToString();
            hf.Value = r.p.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }

    protected void fillSouvenirDJ()
    {
        var rs = from p in db.SalesOrderSouvenir
                 join q in db.StockProductDJ on p.IDProductSouvenir equals q.ID
                 where p.IDForm == myID && p.TipeProduct == 1
                 select new { p, q };

        //int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenir();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(1, r.p.ID, r.p.IDProductSouvenir.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
			txt.Attributes["type"] = "number";
			txt.Attributes["max"] = "1";
			txt.Attributes["min"] = "1";
			txt.Attributes["disabled"] = "disabled";
            txt.Text = r.p.Qty.HasValue ? r.p.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
			txt.TextChanged += new EventHandler(OnKeyPressQtyProduct);

            HiddenField hf = new HiddenField();
            hf.ID = "hfsv" + index.ToString();
            hf.Value = r.p.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }

    protected void fillSouvenirLD()
    {
        var rs = from p in db.SalesOrderSouvenir
                 join q in db.StockProductLD_Stone1B on p.IDProductSouvenir equals q.ID
                 where p.IDForm == myID && p.TipeProduct == 4
                 select new { p, q };

        //int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenir();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(4, r.p.ID, r.p.IDProductSouvenir.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "-";
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
			txt.Attributes["type"] = "number";
			txt.Attributes["max"] = "1";
			txt.Attributes["min"] = "1";
			txt.Attributes["disabled"] = "disabled";
            txt.Text = r.p.Qty.HasValue ? r.p.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
			txt.TextChanged += new EventHandler(OnKeyPressQtyProduct);

            HiddenField hf = new HiddenField();
            hf.ID = "hfsv" + index.ToString();
            hf.Value = r.p.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }

    protected void fillSouvenirPG()
    {
        var rs = from p in db.SalesOrderSouvenir
                 join q in db.StockProductPG on p.IDProductSouvenir equals q.ID
                 where p.IDForm == myID && p.TipeProduct == 3
                 select new { p, q };

        //int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSouvenir();

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(3, r.p.ID, r.p.IDProductSouvenir.Value);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.q.CharProductItem.Nama;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "sv_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
			txt.Attributes["type"] = "number";
			txt.Attributes["max"] = "1";
			txt.Attributes["min"] = "1";
			txt.Attributes["disabled"] = "disabled";
            txt.Text = r.p.Qty.HasValue ? r.p.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
			txt.TextChanged += new EventHandler(OnKeyPressQtyProduct);

            HiddenField hf = new HiddenField();
            hf.ID = "hfsv" + index.ToString();
            hf.Value = r.p.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

            index++;
        }
    }

    //Custom Row
    protected void Sub_HeaderSouvenir()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "Souvenir";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }

    //Display Column
    protected string TD_HyperLink(int ItemIndex, int ID, int IDProduct)
    {
        string x = string.Empty;
        string href = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + ItemIndex + "','" + ID + "')\">";
        x += "      <i class=\"icon-trash\"></i> ";
        x += "      Delete";
        x += "  </a>";
        x += "</div>";

        return x;
    }
	
	protected void OnKeyPressQty(object sender, EventArgs e)
    {
		int qty = Convert.ToInt32(((TextBox)sender).Text);
		if(qty == 0)
		{
			string script = @"<script type='text/javascript'>
				alert('Item Yg Diinput Tidak Boleh 0.')</script>";

			ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script);
			((TextBox)sender).Text = "1";
		}
		else
		{
			((TextBox)sender).Text = ((TextBox)sender).Text;
		}
	}
	
	protected void OnKeyPressQtyProduct(object sender, EventArgs e)
    {
		int qty = Convert.ToInt32(((TextBox)sender).Text);
		if(qty > 1)
		{
			string script = @"<script type='text/javascript'>
				alert('Maksimal Item Yg Diinput Tidak Boleh Lebih Dari 1.')</script>";

			ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script);
		}
		else if(qty == 0)
		{
			string script = @"<script type='text/javascript'>
				alert('Maksimal Item Yg Diinput Tidak Boleh 0.')</script>";

			ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script);
		}
		else
		{
			((TextBox)sender).Text = ((TextBox)sender).Text;
		}
	}

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            saveSouvenir(d);

            d.ModalRupiah = d.SalesOrderDJ.Sum(p => p.ModalRupiah) + d.SalesOrderPG.Sum(p => p.ModalRupiah) + d.SalesOrderGJ.Sum(p => p.ModalRupiah) + d.SalesOrderLD.Sum(p => p.ModalRupiah);
            d.HargaRupiah = d.SalesOrderDJ.Sum(p => p.HargaRupiah) + d.SalesOrderPG.Sum(p => p.HargaRupiah) + d.SalesOrderGJ.Sum(p => p.HargaRupiah) + d.SalesOrderLD.Sum(p => p.HargaRupiah);
            d.TotalBayarBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalBayar) + d.SalesOrderPG.Sum(p => p.TotalBayar) + d.SalesOrderGJ.Sum(p => p.TotalBayar) + d.SalesOrderLD.Sum(p => p.TotalBayar));
            d.TotalRupiahBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalRupiah) + d.SalesOrderPG.Sum(p => p.TotalRupiah) + d.SalesOrderGJ.Sum(p => p.TotalRupiah) + d.SalesOrderLD.Sum(p => p.TotalRupiah));
            d.Discount = 0m;
            d.DiscountNominal = 0m;
            d.DiscountGift = 0m;
            d.DiscountProgram = 0m;
            d.DiscountProgramNominal = 0m;
            d.DiscountRound = 0m;
            d.TotalBayar = d.TotalBayarBeforeDisc;
            d.TotalRupiah = d.TotalRupiahBeforeDisc;
            d.TotalResell = 0m;
            d.Unpaid = d.TotalBayarBeforeDisc;
            d.Paid = 0m;
            d.Draft = false;
            d.DraftDate = DateTime.Now;
            d.Nomor = Document.Generate_Nomor_Souvenir(d);
            //d.Nomor = "";
            db.SubmitChanges();

            decimal TotalBayar = 0m;

            int IDSalesReceipt = 0;

            string NoInvoice = "";

            NoInvoice = Document.Generate_Nomor_Invoice(d);
            //NoDP = GenerateBooking(d);

            SalesReceipt s = new SalesReceipt();

            s.IDSalesOrder = myID;
            s.NoSalesOrder = d.Nomor;
            s.OperatorNama = App.UserNama;
            s.OperatorTgl = DateTime.Now;
            s.TotalBayar = TotalBayar;
            s.NomorInvoice = NoInvoice;

            db.SalesReceipt.InsertOnSubmit(s);
            db.SubmitChanges();

            d.Paid = d.Paid + TotalBayar;
            d.Unpaid = d.TotalBayar - d.Paid;
            d.StatusPembayaran = false;

            db.SubmitChanges();

            var x = db.SalesReceipt.SingleOrDefault(p => p.ID == IDSalesReceipt);

            /*if (d.Unpaid <= 0m)
            {
                var dj = from p in db.SalesOrderSouvenir
                         join q in db.StockProductDJ on p.IDProductSouvenir equals q.ID
                         where p.IDForm == myID && p.TipeProduct == 1
                         select new { p, q };
                foreach (var r in dj)
                {
                    r.q.StockActualDJ.Substorage = 7;
                    r.q.StockActualDJ.InHand = 0;
                    r.q.StatusPenjualan = 2;
                }
                var pg = from p in db.SalesOrderSouvenir
                         join q in db.StockProductPG on p.IDProductSouvenir equals q.ID
                         where p.IDForm == myID && p.TipeProduct == 3
                         select new { p, q };
                foreach (var r in pg)
                {
                    r.q.StockActualPG.Substorage = 7;
                    r.q.StockActualPG.InHand = 0;
                    r.q.StatusPenjualan = 2;
                }
                var gj = from p in db.SalesOrderSouvenir
                         join q in db.StockProductGJ on p.IDProductSouvenir equals q.ID
                         where p.IDForm == myID && p.TipeProduct == 2
                         select new { p, q };
                foreach (var r in gj)
                {
                    r.q.StockActualGJ.Substorage = 7;
                    r.q.StockActualGJ.InHand = 0;
                    r.q.StatusPenjualan = 2;
                }
                var ld = from p in db.SalesOrderSouvenir
                         join q in db.StockProductLD_Stone1B on p.IDProductSouvenir equals q.ID
                         where p.IDForm == myID && p.TipeProduct == 2
                         select new { p, q };
                foreach (var r in ld)
                {
                    r.q.StockActualLD_Stone1B.Substorage = 7;
                    r.q.StockActualLD_Stone1B.InHand = 0;
                    r.q.StatusPenjualan = 2;
                }
                //Commit
                db.SubmitChanges();
            }

            foreach (var r in d.SalesOrderSouvenir)
            {
                if (r.TipeProduct == 1)
                {
                    // Ledger 
                    StockLedgerDJ l = new StockLedgerDJ();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out
                    int Remark = 21; // See Library DB - Ledger
                    string Keterangan = r.Invoice; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (d.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                    else if (d.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                    //Input
                    l.IDProduct = r.IDProductSouvenir.Value;
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

                else if (r.TipeProduct == 2)
                {
                    // Ledger 
                    StockLedgerGJ l = new StockLedgerGJ();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out
                    int Remark = 21; // See Library DB - Ledger
                    string Keterangan = r.Invoice; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (d.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                    else if (d.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                    //Input
                    l.IDProduct = r.IDProductSouvenir.Value;
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
                else if (r.TipeProduct == 3)
                {
                    // Ledger 
                    StockLedgerPG l = new StockLedgerPG();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out
                    int Remark = 21; // See Library DB - Ledger
                    string Keterangan = r.Invoice; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (d.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                    else if (d.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                    //Input
                    l.IDProduct = r.IDProductSouvenir.Value;
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

                else if (r.IDProductSouvenir == 4)
                {
                    // Ledger 
                    StockLedgerLD l = new StockLedgerLD();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out
                    int Remark = 21; // See Library DB - Ledger
                    string Keterangan = r.Invoice; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (d.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);
                    else if (d.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi.Value, Db.Cnn_DB);

                    //Input
                    l.IDProduct = r.IDProductSouvenir.Value;
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
                    db.StockLedgerLD.InsertOnSubmit(l);
                    db.SubmitChanges();
                }
            }*/
			
			string msg = saveApprovalSouvenir(d.ID);
			
			string script = "<script type='text/javascript'> " +
				"alert('Mohon Approve PK / SV dengan Nomor " + d.Nomor + " untuk Melanjutkan Transaksi.'); " +
				"window.location.href = '../../Summary.aspx';" +
			"</script>";

			ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script);
			
            //Response.Redirect("/OutletSales/Page/Create/Payment/Sales_Packaging.aspx?&id=" + d.ID + "&sid=" + s.ID);
        }
        //Response.Redirect("Sales_Submit.aspx?id=" + myID);
        //Response.Redirect("/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?d=1&id=" + d.ID);
    }

    protected void saveSouvenir(SalesOrder d)
    {
        TextBox txt = new TextBox();
        string hf = "";
        int index2 = 1;

        foreach (TableRow row in tb3.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfsv" + index2.ToString())).Value;
                txt = (TextBox)row.FindControl("sv_" + index2.ToString());

                var s = db.SalesOrderSouvenir.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (s != null)
                {
                    /*var x = db.StockActualSouvenir.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == s.IDProduct && p.InHand > 0);
                    if (x == null) DataError();
                    else
                    {
			   
																							  
                        x.InHand = x.InHand + s.Qty.Value;
                        db.SubmitChanges();
                    }*/

                    s.Qty = Convert.ToDecimal(txt.Text);
                    s.TotalModalRupiah = s.Qty * s.ModalRupiah;
                    //x.InHand = x.InHand - Convert.ToDecimal(txt.Text);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) { }
            index2++;
        }
    }
	
	protected string saveApprovalSouvenir(int id)
	{
		string result = string.Empty;
		try
		{
			Db.Execute("EXEC sp_insert_approval_pksv '" + id + "','SV'", Db.Cnn_DB);
		}
		catch(Exception ex) 
		{ 
			result = ex.Message;
		}

		return result;
	}
}
