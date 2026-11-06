using System;
using System.Linq;

public partial class Page_Approval_LD_Pricing : System.Web.UI.Page {
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
    App price = new App();

    decimal Setting = 0m;

	protected int myID { get { return App.QueryInt(this); } }

	//Exception
	protected void PricingError(string sebab) {
		Response.Redirect("LD_Form.aspx?id=" + myID + "&prcerror=" + sebab);
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Delete

			//Process
			
				saveStone(d);

			//Redirect
			Response.Redirect("LD_PricingSubmit.aspx?id=" + myID);
		}
	}

	//Stone Pricing
	protected void saveStone(StockReceiveLD d) {
		saveStone1B(d);
	}
	protected void saveStone1B(StockReceiveLD d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockReceiveLD_Stone1B select p;
		foreach (var r in rs) {

			StonePricing1B prc = r.Stone1B.StonePricing1B;
            StoneCosting1B cost = r.Stone1B.StoneCosting1B;
			if (prc == null || cost == null) PricingError(sebab);
			else {

				if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue || !prc.InputH.HasValue || !prc.InputP.HasValue) PricingError(sebab);
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

					//Rapaport
					r.HargaInputH = prc.InputH.Value;
					r.HargaInputP = prc.InputP.Value;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    r.HargaMInputH = cost.InputH;
                    r.HargaMInputP =  cost.InputP;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = 0m;
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	
}