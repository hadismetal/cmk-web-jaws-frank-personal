using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class LD_Print : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Fill();
        }
    }
    protected void Error(string Message)
    {

    }
    protected void Fill()
    {
        //Form-Default
        FillTable();
    }
    protected void FillTable()
    {
        TableRow tr;
        TableCell td;

        tb.Visible = true;

        //SELECT
        var rs = (from p in db.StockIncomingLD_Product where p.StockIncomingLD.ID == myID && p.SubStorageResult == 1 select p); // Outgoing


        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {

            foreach (var item in rs)
            {

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";

                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductLD_Stone1B.GIA;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductLD_Stone1B.Nomor;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }
}