using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Titipan : System.Web.UI.Page {
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
			kw.Focus(); //Keyword

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
        href += "&id=" + myID;
		href += "&kw=" + kw.Text;
        href += "&searchby=" + searchby.SelectedValue;

		Response.Redirect(href);
	}
    protected void AddPLU(object sender, EventArgs e)
    {
        string href = "Titipan_CustomerForm.aspx?id=" + myID;

        Response.Redirect(href);
    }
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
            searchby.SelectedValue = Request.QueryString["searchby"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
        fillDJ();

		
	}

    protected void fillDJ()
    {
        int SearchType = 1;

        if (kw.Text.Contains("D"))
            SearchType = 1;
        else if (kw.Text.Contains("P"))
            SearchType = 2;

        if (SearchType == 1)
        {
            var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID);

            //SELECT
            var rs = from p in db.SalesOrderDJ
                     where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && p.SalesOrder.IDCustomer == d.IDCustomer
                     select p;

            int SearchBy = Convert.ToInt32(searchby.SelectedValue);
            if (SearchBy == 1)
                rs = rs.Where(p => p.SalesOrder.Nomor.Contains(kw.Text));
            else
                rs = rs.Where(p => p.StockProductDJ.Nomor.Contains(kw.Text));

            rs = rs.OrderBy(p => p.SalesOrder.Tgl.Value);

            //NO-RESULT
            if (rs.Count() == 0) App.TableNoResult(tb);

            //RESULT
            int prev = 0;
            int index = 1;
            if (rs.Count() == 1)
            {
                Response.Redirect("Titipan_ItemAddSave.aspx?id=" + myID + "&item=1&iditem=" + rs.First().IDProduct);
            }
            else
            {
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;


                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.IDProduct, 1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<img src=\"/Images.aspx?path=" + r.StockProductDJ.ImgPicture + "\" border=\"0\" width=\"100\" />";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SalesOrder.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
        }
    }

    
	protected string TD_HyperLink(int ID, int itemindex) {
        string x = string.Empty;
        string href = string.Empty;

        href += "Titipan_ItemAddSave.aspx";
        href += "?id=" + myID;
        href += "&item=" + itemindex;
        href += "&iditem=" + ID;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
	}
	protected string TD_Nama(LocOutlet r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_Addr(LocOutlet r) {
		string x = string.Empty;

		if (r.AddrAlamat != "" && r.AddrAlamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

		return x;
	}
	protected string TD_Group(LocOutlet r) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + r.LocOutletGroup.NamaKode + "</p>";

		return x;
	}


    protected void Sub_Header(LocExhibition r)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += r.CompanyBrand.Nama + " (" + r.CompanyBrand.NamaKode + ")";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(LocExhibition r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Titipan_Form.aspx?idlokasi=" + r.ID + "&tipe=2\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(LocExhibition r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";

        return x;
    }
    protected string TD_Addr(LocExhibition r)
    {
        string x = string.Empty;

        if (r.AddrAlamat != "" && r.AddrAlamat != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

        return x;
    }
    protected string TD_Group(LocExhibition r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.LocExhibitionGroup.NamaKode + "</p>";

        return x;
    }
}