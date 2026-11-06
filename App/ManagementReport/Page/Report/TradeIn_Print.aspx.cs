using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_TradeIn_Print : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);
        if (!IsPostBack)
        {
            FillHeader();
            fillTable(sender, e);
        }
    }

    protected void FillHeader()
    {
        string id = Request.QueryString["id"];
        string input = Request.QueryString["kw"];
        string status = Request.QueryString["status"];
        string dari = Request.QueryString["dari"];
        string sampai = Request.QueryString["sampai"];
        string tipelokasi = Request.QueryString["tipelokasi"];
        string lokasi = Request.QueryString["lokasi"];

        lbl_kw.Text = input;
        //lbl_status.Text = status;
        if (status == "0") { lbl_status.Text = "Semua"; } else if (status == "1") { lbl_status.Text = "Active"; } else if (status == "2") { lbl_status.Text = "Draft"; }
        lbl_from.Text = dari;
        lbl_to.Text = sampai;

        if (tipelokasi == "1")
        {
            if (Convert.ToInt32(lokasi) > 0)
            {
                var rs = from p in db.LocOutletGroup where !p.Disable select p;
                var rb = from p in db.LocOutlet where !p.Disable && p.IDGroup == p.LocOutletGroup.ID select p;
                var v_lokasi = rb.SingleOrDefault(p => p.ID == Convert.ToInt32(lokasi));
                var v_lokasiGroup = rs.SingleOrDefault(p => p.ID == v_lokasi.IDGroup);
                lbl_location.Text = v_lokasiGroup.Nama + " (" + v_lokasi.Nama + ")";
                lbl_typelocation.Text = "Outlet";
            }
        }
        else if (tipelokasi == "2")
        {
            if (Convert.ToInt32(lokasi) > 0)
            {
                var rs = from p in db.LocExhibitionGroup where !p.Disable select p;
                var rb = from p in db.LocExhibition where !p.Disable select p;
                var v_lokasi = rb.SingleOrDefault(p => p.ID == Convert.ToInt32(lokasi));
                var v_lokasiGroup = rs.SingleOrDefault(p => p.ID == v_lokasi.IDGroup);
                lbl_location.Text = v_lokasiGroup.Nama + " (" + v_lokasi.Nama + ")";
                lbl_typelocation.Text = "Exhibition";
            }
        }
        else
        {
            lbl_typelocation.Text = "Semua";
            lbl_location.Text = "Semua";
        }
        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }

    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.TradeIn

                 select p;

        string Status = "";
        Status = Request.QueryString["status"];
        if (Status == "1")
            rs = rs.Where(p => p.Draft == false);
        else if (Status == "2")
            rs = rs.Where(p => p.Draft == true);

        //check admin sales
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int Lokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

        if ((Tipe == 1 || Tipe == 2) && Lokasi > 0)
        {
            rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "" || p.CustomerNama.Contains(Request.QueryString["kw"]));

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalQtyTradeIn = 0m;
            decimal TotalBerat = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                decimal Harga = 0m;
                int QtyTradeIn = 0;


                QtyTradeIn = r.Resell.ResellDJ.Count() + r.Resell.ResellPG.Count();

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                /*td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = QtyTradeIn.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight).ToString("N3");
                tr.Cells.Add(td);

                try
                {
                    Harga = Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.HargaRupiah)) + Convert.ToDecimal(r.Resell.ResellPG.Sum(p => p.HargaRupiah));
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = Harga.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Total += Harga;
                TotalQtyTradeIn += QtyTradeIn;
                TotalBerat += r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight);
                TotalButir += Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty));
                TotalCarat += r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 5;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBerat).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(Total).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}

