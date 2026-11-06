using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_PG_List : System.Web.UI.Page
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
                 where p.Draft == false && p.Approval != null && p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 select p;

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
            if (brand.SelectedValue != "" && brand.SelectedValue != "0")
                rs = rs.Where(p => p.IDBrand == Convert.ToInt32(brand.SelectedValue));

            var pg = from p in rs
                     select new
                     {
                         ID = p.ID,
                         Nomor = p.Nomor,
                         NoTTB = p.NoTTB,
                         Qty = 1,
                         ProductItem = p.CharProductItem.Nama,
                         ProductLevel = p.CharGoldLevel.Nama,
                         Model = p.CharGoldModel.Nama,
                         KadarLogam = p.KadarLogam,
                         FrameColor = p.CharFrameColor.Nama,
                         Weight = p.GrossWeight,
                         Ongkos = p.MountingM,
                         TotalHarga = p.TotalHarga,
                         TotalBerat24 = p.NettoWeight,
                         TotalHarga24 = p.TotalHarga24,
                         Harga = p.Harga,
                         ApprovalTgl = p.ApprovalTgl,
                         TotalHargaJual = p.TotalHargaJual
                     };

            if (kw.Text != "")
                pg = pg.Where(p => p.ProductItem.Contains(kw.Text) || p.ProductLevel.Contains(kw.Text) || p.Model.Contains(kw.Text) || p.FrameColor.Contains(kw.Text));

            pg = pg.OrderByDescending(p => p.ApprovalTgl);

            string status, status2 = "";

            foreach (var rslt in pg)
            {
                //if (!Response.IsClientConnected) break;
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.Nomor + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(rslt.ApprovalTgl);
                tr.Cells.Add(td);

                try
                {
                    var d = db.StockActualPG.SingleOrDefault(p => p.IDProduct == rslt.ID);

                    td = new TableCell();
                    td.Text = DL.LocationType(d.TipeLokasi);
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    if (d.TipeLokasi == 0)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                    else if (d.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                    else if (d.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);
                }
                catch (Exception ex)
                {
                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);
                }

                

                td = new TableCell();
                td.Text = rslt.ProductItem;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.ProductLevel;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Model;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.KadarLogam).ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.FrameColor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Qty.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Weight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Ongkos).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Harga).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.TotalBerat24).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (Convert.ToDecimal(rslt.Harga) + Convert.ToDecimal(rslt.Ongkos)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.TotalHargaJual).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty += rslt.Qty;
                TotalWeight += Convert.ToDecimal(rslt.Weight);
                TotalOngkos += Convert.ToDecimal(rslt.Ongkos);
                TotalHarga += Convert.ToDecimal(rslt.Harga);
                TotalBerat += Convert.ToDecimal(rslt.TotalBerat24);
                TotalTotal += (Convert.ToDecimal(rslt.Harga) + Convert.ToDecimal(rslt.Ongkos));
                TtlHargaJual += Convert.ToDecimal(rslt.TotalHargaJual);
                index++;
            }

            if (pg.Count() > 0)
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
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalOngkos.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalBerat.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalTotal.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TtlHargaJual.ToString("N0") + "</b>";
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
            brand.SelectedValue = Request.QueryString["brand"];
            model.SelectedValue = Request.QueryString["model"];
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

        Response.Redirect(href);
    }
}