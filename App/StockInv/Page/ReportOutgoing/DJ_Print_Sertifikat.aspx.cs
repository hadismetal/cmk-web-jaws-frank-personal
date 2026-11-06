using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class DJ_Print : System.Web.UI.Page
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

	protected void Page_Load(object sender, EventArgs e) {
		
        if (!IsPostBack) {
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
        var rs = (from p in db.StockOutgoingDJ_Product where p.StockOutgoingDJ.ID == myID select p); // Outgoing

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {

            foreach (var item in rs)
            {
                
                var crt = from p in db.StockProductDJ_Stone1B
                          where p.IDForm == item.StockProductDJ.ID
                          select p;
                if (crt != null)
                {
                    
                    foreach (var c in crt)
                    {

                        if (!Response.IsClientConnected) break;

                        tr = new TableRow();

                        td = new TableCell();
                        td.Text = "<p>" + index++ + ".</p>";

                        td.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = c.GIA;
                        td.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = item.StockProductDJ.Nomor;
                        td.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = item.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                        td.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
        }
    }
}