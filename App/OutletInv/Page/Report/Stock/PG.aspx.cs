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
		Ddl.Substorage(substorage);

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
                 where p.Draft == false && p.Approval != null && p.ReturnProduct == false /*&& p.StatusPenjualan == 0*/ && p.StatusLebur == false
                 && !(from s in db.SalesOrderPG
                      where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                      select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                select s.IDProductPG).Contains(p.ID)
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.StockActualPG.TipeLokasi == d.First().TipeLokasi && p.StockActualPG.IDLokasi == d.First().IDLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualPG.IDLokasi) && p.StockActualPG.TipeLokasi == 1);
        }

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualPG.Substorage == Substorage);
        }

        if (rs.Count() > 0)
        {
            int index = 1;
            if (productlevel.SelectedValue != "" && productlevel.SelectedValue != "0")
                rs = rs.Where(p => p.ProductLevel == Convert.ToInt32(productlevel.SelectedValue));
            if (model.SelectedValue != "" && model.SelectedValue != "0")
                rs = rs.Where(p => p.Model == Convert.ToInt32(model.SelectedValue));
            if (framecolor.SelectedValue != "" && framecolor.SelectedValue != "0")
                rs = rs.Where(p => p.FrameColor == Convert.ToInt32(framecolor.SelectedValue));
            if (productitem.SelectedValue != "" && productitem.SelectedValue != "0")
                rs = rs.Where(p => p.ProductItem == Convert.ToInt32(productitem.SelectedValue));

            /*var pg = from p in rs
                     group p by p.NoTTB into g
                     select new { NoTTB = g.Key, Qty = g.Count(), ProductLevel = g.Select(x=>x.CharGoldLevel.Nama), 
                     Model = g.Select(x=>x.CharGoldModel.Nama), KadarLogam = g.Select(x=>x.KadarLogam), FrameColor = g.Select(x=>x.CharFrameColor.Nama),
                     Weight = g.Sum(x=>x.GrossWeight), Ongkos = g.Sum(x=>x.MountingR),TotalHarga = g.Sum(x=>x.TotalHarga),
                     TotalBerat24 = g.Sum(x=>x.NettoWeight), TotalHarga24 = g.Sum(x=>x.TotalHarga24), Harga=g.Sum(x=>x.Harga)};*/

            string status, status2 = "";

            int pageNumber = 0;
            int numberOfObjectsPerPage = 30;

            if (Request.QueryString["page"] != "")
                pageNumber = Convert.ToInt32(Request.QueryString["page"]);
            if (pageNumber < 0)
                pageNumber = 0;

            rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

            TableRow tr;
            TableCell td;

            Decimal GTotalQty = 0m;
            Decimal GTotalWeight = 0m;

            foreach (var rslt in rs)
            {
                //if (!Response.IsClientConnected) break;
                tr = new TableRow();

                string Tanggal = Str.FullDate(Db.SingleTime("SELECT TOP 1 Tgl FROM StockProductPG WHERE NoTTB='"+rslt.NoTTB+"'",Db.Cnn_DB));

                Qty = Convert.ToDecimal((from p in db.StockProductPG
                              where p.NoTTB == "" + rslt.NoTTB + ""
                              select p).Count());

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.Nomor + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + Str.FullDate(rslt.Tgl) + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(rslt.StockActualPG.Substorage);
                tr.Cells.Add(td);

                string Lokasi = "";
                if (rslt.StockActualPG.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rslt.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (rslt.StockActualPG.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rslt.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (rslt.StockActualPG.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rslt.StockActualPG.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharGoldModel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.KadarLogam).ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharFrameColor.Nama;
                tr.Cells.Add(td);

                GTotalQty++;
                td = new TableCell();
                td.Text = Convert.ToInt32(rslt.StockActualPG.InHand).ToString();
                tr.Cells.Add(td);

                GTotalWeight += Convert.ToDecimal(rslt.GrossWeight);
                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }

            tr = new TableRow();
            tr.Font.Bold = true;

            td = new TableCell();
            td.Text = "Total";
            td.HorizontalAlign = HorizontalAlign.Right;
            td.ColumnSpan = 9;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = GTotalQty.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = GTotalWeight.ToString("N2");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);


            string href = App.RefreshURL(this);

            //Customize-Qstr
            href += "&kw=" + kw.Text;
            href += "&productitem=" + productitem.Text;
            href += "&productlevel=" + productlevel.Text;
            href += "&framecolor=" + framecolor.Text;
            href += "&model=" + model.Text;
            href += "&page=" + (pageNumber + 1).ToString();
            href += "&substorage=" + substorage.SelectedValue;

            string href2 = App.RefreshURL(this);

            //Customize-Qstr
            href2 += "&kw=" + kw.Text;
            href2 += "&productitem=" + productitem.Text;
            href2 += "&productlevel=" + productlevel.Text;
            href2 += "&framecolor=" + framecolor.Text;
            href2 += "&model=" + model.Text;
            href2 += "&page=" + (pageNumber - 1).ToString();
            href2 += "&substorage=" + substorage.SelectedValue;

            string paging = "";

            paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
            paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href + "\">Next</a>";
            paging += "</div>";

            paging_text.Text = paging;
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
            substorage.SelectedValue = Request.QueryString["substorage"];
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
        href += "&substorage=" + substorage.SelectedValue;

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
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }
}