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
    protected int myID { get { return App.QueryInt(this, "id"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            
            fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //DateTime start;
        //DateTime end;

        //var rs = from p in db.StockProductPG
        //         where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
        //         && p.ID == myID
        //         select p;

        var rs = from p in db.StockReturnPG_Product
                 where p.IDForm == myID
                 select p;

        if (rs != null)
        {
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalWeight = 0m;
            decimal TotalOngkos = 0m;
            decimal TotalHarga = 0m;
            decimal TotalHargaAll = 0m;
            decimal TotalHarga24 = 0m;
            int TotalQty = rs.Count();

            foreach (var r in rs)
            {
                //var x = from p in db.StockProductPG
                //          where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                //          && p.ID == r.IDProduct
                //          select p;

                //foreach (var y in x)
                //{
                    decimal Jual = 0m;
                    decimal Exchange = 0m;
                    decimal Resell = 0m;

                    if (r.StockProductPG.StockActualPG.Substorage == 1)
                        Jual = r.StockProductPG.StockActualPG.InHand;
                    else if (r.StockProductPG.StockActualPG.Substorage == 3)
                        Exchange = r.StockProductPG.StockActualPG.InHand;
                    else if (r.StockProductPG.StockActualPG.Substorage == 2)
                        Resell = r.StockProductPG.StockActualPG.InHand;

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharProductItem.Nama;
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
                    td.Text = r.StockProductPG.StockActualPG.InHand.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = DL.SubStorage(r.StockProductPG.StockActualPG.Substorage);
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    if (r.StockProductPG.StockActualPG.TipeLokasi == 0)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockProductPG.StockActualPG.IDLokasi, Db.Cnn_DB);
                    else if (r.StockProductPG.StockActualPG.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockProductPG.StockActualPG.IDLokasi, Db.Cnn_DB);
                    else if (r.StockProductPG.StockActualPG.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockProductPG.StockActualPG.IDLokasi, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharGoldLevel.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharGoldModel.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductPG.KadarLogam).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharFrameColor.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.SupplierNama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductPG.GrossWeight).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductPG.MountingM).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductPG.TotalHarga24.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductPG.Harga).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductPG.TotalHargaJual).ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    TotalStock += Jual;
                    TotalResell += Resell;
                    TotalExchange += Exchange;
                    TotalWeight += Convert.ToDecimal(r.StockProductPG.GrossWeight);
                    TotalOngkos += Convert.ToDecimal(r.StockProductPG.MountingM);
                    TotalHarga24 += Convert.ToDecimal(r.StockProductPG.TotalHarga24);
                    TotalHarga += Convert.ToDecimal(r.StockProductPG.Harga);
                    TotalHargaAll += Convert.ToDecimal(r.StockProductPG.TotalHargaJual);
                //}
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 7;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalOngkos.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHarga24.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHarga.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaAll.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}