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

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            int Tipe = 0;
            try
            {
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            }
            catch (Exception ex) { }

            DLC.LocationList(lokasi, Tipe);

            int UserID = App.UserID;
            var d = from p in db.DataAdminSales
                    where p.IDUser == UserID
                    select p;
            if (d.Count() > 0)
            {
                tipelokasi.Items.Clear();
                tipelokasi.Enabled = false;
                lokasi.Items.Clear();
                lokasi.Enabled = false;
            }

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
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
                  where 1==1
                  && p.SalesReceipt.SalesOrder.StatusVoid == false
                  && !p.SalesReceipt.SalesOrder.Draft
                  select new
                  {
                      ID = p.ID,
                      IDLokasi = p.SalesReceipt.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesReceipt.SalesOrder.TipeLokasi,
                      Tgl = p.SalesReceipt.SalesOrder.Tgl.Value,
                      PaymentType = p.IDPaymentType,
                      PaymentNama = p.PaymentType.Nama,
                      Nominal = p.Nominal.Value,
                      MDR = p.MDR.Value,
                      IDCustomer = p.SalesReceipt.SalesOrder.IDCustomer
                  }
                 ).Union(
                 from p in db.SalesReceiptDPPODetail
                 where 1==1
                 && p.SalesReceiptDPPO.SalesOrder.StatusVoid == false
                 && p.SalesReceiptDPPO.StatusDP == 0
                 select new
                 {
                      ID = p.ID,
                      IDLokasi = p.SalesReceiptDPPO.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesReceiptDPPO.SalesOrder.TipeLokasi,
                      Tgl = p.SalesReceiptDPPO.SalesOrder.Tgl.Value,
                      PaymentType = p.IDPaymentType,
                      PaymentNama = p.PaymentType.Nama,
                      Nominal = p.Nominal.Value,
                      MDR = p.MDR.Value,
                      IDCustomer = p.SalesReceiptDPPO.SalesOrder.IDCustomer
                  });

        //var rs = (from p in db.SalesReceiptDetail
        //          where p.SalesReceipt.SalesOrder.StatusVoid == false
        //          select new
        //          {
        //              IDLokasi = p.SalesReceipt.SalesOrder.IDLokasi,
        //              TipeLokasi = p.SalesReceipt.SalesOrder.TipeLokasi,
        //              Tgl = p.SalesReceipt.SalesOrder.Tgl.Value,
        //              PaymentType = p.PaymentType,
        //              Nominal = p.Nominal.Value,
        //              MDR = p.MDR.Value,
        //              IDCustomer = p.SalesReceipt.SalesOrder.IDCustomer
        //          }
        //         );

        try
        {
            int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
            int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

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

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        int Group = Convert.ToInt32(groupby.SelectedValue);

        if (Group == 0)
        {
            //foreach (var d in rs)
            //{
            //    if (d.PaymentNama.ToString() == "Transfer")
            //    {
            //        lit_test.Text += d.ID.ToString() + " - " + d.PaymentNama.ToString() 
            //                        + " - " + d.Nominal.ToString("N0")
            //                        + "<br />";
            //    }
            //}

            var rslt = rs.GroupBy(p => new { p.PaymentType, p.PaymentNama });

            rslt = rslt.OrderBy(p => p.Key.PaymentType);

            //var rslt = from p in rs
            //           group p by new { p.PaymentType, p.PaymentNama } into g
            //           select new
            //           {

            //           };

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
                    td.Text = TD_HyperLink(r.Key.PaymentNama, r.Key.PaymentType);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.Sum(p => p.Nominal)).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
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
                td2.Text = "<b>" + Total.ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + TotalMDR.ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
        else if (Group == 1)
        {
            var rslt = rs.GroupBy(p => p.IDCustomer);

            //rslt = rslt.OrderBy(p => p.Key.IDCustomer);

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
                    td.Text = TD_HyperLink(r.Key.Value, Customer);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.Sum(p => p.Nominal)).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
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
                td2.Text = "<b>" + Total.ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + TotalMDR.ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
            groupby.SelectedValue = Request.QueryString["groupby"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&groupby=" + groupby.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "Payment_Export.aspx?id=1";

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&groupby=" + groupby.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "Payment_Print.aspx?id=1";

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&groupby=" + groupby.SelectedValue;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(string Tipe, int ID)
    {
        string x = string.Empty;

        string TipeLokasi = tipelokasi.SelectedValue;
        string IDLokasi = lokasi.SelectedValue;

        x += "<a target=\"_blank\" href=\"Payment_Detail.aspx?id=" + ID + "&dari=" + dari.Text + "&sampai=" + sampai.Text +"&tipelokasi=" + TipeLokasi + "&lokasi=" + IDLokasi + "\">";
        x += Tipe;
        x += "</a>";

        return x;
    }
    protected string TD_HyperLink(int IDCustomer, string CustomerNama)
    {
        string x = string.Empty;

        x += "<a target=\"_blank\" href=\"Payment_DetailCustomer.aspx?id=" + IDCustomer + "&dari=" + dari.Text + "&sampai=" + sampai.Text + "\">";
        x += CustomerNama;
        x += "</a>";

        return x;
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }
}