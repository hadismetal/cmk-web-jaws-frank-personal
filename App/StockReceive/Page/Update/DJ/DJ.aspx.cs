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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.CharDesignCategory(designcategory);
            DLC.CharDesignConcept(designconcept);
            DLC.CharDesignProcess(designprocess);
            DLC.CharTargetAge(targetage);
            DLC.CharTargetGender(targetgender);
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharStoneDist(stonedistribution);
            DLC.CharFrameMaterial(framematerial);
            DLC.CharFrameFinishing(framefinishing);
            DLC.CharFrameColor(framecolor);
            DLC.CharProcessCons(processconstruction);
            DLC.DDLSupplier(supplier, 0);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.StockReceiveDJ
                 where p.StatusReceive == 0
					&& p.Draft == false
					&& (p.Nomor == null || p.Nomor == "")
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.Nomor.Contains(keyword) || p.ProtoNomor.Contains(keyword) || p.SupplierNomor.Contains(keyword) || p.ProductOrderNomor.Contains(keyword)) ;
        int DesignCategory = designcategory.SelectedIndex;
        if (DesignCategory > 0) rs = rs.Where(p => p.StockReceiveDJ_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = designconcept.SelectedIndex;
        if (DesignConcept > 0) rs = rs.Where(p => p.StockReceiveDJ_CharDesign.DesignConcept == DesignConcept);
        int DesignProcess = designprocess.SelectedIndex;
        if (DesignProcess > 0) rs = rs.Where(p => p.StockReceiveDJ_CharDesign.DesignProcess == DesignProcess);
        int TargetAge = targetage.SelectedIndex;
        if (TargetAge > 0) rs = rs.Where(p => p.StockReceiveDJ_CharDesign.TargetAge == TargetAge);
        int TargetGender = targetgender.SelectedIndex;
        if (TargetGender > 0) rs = rs.Where(p => p.StockReceiveDJ_CharDesign.TargetGender == TargetGender);
        int ProductItem = productitem.SelectedIndex;
        if (ProductItem > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = productcategory.SelectedIndex;
        if (ProductCategory > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
        int StoneDistribution = stonedistribution.SelectedIndex;
        if (StoneDistribution > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.StoneDist == StoneDistribution);
        int FrameMaterial = framematerial.SelectedIndex;
        if (FrameMaterial > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.FrameMaterial == FrameMaterial);
        int FrameFinishing = framefinishing.SelectedIndex;
        if (FrameFinishing > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.FrameFinishing == FrameFinishing);
        int FrameColor = framecolor.SelectedIndex;
        if (FrameColor > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.FrameColor == FrameColor);
        int ProcessConstruction = processconstruction.SelectedIndex;
        if (ProcessConstruction > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProcessCons == ProcessConstruction);

        string Supplier = supplier.SelectedValue;

        if (Supplier != "0")
            rs = rs.Where(p => p.SupplierNama.Equals(Supplier));

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
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Data(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SupplierNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Tgl);
                tr.Cells.Add(td);

                string TglApprove = "";
                TglApprove = r.ApprovalTgl.HasValue ? Str.Tgl(r.ApprovalTgl.Value) : "-";

                td = new TableCell();
                td.Text = TglApprove;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Approval;
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
                else
                {
                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);
                }

                td = new TableCell();
                td.Text = r.Operator;
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
            designcategory.SelectedValue = Request.QueryString["designcategory"];
            designconcept.SelectedValue = Request.QueryString["designconcept"];
            designprocess.SelectedValue = Request.QueryString["designprocess"];
            targetage.SelectedValue = Request.QueryString["targetage"];
            targetgender.SelectedValue = Request.QueryString["targetgender"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            stonedistribution.SelectedValue = Request.QueryString["stonedistribution"];
            framematerial.SelectedValue = Request.QueryString["framematerial"];
            framefinishing.SelectedValue = Request.QueryString["framefinishing"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            processconstruction.SelectedValue = Request.QueryString["processconstruction"];
            supplier.SelectedValue = Request.QueryString["supplier"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&designprocess=" + designprocess.Text;
        href += "&targetage=" + targetage.Text;
        href += "&targetgender=" + targetgender.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&stonedistribution=" + stonedistribution.Text;
        href += "&framematerial=" + framematerial.Text;
        href += "&framefinishing=" + framefinishing.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&processconstruction=" + processconstruction.Text;
        href += "&supplier=" + supplier.Text;


        Response.Redirect(href);
    }
    protected string TD_HyperLink(StockReceiveDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Form.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        x += "&nbsp;<a class=\"btn btn-danger\" onClick =\"javascript: return confirm('Yakin menghapus Data ini?');\" href=\"DJ_Delete.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-trash\"></i> ";
        x += "  Delete";
        x += "</a>";

        return x;
    }
    protected string TD_Data(StockReceiveDJ r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.Nomor + "</p>";
        if(r.ProtoNomor != null)
            x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";
        if(r.ProductOrderNomor!= null)
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