using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_DJ_Void : System.Web.UI.Page
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
        App.Protect();
        App.ProtectRole(Role.Manager);

        var d = db.StockRetireDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;
            d.Status = 2;
            d.StockProductDJ.StatusLebur = false;
            d.Keterangan = "Void Lebur";
            db.SubmitChanges();
            try {
                int WO = Db.SingleInteger("SELECT IDWO FROM StockRetireDJ WHERE ID = " + myID.ToString(), Db.Cnn_DB);
                int QtyWO = Db.SingleInteger("SELECT COUNT(ID) FROM StockRetireDJ WHERE IDWO = " + WO.ToString(), Db.Cnn_DB);

                if (QtyWO > 1) Db.Execute("EXEC sp_deleteWorkOrderLebur " + myID.ToString(), Db.Cnn_DB);
                else Db.Execute("EXEC sp_deleteWorkOrderLebur " + WO.ToString() + ", 'ALL'", Db.Cnn_DB);
            }
            catch {}
            var GetPrev = (from p in db.StockLedgerDJ
                           where p.IDProduct == d.IDProduct && p.Remark == 4 && p.Value == 1
                           orderby p.ID descending
                           select p).First();

            StockLedgerDJ sldj = new StockLedgerDJ();
            sldj.IDProduct = GetPrev.IDProduct;
            sldj.IDLokasi = GetPrev.IDLokasi;
            sldj.Keterangan = "Void " + d.Nomor;
            sldj.NamaLokasi = GetPrev.NamaLokasi;
            sldj.Operator = App.UserNama;
            sldj.OperatorTgl = DateTime.Now;
            //sldj.Remark = GetPrev.Remark;
            sldj.Remark = 10; // void = 10 --> look at library - db-ledger
            sldj.TipeLokasi = GetPrev.TipeLokasi;
            sldj.Value = 1;

            db.StockLedgerDJ.InsertOnSubmit(sldj);
            db.SubmitChanges();

            DeleteRetireLD(d);

            Response.Redirect("DJ.aspx?&d=1");
        }

    }

    private void DeleteRetireLD(StockRetireDJ d)
    {
        DataTable dt = Db.Rs("SELECT IDProduct FROM StockRetireDJLD WHERE IDForm = " + myID.ToString(), Db.Cnn_DB);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var ld = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == Convert.ToInt32(row["IDProduct"]));

                ld.ReturnProduct = false;
                ld.StockActualLD_Stone1B.InHand = 1;
                db.SubmitChanges();

                StockLedgerLD ledger = new StockLedgerLD();
                ledger.IDProduct = ld.ID;
                ledger.IDLokasi = ld.StockActualLD_Stone1B.IDLokasi;
                ledger.TipeLokasi = ld.StockActualLD_Stone1B.TipeLokasi;
                ledger.Value = 1;
                ledger.Keterangan = "Void " + d.Nomor;
                ledger.NamaLokasi = ld.StockActualLD_Stone1B.NamaLokasi;
                ledger.Remark = 10;
                ledger.Operator = App.UserNama;
                ledger.OperatorTgl = DateTime.Now;

                db.StockLedgerLD.InsertOnSubmit(ledger);
                db.SubmitChanges();
            }
        }
    }
}