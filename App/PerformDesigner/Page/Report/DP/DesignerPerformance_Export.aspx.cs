using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_DP_DesignerPerformance_Export : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DesignerPerformance_ExportExcel.xls");
            //Generate
            fillTable(sender, e);
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
                 select new
                 {
                     Nomor = karetDJ.Nomor,
                     Designer = design.HumanDesignerNama,
                     DataHuman = design.DataHuman,
                     ProductDesign_CharDesign = karetDJ.KodeKaretDJ_CharDesign,
                     ProductDesign_CharProduct = karetDJ.KodeKaretDJ_CharProduct,
                     ProductDesign_PricingSegmen = design.ProductDesign_PricingSegmen,
                     Approval = design.Approval,
                     Tgl = design.Tgl,
                     ImgPicture = design.ImgPicture
                 };



        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "");
        int DesignCategory = Convert.ToInt32(Request.QueryString["designcategory"]);
        if (DesignCategory > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = Convert.ToInt32(Request.QueryString["designconcept"]);
        if (DesignConcept > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignConcept == DesignConcept);
        int TargetAge = Convert.ToInt32(Request.QueryString["targetage"]);
        if (TargetAge > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.TargetAge == TargetAge);
        int TargetGender = Convert.ToInt32(Request.QueryString["targetgender"]);
        if (TargetGender > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.TargetGender == TargetGender);
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        if (ProductItem > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductItem == ProductItem);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        if (ProductLevel > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductLevel == ProductLevel);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        if (ProductCategory > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductCategory == ProductCategory);
        int Designer = Convert.ToInt32(Request.QueryString["designer"]);
        if (Designer > 0) rs = rs.Where(p => p.DataHuman.ID == Designer);

        int StoneDist = Convert.ToInt32(Request.QueryString["stonedistribution"]);
        if (StoneDist > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.StoneDist == StoneDist);
        int ProductSegmen = Convert.ToInt32(Request.QueryString["segmen"]);
        if (ProductSegmen > 0) rs = rs.Where(p => p.ProductDesign_PricingSegmen.ProductSegmen == ProductSegmen);

        if (Request.QueryString["groupby"] == "1")
            rs = rs.Where(p => p.Approval != null);
        else if (Request.QueryString["groupby"] == "2")
            rs = rs.Where(p => p.Approval == null);
        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]));
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
                td.Text = r.First().Designer;
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
                           select new { Nomor = karetDJ.Nomor };

                td = new TableCell();
                td.Text = spDJ.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p><img src=\"/Images.aspx?path=" + r.First().ImgPicture + "\" width=\"100\" /></p>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }
}