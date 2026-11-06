using System;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;

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
    Modelize cross = new Modelize();
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
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
        var rs = cross.IncomingLD(myID).SingleOrDefault();

        string A = "";
        string T = "";

        // Asal
        if (rs.CrossBrand_StockOutgoingLD.TipeAsal == 0)
        {
            var rsA = cross.Warehouses(rs.CrossBrand_StockOutgoingLD.IDBrandAsal, rs.CrossBrand_StockOutgoingLD.IDAsal, rs.CrossBrand_StockOutgoingLD.TipeAsal).SingleOrDefault();
            A = rsA.Nama + " (" + rsA.LocGroup + ")";
        }
        else if (rs.CrossBrand_StockOutgoingLD.TipeAsal == 1)
        {
            var rsA = cross.Outlets(rs.CrossBrand_StockOutgoingLD.IDBrandAsal, rs.CrossBrand_StockOutgoingLD.IDAsal, rs.CrossBrand_StockOutgoingLD.TipeAsal).SingleOrDefault();
            A = rsA.Nama + " (" + rsA.LocGroup + ")";
        }
        else if (rs.CrossBrand_StockOutgoingLD.TipeAsal == 2)
        {
            var rsA = cross.Exhibitions(rs.CrossBrand_StockOutgoingLD.IDBrandAsal, rs.CrossBrand_StockOutgoingLD.IDAsal, rs.CrossBrand_StockOutgoingLD.TipeAsal).SingleOrDefault();
            A = rsA.Nama + " (" + rsA.LocGroup + ")";
        }

        // Tujuan
        if (rs.CrossBrand_StockOutgoingLD.TipeTujuan == 0)
        {
            var rsA = cross.Warehouses(rs.CrossBrand_StockOutgoingLD.IDBrandTujuan, rs.CrossBrand_StockOutgoingLD.IDTujuan, rs.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault();
            T = rsA.Nama + " (" + rsA.LocGroup + ")";
        }
        else if (rs.CrossBrand_StockOutgoingLD.TipeTujuan == 1)
        {
            var rsA = cross.Outlets(rs.CrossBrand_StockOutgoingLD.IDBrandTujuan, rs.CrossBrand_StockOutgoingLD.IDTujuan, rs.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault();
            T = rsA.Nama + " (" + rsA.LocGroup + ")";
        }
        else if (rs.CrossBrand_StockOutgoingLD.TipeTujuan == 2)
        {
            var rsA = cross.Exhibitions(rs.CrossBrand_StockOutgoingLD.IDBrandTujuan, rs.CrossBrand_StockOutgoingLD.IDTujuan, rs.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault();
            T = rsA.Nama + " (" + rsA.LocGroup + ")";
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
        var rs = cross.IncomingLD(myID).SingleOrDefault().CrossBrand_StockIncomingLD_Product; // Outgoing

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            //Diamond Reguler
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