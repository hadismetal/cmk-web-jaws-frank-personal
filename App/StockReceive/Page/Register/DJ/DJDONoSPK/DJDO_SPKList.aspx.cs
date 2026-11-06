using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJDO_SPKList : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected string myNOBRG { get { return Request.QueryString["kdbrg"]; } }
    protected string myNOVAL { get { return Request.QueryString["kdvlds"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.KodeKaret_Detail
                 where p.ProductOrder.StatusPenerimaan == 0
                 select p;

        string keyword = kw.Text.Trim();
        if (keyword != "") rs = rs.Where(p => p.KodeKaretDetail.Contains(keyword) || p.NoSPK.Contains(keyword));
        int ProductItem = productitem.SelectedIndex;
        if (ProductItem > 0) rs = rs.Where(p => p.KodeKaretDJ.KodeKaretDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = productcategory.SelectedIndex;
        if (ProductCategory > 0) rs = rs.Where(p => p.KodeKaretDJ.KodeKaretDJ_CharProduct.ProductCategory == ProductCategory);

        if (rs != null)
        {
            int maxdata = 10;
            if (rs.Count() > maxdata) { lit_countdata.Text = "Showing " + maxdata.ToString() + " / " + rs.Count() + " data"; rs = rs.Take(maxdata); }
            
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
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (keyword == "") ? r.KodeKaretDetail : r.KodeKaretDetail.Replace(keyword, "<span style='color:blue;'>" + keyword + "</span>");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (keyword == "") ? r.NoSPK : r.NoSPK.Replace(keyword, "<span style='color:blue;'>" + keyword + "</span>");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.KodeKaretDJ.KodeKaretDJ_CharProduct != null) ? TD_Product01(r.KodeKaretDJ.KodeKaretDJ_CharProduct) : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.KodeKaretDJ.KodeKaretDJ_CharProduct != null) ? TD_Product02(r.KodeKaretDJ.KodeKaretDJ_CharProduct) : "";
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.KodeKaretDJ.KodeKaretDJ_CharProduct != null) ? TD_Product03(r.KodeKaretDJ.KodeKaretDJ_CharProduct) : "";
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
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
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&nodo=" + myNODO;
        href += "&kdbrg=" + myNOBRG;
        href += "&kdvlds=" + myNOVAL;
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(KodeKaret_Detail r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJDO_InsertSPK.aspx?idkodekaret=" + r.ID + "&nodo=" + myNODO + "&kdbrg=" + myNOBRG + "&kdvlds=" + myNOVAL + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected string TD_Data(KodeKaret_Detail r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.KodeKaretDetail + "</p>";

        return x;
    }
    protected string TD_Product01(KodeKaretDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(KodeKaretDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(KodeKaretDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.Nama;
        x += "</p>";

        return x;
    }
}