using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            int Tipe = 1;
            if (Request.QueryString["tipelokasi"] != "")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            DLC.LocationList(lokasi, Tipe);
            DLC.EDCList(bankissuer);
            DLC.ProgramList(program);
            DLC.BankList(edc);
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
                  where p.SalesReceipt.SalesOrder.StatusVoid == false
                  && p.IDEDC.HasValue && p.IDProgram.HasValue
                  && p.IDBankIssuer.HasValue
                  select new
                  {
                      IDBank = p.IDBankIssuer.Value,
                      IDEDC = p.IDEDC.Value,
                      IDProgram = p.IDProgram.Value,
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
                 && p.IDEDC.HasValue && p.IDProgram.HasValue
                 && p.IDBankIssuer.HasValue
                 select new
                 {
                     IDBank = p.IDBankIssuer.Value,
                     IDEDC = p.IDEDC.Value,
                     IDProgram = p.IDProgram.Value,
                     IDLokasi = p.SalesReceiptDPPO.SalesOrder.IDLokasi,
                     TipeLokasi = p.SalesReceiptDPPO.SalesOrder.TipeLokasi,
                     Tgl = p.SalesReceiptDPPO.SalesOrder.Tgl.Value,
                     PaymentType = p.PaymentType,
                     Nominal = p.Nominal.Value,
                     MDR = p.MDR.Value,
                     IDCustomer = p.SalesReceiptDPPO.SalesOrder.IDCustomer
                 });

        //check admin sales
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

        if (Tipe == 1 || Tipe == 2)
        {
            rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
        }

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        int EDC = Convert.ToInt32(edc.SelectedValue);
        int BankIssuer = Convert.ToInt32(bankissuer.SelectedValue);
        int ProgramCicilan = Convert.ToInt32(program.SelectedValue);
        if (EDC > 0)
            rs = rs.Where(p => p.IDEDC == EDC);
        if (BankIssuer > 0)
            rs = rs.Where(p => p.IDBank == BankIssuer);
        if (ProgramCicilan > 0)
            rs = rs.Where(p => p.IDProgram == ProgramCicilan);

        var rslt = rs.GroupBy(p => new { p.IDEDC, p.IDProgram, p.IDBank });

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

                

                string EDCName = Db.SingleString("SELECT Nama FROM BankIssuer WHERE ID=" + r.Key.IDBank, Db.Cnn_DB);

                td = new TableCell();
                td.Text = EDCName;
                tr.Cells.Add(td);

                string Bank = Db.SingleString("SELECT Nama FROM EDCList WHERE ID=" + r.Key.IDEDC, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Bank;
                tr.Cells.Add(td);

                string Program = Db.SingleString("SELECT Nama FROM ProgramCicilan WHERE ID=" + r.Key.IDEDC, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Program;
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

                TotalMDR += MDR;

                tb.Rows.Add(tr);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 4;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.HorizontalAlign = HorizontalAlign.Left;
            td2.Text = "<b>" + TotalMDR.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
            edc.SelectedValue = Request.QueryString["edc"];
            program.SelectedValue = Request.QueryString["program"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&edc=" + edc.SelectedValue;
        href += "&program=" + program.SelectedValue;

        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
        }
        lokasi.Focus();
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "MDR_Print.aspx?id=1";
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&edc=" + edc.SelectedValue;
        href += "&program=" + program.SelectedValue;
        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "MDR_Export.aspx?id=1";
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&edc=" + edc.SelectedValue;
        href += "&program=" + program.SelectedValue;
        Response.Redirect(href);
    }
}