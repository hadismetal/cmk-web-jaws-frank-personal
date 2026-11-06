using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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

		if (!IsPostBack) {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharStoneDist(stonedistribution);
            DLC.CharFrameMaterial(framematerial);
            DLC.CharFrameFinishing(framefinishing);
            DLC.CharFrameColor(framecolor);
            DLC.CharProcessCons(processconstruction);
            DLC.CompanyBrand(brand);

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

        var rs = from p in db.StockReceiveDJ
                 where p.Nomor!="" || p.Nomor != null
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end);
        }

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.Nomor.Contains(keyword) || p.ProtoNomor.Contains(keyword) || p.SupplierNomor.Contains(keyword));


        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
        int StoneDistribution = Convert.ToInt32(stonedistribution.SelectedValue);
        if (StoneDistribution > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.StoneDist == StoneDistribution);
        int FrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
        if (FrameMaterial > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.FrameMaterial == FrameMaterial);
        int FrameFinishing = Convert.ToInt32(framefinishing.SelectedValue);
        if (FrameFinishing > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.FrameFinishing == FrameFinishing);
        int FrameColor = Convert.ToInt32(framecolor.SelectedValue);
        if (FrameColor > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.FrameColor == FrameColor);
        int ProcessConstruction = Convert.ToInt32(processconstruction.SelectedValue);
        if (ProcessConstruction > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProcessCons == ProcessConstruction);
        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);
        int Status = Convert.ToInt32(status.SelectedValue);
        if (Status == 1)
            rs = rs.Where(p => p.Approval != null);
        else if (Status == 2)
            rs = rs.Where(p => p.Draft);
        else if (Status == 3)
            rs = rs.Where(p => p.Approval == null && p.Draft == false);
            if (rs != null)
            {
                int prev = 0;
                int index = 1;
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Hyperlink(r);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Data(r);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Tgl(r.Tgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Operator;
                    tr.Cells.Add(td);

                    string TglApprove = "";
                    TglApprove = r.ApprovalTgl.HasValue ? Str.Tgl(r.ApprovalTgl.Value) : "-";

                    td = new TableCell();
                    td.Text = TglApprove;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Approval;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SupplierNomor;
                    tr.Cells.Add(td);


                    td = new TableCell();
                    td.Text = r.SupplierNama;
                    tr.Cells.Add(td);
                    

                    if (r.StockReceiveDJ_CharProduct != null)
                    {
                        td = new TableCell();
                        td.Text = TD_Product01(r.StockReceiveDJ_CharProduct);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Product02(r.StockReceiveDJ_CharProduct);
                        td.CssClass = "text-center";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Product03(r.StockReceiveDJ_CharProduct);
                        td.CssClass = "text-center";
                        tr.Cells.Add(td);
                    }

                    tb.Rows.Add(tr);
                }
            }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            brand.SelectedValue = Request.QueryString["brand"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            stonedistribution.SelectedValue = Request.QueryString["stonedistribution"];
            framematerial.SelectedValue = Request.QueryString["framematerial"];
            framefinishing.SelectedValue = Request.QueryString["framefinishing"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            processconstruction.SelectedValue = Request.QueryString["processconstruction"];
            status.SelectedValue = Request.QueryString["status"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&brand=" + brand.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&stonedistribution=" + stonedistribution.SelectedValue;
        href += "&framematerial=" + framematerial.SelectedValue;
        href += "&framefinishing=" + framefinishing.SelectedValue;
        href += "&framecolor=" + framecolor.SelectedValue;
        href += "&processconstruction=" + processconstruction.SelectedValue;
        href += "&status=" + status.SelectedValue;


        Response.Redirect(href);
    }

    protected string TD_Hyperlink(StockReceiveDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        x += "</a>";

        return x;
    }

    protected string TD_Data(StockReceiveDJ r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.Nomor + "</p>";
        if (r.ProtoNomor != null)
            x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";
        if (r.ProductOrderNomor != null)
            x += "<p class=\"text-sm\">" + r.ProductOrderNomor + "</p>";
        if (r.SupplierNomor != null)
            x += "<p class=\"text-sm\">" + r.SupplierNomor + "</p>";

        return x;
    }
    protected string TD_Product01(StockReceiveDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(StockReceiveDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(StockReceiveDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.NamaKode;
        x += "</p>";

        return x;
    }
}