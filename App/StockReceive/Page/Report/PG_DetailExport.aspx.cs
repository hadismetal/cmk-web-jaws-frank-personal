using System;
using System.Linq;
using System.Linq.Dynamic;
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

            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=PG_Detail_ExportExcel.xls");
            //Generate
            fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;
        string consignment = Request.QueryString["consignment"];
        
        var rs = from p in db.StockReceivePG
                 //where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 select p;

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        string kw = Request.QueryString["kw"];
        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int Model = Convert.ToInt32(Request.QueryString["model"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        string orderby = Request.QueryString["orderby"];
        string datepicker = Request.QueryString["dari"];
        string datepicker2 = Request.QueryString["sampai"];

        if (datepicker != "" && datepicker2 != "")
        {
            start = Convert.ToDateTime(datepicker);
            end = Convert.ToDateTime(datepicker2);
            rs = rs.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
        }

        if(kw!="")
            rs = rs.Where(p=>p.Nomor == "" +kw+ "");
        
        if (Cabang > 0) rs = rs.Where(p => p.IDWarehouse == Cabang);
        
        if (ProductLevel > 0) rs = rs.Where(p => p.ProductLevel == ProductLevel);
        
        if (ProductItem > 0) rs = rs.Where(p => p.ProductItem == ProductItem);
        
        if (Model > 0) rs = rs.Where(p => p.Model == Model);

        int Supplier = Convert.ToInt32(Request.QueryString["supplier"]);
        if (Supplier > 0) rs = rs.Where(p => p.IDSupplier == Supplier);

        if (orderby == "1")
            rs.OrderBy(p => p.LocWarehouse.Nama);
        else if (orderby == "2")
            rs.OrderBy(p => p.CharGoldLevel.Nama);
        else if (orderby == "3")
            rs.OrderBy(p => p.CharGoldModel.Nama);
        else if (orderby == "4")
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
            decimal TotalHPPM = 0m;
            foreach (var r in rs)
            {
                string status = Db.SingleString("SELECT CONVERT(varchar, ISNULL(Consignment, 0), 0) FROM StockReceivePG WHERE ID = " + r.ID, Db.Cnn_DB);
                if(consignment != "2" && consignment != status) continue;
                switch (status)
                {
                    case "1" :
                    status = "Consignment";
                    break;
                    case "0" :
                    status = "Non-Consignment";
                    break;
                }
                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Tgl.ToString("dd-MM-yyyy");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.KodeBarang;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = status;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Jual.ToString("N0").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharGoldModel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.KadarLogam).ToString("N0").Replace(".", "") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.KadarTukaranBeli).ToString("N0").Replace(".", "") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharFrameColor.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DataSupplier.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.GrossWeight).ToString("N2").Replace("." , "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.GoldRate).ToString("N2").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.MountingM).ToString("N0").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalHarga24 / r.TotalRate).ToString("N2").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Harga).ToString("N0").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalHargaJual).ToString("N0").Replace(".", "");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = r.SupplierNama;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SupplierSJ;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalWeight += Convert.ToDecimal(r.GrossWeight);
                TotalOngkos += Convert.ToDecimal(r.MountingM);
                TotalHPPM += Convert.ToDecimal(r.TotalHarga24 / r.TotalRate);
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
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalStock.ToString("N0").Replace(".", "") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalExchange.ToString("N0").Replace(".", "") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalResell.ToString("N0").Replace(".", "") + "</b>";
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
                td.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2").Replace("." , "") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalOngkos).ToString("N0").Replace(".", "") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalHPPM).ToString("N2").Replace(".", "") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0").Replace(".", "") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalHargaAll).ToString("N0").Replace(".", "") + "</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }


    }
}