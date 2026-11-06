using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_LD : System.Web.UI.Page {
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
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
 
		}
        Response.ContentType = "application/vnd.ms-excel";

        Response.AppendHeader("content-disposition", "attachment; filename=DJ_List_ExportExcel.xls");
            fillTable(sender, e);
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductLD_Stone1B
                 //where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;

        rs = rs.OrderByDescending(p => p.StockProductLD.ApprovalTgl);

        string datepicker = Request.QueryString["dari"];
        string datepicker2 = Request.QueryString["sampai"];

        if (datepicker != "" && datepicker2 != "")
        {
            start = Convert.ToDateTime(datepicker);
            end = Convert.ToDateTime(datepicker2);
            rs = rs.Where(p => p.StockProductLD.ApprovalTgl >= start && p.StockProductLD.ApprovalTgl < end.AddDays(1));
        }

        string kw = Request.QueryString["kw"];

        if(kw!="")
            rs = rs.Where(p=>p.Nomor==""+kw+"");
        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
        if (Cabang > 0) rs = rs.Where(p => p.StockProductLD.IDWarehouse == Cabang);
        //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "")
            rs = rs.Where(p => p.StockProductLD.SupplierNama.Equals(Supplier));

        if (rs != null)
        {
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalCarat = 0m;
            foreach (var r in rs)
            {
                

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualLD_Stone1B.Substorage == 1)
                    Jual = r.StockActualLD_Stone1B.InHand;
                else if (r.StockActualLD_Stone1B.Substorage == 3)
                    Exchange = r.StockActualLD_Stone1B.InHand;
                else if (r.StockActualLD_Stone1B.Substorage == 2)
                    Resell = r.StockActualLD_Stone1B.InHand;

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
                td.Text = r.StockActualLD_Stone1B.InHand.ToString("N0").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(r.StockActualLD_Stone1B.Substorage);
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.StockActualLD_Stone1B.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualLD_Stone1B.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualLD_Stone1B.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Convert.ToDecimal(r.HargaTotal.Value).ToString("N2").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.HargaTotalM.Value).ToString("N2").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalCarat).ToString("N3").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductLD.SupplierNama;
                tr.Cells.Add(td);
               
                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalModal += Convert.ToDecimal(r.HargaTotalM.Value);
                TotalHarga += Convert.ToDecimal(r.HargaTotal.Value);
                TotalCarat += Convert.ToDecimal(r.TotalCarat);

            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.Sum(p => p.StockActualLD_Stone1B.InHand).ToString("N0").Replace(".", "");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalModal.ToString("N2").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N2").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalCarat.ToString("N3").Replace(".","").Replace(",",".") + "</b>";
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