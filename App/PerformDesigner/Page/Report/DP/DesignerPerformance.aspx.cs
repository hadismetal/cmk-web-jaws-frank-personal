using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;


public partial class Page_Report_DP_DesignerPerformance : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            DLC.CharDesignCategory(designcategory);
            DLC.CharDesignConcept(designconcept);
            DLC.CharProductItem(productitem);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);
            DLC.Designer(designer);
            DLC.CharStoneDist(stonedistribution);
            DLC.CharProductSegmen(segmen);

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
        var rs = from design in db.ProductDesign
                 join order in db.ProductOrder on design.ID equals order.IDProductDesign
                 join karetDJ in db.KodeKaretDJ on order.ID equals karetDJ.IDProductOrder
                 join karetDetail in db.KodeKaret_Detail on karetDJ.ID equals karetDetail.IDKodeKaret
                 join receiveDJ in db.StockReceiveDJ on karetDetail.ID equals receiveDJ.Proto
                 select new {Nomor = karetDJ.Nomor, Designer = design.HumanDesignerNama,DataHuman = design.DataHuman,
                 ProductDesign_CharDesign = karetDJ.KodeKaretDJ_CharDesign,
                 ProductDesign_CharProduct = karetDJ.KodeKaretDJ_CharProduct,
                 ProductDesign_PricingSegmen = design.ProductDesign_PricingSegmen,
                 Approval = design.Approval,Tgl = design.Tgl,ImgPicture = design.ImgPicture
                 };

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "");
        int DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        if (DesignCategory > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        if (DesignConcept > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignConcept == DesignConcept);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductItem == ProductItem);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductLevel == ProductLevel);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductCategory == ProductCategory);
        int Designer = Convert.ToInt32(designer.SelectedValue);
        if (Designer > 0) rs = rs.Where(p => p.DataHuman.ID == Designer);

        int StoneDist = Convert.ToInt32(stonedistribution.SelectedValue);
        if (StoneDist > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.StoneDist == StoneDist);
        int ProductSegmen = Convert.ToInt32(segmen.SelectedValue);
        if (ProductSegmen > 0) rs = rs.Where(p => p.ProductDesign_PricingSegmen.ProductSegmen == ProductSegmen);

        if (status.SelectedValue == "1")
            rs = rs.Where(p => p.Approval != null);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.Approval == null);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl <= Convert.ToDateTime(sampai.Text));
        }

        var rsg = rs.GroupBy(p => p.Nomor);

        if (rsg != null)
        {
            int index = 1;
            foreach (var r in rsg)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().ProductDesign_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().ProductDesign_CharProduct.CharProductLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().ProductDesign_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().ProductDesign_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().ProductDesign_CharDesign.CharDesignCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().ProductDesign_CharDesign.CharDesignConcept.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().ProductDesign_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().ProductDesign_CharDesign.Naming;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().DataHuman.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Count().ToString();
                tr.Cells.Add(td);

                var spDJ = from design in db.ProductDesign
                           join order in db.ProductOrder on design.ID equals order.IDProductDesign
                           join karetDJ in db.KodeKaretDJ on order.ID equals karetDJ.IDProductOrder
                           join karetDetail in db.KodeKaret_Detail on karetDJ.ID equals karetDetail.IDKodeKaret
                           join receiveDJ in db.StockReceiveDJ on karetDetail.ID equals receiveDJ.Proto
                           join productDJ in db.StockProductDJ on receiveDJ.Nomor equals productDJ.Nomor
                           where karetDJ.Nomor == r.First().Nomor && productDJ.StatusPenjualan == 2
                           select new { Nomor = karetDJ.Nomor};

                td = new TableCell();
                td.Text = spDJ.Count().ToString();
                tr.Cells.Add(td);

                var DJ = from design in db.ProductDesign
                           join order in db.ProductOrder on design.ID equals order.IDProductDesign
                           join karetDJ in db.KodeKaretDJ on order.ID equals karetDJ.IDProductOrder
                           join karetDetail in db.KodeKaret_Detail on karetDJ.ID equals karetDetail.IDKodeKaret
                           join receiveDJ in db.StockReceiveDJ on karetDetail.ID equals receiveDJ.Proto
                           join productDJ in db.StockProductDJ on receiveDJ.Nomor equals productDJ.Nomor
                           join salesDJ in db.SalesOrderDJ on productDJ.ID equals salesDJ.IDProduct
                           where karetDJ.Nomor == r.First().Nomor && productDJ.StatusPenjualan == 2
                           select new { TotalBayar = salesDJ.TotalBayar };

                td = new TableCell();
                td.Text = Convert.ToDecimal(DJ.Sum(p=>p.TotalBayar)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p><img src=\"/Images.aspx?path=" + r.First().ImgPicture + "\" width=\"100\" /></p>";
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
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            designer.SelectedValue = Request.QueryString["designer"];
            stonedistribution.SelectedValue = Request.QueryString["stonedistribution"];
            segmen.SelectedValue = Request.QueryString["segmen"];
            designcategory.SelectedValue = Request.QueryString["designcategory"];
            designconcept.SelectedValue = Request.QueryString["designconcept"];
            status.SelectedValue = Request.QueryString["orderby"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productitem=" + productitem.Text;
        href += "&designer=" + designer.Text;
        href += "&stonedistribution=" + stonedistribution.Text;
        href += "&segmen=" + segmen.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&orderby=" + status;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DesignerPerformance_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productitem=" + productitem.Text;
        href += "&designer=" + designer.Text;
        href += "&stonedistribution=" + stonedistribution.Text;
        href += "&segmen=" + segmen.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&orderby=" + status;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "DesignerPerformance_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productitem=" + productitem.Text;
        href += "&designer=" + designer.Text;
        href += "&stonedistribution=" + stonedistribution.Text;
        href += "&segmen=" + segmen.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&orderby=" + status;

        Response.Redirect(href);
    }
}