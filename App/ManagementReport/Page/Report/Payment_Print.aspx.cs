using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Payment_Print : System.Web.UI.Page
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
            fillHeader();
            fillTable(sender, e);
        }
    }

    protected void fillHeader()
    {
        string ada = Request.QueryString["id"];
        string from = Request.QueryString["dari"];
        string to = Request.QueryString["sampai"];
        string tipelokasi = Request.QueryString["tipelokasi"];
        string lokasi = Request.QueryString["lokasi"];

        lbl_from.Text = from.ToString();
        lbl_to.Text = to.ToString();


        if (tipelokasi == "1")
        {
            if (Convert.ToInt32(lokasi) > 0)
            {
                var rs = from p in db.LocOutletGroup where !p.Disable select p;
                var rb = from p in db.LocOutlet where !p.Disable && p.IDGroup == p.LocOutletGroup.ID select p;
                var v_lokasi = rb.SingleOrDefault(p => p.ID == Convert.ToInt32(lokasi));
                var v_lokasiGroup = rs.SingleOrDefault(p => p.ID == v_lokasi.IDGroup);
                lbl_location.Text = v_lokasiGroup.Nama + " (" + v_lokasi.Nama + ")";
                lbl_type_location.Text = "Outlet";
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
                lbl_type_location.Text = "Exhibition";
            }
        }
        else
        {
            lbl_type_location.Text = "Semua";
            lbl_location.Text = "Semua";
        }
        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }

    protected void fillTable(object sender, EventArgs e)
    {
        /*var rs = from p in db.SalesReceiptDetail
                 where p.SalesReceipt.SalesOrder.StatusVoid == false
                 group p by p.PaymentType into g
                 select new { Tipe = g.Key.Nama, Jumlah = g.Sum(x=>x.Nominal)};*/

        var rs = (from p in db.SalesReceiptDetail
                  where p.SalesReceipt.SalesOrder.StatusVoid == false
                  select new
                  {
                      IDLokasi = p.SalesReceipt.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesReceipt.SalesOrder.TipeLokasi,
                      Tgl = p.SalesReceipt.SalesOrder.Tgl.Value,
                      PaymentType = p.PaymentType,
                      Nominal = p.Nominal.Value,
                      MDR = p.MDR.Value,
                      IDCustomer = p.SalesReceipt.SalesOrder.IDCustomer
                  }
                 ).Union(
                 from p in db.SalesReceiptDPPODetail
                 where p.SalesReceiptDPPO.SalesOrder.StatusVoid == false
                 select new
                 {
                     IDLokasi = p.SalesReceiptDPPO.SalesOrder.IDLokasi,
                     TipeLokasi = p.SalesReceiptDPPO.SalesOrder.TipeLokasi,
                     Tgl = p.SalesReceiptDPPO.SalesOrder.Tgl.Value,
                     PaymentType = p.PaymentType,
                     Nominal = p.Nominal.Value,
                     MDR = p.MDR.Value,
                     IDCustomer = p.SalesReceiptDPPO.SalesOrder.IDCustomer
                 });

        //check admin sales
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int Lokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

        if (Tipe == 1 || Tipe == 2)
        {
            rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
        }

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        var rslt = rs.GroupBy(p => p.PaymentType);

        if (rslt.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalMDR = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rslt)
            {

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Key.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Left;
                td.Text = Convert.ToDecimal(r.Sum(p => p.Nominal)).ToString("N0");
                tr.Cells.Add(td);

                decimal MDR = 0m;

                try
                {
                    MDR = Convert.ToDecimal(r.Sum(p => p.MDR));
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Left;
                td.Text = MDR.ToString("N0");
                tr.Cells.Add(td);

                Total += Convert.ToDecimal(r.Sum(p => p.Nominal));
                TotalMDR += MDR;

                tb.Rows.Add(tr);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.HorizontalAlign = HorizontalAlign.Left;
            td2.Text = "<b>" + Total.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.HorizontalAlign = HorizontalAlign.Left;
            td2.Text = "<b>" + TotalMDR.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}