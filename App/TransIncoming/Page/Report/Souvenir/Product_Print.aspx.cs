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
	protected int id { get { return App.QueryInt(this); } }

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

        try
        {
            var rs = db.StockIncomingSouvenir.SingleOrDefault(p => p.ID == id);

            FillHeader();
            FillTable();
            //FillItem();
            //FillSubStorage();
        }
        catch (Exception ex)
        {

        }

	}
	protected void FillHeader()
    {
        var rs = db.StockIncomingSouvenir.SingleOrDefault(p => p.ID == id);

        string A = "";
        string T = "";

        // Asal
        if (rs.StockOutgoingSouvenir.TipeAsal == 0) 
        {
            var rsA = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.StockOutgoingSouvenir.IDAsal);
            A = rsA.Nama + " (" + rsA.LocWarehouseGroup.Nama + ")"; 
        }
        else if (rs.StockOutgoingSouvenir.TipeAsal == 1)
        {
            var rsA = db.LocOutlet.SingleOrDefault(p => p.ID == rs.StockOutgoingSouvenir.IDAsal);
            A = rsA.Nama + " (" + rsA.LocOutletGroup.Nama + ")";
        }
        /*else if (rs.TipeAsal == 2)
        {
            var rsA = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDAsal);
            A = rsA.Nama + " (" + rsA.LocExhibitionGroup.Nama + ")";
        }*/

        // Tujuan
        if (rs.StockOutgoingSouvenir.TipeTujuan == 0)
        {
            var rsA = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.StockOutgoingSouvenir.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocWarehouseGroup.Nama + ")";
        }
        else if (rs.StockOutgoingSouvenir.TipeTujuan == 1)
        {
            var rsA = db.LocOutlet.SingleOrDefault(p => p.ID == rs.StockOutgoingSouvenir.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocOutletGroup.Nama + ")";
        }
        /*else if (rs.TipeTujuan == 2)
        {
            var rsA = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocExhibitionGroup.Nama + ")";
        }*/

        nodokumen.Text = rs.Nomor;
        date.Text = Str.Tgl(rs.Tgl);
        from.Text = A;
        to.Text = T;
    }
    protected void FillTable()
    {
        var rs = from p in db.StockIncomingSouvenir_Product where p.IDForm == id select p;

        TableHeaderRow htr;
        TableHeaderCell htd;

        TableRow tr;
        TableCell td;

        // Header
        int Qty = 0;
        // List

        int index = 1;
        
        foreach (var item in rs)
        {
            tr = new TableRow();

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = item.Souvenir.Kode; // PLU
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = item.Souvenir.Nama; // Gross Weight
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(item.Qty); // Total Butir
            td.CssClass = "right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            Qty += Convert.ToInt32(item.Qty);
        }

        tr = new TableRow();

        td = new TableCell();
        td.Text = "Total Quantity";
        td.ColumnSpan = 3;
        td.CssClass = "right total";
        tr.Cells.Add(td);

        td = new TableCell();
        td.HorizontalAlign = HorizontalAlign.Right;
        td.Text = Qty.ToString(); // PLU
        td.CssClass = "right total";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
}