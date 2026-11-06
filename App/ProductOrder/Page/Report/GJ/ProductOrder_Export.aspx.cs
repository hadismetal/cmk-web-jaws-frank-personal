using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=ProductOrderGJGJ_Export.xls");

            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        decimal TotalEmas = 0m;
        decimal TotalStone = 0m;
        decimal TotalEstimasi = 0m;
        decimal TotalBeratEmas = 0m;
        decimal TotalCaratStone = 0m;

        var rs = from p in db.ProductOrderGJ
                 where p.Draft == false && p.Approval != null && p.StatusPenerimaan == 0
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.NoSPK == "" + keyword + "" || p.KodeKaretLama == "" + keyword + "");
        int DesignCategory = Convert.ToInt32(Request.QueryString["designcategory"]);
        if (DesignCategory > 0) rs = rs.Where(p => p.ProductOrderGJ_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = Convert.ToInt32(Request.QueryString["designconcept"]);
        if (DesignConcept > 0) rs = rs.Where(p => p.ProductOrderGJ_CharDesign.DesignConcept == DesignConcept);
        int TargetAge = Convert.ToInt32(Request.QueryString["targetage"]);
        if (TargetAge > 0) rs = rs.Where(p => p.ProductOrderGJ_CharDesign.TargetAge == TargetAge);
        int TargetGender = Convert.ToInt32(Request.QueryString["targetgender"]);
        if (TargetGender > 0) rs = rs.Where(p => p.ProductOrderGJ_CharDesign.TargetGender == TargetGender);
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        if (ProductItem > 0) rs = rs.Where(p => p.ProductOrderGJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        if (ProductCategory > 0) rs = rs.Where(p => p.ProductOrderGJ_CharProduct.ProductCategory == ProductCategory);
        int Designer = Convert.ToInt32(Request.QueryString["designer"]);
        if (Designer > 0) rs = rs.Where(p => p.DataHuman.ID == Designer);
        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]));
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
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Data(r);
                tr.Cells.Add(td);

                if (r.ProductOrderGJ_CharProduct != null)
                {
                    td = new TableCell();
                    td.Text = TD_Product01(r.ProductOrderGJ_CharProduct);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.ProductOrderGJ_CharProduct.CharProductCategory.Nama);
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
                    decimal BeratEmas = 0m;
                    try
                    {
                        BeratEmas = r.ProductOrderGJ_CharProduct.NettoWeight;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = BeratEmas.ToString("N2").Replace(".","").Replace(",",".");
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    
                    TotalEmas += Emas;
                    TotalBeratEmas += BeratEmas;
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
            td2.ColumnSpan = 6;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalBeratEmas.ToString("N2").Replace(".","").Replace(",",".") + "</b>";
            td2.CssClass = "text-center";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
    protected string TD_Data(ProductOrderGJ r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.NoSPK + "</p>";
        if (r.KodeKaretLama != null)
            x += "<p class=\"text-sm text-muted\">" + r.KodeKaretLama + "</p>";
        if (r.IDKodeKaret != null)
        {
            var kode = db.KodeKaretGJ_Detail.SingleOrDefault(p => p.IDProductOrder == r.ID);
            if (kode != null)
                x += "<p class=\"text-sm text-muted\">" + kode.KodeKaretDetail + "</p>";
        }

        return x;
    }
    protected string TD_Product01(ProductOrderGJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(ProductOrderGJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(ProductOrderGJ_CharProduct r)
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