using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_Detail : System.Web.UI.Page {
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
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
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
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi,Db.Cnn_DB);

            brand.InnerHtml = Brand;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            customer.InnerHtml = d.DataCustomer.Nama;
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			
			//Stone
			fillItem();

            nilaiso.Text = d.HargaRupiah.Value.ToString("N0");
            discount.Text = (d.HargaRupiah.Value - d.TotalBayar.Value).ToString("N0");
            totalbayar.Text = (d.TotalBayar.Value).ToString("N0");
            terbayar.Text = d.Paid.Value.ToString("N0");
            kekurangan.Text = d.Unpaid.Value.ToString("N0");

            invoice.Visible = false;
            booking.Visible = false;

            if (d.StatusPembayaran == true)
                invoice.Visible = true;
            else
                booking.Visible = true;
		}
	}

	//Stone
    protected void fillItem()
    {
        decimal GT = 0m;
        decimal GT2 = 0m;
        //Item
        fillItemDJ(ref GT, ref GT2);
        fillItemPG(ref GT, ref GT2);
        fillItemGJ(ref GT, ref GT2);

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

            decimal Discount = 0m;
            Discount = r.DiscountNominal.Value + r.DiscountProgramNominal.Value + r.DiscountGift.Value + r.DiscountRound.Value;

            td = new TableCell();
            td.Text = Convert.ToDecimal(Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_HyperLink(r);
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

            Sub_Total(ST, ST2, false);
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

            decimal Discount = 0m;
            Discount = r.DiscountNominal.Value + r.DiscountProgramNominal.Value + r.DiscountGift.Value + r.DiscountRound.Value;

            td = new TableCell();
            td.Text = Convert.ToDecimal(Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_HyperLink(r);
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
    protected void PrintSO_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Accounting/Page/Create/Payment/Sales_Invoice.aspx?id=" + myID);
    }
    protected void PrintBooking_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Accounting/Page/Create/Payment/Sales_DP.aspx?id=" + myID);
    }
    protected string TD_HyperLink(SalesOrderDJ r)
    {
        string x = string.Empty;


        if (r.SalesOrder.StatusPembayaran == true)
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Print_InvoiceDJ.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Invoice";
            x += "</a>";
            x += "<br /><br /><a class=\"btn btn-info\" target=\"_blank\" href=\"Print_Certificate.aspx?id=" + r.StockProductDJ.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Certificate";
            x += "</a>";
        }
        return x;
    }
    protected string TD_HyperLink(SalesOrderPG r)
    {
        string x = string.Empty;


        if (r.SalesOrder.StatusPembayaran == true)
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Print_InvoicePG.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Print Invoice";
            x += "</a>";
        }
        return x;
    }
}