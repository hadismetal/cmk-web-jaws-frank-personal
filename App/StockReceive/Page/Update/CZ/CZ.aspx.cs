using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_CZ : System.Web.UI.Page
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
	protected void Page_Load(object sender, EventArgs e) {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharGoldLevel(productlevel);
            DLC.CharFrameColor(framecolor);
            DLC.CharGoldModelAllFilter(model);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.StockReceivePG
                 where p.Approval == null && p.Nomor.StartsWith("Z")
                 group p by p.NoTTB into g
                 select new { NoTTB = g.First().NoTTB, Tgl = (g.First().Tgl!=null) ? g.First().Tgl : DateTime.Today, ProductLevel = g.First().ProductLevel, Model = g.First().Model,
                 FrameColor = g.First().FrameColor, ProductItem = g.First().ProductItem, Supplier = g.First().SupplierSJ, NoKirimCZ = g.First().NoKirimCZ};

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.NoTTB == "" + keyword + "");
        if (productlevel.SelectedValue != "" && productlevel.SelectedValue != "0")
            rs = rs.Where(p => p.ProductLevel == Convert.ToInt32(productlevel.SelectedValue));
        if (model.SelectedValue != "" && model.SelectedValue != "0")
            rs = rs.Where(p => p.Model == Convert.ToInt32(model.SelectedValue));
        if (framecolor.SelectedValue != "" && framecolor.SelectedValue != "0")
            rs = rs.Where(p => p.FrameColor == Convert.ToInt32(framecolor.SelectedValue));
        if (productitem.SelectedValue != "" && productitem.SelectedValue != "0")
            rs = rs.Where(p => p.ProductItem == Convert.ToInt32(productitem.SelectedValue));

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
                td.Text = TD_HyperLink(r.NoTTB);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoTTB;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Tgl);
                tr.Cells.Add(td);

                string ProductItem = Db.SingleString("SELECT Nama FROM CharProductItem WHERE ID=" + r.ProductItem, Db.Cnn_DB);

                td = new TableCell();
                td.Text = ProductItem;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Supplier;
                tr.Cells.Add(td);

                string Model = Db.SingleString("SELECT Nama FROM CharGoldModel WHERE ID=" + r.Model, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Model;
                tr.Cells.Add(td);

                string ProductLevel = Db.SingleString("SELECT Nama FROM CharGoldLevel WHERE ID=" + r.ProductLevel, Db.Cnn_DB);

                td = new TableCell();
                td.Text = ProductItem;
                tr.Cells.Add(td);

                string FrameColor = Db.SingleString("SELECT Nama FROM CharFrameColor WHERE ID=" + r.FrameColor, Db.Cnn_DB);

                td = new TableCell();
                td.Text = FrameColor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoKirimCZ;
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
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            model.SelectedValue = Request.QueryString["model"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&framecolor=" + framecolor.SelectedValue;
        href += "&model=" + model.SelectedValue;


        Response.Redirect(href);
    }
    protected string TD_HyperLink(string NoTTB)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"CZ_Form.aspx?nottb=" + NoTTB + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        x += "&nbsp;<a class=\"btn btn-danger\" onClick =\"javascript: return confirm('Yakin menghapus Data ini?');\" href=\"CZ_Delete.aspx?nottb=" + NoTTB + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Delete";
        x += "</a>";

        return x;
    }
}