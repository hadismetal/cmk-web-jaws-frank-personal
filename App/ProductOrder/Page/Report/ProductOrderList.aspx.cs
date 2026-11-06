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

            //Form-Default
            dari.Text = Str.Tgl(DateDrv.AwalBulan());
            sampai.Text = Str.Tgl(DateTime.Today);

            DLC.CharDesignCategory(designcategory);
            DLC.CharDesignConcept(designconcept);
            DLC.CharTargetAge(targetage);
            DLC.CharTargetGender(targetgender);
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            Lib.Designer(designer);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.ProductOrder
                 where p.Draft == false
                    && p.Tgl >= Convert.ToDateTime(dari.Text) 
                    && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                 select p;

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.NoSPK.Contains(keyword) || p.KodeKaretLama.Contains(keyword) || p.KodeKaret.Contains(keyword) || p.KodeKaret_Detail.Any(z=>z.KodeKaretDetail.Contains(keyword)));
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

        int statusorder = Convert.ToInt32(statorder.SelectedValue);
        if (statusorder > 0) rs = rs.Where(p => p.StatusOrder == statusorder);

        if (status.SelectedValue == "1")
            rs = rs.Where(p => p.Approval != null);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.Approval == null);
        
        rs = rs.OrderBy(p => p.Operator).ThenBy(p => p.KodeKaretLama).ThenBy(p => p.NoSPK);

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
                td.Text = TD_HyperLink(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = index++.ToString() + ".";
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                td = new TableCell();
                //td.Text = "<p><img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" /></p>";
                td.Text = Img.ResizeImageToBase64Html(r.ImgPicture, 30, 30);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Data(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.Approval != null) ? r.Approval + "<br />" + Str.FullDate(r.ApprovalTgl) : "";
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                if (r.ProductOrder_CharProduct != null)
                {
                    td = new TableCell();
                    td.Text = r.DataHuman.Nama;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.ProductOrder_CharProduct.CharProductItem.Nama);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.ProductOrder_CharProduct.CharProductCategory.Nama);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.CharProductSegmen.Nama);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_KelasHargaNew(r);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
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
            designcategory.SelectedValue = Request.QueryString["designcategory"];
            designconcept.SelectedValue = Request.QueryString["designconcept"];
            targetage.SelectedValue = Request.QueryString["targetage"];
            targetgender.SelectedValue = Request.QueryString["targetgender"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            designer.Text = Request.QueryString["designer"];
            statorder.SelectedValue = Request.QueryString["statusorder"];
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
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&designer=" + designer.Text;
        href += "&statusorder=" + statorder.Text;
        Response.Redirect(href);
    }
    protected string TD_HyperLink(ProductOrder r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"SPK_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        if (r.Approval == null)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"SPK_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-remove\"></i> ";
            x += "  Delete";
            x += "</a>";
        }
        return x;
    }
    protected string TD_Data(ProductOrder r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.NoSPK + "</p>";
        if (r.KodeKaretLama != null)
            x += "<p class=\"text-sm text-muted\">" + r.KodeKaretLama
                + ((r.KodeKaretRepeat != null && r.KodeKaretRepeat != 0) ? ("/" + r.KodeKaretRepeat.ToString().PadLeft(4, '0')) : "")
                + "</p>";

        try
        {
            if (r.IDKodeKaret != null)
            {
                var kode = db.KodeKaret_Detail.SingleOrDefault(p => p.IDProductOrder == r.ID);
                if (kode != null)
                    x += "<p class=\"text-sm text-muted\">" + kode.KodeKaretDetail + "</p>";
            }
        }
        catch (Exception ex) { }
        
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

        x += "<p class=\"text-md\">";
        x += text;
        x += "</p>";

        return x;
    }

    protected string TD_Designer(string text)
    {
        string x = string.Empty;

        x += "<p class=\"text-md text-primary\">";
        x += text;
        x += "</p>";

        return x;
    }

    protected string TD_KelasHargaNew(ProductOrder d)
    {
        decimal harga = 0m;

        try
        {
            harga = (decimal) d.ProductOrder_PricingMU.HargaRupiahRound;
        }
        catch (Exception) { }
            
            //(Convert.ToDecimal((from p in db.ProductOrder_PricingMU where p.ID == myID select p.HargaUSD).SingleOrDefault()));
        
        string kelas_harga = "";
        if (d.ProductOrder_CharProduct.CharStoneDist.NamaKode == "SOL"
            && (d.ProductOrder_CharProduct.CharProductItem.NamaKode.Equals("A")
                || d.ProductOrder_CharProduct.CharProductItem.NamaKode.Equals("L")
                || d.ProductOrder_CharProduct.CharProductItem.NamaKode.Equals("M")
                || d.ProductOrder_CharProduct.CharProductItem.NamaKode.Equals("W")
            ))
        {
            // cetak kelas harga SOLITAIRE jika masuk diantara 4 item : 
            /*
            52	Earrings	A
            63	Pendant	L
            64	Wedding Ring	M
            73	Ladies Ring	W
            */

            kelas_harga = "SOLITAIRE";
        }
        else
        {

            if (harga <= 3000000)
            {
                kelas_harga = "INTRO";
            }
            else if ((harga > 3000000) && (harga <= 10000000))
            {
                kelas_harga = "LOW END";
            }
            else if ((harga > 10000000) && (harga <= 30000000))
            {
                kelas_harga = "MIDDLE END";
            }
            else if ((harga > 30000000) && (harga <= 100000000))
            {
                kelas_harga = "HIGH END";
            }
            else if ((harga > 100000000) && (harga <= 300000000))
            {
                kelas_harga = "TOP END";
            }
            else if (harga > 300000000)
            {
                kelas_harga = "SUPER END";
            }

        }

        string x = string.Empty;

        x += "<p class=\"text-md text-primary\">";
        x += kelas_harga;
        x += "</p>";

        return x;
    }
	protected void refreshPage_Excel(object sender, EventArgs e)
    {
        string href = "ProductOrderList_Export.aspx?refresh_Page=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&targetage=" + targetage.Text;
        href += "&targetgender=" + targetgender.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&designer=" + designer.Text;
        href += "&statusorder=" + statorder.Text;
        Response.Redirect(href);
    }
}