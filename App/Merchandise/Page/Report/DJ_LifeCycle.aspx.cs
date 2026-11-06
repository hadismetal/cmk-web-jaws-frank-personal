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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            //datepicker.Text = Str.Tgl(DateTime.Now);
            //datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductDJ
                 //where !p.Draft.Value && p.Legacy == null
                        
                 select p;

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        if (kw.Text != "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(kw.Text) || p.ProtoNomor.Contains(kw.Text)
                 select p;
        }
        
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
        //int Brand = Convert.ToInt32(brand.SelectedValue);
        //if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);

        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 1;

        rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

        if (rs.Count() > 0)
        {
            int index = 1;

            foreach (var r in rs)
            {

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Hyperlink(r.Nomor);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                int Proto = 0;
                int SPKDay = 0;
                int LDKPDay = 0;
                int SalesDay = 0;
                string DesignDate = "-";
                string SPKDate = "-";
                string LDKPDate = "-";
                string ReceivingDate = "-";
                int ReceivingDay = 0;
                string SalesDate = "-";
                if (r.Proto != 0)
                {
                    Proto = r.Proto.Value;
                    var spk = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == Proto);
                    DesignDate = Str.Tgl(spk.ProductOrder.ProductDesign.ApprovalTgl.Value);
                    SPKDate = Str.Tgl(spk.ProductOrder.ApprovalTgl.Value);
                    SPKDay = Convert.ToInt32((spk.ProductOrder.ApprovalTgl.Value - spk.ProductOrder.ProductDesign.ApprovalTgl.Value).TotalDays);
                    
                    try
                    {
                        LDKPDate = Str.Tgl(spk.ProductOrder.TglLDKP.Value);
                        LDKPDay = Convert.ToInt32((spk.ProductOrder.TglLDKP.Value - spk.ProductOrder.ApprovalTgl.Value).TotalDays);
                    }
                    catch (Exception ex) { }

                    try
                    {
                        ReceivingDate = Str.Tgl(r.ApprovalTgl.Value);
                        ReceivingDay = Convert.ToInt32((r.ApprovalTgl.Value -spk.ProductOrder.TglLDKP.Value).TotalDays);
                    }
                    catch (Exception ex) { }

                    var sales = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == r.ID && p.SalesOrder.StatusVoid == false);

                    try
                    {
                        SalesDate = Str.Tgl(sales.SalesOrder.Tgl.Value);
                        SalesDay = Convert.ToInt32((sales.SalesOrder.Tgl.Value - r.ApprovalTgl.Value).TotalDays);
                    }
                    catch (Exception ex) { }
                }

                td = new TableCell();
                td.Text = DesignDate;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = SPKDate;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = SPKDay.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = LDKPDate;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = LDKPDay.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ReceivingDate;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ReceivingDay.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = SalesDate;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = SalesDay.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&page=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&productitem=" + productitem.Text;
        href2 += "&productcategory=" + productcategory.Text;
        href2 += "&productlevel=" + productlevel.Text;
        href2 += "&page=" + (pageNumber - 1).ToString();

        string paging = "";

        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href + "\">Next</a>";
        paging += "</div>";

        paging_text.Text = paging;
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
        }
    }

    protected string TD_Hyperlink(string PLU)
    {
        string x = string.Empty;

        x += "<a target='_blank' href='DJ_Detail.aspx?refreshPage=1&kw=" + PLU + "'>" + PLU + "</a>";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
       // href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}