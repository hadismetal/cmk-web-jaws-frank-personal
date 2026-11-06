using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

        int Filter = Convert.ToInt32(Request.QueryString["filter"]);

        var rs = (from p in db.StockProductDJ_Stone1B
                  join q in db.SalesOrderDJ on p.IDForm equals q.IDProduct
                  where q.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                  && q.StockProductDJ.StockProductDJ_Stone1B.Count() > 0
                  && !(from z in db.SalesOrderDJ
                       where z.StockProductDJ.Legacy.Value == 1
                       select z.SalesOrder.ID).Contains(q.IDForm)
                      //&& p.SalesOrder.StatusPembayaran == false
                  && q.SalesOrder.StatusPembayaran == true
                  && ((Filter == 1) ?
                  (from z in db.StockBasic
                   select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : (Filter == 2) ? !(from z in db.StockBasic
                                      select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : 1 == 1)
                  select new
                  {
                      ProtoNomor = p.StockProductDJ.ProtoNomor,
                      ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.ProductItem,
                      ProductCategory = p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory,
                      ProductSegmen = p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen,
                      TipeLokasi = q.SalesOrder.TipeLokasi.Value,
                      IDLokasi = q.SalesOrder.IDLokasi.Value,
                      NoSO = q.SalesOrder.Nomor,
                      CustomerNama = q.SalesOrder.CustomerNama,
					  SalesNama = q.SalesOrder.SalesNama,
                      Invoice = q.Invoice,
                      Nomor = q.StockProductDJ.Nomor,
                      StatusVoid = q.SalesOrder.StatusVoid.Value,
                      Tgl = q.SalesOrder.Tgl.Value,
                      Stone1B = p.Stone1B,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      GIA = p.GIA,
                      HargaRupiah = q.HargaRupiah.GetValueOrDefault(),
                      //HargaM = q.StockProductDJ.StockProductDJ_Costing.TotalBiaya + q.StockProductDJ.StockProductDJ_CostingProduct.Total,
                      HargaM = p.HargaTotalM.GetValueOrDefault(),
                      HargaR = p.HargaTotal.GetValueOrDefault(),
                      HargaTotal = (p.HargaTotal.GetValueOrDefault() > 0) ? p.HargaTotal.GetValueOrDefault() : 1,
                      HargaMJual = p.HargaMJual.GetValueOrDefault(),
                      HargaRJual = p.HargaRJual.GetValueOrDefault(),
                      DiscMJual = p.DiscMJual.GetValueOrDefault(),
                      DiscRJual = p.DiscRJual.GetValueOrDefault()
                  }).Concat(
                  from p in db.SalesOrderLD
                  where !p.SalesOrder.Draft
                      //&& p.SalesOrder.StatusPembayaran == false
                  && p.SalesOrder.StatusPembayaran == true
                  select new
                  {
                      ProtoNomor = "-",
                      ProductItem = DLC.ProductItem(),
                      ProductCategory = DLC.ProductCategory(),
                      ProductSegmen = DLC.ProductSegmen(),
                      TipeLokasi = p.SalesOrder.TipeLokasi.Value,
                      IDLokasi = p.SalesOrder.IDLokasi.Value,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
					  SalesNama = p.SalesOrder.SalesNama,
                      Invoice = p.Invoice,
                      Nomor = p.StockProductLD_Stone1B.Nomor,
                      StatusVoid = p.SalesOrder.StatusVoid.Value,
                      Tgl = p.SalesOrder.Tgl.Value,
                      Stone1B = p.StockProductLD_Stone1B.Stone1B,
                      TotalButir = p.StockProductLD_Stone1B.TotalButir,
                      TotalCarat = p.StockProductLD_Stone1B.TotalCarat,
                      GIA = p.StockProductLD_Stone1B.GIA,
                      HargaRupiah = p.HargaRupiah.Value,
                      HargaM = p.StockProductLD_Stone1B.HargaTotalM.Value,
                      HargaR = p.StockProductLD_Stone1B.HargaTotal.Value,
                      HargaTotal = (p.StockProductLD_Stone1B.HargaTotal.Value > 0) ? p.StockProductLD_Stone1B.HargaTotal.Value : 1,
                      HargaMJual = p.StockProductLD_Stone1B.HargaMJual.Value,
                      HargaRJual = p.StockProductLD_Stone1B.HargaRJual.Value,
                      DiscMJual = p.StockProductLD_Stone1B.DiscMJual.Value,
                      DiscRJual = p.StockProductLD_Stone1B.DiscRJual.Value
                  });

        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductSegmen = Convert.ToInt32(Request.QueryString["productsegmen"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int Payment = Convert.ToInt32(Request.QueryString["payment"]);

        if (ProductItem > 0)
            rs = rs.Where(p => p.ProductItem == ProductItem);
        if (ProductCategory > 0)
            rs = rs.Where(p => p.ProductCategory == ProductCategory);
        if (ProductSegmen > 0)
            rs = rs.Where(p => p.ProductSegmen == ProductSegmen);

        if (Tipe >= 0)
            rs = rs.Where(p => p.TipeLokasi == Tipe);
        if (IDLokasi >= 0)
            rs = rs.Where(p => p.IDLokasi == IDLokasi);

        rs = rs.OrderByDescending(p => p.Tgl).ThenBy(p => p.NoSO);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.NoSO == "" + keyword + "" || p.CustomerNama.Contains(Request.QueryString["kw"]) || p.Invoice.Contains(Request.QueryString["kw"]) || p.Nomor.Contains(Request.QueryString["kw"]));

        if (Request.QueryString["status"] == "1")
            rs = rs.Where(p => p.StatusVoid == true);
        else if (Request.QueryString["status"] == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHargaRapaport = 0m;
            decimal TotalHargaM = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalHargaJual = 0m;

            int prev = 0;
            int index = 1;
            string SO = "";
            foreach (var r in rs)
            {


                decimal Butir = 0m;
                decimal Carat = 0m;
                decimal TotalC = 0m;
                int Qty = 1;

                Butir = r.TotalButir;
                Carat = r.TotalCarat / r.TotalButir;
                TotalC = r.TotalCarat;
                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                else if (r.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoSO;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string CategoryNama = Db.SingleString("SELECT Nama FROM CharProductCategory WHERE ID=" + r.ProductCategory, Db.Cnn_DB);
                string ItemNama = Db.SingleString("SELECT Nama FROM CharProductItem WHERE ID=" + r.ProductItem, Db.Cnn_DB);
                string SegmenNama = Db.SingleString("SELECT Nama FROM CharProductSegmen WHERE ID=" + r.ProductSegmen, Db.Cnn_DB);

                td = new TableCell();
                td.Text = CategoryNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProtoNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ItemNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Stone1B.Parcel106.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Stone1B.Parcel103.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Stone1B.Parcel104.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalC.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + r.GIA + "\" target=\"_blank\">" + r.GIA + "</a>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaRupiah.ToString("N0");
                tr.Cells.Add(td);

                decimal HargaM = (r.HargaM);

                td = new TableCell();
                td.Text = HargaM.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaR.ToString("N2");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "<b>" + Convert.ToDecimal(((r.HargaTotal - HargaM) / r.HargaTotal) * 100).ToString("N0") + " %</b>";
                //tr.Cells.Add(td);

                decimal HargaMJual = 0m;
                HargaMJual = (r.HargaMJual + (r.HargaMJual * r.DiscMJual / 100)) * r.TotalCarat;

                td = new TableCell();
                td.Text = HargaMJual.ToString("N2");
                tr.Cells.Add(td);

                decimal HargaRJual = 0m;

                HargaRJual = (r.HargaRJual + (r.HargaRJual * r.DiscRJual / 100)) * r.TotalCarat;

                td = new TableCell();
                td.Text = (HargaRJual).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.HargaRJual).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.DiscMJual).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.DiscRJual).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(((r.HargaTotal - HargaM) / r.HargaTotal) * 100).ToString("N0") + " %</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalHargaRapaport += r.HargaTotal;
                TotalHargaM += (r.HargaM);
                TotalButir += r.TotalButir;
                TotalCarat += r.TotalCarat;
                TotalHargaJual += r.HargaRupiah;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 13;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaJual.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaM).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaRapaport).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);



            tb.Rows.Add(tr2);
        }
    }
}