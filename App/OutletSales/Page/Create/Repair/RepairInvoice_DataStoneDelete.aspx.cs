using System;
using System.Linq;

public partial class Page_Register_Repair_StoneDelete : System.Web.UI.Page {
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
	protected int myStoneIndex { get { return App.QueryInt(this, "si"); } }
	protected int myTableID { get { return App.QueryInt(this, "tid"); } }

	//Exception
    protected void DataError() { Response.Redirect("RepairInvoice.aspx"); }
    protected void LinkError() { Response.Redirect("RepairInvoice_DataStone.aspx?id=" + myID); }
    protected void ProductError() { Response.Redirect("RepairInvoice_CharProduct02.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Process
			var product = d.DocRepairResult_CharProduct;
			if (product == null) ProductError();
			else {
                switch (myStoneIndex)
                {
                    case 0: save1A(d); break;
                    case 1: save1B(d); break;
                    case 2: save2(d); break;
                    case 3: save3(d); break;
                    case 4: save4(d); break;
                    case 5: save5(d); break;
                }

				//Recalculate
				//Lib.RecalWeight(db, d, product);
			}

			//Redirect
            Response.Redirect("RepairInvoice_DataStone.aspx?id=" + myID);
		}
	}
    protected void save1A(DocRepairResult ms)
    {
        var d = db.DocRepairResult_Stone1A.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.DocRepairResult_Stone1A.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save1B(DocRepairResult ms)
    {
        var d = db.DocRepairResult_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.DocRepairResult_Stone1B.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save2(DocRepairResult ms)
    {
        var d = db.DocRepairResult_Stone2.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.DocRepairResult_Stone2.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save3(DocRepairResult ms)
    {
        var d = db.DocRepairResult_Stone3.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.DocRepairResult_Stone3.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save4(DocRepairResult ms)
    {
        var d = db.DocRepairResult_Stone4.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.DocRepairResult_Stone4.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void save5(DocRepairResult ms)
    {
        var d = db.DocRepairResult_Stone5.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.DocRepairResult_Stone5.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}