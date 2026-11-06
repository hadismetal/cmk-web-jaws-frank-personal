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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.CharDesignCategory(designcategory);
            DLC.CharDesignConcept(designconcept);
            DLC.CharTargetAge(targetage);
            DLC.CharTargetGender(targetgender);
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.Designer(designer);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        decimal TotalEmas = 0m;
        decimal TotalStone = 0m;

        var rs = from p in db.ProductOrder
                 where p.Draft == false && p.Approval != null && p.StatusPenerimaan == 0
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.NoSPK == "" + keyword + "" || p.KodeKaretLama == "" + keyword + "");
        int DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        if (DesignCategory > 0) rs = rs.Where(p => p.ProductOrder_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        if (DesignConcept > 0) rs = rs.Where(p => p.ProductOrder_CharDesign.DesignConcept == DesignConcept);
        int TargetAge = Convert.ToInt32(targetage.SelectedValue);
        if (TargetAge > 0) rs = rs.Where(p => p.ProductOrder_CharDesign.TargetAge == TargetAge);
        int TargetGender = Convert.ToInt32(targetgender.SelectedValue);
        if (TargetGender > 0) rs = rs.Where(p => p.ProductOrder_CharDesign.TargetGender == TargetGender);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.ProductOrder_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.ProductOrder_CharProduct.ProductCategory == ProductCategory);
        int Designer = Convert.ToInt32(designer.SelectedValue);
        if (Designer > 0) rs = rs.Where(p => p.DataHuman.ID == Designer);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl <= Convert.ToDateTime(sampai.Text));
        }

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
                td.Text = "<p><img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" /></p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Data(r);
                tr.Cells.Add(td);

                if (r.ProductOrder_CharProduct != null)
                {
                    td = new TableCell();
                    td.Text = TD_Product01(r.ProductOrder_CharProduct);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Product02(r.ProductOrder_CharProduct);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.ProductOrder_CharProduct.CharProductCategory.Nama);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.CharProductSegmen.Nama);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.DataHuman.Nama);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    decimal Emas = 0m;
                    try
                    {
                        Emas = r.ProductOrder_PricingProduct.TotalProduct;
                    }
                    catch (Exception ex) { }

                    decimal Stone = 0m;
                    try
                    {
                        Stone = r.ProductOrder_PricingProduct.Total;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = Emas.ToString("N2");
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Stone.ToString("N2");
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    TotalEmas += Emas;
                    TotalStone += Stone;
                }

                tb.Rows.Add(tr);
            }
            
            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 8;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalEmas.ToString("N2")  + "</b>";
            td2.CssClass = "text-center";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalStone.ToString("N2") + "</b>";
            td2.CssClass = "text-center";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            designcategory.SelectedValue = Request.QueryString["designcategory"];
            designconcept.SelectedValue = Request.QueryString["designconcept"];
            targetage.SelectedValue = Request.QueryString["targetage"];
            targetgender.SelectedValue = Request.QueryString["targetgender"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            designer.Text = Request.QueryString["designer"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&targetage=" + targetage.Text;
        href += "&targetgender=" + targetgender.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&designer=" + designer.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "ProductOrder_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&targetage=" + targetage.Text;
        href += "&targetgender=" + targetgender.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&designer=" + designer.Text;

        Response.Redirect(href);
    }

    protected string TD_Data(ProductOrder r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.NoSPK + "</p>";
        if (r.KodeKaretLama != null)
            x += "<p class=\"text-sm text-muted\">" + r.KodeKaretLama + "</p>";
        if (r.IDKodeKaret != null)
        {
            var kode = db.KodeKaret_Detail.SingleOrDefault(p => p.IDProductOrder == r.ID);
            if(kode!=null)
                x += "<p class=\"text-sm text-muted\">" + kode.KodeKaretDetail + "</p>";
        }

        return x;
    }

    protected string TD_Product01(ProductOrder_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }

    protected string TD_Product02(ProductOrder_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }

    protected string TD_Product03(ProductOrder_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.NamaKode;
        x += "</p>";

        return x;
    }

    protected string TD_ProductChar(string text)
    {
        string x = string.Empty;

        x += "<p class=\"text-orange\">";
        x += text;
        x += "</p>";

        return x;
    }
}