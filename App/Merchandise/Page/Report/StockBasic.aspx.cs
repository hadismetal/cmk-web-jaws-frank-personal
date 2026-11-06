using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

public partial class Page_DJ : System.Web.UI.Page
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
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductSegmen(productsegmen);
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

        var rs = from p in db.StockBasic
                 where 
                    (p.ProtoNomor.Contains(kw.Text.Trim()))
                 select p;

        if (rs != null)
        {
            int index = 1;
            foreach (var r in rs)
            {
                var rsh = from p in db.StockProductDJ
                          where (p.ProtoNomor.Contains("-") ? p.ProtoNomor.Substring(0, p.ProtoNomor.IndexOf('-')) : p.ProtoNomor) == r.ProtoNomor
                          && p.StatusPenjualan == 0 
                          && !(from s in db.SalesOrderDJ
                               where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                               select s.IDProduct).Contains(p.ID)
                          select p;

                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0) rsh = rsh.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0) rsh = rsh.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
                if (ProductCategory > 0) rsh = rsh.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                int ProductSegmen = Convert.ToInt32(productsegmen.SelectedValue);
                if (ProductSegmen > 0) rsh = rsh.Where(p => p.StockProductDJ_PricingSegmen.ProductSegmen == ProductSegmen);

                int Tipe = Convert.ToInt32(Request.QueryString["locationtype"]);

                int Lokasi = 0;
                try
                {
                    Lokasi = Convert.ToInt32(Request.QueryString["location"]);
                }
                catch (Exception ex) { }

                if (Tipe >= 0 && Tipe < 3)
                {
                    if (Lokasi != 0)
                        rsh = rsh.Where(p => p.StockActualDJ.IDLokasi == Lokasi);
                    rsh = rsh.Where(p => p.StockActualDJ.TipeLokasi == Tipe);

                }

                if (rsh.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(r.ID);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<img src=\"/Images.aspx?path=" + rsh.First().ImgPicture + "\" border=\"0\" width=\"100\" />";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperlinkKaret(r.ProtoNomor);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rsh.Count().ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rsh.First().StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rsh.First().StockProductDJ_CharProduct.CharProductCategory.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rsh.First().StockProductDJ_CharProduct.CharProductLevel.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rsh.First().StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (from p in rsh
                                   select p.StockProductDJ_PricingMU.HargaRupiahRound.Value).Min().ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (from p in rsh
                               select p.StockProductDJ_PricingMU.HargaRupiahRound.Value).Max().ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
        }


    }
    protected void fillFilter()
    {

        tipelokasi.Items.Clear();
        tipelokasi.Items.Add(new ListItem("All", "-1"));
        tipelokasi.Items.Add(new ListItem("Warehouse", "0"));
        tipelokasi.Items.Add(new ListItem("Outlet", "1"));
        tipelokasi.Items.Add(new ListItem("Exhibition", "2"));



        if (App.Refresh(this))
        {
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            DLC.LocationList(lokasi, Convert.ToInt32(Request.QueryString["tipelokasi"]));

            lokasi.SelectedValue = Request.QueryString["lokasi"];
        }
        else
        {
            lokasi.Items.Clear();
            lokasi.Items.Add(new ListItem("All", "-1"));
        }
        productitem.SelectedValue = Request.QueryString["productitem"];
        productcategory.SelectedValue = Request.QueryString["productcategory"];
        productlevel.SelectedValue = Request.QueryString["productlevel"];
        productsegmen.SelectedValue = Request.QueryString["productsegmen"];
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi .Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&productsegmen=" + productsegmen.Text;

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

    protected string TD_HyperlinkKaret(string KodeKaret)
    {
        string x = string.Empty;

        x += "  <a target=\"_blank\" href=\"DJ_View.aspx?proto="  + KodeKaret +  "\">";
        x += KodeKaret;
        x += "  </a>";

        return x;
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        lokasi.Items.Clear();
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        else { lokasi.Items.Add(new ListItem("All", "-1")); }
        lokasi.Focus();
    }
}