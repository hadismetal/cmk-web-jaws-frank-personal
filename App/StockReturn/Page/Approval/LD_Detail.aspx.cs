using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_LD : System.Web.UI.Page
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
    protected int ReturnID { get { return App.QueryInt(this, "id"); } }
    protected int ItemID { get { return App.QueryInt(this, "item"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            Set();

        }
    }

    protected void Set()
    {
        var d = db.StockReturnLD.SingleOrDefault(p => p.ID == ReturnID);
        if (d == null)
            Response.Redirect("LD.aspx");
        else
        {

            var rs = from p in db.StockReturnLD_Product
                     where p.IDForm == d.ID
                     select p;
            rs = rs.OrderByDescending(p => p.ID);

            if (rs != null)
            {
                int index = 1;
                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductLD_Stone1B.Stone1B.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductLD_Stone1B.TotalButir).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductLD_Stone1B.TotalCarat).ToString("N3");
                    tr.Cells.Add(td);

                    HiddenField hf = new HiddenField();
                    hf.ID = "hf" + index.ToString();
                    hf.Value = r.IDProduct.ToString();
                    hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    td = new TableCell();
                    td.Controls.Add(hf);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }
    }

    protected void Approve(object sender, EventArgs e)
    {

        var rs = db.StockReturnLD.SingleOrDefault(p=>p.ID == ReturnID);
        if(rs!=null)
        {
            var det = from p in db.StockReturnLD_Product
                      where p.IDForm == ReturnID
                      select p;
            foreach (var r in det)
            {
                var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProduct);
                if (x != null)
                {
                    x.ReturnProduct = true;
                    db.SubmitChanges();

                    StockLedgerLD l = new StockLedgerLD();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out
                    int Remark = 9; // See Library DB - Ledger
                    string Keterangan = rs.NoDokumen; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (x.StockActualLD_Stone1B.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                    else if (x.StockActualLD_Stone1B.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);

                    //Input
                    l.IDProduct = x.ID;
                    l.TipeLokasi = x.StockActualLD_Stone1B.TipeLokasi;
                    l.IDLokasi = x.StockActualLD_Stone1B.IDLokasi;
                    l.NamaLokasi = Lokasi;
                    l.Value = Value;
                    l.Remark = Remark;
                    l.Keterangan = Keterangan;

                    //User-Info
                    l.Operator = App.UserNama;
                    l.OperatorTgl = DateTime.Now;

                    //Commit
                    db.StockLedgerLD.InsertOnSubmit(l);
                    db.SubmitChanges();
                }
            }
            rs.Approval = App.UserNama;
            rs.ApprovalTgl = DateTime.Now;
            db.SubmitChanges();
        }

        Response.Redirect("LD.aspx?d=1");
    }
}