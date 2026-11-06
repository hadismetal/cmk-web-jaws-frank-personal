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
            DLC.CharGoldModelAll2(model);

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

        var rs = from p in db.StockProductPG
                 where p.Draft == false && p.Approval != null && p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.StockActualPG.TipeLokasi == d.First().TipeLokasi && p.StockActualPG.IDLokasi == d.First().IDLokasi);
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

            var pg = from p in rs
                     group p by p.NoTTB into g
                     select new { NoTTB = g.Key, Qty = g.Count(), ProductLevel = g.Select(x=>x.CharGoldLevel.Nama), 
                     Model = g.Select(x=>x.CharGoldModel.Nama), KadarLogam = g.Select(x=>x.KadarLogam), FrameColor = g.Select(x=>x.CharFrameColor.Nama),
                     Weight = g.Sum(x=>x.GrossWeight), Ongkos = g.Sum(x=>x.MountingR),TotalHarga = g.Sum(x=>x.TotalHarga),
                     TotalBerat24 = g.Sum(x=>x.NettoWeight), TotalHarga24 = g.Sum(x=>x.TotalHarga24), Harga=g.Sum(x=>x.Harga)};

            string status, status2 = "";

            foreach (var rslt in pg)
            {
                //if (!Response.IsClientConnected) break;
                TableRow tr = new TableRow();
                TableCell td;

                string Tanggal = Str.FullDate(Db.SingleTime("SELECT TOP 1 Tgl FROM StockProductPG WHERE NoTTB='"+rslt.NoTTB+"'",Db.Cnn_DB));

                Qty = Convert.ToDecimal((from p in db.StockProductPG
                              where p.NoTTB == "" + rslt.NoTTB + ""
                              select p).Count());

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.NoTTB + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + Tanggal + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.ProductLevel.First();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Model.First();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.KadarLogam.First()).ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.FrameColor.First();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Qty.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Weight).ToString("N2");
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
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;


        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "PG_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;

        Response.Redirect(href);
    }
}