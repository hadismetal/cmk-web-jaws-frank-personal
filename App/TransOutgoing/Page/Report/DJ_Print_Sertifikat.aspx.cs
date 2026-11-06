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
            FillHeader();
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
    protected void FillHeader()
    {
        var rs = db.StockOutgoingDJ.SingleOrDefault(p => p.ID == myID);

        string A = "";
        string T = "";

        // Asal
        if (rs.TipeAsal == 0)
        {
            var rsA = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDAsal);
            A = rsA.Nama + " (" + rsA.LocWarehouseGroup.Nama + ")";
        }
        else if (rs.TipeAsal == 1)
        {
            var rsA = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDAsal);
            A = rsA.Nama + " (" + rsA.LocOutletGroup.Nama + ")";
        }
        else if (rs.TipeAsal == 2)
        {
            var rsA = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDAsal);
            A = rsA.Nama + " (" + rsA.LocExhibitionGroup.Nama + ")";
        }

        // Tujuan
        if (rs.TipeTujuan == 0)
        {
            var rsA = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocWarehouseGroup.Nama + ")";
        }
        else if (rs.TipeTujuan == 1)
        {
            var rsA = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocOutletGroup.Nama + ")";
        }
        else if (rs.TipeTujuan == 2)
        {
            var rsA = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocExhibitionGroup.Nama + ")";
        }

        nodokumen.InnerText = rs.Nomor;
        tanggalkirim.InnerText = Str.Tgl(rs.Tgl);
        asal.InnerText = A;
        tujuan.InnerText = T;
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
            //Diamond Reguler
            foreach (var item in rs)
            {

                var crt = from p in db.StockProductDJ_Stone1A
                          where p.IDForm == item.StockProductDJ.ID
                          select p;
                if (crt != null)
                {
                    foreach (var c in crt)
                    {
						if (c.Stone1A.Nama.EndsWith("FV") || c.Stone1A.Nama.EndsWith("SR") || c.Stone1A.Nama.Contains("FV"))
                        {
							string GIA = Db.SingleString("SELECT TOP 1 ISNULL(CertificateNumber,'') FROM StockProductDJ_Stone1A WHERE ID = " + c.ID + " ", Db.Cnn_DB);
							if (GIA != "")
							{
								if (!Response.IsClientConnected) break;

								tr = new TableRow();

								td = new TableCell();
								td.Text = "<p>" + index++ + ".</p>";

								td.VerticalAlign = VerticalAlign.Top;
								tr.Cells.Add(td);

								td = new TableCell();
								td.Text = GIA;
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
            //Diamond Dossier
            foreach (var item in rs)
            {

                var crt = from p in db.StockProductDJ_Stone1B
                          where p.IDForm == item.StockProductDJ.ID
                          select p;
                if (crt != null)
                {

                    foreach (var c in crt)
                    {
                        string GIA = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') FROM StockProductDJ_Stone1B WHERE ID = " + c.ID + " ", Db.Cnn_DB);
                        if (GIA != "")
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
            //Colour Diamond
            foreach (var item in rs)
            {
                var crt = from p in db.StockProductDJ_Stone2
                          where p.IDForm == item.StockProductDJ.ID
                          select p;
                if (crt != null)
                {

                    foreach (var c in crt)
                    {
                        string GIA = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') FROM StockProductDJ_Stone2 WHERE ID = " + c.ID + " ", Db.Cnn_DB);
                        if (GIA != "")
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
            //Precious Stone
            foreach (var item in rs)
            {
                var crt = from p in db.StockProductDJ_Stone3
                          where p.IDForm == item.StockProductDJ.ID
                          select p;
                if (crt != null)
                {

                    foreach (var c in crt)
                    {
                        string GIA = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') FROM StockProductDJ_Stone3 WHERE ID = " + c.ID + " ", Db.Cnn_DB);
                        if (c.GIA != "")
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

            //Semi Stone
            foreach (var item in rs)
            {
                var crt = from p in db.StockProductDJ_Stone4
                          where p.IDForm == item.StockProductDJ.ID
                          select p;
                if (crt != null)
                {

                    foreach (var c in crt)
                    {
                        string GIA = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') FROM StockProductDJ_Stone4 WHERE ID = " + c.ID + " ", Db.Cnn_DB);
                        if (c.GIA != "")
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

            //Pearl
            foreach (var item in rs)
            {
                var crt = from p in db.StockProductDJ_Stone5
                          where p.IDForm == item.StockProductDJ.ID
                          select p;
                if (crt != null)
                {

                    foreach (var c in crt)
                    {
                        string GIA = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') FROM StockProductDJ_Stone5 WHERE ID = " + c.ID + " ", Db.Cnn_DB);
                        if (c.GIA != "")
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
}