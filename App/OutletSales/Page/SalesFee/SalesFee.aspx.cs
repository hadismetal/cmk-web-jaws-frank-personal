using System;
using System.Linq;

public partial class Page_Create_SalesFeeAccount : System.Web.UI.Page {
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

    private Map_AC _ac;

    private Map_AC ac
    {
        get
        {
            if (_ac == null)
                _ac = new Map_AC();
            return _ac;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (ac != null)
	        ac.Dispose();
	    base.Dispose();
	}
	protected int myIDGroup { get { return App.QueryInt(this, "idgroup"); } }
	protected int myIDBrand { get { return App.QueryInt(this, "idbrand"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
            App.NumberFormat(totalpoin);
            App.NumberFormat(full);
            App.NumberFormat(ae);
            App.NumberFormat(dj);
            App.NumberFormat(pg);
            App.NumberFormat(lm);
            App.NumberFormat(pc1);
            App.NumberFormat(pc2);
            App.NumberFormat(pc3);
            App.NumberFormat(pc4);
            App.NumberFormat(inc1);
            App.NumberFormat(inc2);
            App.NumberFormat(inc3);
            App.NumberFormat(inc4);
            App.NumberFormat(kom1);
            App.NumberFormat(kom2);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SalesFee.aspx"); }

    protected void CheckBulan(object sender, EventArgs e)
    {
        if (pertahun.Checked == true)
        {
            bulan.Enabled = false;
            bulan_RF.Enabled = false;
        }
        else
        {
            bulan.Enabled = true;
            bulan_RF.Enabled = true;
        }
    }

    protected bool valid
    {
        get
        {
            bool x = true;

            decimal pacing1 = 0m;
            decimal pacing2 = 0m;
            decimal pacing3 = 0m;
            decimal pacing4 = 0m;

            pacing1 = Convert.ToDecimal(pc1.Text);
            pacing2 = Convert.ToDecimal(pc2.Text);
            pacing3 = Convert.ToDecimal(pc3.Text);
            pacing4 = Convert.ToDecimal(pc4.Text);

            if (pacing1 + pacing2 + pacing3 + pacing4 != 100)
                x = false;

            return x;
        }
    }

    protected void AutoCheckBulan(object sender, EventArgs e)
    {
        int Bulan = Convert.ToInt32(bulan.Text);
        if (Bulan < 1 || Bulan > 12)
            bulan.Text = "1";
        bulan.Focus();
    }
	
	protected void save_Click(object sender, EventArgs e) {

        if (valid)
        {
            decimal TotalPoint = 0m;
            decimal Full = 0m;
            decimal AE = 0m;
            decimal PG = 0m;
            decimal DJ = 0m;
            decimal LM = 0m;
            decimal PC1 = 0m;
            decimal PC2 = 0m;
            decimal PC3 = 0m;
            decimal PC4 = 0m;
            decimal Inc1 = 0m;
            decimal Inc2 = 0m;
            decimal Inc3 = 0m;
            decimal Inc4 = 0m;
            decimal Kom1 = 0m;
            decimal Kom2 = 0m;
            int Tahun = DateTime.Now.Year;
            int Bulan = DateTime.Now.Month;

            TotalPoint = Convert.ToDecimal(totalpoin.Text);
            Full = Convert.ToDecimal(full.Text);
            AE = Convert.ToDecimal(ae.Text);
            PG = Convert.ToDecimal(pg.Text);
            DJ = Convert.ToDecimal(dj.Text);
            LM = Convert.ToDecimal(lm.Text);
            PC1 = Convert.ToDecimal(pc1.Text);
            PC2 = Convert.ToDecimal(pc2.Text);
            PC3 = Convert.ToDecimal(pc3.Text);
            PC4 = Convert.ToDecimal(pc4.Text);
            Inc1 = Convert.ToDecimal(inc1.Text);
            Inc2 = Convert.ToDecimal(inc2.Text);
            Inc3 = Convert.ToDecimal(inc3.Text);
            Inc4 = Convert.ToDecimal(inc4.Text);
            Kom1 = Convert.ToDecimal(kom1.Text);
            Kom2 = Convert.ToDecimal(kom2.Text);
            Tahun = Convert.ToInt32(tahun.Text);

            if (pertahun.Checked == true)
            {
                for (int i = 1; i < 13; i++)
                {
                    Bulan = i;
                    var d = db.SalesFee.SingleOrDefault(p => p.Tahun == Tahun && p.Bulan == Bulan);
                    if (d == null)
                    {
                        SalesFee sf = new SalesFee();
                        sf.Tahun = Tahun;
                        sf.Bulan = Bulan;
                        sf.TotalPoint = TotalPoint;
                        sf.FullMoney = Full;
                        sf.AE = AE;
                        sf.PG = PG;
                        sf.DJ = DJ;
                        sf.LM = LM;
                        sf.PC1 = PC1;
                        sf.PC2 = PC2;
                        sf.PC3 = PC3;
                        sf.PC4 = PC4;
                        sf.Inc1 = Inc1;
                        sf.Inc2 = Inc2;
                        sf.Inc3 = Inc3;
                        sf.Inc4 = Inc4;
                        sf.Kom1 = Kom1;
                        sf.Kom2 = Kom2;
                        sf.OperatorNama = App.UserNama;
                        sf.OperatorTgl = DateTime.Now;
                        sf.ApprovalNama = App.UserNama;
                        sf.ApprovalTgl = DateTime.Now;
                        db.SalesFee.InsertOnSubmit(sf);
                        db.SubmitChanges();
                    }
                    else
                    {
                        d.TotalPoint = TotalPoint;
                        d.FullMoney = Full;
                        d.AE = AE;
                        d.PG = PG;
                        d.DJ = DJ;
                        d.LM = LM;
                        d.PC1 = PC1;
                        d.PC2 = PC2;
                        d.PC3 = PC3;
                        d.PC4 = PC4;
                        d.Inc1 = Inc1;
                        d.Inc2 = Inc2;
                        d.Inc3 = Inc3;
                        d.Inc4 = Inc4;
                        d.Kom1 = Kom1;
                        d.Kom2 = Kom2;
                        d.OperatorNama = App.UserNama;
                        d.OperatorTgl = DateTime.Now;
                        d.ApprovalNama = App.UserNama;
                        d.ApprovalTgl = DateTime.Now;
                        db.SubmitChanges();
                    }
                }
            }
            else
            {
                Bulan = Convert.ToInt32(bulan.Text);

                var d = db.SalesFee.SingleOrDefault(p => p.Tahun == Tahun && p.Bulan == Bulan);
                if (d == null)
                {
                    SalesFee sf = new SalesFee();
                    sf.Tahun = Tahun;
                    sf.Bulan = Bulan;
                    sf.TotalPoint = TotalPoint;
                    sf.FullMoney = Full;
                    sf.AE = AE;
                    sf.PG = PG;
                    sf.DJ = DJ;
                    sf.LM = LM;
                    sf.PC1 = PC1;
                    sf.PC2 = PC2;
                    sf.PC3 = PC3;
                    sf.PC4 = PC4;
                    sf.Inc1 = Inc1;
                    sf.Inc2 = Inc2;
                    sf.Inc3 = Inc3;
                    sf.Inc4 = Inc4;
                    sf.Kom1 = Kom1;
                    sf.Kom2 = Kom2;
                    sf.OperatorNama = App.UserNama;
                    sf.OperatorTgl = DateTime.Now;
                    sf.ApprovalNama = App.UserNama;
                    sf.ApprovalTgl = DateTime.Now;
                    db.SalesFee.InsertOnSubmit(sf);
                    db.SubmitChanges();
                }
                else
                {
                    d.TotalPoint = TotalPoint;
                    d.FullMoney = Full;
                    d.AE = AE;
                    d.PG = PG;
                    d.DJ = DJ;
                    d.LM = LM;
                    d.PC1 = PC1;
                    d.PC2 = PC2;
                    d.PC3 = PC3;
                    d.PC4 = PC4;
                    d.Inc1 = Inc1;
                    d.Inc2 = Inc2;
                    d.Inc3 = Inc3;
                    d.Inc4 = Inc4;
                    d.Kom1 = Kom1;
                    d.Kom2 = Kom2;
                    d.OperatorNama = App.UserNama;
                    d.OperatorTgl = DateTime.Now;
                    d.ApprovalNama = App.UserNama;
                    d.ApprovalTgl = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            Response.Redirect("SalesFeeList.aspx?d=1");
        }
        else
        {
            err_msg.Text = "<div class=\"text-danger\">Pacing harus berjumlah 100 %</div><br />";
        }
	}
}
