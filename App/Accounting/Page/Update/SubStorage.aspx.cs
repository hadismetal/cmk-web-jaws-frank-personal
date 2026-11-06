using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ : System.Web.UI.Page {
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
		App.Protect();

		if (!IsPostBack) {

			
		}
	}

    protected void updatePLU(object sender, EventArgs e)
    {
        if (kw.Text != "")
        {
            var u = db.StockProductDJ.SingleOrDefault(p => p.Nomor.Equals(kw.Text) && !p.Draft.Value);
            if(u!=null)
            {
                StockLedgerDJ l = new StockLedgerDJ();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 7; // See Library DB - Ledger
                string Keterangan = "-"; // Commonly filled by document number or id 
                //if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!
                Keterangan = keterangan.Text;


                string Lokasi = "";
                if (u.StockActualDJ.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + u.StockActualDJ.IDLokasi, Db.Cnn_DB);
                else if (u.StockActualDJ.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + u.StockActualDJ.IDLokasi, Db.Cnn_DB);
                else if (u.StockActualDJ.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + u.StockActualDJ.IDLokasi, Db.Cnn_DB);

                //Input
                l.IDProduct = u.ID;
                l.TipeLokasi = u.StockActualDJ.TipeLokasi;
                l.IDLokasi = u.StockActualDJ.IDLokasi;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerDJ.InsertOnSubmit(l);
                db.SubmitChanges();

                u.StockActualDJ.InHand = 1m;
                u.StockActualDJ.Substorage = 1;
                db.SubmitChanges();

                Response.Redirect("PLU_UpdateSubstorage.aspx?d=1");
            }
        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            kw.Text = Request.QueryString["kw"];
        }
    }
}