using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Resell_Detail : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Resell.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.Resell.SingleOrDefault(p => p.ID == myID);
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
            td.Text = TD_HyperLink(0, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            decimal HargaJual = 0m;
            DateTime sales = r.SalesOrder.Tgl.Value;
            DateTime resell = r.Resell.Tgl.Value;

            HargaJual = Db.SingleDecimal("SELECT TotalBayar FROM SalesOrderDJ WHERE IDForm=" + r.SalesOrder.ID + " AND IDProduct = " + r.IDProduct, Db.Cnn_DB);

            td = new TableCell();
            td.Text = Convert.ToDecimal(HargaJual).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(100 - ((r.HargaRupiah.Value / HargaJual) * 100)).ToString("N0") + "%";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (resell - sales).TotalDays.ToString() + " hari";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaAcuan.Value);
            ST2 += Convert.ToDecimal(r.HargaRupiah.Value);

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
            td.Text = TD_HyperLink(1, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.CharProductItem.Nama;
            tr.Cells.Add(td);

            decimal HargaJual = 0m;
            DateTime sales = r.SalesOrder.Tgl.Value;
            DateTime resell = r.Resell.Tgl.Value;

            HargaJual = Db.SingleDecimal("SELECT TotalBayar FROM SalesOrderDJ WHERE IDForm=" + r.SalesOrder.ID + " AND IDProduct = " + r.IDProduct, Db.Cnn_DB);

            td = new TableCell();
            td.Text = Convert.ToDecimal(HargaJual).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(100 - ((r.HargaRupiah.Value / HargaJual) * 100)).ToString("N0") + "%";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (resell - sales).TotalDays.ToString() + " hari";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaAcuan.Value);
            ST2 += Convert.ToDecimal(r.HargaRupiah.Value);
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
        td.ColumnSpan = 6;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalBayar) + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
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
    protected string TD_HyperLink(int ItemIndex, int ID, int IDProduct)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (ItemIndex == 0)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"DJ_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        else if (ItemIndex == 1)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"PG_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        x += "</div>";

        return x;
    }
}