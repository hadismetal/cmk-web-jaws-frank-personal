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
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=MDR_Export.xls");
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

        int EDC = Convert.ToInt32(Request.QueryString["edc"]);
        int BankIssuer = Convert.ToInt32(Request.QueryString["bankissuer"]);
        int ProgramCicilan = Convert.ToInt32(Request.QueryString["program"]);
        if (EDC > 0)
            rs = rs.Where(p => p.IDEDC == EDC);
        if (BankIssuer > 0)
            rs = rs.Where(p => p.IDBank == BankIssuer);
        if (ProgramCicilan > 0)
            rs = rs.Where(p => p.IDProgram == ProgramCicilan);

        var rslt = rs.GroupBy(p => new {p.IDEDC,p.IDProgram,p.IDBank});

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
                td.Text = MDR.ToString("N0").Replace(".", "").Replace(",", ".");
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
            td2.Text = "<b>" + TotalMDR.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}