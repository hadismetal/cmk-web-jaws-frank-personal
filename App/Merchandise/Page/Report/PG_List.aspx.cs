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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharGoldLevel(productlevel);
            DLC.CharFrameColor(framecolor);
            DLC.CompanyBrand(brand);
            DLC.CharGoldModelAll2(model);

            DLC.LocationList(cabang, tipe);

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
        decimal TotalQty = 0m;
        decimal TotalWeight = 0m;
        decimal TotalOngkos = 0m;
        decimal TotalTotal = 0m;
        decimal Harga = 0m;
        decimal OngkosR = 0m;
        decimal Berat = 0m;
        decimal Qty = 0m;
        decimal KadarLogam = 0m;
        decimal KadarTukaranJual = 0m;
        decimal KadarTukaranBeli = 0m;
        decimal TotalBerat24 = 0m;
        decimal TotalHarga24 = 0m;
        decimal TtlHargaJual = 0m;

        var rs = from p in db.StockProductPG
                 where p.Draft == false && p.Approval != null && p.ReturnProduct == false /*&& p.StatusPenjualan == 0*/ && p.StatusLebur == false
                 && p.StockActualPG.IDLokasi > 0
                 && !(from s in db.SalesOrderPG
                      where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                      select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                select s.IDProductSouvenir).Contains(p.ID)
                 select p;

        if (rs.Count() > 0)
        {
            if (kw.Text != "")
                rs = rs.Where(p => p.Nomor.Contains(kw.Text));
            if (productlevel.SelectedValue != "" && productlevel.SelectedValue != "0")
                rs = rs.Where(p => p.ProductLevel == Convert.ToInt32(productlevel.SelectedValue));
            if (model.SelectedValue != "" && model.SelectedValue != "0")
                rs = rs.Where(p => p.Model == Convert.ToInt32(model.SelectedValue));
            if (framecolor.SelectedValue != "" && framecolor.SelectedValue != "0")
                rs = rs.Where(p => p.FrameColor == Convert.ToInt32(framecolor.SelectedValue));
            if (productitem.SelectedValue != "" && productitem.SelectedValue != "0")
                rs = rs.Where(p => p.ProductItem == Convert.ToInt32(productitem.SelectedValue));
            if (brand.SelectedValue != "" && brand.SelectedValue != "0")
                rs = rs.Where(p => p.IDBrand == Convert.ToInt32(brand.SelectedValue));

            int Substorage = Convert.ToInt32(substorage.SelectedValue);
            if (Substorage > 0)
            {
                rs = rs.Where(p => p.StockActualPG.Substorage == Substorage);
            }
            
            int Cabang = Convert.ToInt32(cabang.SelectedValue);
            if (Cabang > 0)
            {
                rs = rs.Where(p => p.StockActualPG.IDLokasi == Cabang && p.StockActualPG.TipeLokasi == tipe);
            }
            else
            {
                rs = rs.Where(p => p.StockActualPG.TipeLokasi == tipe);
            }

            int pageNumber = 1;
            int numberOfObjectsPerPage = 30;

            if (Request.QueryString["page"] != "")
                pageNumber = Convert.ToInt32(Request.QueryString["page"]);
            if (pageNumber < 0)
                pageNumber = 1;

            decimal TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

            TotalPage = Math.Ceiling(TotalPage);

            rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

            string status, status2 = "";

            foreach (var rslt in rs)
            {
                //if (!Response.IsClientConnected) break;
                TableRow tr = new TableRow();
                TableCell td;

                string Tanggal = Str.FullDate(Db.SingleTime("SELECT TOP 1 Tgl FROM StockProductPG WHERE NoTTB='"+rslt.NoTTB+"'",Db.Cnn_DB));

                Qty = Convert.ToDecimal((from p in db.StockProductPG
                              where p.NoTTB == "" + rslt.NoTTB + ""
                              select p).Count());

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.Nomor + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + Tanggal + "</span>";
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
                td.Text = rslt.KodeBarang;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharProductItem.Nama;
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

                td = new TableCell();
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.TotalHargaJual).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty += 1;
                TotalWeight += Convert.ToDecimal(rslt.GrossWeight);
                TotalOngkos += Convert.ToDecimal(rslt.MountingMUSD);
                TotalHarga += Convert.ToDecimal(rslt.HargaUSD);
                TotalBerat += Convert.ToDecimal(rslt.NettoWeight);
                TotalTotal += (Convert.ToDecimal(rslt.HargaUSD) + Convert.ToDecimal(rslt.MountingMUSD));
                TtlHargaJual += Convert.ToDecimal(rslt.TotalHargaJual);
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
                td.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalBerat.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TtlHargaJual.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
            string href = App.RefreshURL(this);

            //Customize-Qstr
            href += "&kw=" + kw.Text;
            href += "&brand=" + brand.Text;
            href += "&productitem=" + productitem.Text;
            href += "&productlevel=" + productlevel.Text;
            href += "&framecolor=" + framecolor.Text;
            href += "&model=" + model.Text;
            href += "&tipe=" + tipe;
            href += "&cabang=" + cabang.Text;
            href += "&substorage=" + substorage.SelectedValue;
            href += "&page=" + (pageNumber + 1).ToString();

            string href2 = App.RefreshURL(this);

            //Customize-Qstr
            href2 += "&kw=" + kw.Text;
            href2 += "&brand=" + brand.Text;
            href2 += "&productitem=" + productitem.Text;
            href2 += "&productlevel=" + productlevel.Text;
            href2 += "&framecolor=" + framecolor.Text;
            href2 += "&model=" + model.Text;
            href2 += "&tipe=" + tipe;
            href2 += "&cabang=" + cabang.Text;
            href2 += "&substorage=" + substorage.SelectedValue;
            href2 += "&page=" + (pageNumber - 1).ToString();

            string href3 = App.RefreshURL(this);

            //Customize-Qstr
            href3 += "&kw=" + kw.Text;
            href3 += "&brand=" + brand.Text;
            href3 += "&productitem=" + productitem.Text;
            href3 += "&productlevel=" + productlevel.Text;
            href3 += "&framecolor=" + framecolor.Text;
            href3 += "&model=" + model.Text;
            href3 += "&tipe=" + tipe;
            href3 += "&cabang=" + cabang.Text;
            href3 += "&substorage=" + substorage.SelectedValue;
            href3 += "&page=1";

            string href4 = App.RefreshURL(this);

            //Customize-Qstr
            href4 += "&kw=" + kw.Text;
            href4 += "&brand=" + brand.Text;
            href4 += "&productitem=" + productitem.Text;
            href4 += "&productlevel=" + productlevel.Text;
            href4 += "&framecolor=" + framecolor.Text;
            href4 += "&model=" + model.Text;
            href4 += "&tipe=" + tipe;
            href4 += "&cabang=" + cabang.Text;
            href4 += "&substorage=" + substorage.SelectedValue;
            href4 += "&page=" + TotalPage;

            string paging = "";

            paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
            paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href4 + "\">Last</a>";
            paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

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
            brand.SelectedValue = Request.QueryString["brand"];
            model.SelectedValue = Request.QueryString["model"];
            cabang.SelectedValue = Request.QueryString["cabang"];
            substorage.SelectedValue = Request.QueryString["substorage"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "PG_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "PG_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }
}