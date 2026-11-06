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
        int ID = Convert.ToInt32(Request.QueryString["id"]);

        var rs = from p in db.SalesReceiptDetail
                 where p.SalesReceipt.SalesOrder.StatusVoid == false
                 && p.SalesReceipt.SalesOrder.IDCustomer == ID
                 select p;

        if (Request.QueryString["dari"]!= "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.SalesReceipt.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesReceipt.SalesOrder.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.SalesReceipt.SalesOrder.IDLokasi == c.IDLokasi && p.SalesReceipt.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.SalesReceipt.SalesOrder.IDLokasi.Value) && p.SalesReceipt.SalesOrder.TipeLokasi == 1);
        }

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalMDR = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.SalesReceipt.SalesOrder.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                //td.HorizontalAlign = HorizontalAlign.Right;
                td.Text = Convert.ToDecimal(r.Nominal.Value).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesReceipt.SalesOrder.CustomerNama;
                tr.Cells.Add(td);

                string EDC = "-";
                try
                {
                    EDC = r.EDCList.Nama;
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = EDC;
                tr.Cells.Add(td);

                string BankIssuer = "-";
                try
                {
                    BankIssuer = r.BankIssuer.Nama;
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = BankIssuer;
                tr.Cells.Add(td);

                string CCNumber = "-";
                try
                {
                    CCNumber = r.CCNumber;
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = CCNumber;
                tr.Cells.Add(td);

                string CCName = "-";
                try
                {
                    CCName = r.CCName;
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = CCName;
                tr.Cells.Add(td);

                string CC = "-";
                try
                {
                    CC=  r.CardType.Nama;
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = CC;
                tr.Cells.Add(td);

                string Program = "-";
                try
                {
                    Program = r.ProgramCicilan.Nama;
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = CC;
                tr.Cells.Add(td);

                Total += Convert.ToDecimal(r.Nominal);

                tb.Rows.Add(tr);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            //td2.HorizontalAlign = HorizontalAlign.Right;
            td2.Text = "<b>" + Total.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}