using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

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

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            DLC.LocationList(cabang, tipe);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0) rs = rs.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
        else rs = rs.Where(p=>p.StockActualDJ.TipeLokasi == tipe);

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.StockActualDJ.NamaLokasi);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int PrevID = 0;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProtoNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingProduct.TotalProduct.ToString("N2");
                tr.Cells.Add(td);

                var stone = (from p in r.StockProductDJ_Stone1A
                             select new {HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1A.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Union(
                            from p in r.StockProductDJ_Stone1B
                            select new { HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1B.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Union(
                            from p in r.StockProductDJ_Stone2
                            select new { HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone2.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Union(
                            from p in r.StockProductDJ_Stone3
                            select new { HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone3.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Union(
                            from p in r.StockProductDJ_Stone4
                            select new { HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone4.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Union(
                            from p in r.StockProductDJ_Stone5
                            select new { HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone5.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir });
                if (stone.Count()>0)
                {
                    int counter = 1;
                    foreach (var rstone in stone)
                    {
                        if (counter > 1)
                        {
                            tr = new TableRow();

                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);
                        }

                        td = new TableCell();
                        td.Text = rstone.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rstone.TotalButir.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rstone.TotalCarat.ToString("N3");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(rstone.TotalHarga).ToString("N2");
                        tr.Cells.Add(td);

                        if (stone.Count() > 1)
                            tb.Rows.Add(tr);

                        counter++;
                    }
                }
                else
                {
                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);
                }

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingBiaya.MountingNO.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingBiaya.MountingRO.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingBiaya.Setting).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingMU.HargaUSD.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }


    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            orderby.SelectedValue = Request.QueryString["orderby"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_DetailExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.SelectedValue;

        Response.Redirect(href);
    }
}