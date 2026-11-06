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

		if (!IsPostBack) {
            DLC.CharProductItem(productitem);
            DLC.CharGoldModel(model);
            DLC.CharGoldLevel(productlevel);

            DLC.LocationList(cabang, tipe);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        var rs = from p in db.StockProductPG
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        if(kw.Text!="")
            rs = rs.Where(p=>p.Nomor == "" +kw.Text+ "");
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0) rs = rs.Where(p => p.StockActualPG.IDLokasi == Cabang && p.StockActualPG.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualPG.TipeLokasi == tipe);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.ProductItem == ProductItem);
        int Model = Convert.ToInt32(model.SelectedValue);
        if (Model > 0) rs = rs.Where(p => p.Model == Model);

        if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.StockActualPG.NamaLokasi);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.CharGoldLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.CharGoldModel.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.CharProductItem.Nama);

        if (rs != null)
        {
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalWeight = 0m;
            decimal TotalOngkos = 0m;
            decimal TotalHarga = 0m;
            decimal TotalHargaAll = 0m;
            foreach (var r in rs)
            {
                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualPG.Substorage == 1)
                    Jual = r.StockActualPG.InHand;
                else if (r.StockActualPG.Substorage == 3)
                    Exchange = r.StockActualPG.InHand;
                else if (r.StockActualPG.Substorage == 2)
                    Resell = r.StockActualPG.InHand;


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharProductItem.Nama;
                tr.Cells.Add(td);

                /*td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = r.StockActualPG.InHand.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(r.StockActualPG.Substorage);
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.StockActualPG.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualPG.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualPG.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualPG.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharGoldModel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.KadarLogam).ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharFrameColor.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.MountingM).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Harga).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalHarga).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalWeight += Convert.ToDecimal(r.GrossWeight);
                TotalOngkos += Convert.ToDecimal(r.MountingM);
                TotalHarga += Convert.ToDecimal(r.Harga);
                TotalHargaAll += Convert.ToDecimal(r.TotalHarga);
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
                td.Text = "<b>" + rs.Sum(p=>p.StockActualPG.InHand).ToString("N0") + "</b>";
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
                td.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalOngkos).ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalHargaAll).ToString("N0") + "</b>";
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
            cabang.SelectedValue = Request.QueryString["cabang"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            model.SelectedValue = Request.QueryString["model"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&model=" + model.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "PG_DetailExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&model=" + model.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;

        Response.Redirect(href);
    }
}