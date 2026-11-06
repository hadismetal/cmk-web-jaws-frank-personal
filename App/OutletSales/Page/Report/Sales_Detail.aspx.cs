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
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            //Trans. Form
            string Brand = "";
            if (d.TipeLokasi == 1)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
            else if (d.TipeLokasi == 2)
                Brand = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

            brand.InnerHtml = Brand;
            keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            customer.InnerHtml = d.DataCustomer.Nama + " (" + d.DataCustomer.NoCustomer + ")"; 
            tgl.InnerHtml = Str.Tgl(d.Tgl);


            //Stone
            fillItem();

            decimal NilaiSO = 0m;
            decimal Paid = 0m;
            decimal Unpaid = 0m;

            try
            {
                NilaiSO = d.TotalBayar.Value;
            }
            catch (Exception ex) { }
            try
            {
                Paid = d.Paid.Value;
            }
            catch (Exception ex) { }
            try
            {
                Unpaid = d.Unpaid.Value;
            }
            catch (Exception ex) { }

            nilaiso.Text = NilaiSO.ToString("N0");
            discount.Text = ((d.DiscountNominal.HasValue ? d.DiscountNominal.Value : 0) + (d.DiscountProgramNominal.HasValue ? d.DiscountProgramNominal.Value : 0) + (d.DiscountGift.HasValue ? d.DiscountGift.Value : 0) + (d.DiscountRound.HasValue ? d.DiscountRound.Value : 0)).ToString("N0");
            totalbayar.Text = (d.TotalBayar.HasValue ? (d.TotalBayar.Value).ToString("N0") : "0");
            terbayar.Text = Paid.ToString("N0");
            kekurangan.Text = Unpaid.ToString("N0");
            resell.Text = d.TotalResell.HasValue ? d.TotalResell.Value.ToString("N0") : "0";
            pembayaran.Text = "0";

            invoice.Visible = false;
            booking.Visible = false;

            if (d.StatusPembayaran == true)
                invoice.Visible = true;
            else
                booking.Visible = true;

            fillDP();
            fillPembayaran();
        }
	}

    protected void fillDP()
    {
        
    }

    protected void fillPembayaran()
    {
        var rs = from p in db.SalesReceiptDetail
                 where p.SalesReceipt.IDSalesOrder.Value == myID
                 select p;

        if (rs.Count() == 0) App.TableNoResult(tb3);
        else
        {
            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.PaymentType.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nominal.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : "-";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDEDC.HasValue ? r.EDCList.Nama : "-";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CCName;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CCNumber;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDProgram.HasValue ? r.ProgramCicilan.Nama : "-";
                tr.Cells.Add(td);

                tb3.Rows.Add(tr);

                index++;
            }
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
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            decimal DiscountPromo = 0m;
            DiscountPromo = r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0;

            td = new TableCell();
            td.Text = (Convert.ToDecimal(r.DiscountRound) + Convert.ToDecimal(DiscountPromo)).ToString("N0");
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

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (Convert.ToDecimal(r.DiscountRound) + Convert.ToDecimal(r.DiscountPromo.Value)).ToString("N0");
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
        var rs = from p in db.SalesOrderGJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(2);

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
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (Convert.ToDecimal(r.DiscountRound) + (r.DiscountPromo.HasValue ? Convert.ToDecimal(r.DiscountPromo.Value) : 0)).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_HyperLink(r);
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
        if (rs.Count() > 0) Sub_Header(3);

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
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountRound).ToString("N0");
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
    protected void PrintSO_Click(object sender, EventArgs e)
    {
        Response.Redirect("/OutletSales/Page/Create/Payment/Sales_Invoice.aspx?id=" + myID);
    }
    protected void PrintBooking_Click(object sender, EventArgs e)
    {
        Response.Redirect("/OutletSales/Page/Create/Payment/Sales_DP.aspx?id=" + myID);
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
    protected string TD_HyperLink(SalesOrderGJ r)
    {
        string x = string.Empty;


        if (r.SalesOrder.StatusPembayaran == true)
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Print_InvoiceGJ.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Invoice";
            x += "</a>";
            x += "<br /><br /><a class=\"btn btn-info\" target=\"_blank\" href=\"Print_Certificate.aspx?id=" + r.StockProductGJ.ID + "\">";
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
    protected string TD_HyperLink(SalesOrderLD r)
    {
        string x = string.Empty;


        if (r.SalesOrder.StatusPembayaran == true)
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Print_InvoiceLD.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Print Invoice";
            x += "</a>";
        }
        return x;
    }
}