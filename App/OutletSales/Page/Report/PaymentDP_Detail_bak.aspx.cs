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
    protected int myID { get { return App.QueryInt(this); } }
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
        var rs = from p in db.SalesReceiptDPPODetail
                 where p.IDSalesReceipt == myID
                 select p;

        if (rs.Count() > 0)
        {

            decimal TotalBayar = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                
                int Qty = 1;

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.SalesReceiptDPPO.NomorDP;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DLC.PaymentType(r.IDPaymentType);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DLC.CardType(r.IDCard.HasValue ? r.IDCard.Value : 0);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DLC.EDC(r.IDEDC.HasValue ? r.IDEDC.Value : 0);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DLC.BankIssuer(r.IDBankIssuer.HasValue ? r.IDBankIssuer.Value : 0);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CCNumber;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DLC.Program(r.IDProgram.HasValue ? r.IDProgram.Value : 0);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nominal.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.MDR.Value.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

            }
        }
    }
}