using System;
using System.Linq;

public partial class Page_Register_DJDO_DataSave : System.Web.UI.Page
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (cs != null)
	        cs.Dispose();
	    base.Dispose();
	}
    //protected int myID { get { return App.QueryInt(this); } }
    //protected int myCMD { get { return App.QueryInt(this); } }
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected int myCMD { get { return Convert.ToInt32(Request.QueryString["cmd"]); } }
    protected int myWarehouse { get { return Convert.ToInt32(Request.QueryString["warehouseid"]); } }

	//Exception
	protected void DataError() { Response.Redirect("DJDO.aspx"); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

        var d = cs.T_brj_receive.SingleOrDefault(p => p.NoDOBRJ == myNODO);
		if (d == null) DataError();
		else {
			saveLink(d);

            if (myCMD == 1 || myCMD == 2)
            {
                //Redirect
                Response.Redirect("DJDO.aspx?nodo=" + myNODO);
            }
            else
            {
                Response.Redirect("DJDO.aspx");
            }
		}
	}

	protected void saveLink(T_brj_receive r) {
        
        var d = cs.T_brj_receive.Single(p => p.NoDOBRJ == myNODO);

        if (d == null) DataError();
		else {

            if (d.StatusDO == 0)
            {
                if (myCMD == 1)
                {
                    d.StatusDO = myCMD;
                    d.Operator = App.UserNama;
                    d.OperatorTgl = System.DateTime.Now;


                    //Commit
                    cs.SubmitChanges();
                }
            }
            else if (d.StatusDO == 1)
            {
                if (myCMD == 2)
                {
                    // update status DO BRJ
                    d.StatusDO = myCMD;
					//d.StatusDO = 3;

                    // add warehouse location
                    var w = db.LocWarehouse.Single(p => p.ID == myWarehouse);

                    d.IDWarehouse = w.ID;
                    d.NamaWarehouse = w.Nama;

                    // add company brand
                    d.IDCompanyBrand = w.CompanyBrand.ID;
                    d.NamaCompanyBrand = w.CompanyBrand.Nama;

                    d.Operator = App.UserNama;
                    d.OperatorTgl = System.DateTime.Now;

                    // add list receiving by do
                    var s = cs.T_brj_receive.Where(p => p.NoDOBRJ == myNODO);
					
					// auto update string empty SPK
					var y = cs.T_brj.Where(p => p.NoDOBRJ == myNODO);
                    foreach (var item in y)
                    {
                        item.NoSPK = "";
                        cs.SubmitChanges();
                    }

                    var x = cs.T_brj.Where(p => p.NoDOBRJ == myNODO && p.NoSPK != null);

                    // update ID SPK
                    if (x.Count() > 0)
                    {
                        foreach (var data in x)
                        {
                            if(data.IDSPK == null && data.NoSPK != null)
                            {
                                string nospk = data.NoSPK;
                                try
                                {
                                    var rs = db.ProductOrder.SingleOrDefault(p => p.NoSPK == nospk);
                                    if (rs != null)
                                    {
                                        int idspk = rs.ID;

                                        var upd = cs.T_brj.Where(p => p.NoSPK == nospk && p.IDSPK == null);
                                        foreach (var data2 in upd)
                                        {
                                            data2.IDSPK = idspk;
                                            cs.SubmitChanges();
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    var upd = cs.T_brj.Where(p => p.NoSPK == nospk && p.IDSPK == null);
                                    foreach (var data2 in upd)
                                    {
                                        data2.NoSPK = null;
                                        cs.SubmitChanges();
                                    }
                                }
                            }

                        }
                    }
                    

                }
                else if (myCMD == 0)
                {
                    // update status DO BRJ
                    d.StatusDO = myCMD;

                    d.NamaCompanyBrand = null;
                    d.IDWarehouse = null;
                    d.NamaWarehouse = null;
                }

                //Commit
                cs.SubmitChanges();
            }
		}
	}
}