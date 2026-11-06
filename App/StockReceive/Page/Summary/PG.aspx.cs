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

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharGoldLevel(productlevel);
            DLC.CharFrameColor(framecolor);
            DLC.CompanyBrand(brand);
            DLC.CharGoldModel(model);

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

        decimal TotalHarga = 0m;
        decimal TotalBerat = 0m;
        decimal Harga = 0m;
        decimal OngkosR = 0m;
        decimal Berat = 0m;
        decimal Qty = 0m;
        decimal KadarLogam = 0m;
        decimal KadarTukaranJual = 0m;
        decimal KadarTukaranBeli = 0m;
        decimal TotalBerat24 = 0m;
        decimal TotalHarga24 = 0m;

        var rs = from p in db.StockReceivePG
                 select p;


        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
        }

        if (kw.Text != "")
        {
            rs = rs.Where(p => p.KodeBarang.Contains(kw.Text) || p.Nomor.Contains(kw.Text) || p.SupplierSJ.Contains(kw.Text));
        }

        if (rs.Count() > 0)
        {

            if (productlevel.SelectedValue != "" && productlevel.SelectedValue != "0")
                rs = rs.Where(p => p.ProductLevel == Convert.ToInt32(productlevel.SelectedValue));
            if (model.SelectedValue != "" && model.SelectedValue != "0")
                rs = rs.Where(p => p.Model == Convert.ToInt32(model.SelectedValue));
            if (framecolor.SelectedValue != "" && framecolor.SelectedValue != "0")
                rs = rs.Where(p => p.FrameColor == Convert.ToInt32(framecolor.SelectedValue));
            if (productitem.SelectedValue != "" && productitem.SelectedValue != "0")
                rs = rs.Where(p => p.ProductItem == Convert.ToInt32(productitem.SelectedValue));
            int Status = Convert.ToInt32(statusdoc.SelectedValue);
            if (Status == 1)
                rs = rs.Where(p => p.Approval != null);
            else if (Status == 2)
                rs = rs.Where(p => p.Draft);

            /*var pg = from p in rs
                     group p by p.NoTTB into g
                     select new { NoTTB = g.Key, Qty = g.Count(), ProductLevel = g.Select(x=>x.CharGoldLevel.Nama), 
                     Model = g.Select(x=>x.CharGoldModel.Nama), KadarLogam = g.Select(x=>x.KadarLogam), FrameColor = g.Select(x=>x.CharFrameColor.Nama),
                     Weight = g.Sum(x=>x.GrossWeight), Ongkos = g.Sum(x=>x.MountingR),TotalHarga = g.Sum(x=>x.TotalHarga),
                     TotalBerat24 = g.Sum(x=>x.NettoWeight), TotalHarga24 = g.Sum(x=>x.TotalHarga24), Harga=g.Sum(x=>x.Harga)};
             */

            string status, status2 = "";

            foreach (var rslt in rs)
            {
                //if (!Response.IsClientConnected) break;
                TableRow tr = new TableRow();
                TableCell td;

                string Tanggal = Str.FullDate(Db.SingleTime("SELECT TOP 1 Tgl FROM StockReceivePG WHERE NoTTB='"+rslt.NoTTB+"'",Db.Cnn_DB));

                /*Qty = Convert.ToDecimal((from p in db.StockReceivePG
                              where p.NoTTB == "" + rslt.NoTTB + ""
                              select p).Count());
                 */
                Qty = 1m;

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.NoTTB + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.Nomor + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.SupplierSJ + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + Tanggal + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharGoldModel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.KadarLogam.ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharFrameColor.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.GrossWeight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.MountingR.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Harga).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.NettoWeight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (Convert.ToDecimal(rslt.Harga)+rslt.MountingR).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.SupplierNama;
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
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            brand.SelectedValue = Request.QueryString["brand"];
            model.SelectedValue = Request.QueryString["model"];
            statusdoc.SelectedValue = Request.QueryString["statusdoc"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&brand=" + brand.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&framecolor=" + framecolor.SelectedValue;
        href += "&model=" + model.SelectedValue;
        href += "&statusdoc=" + statusdoc.SelectedValue;

        Response.Redirect(href);
    }
}