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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
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

        var rs = from p in db.StockProductDJLD
                 where p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusLebur == false
                           && !(from s in db.SalesOrderDJ
                               where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                               select s.IDProduct).Contains(p.IDProductDJ.Value)
                 select p;

        rs = rs.OrderByDescending(p => p.StockProductDJ.ApprovalTgl);

        if(kw.Text!="")
            rs = rs.Where(p=>p.StockProductDJ.Nomor==""+kw.Text+"" || p.StockProductLD_Stone1B.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        var rsg = rs.GroupBy(p => p.StockProductDJ.ID);

        if (rs != null)
        {
            int index = 1;
            foreach (var r in rsg)
            {
                var ld = from p in db.StockProductDJLD
                         where p.IDProductDJ == r.Key && p.Draft == false
                         select p;

                foreach (var j in ld)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(j.ID);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = j.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = j.StockProductDJ.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = j.StockProductLD_Stone1B.Nomor;
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    var x = db.StockActualDJ.SingleOrDefault(p => p.IDProduct == j.StockProductDJ.ID);
                    if (x.TipeLokasi == 0)
                    {
                        Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    }
                    else if (x.TipeLokasi == 1)
                    {
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    }
                    else if (x.TipeLokasi == 2)
                    {
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    }

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }

                

            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
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
            //datepicker.Text = Request.QueryString["dari"];
            //datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected string TD_Hyperlink(int ID)
    {
        string x = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + ID + "')\">";
        x += "      <i class=\"icon-trash\"></i> ";
        x += "      Delete";
        x += "  </a>";
        x += "</div>";

        return x;
    }
}