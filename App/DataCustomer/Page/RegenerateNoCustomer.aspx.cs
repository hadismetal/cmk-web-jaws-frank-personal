using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_CustomerSubmit : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {

		if (!IsPostBack) {
			regenerateNoCustomer();
		}
	}

	//Exception
    protected void regenerateNoCustomer()
    {
        var rs = from p in db.DataCustomer
                 where p.NoCustomer == ""
                 && p.Draft == false
                 select p;
        foreach (var r in rs)
        {
                var d = (from p in db.SalesOrder
                         where p.IDCustomer == r.ID
                         && p.Draft == false
                         orderby p.Tgl
                         select p);

                if (d.Count() > 0)
                {
                    var u = d.First();
                    string Kode = "";
                    if (u.TipeLokasi == 1)
                        Kode = Db.SingleString("SELECT Kode FROM LocOutlet WHERE ID=" + u.IDLokasi, Db.Cnn_DB);
                    else if (u.TipeLokasi == 2)
                        Kode = Db.SingleString("SELECT Kode FROM LocExhibition WHERE ID=" + u.IDLokasi, Db.Cnn_DB);

                    r.NoCustomer = Generate_Nomor(Kode);
                    db.SubmitChanges();
                }
            
        }
    }

    protected string Generate_Nomor(string Kode)
    {
        string x = string.Empty;

        int Tahun = DateTime.Today.Year;
        int Bulan = DateTime.Today.Month;

        /*int Nomor = db.StockReceiveDJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;

        return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        int Nomor = 1;

        try
        {
            /*var s = (from p in db.DataCustomer
                     where p.Tgl.Year == Tahun 
                     && p.NoCustomer != null
                     orderby p.NoCustomer.Remove(3).EndsWith(Kode) descending
                     select p).First();*/

            Nomor = Db.SingleInteger("SELECT TOP 1 CAST(RIGHT(NoCustomer,6) AS INT) FROM DataCustomer WHERE NoCustomer is not null "
                        + "AND LEFT(NoCustomer,3) = '" + Kode + "'"
                        + "AND LEN(NoCustomer) = 12"
                        + "ORDER BY RIGHT(NoCustomer,6) DESC", Db.Cnn_DB);

            //if (s.NoCustomer != null)
            Nomor = Nomor + 1;
        }
        catch (Exception ex) { }

        string PLU = Kode + "-" + Tahun.ToString().Remove(0, 2) + Str.KodeFormat(Nomor, 6);

        return PLU;

    }
}