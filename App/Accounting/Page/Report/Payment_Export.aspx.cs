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
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Payment_Export.xls");
            fillTable(sender, e);  
        }
    }

    //Display Table
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
                 ).Concat(
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

        try
        {
            int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            int Lokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

            if (Tipe == 1 || Tipe == 2)
            {

                rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
            }
        }
        catch (Exception ex) { }

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        int Group = Convert.ToInt32(Request.QueryString["groupby"]);

        if (Group == 0)
        {
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
                    td.HorizontalAlign = HorizontalAlign.Right;
                    td.Text = Convert.ToDecimal(r.Sum(p => p.Nominal)).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    td.Text = Convert.ToDecimal(r.Sum(p => p.MDR)).ToString("N0");
                    tr.Cells.Add(td);

                    Total += Convert.ToDecimal(r.Sum(p => p.Nominal));
                    TotalMDR += Convert.ToDecimal(r.Sum(p => p.MDR));

                    tb.Rows.Add(tr);
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 2;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Right;
                td2.Text = "<b>" + Total.ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Right;
                td2.Text = "<b>" + TotalMDR.ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
        else if (Group == 1)
        {
            var rslt = rs.GroupBy(p => p.IDCustomer);

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

                    string Customer = "";
                    Customer = Db.SingleString("SELECT Nama FROM DataCustomer WHERE ID=" + r.Key.Value, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Customer;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    td.Text = Convert.ToDecimal(r.Sum(p => p.Nominal)).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    td.Text = Convert.ToDecimal(r.Sum(p => p.MDR)).ToString("N0");
                    tr.Cells.Add(td);

                    Total += Convert.ToDecimal(r.Sum(p => p.Nominal));
                    TotalMDR += Convert.ToDecimal(r.Sum(p => p.MDR));

                    tb.Rows.Add(tr);
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 2;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Right;
                td2.Text = "<b>" + Total.ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Right;
                td2.Text = "<b>" + TotalMDR.ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
    }
}