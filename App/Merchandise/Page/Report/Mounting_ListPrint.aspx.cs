using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Mounting : System.Web.UI.Page {
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
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {

            //Generate
            fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductMounting
                 join q in db.StockReceiveMounting on p.Nomor equals q.Nomor
                 where q.StatusReceive == 0 && p.Draft == false
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        string datepicker = Request.QueryString["dari"];
        string datepicker2 = Request.QueryString["sampai"];

        if (datepicker != "" && datepicker2 != "")
        {
            start = Convert.ToDateTime(datepicker);
            end = Convert.ToDateTime(datepicker2);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
        }

        string kw = Request.QueryString["kw"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        string orderby = Request.QueryString["orderby"];
        string status = Request.QueryString["status"];

        if (kw != "")
            rs = rs.Where(p => p.Nomor == "" + kw + "");

        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductLevel == ProductLevel);

        if (ProductItem > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductItem == ProductItem);

        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductCategory == ProductCategory);

        if (orderby == "1")
            rs.OrderBy(p => p.LocWarehouse.Nama);
        else if (orderby == "2")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductLevel.Nama);
        else if (orderby == "3")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductCategory.Nama);
        else if (orderby == "4")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductItem.Nama);

        if (status == "1")
            rs = rs.Where(p => p.StatusReceive == true);
        else if (status == "2")
            rs = rs.Where(p => p.StatusReceive == false);

        if (rs != null)
        {
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalGrossWeight = 0m;
            decimal TotalButirAll = 0m;
            decimal TotalCaratAll = 0m;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor!=null) ? r.ProtoNomor : "";

                td = new TableCell();
                td.Text = "<small>" + KodeKaret + "</small>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualMounting.Substorage == 1)
                    Jual = r.StockActualMounting.InHand;
                else if (r.StockActualMounting.Substorage == 3)
                    Exchange = r.StockActualMounting.InHand;
                else if (r.StockActualMounting.Substorage == 2)
                    Resell = r.StockActualMounting.InHand;

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
                td.Text = r.StockActualMounting.InHand.ToString("N0");
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.StockActualMounting.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualMounting.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualMounting.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualMounting.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualMounting.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualMounting.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_PricingMU.ModalRupiah).ToString("N0") + " / " + Convert.ToDecimal(r.StockProductMounting_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);
               
                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalHarga += Convert.ToDecimal(r.StockProductMounting_PricingMU.HargaRupiahRound);
                TotalModal += Convert.ToDecimal(r.StockProductMounting_PricingMU.ModalRupiah);
                TotalM += r.StockProductMounting_PricingMU.ModalUSD;
                TotalR += r.StockProductMounting_PricingMU.HargaUSD;
                TotalNetWeight += r.StockProductMounting_CharProduct.NettoWeight;
                TotalGrossWeight = r.StockProductMounting_CharProduct.GrossWeight;

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

                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.Sum(p => p.StockActualMounting.InHand).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalModal.ToString("N0") + " / "  + TotalHarga.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
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

                tb.Rows.Add(tr);
            }
        }


    }
}